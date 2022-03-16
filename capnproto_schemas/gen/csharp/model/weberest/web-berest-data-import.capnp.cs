using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Rpc
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa1a4ad9d143eaa6fUL), Proxy(typeof(DWLABImport_Proxy)), Skeleton(typeof(DWLABImport_Skeleton))]
    public interface IDWLABImport : IDisposable
    {
        Task<(string, bool, bool)> ImportData(string id, string dwla, string dwlb, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa1a4ad9d143eaa6fUL)]
    public class DWLABImport_Proxy : Proxy, IDWLABImport
    {
        public async Task<(string, bool, bool)> ImportData(string id, string dwla, string dwlb, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.DWLABImport.Params_ImportData.WRITER>();
            var arg_ = new Mas.Rpc.DWLABImport.Params_ImportData()
            {Id = id, Dwla = dwla, Dwlb = dwlb};
            arg_?.serialize(in_);
            using (var d_ = await Call(11647625426448067183UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.DWLABImport.Result_ImportData>(d_);
                return (r_.Id, r_.SuccessA, r_.SuccessB);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa1a4ad9d143eaa6fUL)]
    public class DWLABImport_Skeleton : Skeleton<IDWLABImport>
    {
        public DWLABImport_Skeleton()
        {
            SetMethodTable(ImportData);
        }

        public override ulong InterfaceId => 11647625426448067183UL;
        Task<AnswerOrCounterquestion> ImportData(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.DWLABImport.Params_ImportData>(d_);
                return Impatient.MaybeTailCall(Impl.ImportData(in_.Id, in_.Dwla, in_.Dwlb, cancellationToken_), (id, successA, successB) =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.DWLABImport.Result_ImportData.WRITER>();
                    var r_ = new Mas.Rpc.DWLABImport.Result_ImportData{Id = id, SuccessA = successA, SuccessB = successB};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class DWLABImport
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xeb03972caa23c7d2UL)]
        public class Params_ImportData : ICapnpSerializable
        {
            public const UInt64 typeId = 0xeb03972caa23c7d2UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Id = reader.Id;
                Dwla = reader.Dwla;
                Dwlb = reader.Dwlb;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Id = Id;
                writer.Dwla = Dwla;
                writer.Dwlb = Dwlb;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Id
            {
                get;
                set;
            }

            public string Dwla
            {
                get;
                set;
            }

            public string Dwlb
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
                public string Id => ctx.ReadText(0, null);
                public string Dwla => ctx.ReadText(1, null);
                public string Dwlb => ctx.ReadText(2, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 3);
                }

                public string Id
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public string Dwla
                {
                    get => this.ReadText(1, null);
                    set => this.WriteText(1, value, null);
                }

                public string Dwlb
                {
                    get => this.ReadText(2, null);
                    set => this.WriteText(2, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb9bc568c49fcca07UL)]
        public class Result_ImportData : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb9bc568c49fcca07UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Id = reader.Id;
                SuccessA = reader.SuccessA;
                SuccessB = reader.SuccessB;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Id = Id;
                writer.SuccessA = SuccessA;
                writer.SuccessB = SuccessB;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Id
            {
                get;
                set;
            }

            public bool SuccessA
            {
                get;
                set;
            }

            public bool SuccessB
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
                public string Id => ctx.ReadText(0, null);
                public bool SuccessA => ctx.ReadDataBool(0UL, false);
                public bool SuccessB => ctx.ReadDataBool(1UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public string Id
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public bool SuccessA
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }

                public bool SuccessB
                {
                    get => this.ReadDataBool(1UL, false);
                    set => this.WriteData(1UL, value, false);
                }
            }
        }
    }
}