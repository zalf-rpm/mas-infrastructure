// Copyright (c) 2013-2015 Sandstorm Development Group, Inc. and contributors
// Licensed under the MIT License:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

use capnp::list_list;
use capnp::primitive_list;
use capnp::Error;

use capnp_rpc::{rpc_twoparty_capnp, twoparty, RpcSystem};

use capnp::capability::Promise;

use climate_data_capnp::climate;
use common_capnp::common;
use model_capnp::model;
use persistent_capnp::persistent;
use service_capnp::service;

use futures::{Future, Stream};
use tokio::io::AsyncRead;
use tokio::runtime::current_thread;

use chrono::prelude::*;
use chrono::Duration;
//use std::collections::HashMap;

struct Callback<T>
where
    T: Fn() -> (),
{
    callback: T,
    already_called: bool,
    exec_callback_on_del: bool,
}

impl<T> Callback<T>
where
    T: Fn() -> (),
{
    fn new(callback: T, exec_callback_on_del: bool) -> Callback<T> {
        Callback {
            callback,
            already_called: false,
            exec_callback_on_del,
        }
    }
}

impl<T> Drop for Callback<T>
where
    T: Fn() -> (),
{
    fn drop(&mut self) {
        if self.exec_callback_on_del && !self.already_called {
            (self.callback)();
        }
    }
}

impl<T> common::callback::Server for Callback<T>
where
    T: Fn() -> (),
{
    fn call(
        &mut self,
        _params: common::callback::CallParams,
        _results: common::callback::CallResults,
    ) -> Promise<(), Error> {
        (self.callback)();
        self.already_called = true;
        Promise::ok(())
    }
}

//---------------------------------------------------------------------------------------

struct CapHolder<F, C>
where
    F: Fn() -> (),
    for<'c> C: capnp::traits::Owned<'c>,
{
    cap: C,
    sturdy_ref: String,
    cleanup: F,
    already_cleaned_up: bool,
    cleanup_on_del: bool,
}

impl<F, C> CapHolder<F, C>
where
    F: Fn() -> (),
    for<'c> C: capnp::traits::Owned<'c>,
{
    fn new(cap: C, sturdy_ref: String, cleanup: F, cleanup_on_del: bool) -> CapHolder<F, C> {
        CapHolder {
            cap,
            sturdy_ref,
            cleanup,
            already_cleaned_up: false,
            cleanup_on_del,
        }
    }
}

//impl<F, C> Drop for CapHolder<F, C>
//where F: Fn() -> (),
//for<'c> C: capnp::traits::Owned<'c> {
//    fn drop(&mut self) {
//        if self.cleanup_on_del && !self.already_cleaned_up {
//            (self.cleanup)();
//        }
//    }
//}

impl<F, C> common::cap_holder::Server<C> for CapHolder<F, C>
where
    F: Fn() -> (),
    for<'c> C: capnp::traits::Owned<'c>,
{
    fn cap(
        &mut self,
        _params: common::cap_holder::CapParams<C>,
        mut results: common::cap_holder::CapResults<C>,
    ) -> Promise<(), Error> {
        match results.get().get_cap() {
            Ok(r) => ,
            Err(e) => {println!("{:?}", e); Promise::ok(());},
        }
        //results
        //    .get()
        //    .get_cap().;

        //.set_as_capability(self.cap.hook.add_ref());
        //let b = results.get().init_cap();

        //results.get().set_cap(self.cap);
        Promise::ok(())
    }

    fn free(
        &mut self,
        _params: common::cap_holder::FreeParams<C>,
        _results: common::cap_holder::FreeResults<C>,
    ) -> Promise<(), Error> {
        (self.cleanup)();
        self.already_cleaned_up = true;
        Promise::ok(())
    }
}

impl<F, C> persistent::Server<capnp::text::Owned, capnp::text::Owned> for CapHolder<F, C>
where
    F: Fn() -> (),
    for<'c> C: capnp::traits::Owned<'c>,
{
    fn save(
        &mut self,
        _params: persistent::SaveParams<capnp::text::Owned, capnp::text::Owned>,
        results: persistent::SaveResults<capnp::text::Owned, capnp::text::Owned>,
    ) -> Promise<(), Error> {
        //results.sturdy_ref = self.sturdy_ref;
        Promise::ok(())
    }
}

