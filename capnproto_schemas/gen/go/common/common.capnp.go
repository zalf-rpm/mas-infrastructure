// Code generated by capnpc-go. DO NOT EDIT.

package common

import (
	capnp "capnproto.org/go/capnp/v3"
	text "capnproto.org/go/capnp/v3/encoding/text"
	schemas "capnproto.org/go/capnp/v3/schemas"
	server "capnproto.org/go/capnp/v3/server"
	context "context"
	math "math"
	strconv "strconv"
)

type IdInformation struct{ capnp.Struct }

// IdInformation_TypeID is the unique identifier for the type IdInformation.
const IdInformation_TypeID = 0xd4cb7ecbfe03dad3

func NewIdInformation(s *capnp.Segment) (IdInformation, error) {
	st, err := capnp.NewStruct(s, capnp.ObjectSize{DataSize: 0, PointerCount: 3})
	return IdInformation{st}, err
}

func NewRootIdInformation(s *capnp.Segment) (IdInformation, error) {
	st, err := capnp.NewRootStruct(s, capnp.ObjectSize{DataSize: 0, PointerCount: 3})
	return IdInformation{st}, err
}

func ReadRootIdInformation(msg *capnp.Message) (IdInformation, error) {
	root, err := msg.Root()
	return IdInformation{root.Struct()}, err
}

func (s IdInformation) String() string {
	str, _ := text.Marshal(0xd4cb7ecbfe03dad3, s.Struct)
	return str
}

func (s IdInformation) Id() (string, error) {
	p, err := s.Struct.Ptr(0)
	return p.Text(), err
}

func (s IdInformation) HasId() bool {
	return s.Struct.HasPtr(0)
}

func (s IdInformation) IdBytes() ([]byte, error) {
	p, err := s.Struct.Ptr(0)
	return p.TextBytes(), err
}

func (s IdInformation) SetId(v string) error {
	return s.Struct.SetText(0, v)
}

func (s IdInformation) Name() (string, error) {
	p, err := s.Struct.Ptr(1)
	return p.Text(), err
}

func (s IdInformation) HasName() bool {
	return s.Struct.HasPtr(1)
}

func (s IdInformation) NameBytes() ([]byte, error) {
	p, err := s.Struct.Ptr(1)
	return p.TextBytes(), err
}

func (s IdInformation) SetName(v string) error {
	return s.Struct.SetText(1, v)
}

func (s IdInformation) Description() (string, error) {
	p, err := s.Struct.Ptr(2)
	return p.Text(), err
}

func (s IdInformation) HasDescription() bool {
	return s.Struct.HasPtr(2)
}

func (s IdInformation) DescriptionBytes() ([]byte, error) {
	p, err := s.Struct.Ptr(2)
	return p.TextBytes(), err
}

func (s IdInformation) SetDescription(v string) error {
	return s.Struct.SetText(2, v)
}

// IdInformation_List is a list of IdInformation.
type IdInformation_List struct{ capnp.List }

// NewIdInformation creates a new list of IdInformation.
func NewIdInformation_List(s *capnp.Segment, sz int32) (IdInformation_List, error) {
	l, err := capnp.NewCompositeList(s, capnp.ObjectSize{DataSize: 0, PointerCount: 3}, sz)
	return IdInformation_List{l}, err
}

func (s IdInformation_List) At(i int) IdInformation { return IdInformation{s.List.Struct(i)} }

func (s IdInformation_List) Set(i int, v IdInformation) error { return s.List.SetStruct(i, v.Struct) }

func (s IdInformation_List) String() string {
	str, _ := text.MarshalList(0xd4cb7ecbfe03dad3, s.List)
	return str
}

// IdInformation_Future is a wrapper for a IdInformation promised by a client call.
type IdInformation_Future struct{ *capnp.Future }

func (p IdInformation_Future) Struct() (IdInformation, error) {
	s, err := p.Future.Struct()
	return IdInformation{s}, err
}

type Identifiable struct{ Client *capnp.Client }

// Identifiable_TypeID is the unique identifier for the type Identifiable.
const Identifiable_TypeID = 0xb2afd1cb599c48d5

func (c Identifiable) Info(ctx context.Context, params func(Identifiable_info_Params) error) (IdInformation_Future, capnp.ReleaseFunc) {
	s := capnp.Send{
		Method: capnp.Method{
			InterfaceID:   0xb2afd1cb599c48d5,
			MethodID:      0,
			InterfaceName: "common.capnp:Identifiable",
			MethodName:    "info",
		},
	}
	if params != nil {
		s.ArgsSize = capnp.ObjectSize{DataSize: 0, PointerCount: 0}
		s.PlaceArgs = func(s capnp.Struct) error { return params(Identifiable_info_Params{Struct: s}) }
	}
	ans, release := c.Client.SendCall(ctx, s)
	return IdInformation_Future{Future: ans.Future()}, release
}

func (c Identifiable) AddRef() Identifiable {
	return Identifiable{
		Client: c.Client.AddRef(),
	}
}

func (c Identifiable) Release() {
	c.Client.Release()
}

// A Identifiable_Server is a Identifiable with a local implementation.
type Identifiable_Server interface {
	Info(context.Context, Identifiable_info) error
}

// Identifiable_NewServer creates a new Server from an implementation of Identifiable_Server.
func Identifiable_NewServer(s Identifiable_Server, policy *server.Policy) *server.Server {
	c, _ := s.(server.Shutdowner)
	return server.New(Identifiable_Methods(nil, s), s, c, policy)
}

// Identifiable_ServerToClient creates a new Client from an implementation of Identifiable_Server.
// The caller is responsible for calling Release on the returned Client.
func Identifiable_ServerToClient(s Identifiable_Server, policy *server.Policy) Identifiable {
	return Identifiable{Client: capnp.NewClient(Identifiable_NewServer(s, policy))}
}

// Identifiable_Methods appends Methods to a slice that invoke the methods on s.
// This can be used to create a more complicated Server.
func Identifiable_Methods(methods []server.Method, s Identifiable_Server) []server.Method {
	if cap(methods) == 0 {
		methods = make([]server.Method, 0, 1)
	}

	methods = append(methods, server.Method{
		Method: capnp.Method{
			InterfaceID:   0xb2afd1cb599c48d5,
			MethodID:      0,
			InterfaceName: "common.capnp:Identifiable",
			MethodName:    "info",
		},
		Impl: func(ctx context.Context, call *server.Call) error {
			return s.Info(ctx, Identifiable_info{call})
		},
	})

	return methods
}

