package main

import (
	"fmt"
	"log"
	"math"

	"github.com/batchatco/go-native-netcdf/netcdf"
	"github.com/batchatco/go-native-netcdf/netcdf/api"
	"github.com/google/uuid"
	"github.com/zalf-rpm/mas-infrastructure/src/go/commonlib"
)

type gridService struct {
	commonGrid *commonlib.Grid
}

func newGridService(restorer *commonlib.Restorer) *gridService {
	newCommonGrid := commonlib.NewGrid(restorer, uuid.New().String(), "groundwater", "Groundwater service")

	gs := gridService{
		commonGrid: newCommonGrid,
	}

	return &gs
}

type GridCoord struct {
	row int
	col int
}

// load a netcdf file, print credentials
func loadNetCDF(inputFile string) *api.Group {

	// Open the file
	nc, err := netcdf.Open(inputFile)
	if err != nil {
		panic(err)
	}
	defer nc.Close()

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

	for _, attr := range nc.ListVariables() {

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

	//createGWTimeSeries(&nc, start, end, *inputFile+".png", *eumask)
	return &nc
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

func createGWTimeSeries(nc *api.Group, inLat, inLon float64) float64 {

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

	startLat, startLon := findStartLatLon(valLat, valLon, lenLat, lenLon)
	stepLatSize := math.Abs(float64(valLat[0] - valLat[1]))
	stepLonSize := math.Abs(float64(valLon[0] - valLon[1]))

	// calculate index of lat and lon in gwValues
	iLat := ((startLat) + int64(inLat/stepLatSize))
	iLon := ((startLon) + int64(inLon/stepLonSize))

	// check if lat and lon of a neibor is closer, to correct rounding errors
	iLat = isNeiborCloser(valLat, iLat, inLat)
	iLon = isNeiborCloser(valLon, iLon, inLon)

	gw := gwValues[iLat][iLon] * -1
	if gw < 0 {
		gw = 0
	}
	return gw
}
