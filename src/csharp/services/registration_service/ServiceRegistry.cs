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

namespace Mas.Infrastructure.ServiceRegistry
{
    class PersistableProxy : Proxy
    {
        private BareProxy _bareProxy;
        /// <summary>
        /// Wraps a capability implementation in a Proxy.
        /// </summary>
        /// <param name="impl">Capability implementation</param>
        /// <returns>Proxy</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="impl"/> is null.</exception>
        /// <exception cref="InvalidCapabilityInterfaceException">No <see cref="SkeletonAttribute"/> found on implemented interface(s).</exception>
        /// <exception cref="System.InvalidOperationException">Mismatch between generic type arguments (if capability interface is generic).</exception>
        /// <exception cref="System.ArgumentException">Mismatch between generic type arguments (if capability interface is generic).</exception>
        /// <exception cref="System.Reflection.TargetInvocationException">Problem with instatiating the Skeleton (constructor threw exception).</exception>
        /// <exception cref="System.MemberAccessException">Caller does not have permission to invoke the Skeleton constructor.</exception>
        /// <exception cref="System.TypeLoadException">Problem with building the Skeleton type, or problem with loading some dependent class.</exception>
        public static PersistableProxy FromBareProxy(BareProxy bp)
        {
            return new PersistableProxy(bp);
        }

        /// <summary>
        /// Constructs an unbound instance.
        /// </summary>
        public PersistableProxy()
        {
        }

        /// <summary>
        /// Constructs an instance and binds it to the given low-level capability.
        /// </summary>
        /// <param name="cap">low-level capability</param>
        public PersistableProxy(BareProxy bp)
        {
            _bareProxy = bp;
        }

        /// <summary>
        /// Requests a method call.
        /// </summary>
        /// <param name="interfaceId">Target interface ID</param>
        /// <param name="methodId">Target method ID</param>
        /// <param name="args">Method arguments</param>
        /// <returns>Answer promise</returns>
        public IPromisedAnswer Call(ulong interfaceId, ushort methodId, Capnp.DynamicSerializerState args)
        {
            if(interfaceId == 14468694717054801553UL) //Capnp.Persistent_Skeleton<string, string>.)
            {
                Console.WriteLine("bla");
            }
            return base.Call(interfaceId, methodId, args, default);
        }
    }


    class ServiceRegistry : Mas.Rpc.Registry.IRegistry, Mas.Rpc.Persistence.IRestorer<string>
    {
        private string _id, _name, _description;
        private ConcurrentBag<Common.IdInformation> _supportedCategories;
        private ConcurrentDictionary<string, Tuple<string, Registry.Entry, Unregister>> _srToken2Entry;
        private ConcurrentDictionary<string, BareProxy> _srToken2Proxy;

        public ServiceRegistry(string id = "", string name = "", string description = "")
        {
            _id = id;
            _name = name;
            _description = description;
            _supportedCategories = new ConcurrentBag<Common.IdInformation>();
            _supportedCategories.Add(new Common.IdInformation() { Id = "abc" });
            _srToken2Entry = new ConcurrentDictionary<string, Tuple<string, Registry.Entry, Unregister>>();
            _srToken2Proxy = new ConcurrentDictionary<string, BareProxy>();
            _srToken2Proxy["registratorABCD"] = BareProxy.FromImpl(this);

            //IEquatable
            //IInterceptionPolicy
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
                entries.AddRange(from p in _srToken2Entry
                                 where p.Value.Item1 == categoryId
                                 select p.Value.Item2);
            else
                entries.AddRange(from p in _srToken2Entry select p.Value.Item2);
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
            BareProxy bp;
            _srToken2Proxy.Remove(srToken, out bp);
            return Task.CompletedTask;
        }

        public Task<BareProxy> Restore(string srToken, SturdyRef.Owner owner, CancellationToken cancellationToken_ = default)
        {
            if (_srToken2Proxy.ContainsKey(srToken))
                return Task.FromResult(Proxy.Share(_srToken2Proxy[srToken]));
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

                using (@ref)
                {
                    if (_registry._supportedCategories.Where(cat => cat.Id == categoryId).Count() > 0)
                    {
                        var uuid = System.Guid.NewGuid().ToString();
                        var unreg = new Unregister(uuid, () => _registry._srToken2Entry.TryRemove(uuid, out Tuple<string, Registry.Entry, Unregister> removedValue));
                        _registry._srToken2Entry[categoryId] = Tuple.Create(uuid, new Registry.Entry() { CategoryId = categoryId, Ref = Proxy.Share(@ref) }, unreg);
                        return Task.FromResult<Common.ICallback>(unreg);
                    }
                }
                return Task.FromResult<Common.ICallback>(null);
            }
            #endregion

            public void Dispose()
            {
                Console.WriteLine("RegistratorImpl.Dispose");
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
