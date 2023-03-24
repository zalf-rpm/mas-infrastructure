package main

import (
	"errors"
	"flag"
	"fmt"
	"log"
	"math"

	"github.com/batchatco/go-native-netcdf/netcdf"
	"github.com/batchatco/go-native-netcdf/netcdf/api"
	"github.com/google/uuid"
	"github.com/zalf-rpm/mas-infrastructure/src/go/commonlib"
)

type gridService struct {
	commonGrid  *commonlib.Grid
	data        *api.Group
	startLatIdx int64
	startLonIdx int64
	stepLatSize float64
	stepLonSize float64
	timeValues  []float32
}

func newGridService(restorer *commonlib.Restorer) *gridService {

	fileLocation := flag.String("netcdf", "", "netcdf file to load")
	description := flag.String("description", "Groundwater service", "description of the netcdf file")
	name := flag.String("name", "groundwater", "name of the service")
	flag.Parse()

	if *fileLocation == "" {
		log.Fatal("no netcdf file specified")
	}

	newCommonGrid := commonlib.NewGrid(restorer, uuid.New().String(), *name, *description)
	gs := &gridService{
		commonGrid: newCommonGrid,
	}
	meta, err := loadNetCDF(*fileLocation)
	gs.data = meta.data
	gs.startLatIdx = meta.startLatIdx
	gs.startLonIdx = meta.startLonIdx
	gs.stepLatSize = meta.stepLatSize
	gs.stepLonSize = meta.stepLonSize
	gs.commonGrid.NumRows = uint64(meta.numRows)
	gs.commonGrid.NumCols = uint64(meta.numCols)
	gs.commonGrid.GridResolution = meta.gridResolution
	gs.commonGrid.GridUnit = meta.gridUnit
	gs.commonGrid.NoDataType = meta.noDataType
	gs.commonGrid.Bounds = meta.bounds
	gs.commonGrid.BoundsFromCellCenter = meta.boundsFromCellCenter
	gs.timeValues = meta.timeValues

	if err != nil {
		log.Fatal(err)
	}
	gs.setupCallbacks()
	return gs
}

type loadedNetCDFMeta struct {
	data                 *api.Group
	numRows              int64
	numCols              int64
	startLatIdx          int64
	startLonIdx          int64
	stepLatSize          float64
	stepLonSize          float64
	gridResolution       commonlib.Resolution
	gridUnit             string
	noDataType           int64
	bounds               commonlib.LatLonBoundaries
	boundsFromCellCenter commonlib.LatLonBoundaries
	timeValues           []float32
}

