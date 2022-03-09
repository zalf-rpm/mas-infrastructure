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
using System.Text.Json;

namespace Mas.Infrastructure.ServiceRegistry
{
    
    class ServiceRegistry : Rpc.Registry.IRegistry
    {
        public Common.Restorer Restorer { get; set; }

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

        private string _categoriesFilePath = "categories.json";
        public string CategoriesFilePath { 
            get { return _categoriesFilePath; }
            set {
                _categoriesFilePath = value;
                Categories = DeserializeCats(System.IO.File.ReadAllText(_categoriesFilePath));
            } 
        } 

        static public Rpc.Common.IdInformation[] DeserializeCats(string catsJsonStr)
        {
            return JsonSerializer.Deserialize<Rpc.Common.IdInformation[]>(
                catsJsonStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }


        public struct RegData
        {
            public string Id { get; set; }
            public Registry.Entry Entry { get; set; }
            public Common.Action Unreg { get; set; }
            public Rpc.Common.IIdentifiable Cap { get; set; }
            public Common.Action ReregUnsave { get; set; }
        }

        private ConcurrentDictionary<string, RegData> _regId2Entry;
        private IInterceptionPolicy _savePolicy;

        public ServiceRegistry()
        {
            _CatId2SupportedCategories = new ConcurrentDictionary<string, Rpc.Common.IdInformation>();
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

        public Task<IReadOnlyList<Registry.Entry>> Entries(string categoryId, CancellationToken cancellationToken_ = default)
        {
            var entries = new List<Registry.Entry>();
            if (categoryId != null)
                entries.AddRange((from p in _regId2Entry
                                  where p.Value.Entry.CategoryId == categoryId
                                  select p.Value.Entry).
                                  Select(e => new Registry.Entry(){
                                      CategoryId = e.CategoryId,
                                      Ref = Proxy.Share(e.Ref),
                                      Name = e.Name
                                 }));
            else
                entries.AddRange((from p in _regId2Entry select p.Value.Entry).
                    Select(e => new Registry.Entry()
                    {
                        CategoryId = e.CategoryId,
                        Ref = Proxy.Share(e.Ref),
                        Name = e.Name
                    }));
            return Task.FromResult<IReadOnlyList<Registry.Entry>>(entries);
        }

        public Task<IReadOnlyList<Rpc.Common.IdInformation>> SupportedCategories(CancellationToken cancellationToken_ = default)
        {
            return Task.FromResult<IReadOnlyList<Rpc.Common.IdInformation>>(_CatId2SupportedCategories.Values.ToList());
        }
        #endregion


        public class Admin : Rpc.Registry.IAdmin
        {
            private ServiceRegistry _Registry;

            public Admin(ServiceRegistry registry)
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
                        _Registry._regId2Entry[oid].Entry.CategoryId = toCatId;
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
                foreach(var oid2entry in from p in _Registry._regId2Entry 
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
                    _Registry._regId2Entry.Remove(oid, out RegData removedValue);

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
                        var entry = _Registry._regId2Entry[oid];
                        var obj = entry.Entry.Ref;
                        await _Registry._regId2Entry[oid].Unreg.Do(cancellationToken_);
                        if (!_Registry._regId2Entry.ContainsKey(oid))
                            removed.Add(Capnp.Rpc.Proxy.Share(entry.Entry.Ref));
                    }
                    catch (KeyNotFoundException) { }
                }
                return removed;
            }
            #endregion
        }

        public class Registrar : Rpc.Registry.IRegistrar
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

            #region implementation of Mas.Rpc.Registry.IRegistrar
            // register @0 (cap :Common.Identifiable, regName :Text, categoryId :Text) -> (unreg :Common.Action, reregSR :Text);
            public Task<(Mas.Rpc.Common.IAction, string)> Register(Rpc.Common.IIdentifiable cap, string regName, string categoryId, CancellationToken cancellationToken_ = default)
            {
                if (categoryId == null || regName == null)
                    return Task.FromResult<(Mas.Rpc.Common.IAction, string)>((null, null));

                if (_registry._CatId2SupportedCategories.ContainsKey(categoryId))
                {
                    try
                    {
                        // uuid to register cap under
                        var regId = System.Guid.NewGuid().ToString();

                        // attach a membrane around the capability to intercept save messages
                        var interceptedCap = _registry._savePolicy.Attach(cap);

                        // create an unregister action
                        var unreg = new Common.Action(() => {
                            _registry._regId2Entry.TryRemove(regId, out var removedRegData);
                            removedRegData.ReregUnsave?.Do();
                        }, restorer : _restorer, callActionOnDispose : true);

                        var regData = new RegData
                        {
                            Entry = new Registry.Entry()
                            {
                                CategoryId = categoryId,
                                Ref = interceptedCap,
                                Name = regName,
                            },
                            Unreg = unreg,
                            Cap = Proxy.Share(cap)
                        };

                        // create an reregister action and sturdy ref to it
                        var rereg = new Common.Action1((object anyp) => {
                            if(anyp is Rpc.Common.IIdentifiable cap)
                            {
                                var interceptedCap = _registry._savePolicy.Attach(cap);

                                _registry._regId2Entry[regId] = new RegData
                                {
                                    Entry = new Registry.Entry()
                                    {
                                        CategoryId = categoryId,
                                        Ref = interceptedCap,
                                        Name = regName,
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

                        return Task.FromResult<(Mas.Rpc.Common.IAction, string)>((unreg, res.SturdyRef));
                    }
                    catch (Capnp.Rpc.RpcException e)
                    {
                        Console.Error.WriteLine(e.Message);
                    }
                }
                return Task.FromResult<(Mas.Rpc.Common.IAction, string)>((null, null));
            }
            #endregion
        }

        class InterceptPersistentPolicy : IInterceptionPolicy
        {
            private ulong PersistentInterfaceId;
            private ServiceRegistry _registry;

            public InterceptPersistentPolicy(ServiceRegistry registry)
            {
                _registry = registry;
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
                    var result = CapnpSerializable.Create<Mas.Rpc.Persistence.Persistent.Result_Save>(callContext.OutArgs);
                    var res = _registry.Restorer.Save(new BareProxy((ConsumedCapability)callContext.Bob));
                    result.SturdyRef = res.SturdyRef;
                    result.UnsaveSR = res.UnsaveSR;
                    var resultWriter = SerializerState.CreateForRpc<Mas.Rpc.Persistence.Persistent.Result_Save.WRITER>();
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
