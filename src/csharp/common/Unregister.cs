using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Infrastructure.Common
{
    public class Unregister : Mas.Rpc.Common.ICallback
    {
        private string _OId;
        private Action _RemoveService;
        private bool _alreadyUnregistered = false;

        public Unregister(string oid, Action removeService)
        {
            _OId = oid;
            _RemoveService = removeService;
        }
        public void Dispose()
        {
            if (!_alreadyUnregistered)
                _RemoveService();
        }

        #region implementation of Common.ICallback
        public Task Call(CancellationToken cancellationToken_ = default)
        {
            _RemoveService();
            _alreadyUnregistered = true;
            return Task.CompletedTask;
        }
        #endregion
    }
}