// load a netcdf file, print credentials
func loadNetCDF(inputFile string) (loadedNetCDFMeta, error) {

	meta := loadedNetCDFMeta{
		data:                 nil,
		numRows:              0,
		numCols:              0,
		startLatIdx:          0,
		startLonIdx:          0,
		stepLatSize:          0,
		stepLonSize:          0,
		gridResolution:       commonlib.Resolution{},
		gridUnit:             "",
		noDataType:           0,
		bounds:               commonlib.LatLonBoundaries{},
		boundsFromCellCenter: commonlib.LatLonBoundaries{},
		timeValues:           []float32{},
	}

	// Open the file
	nc, err := netcdf.Open(inputFile)
	if err != nil {
		return meta, err
	}
	meta.data = &nc

	// part 1: get a base overview of the file
	for _, key := range nc.Attributes().Keys() {
		if val, ok := nc.Attributes().Get(key); ok {
			fmt.Println(key, ":")
			fmt.Println(val)
		}
	}

	fmt.Println(nc.ListVariables())
	fmt.Println(nc.ListSubgroups())
	fmt.Println(nc.ListTypes())
	fmt.Println(nc.ListDimensions())
	for _, dim := range nc.ListDimensions() {
		val, has := nc.GetDimension(dim)
		if has {
			fmt.Println(dim, val)
		}
	}

	// print details of each variable in the file
	for _, attr := range nc.ListVariables() {
		// attribute name
		fmt.Println(attr)

		getVar, err := nc.GetVarGetter(attr)
		if err != nil {
			fmt.Println(err)
			continue
		}
		fmt.Println("Len:", getVar.Len())
		fmt.Println("Type:", getVar.Type())
		gotype := getVar.GoType()
		fmt.Println("GoType:", gotype)
		lenDim := len(getVar.Dimensions())
		fmt.Println("Dimensions:", getVar.Dimensions(), lenDim)
		fmt.Println("Attributes:")
		for _, key := range getVar.Attributes().Keys() {
			if val, ok := getVar.Attributes().Get(key); ok {
				fmt.Println("  ", key, ":", val)
			}
		}
	}

	// latitudes
	latVar, err := nc.GetVarGetter("lat")
	if err != nil {
		log.Fatal(err)
	}
	lenLat := latVar.Len()
	valsLat, err := latVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	valLat := valsLat.([]float32)
	// longitude
	lonVar, err := nc.GetVarGetter("lon")
	if err != nil {
		log.Fatal(err)
	}
	lenLon := lonVar.Len()
	valsLon, err := lonVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	valLon := valsLon.([]float32)

	meta.startLatIdx, meta.startLonIdx = findStartLatLon(valLat, valLon, lenLat, lenLon)
	meta.stepLatSize = math.Abs(float64(valLat[0] - valLat[1]))
	meta.stepLonSize = math.Abs(float64(valLon[0] - valLon[1]))

	if meta.stepLatSize-meta.stepLonSize > 0.0001 {
		log.Fatal("lat and lon steps are not equal")
	}

	meta.gridResolution = commonlib.Resolution{
		Value: meta.stepLonSize,
	}
	meta.gridUnit = "degrees"
	meta.bounds = commonlib.LatLonBoundaries{
		TopLeft: commonlib.LatLon{
			Lat: float64(valLat[lenLat-1]),
			Lon: float64(valLon[0]),
		},
		TopRight: commonlib.LatLon{
			Lat: float64(valLat[lenLat-1]),
			Lon: float64(valLon[lenLon-1]),
		},
		BottomLeft: commonlib.LatLon{
			Lat: float64(valLat[0]),
			Lon: float64(valLon[0]),
		},
		BottomRight: commonlib.LatLon{
			Lat: float64(valLat[0]),
			Lon: float64(valLon[lenLon-1]),
		},
	}
	meta.boundsFromCellCenter = commonlib.LatLonBoundaries{
		TopLeft: commonlib.LatLon{
			Lat: float64(valLat[lenLat-1]) - meta.stepLatSize/2,
			Lon: float64(valLon[0]) + meta.stepLonSize/2,
		},
		TopRight: commonlib.LatLon{
			Lat: float64(valLat[lenLat-1]) - meta.stepLatSize/2,
			Lon: float64(valLon[lenLon-1]) - meta.stepLonSize/2,
		},
		BottomLeft: commonlib.LatLon{
			Lat: float64(valLat[0]) + meta.stepLatSize/2,
			Lon: float64(valLon[0]) + meta.stepLonSize/2,
		},
		BottomRight: commonlib.LatLon{
			Lat: float64(valLat[0]) + meta.stepLatSize/2,
			Lon: float64(valLon[lenLon-1]) - meta.stepLonSize/2,
		},
	}

	meta.numRows = lenLat
	meta.numCols = lenLon
	meta.noDataType = int64(2)

	// time
	timeVar, err := (nc).GetVarGetter("time")
	if err != nil {
		log.Fatal(err)

	}
	valsTime, err := timeVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	var timeValues []float32
	switch timeVar.GoType() {
	case "float32":
		timeValues = valsTime.([]float32)
	case "int8":
		timeValuesInt := valsTime.([]int8)
		timeValues = make([]float32, 0, len(timeValuesInt))
		for _, val := range timeValuesInt {
			timeValues = append(timeValues, float32(val))
		}
	}
	meta.timeValues = timeValues
	return meta, nil
}

