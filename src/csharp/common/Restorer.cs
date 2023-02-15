using Capnp.Rpc;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Crypt = NSec.Cryptography;
using P = Mas.Schema.Persistence;

namespace Mas.Infrastructure.Common
{
    public class Restorer : Mas.Schema.Persistence.IRestorer {
        private ConcurrentDictionary<string, Proxy> _srToken2Capability = new();

        public string TcpHost { get; set; } = "127.0.0.1";
        public ushort TcpPort { get; set; } = 0;
        public byte[] VatId { get; set; }

        public Mas.Schema.Storage.Store.IContainer StorageContainer { get; set; } = null;

        private ConcurrentDictionary<string, ((ulong, ulong, ulong, ulong), string)> _extSRT2VatIdAndIntSRT = new(); // mapping of external sturdy ref token to internal one

        private ConcurrentDictionary<(ulong, ulong, ulong, ulong), Mas.Schema.Persistence.IRestorer> _vatId2Restorer = new();   

        private Crypt.Key _vatKey;

        public Restorer() {
            //TcpHost = Dns.GetHostName(); //"localhost";// GetLocalIPAddress();

            var algorithm = Crypt.SignatureAlgorithm.Ed25519;
            _vatKey = Crypt.Key.Create(algorithm);
            VatId = _vatKey.Export(Crypt.KeyBlobFormat.PkixPublicKey);
        }
        

        public struct SaveRes {
            public Mas.Schema.Persistence.SturdyRef SturdyRef { get; set; }
            public Mas.Schema.Persistence.SturdyRef UnsaveSR { get; set; } 
            //public string SturdyRefStr { get; set; }
            //public string SRToken { get; set; }
            //public string UnsaveSR { get; set; } 
            //public string UnsaveSRToken { get; set; }
            public Action UnsaveAction { get; set; }
        }

        public (string, string) SaveStr(Capnp.Rpc.Proxy proxy, string fixedSrToken = null, string sealForOwner = null, 
            bool includeUnsave = true) {
            var srToken = fixedSrToken ?? System.Guid.NewGuid().ToString();
            _srToken2Capability[srToken] = proxy;
            if(includeUnsave) {
                var unsaveSRToken = System.Guid.NewGuid().ToString();
                var unsaveAction = new Action(() => { Unsave(srToken); Unsave(unsaveSRToken); }); 
                _srToken2Capability[unsaveSRToken] = BareProxy.FromImpl(unsaveAction);
                return (SturdyRefStr(srToken), SturdyRefStr(unsaveSRToken));
            } else {
                return (SturdyRefStr(srToken), null);
            }   
        }

        public string SturdyRefStr(string srToken) {
            var vatIdBase64 = Convert.ToBase64String(VatId, 0, VatId.Length);
            var srTokenBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(srToken));
            return $"capnp://{vatIdBase64}@{TcpHost}:{TcpPort}/{srTokenBase64}";
        }

