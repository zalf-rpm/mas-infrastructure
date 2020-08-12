using Capnp;
using Capnp.Rpc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Rpc
{
    [TypeId(0x8a4e6bd0ada5da48UL)]
    public class ClimateData : ICapnpSerializable
    {
        public const UInt64 typeId = 0x8a4e6bd0ada5da48UL;
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

        [TypeId(0xb934550067725ee5UL)]
        public enum Element : ushort
        {
            tmin,
            tavg,
            tmax,
            precip,
            globrad,
            wind,
            sunhours,
            cloudamount,
            relhumid,
            airpress,
            vaporpress,
            co2,
            o3,
            et0,
            dewpointTemp
        }

        [TypeId(0xdc846311ee2cbeadUL), Proxy(typeof(Station_Proxy)), Skeleton(typeof(Station_Skeleton))]
        public interface IStation : Mas.Rpc.Common.IIdentifiable
        {
            Task<Mas.Rpc.Common.IdInformation> SimulationInfo(CancellationToken cancellationToken_ = default);
            Task<float> HeightNN(CancellationToken cancellationToken_ = default);
            Task<Mas.Rpc.Geo.Coord> GeoCoord(CancellationToken cancellationToken_ = default);
            Task<IReadOnlyList<Mas.Rpc.ClimateData.ITimeSeries>> AllTimeSeries(CancellationToken cancellationToken_ = default);
            Task<Mas.Rpc.ClimateData.ITimeSeries> TimeSeriesFor(string scenarioId, string realizationId, CancellationToken cancellationToken_ = default);
        }

        public class Station_Proxy : Proxy, IStation
        {
            public async Task<Mas.Rpc.Common.IdInformation> SimulationInfo(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Station.Params_simulationInfo.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.Station.Params_simulationInfo()
                {};
                arg_.serialize(in_);
                var d_ = await Call(15889934313931456173UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.Station.Result_simulationInfo>(d_);
                return (r_.SimInfo);
            }

            public async Task<float> HeightNN(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Station.Params_heightNN.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.Station.Params_heightNN()
                {};
                arg_.serialize(in_);
                var d_ = await Call(15889934313931456173UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.Station.Result_heightNN>(d_);
                return (r_.HeightNN);
            }

            public async Task<Mas.Rpc.Geo.Coord> GeoCoord(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Station.Params_geoCoord.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.Station.Params_geoCoord()
                {};
                arg_.serialize(in_);
                var d_ = await Call(15889934313931456173UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.Station.Result_geoCoord>(d_);
                return (r_.GeoCoord);
            }

            public Task<IReadOnlyList<Mas.Rpc.ClimateData.ITimeSeries>> AllTimeSeries(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Station.Params_allTimeSeries.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.Station.Params_allTimeSeries()
                {};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(15889934313931456173UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.Station.Result_allTimeSeries>(d_);
                    return (r_.AllTimeSeries);
                }

                );
            }

            public Task<Mas.Rpc.ClimateData.ITimeSeries> TimeSeriesFor(string scenarioId, string realizationId, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Station.Params_timeSeriesFor.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.Station.Params_timeSeriesFor()
                {ScenarioId = scenarioId, RealizationId = realizationId};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(15889934313931456173UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.Station.Result_timeSeriesFor>(d_);
                    return (r_.TimeSeries);
                }

                );
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

        public class Station_Skeleton : Skeleton<IStation>
        {
            public Station_Skeleton()
            {
                SetMethodTable(SimulationInfo, HeightNN, GeoCoord, AllTimeSeries, TimeSeriesFor);
            }

            public override ulong InterfaceId => 15889934313931456173UL;
            Task<AnswerOrCounterquestion> SimulationInfo(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.SimulationInfo(cancellationToken_), simInfo =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Station.Result_simulationInfo.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.Station.Result_simulationInfo{SimInfo = simInfo};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> HeightNN(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.HeightNN(cancellationToken_), heightNN =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Station.Result_heightNN.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.Station.Result_heightNN{HeightNN = heightNN};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> GeoCoord(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.GeoCoord(cancellationToken_), geoCoord =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Station.Result_geoCoord.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.Station.Result_geoCoord{GeoCoord = geoCoord};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> AllTimeSeries(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.AllTimeSeries(cancellationToken_), allTimeSeries =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Station.Result_allTimeSeries.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.Station.Result_allTimeSeries{AllTimeSeries = allTimeSeries};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> TimeSeriesFor(DeserializerState d_, CancellationToken cancellationToken_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.Station.Params_timeSeriesFor>(d_);
                return Impatient.MaybeTailCall(Impl.TimeSeriesFor(in_.ScenarioId, in_.RealizationId, cancellationToken_), timeSeries =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Station.Result_timeSeriesFor.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.Station.Result_timeSeriesFor{TimeSeries = timeSeries};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        public static class Station
        {
            [TypeId(0xe5b2afad49e0e426UL)]
            public class Params_simulationInfo : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe5b2afad49e0e426UL;
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

            [TypeId(0x836c38e976857365UL)]
            public class Result_simulationInfo : ICapnpSerializable
            {
                public const UInt64 typeId = 0x836c38e976857365UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    SimInfo = CapnpSerializable.Create<Mas.Rpc.Common.IdInformation>(reader.SimInfo);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    SimInfo?.serialize(writer.SimInfo);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Rpc.Common.IdInformation SimInfo
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
                    public Mas.Rpc.Common.IdInformation.READER SimInfo => ctx.ReadStruct(0, Mas.Rpc.Common.IdInformation.READER.create);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.IdInformation.WRITER SimInfo
                    {
                        get => BuildPointer<Mas.Rpc.Common.IdInformation.WRITER>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0xb68673255f2d4997UL)]
            public class Params_heightNN : ICapnpSerializable
            {
                public const UInt64 typeId = 0xb68673255f2d4997UL;
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

            [TypeId(0x98559e9acd7fc437UL)]
            public class Result_heightNN : ICapnpSerializable
            {
                public const UInt64 typeId = 0x98559e9acd7fc437UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    HeightNN = reader.HeightNN;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.HeightNN = HeightNN;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public float HeightNN
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
                    public float HeightNN => ctx.ReadDataFloat(0UL, 0F);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 0);
                    }

                    public float HeightNN
                    {
                        get => this.ReadDataFloat(0UL, 0F);
                        set => this.WriteData(0UL, value, 0F);
                    }
                }
            }

            [TypeId(0x84321b24be3bfdb9UL)]
            public class Params_geoCoord : ICapnpSerializable
            {
                public const UInt64 typeId = 0x84321b24be3bfdb9UL;
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

            [TypeId(0x9c75cf7436cbcc39UL)]
            public class Result_geoCoord : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9c75cf7436cbcc39UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    GeoCoord = CapnpSerializable.Create<Mas.Rpc.Geo.Coord>(reader.GeoCoord);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    GeoCoord?.serialize(writer.GeoCoord);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Rpc.Geo.Coord GeoCoord
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
                    public Mas.Rpc.Geo.Coord.READER GeoCoord => ctx.ReadStruct(0, Mas.Rpc.Geo.Coord.READER.create);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Geo.Coord.WRITER GeoCoord
                    {
                        get => BuildPointer<Mas.Rpc.Geo.Coord.WRITER>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0xdbe53aae8cda359fUL)]
            public class Params_allTimeSeries : ICapnpSerializable
            {
                public const UInt64 typeId = 0xdbe53aae8cda359fUL;
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

            [TypeId(0xf96a2e47129dadecUL)]
            public class Result_allTimeSeries : ICapnpSerializable
            {
                public const UInt64 typeId = 0xf96a2e47129dadecUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    AllTimeSeries = reader.AllTimeSeries;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.AllTimeSeries.Init(AllTimeSeries);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Rpc.ClimateData.ITimeSeries> AllTimeSeries
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
                    public IReadOnlyList<Mas.Rpc.ClimateData.ITimeSeries> AllTimeSeries => ctx.ReadCapList<Mas.Rpc.ClimateData.ITimeSeries>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfCapsSerializer<Mas.Rpc.ClimateData.ITimeSeries> AllTimeSeries
                    {
                        get => BuildPointer<ListOfCapsSerializer<Mas.Rpc.ClimateData.ITimeSeries>>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0x9a64c35cd42aa2caUL)]
            public class Params_timeSeriesFor : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9a64c35cd42aa2caUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    ScenarioId = reader.ScenarioId;
                    RealizationId = reader.RealizationId;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.ScenarioId = ScenarioId;
                    writer.RealizationId = RealizationId;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public string ScenarioId
                {
                    get;
                    set;
                }

                public string RealizationId
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
                    public string ScenarioId => ctx.ReadText(0, "");
                    public string RealizationId => ctx.ReadText(1, "");
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 2);
                    }

                    public string ScenarioId
                    {
                        get => this.ReadText(0, "");
                        set => this.WriteText(0, value, "");
                    }

                    public string RealizationId
                    {
                        get => this.ReadText(1, "");
                        set => this.WriteText(1, value, "");
                    }
                }
            }

            [TypeId(0xe1ab6c6c59e77462UL)]
            public class Result_timeSeriesFor : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe1ab6c6c59e77462UL;
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
        }

        [TypeId(0xca4b7d8593ea4dc9UL)]
        public enum TimeResolution : ushort
        {
            daily,
            hourly
        }

        [TypeId(0xa462b37bd5b6255dUL), Proxy(typeof(TimeSeries_Proxy)), Skeleton(typeof(TimeSeries_Skeleton))]
        public interface ITimeSeries : IDisposable
        {
            Task<Mas.Rpc.ClimateData.TimeResolution> Resolution(CancellationToken cancellationToken_ = default);
            Task<(Mas.Rpc.Common.Date, Mas.Rpc.Common.Date)> Range(CancellationToken cancellationToken_ = default);
            Task<IReadOnlyList<Mas.Rpc.ClimateData.Element>> Header(CancellationToken cancellationToken_ = default);
            Task<IReadOnlyList<IReadOnlyList<float>>> Data(CancellationToken cancellationToken_ = default);
            Task<IReadOnlyList<IReadOnlyList<float>>> DataT(CancellationToken cancellationToken_ = default);
            Task<Mas.Rpc.ClimateData.ITimeSeries> Subrange(Mas.Rpc.Common.Date @from, Mas.Rpc.Common.Date to, CancellationToken cancellationToken_ = default);
            Task<Mas.Rpc.ClimateData.ITimeSeries> Subheader(IReadOnlyList<Mas.Rpc.ClimateData.Element> elements, CancellationToken cancellationToken_ = default);
            Task<Mas.Rpc.Common.IdInformation> SimulationInfo(CancellationToken cancellationToken_ = default);
            Task<Mas.Rpc.Common.IdInformation> ScenarioInfo(CancellationToken cancellationToken_ = default);
            Task<Mas.Rpc.Common.IdInformation> RealizationInfo(CancellationToken cancellationToken_ = default);
        }

        public class TimeSeries_Proxy : Proxy, ITimeSeries
        {
            public async Task<Mas.Rpc.ClimateData.TimeResolution> Resolution(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Params_resolution.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.TimeSeries.Params_resolution()
                {};
                arg_.serialize(in_);
                var d_ = await Call(11845227314385659229UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.TimeSeries.Result_resolution>(d_);
                return (r_.Resolution);
            }

            public async Task<(Mas.Rpc.Common.Date, Mas.Rpc.Common.Date)> Range(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Params_range.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.TimeSeries.Params_range()
                {};
                arg_.serialize(in_);
                var d_ = await Call(11845227314385659229UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.TimeSeries.Result_range>(d_);
                return (r_.StartDate, r_.EndDate);
            }

            public async Task<IReadOnlyList<Mas.Rpc.ClimateData.Element>> Header(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Params_header.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.TimeSeries.Params_header()
                {};
                arg_.serialize(in_);
                var d_ = await Call(11845227314385659229UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.TimeSeries.Result_header>(d_);
                return (r_.Header);
            }

            public async Task<IReadOnlyList<IReadOnlyList<float>>> Data(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Params_data.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.TimeSeries.Params_data()
                {};
                arg_.serialize(in_);
                var d_ = await Call(11845227314385659229UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.TimeSeries.Result_data>(d_);
                return (r_.Data);
            }

            public async Task<IReadOnlyList<IReadOnlyList<float>>> DataT(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Params_dataT.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.TimeSeries.Params_dataT()
                {};
                arg_.serialize(in_);
                var d_ = await Call(11845227314385659229UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.TimeSeries.Result_dataT>(d_);
                return (r_.Data);
            }

            public Task<Mas.Rpc.ClimateData.ITimeSeries> Subrange(Mas.Rpc.Common.Date @from, Mas.Rpc.Common.Date to, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Params_subrange.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.TimeSeries.Params_subrange()
                {From = @from, To = to};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(11845227314385659229UL, 5, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.TimeSeries.Result_subrange>(d_);
                    return (r_.TimeSeries);
                }

                );
            }

            public Task<Mas.Rpc.ClimateData.ITimeSeries> Subheader(IReadOnlyList<Mas.Rpc.ClimateData.Element> elements, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Params_subheader.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.TimeSeries.Params_subheader()
                {Elements = elements};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(11845227314385659229UL, 6, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.TimeSeries.Result_subheader>(d_);
                    return (r_.TimeSeries);
                }

                );
            }

            public async Task<Mas.Rpc.Common.IdInformation> SimulationInfo(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Params_simulationInfo.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.TimeSeries.Params_simulationInfo()
                {};
                arg_.serialize(in_);
                var d_ = await Call(11845227314385659229UL, 7, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.TimeSeries.Result_simulationInfo>(d_);
                return (r_.SimulationInfo);
            }

            public async Task<Mas.Rpc.Common.IdInformation> ScenarioInfo(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Params_scenarioInfo.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.TimeSeries.Params_scenarioInfo()
                {};
                arg_.serialize(in_);
                var d_ = await Call(11845227314385659229UL, 8, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.TimeSeries.Result_scenarioInfo>(d_);
                return (r_.ScenarioInfo);
            }

            public async Task<Mas.Rpc.Common.IdInformation> RealizationInfo(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Params_realizationInfo.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.TimeSeries.Params_realizationInfo()
                {};
                arg_.serialize(in_);
                var d_ = await Call(11845227314385659229UL, 9, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.TimeSeries.Result_realizationInfo>(d_);
                return (r_.RealizationInfo);
            }
        }

        public class TimeSeries_Skeleton : Skeleton<ITimeSeries>
        {
            public TimeSeries_Skeleton()
            {
                SetMethodTable(Resolution, Range, Header, Data, DataT, Subrange, Subheader, SimulationInfo, ScenarioInfo, RealizationInfo);
            }

            public override ulong InterfaceId => 11845227314385659229UL;
            Task<AnswerOrCounterquestion> Resolution(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.Resolution(cancellationToken_), resolution =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Result_resolution.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.TimeSeries.Result_resolution{Resolution = resolution};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> Range(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.Range(cancellationToken_), (startDate, endDate) =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Result_range.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.TimeSeries.Result_range{StartDate = startDate, EndDate = endDate};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> Header(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.Header(cancellationToken_), header =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Result_header.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.TimeSeries.Result_header{Header = header};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> Data(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.Data(cancellationToken_), data =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Result_data.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.TimeSeries.Result_data{Data = data};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> DataT(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.DataT(cancellationToken_), data =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Result_dataT.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.TimeSeries.Result_dataT{Data = data};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> Subrange(DeserializerState d_, CancellationToken cancellationToken_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.TimeSeries.Params_subrange>(d_);
                return Impatient.MaybeTailCall(Impl.Subrange(in_.From, in_.To, cancellationToken_), timeSeries =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Result_subrange.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.TimeSeries.Result_subrange{TimeSeries = timeSeries};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> Subheader(DeserializerState d_, CancellationToken cancellationToken_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.TimeSeries.Params_subheader>(d_);
                return Impatient.MaybeTailCall(Impl.Subheader(in_.Elements, cancellationToken_), timeSeries =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Result_subheader.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.TimeSeries.Result_subheader{TimeSeries = timeSeries};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> SimulationInfo(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.SimulationInfo(cancellationToken_), simulationInfo =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Result_simulationInfo.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.TimeSeries.Result_simulationInfo{SimulationInfo = simulationInfo};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> ScenarioInfo(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.ScenarioInfo(cancellationToken_), scenarioInfo =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Result_scenarioInfo.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.TimeSeries.Result_scenarioInfo{ScenarioInfo = scenarioInfo};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> RealizationInfo(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.RealizationInfo(cancellationToken_), realizationInfo =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.TimeSeries.Result_realizationInfo.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.TimeSeries.Result_realizationInfo{RealizationInfo = realizationInfo};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        public static class TimeSeries
        {
            [TypeId(0x9282d167c63b2b93UL)]
            public class Params_resolution : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9282d167c63b2b93UL;
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

            [TypeId(0xd7542e09cc32ca45UL)]
            public class Result_resolution : ICapnpSerializable
            {
                public const UInt64 typeId = 0xd7542e09cc32ca45UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Resolution = reader.Resolution;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Resolution = Resolution;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Rpc.ClimateData.TimeResolution Resolution
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
                    public Mas.Rpc.ClimateData.TimeResolution Resolution => (Mas.Rpc.ClimateData.TimeResolution)ctx.ReadDataUShort(0UL, (ushort)0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 0);
                    }

                    public Mas.Rpc.ClimateData.TimeResolution Resolution
                    {
                        get => (Mas.Rpc.ClimateData.TimeResolution)this.ReadDataUShort(0UL, (ushort)0);
                        set => this.WriteData(0UL, (ushort)value, (ushort)0);
                    }
                }
            }

            [TypeId(0xd9ebacab7ba151dcUL)]
            public class Params_range : ICapnpSerializable
            {
                public const UInt64 typeId = 0xd9ebacab7ba151dcUL;
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

            [TypeId(0xf8be1922604dafd2UL)]
            public class Result_range : ICapnpSerializable
            {
                public const UInt64 typeId = 0xf8be1922604dafd2UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    StartDate = CapnpSerializable.Create<Mas.Rpc.Common.Date>(reader.StartDate);
                    EndDate = CapnpSerializable.Create<Mas.Rpc.Common.Date>(reader.EndDate);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    StartDate?.serialize(writer.StartDate);
                    EndDate?.serialize(writer.EndDate);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Rpc.Common.Date StartDate
                {
                    get;
                    set;
                }

                public Mas.Rpc.Common.Date EndDate
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
                    public Mas.Rpc.Common.Date.READER StartDate => ctx.ReadStruct(0, Mas.Rpc.Common.Date.READER.create);
                    public Mas.Rpc.Common.Date.READER EndDate => ctx.ReadStruct(1, Mas.Rpc.Common.Date.READER.create);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 2);
                    }

                    public Mas.Rpc.Common.Date.WRITER StartDate
                    {
                        get => BuildPointer<Mas.Rpc.Common.Date.WRITER>(0);
                        set => Link(0, value);
                    }

                    public Mas.Rpc.Common.Date.WRITER EndDate
                    {
                        get => BuildPointer<Mas.Rpc.Common.Date.WRITER>(1);
                        set => Link(1, value);
                    }
                }
            }

            [TypeId(0x88793b2f2f852436UL)]
            public class Params_header : ICapnpSerializable
            {
                public const UInt64 typeId = 0x88793b2f2f852436UL;
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

            [TypeId(0x887efd3ecc3c1447UL)]
            public class Result_header : ICapnpSerializable
            {
                public const UInt64 typeId = 0x887efd3ecc3c1447UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Header = reader.Header;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Header.Init(Header);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Rpc.ClimateData.Element> Header
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
                    public IReadOnlyList<Mas.Rpc.ClimateData.Element> Header => ctx.ReadList(0).CastEnums(_0 => (Mas.Rpc.ClimateData.Element)_0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfPrimitivesSerializer<Mas.Rpc.ClimateData.Element> Header
                    {
                        get => BuildPointer<ListOfPrimitivesSerializer<Mas.Rpc.ClimateData.Element>>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0xb600cff68674914bUL)]
            public class Params_data : ICapnpSerializable
            {
                public const UInt64 typeId = 0xb600cff68674914bUL;
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

            [TypeId(0xdb7591f768f21e1cUL)]
            public class Result_data : ICapnpSerializable
            {
                public const UInt64 typeId = 0xdb7591f768f21e1cUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Data = reader.Data;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Data.Init(Data, (_s2, _v2) => _s2.Init(_v2));
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<IReadOnlyList<float>> Data
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
                    public IReadOnlyList<IReadOnlyList<float>> Data => ctx.ReadList(0).Cast(_0 => _0.RequireList().CastFloat());
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfPointersSerializer<ListOfPrimitivesSerializer<float>> Data
                    {
                        get => BuildPointer<ListOfPointersSerializer<ListOfPrimitivesSerializer<float>>>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0xdb1c8a0d6fe6dad8UL)]
            public class Params_dataT : ICapnpSerializable
            {
                public const UInt64 typeId = 0xdb1c8a0d6fe6dad8UL;
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

            [TypeId(0xe1c9711c4d748b74UL)]
            public class Result_dataT : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe1c9711c4d748b74UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Data = reader.Data;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Data.Init(Data, (_s2, _v2) => _s2.Init(_v2));
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<IReadOnlyList<float>> Data
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
                    public IReadOnlyList<IReadOnlyList<float>> Data => ctx.ReadList(0).Cast(_0 => _0.RequireList().CastFloat());
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfPointersSerializer<ListOfPrimitivesSerializer<float>> Data
                    {
                        get => BuildPointer<ListOfPointersSerializer<ListOfPrimitivesSerializer<float>>>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0xa2f3c0f7d449b09cUL)]
            public class Params_subrange : ICapnpSerializable
            {
                public const UInt64 typeId = 0xa2f3c0f7d449b09cUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    From = CapnpSerializable.Create<Mas.Rpc.Common.Date>(reader.From);
                    To = CapnpSerializable.Create<Mas.Rpc.Common.Date>(reader.To);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    From?.serialize(writer.From);
                    To?.serialize(writer.To);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Rpc.Common.Date From
                {
                    get;
                    set;
                }

                public Mas.Rpc.Common.Date To
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
                    public Mas.Rpc.Common.Date.READER From => ctx.ReadStruct(0, Mas.Rpc.Common.Date.READER.create);
                    public Mas.Rpc.Common.Date.READER To => ctx.ReadStruct(1, Mas.Rpc.Common.Date.READER.create);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 2);
                    }

                    public Mas.Rpc.Common.Date.WRITER From
                    {
                        get => BuildPointer<Mas.Rpc.Common.Date.WRITER>(0);
                        set => Link(0, value);
                    }

                    public Mas.Rpc.Common.Date.WRITER To
                    {
                        get => BuildPointer<Mas.Rpc.Common.Date.WRITER>(1);
                        set => Link(1, value);
                    }
                }
            }

            [TypeId(0x9bc06c13e9bafa39UL)]
            public class Result_subrange : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9bc06c13e9bafa39UL;
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

            [TypeId(0xc101328bf7606753UL)]
            public class Params_subheader : ICapnpSerializable
            {
                public const UInt64 typeId = 0xc101328bf7606753UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Elements = reader.Elements;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Elements.Init(Elements);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Rpc.ClimateData.Element> Elements
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
                    public IReadOnlyList<Mas.Rpc.ClimateData.Element> Elements => ctx.ReadList(0).CastEnums(_0 => (Mas.Rpc.ClimateData.Element)_0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfPrimitivesSerializer<Mas.Rpc.ClimateData.Element> Elements
                    {
                        get => BuildPointer<ListOfPrimitivesSerializer<Mas.Rpc.ClimateData.Element>>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0xd7c77ac524468700UL)]
            public class Result_subheader : ICapnpSerializable
            {
                public const UInt64 typeId = 0xd7c77ac524468700UL;
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

            [TypeId(0xf30200ea2376a78eUL)]
            public class Params_simulationInfo : ICapnpSerializable
            {
                public const UInt64 typeId = 0xf30200ea2376a78eUL;
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

            [TypeId(0xa6d9aa9efbf984bbUL)]
            public class Result_simulationInfo : ICapnpSerializable
            {
                public const UInt64 typeId = 0xa6d9aa9efbf984bbUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    SimulationInfo = CapnpSerializable.Create<Mas.Rpc.Common.IdInformation>(reader.SimulationInfo);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    SimulationInfo?.serialize(writer.SimulationInfo);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Rpc.Common.IdInformation SimulationInfo
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
                    public Mas.Rpc.Common.IdInformation.READER SimulationInfo => ctx.ReadStruct(0, Mas.Rpc.Common.IdInformation.READER.create);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.IdInformation.WRITER SimulationInfo
                    {
                        get => BuildPointer<Mas.Rpc.Common.IdInformation.WRITER>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0xa7a239fb0e5e10e5UL)]
            public class Params_scenarioInfo : ICapnpSerializable
            {
                public const UInt64 typeId = 0xa7a239fb0e5e10e5UL;
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

            [TypeId(0xd4f126549022180bUL)]
            public class Result_scenarioInfo : ICapnpSerializable
            {
                public const UInt64 typeId = 0xd4f126549022180bUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    ScenarioInfo = CapnpSerializable.Create<Mas.Rpc.Common.IdInformation>(reader.ScenarioInfo);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    ScenarioInfo?.serialize(writer.ScenarioInfo);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Rpc.Common.IdInformation ScenarioInfo
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
                    public Mas.Rpc.Common.IdInformation.READER ScenarioInfo => ctx.ReadStruct(0, Mas.Rpc.Common.IdInformation.READER.create);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.IdInformation.WRITER ScenarioInfo
                    {
                        get => BuildPointer<Mas.Rpc.Common.IdInformation.WRITER>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0x90e6064d11667325UL)]
            public class Params_realizationInfo : ICapnpSerializable
            {
                public const UInt64 typeId = 0x90e6064d11667325UL;
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

            [TypeId(0xfe085372225c3a2eUL)]
            public class Result_realizationInfo : ICapnpSerializable
            {
                public const UInt64 typeId = 0xfe085372225c3a2eUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    RealizationInfo = CapnpSerializable.Create<Mas.Rpc.Common.IdInformation>(reader.RealizationInfo);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    RealizationInfo?.serialize(writer.RealizationInfo);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Rpc.Common.IdInformation RealizationInfo
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
                    public Mas.Rpc.Common.IdInformation.READER RealizationInfo => ctx.ReadStruct(0, Mas.Rpc.Common.IdInformation.READER.create);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.IdInformation.WRITER RealizationInfo
                    {
                        get => BuildPointer<Mas.Rpc.Common.IdInformation.WRITER>(0);
                        set => Link(0, value);
                    }
                }
            }
        }

        [TypeId(0x8133f52916cfc817UL), Proxy(typeof(Simulation_Proxy)), Skeleton(typeof(Simulation_Skeleton))]
        public interface ISimulation : Mas.Rpc.Common.IIdentifiable
        {
            Task<IReadOnlyList<Mas.Rpc.ClimateData.IScenario>> Scenarios(CancellationToken cancellationToken_ = default);
            Task<IReadOnlyList<Mas.Rpc.ClimateData.IStation>> Stations(CancellationToken cancellationToken_ = default);
        }

        public class Simulation_Proxy : Proxy, ISimulation
        {
            public Task<IReadOnlyList<Mas.Rpc.ClimateData.IScenario>> Scenarios(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Simulation.Params_scenarios.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.Simulation.Params_scenarios()
                {};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(9310054411530127383UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.Simulation.Result_scenarios>(d_);
                    return (r_.Scenarios);
                }

                );
            }

            public Task<IReadOnlyList<Mas.Rpc.ClimateData.IStation>> Stations(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Simulation.Params_stations.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.Simulation.Params_stations()
                {};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(9310054411530127383UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.Simulation.Result_stations>(d_);
                    return (r_.Stations);
                }

                );
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

        public class Simulation_Skeleton : Skeleton<ISimulation>
        {
            public Simulation_Skeleton()
            {
                SetMethodTable(Scenarios, Stations);
            }

            public override ulong InterfaceId => 9310054411530127383UL;
            Task<AnswerOrCounterquestion> Scenarios(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.Scenarios(cancellationToken_), scenarios =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Simulation.Result_scenarios.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.Simulation.Result_scenarios{Scenarios = scenarios};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> Stations(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.Stations(cancellationToken_), stations =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Simulation.Result_stations.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.Simulation.Result_stations{Stations = stations};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        public static class Simulation
        {
            [TypeId(0xe8b1bfc484d642caUL)]
            public class Params_scenarios : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe8b1bfc484d642caUL;
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

            [TypeId(0xbc4dda286764224bUL)]
            public class Result_scenarios : ICapnpSerializable
            {
                public const UInt64 typeId = 0xbc4dda286764224bUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Scenarios = reader.Scenarios;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Scenarios.Init(Scenarios);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Rpc.ClimateData.IScenario> Scenarios
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
                    public IReadOnlyList<Mas.Rpc.ClimateData.IScenario> Scenarios => ctx.ReadCapList<Mas.Rpc.ClimateData.IScenario>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfCapsSerializer<Mas.Rpc.ClimateData.IScenario> Scenarios
                    {
                        get => BuildPointer<ListOfCapsSerializer<Mas.Rpc.ClimateData.IScenario>>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0xc5e5aace5a87ef93UL)]
            public class Params_stations : ICapnpSerializable
            {
                public const UInt64 typeId = 0xc5e5aace5a87ef93UL;
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

            [TypeId(0xf984ca77a35c7312UL)]
            public class Result_stations : ICapnpSerializable
            {
                public const UInt64 typeId = 0xf984ca77a35c7312UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Stations = reader.Stations;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Stations.Init(Stations);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Rpc.ClimateData.IStation> Stations
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
                    public IReadOnlyList<Mas.Rpc.ClimateData.IStation> Stations => ctx.ReadCapList<Mas.Rpc.ClimateData.IStation>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfCapsSerializer<Mas.Rpc.ClimateData.IStation> Stations
                    {
                        get => BuildPointer<ListOfCapsSerializer<Mas.Rpc.ClimateData.IStation>>(0);
                        set => Link(0, value);
                    }
                }
            }
        }

        [TypeId(0xb84631ddd49c2153UL), Proxy(typeof(Scenario_Proxy)), Skeleton(typeof(Scenario_Skeleton))]
        public interface IScenario : Mas.Rpc.Common.IIdentifiable
        {
            Task<Mas.Rpc.Common.IdInformation> SimulationInfo(CancellationToken cancellationToken_ = default);
            Task<IReadOnlyList<Mas.Rpc.ClimateData.IRealization>> Realizations(CancellationToken cancellationToken_ = default);
        }

        public class Scenario_Proxy : Proxy, IScenario
        {
            public async Task<Mas.Rpc.Common.IdInformation> SimulationInfo(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Scenario.Params_simulationInfo.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.Scenario.Params_simulationInfo()
                {};
                arg_.serialize(in_);
                var d_ = await Call(13278355380173021523UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.Scenario.Result_simulationInfo>(d_);
                return (r_.SimulationInfo);
            }

            public Task<IReadOnlyList<Mas.Rpc.ClimateData.IRealization>> Realizations(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Scenario.Params_realizations.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.Scenario.Params_realizations()
                {};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(13278355380173021523UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.Scenario.Result_realizations>(d_);
                    return (r_.Realizations);
                }

                );
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

        public class Scenario_Skeleton : Skeleton<IScenario>
        {
            public Scenario_Skeleton()
            {
                SetMethodTable(SimulationInfo, Realizations);
            }

            public override ulong InterfaceId => 13278355380173021523UL;
            Task<AnswerOrCounterquestion> SimulationInfo(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.SimulationInfo(cancellationToken_), simulationInfo =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Scenario.Result_simulationInfo.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.Scenario.Result_simulationInfo{SimulationInfo = simulationInfo};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> Realizations(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.Realizations(cancellationToken_), realizations =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Scenario.Result_realizations.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.Scenario.Result_realizations{Realizations = realizations};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        public static class Scenario
        {
            [TypeId(0xa1925ff2741f66a9UL)]
            public class Params_simulationInfo : ICapnpSerializable
            {
                public const UInt64 typeId = 0xa1925ff2741f66a9UL;
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

            [TypeId(0xca6e47f9400548cdUL)]
            public class Result_simulationInfo : ICapnpSerializable
            {
                public const UInt64 typeId = 0xca6e47f9400548cdUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    SimulationInfo = CapnpSerializable.Create<Mas.Rpc.Common.IdInformation>(reader.SimulationInfo);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    SimulationInfo?.serialize(writer.SimulationInfo);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Rpc.Common.IdInformation SimulationInfo
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
                    public Mas.Rpc.Common.IdInformation.READER SimulationInfo => ctx.ReadStruct(0, Mas.Rpc.Common.IdInformation.READER.create);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.IdInformation.WRITER SimulationInfo
                    {
                        get => BuildPointer<Mas.Rpc.Common.IdInformation.WRITER>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0x8b6c8045c4c9d02fUL)]
            public class Params_realizations : ICapnpSerializable
            {
                public const UInt64 typeId = 0x8b6c8045c4c9d02fUL;
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

            [TypeId(0xdbba287669f89b40UL)]
            public class Result_realizations : ICapnpSerializable
            {
                public const UInt64 typeId = 0xdbba287669f89b40UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Realizations = reader.Realizations;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Realizations.Init(Realizations);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Rpc.ClimateData.IRealization> Realizations
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
                    public IReadOnlyList<Mas.Rpc.ClimateData.IRealization> Realizations => ctx.ReadCapList<Mas.Rpc.ClimateData.IRealization>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfCapsSerializer<Mas.Rpc.ClimateData.IRealization> Realizations
                    {
                        get => BuildPointer<ListOfCapsSerializer<Mas.Rpc.ClimateData.IRealization>>(0);
                        set => Link(0, value);
                    }
                }
            }
        }

        [TypeId(0xf1c328f5d08e0a02UL), Proxy(typeof(Realization_Proxy)), Skeleton(typeof(Realization_Skeleton))]
        public interface IRealization : Mas.Rpc.Common.IIdentifiable
        {
            Task<Mas.Rpc.Common.IdInformation> ScenarioInfo(CancellationToken cancellationToken_ = default);
            Task<IReadOnlyList<Mas.Rpc.ClimateData.ITimeSeries>> ClosestTimeSeriesAt(Mas.Rpc.Geo.Coord geoCoord, CancellationToken cancellationToken_ = default);
        }

        public class Realization_Proxy : Proxy, IRealization
        {
            public async Task<Mas.Rpc.Common.IdInformation> ScenarioInfo(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Realization.Params_scenarioInfo.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.Realization.Params_scenarioInfo()
                {};
                arg_.serialize(in_);
                var d_ = await Call(17420812819830278658UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned;
                var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.Realization.Result_scenarioInfo>(d_);
                return (r_.ScenarioInfo);
            }

            public Task<IReadOnlyList<Mas.Rpc.ClimateData.ITimeSeries>> ClosestTimeSeriesAt(Mas.Rpc.Geo.Coord geoCoord, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Realization.Params_closestTimeSeriesAt.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.Realization.Params_closestTimeSeriesAt()
                {GeoCoord = geoCoord};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(17420812819830278658UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.Realization.Result_closestTimeSeriesAt>(d_);
                    return (r_.TimeSeries);
                }

                );
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

        public class Realization_Skeleton : Skeleton<IRealization>
        {
            public Realization_Skeleton()
            {
                SetMethodTable(ScenarioInfo, ClosestTimeSeriesAt);
            }

            public override ulong InterfaceId => 17420812819830278658UL;
            Task<AnswerOrCounterquestion> ScenarioInfo(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.ScenarioInfo(cancellationToken_), scenarioInfo =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Realization.Result_scenarioInfo.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.Realization.Result_scenarioInfo{ScenarioInfo = scenarioInfo};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> ClosestTimeSeriesAt(DeserializerState d_, CancellationToken cancellationToken_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.Realization.Params_closestTimeSeriesAt>(d_);
                return Impatient.MaybeTailCall(Impl.ClosestTimeSeriesAt(in_.GeoCoord, cancellationToken_), timeSeries =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Realization.Result_closestTimeSeriesAt.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.Realization.Result_closestTimeSeriesAt{TimeSeries = timeSeries};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        public static class Realization
        {
            [TypeId(0xd7669b6cecca916dUL)]
            public class Params_scenarioInfo : ICapnpSerializable
            {
                public const UInt64 typeId = 0xd7669b6cecca916dUL;
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

            [TypeId(0xfe930f5fbdaecaf2UL)]
            public class Result_scenarioInfo : ICapnpSerializable
            {
                public const UInt64 typeId = 0xfe930f5fbdaecaf2UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    ScenarioInfo = CapnpSerializable.Create<Mas.Rpc.Common.IdInformation>(reader.ScenarioInfo);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    ScenarioInfo?.serialize(writer.ScenarioInfo);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Rpc.Common.IdInformation ScenarioInfo
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
                    public Mas.Rpc.Common.IdInformation.READER ScenarioInfo => ctx.ReadStruct(0, Mas.Rpc.Common.IdInformation.READER.create);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Common.IdInformation.WRITER ScenarioInfo
                    {
                        get => BuildPointer<Mas.Rpc.Common.IdInformation.WRITER>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0xc3dcc7253313eb7cUL)]
            public class Params_closestTimeSeriesAt : ICapnpSerializable
            {
                public const UInt64 typeId = 0xc3dcc7253313eb7cUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    GeoCoord = CapnpSerializable.Create<Mas.Rpc.Geo.Coord>(reader.GeoCoord);
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    GeoCoord?.serialize(writer.GeoCoord);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Rpc.Geo.Coord GeoCoord
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
                    public Mas.Rpc.Geo.Coord.READER GeoCoord => ctx.ReadStruct(0, Mas.Rpc.Geo.Coord.READER.create);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.Geo.Coord.WRITER GeoCoord
                    {
                        get => BuildPointer<Mas.Rpc.Geo.Coord.WRITER>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0xf1f7f0b3386be672UL)]
            public class Result_closestTimeSeriesAt : ICapnpSerializable
            {
                public const UInt64 typeId = 0xf1f7f0b3386be672UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    TimeSeries = reader.TimeSeries;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.TimeSeries.Init(TimeSeries);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Rpc.ClimateData.ITimeSeries> TimeSeries
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
                    public IReadOnlyList<Mas.Rpc.ClimateData.ITimeSeries> TimeSeries => ctx.ReadCapList<Mas.Rpc.ClimateData.ITimeSeries>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfCapsSerializer<Mas.Rpc.ClimateData.ITimeSeries> TimeSeries
                    {
                        get => BuildPointer<ListOfCapsSerializer<Mas.Rpc.ClimateData.ITimeSeries>>(0);
                        set => Link(0, value);
                    }
                }
            }
        }

        [TypeId(0xe0a7f8d7ffb5a1baUL), Proxy(typeof(Service_Proxy)), Skeleton(typeof(Service_Skeleton))]
        public interface IService : Mas.Rpc.Common.IIdentifiable
        {
            Task<IReadOnlyList<Mas.Rpc.ClimateData.ISimulation>> GetAvailableSimulations(CancellationToken cancellationToken_ = default);
            Task<Mas.Rpc.ClimateData.ISimulation> GetSimulation(ulong id, CancellationToken cancellationToken_ = default);
        }

        public class Service_Proxy : Proxy, IService
        {
            public Task<IReadOnlyList<Mas.Rpc.ClimateData.ISimulation>> GetAvailableSimulations(CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Service.Params_getAvailableSimulations.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.Service.Params_getAvailableSimulations()
                {};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(16188180992198287802UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.Service.Result_getAvailableSimulations>(d_);
                    return (r_.AvailableSimulations);
                }

                );
            }

            public Task<Mas.Rpc.ClimateData.ISimulation> GetSimulation(ulong id, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Service.Params_getSimulation.WRITER>();
                var arg_ = new Mas.Rpc.ClimateData.Service.Params_getSimulation()
                {Id = id};
                arg_.serialize(in_);
                return Impatient.MakePipelineAware(Call(16188180992198287802UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
                {
                    var r_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.Service.Result_getSimulation>(d_);
                    return (r_.Simulation);
                }

                );
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

        public class Service_Skeleton : Skeleton<IService>
        {
            public Service_Skeleton()
            {
                SetMethodTable(GetAvailableSimulations, GetSimulation);
            }

            public override ulong InterfaceId => 16188180992198287802UL;
            Task<AnswerOrCounterquestion> GetAvailableSimulations(DeserializerState d_, CancellationToken cancellationToken_)
            {
                return Impatient.MaybeTailCall(Impl.GetAvailableSimulations(cancellationToken_), availableSimulations =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Service.Result_getAvailableSimulations.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.Service.Result_getAvailableSimulations{AvailableSimulations = availableSimulations};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }

            Task<AnswerOrCounterquestion> GetSimulation(DeserializerState d_, CancellationToken cancellationToken_)
            {
                var in_ = CapnpSerializable.Create<Mas.Rpc.ClimateData.Service.Params_getSimulation>(d_);
                return Impatient.MaybeTailCall(Impl.GetSimulation(in_.Id, cancellationToken_), simulation =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Rpc.ClimateData.Service.Result_getSimulation.WRITER>();
                    var r_ = new Mas.Rpc.ClimateData.Service.Result_getSimulation{Simulation = simulation};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        public static class Service
        {
            [TypeId(0x8409ef6cadada156UL)]
            public class Params_getAvailableSimulations : ICapnpSerializable
            {
                public const UInt64 typeId = 0x8409ef6cadada156UL;
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

            [TypeId(0xcd1c948e2c72f312UL)]
            public class Result_getAvailableSimulations : ICapnpSerializable
            {
                public const UInt64 typeId = 0xcd1c948e2c72f312UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    AvailableSimulations = reader.AvailableSimulations;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.AvailableSimulations.Init(AvailableSimulations);
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Rpc.ClimateData.ISimulation> AvailableSimulations
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
                    public IReadOnlyList<Mas.Rpc.ClimateData.ISimulation> AvailableSimulations => ctx.ReadCapList<Mas.Rpc.ClimateData.ISimulation>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfCapsSerializer<Mas.Rpc.ClimateData.ISimulation> AvailableSimulations
                    {
                        get => BuildPointer<ListOfCapsSerializer<Mas.Rpc.ClimateData.ISimulation>>(0);
                        set => Link(0, value);
                    }
                }
            }

            [TypeId(0x9622a07e81678f8bUL)]
            public class Params_getSimulation : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9622a07e81678f8bUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Id = reader.Id;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Id = Id;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public ulong Id
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
                    public ulong Id => ctx.ReadDataULong(0UL, 0UL);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 0);
                    }

                    public ulong Id
                    {
                        get => this.ReadDataULong(0UL, 0UL);
                        set => this.WriteData(0UL, value, 0UL);
                    }
                }
            }

            [TypeId(0xc35cec488705ed92UL)]
            public class Result_getSimulation : ICapnpSerializable
            {
                public const UInt64 typeId = 0xc35cec488705ed92UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Simulation = reader.Simulation;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Simulation = Simulation;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Rpc.ClimateData.ISimulation Simulation
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
                    public Mas.Rpc.ClimateData.ISimulation Simulation => ctx.ReadCap<Mas.Rpc.ClimateData.ISimulation>(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public Mas.Rpc.ClimateData.ISimulation Simulation
                    {
                        get => ReadCap<Mas.Rpc.ClimateData.ISimulation>(0);
                        set => LinkObject(0, value);
                    }
                }
            }
        }
    }
}