func (gs *gridService) setupCallbacks() {

	gs.commonGrid.GetValueLatLon = func(lat, lon float64) (interface{}, commonlib.RowCol, commonlib.RowCol, error) {
		// check if lat and lon are in bounds
		if lat >= gs.commonGrid.Bounds.TopLeft.Lat || lat <= gs.commonGrid.Bounds.BottomRight.Lat {
			return nil, commonlib.RowCol{}, commonlib.RowCol{}, fmt.Errorf("lat %f is out of bounds", lat)
		}
		if lon <= gs.commonGrid.Bounds.TopLeft.Lon || lon >= gs.commonGrid.Bounds.BottomRight.Lon {
			return nil, commonlib.RowCol{}, commonlib.RowCol{}, fmt.Errorf("lon %f is out of bounds", lon)
		}

		gw, ilat, ilon := gs.GetValueLatLon(gs.data, lat, lon)

		return gw, commonlib.RowCol{
				Row: uint64(ilat),
				Col: uint64(ilon),
			}, commonlib.RowCol{
				Row: uint64(ilat),
				Col: uint64(ilon),
			}, nil
	}
	gs.commonGrid.GetValueRowCol = func(row, col uint64) (interface{}, error) {
		val, err := gs.GetValueRowCol(gs.data, row, col)
		return val, err
	}

}
func findStartLatLon(valLat, valLon []float32, lenLat, lenLon int64) (int64, int64) {

	nearestToMiddle := func(val []float32, len int64, grads float64) int64 {
		min := math.Abs(float64(val[0]))
		currentIdx := int64(0)
		for i := int64(1); i < len; i++ {
			currVal := math.Abs(float64(val[i]))
			if currVal < min {
				min = currVal
				currentIdx = i
			}
		}
		// calculate step size

		if math.Abs(float64(valLat[currentIdx])) > 0.0001 {
			otherIdx := currentIdx + 1
			if otherIdx >= len {
				otherIdx = currentIdx - 1
			}
			stepSize := math.Abs(float64(valLat[currentIdx] - valLat[otherIdx]))
			numSteps := grads / stepSize

			currentIdx = currentIdx - int64(math.Round(float64(valLat[currentIdx])/numSteps))
		}

		return currentIdx
	}
	idxLat := nearestToMiddle(valLat, lenLat, 180.0)
	idxLon := nearestToMiddle(valLon, lenLon, 365.0)

	return idxLat, idxLon
}

func isNeiborCloser(arr []float32, idx int64, val float64) int64 {

	if idx+1 < int64(len(arr)) {
		next := math.Abs(float64(arr[idx])-val) > math.Abs(float64(arr[idx+1])-val)
		if next {
			return idx + 1
		}
	}
	if idx > 0 {
		prev := math.Abs(float64(arr[idx])-val) > math.Abs(float64(arr[idx-1])-val)
		if prev {
			return idx - 1
		}
	}
	return idx
}
func min(a, b int64) int64 {
	if a < b {
		return a
	}
	return b
}
func max(a, b int64) int64 {
	if a > b {
		return a
	}
	return b
}

