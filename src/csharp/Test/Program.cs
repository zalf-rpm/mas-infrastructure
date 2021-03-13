using System;
using System.Linq;
using Mas.Rpc;
using Capnp.Rpc;
using System.Threading.Tasks;

namespace Mas.Infrastructure.Common
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var conMan = new ConnectionManager();
            //var ts = await conMan.Connect<Climate.ITimeSeries>("capnp://localhost:11002");
            //var hs = await ts.Header();
            //foreach (var h in hs) Console.WriteLine(h.ToString());

            //*
            //using var con = new TcpRpcClient("localhost", 10000);
            //await Task.WhenAll(con.WhenConnected);
            //using var ss = con.GetMain<Soil.IService>();

            var ss = await conMan.Connect<Soil.IService>("capnp://localhost:10000");
            for(int i = 0; i < 100; i++)
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

            Console.WriteLine("finished");
            
        }
    }
}
