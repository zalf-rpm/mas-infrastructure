package commonlib

import (
	"fmt"
	"net"
	"strings"
)

// check if an ip address is IPV6
func IsIpv6(ip string) bool {
	return strings.Contains(ip, ":")
}

func GetFreePort() (port int, err error) {
	var a *net.TCPAddr
	if a, err = net.ResolveTCPAddr("tcp", "localhost:0"); err == nil {
		var l *net.TCPListener
		if l, err = net.ListenTCP("tcp", a); err == nil {
			defer l.Close()
			return l.Addr().(*net.TCPAddr).Port, nil
		}
	}
	return
}

type ConnError struct {
	Out chan<- error
	Msg chan<- string
}

// Logger interface
func (cerr *ConnError) Debug(message string, args ...any) {
	cerr.Msg <- message
}
func (cerr *ConnError) Info(message string, args ...any) {
	cerr.Msg <- message
}
func (cerr *ConnError) Warn(message string, args ...any) {
	cerr.Msg <- message
}

func (cerr *ConnError) Error(message string, args ...any) {
	cerr.Out <- fmt.Errorf(message)
}
