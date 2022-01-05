using System;
using Mas;
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
using Mas.Infrastructure;

namespace Mas.Infrastructure.ServiceRegistry
{
    class ServiceRegistry : Rpc.Registry.IRegistry, Rpc.Persistence.IRestorer<string>//, Rpc.Persistence.IExternalPersistent<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        private ConcurrentDictionary<string, Rpc.Common.IdInformation> _CatId2SupportedCategories;
        public Rpc.Common.IdInformation[] Categories
        {
            get { return _CatId2SupportedCategories.Values.ToArray(); }
            set {
                foreach (var cat in value)
                {
                    try { _CatId2SupportedCategories[cat.Id] = cat; } catch (System.Exception) { }
                }
            }
        }
        
        public struct RegData
        {
            public string Id { get; set; }
            public Registry.Entry Entry { get; set; }
            public Common.Unregister Unreg { get; set; }
            public string SturdyRef { get; set; }
        }

        //private ConcurrentDictionary<string, Tuple<string, Registry.Entry, Common.Unregister>> _OId2Entry;
        private ConcurrentDictionary<string, RegData> _OId2Entry;
        private ConcurrentDictionary<string, Proxy> _SrToken2Capability;
        private IInterceptionPolicy _savePolicy;

        public int TcpPort { get; set; }

        public ServiceRegistry()
        {
            _CatId2SupportedCategories = new ConcurrentDictionary<string, Rpc.Common.IdInformation>();
            _OId2Entry = new ConcurrentDictionary<string, RegData>();//Tuple<string, Registry.Entry, Common.Unregister>>();
            _SrToken2Capability = new ConcurrentDictionary<string, Proxy>();
            _savePolicy = new InterceptPersistentPolicy(this);
        }
        public void Dispose()
        {
            // dispose registered caps
            foreach (var oid2e in _OId2Entry)
            {
                oid2e.Value.Entry.Ref?.Dispose(); // registered services
                oid2e.Value.Unreg.Dispose();
            }

            // dispose sturdy ref caps
            foreach (var sr2p in _SrToken2Capability)
                sr2p.Value?.Dispose();

            Console.WriteLine("Dispose");
        }

        /*
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
        */

        public string SaveCapability(Proxy proxy, string fixedSrToken = null)
        {
            var existingSturdyRefs = from p in _SrToken2Capability
                                     where p.Value.ConsumedCap == proxy.ConsumedCap
                                     select p.Key;
            var srToken = "";
            if (existingSturdyRefs.Count() > 0)
                srToken = existingSturdyRefs.First();
            else
            {
                srToken = fixedSrToken ?? System.Guid.NewGuid().ToString();
                _SrToken2Capability[srToken] = proxy;
            }
            var ip = Dns.GetHostName(); // "localhost";// GetLocalIPAddress();
            return $"capnp://insecure@{ip}:{TcpPort}/{srToken}";
        }

        #region implementation of Mas.Rpc.Common.IIdentifiable
        public Task<Rpc.Common.IdInformation> Info(CancellationToken cancellationToken_ = default)
        {
            return Task.FromResult(new Rpc.Common.IdInformation()
            { Id = Id, Name = Name, Description = Description });
        }
        #endregion

        #region implementation of Mas.Rpc.Registry.IRegistry
        public Task<Rpc.Common.IdInformation> CategoryInfo(string categoryId, CancellationToken cancellationToken_ = default)
        {
            return Task.FromResult(_CatId2SupportedCategories.GetValueOrDefault(categoryId));
        }

        public Task<IReadOnlyList<Registry.Entry>> Entries(string categoryId, bool forceRefInfos, CancellationToken cancellationToken_ = default)
        {
            var entries = new List<Registry.Entry>();
            if (categoryId != null)
                entries.AddRange((from p in _OId2Entry
                                  where p.Value.Entry.CategoryId == categoryId
                                  select p.Value.Entry).
                                  Select(e => new Registry.Entry(){
                                      CategoryId = e.CategoryId,
                                      Ref = Proxy.Share(e.Ref)
                                 }));
            else
                entries.AddRange((from p in _OId2Entry select p.Value.Entry).
                    Select(e => new Registry.Entry()
                    {
                        CategoryId = e.CategoryId,
                        Ref = Proxy.Share(e.Ref)
                    }));
            return Task.FromResult<IReadOnlyList<Registry.Entry>>(entries);
        }

