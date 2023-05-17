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
	type args struct {
		conn     net.Conn
		restorer *commonlib.Restorer
		errChan  chan error
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
	t.Run("stream", func(t *testing.T) {
		result, rel := gridClient.StreamCells(context.Background(), func(p grid.Grid_streamCells_Params) error {
			fmt.Println("StreamCells")
			return nil
		})
		defer rel()
		results, err := result.Struct()
		assert.NoError(t, err)
		assert.True(t, results.HasCallback())

	})

}
