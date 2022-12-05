using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;
using S = Mas.Schema;
using R = Mas.Schema.Registry;
using P = Mas.Schema.Persistence;
using C = Mas.Schema.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Reflection;
using Capnp;
using Capnp.Rpc;
using Capnp.Rpc.Interception;
using Crypt = NSec.Cryptography;

namespace Mas.Infrastructure.ServiceRegistry
{
    
    class ServiceRegistry : R.IRegistry
    {
        public Common.Restorer Restorer { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        private Mas.Schema.Storage.Store.IContainer _categoriesStorage = null;

        public async Task SetCategoriesStorage(Mas.Schema.Storage.Store.IContainer storage)
        {
            _categoriesStorage = storage;
            var objs = await storage.ListObjects();
            Categories = objs.Select(o => (C.IdInformation)o.Value.AnyValue).ToArray();
        }

        private ConcurrentDictionary<string, C.IdInformation> _catId2SupportedCategories;
        public C.IdInformation[] Categories
        {
            get { return _catId2SupportedCategories.Values.ToArray(); }
            set {
                foreach (var cat in value)
                {
                    try { _catId2SupportedCategories[cat.Id] = cat; } catch (System.Exception) { }
                }
            }
        }

        public struct RegData
        {
            public string Id { get; set; }
            public R.Registry.Entry Entry { get; set; }
            public Common.Action Unreg { get; set; }
            public C.IIdentifiable Cap { get; set; }
            public Common.Action ReregUnsave { get; set; }
        }

        private ConcurrentDictionary<string, RegData> _regId2Entry;
        //private ConcurrentDictionary<string, (ulong[], string)> _extSRT2VatIdAndIntSRT = new(); // mapping of external sturdy ref token to internal one
        private IInterceptionPolicy _savePolicy;
        //private ConcurrentDictionary<ulong[], Mas.Schema.Persistence.IRestorer> _vatId2Restorer = new();   

        public ServiceRegistry()
        {
            _catId2SupportedCategories = new ConcurrentDictionary<string, C.IdInformation>();
            _regId2Entry = new ConcurrentDictionary<string, RegData>();//Tuple<string, Registry.Entry, Common.Unregister>>();
            _savePolicy = new InterceptPersistentPolicy(this);
        }

        public void Dispose()
        {
            // dispose registered caps
            foreach (var oid2e in _regId2Entry)
            {
                oid2e.Value.Entry.Ref?.Dispose(); // registered services
                oid2e.Value.Unreg.Dispose();
            }

            Console.WriteLine("Dispose");
        }

        #region implementation of Mas.C.IIdentifiable
        public Task<C.IdInformation> Info(CancellationToken cancellationToken_ = default)
        {
            return Task.FromResult(new C.IdInformation()
            { Id = Id, Name = Name, Description = Description });
        }
        #endregion

        #region implementation of Mas.R.IRegistry
        public Task<C.IdInformation> CategoryInfo(string categoryId, CancellationToken cancellationToken_ = default)
        {
            return Task.FromResult(_catId2SupportedCategories.GetValueOrDefault(categoryId));
        }

        public Task<IReadOnlyList<R.Registry.Entry>> Entries(string categoryId, CancellationToken cancellationToken_ = default)
        {
            var entries = new List<R.Registry.Entry>();
            if (categoryId != null)
                entries.AddRange((from p in _regId2Entry
                                  where p.Value.Entry.CategoryId == categoryId
                                  select p.Value.Entry).
                                  Select(e => new R.Registry.Entry(){
                                      CategoryId = e.CategoryId,
                                      Ref = Proxy.Share(e.Ref),
                                      Name = e.Name
                                 }));
            else
                entries.AddRange((from p in _regId2Entry select p.Value.Entry).
                    Select(e => new R.Registry.Entry()
                    {
                        CategoryId = e.CategoryId,
                        Ref = Proxy.Share(e.Ref),
                        Name = e.Name
                    }));
            return Task.FromResult<IReadOnlyList<R.Registry.Entry>>(entries);
        }

        public Task<IReadOnlyList<C.IdInformation>> SupportedCategories(CancellationToken cancellationToken_ = default)
        {
            return Task.FromResult<IReadOnlyList<C.IdInformation>>(_catId2SupportedCategories.Values.ToList());
        }
        #endregion


        public class Admin : R.IAdmin {
            private ServiceRegistry _registry;

            public Admin(ServiceRegistry registry) {
                _registry = registry;
            }

            public void Dispose() { }

            #region implementation of Mas.C.IIdentifiable
            public Task<C.IdInformation> Info(CancellationToken cancellationToken_ = default) {
                return Task.FromResult(new C.IdInformation()
                { Id = "Admin_"+_registry.Id, Name = "Admin of " + _registry.Name, Description = "Admin description of " + _registry.Description });
            }
            #endregion

            #region implementation of Mas.Schema.Registry.IAdmin
            public async Task<bool> AddCategory(C.IdInformation category, bool upsert, 
                CancellationToken cancellationToken_ = default) {
                if (!_registry._catId2SupportedCategories.ContainsKey(category.Id) || upsert) {
                    _registry._catId2SupportedCategories[category.Id] = category;

                    // save new category to storage
                    if (_registry._categoriesStorage != null) {
                        await _registry._categoriesStorage.AddObject(new S.Storage.Store.Container.Object {
                            Key = category.Id, Value = new S.Storage.Store.Container.Object.value {
                                AnyValue=category
                            }
                        });
                    }
                    return true;
                }
                return false;
            }
            
            public Task<IReadOnlyList<string>> MoveObjects(IReadOnlyList<string> objectIds, string toCatId, 
                CancellationToken cancellationToken_ = default) {
                // check that the move to category actually exists, else treat it as none existing
                if (!_registry._catId2SupportedCategories.ContainsKey(toCatId)) toCatId = null;

                // if there is no category to move to, do rather nothing
                // another option would be to remove the objects, but this is what removeObjects is for
                if (toCatId == null) return Task.FromResult<IReadOnlyList<string>>(new List<string>());

                var moved = new List<string>();
                foreach(var oid in objectIds) {
                    try {
                        _registry._regId2Entry[oid].Entry.CategoryId = toCatId;
                        moved.Add(oid);
                    }
                    catch (KeyNotFoundException){}
                }
                return Task.FromResult<IReadOnlyList<string>>(moved);
            }

            public Task<R.IRegistry> Registry(CancellationToken cancellationToken_ = default) {
                return Task.FromResult<R.IRegistry>(_registry);
            }

            public async Task<IReadOnlyList<C.IIdentifiable>> RemoveCategory(string categoryId, string moveObjectsToCategoryId, 
                CancellationToken cancellationToken_ = default) {
                var removed = new List<C.IIdentifiable>();
                // no category means nothing to remove
                if (categoryId == null) return removed;

                // check that the move to category actually exists, else treat it as none existing
                if (moveObjectsToCategoryId != null && !_registry._catId2SupportedCategories.ContainsKey(moveObjectsToCategoryId)) {
                    moveObjectsToCategoryId = null;
                }

                // move objects from old to new category
                // but remember objects to be removed, to remove them outside of the iterator
                var removedIds = new List<string>();
                foreach(var oid2entry in from p in _registry._regId2Entry 
                                     where p.Value.Entry.CategoryId == categoryId 
                                     select p) {
                    if (moveObjectsToCategoryId == null) {
                        removed.Add(Capnp.Rpc.Proxy.Share(oid2entry.Value.Entry.Ref));
                        removedIds.Add(oid2entry.Key);
                    } else {
                        oid2entry.Value.Entry.CategoryId = moveObjectsToCategoryId;
                    }
                }

                // remove remembered objects from registry
                foreach (var oid in removedIds) _registry._regId2Entry.Remove(oid, out RegData removedValue);

                // finally remove the category
                _registry._catId2SupportedCategories.TryRemove(categoryId, out _);

                // remove category from storage
                await _registry._categoriesStorage.RemoveObject(categoryId);

                return removed;
            }

            public async Task<IReadOnlyList<C.IIdentifiable>> RemoveObjects(IReadOnlyList<string> objectIds, CancellationToken cancellationToken_ = default) {
                var removed = new List<C.IIdentifiable>();
                foreach (var oid in objectIds) {
                    try {
                        var entry = _registry._regId2Entry[oid];
                        var obj = entry.Entry.Ref;
                        await _registry._regId2Entry[oid].Unreg.Do(cancellationToken_);
                        if (!_registry._regId2Entry.ContainsKey(oid)) {
                            removed.Add(Capnp.Rpc.Proxy.Share(entry.Entry.Ref));
                        }
                    } catch (KeyNotFoundException) { }
                }
                return removed;
            }
            #endregion
        }

        public class Registrar : R.IRegistrar
        {
            private ServiceRegistry _registry;
            private Common.Restorer _restorer;

            public Registrar(ServiceRegistry reg, Common.Restorer restorer)
            {
                _registry = reg;
                _restorer = restorer;
            }

            public void Dispose()
            {
                Console.WriteLine("RegistratorImpl.Dispose");
            }

            #region implementation of Mas.C.IIdentifiable
            public Task<C.IdInformation> Info(CancellationToken cancellationToken_ = default)
            {
                return Task.FromResult(new C.IdInformation()
                { Id = "Registrar_"+_registry.Id, Name = "Registrar of " + _registry.Name, Description = "Registrar description of " + _registry.Description });
            }
            #endregion

            #region implementation of Mas.Schema.Registry.IRegistrar
            // register @0 (cap :Common.Identifiable, regName :Text, categoryId :Text) -> (unreg :Common.Action, reregSR :Text);
            public Task<(C.IAction, P.SturdyRef)> Register(R.Registrar.RegParams ps, CancellationToken cancellationToken_ = default)
            {
                if (ps.CategoryId == null || ps.RegName == null)
                    return Task.FromResult<(C.IAction, P.SturdyRef)>((null, null));

                // if category exists
                if (_registry._catId2SupportedCategories.ContainsKey(ps.CategoryId))
                {
                    try
                    {
                        // uuid to register cap under
                        var regId = System.Guid.NewGuid().ToString();

                        // attach a membrane around the capability to intercept save messages
                        var interceptedCap = _registry._savePolicy.Attach(ps.Cap);
                        
                        // if this capabiity is supposed to be restorable accross domains, register it at the restorer
                        if (ps.XDomain != null && ps.XDomain.Restorer != null){
                            _restorer.AddOrUpdateCrossDomainRestore(ps.XDomain.VatId, ps.XDomain.Restorer);
                        }

                        // create an unregister action
                        var unreg = new Common.Action(() => {
                            _registry._regId2Entry.TryRemove(regId, out var removedRegData);
                            removedRegData.ReregUnsave?.Do();
                        }, restorer : _restorer, callActionOnDispose : true);

                        var regData = new RegData
                        {
                            Entry = new R.Registry.Entry()
                            {
                                CategoryId = ps.CategoryId,
                                Ref = interceptedCap,
                                Name = ps.RegName,
                            },
                            Unreg = unreg,
                            Cap = Proxy.Share(ps.Cap)
                        };

                        // create an reregister action and sturdy ref to it
                        var rereg = new Common.Action1((object anyp) => {
                            if(anyp is C.IIdentifiable cap)
                            {
                                var interceptedCap = _registry._savePolicy.Attach(cap);

                                _registry._regId2Entry[regId] = new RegData
                                {
                                    Entry = new R.Registry.Entry()
                                    {
                                        CategoryId = ps.CategoryId,
                                        Ref = interceptedCap,
                                        Name = ps.RegName,
                                    },
                                    Unreg = unreg,
                                    Cap = Proxy.Share(cap)
                                };
                            }
                        });
                        // get the sturdy ref to the reregister action
                        var res = _restorer.Save(BareProxy.FromImpl(rereg));
                        // and save the unsave action to remove the sturdy ref on unregistration of the capability
                        regData.ReregUnsave = res.UnsaveAction;

                        _registry._regId2Entry[regId] = regData;
                        
                        // !!! note it is fine to accept manually aquired sturdy refs to the unreg action
                        // !!! to not be automatically removed on unregistration of the capability as the user might
                        // !!! still want to keep the aquired sturdy ref to get a reference to the capability
                        // !!! in this case the registry's vat acts just as proxy and not anymore as registry

                        return Task.FromResult<(C.IAction, P.SturdyRef)>((unreg, res.SturdyRef));
                    }
                    catch (Capnp.Rpc.RpcException e)
                    {
                        Console.Error.WriteLine(e.Message);
                    }
                }
                return Task.FromResult<(C.IAction, P.SturdyRef)>((null, null));
            }
            #endregion
        }

        class InterceptPersistentPolicy : IInterceptionPolicy
        {
            private ulong PersistentInterfaceId;
            private ulong SaveMethodId = 0;
            //private ulong RestorerInterfaceId;
            //private ulong RestoreMethodId = 0;
            private ServiceRegistry _registry;

            public InterceptPersistentPolicy(ServiceRegistry registry)
            {
                _registry = registry;
                PersistentInterfaceId = typeof(P.IPersistent).GetCustomAttribute<Capnp.TypeIdAttribute>(false)?.Id ?? 0;
                //RestorerInterfaceId = typeof(P.IRestorer).GetCustomAttribute<Capnp.TypeIdAttribute>(false)?.Id ?? 0;
            }

            public bool Equals([AllowNull] IInterceptionPolicy other)
            {
                return this.Equals(other);
            }

            public void OnCallFromAlice(CallContext callContext)
            {
                callContext.ForwardToBob();
            }

            public void OnReturnFromBob(CallContext callContext)
            {
                if (callContext.InterfaceId == PersistentInterfaceId && callContext.MethodId == SaveMethodId)
                {
                    var result = CapnpSerializable.Create<P.Persistent.SaveResults>(callContext.OutArgs);
                    var ds = (DeserializerState)result.SturdyRef.TheTransient.LocalRef; 
                    var intSRT = CapnpSerializable.Create<string>(ds);
                    var extSRT = System.Guid.NewGuid().ToString();
                    var vid = result.SturdyRef.TheTransient.Vat.Id;
                    _registry.Restorer.InstallCrossDomainMapping(extSRT, result.SturdyRef.TheTransient.Vat.Id, intSRT);
                    result.SturdyRef = _registry.Restorer.SturdyRef(extSRT);
                    var resultWriter = SerializerState.CreateForRpc<P.Persistent.SaveResults.WRITER>();
                    result.serialize(resultWriter);
                    callContext.OutArgs = resultWriter;
                } 
                callContext.ReturnToAlice();
            }
        }

    }
}
