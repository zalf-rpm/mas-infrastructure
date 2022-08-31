using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Schema.Config
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x860d660620aefcdaUL), Proxy(typeof(Service_Proxy<>)), Skeleton(typeof(Service_Skeleton<>))]
    public interface IService<TC> : IDisposable where TC : class
    {
        Task<(TC, bool)> NextConfig(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x860d660620aefcdaUL)]
    public class Service_Proxy<TC> : Proxy, IService<TC> where TC : class
    {
        public Task<(TC, bool)> NextConfig(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Config.Service<TC>.Params_NextConfig.WRITER>();
            var arg_ = new Mas.Schema.Config.Service<TC>.Params_NextConfig()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(9659488952283757786UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Config.Service<TC>.Result_NextConfig>(d_);
                    return (r_.Config, r_.NoFurtherConfigs);
                }
            }

            );
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x860d660620aefcdaUL)]
    public class Service_Skeleton<TC> : Skeleton<IService<TC>> where TC : class
    {
        public Service_Skeleton()
        {
            SetMethodTable(NextConfig);
        }

        public override ulong InterfaceId => 9659488952283757786UL;
        Task<AnswerOrCounterquestion> NextConfig(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.NextConfig(cancellationToken_), (config, noFurtherConfigs) =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Config.Service<TC>.Result_NextConfig.WRITER>();
                    var r_ = new Mas.Schema.Config.Service<TC>.Result_NextConfig{Config = config, NoFurtherConfigs = noFurtherConfigs};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Service<TC>
        where TC : class
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8a931778446b73d8UL)]
        public class Params_NextConfig : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8a931778446b73d8UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
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
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb0cc157dd72bb20bUL)]
        public class Result_NextConfig : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb0cc157dd72bb20bUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Config = CapnpSerializable.Create<TC>(reader.Config);
                NoFurtherConfigs = reader.NoFurtherConfigs;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Config.SetObject(Config);
                writer.NoFurtherConfigs = NoFurtherConfigs;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public TC Config
            {
                get;
                set;
            }

            public bool NoFurtherConfigs
            {
                get;
                set;
            }

            = false;
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
                public DeserializerState Config => ctx.StructReadPointer(0);
                public bool NoFurtherConfigs => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public DynamicSerializerState Config
                {
                    get => BuildPointer<DynamicSerializerState>(0);
                    set => Link(0, value);
                }

                public bool NoFurtherConfigs
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }
    }
}