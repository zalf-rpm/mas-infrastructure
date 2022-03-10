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
    public class Admin : Mas.Rpc.Service.IAdmin
    {
        private System.Timers.Timer _timer = new();
        private System.Timers.Timer _killTimer = new();

        private Mas.Rpc.Registry.IRegistry _registry;

        private System.Action<Mas.Rpc.Common.IdInformation> _updateIdentity;

        public Admin(Mas.Rpc.Registry.IRegistry registry, System.Action<Mas.Rpc.Common.IdInformation> updateIdentity)
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
        public Task<Mas.Rpc.Common.IdInformation> Identity(CancellationToken cancellationToken_ = default)
        {
            var info = _registry.Info().Result;
            return Task.FromResult(new Mas.Rpc.Common.IdInformation 
            { Id = info.Id, Name = info.Name, Description = info.Description });
        }

        // updateIdentity @4 Common.IdInformation;
        public Task UpdateIdentity(Mas.Rpc.Common.IdInformation info, CancellationToken cancellationToken_ = default)
        {
            _updateIdentity(info);
            return Task.CompletedTask;
        }
        #endregion
    }
}