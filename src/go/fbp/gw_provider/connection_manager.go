package main

import (
	"context"
	"encoding/base64"
	"encoding/binary"
	"fmt"
	"log"
	"net"
	"net/url"
	"strconv"
	"strings"
	"time"

	"capnproto.org/go/capnp/v3"
	"capnproto.org/go/capnp/v3/rpc"
	"github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence"
)

type ConnectionManager struct {
	connStoppedChan chan string
	connections     map[string]net.Conn
	bootstraps      map[string]*capnp.Client
}

func NewConnectionManager() *ConnectionManager {

	return &ConnectionManager{
		connStoppedChan: make(chan string),
		connections:     make(map[string]net.Conn),
		bootstraps:      make(map[string]*capnp.Client),
	}
}

// run the connection manager
func (cm *ConnectionManager) Run() {
	for {
		connId := <-cm.connStoppedChan
		cm.connections[connId].Close()
		cm.bootstraps[connId].Release()
		delete(cm.connections, connId)
		delete(cm.bootstraps, connId)
	}
}

type SturdyRef struct {
	vat      vat
	localRef string
}

type vat struct {
	id      vatId
	address address
}
type address struct {
	host string
	port uint16
}
type vatId struct {
	publicKey0 uint64
	publicKey1 uint64
	publicKey2 uint64
	publicKey3 uint64
}

func NewSturdyRef(vatIds []uint64, host string, port uint16) *SturdyRef {
	return &SturdyRef{
		vat: vat{
			id: vatId{
				publicKey0: vatIds[0],
				publicKey1: vatIds[1],
				publicKey2: vatIds[2],
				publicKey3: vatIds[3],
			},
			address: address{
				host: host,
				port: port,
			},
		},
		localRef: "",
	}

}
func NewSturdyRefByString(sturdyRef string) (*SturdyRef, error) {
	//  capnp://vat-id_base64-curve25519-public-key@host:port/sturdy-ref-token_base64

	u, err := url.Parse(sturdyRef)
	if err != nil {
		return nil, err
	}
	vatIdBase64 := u.User.Username()
	vatId := vatId{}
	if vatIdBase64 != "" {
		vatIdBytes, err := base64.URLEncoding.DecodeString(vatIdBase64)
		if err != nil {
			vatIdBytes, err = base64.URLEncoding.DecodeString(vatIdBase64 + "=")
			if err != nil {
				return nil, err
			}
		}

		if len(vatIdBytes) != 32 {
			return nil, fmt.Errorf("vatIdBytes has wrong length: %d", len(vatIdBytes))
		}
		vatId.publicKey0 = binary.LittleEndian.Uint64(vatIdBytes[0:8])
		vatId.publicKey1 = binary.LittleEndian.Uint64(vatIdBytes[8:16])
		vatId.publicKey2 = binary.LittleEndian.Uint64(vatIdBytes[16:24])
		vatId.publicKey3 = binary.LittleEndian.Uint64(vatIdBytes[24:32])
	}
	host, port, err := net.SplitHostPort(u.Host)
	if err != nil {
		return nil, err
	}
	// port to uint16
	uintPort, err := strconv.ParseUint(port, 10, 16)
	if err != nil {
		return nil, err
	}
	sr_token_base64 := u.Path
	sr_token_base64 = strings.TrimPrefix(sr_token_base64, "/")
	sr_token := []byte{}
	if sr_token_base64 != "" {
		sr_token, err = base64.URLEncoding.DecodeString(sr_token_base64)
		if err != nil {
			return nil, err
		}
	}
	return &SturdyRef{
		vat: vat{
			id: vatId,
			address: address{
				host: host,
				port: uint16(uintPort),
			},
		},
		localRef: string(sr_token),
	}, nil
}

