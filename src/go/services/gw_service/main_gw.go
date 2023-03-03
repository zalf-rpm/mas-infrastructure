package main

import (
	"context"
	"fmt"
	"log"
	"net"

	"capnproto.org/go/capnp/v3"
	"capnproto.org/go/capnp/v3/rpc"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence"
	"github.com/zalf-rpm/mas-infrastructure/src/go/commonlib"
)

// a capnproto service
// implementing the grid interface
// serves groundwater data

// TODO:
// implement capnp service interface
// implement loading code for gw data from netcdf files
// implement test client

// Provide inital capabilites to the client
//service: service sr: capnp://vat@host:port/uuid
//service: admin sr: capnp://vat@host:port/uuid
//restorer_sr: capnp://vat@host:port
// debug grid: capnp://vat@host:port/uuid

func main() {
	port, err := commonlib.GetFreePort()
	if err != nil {
		panic(err)
	}
	gs := gridService{host: "localhost", port: port}
	l, err := net.Listen("tcp", fmt.Sprintf("%s:%d", gs.host, gs.port))
	if err != nil {
		log.Fatal(err)
	}
	fmt.Printf("Service is listening on %s\n", l.Addr())
	errChan := make(chan error)
	// accept incomming connection from services we started
	go func() {
		for {
			c, err := l.Accept()
			fmt.Printf("service: request from %v\n", c.RemoteAddr())
			if err != nil {
				errChan <- err
				continue
			}
			ServeGrid(c, &gs, errChan)
		}
	}()

	for {
		err := <-errChan
		fmt.Println(err)
	}

}

func ServeGrid(conn net.Conn, gs *gridService, errChan chan error) {

	main := persistence.Restorer_ServerToClient(gs)
	// Listen for calls, using  bootstrap interface.
	rpc.NewConn(rpc.NewPackedStreamTransport(conn), &rpc.Options{BootstrapClient: capnp.Client(main), ErrorReporter: &commonlib.ConnError{Out: errChan}})
	// this connection will be close when the client closes the connection

}

type gridService struct {
	host string
	port int

	capabilities []string
}

// type Restorer_Server interface

func (gs *gridService) Restore(c context.Context, r persistence.Restorer_restore) error {
	fmt.Printf("service: restore request from %v\n", c)
	return nil
}
