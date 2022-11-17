using System;
using Capnp.Rpc;
using Capnp.Rpc.Interception;
using System.Net;
using Mas.Infrastructure;
using System.Text.Json;
using Mas;
using System.Threading.Tasks;
using Crypt = NSec.Cryptography;

namespace Mas.Infrastructure.ServiceRegistry
{
    class Program
    {
        class RegData {
            public string reg_sr { get; set; }
            public string reg_name { get; set; }
            public string cat_id { get; set; }
            public Mas.Schema.Common.IAction unreg { get; set; }
            public Mas.Schema.Persistence.SturdyRef reregSR { get; set; }
        }

        struct Reg {
            public RegData registry { get; set; }
            //public RegData registrar { get; set; }
            //public RegData registry_admin { get; set; }
            //public RegData service_admin { get; set; }
        }

        static Reg DeserializeRegs(string regsJsonStr)
        {
            return JsonSerializer.Deserialize<Reg>(regsJsonStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        static async Task TryRegisterService(Common.ConnectionManager conMan, RegData r, Mas.Schema.Common.IIdentifiable service) {
            if (r != null)
            {
                try {
                    var remReg = await conMan.Connect<Mas.Schema.Registry.IRegistrar>(r.reg_sr);
                    var regParams = new Mas.Schema.Registry.Registrar.RegParams { 
                        Cap = service, RegName = r.reg_name, CategoryId = r.cat_id };
                    var res = await remReg.Register(regParams);
                    r.unreg = res.Item1;
                    r.reregSR = res.Item2;
                }
                catch(System.Exception e){ 
                    Console.WriteLine($"Exception trying to register registry SR. Exception {e}"); 
                }
            }
        }

        static async Task Main(string[] args)
        {
            var id = System.Guid.NewGuid().ToString();
            var name = id;
            var desc = "";
            Reg regs = new();
            var catsFilePath = "categories.json";
            var tcpPort = 0;
            var readRegSRsFromStdIn = false;
            string regFilePath = "regs.json"; //null;

            for (var i = 0; i < args.Length; i++)
            {
                try
                {
                    if (args[i].StartsWith("id")) id = args[i].Split('=')[1];
                    else if (args[i].StartsWith("name")) name = args[i].Split('=')[1];
                    else if (args[i].StartsWith("desc")) desc = args[i].Split('=')[1];
                    else if (args[i].StartsWith("cats")) catsFilePath = args[i].Split('=')[1];
                    else if (args[i].StartsWith("port")) tcpPort = int.Parse(args[i].Split('=')[1]);
                    else if (args[i].StartsWith("regstdin")) readRegSRsFromStdIn = true;
                    else if (args[i].StartsWith("regfile")) regFilePath = args[i].Split('=')[1];
                }
                catch (System.Exception) { }
            }

            if (readRegSRsFromStdIn && Console.IsInputRedirected)
            {
                var regsJson = new System.Text.StringBuilder();
                while(true)
                {
                    var s = Console.ReadLine();
                    if (s == null) break;
                    else regsJson.Append(s);
                }

                regs = DeserializeRegs(regsJson.ToString());
            }
            else if (regFilePath != null)
            {
                var regsJson = System.IO.File.ReadAllText(regFilePath);
                regs = DeserializeRegs(regsJson.ToString());
            }

            var restorer = new Common.Restorer();
            using var conMan = new Common.ConnectionManager(restorer);
            var registry = new ServiceRegistry ()
            {
                Restorer = restorer,
                CategoriesFilePath = catsFilePath,
                Id = id,
                Name = name,
                Description = desc
            };
            conMan.Bind(IPAddress.Any, tcpPort, restorer);
            restorer.TcpPort = conMan.Port;
            
            Console.WriteLine("Started ServiceRegistry with these Categories:");
            foreach (var cat in registry.Categories) Console.WriteLine(cat.Id);
            var registrySturdyRef = restorer.SaveStr(BareProxy.FromImpl(registry)).Item1;
            Console.WriteLine($"registry_sr: {registrySturdyRef}");
            //await TryRegisterService(conMan, regs.registry, registry);

            var registrar = new ServiceRegistry.Registrar(registry, restorer);
            var regSturdyRef = restorer.SaveStr(BareProxy.FromImpl(registrar)).Item1;
            Console.WriteLine($"registrar_sr: {regSturdyRef}");
            //await TryRegisterService(conMan, regs.registrar, registrar);

            var registryAdmin = new ServiceRegistry.Admin(registry);
            var registryAdminSturdyRef = restorer.SaveStr(BareProxy.FromImpl(registryAdmin)).Item1;
            Console.WriteLine($"registry_admin_sr: {registryAdminSturdyRef}");
            //await TryRegisterService(conMan, regs.registry_admin, registryAdmin);

            var serviceAdmin = new Service.Admin(registry, (info) => {
                registry.Id = info.Id;
                registry.Name = info.Name;
                registry.Description = info.Description;
            });
            var serviceAdminSturdyRef = restorer.SaveStr(BareProxy.FromImpl(serviceAdmin)).Item1;
            Console.WriteLine($"service_admin_sr: {serviceAdminSturdyRef}");
            //await TryRegisterService(conMan, regs.service_admin, serviceAdmin);            

            while (true) System.Threading.Thread.Sleep(1000); 

            //Console.WriteLine("Press RETURN to stop listening");
            //Console.ReadLine();
        }
    }
}
