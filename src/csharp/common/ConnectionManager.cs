﻿using Capnp.Rpc;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;

namespace Mas.Infrastructure.Common
{
    public class ConnectionManager : IDisposable
    {
        private ConcurrentDictionary<string, TcpRpcClient> _Connections = new ConcurrentDictionary<string, TcpRpcClient>();
        private TcpRpcServer _Server;

        public ConnectionManager() 
        {
        }

        public void Dispose()
        {
            foreach (var con in _Connections.Values)
                con.Dispose();

            if (_Server != null)
                _Server.Dispose();
        }


        public async Task<Proxy> Connect(string sturdyRefToken)
        {
            // we assume that a sturdy ref url looks always like capnp://hash-digest-or-insecure@host:port/sturdy-ref-token
            if (sturdyRefToken.StartsWith("capnp://")) 
            {
                var hashDigest = "";
                var addressPort = "";
                var address = "";
                var port = 0;
                var srToken = "";

                var rest = sturdyRefToken.Substring(8);
                // is unix domain socket
                if (rest.StartsWith("/"))
                {
                    rest = rest.Substring(1);
                } 
                else
                {
                    var hashDigestAndRest = rest.Split("@");
                    if (hashDigestAndRest.Length > 0) hashDigest = hashDigestAndRest[0];
                    var addressPortAndRest = hashDigestAndRest[^1].Split("/");
                    if (addressPortAndRest.Length > 0)
                    {
                        addressPort = addressPortAndRest[0];
                        var addressAndPort = addressPort.Split(":");
                        if (addressAndPort.Length > 0) address = addressAndPort[0];
                        if (addressAndPort.Length > 1) port = Int32.Parse(addressAndPort[1]);
                    }
                    if (addressPortAndRest.Length > 1) srToken = addressPortAndRest[1];
                }

                if(addressPort.Length > 0)
                {
                    var con = _Connections.GetOrAdd(addressPort, new TcpRpcClient(address, port));
                    await Task.WhenAll(con.WhenConnected);
                    if (!string.IsNullOrEmpty(srToken))
                    {
                        var restorer = con.GetMain<Rpc.Persistence.IRestorer<string>>();
                        var cap = await restorer.Restore(srToken, null);
                        return cap;
                    }
                    else
                    {
                        var bootstrap = con.GetMain<Proxy>();
                        return bootstrap;
                    }
                }
            }
            return null;
        }

        public void Bind(IPAddress address, int tcpPort, object bootstrap)
        {
            if (_Server != null)
                _Server.Dispose();

            _Server = new TcpRpcServer();
            _Server.AddBuffering();
            _Server.Main = bootstrap;
            _Server.StartAccepting(address, tcpPort);
        }
    }
}