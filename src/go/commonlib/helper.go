package commonlib

import (
	"net"
	"strings"
)

// check if an ip address is IPV6
func IsIpv6(ip string) bool {
	return strings.Contains(ip, ":")
}

type ConnError struct {
	Out chan<- error
}

func (cerr *ConnError) ReportError(err error) {
	cerr.Out <- err
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
