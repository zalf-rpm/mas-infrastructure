using Capnp.Rpc;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Crypt = NSec.Cryptography;
using P = Mas.Schema.Persistence;

namespace Mas.Infrastructure.Common
{
    public class Action : Mas.Schema.Common.IAction {
        private System.Action _removeService;
        private bool _alreadyCalled = false;
        private bool _callActionOnDispose = false;
        private Restorer _restorer;

        public Action(System.Action removeService, Restorer restorer = null, bool callActionOnDispose = false) {
            _removeService = removeService;
            _callActionOnDispose = callActionOnDispose;
            _restorer = restorer;
        }
        
        public void Dispose() {
            if (_callActionOnDispose && !_alreadyCalled)
                _removeService();
        }

        #region implementation of Persistence.IPersistent
        public Task<Mas.Schema.Persistence.Persistent.SaveResults> Save(Mas.Schema.Persistence.Persistent.SaveParams ps, 
            CancellationToken cancellationToken_ = default) {
            if(_restorer == null) {
                return Task.FromResult<Mas.Schema.Persistence.Persistent.SaveResults>(null);//Task.FromResult<(string, string)>((null, null));
            } else {
                var res = _restorer.Save(BareProxy.FromImpl(this));
                return Task.FromResult<Mas.Schema.Persistence.Persistent.SaveResults>(
                    new Mas.Schema.Persistence.Persistent.SaveResults(){ SturdyRef = res.SturdyRef});//Task.FromResult<(string, string)>((res.SturdyRef, res.UnsaveSR));
            }
        }
        #endregion

        #region implementation of Common.IAction
        public Task Do(CancellationToken cancellationToken_ = default) {
            _removeService();
            _alreadyCalled = true;
            return Task.CompletedTask;
        }
        #endregion
    }

//-----------------------------------------------------------------------------

    public class Action1 : Mas.Schema.Common.IAction1
    {
        private System.Action<object> _removeService;
        private Restorer _restorer;

        public Action1(System.Action<object> removeService, Restorer restorer = null)
        {
            _removeService = removeService;
            _restorer = restorer;
        }
        
        public void Dispose() {}

        #region implementation of Persistence.IPersistent
                public Task<Mas.Schema.Persistence.Persistent.SaveResults> Save(Mas.Schema.Persistence.Persistent.SaveParams ps, CancellationToken cancellationToken_ = default)
        {
            if(_restorer == null)
                return Task.FromResult<Mas.Schema.Persistence.Persistent.SaveResults>(null);//Task.FromResult<(string, string)>((null, null));
            else {
                var res = _restorer.Save(BareProxy.FromImpl(this));
                return Task.FromResult<Mas.Schema.Persistence.Persistent.SaveResults>(
                    new Mas.Schema.Persistence.Persistent.SaveResults(){ SturdyRef = res.SturdyRef});//Task.FromResult<(string, string)>((res.SturdyRef, res.UnsaveSR));
            }
        }
        // public Task<(string, string)> Save(CancellationToken cancellationToken_ = default)
        // {
        //     if(_restorer == null)
        //         return Task.FromResult<(string, string)>((null, null));
        //     else {
        //         var res = _restorer.Save(BareProxy.FromImpl(this));
        //         return Task.FromResult<(string, string)>((res.SturdyRef, res.UnsaveSR));
        //     }
            
        // }
        #endregion


        #region implementation of Common.IAction1
        public Task Do(object p, CancellationToken cancellationToken_ = default)
        {
            _removeService(p);
            return Task.CompletedTask;
        }
        #endregion
    }

}

//-----------------------------------------------------------------------------