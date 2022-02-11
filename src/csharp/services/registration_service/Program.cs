using System;
using Capnp.Rpc;
using Capnp.Rpc.Interception;
using System.Net;
using Mas.Infrastructure;
using System.Text.Json;

namespace Mas.Infrastructure.ServiceRegistry
{
    class Program
    {
        static void Main(string[] args)
        {
            var id = System.Guid.NewGuid().ToString();
            var name = id;
            var desc = "";
            var cats = new Rpc.Common.IdInformation[0];
            var tcpPort = 10001;

            static Rpc.Common.IdInformation[] deserializeCats(string catsJsonStr)
            {
                return JsonSerializer.Deserialize<Rpc.Common.IdInformation[]>(
                    catsJsonStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }

            for (var i = 0; i < args.Length; i++)
            {
                try
                {
                    if (args[i].StartsWith("id")) id = args[i].Split('=')[1];
                    else if (args[i].StartsWith("name")) name = args[i].Split('=')[1];
                    else if (args[i].StartsWith("desc")) desc = args[i].Split('=')[1];
                    else if (args[i].StartsWith("cats")) cats = deserializeCats(args[i].Split('=')[1]);
                    else if (args[i].StartsWith("port")) tcpPort = int.Parse(args[i].Split('=')[1]);
                }
                catch (System.Exception) { }
            }

            if (Console.IsInputRedirected)
            {
                var catsJson = new System.Text.StringBuilder();
                while(true)
                {
                    var s = Console.ReadLine();
                    if (s == null) break;
                    else catsJson.Append(s);
                }

                cats = deserializeCats(catsJson.ToString());
            }

            using var conMan = new Common.ConnectionManager();
            var bootstrap = new ServiceRegistry
            {
                TcpPort = tcpPort,
                Categories = cats,
                Id = id,
                Name = name,
                Description = desc
            };
            Console.WriteLine("Started ServiceRegistry with these Categories:");
            foreach (var cat in bootstrap.Categories) Console.WriteLine(cat.Id);
            
            conMan.Bind(IPAddress.Any, tcpPort, bootstrap);
            var registrator = new ServiceRegistry.RegistrarImpl(bootstrap);
            var regSturdyRef = bootstrap.SaveCapability(BareProxy.FromImpl(registrator));//, "abcd");
            Console.WriteLine($"SturdyRef to Registrator interface: [{regSturdyRef}]");
            var admin = new ServiceRegistry.AdminImpl(bootstrap);
            var adminSturdyRef = bootstrap.SaveCapability(BareProxy.FromImpl(admin));
            Console.WriteLine($"SturdyRef to Admin interface: [{adminSturdyRef}]");
            var bootstrapSturdyRef = bootstrap.SaveCapability(BareProxy.FromImpl(bootstrap));
            Console.WriteLine($"SturdyRef to Registry interface: [{bootstrapSturdyRef}]");

            while (true) System.Threading.Thread.Sleep(1000); 

            //Console.WriteLine("Press RETURN to stop listening");
            //Console.ReadLine();
        }
    }
}
