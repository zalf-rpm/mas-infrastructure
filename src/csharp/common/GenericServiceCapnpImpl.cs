using Capnp.Rpc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Infrastructure.Service
{
    public class Admin : Mas.Schema.Service.IAdmin
    {
        private System.Timers.Timer _timer = new();
        private System.Timers.Timer _killTimer = new();

        private Mas.Schema.Registry.IRegistry _registry;

        private System.Action<Mas.Schema.Common.IdInformation> _updateIdentity;

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Admin(Mas.Schema.Registry.IRegistry registry, System.Action<Mas.Schema.Common.IdInformation> updateIdentity)
        {
            _registry = registry;
            _timer.AutoReset = false;
            _updateIdentity = updateIdentity;
        }
        
        public void Dispose()
        {
        }

        //private void store_unreg_data(name, unreg_action, rereg_sr)
        //{
        //    self._unreg_sturdy_refs[name] = (unreg_action, rereg_sr)
        //}


        #region implementation of Mas.Rpc.Common.IIdentifiable
        public Task<Mas.Schema.Common.IdInformation> Info(CancellationToken cancellationToken_ = default)
        {
            return Task.FromResult(new Schema.Common.IdInformation()
            { Id = Id, Name = Name, Description = Description });
        }
        #endregion

        #region implementation of Persistence.IPersistent
        // heartbeat @0 ();
        public Task Heartbeat(CancellationToken cancellationToken_ = default)
        {
            _timer.Stop();
            _timer.Start();
            return Task.CompletedTask;
        }

        // setTimeout @1 (seconds :UInt64);
        public Task SetTimeout(ulong seconds, CancellationToken cancellationToken_ = default)
        {
            _timer.Interval = Math.Max(0, seconds*1000);
            if(_timer.Interval > 0)
                _timer.Start();
            else 
                _timer.Stop();
            return Task.CompletedTask;
        }

        // stop @2 ();
        public Task Stop(CancellationToken cancellationToken_ = default)
        {
            _killTimer.Interval = 2000;
            _killTimer.Elapsed += (s, e) => System.Environment.Exit(0);
            _killTimer.Start();
            return Task.CompletedTask;
        }

        // identity @3 () -> Common.IdInformation;
        public Task<IReadOnlyList<Mas.Schema.Common.IdInformation>> Identities(CancellationToken cancellationToken_ = default)
        {
            var info = _registry.Info().Result;
            IReadOnlyList<Mas.Schema.Common.IdInformation> l = new List<Mas.Schema.Common.IdInformation>{ 
                new Mas.Schema.Common.IdInformation { Id = info.Id, Name = info.Name, Description = info.Description }
            };
            return Task.FromResult(l);
        }

        // updateIdentity @4 Common.IdInformation;
        public Task UpdateIdentity(string oldId, Mas.Schema.Common.IdInformation info, CancellationToken cancellationToken_ = default)
        {
            _updateIdentity(info);
            return Task.CompletedTask;
        }
        #endregion
    }
}