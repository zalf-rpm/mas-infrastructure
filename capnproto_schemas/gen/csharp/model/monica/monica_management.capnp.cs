using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Schema.Model.Monica
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa1f99f32eea02590UL)]
    public class ILRDates : ICapnpSerializable
    {
        public const UInt64 typeId = 0xa1f99f32eea02590UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Sowing = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.Sowing);
            EarliestSowing = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.EarliestSowing);
            LatestSowing = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.LatestSowing);
            Harvest = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.Harvest);
            LatestHarvest = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.LatestHarvest);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            Sowing?.serialize(writer.Sowing);
            EarliestSowing?.serialize(writer.EarliestSowing);
            LatestSowing?.serialize(writer.LatestSowing);
            Harvest?.serialize(writer.Harvest);
            LatestHarvest?.serialize(writer.LatestHarvest);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Schema.Common.Date Sowing
        {
            get;
            set;
        }

        public Mas.Schema.Common.Date EarliestSowing
        {
            get;
            set;
        }

        public Mas.Schema.Common.Date LatestSowing
        {
            get;
            set;
        }

        public Mas.Schema.Common.Date Harvest
        {
            get;
            set;
        }

        public Mas.Schema.Common.Date LatestHarvest
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
            public Mas.Schema.Common.Date.READER Sowing => ctx.ReadStruct(0, Mas.Schema.Common.Date.READER.create);
            public bool HasSowing => ctx.IsStructFieldNonNull(0);
            public Mas.Schema.Common.Date.READER EarliestSowing => ctx.ReadStruct(1, Mas.Schema.Common.Date.READER.create);
            public bool HasEarliestSowing => ctx.IsStructFieldNonNull(1);
            public Mas.Schema.Common.Date.READER LatestSowing => ctx.ReadStruct(2, Mas.Schema.Common.Date.READER.create);
            public bool HasLatestSowing => ctx.IsStructFieldNonNull(2);
            public Mas.Schema.Common.Date.READER Harvest => ctx.ReadStruct(3, Mas.Schema.Common.Date.READER.create);
            public bool HasHarvest => ctx.IsStructFieldNonNull(3);
            public Mas.Schema.Common.Date.READER LatestHarvest => ctx.ReadStruct(4, Mas.Schema.Common.Date.READER.create);
            public bool HasLatestHarvest => ctx.IsStructFieldNonNull(4);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 5);
            }

            public Mas.Schema.Common.Date.WRITER Sowing
            {
                get => BuildPointer<Mas.Schema.Common.Date.WRITER>(0);
                set => Link(0, value);
            }

            public Mas.Schema.Common.Date.WRITER EarliestSowing
            {
                get => BuildPointer<Mas.Schema.Common.Date.WRITER>(1);
                set => Link(1, value);
            }

            public Mas.Schema.Common.Date.WRITER LatestSowing
            {
                get => BuildPointer<Mas.Schema.Common.Date.WRITER>(2);
                set => Link(2, value);
            }

            public Mas.Schema.Common.Date.WRITER Harvest
            {
                get => BuildPointer<Mas.Schema.Common.Date.WRITER>(3);
                set => Link(3, value);
            }

            public Mas.Schema.Common.Date.WRITER LatestHarvest
            {
                get => BuildPointer<Mas.Schema.Common.Date.WRITER>(4);
                set => Link(4, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x94aa9c195abf0a1aUL)]
    public enum MineralFertilizer : ushort
    {
        ahls,
        alzon,
        an,
        ap,
        @as,
        ash,
        cf4,
        cp1,
        cp2,
        cp3,
        npk,
        ns,
        u,
        uan,
        uas,
        uni
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfd676465ae0b8cf0UL)]
    public enum OrganicFertilizer : ushort
    {
        ash,
        cadlm,
        cam,
        cas,
        cau,
        dgdlm,
        gwc,
        hodlm,
        mc,
        ms,
        oic,
        pidlm,
        pim,
        pis,
        piu,
        piudk,
        plw,
        podlm,
        pom,
        soy,
        ss,
        tudlm,
        weeds,
        ws
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd0290daf8de9f2b0UL)]
    public enum EventType : ushort
    {
        sowing,
        automaticSowing,
        harvest,
        automaticHarvest,
        irrigation,
        tillage,
        organicFertilization,
        mineralFertilization,
        nDemandFertilization,
        cutting,
        setValue,
        saveState
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb33447204cdf022cUL)]
    public enum PlantOrgan : ushort
    {
        root,
        leaf,
        shoot,
        fruit,
        strukt,
        sugar
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcf672ab379467704UL)]
    public class Event : ICapnpSerializable
    {
        public const UInt64 typeId = 0xcf672ab379467704UL;
        public enum WHICH : ushort
        {
            At = 0,
            Between = 1,
            After = 2,
            undefined = 65535
        }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            switch (reader.which)
            {
                case WHICH.At:
                    At = CapnpSerializable.Create<Mas.Schema.Model.Monica.Event.at>(reader.At);
                    break;
                case WHICH.Between:
                    Between = CapnpSerializable.Create<Mas.Schema.Model.Monica.Event.between>(reader.Between);
                    break;
                case WHICH.After:
                    After = CapnpSerializable.Create<Mas.Schema.Model.Monica.Event.after>(reader.After);
                    break;
            }

            TheType = reader.TheType;
            Info = CapnpSerializable.Create<Mas.Schema.Common.IdInformation>(reader.Info);
            Params = CapnpSerializable.Create<object>(reader.Params);
            RunAtStartOfDay = reader.RunAtStartOfDay;
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
                    case WHICH.At:
                        _content = null;
                        break;
                    case WHICH.Between:
                        _content = null;
                        break;
                    case WHICH.After:
                        _content = null;
                        break;
                }
            }
        }

        public void serialize(WRITER writer)
        {
            writer.which = which;
            switch (which)
            {
                case WHICH.At:
                    At?.serialize(writer.At);
                    break;
                case WHICH.Between:
                    Between?.serialize(writer.Between);
                    break;
                case WHICH.After:
                    After?.serialize(writer.After);
                    break;
            }

            writer.TheType = TheType;
            Info?.serialize(writer.Info);
            writer.Params.SetObject(Params);
            writer.RunAtStartOfDay = RunAtStartOfDay;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Schema.Model.Monica.Event.ExternalType TheType
        {
            get;
            set;
        }

        public Mas.Schema.Common.IdInformation Info
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.Event.at At
        {
            get => _which == WHICH.At ? (Mas.Schema.Model.Monica.Event.at)_content : null;
            set
            {
                _which = WHICH.At;
                _content = value;
            }
        }

        public Mas.Schema.Model.Monica.Event.between Between
        {
            get => _which == WHICH.Between ? (Mas.Schema.Model.Monica.Event.between)_content : null;
            set
            {
                _which = WHICH.Between;
                _content = value;
            }
        }

        public Mas.Schema.Model.Monica.Event.after After
        {
            get => _which == WHICH.After ? (Mas.Schema.Model.Monica.Event.after)_content : null;
            set
            {
                _which = WHICH.After;
                _content = value;
            }
        }

        public object Params
        {
            get;
            set;
        }

        public bool RunAtStartOfDay
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
            public WHICH which => (WHICH)ctx.ReadDataUShort(16U, (ushort)0);
            public Mas.Schema.Model.Monica.Event.ExternalType TheType => (Mas.Schema.Model.Monica.Event.ExternalType)ctx.ReadDataUShort(0UL, (ushort)0);
            public Mas.Schema.Common.IdInformation.READER Info => ctx.ReadStruct(0, Mas.Schema.Common.IdInformation.READER.create);
            public bool HasInfo => ctx.IsStructFieldNonNull(0);
            public at.READER At => which == WHICH.At ? new at.READER(ctx) : default;
            public between.READER Between => which == WHICH.Between ? new between.READER(ctx) : default;
            public after.READER After => which == WHICH.After ? new after.READER(ctx) : default;
            public DeserializerState Params => ctx.StructReadPointer(3);
            public bool RunAtStartOfDay => ctx.ReadDataBool(48UL, false);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 4);
            }

            public WHICH which
            {
                get => (WHICH)this.ReadDataUShort(16U, (ushort)0);
                set => this.WriteData(16U, (ushort)value, (ushort)0);
            }

            public Mas.Schema.Model.Monica.Event.ExternalType TheType
            {
                get => (Mas.Schema.Model.Monica.Event.ExternalType)this.ReadDataUShort(0UL, (ushort)0);
                set => this.WriteData(0UL, (ushort)value, (ushort)0);
            }

            public Mas.Schema.Common.IdInformation.WRITER Info
            {
                get => BuildPointer<Mas.Schema.Common.IdInformation.WRITER>(0);
                set => Link(0, value);
            }

            public at.WRITER At
            {
                get => which == WHICH.At ? Rewrap<at.WRITER>() : default;
            }

            public between.WRITER Between
            {
                get => which == WHICH.Between ? Rewrap<between.WRITER>() : default;
            }

            public after.WRITER After
            {
                get => which == WHICH.After ? Rewrap<after.WRITER>() : default;
            }

            public DynamicSerializerState Params
            {
                get => BuildPointer<DynamicSerializerState>(3);
                set => Link(3, value);
            }

            public bool RunAtStartOfDay
            {
                get => this.ReadDataBool(48UL, false);
                set => this.WriteData(48UL, value, false);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdb4674cbf3154bfaUL)]
        public class at : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdb4674cbf3154bfaUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Date = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.Date);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Date?.serialize(writer.Date);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Common.Date Date
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
                public Mas.Schema.Common.Date.READER Date => ctx.ReadStruct(1, Mas.Schema.Common.Date.READER.create);
                public bool HasDate => ctx.IsStructFieldNonNull(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                }

                public Mas.Schema.Common.Date.WRITER Date
                {
                    get => BuildPointer<Mas.Schema.Common.Date.WRITER>(1);
                    set => Link(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc8479e0f1798b1fcUL)]
        public class between : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc8479e0f1798b1fcUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Earliest = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.Earliest);
                Latest = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.Latest);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Earliest?.serialize(writer.Earliest);
                Latest?.serialize(writer.Latest);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Common.Date Earliest
            {
                get;
                set;
            }

            public Mas.Schema.Common.Date Latest
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
                public Mas.Schema.Common.Date.READER Earliest => ctx.ReadStruct(1, Mas.Schema.Common.Date.READER.create);
                public bool HasEarliest => ctx.IsStructFieldNonNull(1);
                public Mas.Schema.Common.Date.READER Latest => ctx.ReadStruct(2, Mas.Schema.Common.Date.READER.create);
                public bool HasLatest => ctx.IsStructFieldNonNull(2);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                }

                public Mas.Schema.Common.Date.WRITER Earliest
                {
                    get => BuildPointer<Mas.Schema.Common.Date.WRITER>(1);
                    set => Link(1, value);
                }

                public Mas.Schema.Common.Date.WRITER Latest
                {
                    get => BuildPointer<Mas.Schema.Common.Date.WRITER>(2);
                    set => Link(2, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbeb6ab7b0e6b585eUL)]
        public class after : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbeb6ab7b0e6b585eUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Event = CapnpSerializable.Create<Mas.Schema.Model.Monica.Event.Type>(reader.Event);
                Days = reader.Days;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Event?.serialize(writer.Event);
                writer.Days = Days;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Model.Monica.Event.Type Event
            {
                get;
                set;
            }

            public ushort Days
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
                public Mas.Schema.Model.Monica.Event.Type.READER Event => ctx.ReadStruct(1, Mas.Schema.Model.Monica.Event.Type.READER.create);
                public bool HasEvent => ctx.IsStructFieldNonNull(1);
                public ushort Days => ctx.ReadDataUShort(32UL, (ushort)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                }

                public Mas.Schema.Model.Monica.Event.Type.WRITER Event
                {
                    get => BuildPointer<Mas.Schema.Model.Monica.Event.Type.WRITER>(1);
                    set => Link(1, value);
                }

                public ushort Days
                {
                    get => this.ReadDataUShort(32UL, (ushort)0);
                    set => this.WriteData(32UL, value, (ushort)0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe5484dc513ee11e0UL)]
        public enum ExternalType : ushort
        {
            sowing,
            automaticSowing,
            harvest,
            automaticHarvest,
            irrigation,
            tillage,
            organicFertilization,
            mineralFertilization,
            nDemandFertilization,
            cutting
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb2bf3a5557791bc1UL)]
        public enum PhenoStage : ushort
        {
            emergence,
            flowering,
            anthesis,
            maturity
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb91010c363e568a4UL)]
        public class Type : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb91010c363e568a4UL;
            public enum WHICH : ushort
            {
                External = 0,
                Internal = 1,
                undefined = 65535
            }

            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                switch (reader.which)
                {
                    case WHICH.External:
                        External = reader.External;
                        break;
                    case WHICH.Internal:
                        Internal = reader.Internal;
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
                        case WHICH.External:
                            _content = (Mas.Schema.Model.Monica.Event.ExternalType)0;
                            break;
                        case WHICH.Internal:
                            _content = (Mas.Schema.Model.Monica.Event.PhenoStage)0;
                            break;
                    }
                }
            }

            public void serialize(WRITER writer)
            {
                writer.which = which;
                switch (which)
                {
                    case WHICH.External:
                        writer.External = External.Value;
                        break;
                    case WHICH.Internal:
                        writer.Internal = Internal.Value;
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

            public Mas.Schema.Model.Monica.Event.ExternalType? External
            {
                get => _which == WHICH.External ? (Mas.Schema.Model.Monica.Event.ExternalType? )_content : null;
                set
                {
                    _which = WHICH.External;
                    _content = value;
                }
            }

            public Mas.Schema.Model.Monica.Event.PhenoStage? Internal
            {
                get => _which == WHICH.Internal ? (Mas.Schema.Model.Monica.Event.PhenoStage? )_content : null;
                set
                {
                    _which = WHICH.Internal;
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
                public WHICH which => (WHICH)ctx.ReadDataUShort(16U, (ushort)0);
                public Mas.Schema.Model.Monica.Event.ExternalType External => which == WHICH.External ? (Mas.Schema.Model.Monica.Event.ExternalType)ctx.ReadDataUShort(0UL, (ushort)0) : default;
                public Mas.Schema.Model.Monica.Event.PhenoStage Internal => which == WHICH.Internal ? (Mas.Schema.Model.Monica.Event.PhenoStage)ctx.ReadDataUShort(0UL, (ushort)0) : default;
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public WHICH which
                {
                    get => (WHICH)this.ReadDataUShort(16U, (ushort)0);
                    set => this.WriteData(16U, (ushort)value, (ushort)0);
                }

                public Mas.Schema.Model.Monica.Event.ExternalType External
                {
                    get => which == WHICH.External ? (Mas.Schema.Model.Monica.Event.ExternalType)this.ReadDataUShort(0UL, (ushort)0) : default;
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }

                public Mas.Schema.Model.Monica.Event.PhenoStage Internal
                {
                    get => which == WHICH.Internal ? (Mas.Schema.Model.Monica.Event.PhenoStage)this.ReadDataUShort(0UL, (ushort)0) : default;
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcb20e21466098705UL)]
    public class Params : ICapnpSerializable
    {
        public const UInt64 typeId = 0xcb20e21466098705UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc6880d1c13ec14dcUL)]
        public class Sowing : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc6880d1c13ec14dcUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Cultivar = reader.Cultivar;
                PlantDensity = reader.PlantDensity;
                Crop = reader.Crop;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Cultivar = Cultivar;
                writer.PlantDensity = PlantDensity;
                writer.Crop = Crop;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Crop.Cultivar Cultivar
            {
                get;
                set;
            }

            public ushort PlantDensity
            {
                get;
                set;
            }

            = 0;
            public Mas.Schema.Crop.ICrop Crop
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
                public Mas.Schema.Crop.Cultivar Cultivar => (Mas.Schema.Crop.Cultivar)ctx.ReadDataUShort(0UL, (ushort)0);
                public ushort PlantDensity => ctx.ReadDataUShort(16UL, (ushort)0);
                public Mas.Schema.Crop.ICrop Crop => ctx.ReadCap<Mas.Schema.Crop.ICrop>(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public Mas.Schema.Crop.Cultivar Cultivar
                {
                    get => (Mas.Schema.Crop.Cultivar)this.ReadDataUShort(0UL, (ushort)0);
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }

                public ushort PlantDensity
                {
                    get => this.ReadDataUShort(16UL, (ushort)0);
                    set => this.WriteData(16UL, value, (ushort)0);
                }

                public Mas.Schema.Crop.ICrop Crop
                {
                    get => ReadCap<Mas.Schema.Crop.ICrop>(0);
                    set => LinkObject(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd1bfc1c9617d9453UL)]
        public class AutomaticSowing : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd1bfc1c9617d9453UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                MinTempThreshold = reader.MinTempThreshold;
                DaysInTempWindow = reader.DaysInTempWindow;
                MinPercentASW = reader.MinPercentASW;
                MaxPercentASW = reader.MaxPercentASW;
                Max3dayPrecipSum = reader.Max3dayPrecipSum;
                MaxCurrentDayPrecipSum = reader.MaxCurrentDayPrecipSum;
                TempSumAboveBaseTemp = reader.TempSumAboveBaseTemp;
                BaseTemp = reader.BaseTemp;
                TheAvgSoilTemp = CapnpSerializable.Create<Mas.Schema.Model.Monica.Params.AutomaticSowing.AvgSoilTemp>(reader.TheAvgSoilTemp);
                Sowing = CapnpSerializable.Create<Mas.Schema.Model.Monica.Params.Sowing>(reader.Sowing);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.MinTempThreshold = MinTempThreshold;
                writer.DaysInTempWindow = DaysInTempWindow;
                writer.MinPercentASW = MinPercentASW;
                writer.MaxPercentASW = MaxPercentASW;
                writer.Max3dayPrecipSum = Max3dayPrecipSum;
                writer.MaxCurrentDayPrecipSum = MaxCurrentDayPrecipSum;
                writer.TempSumAboveBaseTemp = TempSumAboveBaseTemp;
                writer.BaseTemp = BaseTemp;
                TheAvgSoilTemp?.serialize(writer.TheAvgSoilTemp);
                Sowing?.serialize(writer.Sowing);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double MinTempThreshold
            {
                get;
                set;
            }

            public ushort DaysInTempWindow
            {
                get;
                set;
            }

            public double MinPercentASW
            {
                get;
                set;
            }

            = 0;
            public double MaxPercentASW
            {
                get;
                set;
            }

            = 100;
            public double Max3dayPrecipSum
            {
                get;
                set;
            }

            public double MaxCurrentDayPrecipSum
            {
                get;
                set;
            }

            public double TempSumAboveBaseTemp
            {
                get;
                set;
            }

            public double BaseTemp
            {
                get;
                set;
            }

            public Mas.Schema.Model.Monica.Params.AutomaticSowing.AvgSoilTemp TheAvgSoilTemp
            {
                get;
                set;
            }

            public Mas.Schema.Model.Monica.Params.Sowing Sowing
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
                public double MinTempThreshold => ctx.ReadDataDouble(0UL, 0);
                public ushort DaysInTempWindow => ctx.ReadDataUShort(64UL, (ushort)0);
                public double MinPercentASW => ctx.ReadDataDouble(128UL, 0);
                public double MaxPercentASW => ctx.ReadDataDouble(192UL, 100);
                public double Max3dayPrecipSum => ctx.ReadDataDouble(256UL, 0);
                public double MaxCurrentDayPrecipSum => ctx.ReadDataDouble(320UL, 0);
                public double TempSumAboveBaseTemp => ctx.ReadDataDouble(384UL, 0);
                public double BaseTemp => ctx.ReadDataDouble(448UL, 0);
                public Mas.Schema.Model.Monica.Params.AutomaticSowing.AvgSoilTemp.READER TheAvgSoilTemp => ctx.ReadStruct(0, Mas.Schema.Model.Monica.Params.AutomaticSowing.AvgSoilTemp.READER.create);
                public bool HasTheAvgSoilTemp => ctx.IsStructFieldNonNull(0);
                public Mas.Schema.Model.Monica.Params.Sowing.READER Sowing => ctx.ReadStruct(1, Mas.Schema.Model.Monica.Params.Sowing.READER.create);
                public bool HasSowing => ctx.IsStructFieldNonNull(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(8, 2);
                }

                public double MinTempThreshold
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public ushort DaysInTempWindow
                {
                    get => this.ReadDataUShort(64UL, (ushort)0);
                    set => this.WriteData(64UL, value, (ushort)0);
                }

                public double MinPercentASW
                {
                    get => this.ReadDataDouble(128UL, 0);
                    set => this.WriteData(128UL, value, 0);
                }

                public double MaxPercentASW
                {
                    get => this.ReadDataDouble(192UL, 100);
                    set => this.WriteData(192UL, value, 100);
                }

                public double Max3dayPrecipSum
                {
                    get => this.ReadDataDouble(256UL, 0);
                    set => this.WriteData(256UL, value, 0);
                }

                public double MaxCurrentDayPrecipSum
                {
                    get => this.ReadDataDouble(320UL, 0);
                    set => this.WriteData(320UL, value, 0);
                }

                public double TempSumAboveBaseTemp
                {
                    get => this.ReadDataDouble(384UL, 0);
                    set => this.WriteData(384UL, value, 0);
                }

                public double BaseTemp
                {
                    get => this.ReadDataDouble(448UL, 0);
                    set => this.WriteData(448UL, value, 0);
                }

                public Mas.Schema.Model.Monica.Params.AutomaticSowing.AvgSoilTemp.WRITER TheAvgSoilTemp
                {
                    get => BuildPointer<Mas.Schema.Model.Monica.Params.AutomaticSowing.AvgSoilTemp.WRITER>(0);
                    set => Link(0, value);
                }

                public Mas.Schema.Model.Monica.Params.Sowing.WRITER Sowing
                {
                    get => BuildPointer<Mas.Schema.Model.Monica.Params.Sowing.WRITER>(1);
                    set => Link(1, value);
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x846f567433b186d1UL)]
            public class AvgSoilTemp : ICapnpSerializable
            {
                public const UInt64 typeId = 0x846f567433b186d1UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    SoilDepthForAveraging = reader.SoilDepthForAveraging;
                    DaysInSoilTempWindow = reader.DaysInSoilTempWindow;
                    SowingIfAboveAvgSoilTemp = reader.SowingIfAboveAvgSoilTemp;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.SoilDepthForAveraging = SoilDepthForAveraging;
                    writer.DaysInSoilTempWindow = DaysInSoilTempWindow;
                    writer.SowingIfAboveAvgSoilTemp = SowingIfAboveAvgSoilTemp;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public double SoilDepthForAveraging
                {
                    get;
                    set;
                }

                = 0.3;
                public ushort DaysInSoilTempWindow
                {
                    get;
                    set;
                }

                public double SowingIfAboveAvgSoilTemp
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
                    public double SoilDepthForAveraging => ctx.ReadDataDouble(0UL, 0.3);
                    public ushort DaysInSoilTempWindow => ctx.ReadDataUShort(64UL, (ushort)0);
                    public double SowingIfAboveAvgSoilTemp => ctx.ReadDataDouble(128UL, 0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(3, 0);
                    }

                    public double SoilDepthForAveraging
                    {
                        get => this.ReadDataDouble(0UL, 0.3);
                        set => this.WriteData(0UL, value, 0.3);
                    }

                    public ushort DaysInSoilTempWindow
                    {
                        get => this.ReadDataUShort(64UL, (ushort)0);
                        set => this.WriteData(64UL, value, (ushort)0);
                    }

                    public double SowingIfAboveAvgSoilTemp
                    {
                        get => this.ReadDataDouble(128UL, 0);
                        set => this.WriteData(128UL, value, 0);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8feb941d70f2a468UL)]
        public class Harvest : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8feb941d70f2a468UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Exported = reader.Exported;
                OptCarbMgmtData = CapnpSerializable.Create<Mas.Schema.Model.Monica.Params.Harvest.OptCarbonMgmtData>(reader.OptCarbMgmtData);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Exported = Exported;
                OptCarbMgmtData?.serialize(writer.OptCarbMgmtData);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Exported
            {
                get;
                set;
            }

            = true;
            public Mas.Schema.Model.Monica.Params.Harvest.OptCarbonMgmtData OptCarbMgmtData
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
                public bool Exported => ctx.ReadDataBool(0UL, true);
                public Mas.Schema.Model.Monica.Params.Harvest.OptCarbonMgmtData.READER OptCarbMgmtData => ctx.ReadStruct(0, Mas.Schema.Model.Monica.Params.Harvest.OptCarbonMgmtData.READER.create);
                public bool HasOptCarbMgmtData => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public bool Exported
                {
                    get => this.ReadDataBool(0UL, true);
                    set => this.WriteData(0UL, value, true);
                }

                public Mas.Schema.Model.Monica.Params.Harvest.OptCarbonMgmtData.WRITER OptCarbMgmtData
                {
                    get => BuildPointer<Mas.Schema.Model.Monica.Params.Harvest.OptCarbonMgmtData.WRITER>(0);
                    set => Link(0, value);
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa9a9bc941e963701UL)]
            public enum CropUsage : ushort
            {
                greenManure,
                biomassProduction
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaf49ab9bbe76e375UL)]
            public class OptCarbonMgmtData : ICapnpSerializable
            {
                public const UInt64 typeId = 0xaf49ab9bbe76e375UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    OptCarbonConservation = reader.OptCarbonConservation;
                    CropImpactOnHumusBalance = reader.CropImpactOnHumusBalance;
                    CropUsage = reader.CropUsage;
                    ResidueHeq = reader.ResidueHeq;
                    OrganicFertilizerHeq = reader.OrganicFertilizerHeq;
                    MaxResidueRecoverFraction = reader.MaxResidueRecoverFraction;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.OptCarbonConservation = OptCarbonConservation;
                    writer.CropImpactOnHumusBalance = CropImpactOnHumusBalance;
                    writer.CropUsage = CropUsage;
                    writer.ResidueHeq = ResidueHeq;
                    writer.OrganicFertilizerHeq = OrganicFertilizerHeq;
                    writer.MaxResidueRecoverFraction = MaxResidueRecoverFraction;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public bool OptCarbonConservation
                {
                    get;
                    set;
                }

                = false;
                public double CropImpactOnHumusBalance
                {
                    get;
                    set;
                }

                = 0;
                public Mas.Schema.Model.Monica.Params.Harvest.CropUsage CropUsage
                {
                    get;
                    set;
                }

                = Mas.Schema.Model.Monica.Params.Harvest.CropUsage.biomassProduction;
                public double ResidueHeq
                {
                    get;
                    set;
                }

                = 0;
                public double OrganicFertilizerHeq
                {
                    get;
                    set;
                }

                = 0;
                public double MaxResidueRecoverFraction
                {
                    get;
                    set;
                }

                = 0;
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
                    public bool OptCarbonConservation => ctx.ReadDataBool(0UL, false);
                    public double CropImpactOnHumusBalance => ctx.ReadDataDouble(64UL, 0);
                    public Mas.Schema.Model.Monica.Params.Harvest.CropUsage CropUsage => (Mas.Schema.Model.Monica.Params.Harvest.CropUsage)ctx.ReadDataUShort(16UL, (ushort)1);
                    public double ResidueHeq => ctx.ReadDataDouble(128UL, 0);
                    public double OrganicFertilizerHeq => ctx.ReadDataDouble(192UL, 0);
                    public double MaxResidueRecoverFraction => ctx.ReadDataDouble(256UL, 0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(5, 0);
                    }

                    public bool OptCarbonConservation
                    {
                        get => this.ReadDataBool(0UL, false);
                        set => this.WriteData(0UL, value, false);
                    }

                    public double CropImpactOnHumusBalance
                    {
                        get => this.ReadDataDouble(64UL, 0);
                        set => this.WriteData(64UL, value, 0);
                    }

                    public Mas.Schema.Model.Monica.Params.Harvest.CropUsage CropUsage
                    {
                        get => (Mas.Schema.Model.Monica.Params.Harvest.CropUsage)this.ReadDataUShort(16UL, (ushort)1);
                        set => this.WriteData(16UL, (ushort)value, (ushort)1);
                    }

                    public double ResidueHeq
                    {
                        get => this.ReadDataDouble(128UL, 0);
                        set => this.WriteData(128UL, value, 0);
                    }

                    public double OrganicFertilizerHeq
                    {
                        get => this.ReadDataDouble(192UL, 0);
                        set => this.WriteData(192UL, value, 0);
                    }

                    public double MaxResidueRecoverFraction
                    {
                        get => this.ReadDataDouble(256UL, 0);
                        set => this.WriteData(256UL, value, 0);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf805d22fabb80702UL)]
        public class AutomaticHarvest : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf805d22fabb80702UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                MinPercentASW = reader.MinPercentASW;
                MaxPercentASW = reader.MaxPercentASW;
                Max3dayPrecipSum = reader.Max3dayPrecipSum;
                MaxCurrentDayPrecipSum = reader.MaxCurrentDayPrecipSum;
                HarvestTime = reader.HarvestTime;
                Harvest = CapnpSerializable.Create<Mas.Schema.Model.Monica.Params.Harvest>(reader.Harvest);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.MinPercentASW = MinPercentASW;
                writer.MaxPercentASW = MaxPercentASW;
                writer.Max3dayPrecipSum = Max3dayPrecipSum;
                writer.MaxCurrentDayPrecipSum = MaxCurrentDayPrecipSum;
                writer.HarvestTime = HarvestTime;
                Harvest?.serialize(writer.Harvest);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double MinPercentASW
            {
                get;
                set;
            }

            public double MaxPercentASW
            {
                get;
                set;
            }

            = 100;
            public double Max3dayPrecipSum
            {
                get;
                set;
            }

            public double MaxCurrentDayPrecipSum
            {
                get;
                set;
            }

            public Mas.Schema.Model.Monica.Event.PhenoStage HarvestTime
            {
                get;
                set;
            }

            = Mas.Schema.Model.Monica.Event.PhenoStage.maturity;
            public Mas.Schema.Model.Monica.Params.Harvest Harvest
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
                public double MinPercentASW => ctx.ReadDataDouble(0UL, 0);
                public double MaxPercentASW => ctx.ReadDataDouble(64UL, 100);
                public double Max3dayPrecipSum => ctx.ReadDataDouble(128UL, 0);
                public double MaxCurrentDayPrecipSum => ctx.ReadDataDouble(192UL, 0);
                public Mas.Schema.Model.Monica.Event.PhenoStage HarvestTime => (Mas.Schema.Model.Monica.Event.PhenoStage)ctx.ReadDataUShort(256UL, (ushort)3);
                public Mas.Schema.Model.Monica.Params.Harvest.READER Harvest => ctx.ReadStruct(0, Mas.Schema.Model.Monica.Params.Harvest.READER.create);
                public bool HasHarvest => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(5, 1);
                }

                public double MinPercentASW
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public double MaxPercentASW
                {
                    get => this.ReadDataDouble(64UL, 100);
                    set => this.WriteData(64UL, value, 100);
                }

                public double Max3dayPrecipSum
                {
                    get => this.ReadDataDouble(128UL, 0);
                    set => this.WriteData(128UL, value, 0);
                }

                public double MaxCurrentDayPrecipSum
                {
                    get => this.ReadDataDouble(192UL, 0);
                    set => this.WriteData(192UL, value, 0);
                }

                public Mas.Schema.Model.Monica.Event.PhenoStage HarvestTime
                {
                    get => (Mas.Schema.Model.Monica.Event.PhenoStage)this.ReadDataUShort(256UL, (ushort)3);
                    set => this.WriteData(256UL, (ushort)value, (ushort)3);
                }

                public Mas.Schema.Model.Monica.Params.Harvest.WRITER Harvest
                {
                    get => BuildPointer<Mas.Schema.Model.Monica.Params.Harvest.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8460dac6abff7ed9UL)]
        public class Cutting : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8460dac6abff7ed9UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                CuttingSpec = reader.CuttingSpec?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Model.Monica.Params.Cutting.Spec>(_));
                CutMaxAssimilationRatePercentage = reader.CutMaxAssimilationRatePercentage;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.CuttingSpec.Init(CuttingSpec, (_s1, _v1) => _v1?.serialize(_s1));
                writer.CutMaxAssimilationRatePercentage = CutMaxAssimilationRatePercentage;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Schema.Model.Monica.Params.Cutting.Spec> CuttingSpec
            {
                get;
                set;
            }

            public double CutMaxAssimilationRatePercentage
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
                public IReadOnlyList<Mas.Schema.Model.Monica.Params.Cutting.Spec.READER> CuttingSpec => ctx.ReadList(0).Cast(Mas.Schema.Model.Monica.Params.Cutting.Spec.READER.create);
                public bool HasCuttingSpec => ctx.IsStructFieldNonNull(0);
                public double CutMaxAssimilationRatePercentage => ctx.ReadDataDouble(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public ListOfStructsSerializer<Mas.Schema.Model.Monica.Params.Cutting.Spec.WRITER> CuttingSpec
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Model.Monica.Params.Cutting.Spec.WRITER>>(0);
                    set => Link(0, value);
                }

                public double CutMaxAssimilationRatePercentage
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe444f780b29541a7UL)]
            public enum CL : ushort
            {
                cut,
                left
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x94d32947f136655eUL)]
            public enum Unit : ushort
            {
                percentage,
                biomass,
                lai
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfae5dcfccbb93a23UL)]
            public class Spec : ICapnpSerializable
            {
                public const UInt64 typeId = 0xfae5dcfccbb93a23UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Organ = reader.Organ;
                    Value = reader.Value;
                    Unit = reader.Unit;
                    CutOrLeft = reader.CutOrLeft;
                    ExportPercentage = reader.ExportPercentage;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Organ = Organ;
                    writer.Value = Value;
                    writer.Unit = Unit;
                    writer.CutOrLeft = CutOrLeft;
                    writer.ExportPercentage = ExportPercentage;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Schema.Model.Monica.PlantOrgan Organ
                {
                    get;
                    set;
                }

                public double Value
                {
                    get;
                    set;
                }

                public Mas.Schema.Model.Monica.Params.Cutting.Unit Unit
                {
                    get;
                    set;
                }

                = Mas.Schema.Model.Monica.Params.Cutting.Unit.percentage;
                public Mas.Schema.Model.Monica.Params.Cutting.CL CutOrLeft
                {
                    get;
                    set;
                }

                = Mas.Schema.Model.Monica.Params.Cutting.CL.cut;
                public double ExportPercentage
                {
                    get;
                    set;
                }

                = 100;
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
                    public Mas.Schema.Model.Monica.PlantOrgan Organ => (Mas.Schema.Model.Monica.PlantOrgan)ctx.ReadDataUShort(0UL, (ushort)0);
                    public double Value => ctx.ReadDataDouble(64UL, 0);
                    public Mas.Schema.Model.Monica.Params.Cutting.Unit Unit => (Mas.Schema.Model.Monica.Params.Cutting.Unit)ctx.ReadDataUShort(16UL, (ushort)0);
                    public Mas.Schema.Model.Monica.Params.Cutting.CL CutOrLeft => (Mas.Schema.Model.Monica.Params.Cutting.CL)ctx.ReadDataUShort(32UL, (ushort)0);
                    public double ExportPercentage => ctx.ReadDataDouble(128UL, 100);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(3, 0);
                    }

                    public Mas.Schema.Model.Monica.PlantOrgan Organ
                    {
                        get => (Mas.Schema.Model.Monica.PlantOrgan)this.ReadDataUShort(0UL, (ushort)0);
                        set => this.WriteData(0UL, (ushort)value, (ushort)0);
                    }

                    public double Value
                    {
                        get => this.ReadDataDouble(64UL, 0);
                        set => this.WriteData(64UL, value, 0);
                    }

                    public Mas.Schema.Model.Monica.Params.Cutting.Unit Unit
                    {
                        get => (Mas.Schema.Model.Monica.Params.Cutting.Unit)this.ReadDataUShort(16UL, (ushort)0);
                        set => this.WriteData(16UL, (ushort)value, (ushort)0);
                    }

                    public Mas.Schema.Model.Monica.Params.Cutting.CL CutOrLeft
                    {
                        get => (Mas.Schema.Model.Monica.Params.Cutting.CL)this.ReadDataUShort(32UL, (ushort)0);
                        set => this.WriteData(32UL, (ushort)value, (ushort)0);
                    }

                    public double ExportPercentage
                    {
                        get => this.ReadDataDouble(128UL, 100);
                        set => this.WriteData(128UL, value, 100);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa363d226e178debdUL)]
        public class MineralFertilization : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa363d226e178debdUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Partition = CapnpSerializable.Create<Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters>(reader.Partition);
                Amount = reader.Amount;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Partition?.serialize(writer.Partition);
                writer.Amount = Amount;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters Partition
            {
                get;
                set;
            }

            public double Amount
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
                public Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters.READER Partition => ctx.ReadStruct(0, Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters.READER.create);
                public bool HasPartition => ctx.IsStructFieldNonNull(0);
                public double Amount => ctx.ReadDataDouble(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters.WRITER Partition
                {
                    get => BuildPointer<Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters.WRITER>(0);
                    set => Link(0, value);
                }

                public double Amount
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc75b5ef2e9b05c2dUL)]
            public class Parameters : ICapnpSerializable
            {
                public const UInt64 typeId = 0xc75b5ef2e9b05c2dUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Carbamid = reader.Carbamid;
                    Nh4 = reader.Nh4;
                    No3 = reader.No3;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.Carbamid = Carbamid;
                    writer.Nh4 = Nh4;
                    writer.No3 = No3;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public double Carbamid
                {
                    get;
                    set;
                }

                public double Nh4
                {
                    get;
                    set;
                }

                public double No3
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
                    public double Carbamid => ctx.ReadDataDouble(0UL, 0);
                    public double Nh4 => ctx.ReadDataDouble(64UL, 0);
                    public double No3 => ctx.ReadDataDouble(128UL, 0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(3, 0);
                    }

                    public double Carbamid
                    {
                        get => this.ReadDataDouble(0UL, 0);
                        set => this.WriteData(0UL, value, 0);
                    }

                    public double Nh4
                    {
                        get => this.ReadDataDouble(64UL, 0);
                        set => this.WriteData(64UL, value, 0);
                    }

                    public double No3
                    {
                        get => this.ReadDataDouble(128UL, 0);
                        set => this.WriteData(128UL, value, 0);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc7c14e92e0cd461cUL)]
        public class NDemandFertilization : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc7c14e92e0cd461cUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                NDemand = reader.NDemand;
                Partition = CapnpSerializable.Create<Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters>(reader.Partition);
                Depth = reader.Depth;
                Stage = reader.Stage;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.NDemand = NDemand;
                Partition?.serialize(writer.Partition);
                writer.Depth = Depth;
                writer.Stage = Stage;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double NDemand
            {
                get;
                set;
            }

            public Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters Partition
            {
                get;
                set;
            }

            public double Depth
            {
                get;
                set;
            }

            public byte Stage
            {
                get;
                set;
            }

            = 1;
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
                public double NDemand => ctx.ReadDataDouble(0UL, 0);
                public Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters.READER Partition => ctx.ReadStruct(0, Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters.READER.create);
                public bool HasPartition => ctx.IsStructFieldNonNull(0);
                public double Depth => ctx.ReadDataDouble(64UL, 0);
                public byte Stage => ctx.ReadDataByte(128UL, (byte)1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(3, 1);
                }

                public double NDemand
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters.WRITER Partition
                {
                    get => BuildPointer<Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters.WRITER>(0);
                    set => Link(0, value);
                }

                public double Depth
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }

                public byte Stage
                {
                    get => this.ReadDataByte(128UL, (byte)1);
                    set => this.WriteData(128UL, value, (byte)1);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb492838c7fed50b0UL)]
        public class OrganicFertilization : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb492838c7fed50b0UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Params = CapnpSerializable.Create<Mas.Schema.Model.Monica.Params.OrganicFertilization.Parameters>(reader.Params);
                Amount = reader.Amount;
                Incorporation = reader.Incorporation;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Params?.serialize(writer.Params);
                writer.Amount = Amount;
                writer.Incorporation = Incorporation;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Model.Monica.Params.OrganicFertilization.Parameters Params
            {
                get;
                set;
            }

            public double Amount
            {
                get;
                set;
            }

            public bool Incorporation
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
                public Mas.Schema.Model.Monica.Params.OrganicFertilization.Parameters.READER Params => ctx.ReadStruct(0, Mas.Schema.Model.Monica.Params.OrganicFertilization.Parameters.READER.create);
                public bool HasParams => ctx.IsStructFieldNonNull(0);
                public double Amount => ctx.ReadDataDouble(0UL, 0);
                public bool Incorporation => ctx.ReadDataBool(64UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 1);
                }

                public Mas.Schema.Model.Monica.Params.OrganicFertilization.Parameters.WRITER Params
                {
                    get => BuildPointer<Mas.Schema.Model.Monica.Params.OrganicFertilization.Parameters.WRITER>(0);
                    set => Link(0, value);
                }

                public double Amount
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public bool Incorporation
                {
                    get => this.ReadDataBool(64UL, false);
                    set => this.WriteData(64UL, value, false);
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x95cdc661a6600137UL)]
            public class OrganicMatterParameters : ICapnpSerializable
            {
                public const UInt64 typeId = 0x95cdc661a6600137UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    AomDryMatterContent = reader.AomDryMatterContent;
                    AomNH4Content = reader.AomNH4Content;
                    AomNO3Content = reader.AomNO3Content;
                    AomCarbamidContent = reader.AomCarbamidContent;
                    AomSlowDecCoeffStandard = reader.AomSlowDecCoeffStandard;
                    AomFastDecCoeffStandard = reader.AomFastDecCoeffStandard;
                    PartAOMToAOMSlow = reader.PartAOMToAOMSlow;
                    PartAOMToAOMFast = reader.PartAOMToAOMFast;
                    CnRatioAOMSlow = reader.CnRatioAOMSlow;
                    CnRatioAOMFast = reader.CnRatioAOMFast;
                    PartAOMSlowToSMBSlow = reader.PartAOMSlowToSMBSlow;
                    PartAOMSlowToSMBFast = reader.PartAOMSlowToSMBFast;
                    NConcentration = reader.NConcentration;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.AomDryMatterContent = AomDryMatterContent;
                    writer.AomNH4Content = AomNH4Content;
                    writer.AomNO3Content = AomNO3Content;
                    writer.AomCarbamidContent = AomCarbamidContent;
                    writer.AomSlowDecCoeffStandard = AomSlowDecCoeffStandard;
                    writer.AomFastDecCoeffStandard = AomFastDecCoeffStandard;
                    writer.PartAOMToAOMSlow = PartAOMToAOMSlow;
                    writer.PartAOMToAOMFast = PartAOMToAOMFast;
                    writer.CnRatioAOMSlow = CnRatioAOMSlow;
                    writer.CnRatioAOMFast = CnRatioAOMFast;
                    writer.PartAOMSlowToSMBSlow = PartAOMSlowToSMBSlow;
                    writer.PartAOMSlowToSMBFast = PartAOMSlowToSMBFast;
                    writer.NConcentration = NConcentration;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public double AomDryMatterContent
                {
                    get;
                    set;
                }

                public double AomNH4Content
                {
                    get;
                    set;
                }

                public double AomNO3Content
                {
                    get;
                    set;
                }

                public double AomCarbamidContent
                {
                    get;
                    set;
                }

                public double AomSlowDecCoeffStandard
                {
                    get;
                    set;
                }

                public double AomFastDecCoeffStandard
                {
                    get;
                    set;
                }

                public double PartAOMToAOMSlow
                {
                    get;
                    set;
                }

                public double PartAOMToAOMFast
                {
                    get;
                    set;
                }

                public double CnRatioAOMSlow
                {
                    get;
                    set;
                }

                public double CnRatioAOMFast
                {
                    get;
                    set;
                }

                public double PartAOMSlowToSMBSlow
                {
                    get;
                    set;
                }

                public double PartAOMSlowToSMBFast
                {
                    get;
                    set;
                }

                public double NConcentration
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
                    public double AomDryMatterContent => ctx.ReadDataDouble(0UL, 0);
                    public double AomNH4Content => ctx.ReadDataDouble(64UL, 0);
                    public double AomNO3Content => ctx.ReadDataDouble(128UL, 0);
                    public double AomCarbamidContent => ctx.ReadDataDouble(192UL, 0);
                    public double AomSlowDecCoeffStandard => ctx.ReadDataDouble(256UL, 0);
                    public double AomFastDecCoeffStandard => ctx.ReadDataDouble(320UL, 0);
                    public double PartAOMToAOMSlow => ctx.ReadDataDouble(384UL, 0);
                    public double PartAOMToAOMFast => ctx.ReadDataDouble(448UL, 0);
                    public double CnRatioAOMSlow => ctx.ReadDataDouble(512UL, 0);
                    public double CnRatioAOMFast => ctx.ReadDataDouble(576UL, 0);
                    public double PartAOMSlowToSMBSlow => ctx.ReadDataDouble(640UL, 0);
                    public double PartAOMSlowToSMBFast => ctx.ReadDataDouble(704UL, 0);
                    public double NConcentration => ctx.ReadDataDouble(768UL, 0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(13, 0);
                    }

                    public double AomDryMatterContent
                    {
                        get => this.ReadDataDouble(0UL, 0);
                        set => this.WriteData(0UL, value, 0);
                    }

                    public double AomNH4Content
                    {
                        get => this.ReadDataDouble(64UL, 0);
                        set => this.WriteData(64UL, value, 0);
                    }

                    public double AomNO3Content
                    {
                        get => this.ReadDataDouble(128UL, 0);
                        set => this.WriteData(128UL, value, 0);
                    }

                    public double AomCarbamidContent
                    {
                        get => this.ReadDataDouble(192UL, 0);
                        set => this.WriteData(192UL, value, 0);
                    }

                    public double AomSlowDecCoeffStandard
                    {
                        get => this.ReadDataDouble(256UL, 0);
                        set => this.WriteData(256UL, value, 0);
                    }

                    public double AomFastDecCoeffStandard
                    {
                        get => this.ReadDataDouble(320UL, 0);
                        set => this.WriteData(320UL, value, 0);
                    }

                    public double PartAOMToAOMSlow
                    {
                        get => this.ReadDataDouble(384UL, 0);
                        set => this.WriteData(384UL, value, 0);
                    }

                    public double PartAOMToAOMFast
                    {
                        get => this.ReadDataDouble(448UL, 0);
                        set => this.WriteData(448UL, value, 0);
                    }

                    public double CnRatioAOMSlow
                    {
                        get => this.ReadDataDouble(512UL, 0);
                        set => this.WriteData(512UL, value, 0);
                    }

                    public double CnRatioAOMFast
                    {
                        get => this.ReadDataDouble(576UL, 0);
                        set => this.WriteData(576UL, value, 0);
                    }

                    public double PartAOMSlowToSMBSlow
                    {
                        get => this.ReadDataDouble(640UL, 0);
                        set => this.WriteData(640UL, value, 0);
                    }

                    public double PartAOMSlowToSMBFast
                    {
                        get => this.ReadDataDouble(704UL, 0);
                        set => this.WriteData(704UL, value, 0);
                    }

                    public double NConcentration
                    {
                        get => this.ReadDataDouble(768UL, 0);
                        set => this.WriteData(768UL, value, 0);
                    }
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xba0c11cf818d29fdUL)]
            public class Parameters : ICapnpSerializable
            {
                public const UInt64 typeId = 0xba0c11cf818d29fdUL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    Params = CapnpSerializable.Create<Mas.Schema.Model.Monica.Params.OrganicFertilization.OrganicMatterParameters>(reader.Params);
                    Id = reader.Id;
                    Name = reader.Name;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    Params?.serialize(writer.Params);
                    writer.Id = Id;
                    writer.Name = Name;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public Mas.Schema.Model.Monica.Params.OrganicFertilization.OrganicMatterParameters Params
                {
                    get;
                    set;
                }

                public string Id
                {
                    get;
                    set;
                }

                public string Name
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
                    public Mas.Schema.Model.Monica.Params.OrganicFertilization.OrganicMatterParameters.READER Params => ctx.ReadStruct(0, Mas.Schema.Model.Monica.Params.OrganicFertilization.OrganicMatterParameters.READER.create);
                    public bool HasParams => ctx.IsStructFieldNonNull(0);
                    public string Id => ctx.ReadText(1, null);
                    public string Name => ctx.ReadText(2, null);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(0, 3);
                    }

                    public Mas.Schema.Model.Monica.Params.OrganicFertilization.OrganicMatterParameters.WRITER Params
                    {
                        get => BuildPointer<Mas.Schema.Model.Monica.Params.OrganicFertilization.OrganicMatterParameters.WRITER>(0);
                        set => Link(0, value);
                    }

                    public string Id
                    {
                        get => this.ReadText(1, null);
                        set => this.WriteText(1, value, null);
                    }

                    public string Name
                    {
                        get => this.ReadText(2, null);
                        set => this.WriteText(2, value, null);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaa49811a4e3e2c59UL)]
        public class Tillage : ICapnpSerializable
        {
            public const UInt64 typeId = 0xaa49811a4e3e2c59UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Depth = reader.Depth;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Depth = Depth;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Depth
            {
                get;
                set;
            }

            = 0.3;
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
                public double Depth => ctx.ReadDataDouble(0UL, 0.3);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public double Depth
                {
                    get => this.ReadDataDouble(0UL, 0.3);
                    set => this.WriteData(0UL, value, 0.3);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd90939a58e404ff8UL)]
        public class Irrigation : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd90939a58e404ff8UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Amount = reader.Amount;
                Params = CapnpSerializable.Create<Mas.Schema.Model.Monica.Params.Irrigation.Parameters>(reader.Params);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Amount = Amount;
                Params?.serialize(writer.Params);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Amount
            {
                get;
                set;
            }

            public Mas.Schema.Model.Monica.Params.Irrigation.Parameters Params
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
                public double Amount => ctx.ReadDataDouble(0UL, 0);
                public Mas.Schema.Model.Monica.Params.Irrigation.Parameters.READER Params => ctx.ReadStruct(0, Mas.Schema.Model.Monica.Params.Irrigation.Parameters.READER.create);
                public bool HasParams => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public double Amount
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public Mas.Schema.Model.Monica.Params.Irrigation.Parameters.WRITER Params
                {
                    get => BuildPointer<Mas.Schema.Model.Monica.Params.Irrigation.Parameters.WRITER>(0);
                    set => Link(0, value);
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaec9e089e87f1599UL)]
            public class Parameters : ICapnpSerializable
            {
                public const UInt64 typeId = 0xaec9e089e87f1599UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    NitrateConcentration = reader.NitrateConcentration;
                    SulfateConcentration = reader.SulfateConcentration;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.NitrateConcentration = NitrateConcentration;
                    writer.SulfateConcentration = SulfateConcentration;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public double NitrateConcentration
                {
                    get;
                    set;
                }

                public double SulfateConcentration
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
                    public double NitrateConcentration => ctx.ReadDataDouble(0UL, 0);
                    public double SulfateConcentration => ctx.ReadDataDouble(64UL, 0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(2, 0);
                    }

                    public double NitrateConcentration
                    {
                        get => this.ReadDataDouble(0UL, 0);
                        set => this.WriteData(0UL, value, 0);
                    }

                    public double SulfateConcentration
                    {
                        get => this.ReadDataDouble(64UL, 0);
                        set => this.WriteData(64UL, value, 0);
                    }
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe3da81cc36d5741dUL), Proxy(typeof(FertilizerService_Proxy)), Skeleton(typeof(FertilizerService_Skeleton))]
    public interface IFertilizerService : Mas.Schema.Common.IIdentifiable
    {
        Task<Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters> MineralFertilizerPartitionFor(Mas.Schema.Model.Monica.MineralFertilizer minFert, CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Model.Monica.Params.OrganicFertilization.Parameters> OrganicFertilizerParametersFor(Mas.Schema.Model.Monica.OrganicFertilizer orgFert, CancellationToken cancellationToken_ = default);
        Task<IReadOnlyList<Mas.Schema.Model.Monica.FertilizerService.Entry>> AvailableMineralFertilizers(CancellationToken cancellationToken_ = default);
        Task<IReadOnlyList<Mas.Schema.Model.Monica.FertilizerService.Entry>> AvailableOrganicFertilizers(CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters> MineralFertilizer(string id, CancellationToken cancellationToken_ = default);
        Task<Mas.Schema.Model.Monica.Params.OrganicFertilization.OrganicMatterParameters> OrganicFertilizer(string id, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe3da81cc36d5741dUL)]
    public class FertilizerService_Proxy : Proxy, IFertilizerService
    {
        public async Task<Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters> MineralFertilizerPartitionFor(Mas.Schema.Model.Monica.MineralFertilizer minFert, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Model.Monica.FertilizerService.Params_MineralFertilizerPartitionFor.WRITER>();
            var arg_ = new Mas.Schema.Model.Monica.FertilizerService.Params_MineralFertilizerPartitionFor()
            {MinFert = minFert};
            arg_?.serialize(in_);
            using (var d_ = await Call(16418578105625834525UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Model.Monica.FertilizerService.Result_MineralFertilizerPartitionFor>(d_);
                return (r_.Partition);
            }
        }

        public async Task<Mas.Schema.Model.Monica.Params.OrganicFertilization.Parameters> OrganicFertilizerParametersFor(Mas.Schema.Model.Monica.OrganicFertilizer orgFert, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Model.Monica.FertilizerService.Params_OrganicFertilizerParametersFor.WRITER>();
            var arg_ = new Mas.Schema.Model.Monica.FertilizerService.Params_OrganicFertilizerParametersFor()
            {OrgFert = orgFert};
            arg_?.serialize(in_);
            using (var d_ = await Call(16418578105625834525UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Model.Monica.FertilizerService.Result_OrganicFertilizerParametersFor>(d_);
                return (r_.Params);
            }
        }

        public Task<IReadOnlyList<Mas.Schema.Model.Monica.FertilizerService.Entry>> AvailableMineralFertilizers(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Model.Monica.FertilizerService.Params_AvailableMineralFertilizers.WRITER>();
            var arg_ = new Mas.Schema.Model.Monica.FertilizerService.Params_AvailableMineralFertilizers()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(16418578105625834525UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Model.Monica.FertilizerService.Result_AvailableMineralFertilizers>(d_);
                    return (r_.Entries);
                }
            }

            );
        }

        public Task<IReadOnlyList<Mas.Schema.Model.Monica.FertilizerService.Entry>> AvailableOrganicFertilizers(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Model.Monica.FertilizerService.Params_AvailableOrganicFertilizers.WRITER>();
            var arg_ = new Mas.Schema.Model.Monica.FertilizerService.Params_AvailableOrganicFertilizers()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(16418578105625834525UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Model.Monica.FertilizerService.Result_AvailableOrganicFertilizers>(d_);
                    return (r_.Entries);
                }
            }

            );
        }

        public async Task<Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters> MineralFertilizer(string id, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Model.Monica.FertilizerService.Params_MineralFertilizer.WRITER>();
            var arg_ = new Mas.Schema.Model.Monica.FertilizerService.Params_MineralFertilizer()
            {Id = id};
            arg_?.serialize(in_);
            using (var d_ = await Call(16418578105625834525UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Model.Monica.FertilizerService.Result_MineralFertilizer>(d_);
                return (r_.Fert);
            }
        }

        public async Task<Mas.Schema.Model.Monica.Params.OrganicFertilization.OrganicMatterParameters> OrganicFertilizer(string id, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Model.Monica.FertilizerService.Params_OrganicFertilizer.WRITER>();
            var arg_ = new Mas.Schema.Model.Monica.FertilizerService.Params_OrganicFertilizer()
            {Id = id};
            arg_?.serialize(in_);
            using (var d_ = await Call(16418578105625834525UL, 5, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Model.Monica.FertilizerService.Result_OrganicFertilizer>(d_);
                return (r_.Fert);
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe3da81cc36d5741dUL)]
    public class FertilizerService_Skeleton : Skeleton<IFertilizerService>
    {
        public FertilizerService_Skeleton()
        {
            SetMethodTable(MineralFertilizerPartitionFor, OrganicFertilizerParametersFor, AvailableMineralFertilizers, AvailableOrganicFertilizers, MineralFertilizer, OrganicFertilizer);
        }

        public override ulong InterfaceId => 16418578105625834525UL;
        Task<AnswerOrCounterquestion> MineralFertilizerPartitionFor(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Model.Monica.FertilizerService.Params_MineralFertilizerPartitionFor>(d_);
                return Impatient.MaybeTailCall(Impl.MineralFertilizerPartitionFor(in_.MinFert, cancellationToken_), partition =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Model.Monica.FertilizerService.Result_MineralFertilizerPartitionFor.WRITER>();
                    var r_ = new Mas.Schema.Model.Monica.FertilizerService.Result_MineralFertilizerPartitionFor{Partition = partition};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> OrganicFertilizerParametersFor(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Model.Monica.FertilizerService.Params_OrganicFertilizerParametersFor>(d_);
                return Impatient.MaybeTailCall(Impl.OrganicFertilizerParametersFor(in_.OrgFert, cancellationToken_), @params =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Model.Monica.FertilizerService.Result_OrganicFertilizerParametersFor.WRITER>();
                    var r_ = new Mas.Schema.Model.Monica.FertilizerService.Result_OrganicFertilizerParametersFor{Params = @params};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> AvailableMineralFertilizers(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.AvailableMineralFertilizers(cancellationToken_), entries =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Model.Monica.FertilizerService.Result_AvailableMineralFertilizers.WRITER>();
                    var r_ = new Mas.Schema.Model.Monica.FertilizerService.Result_AvailableMineralFertilizers{Entries = entries};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> AvailableOrganicFertilizers(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.AvailableOrganicFertilizers(cancellationToken_), entries =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Model.Monica.FertilizerService.Result_AvailableOrganicFertilizers.WRITER>();
                    var r_ = new Mas.Schema.Model.Monica.FertilizerService.Result_AvailableOrganicFertilizers{Entries = entries};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> MineralFertilizer(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Model.Monica.FertilizerService.Params_MineralFertilizer>(d_);
                return Impatient.MaybeTailCall(Impl.MineralFertilizer(in_.Id, cancellationToken_), fert =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Model.Monica.FertilizerService.Result_MineralFertilizer.WRITER>();
                    var r_ = new Mas.Schema.Model.Monica.FertilizerService.Result_MineralFertilizer{Fert = fert};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> OrganicFertilizer(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<Mas.Schema.Model.Monica.FertilizerService.Params_OrganicFertilizer>(d_);
                return Impatient.MaybeTailCall(Impl.OrganicFertilizer(in_.Id, cancellationToken_), fert =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Model.Monica.FertilizerService.Result_OrganicFertilizer.WRITER>();
                    var r_ = new Mas.Schema.Model.Monica.FertilizerService.Result_OrganicFertilizer{Fert = fert};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class FertilizerService
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf54586500e2b72cdUL)]
        public class Entry : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf54586500e2b72cdUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Info = CapnpSerializable.Create<Mas.Schema.Common.IdInformation>(reader.Info);
                Ref = reader.Ref;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Info?.serialize(writer.Info);
                writer.Ref = Ref;
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

            public Mas.Schema.Common.IAnyValueHolder Ref
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
                public Mas.Schema.Common.IAnyValueHolder Ref => ctx.ReadCap<Mas.Schema.Common.IAnyValueHolder>(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public Mas.Schema.Common.IdInformation.WRITER Info
                {
                    get => BuildPointer<Mas.Schema.Common.IdInformation.WRITER>(0);
                    set => Link(0, value);
                }

                public Mas.Schema.Common.IAnyValueHolder Ref
                {
                    get => ReadCap<Mas.Schema.Common.IAnyValueHolder>(1);
                    set => LinkObject(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xba3900305e908e31UL)]
        public class Params_MineralFertilizerPartitionFor : ICapnpSerializable
        {
            public const UInt64 typeId = 0xba3900305e908e31UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                MinFert = reader.MinFert;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.MinFert = MinFert;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Model.Monica.MineralFertilizer MinFert
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
                public Mas.Schema.Model.Monica.MineralFertilizer MinFert => (Mas.Schema.Model.Monica.MineralFertilizer)ctx.ReadDataUShort(0UL, (ushort)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public Mas.Schema.Model.Monica.MineralFertilizer MinFert
                {
                    get => (Mas.Schema.Model.Monica.MineralFertilizer)this.ReadDataUShort(0UL, (ushort)0);
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbdb5eaa658c7ababUL)]
        public class Result_MineralFertilizerPartitionFor : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbdb5eaa658c7ababUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Partition = CapnpSerializable.Create<Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters>(reader.Partition);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Partition?.serialize(writer.Partition);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters Partition
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
                public Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters.READER Partition => ctx.ReadStruct(0, Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters.READER.create);
                public bool HasPartition => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters.WRITER Partition
                {
                    get => BuildPointer<Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xead8c24402f189b4UL)]
        public class Params_OrganicFertilizerParametersFor : ICapnpSerializable
        {
            public const UInt64 typeId = 0xead8c24402f189b4UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                OrgFert = reader.OrgFert;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.OrgFert = OrgFert;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Model.Monica.OrganicFertilizer OrgFert
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
                public Mas.Schema.Model.Monica.OrganicFertilizer OrgFert => (Mas.Schema.Model.Monica.OrganicFertilizer)ctx.ReadDataUShort(0UL, (ushort)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public Mas.Schema.Model.Monica.OrganicFertilizer OrgFert
                {
                    get => (Mas.Schema.Model.Monica.OrganicFertilizer)this.ReadDataUShort(0UL, (ushort)0);
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf6b53f1a4cf29599UL)]
        public class Result_OrganicFertilizerParametersFor : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf6b53f1a4cf29599UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Params = CapnpSerializable.Create<Mas.Schema.Model.Monica.Params.OrganicFertilization.Parameters>(reader.Params);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Params?.serialize(writer.Params);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Model.Monica.Params.OrganicFertilization.Parameters Params
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
                public Mas.Schema.Model.Monica.Params.OrganicFertilization.Parameters.READER Params => ctx.ReadStruct(0, Mas.Schema.Model.Monica.Params.OrganicFertilization.Parameters.READER.create);
                public bool HasParams => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Model.Monica.Params.OrganicFertilization.Parameters.WRITER Params
                {
                    get => BuildPointer<Mas.Schema.Model.Monica.Params.OrganicFertilization.Parameters.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa9b89d473e06f2c7UL)]
        public class Params_AvailableMineralFertilizers : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa9b89d473e06f2c7UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbd608077a7cec156UL)]
        public class Result_AvailableMineralFertilizers : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbd608077a7cec156UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Entries = reader.Entries?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Model.Monica.FertilizerService.Entry>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Entries.Init(Entries, (_s1, _v1) => _v1?.serialize(_s1));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Schema.Model.Monica.FertilizerService.Entry> Entries
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
                public IReadOnlyList<Mas.Schema.Model.Monica.FertilizerService.Entry.READER> Entries => ctx.ReadList(0).Cast(Mas.Schema.Model.Monica.FertilizerService.Entry.READER.create);
                public bool HasEntries => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfStructsSerializer<Mas.Schema.Model.Monica.FertilizerService.Entry.WRITER> Entries
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Model.Monica.FertilizerService.Entry.WRITER>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9437da695f7567e7UL)]
        public class Params_AvailableOrganicFertilizers : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9437da695f7567e7UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xab49546abd2a478aUL)]
        public class Result_AvailableOrganicFertilizers : ICapnpSerializable
        {
            public const UInt64 typeId = 0xab49546abd2a478aUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Entries = reader.Entries?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Model.Monica.FertilizerService.Entry>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Entries.Init(Entries, (_s1, _v1) => _v1?.serialize(_s1));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Schema.Model.Monica.FertilizerService.Entry> Entries
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
                public IReadOnlyList<Mas.Schema.Model.Monica.FertilizerService.Entry.READER> Entries => ctx.ReadList(0).Cast(Mas.Schema.Model.Monica.FertilizerService.Entry.READER.create);
                public bool HasEntries => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfStructsSerializer<Mas.Schema.Model.Monica.FertilizerService.Entry.WRITER> Entries
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Model.Monica.FertilizerService.Entry.WRITER>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9ead0b36096d9073UL)]
        public class Params_MineralFertilizer : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9ead0b36096d9073UL;
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

            public string Id
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
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Id
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x82a3c6bf1cc5ad84UL)]
        public class Result_MineralFertilizer : ICapnpSerializable
        {
            public const UInt64 typeId = 0x82a3c6bf1cc5ad84UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Fert = CapnpSerializable.Create<Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters>(reader.Fert);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Fert?.serialize(writer.Fert);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters Fert
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
                public Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters.READER Fert => ctx.ReadStruct(0, Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters.READER.create);
                public bool HasFert => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters.WRITER Fert
                {
                    get => BuildPointer<Mas.Schema.Model.Monica.Params.MineralFertilization.Parameters.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd945095e4d1d3ad4UL)]
        public class Params_OrganicFertilizer : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd945095e4d1d3ad4UL;
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

            public string Id
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
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Id
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcf94305221c00752UL)]
        public class Result_OrganicFertilizer : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcf94305221c00752UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Fert = CapnpSerializable.Create<Mas.Schema.Model.Monica.Params.OrganicFertilization.OrganicMatterParameters>(reader.Fert);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Fert?.serialize(writer.Fert);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Model.Monica.Params.OrganicFertilization.OrganicMatterParameters Fert
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
                public Mas.Schema.Model.Monica.Params.OrganicFertilization.OrganicMatterParameters.READER Fert => ctx.ReadStruct(0, Mas.Schema.Model.Monica.Params.OrganicFertilization.OrganicMatterParameters.READER.create);
                public bool HasFert => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public Mas.Schema.Model.Monica.Params.OrganicFertilization.OrganicMatterParameters.WRITER Fert
                {
                    get => BuildPointer<Mas.Schema.Model.Monica.Params.OrganicFertilization.OrganicMatterParameters.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbfda1920aff38c07UL), Proxy(typeof(Service_Proxy)), Skeleton(typeof(Service_Skeleton))]
    public interface IService : Mas.Schema.Common.IIdentifiable
    {
        Task<IReadOnlyList<Mas.Schema.Model.Monica.Event>> ManagementAt(Mas.Schema.Geo.LatLonCoord arg_, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbfda1920aff38c07UL)]
    public class Service_Proxy : Proxy, IService
    {
        public async Task<IReadOnlyList<Mas.Schema.Model.Monica.Event>> ManagementAt(Mas.Schema.Geo.LatLonCoord arg_, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Geo.LatLonCoord.WRITER>();
            arg_?.serialize(in_);
            using (var d_ = await Call(13824389634348780551UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Model.Monica.Service.Result_ManagementAt>(d_);
                return (r_.Mgmt);
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbfda1920aff38c07UL)]
    public class Service_Skeleton : Skeleton<IService>
    {
        public Service_Skeleton()
        {
            SetMethodTable(ManagementAt);
        }

        public override ulong InterfaceId => 13824389634348780551UL;
        Task<AnswerOrCounterquestion> ManagementAt(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.ManagementAt(CapnpSerializable.Create<Mas.Schema.Geo.LatLonCoord>(d_), cancellationToken_), mgmt =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Model.Monica.Service.Result_ManagementAt.WRITER>();
                    var r_ = new Mas.Schema.Model.Monica.Service.Result_ManagementAt{Mgmt = mgmt};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Service
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf32d7a3fdc567bdbUL)]
        public class Result_ManagementAt : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf32d7a3fdc567bdbUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mgmt = reader.Mgmt?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Model.Monica.Event>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mgmt.Init(Mgmt, (_s1, _v1) => _v1?.serialize(_s1));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Schema.Model.Monica.Event> Mgmt
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
                public IReadOnlyList<Mas.Schema.Model.Monica.Event.READER> Mgmt => ctx.ReadList(0).Cast(Mas.Schema.Model.Monica.Event.READER.create);
                public bool HasMgmt => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfStructsSerializer<Mas.Schema.Model.Monica.Event.WRITER> Mgmt
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Model.Monica.Event.WRITER>>(0);
                    set => Link(0, value);
                }
            }
        }
    }
}