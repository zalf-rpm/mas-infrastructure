// Code generated by capnpc-go. DO NOT EDIT.

package yieldstat

import (
	capnp "capnproto.org/go/capnp/v3"
	text "capnproto.org/go/capnp/v3/encoding/text"
	schemas "capnproto.org/go/capnp/v3/schemas"
	crop "github.com/zalf-rpm/mas-infrastructure/capnp_schemas/gen/go/crop"
	math "math"
)

type ResultId uint16

// ResultId_TypeID is the unique identifier for the type ResultId.
const ResultId_TypeID = 0xcfe218c48d227e0d

// Values of ResultId.
const (
	ResultId_primaryYield               ResultId = 0
	ResultId_dryMatter                  ResultId = 1
	ResultId_carbonInAboveGroundBiomass ResultId = 2
	ResultId_sumFertilizer              ResultId = 3
	ResultId_sumIrrigation              ResultId = 4
	ResultId_primaryYieldCU             ResultId = 5
)

// String returns the enum's constant name.
func (c ResultId) String() string {
	switch c {
	case ResultId_primaryYield:
		return "primaryYield"
	case ResultId_dryMatter:
		return "dryMatter"
	case ResultId_carbonInAboveGroundBiomass:
		return "carbonInAboveGroundBiomass"
	case ResultId_sumFertilizer:
		return "sumFertilizer"
	case ResultId_sumIrrigation:
		return "sumIrrigation"
	case ResultId_primaryYieldCU:
		return "primaryYieldCU"

	default:
		return ""
	}
}

// ResultIdFromString returns the enum value with a name,
// or the zero value if there's no such value.
func ResultIdFromString(c string) ResultId {
	switch c {
	case "primaryYield":
		return ResultId_primaryYield
	case "dryMatter":
		return ResultId_dryMatter
	case "carbonInAboveGroundBiomass":
		return ResultId_carbonInAboveGroundBiomass
	case "sumFertilizer":
		return ResultId_sumFertilizer
	case "sumIrrigation":
		return ResultId_sumIrrigation
	case "primaryYieldCU":
		return ResultId_primaryYieldCU

	default:
		return 0
	}
}

type ResultId_List struct{ capnp.List }

func NewResultId_List(s *capnp.Segment, sz int32) (ResultId_List, error) {
	l, err := capnp.NewUInt16List(s, sz)
	return ResultId_List{l.List}, err
}

func (l ResultId_List) At(i int) ResultId {
	ul := capnp.UInt16List{List: l.List}
	return ResultId(ul.At(i))
}

func (l ResultId_List) Set(i int, v ResultId) {
	ul := capnp.UInt16List{List: l.List}
	ul.Set(i, uint16(v))
}

type RestInput struct{ capnp.Struct }

// RestInput_TypeID is the unique identifier for the type RestInput.
const RestInput_TypeID = 0xa47f8d65869200af

func NewRestInput(s *capnp.Segment) (RestInput, error) {
	st, err := capnp.NewStruct(s, capnp.ObjectSize{DataSize: 24, PointerCount: 0})
	return RestInput{st}, err
}

func NewRootRestInput(s *capnp.Segment) (RestInput, error) {
	st, err := capnp.NewRootStruct(s, capnp.ObjectSize{DataSize: 24, PointerCount: 0})
	return RestInput{st}, err
}

func ReadRootRestInput(msg *capnp.Message) (RestInput, error) {
	root, err := msg.Root()
	return RestInput{root.Struct()}, err
}

func (s RestInput) String() string {
	str, _ := text.Marshal(0xa47f8d65869200af, s.Struct)
	return str
}

func (s RestInput) UseDevTrend() bool {
	return s.Struct.Bit(0)
}

func (s RestInput) SetUseDevTrend(v bool) {
	s.Struct.SetBit(0, v)
}

func (s RestInput) UseCO2Increase() bool {
	return !s.Struct.Bit(1)
}

func (s RestInput) SetUseCO2Increase(v bool) {
	s.Struct.SetBit(1, !v)
}

