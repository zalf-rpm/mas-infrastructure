using Capnp;
using Capnp.Rpc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Capnp
{
    [TypeId(0xc8cb212fcd9f5691UL), Proxy(typeof(Persistent_Proxy<, >)), Skeleton(typeof(Persistent_Skeleton<, >))]
    public interface IPersistent<TSturdyRef, TOwner> : IDisposable where TSturdyRef : class where TOwner : class
    {
        Task<Capnp.Persistent<TSturdyRef, TOwner>.SaveResults> Save(Capnp.Persistent<TSturdyRef, TOwner>.SaveParams arg_, CancellationToken cancellationToken_ = default);
    }

    public class Persistent_Proxy<TSturdyRef, TOwner> : Proxy, IPersistent<TSturdyRef, TOwner> where TSturdyRef : class where TOwner : class
    {
        public Task<Capnp.Persistent<TSturdyRef, TOwner>.SaveResults> Save(Capnp.Persistent<TSturdyRef, TOwner>.SaveParams arg_, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Capnp.Persistent<TSturdyRef, TOwner>.SaveParams.WRITER>();
            arg_.serialize(in_);
            return Impatient.MakePipelineAware(Call(14468694717054801553UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                var r_ = CapnpSerializable.Create<Capnp.Persistent<TSturdyRef, TOwner>.SaveResults>(d_);
                return r_;
            }

            );
        }
    }

    public class Persistent_Skeleton<TSturdyRef, TOwner> : Skeleton<IPersistent<TSturdyRef, TOwner>> where TSturdyRef : class where TOwner : class
    {
        public Persistent_Skeleton()
        {
            SetMethodTable(Save);
        }

        public override ulong InterfaceId => 14468694717054801553UL;
        Task<AnswerOrCounterquestion> Save(DeserializerState d_, CancellationToken cancellationToken_)
        {
            return Impatient.MaybeTailCall(Impl.Save(CapnpSerializable.Create<Capnp.Persistent<TSturdyRef, TOwner>.SaveParams>(d_), cancellationToken_), r_ =>
            {
                var s_ = SerializerState.CreateForRpc<Capnp.Persistent<TSturdyRef, TOwner>.SaveResults.WRITER>();
                r_.serialize(s_);
                return s_;
            }

            );
        }
    }

    public static class Persistent<TSturdyRef, TOwner>
        where TSturdyRef : class where TOwner : class
    {
        [TypeId(0xf76fba59183073a5UL)]
        public class SaveParams : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf76fba59183073a5UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                SealFor = CapnpSerializable.Create<TOwner>(reader.SealFor);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.SealFor.SetObject(SealFor);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public TOwner SealFor
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public DeserializerState SealFor => ctx.StructReadPointer(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public DynamicSerializerState SealFor
                {
                    get => BuildPointer<DynamicSerializerState>(0);
                    set => Link(0, value);
                }
            }
        }

        [TypeId(0xb76848c18c40efbfUL)]
        public class SaveResults : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb76848c18c40efbfUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                SturdyRef = CapnpSerializable.Create<TSturdyRef>(reader.SturdyRef);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.SturdyRef.SetObject(SturdyRef);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public TSturdyRef SturdyRef
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public DeserializerState SturdyRef => ctx.StructReadPointer(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public DynamicSerializerState SturdyRef
                {
                    get => BuildPointer<DynamicSerializerState>(0);
                    set => Link(0, value);
                }
            }
        }
    }

    [TypeId(0x84ff286cd00a3ed4UL), Proxy(typeof(RealmGateway_Proxy<,,, >)), Skeleton(typeof(RealmGateway_Skeleton<,,, >))]
    public interface IRealmGateway<TInternalRef, TExternalRef, TInternalOwner, TExternalOwner> : IDisposable where TInternalRef : class where TExternalRef : class where TInternalOwner : class where TExternalOwner : class
    {
        Task<Capnp.Persistent<TInternalRef, TInternalOwner>.SaveResults> Import(Capnp.IPersistent<TExternalRef, TExternalOwner> cap, Capnp.Persistent<TInternalRef, TInternalOwner>.SaveParams @params, CancellationToken cancellationToken_ = default);
        Task<Capnp.Persistent<TExternalRef, TExternalOwner>.SaveResults> Export(Capnp.IPersistent<TInternalRef, TInternalOwner> cap, Capnp.Persistent<TExternalRef, TExternalOwner>.SaveParams @params, CancellationToken cancellationToken_ = default);
    }

    public class RealmGateway_Proxy<TInternalRef, TExternalRef, TInternalOwner, TExternalOwner> : Proxy, IRealmGateway<TInternalRef, TExternalRef, TInternalOwner, TExternalOwner> where TInternalRef : class where TExternalRef : class where TInternalOwner : class where TExternalOwner : class
    {
        public Task<Capnp.Persistent<TInternalRef, TInternalOwner>.SaveResults> Import(Capnp.IPersistent<TExternalRef, TExternalOwner> cap, Capnp.Persistent<TInternalRef, TInternalOwner>.SaveParams @params, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Capnp.RealmGateway<TInternalRef, TExternalRef, TInternalOwner, TExternalOwner>.Params_import.WRITER>();
            var arg_ = new Capnp.RealmGateway<TInternalRef, TExternalRef, TInternalOwner, TExternalOwner>.Params_import()
            {Cap = cap, Params = @params};
            arg_.serialize(in_);
            return Impatient.MakePipelineAware(Call(9583422979879616212UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                var r_ = CapnpSerializable.Create<Capnp.Persistent<TInternalRef, TInternalOwner>.SaveResults>(d_);
                return r_;
            }

            );
        }

        public Task<Capnp.Persistent<TExternalRef, TExternalOwner>.SaveResults> Export(Capnp.IPersistent<TInternalRef, TInternalOwner> cap, Capnp.Persistent<TExternalRef, TExternalOwner>.SaveParams @params, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Capnp.RealmGateway<TInternalRef, TExternalRef, TInternalOwner, TExternalOwner>.Params_export.WRITER>();
            var arg_ = new Capnp.RealmGateway<TInternalRef, TExternalRef, TInternalOwner, TExternalOwner>.Params_export()
            {Cap = cap, Params = @params};
            arg_.serialize(in_);
            return Impatient.MakePipelineAware(Call(9583422979879616212UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                var r_ = CapnpSerializable.Create<Capnp.Persistent<TExternalRef, TExternalOwner>.SaveResults>(d_);
                return r_;
            }

            );
        }
    }

    public class RealmGateway_Skeleton<TInternalRef, TExternalRef, TInternalOwner, TExternalOwner> : Skeleton<IRealmGateway<TInternalRef, TExternalRef, TInternalOwner, TExternalOwner>> where TInternalRef : class where TExternalRef : class where TInternalOwner : class where TExternalOwner : class
    {
        public RealmGateway_Skeleton()
        {
            SetMethodTable(Import, Export);
        }

        public override ulong InterfaceId => 9583422979879616212UL;
        Task<AnswerOrCounterquestion> Import(DeserializerState d_, CancellationToken cancellationToken_)
        {
            var in_ = CapnpSerializable.Create<Capnp.RealmGateway<TInternalRef, TExternalRef, TInternalOwner, TExternalOwner>.Params_import>(d_);
            return Impatient.MaybeTailCall(Impl.Import(in_.Cap, in_.Params, cancellationToken_), r_ =>
            {
                var s_ = SerializerState.CreateForRpc<Capnp.Persistent<TInternalRef, TInternalOwner>.SaveResults.WRITER>();
                r_.serialize(s_);
                return s_;
            }

            );
        }

        Task<AnswerOrCounterquestion> Export(DeserializerState d_, CancellationToken cancellationToken_)
        {
            var in_ = CapnpSerializable.Create<Capnp.RealmGateway<TInternalRef, TExternalRef, TInternalOwner, TExternalOwner>.Params_export>(d_);
            return Impatient.MaybeTailCall(Impl.Export(in_.Cap, in_.Params, cancellationToken_), r_ =>
            {
                var s_ = SerializerState.CreateForRpc<Capnp.Persistent<TExternalRef, TExternalOwner>.SaveResults.WRITER>();
                r_.serialize(s_);
                return s_;
            }

            );
        }
    }

    public static class RealmGateway<TInternalRef, TExternalRef, TInternalOwner, TExternalOwner>
        where TInternalRef : class where TExternalRef : class where TInternalOwner : class where TExternalOwner : class
    {
        [TypeId(0xf0c2cc1d3909574dUL)]
        public class Params_import : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf0c2cc1d3909574dUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Cap = reader.Cap;
                Params = CapnpSerializable.Create<Capnp.Persistent<TInternalRef, TInternalOwner>.SaveParams>(reader.Params);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Cap = Cap;
                Params?.serialize(writer.Params);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Capnp.IPersistent<TExternalRef, TExternalOwner> Cap
            {
                get;
                set;
            }

            public Capnp.Persistent<TInternalRef, TInternalOwner>.SaveParams Params
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public Capnp.IPersistent<TExternalRef, TExternalOwner> Cap => ctx.ReadCap<Capnp.IPersistent<TExternalRef, TExternalOwner>>(0);
                public Capnp.Persistent<TInternalRef, TInternalOwner>.SaveParams.READER Params => ctx.ReadStruct(1, Capnp.Persistent<TInternalRef, TInternalOwner>.SaveParams.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public Capnp.IPersistent<TExternalRef, TExternalOwner> Cap
                {
                    get => ReadCap<Capnp.IPersistent<TExternalRef, TExternalOwner>>(0);
                    set => LinkObject(0, value);
                }

                public Capnp.Persistent<TInternalRef, TInternalOwner>.SaveParams.WRITER Params
                {
                    get => BuildPointer<Capnp.Persistent<TInternalRef, TInternalOwner>.SaveParams.WRITER>(1);
                    set => Link(1, value);
                }
            }
        }

        [TypeId(0xecafa18b482da3aaUL)]
        public class Params_export : ICapnpSerializable
        {
            public const UInt64 typeId = 0xecafa18b482da3aaUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Cap = reader.Cap;
                Params = CapnpSerializable.Create<Capnp.Persistent<TExternalRef, TExternalOwner>.SaveParams>(reader.Params);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Cap = Cap;
                Params?.serialize(writer.Params);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Capnp.IPersistent<TInternalRef, TInternalOwner> Cap
            {
                get;
                set;
            }

            public Capnp.Persistent<TExternalRef, TExternalOwner>.SaveParams Params
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public Capnp.IPersistent<TInternalRef, TInternalOwner> Cap => ctx.ReadCap<Capnp.IPersistent<TInternalRef, TInternalOwner>>(0);
                public Capnp.Persistent<TExternalRef, TExternalOwner>.SaveParams.READER Params => ctx.ReadStruct(1, Capnp.Persistent<TExternalRef, TExternalOwner>.SaveParams.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public Capnp.IPersistent<TInternalRef, TInternalOwner> Cap
                {
                    get => ReadCap<Capnp.IPersistent<TInternalRef, TInternalOwner>>(0);
                    set => LinkObject(0, value);
                }

                public Capnp.Persistent<TExternalRef, TExternalOwner>.SaveParams.WRITER Params
                {
                    get => BuildPointer<Capnp.Persistent<TExternalRef, TExternalOwner>.SaveParams.WRITER>(1);
                    set => Link(1, value);
                }
            }
        }
    }
}