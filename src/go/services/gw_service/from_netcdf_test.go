package main

import (
	"math"
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
