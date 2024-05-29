using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Schema.Management
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x82a74595175b71a3UL)]
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc2d50914b83d42deUL)]
    public enum PlantOrgan : ushort
    {
        root,
        leaf,
        shoot,
        fruit,
        strukt,
        sugar
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9c5dedfd679ac842UL)]
    public class Event : ICapnpSerializable
    {
        public const UInt64 typeId = 0x9c5dedfd679ac842UL;
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
                    At = CapnpSerializable.Create<Mas.Schema.Management.Event.at>(reader.At);
                    break;
                case WHICH.Between:
                    Between = CapnpSerializable.Create<Mas.Schema.Management.Event.between>(reader.Between);
                    break;
                case WHICH.After:
                    After = CapnpSerializable.Create<Mas.Schema.Management.Event.after>(reader.After);
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

        public Mas.Schema.Management.Event.ExternalType TheType
        {
            get;
            set;
        }

        public Mas.Schema.Common.IdInformation Info
        {
            get;
            set;
        }

        public Mas.Schema.Management.Event.at At
        {
            get => _which == WHICH.At ? (Mas.Schema.Management.Event.at)_content : null;
            set
            {
                _which = WHICH.At;
                _content = value;
            }
        }

        public Mas.Schema.Management.Event.between Between
        {
            get => _which == WHICH.Between ? (Mas.Schema.Management.Event.between)_content : null;
            set
            {
                _which = WHICH.Between;
                _content = value;
            }
        }

        public Mas.Schema.Management.Event.after After
        {
            get => _which == WHICH.After ? (Mas.Schema.Management.Event.after)_content : null;
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
            public Mas.Schema.Management.Event.ExternalType TheType => (Mas.Schema.Management.Event.ExternalType)ctx.ReadDataUShort(0UL, (ushort)0);
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

            public Mas.Schema.Management.Event.ExternalType TheType
            {
                get => (Mas.Schema.Management.Event.ExternalType)this.ReadDataUShort(0UL, (ushort)0);
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc6c4991fe51b272fUL)]
        public class at : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc6c4991fe51b272fUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9539b8e14ac7d5a9UL)]
        public class between : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9539b8e14ac7d5a9UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc58610b90af83811UL)]
        public class after : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc58610b90af83811UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Event = CapnpSerializable.Create<Mas.Schema.Management.Event.Type>(reader.Event);
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

            public Mas.Schema.Management.Event.Type Event
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
                public Mas.Schema.Management.Event.Type.READER Event => ctx.ReadStruct(1, Mas.Schema.Management.Event.Type.READER.create);
                public bool HasEvent => ctx.IsStructFieldNonNull(1);
                public ushort Days => ctx.ReadDataUShort(32UL, (ushort)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                }

                public Mas.Schema.Management.Event.Type.WRITER Event
                {
                    get => BuildPointer<Mas.Schema.Management.Event.Type.WRITER>(1);
                    set => Link(1, value);
                }

                public ushort Days
                {
                    get => this.ReadDataUShort(32UL, (ushort)0);
                    set => this.WriteData(32UL, value, (ushort)0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf082ec2d0eb50c9bUL)]
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8fa09457bc1bfc34UL)]
        public enum PhenoStage : ushort
        {
            emergence,
            flowering,
            anthesis,
            maturity
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe1ed73d59c8ce359UL)]
        public class Type : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe1ed73d59c8ce359UL;
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
                            _content = (Mas.Schema.Management.Event.ExternalType)0;
                            break;
                        case WHICH.Internal:
                            _content = (Mas.Schema.Management.Event.PhenoStage)0;
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

            public Mas.Schema.Management.Event.ExternalType? External
            {
                get => _which == WHICH.External ? (Mas.Schema.Management.Event.ExternalType? )_content : null;
                set
                {
                    _which = WHICH.External;
                    _content = value;
                }
            }

            public Mas.Schema.Management.Event.PhenoStage? Internal
            {
                get => _which == WHICH.Internal ? (Mas.Schema.Management.Event.PhenoStage? )_content : null;
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
                public Mas.Schema.Management.Event.ExternalType External => which == WHICH.External ? (Mas.Schema.Management.Event.ExternalType)ctx.ReadDataUShort(0UL, (ushort)0) : default;
                public Mas.Schema.Management.Event.PhenoStage Internal => which == WHICH.Internal ? (Mas.Schema.Management.Event.PhenoStage)ctx.ReadDataUShort(0UL, (ushort)0) : default;
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

                public Mas.Schema.Management.Event.ExternalType External
                {
                    get => which == WHICH.External ? (Mas.Schema.Management.Event.ExternalType)this.ReadDataUShort(0UL, (ushort)0) : default;
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }

                public Mas.Schema.Management.Event.PhenoStage Internal
                {
                    get => which == WHICH.Internal ? (Mas.Schema.Management.Event.PhenoStage)this.ReadDataUShort(0UL, (ushort)0) : default;
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9d247c812334c917UL)]
    public class Params : ICapnpSerializable
    {
        public const UInt64 typeId = 0x9d247c812334c917UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x80ce153f3bc9a9e8UL)]
        public class Sowing : ICapnpSerializable
        {
            public const UInt64 typeId = 0x80ce153f3bc9a9e8UL;
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

            public string Cultivar
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
                public string Cultivar => ctx.ReadText(0, null);
                public ushort PlantDensity => ctx.ReadDataUShort(0UL, (ushort)0);
                public Mas.Schema.Crop.ICrop Crop => ctx.ReadCap<Mas.Schema.Crop.ICrop>(1);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 2);
                }

                public string Cultivar
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public ushort PlantDensity
                {
                    get => this.ReadDataUShort(0UL, (ushort)0);
                    set => this.WriteData(0UL, value, (ushort)0);
                }

                public Mas.Schema.Crop.ICrop Crop
                {
                    get => ReadCap<Mas.Schema.Crop.ICrop>(1);
                    set => LinkObject(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcfcf44997e7ceab4UL)]
        public class AutomaticSowing : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcfcf44997e7ceab4UL;
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
                TheAvgSoilTemp = CapnpSerializable.Create<Mas.Schema.Management.Params.AutomaticSowing.AvgSoilTemp>(reader.TheAvgSoilTemp);
                Sowing = CapnpSerializable.Create<Mas.Schema.Management.Params.Sowing>(reader.Sowing);
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

            public Mas.Schema.Management.Params.AutomaticSowing.AvgSoilTemp TheAvgSoilTemp
            {
                get;
                set;
            }

            public Mas.Schema.Management.Params.Sowing Sowing
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
                public Mas.Schema.Management.Params.AutomaticSowing.AvgSoilTemp.READER TheAvgSoilTemp => ctx.ReadStruct(0, Mas.Schema.Management.Params.AutomaticSowing.AvgSoilTemp.READER.create);
                public bool HasTheAvgSoilTemp => ctx.IsStructFieldNonNull(0);
                public Mas.Schema.Management.Params.Sowing.READER Sowing => ctx.ReadStruct(1, Mas.Schema.Management.Params.Sowing.READER.create);
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

                public Mas.Schema.Management.Params.AutomaticSowing.AvgSoilTemp.WRITER TheAvgSoilTemp
                {
                    get => BuildPointer<Mas.Schema.Management.Params.AutomaticSowing.AvgSoilTemp.WRITER>(0);
                    set => Link(0, value);
                }

                public Mas.Schema.Management.Params.Sowing.WRITER Sowing
                {
                    get => BuildPointer<Mas.Schema.Management.Params.Sowing.WRITER>(1);
                    set => Link(1, value);
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9d81d2bf4cd0f868UL)]
            public class AvgSoilTemp : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9d81d2bf4cd0f868UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xeed4e55bb04289efUL)]
        public class Harvest : ICapnpSerializable
        {
            public const UInt64 typeId = 0xeed4e55bb04289efUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Exported = reader.Exported;
                OptCarbMgmtData = CapnpSerializable.Create<Mas.Schema.Management.Params.Harvest.OptCarbonMgmtData>(reader.OptCarbMgmtData);
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
            public Mas.Schema.Management.Params.Harvest.OptCarbonMgmtData OptCarbMgmtData
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
                public Mas.Schema.Management.Params.Harvest.OptCarbonMgmtData.READER OptCarbMgmtData => ctx.ReadStruct(0, Mas.Schema.Management.Params.Harvest.OptCarbonMgmtData.READER.create);
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

                public Mas.Schema.Management.Params.Harvest.OptCarbonMgmtData.WRITER OptCarbMgmtData
                {
                    get => BuildPointer<Mas.Schema.Management.Params.Harvest.OptCarbonMgmtData.WRITER>(0);
                    set => Link(0, value);
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8f0cbec420589373UL)]
            public enum CropUsage : ushort
            {
                greenManure,
                biomassProduction
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8cb6b3e3c50d3665UL)]
            public class OptCarbonMgmtData : ICapnpSerializable
            {
                public const UInt64 typeId = 0x8cb6b3e3c50d3665UL;
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
                public Mas.Schema.Management.Params.Harvest.CropUsage CropUsage
                {
                    get;
                    set;
                }

                = Mas.Schema.Management.Params.Harvest.CropUsage.biomassProduction;
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
                    public Mas.Schema.Management.Params.Harvest.CropUsage CropUsage => (Mas.Schema.Management.Params.Harvest.CropUsage)ctx.ReadDataUShort(16UL, (ushort)1);
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

                    public Mas.Schema.Management.Params.Harvest.CropUsage CropUsage
                    {
                        get => (Mas.Schema.Management.Params.Harvest.CropUsage)this.ReadDataUShort(16UL, (ushort)1);
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe3a37e340f816cd1UL)]
        public class AutomaticHarvest : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe3a37e340f816cd1UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                MinPercentASW = reader.MinPercentASW;
                MaxPercentASW = reader.MaxPercentASW;
                Max3dayPrecipSum = reader.Max3dayPrecipSum;
                MaxCurrentDayPrecipSum = reader.MaxCurrentDayPrecipSum;
                HarvestTime = reader.HarvestTime;
                Harvest = CapnpSerializable.Create<Mas.Schema.Management.Params.Harvest>(reader.Harvest);
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

            public Mas.Schema.Management.Event.PhenoStage HarvestTime
            {
                get;
                set;
            }

            = Mas.Schema.Management.Event.PhenoStage.maturity;
            public Mas.Schema.Management.Params.Harvest Harvest
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
                public Mas.Schema.Management.Event.PhenoStage HarvestTime => (Mas.Schema.Management.Event.PhenoStage)ctx.ReadDataUShort(256UL, (ushort)3);
                public Mas.Schema.Management.Params.Harvest.READER Harvest => ctx.ReadStruct(0, Mas.Schema.Management.Params.Harvest.READER.create);
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

                public Mas.Schema.Management.Event.PhenoStage HarvestTime
                {
                    get => (Mas.Schema.Management.Event.PhenoStage)this.ReadDataUShort(256UL, (ushort)3);
                    set => this.WriteData(256UL, (ushort)value, (ushort)3);
                }

                public Mas.Schema.Management.Params.Harvest.WRITER Harvest
                {
                    get => BuildPointer<Mas.Schema.Management.Params.Harvest.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfec75f2ddd43431dUL)]
        public class Cutting : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfec75f2ddd43431dUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                CuttingSpec = reader.CuttingSpec?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Management.Params.Cutting.Spec>(_));
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

            public IReadOnlyList<Mas.Schema.Management.Params.Cutting.Spec> CuttingSpec
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
                public IReadOnlyList<Mas.Schema.Management.Params.Cutting.Spec.READER> CuttingSpec => ctx.ReadList(0).Cast(Mas.Schema.Management.Params.Cutting.Spec.READER.create);
                public bool HasCuttingSpec => ctx.IsStructFieldNonNull(0);
                public double CutMaxAssimilationRatePercentage => ctx.ReadDataDouble(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public ListOfStructsSerializer<Mas.Schema.Management.Params.Cutting.Spec.WRITER> CuttingSpec
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Management.Params.Cutting.Spec.WRITER>>(0);
                    set => Link(0, value);
                }

                public double CutMaxAssimilationRatePercentage
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x825bb2508c0b37b2UL)]
            public enum CL : ushort
            {
                cut,
                left
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf0c763e472409ba2UL)]
            public enum Unit : ushort
            {
                percentage,
                biomass,
                lai
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9a221e04faf79efcUL)]
            public class Spec : ICapnpSerializable
            {
                public const UInt64 typeId = 0x9a221e04faf79efcUL;
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

                public Mas.Schema.Management.PlantOrgan Organ
                {
                    get;
                    set;
                }

                public double Value
                {
                    get;
                    set;
                }

                public Mas.Schema.Management.Params.Cutting.Unit Unit
                {
                    get;
                    set;
                }

                = Mas.Schema.Management.Params.Cutting.Unit.percentage;
                public Mas.Schema.Management.Params.Cutting.CL CutOrLeft
                {
                    get;
                    set;
                }

                = Mas.Schema.Management.Params.Cutting.CL.cut;
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
                    public Mas.Schema.Management.PlantOrgan Organ => (Mas.Schema.Management.PlantOrgan)ctx.ReadDataUShort(0UL, (ushort)0);
                    public double Value => ctx.ReadDataDouble(64UL, 0);
                    public Mas.Schema.Management.Params.Cutting.Unit Unit => (Mas.Schema.Management.Params.Cutting.Unit)ctx.ReadDataUShort(16UL, (ushort)0);
                    public Mas.Schema.Management.Params.Cutting.CL CutOrLeft => (Mas.Schema.Management.Params.Cutting.CL)ctx.ReadDataUShort(32UL, (ushort)0);
                    public double ExportPercentage => ctx.ReadDataDouble(128UL, 100);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(3, 0);
                    }

                    public Mas.Schema.Management.PlantOrgan Organ
                    {
                        get => (Mas.Schema.Management.PlantOrgan)this.ReadDataUShort(0UL, (ushort)0);
                        set => this.WriteData(0UL, (ushort)value, (ushort)0);
                    }

                    public double Value
                    {
                        get => this.ReadDataDouble(64UL, 0);
                        set => this.WriteData(64UL, value, 0);
                    }

                    public Mas.Schema.Management.Params.Cutting.Unit Unit
                    {
                        get => (Mas.Schema.Management.Params.Cutting.Unit)this.ReadDataUShort(16UL, (ushort)0);
                        set => this.WriteData(16UL, (ushort)value, (ushort)0);
                    }

                    public Mas.Schema.Management.Params.Cutting.CL CutOrLeft
                    {
                        get => (Mas.Schema.Management.Params.Cutting.CL)this.ReadDataUShort(32UL, (ushort)0);
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd3da30ea7b25d921UL)]
        public class MineralFertilization : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd3da30ea7b25d921UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Fertilizer = reader.Fertilizer;
                Amount = reader.Amount;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Fertilizer = Fertilizer;
                writer.Amount = Amount;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Management.IFertilizer Fertilizer
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
                public Mas.Schema.Management.IFertilizer Fertilizer => ctx.ReadCap<Mas.Schema.Management.IFertilizer>(0);
                public double Amount => ctx.ReadDataDouble(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public Mas.Schema.Management.IFertilizer Fertilizer
                {
                    get => ReadCap<Mas.Schema.Management.IFertilizer>(0);
                    set => LinkObject(0, value);
                }

                public double Amount
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x953375ac67d4f573UL)]
        public class NDemandFertilization : ICapnpSerializable
        {
            public const UInt64 typeId = 0x953375ac67d4f573UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                NDemand = reader.NDemand;
                Fertilizer = reader.Fertilizer;
                Depth = reader.Depth;
                Stage = reader.Stage;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.NDemand = NDemand;
                writer.Fertilizer = Fertilizer;
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

            public Mas.Schema.Management.IFertilizer Fertilizer
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
                public Mas.Schema.Management.IFertilizer Fertilizer => ctx.ReadCap<Mas.Schema.Management.IFertilizer>(0);
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

                public Mas.Schema.Management.IFertilizer Fertilizer
                {
                    get => ReadCap<Mas.Schema.Management.IFertilizer>(0);
                    set => LinkObject(0, value);
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe98c76fb0fb0b2cdUL)]
        public class OrganicFertilization : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe98c76fb0fb0b2cdUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Fertilizer = reader.Fertilizer;
                Amount = reader.Amount;
                Incorporation = reader.Incorporation;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Fertilizer = Fertilizer;
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

            public Mas.Schema.Management.IFertilizer Fertilizer
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
                public Mas.Schema.Management.IFertilizer Fertilizer => ctx.ReadCap<Mas.Schema.Management.IFertilizer>(0);
                public double Amount => ctx.ReadDataDouble(0UL, 0);
                public bool Incorporation => ctx.ReadDataBool(64UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 1);
                }

                public Mas.Schema.Management.IFertilizer Fertilizer
                {
                    get => ReadCap<Mas.Schema.Management.IFertilizer>(0);
                    set => LinkObject(0, value);
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
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x88a5848ef8603554UL)]
        public class Tillage : ICapnpSerializable
        {
            public const UInt64 typeId = 0x88a5848ef8603554UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x87feb816363ff43cUL)]
        public class Irrigation : ICapnpSerializable
        {
            public const UInt64 typeId = 0x87feb816363ff43cUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Amount = reader.Amount;
                NutrientConcentrations = reader.NutrientConcentrations?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Management.Nutrient>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Amount = Amount;
                writer.NutrientConcentrations.Init(NutrientConcentrations, (_s1, _v1) => _v1?.serialize(_s1));
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

            public IReadOnlyList<Mas.Schema.Management.Nutrient> NutrientConcentrations
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
                public IReadOnlyList<Mas.Schema.Management.Nutrient.READER> NutrientConcentrations => ctx.ReadList(0).Cast(Mas.Schema.Management.Nutrient.READER.create);
                public bool HasNutrientConcentrations => ctx.IsStructFieldNonNull(0);
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

                public ListOfStructsSerializer<Mas.Schema.Management.Nutrient.WRITER> NutrientConcentrations
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Management.Nutrient.WRITER>>(0);
                    set => Link(0, value);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaafe4332e17aa43eUL)]
    public class Nutrient : ICapnpSerializable
    {
        public const UInt64 typeId = 0xaafe4332e17aa43eUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            TheNutrient = reader.TheNutrient;
            Value = reader.Value;
            TheUnit = reader.TheUnit;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.TheNutrient = TheNutrient;
            writer.Value = Value;
            writer.TheUnit = TheUnit;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Schema.Management.Nutrient.Name TheNutrient
        {
            get;
            set;
        }

        public double Value
        {
            get;
            set;
        }

        public Mas.Schema.Management.Nutrient.Unit TheUnit
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
            public Mas.Schema.Management.Nutrient.Name TheNutrient => (Mas.Schema.Management.Nutrient.Name)ctx.ReadDataUShort(0UL, (ushort)0);
            public double Value => ctx.ReadDataDouble(64UL, 0);
            public Mas.Schema.Management.Nutrient.Unit TheUnit => (Mas.Schema.Management.Nutrient.Unit)ctx.ReadDataUShort(16UL, (ushort)0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(2, 0);
            }

            public Mas.Schema.Management.Nutrient.Name TheNutrient
            {
                get => (Mas.Schema.Management.Nutrient.Name)this.ReadDataUShort(0UL, (ushort)0);
                set => this.WriteData(0UL, (ushort)value, (ushort)0);
            }

            public double Value
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public Mas.Schema.Management.Nutrient.Unit TheUnit
            {
                get => (Mas.Schema.Management.Nutrient.Unit)this.ReadDataUShort(16UL, (ushort)0);
                set => this.WriteData(16UL, (ushort)value, (ushort)0);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbc6b579acf43fb6eUL)]
        public enum Name : ushort
        {
            urea,
            ammonia,
            nitrate,
            phosphorus,
            potassium,
            sulfate,
            organicC,
            organicN,
            organicP,
            organicNFast,
            organicNSlow
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x987b68b57edbbdb6UL)]
        public enum Unit : ushort
        {
            none,
            fraction,
            percent
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8c4cb8d60ae5aec7UL), Proxy(typeof(Fertilizer_Proxy)), Skeleton(typeof(Fertilizer_Skeleton))]
    public interface IFertilizer : Mas.Schema.Common.IIdentifiable, Mas.Schema.Persistence.IPersistent
    {
        Task<IReadOnlyList<Mas.Schema.Management.Nutrient>> Nutrients(CancellationToken cancellationToken_ = default);
        Task<object> Parameters(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8c4cb8d60ae5aec7UL)]
    public class Fertilizer_Proxy : Proxy, IFertilizer
    {
        public async Task<IReadOnlyList<Mas.Schema.Management.Nutrient>> Nutrients(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Management.Fertilizer.Params_Nutrients.WRITER>();
            var arg_ = new Mas.Schema.Management.Fertilizer.Params_Nutrients()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(10109658492985257671UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Management.Fertilizer.Result_Nutrients>(d_);
                return (r_.Nutrients);
            }
        }

        public Task<object> Parameters(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Management.Fertilizer.Params_Parameters.WRITER>();
            var arg_ = new Mas.Schema.Management.Fertilizer.Params_Parameters()
            {};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(10109658492985257671UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Management.Fertilizer.Result_Parameters>(d_);
                    return (r_.Params);
                }
            }

            );
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8c4cb8d60ae5aec7UL)]
    public class Fertilizer_Skeleton : Skeleton<IFertilizer>
    {
        public Fertilizer_Skeleton()
        {
            SetMethodTable(Nutrients, Parameters);
        }

        public override ulong InterfaceId => 10109658492985257671UL;
        Task<AnswerOrCounterquestion> Nutrients(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Nutrients(cancellationToken_), nutrients =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Management.Fertilizer.Result_Nutrients.WRITER>();
                    var r_ = new Mas.Schema.Management.Fertilizer.Result_Nutrients{Nutrients = nutrients};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> Parameters(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.Parameters(cancellationToken_), @params =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Management.Fertilizer.Result_Parameters.WRITER>();
                    var r_ = new Mas.Schema.Management.Fertilizer.Result_Parameters{Params = @params};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Fertilizer
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcb5a624fdc982a1bUL)]
        public class Params_Nutrients : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcb5a624fdc982a1bUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xae2976259bce5460UL)]
        public class Result_Nutrients : ICapnpSerializable
        {
            public const UInt64 typeId = 0xae2976259bce5460UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Nutrients = reader.Nutrients?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Management.Nutrient>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Nutrients.Init(Nutrients, (_s1, _v1) => _v1?.serialize(_s1));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Schema.Management.Nutrient> Nutrients
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
                public IReadOnlyList<Mas.Schema.Management.Nutrient.READER> Nutrients => ctx.ReadList(0).Cast(Mas.Schema.Management.Nutrient.READER.create);
                public bool HasNutrients => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfStructsSerializer<Mas.Schema.Management.Nutrient.WRITER> Nutrients
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Management.Nutrient.WRITER>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc0032af5b7bc50e4UL)]
        public class Params_Parameters : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc0032af5b7bc50e4UL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfd4dbbbb758bb8f7UL)]
        public class Result_Parameters : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfd4dbbbb758bb8f7UL;
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
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbbb7aeae0d097e05UL), Proxy(typeof(FertilizerService_Proxy)), Skeleton(typeof(FertilizerService_Skeleton))]
    public interface IFertilizerService : Mas.Schema.Registry.IRegistry
    {
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbbb7aeae0d097e05UL)]
    public class FertilizerService_Proxy : Proxy, IFertilizerService
    {
        public async Task<IReadOnlyList<Mas.Schema.Common.IdInformation>> SupportedCategories(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Registry.Registry.Params_SupportedCategories.WRITER>();
            var arg_ = new Mas.Schema.Registry.Registry.Params_SupportedCategories()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(14590338780428121016UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Registry.Registry.Result_SupportedCategories>(d_);
                return (r_.Cats);
            }
        }

        public async Task<Mas.Schema.Common.IdInformation> CategoryInfo(string categoryId, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Registry.Registry.Params_CategoryInfo.WRITER>();
            var arg_ = new Mas.Schema.Registry.Registry.Params_CategoryInfo()
            {CategoryId = categoryId};
            arg_?.serialize(in_);
            using (var d_ = await Call(14590338780428121016UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Common.IdInformation>(d_);
                return r_;
            }
        }

        public Task<IReadOnlyList<Mas.Schema.Registry.Registry.Entry>> Entries(string categoryId, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Registry.Registry.Params_Entries.WRITER>();
            var arg_ = new Mas.Schema.Registry.Registry.Params_Entries()
            {CategoryId = categoryId};
            arg_?.serialize(in_);
            return Impatient.MakePipelineAware(Call(14590338780428121016UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_), d_ =>
            {
                using (d_)
                {
                    var r_ = CapnpSerializable.Create<Mas.Schema.Registry.Registry.Result_Entries>(d_);
                    return (r_.Entries);
                }
            }

            );
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbbb7aeae0d097e05UL)]
    public class FertilizerService_Skeleton : Skeleton<IFertilizerService>
    {
        public FertilizerService_Skeleton()
        {
            SetMethodTable();
        }

        public override ulong InterfaceId => 13526472068396842501UL;
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc876b729b7d7f6d9UL), Proxy(typeof(Service_Proxy)), Skeleton(typeof(Service_Skeleton))]
    public interface IService : Mas.Schema.Common.IIdentifiable
    {
        Task<IReadOnlyList<Mas.Schema.Management.Event>> ManagementAt(Mas.Schema.Geo.LatLonCoord arg_, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc876b729b7d7f6d9UL)]
    public class Service_Proxy : Proxy, IService
    {
        public async Task<IReadOnlyList<Mas.Schema.Management.Event>> ManagementAt(Mas.Schema.Geo.LatLonCoord arg_, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<Mas.Schema.Geo.LatLonCoord.WRITER>();
            arg_?.serialize(in_);
            using (var d_ = await Call(14444934244643370713UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<Mas.Schema.Management.Service.Result_ManagementAt>(d_);
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc876b729b7d7f6d9UL)]
    public class Service_Skeleton : Skeleton<IService>
    {
        public Service_Skeleton()
        {
            SetMethodTable(ManagementAt);
        }

        public override ulong InterfaceId => 14444934244643370713UL;
        Task<AnswerOrCounterquestion> ManagementAt(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.ManagementAt(CapnpSerializable.Create<Mas.Schema.Geo.LatLonCoord>(d_), cancellationToken_), mgmt =>
                {
                    var s_ = SerializerState.CreateForRpc<Mas.Schema.Management.Service.Result_ManagementAt.WRITER>();
                    var r_ = new Mas.Schema.Management.Service.Result_ManagementAt{Mgmt = mgmt};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class Service
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcca7748d367db151UL)]
        public class Result_ManagementAt : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcca7748d367db151UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mgmt = reader.Mgmt?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Management.Event>(_));
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

            public IReadOnlyList<Mas.Schema.Management.Event> Mgmt
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
                public IReadOnlyList<Mas.Schema.Management.Event.READER> Mgmt => ctx.ReadList(0).Cast(Mas.Schema.Management.Event.READER.create);
                public bool HasMgmt => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfStructsSerializer<Mas.Schema.Management.Event.WRITER> Mgmt
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Management.Event.WRITER>>(0);
                    set => Link(0, value);
                }
            }
        }
    }
}