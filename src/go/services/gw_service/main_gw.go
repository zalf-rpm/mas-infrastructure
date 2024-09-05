package main

import (
	"crypto/tls"
	"flag"
	"fmt"
	"log"
	"net"
	"os"
	"path/filepath"

	"capnproto.org/go/capnp/v3"
	"capnproto.org/go/capnp/v3/rpc"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence"
	"github.com/zalf-rpm/mas-infrastructure/src/go/commonlib"
)

// a capnproto service
// implementing the grid interface
// serves groundwater data from netcdf file

// After loading the netcdf file, the service will provide the following information to the client:
// Provide inital capabilites to the client
//service: service sr: capnp://vat@host:port/uuid
//restorer_sr: capnp://vat@host:port

func main() {
	useTLS := flag.String("tls", "", "directory containing server.crt and server.key for TLS")
	flag.Parse()

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
	hostStr := fmt.Sprintf("%s:%d", restorer.Host(), restorer.Port())
	var listener net.Listener
	if *useTLS != "" {
		// read the cert and key file
		certFile := filepath.Join(*useTLS, "server.crt")
		keyFile := filepath.Join(*useTLS, "server.key")
		_, err = os.Stat(certFile)
		if err != nil {
			log.Fatal(err)
		}
		_, err = os.Stat(keyFile)
		if err != nil {
			log.Fatal(err)
		}
		cert, err := tls.LoadX509KeyPair(certFile, keyFile)
		if err != nil {
			log.Fatal(err)
		}
		cfg := &tls.Config{Certificates: []tls.Certificate{cert}}
		listener, err = tls.Listen("tcp", hostStr, cfg)
		if err != nil {
			log.Fatal(err)
		}
		defer listener.Close()
	} else {

		// listen on a socket
		listener, err = net.Listen("tcp", hostStr)
		if err != nil {
			log.Fatal(err)
		}
		defer listener.Close()
	}

	fmt.Printf("Service is listening on %s\n", listener.Addr())
	fmt.Printf("service: service sr: %s\n", initialSturdyRef)
	fmt.Printf("restorer_sr: %s \n", bootStrapSturdyRef)

	errChan := make(chan error)
	msgChan := make(chan string)
	// accept incomming connection from clients
	go func() {
		main := persistence.Restorer_ServerToClient(restorer)
		defer main.Release()
		for {
			c, err := listener.Accept()
			fmt.Printf("service: request from %v\n", c.RemoteAddr())
			if err != nil {
				errChan <- err
				continue
			}
			Serve(c, capnp.Client(main.AddRef()), errChan, msgChan)
		}

	}()

	for {
		select {
		case msg := <-msgChan:
			fmt.Println(msg)
		case err := <-errChan:
			fmt.Println(err)
		}
	}

}

func Serve(conn net.Conn, boot capnp.Client, errChan chan error, msgChan chan string) {

	// Listen for calls, using  bootstrap interface.
	rpc.NewConn(rpc.NewStreamTransport(conn), &rpc.Options{BootstrapClient: boot, Logger: &commonlib.ConnError{Out: errChan, Msg: msgChan}})
	// this connection will be close when the client closes the connection
}
