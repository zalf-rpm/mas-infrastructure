using System;
using Capnp.Rpc;
using Capnp.Rpc.Interception;
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
                var bootstrap = new ServiceRegistry("test-id", "test-name");
                server.Main = bootstrap;
                var registrator = new ServiceRegistry.RegistratorImpl(bootstrap);
                var sturdyRef = bootstrap.saveCapability(BareProxy.FromImpl(registrator), "abcd");
                Console.WriteLine($"SturdyRef to Registrator: [{sturdyRef}]");
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
