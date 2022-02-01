using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Rpc.Test
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xba9eff6fb3abc84fUL), Proxy(typeof(A_Proxy)), Skeleton(typeof(A_Skeleton))]
    public interface IA : IDisposable
    {
        Task<string> Method(string @param, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xba9eff6fb3abc84fUL)]
    public class A_Proxy : Proxy, IA
    {
        public async Task<string> Method(string @param, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Test.A.Params_Method.WRITER>();
            var arg_ = new Mas.Rpc.Test.A.Params_Method()
            {Param = @param};
            arg_?.serialize(in_);
            using (var d_ = await Call(13447466392595712079UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Test.A.Result_Method>(d_);
                return (r_.Res);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xba9eff6fb3abc84fUL)]
    public class A_Skeleton : Skeleton<IA>
    {
        public A_Skeleton()
        {
            SetMethodTable(Method);
        }

        public override ulong InterfaceId => 13447466392595712079UL;
        Task<AnswerOrCounterquestion> Method(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.Test.A.Params_Method>(d_);
                return Impatient.MaybeTailCall(Impl.Method(in_.Param, cancellationToken_), res =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Test.A.Result_Method.WRITER>();
                    var r_ = new Mas.Rpc.Test.A.Result_Method{Res = res};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class A
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc506e9c0e16825f7UL)]
        public class Params_Method : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc506e9c0e16825f7UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Param = reader.Param;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Param = Param;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Param
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
                public string Param => ctx.ReadText(0, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Param
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9e2108f9306a75efUL)]
        public class Result_Method : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9e2108f9306a75efUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Res = reader.Res;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Res = Res;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Res
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
                public string Res => ctx.ReadText(0, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Res
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }
    }
}