package main

import (
	"context"
	"crypto/tls"
	"crypto/x509"
	"flag"
	"fmt"
	"log"
	"net"
	"os"
	"path/filepath"

	"capnproto.org/go/capnp/v3/rpc"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/test"
)

func main() {
	useTls := flag.String("tls", "", "path to tls root cert")
	registryAddrPtr := flag.String("registry", "localhost:1234", "address of registry")
	flag.Parse()

	var conn net.Conn
	var err error
	if *useTls != "" {
		caFile := filepath.Join(*useTls, "ca.crt")
		_, err := os.Stat(caFile)
		if err != nil {
			log.Fatal(err)
		}
		data, err := os.ReadFile(caFile)
		if err != nil {
			log.Fatal(err)
		}
		roots := x509.NewCertPool()
		ok := roots.AppendCertsFromPEM(data)
		if !ok {
			log.Fatal("failed to parse root certificate")
		}
		tlsconfig := &tls.Config{
			RootCAs: roots,
		}
		conn, err = tls.Dial("tcp", *registryAddrPtr, tlsconfig)
		if err != nil {
			log.Fatal(err)
		}
		defer conn.Close()
	} else {
		conn, err = net.Dial("tcp", *registryAddrPtr)
		if err != nil {
			log.Fatal(err)
		}
		defer conn.Close()
	}
	// establish connection to registry
	connection := rpc.NewConn(rpc.NewPackedStreamTransport(conn), &rpc.Options{Logger: &ConnError{}})
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

// Logger interface
func (cerr *ConnError) Debug(message string, args ...any) {
	fmt.Println(message)
}
func (cerr *ConnError) Info(message string, args ...any) {
	fmt.Println(message)
}
func (cerr *ConnError) Warn(message string, args ...any) {
	fmt.Println(message)
}

func (cerr *ConnError) Error(message string, args ...any) {
	fmt.Println(message)
}

func (cerr *ConnError) ReportError(err error) {
	fmt.Println(err)
}
