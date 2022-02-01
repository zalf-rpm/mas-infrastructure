using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Models.Yieldstat
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcfe218c48d227e0dUL)]
    public enum ResultId : ushort
    {
        primaryYield,
        dryMatter,
        carbonInAboveGroundBiomass,
        sumFertilizer,
        sumIrrigation,
        primaryYieldCU
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa47f8d65869200afUL)]
    public class RestInput : ICapnpSerializable
    {
        public const UInt64 typeId = 0xa47f8d65869200afUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            UseDevTrend = reader.UseDevTrend;
            UseCO2Increase = reader.UseCO2Increase;
            Dgm = reader.Dgm;
            Hft = reader.Hft;
            Nft = reader.Nft;
            Sft = reader.Sft;
            Slope = reader.Slope;
            Steino = reader.Steino;
            Az = reader.Az;
            Klz = reader.Klz;
            Stt = reader.Stt;
            GermanFederalStates = reader.GermanFederalStates;
            GetDryYearWaterNeed = reader.GetDryYearWaterNeed;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.UseDevTrend = UseDevTrend;
            writer.UseCO2Increase = UseCO2Increase;
            writer.Dgm = Dgm;
            writer.Hft = Hft;
            writer.Nft = Nft;
            writer.Sft = Sft;
            writer.Slope = Slope;
            writer.Steino = Steino;
            writer.Az = Az;
            writer.Klz = Klz;
            writer.Stt = Stt;
            writer.GermanFederalStates = GermanFederalStates;
            writer.GetDryYearWaterNeed = GetDryYearWaterNeed;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public bool UseDevTrend
        {
            get;
            set;
        }

        = false;
        public bool UseCO2Increase
        {
            get;
            set;
        }

        = true;
        public double Dgm
        {
            get;
            set;
        }

        = 0;
        public byte Hft
        {
            get;
            set;
        }

        = 0;
        public byte Nft
        {
            get;
            set;
        }

        = 0;
        public byte Sft
        {
            get;
            set;
        }

        = 0;
        public byte Slope
        {
            get;
            set;
        }

        = 0;
        public byte Steino
        {
            get;
            set;
        }

        = 0;
        public byte Az
        {
            get;
            set;
        }

        = 0;
        public byte Klz
        {
            get;
            set;
        }

        = 0;
        public byte Stt
        {
            get;
            set;
        }

        = 0;
        public sbyte GermanFederalStates
        {
            get;
            set;
        }

        = -1;
        public bool GetDryYearWaterNeed
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
            public bool UseDevTrend => ctx.ReadDataBool(0UL, false);
            public bool UseCO2Increase => ctx.ReadDataBool(1UL, true);
            public double Dgm => ctx.ReadDataDouble(64UL, 0);
            public byte Hft => ctx.ReadDataByte(8UL, (byte)0);
            public byte Nft => ctx.ReadDataByte(16UL, (byte)0);
            public byte Sft => ctx.ReadDataByte(24UL, (byte)0);
            public byte Slope => ctx.ReadDataByte(32UL, (byte)0);
            public byte Steino => ctx.ReadDataByte(40UL, (byte)0);
            public byte Az => ctx.ReadDataByte(48UL, (byte)0);
            public byte Klz => ctx.ReadDataByte(56UL, (byte)0);
            public byte Stt => ctx.ReadDataByte(128UL, (byte)0);
            public sbyte GermanFederalStates => ctx.ReadDataSByte(136UL, (sbyte)-1);
            public bool GetDryYearWaterNeed => ctx.ReadDataBool(2UL, false);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(3, 0);
            }

            public bool UseDevTrend
            {
                get => this.ReadDataBool(0UL, false);
                set => this.WriteData(0UL, value, false);
            }

            public bool UseCO2Increase
            {
                get => this.ReadDataBool(1UL, true);
                set => this.WriteData(1UL, value, true);
            }

            public double Dgm
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public byte Hft
            {
                get => this.ReadDataByte(8UL, (byte)0);
                set => this.WriteData(8UL, value, (byte)0);
            }

            public byte Nft
            {
                get => this.ReadDataByte(16UL, (byte)0);
                set => this.WriteData(16UL, value, (byte)0);
            }

            public byte Sft
            {
                get => this.ReadDataByte(24UL, (byte)0);
                set => this.WriteData(24UL, value, (byte)0);
            }

            public byte Slope
            {
                get => this.ReadDataByte(32UL, (byte)0);
                set => this.WriteData(32UL, value, (byte)0);
            }

            public byte Steino
            {
                get => this.ReadDataByte(40UL, (byte)0);
                set => this.WriteData(40UL, value, (byte)0);
            }

            public byte Az
            {
                get => this.ReadDataByte(48UL, (byte)0);
                set => this.WriteData(48UL, value, (byte)0);
            }

            public byte Klz
            {
                get => this.ReadDataByte(56UL, (byte)0);
                set => this.WriteData(56UL, value, (byte)0);
            }

            public byte Stt
            {
                get => this.ReadDataByte(128UL, (byte)0);
                set => this.WriteData(128UL, value, (byte)0);
            }

            public sbyte GermanFederalStates
            {
                get => this.ReadDataSByte(136UL, (sbyte)-1);
                set => this.WriteData(136UL, value, (sbyte)-1);
            }

            public bool GetDryYearWaterNeed
            {
                get => this.ReadDataBool(2UL, false);
                set => this.WriteData(2UL, value, false);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8db55634a0e7d054UL)]
    public class Result : ICapnpSerializable
    {
        public const UInt64 typeId = 0x8db55634a0e7d054UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Cultivar = reader.Cultivar;
            IsNoData = reader.IsNoData;
            Values = reader.Values?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Models.Yieldstat.Result.ResultToValue>(_));
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Cultivar = Cultivar;
            writer.IsNoData = IsNoData;
            writer.Values.Init(Values, (_s1, _v1) => _v1?.serialize(_s1));
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Rpc.Crop.Cultivar Cultivar
        {
            get;
            set;
        }

        public bool IsNoData
        {
            get;
            set;
        }

        = false;
        public IReadOnlyList<Mas.Models.Yieldstat.Result.ResultToValue> Values
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
            public Mas.Rpc.Crop.Cultivar Cultivar => (Mas.Rpc.Crop.Cultivar)ctx.ReadDataUShort(0UL, (ushort)0);
            public bool IsNoData => ctx.ReadDataBool(16UL, false);
            public IReadOnlyList<Mas.Models.Yieldstat.Result.ResultToValue.READER> Values => ctx.ReadList(0).Cast(Mas.Models.Yieldstat.Result.ResultToValue.READER.create);
            public bool HasValues => ctx.IsStructFieldNonNull(0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 1);
            }

            public Mas.Rpc.Crop.Cultivar Cultivar
            {
                get => (Mas.Rpc.Crop.Cultivar)this.ReadDataUShort(0UL, (ushort)0);
                set => this.WriteData(0UL, (ushort)value, (ushort)0);
            }

            public bool IsNoData
            {
                get => this.ReadDataBool(16UL, false);
                set => this.WriteData(16UL, value, false);
            }

            public ListOfStructsSerializer<Mas.Models.Yieldstat.Result.ResultToValue.WRITER> Values
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Models.Yieldstat.Result.ResultToValue.WRITER>>(0);
                set => Link(0, value);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8d365bd4f0136fc0UL)]
        public class ResultToValue : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8d365bd4f0136fc0UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Id = reader.Id;
                Value = reader.Value;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Id = Id;
                writer.Value = Value;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Models.Yieldstat.ResultId Id
            {
                get;
                set;
            }

            public double Value
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
                public Mas.Models.Yieldstat.ResultId Id => (Mas.Models.Yieldstat.ResultId)ctx.ReadDataUShort(0UL, (ushort)0);
                public double Value => ctx.ReadDataDouble(64UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 0);
                }

                public Mas.Models.Yieldstat.ResultId Id
                {
                    get => (Mas.Models.Yieldstat.ResultId)this.ReadDataUShort(0UL, (ushort)0);
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }

                public double Value
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x932a681f81b4be19UL)]
    public class Output : ICapnpSerializable
    {
        public const UInt64 typeId = 0x932a681f81b4be19UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Id = reader.Id;
            RunFailed = reader.RunFailed;
            Reason = reader.Reason;
            Results = reader.Results?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Models.Yieldstat.Output.YearToResult>(_));
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Id = Id;
            writer.RunFailed = RunFailed;
            writer.Reason = Reason;
            writer.Results.Init(Results, (_s1, _v1) => _v1?.serialize(_s1));
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

        public bool RunFailed
        {
            get;
            set;
        }

        = false;
        public string Reason
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Models.Yieldstat.Output.YearToResult> Results
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
            public bool RunFailed => ctx.ReadDataBool(0UL, false);
            public string Reason => ctx.ReadText(1, null);
            public IReadOnlyList<Mas.Models.Yieldstat.Output.YearToResult.READER> Results => ctx.ReadList(2).Cast(Mas.Models.Yieldstat.Output.YearToResult.READER.create);
            public bool HasResults => ctx.IsStructFieldNonNull(2);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 3);
            }

            public string Id
            {
                get => this.ReadText(0, null);
                set => this.WriteText(0, value, null);
            }

            public bool RunFailed
            {
                get => this.ReadDataBool(0UL, false);
                set => this.WriteData(0UL, value, false);
            }

            public string Reason
            {
                get => this.ReadText(1, null);
                set => this.WriteText(1, value, null);
            }

            public ListOfStructsSerializer<Mas.Models.Yieldstat.Output.YearToResult.WRITER> Results
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Models.Yieldstat.Output.YearToResult.WRITER>>(2);
                set => Link(2, value);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa008c533888c3a5eUL)]
        public class YearToResult : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa008c533888c3a5eUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Year = reader.Year;
                Result = CapnpSerializable.Create<Mas.Models.Yieldstat.Result>(reader.Result);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Year = Year;
                Result?.serialize(writer.Result);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public short Year
            {
                get;
                set;
            }

            public Mas.Models.Yieldstat.Result Result
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
                public short Year => ctx.ReadDataShort(0UL, (short)0);
                public Mas.Models.Yieldstat.Result.READER Result => ctx.ReadStruct(0, Mas.Models.Yieldstat.Result.READER.create);
                public bool HasResult => ctx.IsStructFieldNonNull(0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public short Year
                {
                    get => this.ReadDataShort(0UL, (short)0);
                    set => this.WriteData(0UL, value, (short)0);
                }

                public Mas.Models.Yieldstat.Result.WRITER Result
                {
                    get => BuildPointer<Mas.Models.Yieldstat.Result.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }
    }
}