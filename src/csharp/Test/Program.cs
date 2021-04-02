using System;
using System.Linq;
using Mas.Rpc;
using Capnp.Rpc;
using System.Threading.Tasks;
using System.Threading;

namespace Mas.Infrastructure.Common
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("ThreadId: " + Thread.CurrentThread.ManagedThreadId);

            using var conMan = new ConnectionManager();

            //*
            try {
                var fs = await conMan.Connect<Rpc.Management.IFertilizerService>("capnp://localhost:13001");
                var mf = await fs.MineralFertilizerPartitionFor(Rpc.Management.MineralFertilizer.an);
                var of = await fs.OrganicFertilizerParametersFor(Rpc.Management.OrganicFertilizer.cadlm);
                Console.WriteLine(mf.Name);
                Console.WriteLine(of.Name);
            }
            catch (RpcException e) { Console.WriteLine(e.Message); }
            //*/



            //var ts = await conMan.Connect<Climate.ITimeSeries>("capnp://localhost:11002");
            //var hs = await ts.Header();
            //foreach (var h in hs) Console.WriteLine(h.ToString());

            //*
            //using var con = new TcpRpcClient("localhost", 10000);
            //await Task.WhenAll(con.WhenConnected);
            //using var ss = con.GetMain<Soil.IService>();

            /*
            var ss = await conMan.Connect<Soil.IService>("capnp://login01.cluster.zalf.de:10000");//"capnp://localhost:10000");
            //await Task.Delay(10000);
            for (int i = 0; i < 100; i++)
            {
                var (mps, ops) = await ss.GetAllAvailableParameters(false);
                Console.WriteLine("" + i + " >>>> " + mps.Select(p => p.ToString()).Aggregate((a, p) => a + "," + p));
                var profiles = await ss.ProfilesAt(new Geo.LatLonCoord { Lat = 52.0, Lon = 11.0 }, new Soil.Query
                {
                    Mandatory = new Soil.PropertyName[] { Soil.PropertyName.sand, Soil.PropertyName.organicMatter },
                    OnlyRawData = false
                });
                Console.WriteLine("layers: " + profiles[0].Layers.Count());
                //await Task.Delay(500);
            }
            //*/

            /*
            var ts = await conMan.Connect<Climate.ITimeSeries>("capnp://localhost:11002");
            for (int i = 0; i < 100; i++)
            {
                var hs = await ts.Header();
                Console.WriteLine("" + i + " >>>> " + hs.Select(e => e.ToString()).Aggregate((a, e) => a + "," + e));
                await Task.Delay(300);
            }
            //*/

            /*
            var csv = "iso-date,tavg,tmin,tmax,wind,globrad,precip,relhumid,\n, C_deg, C_deg, C_deg, m/ s,MJ m-2 d - 1,mm,%,\n1991-01-01,-0.6,-1.5,1,6.7,0.52,0,90,\n1991-01-02,2.8,0,6,12.8,0.52,0,85,\n1991-01-03,7,6,8,11.4,1.63,0,77,\n1991-01-04,5.7,4,8,8.6,0.52,0,70,\n1991-01-05,5.6,4,7,7.8,0.52,1,76,";
            var tsf = await conMan.Connect<Climate.ICSVTimeSeriesFactory>("capnp://10.10.24.86:11005");
            //using var tsf = await conMan.Connect<Climate.ICSVTimeSeriesFactory>("capnp://localhost:11005");
            //using var con = new TcpRpcClient("login01.cluster.zalf.de", 11005);
            //await Task.WhenAll(con.WhenConnected);
            //using var tsf = con.GetMain<Climate.ICSVTimeSeriesFactory>();
            var info = await tsf.Info();
            var (ts, error) = await tsf.Create(csv, new Climate.CSVTimeSeriesFactory.CSVConfig());
            if (ts == null && error.Length != 0)
            {
                Console.WriteLine(error);
            }
            else
            {
                for (int i = 0; i < 100; i++)
                {
                    var hs = await ts.Header();
                    Console.WriteLine("" + i + " >>>> " + hs.Select(e => e.ToString()).Aggregate((a, e) => a + "," + e));
                    await Task.Delay(300);
                }
            }
            //*/

            Console.WriteLine("finished");

            }
    }
}