func (s RestInput) Dgm() float64 {
	return math.Float64frombits(s.Struct.Uint64(8))
}

func (s RestInput) SetDgm(v float64) {
	s.Struct.SetUint64(8, math.Float64bits(v))
}

func (s RestInput) Hft() uint8 {
	return s.Struct.Uint8(1)
}

func (s RestInput) SetHft(v uint8) {
	s.Struct.SetUint8(1, v)
}

func (s RestInput) Nft() uint8 {
	return s.Struct.Uint8(2)
}

func (s RestInput) SetNft(v uint8) {
	s.Struct.SetUint8(2, v)
}

func (s RestInput) Sft() uint8 {
	return s.Struct.Uint8(3)
}

func (s RestInput) SetSft(v uint8) {
	s.Struct.SetUint8(3, v)
}

func (s RestInput) Slope() uint8 {
	return s.Struct.Uint8(4)
}

func (s RestInput) SetSlope(v uint8) {
	s.Struct.SetUint8(4, v)
}

func (s RestInput) Steino() uint8 {
	return s.Struct.Uint8(5)
}

func (s RestInput) SetSteino(v uint8) {
	s.Struct.SetUint8(5, v)
}

func (s RestInput) Az() uint8 {
	return s.Struct.Uint8(6)
}

func (s RestInput) SetAz(v uint8) {
	s.Struct.SetUint8(6, v)
}

func (s RestInput) Klz() uint8 {
	return s.Struct.Uint8(7)
}

func (s RestInput) SetKlz(v uint8) {
	s.Struct.SetUint8(7, v)
}

func (s RestInput) Stt() uint8 {
	return s.Struct.Uint8(16)
}

func (s RestInput) SetStt(v uint8) {
	s.Struct.SetUint8(16, v)
}

func (s RestInput) GermanFederalStates() int8 {
	return int8(s.Struct.Uint8(17) ^ 255)
}

func (s RestInput) SetGermanFederalStates(v int8) {
	s.Struct.SetUint8(17, uint8(v)^255)
}

func (s RestInput) GetDryYearWaterNeed() bool {
	return s.Struct.Bit(2)
}

func (s RestInput) SetGetDryYearWaterNeed(v bool) {
	s.Struct.SetBit(2, v)
}

// RestInput_List is a list of RestInput.
type RestInput_List struct{ capnp.List }

// NewRestInput creates a new list of RestInput.
func NewRestInput_List(s *capnp.Segment, sz int32) (RestInput_List, error) {
	l, err := capnp.NewCompositeList(s, capnp.ObjectSize{DataSize: 24, PointerCount: 0}, sz)
	return RestInput_List{l}, err
}

func (s RestInput_List) At(i int) RestInput { return RestInput{s.List.Struct(i)} }

func (s RestInput_List) Set(i int, v RestInput) error { return s.List.SetStruct(i, v.Struct) }

func (s RestInput_List) String() string {
	str, _ := text.MarshalList(0xa47f8d65869200af, s.List)
	return str
}

// RestInput_Future is a wrapper for a RestInput promised by a client call.
type RestInput_Future struct{ *capnp.Future }

func (p RestInput_Future) Struct() (RestInput, error) {
	s, err := p.Future.Struct()
	return RestInput{s}, err
}

type Result struct{ capnp.Struct }

// Result_TypeID is the unique identifier for the type Result.
const Result_TypeID = 0x8db55634a0e7d054

func NewResult(s *capnp.Segment) (Result, error) {
	st, err := capnp.NewStruct(s, capnp.ObjectSize{DataSize: 8, PointerCount: 1})
	return Result{st}, err
}

func NewRootResult(s *capnp.Segment) (Result, error) {
	st, err := capnp.NewRootStruct(s, capnp.ObjectSize{DataSize: 8, PointerCount: 1})
	return Result{st}, err
}

func ReadRootResult(msg *capnp.Message) (Result, error) {
	root, err := msg.Root()
	return Result{root.Struct()}, err
}

func (s Result) String() string {
	str, _ := text.Marshal(0x8db55634a0e7d054, s.Struct)
	return str
}