// Identifiable_info holds the state for a server call to Identifiable.info.
// See server.Call for documentation.
type Identifiable_info struct {
	*server.Call
}

// Args returns the call's arguments.
func (c Identifiable_info) Args() Identifiable_info_Params {
	return Identifiable_info_Params{Struct: c.Call.Args()}
}

// AllocResults allocates the results struct.
func (c Identifiable_info) AllocResults() (IdInformation, error) {
	r, err := c.Call.AllocResults(capnp.ObjectSize{DataSize: 0, PointerCount: 3})
	return IdInformation{Struct: r}, err
}

type Identifiable_info_Params struct{ capnp.Struct }

// Identifiable_info_Params_TypeID is the unique identifier for the type Identifiable_info_Params.
const Identifiable_info_Params_TypeID = 0x9d8aa1cf1e49deb1

func NewIdentifiable_info_Params(s *capnp.Segment) (Identifiable_info_Params, error) {
	st, err := capnp.NewStruct(s, capnp.ObjectSize{DataSize: 0, PointerCount: 0})
	return Identifiable_info_Params{st}, err
}

func NewRootIdentifiable_info_Params(s *capnp.Segment) (Identifiable_info_Params, error) {
	st, err := capnp.NewRootStruct(s, capnp.ObjectSize{DataSize: 0, PointerCount: 0})
	return Identifiable_info_Params{st}, err
}

func ReadRootIdentifiable_info_Params(msg *capnp.Message) (Identifiable_info_Params, error) {
	root, err := msg.Root()
	return Identifiable_info_Params{root.Struct()}, err
}

func (s Identifiable_info_Params) String() string {
	str, _ := text.Marshal(0x9d8aa1cf1e49deb1, s.Struct)
	return str
}

// Identifiable_info_Params_List is a list of Identifiable_info_Params.
type Identifiable_info_Params_List struct{ capnp.List }

// NewIdentifiable_info_Params creates a new list of Identifiable_info_Params.
func NewIdentifiable_info_Params_List(s *capnp.Segment, sz int32) (Identifiable_info_Params_List, error) {
	l, err := capnp.NewCompositeList(s, capnp.ObjectSize{DataSize: 0, PointerCount: 0}, sz)
	return Identifiable_info_Params_List{l}, err
}

func (s Identifiable_info_Params_List) At(i int) Identifiable_info_Params {
	return Identifiable_info_Params{s.List.Struct(i)}
}

func (s Identifiable_info_Params_List) Set(i int, v Identifiable_info_Params) error {
	return s.List.SetStruct(i, v.Struct)
}

func (s Identifiable_info_Params_List) String() string {
	str, _ := text.MarshalList(0x9d8aa1cf1e49deb1, s.List)
	return str
}

// Identifiable_info_Params_Future is a wrapper for a Identifiable_info_Params promised by a client call.
type Identifiable_info_Params_Future struct{ *capnp.Future }

func (p Identifiable_info_Params_Future) Struct() (Identifiable_info_Params, error) {
	s, err := p.Future.Struct()
	return Identifiable_info_Params{s}, err
}

type StructuredText struct{ capnp.Struct }
type StructuredText_structure StructuredText
type StructuredText_structure_Which uint16

const (
	StructuredText_structure_Which_none StructuredText_structure_Which = 0
	StructuredText_structure_Which_json StructuredText_structure_Which = 1
	StructuredText_structure_Which_xml  StructuredText_structure_Which = 2
)

func (w StructuredText_structure_Which) String() string {
	const s = "nonejsonxml"
	switch w {
	case StructuredText_structure_Which_none:
		return s[0:4]
	case StructuredText_structure_Which_json:
		return s[4:8]
	case StructuredText_structure_Which_xml:
		return s[8:11]

	}
	return "StructuredText_structure_Which(" + strconv.FormatUint(uint64(w), 10) + ")"
}

// StructuredText_TypeID is the unique identifier for the type StructuredText.
const StructuredText_TypeID = 0xed6c098b67cad454

func NewStructuredText(s *capnp.Segment) (StructuredText, error) {
	st, err := capnp.NewStruct(s, capnp.ObjectSize{DataSize: 8, PointerCount: 1})
	return StructuredText{st}, err
}

func NewRootStructuredText(s *capnp.Segment) (StructuredText, error) {
	st, err := capnp.NewRootStruct(s, capnp.ObjectSize{DataSize: 8, PointerCount: 1})
	return StructuredText{st}, err
}

func ReadRootStructuredText(msg *capnp.Message) (StructuredText, error) {
	root, err := msg.Root()
	return StructuredText{root.Struct()}, err
}

func (s StructuredText) String() string {
	str, _ := text.Marshal(0xed6c098b67cad454, s.Struct)
	return str
}

func (s StructuredText) Value() (string, error) {
	p, err := s.Struct.Ptr(0)
	return p.Text(), err
}

func (s StructuredText) HasValue() bool {
	return s.Struct.HasPtr(0)
}

func (s StructuredText) ValueBytes() ([]byte, error) {
	p, err := s.Struct.Ptr(0)
	return p.TextBytes(), err
}

func (s StructuredText) SetValue(v string) error {
	return s.Struct.SetText(0, v)
}

func (s StructuredText) Structure() StructuredText_structure { return StructuredText_structure(s) }

func (s StructuredText_structure) Which() StructuredText_structure_Which {
	return StructuredText_structure_Which(s.Struct.Uint16(0))
}
func (s StructuredText_structure) SetNone() {
	s.Struct.SetUint16(0, 0)

}

func (s StructuredText_structure) SetJson() {
	s.Struct.SetUint16(0, 1)

}

func (s StructuredText_structure) SetXml() {
	s.Struct.SetUint16(0, 2)

}

// StructuredText_List is a list of StructuredText.
type StructuredText_List struct{ capnp.List }

// NewStructuredText creates a new list of StructuredText.
func NewStructuredText_List(s *capnp.Segment, sz int32) (StructuredText_List, error) {
	l, err := capnp.NewCompositeList(s, capnp.ObjectSize{DataSize: 8, PointerCount: 1}, sz)
	return StructuredText_List{l}, err
}

