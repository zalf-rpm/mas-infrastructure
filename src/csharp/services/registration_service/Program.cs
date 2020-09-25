using System;
using Capnp.Rpc;
using System.Net;

namespace Mas.Infrastructure.ServiceRegistry
{
    class Program
    {
        public static int TcpPort = 10001;

        static void Main(string[] args)
        {
            using (var server = new TcpRpcServer())
            {
                server.AddBuffering();
                server.Main = new ServiceRegistryImpl("test-id", "test-name");
                server.StartAccepting(IPAddress.Any, TcpPort);

                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                }

                Console.WriteLine("Press RETURN to stop listening");
                Console.ReadLine();
            }
        }
           
    }
}