func (s Result) Cultivar() crop.Cultivar {
	return crop.Cultivar(s.Struct.Uint16(0))
}

func (s Result) SetCultivar(v crop.Cultivar) {
	s.Struct.SetUint16(0, uint16(v))
}

func (s Result) IsNoData() bool {
	return s.Struct.Bit(16)
}

func (s Result) SetIsNoData(v bool) {
	s.Struct.SetBit(16, v)
}

func (s Result) Values() (Result_ResultToValue_List, error) {
	p, err := s.Struct.Ptr(0)
	return Result_ResultToValue_List{List: p.List()}, err
}

func (s Result) HasValues() bool {
	return s.Struct.HasPtr(0)
}

func (s Result) SetValues(v Result_ResultToValue_List) error {
	return s.Struct.SetPtr(0, v.List.ToPtr())
}

// NewValues sets the values field to a newly
// allocated Result_ResultToValue_List, preferring placement in s's segment.
func (s Result) NewValues(n int32) (Result_ResultToValue_List, error) {
	l, err := NewResult_ResultToValue_List(s.Struct.Segment(), n)
	if err != nil {
		return Result_ResultToValue_List{}, err
	}
	err = s.Struct.SetPtr(0, l.List.ToPtr())
	return l, err
}

// Result_List is a list of Result.
type Result_List struct{ capnp.List }

// NewResult creates a new list of Result.
func NewResult_List(s *capnp.Segment, sz int32) (Result_List, error) {
	l, err := capnp.NewCompositeList(s, capnp.ObjectSize{DataSize: 8, PointerCount: 1}, sz)
	return Result_List{l}, err
}

func (s Result_List) At(i int) Result { return Result{s.List.Struct(i)} }

func (s Result_List) Set(i int, v Result) error { return s.List.SetStruct(i, v.Struct) }

func (s Result_List) String() string {
	str, _ := text.MarshalList(0x8db55634a0e7d054, s.List)
	return str
}

// Result_Future is a wrapper for a Result promised by a client call.
type Result_Future struct{ *capnp.Future }

func (p Result_Future) Struct() (Result, error) {
	s, err := p.Future.Struct()
	return Result{s}, err
}

type Result_ResultToValue struct{ capnp.Struct }

// Result_ResultToValue_TypeID is the unique identifier for the type Result_ResultToValue.
const Result_ResultToValue_TypeID = 0x8d365bd4f0136fc0

func NewResult_ResultToValue(s *capnp.Segment) (Result_ResultToValue, error) {
	st, err := capnp.NewStruct(s, capnp.ObjectSize{DataSize: 16, PointerCount: 0})
	return Result_ResultToValue{st}, err
}

func NewRootResult_ResultToValue(s *capnp.Segment) (Result_ResultToValue, error) {
	st, err := capnp.NewRootStruct(s, capnp.ObjectSize{DataSize: 16, PointerCount: 0})
	return Result_ResultToValue{st}, err
}

func ReadRootResult_ResultToValue(msg *capnp.Message) (Result_ResultToValue, error) {
	root, err := msg.Root()
	return Result_ResultToValue{root.Struct()}, err
}

func (s Result_ResultToValue) String() string {
	str, _ := text.Marshal(0x8d365bd4f0136fc0, s.Struct)
	return str
}

func (s Result_ResultToValue) Id() ResultId {
	return ResultId(s.Struct.Uint16(0))
}

func (s Result_ResultToValue) SetId(v ResultId) {
	s.Struct.SetUint16(0, uint16(v))
}

func (s Result_ResultToValue) Value() float64 {
	return math.Float64frombits(s.Struct.Uint64(8))
}

func (s Result_ResultToValue) SetValue(v float64) {
	s.Struct.SetUint64(8, math.Float64bits(v))
}

// Result_ResultToValue_List is a list of Result_ResultToValue.
type Result_ResultToValue_List struct{ capnp.List }

