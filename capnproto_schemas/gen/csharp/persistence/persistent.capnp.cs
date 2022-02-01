using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Capnp
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc8cb212fcd9f5691UL), Proxy(typeof(Persistent_Proxy<, >)), Skeleton(typeof(Persistent_Skeleton<, >))]
    public interface IPersistent<TSturdyRef, TOwner> : IDisposable where TSturdyRef : class where TOwner : class
    {
        Task<Capnp.Persistent<TSturdyRef, TOwner>.SaveResults> Save(Capnp.Persistent<TSturdyRef, TOwner>.SaveParams arg_, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc8cb212fcd9f5691UL)]
    public class Persistent_Proxy<TSturdyRef, TOwner> : Proxy, IPersistent<TSturdyRef, TOwner> where TSturdyRef : class where TOwner : class
    {
        public Task<Capnp.Persistent<TSturdyRef, TOwner>.SaveResults> Save(Capnp.Persistent<TSturdyRef, TOwner>.SaveParams arg_, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Capnp.Persistent<TSturdyRef, TOwner>.SaveParams.WRITER>();
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(14468694717054801553UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Capnp.Persistent<TSturdyRef, TOwner>.SaveResults>(d_);
                    return r_;
                }
            }

            );
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc8cb212fcd9f5691UL)]
    public class Persistent_Skeleton<TSturdyRef, TOwner> : Skeleton<IPersistent<TSturdyRef, TOwner>> where TSturdyRef : class where TOwner : class
    {
        public Persistent_Skeleton()
        {
            SetMethodTable(Save);
        }

        public override ulong InterfaceId => 14468694717054801553UL;
        Task<AnswerOrCounterquestion> Save(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
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
    }

    public static class Persistent<TSturdyRef, TOwner>
        where TSturdyRef : class where TOwner : class
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf76fba59183073a5UL)]
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb76848c18c40efbfUL)]
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
}