func (s StructuredText_List) At(i int) StructuredText { return StructuredText{s.List.Struct(i)} }

func (s StructuredText_List) Set(i int, v StructuredText) error { return s.List.SetStruct(i, v.Struct) }

func (s StructuredText_List) String() string {
	str, _ := text.MarshalList(0xed6c098b67cad454, s.List)
	return str
}

// StructuredText_Future is a wrapper for a StructuredText promised by a client call.
type StructuredText_Future struct{ *capnp.Future }

func (p StructuredText_Future) Struct() (StructuredText, error) {
	s, err := p.Future.Struct()
	return StructuredText{s}, err
}

func (p StructuredText_Future) Structure() StructuredText_structure_Future {
	return StructuredText_structure_Future{p.Future}
}

// StructuredText_structure_Future is a wrapper for a StructuredText_structure promised by a client call.
type StructuredText_structure_Future struct{ *capnp.Future }

func (p StructuredText_structure_Future) Struct() (StructuredText_structure, error) {
	s, err := p.Future.Struct()
	return StructuredText_structure{s}, err
}

type Value struct{ capnp.Struct }
type Value_Which uint16

const (
	Value_Which_f64   Value_Which = 0
	Value_Which_f32   Value_Which = 1
	Value_Which_i64   Value_Which = 2
	Value_Which_i32   Value_Which = 3
	Value_Which_i16   Value_Which = 4
	Value_Which_i8    Value_Which = 5
	Value_Which_ui64  Value_Which = 6
	Value_Which_ui32  Value_Which = 7
	Value_Which_ui16  Value_Which = 8
	Value_Which_ui8   Value_Which = 9
	Value_Which_b     Value_Which = 10
	Value_Which_t     Value_Which = 11
	Value_Which_d     Value_Which = 12
	Value_Which_p     Value_Which = 13
	Value_Which_cap   Value_Which = 14
	Value_Which_lf64  Value_Which = 15
	Value_Which_lf32  Value_Which = 16
	Value_Which_li64  Value_Which = 17
	Value_Which_li32  Value_Which = 18
	Value_Which_li16  Value_Which = 19
	Value_Which_li8   Value_Which = 20
	Value_Which_lui64 Value_Which = 21
	Value_Which_lui32 Value_Which = 22
	Value_Which_lui16 Value_Which = 23
	Value_Which_lui8  Value_Which = 24
	Value_Which_lb    Value_Which = 25
	Value_Which_lt    Value_Which = 26
	Value_Which_ld    Value_Which = 27
	Value_Which_lcap  Value_Which = 28
)

func (w Value_Which) String() string {
	const s = "f64f32i64i32i16i8ui64ui32ui16ui8btdpcaplf64lf32li64li32li16li8lui64lui32lui16lui8lbltldlcap"
	switch w {
	case Value_Which_f64:
		return s[0:3]
	case Value_Which_f32:
		return s[3:6]
	case Value_Which_i64:
		return s[6:9]
	case Value_Which_i32:
		return s[9:12]
	case Value_Which_i16:
		return s[12:15]
	case Value_Which_i8:
		return s[15:17]
	case Value_Which_ui64:
		return s[17:21]
	case Value_Which_ui32:
		return s[21:25]
	case Value_Which_ui16:
		return s[25:29]
	case Value_Which_ui8:
		return s[29:32]
	case Value_Which_b:
		return s[32:33]
	case Value_Which_t:
		return s[33:34]
	case Value_Which_d:
		return s[34:35]
	case Value_Which_p:
		return s[35:36]
	case Value_Which_cap:
		return s[36:39]
	case Value_Which_lf64:
		return s[39:43]
	case Value_Which_lf32:
		return s[43:47]
	case Value_Which_li64:
		return s[47:51]
	case Value_Which_li32:
		return s[51:55]
	case Value_Which_li16:
		return s[55:59]
	case Value_Which_li8:
		return s[59:62]
	case Value_Which_lui64:
		return s[62:67]
	case Value_Which_lui32:
		return s[67:72]
	case Value_Which_lui16:
		return s[72:77]
	case Value_Which_lui8:
		return s[77:81]
	case Value_Which_lb:
		return s[81:83]
	case Value_Which_lt:
		return s[83:85]
	case Value_Which_ld:
		return s[85:87]
	case Value_Which_lcap:
		return s[87:91]

	}
	return "Value_Which(" + strconv.FormatUint(uint64(w), 10) + ")"
}

// Value_TypeID is the unique identifier for the type Value.
const Value_TypeID = 0xe17592335373b246

func NewValue(s *capnp.Segment) (Value, error) {
	st, err := capnp.NewStruct(s, capnp.ObjectSize{DataSize: 16, PointerCount: 1})
	return Value{st}, err
}

func NewRootValue(s *capnp.Segment) (Value, error) {
	st, err := capnp.NewRootStruct(s, capnp.ObjectSize{DataSize: 16, PointerCount: 1})
	return Value{st}, err
}

func ReadRootValue(msg *capnp.Message) (Value, error) {
	root, err := msg.Root()
	return Value{root.Struct()}, err
}

func (s Value) String() string {
	str, _ := text.Marshal(0xe17592335373b246, s.Struct)
	return str
}

func (s Value) Which() Value_Which {
	return Value_Which(s.Struct.Uint16(8))
}
func (s Value) F64() float64 {
	if s.Struct.Uint16(8) != 0 {
		panic("Which() != f64")
	}
	return math.Float64frombits(s.Struct.Uint64(0))
}

func (s Value) SetF64(v float64) {
	s.Struct.SetUint16(8, 0)
	s.Struct.SetUint64(0, math.Float64bits(v))
}

func (s Value) F32() float32 {
	if s.Struct.Uint16(8) != 1 {
		panic("Which() != f32")
	}
	return math.Float32frombits(s.Struct.Uint32(0))
}

func (s Value) SetF32(v float32) {
	s.Struct.SetUint16(8, 1)
	s.Struct.SetUint32(0, math.Float32bits(v))
}

func (s Value) I64() int64 {
	if s.Struct.Uint16(8) != 2 {
		panic("Which() != i64")
	}
	return int64(s.Struct.Uint64(0))
}

func (s Value) SetI64(v int64) {
	s.Struct.SetUint16(8, 2)
	s.Struct.SetUint64(0, uint64(v))
}

