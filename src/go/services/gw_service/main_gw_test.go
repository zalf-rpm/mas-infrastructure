package main

import (
	"context"
	"flag"
	"fmt"
	"log"
	"net"
	"os"
	"runtime/pprof"
	"testing"

	"github.com/stretchr/testify/assert"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/grid"
	"github.com/zalf-rpm/mas-infrastructure/src/go/commonlib"
)

var cpuprofile = flag.String("cpuprofile", "cpu.prof", "write cpu profile to `file`")
var memprofile = flag.String("memprofile", "mem.prof", "write memory profile to `file`")

func TestServe(t *testing.T) {

	flag.Parse()
	if *cpuprofile != "" {
		f, err := os.Create(*cpuprofile)
		if err != nil {
			log.Fatal("could not create CPU profile: ", err)
		}
		defer f.Close() // error handling omitted for example
		if err := pprof.StartCPUProfile(f); err != nil {
			log.Fatal("could not start CPU profile: ", err)
		}
		defer pprof.StopCPUProfile()
	}

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

		counter := 0

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
			//assert.NoError(t, err)
			if res.HasLocations() == false {
				break
			}

			locations, err := res.Locations()
			//assert.NoError(t, err)
			for i := 0; i < locations.Len(); i++ {
				// loc := locations.At(i)
				// assert.NotNil(t, loc)
				// assert.True(t, loc.HasRowCol())
				// assert.True(t, loc.HasValue())
				// assert.True(t, loc.HasLatLonCoord())
				counter++
				// TODO: check if the values are correct
			}
			release()
		}
		// if counter != 9960*23280+1 {
		// 	t.Errorf("counter is %d, expected %d", counter, 9960*23280+1)
		// }
	})

	// if *memprofile != "" {
	// 	f, err := os.Create(*memprofile)
	// 	if err != nil {
	// 		log.Fatal("could not create memory profile: ", err)
	// 	}
	// 	defer f.Close() // error handling omitted for example
	// 	runtime.GC()    // get up-to-date statistics
	// 	if err := pprof.WriteHeapProfile(f); err != nil {
	// 		log.Fatal("could not write memory profile: ", err)
	// 	}
	// }
}
