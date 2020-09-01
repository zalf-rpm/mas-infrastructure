using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Capnp.Rpc;

namespace Mas.Rpc.Monica
{
    class SlurmMonicaInstanceFactory : Mas.Rpc.Cluster.IModelInstanceFactory
    {
        public SlurmMonicaInstanceFactory(string monicaId, string pathToMonicaCapnpServerExe, string factoryAddress, int factoryPort)
        {
            _monicaId = monicaId;
            _pathToMonicaCapnpServerExe = pathToMonicaCapnpServerExe;
            _factoryAddress = factoryAddress;
            _factoryPort = factoryPort;
        }

        struct RegEntry
        {
            public Dictionary<int, Common.ICapHolder<object>> instanceCaps;
            public List<Common.ICallback> unregisterCaps;
            public TaskCompletionSource<bool> taskFulfiller;
            public int fulfillCount;
        }

        private Dictionary<string, RegEntry> _registry;
        private RegEntry RegistryFindOrCreate(string key)
        {
            if (!_registry.ContainsKey(key))
                _registry.Add(key, new RegEntry());

            return _registry[key];
        }
        
        private string _monicaId = "MONICA vXXX";
        private string _pathToMonicaCapnpServerExe = "monica-capnp-server";
        private string _factoryAddress = "localhost";
        private int _factoryPort = 10000;
        private int _instancesStarted = 0;
        private int _instancesRegistered = 0;
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        // info @0 () -> (info :IdInformation);
        public Task<Common.IdInformation> Info(CancellationToken cancellationToken_ = default)
        {
            var info = new Common.IdInformation();
            info.Id = Guid.NewGuid().ToString();
            info.Name = $"SlurmMonicaInstanceFactory({info.Id})";
            return Task.FromResult(info);
        }

        // modelId @4 () -> (id :Text);
        public Task<string> ModelId(CancellationToken cancellationToken_ = default)
        {
            return Task.FromResult(_monicaId);
        }

        public Task<Common.ICapHolder<object>> NewCloudViaProxy(short numberOfInstances, CancellationToken cancellationToken_ = default)
        {
            throw new NotImplementedException();
        }

        public Task<Common.ICapHolder<Common.ZmqPipelineAddresses>> NewCloudViaZmqPipelineProxies(short numberOfInstances, CancellationToken cancellationToken_ = default)
        {
            throw new NotImplementedException();
        }

        // newInstance @0 () -> (instance :Common.CapHolder);
        public Task<Common.ICapHolder<object>> NewInstance(CancellationToken cancellationToken_ = default)
        {
            //string registrationToken = "aaaa";
            var registrationToken = Guid.NewGuid().ToString();
            var reg = RegistryFindOrCreate(registrationToken);
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {

            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var proc = new Process();
                var si = proc.StartInfo;
                si.FileName = _pathToMonicaCapnpServerExe;
                si.Arguments = $" -d -cf -fa {_factoryAddress} -fp {_factoryPort} -rt {registrationToken}:0";
                si.CreateNoWindow = true;
                //si.UseShellExecute = false;

                proc.Start();
                _instancesStarted++;
                Console.WriteLine($"start: {_instancesStarted}");
            }

            reg.fulfillCount++;

            reg.taskFulfiller = new TaskCompletionSource<bool>();
            var task = reg.taskFulfiller.Task;

            return task.ContinueWith((_) => reg.instanceCaps[0]);
        }

        public Task<Common.ICapHolder<IReadOnlyList<Common.ListEntry<Common.ICapHolder<object>>>>> NewInstances(short numberOfInstances, CancellationToken cancellationToken_ = default)
        {
            throw new NotImplementedException();
        }

        // registerModelInstance @5 (instance :Capability, registrationToken :Text = "") -> (unregister :Common.Callback);
        public Task<Common.ICallback> RegisterModelInstance(BareProxy instance, string registrationToken, CancellationToken cancellationToken_ = default)
        {
            var v = $"{registrationToken}:0".Split(":");
            var regToken = v[0];
            var procId = int.Parse(v[1]);

            var reg = _registry[regToken];
            _instancesRegistered++;
            Console.WriteLine($"regs: {_instancesRegistered}");

            /*
            var capHolder = new CommonImpl.CapHolderImpl<object>((object)instance, regToken, false, regToken);
            reg.instanceCaps.Add(procId, capHolder);

            var unregCap = new CommonImpl.CallbackImpl(() => _registry.Remove(regToken), true, regToken);
            reg.unregisterCaps.Add(unregCap);
            reg.fulfillCount--;
            

            if (reg.fulfillCount == 0) reg.taskFulfiller.SetResult(true);
            return Task.FromResult((Common.ICallback)unregCap);
            */
            return Task.FromResult((Common.ICallback)null);
        }


        public Task<Common.ICapHolder<object>> RestoreSturdyRef(string sturdyRef, CancellationToken cancellationToken_ = default)
        {
            var reg = _registry[sturdyRef];

            if (reg.instanceCaps.Count > 1)
                return Task.FromResult(reg.instanceCaps[-1]);
            else
                return Task.FromResult(reg.instanceCaps[0]);
        }
    }
}