func (s Value) I32() int32 {
	if s.Struct.Uint16(8) != 3 {
		panic("Which() != i32")
	}
	return int32(s.Struct.Uint32(0))
}

func (s Value) SetI32(v int32) {
	s.Struct.SetUint16(8, 3)
	s.Struct.SetUint32(0, uint32(v))
}

func (s Value) I16() int16 {
	if s.Struct.Uint16(8) != 4 {
		panic("Which() != i16")
	}
	return int16(s.Struct.Uint16(0))
}

func (s Value) SetI16(v int16) {
	s.Struct.SetUint16(8, 4)
	s.Struct.SetUint16(0, uint16(v))
}

func (s Value) I8() int8 {
	if s.Struct.Uint16(8) != 5 {
		panic("Which() != i8")
	}
	return int8(s.Struct.Uint8(0))
}

func (s Value) SetI8(v int8) {
	s.Struct.SetUint16(8, 5)
	s.Struct.SetUint8(0, uint8(v))
}

func (s Value) Ui64() uint64 {
	if s.Struct.Uint16(8) != 6 {
		panic("Which() != ui64")
	}
	return s.Struct.Uint64(0)
}

func (s Value) SetUi64(v uint64) {
	s.Struct.SetUint16(8, 6)
	s.Struct.SetUint64(0, v)
}

func (s Value) Ui32() uint32 {
	if s.Struct.Uint16(8) != 7 {
		panic("Which() != ui32")
	}
	return s.Struct.Uint32(0)
}

func (s Value) SetUi32(v uint32) {
	s.Struct.SetUint16(8, 7)
	s.Struct.SetUint32(0, v)
}

func (s Value) Ui16() uint16 {
	if s.Struct.Uint16(8) != 8 {
		panic("Which() != ui16")
	}
	return s.Struct.Uint16(0)
}

func (s Value) SetUi16(v uint16) {
	s.Struct.SetUint16(8, 8)
	s.Struct.SetUint16(0, v)
}

func (s Value) Ui8() uint8 {
	if s.Struct.Uint16(8) != 9 {
		panic("Which() != ui8")
	}
	return s.Struct.Uint8(0)
}

func (s Value) SetUi8(v uint8) {
	s.Struct.SetUint16(8, 9)
	s.Struct.SetUint8(0, v)
}

func (s Value) B() bool {
	if s.Struct.Uint16(8) != 10 {
		panic("Which() != b")
	}
	return s.Struct.Bit(0)
}

func (s Value) SetB(v bool) {
	s.Struct.SetUint16(8, 10)
	s.Struct.SetBit(0, v)
}

func (s Value) T() (string, error) {
	if s.Struct.Uint16(8) != 11 {
		panic("Which() != t")
	}
	p, err := s.Struct.Ptr(0)
	return p.Text(), err
}

func (s Value) HasT() bool {
	if s.Struct.Uint16(8) != 11 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) TBytes() ([]byte, error) {
	p, err := s.Struct.Ptr(0)
	return p.TextBytes(), err
}

func (s Value) SetT(v string) error {
	s.Struct.SetUint16(8, 11)
	return s.Struct.SetText(0, v)
}

func (s Value) D() ([]byte, error) {
	if s.Struct.Uint16(8) != 12 {
		panic("Which() != d")
	}
	p, err := s.Struct.Ptr(0)
	return []byte(p.Data()), err
}

func (s Value) HasD() bool {
	if s.Struct.Uint16(8) != 12 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) SetD(v []byte) error {
	s.Struct.SetUint16(8, 12)
	return s.Struct.SetData(0, v)
}

func (s Value) P() (capnp.Ptr, error) {
	if s.Struct.Uint16(8) != 13 {
		panic("Which() != p")
	}
	return s.Struct.Ptr(0)
}

func (s Value) HasP() bool {
	if s.Struct.Uint16(8) != 13 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) SetP(v capnp.Ptr) error {
	s.Struct.SetUint16(8, 13)
	return s.Struct.SetPtr(0, v)
}

func (s Value) Cap() (capnp.Ptr, error) {
	if s.Struct.Uint16(8) != 14 {
		panic("Which() != cap")
	}
	return s.Struct.Ptr(0)
}

func (s Value) HasCap() bool {
	if s.Struct.Uint16(8) != 14 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) SetCap(v capnp.Ptr) error {
	s.Struct.SetUint16(8, 14)
	return s.Struct.SetPtr(0, v)
}

func (s Value) Lf64() (capnp.Float64List, error) {
	if s.Struct.Uint16(8) != 15 {
		panic("Which() != lf64")
	}
	p, err := s.Struct.Ptr(0)
	return capnp.Float64List{List: p.List()}, err
}

func (s Value) HasLf64() bool {
	if s.Struct.Uint16(8) != 15 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) SetLf64(v capnp.Float64List) error {
	s.Struct.SetUint16(8, 15)
	return s.Struct.SetPtr(0, v.List.ToPtr())
}

// NewLf64 sets the lf64 field to a newly
// allocated capnp.Float64List, preferring placement in s's segment.
func (s Value) NewLf64(n int32) (capnp.Float64List, error) {
	s.Struct.SetUint16(8, 15)
	l, err := capnp.NewFloat64List(s.Struct.Segment(), n)
	if err != nil {
		return capnp.Float64List{}, err
	}
	err = s.Struct.SetPtr(0, l.List.ToPtr())
	return l, err
}

func (s Value) Lf32() (capnp.Float32List, error) {
	if s.Struct.Uint16(8) != 16 {
		panic("Which() != lf32")
	}
	p, err := s.Struct.Ptr(0)
	return capnp.Float32List{List: p.List()}, err
}

func (s Value) HasLf32() bool {
	if s.Struct.Uint16(8) != 16 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) SetLf32(v capnp.Float32List) error {
	s.Struct.SetUint16(8, 16)
	return s.Struct.SetPtr(0, v.List.ToPtr())
}

// NewLf32 sets the lf32 field to a newly
// allocated capnp.Float32List, preferring placement in s's segment.
func (s Value) NewLf32(n int32) (capnp.Float32List, error) {
	s.Struct.SetUint16(8, 16)
	l, err := capnp.NewFloat32List(s.Struct.Segment(), n)
	if err != nil {
		return capnp.Float32List{}, err
	}
	err = s.Struct.SetPtr(0, l.List.ToPtr())
	return l, err
}

