using System;
using Mas.Rpc;
using System.Threading.Tasks;

namespace Mas.Infrastructure.Common
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var conMan = new ConnectionManager();
            var ts = await conMan.Connect<Climate.ITimeSeries>("capnp://localhost:11002");

            var hs = await ts.Header();
            foreach (var h in hs) Console.WriteLine(h.ToString());
            
        }
    }
}
