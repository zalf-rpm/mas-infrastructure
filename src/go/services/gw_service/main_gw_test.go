package main

import (
	"context"
	"fmt"
	"net"
	"testing"

	"github.com/stretchr/testify/assert"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/grid"
	"github.com/zalf-rpm/mas-infrastructure/src/go/commonlib"
)

func TestServe(t *testing.T) {

	// create a restorer
	restorer := commonlib.NewRestorer("localhost", 0) // port 0 means: use any free port
	// create a service
	gs := newGridService(restorer)
	// get the initial sturdy ref of that service
	initialSturdyRef, err := gs.commonGrid.InitialSturdyRef()
	assert.NoError(t, err)
	t.Log("StrudyRef:", initialSturdyRef)
	errChan := make(chan error)

	go func() {
		l, err := net.Listen("tcp", fmt.Sprintf("%s:%d", restorer.Host(), restorer.Port()))
		assert.NoError(t, err)

		for {
			c, err := l.Accept()
			fmt.Printf("service: request from %v\n", c.RemoteAddr())
			if err != nil {
				errChan <- err
				continue
			}
			Serve(c, restorer, errChan)
		}
	}()

	go func() {
		for {
			err := <-errChan
			fmt.Println(err)
		}
	}()

	conMgr := commonlib.NewConnectionManager()
	// create a connection to the restorer
	initialSdr, err := conMgr.TryConnect(initialSturdyRef.String(), 1, 1, false)
	assert.NoError(t, err)
	// get the grid service interface
	gridClient := grid.Grid(*initialSdr)

	// TODO: test all fuctions of the grid service interface
	var maxRows uint64 = 10
	var maxCols uint64 = 10
	t.Run("dimension", func(t *testing.T) {
		result, rel := gridClient.Dimension(context.Background(), func(p grid.Grid_dimension_Params) error {
			fmt.Println("dimension")
			return nil
		})
		defer rel()
		results, err := result.Struct()
		assert.NoError(t, err)
		maxRows = results.Rows()
		assert.Equal(t, uint64(9960), maxRows)
		maxCols = results.Cols()
		assert.Equal(t, uint64(23280), maxCols)
	})

	t.Run("unit", func(t *testing.T) {
		result, rel := gridClient.Unit(context.Background(), func(p grid.Grid_unit_Params) error {
			fmt.Println("unit")
			return nil
		})
		defer rel()
		results, err := result.Struct()
		assert.NoError(t, err)
		assert.True(t, results.HasUnit())
		unit, err := results.Unit()
		assert.NoError(t, err)
		assert.Equal(t, "degree", unit)
	})

	t.Run("noDataValue", func(t *testing.T) {
		result, rel := gridClient.NoDataValue(context.Background(), func(p grid.Grid_noDataValue_Params) error {
			fmt.Println("noDataValue")
			return nil
		})
		defer rel()
		results, err := result.Struct()
		assert.NoError(t, err)
		assert.True(t, results.HasNodata())
		val, err := results.Nodata()
		assert.NoError(t, err)
		if val.Which() == grid.Grid_Value_Which_f {
			assert.Equal(t, 2.0, val.F())
		} else if val.Which() == grid.Grid_Value_Which_i {
			assert.Equal(t, int64(2), val.I())
		}

	})

	t.Run("latLonBounds_use_center", func(t *testing.T) {
		result, rel := gridClient.LatLonBounds(context.Background(), func(p grid.Grid_latLonBounds_Params) error {
			fmt.Println("latLonBounds_use_center")
			p.SetUseCellCenter(true)
			return nil
		})
		defer rel()
		results, err := result.Struct()
		assert.NoError(t, err)
		assert.True(t, results.HasTl()) // top left
		assert.True(t, results.HasBr()) // bottom right
		assert.True(t, results.HasTr()) // top right
		assert.True(t, results.HasBl()) // bottom left

		topLeft, err := results.Tl()
		assert.NoError(t, err)
		lat := topLeft.Lat()
		lon := topLeft.Lon()
		assert.Equal(t, 82.9955062866211, lat)
		assert.Equal(t, -13.995833396911621, lon)

		bottomRight, err := results.Br()
		assert.NoError(t, err)
		lat = bottomRight.Lat()
		lon = bottomRight.Lon()
		assert.Equal(t, 0.004166666883975267, lat)
		assert.Equal(t, 179.99505615234375, lon)

		topRight, err := results.Tr()
		assert.NoError(t, err)
		lat = topRight.Lat()
		lon = topRight.Lon()
		assert.Equal(t, 82.9955062866211, lat)
		assert.Equal(t, 179.99505615234375, lon)

		bottomLeft, err := results.Bl()
		assert.NoError(t, err)
		lat = bottomLeft.Lat()
		lon = bottomLeft.Lon()
		assert.Equal(t, 0.004166666883975267, lat)
		assert.Equal(t, -13.995833396911621, lon)
	})

	t.Run("latLonBounds", func(t *testing.T) {
		result, rel := gridClient.LatLonBounds(context.Background(), func(p grid.Grid_latLonBounds_Params) error {
			fmt.Println("latLonBounds")
			p.SetUseCellCenter(false)
			return nil
		})
		defer rel()

		results, err := result.Struct()
		assert.NoError(t, err)
		assert.True(t, results.HasTl()) // top left
		assert.True(t, results.HasBr()) // bottom right
		assert.True(t, results.HasTr()) // top right
		assert.True(t, results.HasBl()) // bottom left

		topLeft, err := results.Tl()
		assert.NoError(t, err)
		lat := topLeft.Lat()
		lon := topLeft.Lon()
		assert.InDelta(t, 82.9955062866211+0.008333206176757812/2, lat, 0.0001)
		assert.InDelta(t, -13.995833396911621-0.008333206176757812/2, lon, 0.0001)

		bottomRight, err := results.Br()
		assert.NoError(t, err)
		lat = bottomRight.Lat()
		lon = bottomRight.Lon()
		assert.InDelta(t, 0.004166666883975267-0.008333206176757812/2, lat, 0.0001)
		assert.InDelta(t, 179.99505615234375+0.008333206176757812/2, lon, 0.0001)

		topRight, err := results.Tr()
		assert.NoError(t, err)
		lat = topRight.Lat()
		lon = topRight.Lon()
		assert.InDelta(t, 82.9955062866211+0.008333206176757812/2, lat, 0.0001)
		assert.InDelta(t, 179.99505615234375+0.008333206176757812/2, lon, 0.0001)

		bottomLeft, err := results.Bl()
		assert.NoError(t, err)
		lat = bottomLeft.Lat()
		lon = bottomLeft.Lon()
		valLat := 0.004166666883975267 - 0.008333206176757812/2
		valLon := -13.995833396911621 - 0.008333206176757812/2
		assert.InDelta(t, valLat, lat, 0.0001)
		assert.InDelta(t, valLon, lon, 0.0001)

	})

	t.Run("resolution", func(t *testing.T) {
		result, rel := gridClient.Resolution(context.Background(), func(p grid.Grid_resolution_Params) error {
			fmt.Println("resolution")
			return nil
		})
		defer rel()
		results, err := result.Struct()
		assert.NoError(t, err)
		assert.True(t, results.HasRes())
		res, err := results.Res()
		assert.NoError(t, err)
		if res.Which() == grid.Grid_Resolution_Which_degree {
			assert.Equal(t, 0.008333206176757812, res.Degree())
		}
	})

	t.Run("stream", func(t *testing.T) {
		result, rel := gridClient.StreamCells(context.Background(), func(p grid.Grid_streamCells_Params) error {
			topleft, err := p.NewTopLeft()
			assert.NoError(t, err)
			topleft.SetRow(0)
			topleft.SetCol(0)
			bottomright, err := p.NewBottomRight()
			assert.NoError(t, err)
			bottomright.SetRow(maxRows - 1)
			bottomright.SetCol(maxCols - 1)

			fmt.Println("StreamCells")
			return nil
		})
		defer rel()
		results, err := result.Struct()
		assert.NoError(t, err)
		assert.True(t, results.HasCallback())
		callBack := results.Callback()
		assert.NotNil(t, callBack)

		for sendIdx := 0; sendIdx < 15; sendIdx++ {
			resFut, release := callBack.SendCells(context.Background(), func(p grid.Grid_Callback_sendCells_Params) error {
				p.SetMaxCount(10000)
				fmt.Println("StreamCells callback")
				return nil
			})

			res, err := resFut.Struct()
			if err != nil {
				fmt.Println(err)
				break
			}
			assert.NoError(t, err)
			if res.HasLocations() == false {
				break
			}

			locations, err := res.Locations()
			assert.NoError(t, err)
			for i := 0; i < locations.Len(); i++ {
				loc := locations.At(i)
				assert.NotNil(t, loc)
				assert.True(t, loc.HasRowCol())
				assert.True(t, loc.HasValue())
				assert.True(t, loc.HasLatLonCoord())
			}
			release()
		}
	})

}