func (s Value) Li64() (capnp.Int64List, error) {
	if s.Struct.Uint16(8) != 17 {
		panic("Which() != li64")
	}
	p, err := s.Struct.Ptr(0)
	return capnp.Int64List{List: p.List()}, err
}

func (s Value) HasLi64() bool {
	if s.Struct.Uint16(8) != 17 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) SetLi64(v capnp.Int64List) error {
	s.Struct.SetUint16(8, 17)
	return s.Struct.SetPtr(0, v.List.ToPtr())
}

// NewLi64 sets the li64 field to a newly
// allocated capnp.Int64List, preferring placement in s's segment.
func (s Value) NewLi64(n int32) (capnp.Int64List, error) {
	s.Struct.SetUint16(8, 17)
	l, err := capnp.NewInt64List(s.Struct.Segment(), n)
	if err != nil {
		return capnp.Int64List{}, err
	}
	err = s.Struct.SetPtr(0, l.List.ToPtr())
	return l, err
}

func (s Value) Li32() (capnp.Int32List, error) {
	if s.Struct.Uint16(8) != 18 {
		panic("Which() != li32")
	}
	p, err := s.Struct.Ptr(0)
	return capnp.Int32List{List: p.List()}, err
}

func (s Value) HasLi32() bool {
	if s.Struct.Uint16(8) != 18 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) SetLi32(v capnp.Int32List) error {
	s.Struct.SetUint16(8, 18)
	return s.Struct.SetPtr(0, v.List.ToPtr())
}

// NewLi32 sets the li32 field to a newly
// allocated capnp.Int32List, preferring placement in s's segment.
func (s Value) NewLi32(n int32) (capnp.Int32List, error) {
	s.Struct.SetUint16(8, 18)
	l, err := capnp.NewInt32List(s.Struct.Segment(), n)
	if err != nil {
		return capnp.Int32List{}, err
	}
	err = s.Struct.SetPtr(0, l.List.ToPtr())
	return l, err
}

func (s Value) Li16() (capnp.Int16List, error) {
	if s.Struct.Uint16(8) != 19 {
		panic("Which() != li16")
	}
	p, err := s.Struct.Ptr(0)
	return capnp.Int16List{List: p.List()}, err
}

func (s Value) HasLi16() bool {
	if s.Struct.Uint16(8) != 19 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) SetLi16(v capnp.Int16List) error {
	s.Struct.SetUint16(8, 19)
	return s.Struct.SetPtr(0, v.List.ToPtr())
}

// NewLi16 sets the li16 field to a newly
// allocated capnp.Int16List, preferring placement in s's segment.
func (s Value) NewLi16(n int32) (capnp.Int16List, error) {
	s.Struct.SetUint16(8, 19)
	l, err := capnp.NewInt16List(s.Struct.Segment(), n)
	if err != nil {
		return capnp.Int16List{}, err
	}
	err = s.Struct.SetPtr(0, l.List.ToPtr())
	return l, err
}

func (s Value) Li8() (capnp.Int8List, error) {
	if s.Struct.Uint16(8) != 20 {
		panic("Which() != li8")
	}
	p, err := s.Struct.Ptr(0)
	return capnp.Int8List{List: p.List()}, err
}

func (s Value) HasLi8() bool {
	if s.Struct.Uint16(8) != 20 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) SetLi8(v capnp.Int8List) error {
	s.Struct.SetUint16(8, 20)
	return s.Struct.SetPtr(0, v.List.ToPtr())
}

// NewLi8 sets the li8 field to a newly
// allocated capnp.Int8List, preferring placement in s's segment.
func (s Value) NewLi8(n int32) (capnp.Int8List, error) {
	s.Struct.SetUint16(8, 20)
	l, err := capnp.NewInt8List(s.Struct.Segment(), n)
	if err != nil {
		return capnp.Int8List{}, err
	}
	err = s.Struct.SetPtr(0, l.List.ToPtr())
	return l, err
}

func (s Value) Lui64() (capnp.UInt64List, error) {
	if s.Struct.Uint16(8) != 21 {
		panic("Which() != lui64")
	}
	p, err := s.Struct.Ptr(0)
	return capnp.UInt64List{List: p.List()}, err
}

func (s Value) HasLui64() bool {
	if s.Struct.Uint16(8) != 21 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) SetLui64(v capnp.UInt64List) error {
	s.Struct.SetUint16(8, 21)
	return s.Struct.SetPtr(0, v.List.ToPtr())
}

// NewLui64 sets the lui64 field to a newly
// allocated capnp.UInt64List, preferring placement in s's segment.
func (s Value) NewLui64(n int32) (capnp.UInt64List, error) {
	s.Struct.SetUint16(8, 21)
	l, err := capnp.NewUInt64List(s.Struct.Segment(), n)
	if err != nil {
		return capnp.UInt64List{}, err
	}
	err = s.Struct.SetPtr(0, l.List.ToPtr())
	return l, err
}

func (s Value) Lui32() (capnp.UInt32List, error) {
	if s.Struct.Uint16(8) != 22 {
		panic("Which() != lui32")
	}
	p, err := s.Struct.Ptr(0)
	return capnp.UInt32List{List: p.List()}, err
}

func (s Value) HasLui32() bool {
	if s.Struct.Uint16(8) != 22 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) SetLui32(v capnp.UInt32List) error {
	s.Struct.SetUint16(8, 22)
	return s.Struct.SetPtr(0, v.List.ToPtr())
}

// NewLui32 sets the lui32 field to a newly
// allocated capnp.UInt32List, preferring placement in s's segment.
func (s Value) NewLui32(n int32) (capnp.UInt32List, error) {
	s.Struct.SetUint16(8, 22)
	l, err := capnp.NewUInt32List(s.Struct.Segment(), n)
	if err != nil {
		return capnp.UInt32List{}, err
	}
	err = s.Struct.SetPtr(0, l.List.ToPtr())
	return l, err
}

func (s Value) Lui16() (capnp.UInt16List, error) {
	if s.Struct.Uint16(8) != 23 {
		panic("Which() != lui16")
	}
	p, err := s.Struct.Ptr(0)
	return capnp.UInt16List{List: p.List()}, err
}

