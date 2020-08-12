package main

import (
	"crypto/sha1"
	"fmt"
	"hash"
	"net"

	"github.com/zalf-rpm/climate_data_capnp_access/climate_data"
	"github.com/zalf-rpm/climate_data_capnp_access/hashes"
	"github.com/zalf-rpm/data_services"
	"golang.org/x/net/context"
	capnp "zombiezen.com/go/capnproto2"
	"zombiezen.com/go/capnproto2/rpc"
)

type dataServiceImpl struct{}

func (dsi dataServiceImpl) Models(call climate_data.Climate_DataService_models) error {
	l, err := call.Results.NewModels(1)
	if err != nil {
		return err
	}
	yt := climate_data.Climate_Model_ServerToClient(yearlyTavg{})
	return l.Set(0, yt.ToPtr())
}

type yearlyTavg struct{}

func (yt yearlyTavg) Info(call climate_data.Climate_Identifiable_info) error {
	i, err := call.Results.Info()
	if err != nil {
		return err
	}
	i.SetId("yearlytavg")
	i.SetName("YearlyTavg")
	i.SetDescription("yearly Tavg")

	return err
}

func (yt yearlyTavg) Run(call climate_data.Climate_Model_run) error {
	ts := call.Params.TimeSeries()

	ctx := context.Background()

	r, err := ts.Range(ctx, func(p climate_data.Climate_TimeSeries_range_Params) error {
		return nil
	}).Struct()
	if err != nil {
		return err
	}

	hs, err := ts.Header(ctx, func(p climate_data.Climate_TimeSeries_header_Params) error {
		return nil
	}).Struct()
	if err != nil {
		return err
	}

	ds, err := ts.Data(ctx, func(p climate_data.Climate_TimeSeries_data_Params) error {
		return nil
	}).Struct()
	if err != nil {
		return err
	}

	sd, err := r.StartDate()
	ed, err := r.EndDate()
	hs2, err := hs.Header()
	ds2, err := ds.Data()
	return calcYearlyTavg(sd, ed, hs2, ds2)

}

func (yt yearlyTavg) RunSet(call climate_data.Climate_Model_runSet) error {
	return nil
}

func calcYearlyTavg(startDate data_services.Date, endDate data_services.Date, headers climate_data.Climate_Element_List, data capnp.PointerList) error {

	return nil
	//start_date = create_date(start_date)
	//end_date = create_date(end_date)

	//current_year = start_date.year
	//current_sum_t = 0
	//current_day_count = 0
	//years = []
	//tavgs = []
	//for day in range((end_date - start_date).days + 1):

	//		current_date = start_date + timedelta(days=day)

	//		if current_year != current_date.year:
	//				years.append(current_year)
	//				tavgs.append(round(current_sum_t / current_day_count, 2))
	//				current_year = current_date.year
	//				current_sum_t = 0
	//				current_day_count = 0

	//		current_sum_t += data[day][0]
	//		current_day_count += 1

	//return {"xs": years, "ys": tavgs}
}

// hashFactory is a local implementation of HashFactory.
type hashFactory struct{}

func (hf hashFactory) NewSha1(call hashes.HashFactory_newSha1) error {
	// Create a new locally implemented Hash capability.
	hs := hashes.Hash_ServerToClient(hashServer{sha1.New()})
	// Notice that methods can return other interfaces.
	return call.Results.SetHash(hs)
}

// hashServer is a local implementation of Hash.
type hashServer struct {
	h hash.Hash
}

func (hs hashServer) Write(call hashes.Hash_write) error {
	data, err := call.Params.Data()
	if err != nil {
		return err
	}
	_, err = hs.h.Write(data)
	if err != nil {
		return err
	}
	return nil
}

func (hs hashServer) Sum(call hashes.Hash_sum) error {
	s := hs.h.Sum(nil)
	return call.Results.SetHash(s)
}

func server(c net.Conn) error {
	// Create a new locally implemented HashFactory.
	main := hashes.HashFactory_ServerToClient(hashFactory{})
	// Listen for calls, using the HashFactory as the bootstrap interface.
	conn := rpc.NewConn(rpc.StreamTransport(c), rpc.MainInterface(main.Client))
	// Wait for connection to abort.
	err := conn.Wait()
	return err
}

func client(ctx context.Context, c net.Conn) error {
	// Create a connection that we can use to get the HashFactory.
	conn := rpc.NewConn(rpc.StreamTransport(c))
	defer conn.Close()
	// Get the "bootstrap" interface.  This is the capability set with
	// rpc.MainInterface on the remote side.
	hf := hashes.HashFactory{Client: conn.Bootstrap(ctx)}

	// Now we can call methods on hf, and they will be sent over c.
	s := hf.NewSha1(ctx, func(p hashes.HashFactory_newSha1_Params) error {
		return nil
	}).Hash()
	// s refers to a remote Hash.  Method calls are delivered in order.
	s.Write(ctx, func(p hashes.Hash_write_Params) error {
		err := p.SetData([]byte("Hello, "))
		return err
	})
	s.Write(ctx, func(p hashes.Hash_write_Params) error {
		err := p.SetData([]byte("World!"))
		return err
	})
	// Get the sum, waiting for the result.
	result, err := s.Sum(ctx, func(p hashes.Hash_sum_Params) error {
		return nil
	}).Struct()
	if err != nil {
		return err
	}

	// Display the result.
	sha1Val, err := result.Hash()
	if err != nil {
		return err
	}
	fmt.Printf("sha1: %x\n", sha1Val)
	return nil
}

func main() {
	c1, c2 := net.Pipe()
	go server(c1)
	client(context.Background(), c2)
}