func (gs *gridService) GetValueLatLon(nc *api.Group, inLat, inLon float64) (float64, int64, int64) {

	// calculate index of lat and lon in gwValues
	iLat := ((gs.startLatIdx) + int64(inLat/gs.stepLatSize))
	iLon := ((gs.startLonIdx) + int64(inLon/gs.stepLonSize))

	iLatMin := min(iLat-10, 0)
	iLatMax := max(iLat+10, int64(gs.commonGrid.NumRows-1))
	iLonMin := min(iLon-10, 0)
	iLonMax := max(iLon+10, int64(gs.commonGrid.NumCols-1))

	// latitudes
	latVar, err := (*nc).GetVarGetter("lat")
	if err != nil {
		log.Fatal(err)
	}
	//lenLat := latVar.Len()

	valsLat, err := latVar.GetSlice(iLatMin, iLatMax) // latVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	valLat := valsLat.([]float32)
	// longitude
	lonVar, err := (*nc).GetVarGetter("lon")
	if err != nil {
		log.Fatal(err)
	}
	//lenLon := lonVar.Len()
	valsLon, err := lonVar.GetSlice(iLonMin, iLonMax) // lonVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	valLon := valsLon.([]float32)
	// ground water
	WTDVar, err := (*nc).GetVarGetter("WTD")
	if err != nil {
		log.Fatal(err)
	}
	valsWTD, err := WTDVar.GetSlice(0, 0) //WTDVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	valWTD := valsWTD.([][][]int16)

	// check if lat and lon of a neibor is closer, to correct rounding errors
	iLat = isNeiborCloser(valLat, iLat, inLat)
	iLon = isNeiborCloser(valLon, iLon, inLon)

	scaleFactor := 1.0
	if val, ok := WTDVar.Attributes().Get("scale_factor"); ok {
		scaleFactor = val.(float64)
	}
	var add_offset float64 = 0.0
	if val, ok := WTDVar.Attributes().Get("add_offset"); ok {
		add_offset = val.(float64)
	}

	// mask for valid data
	maskVar, err := (*nc).GetVarGetter("mask")
	if err != nil {
		log.Fatal(err)
	}
	valsMask, err := maskVar.GetSlice(iLat, iLat) //maskVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	var gw float64
	if valsMask.([][]int8)[iLat][iLon] == 1 {
		value := valWTD[0][iLat][iLon]
		gw = math.Ceil((float64(value)*scaleFactor + add_offset))
	} else {
		gw = 2
	}
	return gw, iLat, iLon
}

func (gs *gridService) GetValueRowCol(nc *api.Group, row, col uint64) (interface{}, error) {
	// check if row and col are in range
	if row >= gs.commonGrid.NumRows || col >= gs.commonGrid.NumCols {
		return nil, errors.New("row or col out of range")
	}

	// ground water
	WTDVar, err := (*nc).GetVarGetter("WTD")
	if err != nil {
		log.Fatal(err)
	}
	valsWTD, err := WTDVar.GetSlice(0, 0) //WTDVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	valWTD := valsWTD.([][][]int16)

	scaleFactor := 1.0
	if val, ok := WTDVar.Attributes().Get("scale_factor"); ok {
		scaleFactor = val.(float64)
	}
	var add_offset float64 = 0.0
	if val, ok := WTDVar.Attributes().Get("add_offset"); ok {
		add_offset = val.(float64)
	}

	// mask for valid data
	maskVar, err := (*nc).GetVarGetter("mask")
	if err != nil {
		log.Fatal(err)
	}
	valsMask, err := maskVar.GetSlice(int64(row), int64(row)) //maskVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	var gw float64
	if valsMask.([][]int8)[row][col] == 1 {
		value := valWTD[0][row][col]
		gw = math.Ceil((float64(value)*scaleFactor + add_offset))
	} else {
		gw = 2
	}
	return gw, nil
}