func (s Value) HasLui16() bool {
	if s.Struct.Uint16(8) != 23 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) SetLui16(v capnp.UInt16List) error {
	s.Struct.SetUint16(8, 23)
	return s.Struct.SetPtr(0, v.List.ToPtr())
}

// NewLui16 sets the lui16 field to a newly
// allocated capnp.UInt16List, preferring placement in s's segment.
func (s Value) NewLui16(n int32) (capnp.UInt16List, error) {
	s.Struct.SetUint16(8, 23)
	l, err := capnp.NewUInt16List(s.Struct.Segment(), n)
	if err != nil {
		return capnp.UInt16List{}, err
	}
	err = s.Struct.SetPtr(0, l.List.ToPtr())
	return l, err
}

func (s Value) Lui8() (capnp.UInt8List, error) {
	if s.Struct.Uint16(8) != 24 {
		panic("Which() != lui8")
	}
	p, err := s.Struct.Ptr(0)
	return capnp.UInt8List{List: p.List()}, err
}

func (s Value) HasLui8() bool {
	if s.Struct.Uint16(8) != 24 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) SetLui8(v capnp.UInt8List) error {
	s.Struct.SetUint16(8, 24)
	return s.Struct.SetPtr(0, v.List.ToPtr())
}

// NewLui8 sets the lui8 field to a newly
// allocated capnp.UInt8List, preferring placement in s's segment.
func (s Value) NewLui8(n int32) (capnp.UInt8List, error) {
	s.Struct.SetUint16(8, 24)
	l, err := capnp.NewUInt8List(s.Struct.Segment(), n)
	if err != nil {
		return capnp.UInt8List{}, err
	}
	err = s.Struct.SetPtr(0, l.List.ToPtr())
	return l, err
}

func (s Value) Lb() (capnp.BitList, error) {
	if s.Struct.Uint16(8) != 25 {
		panic("Which() != lb")
	}
	p, err := s.Struct.Ptr(0)
	return capnp.BitList{List: p.List()}, err
}

func (s Value) HasLb() bool {
	if s.Struct.Uint16(8) != 25 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) SetLb(v capnp.BitList) error {
	s.Struct.SetUint16(8, 25)
	return s.Struct.SetPtr(0, v.List.ToPtr())
}

// NewLb sets the lb field to a newly
// allocated capnp.BitList, preferring placement in s's segment.
func (s Value) NewLb(n int32) (capnp.BitList, error) {
	s.Struct.SetUint16(8, 25)
	l, err := capnp.NewBitList(s.Struct.Segment(), n)
	if err != nil {
		return capnp.BitList{}, err
	}
	err = s.Struct.SetPtr(0, l.List.ToPtr())
	return l, err
}

func (s Value) Lt() (capnp.TextList, error) {
	if s.Struct.Uint16(8) != 26 {
		panic("Which() != lt")
	}
	p, err := s.Struct.Ptr(0)
	return capnp.TextList{List: p.List()}, err
}

func (s Value) HasLt() bool {
	if s.Struct.Uint16(8) != 26 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) SetLt(v capnp.TextList) error {
	s.Struct.SetUint16(8, 26)
	return s.Struct.SetPtr(0, v.List.ToPtr())
}

// NewLt sets the lt field to a newly
// allocated capnp.TextList, preferring placement in s's segment.
func (s Value) NewLt(n int32) (capnp.TextList, error) {
	s.Struct.SetUint16(8, 26)
	l, err := capnp.NewTextList(s.Struct.Segment(), n)
	if err != nil {
		return capnp.TextList{}, err
	}
	err = s.Struct.SetPtr(0, l.List.ToPtr())
	return l, err
}

func (s Value) Ld() (capnp.DataList, error) {
	if s.Struct.Uint16(8) != 27 {
		panic("Which() != ld")
	}
	p, err := s.Struct.Ptr(0)
	return capnp.DataList{List: p.List()}, err
}

func (s Value) HasLd() bool {
	if s.Struct.Uint16(8) != 27 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) SetLd(v capnp.DataList) error {
	s.Struct.SetUint16(8, 27)
	return s.Struct.SetPtr(0, v.List.ToPtr())
}

// NewLd sets the ld field to a newly
// allocated capnp.DataList, preferring placement in s's segment.
func (s Value) NewLd(n int32) (capnp.DataList, error) {
	s.Struct.SetUint16(8, 27)
	l, err := capnp.NewDataList(s.Struct.Segment(), n)
	if err != nil {
		return capnp.DataList{}, err
	}
	err = s.Struct.SetPtr(0, l.List.ToPtr())
	return l, err
}

func (s Value) Lcap() (capnp.PointerList, error) {
	if s.Struct.Uint16(8) != 28 {
		panic("Which() != lcap")
	}
	p, err := s.Struct.Ptr(0)
	return capnp.PointerList{List: p.List()}, err
}

func (s Value) HasLcap() bool {
	if s.Struct.Uint16(8) != 28 {
		return false
	}
	return s.Struct.HasPtr(0)
}

func (s Value) SetLcap(v capnp.PointerList) error {
	s.Struct.SetUint16(8, 28)
	return s.Struct.SetPtr(0, v.List.ToPtr())
}

// NewLcap sets the lcap field to a newly
// allocated capnp.PointerList, preferring placement in s's segment.
func (s Value) NewLcap(n int32) (capnp.PointerList, error) {
	s.Struct.SetUint16(8, 28)
	l, err := capnp.NewPointerList(s.Struct.Segment(), n)
	if err != nil {
		return capnp.PointerList{}, err
	}
	err = s.Struct.SetPtr(0, l.List.ToPtr())
	return l, err
}

// Value_List is a list of Value.
type Value_List struct{ capnp.List }

// NewValue creates a new list of Value.
func NewValue_List(s *capnp.Segment, sz int32) (Value_List, error) {
	l, err := capnp.NewCompositeList(s, capnp.ObjectSize{DataSize: 16, PointerCount: 1}, sz)
	return Value_List{l}, err
}

func (s Value_List) At(i int) Value { return Value{s.List.Struct(i)} }

func (s Value_List) Set(i int, v Value) error { return s.List.SetStruct(i, v.Struct) }

func (s Value_List) String() string {
	str, _ := text.MarshalList(0xe17592335373b246, s.List)
	return str
}

// Value_Future is a wrapper for a Value promised by a client call.
type Value_Future struct{ *capnp.Future }