        static public string SturdyRefStr(P.SturdyRef sturdyRef) {
            var id = sturdyRef.TheTransient.Vat.Id;
            byte[] vatIdBytes = new byte[4 * 8];
            BitConverter.GetBytes(id.PublicKey0).CopyTo(vatIdBytes, 0);
            BitConverter.GetBytes(id.PublicKey1).CopyTo(vatIdBytes, 8);
            BitConverter.GetBytes(id.PublicKey2).CopyTo(vatIdBytes, 16);
            BitConverter.GetBytes(id.PublicKey3).CopyTo(vatIdBytes, 24);
            var vatIdBase64 = Convert.ToBase64String(vatIdBytes);
            var srTokenBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes((string)sturdyRef.TheTransient.LocalRef));
            return $"capnp://{vatIdBase64}@{sturdyRef.TheTransient.Vat.Address.Host}:{sturdyRef.TheTransient.Vat.Address.Port}/{srTokenBase64}";
        }

        public void InstallCrossDomainMapping(string extSRT, P.VatId vatId, string intSRT) {
            _extSRT2VatIdAndIntSRT[extSRT] = 
                ((vatId.PublicKey0, vatId.PublicKey1, vatId.PublicKey2, vatId.PublicKey3), intSRT);
        }

        public SaveRes Save(Capnp.Rpc.Proxy proxy, string fixedSRToken = null, string sealForOwner = null, 
            bool includeUnsave = true) {
            var srToken = fixedSRToken ?? System.Guid.NewGuid().ToString();
            _srToken2Capability[srToken] = proxy;

            if(includeUnsave) {
                var unsaveSRToken = System.Guid.NewGuid().ToString();
                var unsaveAction = new Action(() => { Unsave(srToken); Unsave(unsaveSRToken); }); 
                _srToken2Capability[unsaveSRToken] = BareProxy.FromImpl(unsaveAction);
                return new SaveRes { 
                    SturdyRef = SturdyRef(srToken), 
                    //SRToken = srToken,
                    UnsaveSR = SturdyRef(unsaveSRToken),
                    //UnsaveSRToken = unsaveSRToken,
                    UnsaveAction = unsaveAction
                };
            } else {
                return new SaveRes { 
                    SturdyRef = SturdyRef(srToken), 
                    //SRToken = srToken
                };  
            }   
        }



        public Mas.Schema.Persistence.SturdyRef SturdyRef(string srToken) {
            var vid = VatId;
            if(!BitConverter.IsLittleEndian) Array.Reverse(vid, 0, vid.Length);

            return new Schema.Persistence.SturdyRef() {
                TheTransient = new Schema.Persistence.SturdyRef.Transient(){
                    Vat = new Schema.Persistence.VatPath {
                        Id = new Schema.Persistence.VatId {
                            PublicKey0 = BitConverter.ToUInt64(vid, 0),
                            PublicKey1 = BitConverter.ToUInt64(vid, 8),
                            PublicKey2 = BitConverter.ToUInt64(vid, 16),
                            PublicKey3 = BitConverter.ToUInt64(vid, 24)
                        },
                        Address = new Schema.Persistence.Address {
                            Host = TcpHost,
                            Port = TcpPort
                        }
                    },
                    LocalRef = srToken
                }
            };
        }

        public void Unsave(string srToken) {
            _srToken2Capability.TryRemove(srToken, out _);
        }

        public void AddOrUpdateCrossDomainRestore(P.VatId vatId, P.IRestorer restorer) {
            _vatId2Restorer.AddOrUpdate(
                (vatId.PublicKey0, vatId.PublicKey1, vatId.PublicKey2, vatId.PublicKey3), 
                (k) => restorer, (k,oldRestorer) => { oldRestorer?.Dispose(); return restorer; });
        }


        #region implementation of Mas.Schema.Persistence.Restorer
        public async Task<BareProxy> Restore(Mas.Schema.Persistence.Restorer.RestoreParams ps, 
            CancellationToken cancellationToken_ = default) {
            var ds = (Capnp.DeserializerState)ps.LocalRef; 
            var srToken = Capnp.CapnpSerializable.Create<string>(ds);
            //var sealedFor = ps.SealedFor;

            // is cross domain restore? then always query the remote restorer
            if (_extSRT2VatIdAndIntSRT.ContainsKey(srToken)){
                var (vatId, intSRT) = _extSRT2VatIdAndIntSRT[srToken];
                if (_vatId2Restorer.TryGetValue(vatId, out var restorer)) {
                    return await restorer.Restore(new Mas.Schema.Persistence.Restorer.RestoreParams{ LocalRef=intSRT});
                }
            }

            // no remote restorer for cross domain available or just a normal restore
            if (_srToken2Capability.ContainsKey(srToken)) {
                var cap = _srToken2Capability[srToken];
                if (cap is BareProxy bareProxy) {
                    return Proxy.Share(bareProxy);
                } else {
                    var sharedProxy = Proxy.Share(cap);
                    var bareProxy2 = new BareProxy(sharedProxy.ConsumedCap);
                    return bareProxy2;// Proxy.Share(_srToken2Proxy[srToken]));
                }
            }
            return null;
        }
        #endregion

        public void Dispose() {
            // dispose sturdy ref caps
            //foreach (var sr2p in _srToken2Capability)
            //    sr2p.Value?.Dispose();

            Console.WriteLine("Dispose");
        }
    }
}

