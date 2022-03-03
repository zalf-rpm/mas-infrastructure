using System;
using Capnp.Rpc;
using Capnp.Rpc.Interception;
using System.Net;
using Mas.Infrastructure;
using System.Text.Json;
using Mas;

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

            if (!Console.IsInputRedirected)
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
            var restorer = new Common.Restorer() { TcpPort = tcpPort };
            conMan.Bind(IPAddress.Any, tcpPort, restorer);
            
            var registry = new ServiceRegistry
            {
                Restorer = restorer,
                Categories = cats,
                Id = id,
                Name = name,
                Description = desc
            };
            Console.WriteLine("Started ServiceRegistry with these Categories:");
            foreach (var cat in registry.Categories) Console.WriteLine(cat.Id);
            var registrySturdyRef = restorer.Save(BareProxy.FromImpl(registry)).SturdyRef;
            Console.WriteLine($"registry_sr: {registrySturdyRef}");
            
            var registrar = new ServiceRegistry.Registrar(registry, restorer);
            var regSturdyRef = restorer.Save(BareProxy.FromImpl(registrar)).SturdyRef;
            Console.WriteLine($"registrar_sr: {regSturdyRef}, unsave_sr: ");

            var registryAdmin = new ServiceRegistry.Admin(registry);
            var registryAdminSturdyRef = restorer.Save(BareProxy.FromImpl(registryAdmin)).SturdyRef;
            Console.WriteLine($"registry_admin_sr: {registryAdminSturdyRef}");
            
            var serviceAdmin = new Service.Admin(registry, (info) => {
                registry.Id = info.Id;
                registry.Name = info.Name;
                registry.Description = info.Description;
            });
            var serviceAdminSturdyRef = restorer.Save(BareProxy.FromImpl(serviceAdmin)).SturdyRef;
            Console.WriteLine($"service_admin_sr: {serviceAdminSturdyRef}");

            while (true) System.Threading.Thread.Sleep(1000); 

            //Console.WriteLine("Press RETURN to stop listening");
            //Console.ReadLine();
        }
    }
}
