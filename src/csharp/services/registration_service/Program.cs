using System;
using Capnp.Rpc;
using Capnp.Rpc.Interception;
using System.Net;
using Mas.Infrastructure;

namespace Mas.Infrastructure.ServiceRegistry
{
    class Program
    {
        public static int TcpPort = 10001;

        static void Main(string[] args)
        {
            using (var conMan = new Common.ConnectionManager())
            {
                var bootstrap = new ServiceRegistry("test-id", "test-name");
                conMan.Bind(IPAddress.Any, TcpPort, bootstrap);
                var registrator = new ServiceRegistry.RegistratorImpl(bootstrap);
                var regSturdyRef = bootstrap.SaveCapability(BareProxy.FromImpl(registrator), "abcd");
                Console.WriteLine($"SturdyRef to Registrator interface: [{regSturdyRef}]");
                var admin = new ServiceRegistry.AdminImpl(bootstrap);
                var adminSturdyRef = bootstrap.SaveCapability(BareProxy.FromImpl(admin));
                Console.WriteLine($"SturdyRef to Admin interface: [{adminSturdyRef}]");

                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                }

                Console.WriteLine("Press RETURN to stop listening");
                Console.ReadLine();
            }

            /*
            using (var server = new TcpRpcServer())
            {
                server.AddBuffering();
                var bootstrap = new ServiceRegistry("test-id", "test-name");
                server.Main = bootstrap;
                var registrator = new ServiceRegistry.RegistratorImpl(bootstrap);
                var regSturdyRef = bootstrap.SaveCapability(BareProxy.FromImpl(registrator));//, "abcd");
                Console.WriteLine($"SturdyRef to Registrator interface: [{regSturdyRef}]");
                var admin = new ServiceRegistry.AdminImpl(bootstrap);
                var adminSturdyRef = bootstrap.SaveCapability(BareProxy.FromImpl(admin));
                Console.WriteLine($"SturdyRef to Admin interface: [{adminSturdyRef}]");
                server.StartAccepting(IPAddress.Any, TcpPort);

                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                }

                Console.WriteLine("Press RETURN to stop listening");
                Console.ReadLine();
            }
            */
        }
           
    }
}
