// Code generated by capnpc-go. DO NOT EDIT.

package test

import (
	capnp "capnproto.org/go/capnp/v3"
	text "capnproto.org/go/capnp/v3/encoding/text"
	schemas "capnproto.org/go/capnp/v3/schemas"
	server "capnproto.org/go/capnp/v3/server"
	context "context"
)

type A struct{ Client *capnp.Client }

// A_TypeID is the unique identifier for the type A.
const A_TypeID = 0xba9eff6fb3abc84f

func (c A) Method(ctx context.Context, params func(A_method_Params) error) (A_method_Results_Future, capnp.ReleaseFunc) {
	s := capnp.Send{
		Method: capnp.Method{
			InterfaceID:   0xba9eff6fb3abc84f,
			MethodID:      0,
			InterfaceName: "a.capnp:A",
			MethodName:    "method",
		},
	}
	if params != nil {
		s.ArgsSize = capnp.ObjectSize{DataSize: 0, PointerCount: 1}
		s.PlaceArgs = func(s capnp.Struct) error { return params(A_method_Params{Struct: s}) }
	}
	ans, release := c.Client.SendCall(ctx, s)
	return A_method_Results_Future{Future: ans.Future()}, release
}

func (c A) AddRef() A {
	return A{
		Client: c.Client.AddRef(),
	}
}

func (c A) Release() {
	c.Client.Release()
}

// A A_Server is a A with a local implementation.
type A_Server interface {
	Method(context.Context, A_method) error
}

// A_NewServer creates a new Server from an implementation of A_Server.
func A_NewServer(s A_Server, policy *server.Policy) *server.Server {
	c, _ := s.(server.Shutdowner)
	return server.New(A_Methods(nil, s), s, c, policy)
}

// A_ServerToClient creates a new Client from an implementation of A_Server.
// The caller is responsible for calling Release on the returned Client.
func A_ServerToClient(s A_Server, policy *server.Policy) A {
	return A{Client: capnp.NewClient(A_NewServer(s, policy))}
}

// A_Methods appends Methods to a slice that invoke the methods on s.
// This can be used to create a more complicated Server.
func A_Methods(methods []server.Method, s A_Server) []server.Method {
	if cap(methods) == 0 {
		methods = make([]server.Method, 0, 1)
	}

	methods = append(methods, server.Method{
		Method: capnp.Method{
			InterfaceID:   0xba9eff6fb3abc84f,
			MethodID:      0,
			InterfaceName: "a.capnp:A",
			MethodName:    "method",
		},
		Impl: func(ctx context.Context, call *server.Call) error {
			return s.Method(ctx, A_method{call})
		},
	})

	return methods
}

// A_method holds the state for a server call to A.method.
// See server.Call for documentation.
type A_method struct {
	*server.Call
}

// Args returns the call's arguments.
func (c A_method) Args() A_method_Params {
	return A_method_Params{Struct: c.Call.Args()}
}

// AllocResults allocates the results struct.
func (c A_method) AllocResults() (A_method_Results, error) {
	r, err := c.Call.AllocResults(capnp.ObjectSize{DataSize: 0, PointerCount: 1})
	return A_method_Results{Struct: r}, err
}

type A_method_Params struct{ capnp.Struct }

// A_method_Params_TypeID is the unique identifier for the type A_method_Params.
const A_method_Params_TypeID = 0xc506e9c0e16825f7

func NewA_method_Params(s *capnp.Segment) (A_method_Params, error) {
	st, err := capnp.NewStruct(s, capnp.ObjectSize{DataSize: 0, PointerCount: 1})
	return A_method_Params{st}, err
}

func NewRootA_method_Params(s *capnp.Segment) (A_method_Params, error) {
	st, err := capnp.NewRootStruct(s, capnp.ObjectSize{DataSize: 0, PointerCount: 1})
	return A_method_Params{st}, err
}

func ReadRootA_method_Params(msg *capnp.Message) (A_method_Params, error) {
	root, err := msg.Root()
	return A_method_Params{root.Struct()}, err
}

func (s A_method_Params) String() string {
	str, _ := text.Marshal(0xc506e9c0e16825f7, s.Struct)
	return str
}

func (s A_method_Params) Param() (string, error) {
	p, err := s.Struct.Ptr(0)
	return p.Text(), err
}

func (s A_method_Params) HasParam() bool {
	return s.Struct.HasPtr(0)
}

func (s A_method_Params) ParamBytes() ([]byte, error) {
	p, err := s.Struct.Ptr(0)
	return p.TextBytes(), err
}

func (s A_method_Params) SetParam(v string) error {
	return s.Struct.SetText(0, v)
}

// A_method_Params_List is a list of A_method_Params.
type A_method_Params_List struct{ capnp.List }

// NewA_method_Params creates a new list of A_method_Params.
func NewA_method_Params_List(s *capnp.Segment, sz int32) (A_method_Params_List, error) {
	l, err := capnp.NewCompositeList(s, capnp.ObjectSize{DataSize: 0, PointerCount: 1}, sz)
	return A_method_Params_List{l}, err
}

func (s A_method_Params_List) At(i int) A_method_Params { return A_method_Params{s.List.Struct(i)} }