// NewResult_ResultToValue creates a new list of Result_ResultToValue.
func NewResult_ResultToValue_List(s *capnp.Segment, sz int32) (Result_ResultToValue_List, error) {
	l, err := capnp.NewCompositeList(s, capnp.ObjectSize{DataSize: 16, PointerCount: 0}, sz)
	return Result_ResultToValue_List{l}, err
}

func (s Result_ResultToValue_List) At(i int) Result_ResultToValue {
	return Result_ResultToValue{s.List.Struct(i)}
}

func (s Result_ResultToValue_List) Set(i int, v Result_ResultToValue) error {
	return s.List.SetStruct(i, v.Struct)
}

func (s Result_ResultToValue_List) String() string {
	str, _ := text.MarshalList(0x8d365bd4f0136fc0, s.List)
	return str
}

// Result_ResultToValue_Future is a wrapper for a Result_ResultToValue promised by a client call.
type Result_ResultToValue_Future struct{ *capnp.Future }

func (p Result_ResultToValue_Future) Struct() (Result_ResultToValue, error) {
	s, err := p.Future.Struct()
	return Result_ResultToValue{s}, err
}

type Output struct{ capnp.Struct }

// Output_TypeID is the unique identifier for the type Output.
const Output_TypeID = 0x932a681f81b4be19

func NewOutput(s *capnp.Segment) (Output, error) {
	st, err := capnp.NewStruct(s, capnp.ObjectSize{DataSize: 8, PointerCount: 3})
	return Output{st}, err
}

func NewRootOutput(s *capnp.Segment) (Output, error) {
	st, err := capnp.NewRootStruct(s, capnp.ObjectSize{DataSize: 8, PointerCount: 3})
	return Output{st}, err
}

func ReadRootOutput(msg *capnp.Message) (Output, error) {
	root, err := msg.Root()
	return Output{root.Struct()}, err
}

func (s Output) String() string {
	str, _ := text.Marshal(0x932a681f81b4be19, s.Struct)
	return str
}

func (s Output) Id() (string, error) {
	p, err := s.Struct.Ptr(0)
	return p.Text(), err
}

func (s Output) HasId() bool {
	return s.Struct.HasPtr(0)
}

func (s Output) IdBytes() ([]byte, error) {
	p, err := s.Struct.Ptr(0)
	return p.TextBytes(), err
}

func (s Output) SetId(v string) error {
	return s.Struct.SetText(0, v)
}

func (s Output) RunFailed() bool {
	return s.Struct.Bit(0)
}

func (s Output) SetRunFailed(v bool) {
	s.Struct.SetBit(0, v)
}

func (s Output) Reason() (string, error) {
	p, err := s.Struct.Ptr(1)
	return p.Text(), err
}

func (s Output) HasReason() bool {
	return s.Struct.HasPtr(1)
}

func (s Output) ReasonBytes() ([]byte, error) {
	p, err := s.Struct.Ptr(1)
	return p.TextBytes(), err
}

func (s Output) SetReason(v string) error {
	return s.Struct.SetText(1, v)
}

func (s Output) Results() (Output_YearToResult_List, error) {
	p, err := s.Struct.Ptr(2)
	return Output_YearToResult_List{List: p.List()}, err
}

func (s Output) HasResults() bool {
	return s.Struct.HasPtr(2)
}

func (s Output) SetResults(v Output_YearToResult_List) error {
	return s.Struct.SetPtr(2, v.List.ToPtr())
}

// NewResults sets the results field to a newly
// allocated Output_YearToResult_List, preferring placement in s's segment.
func (s Output) NewResults(n int32) (Output_YearToResult_List, error) {
	l, err := NewOutput_YearToResult_List(s.Struct.Segment(), n)
	if err != nil {
		return Output_YearToResult_List{}, err
	}
	err = s.Struct.SetPtr(2, l.List.ToPtr())
	return l, err
}

// Output_List is a list of Output.
type Output_List struct{ capnp.List }

// NewOutput creates a new list of Output.
func NewOutput_List(s *capnp.Segment, sz int32) (Output_List, error) {
	l, err := capnp.NewCompositeList(s, capnp.ObjectSize{DataSize: 8, PointerCount: 3}, sz)
	return Output_List{l}, err
}

