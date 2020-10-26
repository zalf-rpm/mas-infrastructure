using System;
using Mas.Rpc;
using Capnp.Rpc;
using Capnp.Rpc.Interception;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;
using Mas.Rpc.Registry;
using Mas.Rpc.Persistence;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using Capnp;

namespace Mas.Infrastructure.ServiceRegistry
{

    class ServiceRegistry : Mas.Rpc.Registry.IRegistry, Mas.Rpc.Persistence.IRestorer<string>
    {
        private string _id, _name, _description;
        private ConcurrentBag<Common.IdInformation> _supportedCategories;
        private ConcurrentDictionary<string, Tuple<string, Registry.Entry, Unregister>> _srToken2Entry;
        private ConcurrentDictionary<string, Proxy> _srToken2Proxy;
        private IInterceptionPolicy _savePolicy;

        public ServiceRegistry(string id = "", string name = "", string description = "")
        {
            _id = id;
            _name = name;
            _description = description;
            _supportedCategories = new ConcurrentBag<Common.IdInformation>();
            _supportedCategories.Add(new Common.IdInformation() { Id = "abc" });
            _srToken2Entry = new ConcurrentDictionary<string, Tuple<string, Registry.Entry, Unregister>>();
            _srToken2Proxy = new ConcurrentDictionary<string, Proxy>();
            _savePolicy = new InterceptPersistentPolicy(this);
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new System.Exception("No network adapters with an IPv4 address in the system!");
        }

        public string saveCapability(Proxy proxy, string fixedSrToken = null)
        {
            var srToken = fixedSrToken ?? System.Guid.NewGuid().ToString();
            _srToken2Proxy[srToken] = proxy;
            var ip = "localhost";// GetLocalIPAddress();
            return $"capnp://insecure@{ip}:{Program.TcpPort}/{srToken}";
        }

        #region implementation of Mas.Rpc.Common.IIdentifiable
        public Task<Common.IdInformation> Info(CancellationToken cancellationToken_ = default)
        {
            return Task.FromResult<Common.IdInformation>(new Common.IdInformation()
            { Id = _id, Name = _name, Description = _description });
        }
        #endregion

        #region implementation of Mas.Rpc.Registry.IRegistry
        public Task<Common.IdInformation> CategoryInfo(string categoryId, CancellationToken cancellationToken_ = default)
        {
            return Task.FromResult(_supportedCategories.Where(info => info.Id == categoryId).FirstOrDefault());
        }

        public Task<IReadOnlyList<Registry.Entry>> Entries(string categoryId, CancellationToken cancellationToken_ = default)
        {
            var entries = new List<Registry.Entry>();
            if (categoryId != null)
                entries.AddRange((from p in _srToken2Entry
                                  where p.Key == categoryId
                                  select p.Value.Item2).
                                  Select(e => new Registry.Entry(){
                                      CategoryId = e.CategoryId,
                                      Ref = Proxy.Share(e.Ref)
                                 }));
            else
                entries.AddRange((from p in _srToken2Entry select p.Value.Item2).
                    Select(e => new Registry.Entry()
                    {
                        CategoryId = e.CategoryId,
                        Ref = Proxy.Share(e.Ref)
                    }));
            return Task.FromResult<IReadOnlyList<Registry.Entry>>(entries);
        }

        public Task<IReadOnlyList<Common.IdInformation>> SupportedCategories(CancellationToken cancellationToken_ = default)
        {
            return Task.FromResult<IReadOnlyList<Common.IdInformation>>(_supportedCategories.ToList());
        }
        #endregion

        #region implementation of Mas.Rpc.Persistence.IRestorer<string> 
        public Task Drop(string srToken, SturdyRef.Owner owner, CancellationToken cancellationToken_ = default)
        {
            Proxy bp;
            _srToken2Proxy.Remove(srToken, out bp);
            return Task.CompletedTask;
        }