//-----------------------------------------------------------------------------

struct CapHolder2<F>
where
    F: Fn() -> (),
{
    cap: capnp::capability::Client,
    sturdy_ref: String,
    cleanup: F,
    already_cleaned_up: bool,
    cleanup_on_del: bool,
}

impl<F> CapHolder2<F>
where
    F: Fn() -> (),
{
    fn new(
        cap: capnp::capability::Client,
        sturdy_ref: String,
        cleanup: F,
        cleanup_on_del: bool,
    ) -> CapHolder2<F> {
        CapHolder2 {
            cap,
            sturdy_ref,
            cleanup,
            already_cleaned_up: false,
            cleanup_on_del,
        }
    }
}

impl<F> Drop for CapHolder2<F>
where
    F: Fn() -> (),
{
    fn drop(&mut self) {
        if self.cleanup_on_del && !self.already_cleaned_up {
            (self.cleanup)();
        }
    }
}

impl<F> common::cap_holder2::Server for CapHolder2<F>
where
    F: Fn() -> (),
{
    fn cap(
        &mut self,
        _params: common::cap_holder2::CapParams,
        mut results: common::cap_holder2::CapResults,
    ) -> Promise<(), Error> {
        results
            .get()
            .get_cap()
            .set_as_capability(self.cap.hook.add_ref());
        Promise::ok(())
    }

    fn free(
        &mut self,
        _params: common::cap_holder2::FreeParams,
        _results: common::cap_holder2::FreeResults,
    ) -> Promise<(), Error> {
        (self.cleanup)();
        self.already_cleaned_up = true;
        Promise::ok(())
    }
}

impl<F> persistent::Server<capnp::text::Owned, capnp::text::Owned> for CapHolder2<F>
where
    F: Fn() -> (),
{
    fn save(
        &mut self,
        _params: persistent::SaveParams<capnp::text::Owned, capnp::text::Owned>,
        results: persistent::SaveResults<capnp::text::Owned, capnp::text::Owned>,
    ) -> Promise<(), Error> {
        //results.sturdy_ref = self.sturdy_ref;
        Promise::ok(())
    }
}

//-----------------------------------------------------------------------------

struct YearlyTavg;

impl YearlyTavg {
    fn new() -> YearlyTavg {
        YearlyTavg
    }

    /*
    fn calc_yearly_tavg(
        &self,
        start_date: Date<Utc>,
        end_date: Date<Utc>,
        header: climate::time_series::header_results::Reader,//<'_>,
        data: climate::time_series::data_results::Reader,//<'_>,
    ) -> (Vec<f64>, Vec<f64>) {
        (vec![], vec![])
    }
    */
}

impl common::identifiable::Server for YearlyTavg {
    fn info(
        &mut self,
        _params: common::identifiable::InfoParams,
        mut results: common::identifiable::InfoResults,
    ) -> Promise<(), Error> {
        //results.get().set_info()
        Promise::ok(())
    }
}

impl common::cancelable::Server for YearlyTavg {
    fn cancel(
        &mut self,
        _params: common::cancelable::CancelParams,
        mut results: common::cancelable::CancelResults,
    ) -> Promise<(), Error> {
        //results.get().set_info()
        Promise::ok(())
    }
}

fn round(num: f64, digits: i32) -> f64 {
    (10_f64.powi(digits) * num).round() / 10_f64.powi(digits)
}

fn calc_yearly_tavg(
    start_date: Date<Utc>,
    end_date: Date<Utc>,
    _header: climate::time_series::header_results::Reader,
    data: list_list::Reader<primitive_list::Owned<f32>>,
) -> (Vec<f64>, Vec<f64>) {
    let mut current_year = start_date.year();
    let mut current_sum_t = 0_f64;
    let mut current_day_count = 0;
    let mut years: Vec<f64> = Vec::new();
    let mut tavgs: Vec<f64> = Vec::new();
    let no_of_days = end_date.signed_duration_since(start_date).num_days();
    print!("no_of_days: {}", no_of_days);
    let daily_tavgs = data.get(0).unwrap();
    for day in 0..no_of_days {
        let current_date = start_date + Duration::days(day);

        if current_year != current_date.year() {
            years.push(current_year as f64);
            tavgs.push(round(current_sum_t / (current_day_count as f64), 2));
            current_year = current_date.year();
            current_sum_t = 0_f64;
            current_day_count = 0;
        }

        current_sum_t += daily_tavgs.get(day as u32) as f64;
        current_day_count += 1;
    }

    (years, tavgs)
}

