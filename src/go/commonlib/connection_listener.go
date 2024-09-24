package commonlib

import (
	"crypto/tls"
	"fmt"
	"net"
	"os"
	"path/filepath"
)

// ListenForConnections listens for incoming connections
func (c *Config) ListenForConnections(host string, port uint16) (net.Listener, error) {

	// start listening for connections
	hostStr := fmt.Sprintf("%s:%d", host, port)
	var listener net.Listener
	var err error
	if c.TLS.Use {
		// read the cert and key file
		certFile := filepath.Join(c.TLS.CertName)
		keyFile := filepath.Join(c.TLS.KeyName)
		_, err := os.Stat(certFile)
		if err != nil {
			// check if the file exists in current executable directory
			certFile = filepath.Join(filepath.Dir(os.Args[0]), c.TLS.CertName)
			_, err = os.Stat(certFile)
			if err != nil {
				return nil, err
			}
		}
		_, err = os.Stat(keyFile)
		if err != nil {
			// check if the file exists in current executable directory
			keyFile = filepath.Join(filepath.Dir(os.Args[0]), c.TLS.KeyName)
			_, err = os.Stat(keyFile)
			if err != nil {
				return nil, err
			}
		}
		cert, err := tls.LoadX509KeyPair(certFile, keyFile)
		if err != nil {
			return nil, err
		}
		cfg := &tls.Config{Certificates: []tls.Certificate{cert}}
		listener, err = tls.Listen("tcp", hostStr, cfg)
		if err != nil {
			return nil, err
		}
	} else {

		// listen on a socket
		listener, err = net.Listen("tcp", hostStr)
		if err != nil {
			return nil, err
		}
	}
	return listener, nil
}