func (s Output_List) At(i int) Output { return Output{s.List.Struct(i)} }

func (s Output_List) Set(i int, v Output) error { return s.List.SetStruct(i, v.Struct) }

func (s Output_List) String() string {
	str, _ := text.MarshalList(0x932a681f81b4be19, s.List)
	return str
}

// Output_Future is a wrapper for a Output promised by a client call.
type Output_Future struct{ *capnp.Future }

func (p Output_Future) Struct() (Output, error) {
	s, err := p.Future.Struct()
	return Output{s}, err
}

type Output_YearToResult struct{ capnp.Struct }

// Output_YearToResult_TypeID is the unique identifier for the type Output_YearToResult.
const Output_YearToResult_TypeID = 0xa008c533888c3a5e

func NewOutput_YearToResult(s *capnp.Segment) (Output_YearToResult, error) {
	st, err := capnp.NewStruct(s, capnp.ObjectSize{DataSize: 8, PointerCount: 1})
	return Output_YearToResult{st}, err
}

func NewRootOutput_YearToResult(s *capnp.Segment) (Output_YearToResult, error) {
	st, err := capnp.NewRootStruct(s, capnp.ObjectSize{DataSize: 8, PointerCount: 1})
	return Output_YearToResult{st}, err
}

func ReadRootOutput_YearToResult(msg *capnp.Message) (Output_YearToResult, error) {
	root, err := msg.Root()
	return Output_YearToResult{root.Struct()}, err
}

func (s Output_YearToResult) String() string {
	str, _ := text.Marshal(0xa008c533888c3a5e, s.Struct)
	return str
}

func (s Output_YearToResult) Year() int16 {
	return int16(s.Struct.Uint16(0))
}

func (s Output_YearToResult) SetYear(v int16) {
	s.Struct.SetUint16(0, uint16(v))
}

func (s Output_YearToResult) Result() (Result, error) {
	p, err := s.Struct.Ptr(0)
	return Result{Struct: p.Struct()}, err
}

func (s Output_YearToResult) HasResult() bool {
	return s.Struct.HasPtr(0)
}

func (s Output_YearToResult) SetResult(v Result) error {
	return s.Struct.SetPtr(0, v.Struct.ToPtr())
}

// NewResult sets the result field to a newly
// allocated Result struct, preferring placement in s's segment.
func (s Output_YearToResult) NewResult() (Result, error) {
	ss, err := NewResult(s.Struct.Segment())
	if err != nil {
		return Result{}, err
	}
	err = s.Struct.SetPtr(0, ss.Struct.ToPtr())
	return ss, err
}

// Output_YearToResult_List is a list of Output_YearToResult.
type Output_YearToResult_List struct{ capnp.List }

// NewOutput_YearToResult creates a new list of Output_YearToResult.
func NewOutput_YearToResult_List(s *capnp.Segment, sz int32) (Output_YearToResult_List, error) {
	l, err := capnp.NewCompositeList(s, capnp.ObjectSize{DataSize: 8, PointerCount: 1}, sz)
	return Output_YearToResult_List{l}, err
}

func (s Output_YearToResult_List) At(i int) Output_YearToResult {
	return Output_YearToResult{s.List.Struct(i)}
}

func (s Output_YearToResult_List) Set(i int, v Output_YearToResult) error {
	return s.List.SetStruct(i, v.Struct)
}

func (s Output_YearToResult_List) String() string {
	str, _ := text.MarshalList(0xa008c533888c3a5e, s.List)
	return str
}

// Output_YearToResult_Future is a wrapper for a Output_YearToResult promised by a client call.
type Output_YearToResult_Future struct{ *capnp.Future }

func (p Output_YearToResult_Future) Struct() (Output_YearToResult, error) {
	s, err := p.Future.Struct()
	return Output_YearToResult{s}, err
}

func (p Output_YearToResult_Future) Result() Result_Future {
	return Result_Future{Future: p.Future.Field(0, nil)}
}

