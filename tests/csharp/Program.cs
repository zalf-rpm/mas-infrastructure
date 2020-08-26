using System;
using Capnp.Rpc;
using Mas.Rpc;
using System.Threading.Tasks;
using System.Net;
using System.Linq;

namespace test_mas_infrastructure
{
    class Program
    {
        public static int TcpPort = 9000;

        static async Task Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "client")
            {
                /*
                using (TcpRpcClient client = new TcpRpcClient("localhost", 11111))
                {
                    await Task.WhenAll(client.WhenConnected);

                    var service = client.GetMain<Mas.Rpc.IA>();

                    var res = await service.Method();

                    Console.WriteLine(res);

                    return;
                }
                //*/

                using (TcpRpcClient soilServiceClient = new TcpRpcClient("localhost", 6003))
                {
                    await Task.WhenAll(soilServiceClient.WhenConnected);

                    var service = soilServiceClient.GetMain<Mas.Rpc.Soil.IService>();
                    
                    var allParams = await service.GetAllAvailableParameters(true);
                    Console.WriteLine("soilService.getAllAvailableParameters(true) -> ");
                    Console.WriteLine("mandatory:");
                    foreach (var m in allParams.Item1)
                    {
                        Console.WriteLine(m.which);
                    }
                    Console.WriteLine("optional:");
                    foreach (var o in allParams.Item2)
                    {
                        Console.WriteLine(o.which);
                    }
                    Console.WriteLine("---------------------------------");

                    
                    var profiles = await service.ProfilesAt(
                        new Geo.LatLonCoord() { Lat = 53.0, Lon = 12.5 }, 
                        new Soil.Query()
                        {
                            Mandatory = new Soil.Parameter[]
                        {
                            new Soil.Parameter() { which = Soil.Parameter.WHICH.Sand },
                            new Soil.Parameter() { which = Soil.Parameter.WHICH.Clay },
                            new Soil.Parameter() { which = Soil.Parameter.WHICH.BulkDensity },
                            new Soil.Parameter() { which = Soil.Parameter.WHICH.OrganicCarbon }
                        },
                            Optional = new Soil.Parameter[]
                        {
                            new Soil.Parameter() { which = Soil.Parameter.WHICH.PH }
                        },
                            OnlyRawData = false
                        }
                    );
                    Console.WriteLine("soilService.profileAt(...) -> ");
                    Console.WriteLine("profiles[0]:");
                    foreach (var profile in profiles)
                    {
                        Console.WriteLine("percentageOfArea: " + profile.PercentageOfArea);
                        foreach (var layer in profile.Layers)
                        {
                            Console.WriteLine(layer.Params);
                            foreach (var param in layer.Params)
                            {
                                Console.WriteLine(param.which.ToString());
                            }
                        }
                    }
                    Console.WriteLine("---------------------------------");
                    return;

                    var query = new Soil.Query()
                    {
                        Mandatory = new Soil.Parameter[]
                        {
                            new Soil.Parameter() { which = Soil.Parameter.WHICH.Sand },
                            new Soil.Parameter() { which = Soil.Parameter.WHICH.Clay },
                            new Soil.Parameter() { which = Soil.Parameter.WHICH.BulkDensity },
                            new Soil.Parameter() { which = Soil.Parameter.WHICH.OrganicCarbon }
                        },
                        Optional = new Soil.Parameter[]
                        {
                            new Soil.Parameter() { which = Soil.Parameter.WHICH.PH }
                        },
                        OnlyRawData = false
                    };
                    var locs = await service.AllLocations(query);
                    var latlonAndCap = locs[0];
                    var capList = latlonAndCap.Snd;
                    var p = await capList[0].Cap();

                    //var data = await ts.Data();
                    //int x = 0;
                    //foreach (var d in data)
                    //{
                    //x += d.Count;
                    //}
                    //Console.WriteLine("data.Count=" + data.Count + " should be: " + data.Count * 7 + " is: " + x);
                    Console.WriteLine("\n");
                }


                /*
                using (TcpRpcClient csvTimeSeriesClient = new TcpRpcClient("login01.cluster.zalf.de", 11002))
                //using (TcpRpcClient csvTimeSeriesClient = new TcpRpcClient("localhost", 11002))
                //using (TcpRpcClient csvTimeSeriesClient = new TcpRpcClient("10.10.24.206", 11002))
                {
                    await Task.WhenAll(csvTimeSeriesClient.WhenConnected);

                    var ts = csvTimeSeriesClient.GetMain<Mas.Rpc.ClimateData.IListTests>();

                    var testLI32_2 = await ts.TestLI32();
                    var testLI32_2_Count = testLI32_2.Count;
                    var testLI32_2_Sum = testLI32_2.Sum();


                    var receivedBytes = 0;
                    for (var i = 1; i < 10000; i++)
                    {
                        var testLI32 = await ts.TestLI32();
                        var testLI32_Count = testLI32.Count;
                        receivedBytes += testLI32_Count * 4;
                        var testLI32_Sum_Soll = testLI32_Count * 1;
                        var testLI32_Sum = testLI32.Sum();
                        Console.WriteLine("Count: " + testLI32_Count 
                            + " ok?: " + (testLI32_Sum == testLI32_Sum_Soll)
                            + " received bytes so far: " + receivedBytes);
                    }
                    //var testLI32_2 = await ts.TestLI32();
                    //var testLI32_2_Count = testLI32_2.Count;
                    //var testLI32_2_Sum = testLI32_2.Sum();
                    var testLI32_3 = await ts.TestLI32();
                    var testLI32_3_Count = testLI32_3.Count;
                    var testLI32_3_Sum = testLI32_3.Sum();
                    //var testLLI32 = await ts.TestLLI32();
                    //int testLLI32_Count = 0;
                    //int testLLI32_Soll = testLLI32.Count * 7;
                    //foreach (var d in testLLI32)
                    //{
                    //    testLLI32_Count += d.Count;
                    //}

                    var testLF32 = await ts.TestLF32();
                    var testLF32_Count = testLF32.Count;
                    var testLLF32 = await ts.TestLLF32();
                    int testLLF32_Count = 0;
                    int testLLF32_Soll = testLLF32.Count * 7;
                    foreach (var d in testLLF32)
                    {
                        testLLF32_Count += d.Count;
                    }

                    //var testLI32_2 = await ts.TestLI32();
                    //var testLI32_Count_2 = testLI32_2.Count;

                    //Console.WriteLine("data.Count=" + data.Count + " should be: " + data.Count * 7 + " is: " + x);
                    Console.WriteLine("\n");
                }
                //*/


                /*
                using (TcpRpcClient csvTimeSeriesClient = new TcpRpcClient("login01.cluster.zalf.de", 11000))
                //using (TcpRpcClient csvTimeSeriesClient = new TcpRpcClient("localhost", 11002))
                {
                    await Task.WhenAll(csvTimeSeriesClient.WhenConnected);

                    var ts = csvTimeSeriesClient.GetMain<Mas.Rpc.ClimateData.ITimeSeries>();
                    var data = await ts.Data();
                    int x = 0;
                    foreach(var d in data)
                    {
                        x += d.Count;
                    }
                    Console.WriteLine("data.Count=" + data.Count + " should be: " + data.Count*7 + " is: " + x);
                    Console.WriteLine("\n");
                }
                //*/

                /*
                using (TcpRpcClient climateDataServiceClient = new TcpRpcClient("login01.cluster.zalf.de", 11001))
                {
                    await Task.WhenAll(climateDataServiceClient.WhenConnected);

                    var service = climateDataServiceClient.GetMain<Mas.Rpc.ClimateData.IService>();
                    var sims = await service.GetAvailableSimulations();
                    var sim = sims[0];
                    var scens = await sim.Scenarios();
                    var scen = scens[0];
                    var reals = await scen.Realizations();
                    var real = reals[0];
                    var coord = new Geo.Coord();
                    coord.Latlon = new Geo.LatLonCoord();
                    coord.Latlon.Lat = 46.51412;
                    coord.Latlon.Lon = 12.81895;
                    var tss = await real.ClosestTimeSeriesAt(coord);
                    var ts = tss[0];
                    var data = await ts.Data();
                    Console.WriteLine(data.Count);
                }
                //*/

                /*
                using (TcpRpcClient csvTimeSeriesClient = new TcpRpcClient("localhost", 11000),
                    adminMasterClient = new TcpRpcClient("10.10.24.186", 8000))
                {
                    await Task.WhenAll(csvTimeSeriesClient.WhenConnected, adminMasterClient.WhenConnected);

                    //csv_time_series = capnp.TwoPartyClient("localhost:11000").bootstrap().cast_as(
                    //climate_data_capnp.Climate.TimeSeries)
                    var adminMaster = adminMasterClient.GetMain<Cluster.IAdminMaster>();
                    var factories = await adminMaster.AvailableModels();
                    if (factories.Count > 0)
                    {
                        var factory = factories[0];
                        var instanceCapHolder = factory.NewInstance().Eager<Mas.Rpc.Common.ICapHolder<object>>();
                        var envInstance = ((BareProxy)(await instanceCapHolder.Cap())).Cast<Mas.Rpc.Model.IEnvInstance>(true);
                        //var envInstance = ((BareProxy)(await instanceCapHolder.Cap())).Cast<Mas.Rpc.Model.IEnvInstance>(true);
                        var infos = await envInstance.Info();
                        Console.WriteLine(infos.ToString());
                        //modelRes.PseudoEager<Mas.Rpc.Model.IEnvInstance>();
                        //var x = modelRes.PseudoEager();

                        Console.WriteLine("Hello World!");

                        auto monica = modelRes.getCap().getAs<rpc::Model::EnvInstance>();
                        //auto monica = cap.getAs<rpc::Model::EnvInstance>();
                        //auto monica = capHolder.capRequest().send().wait(waitScope).getCap(); //<rpc::Model::EnvInstance>().;
                        //auto monicaId = monica.infoRequest().send().wait(waitScope).getInfo().getId();
                        monica.infoRequest().send().then([](auto && res) {
                            cout << "monicaId: " << res.getInfo().getId().cStr() << endl;
                        }).wait(waitScope);
                        //auto monicaId = monicaRes.getInfo().getId().cStr();
                        //cout << "monicaId: " << monicaId << endl;
                        //auto sturdyRef = capHolder.saveRequest().send().wait(waitScope).getSturdyRef();
                        instanceCapHolder.saveRequest().send().then([](auto && res) {
                            cout << "sturdyRef: " << res.getSturdyRef().cStr() << endl;
                        }).wait(waitScope);
                        //auto sturdyRefP = instanceCapHolder.saveRequest().send().wait(waitScope);// .getSturdyRef();

                    }

            var csvTimeSeries = csvTimeSeriesClient.GetMain<Climate.ITimeSeries>();
            var header = csvTimeSeries.Header();
            header.Wait();
            var elems = header.Result;
            foreach(var elem in elems)
            {
                Console.WriteLine(elem);
            }
            //csvTimeSeries.WhenResolved.Wait(3000);

            //SpinWait.SpinUntil(() => server.ConnectionCount > 0, MediumTimeout);
            //Assert.AreEqual(1, server.ConnectionCount);

            //server.Main = new ProvidedCapabilityMock();
            //var main = client.GetMain<BareProxy>();
            //var resolving = main as IResolvingCapability;
            //Assert.IsTrue(resolving.WhenResolved.Wait(MediumTimeout));
        }
        //*/
            }
            else if(args.Length > 0 && args[0] == "server")
            {
                /*
                using (TcpRpcClient runtimeClient = new TcpRpcClient("10.10.24.186", 9000),
                    adminMasterClient = new TcpRpcClient("10.10.24.186", 8000))

                //using (client)
                {
                    await Task.WhenAll(csvTimeSeriesClient.WhenConnected, adminMasterClient.WhenConnected);

                    //csv_time_series = capnp.TwoPartyClient("localhost:11000").bootstrap().cast_as(
                    //climate_data_capnp.Climate.TimeSeries)
                    var adminMaster = adminMasterClient.GetMain<Cluster.IAdminMaster>();
                    var factories = await adminMaster.AvailableModels();
                    if (factories.Count > 0)


}
                        using (var server = new TcpRpcServer(IPAddress.Any, TcpPort))
                {
                    Console.WriteLine($"starting server at localhost:{TcpPort}");
                    server.Main = new Mas.Rpc.Monica.SlurmMonicaInstanceFactory("MONICA v3", "C:\\Users}\berg.ZALF-AD\\GitHub\\monica\\_cmake_vs2019_win64\\Debug\\monica-capnp-server.exe", "localhost", 10000);
                    Console.WriteLine($"after");
                }
                */

            }
            Console.WriteLine("Hello World!");
        }
    }
}
