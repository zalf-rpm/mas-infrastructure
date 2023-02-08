using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Schema.Crop
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
    public interface ICrop : Mas.Schema.Common.IIdentifiable
    {
        Task<object> Parameters(CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Common.IdInformation> Cultivar(CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Common.IdInformation> Species(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe88d97a324bf5c84UL)]
    public class Crop_Proxy : Proxy, ICrop
    {
        public Task<object> Parameters(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Crop.Crop.Params_Parameters.WRITER>();
            var arg_ = new Mas.Schema.Crop.Crop.Params_Parameters()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(16757216515467467908UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Crop.Crop.Result_Parameters>(d_);
                    return (r_.Params);
                }
            }

            );
        }

        public async Task<Mas.Schema.Common.IdInformation> Cultivar(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Crop.Crop.Params_Cultivar.WRITER>();
            var arg_ = new Mas.Schema.Crop.Crop.Params_Cultivar()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(16757216515467467908UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Crop.Crop.Result_Cultivar>(d_);
                return (r_.Info);
            }
        }

        public async Task<Mas.Schema.Common.IdInformation> Species(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Crop.Crop.Params_Species.WRITER>();
            var arg_ = new Mas.Schema.Crop.Crop.Params_Species()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(16757216515467467908UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Crop.Crop.Result_Species>(d_);
                return (r_.Info);
            }
        }

        public async Task<Mas.Schema.Common.IdInformation> Info(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Identifiable.Params_Info.WRITER>();
            var arg_ = new Mas.Schema.Common.Identifiable.Params_Info()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(12875740530987518165UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Common.IdInformation>(d_);
                return r_;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe88d97a324bf5c84UL)]
    public class Crop_Skeleton : Skeleton<ICrop>
    {
        public Crop_Skeleton()
        {
            SetMethodTable(Parameters, Cultivar, Species);
        }

        public override ulong InterfaceId => 16757216515467467908UL;
        Task<AnswerOrCounterquestion> Parameters(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Parameters(cancellationToken_), @params =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Crop.Crop.Result_Parameters.WRITER>();
                    var r_ = new Mas.Schema.Crop.Crop.Result_Parameters{Params = @params};
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
                return Impatient.MaybeTailCall(Impl.Cultivar(cancellationToken_), info =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Crop.Crop.Result_Cultivar.WRITER>();
                    var r_ = new Mas.Schema.Crop.Crop.Result_Cultivar{Info = info};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> Species(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Species(cancellationToken_), info =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Crop.Crop.Result_Species.WRITER>();
                    var r_ = new Mas.Schema.Crop.Crop.Result_Species{Info = info};
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
                Info = CapnpSerializable.Create<Mas.Schema.Common.IdInformation>(reader.Info);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Info?.serialize(writer.Info);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Common.IdInformation Info
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
                public Mas.Schema.Common.IdInformation.READER Info => ctx.ReadStruct(0, Mas.Schema.Common.IdInformation.READER.create);
                public bool HasInfo => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Common.IdInformation.WRITER Info
                {
                    get => BuildPointer<Mas.Schema.Common.IdInformation.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf4dd1c322a3130b4UL)]
        public class Params_Species : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf4dd1c322a3130b4UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb4aa895eeede6448UL)]
        public class Result_Species : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb4aa895eeede6448UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Info = CapnpSerializable.Create<Mas.Schema.Common.IdInformation>(reader.Info);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Info?.serialize(writer.Info);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Common.IdInformation Info
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
                public Mas.Schema.Common.IdInformation.READER Info => ctx.ReadStruct(0, Mas.Schema.Common.IdInformation.READER.create);
                public bool HasInfo => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Common.IdInformation.WRITER Info
                {
                    get => BuildPointer<Mas.Schema.Common.IdInformation.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }
    }
}