        public Task<IReadOnlyList<Rpc.Common.IdInformation>> SupportedCategories(CancellationToken cancellationToken_ = default)
        {
            return Task.FromResult<IReadOnlyList<Rpc.Common.IdInformation>>(_CatId2SupportedCategories.Values.ToList());
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

        /*
        #region implementation of Mas.Rpc.Persistence.IExternalPersistent<string>
        public Task<ExternalPersistent<string>.ExternalSaveResults> Save(BareProxy cap, Persistent<string, SturdyRef.Owner>.SaveParams @params, CancellationToken cancellationToken_ = default)
        {
            try
            {
                var sturdyRef = SaveCapability(Capnp.Rpc.Proxy.Share(cap));
                var unreg = new Common.Unregister(sturdyRef, () => {
                    _SrToken2Capability.TryRemove(sturdyRef, out _);
                });
                var res = new ExternalPersistent<string>.ExternalSaveResults
                {
                    Results = new Persistent<string, SturdyRef.Owner>.SaveResults { SturdyRef = sturdyRef },
                    Unreg = unreg
                };
                return Task.FromResult(res);
            }
            catch (Capnp.Rpc.RpcException e)
            {
                Console.Error.WriteLine(e.Message);
            }

            return Task.FromResult(new ExternalPersistent<string>.ExternalSaveResults());
        }
        #endregion
        */


        public class AdminImpl : Rpc.Registry.IAdmin
        {
            private ServiceRegistry _Registry;

            public AdminImpl(ServiceRegistry registry)
            {
                _Registry = registry;
            }

            public void Dispose() { }

            #region implementation of Mas.Rpc.Registry.IAdmin
            public Task<bool> AddCategory(Rpc.Common.IdInformation category, bool upsert, CancellationToken cancellationToken_ = default)
            {
                if (!_Registry._CatId2SupportedCategories.ContainsKey(category.Id) || upsert)
                {
                    _Registry._CatId2SupportedCategories[category.Id] = category;
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            
            public Task<IReadOnlyList<string>> MoveObjects(IReadOnlyList<string> objectIds, string toCatId, CancellationToken cancellationToken_ = default)
            {
                // check that the move to category actually exists, else treat it as none existing
                if (!_Registry._CatId2SupportedCategories.ContainsKey(toCatId))
                    toCatId = null;

                // if there is no category to move to, do rather nothing
                // another option would be to remove the objects, but this is what removeObjects is for
                if (toCatId == null)
                    return Task.FromResult<IReadOnlyList<string>>(new List<string>());

                var moved = new List<string>();
                foreach(var oid in objectIds)
                {
                    try
                    {
                        _Registry._OId2Entry[oid].Entry.CategoryId = toCatId;
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

            public Task<IReadOnlyList<Rpc.Common.IIdentifiable>> RemoveCategory(string categoryId, string moveObjectsToCategoryId, CancellationToken cancellationToken_ = default)
            {
                var removed = new List<Rpc.Common.IIdentifiable>();
                // no category means nothing to remove
                if (categoryId == null)
                    return Task.FromResult<IReadOnlyList<Rpc.Common.IIdentifiable>>(removed);

                // check that the move to category actually exists, else treat it as none existing
                if (moveObjectsToCategoryId != null && !_Registry._CatId2SupportedCategories.ContainsKey(moveObjectsToCategoryId))
                    moveObjectsToCategoryId = null;

                // move objects from old to new category
                // but remember objects to be removed, to remove them outside of the iterator
                var removedIds = new List<string>();
                foreach(var oid2entry in from p in _Registry._OId2Entry 
                                     where p.Value.Entry.CategoryId == categoryId 
                                     select p)
                {
                    if (moveObjectsToCategoryId == null)
                    {
                        removed.Add(Capnp.Rpc.Proxy.Share(oid2entry.Value.Entry.Ref));
                        removedIds.Add(oid2entry.Key);
                    }
                    else
                        oid2entry.Value.Entry.CategoryId = moveObjectsToCategoryId;
                }

                // remove remembered objects from registry
                foreach (var oid in removedIds)
                    _Registry._OId2Entry.Remove(oid, out RegData removedValue);

                // finally remove the category
                _Registry._CatId2SupportedCategories.TryRemove(categoryId, out _);

                return Task.FromResult<IReadOnlyList<Rpc.Common.IIdentifiable>>(removed);
            }

            public async Task<IReadOnlyList<Rpc.Common.IIdentifiable>> RemoveObjects(IReadOnlyList<string> objectIds, CancellationToken cancellationToken_ = default)
            {
                var removed = new List<Rpc.Common.IIdentifiable>();
                foreach (var oid in objectIds)
                {
                    try
                    {
                        var entry = _Registry._OId2Entry[oid];
                        var obj = entry.Entry.Ref;
                        await _Registry._OId2Entry[oid].Unreg.Call(cancellationToken_);
                        if (!_Registry._OId2Entry.ContainsKey(oid))
                            removed.Add(Capnp.Rpc.Proxy.Share(entry.Entry.Ref));
                    }
                    catch (KeyNotFoundException) { }
                }
                return removed;
            }
            #endregion
        }

        public class RegistratorImpl : Rpc.Registry.IRegistrator
        {
            private ServiceRegistry _Registry;

            public RegistratorImpl(ServiceRegistry reg)
            {
                _Registry = reg;
            }

            public void Dispose()
            {
                Console.WriteLine("RegistratorImpl.Dispose");
            }

            #region implementation of Mas.Rpc.Registry.IRegistrator
            public async Task<Rpc.Common.ICallback> Register(Rpc.Common.IIdentifiable @ref, string categoryId, CancellationToken cancellationToken_ = default)
            {
                if (categoryId == null)
                    return null;

                if (_Registry._CatId2SupportedCategories.ContainsKey(categoryId))
                {
                    try
                    {
                        // get id of reference and at the same time test it, before we store it
                        var oid = (await @ref.Info()).Id;
                        //try to get the sturdyRef of the to be registered service
                        string sturdyRef = null;
                        try { sturdyRef = (await ((Proxy)@ref).Cast<IPersistent<string, string>>(false).Save(null)).SturdyRef; }
                        catch (Capnp.Rpc.RpcException) { }
                        var persistentRef = _Registry._savePolicy.Attach(@ref);
                        var unreg = new Common.Unregister(oid, () => {
                            if (_Registry._OId2Entry.TryRemove(oid, out var removedValue))
                                _Registry._SrToken2Capability.TryRemove(removedValue.Id, out _);
                        });
                        _Registry._OId2Entry[oid] = new RegData
                        {
                            Entry = new Registry.Entry()
                            {
                                CategoryId = categoryId,
                                Ref = persistentRef
                            },
                            Unreg = unreg,
                            SturdyRef = sturdyRef
                        };
                        return unreg;
                    }
                    catch (Capnp.Rpc.RpcException e)
                    {
                        Console.Error.WriteLine(e.Message);
                    }
                }
                return null;
            }
            #endregion
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
                    var sturdyRef = _Registry.SaveCapability(new BareProxy((ConsumedCapability)callContext.Bob));
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
}