func (gs *gridService) GetValueLatLonAggregated(inLat, inLon float64, resolution commonlib.Resolution, agg string, includeAggParts bool) (interface{}, []commonlib.AggregationPart, error) {
	nc := gs.data
	val, ok := resolution.Value.(float64)
	if !ok {
		return nil, nil, errors.New("resolution is not a float64")
	}
	// check if lat and lon are in range
	if inLat >= gs.commonGrid.Bounds.TopLeft.Lat || inLat <= gs.commonGrid.Bounds.BottomRight.Lat {
		return nil, nil, fmt.Errorf("lat %f is out of bounds", inLat)
	}
	if inLon <= gs.commonGrid.Bounds.TopLeft.Lon || inLon >= gs.commonGrid.Bounds.BottomRight.Lon {
		return nil, nil, fmt.Errorf("lon %f is out of bounds", inLon)
	}
	latTop := inLat
	latBottom := inLat
	lonLeft := inLon
	lonRight := inLon

	if val > gs.commonGrid.GridResolution.Value.(float64) {
		// calculate extents of the grid cell
		latTop = math.Min(inLat+val/2, gs.commonGrid.Bounds.TopLeft.Lat)
		latBottom = math.Max(inLat-val/2, gs.commonGrid.Bounds.BottomRight.Lat)
		lonLeft = math.Max(inLon-val/2, gs.commonGrid.Bounds.TopLeft.Lon)
		lonRight = math.Min(inLon+val/2, gs.commonGrid.Bounds.BottomRight.Lon)
	}

	// calculate index of lat and lon in gwValues
	iLatTop := ((gs.startLatIdx) + int64(latTop/gs.stepLatSize))
	iLatBottom := ((gs.startLatIdx) + int64(latBottom/gs.stepLatSize))
	iLonLeft := ((gs.startLonIdx) + int64(lonLeft/gs.stepLonSize))
	iLonRight := ((gs.startLonIdx) + int64(lonRight/gs.stepLonSize))

	iLatMin := min(iLatBottom-10, 0)
	iLatMax := max(iLatTop+10, int64(gs.commonGrid.NumRows-1))

	iLonMin := min(iLonLeft-10, 0)
	iLonMax := max(iLonRight+10, int64(gs.commonGrid.NumCols-1))

	// latitudes
	latVar, err := (*nc).GetVarGetter("lat")
	if err != nil {
		log.Fatal(err)
	}
	//lenLat := latVar.Len()

	valsLat, err := latVar.GetSlice(iLatMin, iLatMax) // latVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	valLat := valsLat.([]float32)
	// longitude
	lonVar, err := (*nc).GetVarGetter("lon")
	if err != nil {
		log.Fatal(err)
	}
	//lenLon := lonVar.Len()
	valsLon, err := lonVar.GetSlice(iLonMin, iLonMax) // lonVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	valLon := valsLon.([]float32)

	// check if lat and lon of a neibor is closer, to correct rounding errors
	iLatTop = isNeiborCloser(valLat, iLatTop, inLon)
	iLatBottom = isNeiborCloser(valLat, iLatBottom, inLon)
	iLonLeft = isNeiborCloser(valLon, iLonLeft, inLon)
	iLonRight = isNeiborCloser(valLon, iLonRight, inLon)

	// ground water
	WTDVar, err := (*nc).GetVarGetter("WTD")
	if err != nil {
		log.Fatal(err)
	}
	valsWTD, err := WTDVar.GetSlice(0, 0) //WTDVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	valWTD := valsWTD.([][][]int16)

	scaleFactor := 1.0
	if val, ok := WTDVar.Attributes().Get("scale_factor"); ok {
		scaleFactor = val.(float64)
	}
	var add_offset float64 = 0.0
	if val, ok := WTDVar.Attributes().Get("add_offset"); ok {
		add_offset = val.(float64)
	}

	// mask for valid data
	maskVar, err := (*nc).GetVarGetter("mask")
	if err != nil {
		log.Fatal(err)
	}
	valsMask, err := maskVar.GetSlice(iLatTop, iLatBottom) //maskVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	gwList := make([]commonlib.AggregationPart, 0, 1)
	for iLat := iLatTop; iLat <= iLatBottom; iLat++ {
		for iLon := iLonLeft; iLon <= iLonRight; iLon++ {
			if valsMask.([][]int8)[iLat][iLon] == 1 {
				value := valWTD[0][iLat][iLon]
				gw := math.Ceil((float64(value)*scaleFactor + add_offset))
				gwList = append(gwList, commonlib.AggregationPart{
					OriginalValue: gw,
					RowColTuple: commonlib.RowCol{
						Row: uint64(iLat),
						Col: uint64(iLon),
					},
					AreaWeight: 1,
				})
			}
		}
	}
	if len(gwList) > 0 {
		aggPart := make([]commonlib.AggregationPart, 0, len(gwList))
		for _, gw := range gwList {
			aggPart = append(aggPart, commonlib.AggregationPart{
				OriginalValue: gw,
				RowColTuple:   commonlib.RowCol{},
				AreaWeight:    0,
			})
		}
		return 1, []commonlib.AggregationPart{}, nil

	} else {
		return 2, nil, nil
	}

}