func (sr *SturdyRef) String() string {
	sr_token_base64 := ""
	if sr.localRef != "" {
		sr_token_base64 = base64.URLEncoding.EncodeToString([]byte(sr.localRef))
		sr_token_base64 = "/" + sr_token_base64
	}

	var sign_pk [32]byte
	binary.LittleEndian.PutUint64(sign_pk[0:8], sr.vat.id.publicKey0)
	binary.LittleEndian.PutUint64(sign_pk[8:16], sr.vat.id.publicKey1)
	binary.LittleEndian.PutUint64(sign_pk[16:24], sr.vat.id.publicKey2)
	binary.LittleEndian.PutUint64(sign_pk[24:32], sr.vat.id.publicKey3)
	vatID := base64.URLEncoding.EncodeToString(sign_pk[:])

	str := fmt.Sprintf("capnp://%s@%s:%d%s",
		vatID,
		sr.vat.address.host, // host
		sr.vat.address.port, // port
		sr_token_base64)     // sr_token
	return str
}

func (cm *ConnectionManager) connect(sturdyRef interface{}) (*capnp.Client, error) {

	// check if sturdyRef is a string or a SturdyRef
	var sr *SturdyRef
	var err error
	switch sturdyRefAsT := sturdyRef.(type) {
	case string:
		sr, err = NewSturdyRefByString(sturdyRefAsT)
		if err != nil {
			log.Fatal(err)
		}
	case *SturdyRef:
		sr = sturdyRefAsT
	default:
		log.Fatal(fmt.Errorf("sturdyRef is not a string or a SturdyRef"))
	}

	// creat URL path
	urlPath := fmt.Sprintf("%s:%d", sr.vat.address.host, sr.vat.address.port)
	if _, ok := cm.connections[urlPath]; !ok {
		// create new connection
		conn, err := net.Dial("tcp", urlPath)
		if err != nil {
			return nil, err
		}
		cm.connections[urlPath] = conn

		errC := make(chan error)
		go func(errChan <-chan error, stopChan chan<- string, urlPath string) {

			// program terminated by connection error
			err := <-errChan
			if err != nil {
				fmt.Println(err)
				stopChan <- urlPath
			}
		}(errC, cm.connStoppedChan, urlPath)

		// get Bootstrap
		connection := rpc.NewConn(rpc.NewPackedStreamTransport(conn), &rpc.Options{ErrorReporter: &ConnError{Out: errC}})
		client := connection.Bootstrap(context.Background())
		cm.bootstraps[urlPath] = &client
	}
	bootstrapCap := cm.bootstraps[urlPath]
	if sr.localRef != "" {
		restorer := persistence.Restorer(*bootstrapCap)
		futRes, relRes := restorer.Restore(context.Background(), func(p persistence.Restorer_RestoreParams) error {

			l, err := capnp.NewText(p.Segment(), sr.localRef)
			if err != nil {
				return err
			}

			err = p.SetLocalRef(l.ToPtr())
			//err = p.SetLocalRef(sr.localRef)

			// owner, err := persistence.NewSturdyRef_Owner(p.Segment())
			// if err != nil {
			// 	return err
			// }
			// owner.SetGuid(me) // TBD
			// p.SetSealedFor(owner)

			return err
		})
		defer relRes()
		results, err := futRes.Struct()
		if err != nil {
			log.Fatal(err)
		}
		if results.HasCap() {
			resultsCap := results.Cap()
			client := resultsCap.AddRef()
			return &client, nil
		} else {
			log.Fatal(fmt.Errorf("failed to resolve sturdy_ref"))
		}
	} else {
		return bootstrapCap, nil
	}

	// this should never happen
	return nil, fmt.Errorf("failed to resolve sturdy_ref")
}

func (cm *ConnectionManager) TryConnect(sr string, retry_count int, retry_secs int, print_retry_msgs bool) (*capnp.Client, error) {

	for {
		// try to connect
		conn, err := cm.connect(sr)
		if err == nil {
			return conn, nil
		} else {
			fmt.Println(err)
			if retry_count <= 0 {
				if print_retry_msgs {
					fmt.Println("Couldn't connect to sturdy_ref at ", sr)
				}
				return nil, err
			}
			retry_count--
			if print_retry_msgs {
				fmt.Println("Trying to connect to ", sr, " again in ", retry_secs, " secs!")
			}
			time.Sleep(time.Duration(retry_secs) * time.Second)
		}
	}

}

type ConnError struct {
	Out chan<- error
}

func (cerr *ConnError) ReportError(err error) {
	cerr.Out <- err
}