        public Task<BareProxy> Restore(string srToken, SturdyRef.Owner owner, CancellationToken cancellationToken_ = default)
        {
            if (_srToken2Proxy.ContainsKey(srToken))
            {
                var proxy = _srToken2Proxy[srToken];
                if (proxy is BareProxy bareProxy)
                    return Task.FromResult(Proxy.Share(bareProxy));
                else
                {
                    var sharedProxy = Proxy.Share(proxy);
                    var bareProxy2 = new BareProxy(sharedProxy.ConsumedCap);
                    return Task.FromResult(bareProxy2);// Proxy.Share(_srToken2Proxy[srToken]));
                }
            }
            return Task.FromResult<BareProxy>(null);
        }
        #endregion

        
        public void Dispose()
        {
            // dispose registered caps
            foreach (var sr2e in _srToken2Entry)
            {
                sr2e.Value.Item2.Ref?.Dispose(); // registered services
                //sr2e.Value.Item3.Dispose();
            }

            // dispose sturdy ref caps
            foreach (var sr2p in _srToken2Proxy)
                sr2p.Value?.Dispose();

            Console.WriteLine("Dispose");
        }


        public class RegistratorImpl : Mas.Rpc.Registry.IRegistrator
        {
            private ServiceRegistry _registry;

            public RegistratorImpl(ServiceRegistry reg)
            {
                _registry = reg;
            }

            #region implemenation of Mas.Rpc.Registry.IRegistrator
            public Task<Common.ICallback> Register(Common.IIdentifiable @ref, string categoryId, CancellationToken cancellationToken_ = default)
            {
                if (categoryId == null)
                    return Task.FromResult<Common.ICallback>(null);

                if (_registry._supportedCategories.Where(cat => cat.Id == categoryId).Count() > 0)
                {
                    var uuid = System.Guid.NewGuid().ToString();
                    var persistentRef = _registry._savePolicy.Attach(@ref);
                    var unreg = new Unregister(uuid, () => _registry._srToken2Entry.TryRemove(uuid, out Tuple<string, Registry.Entry, Unregister> removedValue));
                    _registry._srToken2Entry[categoryId] = Tuple.Create(uuid, new Registry.Entry()
                    {
                        CategoryId = categoryId,
                        Ref = persistentRef
                    }, unreg);
                    return Task.FromResult<Common.ICallback>(unreg);
                }
                return Task.FromResult<Common.ICallback>(null);
            }
            #endregion

            public void Dispose()
            {
                Console.WriteLine("RegistratorImpl.Dispose");
            }
        }

        class InterceptPersistentPolicy : IInterceptionPolicy
        {
            private ulong PersistentInterfaceId;
            private ServiceRegistry _Registry;

            public InterceptPersistentPolicy(ServiceRegistry registry)
            {
                _Registry = registry;
                var typeId = (Capnp.TypeIdAttribute)Attribute.GetCustomAttribute(typeof(Capnp.IPersistent<string, string>), typeof(Capnp.TypeIdAttribute));
                PersistentInterfaceId = typeId.Id;
            }

            public bool Equals([AllowNull] IInterceptionPolicy other)
            {
                return this.Equals(other);
            }

            public void OnCallFromAlice(CallContext callContext)
            {
                // is a Persistent interface
                if (callContext.InterfaceId == PersistentInterfaceId)
                {
                    var result = CapnpSerializable.Create<Capnp.Persistent<string, string>.SaveResults>(callContext.OutArgs);
                    var sturdyRef = _Registry.saveCapability(callContext.Bob as Proxy);
                    result.SturdyRef = sturdyRef;
                    var resultWriter = SerializerState.CreateForRpc<Capnp.Persistent<string, string>.SaveResults.WRITER>();
                    result.serialize(resultWriter);
                    callContext.OutArgs = resultWriter;
                    callContext.ReturnToAlice();
                } 
                else
                    callContext.ForwardToBob();
            }

            public void OnReturnFromBob(CallContext callContext)
            {
                callContext.ReturnToAlice();
            }
        }

    }

    class Unregister : Common.ICallback
    {
        private string _srToken;
        private Action _removeService;
        private bool _alreadyUnregistered = false;

        public Unregister(string srToken, Action removeService)
        {
            _srToken = srToken;
            _removeService = removeService;
        }
              
        #region implementation of Common.ICallback
        public Task Call(CancellationToken cancellationToken_ = default)
        {
            _removeService();
            _alreadyUnregistered = true;
            Console.WriteLine("Removing service in Unregister for srToken: " + _srToken);
            return Task.CompletedTask;
        }
        #endregion

        public void Dispose()
        {
            if (!_alreadyUnregistered)
            {
                _removeService();
                Console.WriteLine("Removing service in Dispose for srToken: " + _srToken);
            }
        }
    }
}
