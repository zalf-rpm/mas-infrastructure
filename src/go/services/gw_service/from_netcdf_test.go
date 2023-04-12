package main

import (
	"math"
	"reflect"
	"testing"

	"github.com/google/go-cmp/cmp"
	"github.com/zalf-rpm/mas-infrastructure/src/go/commonlib"
)

const testNC_EURASIA = "EURASIA_WTD_annualmean.nc"

func Test_loadNetCDF(t *testing.T) {
	type args struct {
		inputFile string
	}
	tests := []struct {
		name    string
		args    args
		want    loadedNetCDFMeta
		wantErr bool
	}{
		{
			name: "testNC_EURASIA",
			args: args{
				inputFile: testNC_EURASIA,
			},
			want: loadedNetCDFMeta{
				data:        nil,
				numRows:     9960,
				numCols:     23280,
				startLatIdx: 0,
				startLonIdx: 1680,
				stepLatSize: 0.008333299309015274, //0,008333299309015274 //0,004166649654507637
				stepLonSize: 0.008333206176757812, //0,008333206176757812 //0,004166603088378906
				gridResolution: commonlib.Resolution{
					Value: 0.008333206176757812, //0,008333206176757812
				},
				gridUnit:   "degrees",
				noDataType: 2,
				bounds: commonlib.LatLonBoundaries{
					TopLeft: commonlib.LatLon{
						Lat: 82.9955062866211,    //82,9955062866211
						Lon: -13.995833396911621, //-13,995833396911621
					},
					TopRight: commonlib.LatLon{
						Lat: 82.9955062866211,
						Lon: 179.99505615234375, //179,99505615234375
					},
					BottomLeft: commonlib.LatLon{
						Lat: 0.004166666883975267, //0,004166666883975267
						Lon: -13.995833396911621,
					},
					BottomRight: commonlib.LatLon{
						Lat: 0.004166666883975267,
						Lon: 179.99505615234375,
					},
				},
				boundsFromCellCenter: commonlib.LatLonBoundaries{
					TopLeft: commonlib.LatLon{
						Lat: 82.9955062866211 + 0.008333299309015274/2,
						Lon: -13.995833396911621 - 0.008333206176757812/2,
					},
					TopRight: commonlib.LatLon{
						Lat: 82.9955062866211 + 0.008333299309015274/2,
						Lon: 179.99505615234375 + 0.008333206176757812/2,
					},
					BottomLeft: commonlib.LatLon{
						Lat: 0.004166666883975267 - 0.008333299309015274/2,
						Lon: -13.995833396911621 - 0.008333206176757812/2,
					},
					BottomRight: commonlib.LatLon{
						Lat: 0.004166666883975267 - 0.008333299309015274/2,
						Lon: 179.99505615234375 + 0.008333206176757812/2,
					},
				},
				timeValues: []float32{1},
			},
			wantErr: false,
		},
	}
	opt := cmp.Comparer(func(x, y float64) bool {
		delta := math.Abs(x - y)
		mean := math.Abs(x+y) / 2.0
		return delta/mean < 0.00001
	})
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			got, err := loadNetCDF(tt.args.inputFile)
			if (err != nil) != tt.wantErr {
				t.Errorf("loadNetCDF() error = %v, wantErr %v", err, tt.wantErr)
				return
			}
			defer (*got.data).Close()

			if !cmp.Equal(got.bounds, tt.want.bounds) {
				t.Errorf("loadNetCDF()Bounds = %v, want %v", got.bounds, tt.want.bounds)
			}
			if !cmp.Equal(tt.want.boundsFromCellCenter, got.boundsFromCellCenter, opt) {
				t.Errorf("loadNetCDF()BoundsFromCellCenter = %v, want %v", got.boundsFromCellCenter, tt.want.boundsFromCellCenter)
			}
			if !cmp.Equal(got.gridResolution, tt.want.gridResolution, opt) {
				t.Errorf("loadNetCDF()GridResolution = %v, want %v", got.gridResolution, tt.want.gridResolution)
			}
			if !cmp.Equal(got.gridUnit, tt.want.gridUnit, opt) {
				t.Errorf("loadNetCDF()GridUnit = %v, want %v", got.gridUnit, tt.want.gridUnit)
			}
			if !cmp.Equal(got.noDataType, tt.want.noDataType, opt) {
				t.Errorf("loadNetCDF()NoDataType = %v, want %v", got.noDataType, tt.want.noDataType)
			}
			if !cmp.Equal(got.numCols, tt.want.numCols, opt) {
				t.Errorf("loadNetCDF()NumCols = %v, want %v", got.numCols, tt.want.numCols)
			}
			if !cmp.Equal(got.numRows, tt.want.numRows, opt) {
				t.Errorf("loadNetCDF()NumRows = %v, want %v", got.numRows, tt.want.numRows)
			}
			if !cmp.Equal(got.startLatIdx, tt.want.startLatIdx, opt) {
				t.Errorf("loadNetCDF()StartLatIdx = %v, want %v", got.startLatIdx, tt.want.startLatIdx)
			}
			if !cmp.Equal(got.startLonIdx, tt.want.startLonIdx, opt) {
				t.Errorf("loadNetCDF()StartLonIdx = %v, want %v", got.startLonIdx, tt.want.startLonIdx)
			}
			if !cmp.Equal(got.stepLatSize, tt.want.stepLatSize, opt) {
				t.Errorf("loadNetCDF()StepLatSize = %v, want %v", got.stepLatSize, tt.want.stepLatSize)
			}
			if !cmp.Equal(got.stepLonSize, tt.want.stepLonSize, opt) {
				t.Errorf("loadNetCDF()StepLonSize = %v, want %v", got.stepLonSize, tt.want.stepLonSize)
			}
			if !cmp.Equal(got.timeValues, tt.want.timeValues, opt) {
				t.Errorf("loadNetCDF()TimeValues = %v, want %v", got.timeValues, tt.want.timeValues)
			}
		})
	}
}

