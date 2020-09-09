using System;
using Mas.Rpc;
using Capnp.Rpc;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;

namespace Mas.Infrastructure.ServiceRegistry
{
    class ServiceRegistryImpl : Service.IRegistry
    {
        string _id;
        string _name;
        string _description;
        ConcurrentDictionary<string, Tuple<Service.ServiceType, Common.IIdentifiable, Unregister>> _services = new ConcurrentDictionary<string, Tuple<Service.ServiceType, Common.IIdentifiable, Unregister>>();

        public ServiceRegistryImpl(string id = "", string name = "", string description = "")
        {
            _id = id;
            _name = name;
            _description = description;
        }

        public Task<IReadOnlyList<Service.Registry.Entry>> GetAvailableServices(Service.Registry.Query query, CancellationToken cancellationToken_ = default)
        {
            Func<KeyValuePair<string, Tuple<Service.ServiceType, Common.IIdentifiable, Unregister>>, Service.Registry.Entry> transform =
                kv => new Service.Registry.Entry()
                {
                    RegToken = kv.Key,
                    Type = kv.Value.Item1,
                    Service = Proxy.Share(kv.Value.Item2)
                };

            if (query.which == Service.Registry.Query.WHICH.All)
            {
                var x = _services.Select(transform);
                return Task.FromResult<IReadOnlyList<Service.Registry.Entry>>(x.ToList());
            }
            else
            {
                var x = _services.Where(kv => kv.Value.Item1 == query.Type).Select(transform);
                return Task.FromResult<IReadOnlyList<Service.Registry.Entry>>(x.ToList());
            }
        }

        public Task<TService> GetService<TService>(string regToken, CancellationToken cancellationToken_ = default) where TService : class
        {
            if(_services.ContainsKey(regToken))
            {
                var (_, service, _) = _services[regToken];
                try
                {
                    return Task.FromResult((TService)Proxy.Share(service));
                }
                catch (InvalidCastException) { }
            }
            return Task.FromResult<TService>(null);
        }

        public Task<(string, Common.Registry.IUnregister)> RegisterService(Service.ServiceType type, Common.IIdentifiable service, CancellationToken cancellationToken_ = default)
        {
            var uuid = System.Guid.NewGuid().ToString();
            var unreg = new Unregister(uuid, () => _services.TryRemove(uuid, out Tuple<Service.ServiceType, Common.IIdentifiable, Unregister> removedValue));
            _services[uuid] = Tuple.Create(type, service, unreg);
            return Task.FromResult(ValueTuple.Create(uuid, Proxy.Share((Common.Registry.IUnregister)unreg)));
        }

        void IDisposable.Dispose()
        {
            // free service caps
            foreach (var kvp in _services)
            {
                kvp.Value.Item2?.Dispose();
            }
            Console.WriteLine("Dispose");
        }

        Task<Common.IdInformation> Common.IIdentifiable.Info(CancellationToken cancellationToken_)
        {
            return Task.FromResult<Common.IdInformation>(new Common.IdInformation()
            { Id = _id, Name = _name, Description = _description });
        }
    }

    class Unregister : Common.Registry.IUnregister
    {
        string _regToken;
        Action _removeService;
        bool _alreadyUnregistered = false;

        public Unregister(string regToken, Action removeService)
        {
            _regToken = regToken;
            _removeService = removeService;
        }

        void IDisposable.Dispose()
        {
            if (!_alreadyUnregistered)
            {
                _removeService();
                Console.WriteLine("Removing service in Dispose for regToken: " + _regToken);
            }
        }

        Task Common.Registry.IUnregister.Unregister(CancellationToken cancellationToken_)
        {
            _removeService();
            _alreadyUnregistered = true;
            Console.WriteLine("Removing service in Unregister for regToken: " + _regToken);
            return Task.CompletedTask;
        }
    }
}
