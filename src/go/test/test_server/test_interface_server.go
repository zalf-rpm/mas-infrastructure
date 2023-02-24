package main

import (
	"context"
	"fmt"
	"log"
	"net"

	"capnproto.org/go/capnp/v3"
	"capnproto.org/go/capnp/v3/rpc"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/test"
)

type A_Server struct{}

func (a *A_Server) Method(ctx context.Context, call test.A_method) error {
	first, err := call.Args().Param()
	if err != nil {
		return err
	}

	res, err := call.AllocResults() // allocate the results struct
	if err != nil {
		return err
	}

	res.SetRes(first + " world")

	fmt.Println("server: Method Received", first)

	return nil
}
func main() {

	// listen on a socket
	l, err := net.Listen("tcp", "localhost:1234")
	if err != nil {
		log.Fatal(err)
	}
	defer l.Close()

	// accept connections and serve
	c, err := l.Accept()
	if err != nil {
		log.Fatal(err)
	}
	fmt.Println("server: accepted connection from", c.RemoteAddr())
	server := A_Server{}
	client := test.A_ServerToClient(&server)
	errorChan := make(chan error)
	conn := rpc.NewConn(rpc.NewPackedStreamTransport(c), &rpc.Options{BootstrapClient: capnp.Client(client), ErrorReporter: &ConnError{Out: errorChan}})
	defer conn.Close()

	fmt.Println("Bootstraping" + c.RemoteAddr().String())
	for {
		select {
		case <-conn.Done():
			fmt.Println("Connection closed")
			return
		case err := <-errorChan:
			fmt.Println("Error reported:", err)
			return
		}
	}
}

type ConnError struct {
	Out chan<- error
}

func (cerr *ConnError) ReportError(err error) {
	cerr.Out <- err
}
