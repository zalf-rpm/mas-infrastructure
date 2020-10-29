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
        private ConcurrentDictionary<string, Tuple<string, Registry.Entry, Unregister>> _OId2Entry;
        private ConcurrentDictionary<string, Proxy> _SrToken2Capability;
        private IInterceptionPolicy _savePolicy;

        public ServiceRegistry(string id = "", string name = "", string description = "")
        {
            _id = id;
            _name = name;
            _description = description;
            _supportedCategories = new ConcurrentBag<Common.IdInformation>();
            _supportedCategories.Add(new Common.IdInformation() { Id = "abc" });
            _OId2Entry = new ConcurrentDictionary<string, Tuple<string, Registry.Entry, Unregister>>();
            _SrToken2Capability = new ConcurrentDictionary<string, Proxy>();
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
            _SrToken2Capability[srToken] = proxy;
            var ip = Dns.GetHostName(); // "localhost";// GetLocalIPAddress();
            return $"capnp://insecure@{ip}:{Program.TcpPort}/{srToken}";
        }

        #region implementation of Mas.Rpc.Common.IIdentifiable
        public Task<Common.IdInformation> Info(CancellationToken cancellationToken_ = default)
        {
            return Task.FromResult(new Common.IdInformation()
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
                entries.AddRange((from p in _OId2Entry
                                  where p.Value.Item2.CategoryId == categoryId
                                  select p.Value.Item2).
                                  Select(e => new Registry.Entry(){
                                      CategoryId = e.CategoryId,
                                      Ref = Proxy.Share(e.Ref)
                                 }));
            else
                entries.AddRange((from p in _OId2Entry select p.Value.Item2).
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
            _SrToken2Capability.Remove(srToken, out bp);
            return Task.CompletedTask;
        }

        public Task<BareProxy> Restore(string srToken, SturdyRef.Owner owner, CancellationToken cancellationToken_ = default)
        {
            if (_SrToken2Capability.ContainsKey(srToken))
            {
                var proxy = _SrToken2Capability[srToken];
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
            foreach (var oid2e in _OId2Entry)
            {
                oid2e.Value.Item2.Ref?.Dispose(); // registered services
                oid2e.Value.Item3.Dispose();
            }

            // dispose sturdy ref caps
            foreach (var sr2p in _SrToken2Capability)
                sr2p.Value?.Dispose();

            Console.WriteLine("Dispose");
        }

        public class AdminImpl : Mas.Rpc.Registry.IAdmin
        {
            private ServiceRegistry _Registry;

            public AdminImpl(ServiceRegistry registry)
            {
                _Registry = registry;
            }

            public Task<bool> AddCategory(Common.IdInformation category, bool upsert, CancellationToken cancellationToken_ = default)
            {
                if (_Registry._supportedCategories.Where(info => category.Id == info.Id).Count() == 0 || upsert)
                {
                    _Registry._supportedCategories.Add(category);
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }

            public void Dispose() {}

            public Task<IReadOnlyList<string>> MoveObjects(IReadOnlyList<string> objectIds, string toCatId, CancellationToken cancellationToken_ = default)
            {
                // if there is no category to move to, do rather nothing
                // another option would be to remove the objects, but this is what removeObjects is for
                if (toCatId == null)
                    return Task.FromResult<IReadOnlyList<string>>(new List<string>());

                var moved = new List<string>();
                foreach(var oid in objectIds)
                {
                    try
                    {
                        _Registry._OId2Entry[oid].Item2.CategoryId = toCatId;
                        moved.Add(oid);
                    }
                    catch (KeyNotFoundException){}
                }
                return Task.FromResult<IReadOnlyList<string>>(moved);
            }

            public Task<IRegistry> Registry(CancellationToken cancellationToken_ = default)
            {
                return Task.FromResult<IRegistry>(_Registry);
            }

            public Task<IReadOnlyList<Common.IIdentifiable>> RemoveCategory(string categoryId, string moveObjectsToCategoryId, CancellationToken cancellationToken_ = default)
            {
                var removed = new List<Common.IIdentifiable>();
                // no category means nothing to remove
                if (categoryId == null)
                    return Task.FromResult<IReadOnlyList<Common.IIdentifiable>>(removed);

                var removedIds = new List<string>();
                foreach(var oid2entry in from p in _Registry._OId2Entry 
                                     where p.Value.Item2.CategoryId == categoryId 
                                     select p)
                {
                    if (moveObjectsToCategoryId == null)
                    {
                        removed.Add(Proxy.Share(oid2entry.Value.Item2.Ref));
                        removedIds.Add(oid2entry.Key);
                    }
                    else
                        oid2entry.Value.Item2.CategoryId = moveObjectsToCategoryId;
                }

                foreach (var oid in removedIds)
                    _Registry._OId2Entry.Remove(oid, out Tuple<string, Registry.Entry, Unregister> removedValue);

                return Task.FromResult<IReadOnlyList<Common.IIdentifiable>>(removed);
            }

            public async Task<IReadOnlyList<Common.IIdentifiable>> RemoveObjects(IReadOnlyList<string> objectIds, CancellationToken cancellationToken_ = default)
            {
                var removed = new List<Common.IIdentifiable>();
                foreach (var oid in objectIds)
                {
                    try
                    {
                        var entry = _Registry._OId2Entry[oid];
                        var obj = entry.Item2.Ref;
                        await _Registry._OId2Entry[oid].Item3.Call(cancellationToken_);
                        if (!_Registry._OId2Entry.ContainsKey(oid))
                            removed.Add(Proxy.Share(entry.Item2.Ref));
                    }
                    catch (KeyNotFoundException) { }
                }
                return removed;
            }
        }

        public class RegistratorImpl : Mas.Rpc.Registry.IRegistrator
        {
            private ServiceRegistry _Registry;

            public RegistratorImpl(ServiceRegistry reg)
            {
                _Registry = reg;
            }

            #region implemenation of Mas.Rpc.Registry.IRegistrator
            public async Task<Common.ICallback> Register(Common.IIdentifiable @ref, string categoryId, CancellationToken cancellationToken_ = default)
            {
                if (categoryId == null)
                    return null;

                if (_Registry._supportedCategories.Where(cat => cat.Id == categoryId).Count() > 0)
                {
                    try
                    {
                        // get id of reference and at the same time test it, before we store it
                        var oid = (await @ref.Info()).Id;
                        var persistentRef = _Registry._savePolicy.Attach(@ref);
                        var unreg = new Unregister(oid, () => {
                            if (_Registry._OId2Entry.TryRemove(oid, out var removedValue))
                                _Registry._SrToken2Capability.TryRemove(removedValue.Item1, out _);
                        });
                        _Registry._OId2Entry[oid] = Tuple.Create("", new Registry.Entry()
                        {
                            CategoryId = categoryId,
                            Ref = persistentRef
                        }, unreg);
                        return unreg;
                    }
                    catch(Capnp.Rpc.RpcException e)
                    {
                        //Console.Error.WriteLine(e.Message);
                    }
                }
                return null;
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
        private string _OId;
        private Action _RemoveService;
        private bool _alreadyUnregistered = false;

        public Unregister(string oid, Action removeService)
        {
            _OId = oid;
            _RemoveService = removeService;
        }
              
        #region implementation of Common.ICallback
        public Task Call(CancellationToken cancellationToken_ = default)
        {
            _RemoveService();
            _alreadyUnregistered = true;
            return Task.CompletedTask;
        }
        #endregion

        public void Dispose()
        {
            if (!_alreadyUnregistered)
                _RemoveService();
        }
    }
}