func Test_gridService_GetValueLatLon(t *testing.T) {
	type args struct {
		inLat float64
		inLon float64
	}

	meta, err := loadNetCDF(testNC_EURASIA)
	if err != nil {
		t.Errorf("loadNetCDF() error = %v", err)
		return
	}
	defer (*meta.data).Close()

	newCommonGrid := &commonlib.Grid{
		GridResolution:       commonlib.Resolution{},
		GridUnit:             "",
		NumRows:              0,
		NumCols:              0,
		NoDataType:           t,
		Bounds:               commonlib.LatLonBoundaries{},
		BoundsFromCellCenter: commonlib.LatLonBoundaries{},
	}
	gs := &gridService{
		commonGrid: newCommonGrid,
	}
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
	gs.wdt = meta.wdt
	gs.scaleFactor = meta.scaleFactor
	gs.add_offset = meta.add_offset
	gs.mask = meta.mask

	tests := []struct {
		name  string
		gs    *gridService
		args  args
		want  float64
		want1 int64
		want2 int64
	}{
		{
			name: "testNC_EURASIA_Oderbruch_latlon",
			gs:   gs,
			args: args{
				inLat: 52.583039, // 52.583039, 14.533146
				inLon: 14.533146,
			},
			want:  0,
			want1: 6309,
			want2: 3423,
		},
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			//gw, iLat, iLon := tt.gs.GetValueLatLon(tt.args.nc, tt.args.inLat, tt.args.inLon)
			gw, iLat, iLon := tt.gs.GetValueLatLon(tt.args.inLat, tt.args.inLon)
			if gw != tt.want {
				t.Errorf("gridService.GetValueLatLon() got = %v, want %v", gw, tt.want)
			}
			if iLat != tt.want1 {
				t.Errorf("gridService.GetValueLatLon() got1 = %v, want %v", iLat, tt.want1)
			}
			if iLon != tt.want2 {
				t.Errorf("gridService.GetValueLatLon() got2 = %v, want %v", iLon, tt.want2)
			}
		})
	}
}

func Test_gridService_GetValueRowCol(t *testing.T) {
	type args struct {
		row uint64
		col uint64
	}
	meta, err := loadNetCDF(testNC_EURASIA)
	if err != nil {
		t.Errorf("loadNetCDF() error = %v", err)
		return
	}
	defer (*meta.data).Close()

	newCommonGrid := &commonlib.Grid{
		GridResolution:       commonlib.Resolution{},
		GridUnit:             "",
		NumRows:              0,
		NumCols:              0,
		NoDataType:           t,
		Bounds:               commonlib.LatLonBoundaries{},
		BoundsFromCellCenter: commonlib.LatLonBoundaries{},
	}
	gs := &gridService{
		commonGrid: newCommonGrid,
	}
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
	gs.wdt = meta.wdt
	gs.scaleFactor = meta.scaleFactor
	gs.add_offset = meta.add_offset
	gs.mask = meta.mask

	tests := []struct {
		name    string
		gs      *gridService
		args    args
		want    interface{}
		wantErr bool
	}{
		// TODO: Add test cases.
		{
			name: "testNC_EURASIA_Oderbruch_rowcol",
			gs:   gs,
			args: args{
				row: 6309,
				col: 3423,
			},
			want:    0.0,
			wantErr: false,
		},
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			got, err := tt.gs.GetValueRowCol(tt.args.row, tt.args.col)
			if (err != nil) != tt.wantErr {
				t.Errorf("gridService.GetValueRowCol() error = %v, wantErr %v", err, tt.wantErr)
				return
			}
			if !reflect.DeepEqual(got, tt.want) {
				t.Errorf("gridService.GetValueRowCol() = %v, want %v", got, tt.want)
			}
		})
	}
}
