package main

import (
	"errors"
	"flag"
	"fmt"
	"log"
	"math"
	"sort"
	"strings"

	"github.com/batchatco/go-native-netcdf/netcdf"
	"github.com/batchatco/go-native-netcdf/netcdf/api"
	"github.com/google/uuid"
	"github.com/zalf-rpm/mas-infrastructure/src/go/commonlib"
	"gonum.org/v1/gonum/stat"
)

const testNC_EURASIA = "EURASIA_WTD_annualmean.nc"

type gridService struct {
	commonGrid  *commonlib.Grid
	startLatIdx int64
	startLonIdx int64
	stepLatSize float64
	stepLonSize float64
	timeValues  []float32
	wdt         [][][]int16
	scaleFactor float64
	add_offset  float64
	mask        [][]int8
	latitudes   []float32
	longitudes  []float32
}

func newGridService(restorer *commonlib.Restorer) *gridService {

	fileLocation := flag.String("netcdf", testNC_EURASIA, "netcdf file to load")
	description := flag.String("description", "Groundwater service", "description of the netcdf file")
	name := flag.String("name", "groundwater", "name of the service")
	flag.Parse()

	if *fileLocation == "" {
		log.Fatal("no netcdf file specified")
	}

	newCommonGrid := commonlib.NewGrid(restorer, uuid.New().String(), *name, *description)
	gs := &gridService{
		commonGrid: newCommonGrid,
		wdt:        nil,
	}
	meta, err := loadNetCDF(*fileLocation)
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
	gs.wdt = meta.wdt
	gs.scaleFactor = meta.scaleFactor
	gs.add_offset = meta.add_offset
	gs.mask = meta.mask
	gs.latitudes = meta.latitudes
	gs.longitudes = meta.longitudes
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
	wdt                  [][][]int16
	scaleFactor          float64
	add_offset           float64
	mask                 [][]int8
	latitudes            []float32
	longitudes           []float32
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
		wdt:                  [][][]int16{},
		scaleFactor:          1.0,
		add_offset:           0.0,
		mask:                 [][]int8{},
		latitudes:            []float32{},
		longitudes:           []float32{},
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
	meta.latitudes = valLat
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
	meta.longitudes = valLon

	meta.startLatIdx, meta.startLonIdx = findStartLatLon(valLat, valLon, lenLat, lenLon)
	meta.stepLatSize = math.Abs(float64(valLat[0] - valLat[1]))
	meta.stepLonSize = math.Abs(float64(valLon[0] - valLon[1]))

	if meta.stepLatSize-meta.stepLonSize > 0.0001 {
		log.Fatal("lat and lon steps are not equal")
	}

	meta.gridResolution = commonlib.Resolution{
		Value: meta.stepLonSize,
	}
	meta.gridUnit = "degree"
	meta.boundsFromCellCenter = commonlib.LatLonBoundaries{
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
	meta.bounds = commonlib.LatLonBoundaries{
		TopLeft: commonlib.LatLon{
			Lat: float64(valLat[lenLat-1]) + meta.stepLatSize/2,
			Lon: float64(valLon[0]) - meta.stepLonSize/2,
		},
		TopRight: commonlib.LatLon{
			Lat: float64(valLat[lenLat-1]) + meta.stepLatSize/2,
			Lon: float64(valLon[lenLon-1]) + meta.stepLonSize/2,
		},
		BottomLeft: commonlib.LatLon{
			Lat: float64(valLat[0]) - meta.stepLatSize/2,
			Lon: float64(valLon[0]) - meta.stepLonSize/2,
		},
		BottomRight: commonlib.LatLon{
			Lat: float64(valLat[0]) - meta.stepLatSize/2,
			Lon: float64(valLon[lenLon-1]) + meta.stepLonSize/2,
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

	WTDVar, err := (nc).GetVarGetter("WTD")
	if err != nil {
		log.Fatal(err)
	}
	valsWTD, err := WTDVar.GetSlice(0, 1) // get first time step only
	if err != nil {
		log.Fatal(err)
	}
	valWTD := valsWTD.([][][]int16)
	meta.wdt = valWTD

	if val, ok := WTDVar.Attributes().Get("scale_factor"); ok {
		meta.scaleFactor = val.(float64)
	}

	if val, ok := WTDVar.Attributes().Get("add_offset"); ok {
		meta.add_offset = val.(float64)
	}
	// mask for valid data
	maskVar, err := (nc).GetVarGetter("mask")
	if err != nil {
		log.Fatal(err)
	}
	valsMask, err := maskVar.Values()
	if err != nil {
		log.Fatal(err)
	}
	meta.mask = valsMask.([][]int8)
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

		gw, ilat, ilon := gs.GetValueLatLon(lat, lon)

		return gw, commonlib.RowCol{
				Row: uint64(ilat),
				Col: uint64(ilon),
			}, commonlib.RowCol{
				Row: uint64(ilat),
				Col: uint64(ilon),
			}, nil
	}
	gs.commonGrid.GetValueRowCol = func(row, col uint64) (interface{}, error) {
		val, err := gs.GetValueRowCol(row, col)
		return val, err
	}
	gs.commonGrid.GetValueLatLonAggregated = gs.GetValueLatLonAggregated
	gs.commonGrid.GetValueRowColAggregated = gs.GetValueRowColAggregated
	gs.commonGrid.RowColToLatLon = gs.rowColToLatLon
}

func (gs *gridService) rowColToLatLon(row, col uint64) (commonlib.LatLon, error) {

	if row >= gs.commonGrid.NumRows || col >= gs.commonGrid.NumCols {
		return commonlib.LatLon{}, errors.New("row or col out of range")
	}

	return commonlib.LatLon{
		Lat: float64(gs.latitudes[row]),
		Lon: float64(gs.longitudes[col]),
	}, nil
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

	currentDistance := math.Abs(float64(arr[idx]) - val)
	if idx+1 < int64(len(arr)) {
		nextDistance := math.Abs(float64(arr[idx+1]) - val)
		if nextDistance < currentDistance {
			return idx + 1
		}
	}
	if idx > 0 {
		prevDistance := math.Abs(float64(arr[idx-1]) - val)
		if prevDistance < currentDistance {
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

func (gs *gridService) GetValueLatLon(inLat, inLon float64) (float64, int64, int64) {

	// calculate index of lat and lon in gwValues
	iLat := ((gs.startLatIdx) + int64(inLat/gs.stepLatSize))
	iLon := ((gs.startLonIdx) + int64(inLon/gs.stepLonSize))

	// check if lat and lon of a neibor is closer, to correct rounding errors
	iLat = isNeiborCloser(gs.latitudes, iLat, inLat)
	iLon = isNeiborCloser(gs.longitudes, iLon, inLon)

	var gw float64
	if gs.mask[iLat][iLon] == 1 {
		value := gs.wdt[0][iLat][iLon]
		gw = math.Ceil((float64(value)*gs.scaleFactor + gs.add_offset))
	} else {
		gw = 2
	}
	return gw, iLat, iLon
}

func (gs *gridService) GetValueRowCol(row, col uint64) (interface{}, error) {
	// check if row and col are in range
	if row >= gs.commonGrid.NumRows || col >= gs.commonGrid.NumCols {
		return nil, errors.New("row or col out of range")
	}

	var gw float64
	if gs.mask[row][col] == 1 {
		value := gs.wdt[0][row][col]
		gw = math.Ceil((float64(value)*gs.scaleFactor + gs.add_offset))
	} else {
		gw = 2
	}
	return gw, nil
}

func (gs *gridService) GetValueLatLonAggregated(inLat, inLon float64, resolution commonlib.Resolution, agg string, includeAggParts bool) (interface{}, []*commonlib.AggregationPart, error) {
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

	aggVal := 0.0
	if agg == "none" {
		gw, _, _ := gs.GetValueLatLon(inLat, inLon)
		aggVal = gw
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

	// check if lat and lon of a neibor is closer, to correct rounding errors

	iLatTop = isNeiborCloser(gs.latitudes, iLatTop, latTop)
	iLatBottom = isNeiborCloser(gs.latitudes, iLatBottom, latBottom)
	iLonLeft = isNeiborCloser(gs.longitudes, iLonLeft, lonLeft)
	iLonRight = isNeiborCloser(gs.longitudes, iLonRight, lonRight)
	latBottomC := gs.latitudes[iLatBottom]
	latTopC := gs.latitudes[iLatTop]
	lonLeftC := gs.longitudes[iLonLeft]
	lonRightC := gs.longitudes[iLonRight]

	cellSize := gs.stepLonSize * gs.stepLatSize
	// calculate area weight
	calcAreaWeight := func(ilat, ilon int64) float64 {

		// check if lat and lon are on the edge of the grid
		if (iLatBottom != iLatTop && (ilat == iLatBottom || ilat == iLatTop)) ||
			(iLonLeft != iLonRight && (ilon == iLonLeft || ilon == iLonRight)) {

			lenLat := gs.stepLatSize
			lenLon := gs.stepLonSize
			if ilat == iLatBottom {
				latBottomEdge := latBottomC + float32(gs.stepLatSize)/2
				lenLat = math.Abs(float64(latBottomEdge) - latBottom)
			}
			if ilat == iLatTop {
				latTopEdge := latTopC - float32(gs.stepLatSize)/2
				lenLat = math.Abs(float64(latTopEdge) - latTop)
			}
			if ilon == iLonLeft {
				lonLeftEdge := lonLeftC + float32(gs.stepLonSize)/2
				lenLon = math.Abs(float64(lonLeftEdge) - lonLeft)
			}
			if ilon == iLonRight {
				lonRightEdge := lonRightC - float32(gs.stepLonSize)/2
				lenLon = math.Abs(float64(lonRightEdge) - lonRight)
			}
			// impressions
			if lenLat > gs.stepLatSize {
				lenLat = gs.stepLatSize
			}
			if lenLon > gs.stepLonSize {
				lenLon = gs.stepLonSize
			}
			if lenLat*lenLon > cellSize {
				return 1.0 // this should not happen
			}

			return (lenLat * lenLon) / cellSize
		}
		return 1.0
	}
	closestFullCell := func(ilat, ilon int64) (int64, int64, bool) {
		if iLatBottom == iLatTop || iLonLeft == iLonRight {
			return -1, -1, false
		}
		// check if lat and lon are on the edge of the grid
		if ilat == iLatBottom || ilat == iLatTop || ilon == iLonLeft || ilon == iLonRight {
			var celliLat, celliLon int64 = ilat, ilon
			if ilat == iLatBottom {
				celliLat = ilat + 1
			}
			if ilat == iLatTop {
				celliLat = ilat - 1
			}

			if ilon == iLonLeft {
				celliLon = ilon + 1
			}
			if ilon == iLonRight {
				celliLon = ilon - 1
			}
			return celliLat, celliLon, true
		}

		return -1, -1, false
	}

	// calculate area weight for each grid cell
	gwList := make([]*commonlib.AggregationPart, 0, 1)
	for iLat := iLatBottom; iLat <= iLatTop; iLat++ {
		for iLon := iLonLeft; iLon <= iLonRight; iLon++ {

			if gs.mask[iLat][iLon] == 1 {
				areaWeight := calcAreaWeight(iLat, iLon)

				value := gs.wdt[0][iLat][iLon]
				gw := math.Ceil((float64(value)*gs.scaleFactor + gs.add_offset))
				gwList = append(gwList, &commonlib.AggregationPart{
					OriginalValue: gw,
					RowColTuple: commonlib.RowCol{
						Row: uint64(iLat),
						Col: uint64(iLon),
					},
					AreaWeight: areaWeight,
				})
			} else {
				gwList = append(gwList, &commonlib.AggregationPart{
					OriginalValue: gs.commonGrid.NoDataType,
					RowColTuple: commonlib.RowCol{
						Row: uint64(iLat),
						Col: uint64(iLon),
					},
					AreaWeight: 0.0,
				})
			}
		}
	}
	if len(gwList) == 0 {
		// no data found
		return gs.commonGrid.NoDataType, nil, nil
	}
	if agg != "none" {
		if strings.HasPrefix(agg, "i") {
			// interpolate
			for _, gwEntry := range gwList {
				if gwEntry.AreaWeight > 0.0 && gwEntry.AreaWeight < 1.0 {
					if filat, filon, found := closestFullCell(int64(gwEntry.RowColTuple.Row), int64(gwEntry.RowColTuple.Col)); found {
						dr := math.Abs(float64(int64(gwEntry.RowColTuple.Row) - filat))
						dc := math.Abs(float64(int64(gwEntry.RowColTuple.Col) - filon))
						full_dist := math.Sqrt(dr*cellSize*cellSize + dc*cellSize*cellSize)
						half_full_dist := math.Sqrt(dr*(cellSize/2)*(cellSize/2) + dc*(cellSize/2)*(cellSize/2))
						half_short_dist := math.Sqrt(dr*gwEntry.AreaWeight*(cellSize/2)*(cellSize/2) + dc*gwEntry.AreaWeight*(cellSize/2)*(cellSize/2))

						interpol_value := gwEntry.OriginalValue.(float64) * (half_full_dist + half_short_dist) / full_dist
						gwEntry.InterpolatedValue = interpol_value
						continue
					}
				}
				gwEntry.InterpolatedValue = gwEntry.OriginalValue.(float64)
			}
		}
		// aggregate
		aggVal, err := aggregate(agg, gwList, gs.commonGrid.NoDataType)
		if err != nil {
			return nil, nil, err
		}
		return aggVal, gwList, nil

	}
	return aggVal, gwList, nil
}

func (gs *gridService) GetValueRowColAggregated(row uint64, col uint64, resolution commonlib.Resolution, agg string, includeAggParts bool) (interface{}, []*commonlib.AggregationPart, error) {

	// check if row and col are in range
	if row >= gs.commonGrid.NumRows || col >= gs.commonGrid.NumCols {
		return nil, nil, errors.New("row or col out of range")
	}

	iLat, iLon := float64(gs.latitudes[row]), float64(gs.longitudes[col])
	val, list, err := gs.GetValueLatLonAggregated(iLat, iLon, resolution, agg, includeAggParts)
	return val, list, err
}

func aggregate(aggType string, unfilteredValues []*commonlib.AggregationPart, noVal interface{}) (float64, error) {

	// avg       @8;   # average of cell values
	// wAvg      @1;   # area weighted average
	// iAvg      @6;   # interpolated average
	// median    @9;   # median of cell values
	// wMedian   @2;   # area weighted median
	// iMedian   @7;   # interpolated median
	// min       @3;   # minimum
	// wMin      @12;  # area weighted minimum
	// iMin      @13;  # interpolated minimum
	// max       @4;   # maximum
	// wMax      @14;  # area weighted maximum
	// iMax      @15;  # interpolated maximum
	// sum       @5;   # sum of all cells values
	// wSum      @10;  # area weighted sum
	// iSum      @11;  # interpolated sum

	// filter values
	// remove invalid values
	values := make([]float64, 0, len(unfilteredValues))
	weightsValues := make([]float64, 0, len(unfilteredValues))
	interpolValues := make([]float64, 0, len(unfilteredValues))
	for _, val := range unfilteredValues {
		gw := val.OriginalValue.(float64)
		if gw != noVal &&
			!math.IsInf(gw, 0) && !math.IsNaN(gw) {
			values = append(values, gw)
			interpolValues = append(interpolValues, val.InterpolatedValue)
			weightsValues = append(weightsValues, val.AreaWeight)
		}
	}
	if len(values) == 0 {
		return 0.0, errors.New("no valid values")
	}
	result := 0.0
	switch aggType {
	case "avg":
		result = avg(values)
	case "wAvg":
		result = wAvg(values, weightsValues)
	case "iAvg":
		result = iAvg(interpolValues)
	case "median":
		result = median(values)
	case "wMedian":
		result = wMedian(values, weightsValues)
	case "iMedian":
		result = iMedian(interpolValues)
	case "min":
		result = minVal(values)
	case "wMin":
		result = wMin(values, weightsValues)
	case "iMin":
		result = iMin(interpolValues)
	case "max":
		result = maxVal(values)
	case "wMax":
		result = wMax(values, weightsValues)
	case "iMax":
		result = iMax(interpolValues)
	case "sum":
		result = sum(values)
	case "wSum":
		result = wSum(values, weightsValues)
	case "iSum":
		result = iSum(interpolValues)
	default:
		return 0.0, fmt.Errorf("unknown aggregation type: %v", aggType)
	}
	if math.IsInf(result, 0) || math.IsNaN(result) {
		return result, fmt.Errorf("invalid result: %v", result)
	}

	return result, nil
}

func iSum(values []float64) float64 {
	return sum(values)
}

func wSum(values, weightsValues []float64) float64 {
	wValues := make([]float64, len(values))
	for i, v := range values {
		wValues[i] = v * weightsValues[i]
	}
	return sum(wValues)
}

func sum(values []float64) float64 {
	var sum float64
	for _, v := range values {
		sum += v
	}
	return sum
}

func iMax(values []float64) float64 {
	return maxVal(values)
}

func wMax(values, weightsValues []float64) float64 {
	wValues := make([]float64, len(values))
	for i, v := range values {
		wValues[i] = v * weightsValues[i]
	}
	return maxVal(wValues)
}

func maxVal(values []float64) float64 {
	var max float64
	for i, v := range values {
		if i == 0 || v > max {
			max = v
		}
	}
	return max
}

func minVal(values []float64) float64 {
	var min float64
	for i, v := range values {
		if i == 0 || v < min {
			min = v
		}
	}
	return min
}

func iMin(values []float64) float64 {
	return minVal(values)
}

func wMin(values, weightsValues []float64) float64 {
	wValues := make([]float64, len(values))
	for i, v := range values {
		wValues[i] = v * weightsValues[i]
	}
	return minVal(wValues)
}

func iMedian(values []float64) float64 {
	return median(values)
}

func wMedian(values, weightsValues []float64) float64 {
	// weighted median
	// https://en.wikipedia.org/wiki/Weighted_median
	//https://www.gonum.org/post/intro_to_stats_with_gonum/

	sortedValues, sortedWeights := sortWeightsAndValues(values, weightsValues)
	median := stat.Quantile(0.5, stat.Empirical, sortedValues, sortedWeights)
	return median
}

func sortWeightsAndValues(values, weights []float64) ([]float64, []float64) {
	type pair struct {
		value  float64
		weight float64
	}
	pairs := make([]pair, len(values))
	for i, v := range values {
		pairs[i] = pair{v, weights[i]}
	}
	sort.Slice(pairs, func(i, j int) bool {
		return pairs[i].value < pairs[j].value
	})
	sortedValues := make([]float64, len(values))
	sortedWeights := make([]float64, len(values))
	for i, p := range pairs {
		sortedValues[i] = p.value
		sortedWeights[i] = p.weight
	}
	return sortedValues, sortedWeights
}

func median(values []float64) float64 {
	sort.Float64s(values)
	median := stat.Quantile(0.5, stat.Empirical, values, nil)
	return median
}

func iAvg(values []float64) float64 {
	return avg(values)
}

func wAvg(values, weightsValues []float64) float64 {
	return stat.Mean(values, weightsValues)
}

func avg(values []float64) float64 {

	return stat.Mean(values, nil)
}
