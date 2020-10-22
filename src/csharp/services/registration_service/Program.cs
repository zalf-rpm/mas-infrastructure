using System;
using Capnp.Rpc;
using System.Net;

namespace Mas.Infrastructure.ServiceRegistry
{
    class Program
    {
        public static int TcpPort = 10001;
        private static ServiceRegistry.RegistratorImpl Registrator;

        static void Main(string[] args)
        {
            using (var server = new TcpRpcServer())
            {
                server.AddBuffering();
                var bootstrap = new ServiceRegistry("test-id", "test-name");
                server.Main = bootstrap;
                Registrator = new ServiceRegistry.RegistratorImpl(bootstrap);
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
