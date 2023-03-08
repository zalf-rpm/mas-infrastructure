package main

import (
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

	// create a restorer
	restorer := commonlib.NewRestorer("localhost", 0) // port 0 means: use any free port
	bootStrapSturdyRef := restorer.BootstrapSturdyRef()
	// create a service
	gs := newGridService(restorer)
	// get the initial sturdy ref of that service
	initialSturdyRef, err := gs.commonGrid.InitialSturdyRef()
	if err != nil {
		panic(err)
	}

	// start listening for connections
	l, err := net.Listen("tcp", fmt.Sprintf("%s:%d", restorer.Host(), restorer.Port()))
	if err != nil {
		log.Fatal(err)
	}
	fmt.Printf("Service is listening on %s\n", l.Addr())
	fmt.Printf("service grid: %s)\n", initialSturdyRef)
	fmt.Printf("bootstrap restorer: %s )\n", bootStrapSturdyRef)

	errChan := make(chan error)
	// accept incomming connection from clients
	go func() {
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

	for {
		err := <-errChan
		fmt.Println(err)
	}

}

func Serve(conn net.Conn, restorer *commonlib.Restorer, errChan chan error) {

	main := persistence.Restorer_ServerToClient(restorer)
	// Listen for calls, using  bootstrap interface.
	rpc.NewConn(rpc.NewPackedStreamTransport(conn), &rpc.Options{BootstrapClient: capnp.Client(main), ErrorReporter: &commonlib.ConnError{Out: errChan}})
	// this connection will be close when the client closes the connection
}