func (p Value_Future) Struct() (Value, error) {
	s, err := p.Future.Struct()
	return Value{s}, err
}

func (p Value_Future) P() *capnp.Future {
	return p.Future.Field(0, nil)
}

func (p Value_Future) Cap() *capnp.Future {
	return p.Future.Field(0, nil)
}

type Pair struct{ capnp.Struct }

// Pair_TypeID is the unique identifier for the type Pair.
const Pair_TypeID = 0xb9d4864725174733

func NewPair(s *capnp.Segment) (Pair, error) {
	st, err := capnp.NewStruct(s, capnp.ObjectSize{DataSize: 0, PointerCount: 2})
	return Pair{st}, err
}

func NewRootPair(s *capnp.Segment) (Pair, error) {
	st, err := capnp.NewRootStruct(s, capnp.ObjectSize{DataSize: 0, PointerCount: 2})
	return Pair{st}, err
}

func ReadRootPair(msg *capnp.Message) (Pair, error) {
	root, err := msg.Root()
	return Pair{root.Struct()}, err
}

func (s Pair) String() string {
	str, _ := text.Marshal(0xb9d4864725174733, s.Struct)
	return str
}

func (s Pair) Fst() (capnp.Ptr, error) {
	return s.Struct.Ptr(0)
}

func (s Pair) HasFst() bool {
	return s.Struct.HasPtr(0)
}

func (s Pair) SetFst(v capnp.Ptr) error {
	return s.Struct.SetPtr(0, v)
}

func (s Pair) Snd() (capnp.Ptr, error) {
	return s.Struct.Ptr(1)
}

func (s Pair) HasSnd() bool {
	return s.Struct.HasPtr(1)
}

func (s Pair) SetSnd(v capnp.Ptr) error {
	return s.Struct.SetPtr(1, v)
}

// Pair_List is a list of Pair.
type Pair_List struct{ capnp.List }

// NewPair creates a new list of Pair.
func NewPair_List(s *capnp.Segment, sz int32) (Pair_List, error) {
	l, err := capnp.NewCompositeList(s, capnp.ObjectSize{DataSize: 0, PointerCount: 2}, sz)
	return Pair_List{l}, err
}

func (s Pair_List) At(i int) Pair { return Pair{s.List.Struct(i)} }

func (s Pair_List) Set(i int, v Pair) error { return s.List.SetStruct(i, v.Struct) }

func (s Pair_List) String() string {
	str, _ := text.MarshalList(0xb9d4864725174733, s.List)
	return str
}

// Pair_Future is a wrapper for a Pair promised by a client call.
type Pair_Future struct{ *capnp.Future }

func (p Pair_Future) Struct() (Pair, error) {
	s, err := p.Future.Struct()
	return Pair{s}, err
}

func (p Pair_Future) Fst() *capnp.Future {
	return p.Future.Field(0, nil)
}

func (p Pair_Future) Snd() *capnp.Future {
	return p.Future.Field(1, nil)
}

