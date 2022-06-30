using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Schema.Model.Monica
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd8af9210839bc071UL)]
    public class MaybeBool : ICapnpSerializable
    {
        public const UInt64 typeId = 0xd8af9210839bc071UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Value = reader.Value;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Value = Value;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public bool Value
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
            public bool Value => ctx.ReadDataBool(0UL, false);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 0);
            }

            public bool Value
            {
                get => this.ReadDataBool(0UL, false);
                set => this.WriteData(0UL, value, false);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd599d06dc405571aUL)]
    public class RuntimeState : ICapnpSerializable
    {
        public const UInt64 typeId = 0xd599d06dc405571aUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            ModelState = CapnpSerializable.Create<Mas.Schema.Model.Monica.MonicaModelState>(reader.ModelState);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            ModelState?.serialize(writer.ModelState);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Schema.Model.Monica.MonicaModelState ModelState
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
            public Mas.Schema.Model.Monica.MonicaModelState.READER ModelState => ctx.ReadStruct(0, Mas.Schema.Model.Monica.MonicaModelState.READER.create);
            public bool HasModelState => ctx.IsStructFieldNonNull(0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 1);
            }

            public Mas.Schema.Model.Monica.MonicaModelState.WRITER ModelState
            {
                get => BuildPointer<Mas.Schema.Model.Monica.MonicaModelState.WRITER>(0);
                set => Link(0, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8b008567c93f7c7dUL)]
    public class CropState : ICapnpSerializable
    {
        public const UInt64 typeId = 0x8b008567c93f7c7dUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            AutomaticHarvestParams = CapnpSerializable.Create<Mas.Schema.Model.Monica.AutomaticHarvestParameters>(reader.AutomaticHarvestParams);
            SpeciesName = reader.SpeciesName;
            CultivarName = reader.CultivarName;
            SeedDate = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.SeedDate);
            HarvestDate = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.HarvestDate);
            IsWinterCrop = CapnpSerializable.Create<Mas.Schema.Model.Monica.MaybeBool>(reader.IsWinterCrop);
            IsPerennialCrop = CapnpSerializable.Create<Mas.Schema.Model.Monica.MaybeBool>(reader.IsPerennialCrop);
            CuttingDates = reader.CuttingDates?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Common.Date>(_));
            CropParams = CapnpSerializable.Create<Mas.Schema.Model.Monica.CropParameters>(reader.CropParams);
            PerennialCropParams = CapnpSerializable.Create<Mas.Schema.Model.Monica.CropParameters>(reader.PerennialCropParams);
            ResidueParams = CapnpSerializable.Create<Mas.Schema.Model.Monica.CropResidueParameters>(reader.ResidueParams);
            CrossCropAdaptionFactor = reader.CrossCropAdaptionFactor;
            AutomaticHarvest = reader.AutomaticHarvest;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            AutomaticHarvestParams?.serialize(writer.AutomaticHarvestParams);
            writer.SpeciesName = SpeciesName;
            writer.CultivarName = CultivarName;
            SeedDate?.serialize(writer.SeedDate);
            HarvestDate?.serialize(writer.HarvestDate);
            IsWinterCrop?.serialize(writer.IsWinterCrop);
            IsPerennialCrop?.serialize(writer.IsPerennialCrop);
            writer.CuttingDates.Init(CuttingDates, (_s1, _v1) => _v1?.serialize(_s1));
            CropParams?.serialize(writer.CropParams);
            PerennialCropParams?.serialize(writer.PerennialCropParams);
            ResidueParams?.serialize(writer.ResidueParams);
            writer.CrossCropAdaptionFactor = CrossCropAdaptionFactor;
            writer.AutomaticHarvest = AutomaticHarvest;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Schema.Model.Monica.AutomaticHarvestParameters AutomaticHarvestParams
        {
            get;
            set;
        }

        public string SpeciesName
        {
            get;
            set;
        }

        public string CultivarName
        {
            get;
            set;
        }

        public Mas.Schema.Common.Date SeedDate
        {
            get;
            set;
        }

        public Mas.Schema.Common.Date HarvestDate
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.MaybeBool IsWinterCrop
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.MaybeBool IsPerennialCrop
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Schema.Common.Date> CuttingDates
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.CropParameters CropParams
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.CropParameters PerennialCropParams
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.CropResidueParameters ResidueParams
        {
            get;
            set;
        }

        public double CrossCropAdaptionFactor
        {
            get;
            set;
        }

        = 1;
        public bool AutomaticHarvest
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
            public Mas.Schema.Model.Monica.AutomaticHarvestParameters.READER AutomaticHarvestParams => ctx.ReadStruct(0, Mas.Schema.Model.Monica.AutomaticHarvestParameters.READER.create);
            public bool HasAutomaticHarvestParams => ctx.IsStructFieldNonNull(0);
            public string SpeciesName => ctx.ReadText(1, null);
            public string CultivarName => ctx.ReadText(2, null);
            public Mas.Schema.Common.Date.READER SeedDate => ctx.ReadStruct(3, Mas.Schema.Common.Date.READER.create);
            public bool HasSeedDate => ctx.IsStructFieldNonNull(3);
            public Mas.Schema.Common.Date.READER HarvestDate => ctx.ReadStruct(4, Mas.Schema.Common.Date.READER.create);
            public bool HasHarvestDate => ctx.IsStructFieldNonNull(4);
            public Mas.Schema.Model.Monica.MaybeBool.READER IsWinterCrop => ctx.ReadStruct(5, Mas.Schema.Model.Monica.MaybeBool.READER.create);
            public bool HasIsWinterCrop => ctx.IsStructFieldNonNull(5);
            public Mas.Schema.Model.Monica.MaybeBool.READER IsPerennialCrop => ctx.ReadStruct(6, Mas.Schema.Model.Monica.MaybeBool.READER.create);
            public bool HasIsPerennialCrop => ctx.IsStructFieldNonNull(6);
            public IReadOnlyList<Mas.Schema.Common.Date.READER> CuttingDates => ctx.ReadList(7).Cast(Mas.Schema.Common.Date.READER.create);
            public bool HasCuttingDates => ctx.IsStructFieldNonNull(7);
            public Mas.Schema.Model.Monica.CropParameters.READER CropParams => ctx.ReadStruct(8, Mas.Schema.Model.Monica.CropParameters.READER.create);
            public bool HasCropParams => ctx.IsStructFieldNonNull(8);
            public Mas.Schema.Model.Monica.CropParameters.READER PerennialCropParams => ctx.ReadStruct(9, Mas.Schema.Model.Monica.CropParameters.READER.create);
            public bool HasPerennialCropParams => ctx.IsStructFieldNonNull(9);
            public Mas.Schema.Model.Monica.CropResidueParameters.READER ResidueParams => ctx.ReadStruct(10, Mas.Schema.Model.Monica.CropResidueParameters.READER.create);
            public bool HasResidueParams => ctx.IsStructFieldNonNull(10);
            public double CrossCropAdaptionFactor => ctx.ReadDataDouble(0UL, 1);
            public bool AutomaticHarvest => ctx.ReadDataBool(64UL, false);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(2, 11);
            }

            public Mas.Schema.Model.Monica.AutomaticHarvestParameters.WRITER AutomaticHarvestParams
            {
                get => BuildPointer<Mas.Schema.Model.Monica.AutomaticHarvestParameters.WRITER>(0);
                set => Link(0, value);
            }

            public string SpeciesName
            {
                get => this.ReadText(1, null);
                set => this.WriteText(1, value, null);
            }

            public string CultivarName
            {
                get => this.ReadText(2, null);
                set => this.WriteText(2, value, null);
            }

            public Mas.Schema.Common.Date.WRITER SeedDate
            {
                get => BuildPointer<Mas.Schema.Common.Date.WRITER>(3);
                set => Link(3, value);
            }

            public Mas.Schema.Common.Date.WRITER HarvestDate
            {
                get => BuildPointer<Mas.Schema.Common.Date.WRITER>(4);
                set => Link(4, value);
            }

            public Mas.Schema.Model.Monica.MaybeBool.WRITER IsWinterCrop
            {
                get => BuildPointer<Mas.Schema.Model.Monica.MaybeBool.WRITER>(5);
                set => Link(5, value);
            }

            public Mas.Schema.Model.Monica.MaybeBool.WRITER IsPerennialCrop
            {
                get => BuildPointer<Mas.Schema.Model.Monica.MaybeBool.WRITER>(6);
                set => Link(6, value);
            }

            public ListOfStructsSerializer<Mas.Schema.Common.Date.WRITER> CuttingDates
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Common.Date.WRITER>>(7);
                set => Link(7, value);
            }

            public Mas.Schema.Model.Monica.CropParameters.WRITER CropParams
            {
                get => BuildPointer<Mas.Schema.Model.Monica.CropParameters.WRITER>(8);
                set => Link(8, value);
            }

            public Mas.Schema.Model.Monica.CropParameters.WRITER PerennialCropParams
            {
                get => BuildPointer<Mas.Schema.Model.Monica.CropParameters.WRITER>(9);
                set => Link(9, value);
            }

            public Mas.Schema.Model.Monica.CropResidueParameters.WRITER ResidueParams
            {
                get => BuildPointer<Mas.Schema.Model.Monica.CropResidueParameters.WRITER>(10);
                set => Link(10, value);
            }

            public double CrossCropAdaptionFactor
            {
                get => this.ReadDataDouble(0UL, 1);
                set => this.WriteData(0UL, value, 1);
            }

            public bool AutomaticHarvest
            {
                get => this.ReadDataBool(64UL, false);
                set => this.WriteData(64UL, value, false);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe3512e62df901c18UL)]
    public class AOMProperties : ICapnpSerializable
    {
        public const UInt64 typeId = 0xe3512e62df901c18UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            AomSlow = reader.AomSlow;
            AomFast = reader.AomFast;
            AomSlowDecRatetoSMBSlow = reader.AomSlowDecRatetoSMBSlow;
            AomSlowDecRatetoSMBFast = reader.AomSlowDecRatetoSMBFast;
            AomFastDecRatetoSMBSlow = reader.AomFastDecRatetoSMBSlow;
            AomFastDecRatetoSMBFast = reader.AomFastDecRatetoSMBFast;
            AomSlowDecCoeff = reader.AomSlowDecCoeff;
            AomFastDecCoeff = reader.AomFastDecCoeff;
            AomSlowDecCoeffStandard = reader.AomSlowDecCoeffStandard;
            AomFastDecCoeffStandard = reader.AomFastDecCoeffStandard;
            PartAOMSlowtoSMBSlow = reader.PartAOMSlowtoSMBSlow;
            PartAOMSlowtoSMBFast = reader.PartAOMSlowtoSMBFast;
            CnRatioAOMSlow = reader.CnRatioAOMSlow;
            CnRatioAOMFast = reader.CnRatioAOMFast;
            DaysAfterApplication = reader.DaysAfterApplication;
            AomDryMatterContent = reader.AomDryMatterContent;
            AomNH4Content = reader.AomNH4Content;
            AomSlowDelta = reader.AomSlowDelta;
            AomFastDelta = reader.AomFastDelta;
            Incorporation = reader.Incorporation;
            NoVolatilization = reader.NoVolatilization;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.AomSlow = AomSlow;
            writer.AomFast = AomFast;
            writer.AomSlowDecRatetoSMBSlow = AomSlowDecRatetoSMBSlow;
            writer.AomSlowDecRatetoSMBFast = AomSlowDecRatetoSMBFast;
            writer.AomFastDecRatetoSMBSlow = AomFastDecRatetoSMBSlow;
            writer.AomFastDecRatetoSMBFast = AomFastDecRatetoSMBFast;
            writer.AomSlowDecCoeff = AomSlowDecCoeff;
            writer.AomFastDecCoeff = AomFastDecCoeff;
            writer.AomSlowDecCoeffStandard = AomSlowDecCoeffStandard;
            writer.AomFastDecCoeffStandard = AomFastDecCoeffStandard;
            writer.PartAOMSlowtoSMBSlow = PartAOMSlowtoSMBSlow;
            writer.PartAOMSlowtoSMBFast = PartAOMSlowtoSMBFast;
            writer.CnRatioAOMSlow = CnRatioAOMSlow;
            writer.CnRatioAOMFast = CnRatioAOMFast;
            writer.DaysAfterApplication = DaysAfterApplication;
            writer.AomDryMatterContent = AomDryMatterContent;
            writer.AomNH4Content = AomNH4Content;
            writer.AomSlowDelta = AomSlowDelta;
            writer.AomFastDelta = AomFastDelta;
            writer.Incorporation = Incorporation;
            writer.NoVolatilization = NoVolatilization;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public double AomSlow
        {
            get;
            set;
        }

        public double AomFast
        {
            get;
            set;
        }

        public double AomSlowDecRatetoSMBSlow
        {
            get;
            set;
        }

        public double AomSlowDecRatetoSMBFast
        {
            get;
            set;
        }

        public double AomFastDecRatetoSMBSlow
        {
            get;
            set;
        }

        public double AomFastDecRatetoSMBFast
        {
            get;
            set;
        }

        public double AomSlowDecCoeff
        {
            get;
            set;
        }

        public double AomFastDecCoeff
        {
            get;
            set;
        }

        public double AomSlowDecCoeffStandard
        {
            get;
            set;
        }

        = 1;
        public double AomFastDecCoeffStandard
        {
            get;
            set;
        }

        = 1;
        public double PartAOMSlowtoSMBSlow
        {
            get;
            set;
        }

        public double PartAOMSlowtoSMBFast
        {
            get;
            set;
        }

        public double CnRatioAOMSlow
        {
            get;
            set;
        }

        = 1;
        public double CnRatioAOMFast
        {
            get;
            set;
        }

        = 1;
        public ushort DaysAfterApplication
        {
            get;
            set;
        }

        = 0;
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

        public double AomSlowDelta
        {
            get;
            set;
        }

        public double AomFastDelta
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
        public bool NoVolatilization
        {
            get;
            set;
        }

        = true;
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
            public double AomSlow => ctx.ReadDataDouble(0UL, 0);
            public double AomFast => ctx.ReadDataDouble(64UL, 0);
            public double AomSlowDecRatetoSMBSlow => ctx.ReadDataDouble(128UL, 0);
            public double AomSlowDecRatetoSMBFast => ctx.ReadDataDouble(192UL, 0);
            public double AomFastDecRatetoSMBSlow => ctx.ReadDataDouble(256UL, 0);
            public double AomFastDecRatetoSMBFast => ctx.ReadDataDouble(320UL, 0);
            public double AomSlowDecCoeff => ctx.ReadDataDouble(384UL, 0);
            public double AomFastDecCoeff => ctx.ReadDataDouble(448UL, 0);
            public double AomSlowDecCoeffStandard => ctx.ReadDataDouble(512UL, 1);
            public double AomFastDecCoeffStandard => ctx.ReadDataDouble(576UL, 1);
            public double PartAOMSlowtoSMBSlow => ctx.ReadDataDouble(640UL, 0);
            public double PartAOMSlowtoSMBFast => ctx.ReadDataDouble(704UL, 0);
            public double CnRatioAOMSlow => ctx.ReadDataDouble(768UL, 1);
            public double CnRatioAOMFast => ctx.ReadDataDouble(832UL, 1);
            public ushort DaysAfterApplication => ctx.ReadDataUShort(896UL, (ushort)0);
            public double AomDryMatterContent => ctx.ReadDataDouble(960UL, 0);
            public double AomNH4Content => ctx.ReadDataDouble(1024UL, 0);
            public double AomSlowDelta => ctx.ReadDataDouble(1088UL, 0);
            public double AomFastDelta => ctx.ReadDataDouble(1152UL, 0);
            public bool Incorporation => ctx.ReadDataBool(912UL, false);
            public bool NoVolatilization => ctx.ReadDataBool(913UL, true);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(19, 0);
            }

            public double AomSlow
            {
                get => this.ReadDataDouble(0UL, 0);
                set => this.WriteData(0UL, value, 0);
            }

            public double AomFast
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public double AomSlowDecRatetoSMBSlow
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }

            public double AomSlowDecRatetoSMBFast
            {
                get => this.ReadDataDouble(192UL, 0);
                set => this.WriteData(192UL, value, 0);
            }

            public double AomFastDecRatetoSMBSlow
            {
                get => this.ReadDataDouble(256UL, 0);
                set => this.WriteData(256UL, value, 0);
            }

            public double AomFastDecRatetoSMBFast
            {
                get => this.ReadDataDouble(320UL, 0);
                set => this.WriteData(320UL, value, 0);
            }

            public double AomSlowDecCoeff
            {
                get => this.ReadDataDouble(384UL, 0);
                set => this.WriteData(384UL, value, 0);
            }

            public double AomFastDecCoeff
            {
                get => this.ReadDataDouble(448UL, 0);
                set => this.WriteData(448UL, value, 0);
            }

            public double AomSlowDecCoeffStandard
            {
                get => this.ReadDataDouble(512UL, 1);
                set => this.WriteData(512UL, value, 1);
            }

            public double AomFastDecCoeffStandard
            {
                get => this.ReadDataDouble(576UL, 1);
                set => this.WriteData(576UL, value, 1);
            }

            public double PartAOMSlowtoSMBSlow
            {
                get => this.ReadDataDouble(640UL, 0);
                set => this.WriteData(640UL, value, 0);
            }

            public double PartAOMSlowtoSMBFast
            {
                get => this.ReadDataDouble(704UL, 0);
                set => this.WriteData(704UL, value, 0);
            }

            public double CnRatioAOMSlow
            {
                get => this.ReadDataDouble(768UL, 1);
                set => this.WriteData(768UL, value, 1);
            }

            public double CnRatioAOMFast
            {
                get => this.ReadDataDouble(832UL, 1);
                set => this.WriteData(832UL, value, 1);
            }

            public ushort DaysAfterApplication
            {
                get => this.ReadDataUShort(896UL, (ushort)0);
                set => this.WriteData(896UL, value, (ushort)0);
            }

            public double AomDryMatterContent
            {
                get => this.ReadDataDouble(960UL, 0);
                set => this.WriteData(960UL, value, 0);
            }

            public double AomNH4Content
            {
                get => this.ReadDataDouble(1024UL, 0);
                set => this.WriteData(1024UL, value, 0);
            }

            public double AomSlowDelta
            {
                get => this.ReadDataDouble(1088UL, 0);
                set => this.WriteData(1088UL, value, 0);
            }

            public double AomFastDelta
            {
                get => this.ReadDataDouble(1152UL, 0);
                set => this.WriteData(1152UL, value, 0);
            }

            public bool Incorporation
            {
                get => this.ReadDataBool(912UL, false);
                set => this.WriteData(912UL, value, false);
            }

            public bool NoVolatilization
            {
                get => this.ReadDataBool(913UL, true);
                set => this.WriteData(913UL, value, true);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xef3e4198d3e35596UL)]
    public class SoilColumnState : ICapnpSerializable
    {
        public const UInt64 typeId = 0xef3e4198d3e35596UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            VsSurfaceWaterStorage = reader.VsSurfaceWaterStorage;
            VsInterceptionStorage = reader.VsInterceptionStorage;
            VmGroundwaterTable = reader.VmGroundwaterTable;
            VsFluxAtLowerBoundary = reader.VsFluxAtLowerBoundary;
            VqCropNUptake = reader.VqCropNUptake;
            VtSoilSurfaceTemperature = reader.VtSoilSurfaceTemperature;
            VmSnowDepth = reader.VmSnowDepth;
            PsMaxMineralisationDepth = reader.PsMaxMineralisationDepth;
            VsNumberOfOrganicLayers = reader.VsNumberOfOrganicLayers;
            VfTopDressing = reader.VfTopDressing;
            VfTopDressingPartition = CapnpSerializable.Create<Mas.Schema.Management.Params.MineralFertilization.Parameters>(reader.VfTopDressingPartition);
            VfTopDressingDelay = reader.VfTopDressingDelay;
            CropModule = CapnpSerializable.Create<Mas.Schema.Model.Monica.CropModuleState>(reader.CropModule);
            DelayedNMinApplications = reader.DelayedNMinApplications?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Model.Monica.SoilColumnState.DelayedNMinApplicationParams>(_));
            PmCriticalMoistureDepth = reader.PmCriticalMoistureDepth;
            Layers = reader.Layers?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Model.Monica.SoilLayerState>(_));
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.VsSurfaceWaterStorage = VsSurfaceWaterStorage;
            writer.VsInterceptionStorage = VsInterceptionStorage;
            writer.VmGroundwaterTable = VmGroundwaterTable;
            writer.VsFluxAtLowerBoundary = VsFluxAtLowerBoundary;
            writer.VqCropNUptake = VqCropNUptake;
            writer.VtSoilSurfaceTemperature = VtSoilSurfaceTemperature;
            writer.VmSnowDepth = VmSnowDepth;
            writer.PsMaxMineralisationDepth = PsMaxMineralisationDepth;
            writer.VsNumberOfOrganicLayers = VsNumberOfOrganicLayers;
            writer.VfTopDressing = VfTopDressing;
            VfTopDressingPartition?.serialize(writer.VfTopDressingPartition);
            writer.VfTopDressingDelay = VfTopDressingDelay;
            CropModule?.serialize(writer.CropModule);
            writer.DelayedNMinApplications.Init(DelayedNMinApplications, (_s1, _v1) => _v1?.serialize(_s1));
            writer.PmCriticalMoistureDepth = PmCriticalMoistureDepth;
            writer.Layers.Init(Layers, (_s1, _v1) => _v1?.serialize(_s1));
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public double VsSurfaceWaterStorage
        {
            get;
            set;
        }

        public double VsInterceptionStorage
        {
            get;
            set;
        }

        public ushort VmGroundwaterTable
        {
            get;
            set;
        }

        public double VsFluxAtLowerBoundary
        {
            get;
            set;
        }

        public double VqCropNUptake
        {
            get;
            set;
        }

        public double VtSoilSurfaceTemperature
        {
            get;
            set;
        }

        public double VmSnowDepth
        {
            get;
            set;
        }

        public double PsMaxMineralisationDepth
        {
            get;
            set;
        }

        = 0.4;
        public double VsNumberOfOrganicLayers
        {
            get;
            set;
        }

        public double VfTopDressing
        {
            get;
            set;
        }

        public Mas.Schema.Management.Params.MineralFertilization.Parameters VfTopDressingPartition
        {
            get;
            set;
        }

        public ushort VfTopDressingDelay
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.CropModuleState CropModule
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Schema.Model.Monica.SoilColumnState.DelayedNMinApplicationParams> DelayedNMinApplications
        {
            get;
            set;
        }

        public double PmCriticalMoistureDepth
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Schema.Model.Monica.SoilLayerState> Layers
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
            public double VsSurfaceWaterStorage => ctx.ReadDataDouble(0UL, 0);
            public double VsInterceptionStorage => ctx.ReadDataDouble(64UL, 0);
            public ushort VmGroundwaterTable => ctx.ReadDataUShort(128UL, (ushort)0);
            public double VsFluxAtLowerBoundary => ctx.ReadDataDouble(192UL, 0);
            public double VqCropNUptake => ctx.ReadDataDouble(256UL, 0);
            public double VtSoilSurfaceTemperature => ctx.ReadDataDouble(320UL, 0);
            public double VmSnowDepth => ctx.ReadDataDouble(384UL, 0);
            public double PsMaxMineralisationDepth => ctx.ReadDataDouble(448UL, 0.4);
            public double VsNumberOfOrganicLayers => ctx.ReadDataDouble(512UL, 0);
            public double VfTopDressing => ctx.ReadDataDouble(576UL, 0);
            public Mas.Schema.Management.Params.MineralFertilization.Parameters.READER VfTopDressingPartition => ctx.ReadStruct(0, Mas.Schema.Management.Params.MineralFertilization.Parameters.READER.create);
            public bool HasVfTopDressingPartition => ctx.IsStructFieldNonNull(0);
            public ushort VfTopDressingDelay => ctx.ReadDataUShort(144UL, (ushort)0);
            public Mas.Schema.Model.Monica.CropModuleState.READER CropModule => ctx.ReadStruct(1, Mas.Schema.Model.Monica.CropModuleState.READER.create);
            public bool HasCropModule => ctx.IsStructFieldNonNull(1);
            public IReadOnlyList<Mas.Schema.Model.Monica.SoilColumnState.DelayedNMinApplicationParams.READER> DelayedNMinApplications => ctx.ReadList(2).Cast(Mas.Schema.Model.Monica.SoilColumnState.DelayedNMinApplicationParams.READER.create);
            public bool HasDelayedNMinApplications => ctx.IsStructFieldNonNull(2);
            public double PmCriticalMoistureDepth => ctx.ReadDataDouble(640UL, 0);
            public IReadOnlyList<Mas.Schema.Model.Monica.SoilLayerState.READER> Layers => ctx.ReadList(3).Cast(Mas.Schema.Model.Monica.SoilLayerState.READER.create);
            public bool HasLayers => ctx.IsStructFieldNonNull(3);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(11, 4);
            }

            public double VsSurfaceWaterStorage
            {
                get => this.ReadDataDouble(0UL, 0);
                set => this.WriteData(0UL, value, 0);
            }

            public double VsInterceptionStorage
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public ushort VmGroundwaterTable
            {
                get => this.ReadDataUShort(128UL, (ushort)0);
                set => this.WriteData(128UL, value, (ushort)0);
            }

            public double VsFluxAtLowerBoundary
            {
                get => this.ReadDataDouble(192UL, 0);
                set => this.WriteData(192UL, value, 0);
            }

            public double VqCropNUptake
            {
                get => this.ReadDataDouble(256UL, 0);
                set => this.WriteData(256UL, value, 0);
            }

            public double VtSoilSurfaceTemperature
            {
                get => this.ReadDataDouble(320UL, 0);
                set => this.WriteData(320UL, value, 0);
            }

            public double VmSnowDepth
            {
                get => this.ReadDataDouble(384UL, 0);
                set => this.WriteData(384UL, value, 0);
            }

            public double PsMaxMineralisationDepth
            {
                get => this.ReadDataDouble(448UL, 0.4);
                set => this.WriteData(448UL, value, 0.4);
            }

            public double VsNumberOfOrganicLayers
            {
                get => this.ReadDataDouble(512UL, 0);
                set => this.WriteData(512UL, value, 0);
            }

            public double VfTopDressing
            {
                get => this.ReadDataDouble(576UL, 0);
                set => this.WriteData(576UL, value, 0);
            }

            public Mas.Schema.Management.Params.MineralFertilization.Parameters.WRITER VfTopDressingPartition
            {
                get => BuildPointer<Mas.Schema.Management.Params.MineralFertilization.Parameters.WRITER>(0);
                set => Link(0, value);
            }

            public ushort VfTopDressingDelay
            {
                get => this.ReadDataUShort(144UL, (ushort)0);
                set => this.WriteData(144UL, value, (ushort)0);
            }

            public Mas.Schema.Model.Monica.CropModuleState.WRITER CropModule
            {
                get => BuildPointer<Mas.Schema.Model.Monica.CropModuleState.WRITER>(1);
                set => Link(1, value);
            }

            public ListOfStructsSerializer<Mas.Schema.Model.Monica.SoilColumnState.DelayedNMinApplicationParams.WRITER> DelayedNMinApplications
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Model.Monica.SoilColumnState.DelayedNMinApplicationParams.WRITER>>(2);
                set => Link(2, value);
            }

            public double PmCriticalMoistureDepth
            {
                get => this.ReadDataDouble(640UL, 0);
                set => this.WriteData(640UL, value, 0);
            }

            public ListOfStructsSerializer<Mas.Schema.Model.Monica.SoilLayerState.WRITER> Layers
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Model.Monica.SoilLayerState.WRITER>>(3);
                set => Link(3, value);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd1edcf54f4edf638UL)]
        public class DelayedNMinApplicationParams : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd1edcf54f4edf638UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Fp = CapnpSerializable.Create<Mas.Schema.Management.Params.MineralFertilization.Parameters>(reader.Fp);
                SamplingDepth = reader.SamplingDepth;
                CropNTarget = reader.CropNTarget;
                CropNTarget30 = reader.CropNTarget30;
                FertiliserMinApplication = reader.FertiliserMinApplication;
                FertiliserMaxApplication = reader.FertiliserMaxApplication;
                TopDressingDelay = reader.TopDressingDelay;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Fp?.serialize(writer.Fp);
                writer.SamplingDepth = SamplingDepth;
                writer.CropNTarget = CropNTarget;
                writer.CropNTarget30 = CropNTarget30;
                writer.FertiliserMinApplication = FertiliserMinApplication;
                writer.FertiliserMaxApplication = FertiliserMaxApplication;
                writer.TopDressingDelay = TopDressingDelay;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Schema.Management.Params.MineralFertilization.Parameters Fp
            {
                get;
                set;
            }

            public double SamplingDepth
            {
                get;
                set;
            }

            public double CropNTarget
            {
                get;
                set;
            }

            public double CropNTarget30
            {
                get;
                set;
            }

            public double FertiliserMinApplication
            {
                get;
                set;
            }

            public double FertiliserMaxApplication
            {
                get;
                set;
            }

            public double TopDressingDelay
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
                public Mas.Schema.Management.Params.MineralFertilization.Parameters.READER Fp => ctx.ReadStruct(0, Mas.Schema.Management.Params.MineralFertilization.Parameters.READER.create);
                public bool HasFp => ctx.IsStructFieldNonNull(0);
                public double SamplingDepth => ctx.ReadDataDouble(0UL, 0);
                public double CropNTarget => ctx.ReadDataDouble(64UL, 0);
                public double CropNTarget30 => ctx.ReadDataDouble(128UL, 0);
                public double FertiliserMinApplication => ctx.ReadDataDouble(192UL, 0);
                public double FertiliserMaxApplication => ctx.ReadDataDouble(256UL, 0);
                public double TopDressingDelay => ctx.ReadDataDouble(320UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(6, 1);
                }

                public Mas.Schema.Management.Params.MineralFertilization.Parameters.WRITER Fp
                {
                    get => BuildPointer<Mas.Schema.Management.Params.MineralFertilization.Parameters.WRITER>(0);
                    set => Link(0, value);
                }

                public double SamplingDepth
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public double CropNTarget
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }

                public double CropNTarget30
                {
                    get => this.ReadDataDouble(128UL, 0);
                    set => this.WriteData(128UL, value, 0);
                }

                public double FertiliserMinApplication
                {
                    get => this.ReadDataDouble(192UL, 0);
                    set => this.WriteData(192UL, value, 0);
                }

                public double FertiliserMaxApplication
                {
                    get => this.ReadDataDouble(256UL, 0);
                    set => this.WriteData(256UL, value, 0);
                }

                public double TopDressingDelay
                {
                    get => this.ReadDataDouble(320UL, 0);
                    set => this.WriteData(320UL, value, 0);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdd1e0c7c94dc4211UL)]
    public class SoilLayerState : ICapnpSerializable
    {
        public const UInt64 typeId = 0xdd1e0c7c94dc4211UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            LayerThickness = reader.LayerThickness;
            SoilWaterFlux = reader.SoilWaterFlux;
            VoAOMPool = reader.VoAOMPool?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Model.Monica.AOMProperties>(_));
            SomSlow = reader.SomSlow;
            SomFast = reader.SomFast;
            SmbSlow = reader.SmbSlow;
            SmbFast = reader.SmbFast;
            SoilCarbamid = reader.SoilCarbamid;
            SoilNH4 = reader.SoilNH4;
            SoilNO2 = reader.SoilNO2;
            SoilNO3 = reader.SoilNO3;
            SoilFrozen = reader.SoilFrozen;
            Sps = CapnpSerializable.Create<Mas.Schema.Model.Monica.SoilParameters>(reader.Sps);
            SoilMoistureM3 = reader.SoilMoistureM3;
            SoilTemperature = reader.SoilTemperature;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.LayerThickness = LayerThickness;
            writer.SoilWaterFlux = SoilWaterFlux;
            writer.VoAOMPool.Init(VoAOMPool, (_s1, _v1) => _v1?.serialize(_s1));
            writer.SomSlow = SomSlow;
            writer.SomFast = SomFast;
            writer.SmbSlow = SmbSlow;
            writer.SmbFast = SmbFast;
            writer.SoilCarbamid = SoilCarbamid;
            writer.SoilNH4 = SoilNH4;
            writer.SoilNO2 = SoilNO2;
            writer.SoilNO3 = SoilNO3;
            writer.SoilFrozen = SoilFrozen;
            Sps?.serialize(writer.Sps);
            writer.SoilMoistureM3 = SoilMoistureM3;
            writer.SoilTemperature = SoilTemperature;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public double LayerThickness
        {
            get;
            set;
        }

        = 0.1;
        public double SoilWaterFlux
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Schema.Model.Monica.AOMProperties> VoAOMPool
        {
            get;
            set;
        }

        public double SomSlow
        {
            get;
            set;
        }

        public double SomFast
        {
            get;
            set;
        }

        public double SmbSlow
        {
            get;
            set;
        }

        public double SmbFast
        {
            get;
            set;
        }

        public double SoilCarbamid
        {
            get;
            set;
        }

        public double SoilNH4
        {
            get;
            set;
        }

        = 0.0001;
        public double SoilNO2
        {
            get;
            set;
        }

        = 0.001;
        public double SoilNO3
        {
            get;
            set;
        }

        = 0.0001;
        public bool SoilFrozen
        {
            get;
            set;
        }

        = false;
        public Mas.Schema.Model.Monica.SoilParameters Sps
        {
            get;
            set;
        }

        public double SoilMoistureM3
        {
            get;
            set;
        }

        = 0.25;
        public double SoilTemperature
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
            public double LayerThickness => ctx.ReadDataDouble(0UL, 0.1);
            public double SoilWaterFlux => ctx.ReadDataDouble(64UL, 0);
            public IReadOnlyList<Mas.Schema.Model.Monica.AOMProperties.READER> VoAOMPool => ctx.ReadList(0).Cast(Mas.Schema.Model.Monica.AOMProperties.READER.create);
            public bool HasVoAOMPool => ctx.IsStructFieldNonNull(0);
            public double SomSlow => ctx.ReadDataDouble(128UL, 0);
            public double SomFast => ctx.ReadDataDouble(192UL, 0);
            public double SmbSlow => ctx.ReadDataDouble(256UL, 0);
            public double SmbFast => ctx.ReadDataDouble(320UL, 0);
            public double SoilCarbamid => ctx.ReadDataDouble(384UL, 0);
            public double SoilNH4 => ctx.ReadDataDouble(448UL, 0.0001);
            public double SoilNO2 => ctx.ReadDataDouble(512UL, 0.001);
            public double SoilNO3 => ctx.ReadDataDouble(576UL, 0.0001);
            public bool SoilFrozen => ctx.ReadDataBool(640UL, false);
            public Mas.Schema.Model.Monica.SoilParameters.READER Sps => ctx.ReadStruct(1, Mas.Schema.Model.Monica.SoilParameters.READER.create);
            public bool HasSps => ctx.IsStructFieldNonNull(1);
            public double SoilMoistureM3 => ctx.ReadDataDouble(704UL, 0.25);
            public double SoilTemperature => ctx.ReadDataDouble(768UL, 0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(13, 2);
            }

            public double LayerThickness
            {
                get => this.ReadDataDouble(0UL, 0.1);
                set => this.WriteData(0UL, value, 0.1);
            }

            public double SoilWaterFlux
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public ListOfStructsSerializer<Mas.Schema.Model.Monica.AOMProperties.WRITER> VoAOMPool
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Model.Monica.AOMProperties.WRITER>>(0);
                set => Link(0, value);
            }

            public double SomSlow
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }

            public double SomFast
            {
                get => this.ReadDataDouble(192UL, 0);
                set => this.WriteData(192UL, value, 0);
            }

            public double SmbSlow
            {
                get => this.ReadDataDouble(256UL, 0);
                set => this.WriteData(256UL, value, 0);
            }

            public double SmbFast
            {
                get => this.ReadDataDouble(320UL, 0);
                set => this.WriteData(320UL, value, 0);
            }

            public double SoilCarbamid
            {
                get => this.ReadDataDouble(384UL, 0);
                set => this.WriteData(384UL, value, 0);
            }

            public double SoilNH4
            {
                get => this.ReadDataDouble(448UL, 0.0001);
                set => this.WriteData(448UL, value, 0.0001);
            }

            public double SoilNO2
            {
                get => this.ReadDataDouble(512UL, 0.001);
                set => this.WriteData(512UL, value, 0.001);
            }

            public double SoilNO3
            {
                get => this.ReadDataDouble(576UL, 0.0001);
                set => this.WriteData(576UL, value, 0.0001);
            }

            public bool SoilFrozen
            {
                get => this.ReadDataBool(640UL, false);
                set => this.WriteData(640UL, value, false);
            }

            public Mas.Schema.Model.Monica.SoilParameters.WRITER Sps
            {
                get => BuildPointer<Mas.Schema.Model.Monica.SoilParameters.WRITER>(1);
                set => Link(1, value);
            }

            public double SoilMoistureM3
            {
                get => this.ReadDataDouble(704UL, 0.25);
                set => this.WriteData(704UL, value, 0.25);
            }

            public double SoilTemperature
            {
                get => this.ReadDataDouble(768UL, 0);
                set => this.WriteData(768UL, value, 0);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xab56969492d293b3UL)]
    public class MonicaModelState : ICapnpSerializable
    {
        public const UInt64 typeId = 0xab56969492d293b3UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            SitePs = CapnpSerializable.Create<Mas.Schema.Model.Monica.SiteParameters>(reader.SitePs);
            CultivationMethodCount = reader.CultivationMethodCount;
            EnvPs = CapnpSerializable.Create<Mas.Schema.Model.Monica.EnvironmentParameters>(reader.EnvPs);
            CropPs = CapnpSerializable.Create<Mas.Schema.Model.Monica.CropModuleParameters>(reader.CropPs);
            VsGroundwaterDepth = reader.VsGroundwaterDepth;
            VwAtmosphericO3Concentration = reader.VwAtmosphericO3Concentration;
            VwAtmosphericCO2Concentration = reader.VwAtmosphericCO2Concentration;
            SimPs = CapnpSerializable.Create<Mas.Schema.Model.Monica.SimulationParameters>(reader.SimPs);
            GroundwaterInformation = CapnpSerializable.Create<Mas.Schema.Model.Monica.MeasuredGroundwaterTableInformation>(reader.GroundwaterInformation);
            SoilColumn = CapnpSerializable.Create<Mas.Schema.Model.Monica.SoilColumnState>(reader.SoilColumn);
            SoilTemperature = CapnpSerializable.Create<Mas.Schema.Model.Monica.SoilTemperatureModuleState>(reader.SoilTemperature);
            SoilMoisture = CapnpSerializable.Create<Mas.Schema.Model.Monica.SoilMoistureModuleState>(reader.SoilMoisture);
            SoilOrganic = CapnpSerializable.Create<Mas.Schema.Model.Monica.SoilOrganicModuleState>(reader.SoilOrganic);
            SoilTransport = CapnpSerializable.Create<Mas.Schema.Model.Monica.SoilTransportModuleState>(reader.SoilTransport);
            AccuOxygenStress = reader.AccuOxygenStress;
            CurrentCropModule = CapnpSerializable.Create<Mas.Schema.Model.Monica.CropModuleState>(reader.CurrentCropModule);
            SumFertiliser = reader.SumFertiliser;
            SumOrgFertiliser = reader.SumOrgFertiliser;
            DailySumFertiliser = reader.DailySumFertiliser;
            DailySumOrgFertiliser = reader.DailySumOrgFertiliser;
            DailySumOrganicFertilizerDM = reader.DailySumOrganicFertilizerDM;
            SumOrganicFertilizerDM = reader.SumOrganicFertilizerDM;
            HumusBalanceCarryOver = reader.HumusBalanceCarryOver;
            DailySumIrrigationWater = reader.DailySumIrrigationWater;
            OptCarbonExportedResidues = reader.OptCarbonExportedResidues;
            OptCarbonReturnedResidues = reader.OptCarbonReturnedResidues;
            CurrentStepDate = CapnpSerializable.Create<Mas.Schema.Common.Date>(reader.CurrentStepDate);
            ClimateData = reader.ClimateData?.ToReadOnlyList(_2 => _2?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Model.Monica.MonicaModelState.ACDToValue>(_)));
            CurrentEvents = reader.CurrentEvents;
            PreviousDaysEvents = reader.PreviousDaysEvents;
            ClearCropUponNextDay = reader.ClearCropUponNextDay;
            DaysWithCrop = reader.DaysWithCrop;
            AccuNStress = reader.AccuNStress;
            AccuWaterStress = reader.AccuWaterStress;
            AccuHeatStress = reader.AccuHeatStress;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            SitePs?.serialize(writer.SitePs);
            writer.CultivationMethodCount = CultivationMethodCount;
            EnvPs?.serialize(writer.EnvPs);
            CropPs?.serialize(writer.CropPs);
            writer.VsGroundwaterDepth = VsGroundwaterDepth;
            writer.VwAtmosphericO3Concentration = VwAtmosphericO3Concentration;
            writer.VwAtmosphericCO2Concentration = VwAtmosphericCO2Concentration;
            SimPs?.serialize(writer.SimPs);
            GroundwaterInformation?.serialize(writer.GroundwaterInformation);
            SoilColumn?.serialize(writer.SoilColumn);
            SoilTemperature?.serialize(writer.SoilTemperature);
            SoilMoisture?.serialize(writer.SoilMoisture);
            SoilOrganic?.serialize(writer.SoilOrganic);
            SoilTransport?.serialize(writer.SoilTransport);
            writer.AccuOxygenStress = AccuOxygenStress;
            CurrentCropModule?.serialize(writer.CurrentCropModule);
            writer.SumFertiliser = SumFertiliser;
            writer.SumOrgFertiliser = SumOrgFertiliser;
            writer.DailySumFertiliser = DailySumFertiliser;
            writer.DailySumOrgFertiliser = DailySumOrgFertiliser;
            writer.DailySumOrganicFertilizerDM = DailySumOrganicFertilizerDM;
            writer.SumOrganicFertilizerDM = SumOrganicFertilizerDM;
            writer.HumusBalanceCarryOver = HumusBalanceCarryOver;
            writer.DailySumIrrigationWater = DailySumIrrigationWater;
            writer.OptCarbonExportedResidues = OptCarbonExportedResidues;
            writer.OptCarbonReturnedResidues = OptCarbonReturnedResidues;
            CurrentStepDate?.serialize(writer.CurrentStepDate);
            writer.ClimateData.Init(ClimateData, (_s2, _v2) => _s2.Init(_v2, (_s1, _v1) => _v1?.serialize(_s1)));
            writer.CurrentEvents.Init(CurrentEvents);
            writer.PreviousDaysEvents.Init(PreviousDaysEvents);
            writer.ClearCropUponNextDay = ClearCropUponNextDay;
            writer.DaysWithCrop = DaysWithCrop;
            writer.AccuNStress = AccuNStress;
            writer.AccuWaterStress = AccuWaterStress;
            writer.AccuHeatStress = AccuHeatStress;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Schema.Model.Monica.SiteParameters SitePs
        {
            get;
            set;
        }

        public ushort CultivationMethodCount
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.EnvironmentParameters EnvPs
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.CropModuleParameters CropPs
        {
            get;
            set;
        }

        public double VsGroundwaterDepth
        {
            get;
            set;
        }

        public double VwAtmosphericO3Concentration
        {
            get;
            set;
        }

        public double VwAtmosphericCO2Concentration
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.SimulationParameters SimPs
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.MeasuredGroundwaterTableInformation GroundwaterInformation
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.SoilColumnState SoilColumn
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.SoilTemperatureModuleState SoilTemperature
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.SoilMoistureModuleState SoilMoisture
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.SoilOrganicModuleState SoilOrganic
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.SoilTransportModuleState SoilTransport
        {
            get;
            set;
        }

        public double AccuOxygenStress
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.CropModuleState CurrentCropModule
        {
            get;
            set;
        }

        public double SumFertiliser
        {
            get;
            set;
        }

        public double SumOrgFertiliser
        {
            get;
            set;
        }

        public double DailySumFertiliser
        {
            get;
            set;
        }

        public double DailySumOrgFertiliser
        {
            get;
            set;
        }

        public double DailySumOrganicFertilizerDM
        {
            get;
            set;
        }

        public double SumOrganicFertilizerDM
        {
            get;
            set;
        }

        public double HumusBalanceCarryOver
        {
            get;
            set;
        }

        public double DailySumIrrigationWater
        {
            get;
            set;
        }

        public double OptCarbonExportedResidues
        {
            get;
            set;
        }

        public double OptCarbonReturnedResidues
        {
            get;
            set;
        }

        public Mas.Schema.Common.Date CurrentStepDate
        {
            get;
            set;
        }

        public IReadOnlyList<IReadOnlyList<Mas.Schema.Model.Monica.MonicaModelState.ACDToValue>> ClimateData
        {
            get;
            set;
        }

        public IReadOnlyList<string> CurrentEvents
        {
            get;
            set;
        }

        public IReadOnlyList<string> PreviousDaysEvents
        {
            get;
            set;
        }

        public bool ClearCropUponNextDay
        {
            get;
            set;
        }

        public ushort DaysWithCrop
        {
            get;
            set;
        }

        public double AccuNStress
        {
            get;
            set;
        }

        public double AccuWaterStress
        {
            get;
            set;
        }

        public double AccuHeatStress
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
            public Mas.Schema.Model.Monica.SiteParameters.READER SitePs => ctx.ReadStruct(0, Mas.Schema.Model.Monica.SiteParameters.READER.create);
            public bool HasSitePs => ctx.IsStructFieldNonNull(0);
            public ushort CultivationMethodCount => ctx.ReadDataUShort(0UL, (ushort)0);
            public Mas.Schema.Model.Monica.EnvironmentParameters.READER EnvPs => ctx.ReadStruct(1, Mas.Schema.Model.Monica.EnvironmentParameters.READER.create);
            public bool HasEnvPs => ctx.IsStructFieldNonNull(1);
            public Mas.Schema.Model.Monica.CropModuleParameters.READER CropPs => ctx.ReadStruct(2, Mas.Schema.Model.Monica.CropModuleParameters.READER.create);
            public bool HasCropPs => ctx.IsStructFieldNonNull(2);
            public double VsGroundwaterDepth => ctx.ReadDataDouble(64UL, 0);
            public double VwAtmosphericO3Concentration => ctx.ReadDataDouble(128UL, 0);
            public double VwAtmosphericCO2Concentration => ctx.ReadDataDouble(192UL, 0);
            public Mas.Schema.Model.Monica.SimulationParameters.READER SimPs => ctx.ReadStruct(3, Mas.Schema.Model.Monica.SimulationParameters.READER.create);
            public bool HasSimPs => ctx.IsStructFieldNonNull(3);
            public Mas.Schema.Model.Monica.MeasuredGroundwaterTableInformation.READER GroundwaterInformation => ctx.ReadStruct(4, Mas.Schema.Model.Monica.MeasuredGroundwaterTableInformation.READER.create);
            public bool HasGroundwaterInformation => ctx.IsStructFieldNonNull(4);
            public Mas.Schema.Model.Monica.SoilColumnState.READER SoilColumn => ctx.ReadStruct(5, Mas.Schema.Model.Monica.SoilColumnState.READER.create);
            public bool HasSoilColumn => ctx.IsStructFieldNonNull(5);
            public Mas.Schema.Model.Monica.SoilTemperatureModuleState.READER SoilTemperature => ctx.ReadStruct(6, Mas.Schema.Model.Monica.SoilTemperatureModuleState.READER.create);
            public bool HasSoilTemperature => ctx.IsStructFieldNonNull(6);
            public Mas.Schema.Model.Monica.SoilMoistureModuleState.READER SoilMoisture => ctx.ReadStruct(7, Mas.Schema.Model.Monica.SoilMoistureModuleState.READER.create);
            public bool HasSoilMoisture => ctx.IsStructFieldNonNull(7);
            public Mas.Schema.Model.Monica.SoilOrganicModuleState.READER SoilOrganic => ctx.ReadStruct(8, Mas.Schema.Model.Monica.SoilOrganicModuleState.READER.create);
            public bool HasSoilOrganic => ctx.IsStructFieldNonNull(8);
            public Mas.Schema.Model.Monica.SoilTransportModuleState.READER SoilTransport => ctx.ReadStruct(9, Mas.Schema.Model.Monica.SoilTransportModuleState.READER.create);
            public bool HasSoilTransport => ctx.IsStructFieldNonNull(9);
            public double AccuOxygenStress => ctx.ReadDataDouble(256UL, 0);
            public Mas.Schema.Model.Monica.CropModuleState.READER CurrentCropModule => ctx.ReadStruct(10, Mas.Schema.Model.Monica.CropModuleState.READER.create);
            public bool HasCurrentCropModule => ctx.IsStructFieldNonNull(10);
            public double SumFertiliser => ctx.ReadDataDouble(320UL, 0);
            public double SumOrgFertiliser => ctx.ReadDataDouble(384UL, 0);
            public double DailySumFertiliser => ctx.ReadDataDouble(448UL, 0);
            public double DailySumOrgFertiliser => ctx.ReadDataDouble(512UL, 0);
            public double DailySumOrganicFertilizerDM => ctx.ReadDataDouble(576UL, 0);
            public double SumOrganicFertilizerDM => ctx.ReadDataDouble(640UL, 0);
            public double HumusBalanceCarryOver => ctx.ReadDataDouble(704UL, 0);
            public double DailySumIrrigationWater => ctx.ReadDataDouble(768UL, 0);
            public double OptCarbonExportedResidues => ctx.ReadDataDouble(832UL, 0);
            public double OptCarbonReturnedResidues => ctx.ReadDataDouble(896UL, 0);
            public Mas.Schema.Common.Date.READER CurrentStepDate => ctx.ReadStruct(11, Mas.Schema.Common.Date.READER.create);
            public bool HasCurrentStepDate => ctx.IsStructFieldNonNull(11);
            public IReadOnlyList<IReadOnlyList<Mas.Schema.Model.Monica.MonicaModelState.ACDToValue.READER>> ClimateData => ctx.ReadList(12).Cast(_0 => _0.RequireList().Cast(Mas.Schema.Model.Monica.MonicaModelState.ACDToValue.READER.create));
            public bool HasClimateData => ctx.IsStructFieldNonNull(12);
            public IReadOnlyList<string> CurrentEvents => ctx.ReadList(13).CastText2();
            public bool HasCurrentEvents => ctx.IsStructFieldNonNull(13);
            public IReadOnlyList<string> PreviousDaysEvents => ctx.ReadList(14).CastText2();
            public bool HasPreviousDaysEvents => ctx.IsStructFieldNonNull(14);
            public bool ClearCropUponNextDay => ctx.ReadDataBool(16UL, false);
            public ushort DaysWithCrop => ctx.ReadDataUShort(32UL, (ushort)0);
            public double AccuNStress => ctx.ReadDataDouble(960UL, 0);
            public double AccuWaterStress => ctx.ReadDataDouble(1024UL, 0);
            public double AccuHeatStress => ctx.ReadDataDouble(1088UL, 0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(18, 15);
            }

            public Mas.Schema.Model.Monica.SiteParameters.WRITER SitePs
            {
                get => BuildPointer<Mas.Schema.Model.Monica.SiteParameters.WRITER>(0);
                set => Link(0, value);
            }

            public ushort CultivationMethodCount
            {
                get => this.ReadDataUShort(0UL, (ushort)0);
                set => this.WriteData(0UL, value, (ushort)0);
            }

            public Mas.Schema.Model.Monica.EnvironmentParameters.WRITER EnvPs
            {
                get => BuildPointer<Mas.Schema.Model.Monica.EnvironmentParameters.WRITER>(1);
                set => Link(1, value);
            }

            public Mas.Schema.Model.Monica.CropModuleParameters.WRITER CropPs
            {
                get => BuildPointer<Mas.Schema.Model.Monica.CropModuleParameters.WRITER>(2);
                set => Link(2, value);
            }

            public double VsGroundwaterDepth
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public double VwAtmosphericO3Concentration
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }

            public double VwAtmosphericCO2Concentration
            {
                get => this.ReadDataDouble(192UL, 0);
                set => this.WriteData(192UL, value, 0);
            }

            public Mas.Schema.Model.Monica.SimulationParameters.WRITER SimPs
            {
                get => BuildPointer<Mas.Schema.Model.Monica.SimulationParameters.WRITER>(3);
                set => Link(3, value);
            }

            public Mas.Schema.Model.Monica.MeasuredGroundwaterTableInformation.WRITER GroundwaterInformation
            {
                get => BuildPointer<Mas.Schema.Model.Monica.MeasuredGroundwaterTableInformation.WRITER>(4);
                set => Link(4, value);
            }

            public Mas.Schema.Model.Monica.SoilColumnState.WRITER SoilColumn
            {
                get => BuildPointer<Mas.Schema.Model.Monica.SoilColumnState.WRITER>(5);
                set => Link(5, value);
            }

            public Mas.Schema.Model.Monica.SoilTemperatureModuleState.WRITER SoilTemperature
            {
                get => BuildPointer<Mas.Schema.Model.Monica.SoilTemperatureModuleState.WRITER>(6);
                set => Link(6, value);
            }

            public Mas.Schema.Model.Monica.SoilMoistureModuleState.WRITER SoilMoisture
            {
                get => BuildPointer<Mas.Schema.Model.Monica.SoilMoistureModuleState.WRITER>(7);
                set => Link(7, value);
            }

            public Mas.Schema.Model.Monica.SoilOrganicModuleState.WRITER SoilOrganic
            {
                get => BuildPointer<Mas.Schema.Model.Monica.SoilOrganicModuleState.WRITER>(8);
                set => Link(8, value);
            }

            public Mas.Schema.Model.Monica.SoilTransportModuleState.WRITER SoilTransport
            {
                get => BuildPointer<Mas.Schema.Model.Monica.SoilTransportModuleState.WRITER>(9);
                set => Link(9, value);
            }

            public double AccuOxygenStress
            {
                get => this.ReadDataDouble(256UL, 0);
                set => this.WriteData(256UL, value, 0);
            }

            public Mas.Schema.Model.Monica.CropModuleState.WRITER CurrentCropModule
            {
                get => BuildPointer<Mas.Schema.Model.Monica.CropModuleState.WRITER>(10);
                set => Link(10, value);
            }

            public double SumFertiliser
            {
                get => this.ReadDataDouble(320UL, 0);
                set => this.WriteData(320UL, value, 0);
            }

            public double SumOrgFertiliser
            {
                get => this.ReadDataDouble(384UL, 0);
                set => this.WriteData(384UL, value, 0);
            }

            public double DailySumFertiliser
            {
                get => this.ReadDataDouble(448UL, 0);
                set => this.WriteData(448UL, value, 0);
            }

            public double DailySumOrgFertiliser
            {
                get => this.ReadDataDouble(512UL, 0);
                set => this.WriteData(512UL, value, 0);
            }

            public double DailySumOrganicFertilizerDM
            {
                get => this.ReadDataDouble(576UL, 0);
                set => this.WriteData(576UL, value, 0);
            }

            public double SumOrganicFertilizerDM
            {
                get => this.ReadDataDouble(640UL, 0);
                set => this.WriteData(640UL, value, 0);
            }

            public double HumusBalanceCarryOver
            {
                get => this.ReadDataDouble(704UL, 0);
                set => this.WriteData(704UL, value, 0);
            }

            public double DailySumIrrigationWater
            {
                get => this.ReadDataDouble(768UL, 0);
                set => this.WriteData(768UL, value, 0);
            }

            public double OptCarbonExportedResidues
            {
                get => this.ReadDataDouble(832UL, 0);
                set => this.WriteData(832UL, value, 0);
            }

            public double OptCarbonReturnedResidues
            {
                get => this.ReadDataDouble(896UL, 0);
                set => this.WriteData(896UL, value, 0);
            }

            public Mas.Schema.Common.Date.WRITER CurrentStepDate
            {
                get => BuildPointer<Mas.Schema.Common.Date.WRITER>(11);
                set => Link(11, value);
            }

            public ListOfPointersSerializer<ListOfStructsSerializer<Mas.Schema.Model.Monica.MonicaModelState.ACDToValue.WRITER>> ClimateData
            {
                get => BuildPointer<ListOfPointersSerializer<ListOfStructsSerializer<Mas.Schema.Model.Monica.MonicaModelState.ACDToValue.WRITER>>>(12);
                set => Link(12, value);
            }

            public ListOfTextSerializer CurrentEvents
            {
                get => BuildPointer<ListOfTextSerializer>(13);
                set => Link(13, value);
            }

            public ListOfTextSerializer PreviousDaysEvents
            {
                get => BuildPointer<ListOfTextSerializer>(14);
                set => Link(14, value);
            }

            public bool ClearCropUponNextDay
            {
                get => this.ReadDataBool(16UL, false);
                set => this.WriteData(16UL, value, false);
            }

            public ushort DaysWithCrop
            {
                get => this.ReadDataUShort(32UL, (ushort)0);
                set => this.WriteData(32UL, value, (ushort)0);
            }

            public double AccuNStress
            {
                get => this.ReadDataDouble(960UL, 0);
                set => this.WriteData(960UL, value, 0);
            }

            public double AccuWaterStress
            {
                get => this.ReadDataDouble(1024UL, 0);
                set => this.WriteData(1024UL, value, 0);
            }

            public double AccuHeatStress
            {
                get => this.ReadDataDouble(1088UL, 0);
                set => this.WriteData(1088UL, value, 0);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x98e203c76f83d365UL)]
        public class ACDToValue : ICapnpSerializable
        {
            public const UInt64 typeId = 0x98e203c76f83d365UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Acd = reader.Acd;
                Value = reader.Value;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Acd = Acd;
                writer.Value = Value;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public ushort Acd
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
                public ushort Acd => ctx.ReadDataUShort(0UL, (ushort)0);
                public double Value => ctx.ReadDataDouble(64UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 0);
                }

                public ushort Acd
                {
                    get => this.ReadDataUShort(0UL, (ushort)0);
                    set => this.WriteData(0UL, value, (ushort)0);
                }

                public double Value
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x811d54ac7debc21eUL)]
    public class CropModuleState : ICapnpSerializable
    {
        public const UInt64 typeId = 0x811d54ac7debc21eUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            FrostKillOn = reader.FrostKillOn;
            Ktko = reader.Ktko;
            Ktkc = reader.Ktkc;
            AssimilatePartCoeffsReduced = reader.AssimilatePartCoeffsReduced;
            O3WStomatalClosure = reader.O3WStomatalClosure;
            O3SumUptake = reader.O3SumUptake;
            VsLatitude = reader.VsLatitude;
            AbovegroundBiomass = reader.AbovegroundBiomass;
            AbovegroundBiomassOld = reader.AbovegroundBiomassOld;
            PcAbovegroundOrgan = reader.PcAbovegroundOrgan;
            ActualTranspiration = reader.ActualTranspiration;
            PcAssimilatePartitioningCoeff = reader.PcAssimilatePartitioningCoeff;
            PcAssimilateReallocation = reader.PcAssimilateReallocation;
            Assimilates = reader.Assimilates;
            AssimilationRate = reader.AssimilationRate;
            AstronomicDayLenght = reader.AstronomicDayLenght;
            PcBaseDaylength = reader.PcBaseDaylength;
            PcBaseTemperature = reader.PcBaseTemperature;
            PcBeginSensitivePhaseHeatStress = reader.PcBeginSensitivePhaseHeatStress;
            BelowgroundBiomass = reader.BelowgroundBiomass;
            BelowgroundBiomassOld = reader.BelowgroundBiomassOld;
            PcCarboxylationPathway = reader.PcCarboxylationPathway;
            ClearDayRadiation = reader.ClearDayRadiation;
            PcCo2Method = reader.PcCo2Method;
            CriticalNConcentration = reader.CriticalNConcentration;
            PcCriticalOxygenContent = reader.PcCriticalOxygenContent;
            PcCriticalTemperatureHeatStress = reader.PcCriticalTemperatureHeatStress;
            CropDiameter = reader.CropDiameter;
            CropFrostRedux = reader.CropFrostRedux;
            CropHeatRedux = reader.CropHeatRedux;
            CropHeight = reader.CropHeight;
            PcCropHeightP1 = reader.PcCropHeightP1;
            PcCropHeightP2 = reader.PcCropHeightP2;
            PcCropName = reader.PcCropName;
            CropNDemand = reader.CropNDemand;
            CropNRedux = reader.CropNRedux;
            PcCropSpecificMaxRootingDepth = reader.PcCropSpecificMaxRootingDepth;
            CropWaterUptake = reader.CropWaterUptake;
            CurrentTemperatureSum = reader.CurrentTemperatureSum;
            CurrentTotalTemperatureSum = reader.CurrentTotalTemperatureSum;
            CurrentTotalTemperatureSumRoot = reader.CurrentTotalTemperatureSumRoot;
            PcCuttingDelayDays = reader.PcCuttingDelayDays;
            DaylengthFactor = reader.DaylengthFactor;
            PcDaylengthRequirement = reader.PcDaylengthRequirement;
            DaysAfterBeginFlowering = reader.DaysAfterBeginFlowering;
            Declination = reader.Declination;
            PcDefaultRadiationUseEfficiency = reader.PcDefaultRadiationUseEfficiency;
            VmDepthGroundwaterTable = reader.VmDepthGroundwaterTable;
            PcDevelopmentAccelerationByNitrogenStress = reader.PcDevelopmentAccelerationByNitrogenStress;
            DevelopmentalStage = reader.DevelopmentalStage;
            NoOfCropSteps = reader.NoOfCropSteps;
            DroughtImpactOnFertility = reader.DroughtImpactOnFertility;
            PcDroughtImpactOnFertilityFactor = reader.PcDroughtImpactOnFertilityFactor;
            PcDroughtStressThreshold = reader.PcDroughtStressThreshold;
            PcEmergenceFloodingControlOn = reader.PcEmergenceFloodingControlOn;
            PcEmergenceMoistureControlOn = reader.PcEmergenceMoistureControlOn;
            PcEndSensitivePhaseHeatStress = reader.PcEndSensitivePhaseHeatStress;
            EffectiveDayLength = reader.EffectiveDayLength;
            ErrorStatus = reader.ErrorStatus;
            ErrorMessage = reader.ErrorMessage;
            EvaporatedFromIntercept = reader.EvaporatedFromIntercept;
            ExtraterrestrialRadiation = reader.ExtraterrestrialRadiation;
            PcFieldConditionModifier = reader.PcFieldConditionModifier;
            FinalDevelopmentalStage = reader.FinalDevelopmentalStage;
            FixedN = reader.FixedN;
            PcFrostDehardening = reader.PcFrostDehardening;
            PcFrostHardening = reader.PcFrostHardening;
            GlobalRadiation = reader.GlobalRadiation;
            GreenAreaIndex = reader.GreenAreaIndex;
            GrossAssimilates = reader.GrossAssimilates;
            GrossPhotosynthesis = reader.GrossPhotosynthesis;
            GrossPhotosynthesisMol = reader.GrossPhotosynthesisMol;
            GrossPhotosynthesisReferenceMol = reader.GrossPhotosynthesisReferenceMol;
            GrossPrimaryProduction = reader.GrossPrimaryProduction;
            GrowthCycleEnded = reader.GrowthCycleEnded;
            GrowthRespirationAS = reader.GrowthRespirationAS;
            PcHeatSumIrrigationStart = reader.PcHeatSumIrrigationStart;
            PcHeatSumIrrigationEnd = reader.PcHeatSumIrrigationEnd;
            VsHeightNN = reader.VsHeightNN;
            PcInitialKcFactor = reader.PcInitialKcFactor;
            PcInitialOrganBiomass = reader.PcInitialOrganBiomass;
            PcInitialRootingDepth = reader.PcInitialRootingDepth;
            InterceptionStorage = reader.InterceptionStorage;
            KcFactor = reader.KcFactor;
            LeafAreaIndex = reader.LeafAreaIndex;
            SunlitLeafAreaIndex = reader.SunlitLeafAreaIndex;
            ShadedLeafAreaIndex = reader.ShadedLeafAreaIndex;
            PcLowTemperatureExposure = reader.PcLowTemperatureExposure;
            PcLimitingTemperatureHeatStress = reader.PcLimitingTemperatureHeatStress;
            Lt50 = reader.Lt50;
            PcLt50cultivar = reader.PcLt50cultivar;
            PcLuxuryNCoeff = reader.PcLuxuryNCoeff;
            MaintenanceRespirationAS = reader.MaintenanceRespirationAS;
            PcMaxAssimilationRate = reader.PcMaxAssimilationRate;
            PcMaxCropDiameter = reader.PcMaxCropDiameter;
            PcMaxCropHeight = reader.PcMaxCropHeight;
            MaxNUptake = reader.MaxNUptake;
            PcMaxNUptakeParam = reader.PcMaxNUptakeParam;
            PcMaxRootingDepth = reader.PcMaxRootingDepth;
            PcMinimumNConcentration = reader.PcMinimumNConcentration;
            PcMinimumTemperatureForAssimilation = reader.PcMinimumTemperatureForAssimilation;
            PcOptimumTemperatureForAssimilation = reader.PcOptimumTemperatureForAssimilation;
            PcMaximumTemperatureForAssimilation = reader.PcMaximumTemperatureForAssimilation;
            PcMinimumTemperatureRootGrowth = reader.PcMinimumTemperatureRootGrowth;
            NetMaintenanceRespiration = reader.NetMaintenanceRespiration;
            NetPhotosynthesis = reader.NetPhotosynthesis;
            NetPrecipitation = reader.NetPrecipitation;
            NetPrimaryProduction = reader.NetPrimaryProduction;
            PcNConcentrationAbovegroundBiomass = reader.PcNConcentrationAbovegroundBiomass;
            NConcentrationAbovegroundBiomass = reader.NConcentrationAbovegroundBiomass;
            NConcentrationAbovegroundBiomassOld = reader.NConcentrationAbovegroundBiomassOld;
            PcNConcentrationB0 = reader.PcNConcentrationB0;
            NContentDeficit = reader.NContentDeficit;
            PcNConcentrationPN = reader.PcNConcentrationPN;
            PcNConcentrationRoot = reader.PcNConcentrationRoot;
            NConcentrationRoot = reader.NConcentrationRoot;
            NConcentrationRootOld = reader.NConcentrationRootOld;
            PcNitrogenResponseOn = reader.PcNitrogenResponseOn;
            PcNumberOfDevelopmentalStages = reader.PcNumberOfDevelopmentalStages;
            PcNumberOfOrgans = reader.PcNumberOfOrgans;
            NUptakeFromLayer = reader.NUptakeFromLayer;
            PcOptimumTemperature = reader.PcOptimumTemperature;
            OrganBiomass = reader.OrganBiomass;
            OrganDeadBiomass = reader.OrganDeadBiomass;
            OrganGreenBiomass = reader.OrganGreenBiomass;
            OrganGrowthIncrement = reader.OrganGrowthIncrement;
            PcOrganGrowthRespiration = reader.PcOrganGrowthRespiration;
            PcOrganIdsForPrimaryYield = reader.PcOrganIdsForPrimaryYield?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Model.Monica.YieldComponent>(_));
            PcOrganIdsForSecondaryYield = reader.PcOrganIdsForSecondaryYield?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Model.Monica.YieldComponent>(_));
            PcOrganIdsForCutting = reader.PcOrganIdsForCutting?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Schema.Model.Monica.YieldComponent>(_));
            PcOrganMaintenanceRespiration = reader.PcOrganMaintenanceRespiration;
            OrganSenescenceIncrement = reader.OrganSenescenceIncrement;
            PcOrganSenescenceRate = reader.PcOrganSenescenceRate;
            OvercastDayRadiation = reader.OvercastDayRadiation;
            OxygenDeficit = reader.OxygenDeficit;
            PcPartBiologicalNFixation = reader.PcPartBiologicalNFixation;
            PcPerennial = reader.PcPerennial;
            PhotoperiodicDaylength = reader.PhotoperiodicDaylength;
            PhotActRadiationMean = reader.PhotActRadiationMean;
            PcPlantDensity = reader.PcPlantDensity;
            PotentialTranspiration = reader.PotentialTranspiration;
            ReferenceEvapotranspiration = reader.ReferenceEvapotranspiration;
            RelativeTotalDevelopment = reader.RelativeTotalDevelopment;
            RemainingEvapotranspiration = reader.RemainingEvapotranspiration;
            ReserveAssimilatePool = reader.ReserveAssimilatePool;
            PcResidueNRatio = reader.PcResidueNRatio;
            PcRespiratoryStress = reader.PcRespiratoryStress;
            RootBiomass = reader.RootBiomass;
            RootBiomassOld = reader.RootBiomassOld;
            RootDensity = reader.RootDensity;
            RootDiameter = reader.RootDiameter;
            PcRootDistributionParam = reader.PcRootDistributionParam;
            RootEffectivity = reader.RootEffectivity;
            PcRootFormFactor = reader.PcRootFormFactor;
            PcRootGrowthLag = reader.PcRootGrowthLag;
            RootingDepth = reader.RootingDepth;
            RootingDepthM = reader.RootingDepthM;
            RootingZone = reader.RootingZone;
            PcRootPenetrationRate = reader.PcRootPenetrationRate;
            VmSaturationDeficit = reader.VmSaturationDeficit;
            SoilCoverage = reader.SoilCoverage;
            VsSoilMineralNContent = reader.VsSoilMineralNContent;
            SoilSpecificMaxRootingDepth = reader.SoilSpecificMaxRootingDepth;
            VsSoilSpecificMaxRootingDepth = reader.VsSoilSpecificMaxRootingDepth;
            PcSpecificLeafArea = reader.PcSpecificLeafArea;
            PcSpecificRootLength = reader.PcSpecificRootLength;
            PcStageAfterCut = reader.PcStageAfterCut;
            PcStageAtMaxDiameter = reader.PcStageAtMaxDiameter;
            PcStageAtMaxHeight = reader.PcStageAtMaxHeight;
            PcStageMaxRootNConcentration = reader.PcStageMaxRootNConcentration;
            PcStageKcFactor = reader.PcStageKcFactor;
            PcStageTemperatureSum = reader.PcStageTemperatureSum;
            StomataResistance = reader.StomataResistance;
            PcStorageOrgan = reader.PcStorageOrgan;
            StorageOrgan = reader.StorageOrgan;
            TargetNConcentration = reader.TargetNConcentration;
            TimeStep = reader.TimeStep;
            TimeUnderAnoxia = reader.TimeUnderAnoxia;
            VsTortuosity = reader.VsTortuosity;
            TotalBiomass = reader.TotalBiomass;
            TotalBiomassNContent = reader.TotalBiomassNContent;
            TotalCropHeatImpact = reader.TotalCropHeatImpact;
            TotalNInput = reader.TotalNInput;
            TotalNUptake = reader.TotalNUptake;
            TotalRespired = reader.TotalRespired;
            Respiration = reader.Respiration;
            SumTotalNUptake = reader.SumTotalNUptake;
            TotalRootLength = reader.TotalRootLength;
            TotalTemperatureSum = reader.TotalTemperatureSum;
            TemperatureSumToFlowering = reader.TemperatureSumToFlowering;
            Transpiration = reader.Transpiration;
            TranspirationRedux = reader.TranspirationRedux;
            TranspirationDeficit = reader.TranspirationDeficit;
            VernalisationDays = reader.VernalisationDays;
            VernalisationFactor = reader.VernalisationFactor;
            PcVernalisationRequirement = reader.PcVernalisationRequirement;
            PcWaterDeficitResponseOn = reader.PcWaterDeficitResponseOn;
            O3Senescence = reader.O3Senescence;
            O3LongTermDamage = reader.O3LongTermDamage;
            O3ShortTermDamage = reader.O3ShortTermDamage;
            DyingOut = reader.DyingOut;
            AccumulatedETa = reader.AccumulatedETa;
            AccumulatedTranspiration = reader.AccumulatedTranspiration;
            AccumulatedPrimaryCropYield = reader.AccumulatedPrimaryCropYield;
            SumExportedCutBiomass = reader.SumExportedCutBiomass;
            ExportedCutBiomass = reader.ExportedCutBiomass;
            SumResidueCutBiomass = reader.SumResidueCutBiomass;
            ResidueCutBiomass = reader.ResidueCutBiomass;
            CuttingDelayDays = reader.CuttingDelayDays;
            VsMaxEffectiveRootingDepth = reader.VsMaxEffectiveRootingDepth;
            VsImpenetrableLayerDept = reader.VsImpenetrableLayerDept;
            AnthesisDay = reader.AnthesisDay;
            MaturityDay = reader.MaturityDay;
            MaturityReached = reader.MaturityReached;
            StepSize24 = reader.StepSize24;
            StepSize240 = reader.StepSize240;
            Rad24 = reader.Rad24;
            Rad240 = reader.Rad240;
            Tfol24 = reader.Tfol24;
            Tfol240 = reader.Tfol240;
            Index24 = reader.Index24;
            Index240 = reader.Index240;
            Full24 = reader.Full24;
            Full240 = reader.Full240;
            GuentherEmissions = CapnpSerializable.Create<Mas.Schema.Model.Monica.Voc.Emissions>(reader.GuentherEmissions);
            JjvEmissions = CapnpSerializable.Create<Mas.Schema.Model.Monica.Voc.Emissions>(reader.JjvEmissions);
            VocSpecies = CapnpSerializable.Create<Mas.Schema.Model.Monica.Voc.SpeciesData>(reader.VocSpecies);
            CropPhotosynthesisResults = CapnpSerializable.Create<Mas.Schema.Model.Monica.Voc.CPData>(reader.CropPhotosynthesisResults);
            SpeciesParams = CapnpSerializable.Create<Mas.Schema.Model.Monica.SpeciesParameters>(reader.SpeciesParams);
            CultivarParams = CapnpSerializable.Create<Mas.Schema.Model.Monica.CultivarParameters>(reader.CultivarParams);
            ResidueParams = CapnpSerializable.Create<Mas.Schema.Model.Monica.CropResidueParameters>(reader.ResidueParams);
            IsWinterCrop = reader.IsWinterCrop;
            StemElongationEventFired = reader.StemElongationEventFired;
            Lt50m = reader.Lt50m;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.FrostKillOn = FrostKillOn;
            writer.Ktko = Ktko;
            writer.Ktkc = Ktkc;
            writer.AssimilatePartCoeffsReduced = AssimilatePartCoeffsReduced;
            writer.O3WStomatalClosure = O3WStomatalClosure;
            writer.O3SumUptake = O3SumUptake;
            writer.VsLatitude = VsLatitude;
            writer.AbovegroundBiomass = AbovegroundBiomass;
            writer.AbovegroundBiomassOld = AbovegroundBiomassOld;
            writer.PcAbovegroundOrgan.Init(PcAbovegroundOrgan);
            writer.ActualTranspiration = ActualTranspiration;
            writer.PcAssimilatePartitioningCoeff.Init(PcAssimilatePartitioningCoeff, (_s2, _v2) => _s2.Init(_v2));
            writer.PcAssimilateReallocation = PcAssimilateReallocation;
            writer.Assimilates = Assimilates;
            writer.AssimilationRate = AssimilationRate;
            writer.AstronomicDayLenght = AstronomicDayLenght;
            writer.PcBaseDaylength.Init(PcBaseDaylength);
            writer.PcBaseTemperature.Init(PcBaseTemperature);
            writer.PcBeginSensitivePhaseHeatStress = PcBeginSensitivePhaseHeatStress;
            writer.BelowgroundBiomass = BelowgroundBiomass;
            writer.BelowgroundBiomassOld = BelowgroundBiomassOld;
            writer.PcCarboxylationPathway = PcCarboxylationPathway;
            writer.ClearDayRadiation = ClearDayRadiation;
            writer.PcCo2Method = PcCo2Method;
            writer.CriticalNConcentration = CriticalNConcentration;
            writer.PcCriticalOxygenContent.Init(PcCriticalOxygenContent);
            writer.PcCriticalTemperatureHeatStress = PcCriticalTemperatureHeatStress;
            writer.CropDiameter = CropDiameter;
            writer.CropFrostRedux = CropFrostRedux;
            writer.CropHeatRedux = CropHeatRedux;
            writer.CropHeight = CropHeight;
            writer.PcCropHeightP1 = PcCropHeightP1;
            writer.PcCropHeightP2 = PcCropHeightP2;
            writer.PcCropName = PcCropName;
            writer.CropNDemand = CropNDemand;
            writer.CropNRedux = CropNRedux;
            writer.PcCropSpecificMaxRootingDepth = PcCropSpecificMaxRootingDepth;
            writer.CropWaterUptake.Init(CropWaterUptake);
            writer.CurrentTemperatureSum.Init(CurrentTemperatureSum);
            writer.CurrentTotalTemperatureSum = CurrentTotalTemperatureSum;
            writer.CurrentTotalTemperatureSumRoot = CurrentTotalTemperatureSumRoot;
            writer.PcCuttingDelayDays = PcCuttingDelayDays;
            writer.DaylengthFactor = DaylengthFactor;
            writer.PcDaylengthRequirement.Init(PcDaylengthRequirement);
            writer.DaysAfterBeginFlowering = DaysAfterBeginFlowering;
            writer.Declination = Declination;
            writer.PcDefaultRadiationUseEfficiency = PcDefaultRadiationUseEfficiency;
            writer.VmDepthGroundwaterTable = VmDepthGroundwaterTable;
            writer.PcDevelopmentAccelerationByNitrogenStress = PcDevelopmentAccelerationByNitrogenStress;
            writer.DevelopmentalStage = DevelopmentalStage;
            writer.NoOfCropSteps = NoOfCropSteps;
            writer.DroughtImpactOnFertility = DroughtImpactOnFertility;
            writer.PcDroughtImpactOnFertilityFactor = PcDroughtImpactOnFertilityFactor;
            writer.PcDroughtStressThreshold.Init(PcDroughtStressThreshold);
            writer.PcEmergenceFloodingControlOn = PcEmergenceFloodingControlOn;
            writer.PcEmergenceMoistureControlOn = PcEmergenceMoistureControlOn;
            writer.PcEndSensitivePhaseHeatStress = PcEndSensitivePhaseHeatStress;
            writer.EffectiveDayLength = EffectiveDayLength;
            writer.ErrorStatus = ErrorStatus;
            writer.ErrorMessage = ErrorMessage;
            writer.EvaporatedFromIntercept = EvaporatedFromIntercept;
            writer.ExtraterrestrialRadiation = ExtraterrestrialRadiation;
            writer.PcFieldConditionModifier = PcFieldConditionModifier;
            writer.FinalDevelopmentalStage = FinalDevelopmentalStage;
            writer.FixedN = FixedN;
            writer.PcFrostDehardening = PcFrostDehardening;
            writer.PcFrostHardening = PcFrostHardening;
            writer.GlobalRadiation = GlobalRadiation;
            writer.GreenAreaIndex = GreenAreaIndex;
            writer.GrossAssimilates = GrossAssimilates;
            writer.GrossPhotosynthesis = GrossPhotosynthesis;
            writer.GrossPhotosynthesisMol = GrossPhotosynthesisMol;
            writer.GrossPhotosynthesisReferenceMol = GrossPhotosynthesisReferenceMol;
            writer.GrossPrimaryProduction = GrossPrimaryProduction;
            writer.GrowthCycleEnded = GrowthCycleEnded;
            writer.GrowthRespirationAS = GrowthRespirationAS;
            writer.PcHeatSumIrrigationStart = PcHeatSumIrrigationStart;
            writer.PcHeatSumIrrigationEnd = PcHeatSumIrrigationEnd;
            writer.VsHeightNN = VsHeightNN;
            writer.PcInitialKcFactor = PcInitialKcFactor;
            writer.PcInitialOrganBiomass.Init(PcInitialOrganBiomass);
            writer.PcInitialRootingDepth = PcInitialRootingDepth;
            writer.InterceptionStorage = InterceptionStorage;
            writer.KcFactor = KcFactor;
            writer.LeafAreaIndex = LeafAreaIndex;
            writer.SunlitLeafAreaIndex.Init(SunlitLeafAreaIndex);
            writer.ShadedLeafAreaIndex.Init(ShadedLeafAreaIndex);
            writer.PcLowTemperatureExposure = PcLowTemperatureExposure;
            writer.PcLimitingTemperatureHeatStress = PcLimitingTemperatureHeatStress;
            writer.Lt50 = Lt50;
            writer.PcLt50cultivar = PcLt50cultivar;
            writer.PcLuxuryNCoeff = PcLuxuryNCoeff;
            writer.MaintenanceRespirationAS = MaintenanceRespirationAS;
            writer.PcMaxAssimilationRate = PcMaxAssimilationRate;
            writer.PcMaxCropDiameter = PcMaxCropDiameter;
            writer.PcMaxCropHeight = PcMaxCropHeight;
            writer.MaxNUptake = MaxNUptake;
            writer.PcMaxNUptakeParam = PcMaxNUptakeParam;
            writer.PcMaxRootingDepth = PcMaxRootingDepth;
            writer.PcMinimumNConcentration = PcMinimumNConcentration;
            writer.PcMinimumTemperatureForAssimilation = PcMinimumTemperatureForAssimilation;
            writer.PcOptimumTemperatureForAssimilation = PcOptimumTemperatureForAssimilation;
            writer.PcMaximumTemperatureForAssimilation = PcMaximumTemperatureForAssimilation;
            writer.PcMinimumTemperatureRootGrowth = PcMinimumTemperatureRootGrowth;
            writer.NetMaintenanceRespiration = NetMaintenanceRespiration;
            writer.NetPhotosynthesis = NetPhotosynthesis;
            writer.NetPrecipitation = NetPrecipitation;
            writer.NetPrimaryProduction = NetPrimaryProduction;
            writer.PcNConcentrationAbovegroundBiomass = PcNConcentrationAbovegroundBiomass;
            writer.NConcentrationAbovegroundBiomass = NConcentrationAbovegroundBiomass;
            writer.NConcentrationAbovegroundBiomassOld = NConcentrationAbovegroundBiomassOld;
            writer.PcNConcentrationB0 = PcNConcentrationB0;
            writer.NContentDeficit = NContentDeficit;
            writer.PcNConcentrationPN = PcNConcentrationPN;
            writer.PcNConcentrationRoot = PcNConcentrationRoot;
            writer.NConcentrationRoot = NConcentrationRoot;
            writer.NConcentrationRootOld = NConcentrationRootOld;
            writer.PcNitrogenResponseOn = PcNitrogenResponseOn;
            writer.PcNumberOfDevelopmentalStages = PcNumberOfDevelopmentalStages;
            writer.PcNumberOfOrgans = PcNumberOfOrgans;
            writer.NUptakeFromLayer.Init(NUptakeFromLayer);
            writer.PcOptimumTemperature.Init(PcOptimumTemperature);
            writer.OrganBiomass.Init(OrganBiomass);
            writer.OrganDeadBiomass.Init(OrganDeadBiomass);
            writer.OrganGreenBiomass.Init(OrganGreenBiomass);
            writer.OrganGrowthIncrement.Init(OrganGrowthIncrement);
            writer.PcOrganGrowthRespiration.Init(PcOrganGrowthRespiration);
            writer.PcOrganIdsForPrimaryYield.Init(PcOrganIdsForPrimaryYield, (_s1, _v1) => _v1?.serialize(_s1));
            writer.PcOrganIdsForSecondaryYield.Init(PcOrganIdsForSecondaryYield, (_s1, _v1) => _v1?.serialize(_s1));
            writer.PcOrganIdsForCutting.Init(PcOrganIdsForCutting, (_s1, _v1) => _v1?.serialize(_s1));
            writer.PcOrganMaintenanceRespiration.Init(PcOrganMaintenanceRespiration);
            writer.OrganSenescenceIncrement.Init(OrganSenescenceIncrement);
            writer.PcOrganSenescenceRate.Init(PcOrganSenescenceRate, (_s2, _v2) => _s2.Init(_v2));
            writer.OvercastDayRadiation = OvercastDayRadiation;
            writer.OxygenDeficit = OxygenDeficit;
            writer.PcPartBiologicalNFixation = PcPartBiologicalNFixation;
            writer.PcPerennial = PcPerennial;
            writer.PhotoperiodicDaylength = PhotoperiodicDaylength;
            writer.PhotActRadiationMean = PhotActRadiationMean;
            writer.PcPlantDensity = PcPlantDensity;
            writer.PotentialTranspiration = PotentialTranspiration;
            writer.ReferenceEvapotranspiration = ReferenceEvapotranspiration;
            writer.RelativeTotalDevelopment = RelativeTotalDevelopment;
            writer.RemainingEvapotranspiration = RemainingEvapotranspiration;
            writer.ReserveAssimilatePool = ReserveAssimilatePool;
            writer.PcResidueNRatio = PcResidueNRatio;
            writer.PcRespiratoryStress = PcRespiratoryStress;
            writer.RootBiomass = RootBiomass;
            writer.RootBiomassOld = RootBiomassOld;
            writer.RootDensity.Init(RootDensity);
            writer.RootDiameter.Init(RootDiameter);
            writer.PcRootDistributionParam = PcRootDistributionParam;
            writer.RootEffectivity.Init(RootEffectivity);
            writer.PcRootFormFactor = PcRootFormFactor;
            writer.PcRootGrowthLag = PcRootGrowthLag;
            writer.RootingDepth = RootingDepth;
            writer.RootingDepthM = RootingDepthM;
            writer.RootingZone = RootingZone;
            writer.PcRootPenetrationRate = PcRootPenetrationRate;
            writer.VmSaturationDeficit = VmSaturationDeficit;
            writer.SoilCoverage = SoilCoverage;
            writer.VsSoilMineralNContent.Init(VsSoilMineralNContent);
            writer.SoilSpecificMaxRootingDepth = SoilSpecificMaxRootingDepth;
            writer.VsSoilSpecificMaxRootingDepth = VsSoilSpecificMaxRootingDepth;
            writer.PcSpecificLeafArea.Init(PcSpecificLeafArea);
            writer.PcSpecificRootLength = PcSpecificRootLength;
            writer.PcStageAfterCut = PcStageAfterCut;
            writer.PcStageAtMaxDiameter = PcStageAtMaxDiameter;
            writer.PcStageAtMaxHeight = PcStageAtMaxHeight;
            writer.PcStageMaxRootNConcentration.Init(PcStageMaxRootNConcentration);
            writer.PcStageKcFactor.Init(PcStageKcFactor);
            writer.PcStageTemperatureSum.Init(PcStageTemperatureSum);
            writer.StomataResistance = StomataResistance;
            writer.PcStorageOrgan.Init(PcStorageOrgan);
            writer.StorageOrgan = StorageOrgan;
            writer.TargetNConcentration = TargetNConcentration;
            writer.TimeStep = TimeStep;
            writer.TimeUnderAnoxia = TimeUnderAnoxia;
            writer.VsTortuosity = VsTortuosity;
            writer.TotalBiomass = TotalBiomass;
            writer.TotalBiomassNContent = TotalBiomassNContent;
            writer.TotalCropHeatImpact = TotalCropHeatImpact;
            writer.TotalNInput = TotalNInput;
            writer.TotalNUptake = TotalNUptake;
            writer.TotalRespired = TotalRespired;
            writer.Respiration = Respiration;
            writer.SumTotalNUptake = SumTotalNUptake;
            writer.TotalRootLength = TotalRootLength;
            writer.TotalTemperatureSum = TotalTemperatureSum;
            writer.TemperatureSumToFlowering = TemperatureSumToFlowering;
            writer.Transpiration.Init(Transpiration);
            writer.TranspirationRedux.Init(TranspirationRedux);
            writer.TranspirationDeficit = TranspirationDeficit;
            writer.VernalisationDays = VernalisationDays;
            writer.VernalisationFactor = VernalisationFactor;
            writer.PcVernalisationRequirement.Init(PcVernalisationRequirement);
            writer.PcWaterDeficitResponseOn = PcWaterDeficitResponseOn;
            writer.O3Senescence = O3Senescence;
            writer.O3LongTermDamage = O3LongTermDamage;
            writer.O3ShortTermDamage = O3ShortTermDamage;
            writer.DyingOut = DyingOut;
            writer.AccumulatedETa = AccumulatedETa;
            writer.AccumulatedTranspiration = AccumulatedTranspiration;
            writer.AccumulatedPrimaryCropYield = AccumulatedPrimaryCropYield;
            writer.SumExportedCutBiomass = SumExportedCutBiomass;
            writer.ExportedCutBiomass = ExportedCutBiomass;
            writer.SumResidueCutBiomass = SumResidueCutBiomass;
            writer.ResidueCutBiomass = ResidueCutBiomass;
            writer.CuttingDelayDays = CuttingDelayDays;
            writer.VsMaxEffectiveRootingDepth = VsMaxEffectiveRootingDepth;
            writer.VsImpenetrableLayerDept = VsImpenetrableLayerDept;
            writer.AnthesisDay = AnthesisDay;
            writer.MaturityDay = MaturityDay;
            writer.MaturityReached = MaturityReached;
            writer.StepSize24 = StepSize24;
            writer.StepSize240 = StepSize240;
            writer.Rad24.Init(Rad24);
            writer.Rad240.Init(Rad240);
            writer.Tfol24.Init(Tfol24);
            writer.Tfol240.Init(Tfol240);
            writer.Index24 = Index24;
            writer.Index240 = Index240;
            writer.Full24 = Full24;
            writer.Full240 = Full240;
            GuentherEmissions?.serialize(writer.GuentherEmissions);
            JjvEmissions?.serialize(writer.JjvEmissions);
            VocSpecies?.serialize(writer.VocSpecies);
            CropPhotosynthesisResults?.serialize(writer.CropPhotosynthesisResults);
            SpeciesParams?.serialize(writer.SpeciesParams);
            CultivarParams?.serialize(writer.CultivarParams);
            ResidueParams?.serialize(writer.ResidueParams);
            writer.IsWinterCrop = IsWinterCrop;
            writer.StemElongationEventFired = StemElongationEventFired;
            writer.Lt50m = Lt50m;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public bool FrostKillOn
        {
            get;
            set;
        }

        public double Ktko
        {
            get;
            set;
        }

        public double Ktkc
        {
            get;
            set;
        }

        public bool AssimilatePartCoeffsReduced
        {
            get;
            set;
        }

        public double O3WStomatalClosure
        {
            get;
            set;
        }

        = 1;
        public double O3SumUptake
        {
            get;
            set;
        }

        public double VsLatitude
        {
            get;
            set;
        }

        public double AbovegroundBiomass
        {
            get;
            set;
        }

        public double AbovegroundBiomassOld
        {
            get;
            set;
        }

        public IReadOnlyList<bool> PcAbovegroundOrgan
        {
            get;
            set;
        }

        public double ActualTranspiration
        {
            get;
            set;
        }

        public IReadOnlyList<IReadOnlyList<double>> PcAssimilatePartitioningCoeff
        {
            get;
            set;
        }

        public double PcAssimilateReallocation
        {
            get;
            set;
        }

        public double Assimilates
        {
            get;
            set;
        }

        public double AssimilationRate
        {
            get;
            set;
        }

        public double AstronomicDayLenght
        {
            get;
            set;
        }

        public IReadOnlyList<double> PcBaseDaylength
        {
            get;
            set;
        }

        public IReadOnlyList<double> PcBaseTemperature
        {
            get;
            set;
        }

        public double PcBeginSensitivePhaseHeatStress
        {
            get;
            set;
        }

        public double BelowgroundBiomass
        {
            get;
            set;
        }

        public double BelowgroundBiomassOld
        {
            get;
            set;
        }

        public long PcCarboxylationPathway
        {
            get;
            set;
        }

        public double ClearDayRadiation
        {
            get;
            set;
        }

        public byte PcCo2Method
        {
            get;
            set;
        }

        = 3;
        public double CriticalNConcentration
        {
            get;
            set;
        }

        public IReadOnlyList<double> PcCriticalOxygenContent
        {
            get;
            set;
        }

        public double PcCriticalTemperatureHeatStress
        {
            get;
            set;
        }

        public double CropDiameter
        {
            get;
            set;
        }

        public double CropFrostRedux
        {
            get;
            set;
        }

        = 1;
        public double CropHeatRedux
        {
            get;
            set;
        }

        = 1;
        public double CropHeight
        {
            get;
            set;
        }

        public double PcCropHeightP1
        {
            get;
            set;
        }

        public double PcCropHeightP2
        {
            get;
            set;
        }

        public string PcCropName
        {
            get;
            set;
        }

        public double CropNDemand
        {
            get;
            set;
        }

        public double CropNRedux
        {
            get;
            set;
        }

        = 1;
        public double PcCropSpecificMaxRootingDepth
        {
            get;
            set;
        }

        public IReadOnlyList<double> CropWaterUptake
        {
            get;
            set;
        }

        public IReadOnlyList<double> CurrentTemperatureSum
        {
            get;
            set;
        }

        public double CurrentTotalTemperatureSum
        {
            get;
            set;
        }

        public double CurrentTotalTemperatureSumRoot
        {
            get;
            set;
        }

        public ushort PcCuttingDelayDays
        {
            get;
            set;
        }

        public double DaylengthFactor
        {
            get;
            set;
        }

        public IReadOnlyList<double> PcDaylengthRequirement
        {
            get;
            set;
        }

        public ushort DaysAfterBeginFlowering
        {
            get;
            set;
        }

        public double Declination
        {
            get;
            set;
        }

        public double PcDefaultRadiationUseEfficiency
        {
            get;
            set;
        }

        public ushort VmDepthGroundwaterTable
        {
            get;
            set;
        }

        public ulong PcDevelopmentAccelerationByNitrogenStress
        {
            get;
            set;
        }

        public ushort DevelopmentalStage
        {
            get;
            set;
        }

        public ushort NoOfCropSteps
        {
            get;
            set;
        }

        public double DroughtImpactOnFertility
        {
            get;
            set;
        }

        = 1;
        public double PcDroughtImpactOnFertilityFactor
        {
            get;
            set;
        }

        public IReadOnlyList<double> PcDroughtStressThreshold
        {
            get;
            set;
        }

        public bool PcEmergenceFloodingControlOn
        {
            get;
            set;
        }

        = false;
        public bool PcEmergenceMoistureControlOn
        {
            get;
            set;
        }

        = false;
        public double PcEndSensitivePhaseHeatStress
        {
            get;
            set;
        }

        public double EffectiveDayLength
        {
            get;
            set;
        }

        public bool ErrorStatus
        {
            get;
            set;
        }

        = false;
        public string ErrorMessage
        {
            get;
            set;
        }

        public double EvaporatedFromIntercept
        {
            get;
            set;
        }

        public double ExtraterrestrialRadiation
        {
            get;
            set;
        }

        public double PcFieldConditionModifier
        {
            get;
            set;
        }

        public ushort FinalDevelopmentalStage
        {
            get;
            set;
        }

        public double FixedN
        {
            get;
            set;
        }

        public double PcFrostDehardening
        {
            get;
            set;
        }

        public double PcFrostHardening
        {
            get;
            set;
        }

        public double GlobalRadiation
        {
            get;
            set;
        }

        public double GreenAreaIndex
        {
            get;
            set;
        }

        public double GrossAssimilates
        {
            get;
            set;
        }

        public double GrossPhotosynthesis
        {
            get;
            set;
        }

        public double GrossPhotosynthesisMol
        {
            get;
            set;
        }

        public double GrossPhotosynthesisReferenceMol
        {
            get;
            set;
        }

        public double GrossPrimaryProduction
        {
            get;
            set;
        }

        public bool GrowthCycleEnded
        {
            get;
            set;
        }

        = false;
        public double GrowthRespirationAS
        {
            get;
            set;
        }

        = 0;
        public double PcHeatSumIrrigationStart
        {
            get;
            set;
        }

        public double PcHeatSumIrrigationEnd
        {
            get;
            set;
        }

        public double VsHeightNN
        {
            get;
            set;
        }

        public double PcInitialKcFactor
        {
            get;
            set;
        }

        public IReadOnlyList<double> PcInitialOrganBiomass
        {
            get;
            set;
        }

        public double PcInitialRootingDepth
        {
            get;
            set;
        }

        public double InterceptionStorage
        {
            get;
            set;
        }

        public double KcFactor
        {
            get;
            set;
        }

        = 0.6;
        public double LeafAreaIndex
        {
            get;
            set;
        }

        public IReadOnlyList<double> SunlitLeafAreaIndex
        {
            get;
            set;
        }

        public IReadOnlyList<double> ShadedLeafAreaIndex
        {
            get;
            set;
        }

        public double PcLowTemperatureExposure
        {
            get;
            set;
        }

        public double PcLimitingTemperatureHeatStress
        {
            get;
            set;
        }

        public double Lt50
        {
            get;
            set;
        }

        = -3;
        public double PcLt50cultivar
        {
            get;
            set;
        }

        public double PcLuxuryNCoeff
        {
            get;
            set;
        }

        public double MaintenanceRespirationAS
        {
            get;
            set;
        }

        public double PcMaxAssimilationRate
        {
            get;
            set;
        }

        public double PcMaxCropDiameter
        {
            get;
            set;
        }

        public double PcMaxCropHeight
        {
            get;
            set;
        }

        public double MaxNUptake
        {
            get;
            set;
        }

        public double PcMaxNUptakeParam
        {
            get;
            set;
        }

        public double PcMaxRootingDepth
        {
            get;
            set;
        }

        public double PcMinimumNConcentration
        {
            get;
            set;
        }

        public double PcMinimumTemperatureForAssimilation
        {
            get;
            set;
        }

        public double PcOptimumTemperatureForAssimilation
        {
            get;
            set;
        }

        public double PcMaximumTemperatureForAssimilation
        {
            get;
            set;
        }

        public double PcMinimumTemperatureRootGrowth
        {
            get;
            set;
        }

        public double NetMaintenanceRespiration
        {
            get;
            set;
        }

        public double NetPhotosynthesis
        {
            get;
            set;
        }

        public double NetPrecipitation
        {
            get;
            set;
        }

        public double NetPrimaryProduction
        {
            get;
            set;
        }

        public double PcNConcentrationAbovegroundBiomass
        {
            get;
            set;
        }

        public double NConcentrationAbovegroundBiomass
        {
            get;
            set;
        }

        public double NConcentrationAbovegroundBiomassOld
        {
            get;
            set;
        }

        public double PcNConcentrationB0
        {
            get;
            set;
        }

        public double NContentDeficit
        {
            get;
            set;
        }

        public double PcNConcentrationPN
        {
            get;
            set;
        }

        public double PcNConcentrationRoot
        {
            get;
            set;
        }

        public double NConcentrationRoot
        {
            get;
            set;
        }

        public double NConcentrationRootOld
        {
            get;
            set;
        }

        public bool PcNitrogenResponseOn
        {
            get;
            set;
        }

        public double PcNumberOfDevelopmentalStages
        {
            get;
            set;
        }

        public double PcNumberOfOrgans
        {
            get;
            set;
        }

        public IReadOnlyList<double> NUptakeFromLayer
        {
            get;
            set;
        }

        public IReadOnlyList<double> PcOptimumTemperature
        {
            get;
            set;
        }

        public IReadOnlyList<double> OrganBiomass
        {
            get;
            set;
        }

        public IReadOnlyList<double> OrganDeadBiomass
        {
            get;
            set;
        }

        public IReadOnlyList<double> OrganGreenBiomass
        {
            get;
            set;
        }

        public IReadOnlyList<double> OrganGrowthIncrement
        {
            get;
            set;
        }

        public IReadOnlyList<double> PcOrganGrowthRespiration
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Schema.Model.Monica.YieldComponent> PcOrganIdsForPrimaryYield
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Schema.Model.Monica.YieldComponent> PcOrganIdsForSecondaryYield
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Schema.Model.Monica.YieldComponent> PcOrganIdsForCutting
        {
            get;
            set;
        }

        public IReadOnlyList<double> PcOrganMaintenanceRespiration
        {
            get;
            set;
        }

        public IReadOnlyList<double> OrganSenescenceIncrement
        {
            get;
            set;
        }

        public IReadOnlyList<IReadOnlyList<double>> PcOrganSenescenceRate
        {
            get;
            set;
        }

        public double OvercastDayRadiation
        {
            get;
            set;
        }

        public double OxygenDeficit
        {
            get;
            set;
        }

        public double PcPartBiologicalNFixation
        {
            get;
            set;
        }

        public bool PcPerennial
        {
            get;
            set;
        }

        public double PhotoperiodicDaylength
        {
            get;
            set;
        }

        public double PhotActRadiationMean
        {
            get;
            set;
        }

        public double PcPlantDensity
        {
            get;
            set;
        }

        public double PotentialTranspiration
        {
            get;
            set;
        }

        public double ReferenceEvapotranspiration
        {
            get;
            set;
        }

        public double RelativeTotalDevelopment
        {
            get;
            set;
        }

        public double RemainingEvapotranspiration
        {
            get;
            set;
        }

        public double ReserveAssimilatePool
        {
            get;
            set;
        }

        public double PcResidueNRatio
        {
            get;
            set;
        }

        public double PcRespiratoryStress
        {
            get;
            set;
        }

        public double RootBiomass
        {
            get;
            set;
        }

        public double RootBiomassOld
        {
            get;
            set;
        }

        public IReadOnlyList<double> RootDensity
        {
            get;
            set;
        }

        public IReadOnlyList<double> RootDiameter
        {
            get;
            set;
        }

        public double PcRootDistributionParam
        {
            get;
            set;
        }

        public IReadOnlyList<double> RootEffectivity
        {
            get;
            set;
        }

        public double PcRootFormFactor
        {
            get;
            set;
        }

        public double PcRootGrowthLag
        {
            get;
            set;
        }

        public ushort RootingDepth
        {
            get;
            set;
        }

        public double RootingDepthM
        {
            get;
            set;
        }

        public ushort RootingZone
        {
            get;
            set;
        }

        public double PcRootPenetrationRate
        {
            get;
            set;
        }

        public double VmSaturationDeficit
        {
            get;
            set;
        }

        public double SoilCoverage
        {
            get;
            set;
        }

        public IReadOnlyList<double> VsSoilMineralNContent
        {
            get;
            set;
        }

        public double SoilSpecificMaxRootingDepth
        {
            get;
            set;
        }

        public double VsSoilSpecificMaxRootingDepth
        {
            get;
            set;
        }

        public IReadOnlyList<double> PcSpecificLeafArea
        {
            get;
            set;
        }

        public double PcSpecificRootLength
        {
            get;
            set;
        }

        public ushort PcStageAfterCut
        {
            get;
            set;
        }

        public double PcStageAtMaxDiameter
        {
            get;
            set;
        }

        public double PcStageAtMaxHeight
        {
            get;
            set;
        }

        public IReadOnlyList<double> PcStageMaxRootNConcentration
        {
            get;
            set;
        }

        public IReadOnlyList<double> PcStageKcFactor
        {
            get;
            set;
        }

        public IReadOnlyList<double> PcStageTemperatureSum
        {
            get;
            set;
        }

        public double StomataResistance
        {
            get;
            set;
        }

        public IReadOnlyList<bool> PcStorageOrgan
        {
            get;
            set;
        }

        public ushort StorageOrgan
        {
            get;
            set;
        }

        = 4;
        public double TargetNConcentration
        {
            get;
            set;
        }

        public double TimeStep
        {
            get;
            set;
        }

        = 1;
        public ulong TimeUnderAnoxia
        {
            get;
            set;
        }

        public double VsTortuosity
        {
            get;
            set;
        }

        public double TotalBiomass
        {
            get;
            set;
        }

        public double TotalBiomassNContent
        {
            get;
            set;
        }

        public double TotalCropHeatImpact
        {
            get;
            set;
        }

        public double TotalNInput
        {
            get;
            set;
        }

        public double TotalNUptake
        {
            get;
            set;
        }

        public double TotalRespired
        {
            get;
            set;
        }

        public double Respiration
        {
            get;
            set;
        }

        public double SumTotalNUptake
        {
            get;
            set;
        }

        public double TotalRootLength
        {
            get;
            set;
        }

        public double TotalTemperatureSum
        {
            get;
            set;
        }

        public double TemperatureSumToFlowering
        {
            get;
            set;
        }

        public IReadOnlyList<double> Transpiration
        {
            get;
            set;
        }

        public IReadOnlyList<double> TranspirationRedux
        {
            get;
            set;
        }

        public double TranspirationDeficit
        {
            get;
            set;
        }

        = 1;
        public double VernalisationDays
        {
            get;
            set;
        }

        public double VernalisationFactor
        {
            get;
            set;
        }

        public IReadOnlyList<double> PcVernalisationRequirement
        {
            get;
            set;
        }

        public bool PcWaterDeficitResponseOn
        {
            get;
            set;
        }

        public double O3Senescence
        {
            get;
            set;
        }

        = 1;
        public double O3LongTermDamage
        {
            get;
            set;
        }

        = 1;
        public double O3ShortTermDamage
        {
            get;
            set;
        }

        = 1;
        public bool DyingOut
        {
            get;
            set;
        }

        public double AccumulatedETa
        {
            get;
            set;
        }

        public double AccumulatedTranspiration
        {
            get;
            set;
        }

        public double AccumulatedPrimaryCropYield
        {
            get;
            set;
        }

        public double SumExportedCutBiomass
        {
            get;
            set;
        }

        public double ExportedCutBiomass
        {
            get;
            set;
        }

        public double SumResidueCutBiomass
        {
            get;
            set;
        }

        public double ResidueCutBiomass
        {
            get;
            set;
        }

        public ushort CuttingDelayDays
        {
            get;
            set;
        }

        public double VsMaxEffectiveRootingDepth
        {
            get;
            set;
        }

        public double VsImpenetrableLayerDept
        {
            get;
            set;
        }

        public short AnthesisDay
        {
            get;
            set;
        }

        = -1;
        public short MaturityDay
        {
            get;
            set;
        }

        = -1;
        public bool MaturityReached
        {
            get;
            set;
        }

        public ushort StepSize24
        {
            get;
            set;
        }

        = 24;
        public ushort StepSize240
        {
            get;
            set;
        }

        = 240;
        public IReadOnlyList<double> Rad24
        {
            get;
            set;
        }

        public IReadOnlyList<double> Rad240
        {
            get;
            set;
        }

        public IReadOnlyList<double> Tfol24
        {
            get;
            set;
        }

        public IReadOnlyList<double> Tfol240
        {
            get;
            set;
        }

        public ushort Index24
        {
            get;
            set;
        }

        public ushort Index240
        {
            get;
            set;
        }

        public bool Full24
        {
            get;
            set;
        }

        public bool Full240
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.Voc.Emissions GuentherEmissions
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.Voc.Emissions JjvEmissions
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.Voc.SpeciesData VocSpecies
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.Voc.CPData CropPhotosynthesisResults
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.SpeciesParameters SpeciesParams
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.CultivarParameters CultivarParams
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.CropResidueParameters ResidueParams
        {
            get;
            set;
        }

        public bool IsWinterCrop
        {
            get;
            set;
        }

        public bool StemElongationEventFired
        {
            get;
            set;
        }

        public double Lt50m
        {
            get;
            set;
        }

        = -3;
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
            public bool FrostKillOn => ctx.ReadDataBool(0UL, false);
            public double Ktko => ctx.ReadDataDouble(64UL, 0);
            public double Ktkc => ctx.ReadDataDouble(128UL, 0);
            public bool AssimilatePartCoeffsReduced => ctx.ReadDataBool(1UL, false);
            public double O3WStomatalClosure => ctx.ReadDataDouble(192UL, 1);
            public double O3SumUptake => ctx.ReadDataDouble(256UL, 0);
            public double VsLatitude => ctx.ReadDataDouble(320UL, 0);
            public double AbovegroundBiomass => ctx.ReadDataDouble(384UL, 0);
            public double AbovegroundBiomassOld => ctx.ReadDataDouble(448UL, 0);
            public IReadOnlyList<bool> PcAbovegroundOrgan => ctx.ReadList(0).CastBool();
            public bool HasPcAbovegroundOrgan => ctx.IsStructFieldNonNull(0);
            public double ActualTranspiration => ctx.ReadDataDouble(512UL, 0);
            public IReadOnlyList<IReadOnlyList<double>> PcAssimilatePartitioningCoeff => ctx.ReadList(1).Cast(_0 => _0.RequireList().CastDouble());
            public bool HasPcAssimilatePartitioningCoeff => ctx.IsStructFieldNonNull(1);
            public double PcAssimilateReallocation => ctx.ReadDataDouble(576UL, 0);
            public double Assimilates => ctx.ReadDataDouble(640UL, 0);
            public double AssimilationRate => ctx.ReadDataDouble(704UL, 0);
            public double AstronomicDayLenght => ctx.ReadDataDouble(768UL, 0);
            public IReadOnlyList<double> PcBaseDaylength => ctx.ReadList(2).CastDouble();
            public bool HasPcBaseDaylength => ctx.IsStructFieldNonNull(2);
            public IReadOnlyList<double> PcBaseTemperature => ctx.ReadList(3).CastDouble();
            public bool HasPcBaseTemperature => ctx.IsStructFieldNonNull(3);
            public double PcBeginSensitivePhaseHeatStress => ctx.ReadDataDouble(832UL, 0);
            public double BelowgroundBiomass => ctx.ReadDataDouble(896UL, 0);
            public double BelowgroundBiomassOld => ctx.ReadDataDouble(960UL, 0);
            public long PcCarboxylationPathway => ctx.ReadDataLong(1024UL, 0L);
            public double ClearDayRadiation => ctx.ReadDataDouble(1088UL, 0);
            public byte PcCo2Method => ctx.ReadDataByte(8UL, (byte)3);
            public double CriticalNConcentration => ctx.ReadDataDouble(1152UL, 0);
            public IReadOnlyList<double> PcCriticalOxygenContent => ctx.ReadList(4).CastDouble();
            public bool HasPcCriticalOxygenContent => ctx.IsStructFieldNonNull(4);
            public double PcCriticalTemperatureHeatStress => ctx.ReadDataDouble(1216UL, 0);
            public double CropDiameter => ctx.ReadDataDouble(1280UL, 0);
            public double CropFrostRedux => ctx.ReadDataDouble(1344UL, 1);
            public double CropHeatRedux => ctx.ReadDataDouble(1408UL, 1);
            public double CropHeight => ctx.ReadDataDouble(1472UL, 0);
            public double PcCropHeightP1 => ctx.ReadDataDouble(1536UL, 0);
            public double PcCropHeightP2 => ctx.ReadDataDouble(1600UL, 0);
            public string PcCropName => ctx.ReadText(5, null);
            public double CropNDemand => ctx.ReadDataDouble(1664UL, 0);
            public double CropNRedux => ctx.ReadDataDouble(1728UL, 1);
            public double PcCropSpecificMaxRootingDepth => ctx.ReadDataDouble(1792UL, 0);
            public IReadOnlyList<double> CropWaterUptake => ctx.ReadList(6).CastDouble();
            public bool HasCropWaterUptake => ctx.IsStructFieldNonNull(6);
            public IReadOnlyList<double> CurrentTemperatureSum => ctx.ReadList(7).CastDouble();
            public bool HasCurrentTemperatureSum => ctx.IsStructFieldNonNull(7);
            public double CurrentTotalTemperatureSum => ctx.ReadDataDouble(1856UL, 0);
            public double CurrentTotalTemperatureSumRoot => ctx.ReadDataDouble(1920UL, 0);
            public ushort PcCuttingDelayDays => ctx.ReadDataUShort(16UL, (ushort)0);
            public double DaylengthFactor => ctx.ReadDataDouble(1984UL, 0);
            public IReadOnlyList<double> PcDaylengthRequirement => ctx.ReadList(8).CastDouble();
            public bool HasPcDaylengthRequirement => ctx.IsStructFieldNonNull(8);
            public ushort DaysAfterBeginFlowering => ctx.ReadDataUShort(32UL, (ushort)0);
            public double Declination => ctx.ReadDataDouble(2048UL, 0);
            public double PcDefaultRadiationUseEfficiency => ctx.ReadDataDouble(2112UL, 0);
            public ushort VmDepthGroundwaterTable => ctx.ReadDataUShort(48UL, (ushort)0);
            public ulong PcDevelopmentAccelerationByNitrogenStress => ctx.ReadDataULong(2176UL, 0UL);
            public ushort DevelopmentalStage => ctx.ReadDataUShort(2240UL, (ushort)0);
            public ushort NoOfCropSteps => ctx.ReadDataUShort(2256UL, (ushort)0);
            public double DroughtImpactOnFertility => ctx.ReadDataDouble(2304UL, 1);
            public double PcDroughtImpactOnFertilityFactor => ctx.ReadDataDouble(2368UL, 0);
            public IReadOnlyList<double> PcDroughtStressThreshold => ctx.ReadList(9).CastDouble();
            public bool HasPcDroughtStressThreshold => ctx.IsStructFieldNonNull(9);
            public bool PcEmergenceFloodingControlOn => ctx.ReadDataBool(2UL, false);
            public bool PcEmergenceMoistureControlOn => ctx.ReadDataBool(3UL, false);
            public double PcEndSensitivePhaseHeatStress => ctx.ReadDataDouble(2432UL, 0);
            public double EffectiveDayLength => ctx.ReadDataDouble(2496UL, 0);
            public bool ErrorStatus => ctx.ReadDataBool(4UL, false);
            public string ErrorMessage => ctx.ReadText(10, null);
            public double EvaporatedFromIntercept => ctx.ReadDataDouble(2560UL, 0);
            public double ExtraterrestrialRadiation => ctx.ReadDataDouble(2624UL, 0);
            public double PcFieldConditionModifier => ctx.ReadDataDouble(2688UL, 0);
            public ushort FinalDevelopmentalStage => ctx.ReadDataUShort(2272UL, (ushort)0);
            public double FixedN => ctx.ReadDataDouble(2752UL, 0);
            public double PcFrostDehardening => ctx.ReadDataDouble(2816UL, 0);
            public double PcFrostHardening => ctx.ReadDataDouble(2880UL, 0);
            public double GlobalRadiation => ctx.ReadDataDouble(2944UL, 0);
            public double GreenAreaIndex => ctx.ReadDataDouble(3008UL, 0);
            public double GrossAssimilates => ctx.ReadDataDouble(3072UL, 0);
            public double GrossPhotosynthesis => ctx.ReadDataDouble(3136UL, 0);
            public double GrossPhotosynthesisMol => ctx.ReadDataDouble(3200UL, 0);
            public double GrossPhotosynthesisReferenceMol => ctx.ReadDataDouble(3264UL, 0);
            public double GrossPrimaryProduction => ctx.ReadDataDouble(3328UL, 0);
            public bool GrowthCycleEnded => ctx.ReadDataBool(5UL, false);
            public double GrowthRespirationAS => ctx.ReadDataDouble(3392UL, 0);
            public double PcHeatSumIrrigationStart => ctx.ReadDataDouble(3456UL, 0);
            public double PcHeatSumIrrigationEnd => ctx.ReadDataDouble(3520UL, 0);
            public double VsHeightNN => ctx.ReadDataDouble(3584UL, 0);
            public double PcInitialKcFactor => ctx.ReadDataDouble(3648UL, 0);
            public IReadOnlyList<double> PcInitialOrganBiomass => ctx.ReadList(11).CastDouble();
            public bool HasPcInitialOrganBiomass => ctx.IsStructFieldNonNull(11);
            public double PcInitialRootingDepth => ctx.ReadDataDouble(3712UL, 0);
            public double InterceptionStorage => ctx.ReadDataDouble(3776UL, 0);
            public double KcFactor => ctx.ReadDataDouble(3840UL, 0.6);
            public double LeafAreaIndex => ctx.ReadDataDouble(3904UL, 0);
            public IReadOnlyList<double> SunlitLeafAreaIndex => ctx.ReadList(12).CastDouble();
            public bool HasSunlitLeafAreaIndex => ctx.IsStructFieldNonNull(12);
            public IReadOnlyList<double> ShadedLeafAreaIndex => ctx.ReadList(13).CastDouble();
            public bool HasShadedLeafAreaIndex => ctx.IsStructFieldNonNull(13);
            public double PcLowTemperatureExposure => ctx.ReadDataDouble(3968UL, 0);
            public double PcLimitingTemperatureHeatStress => ctx.ReadDataDouble(4032UL, 0);
            public double Lt50 => ctx.ReadDataDouble(4096UL, -3);
            public double PcLt50cultivar => ctx.ReadDataDouble(4160UL, 0);
            public double PcLuxuryNCoeff => ctx.ReadDataDouble(4224UL, 0);
            public double MaintenanceRespirationAS => ctx.ReadDataDouble(4288UL, 0);
            public double PcMaxAssimilationRate => ctx.ReadDataDouble(4352UL, 0);
            public double PcMaxCropDiameter => ctx.ReadDataDouble(4416UL, 0);
            public double PcMaxCropHeight => ctx.ReadDataDouble(4480UL, 0);
            public double MaxNUptake => ctx.ReadDataDouble(4544UL, 0);
            public double PcMaxNUptakeParam => ctx.ReadDataDouble(4608UL, 0);
            public double PcMaxRootingDepth => ctx.ReadDataDouble(4672UL, 0);
            public double PcMinimumNConcentration => ctx.ReadDataDouble(4736UL, 0);
            public double PcMinimumTemperatureForAssimilation => ctx.ReadDataDouble(4800UL, 0);
            public double PcOptimumTemperatureForAssimilation => ctx.ReadDataDouble(4864UL, 0);
            public double PcMaximumTemperatureForAssimilation => ctx.ReadDataDouble(4928UL, 0);
            public double PcMinimumTemperatureRootGrowth => ctx.ReadDataDouble(4992UL, 0);
            public double NetMaintenanceRespiration => ctx.ReadDataDouble(5056UL, 0);
            public double NetPhotosynthesis => ctx.ReadDataDouble(5120UL, 0);
            public double NetPrecipitation => ctx.ReadDataDouble(5184UL, 0);
            public double NetPrimaryProduction => ctx.ReadDataDouble(5248UL, 0);
            public double PcNConcentrationAbovegroundBiomass => ctx.ReadDataDouble(5312UL, 0);
            public double NConcentrationAbovegroundBiomass => ctx.ReadDataDouble(5376UL, 0);
            public double NConcentrationAbovegroundBiomassOld => ctx.ReadDataDouble(5440UL, 0);
            public double PcNConcentrationB0 => ctx.ReadDataDouble(5504UL, 0);
            public double NContentDeficit => ctx.ReadDataDouble(5568UL, 0);
            public double PcNConcentrationPN => ctx.ReadDataDouble(5632UL, 0);
            public double PcNConcentrationRoot => ctx.ReadDataDouble(5696UL, 0);
            public double NConcentrationRoot => ctx.ReadDataDouble(5760UL, 0);
            public double NConcentrationRootOld => ctx.ReadDataDouble(5824UL, 0);
            public bool PcNitrogenResponseOn => ctx.ReadDataBool(6UL, false);
            public double PcNumberOfDevelopmentalStages => ctx.ReadDataDouble(5888UL, 0);
            public double PcNumberOfOrgans => ctx.ReadDataDouble(5952UL, 0);
            public IReadOnlyList<double> NUptakeFromLayer => ctx.ReadList(14).CastDouble();
            public bool HasNUptakeFromLayer => ctx.IsStructFieldNonNull(14);
            public IReadOnlyList<double> PcOptimumTemperature => ctx.ReadList(15).CastDouble();
            public bool HasPcOptimumTemperature => ctx.IsStructFieldNonNull(15);
            public IReadOnlyList<double> OrganBiomass => ctx.ReadList(16).CastDouble();
            public bool HasOrganBiomass => ctx.IsStructFieldNonNull(16);
            public IReadOnlyList<double> OrganDeadBiomass => ctx.ReadList(17).CastDouble();
            public bool HasOrganDeadBiomass => ctx.IsStructFieldNonNull(17);
            public IReadOnlyList<double> OrganGreenBiomass => ctx.ReadList(18).CastDouble();
            public bool HasOrganGreenBiomass => ctx.IsStructFieldNonNull(18);
            public IReadOnlyList<double> OrganGrowthIncrement => ctx.ReadList(19).CastDouble();
            public bool HasOrganGrowthIncrement => ctx.IsStructFieldNonNull(19);
            public IReadOnlyList<double> PcOrganGrowthRespiration => ctx.ReadList(20).CastDouble();
            public bool HasPcOrganGrowthRespiration => ctx.IsStructFieldNonNull(20);
            public IReadOnlyList<Mas.Schema.Model.Monica.YieldComponent.READER> PcOrganIdsForPrimaryYield => ctx.ReadList(21).Cast(Mas.Schema.Model.Monica.YieldComponent.READER.create);
            public bool HasPcOrganIdsForPrimaryYield => ctx.IsStructFieldNonNull(21);
            public IReadOnlyList<Mas.Schema.Model.Monica.YieldComponent.READER> PcOrganIdsForSecondaryYield => ctx.ReadList(22).Cast(Mas.Schema.Model.Monica.YieldComponent.READER.create);
            public bool HasPcOrganIdsForSecondaryYield => ctx.IsStructFieldNonNull(22);
            public IReadOnlyList<Mas.Schema.Model.Monica.YieldComponent.READER> PcOrganIdsForCutting => ctx.ReadList(23).Cast(Mas.Schema.Model.Monica.YieldComponent.READER.create);
            public bool HasPcOrganIdsForCutting => ctx.IsStructFieldNonNull(23);
            public IReadOnlyList<double> PcOrganMaintenanceRespiration => ctx.ReadList(24).CastDouble();
            public bool HasPcOrganMaintenanceRespiration => ctx.IsStructFieldNonNull(24);
            public IReadOnlyList<double> OrganSenescenceIncrement => ctx.ReadList(25).CastDouble();
            public bool HasOrganSenescenceIncrement => ctx.IsStructFieldNonNull(25);
            public IReadOnlyList<IReadOnlyList<double>> PcOrganSenescenceRate => ctx.ReadList(26).Cast(_0 => _0.RequireList().CastDouble());
            public bool HasPcOrganSenescenceRate => ctx.IsStructFieldNonNull(26);
            public double OvercastDayRadiation => ctx.ReadDataDouble(6016UL, 0);
            public double OxygenDeficit => ctx.ReadDataDouble(6080UL, 0);
            public double PcPartBiologicalNFixation => ctx.ReadDataDouble(6144UL, 0);
            public bool PcPerennial => ctx.ReadDataBool(7UL, false);
            public double PhotoperiodicDaylength => ctx.ReadDataDouble(6208UL, 0);
            public double PhotActRadiationMean => ctx.ReadDataDouble(6272UL, 0);
            public double PcPlantDensity => ctx.ReadDataDouble(6336UL, 0);
            public double PotentialTranspiration => ctx.ReadDataDouble(6400UL, 0);
            public double ReferenceEvapotranspiration => ctx.ReadDataDouble(6464UL, 0);
            public double RelativeTotalDevelopment => ctx.ReadDataDouble(6528UL, 0);
            public double RemainingEvapotranspiration => ctx.ReadDataDouble(6592UL, 0);
            public double ReserveAssimilatePool => ctx.ReadDataDouble(6656UL, 0);
            public double PcResidueNRatio => ctx.ReadDataDouble(6720UL, 0);
            public double PcRespiratoryStress => ctx.ReadDataDouble(6784UL, 0);
            public double RootBiomass => ctx.ReadDataDouble(6848UL, 0);
            public double RootBiomassOld => ctx.ReadDataDouble(6912UL, 0);
            public IReadOnlyList<double> RootDensity => ctx.ReadList(27).CastDouble();
            public bool HasRootDensity => ctx.IsStructFieldNonNull(27);
            public IReadOnlyList<double> RootDiameter => ctx.ReadList(28).CastDouble();
            public bool HasRootDiameter => ctx.IsStructFieldNonNull(28);
            public double PcRootDistributionParam => ctx.ReadDataDouble(6976UL, 0);
            public IReadOnlyList<double> RootEffectivity => ctx.ReadList(29).CastDouble();
            public bool HasRootEffectivity => ctx.IsStructFieldNonNull(29);
            public double PcRootFormFactor => ctx.ReadDataDouble(7040UL, 0);
            public double PcRootGrowthLag => ctx.ReadDataDouble(7104UL, 0);
            public ushort RootingDepth => ctx.ReadDataUShort(2288UL, (ushort)0);
            public double RootingDepthM => ctx.ReadDataDouble(7168UL, 0);
            public ushort RootingZone => ctx.ReadDataUShort(7232UL, (ushort)0);
            public double PcRootPenetrationRate => ctx.ReadDataDouble(7296UL, 0);
            public double VmSaturationDeficit => ctx.ReadDataDouble(7360UL, 0);
            public double SoilCoverage => ctx.ReadDataDouble(7424UL, 0);
            public IReadOnlyList<double> VsSoilMineralNContent => ctx.ReadList(30).CastDouble();
            public bool HasVsSoilMineralNContent => ctx.IsStructFieldNonNull(30);
            public double SoilSpecificMaxRootingDepth => ctx.ReadDataDouble(7488UL, 0);
            public double VsSoilSpecificMaxRootingDepth => ctx.ReadDataDouble(7552UL, 0);
            public IReadOnlyList<double> PcSpecificLeafArea => ctx.ReadList(31).CastDouble();
            public bool HasPcSpecificLeafArea => ctx.IsStructFieldNonNull(31);
            public double PcSpecificRootLength => ctx.ReadDataDouble(7616UL, 0);
            public ushort PcStageAfterCut => ctx.ReadDataUShort(7248UL, (ushort)0);
            public double PcStageAtMaxDiameter => ctx.ReadDataDouble(7680UL, 0);
            public double PcStageAtMaxHeight => ctx.ReadDataDouble(7744UL, 0);
            public IReadOnlyList<double> PcStageMaxRootNConcentration => ctx.ReadList(32).CastDouble();
            public bool HasPcStageMaxRootNConcentration => ctx.IsStructFieldNonNull(32);
            public IReadOnlyList<double> PcStageKcFactor => ctx.ReadList(33).CastDouble();
            public bool HasPcStageKcFactor => ctx.IsStructFieldNonNull(33);
            public IReadOnlyList<double> PcStageTemperatureSum => ctx.ReadList(34).CastDouble();
            public bool HasPcStageTemperatureSum => ctx.IsStructFieldNonNull(34);
            public double StomataResistance => ctx.ReadDataDouble(7808UL, 0);
            public IReadOnlyList<bool> PcStorageOrgan => ctx.ReadList(35).CastBool();
            public bool HasPcStorageOrgan => ctx.IsStructFieldNonNull(35);
            public ushort StorageOrgan => ctx.ReadDataUShort(7264UL, (ushort)4);
            public double TargetNConcentration => ctx.ReadDataDouble(7872UL, 0);
            public double TimeStep => ctx.ReadDataDouble(7936UL, 1);
            public ulong TimeUnderAnoxia => ctx.ReadDataULong(8000UL, 0UL);
            public double VsTortuosity => ctx.ReadDataDouble(8064UL, 0);
            public double TotalBiomass => ctx.ReadDataDouble(8128UL, 0);
            public double TotalBiomassNContent => ctx.ReadDataDouble(8192UL, 0);
            public double TotalCropHeatImpact => ctx.ReadDataDouble(8256UL, 0);
            public double TotalNInput => ctx.ReadDataDouble(8320UL, 0);
            public double TotalNUptake => ctx.ReadDataDouble(8384UL, 0);
            public double TotalRespired => ctx.ReadDataDouble(8448UL, 0);
            public double Respiration => ctx.ReadDataDouble(8512UL, 0);
            public double SumTotalNUptake => ctx.ReadDataDouble(8576UL, 0);
            public double TotalRootLength => ctx.ReadDataDouble(8640UL, 0);
            public double TotalTemperatureSum => ctx.ReadDataDouble(8704UL, 0);
            public double TemperatureSumToFlowering => ctx.ReadDataDouble(8768UL, 0);
            public IReadOnlyList<double> Transpiration => ctx.ReadList(36).CastDouble();
            public bool HasTranspiration => ctx.IsStructFieldNonNull(36);
            public IReadOnlyList<double> TranspirationRedux => ctx.ReadList(37).CastDouble();
            public bool HasTranspirationRedux => ctx.IsStructFieldNonNull(37);
            public double TranspirationDeficit => ctx.ReadDataDouble(8832UL, 1);
            public double VernalisationDays => ctx.ReadDataDouble(8896UL, 0);
            public double VernalisationFactor => ctx.ReadDataDouble(8960UL, 0);
            public IReadOnlyList<double> PcVernalisationRequirement => ctx.ReadList(38).CastDouble();
            public bool HasPcVernalisationRequirement => ctx.IsStructFieldNonNull(38);
            public bool PcWaterDeficitResponseOn => ctx.ReadDataBool(7280UL, false);
            public double O3Senescence => ctx.ReadDataDouble(9024UL, 1);
            public double O3LongTermDamage => ctx.ReadDataDouble(9088UL, 1);
            public double O3ShortTermDamage => ctx.ReadDataDouble(9152UL, 1);
            public bool DyingOut => ctx.ReadDataBool(7281UL, false);
            public double AccumulatedETa => ctx.ReadDataDouble(9216UL, 0);
            public double AccumulatedTranspiration => ctx.ReadDataDouble(9280UL, 0);
            public double AccumulatedPrimaryCropYield => ctx.ReadDataDouble(9344UL, 0);
            public double SumExportedCutBiomass => ctx.ReadDataDouble(9408UL, 0);
            public double ExportedCutBiomass => ctx.ReadDataDouble(9472UL, 0);
            public double SumResidueCutBiomass => ctx.ReadDataDouble(9536UL, 0);
            public double ResidueCutBiomass => ctx.ReadDataDouble(9600UL, 0);
            public ushort CuttingDelayDays => ctx.ReadDataUShort(9664UL, (ushort)0);
            public double VsMaxEffectiveRootingDepth => ctx.ReadDataDouble(9728UL, 0);
            public double VsImpenetrableLayerDept => ctx.ReadDataDouble(9792UL, 0);
            public short AnthesisDay => ctx.ReadDataShort(9680UL, (short)-1);
            public short MaturityDay => ctx.ReadDataShort(9696UL, (short)-1);
            public bool MaturityReached => ctx.ReadDataBool(7282UL, false);
            public ushort StepSize24 => ctx.ReadDataUShort(9712UL, (ushort)24);
            public ushort StepSize240 => ctx.ReadDataUShort(9856UL, (ushort)240);
            public IReadOnlyList<double> Rad24 => ctx.ReadList(39).CastDouble();
            public bool HasRad24 => ctx.IsStructFieldNonNull(39);
            public IReadOnlyList<double> Rad240 => ctx.ReadList(40).CastDouble();
            public bool HasRad240 => ctx.IsStructFieldNonNull(40);
            public IReadOnlyList<double> Tfol24 => ctx.ReadList(41).CastDouble();
            public bool HasTfol24 => ctx.IsStructFieldNonNull(41);
            public IReadOnlyList<double> Tfol240 => ctx.ReadList(42).CastDouble();
            public bool HasTfol240 => ctx.IsStructFieldNonNull(42);
            public ushort Index24 => ctx.ReadDataUShort(9872UL, (ushort)0);
            public ushort Index240 => ctx.ReadDataUShort(9888UL, (ushort)0);
            public bool Full24 => ctx.ReadDataBool(7283UL, false);
            public bool Full240 => ctx.ReadDataBool(7284UL, false);
            public Mas.Schema.Model.Monica.Voc.Emissions.READER GuentherEmissions => ctx.ReadStruct(43, Mas.Schema.Model.Monica.Voc.Emissions.READER.create);
            public bool HasGuentherEmissions => ctx.IsStructFieldNonNull(43);
            public Mas.Schema.Model.Monica.Voc.Emissions.READER JjvEmissions => ctx.ReadStruct(44, Mas.Schema.Model.Monica.Voc.Emissions.READER.create);
            public bool HasJjvEmissions => ctx.IsStructFieldNonNull(44);
            public Mas.Schema.Model.Monica.Voc.SpeciesData.READER VocSpecies => ctx.ReadStruct(45, Mas.Schema.Model.Monica.Voc.SpeciesData.READER.create);
            public bool HasVocSpecies => ctx.IsStructFieldNonNull(45);
            public Mas.Schema.Model.Monica.Voc.CPData.READER CropPhotosynthesisResults => ctx.ReadStruct(46, Mas.Schema.Model.Monica.Voc.CPData.READER.create);
            public bool HasCropPhotosynthesisResults => ctx.IsStructFieldNonNull(46);
            public Mas.Schema.Model.Monica.SpeciesParameters.READER SpeciesParams => ctx.ReadStruct(47, Mas.Schema.Model.Monica.SpeciesParameters.READER.create);
            public bool HasSpeciesParams => ctx.IsStructFieldNonNull(47);
            public Mas.Schema.Model.Monica.CultivarParameters.READER CultivarParams => ctx.ReadStruct(48, Mas.Schema.Model.Monica.CultivarParameters.READER.create);
            public bool HasCultivarParams => ctx.IsStructFieldNonNull(48);
            public Mas.Schema.Model.Monica.CropResidueParameters.READER ResidueParams => ctx.ReadStruct(49, Mas.Schema.Model.Monica.CropResidueParameters.READER.create);
            public bool HasResidueParams => ctx.IsStructFieldNonNull(49);
            public bool IsWinterCrop => ctx.ReadDataBool(7285UL, false);
            public bool StemElongationEventFired => ctx.ReadDataBool(7286UL, false);
            public double Lt50m => ctx.ReadDataDouble(9920UL, -3);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(156, 50);
            }

            public bool FrostKillOn
            {
                get => this.ReadDataBool(0UL, false);
                set => this.WriteData(0UL, value, false);
            }

            public double Ktko
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public double Ktkc
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }

            public bool AssimilatePartCoeffsReduced
            {
                get => this.ReadDataBool(1UL, false);
                set => this.WriteData(1UL, value, false);
            }

            public double O3WStomatalClosure
            {
                get => this.ReadDataDouble(192UL, 1);
                set => this.WriteData(192UL, value, 1);
            }

            public double O3SumUptake
            {
                get => this.ReadDataDouble(256UL, 0);
                set => this.WriteData(256UL, value, 0);
            }

            public double VsLatitude
            {
                get => this.ReadDataDouble(320UL, 0);
                set => this.WriteData(320UL, value, 0);
            }

            public double AbovegroundBiomass
            {
                get => this.ReadDataDouble(384UL, 0);
                set => this.WriteData(384UL, value, 0);
            }

            public double AbovegroundBiomassOld
            {
                get => this.ReadDataDouble(448UL, 0);
                set => this.WriteData(448UL, value, 0);
            }

            public ListOfBitsSerializer PcAbovegroundOrgan
            {
                get => BuildPointer<ListOfBitsSerializer>(0);
                set => Link(0, value);
            }

            public double ActualTranspiration
            {
                get => this.ReadDataDouble(512UL, 0);
                set => this.WriteData(512UL, value, 0);
            }

            public ListOfPointersSerializer<ListOfPrimitivesSerializer<double>> PcAssimilatePartitioningCoeff
            {
                get => BuildPointer<ListOfPointersSerializer<ListOfPrimitivesSerializer<double>>>(1);
                set => Link(1, value);
            }

            public double PcAssimilateReallocation
            {
                get => this.ReadDataDouble(576UL, 0);
                set => this.WriteData(576UL, value, 0);
            }

            public double Assimilates
            {
                get => this.ReadDataDouble(640UL, 0);
                set => this.WriteData(640UL, value, 0);
            }

            public double AssimilationRate
            {
                get => this.ReadDataDouble(704UL, 0);
                set => this.WriteData(704UL, value, 0);
            }

            public double AstronomicDayLenght
            {
                get => this.ReadDataDouble(768UL, 0);
                set => this.WriteData(768UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> PcBaseDaylength
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(2);
                set => Link(2, value);
            }

            public ListOfPrimitivesSerializer<double> PcBaseTemperature
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(3);
                set => Link(3, value);
            }

            public double PcBeginSensitivePhaseHeatStress
            {
                get => this.ReadDataDouble(832UL, 0);
                set => this.WriteData(832UL, value, 0);
            }

            public double BelowgroundBiomass
            {
                get => this.ReadDataDouble(896UL, 0);
                set => this.WriteData(896UL, value, 0);
            }

            public double BelowgroundBiomassOld
            {
                get => this.ReadDataDouble(960UL, 0);
                set => this.WriteData(960UL, value, 0);
            }

            public long PcCarboxylationPathway
            {
                get => this.ReadDataLong(1024UL, 0L);
                set => this.WriteData(1024UL, value, 0L);
            }

            public double ClearDayRadiation
            {
                get => this.ReadDataDouble(1088UL, 0);
                set => this.WriteData(1088UL, value, 0);
            }

            public byte PcCo2Method
            {
                get => this.ReadDataByte(8UL, (byte)3);
                set => this.WriteData(8UL, value, (byte)3);
            }

            public double CriticalNConcentration
            {
                get => this.ReadDataDouble(1152UL, 0);
                set => this.WriteData(1152UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> PcCriticalOxygenContent
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(4);
                set => Link(4, value);
            }

            public double PcCriticalTemperatureHeatStress
            {
                get => this.ReadDataDouble(1216UL, 0);
                set => this.WriteData(1216UL, value, 0);
            }

            public double CropDiameter
            {
                get => this.ReadDataDouble(1280UL, 0);
                set => this.WriteData(1280UL, value, 0);
            }

            public double CropFrostRedux
            {
                get => this.ReadDataDouble(1344UL, 1);
                set => this.WriteData(1344UL, value, 1);
            }

            public double CropHeatRedux
            {
                get => this.ReadDataDouble(1408UL, 1);
                set => this.WriteData(1408UL, value, 1);
            }

            public double CropHeight
            {
                get => this.ReadDataDouble(1472UL, 0);
                set => this.WriteData(1472UL, value, 0);
            }

            public double PcCropHeightP1
            {
                get => this.ReadDataDouble(1536UL, 0);
                set => this.WriteData(1536UL, value, 0);
            }

            public double PcCropHeightP2
            {
                get => this.ReadDataDouble(1600UL, 0);
                set => this.WriteData(1600UL, value, 0);
            }

            public string PcCropName
            {
                get => this.ReadText(5, null);
                set => this.WriteText(5, value, null);
            }

            public double CropNDemand
            {
                get => this.ReadDataDouble(1664UL, 0);
                set => this.WriteData(1664UL, value, 0);
            }

            public double CropNRedux
            {
                get => this.ReadDataDouble(1728UL, 1);
                set => this.WriteData(1728UL, value, 1);
            }

            public double PcCropSpecificMaxRootingDepth
            {
                get => this.ReadDataDouble(1792UL, 0);
                set => this.WriteData(1792UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> CropWaterUptake
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(6);
                set => Link(6, value);
            }

            public ListOfPrimitivesSerializer<double> CurrentTemperatureSum
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(7);
                set => Link(7, value);
            }

            public double CurrentTotalTemperatureSum
            {
                get => this.ReadDataDouble(1856UL, 0);
                set => this.WriteData(1856UL, value, 0);
            }

            public double CurrentTotalTemperatureSumRoot
            {
                get => this.ReadDataDouble(1920UL, 0);
                set => this.WriteData(1920UL, value, 0);
            }

            public ushort PcCuttingDelayDays
            {
                get => this.ReadDataUShort(16UL, (ushort)0);
                set => this.WriteData(16UL, value, (ushort)0);
            }

            public double DaylengthFactor
            {
                get => this.ReadDataDouble(1984UL, 0);
                set => this.WriteData(1984UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> PcDaylengthRequirement
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(8);
                set => Link(8, value);
            }

            public ushort DaysAfterBeginFlowering
            {
                get => this.ReadDataUShort(32UL, (ushort)0);
                set => this.WriteData(32UL, value, (ushort)0);
            }

            public double Declination
            {
                get => this.ReadDataDouble(2048UL, 0);
                set => this.WriteData(2048UL, value, 0);
            }

            public double PcDefaultRadiationUseEfficiency
            {
                get => this.ReadDataDouble(2112UL, 0);
                set => this.WriteData(2112UL, value, 0);
            }

            public ushort VmDepthGroundwaterTable
            {
                get => this.ReadDataUShort(48UL, (ushort)0);
                set => this.WriteData(48UL, value, (ushort)0);
            }

            public ulong PcDevelopmentAccelerationByNitrogenStress
            {
                get => this.ReadDataULong(2176UL, 0UL);
                set => this.WriteData(2176UL, value, 0UL);
            }

            public ushort DevelopmentalStage
            {
                get => this.ReadDataUShort(2240UL, (ushort)0);
                set => this.WriteData(2240UL, value, (ushort)0);
            }

            public ushort NoOfCropSteps
            {
                get => this.ReadDataUShort(2256UL, (ushort)0);
                set => this.WriteData(2256UL, value, (ushort)0);
            }

            public double DroughtImpactOnFertility
            {
                get => this.ReadDataDouble(2304UL, 1);
                set => this.WriteData(2304UL, value, 1);
            }

            public double PcDroughtImpactOnFertilityFactor
            {
                get => this.ReadDataDouble(2368UL, 0);
                set => this.WriteData(2368UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> PcDroughtStressThreshold
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(9);
                set => Link(9, value);
            }

            public bool PcEmergenceFloodingControlOn
            {
                get => this.ReadDataBool(2UL, false);
                set => this.WriteData(2UL, value, false);
            }

            public bool PcEmergenceMoistureControlOn
            {
                get => this.ReadDataBool(3UL, false);
                set => this.WriteData(3UL, value, false);
            }

            public double PcEndSensitivePhaseHeatStress
            {
                get => this.ReadDataDouble(2432UL, 0);
                set => this.WriteData(2432UL, value, 0);
            }

            public double EffectiveDayLength
            {
                get => this.ReadDataDouble(2496UL, 0);
                set => this.WriteData(2496UL, value, 0);
            }

            public bool ErrorStatus
            {
                get => this.ReadDataBool(4UL, false);
                set => this.WriteData(4UL, value, false);
            }

            public string ErrorMessage
            {
                get => this.ReadText(10, null);
                set => this.WriteText(10, value, null);
            }

            public double EvaporatedFromIntercept
            {
                get => this.ReadDataDouble(2560UL, 0);
                set => this.WriteData(2560UL, value, 0);
            }

            public double ExtraterrestrialRadiation
            {
                get => this.ReadDataDouble(2624UL, 0);
                set => this.WriteData(2624UL, value, 0);
            }

            public double PcFieldConditionModifier
            {
                get => this.ReadDataDouble(2688UL, 0);
                set => this.WriteData(2688UL, value, 0);
            }

            public ushort FinalDevelopmentalStage
            {
                get => this.ReadDataUShort(2272UL, (ushort)0);
                set => this.WriteData(2272UL, value, (ushort)0);
            }

            public double FixedN
            {
                get => this.ReadDataDouble(2752UL, 0);
                set => this.WriteData(2752UL, value, 0);
            }

            public double PcFrostDehardening
            {
                get => this.ReadDataDouble(2816UL, 0);
                set => this.WriteData(2816UL, value, 0);
            }

            public double PcFrostHardening
            {
                get => this.ReadDataDouble(2880UL, 0);
                set => this.WriteData(2880UL, value, 0);
            }

            public double GlobalRadiation
            {
                get => this.ReadDataDouble(2944UL, 0);
                set => this.WriteData(2944UL, value, 0);
            }

            public double GreenAreaIndex
            {
                get => this.ReadDataDouble(3008UL, 0);
                set => this.WriteData(3008UL, value, 0);
            }

            public double GrossAssimilates
            {
                get => this.ReadDataDouble(3072UL, 0);
                set => this.WriteData(3072UL, value, 0);
            }

            public double GrossPhotosynthesis
            {
                get => this.ReadDataDouble(3136UL, 0);
                set => this.WriteData(3136UL, value, 0);
            }

            public double GrossPhotosynthesisMol
            {
                get => this.ReadDataDouble(3200UL, 0);
                set => this.WriteData(3200UL, value, 0);
            }

            public double GrossPhotosynthesisReferenceMol
            {
                get => this.ReadDataDouble(3264UL, 0);
                set => this.WriteData(3264UL, value, 0);
            }

            public double GrossPrimaryProduction
            {
                get => this.ReadDataDouble(3328UL, 0);
                set => this.WriteData(3328UL, value, 0);
            }

            public bool GrowthCycleEnded
            {
                get => this.ReadDataBool(5UL, false);
                set => this.WriteData(5UL, value, false);
            }

            public double GrowthRespirationAS
            {
                get => this.ReadDataDouble(3392UL, 0);
                set => this.WriteData(3392UL, value, 0);
            }

            public double PcHeatSumIrrigationStart
            {
                get => this.ReadDataDouble(3456UL, 0);
                set => this.WriteData(3456UL, value, 0);
            }

            public double PcHeatSumIrrigationEnd
            {
                get => this.ReadDataDouble(3520UL, 0);
                set => this.WriteData(3520UL, value, 0);
            }

            public double VsHeightNN
            {
                get => this.ReadDataDouble(3584UL, 0);
                set => this.WriteData(3584UL, value, 0);
            }

            public double PcInitialKcFactor
            {
                get => this.ReadDataDouble(3648UL, 0);
                set => this.WriteData(3648UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> PcInitialOrganBiomass
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(11);
                set => Link(11, value);
            }

            public double PcInitialRootingDepth
            {
                get => this.ReadDataDouble(3712UL, 0);
                set => this.WriteData(3712UL, value, 0);
            }

            public double InterceptionStorage
            {
                get => this.ReadDataDouble(3776UL, 0);
                set => this.WriteData(3776UL, value, 0);
            }

            public double KcFactor
            {
                get => this.ReadDataDouble(3840UL, 0.6);
                set => this.WriteData(3840UL, value, 0.6);
            }

            public double LeafAreaIndex
            {
                get => this.ReadDataDouble(3904UL, 0);
                set => this.WriteData(3904UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> SunlitLeafAreaIndex
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(12);
                set => Link(12, value);
            }

            public ListOfPrimitivesSerializer<double> ShadedLeafAreaIndex
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(13);
                set => Link(13, value);
            }

            public double PcLowTemperatureExposure
            {
                get => this.ReadDataDouble(3968UL, 0);
                set => this.WriteData(3968UL, value, 0);
            }

            public double PcLimitingTemperatureHeatStress
            {
                get => this.ReadDataDouble(4032UL, 0);
                set => this.WriteData(4032UL, value, 0);
            }

            public double Lt50
            {
                get => this.ReadDataDouble(4096UL, -3);
                set => this.WriteData(4096UL, value, -3);
            }

            public double PcLt50cultivar
            {
                get => this.ReadDataDouble(4160UL, 0);
                set => this.WriteData(4160UL, value, 0);
            }

            public double PcLuxuryNCoeff
            {
                get => this.ReadDataDouble(4224UL, 0);
                set => this.WriteData(4224UL, value, 0);
            }

            public double MaintenanceRespirationAS
            {
                get => this.ReadDataDouble(4288UL, 0);
                set => this.WriteData(4288UL, value, 0);
            }

            public double PcMaxAssimilationRate
            {
                get => this.ReadDataDouble(4352UL, 0);
                set => this.WriteData(4352UL, value, 0);
            }

            public double PcMaxCropDiameter
            {
                get => this.ReadDataDouble(4416UL, 0);
                set => this.WriteData(4416UL, value, 0);
            }

            public double PcMaxCropHeight
            {
                get => this.ReadDataDouble(4480UL, 0);
                set => this.WriteData(4480UL, value, 0);
            }

            public double MaxNUptake
            {
                get => this.ReadDataDouble(4544UL, 0);
                set => this.WriteData(4544UL, value, 0);
            }

            public double PcMaxNUptakeParam
            {
                get => this.ReadDataDouble(4608UL, 0);
                set => this.WriteData(4608UL, value, 0);
            }

            public double PcMaxRootingDepth
            {
                get => this.ReadDataDouble(4672UL, 0);
                set => this.WriteData(4672UL, value, 0);
            }

            public double PcMinimumNConcentration
            {
                get => this.ReadDataDouble(4736UL, 0);
                set => this.WriteData(4736UL, value, 0);
            }

            public double PcMinimumTemperatureForAssimilation
            {
                get => this.ReadDataDouble(4800UL, 0);
                set => this.WriteData(4800UL, value, 0);
            }

            public double PcOptimumTemperatureForAssimilation
            {
                get => this.ReadDataDouble(4864UL, 0);
                set => this.WriteData(4864UL, value, 0);
            }

            public double PcMaximumTemperatureForAssimilation
            {
                get => this.ReadDataDouble(4928UL, 0);
                set => this.WriteData(4928UL, value, 0);
            }

            public double PcMinimumTemperatureRootGrowth
            {
                get => this.ReadDataDouble(4992UL, 0);
                set => this.WriteData(4992UL, value, 0);
            }

            public double NetMaintenanceRespiration
            {
                get => this.ReadDataDouble(5056UL, 0);
                set => this.WriteData(5056UL, value, 0);
            }

            public double NetPhotosynthesis
            {
                get => this.ReadDataDouble(5120UL, 0);
                set => this.WriteData(5120UL, value, 0);
            }

            public double NetPrecipitation
            {
                get => this.ReadDataDouble(5184UL, 0);
                set => this.WriteData(5184UL, value, 0);
            }

            public double NetPrimaryProduction
            {
                get => this.ReadDataDouble(5248UL, 0);
                set => this.WriteData(5248UL, value, 0);
            }

            public double PcNConcentrationAbovegroundBiomass
            {
                get => this.ReadDataDouble(5312UL, 0);
                set => this.WriteData(5312UL, value, 0);
            }

            public double NConcentrationAbovegroundBiomass
            {
                get => this.ReadDataDouble(5376UL, 0);
                set => this.WriteData(5376UL, value, 0);
            }

            public double NConcentrationAbovegroundBiomassOld
            {
                get => this.ReadDataDouble(5440UL, 0);
                set => this.WriteData(5440UL, value, 0);
            }

            public double PcNConcentrationB0
            {
                get => this.ReadDataDouble(5504UL, 0);
                set => this.WriteData(5504UL, value, 0);
            }

            public double NContentDeficit
            {
                get => this.ReadDataDouble(5568UL, 0);
                set => this.WriteData(5568UL, value, 0);
            }

            public double PcNConcentrationPN
            {
                get => this.ReadDataDouble(5632UL, 0);
                set => this.WriteData(5632UL, value, 0);
            }

            public double PcNConcentrationRoot
            {
                get => this.ReadDataDouble(5696UL, 0);
                set => this.WriteData(5696UL, value, 0);
            }

            public double NConcentrationRoot
            {
                get => this.ReadDataDouble(5760UL, 0);
                set => this.WriteData(5760UL, value, 0);
            }

            public double NConcentrationRootOld
            {
                get => this.ReadDataDouble(5824UL, 0);
                set => this.WriteData(5824UL, value, 0);
            }

            public bool PcNitrogenResponseOn
            {
                get => this.ReadDataBool(6UL, false);
                set => this.WriteData(6UL, value, false);
            }

            public double PcNumberOfDevelopmentalStages
            {
                get => this.ReadDataDouble(5888UL, 0);
                set => this.WriteData(5888UL, value, 0);
            }

            public double PcNumberOfOrgans
            {
                get => this.ReadDataDouble(5952UL, 0);
                set => this.WriteData(5952UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> NUptakeFromLayer
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(14);
                set => Link(14, value);
            }

            public ListOfPrimitivesSerializer<double> PcOptimumTemperature
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(15);
                set => Link(15, value);
            }

            public ListOfPrimitivesSerializer<double> OrganBiomass
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(16);
                set => Link(16, value);
            }

            public ListOfPrimitivesSerializer<double> OrganDeadBiomass
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(17);
                set => Link(17, value);
            }

            public ListOfPrimitivesSerializer<double> OrganGreenBiomass
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(18);
                set => Link(18, value);
            }

            public ListOfPrimitivesSerializer<double> OrganGrowthIncrement
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(19);
                set => Link(19, value);
            }

            public ListOfPrimitivesSerializer<double> PcOrganGrowthRespiration
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(20);
                set => Link(20, value);
            }

            public ListOfStructsSerializer<Mas.Schema.Model.Monica.YieldComponent.WRITER> PcOrganIdsForPrimaryYield
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Model.Monica.YieldComponent.WRITER>>(21);
                set => Link(21, value);
            }

            public ListOfStructsSerializer<Mas.Schema.Model.Monica.YieldComponent.WRITER> PcOrganIdsForSecondaryYield
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Model.Monica.YieldComponent.WRITER>>(22);
                set => Link(22, value);
            }

            public ListOfStructsSerializer<Mas.Schema.Model.Monica.YieldComponent.WRITER> PcOrganIdsForCutting
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Schema.Model.Monica.YieldComponent.WRITER>>(23);
                set => Link(23, value);
            }

            public ListOfPrimitivesSerializer<double> PcOrganMaintenanceRespiration
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(24);
                set => Link(24, value);
            }

            public ListOfPrimitivesSerializer<double> OrganSenescenceIncrement
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(25);
                set => Link(25, value);
            }

            public ListOfPointersSerializer<ListOfPrimitivesSerializer<double>> PcOrganSenescenceRate
            {
                get => BuildPointer<ListOfPointersSerializer<ListOfPrimitivesSerializer<double>>>(26);
                set => Link(26, value);
            }

            public double OvercastDayRadiation
            {
                get => this.ReadDataDouble(6016UL, 0);
                set => this.WriteData(6016UL, value, 0);
            }

            public double OxygenDeficit
            {
                get => this.ReadDataDouble(6080UL, 0);
                set => this.WriteData(6080UL, value, 0);
            }

            public double PcPartBiologicalNFixation
            {
                get => this.ReadDataDouble(6144UL, 0);
                set => this.WriteData(6144UL, value, 0);
            }

            public bool PcPerennial
            {
                get => this.ReadDataBool(7UL, false);
                set => this.WriteData(7UL, value, false);
            }

            public double PhotoperiodicDaylength
            {
                get => this.ReadDataDouble(6208UL, 0);
                set => this.WriteData(6208UL, value, 0);
            }

            public double PhotActRadiationMean
            {
                get => this.ReadDataDouble(6272UL, 0);
                set => this.WriteData(6272UL, value, 0);
            }

            public double PcPlantDensity
            {
                get => this.ReadDataDouble(6336UL, 0);
                set => this.WriteData(6336UL, value, 0);
            }

            public double PotentialTranspiration
            {
                get => this.ReadDataDouble(6400UL, 0);
                set => this.WriteData(6400UL, value, 0);
            }

            public double ReferenceEvapotranspiration
            {
                get => this.ReadDataDouble(6464UL, 0);
                set => this.WriteData(6464UL, value, 0);
            }

            public double RelativeTotalDevelopment
            {
                get => this.ReadDataDouble(6528UL, 0);
                set => this.WriteData(6528UL, value, 0);
            }

            public double RemainingEvapotranspiration
            {
                get => this.ReadDataDouble(6592UL, 0);
                set => this.WriteData(6592UL, value, 0);
            }

            public double ReserveAssimilatePool
            {
                get => this.ReadDataDouble(6656UL, 0);
                set => this.WriteData(6656UL, value, 0);
            }

            public double PcResidueNRatio
            {
                get => this.ReadDataDouble(6720UL, 0);
                set => this.WriteData(6720UL, value, 0);
            }

            public double PcRespiratoryStress
            {
                get => this.ReadDataDouble(6784UL, 0);
                set => this.WriteData(6784UL, value, 0);
            }

            public double RootBiomass
            {
                get => this.ReadDataDouble(6848UL, 0);
                set => this.WriteData(6848UL, value, 0);
            }

            public double RootBiomassOld
            {
                get => this.ReadDataDouble(6912UL, 0);
                set => this.WriteData(6912UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> RootDensity
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(27);
                set => Link(27, value);
            }

            public ListOfPrimitivesSerializer<double> RootDiameter
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(28);
                set => Link(28, value);
            }

            public double PcRootDistributionParam
            {
                get => this.ReadDataDouble(6976UL, 0);
                set => this.WriteData(6976UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> RootEffectivity
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(29);
                set => Link(29, value);
            }

            public double PcRootFormFactor
            {
                get => this.ReadDataDouble(7040UL, 0);
                set => this.WriteData(7040UL, value, 0);
            }

            public double PcRootGrowthLag
            {
                get => this.ReadDataDouble(7104UL, 0);
                set => this.WriteData(7104UL, value, 0);
            }

            public ushort RootingDepth
            {
                get => this.ReadDataUShort(2288UL, (ushort)0);
                set => this.WriteData(2288UL, value, (ushort)0);
            }

            public double RootingDepthM
            {
                get => this.ReadDataDouble(7168UL, 0);
                set => this.WriteData(7168UL, value, 0);
            }

            public ushort RootingZone
            {
                get => this.ReadDataUShort(7232UL, (ushort)0);
                set => this.WriteData(7232UL, value, (ushort)0);
            }

            public double PcRootPenetrationRate
            {
                get => this.ReadDataDouble(7296UL, 0);
                set => this.WriteData(7296UL, value, 0);
            }

            public double VmSaturationDeficit
            {
                get => this.ReadDataDouble(7360UL, 0);
                set => this.WriteData(7360UL, value, 0);
            }

            public double SoilCoverage
            {
                get => this.ReadDataDouble(7424UL, 0);
                set => this.WriteData(7424UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> VsSoilMineralNContent
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(30);
                set => Link(30, value);
            }

            public double SoilSpecificMaxRootingDepth
            {
                get => this.ReadDataDouble(7488UL, 0);
                set => this.WriteData(7488UL, value, 0);
            }

            public double VsSoilSpecificMaxRootingDepth
            {
                get => this.ReadDataDouble(7552UL, 0);
                set => this.WriteData(7552UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> PcSpecificLeafArea
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(31);
                set => Link(31, value);
            }

            public double PcSpecificRootLength
            {
                get => this.ReadDataDouble(7616UL, 0);
                set => this.WriteData(7616UL, value, 0);
            }

            public ushort PcStageAfterCut
            {
                get => this.ReadDataUShort(7248UL, (ushort)0);
                set => this.WriteData(7248UL, value, (ushort)0);
            }

            public double PcStageAtMaxDiameter
            {
                get => this.ReadDataDouble(7680UL, 0);
                set => this.WriteData(7680UL, value, 0);
            }

            public double PcStageAtMaxHeight
            {
                get => this.ReadDataDouble(7744UL, 0);
                set => this.WriteData(7744UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> PcStageMaxRootNConcentration
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(32);
                set => Link(32, value);
            }

            public ListOfPrimitivesSerializer<double> PcStageKcFactor
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(33);
                set => Link(33, value);
            }

            public ListOfPrimitivesSerializer<double> PcStageTemperatureSum
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(34);
                set => Link(34, value);
            }

            public double StomataResistance
            {
                get => this.ReadDataDouble(7808UL, 0);
                set => this.WriteData(7808UL, value, 0);
            }

            public ListOfBitsSerializer PcStorageOrgan
            {
                get => BuildPointer<ListOfBitsSerializer>(35);
                set => Link(35, value);
            }

            public ushort StorageOrgan
            {
                get => this.ReadDataUShort(7264UL, (ushort)4);
                set => this.WriteData(7264UL, value, (ushort)4);
            }

            public double TargetNConcentration
            {
                get => this.ReadDataDouble(7872UL, 0);
                set => this.WriteData(7872UL, value, 0);
            }

            public double TimeStep
            {
                get => this.ReadDataDouble(7936UL, 1);
                set => this.WriteData(7936UL, value, 1);
            }

            public ulong TimeUnderAnoxia
            {
                get => this.ReadDataULong(8000UL, 0UL);
                set => this.WriteData(8000UL, value, 0UL);
            }

            public double VsTortuosity
            {
                get => this.ReadDataDouble(8064UL, 0);
                set => this.WriteData(8064UL, value, 0);
            }

            public double TotalBiomass
            {
                get => this.ReadDataDouble(8128UL, 0);
                set => this.WriteData(8128UL, value, 0);
            }

            public double TotalBiomassNContent
            {
                get => this.ReadDataDouble(8192UL, 0);
                set => this.WriteData(8192UL, value, 0);
            }

            public double TotalCropHeatImpact
            {
                get => this.ReadDataDouble(8256UL, 0);
                set => this.WriteData(8256UL, value, 0);
            }

            public double TotalNInput
            {
                get => this.ReadDataDouble(8320UL, 0);
                set => this.WriteData(8320UL, value, 0);
            }

            public double TotalNUptake
            {
                get => this.ReadDataDouble(8384UL, 0);
                set => this.WriteData(8384UL, value, 0);
            }

            public double TotalRespired
            {
                get => this.ReadDataDouble(8448UL, 0);
                set => this.WriteData(8448UL, value, 0);
            }

            public double Respiration
            {
                get => this.ReadDataDouble(8512UL, 0);
                set => this.WriteData(8512UL, value, 0);
            }

            public double SumTotalNUptake
            {
                get => this.ReadDataDouble(8576UL, 0);
                set => this.WriteData(8576UL, value, 0);
            }

            public double TotalRootLength
            {
                get => this.ReadDataDouble(8640UL, 0);
                set => this.WriteData(8640UL, value, 0);
            }

            public double TotalTemperatureSum
            {
                get => this.ReadDataDouble(8704UL, 0);
                set => this.WriteData(8704UL, value, 0);
            }

            public double TemperatureSumToFlowering
            {
                get => this.ReadDataDouble(8768UL, 0);
                set => this.WriteData(8768UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> Transpiration
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(36);
                set => Link(36, value);
            }

            public ListOfPrimitivesSerializer<double> TranspirationRedux
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(37);
                set => Link(37, value);
            }

            public double TranspirationDeficit
            {
                get => this.ReadDataDouble(8832UL, 1);
                set => this.WriteData(8832UL, value, 1);
            }

            public double VernalisationDays
            {
                get => this.ReadDataDouble(8896UL, 0);
                set => this.WriteData(8896UL, value, 0);
            }

            public double VernalisationFactor
            {
                get => this.ReadDataDouble(8960UL, 0);
                set => this.WriteData(8960UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> PcVernalisationRequirement
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(38);
                set => Link(38, value);
            }

            public bool PcWaterDeficitResponseOn
            {
                get => this.ReadDataBool(7280UL, false);
                set => this.WriteData(7280UL, value, false);
            }

            public double O3Senescence
            {
                get => this.ReadDataDouble(9024UL, 1);
                set => this.WriteData(9024UL, value, 1);
            }

            public double O3LongTermDamage
            {
                get => this.ReadDataDouble(9088UL, 1);
                set => this.WriteData(9088UL, value, 1);
            }

            public double O3ShortTermDamage
            {
                get => this.ReadDataDouble(9152UL, 1);
                set => this.WriteData(9152UL, value, 1);
            }

            public bool DyingOut
            {
                get => this.ReadDataBool(7281UL, false);
                set => this.WriteData(7281UL, value, false);
            }

            public double AccumulatedETa
            {
                get => this.ReadDataDouble(9216UL, 0);
                set => this.WriteData(9216UL, value, 0);
            }

            public double AccumulatedTranspiration
            {
                get => this.ReadDataDouble(9280UL, 0);
                set => this.WriteData(9280UL, value, 0);
            }

            public double AccumulatedPrimaryCropYield
            {
                get => this.ReadDataDouble(9344UL, 0);
                set => this.WriteData(9344UL, value, 0);
            }

            public double SumExportedCutBiomass
            {
                get => this.ReadDataDouble(9408UL, 0);
                set => this.WriteData(9408UL, value, 0);
            }

            public double ExportedCutBiomass
            {
                get => this.ReadDataDouble(9472UL, 0);
                set => this.WriteData(9472UL, value, 0);
            }

            public double SumResidueCutBiomass
            {
                get => this.ReadDataDouble(9536UL, 0);
                set => this.WriteData(9536UL, value, 0);
            }

            public double ResidueCutBiomass
            {
                get => this.ReadDataDouble(9600UL, 0);
                set => this.WriteData(9600UL, value, 0);
            }

            public ushort CuttingDelayDays
            {
                get => this.ReadDataUShort(9664UL, (ushort)0);
                set => this.WriteData(9664UL, value, (ushort)0);
            }

            public double VsMaxEffectiveRootingDepth
            {
                get => this.ReadDataDouble(9728UL, 0);
                set => this.WriteData(9728UL, value, 0);
            }

            public double VsImpenetrableLayerDept
            {
                get => this.ReadDataDouble(9792UL, 0);
                set => this.WriteData(9792UL, value, 0);
            }

            public short AnthesisDay
            {
                get => this.ReadDataShort(9680UL, (short)-1);
                set => this.WriteData(9680UL, value, (short)-1);
            }

            public short MaturityDay
            {
                get => this.ReadDataShort(9696UL, (short)-1);
                set => this.WriteData(9696UL, value, (short)-1);
            }

            public bool MaturityReached
            {
                get => this.ReadDataBool(7282UL, false);
                set => this.WriteData(7282UL, value, false);
            }

            public ushort StepSize24
            {
                get => this.ReadDataUShort(9712UL, (ushort)24);
                set => this.WriteData(9712UL, value, (ushort)24);
            }

            public ushort StepSize240
            {
                get => this.ReadDataUShort(9856UL, (ushort)240);
                set => this.WriteData(9856UL, value, (ushort)240);
            }

            public ListOfPrimitivesSerializer<double> Rad24
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(39);
                set => Link(39, value);
            }

            public ListOfPrimitivesSerializer<double> Rad240
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(40);
                set => Link(40, value);
            }

            public ListOfPrimitivesSerializer<double> Tfol24
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(41);
                set => Link(41, value);
            }

            public ListOfPrimitivesSerializer<double> Tfol240
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(42);
                set => Link(42, value);
            }

            public ushort Index24
            {
                get => this.ReadDataUShort(9872UL, (ushort)0);
                set => this.WriteData(9872UL, value, (ushort)0);
            }

            public ushort Index240
            {
                get => this.ReadDataUShort(9888UL, (ushort)0);
                set => this.WriteData(9888UL, value, (ushort)0);
            }

            public bool Full24
            {
                get => this.ReadDataBool(7283UL, false);
                set => this.WriteData(7283UL, value, false);
            }

            public bool Full240
            {
                get => this.ReadDataBool(7284UL, false);
                set => this.WriteData(7284UL, value, false);
            }

            public Mas.Schema.Model.Monica.Voc.Emissions.WRITER GuentherEmissions
            {
                get => BuildPointer<Mas.Schema.Model.Monica.Voc.Emissions.WRITER>(43);
                set => Link(43, value);
            }

            public Mas.Schema.Model.Monica.Voc.Emissions.WRITER JjvEmissions
            {
                get => BuildPointer<Mas.Schema.Model.Monica.Voc.Emissions.WRITER>(44);
                set => Link(44, value);
            }

            public Mas.Schema.Model.Monica.Voc.SpeciesData.WRITER VocSpecies
            {
                get => BuildPointer<Mas.Schema.Model.Monica.Voc.SpeciesData.WRITER>(45);
                set => Link(45, value);
            }

            public Mas.Schema.Model.Monica.Voc.CPData.WRITER CropPhotosynthesisResults
            {
                get => BuildPointer<Mas.Schema.Model.Monica.Voc.CPData.WRITER>(46);
                set => Link(46, value);
            }

            public Mas.Schema.Model.Monica.SpeciesParameters.WRITER SpeciesParams
            {
                get => BuildPointer<Mas.Schema.Model.Monica.SpeciesParameters.WRITER>(47);
                set => Link(47, value);
            }

            public Mas.Schema.Model.Monica.CultivarParameters.WRITER CultivarParams
            {
                get => BuildPointer<Mas.Schema.Model.Monica.CultivarParameters.WRITER>(48);
                set => Link(48, value);
            }

            public Mas.Schema.Model.Monica.CropResidueParameters.WRITER ResidueParams
            {
                get => BuildPointer<Mas.Schema.Model.Monica.CropResidueParameters.WRITER>(49);
                set => Link(49, value);
            }

            public bool IsWinterCrop
            {
                get => this.ReadDataBool(7285UL, false);
                set => this.WriteData(7285UL, value, false);
            }

            public bool StemElongationEventFired
            {
                get => this.ReadDataBool(7286UL, false);
                set => this.WriteData(7286UL, value, false);
            }

            public double Lt50m
            {
                get => this.ReadDataDouble(9920UL, -3);
                set => this.WriteData(9920UL, value, -3);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa4da01d10b3b6acdUL)]
    public class SnowModuleState : ICapnpSerializable
    {
        public const UInt64 typeId = 0xa4da01d10b3b6acdUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            SnowRetentionCapacityMax = reader.SnowRetentionCapacityMax;
            SnowDensity = reader.SnowDensity;
            SnowDepth = reader.SnowDepth;
            FrozenWaterInSnow = reader.FrozenWaterInSnow;
            LiquidWaterInSnow = reader.LiquidWaterInSnow;
            WaterToInfiltrate = reader.WaterToInfiltrate;
            MaxSnowDepth = reader.MaxSnowDepth;
            AccumulatedSnowDepth = reader.AccumulatedSnowDepth;
            SnowmeltTemperature = reader.SnowmeltTemperature;
            SnowAccumulationThresholdTemperature = reader.SnowAccumulationThresholdTemperature;
            TemperatureLimitForLiquidWater = reader.TemperatureLimitForLiquidWater;
            CorrectionRain = reader.CorrectionRain;
            CorrectionSnow = reader.CorrectionSnow;
            RefreezeTemperature = reader.RefreezeTemperature;
            RefreezeP1 = reader.RefreezeP1;
            RefreezeP2 = reader.RefreezeP2;
            NewSnowDensityMin = reader.NewSnowDensityMin;
            SnowMaxAdditionalDensity = reader.SnowMaxAdditionalDensity;
            SnowPacking = reader.SnowPacking;
            SnowRetentionCapacityMin = reader.SnowRetentionCapacityMin;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.SnowRetentionCapacityMax = SnowRetentionCapacityMax;
            writer.SnowDensity = SnowDensity;
            writer.SnowDepth = SnowDepth;
            writer.FrozenWaterInSnow = FrozenWaterInSnow;
            writer.LiquidWaterInSnow = LiquidWaterInSnow;
            writer.WaterToInfiltrate = WaterToInfiltrate;
            writer.MaxSnowDepth = MaxSnowDepth;
            writer.AccumulatedSnowDepth = AccumulatedSnowDepth;
            writer.SnowmeltTemperature = SnowmeltTemperature;
            writer.SnowAccumulationThresholdTemperature = SnowAccumulationThresholdTemperature;
            writer.TemperatureLimitForLiquidWater = TemperatureLimitForLiquidWater;
            writer.CorrectionRain = CorrectionRain;
            writer.CorrectionSnow = CorrectionSnow;
            writer.RefreezeTemperature = RefreezeTemperature;
            writer.RefreezeP1 = RefreezeP1;
            writer.RefreezeP2 = RefreezeP2;
            writer.NewSnowDensityMin = NewSnowDensityMin;
            writer.SnowMaxAdditionalDensity = SnowMaxAdditionalDensity;
            writer.SnowPacking = SnowPacking;
            writer.SnowRetentionCapacityMin = SnowRetentionCapacityMin;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public double SnowRetentionCapacityMax
        {
            get;
            set;
        }

        public double SnowDensity
        {
            get;
            set;
        }

        public double SnowDepth
        {
            get;
            set;
        }

        public double FrozenWaterInSnow
        {
            get;
            set;
        }

        public double LiquidWaterInSnow
        {
            get;
            set;
        }

        public double WaterToInfiltrate
        {
            get;
            set;
        }

        public double MaxSnowDepth
        {
            get;
            set;
        }

        public double AccumulatedSnowDepth
        {
            get;
            set;
        }

        public double SnowmeltTemperature
        {
            get;
            set;
        }

        public double SnowAccumulationThresholdTemperature
        {
            get;
            set;
        }

        public double TemperatureLimitForLiquidWater
        {
            get;
            set;
        }

        public double CorrectionRain
        {
            get;
            set;
        }

        public double CorrectionSnow
        {
            get;
            set;
        }

        public double RefreezeTemperature
        {
            get;
            set;
        }

        public double RefreezeP1
        {
            get;
            set;
        }

        public double RefreezeP2
        {
            get;
            set;
        }

        public double NewSnowDensityMin
        {
            get;
            set;
        }

        public double SnowMaxAdditionalDensity
        {
            get;
            set;
        }

        public double SnowPacking
        {
            get;
            set;
        }

        public double SnowRetentionCapacityMin
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
            public double SnowRetentionCapacityMax => ctx.ReadDataDouble(0UL, 0);
            public double SnowDensity => ctx.ReadDataDouble(64UL, 0);
            public double SnowDepth => ctx.ReadDataDouble(128UL, 0);
            public double FrozenWaterInSnow => ctx.ReadDataDouble(192UL, 0);
            public double LiquidWaterInSnow => ctx.ReadDataDouble(256UL, 0);
            public double WaterToInfiltrate => ctx.ReadDataDouble(320UL, 0);
            public double MaxSnowDepth => ctx.ReadDataDouble(384UL, 0);
            public double AccumulatedSnowDepth => ctx.ReadDataDouble(448UL, 0);
            public double SnowmeltTemperature => ctx.ReadDataDouble(512UL, 0);
            public double SnowAccumulationThresholdTemperature => ctx.ReadDataDouble(576UL, 0);
            public double TemperatureLimitForLiquidWater => ctx.ReadDataDouble(640UL, 0);
            public double CorrectionRain => ctx.ReadDataDouble(704UL, 0);
            public double CorrectionSnow => ctx.ReadDataDouble(768UL, 0);
            public double RefreezeTemperature => ctx.ReadDataDouble(832UL, 0);
            public double RefreezeP1 => ctx.ReadDataDouble(896UL, 0);
            public double RefreezeP2 => ctx.ReadDataDouble(960UL, 0);
            public double NewSnowDensityMin => ctx.ReadDataDouble(1024UL, 0);
            public double SnowMaxAdditionalDensity => ctx.ReadDataDouble(1088UL, 0);
            public double SnowPacking => ctx.ReadDataDouble(1152UL, 0);
            public double SnowRetentionCapacityMin => ctx.ReadDataDouble(1216UL, 0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(20, 0);
            }

            public double SnowRetentionCapacityMax
            {
                get => this.ReadDataDouble(0UL, 0);
                set => this.WriteData(0UL, value, 0);
            }

            public double SnowDensity
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public double SnowDepth
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }

            public double FrozenWaterInSnow
            {
                get => this.ReadDataDouble(192UL, 0);
                set => this.WriteData(192UL, value, 0);
            }

            public double LiquidWaterInSnow
            {
                get => this.ReadDataDouble(256UL, 0);
                set => this.WriteData(256UL, value, 0);
            }

            public double WaterToInfiltrate
            {
                get => this.ReadDataDouble(320UL, 0);
                set => this.WriteData(320UL, value, 0);
            }

            public double MaxSnowDepth
            {
                get => this.ReadDataDouble(384UL, 0);
                set => this.WriteData(384UL, value, 0);
            }

            public double AccumulatedSnowDepth
            {
                get => this.ReadDataDouble(448UL, 0);
                set => this.WriteData(448UL, value, 0);
            }

            public double SnowmeltTemperature
            {
                get => this.ReadDataDouble(512UL, 0);
                set => this.WriteData(512UL, value, 0);
            }

            public double SnowAccumulationThresholdTemperature
            {
                get => this.ReadDataDouble(576UL, 0);
                set => this.WriteData(576UL, value, 0);
            }

            public double TemperatureLimitForLiquidWater
            {
                get => this.ReadDataDouble(640UL, 0);
                set => this.WriteData(640UL, value, 0);
            }

            public double CorrectionRain
            {
                get => this.ReadDataDouble(704UL, 0);
                set => this.WriteData(704UL, value, 0);
            }

            public double CorrectionSnow
            {
                get => this.ReadDataDouble(768UL, 0);
                set => this.WriteData(768UL, value, 0);
            }

            public double RefreezeTemperature
            {
                get => this.ReadDataDouble(832UL, 0);
                set => this.WriteData(832UL, value, 0);
            }

            public double RefreezeP1
            {
                get => this.ReadDataDouble(896UL, 0);
                set => this.WriteData(896UL, value, 0);
            }

            public double RefreezeP2
            {
                get => this.ReadDataDouble(960UL, 0);
                set => this.WriteData(960UL, value, 0);
            }

            public double NewSnowDensityMin
            {
                get => this.ReadDataDouble(1024UL, 0);
                set => this.WriteData(1024UL, value, 0);
            }

            public double SnowMaxAdditionalDensity
            {
                get => this.ReadDataDouble(1088UL, 0);
                set => this.WriteData(1088UL, value, 0);
            }

            public double SnowPacking
            {
                get => this.ReadDataDouble(1152UL, 0);
                set => this.WriteData(1152UL, value, 0);
            }

            public double SnowRetentionCapacityMin
            {
                get => this.ReadDataDouble(1216UL, 0);
                set => this.WriteData(1216UL, value, 0);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb4f16ea3144d85a6UL)]
    public class FrostModuleState : ICapnpSerializable
    {
        public const UInt64 typeId = 0xb4f16ea3144d85a6UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            PmHydraulicConductivityRedux = reader.PmHydraulicConductivityRedux;
            FrostDepth = reader.FrostDepth;
            AccumulatedFrostDepth = reader.AccumulatedFrostDepth;
            NegativeDegreeDays = reader.NegativeDegreeDays;
            ThawDepth = reader.ThawDepth;
            FrostDays = reader.FrostDays;
            LambdaRedux = reader.LambdaRedux;
            TemperatureUnderSnow = reader.TemperatureUnderSnow;
            HydraulicConductivityRedux = reader.HydraulicConductivityRedux;
            PtTimeStep = reader.PtTimeStep;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.PmHydraulicConductivityRedux = PmHydraulicConductivityRedux;
            writer.FrostDepth = FrostDepth;
            writer.AccumulatedFrostDepth = AccumulatedFrostDepth;
            writer.NegativeDegreeDays = NegativeDegreeDays;
            writer.ThawDepth = ThawDepth;
            writer.FrostDays = FrostDays;
            writer.LambdaRedux.Init(LambdaRedux);
            writer.TemperatureUnderSnow = TemperatureUnderSnow;
            writer.HydraulicConductivityRedux = HydraulicConductivityRedux;
            writer.PtTimeStep = PtTimeStep;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public double PmHydraulicConductivityRedux
        {
            get;
            set;
        }

        public double FrostDepth
        {
            get;
            set;
        }

        public double AccumulatedFrostDepth
        {
            get;
            set;
        }

        public double NegativeDegreeDays
        {
            get;
            set;
        }

        public double ThawDepth
        {
            get;
            set;
        }

        public ushort FrostDays
        {
            get;
            set;
        }

        public IReadOnlyList<double> LambdaRedux
        {
            get;
            set;
        }

        public double TemperatureUnderSnow
        {
            get;
            set;
        }

        public double HydraulicConductivityRedux
        {
            get;
            set;
        }

        public double PtTimeStep
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
            public double PmHydraulicConductivityRedux => ctx.ReadDataDouble(0UL, 0);
            public double FrostDepth => ctx.ReadDataDouble(64UL, 0);
            public double AccumulatedFrostDepth => ctx.ReadDataDouble(128UL, 0);
            public double NegativeDegreeDays => ctx.ReadDataDouble(192UL, 0);
            public double ThawDepth => ctx.ReadDataDouble(256UL, 0);
            public ushort FrostDays => ctx.ReadDataUShort(320UL, (ushort)0);
            public IReadOnlyList<double> LambdaRedux => ctx.ReadList(0).CastDouble();
            public bool HasLambdaRedux => ctx.IsStructFieldNonNull(0);
            public double TemperatureUnderSnow => ctx.ReadDataDouble(384UL, 0);
            public double HydraulicConductivityRedux => ctx.ReadDataDouble(448UL, 0);
            public double PtTimeStep => ctx.ReadDataDouble(512UL, 0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(9, 1);
            }

            public double PmHydraulicConductivityRedux
            {
                get => this.ReadDataDouble(0UL, 0);
                set => this.WriteData(0UL, value, 0);
            }

            public double FrostDepth
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public double AccumulatedFrostDepth
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }

            public double NegativeDegreeDays
            {
                get => this.ReadDataDouble(192UL, 0);
                set => this.WriteData(192UL, value, 0);
            }

            public double ThawDepth
            {
                get => this.ReadDataDouble(256UL, 0);
                set => this.WriteData(256UL, value, 0);
            }

            public ushort FrostDays
            {
                get => this.ReadDataUShort(320UL, (ushort)0);
                set => this.WriteData(320UL, value, (ushort)0);
            }

            public ListOfPrimitivesSerializer<double> LambdaRedux
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(0);
                set => Link(0, value);
            }

            public double TemperatureUnderSnow
            {
                get => this.ReadDataDouble(384UL, 0);
                set => this.WriteData(384UL, value, 0);
            }

            public double HydraulicConductivityRedux
            {
                get => this.ReadDataDouble(448UL, 0);
                set => this.WriteData(448UL, value, 0);
            }

            public double PtTimeStep
            {
                get => this.ReadDataDouble(512UL, 0);
                set => this.WriteData(512UL, value, 0);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcd05962719bf7ec8UL)]
    public class SoilMoistureModuleState : ICapnpSerializable
    {
        public const UInt64 typeId = 0xcd05962719bf7ec8UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            FrostComponent = CapnpSerializable.Create<Mas.Schema.Model.Monica.FrostModuleState>(reader.FrostComponent);
            SnowComponent = CapnpSerializable.Create<Mas.Schema.Model.Monica.SnowModuleState>(reader.SnowComponent);
            XSACriticalSoilMoisture = reader.XSACriticalSoilMoisture;
            ModuleParams = CapnpSerializable.Create<Mas.Schema.Model.Monica.SoilMoistureModuleParameters>(reader.ModuleParams);
            VwWindSpeedHeight = reader.VwWindSpeedHeight;
            VwWindSpeed = reader.VwWindSpeed;
            NumberOfLayers = reader.NumberOfLayers;
            VsNumberOfLayers = reader.VsNumberOfLayers;
            ActualEvaporation = reader.ActualEvaporation;
            ActualEvapotranspiration = reader.ActualEvapotranspiration;
            ActualTranspiration = reader.ActualTranspiration;
            AvailableWater = reader.AvailableWater;
            CapillaryRise = reader.CapillaryRise;
            CapillaryRiseRate = reader.CapillaryRiseRate;
            CapillaryWater = reader.CapillaryWater;
            CapillaryWater70 = reader.CapillaryWater70;
            Evaporation = reader.Evaporation;
            Evapotranspiration = reader.Evapotranspiration;
            FieldCapacity = reader.FieldCapacity;
            FluxAtLowerBoundary = reader.FluxAtLowerBoundary;
            GravitationalWater = reader.GravitationalWater;
            GrossPrecipitation = reader.GrossPrecipitation;
            GroundwaterAdded = reader.GroundwaterAdded;
            GroundwaterDischarge = reader.GroundwaterDischarge;
            GroundwaterTable = reader.GroundwaterTable;
            HeatConductivity = reader.HeatConductivity;
            HydraulicConductivityRedux = reader.HydraulicConductivityRedux;
            Infiltration = reader.Infiltration;
            Interception = reader.Interception;
            VcKcFactor = reader.VcKcFactor;
            Lambda = reader.Lambda;
            LambdaReduced = reader.LambdaReduced;
            VsLatitude = reader.VsLatitude;
            LayerThickness = reader.LayerThickness;
            PmLayerThickness = reader.PmLayerThickness;
            PmLeachingDepth = reader.PmLeachingDepth;
            PmLeachingDepthLayer = reader.PmLeachingDepthLayer;
            VwMaxAirTemperature = reader.VwMaxAirTemperature;
            PmMaxPercolationRate = reader.PmMaxPercolationRate;
            VwMeanAirTemperature = reader.VwMeanAirTemperature;
            VwMinAirTemperature = reader.VwMinAirTemperature;
            VcNetPrecipitation = reader.VcNetPrecipitation;
            VwNetRadiation = reader.VwNetRadiation;
            PermanentWiltingPoint = reader.PermanentWiltingPoint;
            VcPercentageSoilCoverage = reader.VcPercentageSoilCoverage;
            PercolationRate = reader.PercolationRate;
            VwPrecipitation = reader.VwPrecipitation;
            ReferenceEvapotranspiration = reader.ReferenceEvapotranspiration;
            RelativeHumidity = reader.RelativeHumidity;
            ResidualEvapotranspiration = reader.ResidualEvapotranspiration;
            SaturatedHydraulicConductivity = reader.SaturatedHydraulicConductivity;
            SoilMoisture = reader.SoilMoisture;
            SoilMoisturecrit = reader.SoilMoisturecrit;
            SoilMoistureDeficit = reader.SoilMoistureDeficit;
            SoilPoreVolume = reader.SoilPoreVolume;
            VcStomataResistance = reader.VcStomataResistance;
            SurfaceRoughness = reader.SurfaceRoughness;
            SurfaceRunOff = reader.SurfaceRunOff;
            SumSurfaceRunOff = reader.SumSurfaceRunOff;
            SurfaceWaterStorage = reader.SurfaceWaterStorage;
            PtTimeStep = reader.PtTimeStep;
            TotalWaterRemoval = reader.TotalWaterRemoval;
            Transpiration = reader.Transpiration;
            TranspirationDeficit = reader.TranspirationDeficit;
            WaterFlux = reader.WaterFlux;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            FrostComponent?.serialize(writer.FrostComponent);
            SnowComponent?.serialize(writer.SnowComponent);
            writer.XSACriticalSoilMoisture = XSACriticalSoilMoisture;
            ModuleParams?.serialize(writer.ModuleParams);
            writer.VwWindSpeedHeight = VwWindSpeedHeight;
            writer.VwWindSpeed = VwWindSpeed;
            writer.NumberOfLayers = NumberOfLayers;
            writer.VsNumberOfLayers = VsNumberOfLayers;
            writer.ActualEvaporation = ActualEvaporation;
            writer.ActualEvapotranspiration = ActualEvapotranspiration;
            writer.ActualTranspiration = ActualTranspiration;
            writer.AvailableWater.Init(AvailableWater);
            writer.CapillaryRise = CapillaryRise;
            writer.CapillaryRiseRate.Init(CapillaryRiseRate);
            writer.CapillaryWater.Init(CapillaryWater);
            writer.CapillaryWater70.Init(CapillaryWater70);
            writer.Evaporation.Init(Evaporation);
            writer.Evapotranspiration.Init(Evapotranspiration);
            writer.FieldCapacity.Init(FieldCapacity);
            writer.FluxAtLowerBoundary = FluxAtLowerBoundary;
            writer.GravitationalWater.Init(GravitationalWater);
            writer.GrossPrecipitation = GrossPrecipitation;
            writer.GroundwaterAdded = GroundwaterAdded;
            writer.GroundwaterDischarge = GroundwaterDischarge;
            writer.GroundwaterTable = GroundwaterTable;
            writer.HeatConductivity.Init(HeatConductivity);
            writer.HydraulicConductivityRedux = HydraulicConductivityRedux;
            writer.Infiltration = Infiltration;
            writer.Interception = Interception;
            writer.VcKcFactor = VcKcFactor;
            writer.Lambda.Init(Lambda);
            writer.LambdaReduced = LambdaReduced;
            writer.VsLatitude = VsLatitude;
            writer.LayerThickness.Init(LayerThickness);
            writer.PmLayerThickness = PmLayerThickness;
            writer.PmLeachingDepth = PmLeachingDepth;
            writer.PmLeachingDepthLayer = PmLeachingDepthLayer;
            writer.VwMaxAirTemperature = VwMaxAirTemperature;
            writer.PmMaxPercolationRate = PmMaxPercolationRate;
            writer.VwMeanAirTemperature = VwMeanAirTemperature;
            writer.VwMinAirTemperature = VwMinAirTemperature;
            writer.VcNetPrecipitation = VcNetPrecipitation;
            writer.VwNetRadiation = VwNetRadiation;
            writer.PermanentWiltingPoint.Init(PermanentWiltingPoint);
            writer.VcPercentageSoilCoverage = VcPercentageSoilCoverage;
            writer.PercolationRate.Init(PercolationRate);
            writer.VwPrecipitation = VwPrecipitation;
            writer.ReferenceEvapotranspiration = ReferenceEvapotranspiration;
            writer.RelativeHumidity = RelativeHumidity;
            writer.ResidualEvapotranspiration.Init(ResidualEvapotranspiration);
            writer.SaturatedHydraulicConductivity.Init(SaturatedHydraulicConductivity);
            writer.SoilMoisture.Init(SoilMoisture);
            writer.SoilMoisturecrit = SoilMoisturecrit;
            writer.SoilMoistureDeficit = SoilMoistureDeficit;
            writer.SoilPoreVolume.Init(SoilPoreVolume);
            writer.VcStomataResistance = VcStomataResistance;
            writer.SurfaceRoughness = SurfaceRoughness;
            writer.SurfaceRunOff = SurfaceRunOff;
            writer.SumSurfaceRunOff = SumSurfaceRunOff;
            writer.SurfaceWaterStorage = SurfaceWaterStorage;
            writer.PtTimeStep = PtTimeStep;
            writer.TotalWaterRemoval = TotalWaterRemoval;
            writer.Transpiration.Init(Transpiration);
            writer.TranspirationDeficit = TranspirationDeficit;
            writer.WaterFlux.Init(WaterFlux);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Schema.Model.Monica.FrostModuleState FrostComponent
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.SnowModuleState SnowComponent
        {
            get;
            set;
        }

        public double XSACriticalSoilMoisture
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.SoilMoistureModuleParameters ModuleParams
        {
            get;
            set;
        }

        public double VwWindSpeedHeight
        {
            get;
            set;
        }

        public double VwWindSpeed
        {
            get;
            set;
        }

        public ushort NumberOfLayers
        {
            get;
            set;
        }

        public ushort VsNumberOfLayers
        {
            get;
            set;
        }

        public double ActualEvaporation
        {
            get;
            set;
        }

        public double ActualEvapotranspiration
        {
            get;
            set;
        }

        public double ActualTranspiration
        {
            get;
            set;
        }

        public IReadOnlyList<double> AvailableWater
        {
            get;
            set;
        }

        public double CapillaryRise
        {
            get;
            set;
        }

        public IReadOnlyList<double> CapillaryRiseRate
        {
            get;
            set;
        }

        public IReadOnlyList<double> CapillaryWater
        {
            get;
            set;
        }

        public IReadOnlyList<double> CapillaryWater70
        {
            get;
            set;
        }

        public IReadOnlyList<double> Evaporation
        {
            get;
            set;
        }

        public IReadOnlyList<double> Evapotranspiration
        {
            get;
            set;
        }

        public IReadOnlyList<double> FieldCapacity
        {
            get;
            set;
        }

        public double FluxAtLowerBoundary
        {
            get;
            set;
        }

        public IReadOnlyList<double> GravitationalWater
        {
            get;
            set;
        }

        public double GrossPrecipitation
        {
            get;
            set;
        }

        public double GroundwaterAdded
        {
            get;
            set;
        }

        public double GroundwaterDischarge
        {
            get;
            set;
        }

        public ushort GroundwaterTable
        {
            get;
            set;
        }

        public IReadOnlyList<double> HeatConductivity
        {
            get;
            set;
        }

        public double HydraulicConductivityRedux
        {
            get;
            set;
        }

        public double Infiltration
        {
            get;
            set;
        }

        public double Interception
        {
            get;
            set;
        }

        public double VcKcFactor
        {
            get;
            set;
        }

        = 0.6;
        public IReadOnlyList<double> Lambda
        {
            get;
            set;
        }

        public double LambdaReduced
        {
            get;
            set;
        }

        public double VsLatitude
        {
            get;
            set;
        }

        public IReadOnlyList<double> LayerThickness
        {
            get;
            set;
        }

        public double PmLayerThickness
        {
            get;
            set;
        }

        public double PmLeachingDepth
        {
            get;
            set;
        }

        public ushort PmLeachingDepthLayer
        {
            get;
            set;
        }

        public double VwMaxAirTemperature
        {
            get;
            set;
        }

        public double PmMaxPercolationRate
        {
            get;
            set;
        }

        public double VwMeanAirTemperature
        {
            get;
            set;
        }

        public double VwMinAirTemperature
        {
            get;
            set;
        }

        public double VcNetPrecipitation
        {
            get;
            set;
        }

        public double VwNetRadiation
        {
            get;
            set;
        }

        public IReadOnlyList<double> PermanentWiltingPoint
        {
            get;
            set;
        }

        public double VcPercentageSoilCoverage
        {
            get;
            set;
        }

        public IReadOnlyList<double> PercolationRate
        {
            get;
            set;
        }

        public double VwPrecipitation
        {
            get;
            set;
        }

        public double ReferenceEvapotranspiration
        {
            get;
            set;
        }

        = 6;
        public double RelativeHumidity
        {
            get;
            set;
        }

        public IReadOnlyList<double> ResidualEvapotranspiration
        {
            get;
            set;
        }

        public IReadOnlyList<double> SaturatedHydraulicConductivity
        {
            get;
            set;
        }

        public IReadOnlyList<double> SoilMoisture
        {
            get;
            set;
        }

        public double SoilMoisturecrit
        {
            get;
            set;
        }

        public double SoilMoistureDeficit
        {
            get;
            set;
        }

        public IReadOnlyList<double> SoilPoreVolume
        {
            get;
            set;
        }

        public double VcStomataResistance
        {
            get;
            set;
        }

        public double SurfaceRoughness
        {
            get;
            set;
        }

        public double SurfaceRunOff
        {
            get;
            set;
        }

        public double SumSurfaceRunOff
        {
            get;
            set;
        }

        public double SurfaceWaterStorage
        {
            get;
            set;
        }

        public double PtTimeStep
        {
            get;
            set;
        }

        public double TotalWaterRemoval
        {
            get;
            set;
        }

        public IReadOnlyList<double> Transpiration
        {
            get;
            set;
        }

        public double TranspirationDeficit
        {
            get;
            set;
        }

        public IReadOnlyList<double> WaterFlux
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
            public Mas.Schema.Model.Monica.FrostModuleState.READER FrostComponent => ctx.ReadStruct(0, Mas.Schema.Model.Monica.FrostModuleState.READER.create);
            public bool HasFrostComponent => ctx.IsStructFieldNonNull(0);
            public Mas.Schema.Model.Monica.SnowModuleState.READER SnowComponent => ctx.ReadStruct(1, Mas.Schema.Model.Monica.SnowModuleState.READER.create);
            public bool HasSnowComponent => ctx.IsStructFieldNonNull(1);
            public double XSACriticalSoilMoisture => ctx.ReadDataDouble(0UL, 0);
            public Mas.Schema.Model.Monica.SoilMoistureModuleParameters.READER ModuleParams => ctx.ReadStruct(2, Mas.Schema.Model.Monica.SoilMoistureModuleParameters.READER.create);
            public bool HasModuleParams => ctx.IsStructFieldNonNull(2);
            public double VwWindSpeedHeight => ctx.ReadDataDouble(64UL, 0);
            public double VwWindSpeed => ctx.ReadDataDouble(128UL, 0);
            public ushort NumberOfLayers => ctx.ReadDataUShort(192UL, (ushort)0);
            public ushort VsNumberOfLayers => ctx.ReadDataUShort(208UL, (ushort)0);
            public double ActualEvaporation => ctx.ReadDataDouble(256UL, 0);
            public double ActualEvapotranspiration => ctx.ReadDataDouble(320UL, 0);
            public double ActualTranspiration => ctx.ReadDataDouble(384UL, 0);
            public IReadOnlyList<double> AvailableWater => ctx.ReadList(3).CastDouble();
            public bool HasAvailableWater => ctx.IsStructFieldNonNull(3);
            public double CapillaryRise => ctx.ReadDataDouble(448UL, 0);
            public IReadOnlyList<double> CapillaryRiseRate => ctx.ReadList(4).CastDouble();
            public bool HasCapillaryRiseRate => ctx.IsStructFieldNonNull(4);
            public IReadOnlyList<double> CapillaryWater => ctx.ReadList(5).CastDouble();
            public bool HasCapillaryWater => ctx.IsStructFieldNonNull(5);
            public IReadOnlyList<double> CapillaryWater70 => ctx.ReadList(6).CastDouble();
            public bool HasCapillaryWater70 => ctx.IsStructFieldNonNull(6);
            public IReadOnlyList<double> Evaporation => ctx.ReadList(7).CastDouble();
            public bool HasEvaporation => ctx.IsStructFieldNonNull(7);
            public IReadOnlyList<double> Evapotranspiration => ctx.ReadList(8).CastDouble();
            public bool HasEvapotranspiration => ctx.IsStructFieldNonNull(8);
            public IReadOnlyList<double> FieldCapacity => ctx.ReadList(9).CastDouble();
            public bool HasFieldCapacity => ctx.IsStructFieldNonNull(9);
            public double FluxAtLowerBoundary => ctx.ReadDataDouble(512UL, 0);
            public IReadOnlyList<double> GravitationalWater => ctx.ReadList(10).CastDouble();
            public bool HasGravitationalWater => ctx.IsStructFieldNonNull(10);
            public double GrossPrecipitation => ctx.ReadDataDouble(576UL, 0);
            public double GroundwaterAdded => ctx.ReadDataDouble(640UL, 0);
            public double GroundwaterDischarge => ctx.ReadDataDouble(704UL, 0);
            public ushort GroundwaterTable => ctx.ReadDataUShort(224UL, (ushort)0);
            public IReadOnlyList<double> HeatConductivity => ctx.ReadList(11).CastDouble();
            public bool HasHeatConductivity => ctx.IsStructFieldNonNull(11);
            public double HydraulicConductivityRedux => ctx.ReadDataDouble(768UL, 0);
            public double Infiltration => ctx.ReadDataDouble(832UL, 0);
            public double Interception => ctx.ReadDataDouble(896UL, 0);
            public double VcKcFactor => ctx.ReadDataDouble(960UL, 0.6);
            public IReadOnlyList<double> Lambda => ctx.ReadList(12).CastDouble();
            public bool HasLambda => ctx.IsStructFieldNonNull(12);
            public double LambdaReduced => ctx.ReadDataDouble(1024UL, 0);
            public double VsLatitude => ctx.ReadDataDouble(1088UL, 0);
            public IReadOnlyList<double> LayerThickness => ctx.ReadList(13).CastDouble();
            public bool HasLayerThickness => ctx.IsStructFieldNonNull(13);
            public double PmLayerThickness => ctx.ReadDataDouble(1152UL, 0);
            public double PmLeachingDepth => ctx.ReadDataDouble(1216UL, 0);
            public ushort PmLeachingDepthLayer => ctx.ReadDataUShort(240UL, (ushort)0);
            public double VwMaxAirTemperature => ctx.ReadDataDouble(1280UL, 0);
            public double PmMaxPercolationRate => ctx.ReadDataDouble(1344UL, 0);
            public double VwMeanAirTemperature => ctx.ReadDataDouble(1408UL, 0);
            public double VwMinAirTemperature => ctx.ReadDataDouble(1472UL, 0);
            public double VcNetPrecipitation => ctx.ReadDataDouble(1536UL, 0);
            public double VwNetRadiation => ctx.ReadDataDouble(1600UL, 0);
            public IReadOnlyList<double> PermanentWiltingPoint => ctx.ReadList(14).CastDouble();
            public bool HasPermanentWiltingPoint => ctx.IsStructFieldNonNull(14);
            public double VcPercentageSoilCoverage => ctx.ReadDataDouble(1664UL, 0);
            public IReadOnlyList<double> PercolationRate => ctx.ReadList(15).CastDouble();
            public bool HasPercolationRate => ctx.IsStructFieldNonNull(15);
            public double VwPrecipitation => ctx.ReadDataDouble(1728UL, 0);
            public double ReferenceEvapotranspiration => ctx.ReadDataDouble(1792UL, 6);
            public double RelativeHumidity => ctx.ReadDataDouble(1856UL, 0);
            public IReadOnlyList<double> ResidualEvapotranspiration => ctx.ReadList(16).CastDouble();
            public bool HasResidualEvapotranspiration => ctx.IsStructFieldNonNull(16);
            public IReadOnlyList<double> SaturatedHydraulicConductivity => ctx.ReadList(17).CastDouble();
            public bool HasSaturatedHydraulicConductivity => ctx.IsStructFieldNonNull(17);
            public IReadOnlyList<double> SoilMoisture => ctx.ReadList(18).CastDouble();
            public bool HasSoilMoisture => ctx.IsStructFieldNonNull(18);
            public double SoilMoisturecrit => ctx.ReadDataDouble(1920UL, 0);
            public double SoilMoistureDeficit => ctx.ReadDataDouble(1984UL, 0);
            public IReadOnlyList<double> SoilPoreVolume => ctx.ReadList(19).CastDouble();
            public bool HasSoilPoreVolume => ctx.IsStructFieldNonNull(19);
            public double VcStomataResistance => ctx.ReadDataDouble(2048UL, 0);
            public double SurfaceRoughness => ctx.ReadDataDouble(2112UL, 0);
            public double SurfaceRunOff => ctx.ReadDataDouble(2176UL, 0);
            public double SumSurfaceRunOff => ctx.ReadDataDouble(2240UL, 0);
            public double SurfaceWaterStorage => ctx.ReadDataDouble(2304UL, 0);
            public double PtTimeStep => ctx.ReadDataDouble(2368UL, 0);
            public double TotalWaterRemoval => ctx.ReadDataDouble(2432UL, 0);
            public IReadOnlyList<double> Transpiration => ctx.ReadList(20).CastDouble();
            public bool HasTranspiration => ctx.IsStructFieldNonNull(20);
            public double TranspirationDeficit => ctx.ReadDataDouble(2496UL, 0);
            public IReadOnlyList<double> WaterFlux => ctx.ReadList(21).CastDouble();
            public bool HasWaterFlux => ctx.IsStructFieldNonNull(21);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(40, 22);
            }

            public Mas.Schema.Model.Monica.FrostModuleState.WRITER FrostComponent
            {
                get => BuildPointer<Mas.Schema.Model.Monica.FrostModuleState.WRITER>(0);
                set => Link(0, value);
            }

            public Mas.Schema.Model.Monica.SnowModuleState.WRITER SnowComponent
            {
                get => BuildPointer<Mas.Schema.Model.Monica.SnowModuleState.WRITER>(1);
                set => Link(1, value);
            }

            public double XSACriticalSoilMoisture
            {
                get => this.ReadDataDouble(0UL, 0);
                set => this.WriteData(0UL, value, 0);
            }

            public Mas.Schema.Model.Monica.SoilMoistureModuleParameters.WRITER ModuleParams
            {
                get => BuildPointer<Mas.Schema.Model.Monica.SoilMoistureModuleParameters.WRITER>(2);
                set => Link(2, value);
            }

            public double VwWindSpeedHeight
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public double VwWindSpeed
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }

            public ushort NumberOfLayers
            {
                get => this.ReadDataUShort(192UL, (ushort)0);
                set => this.WriteData(192UL, value, (ushort)0);
            }

            public ushort VsNumberOfLayers
            {
                get => this.ReadDataUShort(208UL, (ushort)0);
                set => this.WriteData(208UL, value, (ushort)0);
            }

            public double ActualEvaporation
            {
                get => this.ReadDataDouble(256UL, 0);
                set => this.WriteData(256UL, value, 0);
            }

            public double ActualEvapotranspiration
            {
                get => this.ReadDataDouble(320UL, 0);
                set => this.WriteData(320UL, value, 0);
            }

            public double ActualTranspiration
            {
                get => this.ReadDataDouble(384UL, 0);
                set => this.WriteData(384UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> AvailableWater
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(3);
                set => Link(3, value);
            }

            public double CapillaryRise
            {
                get => this.ReadDataDouble(448UL, 0);
                set => this.WriteData(448UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> CapillaryRiseRate
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(4);
                set => Link(4, value);
            }

            public ListOfPrimitivesSerializer<double> CapillaryWater
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(5);
                set => Link(5, value);
            }

            public ListOfPrimitivesSerializer<double> CapillaryWater70
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(6);
                set => Link(6, value);
            }

            public ListOfPrimitivesSerializer<double> Evaporation
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(7);
                set => Link(7, value);
            }

            public ListOfPrimitivesSerializer<double> Evapotranspiration
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(8);
                set => Link(8, value);
            }

            public ListOfPrimitivesSerializer<double> FieldCapacity
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(9);
                set => Link(9, value);
            }

            public double FluxAtLowerBoundary
            {
                get => this.ReadDataDouble(512UL, 0);
                set => this.WriteData(512UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> GravitationalWater
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(10);
                set => Link(10, value);
            }

            public double GrossPrecipitation
            {
                get => this.ReadDataDouble(576UL, 0);
                set => this.WriteData(576UL, value, 0);
            }

            public double GroundwaterAdded
            {
                get => this.ReadDataDouble(640UL, 0);
                set => this.WriteData(640UL, value, 0);
            }

            public double GroundwaterDischarge
            {
                get => this.ReadDataDouble(704UL, 0);
                set => this.WriteData(704UL, value, 0);
            }

            public ushort GroundwaterTable
            {
                get => this.ReadDataUShort(224UL, (ushort)0);
                set => this.WriteData(224UL, value, (ushort)0);
            }

            public ListOfPrimitivesSerializer<double> HeatConductivity
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(11);
                set => Link(11, value);
            }

            public double HydraulicConductivityRedux
            {
                get => this.ReadDataDouble(768UL, 0);
                set => this.WriteData(768UL, value, 0);
            }

            public double Infiltration
            {
                get => this.ReadDataDouble(832UL, 0);
                set => this.WriteData(832UL, value, 0);
            }

            public double Interception
            {
                get => this.ReadDataDouble(896UL, 0);
                set => this.WriteData(896UL, value, 0);
            }

            public double VcKcFactor
            {
                get => this.ReadDataDouble(960UL, 0.6);
                set => this.WriteData(960UL, value, 0.6);
            }

            public ListOfPrimitivesSerializer<double> Lambda
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(12);
                set => Link(12, value);
            }

            public double LambdaReduced
            {
                get => this.ReadDataDouble(1024UL, 0);
                set => this.WriteData(1024UL, value, 0);
            }

            public double VsLatitude
            {
                get => this.ReadDataDouble(1088UL, 0);
                set => this.WriteData(1088UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> LayerThickness
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(13);
                set => Link(13, value);
            }

            public double PmLayerThickness
            {
                get => this.ReadDataDouble(1152UL, 0);
                set => this.WriteData(1152UL, value, 0);
            }

            public double PmLeachingDepth
            {
                get => this.ReadDataDouble(1216UL, 0);
                set => this.WriteData(1216UL, value, 0);
            }

            public ushort PmLeachingDepthLayer
            {
                get => this.ReadDataUShort(240UL, (ushort)0);
                set => this.WriteData(240UL, value, (ushort)0);
            }

            public double VwMaxAirTemperature
            {
                get => this.ReadDataDouble(1280UL, 0);
                set => this.WriteData(1280UL, value, 0);
            }

            public double PmMaxPercolationRate
            {
                get => this.ReadDataDouble(1344UL, 0);
                set => this.WriteData(1344UL, value, 0);
            }

            public double VwMeanAirTemperature
            {
                get => this.ReadDataDouble(1408UL, 0);
                set => this.WriteData(1408UL, value, 0);
            }

            public double VwMinAirTemperature
            {
                get => this.ReadDataDouble(1472UL, 0);
                set => this.WriteData(1472UL, value, 0);
            }

            public double VcNetPrecipitation
            {
                get => this.ReadDataDouble(1536UL, 0);
                set => this.WriteData(1536UL, value, 0);
            }

            public double VwNetRadiation
            {
                get => this.ReadDataDouble(1600UL, 0);
                set => this.WriteData(1600UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> PermanentWiltingPoint
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(14);
                set => Link(14, value);
            }

            public double VcPercentageSoilCoverage
            {
                get => this.ReadDataDouble(1664UL, 0);
                set => this.WriteData(1664UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> PercolationRate
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(15);
                set => Link(15, value);
            }

            public double VwPrecipitation
            {
                get => this.ReadDataDouble(1728UL, 0);
                set => this.WriteData(1728UL, value, 0);
            }

            public double ReferenceEvapotranspiration
            {
                get => this.ReadDataDouble(1792UL, 6);
                set => this.WriteData(1792UL, value, 6);
            }

            public double RelativeHumidity
            {
                get => this.ReadDataDouble(1856UL, 0);
                set => this.WriteData(1856UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> ResidualEvapotranspiration
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(16);
                set => Link(16, value);
            }

            public ListOfPrimitivesSerializer<double> SaturatedHydraulicConductivity
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(17);
                set => Link(17, value);
            }

            public ListOfPrimitivesSerializer<double> SoilMoisture
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(18);
                set => Link(18, value);
            }

            public double SoilMoisturecrit
            {
                get => this.ReadDataDouble(1920UL, 0);
                set => this.WriteData(1920UL, value, 0);
            }

            public double SoilMoistureDeficit
            {
                get => this.ReadDataDouble(1984UL, 0);
                set => this.WriteData(1984UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> SoilPoreVolume
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(19);
                set => Link(19, value);
            }

            public double VcStomataResistance
            {
                get => this.ReadDataDouble(2048UL, 0);
                set => this.WriteData(2048UL, value, 0);
            }

            public double SurfaceRoughness
            {
                get => this.ReadDataDouble(2112UL, 0);
                set => this.WriteData(2112UL, value, 0);
            }

            public double SurfaceRunOff
            {
                get => this.ReadDataDouble(2176UL, 0);
                set => this.WriteData(2176UL, value, 0);
            }

            public double SumSurfaceRunOff
            {
                get => this.ReadDataDouble(2240UL, 0);
                set => this.WriteData(2240UL, value, 0);
            }

            public double SurfaceWaterStorage
            {
                get => this.ReadDataDouble(2304UL, 0);
                set => this.WriteData(2304UL, value, 0);
            }

            public double PtTimeStep
            {
                get => this.ReadDataDouble(2368UL, 0);
                set => this.WriteData(2368UL, value, 0);
            }

            public double TotalWaterRemoval
            {
                get => this.ReadDataDouble(2432UL, 0);
                set => this.WriteData(2432UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> Transpiration
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(20);
                set => Link(20, value);
            }

            public double TranspirationDeficit
            {
                get => this.ReadDataDouble(2496UL, 0);
                set => this.WriteData(2496UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> WaterFlux
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(21);
                set => Link(21, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd594e64f6b5f461dUL)]
    public class SoilOrganicModuleState : ICapnpSerializable
    {
        public const UInt64 typeId = 0xd594e64f6b5f461dUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Incorporation = reader.Incorporation;
            TotalDenitrification = reader.TotalDenitrification;
            ModuleParams = CapnpSerializable.Create<Mas.Schema.Model.Monica.SoilOrganicModuleParameters>(reader.ModuleParams);
            VsNumberOfLayers = reader.VsNumberOfLayers;
            VsNumberOfOrganicLayers = reader.VsNumberOfOrganicLayers;
            AddedOrganicMatter = reader.AddedOrganicMatter;
            IrrigationAmount = reader.IrrigationAmount;
            ActAmmoniaOxidationRate = reader.ActAmmoniaOxidationRate;
            ActNitrificationRate = reader.ActNitrificationRate;
            ActDenitrificationRate = reader.ActDenitrificationRate;
            AomFastDeltaSum = reader.AomFastDeltaSum;
            AomFastInput = reader.AomFastInput;
            AomFastSum = reader.AomFastSum;
            AomSlowDeltaSum = reader.AomSlowDeltaSum;
            AomSlowInput = reader.AomSlowInput;
            AomSlowSum = reader.AomSlowSum;
            CBalance = reader.CBalance;
            DecomposerRespiration = reader.DecomposerRespiration;
            ErrorMessage = reader.ErrorMessage;
            InertSoilOrganicC = reader.InertSoilOrganicC;
            N2oProduced = reader.N2oProduced;
            N2oProducedNit = reader.N2oProducedNit;
            N2oProducedDenit = reader.N2oProducedDenit;
            NetEcosystemExchange = reader.NetEcosystemExchange;
            NetEcosystemProduction = reader.NetEcosystemProduction;
            NetNMineralisation = reader.NetNMineralisation;
            NetNMineralisationRate = reader.NetNMineralisationRate;
            TotalNH3Volatilised = reader.TotalNH3Volatilised;
            Nh3Volatilised = reader.Nh3Volatilised;
            SmbCO2EvolutionRate = reader.SmbCO2EvolutionRate;
            SmbFastDelta = reader.SmbFastDelta;
            SmbSlowDelta = reader.SmbSlowDelta;
            VsSoilMineralNContent = reader.VsSoilMineralNContent;
            SoilOrganicC = reader.SoilOrganicC;
            SomFastDelta = reader.SomFastDelta;
            SomFastInput = reader.SomFastInput;
            SomSlowDelta = reader.SomSlowDelta;
            SumDenitrification = reader.SumDenitrification;
            SumNetNMineralisation = reader.SumNetNMineralisation;
            SumN2OProduced = reader.SumN2OProduced;
            SumNH3Volatilised = reader.SumNH3Volatilised;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Incorporation = Incorporation;
            writer.TotalDenitrification = TotalDenitrification;
            ModuleParams?.serialize(writer.ModuleParams);
            writer.VsNumberOfLayers = VsNumberOfLayers;
            writer.VsNumberOfOrganicLayers = VsNumberOfOrganicLayers;
            writer.AddedOrganicMatter = AddedOrganicMatter;
            writer.IrrigationAmount = IrrigationAmount;
            writer.ActAmmoniaOxidationRate.Init(ActAmmoniaOxidationRate);
            writer.ActNitrificationRate.Init(ActNitrificationRate);
            writer.ActDenitrificationRate.Init(ActDenitrificationRate);
            writer.AomFastDeltaSum.Init(AomFastDeltaSum);
            writer.AomFastInput.Init(AomFastInput);
            writer.AomFastSum.Init(AomFastSum);
            writer.AomSlowDeltaSum.Init(AomSlowDeltaSum);
            writer.AomSlowInput.Init(AomSlowInput);
            writer.AomSlowSum.Init(AomSlowSum);
            writer.CBalance.Init(CBalance);
            writer.DecomposerRespiration = DecomposerRespiration;
            writer.ErrorMessage = ErrorMessage;
            writer.InertSoilOrganicC.Init(InertSoilOrganicC);
            writer.N2oProduced = N2oProduced;
            writer.N2oProducedNit = N2oProducedNit;
            writer.N2oProducedDenit = N2oProducedDenit;
            writer.NetEcosystemExchange = NetEcosystemExchange;
            writer.NetEcosystemProduction = NetEcosystemProduction;
            writer.NetNMineralisation = NetNMineralisation;
            writer.NetNMineralisationRate.Init(NetNMineralisationRate);
            writer.TotalNH3Volatilised = TotalNH3Volatilised;
            writer.Nh3Volatilised = Nh3Volatilised;
            writer.SmbCO2EvolutionRate.Init(SmbCO2EvolutionRate);
            writer.SmbFastDelta.Init(SmbFastDelta);
            writer.SmbSlowDelta.Init(SmbSlowDelta);
            writer.VsSoilMineralNContent.Init(VsSoilMineralNContent);
            writer.SoilOrganicC.Init(SoilOrganicC);
            writer.SomFastDelta.Init(SomFastDelta);
            writer.SomFastInput.Init(SomFastInput);
            writer.SomSlowDelta.Init(SomSlowDelta);
            writer.SumDenitrification = SumDenitrification;
            writer.SumNetNMineralisation = SumNetNMineralisation;
            writer.SumN2OProduced = SumN2OProduced;
            writer.SumNH3Volatilised = SumNH3Volatilised;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public bool Incorporation
        {
            get;
            set;
        }

        public double TotalDenitrification
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.SoilOrganicModuleParameters ModuleParams
        {
            get;
            set;
        }

        public ushort VsNumberOfLayers
        {
            get;
            set;
        }

        public ushort VsNumberOfOrganicLayers
        {
            get;
            set;
        }

        public bool AddedOrganicMatter
        {
            get;
            set;
        }

        public double IrrigationAmount
        {
            get;
            set;
        }

        public IReadOnlyList<double> ActAmmoniaOxidationRate
        {
            get;
            set;
        }

        public IReadOnlyList<double> ActNitrificationRate
        {
            get;
            set;
        }

        public IReadOnlyList<double> ActDenitrificationRate
        {
            get;
            set;
        }

        public IReadOnlyList<double> AomFastDeltaSum
        {
            get;
            set;
        }

        public IReadOnlyList<double> AomFastInput
        {
            get;
            set;
        }

        public IReadOnlyList<double> AomFastSum
        {
            get;
            set;
        }

        public IReadOnlyList<double> AomSlowDeltaSum
        {
            get;
            set;
        }

        public IReadOnlyList<double> AomSlowInput
        {
            get;
            set;
        }

        public IReadOnlyList<double> AomSlowSum
        {
            get;
            set;
        }

        public IReadOnlyList<double> CBalance
        {
            get;
            set;
        }

        public double DecomposerRespiration
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public IReadOnlyList<double> InertSoilOrganicC
        {
            get;
            set;
        }

        public double N2oProduced
        {
            get;
            set;
        }

        public double N2oProducedNit
        {
            get;
            set;
        }

        public double N2oProducedDenit
        {
            get;
            set;
        }

        public double NetEcosystemExchange
        {
            get;
            set;
        }

        public double NetEcosystemProduction
        {
            get;
            set;
        }

        public double NetNMineralisation
        {
            get;
            set;
        }

        public IReadOnlyList<double> NetNMineralisationRate
        {
            get;
            set;
        }

        public double TotalNH3Volatilised
        {
            get;
            set;
        }

        public double Nh3Volatilised
        {
            get;
            set;
        }

        public IReadOnlyList<double> SmbCO2EvolutionRate
        {
            get;
            set;
        }

        public IReadOnlyList<double> SmbFastDelta
        {
            get;
            set;
        }

        public IReadOnlyList<double> SmbSlowDelta
        {
            get;
            set;
        }

        public IReadOnlyList<double> VsSoilMineralNContent
        {
            get;
            set;
        }

        public IReadOnlyList<double> SoilOrganicC
        {
            get;
            set;
        }

        public IReadOnlyList<double> SomFastDelta
        {
            get;
            set;
        }

        public IReadOnlyList<double> SomFastInput
        {
            get;
            set;
        }

        public IReadOnlyList<double> SomSlowDelta
        {
            get;
            set;
        }

        public double SumDenitrification
        {
            get;
            set;
        }

        public double SumNetNMineralisation
        {
            get;
            set;
        }

        public double SumN2OProduced
        {
            get;
            set;
        }

        public double SumNH3Volatilised
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
            public bool Incorporation => ctx.ReadDataBool(0UL, false);
            public double TotalDenitrification => ctx.ReadDataDouble(64UL, 0);
            public Mas.Schema.Model.Monica.SoilOrganicModuleParameters.READER ModuleParams => ctx.ReadStruct(0, Mas.Schema.Model.Monica.SoilOrganicModuleParameters.READER.create);
            public bool HasModuleParams => ctx.IsStructFieldNonNull(0);
            public ushort VsNumberOfLayers => ctx.ReadDataUShort(16UL, (ushort)0);
            public ushort VsNumberOfOrganicLayers => ctx.ReadDataUShort(32UL, (ushort)0);
            public bool AddedOrganicMatter => ctx.ReadDataBool(1UL, false);
            public double IrrigationAmount => ctx.ReadDataDouble(128UL, 0);
            public IReadOnlyList<double> ActAmmoniaOxidationRate => ctx.ReadList(1).CastDouble();
            public bool HasActAmmoniaOxidationRate => ctx.IsStructFieldNonNull(1);
            public IReadOnlyList<double> ActNitrificationRate => ctx.ReadList(2).CastDouble();
            public bool HasActNitrificationRate => ctx.IsStructFieldNonNull(2);
            public IReadOnlyList<double> ActDenitrificationRate => ctx.ReadList(3).CastDouble();
            public bool HasActDenitrificationRate => ctx.IsStructFieldNonNull(3);
            public IReadOnlyList<double> AomFastDeltaSum => ctx.ReadList(4).CastDouble();
            public bool HasAomFastDeltaSum => ctx.IsStructFieldNonNull(4);
            public IReadOnlyList<double> AomFastInput => ctx.ReadList(5).CastDouble();
            public bool HasAomFastInput => ctx.IsStructFieldNonNull(5);
            public IReadOnlyList<double> AomFastSum => ctx.ReadList(6).CastDouble();
            public bool HasAomFastSum => ctx.IsStructFieldNonNull(6);
            public IReadOnlyList<double> AomSlowDeltaSum => ctx.ReadList(7).CastDouble();
            public bool HasAomSlowDeltaSum => ctx.IsStructFieldNonNull(7);
            public IReadOnlyList<double> AomSlowInput => ctx.ReadList(8).CastDouble();
            public bool HasAomSlowInput => ctx.IsStructFieldNonNull(8);
            public IReadOnlyList<double> AomSlowSum => ctx.ReadList(9).CastDouble();
            public bool HasAomSlowSum => ctx.IsStructFieldNonNull(9);
            public IReadOnlyList<double> CBalance => ctx.ReadList(10).CastDouble();
            public bool HasCBalance => ctx.IsStructFieldNonNull(10);
            public double DecomposerRespiration => ctx.ReadDataDouble(192UL, 0);
            public string ErrorMessage => ctx.ReadText(11, null);
            public IReadOnlyList<double> InertSoilOrganicC => ctx.ReadList(12).CastDouble();
            public bool HasInertSoilOrganicC => ctx.IsStructFieldNonNull(12);
            public double N2oProduced => ctx.ReadDataDouble(256UL, 0);
            public double N2oProducedNit => ctx.ReadDataDouble(320UL, 0);
            public double N2oProducedDenit => ctx.ReadDataDouble(384UL, 0);
            public double NetEcosystemExchange => ctx.ReadDataDouble(448UL, 0);
            public double NetEcosystemProduction => ctx.ReadDataDouble(512UL, 0);
            public double NetNMineralisation => ctx.ReadDataDouble(576UL, 0);
            public IReadOnlyList<double> NetNMineralisationRate => ctx.ReadList(13).CastDouble();
            public bool HasNetNMineralisationRate => ctx.IsStructFieldNonNull(13);
            public double TotalNH3Volatilised => ctx.ReadDataDouble(640UL, 0);
            public double Nh3Volatilised => ctx.ReadDataDouble(704UL, 0);
            public IReadOnlyList<double> SmbCO2EvolutionRate => ctx.ReadList(14).CastDouble();
            public bool HasSmbCO2EvolutionRate => ctx.IsStructFieldNonNull(14);
            public IReadOnlyList<double> SmbFastDelta => ctx.ReadList(15).CastDouble();
            public bool HasSmbFastDelta => ctx.IsStructFieldNonNull(15);
            public IReadOnlyList<double> SmbSlowDelta => ctx.ReadList(16).CastDouble();
            public bool HasSmbSlowDelta => ctx.IsStructFieldNonNull(16);
            public IReadOnlyList<double> VsSoilMineralNContent => ctx.ReadList(17).CastDouble();
            public bool HasVsSoilMineralNContent => ctx.IsStructFieldNonNull(17);
            public IReadOnlyList<double> SoilOrganicC => ctx.ReadList(18).CastDouble();
            public bool HasSoilOrganicC => ctx.IsStructFieldNonNull(18);
            public IReadOnlyList<double> SomFastDelta => ctx.ReadList(19).CastDouble();
            public bool HasSomFastDelta => ctx.IsStructFieldNonNull(19);
            public IReadOnlyList<double> SomFastInput => ctx.ReadList(20).CastDouble();
            public bool HasSomFastInput => ctx.IsStructFieldNonNull(20);
            public IReadOnlyList<double> SomSlowDelta => ctx.ReadList(21).CastDouble();
            public bool HasSomSlowDelta => ctx.IsStructFieldNonNull(21);
            public double SumDenitrification => ctx.ReadDataDouble(768UL, 0);
            public double SumNetNMineralisation => ctx.ReadDataDouble(832UL, 0);
            public double SumN2OProduced => ctx.ReadDataDouble(896UL, 0);
            public double SumNH3Volatilised => ctx.ReadDataDouble(960UL, 0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(16, 22);
            }

            public bool Incorporation
            {
                get => this.ReadDataBool(0UL, false);
                set => this.WriteData(0UL, value, false);
            }

            public double TotalDenitrification
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public Mas.Schema.Model.Monica.SoilOrganicModuleParameters.WRITER ModuleParams
            {
                get => BuildPointer<Mas.Schema.Model.Monica.SoilOrganicModuleParameters.WRITER>(0);
                set => Link(0, value);
            }

            public ushort VsNumberOfLayers
            {
                get => this.ReadDataUShort(16UL, (ushort)0);
                set => this.WriteData(16UL, value, (ushort)0);
            }

            public ushort VsNumberOfOrganicLayers
            {
                get => this.ReadDataUShort(32UL, (ushort)0);
                set => this.WriteData(32UL, value, (ushort)0);
            }

            public bool AddedOrganicMatter
            {
                get => this.ReadDataBool(1UL, false);
                set => this.WriteData(1UL, value, false);
            }

            public double IrrigationAmount
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> ActAmmoniaOxidationRate
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(1);
                set => Link(1, value);
            }

            public ListOfPrimitivesSerializer<double> ActNitrificationRate
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(2);
                set => Link(2, value);
            }

            public ListOfPrimitivesSerializer<double> ActDenitrificationRate
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(3);
                set => Link(3, value);
            }

            public ListOfPrimitivesSerializer<double> AomFastDeltaSum
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(4);
                set => Link(4, value);
            }

            public ListOfPrimitivesSerializer<double> AomFastInput
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(5);
                set => Link(5, value);
            }

            public ListOfPrimitivesSerializer<double> AomFastSum
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(6);
                set => Link(6, value);
            }

            public ListOfPrimitivesSerializer<double> AomSlowDeltaSum
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(7);
                set => Link(7, value);
            }

            public ListOfPrimitivesSerializer<double> AomSlowInput
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(8);
                set => Link(8, value);
            }

            public ListOfPrimitivesSerializer<double> AomSlowSum
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(9);
                set => Link(9, value);
            }

            public ListOfPrimitivesSerializer<double> CBalance
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(10);
                set => Link(10, value);
            }

            public double DecomposerRespiration
            {
                get => this.ReadDataDouble(192UL, 0);
                set => this.WriteData(192UL, value, 0);
            }

            public string ErrorMessage
            {
                get => this.ReadText(11, null);
                set => this.WriteText(11, value, null);
            }

            public ListOfPrimitivesSerializer<double> InertSoilOrganicC
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(12);
                set => Link(12, value);
            }

            public double N2oProduced
            {
                get => this.ReadDataDouble(256UL, 0);
                set => this.WriteData(256UL, value, 0);
            }

            public double N2oProducedNit
            {
                get => this.ReadDataDouble(320UL, 0);
                set => this.WriteData(320UL, value, 0);
            }

            public double N2oProducedDenit
            {
                get => this.ReadDataDouble(384UL, 0);
                set => this.WriteData(384UL, value, 0);
            }

            public double NetEcosystemExchange
            {
                get => this.ReadDataDouble(448UL, 0);
                set => this.WriteData(448UL, value, 0);
            }

            public double NetEcosystemProduction
            {
                get => this.ReadDataDouble(512UL, 0);
                set => this.WriteData(512UL, value, 0);
            }

            public double NetNMineralisation
            {
                get => this.ReadDataDouble(576UL, 0);
                set => this.WriteData(576UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> NetNMineralisationRate
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(13);
                set => Link(13, value);
            }

            public double TotalNH3Volatilised
            {
                get => this.ReadDataDouble(640UL, 0);
                set => this.WriteData(640UL, value, 0);
            }

            public double Nh3Volatilised
            {
                get => this.ReadDataDouble(704UL, 0);
                set => this.WriteData(704UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> SmbCO2EvolutionRate
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(14);
                set => Link(14, value);
            }

            public ListOfPrimitivesSerializer<double> SmbFastDelta
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(15);
                set => Link(15, value);
            }

            public ListOfPrimitivesSerializer<double> SmbSlowDelta
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(16);
                set => Link(16, value);
            }

            public ListOfPrimitivesSerializer<double> VsSoilMineralNContent
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(17);
                set => Link(17, value);
            }

            public ListOfPrimitivesSerializer<double> SoilOrganicC
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(18);
                set => Link(18, value);
            }

            public ListOfPrimitivesSerializer<double> SomFastDelta
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(19);
                set => Link(19, value);
            }

            public ListOfPrimitivesSerializer<double> SomFastInput
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(20);
                set => Link(20, value);
            }

            public ListOfPrimitivesSerializer<double> SomSlowDelta
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(21);
                set => Link(21, value);
            }

            public double SumDenitrification
            {
                get => this.ReadDataDouble(768UL, 0);
                set => this.WriteData(768UL, value, 0);
            }

            public double SumNetNMineralisation
            {
                get => this.ReadDataDouble(832UL, 0);
                set => this.WriteData(832UL, value, 0);
            }

            public double SumN2OProduced
            {
                get => this.ReadDataDouble(896UL, 0);
                set => this.WriteData(896UL, value, 0);
            }

            public double SumNH3Volatilised
            {
                get => this.ReadDataDouble(960UL, 0);
                set => this.WriteData(960UL, value, 0);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbd3e199eb9b03758UL)]
    public class SoilTemperatureModuleState : ICapnpSerializable
    {
        public const UInt64 typeId = 0xbd3e199eb9b03758UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            SoilSurfaceTemperature = reader.SoilSurfaceTemperature;
            ModuleParams = CapnpSerializable.Create<Mas.Schema.Model.Monica.SoilTemperatureModuleParameters>(reader.ModuleParams);
            DampingFactor = reader.DampingFactor;
            SoilColumnVtGroundLayer = CapnpSerializable.Create<Mas.Schema.Model.Monica.SoilLayerState>(reader.SoilColumnVtGroundLayer);
            SoilColumnVtBottomLayer = CapnpSerializable.Create<Mas.Schema.Model.Monica.SoilLayerState>(reader.SoilColumnVtBottomLayer);
            NumberOfLayers = reader.NumberOfLayers;
            VsNumberOfLayers = reader.VsNumberOfLayers;
            VsSoilMoistureConst = reader.VsSoilMoistureConst;
            SoilTemperature = reader.SoilTemperature;
            V = reader.V;
            VolumeMatrix = reader.VolumeMatrix;
            VolumeMatrixOld = reader.VolumeMatrixOld;
            B = reader.B;
            MatrixPrimaryDiagonal = reader.MatrixPrimaryDiagonal;
            MatrixSecundaryDiagonal = reader.MatrixSecundaryDiagonal;
            HeatFlow = reader.HeatFlow;
            HeatConductivity = reader.HeatConductivity;
            HeatConductivityMean = reader.HeatConductivityMean;
            HeatCapacity = reader.HeatCapacity;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.SoilSurfaceTemperature = SoilSurfaceTemperature;
            ModuleParams?.serialize(writer.ModuleParams);
            writer.DampingFactor = DampingFactor;
            SoilColumnVtGroundLayer?.serialize(writer.SoilColumnVtGroundLayer);
            SoilColumnVtBottomLayer?.serialize(writer.SoilColumnVtBottomLayer);
            writer.NumberOfLayers = NumberOfLayers;
            writer.VsNumberOfLayers = VsNumberOfLayers;
            writer.VsSoilMoistureConst.Init(VsSoilMoistureConst);
            writer.SoilTemperature.Init(SoilTemperature);
            writer.V.Init(V);
            writer.VolumeMatrix.Init(VolumeMatrix);
            writer.VolumeMatrixOld.Init(VolumeMatrixOld);
            writer.B.Init(B);
            writer.MatrixPrimaryDiagonal.Init(MatrixPrimaryDiagonal);
            writer.MatrixSecundaryDiagonal.Init(MatrixSecundaryDiagonal);
            writer.HeatFlow = HeatFlow;
            writer.HeatConductivity.Init(HeatConductivity);
            writer.HeatConductivityMean.Init(HeatConductivityMean);
            writer.HeatCapacity.Init(HeatCapacity);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public double SoilSurfaceTemperature
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.SoilTemperatureModuleParameters ModuleParams
        {
            get;
            set;
        }

        public double DampingFactor
        {
            get;
            set;
        }

        = 0.8;
        public Mas.Schema.Model.Monica.SoilLayerState SoilColumnVtGroundLayer
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.SoilLayerState SoilColumnVtBottomLayer
        {
            get;
            set;
        }

        public ushort NumberOfLayers
        {
            get;
            set;
        }

        public ushort VsNumberOfLayers
        {
            get;
            set;
        }

        public IReadOnlyList<double> VsSoilMoistureConst
        {
            get;
            set;
        }

        public IReadOnlyList<double> SoilTemperature
        {
            get;
            set;
        }

        public IReadOnlyList<double> V
        {
            get;
            set;
        }

        public IReadOnlyList<double> VolumeMatrix
        {
            get;
            set;
        }

        public IReadOnlyList<double> VolumeMatrixOld
        {
            get;
            set;
        }

        public IReadOnlyList<double> B
        {
            get;
            set;
        }

        public IReadOnlyList<double> MatrixPrimaryDiagonal
        {
            get;
            set;
        }

        public IReadOnlyList<double> MatrixSecundaryDiagonal
        {
            get;
            set;
        }

        public double HeatFlow
        {
            get;
            set;
        }

        public IReadOnlyList<double> HeatConductivity
        {
            get;
            set;
        }

        public IReadOnlyList<double> HeatConductivityMean
        {
            get;
            set;
        }

        public IReadOnlyList<double> HeatCapacity
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
            public double SoilSurfaceTemperature => ctx.ReadDataDouble(0UL, 0);
            public Mas.Schema.Model.Monica.SoilTemperatureModuleParameters.READER ModuleParams => ctx.ReadStruct(0, Mas.Schema.Model.Monica.SoilTemperatureModuleParameters.READER.create);
            public bool HasModuleParams => ctx.IsStructFieldNonNull(0);
            public double DampingFactor => ctx.ReadDataDouble(64UL, 0.8);
            public Mas.Schema.Model.Monica.SoilLayerState.READER SoilColumnVtGroundLayer => ctx.ReadStruct(1, Mas.Schema.Model.Monica.SoilLayerState.READER.create);
            public bool HasSoilColumnVtGroundLayer => ctx.IsStructFieldNonNull(1);
            public Mas.Schema.Model.Monica.SoilLayerState.READER SoilColumnVtBottomLayer => ctx.ReadStruct(2, Mas.Schema.Model.Monica.SoilLayerState.READER.create);
            public bool HasSoilColumnVtBottomLayer => ctx.IsStructFieldNonNull(2);
            public ushort NumberOfLayers => ctx.ReadDataUShort(128UL, (ushort)0);
            public ushort VsNumberOfLayers => ctx.ReadDataUShort(144UL, (ushort)0);
            public IReadOnlyList<double> VsSoilMoistureConst => ctx.ReadList(3).CastDouble();
            public bool HasVsSoilMoistureConst => ctx.IsStructFieldNonNull(3);
            public IReadOnlyList<double> SoilTemperature => ctx.ReadList(4).CastDouble();
            public bool HasSoilTemperature => ctx.IsStructFieldNonNull(4);
            public IReadOnlyList<double> V => ctx.ReadList(5).CastDouble();
            public bool HasV => ctx.IsStructFieldNonNull(5);
            public IReadOnlyList<double> VolumeMatrix => ctx.ReadList(6).CastDouble();
            public bool HasVolumeMatrix => ctx.IsStructFieldNonNull(6);
            public IReadOnlyList<double> VolumeMatrixOld => ctx.ReadList(7).CastDouble();
            public bool HasVolumeMatrixOld => ctx.IsStructFieldNonNull(7);
            public IReadOnlyList<double> B => ctx.ReadList(8).CastDouble();
            public bool HasB => ctx.IsStructFieldNonNull(8);
            public IReadOnlyList<double> MatrixPrimaryDiagonal => ctx.ReadList(9).CastDouble();
            public bool HasMatrixPrimaryDiagonal => ctx.IsStructFieldNonNull(9);
            public IReadOnlyList<double> MatrixSecundaryDiagonal => ctx.ReadList(10).CastDouble();
            public bool HasMatrixSecundaryDiagonal => ctx.IsStructFieldNonNull(10);
            public double HeatFlow => ctx.ReadDataDouble(192UL, 0);
            public IReadOnlyList<double> HeatConductivity => ctx.ReadList(11).CastDouble();
            public bool HasHeatConductivity => ctx.IsStructFieldNonNull(11);
            public IReadOnlyList<double> HeatConductivityMean => ctx.ReadList(12).CastDouble();
            public bool HasHeatConductivityMean => ctx.IsStructFieldNonNull(12);
            public IReadOnlyList<double> HeatCapacity => ctx.ReadList(13).CastDouble();
            public bool HasHeatCapacity => ctx.IsStructFieldNonNull(13);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(4, 14);
            }

            public double SoilSurfaceTemperature
            {
                get => this.ReadDataDouble(0UL, 0);
                set => this.WriteData(0UL, value, 0);
            }

            public Mas.Schema.Model.Monica.SoilTemperatureModuleParameters.WRITER ModuleParams
            {
                get => BuildPointer<Mas.Schema.Model.Monica.SoilTemperatureModuleParameters.WRITER>(0);
                set => Link(0, value);
            }

            public double DampingFactor
            {
                get => this.ReadDataDouble(64UL, 0.8);
                set => this.WriteData(64UL, value, 0.8);
            }

            public Mas.Schema.Model.Monica.SoilLayerState.WRITER SoilColumnVtGroundLayer
            {
                get => BuildPointer<Mas.Schema.Model.Monica.SoilLayerState.WRITER>(1);
                set => Link(1, value);
            }

            public Mas.Schema.Model.Monica.SoilLayerState.WRITER SoilColumnVtBottomLayer
            {
                get => BuildPointer<Mas.Schema.Model.Monica.SoilLayerState.WRITER>(2);
                set => Link(2, value);
            }

            public ushort NumberOfLayers
            {
                get => this.ReadDataUShort(128UL, (ushort)0);
                set => this.WriteData(128UL, value, (ushort)0);
            }

            public ushort VsNumberOfLayers
            {
                get => this.ReadDataUShort(144UL, (ushort)0);
                set => this.WriteData(144UL, value, (ushort)0);
            }

            public ListOfPrimitivesSerializer<double> VsSoilMoistureConst
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(3);
                set => Link(3, value);
            }

            public ListOfPrimitivesSerializer<double> SoilTemperature
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(4);
                set => Link(4, value);
            }

            public ListOfPrimitivesSerializer<double> V
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(5);
                set => Link(5, value);
            }

            public ListOfPrimitivesSerializer<double> VolumeMatrix
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(6);
                set => Link(6, value);
            }

            public ListOfPrimitivesSerializer<double> VolumeMatrixOld
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(7);
                set => Link(7, value);
            }

            public ListOfPrimitivesSerializer<double> B
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(8);
                set => Link(8, value);
            }

            public ListOfPrimitivesSerializer<double> MatrixPrimaryDiagonal
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(9);
                set => Link(9, value);
            }

            public ListOfPrimitivesSerializer<double> MatrixSecundaryDiagonal
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(10);
                set => Link(10, value);
            }

            public double HeatFlow
            {
                get => this.ReadDataDouble(192UL, 0);
                set => this.WriteData(192UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> HeatConductivity
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(11);
                set => Link(11, value);
            }

            public ListOfPrimitivesSerializer<double> HeatConductivityMean
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(12);
                set => Link(12, value);
            }

            public ListOfPrimitivesSerializer<double> HeatCapacity
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(13);
                set => Link(13, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb1760f65e652e737UL)]
    public class SoilTransportModuleState : ICapnpSerializable
    {
        public const UInt64 typeId = 0xb1760f65e652e737UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            PcMinimumAvailableN = reader.PcMinimumAvailableN;
            ModuleParams = CapnpSerializable.Create<Mas.Schema.Model.Monica.SoilTransportModuleParameters>(reader.ModuleParams);
            Convection = reader.Convection;
            CropNUptake = reader.CropNUptake;
            DiffusionCoeff = reader.DiffusionCoeff;
            Dispersion = reader.Dispersion;
            DispersionCoeff = reader.DispersionCoeff;
            VsLeachingDepth = reader.VsLeachingDepth;
            LeachingAtBoundary = reader.LeachingAtBoundary;
            VsNDeposition = reader.VsNDeposition;
            VcNUptakeFromLayer = reader.VcNUptakeFromLayer;
            PoreWaterVelocity = reader.PoreWaterVelocity;
            VsSoilMineralNContent = reader.VsSoilMineralNContent;
            SoilNO3 = reader.SoilNO3;
            SoilNO3aq = reader.SoilNO3aq;
            TimeStep = reader.TimeStep;
            TotalDispersion = reader.TotalDispersion;
            PercolationRate = reader.PercolationRate;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.PcMinimumAvailableN = PcMinimumAvailableN;
            ModuleParams?.serialize(writer.ModuleParams);
            writer.Convection.Init(Convection);
            writer.CropNUptake = CropNUptake;
            writer.DiffusionCoeff.Init(DiffusionCoeff);
            writer.Dispersion.Init(Dispersion);
            writer.DispersionCoeff.Init(DispersionCoeff);
            writer.VsLeachingDepth = VsLeachingDepth;
            writer.LeachingAtBoundary = LeachingAtBoundary;
            writer.VsNDeposition = VsNDeposition;
            writer.VcNUptakeFromLayer.Init(VcNUptakeFromLayer);
            writer.PoreWaterVelocity.Init(PoreWaterVelocity);
            writer.VsSoilMineralNContent.Init(VsSoilMineralNContent);
            writer.SoilNO3.Init(SoilNO3);
            writer.SoilNO3aq.Init(SoilNO3aq);
            writer.TimeStep = TimeStep;
            writer.TotalDispersion.Init(TotalDispersion);
            writer.PercolationRate.Init(PercolationRate);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public double PcMinimumAvailableN
        {
            get;
            set;
        }

        public Mas.Schema.Model.Monica.SoilTransportModuleParameters ModuleParams
        {
            get;
            set;
        }

        public IReadOnlyList<double> Convection
        {
            get;
            set;
        }

        public double CropNUptake
        {
            get;
            set;
        }

        public IReadOnlyList<double> DiffusionCoeff
        {
            get;
            set;
        }

        public IReadOnlyList<double> Dispersion
        {
            get;
            set;
        }

        public IReadOnlyList<double> DispersionCoeff
        {
            get;
            set;
        }

        public double VsLeachingDepth
        {
            get;
            set;
        }

        public double LeachingAtBoundary
        {
            get;
            set;
        }

        public double VsNDeposition
        {
            get;
            set;
        }

        public IReadOnlyList<double> VcNUptakeFromLayer
        {
            get;
            set;
        }

        public IReadOnlyList<double> PoreWaterVelocity
        {
            get;
            set;
        }

        public IReadOnlyList<double> VsSoilMineralNContent
        {
            get;
            set;
        }

        public IReadOnlyList<double> SoilNO3
        {
            get;
            set;
        }

        public IReadOnlyList<double> SoilNO3aq
        {
            get;
            set;
        }

        public double TimeStep
        {
            get;
            set;
        }

        = 1;
        public IReadOnlyList<double> TotalDispersion
        {
            get;
            set;
        }

        public IReadOnlyList<double> PercolationRate
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
            public double PcMinimumAvailableN => ctx.ReadDataDouble(0UL, 0);
            public Mas.Schema.Model.Monica.SoilTransportModuleParameters.READER ModuleParams => ctx.ReadStruct(0, Mas.Schema.Model.Monica.SoilTransportModuleParameters.READER.create);
            public bool HasModuleParams => ctx.IsStructFieldNonNull(0);
            public IReadOnlyList<double> Convection => ctx.ReadList(1).CastDouble();
            public bool HasConvection => ctx.IsStructFieldNonNull(1);
            public double CropNUptake => ctx.ReadDataDouble(64UL, 0);
            public IReadOnlyList<double> DiffusionCoeff => ctx.ReadList(2).CastDouble();
            public bool HasDiffusionCoeff => ctx.IsStructFieldNonNull(2);
            public IReadOnlyList<double> Dispersion => ctx.ReadList(3).CastDouble();
            public bool HasDispersion => ctx.IsStructFieldNonNull(3);
            public IReadOnlyList<double> DispersionCoeff => ctx.ReadList(4).CastDouble();
            public bool HasDispersionCoeff => ctx.IsStructFieldNonNull(4);
            public double VsLeachingDepth => ctx.ReadDataDouble(128UL, 0);
            public double LeachingAtBoundary => ctx.ReadDataDouble(192UL, 0);
            public double VsNDeposition => ctx.ReadDataDouble(256UL, 0);
            public IReadOnlyList<double> VcNUptakeFromLayer => ctx.ReadList(5).CastDouble();
            public bool HasVcNUptakeFromLayer => ctx.IsStructFieldNonNull(5);
            public IReadOnlyList<double> PoreWaterVelocity => ctx.ReadList(6).CastDouble();
            public bool HasPoreWaterVelocity => ctx.IsStructFieldNonNull(6);
            public IReadOnlyList<double> VsSoilMineralNContent => ctx.ReadList(7).CastDouble();
            public bool HasVsSoilMineralNContent => ctx.IsStructFieldNonNull(7);
            public IReadOnlyList<double> SoilNO3 => ctx.ReadList(8).CastDouble();
            public bool HasSoilNO3 => ctx.IsStructFieldNonNull(8);
            public IReadOnlyList<double> SoilNO3aq => ctx.ReadList(9).CastDouble();
            public bool HasSoilNO3aq => ctx.IsStructFieldNonNull(9);
            public double TimeStep => ctx.ReadDataDouble(320UL, 1);
            public IReadOnlyList<double> TotalDispersion => ctx.ReadList(10).CastDouble();
            public bool HasTotalDispersion => ctx.IsStructFieldNonNull(10);
            public IReadOnlyList<double> PercolationRate => ctx.ReadList(11).CastDouble();
            public bool HasPercolationRate => ctx.IsStructFieldNonNull(11);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(6, 12);
            }

            public double PcMinimumAvailableN
            {
                get => this.ReadDataDouble(0UL, 0);
                set => this.WriteData(0UL, value, 0);
            }

            public Mas.Schema.Model.Monica.SoilTransportModuleParameters.WRITER ModuleParams
            {
                get => BuildPointer<Mas.Schema.Model.Monica.SoilTransportModuleParameters.WRITER>(0);
                set => Link(0, value);
            }

            public ListOfPrimitivesSerializer<double> Convection
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(1);
                set => Link(1, value);
            }

            public double CropNUptake
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> DiffusionCoeff
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(2);
                set => Link(2, value);
            }

            public ListOfPrimitivesSerializer<double> Dispersion
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(3);
                set => Link(3, value);
            }

            public ListOfPrimitivesSerializer<double> DispersionCoeff
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(4);
                set => Link(4, value);
            }

            public double VsLeachingDepth
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }

            public double LeachingAtBoundary
            {
                get => this.ReadDataDouble(192UL, 0);
                set => this.WriteData(192UL, value, 0);
            }

            public double VsNDeposition
            {
                get => this.ReadDataDouble(256UL, 0);
                set => this.WriteData(256UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> VcNUptakeFromLayer
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(5);
                set => Link(5, value);
            }

            public ListOfPrimitivesSerializer<double> PoreWaterVelocity
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(6);
                set => Link(6, value);
            }

            public ListOfPrimitivesSerializer<double> VsSoilMineralNContent
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(7);
                set => Link(7, value);
            }

            public ListOfPrimitivesSerializer<double> SoilNO3
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(8);
                set => Link(8, value);
            }

            public ListOfPrimitivesSerializer<double> SoilNO3aq
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(9);
                set => Link(9, value);
            }

            public double TimeStep
            {
                get => this.ReadDataDouble(320UL, 1);
                set => this.WriteData(320UL, value, 1);
            }

            public ListOfPrimitivesSerializer<double> TotalDispersion
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(10);
                set => Link(10, value);
            }

            public ListOfPrimitivesSerializer<double> PercolationRate
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(11);
                set => Link(11, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf03d8fd1bbe75519UL)]
    public class ICData : ICapnpSerializable
    {
        public const UInt64 typeId = 0xf03d8fd1bbe75519UL;
        public enum WHICH : ushort
        {
            NoCrop = 0,
            Height = 1,
            Lait = 2,
            undefined = 65535
        }

        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            switch (reader.which)
            {
                case WHICH.NoCrop:
                    which = reader.which;
                    break;
                case WHICH.Height:
                    Height = reader.Height;
                    break;
                case WHICH.Lait:
                    Lait = reader.Lait;
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
                    case WHICH.NoCrop:
                        break;
                    case WHICH.Height:
                        _content = 0;
                        break;
                    case WHICH.Lait:
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
                case WHICH.NoCrop:
                    break;
                case WHICH.Height:
                    writer.Height = Height.Value;
                    break;
                case WHICH.Lait:
                    writer.Lait = Lait.Value;
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

        public double? Height
        {
            get => _which == WHICH.Height ? (double? )_content : null;
            set
            {
                _which = WHICH.Height;
                _content = value;
            }
        }

        public double? Lait
        {
            get => _which == WHICH.Lait ? (double? )_content : null;
            set
            {
                _which = WHICH.Lait;
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
            public WHICH which => (WHICH)ctx.ReadDataUShort(0U, (ushort)0);
            public double Height => which == WHICH.Height ? ctx.ReadDataDouble(64UL, 0) : default;
            public double Lait => which == WHICH.Lait ? ctx.ReadDataDouble(64UL, 0) : default;
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(2, 0);
            }

            public WHICH which
            {
                get => (WHICH)this.ReadDataUShort(0U, (ushort)0);
                set => this.WriteData(0U, (ushort)value, (ushort)0);
            }

            public double Height
            {
                get => which == WHICH.Height ? this.ReadDataDouble(64UL, 0) : default;
                set => this.WriteData(64UL, value, 0);
            }

            public double Lait
            {
                get => which == WHICH.Lait ? this.ReadDataDouble(64UL, 0) : default;
                set => this.WriteData(64UL, value, 0);
            }
        }
    }
}