func (gs *gridService) createGWTimeSeries(nc *api.Group, inLat, inLon float64) float64 {

	// time
	timeVar, err := (*nc).GetVarGetter("time")
	if err != nil {
		log.Fatal(err)

	}
	valsTime, err := timeVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	var timeValues []float32
	switch timeVar.GoType() {
	case "float32":
		timeValues = valsTime.([]float32)
	case "int8":
		timeValuesInt := valsTime.([]int8)
		timeValues = make([]float32, 0, len(timeValuesInt))
		for _, val := range timeValuesInt {
			timeValues = append(timeValues, float32(val))
		}
	}

	// latitudes
	latVar, err := (*nc).GetVarGetter("lat")
	if err != nil {
		log.Fatal(err)
	}
	lenLat := latVar.Len()
	valsLat, err := latVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	valLat := valsLat.([]float32)
	// longitude
	lonVar, err := (*nc).GetVarGetter("lon")
	if err != nil {
		log.Fatal(err)
	}
	lenLon := lonVar.Len()
	valsLon, err := lonVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	valLon := valsLon.([]float32)
	// ground water
	WTDVar, err := (*nc).GetVarGetter("WTD")
	if err != nil {
		log.Fatal(err)
	}
	valsWTD, err := WTDVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	valWTD := valsWTD.([][][]int16)
	scaleFactor := 1.0
	if val, ok := WTDVar.Attributes().Get("scale_factor"); ok {
		scaleFactor = val.(float64)
	}
	var add_offset float64 = 0.0
	if val, ok := WTDVar.Attributes().Get("add_offset"); ok {
		add_offset = val.(float64)
	}

	// mask for valid data
	maskVar, err := (*nc).GetVarGetter("mask")
	if err != nil {
		log.Fatal(err)
	}
	valsMask, err := maskVar.Values()
	if err != nil {
		log.Fatal(err)
	}

	gwValues := make([][]float64, lenLat)
	min, max := 0.0, 0.0
	init := false

	for iLat := int64(0); iLat < lenLat; iLat++ {
		gwValues[iLat] = make([]float64, lenLon)
		for iLon := int64(0); iLon < lenLon; iLon++ {
			// check against mask (1 = valid, 0 = invalid)
			if valsMask.([][]int8)[iLat][iLon] == 1 {
				timeSlice := make([]float64, len(timeValues))
				for iTime := 0; iTime < len(timeValues); iTime++ {
					value := valWTD[iTime][iLat][iLon]
					timeSlice[iTime] = math.Ceil((float64(value)*scaleFactor + add_offset))

					if !init {
						min = timeSlice[iTime]
						max = timeSlice[iTime]
						init = true
					}
					if timeSlice[iTime] < min {
						min = timeSlice[iTime]
					}
					if timeSlice[iTime] > max {
						max = timeSlice[iTime]
					}
					gwValues[iLat][iLon] = timeSlice[iTime]
				}
			} else {
				gwValues[iLat][iLon] = 2 // invalid

			}
		}
	}

	// calculate index of lat and lon in gwValues
	iLat := ((gs.startLatIdx) + int64(inLat/gs.stepLatSize))
	iLon := ((gs.startLonIdx) + int64(inLon/gs.stepLonSize))

	// check if lat and lon of a neibor is closer, to correct rounding errors
	iLat = isNeiborCloser(valLat, iLat, inLat)
	iLon = isNeiborCloser(valLon, iLon, inLon)

	gw := gwValues[iLat][iLon] * -1
	if gw < 0 {
		gw = 0
	}
	return gw
}