impl model::climate_instance::Server for YearlyTavg {
    fn run_set(
        &mut self,
        params: model::climate_instance::RunSetParams,
        mut result: model::climate_instance::RunSetResults,
    ) -> Promise<(), Error> {
        //::capnp::capability::Promise::err(::capnp::Error::unimplemented("method not implemented".to_string()))
        Promise::ok(())
    }

    fn run(
        &mut self,
        params: model::climate_instance::RunParams,
        mut result: model::climate_instance::RunResults,
    ) -> Promise<(), Error> {
        let ts = pry!(pry!(params.get()).get_time_series());

        Promise::from_future(
            ts.header_request()
                .send()
                .promise
                .join3(
                    ts.data_t_request().send().promise,
                    ts.range_request().send().promise,
                )
                .and_then(move |hdr| {
                    let (h, d, r) = hdr;
                    let rr = r.get().unwrap();
                    let sd = rr.get_start_date().unwrap();
                    let ed = rr.get_end_date().unwrap();

                    //let (xs, ys) = self.calc_yearly_tavg(
                    let (xs, ys) = calc_yearly_tavg(
                        Utc.ymd(
                            sd.get_year().into(),
                            sd.get_month().into(),
                            sd.get_day().into(),
                        ),
                        Utc.ymd(
                            ed.get_year().into(),
                            ed.get_month().into(),
                            ed.get_day().into(),
                        ),
                        h.get().unwrap(),
                        d.get().unwrap().get_data().unwrap(),
                    );
                    let mut xy_result_b = result.get().init_result();
                    {
                        let mut xsb = xy_result_b.reborrow().init_xs(xs.len() as u32);
                        for i in 0..xs.len() {
                            xsb.set(i as u32, xs[i]);
                        }
                    }
                    {
                        let mut ysb = xy_result_b.reborrow().init_ys(ys.len() as u32);
                        for i in 0..xs.len() {
                            ysb.set(i as u32, ys[i]);
                        }
                    }

                    Promise::ok(())
                }),
        )
    }
}

struct DataServiceImpl;

impl DataServiceImpl {
    fn new() -> DataServiceImpl {
        DataServiceImpl
    }
}

impl service::climate_service::Server for DataServiceImpl {
    fn simulations(
        &mut self,
        _params: service::climate_service::SimulationsParams,
        mut _results: service::climate_service::SimulationsResults,
    ) -> Promise<(), Error> {
        Promise::ok(())
    }

    fn models(
        &mut self,
        _params: service::climate_service::ModelsParams,
        mut results: service::climate_service::ModelsResults,
    ) -> Promise<(), Error> {
        let mut models = results.get().init_models(1);
        models.set(
            0,
            model::climate_instance::ToClient::new(YearlyTavg)
                .into_client::<::capnp_rpc::Server>()
                .client
                .hook,
        );

        Promise::ok(())
    }
}

pub fn main() {
    use std::net::ToSocketAddrs;
    let args: Vec<String> = ::std::env::args().collect();
    if args.len() != 3 {
        println!("usage: {} server ADDRESS[:PORT]", args[0]);
        return;
    }

    let addr = args[2]
        .to_socket_addrs()
        .unwrap()
        .next()
        .expect("could not parse address");
    let socket = ::tokio::net::TcpListener::bind(&addr).unwrap();

    let data_service = service::climate_service::ToClient::new(DataServiceImpl)
        .into_client::<::capnp_rpc::Server>();

    let done = socket.incoming().for_each(move |socket| {
        socket.set_nodelay(true)?;
        let (reader, writer) = socket.split();

        let network = twoparty::VatNetwork::new(
            reader,
            std::io::BufWriter::new(writer),
            rpc_twoparty_capnp::Side::Server,
            Default::default(),
        );

        let rpc_system = RpcSystem::new(Box::new(network), Some(data_service.clone().client));
        current_thread::spawn(rpc_system.map_err(|e| println!("error: {:?}", e)));
        Ok(())
    });

    current_thread::block_on_all(done).unwrap();
}
