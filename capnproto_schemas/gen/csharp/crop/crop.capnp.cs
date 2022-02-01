using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Rpc.Crop
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdd81b0520864e2b4UL)]
    public enum Cultivar : ushort
    {
        alfalfaClovergrassLeyMix,
        alfalfa,
        bacharia,
        barleySpring,
        barleyWinter,
        cloverGrassLey,
        cottonBrMid,
        cottonLong,
        cottonMid,
        cottonShort,
        einkorn,
        emmer,
        fieldPea24,
        fieldPea26,
        grapevine,
        maizeGrain,
        maizeSilage,
        mustard,
        oatCompound,
        oilRadish,
        phacelia,
        potatoModeratelyEarly,
        rapeWinter,
        ryeGrass,
        ryeSilageWinter,
        ryeSpring,
        ryeWinter,
        sorghum,
        soybean0,
        soybean00,
        soybean000,
        soybean0000,
        soybeanI,
        soybeanII,
        soybeanIII,
        soybeanIV,
        soybeanV,
        soybeanVI,
        soybeanVII,
        soybeanVIII,
        soybeanIX,
        soybeanX,
        soybeanXI,
        soybeanXII,
        sudanGrass,
        sugarBeet,
        sugarcaneTransplant,
        sugarcaneRatoon,
        tomatoField,
        triticaleSpring,
        triticaleWinter,
        wheatDurum,
        wheatSpring,
        wheatWinter
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe88d97a324bf5c84UL), Proxy(typeof(Crop_Proxy)), Skeleton(typeof(Crop_Skeleton))]
    public interface ICrop : Mas.Rpc.Common.IIdentifiable
    {
        Task<object> Parameters(CancellationToken cancellationToken_ = default);
        Task<Mas.Rpc.Crop.Cultivar> Cultivar(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe88d97a324bf5c84UL)]
    public class Crop_Proxy : Proxy, ICrop
    {
        public Task<object> Parameters(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Crop.Crop.Params_Parameters.WRITER>();
            var arg_ = new Mas.Rpc.Crop.Crop.Params_Parameters()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(16757216515467467908UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Crop.Crop.Result_Parameters>(d_);
                    return (r_.Params);
                }
            }

            );
        }

        public async Task<Mas.Rpc.Crop.Cultivar> Cultivar(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Crop.Crop.Params_Cultivar.WRITER>();
            var arg_ = new Mas.Rpc.Crop.Crop.Params_Cultivar()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(16757216515467467908UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Crop.Crop.Result_Cultivar>(d_);
                return (r_.Cult);
            }
        }

        public async Task<Mas.Rpc.Common.IdInformation> Info(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Identifiable.Params_Info.WRITER>();
            var arg_ = new Mas.Rpc.Common.Identifiable.Params_Info()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(12875740530987518165UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.IdInformation>(d_);
                return r_;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe88d97a324bf5c84UL)]
    public class Crop_Skeleton : Skeleton<ICrop>
    {
        public Crop_Skeleton()
        {
            SetMethodTable(Parameters, Cultivar);
        }

        public override ulong InterfaceId => 16757216515467467908UL;
        Task<AnswerOrCounterquestion> Parameters(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Parameters(cancellationToken_), @params =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Crop.Crop.Result_Parameters.WRITER>();
                    var r_ = new Mas.Rpc.Crop.Crop.Result_Parameters{Params = @params};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> Cultivar(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Cultivar(cancellationToken_), cult =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Crop.Crop.Result_Cultivar.WRITER>();
                    var r_ = new Mas.Rpc.Crop.Crop.Result_Cultivar{Cult = cult};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Crop
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc86e010e743c8e5bUL)]
        public class Params_Parameters : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc86e010e743c8e5bUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe4fafc722d515486UL)]
        public class Result_Parameters : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe4fafc722d515486UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Params = CapnpSerializable.Create<object>(reader.Params);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Params.SetObject(Params);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public object Params
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
                public DeserializerState Params => ctx.StructReadPointer(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public DynamicSerializerState Params
                {
                    get => BuildPointer<DynamicSerializerState>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf26ef117dfb4517aUL)]
        public class Params_Cultivar : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf26ef117dfb4517aUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbf3704bba52494baUL)]
        public class Result_Cultivar : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbf3704bba52494baUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Cult = reader.Cult;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Cult = Cult;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Rpc.Crop.Cultivar Cult
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
                public Mas.Rpc.Crop.Cultivar Cult => (Mas.Rpc.Crop.Cultivar)ctx.ReadDataUShort(0UL, (ushort)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public Mas.Rpc.Crop.Cultivar Cult
                {
                    get => (Mas.Rpc.Crop.Cultivar)this.ReadDataUShort(0UL, (ushort)0);
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }
            }
        }
    }
}