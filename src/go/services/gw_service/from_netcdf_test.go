package main

import (
	"reflect"
	"testing"

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
				stepLatSize: 0.008333299309015274,
				stepLonSize: 0.008333206176757812,
				gridResolution: commonlib.Resolution{
					Value: 0.008333206176757812, //0,008333206176757812
				},
				gridUnit:   "degrees",
				noDataType: 2,
				bounds: commonlib.LatLonBoundaries{
					TopLeft: commonlib.LatLon{
						Lat: 82.9955062866211,    //82,9996729362756
						Lon: -13.995833396911621, //-13,991666793823242
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
						Lat: 82.995506333187221094,
						Lon: -13.987500190734863094,
					},
					TopRight: commonlib.LatLon{
						Lat: 82.995506333187221094,
						Lon: 179.990889549255371094,
					},
					BottomLeft: commonlib.LatLon{
						Lat: 0.008333206176757812,
						Lon: -13.987500190734863094,
					},
					BottomRight: commonlib.LatLon{
						Lat: 0.008333206176757812,
						Lon: 179.990889549255371094,
					},
				},
				timeValues: []float32{1},
			},
			wantErr: false,
		},
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			got, err := loadNetCDF(tt.args.inputFile)
			if (err != nil) != tt.wantErr {
				t.Errorf("loadNetCDF() error = %v, wantErr %v", err, tt.wantErr)
				return
			}
			defer (*got.data).Close()

			if !reflect.DeepEqual(got.bounds, tt.want.bounds) {
				t.Errorf("loadNetCDF()Bounds = %v, want %v", got.bounds, tt.want.bounds)
			}
			if !reflect.DeepEqual(got.boundsFromCellCenter, tt.want.boundsFromCellCenter) {
				t.Errorf("loadNetCDF()BoundsFromCellCenter = %v, want %v", got.boundsFromCellCenter, tt.want.boundsFromCellCenter)
			}
			if !reflect.DeepEqual(got.gridResolution, tt.want.gridResolution) {
				t.Errorf("loadNetCDF()GridResolution = %v, want %v", got.gridResolution, tt.want.gridResolution)
			}
			if !reflect.DeepEqual(got.gridUnit, tt.want.gridUnit) {
				t.Errorf("loadNetCDF()GridUnit = %v, want %v", got.gridUnit, tt.want.gridUnit)
			}
			if !reflect.DeepEqual(got.noDataType, tt.want.noDataType) {
				t.Errorf("loadNetCDF()NoDataType = %v, want %v", got.noDataType, tt.want.noDataType)
			}
			if !reflect.DeepEqual(got.numCols, tt.want.numCols) {
				t.Errorf("loadNetCDF()NumCols = %v, want %v", got.numCols, tt.want.numCols)
			}
			if !reflect.DeepEqual(got.numRows, tt.want.numRows) {
				t.Errorf("loadNetCDF()NumRows = %v, want %v", got.numRows, tt.want.numRows)
			}
			if !reflect.DeepEqual(got.startLatIdx, tt.want.startLatIdx) {
				t.Errorf("loadNetCDF()StartLatIdx = %v, want %v", got.startLatIdx, tt.want.startLatIdx)
			}
			if !reflect.DeepEqual(got.startLonIdx, tt.want.startLonIdx) {
				t.Errorf("loadNetCDF()StartLonIdx = %v, want %v", got.startLonIdx, tt.want.startLonIdx)
			}
			if !reflect.DeepEqual(got.stepLatSize, tt.want.stepLatSize) {
				t.Errorf("loadNetCDF()StepLatSize = %v, want %v", got.stepLatSize, tt.want.stepLatSize)
			}
			if !reflect.DeepEqual(got.stepLonSize, tt.want.stepLonSize) {
				t.Errorf("loadNetCDF()StepLonSize = %v, want %v", got.stepLonSize, tt.want.stepLonSize)
			}
			if !reflect.DeepEqual(got.timeValues, tt.want.timeValues) {
				t.Errorf("loadNetCDF()TimeValues = %v, want %v", got.timeValues, tt.want.timeValues)
			}
		})
	}
}
