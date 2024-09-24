package main

import (
	"flag"
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
// serves groundwater data from netcdf file

// After loading the netcdf file, the service will provide the following information to the client:
// Provide inital capabilites to the client
//service: service sr: capnp://vat@host:port/uuid
//restorer_sr: capnp://vat@host:port

func main() {
	configPath := flag.String("config", "", "config file")
	configGen := flag.Bool("config-gen", false, "generate a config file")
	flag.Parse()

	// read the config file, if it exists
	var config *commonlib.Config
	var err error
	if *configGen {
		// generate a config file if it does not exist yet
		config, err = commonlib.ConfigGen(*configPath)
		if err != nil {
			log.Fatal(err)
		}
		fmt.Println("Config file generated at:", *configPath)
	} else {
		config, err = commonlib.ReadConfig(*configPath)
		if err != nil {
			log.Fatal(err)
		}
	}

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
	listener, err := config.ListenForConnections(restorer.Host(), restorer.Port())
	if err != nil {
		log.Fatal(err)
	}
	defer listener.Close()

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