func (s A_method_Params_List) Set(i int, v A_method_Params) error {
	return s.List.SetStruct(i, v.Struct)
}

func (s A_method_Params_List) String() string {
	str, _ := text.MarshalList(0xc506e9c0e16825f7, s.List)
	return str
}

// A_method_Params_Future is a wrapper for a A_method_Params promised by a client call.
type A_method_Params_Future struct{ *capnp.Future }

func (p A_method_Params_Future) Struct() (A_method_Params, error) {
	s, err := p.Future.Struct()
	return A_method_Params{s}, err
}

type A_method_Results struct{ capnp.Struct }

// A_method_Results_TypeID is the unique identifier for the type A_method_Results.
const A_method_Results_TypeID = 0x9e2108f9306a75ef

func NewA_method_Results(s *capnp.Segment) (A_method_Results, error) {
	st, err := capnp.NewStruct(s, capnp.ObjectSize{DataSize: 0, PointerCount: 1})
	return A_method_Results{st}, err
}

func NewRootA_method_Results(s *capnp.Segment) (A_method_Results, error) {
	st, err := capnp.NewRootStruct(s, capnp.ObjectSize{DataSize: 0, PointerCount: 1})
	return A_method_Results{st}, err
}

func ReadRootA_method_Results(msg *capnp.Message) (A_method_Results, error) {
	root, err := msg.Root()
	return A_method_Results{root.Struct()}, err
}

func (s A_method_Results) String() string {
	str, _ := text.Marshal(0x9e2108f9306a75ef, s.Struct)
	return str
}

func (s A_method_Results) Res() (string, error) {
	p, err := s.Struct.Ptr(0)
	return p.Text(), err
}

func (s A_method_Results) HasRes() bool {
	return s.Struct.HasPtr(0)
}

func (s A_method_Results) ResBytes() ([]byte, error) {
	p, err := s.Struct.Ptr(0)
	return p.TextBytes(), err
}

func (s A_method_Results) SetRes(v string) error {
	return s.Struct.SetText(0, v)
}

// A_method_Results_List is a list of A_method_Results.
type A_method_Results_List struct{ capnp.List }

// NewA_method_Results creates a new list of A_method_Results.
func NewA_method_Results_List(s *capnp.Segment, sz int32) (A_method_Results_List, error) {
	l, err := capnp.NewCompositeList(s, capnp.ObjectSize{DataSize: 0, PointerCount: 1}, sz)
	return A_method_Results_List{l}, err
}

func (s A_method_Results_List) At(i int) A_method_Results { return A_method_Results{s.List.Struct(i)} }

func (s A_method_Results_List) Set(i int, v A_method_Results) error {
	return s.List.SetStruct(i, v.Struct)
}

func (s A_method_Results_List) String() string {
	str, _ := text.MarshalList(0x9e2108f9306a75ef, s.List)
	return str
}

// A_method_Results_Future is a wrapper for a A_method_Results promised by a client call.
type A_method_Results_Future struct{ *capnp.Future }

func (p A_method_Results_Future) Struct() (A_method_Results, error) {
	s, err := p.Future.Struct()
	return A_method_Results{s}, err
}

const schema_c4b468a2826bb79b = "x\xda\x12\xd8\xe7\xc0d\xc8\xaa\xce\xc2\xc0\x10h\xc0\xca" +
	"\xf6\xff}i\x96\xc1O\x0e\xc5y\x0c\x82\\\x8c\x0c\x0c" +
	"\xac\x8c\xec\x0c\x0c\xc2?\x19O10\x0a\xffe\xb4g" +
	"`\xfc\xef\x7fb\xf5\xe6\xfc\xff\xf3v1\x08r0\xff" +
	"\x9f\xbd=\xbbiQ\xc6\x96#\x0c\x0c\x8c\xc2\xb2LA" +
	"\xc2\x8aL \xe5\xb2L\xee\xc2\xae \xd6\xff\xef\xaa\x19" +
	"\x0f\x0f\xbcd;\x8al\x98.\xd3!\x06Fa]&" +
	"{\x86\x94\xff\x89z\xc9\x89\x05y\x05VL\x8ez\xb9" +
	"\xa9%\x19\xf9)*A\xa9\xc5\xa59%\xc5\x0c\x0c\x81" +
	",\xcc,\x0c\x0c,\x8c\x0c\x0c\x82\xbcJ\x0c\x0c\x81\x1c" +
	"\xcc\x8c\x81\"L\x8c\xecE\xa9\xc5\x8c<\x0cL\x8c<" +
	"\x0c\x8cp\xed\x0c\x8c\x8e\x01\x8c\x8c\x81,\xcc\xacHv" +
	"2\xc2|\"(h\xc5\xc0$\xc8\xcan\x0f\xb1\xc2\x81" +
	"1\x80\x91\x11\x8b\xcd\x01\x89E\x89\xb9\xc5(\xf6\x1a!" +
	"\xec\x95/\x00I\xc3l\x06\x04\x00\x00\xff\xff\x19\x1bK" +
	"\xa7"

func init() {
	schemas.Register(schema_c4b468a2826bb79b,
		0x9e2108f9306a75ef,
		0xba9eff6fb3abc84f,
		0xc506e9c0e16825f7)
}