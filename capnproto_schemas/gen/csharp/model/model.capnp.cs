using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Schema.Model
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x851d47c6ccdecf08UL)]
    public class XYResult : ICapnpSerializable
    {
        public const UInt64 typeId = 0x851d47c6ccdecf08UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Xs = reader.Xs;
            Ys = reader.Ys;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Xs.Init(Xs);
            writer.Ys.Init(Ys);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public IReadOnlyList<double> Xs
        {
            get;
            set;
        }

        public IReadOnlyList<double> Ys
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
            public IReadOnlyList<double> Xs => ctx.ReadList(0).CastDouble();
            public bool HasXs => ctx.IsStructFieldNonNull(0);
            public IReadOnlyList<double> Ys => ctx.ReadList(1).CastDouble();
            public bool HasYs => ctx.IsStructFieldNonNull(1);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 2);
            }

            public ListOfPrimitivesSerializer<double> Xs
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(0);
                set => Link(0, value);
            }

            public ListOfPrimitivesSerializer<double> Ys
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(1);
                set => Link(1, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa6be2e805ea10a68UL)]
    public class Stat : ICapnpSerializable
    {
        public const UInt64 typeId = 0xa6be2e805ea10a68UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            TheType = reader.TheType;
            Vs = reader.Vs;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.TheType = TheType;
            writer.Vs.Init(Vs);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Schema.Model.Stat.Type TheType
        {
            get;
            set;
        }

        = Mas.Schema.Model.Stat.Type.avg;
        public IReadOnlyList<double> Vs
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
            public Mas.Schema.Model.Stat.Type TheType => (Mas.Schema.Model.Stat.Type)ctx.ReadDataUShort(0UL, (ushort)3);
            public IReadOnlyList<double> Vs => ctx.ReadList(0).CastDouble();
            public bool HasVs => ctx.IsStructFieldNonNull(0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 1);
            }

            public Mas.Schema.Model.Stat.Type TheType
            {
                get => (Mas.Schema.Model.Stat.Type)this.ReadDataUShort(0UL, (ushort)3);
                set => this.WriteData(0UL, (ushort)value, (ushort)3);
            }

            public ListOfPrimitivesSerializer<double> Vs
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(0);
                set => Link(0, value);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbd33bb6d8cbd9ed2UL)]
        public enum Type : ushort
        {
            min,
            max,
            sd,
            avg,
            median
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8f86b66260d02d1dUL)]
    public class XYPlusResult : ICapnpSerializable
    {
        public const UInt64 typeId = 0x8f86b66260d02d1dUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Xy = CapnpSerializable.Create<Mas.Schema.Model.XYResult>(reader.Xy);
            Stats = reader.Stats?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Model.Stat>(_));
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            Xy?.serialize(writer.Xy);
            writer.Stats.Init(Stats, (_s1, _v1) => _v1?.serialize(_s1));
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Schema.Model.XYResult Xy
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Schema.Model.Stat> Stats
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
            public Mas.Schema.Model.XYResult.READER Xy => ctx.ReadStruct(0, Mas.Schema.Model.XYResult.READER.create);
            public bool HasXy => ctx.IsStructFieldNonNull(0);
            public IReadOnlyList<Mas.Schema.Model.Stat.READER> Stats => ctx.ReadList(1).Cast(Mas.Schema.Model.Stat.READER.create);
            public bool HasStats => ctx.IsStructFieldNonNull(1);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 2);
            }

            public Mas.Schema.Model.XYResult.WRITER Xy
            {
                get => BuildPointer<Mas.Schema.Model.XYResult.WRITER>(0);
                set => Link(0, value);
            }

            public ListOfStructsSerializer<Mas.Schema.Model.Stat.WRITER> Stats
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Model.Stat.WRITER>>(1);
                set => Link(1, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdfcfeb783c4948fcUL), Proxy(typeof(ClimateInstance_Proxy)), Skeleton(typeof(ClimateInstance_Skeleton))]
    public interface IClimateInstance : Mas.Schema.Common.IIdentifiable
    {
        Task<Mas.Schema.Model.XYResult> Run(Mas.Schema.Climate.ITimeSeries timeSeries, CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Model.XYPlusResult> RunSet(IReadOnlyList<Mas.Schema.Climate.ITimeSeries> dataset, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdfcfeb783c4948fcUL)]
    public class ClimateInstance_Proxy : Proxy, IClimateInstance
    {
        public async Task<Mas.Schema.Model.XYResult> Run(Mas.Schema.Climate.ITimeSeries timeSeries, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Model.ClimateInstance.Params_Run.WRITER>();
            var arg_ = new Mas.Schema.Model.ClimateInstance.Params_Run()
            {TimeSeries = timeSeries};
            arg_?.serialize(in_);
            using (var d_ = await Call(16127367692277074172UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Model.ClimateInstance.Result_Run>(d_);
                return (r_.Result);
            }
        }

        public async Task<Mas.Schema.Model.XYPlusResult> RunSet(IReadOnlyList<Mas.Schema.Climate.ITimeSeries> dataset, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Model.ClimateInstance.Params_RunSet.WRITER>();
            var arg_ = new Mas.Schema.Model.ClimateInstance.Params_RunSet()
            {Dataset = dataset};
            arg_?.serialize(in_);
            using (var d_ = await Call(16127367692277074172UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Model.ClimateInstance.Result_RunSet>(d_);
                return (r_.Result);
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdfcfeb783c4948fcUL)]
    public class ClimateInstance_Skeleton : Skeleton<IClimateInstance>
    {
        public ClimateInstance_Skeleton()
        {
            SetMethodTable(Run, RunSet);
        }

        public override ulong InterfaceId => 16127367692277074172UL;
        Task<AnswerOrCounterquestion> Run(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Model.ClimateInstance.Params_Run>(d_);
                return Impatient.MaybeTailCall(Impl.Run(in_.TimeSeries, cancellationToken_), result =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Model.ClimateInstance.Result_Run.WRITER>();
                    var r_ = new Mas.Schema.Model.ClimateInstance.Result_Run{Result = result};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> RunSet(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Model.ClimateInstance.Params_RunSet>(d_);
                return Impatient.MaybeTailCall(Impl.RunSet(in_.Dataset, cancellationToken_), result =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Model.ClimateInstance.Result_RunSet.WRITER>();
                    var r_ = new Mas.Schema.Model.ClimateInstance.Result_RunSet{Result = result};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class ClimateInstance
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdf787fd9d51f235bUL)]
        public class Params_Run : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdf787fd9d51f235bUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                TimeSeries = reader.TimeSeries;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.TimeSeries = TimeSeries;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Climate.ITimeSeries TimeSeries
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
                public Mas.Schema.Climate.ITimeSeries TimeSeries => ctx.ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Climate.ITimeSeries TimeSeries
                {
                    get => ReadCap<Mas.Schema.Climate.ITimeSeries>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcc39e47cdead74c4UL)]
        public class Result_Run : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcc39e47cdead74c4UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Result = CapnpSerializable.Create<Mas.Schema.Model.XYResult>(reader.Result);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Result?.serialize(writer.Result);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Model.XYResult Result
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
                public Mas.Schema.Model.XYResult.READER Result => ctx.ReadStruct(0, Mas.Schema.Model.XYResult.READER.create);
                public bool HasResult => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Model.XYResult.WRITER Result
                {
                    get => BuildPointer<Mas.Schema.Model.XYResult.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaa9d146226037822UL)]
        public class Params_RunSet : ICapnpSerializable
        {
            public const UInt64 typeId = 0xaa9d146226037822UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Dataset = reader.Dataset;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Dataset.Init(Dataset);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Schema.Climate.ITimeSeries> Dataset
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
                public IReadOnlyList<Mas.Schema.Climate.ITimeSeries> Dataset => ctx.ReadCapList<Mas.Schema.Climate.ITimeSeries>(0);
                public bool HasDataset => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfCapsSerializer<Mas.Schema.Climate.ITimeSeries> Dataset
                {
                    get => BuildPointer<ListOfCapsSerializer<Mas.Schema.Climate.ITimeSeries>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe22282cb3449bb4aUL)]
        public class Result_RunSet : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe22282cb3449bb4aUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Result = CapnpSerializable.Create<Mas.Schema.Model.XYPlusResult>(reader.Result);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Result?.serialize(writer.Result);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Model.XYPlusResult Result
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
                public Mas.Schema.Model.XYPlusResult.READER Result => ctx.ReadStruct(0, Mas.Schema.Model.XYPlusResult.READER.create);
                public bool HasResult => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Model.XYPlusResult.WRITER Result
                {
                    get => BuildPointer<Mas.Schema.Model.XYPlusResult.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb7fc866ef1127f7cUL)]
    public class Env<TRestInput> : ICapnpSerializable where TRestInput : class
    {
        public const UInt64 typeId = 0xb7fc866ef1127f7cUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Rest = CapnpSerializable.Create<TRestInput>(reader.Rest);
            TimeSeries = reader.TimeSeries;
            SoilProfile = CapnpSerializable.Create<Mas.Schema.Soil.Profile>(reader.SoilProfile);
            MgmtEvents = reader.MgmtEvents?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Management.Event>(_));
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Rest.SetObject(Rest);
            writer.TimeSeries = TimeSeries;
            SoilProfile?.serialize(writer.SoilProfile);
            writer.MgmtEvents.Init(MgmtEvents, (_s1, _v1) => _v1?.serialize(_s1));
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public TRestInput Rest
        {
            get;
            set;
        }

        public Mas.Schema.Climate.ITimeSeries TimeSeries
        {
            get;
            set;
        }

        public Mas.Schema.Soil.Profile SoilProfile
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Schema.Management.Event> MgmtEvents
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
            public DeserializerState Rest => ctx.StructReadPointer(0);
            public Mas.Schema.Climate.ITimeSeries TimeSeries => ctx.ReadCap<Mas.Schema.Climate.ITimeSeries>(1);
            public Mas.Schema.Soil.Profile.READER SoilProfile => ctx.ReadStruct(2, Mas.Schema.Soil.Profile.READER.create);
            public bool HasSoilProfile => ctx.IsStructFieldNonNull(2);
            public IReadOnlyList<Mas.Schema.Management.Event.READER> MgmtEvents => ctx.ReadList(3).Cast(Mas.Schema.Management.Event.READER.create);
            public bool HasMgmtEvents => ctx.IsStructFieldNonNull(3);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 4);
            }

            public DynamicSerializerState Rest
            {
                get => BuildPointer<DynamicSerializerState>(0);
                set => Link(0, value);
            }

            public Mas.Schema.Climate.ITimeSeries TimeSeries
            {
                get => ReadCap<Mas.Schema.Climate.ITimeSeries>(1);
                set => LinkObject(1, value);
            }

            public Mas.Schema.Soil.Profile.WRITER SoilProfile
            {
                get => BuildPointer<Mas.Schema.Soil.Profile.WRITER>(2);
                set => Link(2, value);
            }

            public ListOfStructsSerializer<Mas.Schema.Management.Event.WRITER> MgmtEvents
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Management.Event.WRITER>>(3);
                set => Link(3, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa5feedafa5ec5c4aUL), Proxy(typeof(EnvInstance_Proxy<, >)), Skeleton(typeof(EnvInstance_Skeleton<, >))]
    public interface IEnvInstance<TRestInput, TOutput> : Mas.Schema.Common.IIdentifiable, Mas.Schema.Persistence.IPersistent, Mas.Schema.Common.IStopable where TRestInput : class where TOutput : class
    {
        Task<TOutput> Run(Mas.Schema.Model.Env<TRestInput> env, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa5feedafa5ec5c4aUL)]
    public class EnvInstance_Proxy<TRestInput, TOutput> : Proxy, IEnvInstance<TRestInput, TOutput> where TRestInput : class where TOutput : class
    {
        public Task<TOutput> Run(Mas.Schema.Model.Env<TRestInput> env, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Model.EnvInstance<TRestInput, TOutput>.Params_Run.WRITER>();
            var arg_ = new Mas.Schema.Model.EnvInstance<TRestInput, TOutput>.Params_Run()
            {Env = env};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(11961258999001406538UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Model.EnvInstance<TRestInput, TOutput>.Result_Run>(d_);
                    return (r_.Result);
                }
            }

            );
        }

        public async Task Stop(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Stopable.Params_Stop.WRITER>();
            var arg_ = new Mas.Schema.Common.Stopable.Params_Stop()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(14879402799272964426UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Common.Stopable.Result_Stop>(d_);
                return;
            }
        }

        public async Task<(string, string)> Save(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.Persistent.Params_Save.WRITER>();
            var arg_ = new Mas.Schema.Persistence.Persistent.Params_Save()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(13954362354854972261UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Persistence.Persistent.Result_Save>(d_);
                return (r_.SturdyRef, r_.UnsaveSR);
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa5feedafa5ec5c4aUL)]
    public class EnvInstance_Skeleton<TRestInput, TOutput> : Skeleton<IEnvInstance<TRestInput, TOutput>> where TRestInput : class where TOutput : class
    {
        public EnvInstance_Skeleton()
        {
            SetMethodTable(Run);
        }

        public override ulong InterfaceId => 11961258999001406538UL;
        Task<AnswerOrCounterquestion> Run(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Model.EnvInstance<TRestInput, TOutput>.Params_Run>(d_);
                return Impatient.MaybeTailCall(Impl.Run(in_.Env, cancellationToken_), result =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Model.EnvInstance<TRestInput, TOutput>.Result_Run.WRITER>();
                    var r_ = new Mas.Schema.Model.EnvInstance<TRestInput, TOutput>.Result_Run{Result = result};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class EnvInstance<TRestInput, TOutput>
        where TRestInput : class where TOutput : class
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x811895634b6bd959UL)]
        public class Params_Run : ICapnpSerializable
        {
            public const UInt64 typeId = 0x811895634b6bd959UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Env = CapnpSerializable.Create<Mas.Schema.Model.Env<TRestInput>>(reader.Env);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Env?.serialize(writer.Env);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Model.Env<TRestInput> Env
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
                public Mas.Schema.Model.Env<TRestInput>.READER Env => ctx.ReadStruct(0, Mas.Schema.Model.Env<TRestInput>.READER.create);
                public bool HasEnv => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Model.Env<TRestInput>.WRITER Env
                {
                    get => BuildPointer<Mas.Schema.Model.Env<TRestInput>.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa931ae5cae90ece0UL)]
        public class Result_Run : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa931ae5cae90ece0UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Result = CapnpSerializable.Create<TOutput>(reader.Result);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Result.SetObject(Result);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public TOutput Result
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
                public DeserializerState Result => ctx.StructReadPointer(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public DynamicSerializerState Result
                {
                    get => BuildPointer<DynamicSerializerState>(0);
                    set => Link(0, value);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x87cbebfc1164a24aUL), Proxy(typeof(EnvInstanceProxy_Proxy<, >)), Skeleton(typeof(EnvInstanceProxy_Skeleton<, >))]
    public interface IEnvInstanceProxy<TRestInput, TOutput> : Mas.Schema.Model.IEnvInstance<TRestInput, TOutput> where TRestInput : class where TOutput : class
    {
        Task<Mas.Schema.Common.IAction> RegisterEnvInstance(Mas.Schema.Model.IEnvInstance<TRestInput, TOutput> instance, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x87cbebfc1164a24aUL)]
    public class EnvInstanceProxy_Proxy<TRestInput, TOutput> : Proxy, IEnvInstanceProxy<TRestInput, TOutput> where TRestInput : class where TOutput : class
    {
        public Task<Mas.Schema.Common.IAction> RegisterEnvInstance(Mas.Schema.Model.IEnvInstance<TRestInput, TOutput> instance, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Model.EnvInstanceProxy<TRestInput, TOutput>.Params_RegisterEnvInstance.WRITER>();
            var arg_ = new Mas.Schema.Model.EnvInstanceProxy<TRestInput, TOutput>.Params_RegisterEnvInstance()
            {Instance = instance};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(9785174083248628298UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Model.EnvInstanceProxy<TRestInput, TOutput>.Result_RegisterEnvInstance>(d_);
                    return (r_.Unregister);
                }
            }

            );
        }

        public Task<TOutput> Run(Mas.Schema.Model.Env<TRestInput> env, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Model.EnvInstance<TRestInput, TOutput>.Params_Run.WRITER>();
            var arg_ = new Mas.Schema.Model.EnvInstance<TRestInput, TOutput>.Params_Run()
            {Env = env};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(11961258999001406538UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Model.EnvInstance<TRestInput, TOutput>.Result_Run>(d_);
                    return (r_.Result);
                }
            }

            );
        }

        public async Task Stop(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Common.Stopable.Params_Stop.WRITER>();
            var arg_ = new Mas.Schema.Common.Stopable.Params_Stop()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(14879402799272964426UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Common.Stopable.Result_Stop>(d_);
                return;
            }
        }

        public async Task<(string, string)> Save(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.Persistent.Params_Save.WRITER>();
            var arg_ = new Mas.Schema.Persistence.Persistent.Params_Save()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(13954362354854972261UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Persistence.Persistent.Result_Save>(d_);
                return (r_.SturdyRef, r_.UnsaveSR);
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x87cbebfc1164a24aUL)]
    public class EnvInstanceProxy_Skeleton<TRestInput, TOutput> : Skeleton<IEnvInstanceProxy<TRestInput, TOutput>> where TRestInput : class where TOutput : class
    {
        public EnvInstanceProxy_Skeleton()
        {
            SetMethodTable(RegisterEnvInstance);
        }

        public override ulong InterfaceId => 9785174083248628298UL;
        Task<AnswerOrCounterquestion> RegisterEnvInstance(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Model.EnvInstanceProxy<TRestInput, TOutput>.Params_RegisterEnvInstance>(d_);
                return Impatient.MaybeTailCall(Impl.RegisterEnvInstance(in_.Instance, cancellationToken_), unregister =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Model.EnvInstanceProxy<TRestInput, TOutput>.Result_RegisterEnvInstance.WRITER>();
                    var r_ = new Mas.Schema.Model.EnvInstanceProxy<TRestInput, TOutput>.Result_RegisterEnvInstance{Unregister = unregister};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class EnvInstanceProxy<TRestInput, TOutput>
        where TRestInput : class where TOutput : class
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd10259a623f95bb4UL)]
        public class Params_RegisterEnvInstance : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd10259a623f95bb4UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Instance = reader.Instance;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Instance = Instance;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Model.IEnvInstance<TRestInput, TOutput> Instance
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
                public Mas.Schema.Model.IEnvInstance<TRestInput, TOutput> Instance => ctx.ReadCap<Mas.Schema.Model.IEnvInstance<TRestInput, TOutput>>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Model.IEnvInstance<TRestInput, TOutput> Instance
                {
                    get => ReadCap<Mas.Schema.Model.IEnvInstance<TRestInput, TOutput>>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdf50acfa56a9674eUL)]
        public class Result_RegisterEnvInstance : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdf50acfa56a9674eUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Unregister = reader.Unregister;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Unregister = Unregister;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Common.IAction Unregister
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
                public Mas.Schema.Common.IAction Unregister => ctx.ReadCap<Mas.Schema.Common.IAction>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Common.IAction Unregister
                {
                    get => ReadCap<Mas.Schema.Common.IAction>(0);
                    set => LinkObject(0, value);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xce552eef738a45eaUL), Proxy(typeof(InstanceFactory_Proxy<>)), Skeleton(typeof(InstanceFactory_Skeleton<>))]
    public interface IInstanceFactory<TInstanceType> : Mas.Schema.Common.IIdentifiable where TInstanceType : class
    {
        Task<Mas.Schema.Common.IdInformation> ModelInfo(CancellationToken cancellationToken_ = default);
        Task<TInstanceType> NewInstance(CancellationToken cancellationToken_ = default);
        Task<IReadOnlyList<Mas.Schema.Common.ListEntry<TInstanceType>>> NewInstances(short numberOfInstances, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xce552eef738a45eaUL)]
    public class InstanceFactory_Proxy<TInstanceType> : Proxy, IInstanceFactory<TInstanceType> where TInstanceType : class
    {
        public async Task<Mas.Schema.Common.IdInformation> ModelInfo(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Model.InstanceFactory<TInstanceType>.Params_ModelInfo.WRITER>();
            var arg_ = new Mas.Schema.Model.InstanceFactory<TInstanceType>.Params_ModelInfo()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(14867841350804063722UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Common.IdInformation>(d_);
                return r_;
            }
        }

        public Task<TInstanceType> NewInstance(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Model.InstanceFactory<TInstanceType>.Params_NewInstance.WRITER>();
            var arg_ = new Mas.Schema.Model.InstanceFactory<TInstanceType>.Params_NewInstance()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(14867841350804063722UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Model.InstanceFactory<TInstanceType>.Result_NewInstance>(d_);
                    return (r_.Instance);
                }
            }

            );
        }

        public async Task<IReadOnlyList<Mas.Schema.Common.ListEntry<TInstanceType>>> NewInstances(short numberOfInstances, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Model.InstanceFactory<TInstanceType>.Params_NewInstances.WRITER>();
            var arg_ = new Mas.Schema.Model.InstanceFactory<TInstanceType>.Params_NewInstances()
            {NumberOfInstances = numberOfInstances};
            arg_?.serialize(in_);
            using (var d_ = await Call(14867841350804063722UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Model.InstanceFactory<TInstanceType>.Result_NewInstances>(d_);
                return (r_.Instances);
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xce552eef738a45eaUL)]
    public class InstanceFactory_Skeleton<TInstanceType> : Skeleton<IInstanceFactory<TInstanceType>> where TInstanceType : class
    {
        public InstanceFactory_Skeleton()
        {
            SetMethodTable(ModelInfo, NewInstance, NewInstances);
        }

        public override ulong InterfaceId => 14867841350804063722UL;
        Task<AnswerOrCounterquestion> ModelInfo(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.ModelInfo(cancellationToken_), r_ =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Common.IdInformation.WRITER>();
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> NewInstance(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.NewInstance(cancellationToken_), instance =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Model.InstanceFactory<TInstanceType>.Result_NewInstance.WRITER>();
                    var r_ = new Mas.Schema.Model.InstanceFactory<TInstanceType>.Result_NewInstance{Instance = instance};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> NewInstances(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Model.InstanceFactory<TInstanceType>.Params_NewInstances>(d_);
                return Impatient.MaybeTailCall(Impl.NewInstances(in_.NumberOfInstances, cancellationToken_), instances =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Model.InstanceFactory<TInstanceType>.Result_NewInstances.WRITER>();
                    var r_ = new Mas.Schema.Model.InstanceFactory<TInstanceType>.Result_NewInstances{Instances = instances};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class InstanceFactory<TInstanceType>
        where TInstanceType : class
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbf49e08cc9412aafUL)]
        public class Params_ModelInfo : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbf49e08cc9412aafUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9ee4515395213845UL)]
        public class Params_NewInstance : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9ee4515395213845UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf013eda158070488UL)]
        public class Result_NewInstance : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf013eda158070488UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Instance = CapnpSerializable.Create<TInstanceType>(reader.Instance);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Instance.SetObject(Instance);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public TInstanceType Instance
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
                public DeserializerState Instance => ctx.StructReadPointer(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public DynamicSerializerState Instance
                {
                    get => BuildPointer<DynamicSerializerState>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd9fa9ece71d1db50UL)]
        public class Params_NewInstances : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd9fa9ece71d1db50UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                NumberOfInstances = reader.NumberOfInstances;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.NumberOfInstances = NumberOfInstances;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public short NumberOfInstances
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
                public short NumberOfInstances => ctx.ReadDataShort(0UL, (short)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public short NumberOfInstances
                {
                    get => this.ReadDataShort(0UL, (short)0);
                    set => this.WriteData(0UL, value, (short)0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaf9a1cb72ba68156UL)]
        public class Result_NewInstances : ICapnpSerializable
        {
            public const UInt64 typeId = 0xaf9a1cb72ba68156UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Instances = reader.Instances?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Common.ListEntry<TInstanceType>>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Instances.Init(Instances, (_s1, _v1) => _v1?.serialize(_s1));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Schema.Common.ListEntry<TInstanceType>> Instances
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
                public IReadOnlyList<Mas.Schema.Common.ListEntry<TInstanceType>.READER> Instances => ctx.ReadList(0).Cast(Mas.Schema.Common.ListEntry<TInstanceType>.READER.create);
                public bool HasInstances => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfStructsSerializer<Mas.Schema.Common.ListEntry<TInstanceType>.WRITER> Instances
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Common.ListEntry<TInstanceType>.WRITER>>(0);
                    set => Link(0, value);
                }
            }
        }
    }
}