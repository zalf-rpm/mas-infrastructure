using Capnp;
using Capnp.Rpc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Rpc
{
    [TypeId(0xff5f920dca70d17fUL)]
    public class Model : ICapnpSerializable
    {
        public const UInt64 typeId = 0xff5f920dca70d17fUL;
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

        [TypeId(0xab60bf36005748daUL)]
        public class XYResult : ICapnpSerializable
        {
            public const UInt64 typeId = 0xab60bf36005748daUL;
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
                public IReadOnlyList<double> Ys => ctx.ReadList(1).CastDouble();
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

        [TypeId(0xc00c839c28a0da58UL)]
        public class Stat : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc00c839c28a0da58UL;
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

            public Mas.Rpc.Model.Stat.Type TheType
            {
                get;
                set;
            }

            = Mas.Rpc.Model.Stat.Type.avg;
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
                public Mas.Rpc.Model.Stat.Type TheType => (Mas.Rpc.Model.Stat.Type)ctx.ReadDataUShort(0UL, (ushort)3);
                public IReadOnlyList<double> Vs => ctx.ReadList(0).CastDouble();
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public Mas.Rpc.Model.Stat.Type TheType
                {
                    get => (Mas.Rpc.Model.Stat.Type)this.ReadDataUShort(0UL, (ushort)3);
                    set => this.WriteData(0UL, (ushort)value, (ushort)3);
                }

                public ListOfPrimitivesSerializer<double> Vs
                {
                    get => BuildPointer<ListOfPrimitivesSerializer<double>>(0);
                    set => Link(0, value);
                }
            }

            [TypeId(0xb85895c5190b15ccUL)]
            public enum Type : ushort
            {
                min,
                max,
                sd,
                avg,
                median
            }
        }

        [TypeId(0xef890f60e530cfaaUL)]
        public class XYPlusResult : ICapnpSerializable
        {
            public const UInt64 typeId = 0xef890f60e530cfaaUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Xy = CapnpSerializable.Create<Mas.Rpc.Model.XYResult>(reader.Xy);
                Stats = reader.Stats.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Rpc.Model.Stat>(_));
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

            public Mas.Rpc.Model.XYResult Xy
            {
                get;
                set;
            }

            public IReadOnlyList<Mas.Rpc.Model.Stat> Stats
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
                public Mas.Rpc.Model.XYResult.READER Xy => ctx.ReadStruct(0, Mas.Rpc.Model.XYResult.READER.create);
                public IReadOnlyList<Mas.Rpc.Model.Stat.READER> Stats => ctx.ReadList(1).Cast(Mas.Rpc.Model.Stat.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public Mas.Rpc.Model.XYResult.WRITER Xy
                {
                    get => BuildPointer<Mas.Rpc.Model.XYResult.WRITER>(0);
                    set => Link(0, value);
                }

                public ListOfStructsSerializer<Mas.Rpc.Model.Stat.WRITER> Stats
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Rpc.Model.Stat.WRITER>>(1);
                    set => Link(1, value);
                }
            }
        }

        [TypeId(0x8bce52266c0e0198UL), Proxy(typeof(ClimateInstance_Proxy)), Skeleton(typeof(ClimateInstance_Skeleton))]
        public interface IClimateInstance : Mas.Rpc.Common.IIdentifiable
        {
            Task<Mas.Rpc.Model.XYResult> Run(Mas.Rpc.ClimateData.ITimeSeries timeSeries, CancellationToken cancellationToken_ = default);
            Task<Mas.Rpc.Model.XYPlusResult> RunSet(IReadOnlyList<Mas.Rpc.ClimateData.ITimeSeries> dataset, CancellationToken cancellationToken_ = default);
        }

        public class ClimateInstance_Proxy : Proxy, IClimateInstance
        {
            public async Task<Mas.Rpc.Model.XYResult> Run(Mas.Rpc.ClimateData.ITimeSeries timeSeries, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Model.ClimateInstance.Params_run.WRITER>();
                var arg_ = new Mas.Rpc.Model.ClimateInstance.Params_run()
                {TimeSeries = timeSeries};
                arg_.serialize(in_);
                var d_ = await Call(10074079741449470360UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Model.ClimateInstance.Result_run>(d_);
                return (r_.Result);
            }

            public async Task<Mas.Rpc.Model.XYPlusResult> RunSet(IReadOnlyList<Mas.Rpc.ClimateData.ITimeSeries> dataset, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Model.ClimateInstance.Params_runSet.WRITER>();
                var arg_ = new Mas.Rpc.Model.ClimateInstance.Params_runSet()
                {Dataset = dataset};
                arg_.serialize(in_);
                var d_ = await Call(10074079741449470360UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Model.ClimateInstance.Result_runSet>(d_);
                return (r_.Result);
            }

            public async Task<Mas.Rpc.Common.IdInformation> Info(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Identifiable.Params_info.WRITER>();
                var arg_ = new Mas.Rpc.Common.Identifiable.Params_info()
                {};
                arg_.serialize(in_);
                var d_ = await Call(15298557119806335142UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Identifiable.Result_info>(d_);
                return (r_.Info);
            }
        }

        public class ClimateInstance_Skeleton : Skeleton<IClimateInstance>
        {
            public ClimateInstance_Skeleton()
            {
                SetMethodTable(Run, RunSet);
            }

            public override ulong InterfaceId => 10074079741449470360UL;
            Task<AnswerOrCounterquestion> Run(DeserializerState d_, CancellationToken cancellationToken_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.Model.ClimateInstance.Params_run>(d_);
                return Impatient.MaybeTailCall(Impl.Run(in_.TimeSeries, cancellationToken_), result =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Model.ClimateInstance.Result_run.WRITER>();
                    var r_ = new Mas.Rpc.Model.ClimateInstance.Result_run{Result = result};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> RunSet(DeserializerState d_, CancellationToken cancellationToken_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.Model.ClimateInstance.Params_runSet>(d_);
                return Impatient.MaybeTailCall(Impl.RunSet(in_.Dataset, cancellationToken_), result =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Model.ClimateInstance.Result_runSet.WRITER>();
                    var r_ = new Mas.Rpc.Model.ClimateInstance.Result_runSet{Result = result};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        public static class ClimateInstance
        {
            [TypeId(0xc3f5e7e9fa269327UL)]
            public class Params_run : ICapnpSerializable
            {
                public const UInt64 typeId = 0xc3f5e7e9fa269327UL;
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

                public Mas.Rpc.ClimateData.ITimeSeries TimeSeries
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
                    public Mas.Rpc.ClimateData.ITimeSeries TimeSeries => ctx.ReadCap<Mas.Rpc.ClimateData.ITimeSeries>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.ClimateData.ITimeSeries TimeSeries
                    {
                        get => ReadCap<Mas.Rpc.ClimateData.ITimeSeries>(0);
                        set => LinkObject(0, value);
                    }
                }
            }

            [TypeId(0x80febc638ae94be1UL)]
            public class Result_run : ICapnpSerializable
            {
                public const UInt64 typeId = 0x80febc638ae94be1UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Result = CapnpSerializable.Create<Mas.Rpc.Model.XYResult>(reader.Result);
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

                public Mas.Rpc.Model.XYResult Result
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
                    public Mas.Rpc.Model.XYResult.READER Result => ctx.ReadStruct(0, Mas.Rpc.Model.XYResult.READER.create);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Model.XYResult.WRITER Result
                    {
                        get => BuildPointer<Mas.Rpc.Model.XYResult.WRITER>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0xb60a0b81833da0cdUL)]
            public class Params_runSet : ICapnpSerializable
            {
                public const UInt64 typeId = 0xb60a0b81833da0cdUL;
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

                public IReadOnlyList<Mas.Rpc.ClimateData.ITimeSeries> Dataset
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
                    public IReadOnlyList<Mas.Rpc.ClimateData.ITimeSeries> Dataset => ctx.ReadCapList<Mas.Rpc.ClimateData.ITimeSeries>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfCapsSerializer<Mas.Rpc.ClimateData.ITimeSeries> Dataset
                    {
                        get => BuildPointer<ListOfCapsSerializer<Mas.Rpc.ClimateData.ITimeSeries>>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0xeadede9c91de4de5UL)]
            public class Result_runSet : ICapnpSerializable
            {
                public const UInt64 typeId = 0xeadede9c91de4de5UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Result = CapnpSerializable.Create<Mas.Rpc.Model.XYPlusResult>(reader.Result);
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

                public Mas.Rpc.Model.XYPlusResult Result
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
                    public Mas.Rpc.Model.XYPlusResult.READER Result => ctx.ReadStruct(0, Mas.Rpc.Model.XYPlusResult.READER.create);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Model.XYPlusResult.WRITER Result
                    {
                        get => BuildPointer<Mas.Rpc.Model.XYPlusResult.WRITER>(0);
                        set => Link(0, value);
                    }
                }
            }
        }

        [TypeId(0x99f29857557a901bUL)]
        public class Env : ICapnpSerializable
        {
            public const UInt64 typeId = 0x99f29857557a901bUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Rest = CapnpSerializable.Create<Mas.Rpc.Common.StructuredText>(reader.Rest);
                TimeSeries = reader.TimeSeries;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Rest?.serialize(writer.Rest);
                writer.TimeSeries = TimeSeries;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Rpc.Common.StructuredText Rest
            {
                get;
                set;
            }

            public Mas.Rpc.ClimateData.ITimeSeries TimeSeries
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
                public Mas.Rpc.Common.StructuredText.READER Rest => ctx.ReadStruct(0, Mas.Rpc.Common.StructuredText.READER.create);
                public Mas.Rpc.ClimateData.ITimeSeries TimeSeries => ctx.ReadCap<Mas.Rpc.ClimateData.ITimeSeries>(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public Mas.Rpc.Common.StructuredText.WRITER Rest
                {
                    get => BuildPointer<Mas.Rpc.Common.StructuredText.WRITER>(0);
                    set => Link(0, value);
                }

                public Mas.Rpc.ClimateData.ITimeSeries TimeSeries
                {
                    get => ReadCap<Mas.Rpc.ClimateData.ITimeSeries>(1);
                    set => LinkObject(1, value);
                }
            }
        }

        [TypeId(0x876f930b4bc6f8edUL), Proxy(typeof(A_Proxy)), Skeleton(typeof(A_Skeleton))]
        public interface IA : IDisposable
        {
        }

        public class A_Proxy : Proxy, IA
        {
        }

        public class A_Skeleton : Skeleton<IA>
        {
            public A_Skeleton()
            {
                SetMethodTable();
            }

            public override ulong InterfaceId => 9759180594260408557UL;
        }

        [TypeId(0xbd7525a775a56b4fUL), Proxy(typeof(EnvInstance_Proxy)), Skeleton(typeof(EnvInstance_Skeleton))]
        public interface IEnvInstance : Mas.Rpc.Common.IIdentifiable, Mas.Rpc.Model.IA, Mas.Rpc.Common.IStopable
        {
            Task<Mas.Rpc.Common.StructuredText> Run(Mas.Rpc.Model.Env env, CancellationToken cancellationToken_ = default);
        }

        public class EnvInstance_Proxy : Proxy, IEnvInstance
        {
            public async Task<Mas.Rpc.Common.StructuredText> Run(Mas.Rpc.Model.Env env, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Model.EnvInstance.Params_run.WRITER>();
                var arg_ = new Mas.Rpc.Model.EnvInstance.Params_run()
                {Env = env};
                arg_.serialize(in_);
                var d_ = await Call(13651859246607067983UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Model.EnvInstance.Result_run>(d_);
                return (r_.Result);
            }

            public async Task Stop(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Stopable.Params_stop.WRITER>();
                var arg_ = new Mas.Rpc.Common.Stopable.Params_stop()
                {};
                arg_.serialize(in_);
                var d_ = await Call(11746001905971884673UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Stopable.Result_stop>(d_);
                return;
            }

            public async Task<Mas.Rpc.Common.IdInformation> Info(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Identifiable.Params_info.WRITER>();
                var arg_ = new Mas.Rpc.Common.Identifiable.Params_info()
                {};
                arg_.serialize(in_);
                var d_ = await Call(15298557119806335142UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Identifiable.Result_info>(d_);
                return (r_.Info);
            }
        }

        public class EnvInstance_Skeleton : Skeleton<IEnvInstance>
        {
            public EnvInstance_Skeleton()
            {
                SetMethodTable(Run);
            }

            public override ulong InterfaceId => 13651859246607067983UL;
            Task<AnswerOrCounterquestion> Run(DeserializerState d_, CancellationToken cancellationToken_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.Model.EnvInstance.Params_run>(d_);
                return Impatient.MaybeTailCall(Impl.Run(in_.Env, cancellationToken_), result =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Model.EnvInstance.Result_run.WRITER>();
                    var r_ = new Mas.Rpc.Model.EnvInstance.Result_run{Result = result};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        public static class EnvInstance
        {
            [TypeId(0xf3d2398f9e321b8aUL)]
            public class Params_run : ICapnpSerializable
            {
                public const UInt64 typeId = 0xf3d2398f9e321b8aUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Env = CapnpSerializable.Create<Mas.Rpc.Model.Env>(reader.Env);
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

                public Mas.Rpc.Model.Env Env
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
                    public Mas.Rpc.Model.Env.READER Env => ctx.ReadStruct(0, Mas.Rpc.Model.Env.READER.create);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Model.Env.WRITER Env
                    {
                        get => BuildPointer<Mas.Rpc.Model.Env.WRITER>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0xe8ded162bf9f42fbUL)]
            public class Result_run : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe8ded162bf9f42fbUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Result = CapnpSerializable.Create<Mas.Rpc.Common.StructuredText>(reader.Result);
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

                public Mas.Rpc.Common.StructuredText Result
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
                    public Mas.Rpc.Common.StructuredText.READER Result => ctx.ReadStruct(0, Mas.Rpc.Common.StructuredText.READER.create);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.StructuredText.WRITER Result
                    {
                        get => BuildPointer<Mas.Rpc.Common.StructuredText.WRITER>(0);
                        set => Link(0, value);
                    }
                }
            }
        }

        [TypeId(0xcef6b5916058fcbbUL), Proxy(typeof(EnvInstanceProxy_Proxy)), Skeleton(typeof(EnvInstanceProxy_Skeleton))]
        public interface IEnvInstanceProxy : Mas.Rpc.Model.IEnvInstance
        {
            Task<Mas.Rpc.Common.ICallback> RegisterEnvInstance(Mas.Rpc.Model.IEnvInstance instance, CancellationToken cancellationToken_ = default);
        }

        public class EnvInstanceProxy_Proxy : Proxy, IEnvInstanceProxy
        {
            public Task<Mas.Rpc.Common.ICallback> RegisterEnvInstance(Mas.Rpc.Model.IEnvInstance instance, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Model.EnvInstanceProxy.Params_registerEnvInstance.WRITER>();
                var arg_ = new Mas.Rpc.Model.EnvInstanceProxy.Params_registerEnvInstance()
                {Instance = instance};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(14913306852075306171UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.Model.EnvInstanceProxy.Result_registerEnvInstance>(d_);
                    return (r_.Unregister);
                }

                );
            }

            public async Task<Mas.Rpc.Common.StructuredText> Run(Mas.Rpc.Model.Env env, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Model.EnvInstance.Params_run.WRITER>();
                var arg_ = new Mas.Rpc.Model.EnvInstance.Params_run()
                {Env = env};
                arg_.serialize(in_);
                var d_ = await Call(13651859246607067983UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Model.EnvInstance.Result_run>(d_);
                return (r_.Result);
            }

            public async Task Stop(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Stopable.Params_stop.WRITER>();
                var arg_ = new Mas.Rpc.Common.Stopable.Params_stop()
                {};
                arg_.serialize(in_);
                var d_ = await Call(11746001905971884673UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Stopable.Result_stop>(d_);
                return;
            }

            public async Task<Mas.Rpc.Common.IdInformation> Info(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.Common.Identifiable.Params_info.WRITER>();
                var arg_ = new Mas.Rpc.Common.Identifiable.Params_info()
                {};
                arg_.serialize(in_);
                var d_ = await Call(15298557119806335142UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.Common.Identifiable.Result_info>(d_);
                return (r_.Info);
            }
        }

        public class EnvInstanceProxy_Skeleton : Skeleton<IEnvInstanceProxy>
        {
            public EnvInstanceProxy_Skeleton()
            {
                SetMethodTable(RegisterEnvInstance);
            }

            public override ulong InterfaceId => 14913306852075306171UL;
            Task<AnswerOrCounterquestion> RegisterEnvInstance(DeserializerState d_, CancellationToken cancellationToken_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.Model.EnvInstanceProxy.Params_registerEnvInstance>(d_);
                return Impatient.MaybeTailCall(Impl.RegisterEnvInstance(in_.Instance, cancellationToken_), unregister =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.Model.EnvInstanceProxy.Result_registerEnvInstance.WRITER>();
                    var r_ = new Mas.Rpc.Model.EnvInstanceProxy.Result_registerEnvInstance{Unregister = unregister};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        public static class EnvInstanceProxy
        {
            [TypeId(0xe137e45725f16a43UL)]
            public class Params_registerEnvInstance : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe137e45725f16a43UL;
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

                public Mas.Rpc.Model.IEnvInstance Instance
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
                    public Mas.Rpc.Model.IEnvInstance Instance => ctx.ReadCap<Mas.Rpc.Model.IEnvInstance>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Model.IEnvInstance Instance
                    {
                        get => ReadCap<Mas.Rpc.Model.IEnvInstance>(0);
                        set => LinkObject(0, value);
                    }
                }
            }

            [TypeId(0xe5fe822766a2b68aUL)]
            public class Result_registerEnvInstance : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe5fe822766a2b68aUL;
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

                public Mas.Rpc.Common.ICallback Unregister
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
                    public Mas.Rpc.Common.ICallback Unregister => ctx.ReadCap<Mas.Rpc.Common.ICallback>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.ICallback Unregister
                    {
                        get => ReadCap<Mas.Rpc.Common.ICallback>(0);
                        set => LinkObject(0, value);
                    }
                }
            }
        }
    }
}