const schema_99f1c9a775a88ac9 = "x\xdad\xd5_lSo\x19\x07\xf0\xef\xf7}O{" +
	"\xba\xad]w\xfa\x1e~\x831\x9c\x92-\x81\x85\x91\xad" +
	"\x9d\xcb4\x90\x8c\xb8\xc1\x96\x8c\xa4\x87\x0e\x03\x89F\xbb" +
	"\xad\xd3\x9a\xae\x9dk+\x98\xf8'&\xc6\x0b\xbc1$" +
	"&b\xe4\x82D\xa3&&\x04\xae\xf4\xc2\xc4x\x87#" +
	"1\xe8\xbc\x90\x0b\xe3?\x14\xff\xa0\xa2\xa0\xa2\xc8k\x9e" +
	"\xb7\xb4\xddopu\xce\xf7\xf3\xf2\xbc\xcf\xdes\xfa\x9c" +
	"\xc9\xb3zNM\xc5>\xeb\x01\xd1t,n\xef\xfcb" +
	"\xe9\x1d?\xbeu\xed&\x82!\x02\x9e\x0f\xe4\xc6T\x96" +
	"\xf0\xec\xcf\x16\xbfvy\xe7\xc1\xed\xbb\x08R\xda\xde\xbb" +
	"\xf6\xad\xe67\xef=\xbd\x01\xd0\xa4\xd4O\xcc!\xe5\x03" +
	"\xe6\x80:gN\xcb\x95\xcd\x9d\x1b\x1c;\xf7\x85\xdd\xef" +
	"!H\xb1\xbb8\xa6\xfc\x01\x9a1u\xddL\xb8\xf5\xc7" +
	"\xd5\x15\xc0\xdcT\x83\xf6\xa7\x0f\xf5\xab\x9d\xcf\xec\xec\xee" +
	"[\xafe\xd5\x0d\xf5\xd0|\xc3\xad\xbf\xa5n\x83\xf6\xec" +
	"\xddz!w\xbd\xf9+D)\xaa\xee\xe2\x05\xfaG\xe8" +
	"\x99\xd3\xfa\xabfA\xfe_\xee\x8c\xfeP\x1c\xb4\xf7W" +
	"\x7fx\xe7\xc2\xf3\x9d\xc7\x88\x86I\xbb\xb2\xfb\xa3\x8f|" +
	"\xb1\xa7\xf2\x04\x17\xe9S\x03\xb9\x07\xbdY\x82\xe6\xe7\xbd" +
	"R\xba\xa3Q\x8a{\xfb\xa0\xec\xfe\xf1\xbe_\x9bO\xf7" +
	"\xc9\xd5'\xfb\xae\xe0}v\xad\xb6\xb9Y\xab\x9e\\\xd3" +
	"\xc5\xad\xea\xd6{\x97\xd6K\xd5Fy\xa3\\\\\xad\x94" +
	"N\x96\xab\x1b\xb5\xd1\xfcHq\xbb\xb8Y\xef\xacS\xfb" +
	"\xd7\xb1\x94'#O\xc7\x80\xce\xb9\xb3}\x12A0\x0e" +
	"\x15\xc4\xfc\xb4\xd4\x9ac\x9e\xec\x14\xa2+\x94/\x96\xb9" +
	"-\x05\x12\xda\x03<\x02\xc1\xf1\xa3@4\xaa\x19M*" +
	"\x06dH\x09'$<\xa6\x19M+\xfa\x1b\xf5\x063" +
	"d\xf7\xf9\x00\xcc\x80~\xbd\xba\xceL\xecM8\x93`" +
	"\x10\xcb\x04\xb1\x0c\xcf\xb2\xf0\xc6_\xb2T\xdd\xa8mo" +
	"\x16\x1be]\xabJ'\xc9N'\x0bC@4\xa7\x19" +
	"-\xef\xe9di\x1c\x88\xe65\xa3\xbcb\xa0TH\x05" +
	"\x04\xe7W\x81hY3\xba\xa4\xa8\xcb\xebLB1\x09" +
	"\xa6\xab\xc5\xcdR\xfb\xc6\xae\x97\xeak\xdb\xe5\xad\x06\xfc" +
	"r\xad\xdaI\xdf~\x1e\xef/Vt\xd3\x9dhC{" +
	"Ik\xa5\x0fsD\x1f\x05\x0a\x07\xb5faT+\xa6" +
	"\xf8\xca\xba^\xcc\xbb\x1c\x0c\x0b\x1c\x13P\xff\xb3\xae\x1f" +
	"3\xe6\xe0\x9d\x02'\x04\xf4K\x1b\xca\x8bb\x8e;\x18" +
	"\x15\x98\x14\xf0\xfekCz\x80\x99ppL`Z " +
	"\xf6\x1f\x1b2\x06\x98)=\x04\x14N\x08\xcc\x0a\xc4_" +
	"\xd8\x90q\xc0\xbc[\x8f\x03\x85I\x81S\x02\xfe\xbfm" +
	"\xe8^\xb1\xf78\x98\x16\x98\x13H\xfc\xcb\x86L\x00\xe6" +
	"\xb4\x83Y\x81y\x81\x9e\x7f\xda\x90=\x809\xe36?" +
	"%\xb0(\xd0\xfb\xdc\x86\xec\x05\xcc\x82\xce\x00\x859\x81" +
	"e\x81\xbeg6d\x1f`\x96\x1c\xcc\x0b\xe4\x05\x92\xff" +
	"\xb0!\x93\x809\xef`Q`E \xf5w\x1b2\x05" +
	"\x98\xc8\xc1\xb2\xc0%\x81\xfe\xa76d?`.\xba\xcd" +
	"\xf3\x02\x1f\x10H\xff\xcd\x86L\x03\xe6\xb2kwE`" +
	"K`\xe0\xaf6\xe4\x00`6\x1d|T\xe0\xf3\x02\xc1" +
	"_l\xc8\x000\x9fs\xf0)\x81\xaf\x08d\x9e\xd8\x90" +
	"\x19\xc0|\xd9\xc1\x97\x04\xbe#`\xfelC\x1a\xc0|" +
	"\xdb\xc1\xd7\x05~ \x10\xfe\xc9\x86\x0c\x01\xf3}\xd7\xd5" +
	"w\x05v\x05\x0e\xfc\xd1\x86<\x00\x98\x07:\x0b\x14\xee" +
	"\x0b<\x16x\xeb\x0f6\xe4[\x80y\xe4\xe0\x97\x02/" +
	"\x05\x06\x1f\xdb\x90\x83\x80y\xe1\xe0\x99\xc0\x80\xa7\x98:" +
	"\xf8{\x1b\xf2 `R\x9el\x9e\xf0\xe4\xbd\x128\xf4" +
	";\x1b\xf2\x90\xbcW\x9e<\xf3a\x81Y\x81\xa1G6" +
	"\xe4\x90<s\x07\x93\x02\xcb\x02\x87\x7fkC\x1e\x96\xe7" +
	"\xe1`^\xe0\xc3\x02\xc3\xbf\xb1!\x87\x01\xf3A\xb7\xc7" +
	"%\x81\x86'\xbf\xdd\x99i\xf6A\xb1\x0f\xf47rY" +
	"\xf6B\xb1\x17\xf4\xcb3\xd3\x8cA1&\xd7\xb9,=" +
	"(zr=5C\x0dE\x0d\xea\xf2,\x15\x14\x15\x98" +
	"n\xca\xfa\x1e(\xf6\xb8\x9b\\\x96\x09(&\xdc\xcd\xd4" +
	"\x0c}(\xfa\xa0\xdf,\xcf2\x0e\xc58\xc8U\x12\x8a" +
	"\x04\xd9h\xff\xf8\xb8\xce\x14\x14S \xb7\x98\x81rS" +
	"d\xad\xb8\xc5\x8c\xa7A\xb9KW\xa4\xe1~0\xaf\xe9" +
	"\xfa\xeewY.\xdb\xcez\xdbY\xb9\xbb.\xd6\xc9\xba" +
	"\xeb\xbcN65\xd3\xcet+\xf3+\xe5\xd9v\xa4Z" +
	"\xd1H\xa5\xb9\xa7^O7\xec\x16Lt\xc3nE\xbf" +
	"\xbdK\xb3[2\xde\xcate\xb5\x9d\xb0\x9d4\xdaI" +
	"\xb2\x9d\xac\xb7\x93T\xbb\x92\x1c\xc7\xeb\xacu*\xfd{" +
	"\x86V\xeb\xabQhl7\xd7\x1a\xcd\xed\xd2\xfaJ\xe9" +
	"j\xe3d\xbd\xb1\xdd\x1cq\xf72K\x93\xb65\xac\x82" +
	"\x85\xf1\xee4m\x8d0\xb5o\x9c\xb6\xe6\x97\x96y*" +
	"\xe3~Q3ZQLWk\xd5\x12\xe2\xe9\x8f\xd5k" +
	"U\xc4\xfd\xab\x9b\x15\xc4\xf7\x0d\xf1=\x0d\xf8\xa5\xab\x8d" +
	"}\xdf\x93l\xf7{\"\xff\xba\xdf\xd2`\xe2\x02\xd4\xc8" +
	"'\x8a\x95fwH\xd7_\x97\x02K\xff\x0f\x00\x00\xff" +
	"\xff\x1fy\xa4D"

func init() {
	schemas.Register(schema_99f1c9a775a88ac9,
		0x9d8aa1cf1e49deb1,
		0xb2afd1cb599c48d5,
		0xb9d4864725174733,
		0xd4cb7ecbfe03dad3,
		0xe17592335373b246,
		0xe8cbf552b1c262cc,
		0xed6c098b67cad454)
}