const schema_b80c8fd14e523f9b = "x\xda\x8c\x94\xdf\x8b\x15e\x18\xc7\x9f\xef\xfb\xce\xec9" +
	"\xba[\xbb\xc3L\xa4\xde\x1c\x94.T\xf0Gj\x91\x0b" +
	"\xb2\xb9n\xc6F\xea\xbe\xe7\xac\xd6\x96\x05\xe3\xce\x9bg" +
	"l\xce\xcc\xf1\x9d9\xabg\xb1T\xb0\xd8\xc0CYB" +
	"\x17!,Q\x17F`\x84AWQP\xd7%\x04A" +
	" \x817\xde\xf6\x0f\xc4\xc43\xc7sF\xbcq\xaf\xce" +
	"\xfb~\x9e\xe7}\xf8~\xdf\xf9\xbegw\"^\x14\xcf" +
	"\xda\x7fYD\xea\x05{$\xff9q\xff\xfd\xf3\xcd\xe7" +
	"{\xa4\xb6B\xe4\xf3\x7f\xdc_\xddw\xe2\x87\x1eY\x15" +
	"\xa2\xbdJ|\x0f7\x14\x15\"W\x8bs\x84\xb2\xac6" +
	"\x03\xf9\x17S\xf5\xa3w>\x1e\xfb\x91lp\xf7/b" +
	"\x1a\xee\x1d\xf14\x91{W\xdc\"\xe4\x1b\x7f\xba}\xb9" +
	"\xd6\xdc\xfe\xd9\xa3\xdd\x92\xbb{r\x1a\xee\x0d\xc9\xdd7" +
	"\xe5}B\xfe\xf6\xe4\xd5\x95\xbd\xbfUWY\xc8Cg" +
	"\xfb\xb3\xafY\xdf\xc0\xbd\xc9\xa2\xdc\xaf\xads\x84\xffn" +
	"}\xfa\xa1\xee]\xfcJm\x86,'\x17\xa2a\xbf\x01" +
	"\xf7)\x9b\x97\x8e\xfd\x8f \xe4O\xbc\xbf\xa5\xf7\xeb\x86" +
	"{\xbf\x93\xb3Y\x94\xcd\x84\xbd\x1b\xabu\xb8;\xaa<" +
	"u[\xf5sZ\xc8[I\xa0\xa3tW\xd7\x0eu\x14" +
	"\xa4\x99\x9f\xed\xea\x0eV;\x17\xfdv\xdc\x9e\xac\xeb\xb4" +
	"\x13e;\xfb?\xf3I\xed\x84\x1fu\xf4\x1c\xa0\xaa\xd2" +
	"\"\xb2@\xe4l\xdbD\xa4\x9e\x91P\xbb\x05\x1c\xc0\x03" +
	"\xc3\x1d{\x88\xd4V\x09\xb5O@\x86\x01\xc6KU\x04" +
	"\x8c\x13jK<\x09\xa3$0J\x18*\xb1\x1e\xa3\x84" +
	"\x94\x05\x94\x1f\xd1\x81\xc9\x07\xda\xa8/N\x8d\x0d\x95\xbd" +
	"\xf4\x0a\x91\x9a\x91Ps\xacl\xc2\x03\x00\xe7\x08\xc3W" +
	"%\xd4\xeb\x02\x10\x1e\x04\x91s|\x92H\xcdI\xa8H" +
	" _\xecDY\xb8\xe4\x1b\"\xc2x~\xfb^P\xad" +
	"\x7fw\xf9\xee\x03\xd9y\x98\x1eMf\xfc\xcc\xe7*H" +
	"\x00\x84\xa9\xc2J\x8a'\x09s\x12\x98(\xe5\x11\x18\xae" +
	"\xc1\xdc\xb1N\xd6\xee<07\x0c\x86\x833\xf9\x82\xf6" +
	"\xcd|R\xd74\xce&\xd5\xc4\xd0\x9b\xcf\xb7~RB" +
	"5\x05\x8aK\x07\x1c]'R\x81\x84j\x0b8\x02}" +
	"o-\xf6\xd6\x94P\x99\x80#\x85\x07I\xe4\x9c\x9d&" +
	"R\x91\x84Z\xe9\x7f\x9e1\x12\x18#\xe4\xa6\x13\x1f\xf6" +
	"\xc3H\x13\x82\xa1;\xa3\xfd4\x89\x07-\x17Mq\xdd" +
	"\x0f\xb9\x1d\xea}\xc4\xad\xfd\x18\xb7;\x07\xde\x0ak\x8f" +
	"dj{\x99\xa9a\xa4&\xcbH\x8dw\xb5o I" +
	"@\x16\x0ay\x02&\xca\xf7J\xc0\xc4ZS\x95\xcd\xc6" +
	"m\xd9)\x04\xec\xeb\x0b\x00\xdc\xb7p\x8a\xa8q\x12\x12" +
	"\x8d&\x06\xb9\x06\\\x8de\xa2F\xc0\xbc\x8d\xc1-\x03" +
	"n\x0b[\x88\x1aM\xe6\x19s\x09\x0f\x12p\xcf\x16<" +
	"b~\x9e\xb9%<X\x80\xdb)x\x9b\xf9\x05\xe6\xb6" +
	"\xf4`\x03n\xb7\xe0\x19\xf3K\xccG,\x0f#\x80\xfb" +
	"\x1e\xf6\x105\xce3\xbf\xc2\xbcb{\xa8\x00\xeeeL" +
	"\x125.0_a^\x1d\xf1P\x05\xdc\x0f\xb0\x89\xa8" +
	"q\x89\xf9U\xe6\xeb*\x1e\xd6\x01\xeeG\xc5\xfc+\xcc" +
	"?a\xbe~\xc2\xc3z\xc0\xed\x15|\x85\xf9u\xe6\xa3" +
	"\x8e\x87Q\xc0\xbd\x86/\x89\x1a\xd7\x99\xaf2\x1f\x13\x1e" +
	"\xc6\x00\xf7F\xc1W\x99\x7f\x0b\x81\xbc\x93\xea\x19\xbd4" +
	"o\xa8\xa2\xe3at\x98\x1e:\xb6g6\xa6\xa9EN" +
	"\x91\xe6\x82\xcd\x95Jp\xba5x\xfb\x95\xe6;\x19F" +
	"H`\x84P\x89\x1fZ\xa7\xe5\xba\x96FI[\x0fv" +
	"Si\xa6\xc38\x19l\xa5\xbf<<\xf3nT\xae\xd3" +
	"lx>?\xadM\xcb\x8f\x0fk\x04\xda\xf8Q#\xf3" +
	"+\x99N!H\xd8\"/\xca\xd9\x8c\xe9.h\xf8\xe6" +
	"5?\xd3\xe6hE\xeb\xd2\xc6Z\xff\x9cf\x11p\x8a" +
	"6\x14\xef\xee\xe0\x19N\xa1s\xa0N\x04\xe1\xec\xff\x9b" +
	"\x08\xd29`\x88`9\xfb\xf9\xc7v\x9e[&\xca\xdb" +
	"&l\xf9\xa6\xbb@\xe3<3\x0fL\xf7\x88\x9fe\x9a" +
	"`\xf2E\xdf\x9cJ\xe2\xd9X\x1c<\x95,\xe9\x97M" +
	"\xd2\x89\x83\xe90i\xf92M\xf3\xb4\xd3:\xacM\x16" +
	"R-\x0a\x97\xb5\xe1\xfd\xac1\xe1i\xaa\xf9Y\x98\xc4" +
	"\xe5\xdc)\x9e{\xe8\xf8\xff\x01\x00\x00\xff\xff_3\xa8" +
	"\xe5"

func init() {
	schemas.Register(schema_b80c8fd14e523f9b,
		0x8d365bd4f0136fc0,
		0x8db55634a0e7d054,
		0x932a681f81b4be19,
		0xa008c533888c3a5e,
		0xa47f8d65869200af,
		0xcfe218c48d227e0d)
}