using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Schema.Grid
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa5ecdc7767a6b301UL)]
    public enum Aggregation : ushort
    {
        none,
        avg,
        wAvg,
        iAvg,
        median,
        wMedian,
        iMedian,
        min,
        wMin,
        iMin,
        max,
        wMax,
        iMax,
        sum,
        wSum,
        iSum
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe42973b29661e3c6UL), Proxy(typeof(Grid_Proxy)), Skeleton(typeof(Grid_Skeleton))]
    public interface IGrid : Mas.Schema.Common.IIdentifiable, Mas.Schema.Persistence.IPersistent
    {
        Task<(Mas.Schema.Grid.Grid.Value, Mas.Schema.Grid.Grid.RowCol, Mas.Schema.Grid.Grid.RowCol, IReadOnlyList<Mas.Schema.Grid.Grid.AggregationPart>)> ClosestValueAt(Mas.Schema.Geo.LatLonCoord latlonCoord, bool ignoreNoData, Mas.Schema.Grid.Grid.Resolution resolution, Mas.Schema.Grid.Aggregation agg, bool returnRowCols, bool includeAggParts, CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Grid.Grid.Resolution> Resolution(CancellationToken cancellationToken_ = default);
        Task<(ulong, ulong)> Dimension(CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Grid.Grid.Value> NoDataValue(CancellationToken cancellationToken_ = default);
        Task<(Mas.Schema.Grid.Grid.Value, IReadOnlyList<Mas.Schema.Grid.Grid.AggregationPart>)> ValueAt(ulong row, ulong col, Mas.Schema.Grid.Grid.Resolution resolution, Mas.Schema.Grid.Aggregation agg, bool includeAggParts, CancellationToken cancellationToken_ = default);
        Task<(Mas.Schema.Geo.LatLonCoord, Mas.Schema.Geo.LatLonCoord, Mas.Schema.Geo.LatLonCoord, Mas.Schema.Geo.LatLonCoord)> LatLonBounds(bool useCellCenter, CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Grid.Grid.ICallback> StreamCells(Mas.Schema.Grid.Grid.RowCol topLeft, Mas.Schema.Grid.Grid.RowCol bottomRight, CancellationToken cancellationToken_ = default);
        Task<string> Unit(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe42973b29661e3c6UL)]
    public class Grid_Proxy : Proxy, IGrid
    {
        public async Task<(Mas.Schema.Grid.Grid.Value, Mas.Schema.Grid.Grid.RowCol, Mas.Schema.Grid.Grid.RowCol, IReadOnlyList<Mas.Schema.Grid.Grid.AggregationPart>)> ClosestValueAt(Mas.Schema.Geo.LatLonCoord latlonCoord, bool ignoreNoData, Mas.Schema.Grid.Grid.Resolution resolution, Mas.Schema.Grid.Aggregation agg, bool returnRowCols, bool includeAggParts, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Params_ClosestValueAt.WRITER>();
            var arg_ = new Mas.Schema.Grid.Grid.Params_ClosestValueAt()
            {LatlonCoord = latlonCoord, IgnoreNoData = ignoreNoData, Resolution = resolution, Agg = agg, ReturnRowCols = returnRowCols, IncludeAggParts = includeAggParts};
            arg_?.serialize(in_);
            using (var d_ = await Call(16440799125557076934UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Result_ClosestValueAt>(d_);
                return (r_.Val, r_.Tl, r_.Br, r_.AggParts);
            }
        }

        public async Task<Mas.Schema.Grid.Grid.Resolution> Resolution(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Params_Resolution.WRITER>();
            var arg_ = new Mas.Schema.Grid.Grid.Params_Resolution()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(16440799125557076934UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Result_Resolution>(d_);
                return (r_.Res);
            }
        }

        public async Task<(ulong, ulong)> Dimension(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Params_Dimension.WRITER>();
            var arg_ = new Mas.Schema.Grid.Grid.Params_Dimension()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(16440799125557076934UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Result_Dimension>(d_);
                return (r_.Rows, r_.Cols);
            }
        }

        public async Task<Mas.Schema.Grid.Grid.Value> NoDataValue(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Params_NoDataValue.WRITER>();
            var arg_ = new Mas.Schema.Grid.Grid.Params_NoDataValue()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(16440799125557076934UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Result_NoDataValue>(d_);
                return (r_.Nodata);
            }
        }

        public async Task<(Mas.Schema.Grid.Grid.Value, IReadOnlyList<Mas.Schema.Grid.Grid.AggregationPart>)> ValueAt(ulong row, ulong col, Mas.Schema.Grid.Grid.Resolution resolution, Mas.Schema.Grid.Aggregation agg, bool includeAggParts, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Params_ValueAt.WRITER>();
            var arg_ = new Mas.Schema.Grid.Grid.Params_ValueAt()
            {Row = row, Col = col, Resolution = resolution, Agg = agg, IncludeAggParts = includeAggParts};
            arg_?.serialize(in_);
            using (var d_ = await Call(16440799125557076934UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Result_ValueAt>(d_);
                return (r_.Val, r_.AggParts);
            }
        }

        public async Task<(Mas.Schema.Geo.LatLonCoord, Mas.Schema.Geo.LatLonCoord, Mas.Schema.Geo.LatLonCoord, Mas.Schema.Geo.LatLonCoord)> LatLonBounds(bool useCellCenter, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Params_LatLonBounds.WRITER>();
            var arg_ = new Mas.Schema.Grid.Grid.Params_LatLonBounds()
            {UseCellCenter = useCellCenter};
            arg_?.serialize(in_);
            using (var d_ = await Call(16440799125557076934UL, 5, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Result_LatLonBounds>(d_);
                return (r_.Tl, r_.Tr, r_.Br, r_.Bl);
            }
        }

        public Task<Mas.Schema.Grid.Grid.ICallback> StreamCells(Mas.Schema.Grid.Grid.RowCol topLeft, Mas.Schema.Grid.Grid.RowCol bottomRight, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Params_StreamCells.WRITER>();
            var arg_ = new Mas.Schema.Grid.Grid.Params_StreamCells()
            {TopLeft = topLeft, BottomRight = bottomRight};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(16440799125557076934UL, 6, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Result_StreamCells>(d_);
                    return (r_.Callback);
                }
            }

            );
        }

        public async Task<string> Unit(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Params_Unit.WRITER>();
            var arg_ = new Mas.Schema.Grid.Grid.Params_Unit()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(16440799125557076934UL, 7, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Result_Unit>(d_);
                return (r_.Unit);
            }
        }

        public async Task<Mas.Schema.Persistence.Persistent.SaveResults> Save(Mas.Schema.Persistence.Persistent.SaveParams arg_, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Persistence.Persistent.SaveParams.WRITER>();
            arg_?.serialize(in_);
            using (var d_ = await Call(13954362354854972261UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Persistence.Persistent.SaveResults>(d_);
                return r_;
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe42973b29661e3c6UL)]
    public class Grid_Skeleton : Skeleton<IGrid>
    {
        public Grid_Skeleton()
        {
            SetMethodTable(ClosestValueAt, Resolution, Dimension, NoDataValue, ValueAt, LatLonBounds, StreamCells, Unit);
        }

        public override ulong InterfaceId => 16440799125557076934UL;
        Task<AnswerOrCounterquestion> ClosestValueAt(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Params_ClosestValueAt>(d_);
                return Impatient.MaybeTailCall(Impl.ClosestValueAt(in_.LatlonCoord, in_.IgnoreNoData, in_.Resolution, in_.Agg, in_.ReturnRowCols, in_.IncludeAggParts, cancellationToken_), (val, tl, br, aggParts) =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Result_ClosestValueAt.WRITER>();
                    var r_ = new Mas.Schema.Grid.Grid.Result_ClosestValueAt{Val = val, Tl = tl, Br = br, AggParts = aggParts};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> Resolution(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Resolution(cancellationToken_), res =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Result_Resolution.WRITER>();
                    var r_ = new Mas.Schema.Grid.Grid.Result_Resolution{Res = res};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> Dimension(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Dimension(cancellationToken_), (rows, cols) =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Result_Dimension.WRITER>();
                    var r_ = new Mas.Schema.Grid.Grid.Result_Dimension{Rows = rows, Cols = cols};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> NoDataValue(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.NoDataValue(cancellationToken_), nodata =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Result_NoDataValue.WRITER>();
                    var r_ = new Mas.Schema.Grid.Grid.Result_NoDataValue{Nodata = nodata};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> ValueAt(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Params_ValueAt>(d_);
                return Impatient.MaybeTailCall(Impl.ValueAt(in_.Row, in_.Col, in_.Resolution, in_.Agg, in_.IncludeAggParts, cancellationToken_), (val, aggParts) =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Result_ValueAt.WRITER>();
                    var r_ = new Mas.Schema.Grid.Grid.Result_ValueAt{Val = val, AggParts = aggParts};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> LatLonBounds(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Params_LatLonBounds>(d_);
                return Impatient.MaybeTailCall(Impl.LatLonBounds(in_.UseCellCenter, cancellationToken_), (tl, tr, br, bl) =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Result_LatLonBounds.WRITER>();
                    var r_ = new Mas.Schema.Grid.Grid.Result_LatLonBounds{Tl = tl, Tr = tr, Br = br, Bl = bl};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> StreamCells(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Params_StreamCells>(d_);
                return Impatient.MaybeTailCall(Impl.StreamCells(in_.TopLeft, in_.BottomRight, cancellationToken_), callback =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Result_StreamCells.WRITER>();
                    var r_ = new Mas.Schema.Grid.Grid.Result_StreamCells{Callback = callback};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> Unit(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Unit(cancellationToken_), unit =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Result_Unit.WRITER>();
                    var r_ = new Mas.Schema.Grid.Grid.Result_Unit{Unit = unit};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Grid
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfe2e0dfae573d9d0UL)]
        public class Value : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfe2e0dfae573d9d0UL;
            public enum WHICH : ushort
            {
                F = 0,
                I = 1,
                Ui = 2,
                No = 3,
                undefined = 65535
            }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                switch (reader.which)
                {
                    case WHICH.F:
                        F = reader.F;
                        break;
                    case WHICH.I:
                        I = reader.I;
                        break;
                    case WHICH.Ui:
                        Ui = reader.Ui;
                        break;
                    case WHICH.No:
                        No = reader.No;
                        break;
                }

                applyDefaults();
            }

            private WHICH _which = WHICH.undefined;
            private object _content;
            public WHICH which
            {
                get => _which;
                set
                {
                    if (value == _which)
                        return;
                    _which = value;
                    switch (value)
                    {
                        case WHICH.F:
                            _content = 0;
                            break;
                        case WHICH.I:
                            _content = 0;
                            break;
                        case WHICH.Ui:
                            _content = 0;
                            break;
                        case WHICH.No:
                            _content = false;
                            break;
                    }
                }
            }

            public void serialize(WRITER writer)
            {
                writer.which = which;
                switch (which)
                {
                    case WHICH.F:
                        writer.F = F.Value;
                        break;
                    case WHICH.I:
                        writer.I = I.Value;
                        break;
                    case WHICH.Ui:
                        writer.Ui = Ui.Value;
                        break;
                    case WHICH.No:
                        writer.No = No.Value;
                        break;
                }
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double? F
            {
                get => _which == WHICH.F ? (double? )_content : null;
                set
                {
                    _which = WHICH.F;
                    _content = value;
                }
            }

            public long? I
            {
                get => _which == WHICH.I ? (long? )_content : null;
                set
                {
                    _which = WHICH.I;
                    _content = value;
                }
            }

            public ulong? Ui
            {
                get => _which == WHICH.Ui ? (ulong? )_content : null;
                set
                {
                    _which = WHICH.Ui;
                    _content = value;
                }
            }

            public bool? No
            {
                get => _which == WHICH.No ? (bool? )_content : null;
                set
                {
                    _which = WHICH.No;
                    _content = value;
                }
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
                public WHICH which => (WHICH)ctx.ReadDataUShort(64U, (ushort)0);
                public double F => which == WHICH.F ? ctx.ReadDataDouble(0UL, 0) : default;
                public long I => which == WHICH.I ? ctx.ReadDataLong(0UL, 0L) : default;
                public ulong Ui => which == WHICH.Ui ? ctx.ReadDataULong(0UL, 0UL) : default;
                public bool No => which == WHICH.No ? ctx.ReadDataBool(0UL, false) : default;
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 0);
                }

                public WHICH which
                {
                    get => (WHICH)this.ReadDataUShort(64U, (ushort)0);
                    set => this.WriteData(64U, (ushort)value, (ushort)0);
                }

                public double F
                {
                    get => which == WHICH.F ? this.ReadDataDouble(0UL, 0) : default;
                    set => this.WriteData(0UL, value, 0);
                }

                public long I
                {
                    get => which == WHICH.I ? this.ReadDataLong(0UL, 0L) : default;
                    set => this.WriteData(0UL, value, 0L);
                }

                public ulong Ui
                {
                    get => which == WHICH.Ui ? this.ReadDataULong(0UL, 0UL) : default;
                    set => this.WriteData(0UL, value, 0UL);
                }

                public bool No
                {
                    get => which == WHICH.No ? this.ReadDataBool(0UL, false) : default;
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa9b6fbdd27e7577bUL)]
        public class Resolution : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa9b6fbdd27e7577bUL;
            public enum WHICH : ushort
            {
                Meter = 0,
                Degree = 1,
                undefined = 65535
            }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                switch (reader.which)
                {
                    case WHICH.Meter:
                        Meter = reader.Meter;
                        break;
                    case WHICH.Degree:
                        Degree = reader.Degree;
                        break;
                }

                applyDefaults();
            }

            private WHICH _which = WHICH.undefined;
            private object _content;
            public WHICH which
            {
                get => _which;
                set
                {
                    if (value == _which)
                        return;
                    _which = value;
                    switch (value)
                    {
                        case WHICH.Meter:
                            _content = 0;
                            break;
                        case WHICH.Degree:
                            _content = 0;
                            break;
                    }
                }
            }

            public void serialize(WRITER writer)
            {
                writer.which = which;
                switch (which)
                {
                    case WHICH.Meter:
                        writer.Meter = Meter.Value;
                        break;
                    case WHICH.Degree:
                        writer.Degree = Degree.Value;
                        break;
                }
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public long? Meter
            {
                get => _which == WHICH.Meter ? (long? )_content : null;
                set
                {
                    _which = WHICH.Meter;
                    _content = value;
                }
            }

            public double? Degree
            {
                get => _which == WHICH.Degree ? (double? )_content : null;
                set
                {
                    _which = WHICH.Degree;
                    _content = value;
                }
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
                public WHICH which => (WHICH)ctx.ReadDataUShort(64U, (ushort)0);
                public long Meter => which == WHICH.Meter ? ctx.ReadDataLong(0UL, 0L) : default;
                public double Degree => which == WHICH.Degree ? ctx.ReadDataDouble(0UL, 0) : default;
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 0);
                }

                public WHICH which
                {
                    get => (WHICH)this.ReadDataUShort(64U, (ushort)0);
                    set => this.WriteData(64U, (ushort)value, (ushort)0);
                }

                public long Meter
                {
                    get => which == WHICH.Meter ? this.ReadDataLong(0UL, 0L) : default;
                    set => this.WriteData(0UL, value, 0L);
                }

                public double Degree
                {
                    get => which == WHICH.Degree ? this.ReadDataDouble(0UL, 0) : default;
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb9e2d85d086206ffUL)]
        public class RowCol : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb9e2d85d086206ffUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Row = reader.Row;
                Col = reader.Col;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Row = Row;
                writer.Col = Col;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public ulong Row
            {
                get;
                set;
            }

            public ulong Col
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
                public ulong Row => ctx.ReadDataULong(0UL, 0UL);
                public ulong Col => ctx.ReadDataULong(64UL, 0UL);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 0);
                }

                public ulong Row
                {
                    get => this.ReadDataULong(0UL, 0UL);
                    set => this.WriteData(0UL, value, 0UL);
                }

                public ulong Col
                {
                    get => this.ReadDataULong(64UL, 0UL);
                    set => this.WriteData(64UL, value, 0UL);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xac444617ef333a1dUL)]
        public class AggregationPart : ICapnpSerializable
        {
            public const UInt64 typeId = 0xac444617ef333a1dUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Value = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Value>(reader.Value);
                RowCol = CapnpSerializable.Create<Mas.Schema.Grid.Grid.RowCol>(reader.RowCol);
                AreaFrac = reader.AreaFrac;
                IValue = reader.IValue;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Value?.serialize(writer.Value);
                RowCol?.serialize(writer.RowCol);
                writer.AreaFrac = AreaFrac;
                writer.IValue = IValue;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Grid.Grid.Value Value
            {
                get;
                set;
            }

            public Mas.Schema.Grid.Grid.RowCol RowCol
            {
                get;
                set;
            }

            public double AreaFrac
            {
                get;
                set;
            }

            public double IValue
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
                public Mas.Schema.Grid.Grid.Value.READER Value => ctx.ReadStruct(0, Mas.Schema.Grid.Grid.Value.READER.create);
                public bool HasValue => ctx.IsStructFieldNonNull(0);
                public Mas.Schema.Grid.Grid.RowCol.READER RowCol => ctx.ReadStruct(1, Mas.Schema.Grid.Grid.RowCol.READER.create);
                public bool HasRowCol => ctx.IsStructFieldNonNull(1);
                public double AreaFrac => ctx.ReadDataDouble(0UL, 0);
                public double IValue => ctx.ReadDataDouble(64UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 2);
                }

                public Mas.Schema.Grid.Grid.Value.WRITER Value
                {
                    get => BuildPointer<Mas.Schema.Grid.Grid.Value.WRITER>(0);
                    set => Link(0, value);
                }

                public Mas.Schema.Grid.Grid.RowCol.WRITER RowCol
                {
                    get => BuildPointer<Mas.Schema.Grid.Grid.RowCol.WRITER>(1);
                    set => Link(1, value);
                }

                public double AreaFrac
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public double IValue
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb55ccf1b9ef18d64UL)]
        public class Location : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb55ccf1b9ef18d64UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                LatLonCoord = CapnpSerializable.Create<Mas.Schema.Geo.LatLonCoord>(reader.LatLonCoord);
                RowCol = CapnpSerializable.Create<Mas.Schema.Grid.Grid.RowCol>(reader.RowCol);
                Value = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Value>(reader.Value);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                LatLonCoord?.serialize(writer.LatLonCoord);
                RowCol?.serialize(writer.RowCol);
                Value?.serialize(writer.Value);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Geo.LatLonCoord LatLonCoord
            {
                get;
                set;
            }

            public Mas.Schema.Grid.Grid.RowCol RowCol
            {
                get;
                set;
            }

            public Mas.Schema.Grid.Grid.Value Value
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
                public Mas.Schema.Geo.LatLonCoord.READER LatLonCoord => ctx.ReadStruct(0, Mas.Schema.Geo.LatLonCoord.READER.create);
                public bool HasLatLonCoord => ctx.IsStructFieldNonNull(0);
                public Mas.Schema.Grid.Grid.RowCol.READER RowCol => ctx.ReadStruct(1, Mas.Schema.Grid.Grid.RowCol.READER.create);
                public bool HasRowCol => ctx.IsStructFieldNonNull(1);
                public Mas.Schema.Grid.Grid.Value.READER Value => ctx.ReadStruct(2, Mas.Schema.Grid.Grid.Value.READER.create);
                public bool HasValue => ctx.IsStructFieldNonNull(2);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 3);
                }

                public Mas.Schema.Geo.LatLonCoord.WRITER LatLonCoord
                {
                    get => BuildPointer<Mas.Schema.Geo.LatLonCoord.WRITER>(0);
                    set => Link(0, value);
                }

                public Mas.Schema.Grid.Grid.RowCol.WRITER RowCol
                {
                    get => BuildPointer<Mas.Schema.Grid.Grid.RowCol.WRITER>(1);
                    set => Link(1, value);
                }

                public Mas.Schema.Grid.Grid.Value.WRITER Value
                {
                    get => BuildPointer<Mas.Schema.Grid.Grid.Value.WRITER>(2);
                    set => Link(2, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd639518280cb55d3UL), Proxy(typeof(Callback_Proxy)), Skeleton(typeof(Callback_Skeleton))]
        public interface ICallback : IDisposable
        {
            Task<IReadOnlyList<Mas.Schema.Grid.Grid.Location>> SendCells(long maxCount, CancellationToken cancellationToken_ = default);
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd639518280cb55d3UL)]
        public class Callback_Proxy : Proxy, ICallback
        {
            public async Task<IReadOnlyList<Mas.Schema.Grid.Grid.Location>> SendCells(long maxCount, CancellationToken cancellationToken_ = default)
            {
                var in_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Callback.Params_SendCells.WRITER>();
                var arg_ = new Mas.Schema.Grid.Grid.Callback.Params_SendCells()
                {MaxCount = maxCount};
                arg_?.serialize(in_);
                using (var d_ = await Call(15436458818737493459UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Callback.Result_SendCells>(d_);
                    return (r_.Locations);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd639518280cb55d3UL)]
        public class Callback_Skeleton : Skeleton<ICallback>
        {
            public Callback_Skeleton()
            {
                SetMethodTable(SendCells);
            }

            public override ulong InterfaceId => 15436458818737493459UL;
            Task<AnswerOrCounterquestion> SendCells(DeserializerState d_, CancellationToken cancellationToken_)
            {
                using (d_)
                {
                    var in_ = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Callback.Params_SendCells>(d_);
                    return Impatient.MaybeTailCall(Impl.SendCells(in_.MaxCount, cancellationToken_), locations =>
                    {
                        var s_ = SerializerState.CreateForRpc<Mas.Schema.Grid.Grid.Callback.Result_SendCells.WRITER>();
                        var r_ = new Mas.Schema.Grid.Grid.Callback.Result_SendCells{Locations = locations};
                        r_.serialize(s_);
                        return s_;
                    }

                    );
                }
            }
        }

        public static class Callback
        {
            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe9b0c7718f68f6bbUL)]
            public class Params_SendCells : ICapnpSerializable
            {
                public const UInt64 typeId = 0xe9b0c7718f68f6bbUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    MaxCount = reader.MaxCount;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.MaxCount = MaxCount;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public long MaxCount
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
                    public long MaxCount => ctx.ReadDataLong(0UL, 0L);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(1, 0);
                    }

                    public long MaxCount
                    {
                        get => this.ReadDataLong(0UL, 0L);
                        set => this.WriteData(0UL, value, 0L);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8e536f6e598b2579UL)]
            public class Result_SendCells : ICapnpSerializable
            {
                public const UInt64 typeId = 0x8e536f6e598b2579UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Locations = reader.Locations?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Grid.Grid.Location>(_));
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Locations.Init(Locations, (_s1, _v1) => _v1?.serialize(_s1));
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public IReadOnlyList<Mas.Schema.Grid.Grid.Location> Locations
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
                    public IReadOnlyList<Mas.Schema.Grid.Grid.Location.READER> Locations => ctx.ReadList(0).Cast(Mas.Schema.Grid.Grid.Location.READER.create);
                    public bool HasLocations => ctx.IsStructFieldNonNull(0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 1);
                    }

                    public ListOfStructsSerializer<Mas.Schema.Grid.Grid.Location.WRITER> Locations
                    {
                        get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Grid.Grid.Location.WRITER>>(0);
                        set => Link(0, value);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xeb7e6f1c610c079aUL)]
        public class Params_ClosestValueAt : ICapnpSerializable
        {
            public const UInt64 typeId = 0xeb7e6f1c610c079aUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                LatlonCoord = CapnpSerializable.Create<Mas.Schema.Geo.LatLonCoord>(reader.LatlonCoord);
                IgnoreNoData = reader.IgnoreNoData;
                Resolution = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Resolution>(reader.Resolution);
                Agg = reader.Agg;
                ReturnRowCols = reader.ReturnRowCols;
                IncludeAggParts = reader.IncludeAggParts;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                LatlonCoord?.serialize(writer.LatlonCoord);
                writer.IgnoreNoData = IgnoreNoData;
                Resolution?.serialize(writer.Resolution);
                writer.Agg = Agg;
                writer.ReturnRowCols = ReturnRowCols;
                writer.IncludeAggParts = IncludeAggParts;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Geo.LatLonCoord LatlonCoord
            {
                get;
                set;
            }

            public bool IgnoreNoData
            {
                get;
                set;
            }

            = true;
            public Mas.Schema.Grid.Grid.Resolution Resolution
            {
                get;
                set;
            }

            public Mas.Schema.Grid.Aggregation Agg
            {
                get;
                set;
            }

            = Mas.Schema.Grid.Aggregation.none;
            public bool ReturnRowCols
            {
                get;
                set;
            }

            = false;
            public bool IncludeAggParts
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
                public Mas.Schema.Geo.LatLonCoord.READER LatlonCoord => ctx.ReadStruct(0, Mas.Schema.Geo.LatLonCoord.READER.create);
                public bool HasLatlonCoord => ctx.IsStructFieldNonNull(0);
                public bool IgnoreNoData => ctx.ReadDataBool(0UL, true);
                public Mas.Schema.Grid.Grid.Resolution.READER Resolution => ctx.ReadStruct(1, Mas.Schema.Grid.Grid.Resolution.READER.create);
                public bool HasResolution => ctx.IsStructFieldNonNull(1);
                public Mas.Schema.Grid.Aggregation Agg => (Mas.Schema.Grid.Aggregation)ctx.ReadDataUShort(16UL, (ushort)0);
                public bool ReturnRowCols => ctx.ReadDataBool(1UL, false);
                public bool IncludeAggParts => ctx.ReadDataBool(2UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 2);
                }

                public Mas.Schema.Geo.LatLonCoord.WRITER LatlonCoord
                {
                    get => BuildPointer<Mas.Schema.Geo.LatLonCoord.WRITER>(0);
                    set => Link(0, value);
                }

                public bool IgnoreNoData
                {
                    get => this.ReadDataBool(0UL, true);
                    set => this.WriteData(0UL, value, true);
                }

                public Mas.Schema.Grid.Grid.Resolution.WRITER Resolution
                {
                    get => BuildPointer<Mas.Schema.Grid.Grid.Resolution.WRITER>(1);
                    set => Link(1, value);
                }

                public Mas.Schema.Grid.Aggregation Agg
                {
                    get => (Mas.Schema.Grid.Aggregation)this.ReadDataUShort(16UL, (ushort)0);
                    set => this.WriteData(16UL, (ushort)value, (ushort)0);
                }

                public bool ReturnRowCols
                {
                    get => this.ReadDataBool(1UL, false);
                    set => this.WriteData(1UL, value, false);
                }

                public bool IncludeAggParts
                {
                    get => this.ReadDataBool(2UL, false);
                    set => this.WriteData(2UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa8bd0263833540b0UL)]
        public class Result_ClosestValueAt : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa8bd0263833540b0UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Val = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Value>(reader.Val);
                Tl = CapnpSerializable.Create<Mas.Schema.Grid.Grid.RowCol>(reader.Tl);
                Br = CapnpSerializable.Create<Mas.Schema.Grid.Grid.RowCol>(reader.Br);
                AggParts = reader.AggParts?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Grid.Grid.AggregationPart>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Val?.serialize(writer.Val);
                Tl?.serialize(writer.Tl);
                Br?.serialize(writer.Br);
                writer.AggParts.Init(AggParts, (_s1, _v1) => _v1?.serialize(_s1));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Grid.Grid.Value Val
            {
                get;
                set;
            }

            public Mas.Schema.Grid.Grid.RowCol Tl
            {
                get;
                set;
            }

            public Mas.Schema.Grid.Grid.RowCol Br
            {
                get;
                set;
            }

            public IReadOnlyList<Mas.Schema.Grid.Grid.AggregationPart> AggParts
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
                public Mas.Schema.Grid.Grid.Value.READER Val => ctx.ReadStruct(0, Mas.Schema.Grid.Grid.Value.READER.create);
                public bool HasVal => ctx.IsStructFieldNonNull(0);
                public Mas.Schema.Grid.Grid.RowCol.READER Tl => ctx.ReadStruct(1, Mas.Schema.Grid.Grid.RowCol.READER.create);
                public bool HasTl => ctx.IsStructFieldNonNull(1);
                public Mas.Schema.Grid.Grid.RowCol.READER Br => ctx.ReadStruct(2, Mas.Schema.Grid.Grid.RowCol.READER.create);
                public bool HasBr => ctx.IsStructFieldNonNull(2);
                public IReadOnlyList<Mas.Schema.Grid.Grid.AggregationPart.READER> AggParts => ctx.ReadList(3).Cast(Mas.Schema.Grid.Grid.AggregationPart.READER.create);
                public bool HasAggParts => ctx.IsStructFieldNonNull(3);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 4);
                }

                public Mas.Schema.Grid.Grid.Value.WRITER Val
                {
                    get => BuildPointer<Mas.Schema.Grid.Grid.Value.WRITER>(0);
                    set => Link(0, value);
                }

                public Mas.Schema.Grid.Grid.RowCol.WRITER Tl
                {
                    get => BuildPointer<Mas.Schema.Grid.Grid.RowCol.WRITER>(1);
                    set => Link(1, value);
                }

                public Mas.Schema.Grid.Grid.RowCol.WRITER Br
                {
                    get => BuildPointer<Mas.Schema.Grid.Grid.RowCol.WRITER>(2);
                    set => Link(2, value);
                }

                public ListOfStructsSerializer<Mas.Schema.Grid.Grid.AggregationPart.WRITER> AggParts
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Grid.Grid.AggregationPart.WRITER>>(3);
                    set => Link(3, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf79edcb97e1e2debUL)]
        public class Params_Resolution : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf79edcb97e1e2debUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8cd7ba490778c79aUL)]
        public class Result_Resolution : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8cd7ba490778c79aUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Res = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Resolution>(reader.Res);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Res?.serialize(writer.Res);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Grid.Grid.Resolution Res
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
                public Mas.Schema.Grid.Grid.Resolution.READER Res => ctx.ReadStruct(0, Mas.Schema.Grid.Grid.Resolution.READER.create);
                public bool HasRes => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Grid.Grid.Resolution.WRITER Res
                {
                    get => BuildPointer<Mas.Schema.Grid.Grid.Resolution.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa6005af20cc08dbeUL)]
        public class Params_Dimension : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa6005af20cc08dbeUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe7a46f6b1610256fUL)]
        public class Result_Dimension : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe7a46f6b1610256fUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Rows = reader.Rows;
                Cols = reader.Cols;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Rows = Rows;
                writer.Cols = Cols;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public ulong Rows
            {
                get;
                set;
            }

            public ulong Cols
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
                public ulong Rows => ctx.ReadDataULong(0UL, 0UL);
                public ulong Cols => ctx.ReadDataULong(64UL, 0UL);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 0);
                }

                public ulong Rows
                {
                    get => this.ReadDataULong(0UL, 0UL);
                    set => this.WriteData(0UL, value, 0UL);
                }

                public ulong Cols
                {
                    get => this.ReadDataULong(64UL, 0UL);
                    set => this.WriteData(64UL, value, 0UL);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf804a76f2ada54b6UL)]
        public class Params_NoDataValue : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf804a76f2ada54b6UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9bc132bd2a1b1fcfUL)]
        public class Result_NoDataValue : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9bc132bd2a1b1fcfUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Nodata = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Value>(reader.Nodata);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Nodata?.serialize(writer.Nodata);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Grid.Grid.Value Nodata
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
                public Mas.Schema.Grid.Grid.Value.READER Nodata => ctx.ReadStruct(0, Mas.Schema.Grid.Grid.Value.READER.create);
                public bool HasNodata => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Grid.Grid.Value.WRITER Nodata
                {
                    get => BuildPointer<Mas.Schema.Grid.Grid.Value.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x948ff2bdd6e6972fUL)]
        public class Params_ValueAt : ICapnpSerializable
        {
            public const UInt64 typeId = 0x948ff2bdd6e6972fUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Row = reader.Row;
                Col = reader.Col;
                Resolution = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Resolution>(reader.Resolution);
                Agg = reader.Agg;
                IncludeAggParts = reader.IncludeAggParts;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Row = Row;
                writer.Col = Col;
                Resolution?.serialize(writer.Resolution);
                writer.Agg = Agg;
                writer.IncludeAggParts = IncludeAggParts;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public ulong Row
            {
                get;
                set;
            }

            public ulong Col
            {
                get;
                set;
            }

            public Mas.Schema.Grid.Grid.Resolution Resolution
            {
                get;
                set;
            }

            public Mas.Schema.Grid.Aggregation Agg
            {
                get;
                set;
            }

            = Mas.Schema.Grid.Aggregation.none;
            public bool IncludeAggParts
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
                public ulong Row => ctx.ReadDataULong(0UL, 0UL);
                public ulong Col => ctx.ReadDataULong(64UL, 0UL);
                public Mas.Schema.Grid.Grid.Resolution.READER Resolution => ctx.ReadStruct(0, Mas.Schema.Grid.Grid.Resolution.READER.create);
                public bool HasResolution => ctx.IsStructFieldNonNull(0);
                public Mas.Schema.Grid.Aggregation Agg => (Mas.Schema.Grid.Aggregation)ctx.ReadDataUShort(128UL, (ushort)0);
                public bool IncludeAggParts => ctx.ReadDataBool(144UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(3, 1);
                }

                public ulong Row
                {
                    get => this.ReadDataULong(0UL, 0UL);
                    set => this.WriteData(0UL, value, 0UL);
                }

                public ulong Col
                {
                    get => this.ReadDataULong(64UL, 0UL);
                    set => this.WriteData(64UL, value, 0UL);
                }

                public Mas.Schema.Grid.Grid.Resolution.WRITER Resolution
                {
                    get => BuildPointer<Mas.Schema.Grid.Grid.Resolution.WRITER>(0);
                    set => Link(0, value);
                }

                public Mas.Schema.Grid.Aggregation Agg
                {
                    get => (Mas.Schema.Grid.Aggregation)this.ReadDataUShort(128UL, (ushort)0);
                    set => this.WriteData(128UL, (ushort)value, (ushort)0);
                }

                public bool IncludeAggParts
                {
                    get => this.ReadDataBool(144UL, false);
                    set => this.WriteData(144UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa21ef33efc715994UL)]
        public class Result_ValueAt : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa21ef33efc715994UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Val = CapnpSerializable.Create<Mas.Schema.Grid.Grid.Value>(reader.Val);
                AggParts = reader.AggParts?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Grid.Grid.AggregationPart>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Val?.serialize(writer.Val);
                writer.AggParts.Init(AggParts, (_s1, _v1) => _v1?.serialize(_s1));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Grid.Grid.Value Val
            {
                get;
                set;
            }

            public IReadOnlyList<Mas.Schema.Grid.Grid.AggregationPart> AggParts
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
                public Mas.Schema.Grid.Grid.Value.READER Val => ctx.ReadStruct(0, Mas.Schema.Grid.Grid.Value.READER.create);
                public bool HasVal => ctx.IsStructFieldNonNull(0);
                public IReadOnlyList<Mas.Schema.Grid.Grid.AggregationPart.READER> AggParts => ctx.ReadList(1).Cast(Mas.Schema.Grid.Grid.AggregationPart.READER.create);
                public bool HasAggParts => ctx.IsStructFieldNonNull(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public Mas.Schema.Grid.Grid.Value.WRITER Val
                {
                    get => BuildPointer<Mas.Schema.Grid.Grid.Value.WRITER>(0);
                    set => Link(0, value);
                }

                public ListOfStructsSerializer<Mas.Schema.Grid.Grid.AggregationPart.WRITER> AggParts
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Grid.Grid.AggregationPart.WRITER>>(1);
                    set => Link(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf37338992466bd97UL)]
        public class Params_LatLonBounds : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf37338992466bd97UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                UseCellCenter = reader.UseCellCenter;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.UseCellCenter = UseCellCenter;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool UseCellCenter
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
                public bool UseCellCenter => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool UseCellCenter
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe57fce57d3443377UL)]
        public class Result_LatLonBounds : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe57fce57d3443377UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Tl = CapnpSerializable.Create<Mas.Schema.Geo.LatLonCoord>(reader.Tl);
                Tr = CapnpSerializable.Create<Mas.Schema.Geo.LatLonCoord>(reader.Tr);
                Br = CapnpSerializable.Create<Mas.Schema.Geo.LatLonCoord>(reader.Br);
                Bl = CapnpSerializable.Create<Mas.Schema.Geo.LatLonCoord>(reader.Bl);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Tl?.serialize(writer.Tl);
                Tr?.serialize(writer.Tr);
                Br?.serialize(writer.Br);
                Bl?.serialize(writer.Bl);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Geo.LatLonCoord Tl
            {
                get;
                set;
            }

            public Mas.Schema.Geo.LatLonCoord Tr
            {
                get;
                set;
            }

            public Mas.Schema.Geo.LatLonCoord Br
            {
                get;
                set;
            }

            public Mas.Schema.Geo.LatLonCoord Bl
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
                public Mas.Schema.Geo.LatLonCoord.READER Tl => ctx.ReadStruct(0, Mas.Schema.Geo.LatLonCoord.READER.create);
                public bool HasTl => ctx.IsStructFieldNonNull(0);
                public Mas.Schema.Geo.LatLonCoord.READER Tr => ctx.ReadStruct(1, Mas.Schema.Geo.LatLonCoord.READER.create);
                public bool HasTr => ctx.IsStructFieldNonNull(1);
                public Mas.Schema.Geo.LatLonCoord.READER Br => ctx.ReadStruct(2, Mas.Schema.Geo.LatLonCoord.READER.create);
                public bool HasBr => ctx.IsStructFieldNonNull(2);
                public Mas.Schema.Geo.LatLonCoord.READER Bl => ctx.ReadStruct(3, Mas.Schema.Geo.LatLonCoord.READER.create);
                public bool HasBl => ctx.IsStructFieldNonNull(3);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 4);
                }

                public Mas.Schema.Geo.LatLonCoord.WRITER Tl
                {
                    get => BuildPointer<Mas.Schema.Geo.LatLonCoord.WRITER>(0);
                    set => Link(0, value);
                }

                public Mas.Schema.Geo.LatLonCoord.WRITER Tr
                {
                    get => BuildPointer<Mas.Schema.Geo.LatLonCoord.WRITER>(1);
                    set => Link(1, value);
                }

                public Mas.Schema.Geo.LatLonCoord.WRITER Br
                {
                    get => BuildPointer<Mas.Schema.Geo.LatLonCoord.WRITER>(2);
                    set => Link(2, value);
                }

                public Mas.Schema.Geo.LatLonCoord.WRITER Bl
                {
                    get => BuildPointer<Mas.Schema.Geo.LatLonCoord.WRITER>(3);
                    set => Link(3, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd9add1b3fdcfdbbaUL)]
        public class Params_StreamCells : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd9add1b3fdcfdbbaUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                TopLeft = CapnpSerializable.Create<Mas.Schema.Grid.Grid.RowCol>(reader.TopLeft);
                BottomRight = CapnpSerializable.Create<Mas.Schema.Grid.Grid.RowCol>(reader.BottomRight);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                TopLeft?.serialize(writer.TopLeft);
                BottomRight?.serialize(writer.BottomRight);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Grid.Grid.RowCol TopLeft
            {
                get;
                set;
            }

            public Mas.Schema.Grid.Grid.RowCol BottomRight
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
                public Mas.Schema.Grid.Grid.RowCol.READER TopLeft => ctx.ReadStruct(0, Mas.Schema.Grid.Grid.RowCol.READER.create);
                public bool HasTopLeft => ctx.IsStructFieldNonNull(0);
                public Mas.Schema.Grid.Grid.RowCol.READER BottomRight => ctx.ReadStruct(1, Mas.Schema.Grid.Grid.RowCol.READER.create);
                public bool HasBottomRight => ctx.IsStructFieldNonNull(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public Mas.Schema.Grid.Grid.RowCol.WRITER TopLeft
                {
                    get => BuildPointer<Mas.Schema.Grid.Grid.RowCol.WRITER>(0);
                    set => Link(0, value);
                }

                public Mas.Schema.Grid.Grid.RowCol.WRITER BottomRight
                {
                    get => BuildPointer<Mas.Schema.Grid.Grid.RowCol.WRITER>(1);
                    set => Link(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9b8dd52b78a7ebd2UL)]
        public class Result_StreamCells : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9b8dd52b78a7ebd2UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Callback = reader.Callback;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Callback = Callback;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Grid.Grid.ICallback Callback
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
                public Mas.Schema.Grid.Grid.ICallback Callback => ctx.ReadCap<Mas.Schema.Grid.Grid.ICallback>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Grid.Grid.ICallback Callback
                {
                    get => ReadCap<Mas.Schema.Grid.Grid.ICallback>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbb4e4368bb6a6748UL)]
        public class Params_Unit : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbb4e4368bb6a6748UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd170e76dbd9fc4fbUL)]
        public class Result_Unit : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd170e76dbd9fc4fbUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Unit = reader.Unit;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Unit = Unit;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Unit
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
                public string Unit => ctx.ReadText(0, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Unit
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }
    }
}