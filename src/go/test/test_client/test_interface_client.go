package main

import (
	"context"
	"flag"
	"fmt"
	"log"
	"net"

	"capnproto.org/go/capnp/v3/rpc"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/test"
)

func main() {
	registryAddrPtr := flag.String("registry", "localhost:1234", "address of registry")
	flag.Parse()

	conn, err := net.Dial("tcp", *registryAddrPtr)
	if err != nil {
		log.Fatal(err)
	}
	defer conn.Close()
	// establish connection to registry
	connection := rpc.NewConn(rpc.NewPackedStreamTransport(conn), &rpc.Options{ErrorReporter: &ConnError{}})
	defer connection.Close()
	// get ModelRunController Bootstrap
	a := test.A(connection.Bootstrap(context.Background()))

	f, release := a.Method(context.Background(), func(ps test.A_method_Params) error {
		err := ps.SetParam("hello")
		if err != nil {
			return err
		}

		return nil
	})

	res, err := f.Struct()
	if err != nil {
		fmt.Println(err)
	}
	fmt.Print("Answer: ")
	fmt.Println(res.Res())
	release()
	fmt.Println("Message released")
	a.Release()
	fmt.Println("A released")
}

type ConnError struct {
}

func (cerr *ConnError) ReportError(err error) {
	fmt.Println(err)
}
