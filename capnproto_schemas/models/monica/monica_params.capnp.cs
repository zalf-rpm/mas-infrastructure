using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mas.Models.Monica
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa74f5574681f9d55UL)]
    public class CropSpec : ICapnpSerializable
    {
        public const UInt64 typeId = 0xa74f5574681f9d55UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            CropParams = CapnpSerializable.Create<Mas.Models.Monica.CropParameters>(reader.CropParams);
            ResidueParams = CapnpSerializable.Create<Mas.Models.Monica.CropResidueParameters>(reader.ResidueParams);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            CropParams?.serialize(writer.CropParams);
            ResidueParams?.serialize(writer.ResidueParams);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Models.Monica.CropParameters CropParams
        {
            get;
            set;
        }

        public Mas.Models.Monica.CropResidueParameters ResidueParams
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
            public Mas.Models.Monica.CropParameters.READER CropParams => ctx.ReadStruct(0, Mas.Models.Monica.CropParameters.READER.create);
            public Mas.Models.Monica.CropResidueParameters.READER ResidueParams => ctx.ReadStruct(1, Mas.Models.Monica.CropResidueParameters.READER.create);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 2);
            }

            public Mas.Models.Monica.CropParameters.WRITER CropParams
            {
                get => BuildPointer<Mas.Models.Monica.CropParameters.WRITER>(0);
                set => Link(0, value);
            }

            public Mas.Models.Monica.CropResidueParameters.WRITER ResidueParams
            {
                get => BuildPointer<Mas.Models.Monica.CropResidueParameters.WRITER>(1);
                set => Link(1, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8ac5cfb21988c168UL)]
    public class CropParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0x8ac5cfb21988c168UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            SpeciesParams = CapnpSerializable.Create<Mas.Models.Monica.SpeciesParameters>(reader.SpeciesParams);
            CultivarParams = CapnpSerializable.Create<Mas.Models.Monica.CultivarParameters>(reader.CultivarParams);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            SpeciesParams?.serialize(writer.SpeciesParams);
            CultivarParams?.serialize(writer.CultivarParams);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Models.Monica.SpeciesParameters SpeciesParams
        {
            get;
            set;
        }

        public Mas.Models.Monica.CultivarParameters CultivarParams
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
            public Mas.Models.Monica.SpeciesParameters.READER SpeciesParams => ctx.ReadStruct(0, Mas.Models.Monica.SpeciesParameters.READER.create);
            public Mas.Models.Monica.CultivarParameters.READER CultivarParams => ctx.ReadStruct(1, Mas.Models.Monica.CultivarParameters.READER.create);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 2);
            }

            public Mas.Models.Monica.SpeciesParameters.WRITER SpeciesParams
            {
                get => BuildPointer<Mas.Models.Monica.SpeciesParameters.WRITER>(0);
                set => Link(0, value);
            }

            public Mas.Models.Monica.CultivarParameters.WRITER CultivarParams
            {
                get => BuildPointer<Mas.Models.Monica.CultivarParameters.WRITER>(1);
                set => Link(1, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd2d587c796186e8bUL)]
    public class SpeciesParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0xd2d587c796186e8bUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            SpeciesId = reader.SpeciesId;
            CarboxylationPathway = reader.CarboxylationPathway;
            DefaultRadiationUseEfficiency = reader.DefaultRadiationUseEfficiency;
            PartBiologicalNFixation = reader.PartBiologicalNFixation;
            InitialKcFactor = reader.InitialKcFactor;
            LuxuryNCoeff = reader.LuxuryNCoeff;
            MaxCropDiameter = reader.MaxCropDiameter;
            StageAtMaxHeight = reader.StageAtMaxHeight;
            StageAtMaxDiameter = reader.StageAtMaxDiameter;
            MinimumNConcentration = reader.MinimumNConcentration;
            MinimumTemperatureForAssimilation = reader.MinimumTemperatureForAssimilation;
            OptimumTemperatureForAssimilation = reader.OptimumTemperatureForAssimilation;
            MaximumTemperatureForAssimilation = reader.MaximumTemperatureForAssimilation;
            NConcentrationAbovegroundBiomass = reader.NConcentrationAbovegroundBiomass;
            NConcentrationB0 = reader.NConcentrationB0;
            NConcentrationPN = reader.NConcentrationPN;
            NConcentrationRoot = reader.NConcentrationRoot;
            DevelopmentAccelerationByNitrogenStress = reader.DevelopmentAccelerationByNitrogenStress;
            FieldConditionModifier = reader.FieldConditionModifier;
            AssimilateReallocation = reader.AssimilateReallocation;
            BaseTemperature = reader.BaseTemperature;
            OrganMaintenanceRespiration = reader.OrganMaintenanceRespiration;
            OrganGrowthRespiration = reader.OrganGrowthRespiration;
            StageMaxRootNConcentration = reader.StageMaxRootNConcentration;
            InitialOrganBiomass = reader.InitialOrganBiomass;
            CriticalOxygenContent = reader.CriticalOxygenContent;
            StageMobilFromStorageCoeff = reader.StageMobilFromStorageCoeff;
            AbovegroundOrgan = reader.AbovegroundOrgan;
            StorageOrgan = reader.StorageOrgan;
            SamplingDepth = reader.SamplingDepth;
            TargetNSamplingDepth = reader.TargetNSamplingDepth;
            TargetN30 = reader.TargetN30;
            MaxNUptakeParam = reader.MaxNUptakeParam;
            RootDistributionParam = reader.RootDistributionParam;
            PlantDensity = reader.PlantDensity;
            RootGrowthLag = reader.RootGrowthLag;
            MinimumTemperatureRootGrowth = reader.MinimumTemperatureRootGrowth;
            InitialRootingDepth = reader.InitialRootingDepth;
            RootPenetrationRate = reader.RootPenetrationRate;
            RootFormFactor = reader.RootFormFactor;
            SpecificRootLength = reader.SpecificRootLength;
            StageAfterCut = reader.StageAfterCut;
            LimitingTemperatureHeatStress = reader.LimitingTemperatureHeatStress;
            CuttingDelayDays = reader.CuttingDelayDays;
            DroughtImpactOnFertilityFactor = reader.DroughtImpactOnFertilityFactor;
            EfMono = reader.EfMono;
            EfMonos = reader.EfMonos;
            EfIso = reader.EfIso;
            VcMax25 = reader.VcMax25;
            Aekc = reader.Aekc;
            Aeko = reader.Aeko;
            Aevc = reader.Aevc;
            Kc25 = reader.Kc25;
            Ko25 = reader.Ko25;
            TransitionStageLeafExp = reader.TransitionStageLeafExp;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.SpeciesId = SpeciesId;
            writer.CarboxylationPathway = CarboxylationPathway;
            writer.DefaultRadiationUseEfficiency = DefaultRadiationUseEfficiency;
            writer.PartBiologicalNFixation = PartBiologicalNFixation;
            writer.InitialKcFactor = InitialKcFactor;
            writer.LuxuryNCoeff = LuxuryNCoeff;
            writer.MaxCropDiameter = MaxCropDiameter;
            writer.StageAtMaxHeight = StageAtMaxHeight;
            writer.StageAtMaxDiameter = StageAtMaxDiameter;
            writer.MinimumNConcentration = MinimumNConcentration;
            writer.MinimumTemperatureForAssimilation = MinimumTemperatureForAssimilation;
            writer.OptimumTemperatureForAssimilation = OptimumTemperatureForAssimilation;
            writer.MaximumTemperatureForAssimilation = MaximumTemperatureForAssimilation;
            writer.NConcentrationAbovegroundBiomass = NConcentrationAbovegroundBiomass;
            writer.NConcentrationB0 = NConcentrationB0;
            writer.NConcentrationPN = NConcentrationPN;
            writer.NConcentrationRoot = NConcentrationRoot;
            writer.DevelopmentAccelerationByNitrogenStress = DevelopmentAccelerationByNitrogenStress;
            writer.FieldConditionModifier = FieldConditionModifier;
            writer.AssimilateReallocation = AssimilateReallocation;
            writer.BaseTemperature.Init(BaseTemperature);
            writer.OrganMaintenanceRespiration.Init(OrganMaintenanceRespiration);
            writer.OrganGrowthRespiration.Init(OrganGrowthRespiration);
            writer.StageMaxRootNConcentration.Init(StageMaxRootNConcentration);
            writer.InitialOrganBiomass.Init(InitialOrganBiomass);
            writer.CriticalOxygenContent.Init(CriticalOxygenContent);
            writer.StageMobilFromStorageCoeff.Init(StageMobilFromStorageCoeff);
            writer.AbovegroundOrgan.Init(AbovegroundOrgan);
            writer.StorageOrgan.Init(StorageOrgan);
            writer.SamplingDepth = SamplingDepth;
            writer.TargetNSamplingDepth = TargetNSamplingDepth;
            writer.TargetN30 = TargetN30;
            writer.MaxNUptakeParam = MaxNUptakeParam;
            writer.RootDistributionParam = RootDistributionParam;
            writer.PlantDensity = PlantDensity;
            writer.RootGrowthLag = RootGrowthLag;
            writer.MinimumTemperatureRootGrowth = MinimumTemperatureRootGrowth;
            writer.InitialRootingDepth = InitialRootingDepth;
            writer.RootPenetrationRate = RootPenetrationRate;
            writer.RootFormFactor = RootFormFactor;
            writer.SpecificRootLength = SpecificRootLength;
            writer.StageAfterCut = StageAfterCut;
            writer.LimitingTemperatureHeatStress = LimitingTemperatureHeatStress;
            writer.CuttingDelayDays = CuttingDelayDays;
            writer.DroughtImpactOnFertilityFactor = DroughtImpactOnFertilityFactor;
            writer.EfMono = EfMono;
            writer.EfMonos = EfMonos;
            writer.EfIso = EfIso;
            writer.VcMax25 = VcMax25;
            writer.Aekc = Aekc;
            writer.Aeko = Aeko;
            writer.Aevc = Aevc;
            writer.Kc25 = Kc25;
            writer.Ko25 = Ko25;
            writer.TransitionStageLeafExp = TransitionStageLeafExp;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public string SpeciesId
        {
            get;
            set;
        }

        public byte CarboxylationPathway
        {
            get;
            set;
        }

        public double DefaultRadiationUseEfficiency
        {
            get;
            set;
        }

        public double PartBiologicalNFixation
        {
            get;
            set;
        }

        public double InitialKcFactor
        {
            get;
            set;
        }

        public double LuxuryNCoeff
        {
            get;
            set;
        }

        public double MaxCropDiameter
        {
            get;
            set;
        }

        public double StageAtMaxHeight
        {
            get;
            set;
        }

        public double StageAtMaxDiameter
        {
            get;
            set;
        }

        public double MinimumNConcentration
        {
            get;
            set;
        }

        public double MinimumTemperatureForAssimilation
        {
            get;
            set;
        }

        public double OptimumTemperatureForAssimilation
        {
            get;
            set;
        }

        public double MaximumTemperatureForAssimilation
        {
            get;
            set;
        }

        public double NConcentrationAbovegroundBiomass
        {
            get;
            set;
        }

        public double NConcentrationB0
        {
            get;
            set;
        }

        public double NConcentrationPN
        {
            get;
            set;
        }

        public double NConcentrationRoot
        {
            get;
            set;
        }

        public ushort DevelopmentAccelerationByNitrogenStress
        {
            get;
            set;
        }

        public double FieldConditionModifier
        {
            get;
            set;
        }

        = 1;
        public double AssimilateReallocation
        {
            get;
            set;
        }

        public IReadOnlyList<double> BaseTemperature
        {
            get;
            set;
        }

        public IReadOnlyList<double> OrganMaintenanceRespiration
        {
            get;
            set;
        }

        public IReadOnlyList<double> OrganGrowthRespiration
        {
            get;
            set;
        }

        public IReadOnlyList<double> StageMaxRootNConcentration
        {
            get;
            set;
        }

        public IReadOnlyList<double> InitialOrganBiomass
        {
            get;
            set;
        }

        public IReadOnlyList<double> CriticalOxygenContent
        {
            get;
            set;
        }

        public IReadOnlyList<double> StageMobilFromStorageCoeff
        {
            get;
            set;
        }

        public IReadOnlyList<bool> AbovegroundOrgan
        {
            get;
            set;
        }

        public IReadOnlyList<bool> StorageOrgan
        {
            get;
            set;
        }

        public double SamplingDepth
        {
            get;
            set;
        }

        public double TargetNSamplingDepth
        {
            get;
            set;
        }

        public double TargetN30
        {
            get;
            set;
        }

        public double MaxNUptakeParam
        {
            get;
            set;
        }

        public double RootDistributionParam
        {
            get;
            set;
        }

        public ushort PlantDensity
        {
            get;
            set;
        }

        public double RootGrowthLag
        {
            get;
            set;
        }

        public double MinimumTemperatureRootGrowth
        {
            get;
            set;
        }

        public double InitialRootingDepth
        {
            get;
            set;
        }

        public double RootPenetrationRate
        {
            get;
            set;
        }

        public double RootFormFactor
        {
            get;
            set;
        }

        public double SpecificRootLength
        {
            get;
            set;
        }

        public ushort StageAfterCut
        {
            get;
            set;
        }

        public double LimitingTemperatureHeatStress
        {
            get;
            set;
        }

        public ushort CuttingDelayDays
        {
            get;
            set;
        }

        public double DroughtImpactOnFertilityFactor
        {
            get;
            set;
        }

        public double EfMono
        {
            get;
            set;
        }

        = 0.5;
        public double EfMonos
        {
            get;
            set;
        }

        = 0.5;
        public double EfIso
        {
            get;
            set;
        }

        public double VcMax25
        {
            get;
            set;
        }

        public double Aekc
        {
            get;
            set;
        }

        = 65800;
        public double Aeko
        {
            get;
            set;
        }

        = 1400;
        public double Aevc
        {
            get;
            set;
        }

        = 68800;
        public double Kc25
        {
            get;
            set;
        }

        = 460;
        public double Ko25
        {
            get;
            set;
        }

        = 330;
        public short TransitionStageLeafExp
        {
            get;
            set;
        }

        = -1;
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
            public string SpeciesId => ctx.ReadText(0, null);
            public byte CarboxylationPathway => ctx.ReadDataByte(0UL, (byte)0);
            public double DefaultRadiationUseEfficiency => ctx.ReadDataDouble(64UL, 0);
            public double PartBiologicalNFixation => ctx.ReadDataDouble(128UL, 0);
            public double InitialKcFactor => ctx.ReadDataDouble(192UL, 0);
            public double LuxuryNCoeff => ctx.ReadDataDouble(256UL, 0);
            public double MaxCropDiameter => ctx.ReadDataDouble(320UL, 0);
            public double StageAtMaxHeight => ctx.ReadDataDouble(384UL, 0);
            public double StageAtMaxDiameter => ctx.ReadDataDouble(448UL, 0);
            public double MinimumNConcentration => ctx.ReadDataDouble(512UL, 0);
            public double MinimumTemperatureForAssimilation => ctx.ReadDataDouble(576UL, 0);
            public double OptimumTemperatureForAssimilation => ctx.ReadDataDouble(640UL, 0);
            public double MaximumTemperatureForAssimilation => ctx.ReadDataDouble(704UL, 0);
            public double NConcentrationAbovegroundBiomass => ctx.ReadDataDouble(768UL, 0);
            public double NConcentrationB0 => ctx.ReadDataDouble(832UL, 0);
            public double NConcentrationPN => ctx.ReadDataDouble(896UL, 0);
            public double NConcentrationRoot => ctx.ReadDataDouble(960UL, 0);
            public ushort DevelopmentAccelerationByNitrogenStress => ctx.ReadDataUShort(16UL, (ushort)0);
            public double FieldConditionModifier => ctx.ReadDataDouble(1024UL, 1);
            public double AssimilateReallocation => ctx.ReadDataDouble(1088UL, 0);
            public IReadOnlyList<double> BaseTemperature => ctx.ReadList(1).CastDouble();
            public IReadOnlyList<double> OrganMaintenanceRespiration => ctx.ReadList(2).CastDouble();
            public IReadOnlyList<double> OrganGrowthRespiration => ctx.ReadList(3).CastDouble();
            public IReadOnlyList<double> StageMaxRootNConcentration => ctx.ReadList(4).CastDouble();
            public IReadOnlyList<double> InitialOrganBiomass => ctx.ReadList(5).CastDouble();
            public IReadOnlyList<double> CriticalOxygenContent => ctx.ReadList(6).CastDouble();
            public IReadOnlyList<double> StageMobilFromStorageCoeff => ctx.ReadList(7).CastDouble();
            public IReadOnlyList<bool> AbovegroundOrgan => ctx.ReadList(8).CastBool();
            public IReadOnlyList<bool> StorageOrgan => ctx.ReadList(9).CastBool();
            public double SamplingDepth => ctx.ReadDataDouble(1152UL, 0);
            public double TargetNSamplingDepth => ctx.ReadDataDouble(1216UL, 0);
            public double TargetN30 => ctx.ReadDataDouble(1280UL, 0);
            public double MaxNUptakeParam => ctx.ReadDataDouble(1344UL, 0);
            public double RootDistributionParam => ctx.ReadDataDouble(1408UL, 0);
            public ushort PlantDensity => ctx.ReadDataUShort(32UL, (ushort)0);
            public double RootGrowthLag => ctx.ReadDataDouble(1472UL, 0);
            public double MinimumTemperatureRootGrowth => ctx.ReadDataDouble(1536UL, 0);
            public double InitialRootingDepth => ctx.ReadDataDouble(1600UL, 0);
            public double RootPenetrationRate => ctx.ReadDataDouble(1664UL, 0);
            public double RootFormFactor => ctx.ReadDataDouble(1728UL, 0);
            public double SpecificRootLength => ctx.ReadDataDouble(1792UL, 0);
            public ushort StageAfterCut => ctx.ReadDataUShort(48UL, (ushort)0);
            public double LimitingTemperatureHeatStress => ctx.ReadDataDouble(1856UL, 0);
            public ushort CuttingDelayDays => ctx.ReadDataUShort(1920UL, (ushort)0);
            public double DroughtImpactOnFertilityFactor => ctx.ReadDataDouble(1984UL, 0);
            public double EfMono => ctx.ReadDataDouble(2048UL, 0.5);
            public double EfMonos => ctx.ReadDataDouble(2112UL, 0.5);
            public double EfIso => ctx.ReadDataDouble(2176UL, 0);
            public double VcMax25 => ctx.ReadDataDouble(2240UL, 0);
            public double Aekc => ctx.ReadDataDouble(2304UL, 65800);
            public double Aeko => ctx.ReadDataDouble(2368UL, 1400);
            public double Aevc => ctx.ReadDataDouble(2432UL, 68800);
            public double Kc25 => ctx.ReadDataDouble(2496UL, 460);
            public double Ko25 => ctx.ReadDataDouble(2560UL, 330);
            public short TransitionStageLeafExp => ctx.ReadDataShort(1936UL, (short)-1);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(41, 10);
            }

            public string SpeciesId
            {
                get => this.ReadText(0, null);
                set => this.WriteText(0, value, null);
            }

            public byte CarboxylationPathway
            {
                get => this.ReadDataByte(0UL, (byte)0);
                set => this.WriteData(0UL, value, (byte)0);
            }

            public double DefaultRadiationUseEfficiency
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public double PartBiologicalNFixation
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }

            public double InitialKcFactor
            {
                get => this.ReadDataDouble(192UL, 0);
                set => this.WriteData(192UL, value, 0);
            }

            public double LuxuryNCoeff
            {
                get => this.ReadDataDouble(256UL, 0);
                set => this.WriteData(256UL, value, 0);
            }

            public double MaxCropDiameter
            {
                get => this.ReadDataDouble(320UL, 0);
                set => this.WriteData(320UL, value, 0);
            }

            public double StageAtMaxHeight
            {
                get => this.ReadDataDouble(384UL, 0);
                set => this.WriteData(384UL, value, 0);
            }

            public double StageAtMaxDiameter
            {
                get => this.ReadDataDouble(448UL, 0);
                set => this.WriteData(448UL, value, 0);
            }

            public double MinimumNConcentration
            {
                get => this.ReadDataDouble(512UL, 0);
                set => this.WriteData(512UL, value, 0);
            }

            public double MinimumTemperatureForAssimilation
            {
                get => this.ReadDataDouble(576UL, 0);
                set => this.WriteData(576UL, value, 0);
            }

            public double OptimumTemperatureForAssimilation
            {
                get => this.ReadDataDouble(640UL, 0);
                set => this.WriteData(640UL, value, 0);
            }

            public double MaximumTemperatureForAssimilation
            {
                get => this.ReadDataDouble(704UL, 0);
                set => this.WriteData(704UL, value, 0);
            }

            public double NConcentrationAbovegroundBiomass
            {
                get => this.ReadDataDouble(768UL, 0);
                set => this.WriteData(768UL, value, 0);
            }

            public double NConcentrationB0
            {
                get => this.ReadDataDouble(832UL, 0);
                set => this.WriteData(832UL, value, 0);
            }

            public double NConcentrationPN
            {
                get => this.ReadDataDouble(896UL, 0);
                set => this.WriteData(896UL, value, 0);
            }

            public double NConcentrationRoot
            {
                get => this.ReadDataDouble(960UL, 0);
                set => this.WriteData(960UL, value, 0);
            }

            public ushort DevelopmentAccelerationByNitrogenStress
            {
                get => this.ReadDataUShort(16UL, (ushort)0);
                set => this.WriteData(16UL, value, (ushort)0);
            }

            public double FieldConditionModifier
            {
                get => this.ReadDataDouble(1024UL, 1);
                set => this.WriteData(1024UL, value, 1);
            }

            public double AssimilateReallocation
            {
                get => this.ReadDataDouble(1088UL, 0);
                set => this.WriteData(1088UL, value, 0);
            }

            public ListOfPrimitivesSerializer<double> BaseTemperature
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(1);
                set => Link(1, value);
            }

            public ListOfPrimitivesSerializer<double> OrganMaintenanceRespiration
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(2);
                set => Link(2, value);
            }

            public ListOfPrimitivesSerializer<double> OrganGrowthRespiration
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(3);
                set => Link(3, value);
            }

            public ListOfPrimitivesSerializer<double> StageMaxRootNConcentration
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(4);
                set => Link(4, value);
            }

            public ListOfPrimitivesSerializer<double> InitialOrganBiomass
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(5);
                set => Link(5, value);
            }

            public ListOfPrimitivesSerializer<double> CriticalOxygenContent
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(6);
                set => Link(6, value);
            }

            public ListOfPrimitivesSerializer<double> StageMobilFromStorageCoeff
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(7);
                set => Link(7, value);
            }

            public ListOfBitsSerializer AbovegroundOrgan
            {
                get => BuildPointer<ListOfBitsSerializer>(8);
                set => Link(8, value);
            }

            public ListOfBitsSerializer StorageOrgan
            {
                get => BuildPointer<ListOfBitsSerializer>(9);
                set => Link(9, value);
            }

            public double SamplingDepth
            {
                get => this.ReadDataDouble(1152UL, 0);
                set => this.WriteData(1152UL, value, 0);
            }

            public double TargetNSamplingDepth
            {
                get => this.ReadDataDouble(1216UL, 0);
                set => this.WriteData(1216UL, value, 0);
            }

            public double TargetN30
            {
                get => this.ReadDataDouble(1280UL, 0);
                set => this.WriteData(1280UL, value, 0);
            }

            public double MaxNUptakeParam
            {
                get => this.ReadDataDouble(1344UL, 0);
                set => this.WriteData(1344UL, value, 0);
            }

            public double RootDistributionParam
            {
                get => this.ReadDataDouble(1408UL, 0);
                set => this.WriteData(1408UL, value, 0);
            }

            public ushort PlantDensity
            {
                get => this.ReadDataUShort(32UL, (ushort)0);
                set => this.WriteData(32UL, value, (ushort)0);
            }

            public double RootGrowthLag
            {
                get => this.ReadDataDouble(1472UL, 0);
                set => this.WriteData(1472UL, value, 0);
            }

            public double MinimumTemperatureRootGrowth
            {
                get => this.ReadDataDouble(1536UL, 0);
                set => this.WriteData(1536UL, value, 0);
            }

            public double InitialRootingDepth
            {
                get => this.ReadDataDouble(1600UL, 0);
                set => this.WriteData(1600UL, value, 0);
            }

            public double RootPenetrationRate
            {
                get => this.ReadDataDouble(1664UL, 0);
                set => this.WriteData(1664UL, value, 0);
            }

            public double RootFormFactor
            {
                get => this.ReadDataDouble(1728UL, 0);
                set => this.WriteData(1728UL, value, 0);
            }

            public double SpecificRootLength
            {
                get => this.ReadDataDouble(1792UL, 0);
                set => this.WriteData(1792UL, value, 0);
            }

            public ushort StageAfterCut
            {
                get => this.ReadDataUShort(48UL, (ushort)0);
                set => this.WriteData(48UL, value, (ushort)0);
            }

            public double LimitingTemperatureHeatStress
            {
                get => this.ReadDataDouble(1856UL, 0);
                set => this.WriteData(1856UL, value, 0);
            }

            public ushort CuttingDelayDays
            {
                get => this.ReadDataUShort(1920UL, (ushort)0);
                set => this.WriteData(1920UL, value, (ushort)0);
            }

            public double DroughtImpactOnFertilityFactor
            {
                get => this.ReadDataDouble(1984UL, 0);
                set => this.WriteData(1984UL, value, 0);
            }

            public double EfMono
            {
                get => this.ReadDataDouble(2048UL, 0.5);
                set => this.WriteData(2048UL, value, 0.5);
            }

            public double EfMonos
            {
                get => this.ReadDataDouble(2112UL, 0.5);
                set => this.WriteData(2112UL, value, 0.5);
            }

            public double EfIso
            {
                get => this.ReadDataDouble(2176UL, 0);
                set => this.WriteData(2176UL, value, 0);
            }

            public double VcMax25
            {
                get => this.ReadDataDouble(2240UL, 0);
                set => this.WriteData(2240UL, value, 0);
            }

            public double Aekc
            {
                get => this.ReadDataDouble(2304UL, 65800);
                set => this.WriteData(2304UL, value, 65800);
            }

            public double Aeko
            {
                get => this.ReadDataDouble(2368UL, 1400);
                set => this.WriteData(2368UL, value, 1400);
            }

            public double Aevc
            {
                get => this.ReadDataDouble(2432UL, 68800);
                set => this.WriteData(2432UL, value, 68800);
            }

            public double Kc25
            {
                get => this.ReadDataDouble(2496UL, 460);
                set => this.WriteData(2496UL, value, 460);
            }

            public double Ko25
            {
                get => this.ReadDataDouble(2560UL, 330);
                set => this.WriteData(2560UL, value, 330);
            }

            public short TransitionStageLeafExp
            {
                get => this.ReadDataShort(1936UL, (short)-1);
                set => this.WriteData(1936UL, value, (short)-1);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf206f12e39ab7f9bUL)]
    public class CultivarParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0xf206f12e39ab7f9bUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            CultivarId = reader.CultivarId;
            Description = reader.Description;
            Perennial = reader.Perennial;
            MaxAssimilationRate = reader.MaxAssimilationRate;
            MaxCropHeight = reader.MaxCropHeight;
            ResidueNRatio = reader.ResidueNRatio;
            Lt50cultivar = reader.Lt50cultivar;
            CropHeightP1 = reader.CropHeightP1;
            CropHeightP2 = reader.CropHeightP2;
            CropSpecificMaxRootingDepth = reader.CropSpecificMaxRootingDepth;
            AssimilatePartitioningCoeff = reader.AssimilatePartitioningCoeff;
            OrganSenescenceRate = reader.OrganSenescenceRate;
            BaseDaylength = reader.BaseDaylength;
            OptimumTemperature = reader.OptimumTemperature;
            DaylengthRequirement = reader.DaylengthRequirement;
            DroughtStressThreshold = reader.DroughtStressThreshold;
            SpecificLeafArea = reader.SpecificLeafArea;
            StageKcFactor = reader.StageKcFactor;
            StageTemperatureSum = reader.StageTemperatureSum;
            VernalisationRequirement = reader.VernalisationRequirement;
            HeatSumIrrigationStart = reader.HeatSumIrrigationStart;
            HeatSumIrrigationEnd = reader.HeatSumIrrigationEnd;
            CriticalTemperatureHeatStress = reader.CriticalTemperatureHeatStress;
            BeginSensitivePhaseHeatStress = reader.BeginSensitivePhaseHeatStress;
            EndSensitivePhaseHeatStress = reader.EndSensitivePhaseHeatStress;
            FrostHardening = reader.FrostHardening;
            FrostDehardening = reader.FrostDehardening;
            LowTemperatureExposure = reader.LowTemperatureExposure;
            RespiratoryStress = reader.RespiratoryStress;
            LatestHarvestDoy = reader.LatestHarvestDoy;
            OrganIdsForPrimaryYield = reader.OrganIdsForPrimaryYield?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Models.Monica.YieldComponent>(_));
            OrganIdsForSecondaryYield = reader.OrganIdsForSecondaryYield?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Models.Monica.YieldComponent>(_));
            OrganIdsForCutting = reader.OrganIdsForCutting?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Models.Monica.YieldComponent>(_));
            EarlyRefLeafExp = reader.EarlyRefLeafExp;
            RefLeafExp = reader.RefLeafExp;
            MinTempDevWE = reader.MinTempDevWE;
            OptTempDevWE = reader.OptTempDevWE;
            MaxTempDevWE = reader.MaxTempDevWE;
            WinterCrop = reader.WinterCrop;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.CultivarId = CultivarId;
            writer.Description = Description;
            writer.Perennial = Perennial;
            writer.MaxAssimilationRate = MaxAssimilationRate;
            writer.MaxCropHeight = MaxCropHeight;
            writer.ResidueNRatio = ResidueNRatio;
            writer.Lt50cultivar = Lt50cultivar;
            writer.CropHeightP1 = CropHeightP1;
            writer.CropHeightP2 = CropHeightP2;
            writer.CropSpecificMaxRootingDepth = CropSpecificMaxRootingDepth;
            writer.AssimilatePartitioningCoeff.Init(AssimilatePartitioningCoeff, (_s2, _v2) => _s2.Init(_v2));
            writer.OrganSenescenceRate.Init(OrganSenescenceRate, (_s2, _v2) => _s2.Init(_v2));
            writer.BaseDaylength.Init(BaseDaylength);
            writer.OptimumTemperature.Init(OptimumTemperature);
            writer.DaylengthRequirement.Init(DaylengthRequirement);
            writer.DroughtStressThreshold.Init(DroughtStressThreshold);
            writer.SpecificLeafArea.Init(SpecificLeafArea);
            writer.StageKcFactor.Init(StageKcFactor);
            writer.StageTemperatureSum.Init(StageTemperatureSum);
            writer.VernalisationRequirement.Init(VernalisationRequirement);
            writer.HeatSumIrrigationStart = HeatSumIrrigationStart;
            writer.HeatSumIrrigationEnd = HeatSumIrrigationEnd;
            writer.CriticalTemperatureHeatStress = CriticalTemperatureHeatStress;
            writer.BeginSensitivePhaseHeatStress = BeginSensitivePhaseHeatStress;
            writer.EndSensitivePhaseHeatStress = EndSensitivePhaseHeatStress;
            writer.FrostHardening = FrostHardening;
            writer.FrostDehardening = FrostDehardening;
            writer.LowTemperatureExposure = LowTemperatureExposure;
            writer.RespiratoryStress = RespiratoryStress;
            writer.LatestHarvestDoy = LatestHarvestDoy;
            writer.OrganIdsForPrimaryYield.Init(OrganIdsForPrimaryYield, (_s1, _v1) => _v1?.serialize(_s1));
            writer.OrganIdsForSecondaryYield.Init(OrganIdsForSecondaryYield, (_s1, _v1) => _v1?.serialize(_s1));
            writer.OrganIdsForCutting.Init(OrganIdsForCutting, (_s1, _v1) => _v1?.serialize(_s1));
            writer.EarlyRefLeafExp = EarlyRefLeafExp;
            writer.RefLeafExp = RefLeafExp;
            writer.MinTempDevWE = MinTempDevWE;
            writer.OptTempDevWE = OptTempDevWE;
            writer.MaxTempDevWE = MaxTempDevWE;
            writer.WinterCrop = WinterCrop;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public string CultivarId
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public bool Perennial
        {
            get;
            set;
        }

        public double MaxAssimilationRate
        {
            get;
            set;
        }

        public double MaxCropHeight
        {
            get;
            set;
        }

        public double ResidueNRatio
        {
            get;
            set;
        }

        public double Lt50cultivar
        {
            get;
            set;
        }

        public double CropHeightP1
        {
            get;
            set;
        }

        public double CropHeightP2
        {
            get;
            set;
        }

        public double CropSpecificMaxRootingDepth
        {
            get;
            set;
        }

        public IReadOnlyList<IReadOnlyList<double>> AssimilatePartitioningCoeff
        {
            get;
            set;
        }

        public IReadOnlyList<IReadOnlyList<double>> OrganSenescenceRate
        {
            get;
            set;
        }

        public IReadOnlyList<double> BaseDaylength
        {
            get;
            set;
        }

        public IReadOnlyList<double> OptimumTemperature
        {
            get;
            set;
        }

        public IReadOnlyList<double> DaylengthRequirement
        {
            get;
            set;
        }

        public IReadOnlyList<double> DroughtStressThreshold
        {
            get;
            set;
        }

        public IReadOnlyList<double> SpecificLeafArea
        {
            get;
            set;
        }

        public IReadOnlyList<double> StageKcFactor
        {
            get;
            set;
        }

        public IReadOnlyList<double> StageTemperatureSum
        {
            get;
            set;
        }

        public IReadOnlyList<double> VernalisationRequirement
        {
            get;
            set;
        }

        public double HeatSumIrrigationStart
        {
            get;
            set;
        }

        public double HeatSumIrrigationEnd
        {
            get;
            set;
        }

        public double CriticalTemperatureHeatStress
        {
            get;
            set;
        }

        public double BeginSensitivePhaseHeatStress
        {
            get;
            set;
        }

        public double EndSensitivePhaseHeatStress
        {
            get;
            set;
        }

        public double FrostHardening
        {
            get;
            set;
        }

        public double FrostDehardening
        {
            get;
            set;
        }

        public double LowTemperatureExposure
        {
            get;
            set;
        }

        public double RespiratoryStress
        {
            get;
            set;
        }

        public short LatestHarvestDoy
        {
            get;
            set;
        }

        = -1;
        public IReadOnlyList<Mas.Models.Monica.YieldComponent> OrganIdsForPrimaryYield
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Models.Monica.YieldComponent> OrganIdsForSecondaryYield
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Models.Monica.YieldComponent> OrganIdsForCutting
        {
            get;
            set;
        }

        public double EarlyRefLeafExp
        {
            get;
            set;
        }

        = 12;
        public double RefLeafExp
        {
            get;
            set;
        }

        = 20;
        public double MinTempDevWE
        {
            get;
            set;
        }

        public double OptTempDevWE
        {
            get;
            set;
        }

        public double MaxTempDevWE
        {
            get;
            set;
        }

        public bool WinterCrop
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
            public string CultivarId => ctx.ReadText(0, null);
            public string Description => ctx.ReadText(1, null);
            public bool Perennial => ctx.ReadDataBool(0UL, false);
            public double MaxAssimilationRate => ctx.ReadDataDouble(64UL, 0);
            public double MaxCropHeight => ctx.ReadDataDouble(128UL, 0);
            public double ResidueNRatio => ctx.ReadDataDouble(192UL, 0);
            public double Lt50cultivar => ctx.ReadDataDouble(256UL, 0);
            public double CropHeightP1 => ctx.ReadDataDouble(320UL, 0);
            public double CropHeightP2 => ctx.ReadDataDouble(384UL, 0);
            public double CropSpecificMaxRootingDepth => ctx.ReadDataDouble(448UL, 0);
            public IReadOnlyList<IReadOnlyList<double>> AssimilatePartitioningCoeff => ctx.ReadList(2).Cast(_0 => _0.RequireList().CastDouble());
            public IReadOnlyList<IReadOnlyList<double>> OrganSenescenceRate => ctx.ReadList(3).Cast(_0 => _0.RequireList().CastDouble());
            public IReadOnlyList<double> BaseDaylength => ctx.ReadList(4).CastDouble();
            public IReadOnlyList<double> OptimumTemperature => ctx.ReadList(5).CastDouble();
            public IReadOnlyList<double> DaylengthRequirement => ctx.ReadList(6).CastDouble();
            public IReadOnlyList<double> DroughtStressThreshold => ctx.ReadList(7).CastDouble();
            public IReadOnlyList<double> SpecificLeafArea => ctx.ReadList(8).CastDouble();
            public IReadOnlyList<double> StageKcFactor => ctx.ReadList(9).CastDouble();
            public IReadOnlyList<double> StageTemperatureSum => ctx.ReadList(10).CastDouble();
            public IReadOnlyList<double> VernalisationRequirement => ctx.ReadList(11).CastDouble();
            public double HeatSumIrrigationStart => ctx.ReadDataDouble(512UL, 0);
            public double HeatSumIrrigationEnd => ctx.ReadDataDouble(576UL, 0);
            public double CriticalTemperatureHeatStress => ctx.ReadDataDouble(640UL, 0);
            public double BeginSensitivePhaseHeatStress => ctx.ReadDataDouble(704UL, 0);
            public double EndSensitivePhaseHeatStress => ctx.ReadDataDouble(768UL, 0);
            public double FrostHardening => ctx.ReadDataDouble(832UL, 0);
            public double FrostDehardening => ctx.ReadDataDouble(896UL, 0);
            public double LowTemperatureExposure => ctx.ReadDataDouble(960UL, 0);
            public double RespiratoryStress => ctx.ReadDataDouble(1024UL, 0);
            public short LatestHarvestDoy => ctx.ReadDataShort(16UL, (short)-1);
            public IReadOnlyList<Mas.Models.Monica.YieldComponent.READER> OrganIdsForPrimaryYield => ctx.ReadList(12).Cast(Mas.Models.Monica.YieldComponent.READER.create);
            public IReadOnlyList<Mas.Models.Monica.YieldComponent.READER> OrganIdsForSecondaryYield => ctx.ReadList(13).Cast(Mas.Models.Monica.YieldComponent.READER.create);
            public IReadOnlyList<Mas.Models.Monica.YieldComponent.READER> OrganIdsForCutting => ctx.ReadList(14).Cast(Mas.Models.Monica.YieldComponent.READER.create);
            public double EarlyRefLeafExp => ctx.ReadDataDouble(1088UL, 12);
            public double RefLeafExp => ctx.ReadDataDouble(1152UL, 20);
            public double MinTempDevWE => ctx.ReadDataDouble(1216UL, 0);
            public double OptTempDevWE => ctx.ReadDataDouble(1280UL, 0);
            public double MaxTempDevWE => ctx.ReadDataDouble(1344UL, 0);
            public bool WinterCrop => ctx.ReadDataBool(1UL, false);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(22, 15);
            }

            public string CultivarId
            {
                get => this.ReadText(0, null);
                set => this.WriteText(0, value, null);
            }

            public string Description
            {
                get => this.ReadText(1, null);
                set => this.WriteText(1, value, null);
            }

            public bool Perennial
            {
                get => this.ReadDataBool(0UL, false);
                set => this.WriteData(0UL, value, false);
            }

            public double MaxAssimilationRate
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public double MaxCropHeight
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }

            public double ResidueNRatio
            {
                get => this.ReadDataDouble(192UL, 0);
                set => this.WriteData(192UL, value, 0);
            }

            public double Lt50cultivar
            {
                get => this.ReadDataDouble(256UL, 0);
                set => this.WriteData(256UL, value, 0);
            }

            public double CropHeightP1
            {
                get => this.ReadDataDouble(320UL, 0);
                set => this.WriteData(320UL, value, 0);
            }

            public double CropHeightP2
            {
                get => this.ReadDataDouble(384UL, 0);
                set => this.WriteData(384UL, value, 0);
            }

            public double CropSpecificMaxRootingDepth
            {
                get => this.ReadDataDouble(448UL, 0);
                set => this.WriteData(448UL, value, 0);
            }

            public ListOfPointersSerializer<ListOfPrimitivesSerializer<double>> AssimilatePartitioningCoeff
            {
                get => BuildPointer<ListOfPointersSerializer<ListOfPrimitivesSerializer<double>>>(2);
                set => Link(2, value);
            }

            public ListOfPointersSerializer<ListOfPrimitivesSerializer<double>> OrganSenescenceRate
            {
                get => BuildPointer<ListOfPointersSerializer<ListOfPrimitivesSerializer<double>>>(3);
                set => Link(3, value);
            }

            public ListOfPrimitivesSerializer<double> BaseDaylength
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(4);
                set => Link(4, value);
            }

            public ListOfPrimitivesSerializer<double> OptimumTemperature
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(5);
                set => Link(5, value);
            }

            public ListOfPrimitivesSerializer<double> DaylengthRequirement
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(6);
                set => Link(6, value);
            }

            public ListOfPrimitivesSerializer<double> DroughtStressThreshold
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(7);
                set => Link(7, value);
            }

            public ListOfPrimitivesSerializer<double> SpecificLeafArea
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(8);
                set => Link(8, value);
            }

            public ListOfPrimitivesSerializer<double> StageKcFactor
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(9);
                set => Link(9, value);
            }

            public ListOfPrimitivesSerializer<double> StageTemperatureSum
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(10);
                set => Link(10, value);
            }

            public ListOfPrimitivesSerializer<double> VernalisationRequirement
            {
                get => BuildPointer<ListOfPrimitivesSerializer<double>>(11);
                set => Link(11, value);
            }

            public double HeatSumIrrigationStart
            {
                get => this.ReadDataDouble(512UL, 0);
                set => this.WriteData(512UL, value, 0);
            }

            public double HeatSumIrrigationEnd
            {
                get => this.ReadDataDouble(576UL, 0);
                set => this.WriteData(576UL, value, 0);
            }

            public double CriticalTemperatureHeatStress
            {
                get => this.ReadDataDouble(640UL, 0);
                set => this.WriteData(640UL, value, 0);
            }

            public double BeginSensitivePhaseHeatStress
            {
                get => this.ReadDataDouble(704UL, 0);
                set => this.WriteData(704UL, value, 0);
            }

            public double EndSensitivePhaseHeatStress
            {
                get => this.ReadDataDouble(768UL, 0);
                set => this.WriteData(768UL, value, 0);
            }

            public double FrostHardening
            {
                get => this.ReadDataDouble(832UL, 0);
                set => this.WriteData(832UL, value, 0);
            }

            public double FrostDehardening
            {
                get => this.ReadDataDouble(896UL, 0);
                set => this.WriteData(896UL, value, 0);
            }

            public double LowTemperatureExposure
            {
                get => this.ReadDataDouble(960UL, 0);
                set => this.WriteData(960UL, value, 0);
            }

            public double RespiratoryStress
            {
                get => this.ReadDataDouble(1024UL, 0);
                set => this.WriteData(1024UL, value, 0);
            }

            public short LatestHarvestDoy
            {
                get => this.ReadDataShort(16UL, (short)-1);
                set => this.WriteData(16UL, value, (short)-1);
            }

            public ListOfStructsSerializer<Mas.Models.Monica.YieldComponent.WRITER> OrganIdsForPrimaryYield
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Models.Monica.YieldComponent.WRITER>>(12);
                set => Link(12, value);
            }

            public ListOfStructsSerializer<Mas.Models.Monica.YieldComponent.WRITER> OrganIdsForSecondaryYield
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Models.Monica.YieldComponent.WRITER>>(13);
                set => Link(13, value);
            }

            public ListOfStructsSerializer<Mas.Models.Monica.YieldComponent.WRITER> OrganIdsForCutting
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Models.Monica.YieldComponent.WRITER>>(14);
                set => Link(14, value);
            }

            public double EarlyRefLeafExp
            {
                get => this.ReadDataDouble(1088UL, 12);
                set => this.WriteData(1088UL, value, 12);
            }

            public double RefLeafExp
            {
                get => this.ReadDataDouble(1152UL, 20);
                set => this.WriteData(1152UL, value, 20);
            }

            public double MinTempDevWE
            {
                get => this.ReadDataDouble(1216UL, 0);
                set => this.WriteData(1216UL, value, 0);
            }

            public double OptTempDevWE
            {
                get => this.ReadDataDouble(1280UL, 0);
                set => this.WriteData(1280UL, value, 0);
            }

            public double MaxTempDevWE
            {
                get => this.ReadDataDouble(1344UL, 0);
                set => this.WriteData(1344UL, value, 0);
            }

            public bool WinterCrop
            {
                get => this.ReadDataBool(1UL, false);
                set => this.WriteData(1UL, value, false);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdbfe301c0ddefe4eUL)]
    public class YieldComponent : ICapnpSerializable
    {
        public const UInt64 typeId = 0xdbfe301c0ddefe4eUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            OrganId = reader.OrganId;
            YieldPercentage = reader.YieldPercentage;
            YieldDryMatter = reader.YieldDryMatter;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.OrganId = OrganId;
            writer.YieldPercentage = YieldPercentage;
            writer.YieldDryMatter = YieldDryMatter;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public long OrganId
        {
            get;
            set;
        }

        = -1L;
        public double YieldPercentage
        {
            get;
            set;
        }

        public double YieldDryMatter
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
            public long OrganId => ctx.ReadDataLong(0UL, -1L);
            public double YieldPercentage => ctx.ReadDataDouble(64UL, 0);
            public double YieldDryMatter => ctx.ReadDataDouble(128UL, 0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(3, 0);
            }

            public long OrganId
            {
                get => this.ReadDataLong(0UL, -1L);
                set => this.WriteData(0UL, value, -1L);
            }

            public double YieldPercentage
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public double YieldDryMatter
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc5f724bd00c2f628UL)]
    public class AutomaticHarvestParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0xc5f724bd00c2f628UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            TheHarvestTime = reader.TheHarvestTime;
            LatestHarvestDOY = reader.LatestHarvestDOY;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.TheHarvestTime = TheHarvestTime;
            writer.LatestHarvestDOY = LatestHarvestDOY;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Models.Monica.AutomaticHarvestParameters.HarvestTime TheHarvestTime
        {
            get;
            set;
        }

        = Mas.Models.Monica.AutomaticHarvestParameters.HarvestTime.unknown;
        public short LatestHarvestDOY
        {
            get;
            set;
        }

        = -1;
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
            public Mas.Models.Monica.AutomaticHarvestParameters.HarvestTime TheHarvestTime => (Mas.Models.Monica.AutomaticHarvestParameters.HarvestTime)ctx.ReadDataUShort(0UL, (ushort)1);
            public short LatestHarvestDOY => ctx.ReadDataShort(16UL, (short)-1);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 0);
            }

            public Mas.Models.Monica.AutomaticHarvestParameters.HarvestTime TheHarvestTime
            {
                get => (Mas.Models.Monica.AutomaticHarvestParameters.HarvestTime)this.ReadDataUShort(0UL, (ushort)1);
                set => this.WriteData(0UL, (ushort)value, (ushort)1);
            }

            public short LatestHarvestDOY
            {
                get => this.ReadDataShort(16UL, (short)-1);
                set => this.WriteData(16UL, value, (short)-1);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x990bdcf2be83b604UL)]
        public enum HarvestTime : ushort
        {
            maturity,
            unknown
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xea9236083718fdc2UL)]
    public class NMinCropParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0xea9236083718fdc2UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            SamplingDepth = reader.SamplingDepth;
            NTarget = reader.NTarget;
            NTarget30 = reader.NTarget30;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.SamplingDepth = SamplingDepth;
            writer.NTarget = NTarget;
            writer.NTarget30 = NTarget30;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public double SamplingDepth
        {
            get;
            set;
        }

        public double NTarget
        {
            get;
            set;
        }

        public double NTarget30
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
            public double SamplingDepth => ctx.ReadDataDouble(0UL, 0);
            public double NTarget => ctx.ReadDataDouble(64UL, 0);
            public double NTarget30 => ctx.ReadDataDouble(128UL, 0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(3, 0);
            }

            public double SamplingDepth
            {
                get => this.ReadDataDouble(0UL, 0);
                set => this.WriteData(0UL, value, 0);
            }

            public double NTarget
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public double NTarget30
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xde7576c640b5ad18UL)]
    public class NMinApplicationParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0xde7576c640b5ad18UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Min = reader.Min;
            Max = reader.Max;
            DelayInDays = reader.DelayInDays;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Min = Min;
            writer.Max = Max;
            writer.DelayInDays = DelayInDays;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public double Min
        {
            get;
            set;
        }

        public double Max
        {
            get;
            set;
        }

        public ushort DelayInDays
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
            public double Min => ctx.ReadDataDouble(0UL, 0);
            public double Max => ctx.ReadDataDouble(64UL, 0);
            public ushort DelayInDays => ctx.ReadDataUShort(128UL, (ushort)0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(3, 0);
            }

            public double Min
            {
                get => this.ReadDataDouble(0UL, 0);
                set => this.WriteData(0UL, value, 0);
            }

            public double Max
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public ushort DelayInDays
            {
                get => this.ReadDataUShort(128UL, (ushort)0);
                set => this.WriteData(128UL, value, (ushort)0);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8491dc2c2f94f1d1UL)]
    public class CropResidueParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0x8491dc2c2f94f1d1UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Params = CapnpSerializable.Create<Mas.Rpc.Management.Params.OrganicFertilization.OrganicMatterParameters>(reader.Params);
            Species = reader.Species;
            ResidueType = reader.ResidueType;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            Params?.serialize(writer.Params);
            writer.Species = Species;
            writer.ResidueType = ResidueType;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Rpc.Management.Params.OrganicFertilization.OrganicMatterParameters Params
        {
            get;
            set;
        }

        public string Species
        {
            get;
            set;
        }

        public string ResidueType
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
            public Mas.Rpc.Management.Params.OrganicFertilization.OrganicMatterParameters.READER Params => ctx.ReadStruct(0, Mas.Rpc.Management.Params.OrganicFertilization.OrganicMatterParameters.READER.create);
            public string Species => ctx.ReadText(1, null);
            public string ResidueType => ctx.ReadText(2, null);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(0, 3);
            }

            public Mas.Rpc.Management.Params.OrganicFertilization.OrganicMatterParameters.WRITER Params
            {
                get => BuildPointer<Mas.Rpc.Management.Params.OrganicFertilization.OrganicMatterParameters.WRITER>(0);
                set => Link(0, value);
            }

            public string Species
            {
                get => this.ReadText(1, null);
                set => this.WriteText(1, value, null);
            }

            public string ResidueType
            {
                get => this.ReadText(2, null);
                set => this.WriteText(2, value, null);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb42137d4b8ba3ef6UL)]
    public class SoilParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0xb42137d4b8ba3ef6UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            SoilSandContent = reader.SoilSandContent;
            SoilClayContent = reader.SoilClayContent;
            SoilpH = reader.SoilpH;
            SoilStoneContent = reader.SoilStoneContent;
            Lambda = reader.Lambda;
            FieldCapacity = reader.FieldCapacity;
            Saturation = reader.Saturation;
            PermanentWiltingPoint = reader.PermanentWiltingPoint;
            SoilTexture = reader.SoilTexture;
            SoilAmmonium = reader.SoilAmmonium;
            SoilNitrate = reader.SoilNitrate;
            SoilCNRatio = reader.SoilCNRatio;
            SoilMoisturePercentFC = reader.SoilMoisturePercentFC;
            SoilRawDensity = reader.SoilRawDensity;
            SoilBulkDensity = reader.SoilBulkDensity;
            SoilOrganicCarbon = reader.SoilOrganicCarbon;
            SoilOrganicMatter = reader.SoilOrganicMatter;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.SoilSandContent = SoilSandContent;
            writer.SoilClayContent = SoilClayContent;
            writer.SoilpH = SoilpH;
            writer.SoilStoneContent = SoilStoneContent;
            writer.Lambda = Lambda;
            writer.FieldCapacity = FieldCapacity;
            writer.Saturation = Saturation;
            writer.PermanentWiltingPoint = PermanentWiltingPoint;
            writer.SoilTexture = SoilTexture;
            writer.SoilAmmonium = SoilAmmonium;
            writer.SoilNitrate = SoilNitrate;
            writer.SoilCNRatio = SoilCNRatio;
            writer.SoilMoisturePercentFC = SoilMoisturePercentFC;
            writer.SoilRawDensity = SoilRawDensity;
            writer.SoilBulkDensity = SoilBulkDensity;
            writer.SoilOrganicCarbon = SoilOrganicCarbon;
            writer.SoilOrganicMatter = SoilOrganicMatter;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public double SoilSandContent
        {
            get;
            set;
        }

        = -1;
        public double SoilClayContent
        {
            get;
            set;
        }

        = -1;
        public double SoilpH
        {
            get;
            set;
        }

        = 6.9;
        public double SoilStoneContent
        {
            get;
            set;
        }

        public double Lambda
        {
            get;
            set;
        }

        = -1;
        public double FieldCapacity
        {
            get;
            set;
        }

        = -1;
        public double Saturation
        {
            get;
            set;
        }

        = -1;
        public double PermanentWiltingPoint
        {
            get;
            set;
        }

        = -1;
        public string SoilTexture
        {
            get;
            set;
        }

        public double SoilAmmonium
        {
            get;
            set;
        }

        = 0.0005;
        public double SoilNitrate
        {
            get;
            set;
        }

        = 0.005;
        public double SoilCNRatio
        {
            get;
            set;
        }

        = 10;
        public double SoilMoisturePercentFC
        {
            get;
            set;
        }

        = 100;
        public double SoilRawDensity
        {
            get;
            set;
        }

        = -1;
        public double SoilBulkDensity
        {
            get;
            set;
        }

        = -1;
        public double SoilOrganicCarbon
        {
            get;
            set;
        }

        = -1;
        public double SoilOrganicMatter
        {
            get;
            set;
        }

        = -1;
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
            public double SoilSandContent => ctx.ReadDataDouble(0UL, -1);
            public double SoilClayContent => ctx.ReadDataDouble(64UL, -1);
            public double SoilpH => ctx.ReadDataDouble(128UL, 6.9);
            public double SoilStoneContent => ctx.ReadDataDouble(192UL, 0);
            public double Lambda => ctx.ReadDataDouble(256UL, -1);
            public double FieldCapacity => ctx.ReadDataDouble(320UL, -1);
            public double Saturation => ctx.ReadDataDouble(384UL, -1);
            public double PermanentWiltingPoint => ctx.ReadDataDouble(448UL, -1);
            public string SoilTexture => ctx.ReadText(0, null);
            public double SoilAmmonium => ctx.ReadDataDouble(512UL, 0.0005);
            public double SoilNitrate => ctx.ReadDataDouble(576UL, 0.005);
            public double SoilCNRatio => ctx.ReadDataDouble(640UL, 10);
            public double SoilMoisturePercentFC => ctx.ReadDataDouble(704UL, 100);
            public double SoilRawDensity => ctx.ReadDataDouble(768UL, -1);
            public double SoilBulkDensity => ctx.ReadDataDouble(832UL, -1);
            public double SoilOrganicCarbon => ctx.ReadDataDouble(896UL, -1);
            public double SoilOrganicMatter => ctx.ReadDataDouble(960UL, -1);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(16, 1);
            }

            public double SoilSandContent
            {
                get => this.ReadDataDouble(0UL, -1);
                set => this.WriteData(0UL, value, -1);
            }

            public double SoilClayContent
            {
                get => this.ReadDataDouble(64UL, -1);
                set => this.WriteData(64UL, value, -1);
            }

            public double SoilpH
            {
                get => this.ReadDataDouble(128UL, 6.9);
                set => this.WriteData(128UL, value, 6.9);
            }

            public double SoilStoneContent
            {
                get => this.ReadDataDouble(192UL, 0);
                set => this.WriteData(192UL, value, 0);
            }

            public double Lambda
            {
                get => this.ReadDataDouble(256UL, -1);
                set => this.WriteData(256UL, value, -1);
            }

            public double FieldCapacity
            {
                get => this.ReadDataDouble(320UL, -1);
                set => this.WriteData(320UL, value, -1);
            }

            public double Saturation
            {
                get => this.ReadDataDouble(384UL, -1);
                set => this.WriteData(384UL, value, -1);
            }

            public double PermanentWiltingPoint
            {
                get => this.ReadDataDouble(448UL, -1);
                set => this.WriteData(448UL, value, -1);
            }

            public string SoilTexture
            {
                get => this.ReadText(0, null);
                set => this.WriteText(0, value, null);
            }

            public double SoilAmmonium
            {
                get => this.ReadDataDouble(512UL, 0.0005);
                set => this.WriteData(512UL, value, 0.0005);
            }

            public double SoilNitrate
            {
                get => this.ReadDataDouble(576UL, 0.005);
                set => this.WriteData(576UL, value, 0.005);
            }

            public double SoilCNRatio
            {
                get => this.ReadDataDouble(640UL, 10);
                set => this.WriteData(640UL, value, 10);
            }

            public double SoilMoisturePercentFC
            {
                get => this.ReadDataDouble(704UL, 100);
                set => this.WriteData(704UL, value, 100);
            }

            public double SoilRawDensity
            {
                get => this.ReadDataDouble(768UL, -1);
                set => this.WriteData(768UL, value, -1);
            }

            public double SoilBulkDensity
            {
                get => this.ReadDataDouble(832UL, -1);
                set => this.WriteData(832UL, value, -1);
            }

            public double SoilOrganicCarbon
            {
                get => this.ReadDataDouble(896UL, -1);
                set => this.WriteData(896UL, value, -1);
            }

            public double SoilOrganicMatter
            {
                get => this.ReadDataDouble(960UL, -1);
                set => this.WriteData(960UL, value, -1);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8890f17a143c6896UL)]
    public class AutomaticIrrigationParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0x8890f17a143c6896UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Params = CapnpSerializable.Create<Mas.Rpc.Management.Params.Irrigation.Parameters>(reader.Params);
            Amount = reader.Amount;
            Threshold = reader.Threshold;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            Params?.serialize(writer.Params);
            writer.Amount = Amount;
            writer.Threshold = Threshold;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Rpc.Management.Params.Irrigation.Parameters Params
        {
            get;
            set;
        }

        public double Amount
        {
            get;
            set;
        }

        = 17;
        public double Threshold
        {
            get;
            set;
        }

        = 0.35;
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
            public Mas.Rpc.Management.Params.Irrigation.Parameters.READER Params => ctx.ReadStruct(0, Mas.Rpc.Management.Params.Irrigation.Parameters.READER.create);
            public double Amount => ctx.ReadDataDouble(0UL, 17);
            public double Threshold => ctx.ReadDataDouble(64UL, 0.35);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(2, 1);
            }

            public Mas.Rpc.Management.Params.Irrigation.Parameters.WRITER Params
            {
                get => BuildPointer<Mas.Rpc.Management.Params.Irrigation.Parameters.WRITER>(0);
                set => Link(0, value);
            }

            public double Amount
            {
                get => this.ReadDataDouble(0UL, 17);
                set => this.WriteData(0UL, value, 17);
            }

            public double Threshold
            {
                get => this.ReadDataDouble(64UL, 0.35);
                set => this.WriteData(64UL, value, 0.35);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb599bbd2f1465f9cUL)]
    public class SiteParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0xb599bbd2f1465f9cUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Latitude = reader.Latitude;
            Slope = reader.Slope;
            HeightNN = reader.HeightNN;
            GroundwaterDepth = reader.GroundwaterDepth;
            SoilCNRatio = reader.SoilCNRatio;
            DrainageCoeff = reader.DrainageCoeff;
            VqNDeposition = reader.VqNDeposition;
            MaxEffectiveRootingDepth = reader.MaxEffectiveRootingDepth;
            ImpenetrableLayerDepth = reader.ImpenetrableLayerDepth;
            SoilSpecificHumusBalanceCorrection = reader.SoilSpecificHumusBalanceCorrection;
            SoilParameters = reader.SoilParameters?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Models.Monica.SoilParameters>(_));
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Latitude = Latitude;
            writer.Slope = Slope;
            writer.HeightNN = HeightNN;
            writer.GroundwaterDepth = GroundwaterDepth;
            writer.SoilCNRatio = SoilCNRatio;
            writer.DrainageCoeff = DrainageCoeff;
            writer.VqNDeposition = VqNDeposition;
            writer.MaxEffectiveRootingDepth = MaxEffectiveRootingDepth;
            writer.ImpenetrableLayerDepth = ImpenetrableLayerDepth;
            writer.SoilSpecificHumusBalanceCorrection = SoilSpecificHumusBalanceCorrection;
            writer.SoilParameters.Init(SoilParameters, (_s1, _v1) => _v1?.serialize(_s1));
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public double Latitude
        {
            get;
            set;
        }

        = 52.5;
        public double Slope
        {
            get;
            set;
        }

        = 0.01;
        public double HeightNN
        {
            get;
            set;
        }

        = 50;
        public double GroundwaterDepth
        {
            get;
            set;
        }

        = 70;
        public double SoilCNRatio
        {
            get;
            set;
        }

        = 10;
        public double DrainageCoeff
        {
            get;
            set;
        }

        = 1;
        public double VqNDeposition
        {
            get;
            set;
        }

        = 30;
        public double MaxEffectiveRootingDepth
        {
            get;
            set;
        }

        = 2;
        public double ImpenetrableLayerDepth
        {
            get;
            set;
        }

        = -1;
        public double SoilSpecificHumusBalanceCorrection
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Models.Monica.SoilParameters> SoilParameters
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
            public double Latitude => ctx.ReadDataDouble(0UL, 52.5);
            public double Slope => ctx.ReadDataDouble(64UL, 0.01);
            public double HeightNN => ctx.ReadDataDouble(128UL, 50);
            public double GroundwaterDepth => ctx.ReadDataDouble(192UL, 70);
            public double SoilCNRatio => ctx.ReadDataDouble(256UL, 10);
            public double DrainageCoeff => ctx.ReadDataDouble(320UL, 1);
            public double VqNDeposition => ctx.ReadDataDouble(384UL, 30);
            public double MaxEffectiveRootingDepth => ctx.ReadDataDouble(448UL, 2);
            public double ImpenetrableLayerDepth => ctx.ReadDataDouble(512UL, -1);
            public double SoilSpecificHumusBalanceCorrection => ctx.ReadDataDouble(576UL, 0);
            public IReadOnlyList<Mas.Models.Monica.SoilParameters.READER> SoilParameters => ctx.ReadList(0).Cast(Mas.Models.Monica.SoilParameters.READER.create);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(10, 1);
            }

            public double Latitude
            {
                get => this.ReadDataDouble(0UL, 52.5);
                set => this.WriteData(0UL, value, 52.5);
            }

            public double Slope
            {
                get => this.ReadDataDouble(64UL, 0.01);
                set => this.WriteData(64UL, value, 0.01);
            }

            public double HeightNN
            {
                get => this.ReadDataDouble(128UL, 50);
                set => this.WriteData(128UL, value, 50);
            }

            public double GroundwaterDepth
            {
                get => this.ReadDataDouble(192UL, 70);
                set => this.WriteData(192UL, value, 70);
            }

            public double SoilCNRatio
            {
                get => this.ReadDataDouble(256UL, 10);
                set => this.WriteData(256UL, value, 10);
            }

            public double DrainageCoeff
            {
                get => this.ReadDataDouble(320UL, 1);
                set => this.WriteData(320UL, value, 1);
            }

            public double VqNDeposition
            {
                get => this.ReadDataDouble(384UL, 30);
                set => this.WriteData(384UL, value, 30);
            }

            public double MaxEffectiveRootingDepth
            {
                get => this.ReadDataDouble(448UL, 2);
                set => this.WriteData(448UL, value, 2);
            }

            public double ImpenetrableLayerDepth
            {
                get => this.ReadDataDouble(512UL, -1);
                set => this.WriteData(512UL, value, -1);
            }

            public double SoilSpecificHumusBalanceCorrection
            {
                get => this.ReadDataDouble(576UL, 0);
                set => this.WriteData(576UL, value, 0);
            }

            public ListOfStructsSerializer<Mas.Models.Monica.SoilParameters.WRITER> SoilParameters
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Models.Monica.SoilParameters.WRITER>>(0);
                set => Link(0, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc0ff4a277ca4be0aUL)]
    public class EnvironmentParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0xc0ff4a277ca4be0aUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            Albedo = reader.Albedo;
            AtmosphericCO2 = reader.AtmosphericCO2;
            AtmosphericCO2s = reader.AtmosphericCO2s?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Models.Monica.EnvironmentParameters.YearToValue>(_));
            AtmosphericO3 = reader.AtmosphericO3;
            AtmosphericO3s = reader.AtmosphericO3s?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Models.Monica.EnvironmentParameters.YearToValue>(_));
            WindSpeedHeight = reader.WindSpeedHeight;
            LeachingDepth = reader.LeachingDepth;
            TimeStep = reader.TimeStep;
            MaxGroundwaterDepth = reader.MaxGroundwaterDepth;
            MinGroundwaterDepth = reader.MinGroundwaterDepth;
            MinGroundwaterDepthMonth = reader.MinGroundwaterDepthMonth;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.Albedo = Albedo;
            writer.AtmosphericCO2 = AtmosphericCO2;
            writer.AtmosphericCO2s.Init(AtmosphericCO2s, (_s1, _v1) => _v1?.serialize(_s1));
            writer.AtmosphericO3 = AtmosphericO3;
            writer.AtmosphericO3s.Init(AtmosphericO3s, (_s1, _v1) => _v1?.serialize(_s1));
            writer.WindSpeedHeight = WindSpeedHeight;
            writer.LeachingDepth = LeachingDepth;
            writer.TimeStep = TimeStep;
            writer.MaxGroundwaterDepth = MaxGroundwaterDepth;
            writer.MinGroundwaterDepth = MinGroundwaterDepth;
            writer.MinGroundwaterDepthMonth = MinGroundwaterDepthMonth;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public double Albedo
        {
            get;
            set;
        }

        = 0.23;
        public double AtmosphericCO2
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Models.Monica.EnvironmentParameters.YearToValue> AtmosphericCO2s
        {
            get;
            set;
        }

        public double AtmosphericO3
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Models.Monica.EnvironmentParameters.YearToValue> AtmosphericO3s
        {
            get;
            set;
        }

        public double WindSpeedHeight
        {
            get;
            set;
        }

        = 2;
        public double LeachingDepth
        {
            get;
            set;
        }

        public double TimeStep
        {
            get;
            set;
        }

        public double MaxGroundwaterDepth
        {
            get;
            set;
        }

        = 18;
        public double MinGroundwaterDepth
        {
            get;
            set;
        }

        = 20;
        public byte MinGroundwaterDepthMonth
        {
            get;
            set;
        }

        = 3;
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
            public double Albedo => ctx.ReadDataDouble(0UL, 0.23);
            public double AtmosphericCO2 => ctx.ReadDataDouble(64UL, 0);
            public IReadOnlyList<Mas.Models.Monica.EnvironmentParameters.YearToValue.READER> AtmosphericCO2s => ctx.ReadList(0).Cast(Mas.Models.Monica.EnvironmentParameters.YearToValue.READER.create);
            public double AtmosphericO3 => ctx.ReadDataDouble(128UL, 0);
            public IReadOnlyList<Mas.Models.Monica.EnvironmentParameters.YearToValue.READER> AtmosphericO3s => ctx.ReadList(1).Cast(Mas.Models.Monica.EnvironmentParameters.YearToValue.READER.create);
            public double WindSpeedHeight => ctx.ReadDataDouble(192UL, 2);
            public double LeachingDepth => ctx.ReadDataDouble(256UL, 0);
            public double TimeStep => ctx.ReadDataDouble(320UL, 0);
            public double MaxGroundwaterDepth => ctx.ReadDataDouble(384UL, 18);
            public double MinGroundwaterDepth => ctx.ReadDataDouble(448UL, 20);
            public byte MinGroundwaterDepthMonth => ctx.ReadDataByte(512UL, (byte)3);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(9, 2);
            }

            public double Albedo
            {
                get => this.ReadDataDouble(0UL, 0.23);
                set => this.WriteData(0UL, value, 0.23);
            }

            public double AtmosphericCO2
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public ListOfStructsSerializer<Mas.Models.Monica.EnvironmentParameters.YearToValue.WRITER> AtmosphericCO2s
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Models.Monica.EnvironmentParameters.YearToValue.WRITER>>(0);
                set => Link(0, value);
            }

            public double AtmosphericO3
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }

            public ListOfStructsSerializer<Mas.Models.Monica.EnvironmentParameters.YearToValue.WRITER> AtmosphericO3s
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Models.Monica.EnvironmentParameters.YearToValue.WRITER>>(1);
                set => Link(1, value);
            }

            public double WindSpeedHeight
            {
                get => this.ReadDataDouble(192UL, 2);
                set => this.WriteData(192UL, value, 2);
            }

            public double LeachingDepth
            {
                get => this.ReadDataDouble(256UL, 0);
                set => this.WriteData(256UL, value, 0);
            }

            public double TimeStep
            {
                get => this.ReadDataDouble(320UL, 0);
                set => this.WriteData(320UL, value, 0);
            }

            public double MaxGroundwaterDepth
            {
                get => this.ReadDataDouble(384UL, 18);
                set => this.WriteData(384UL, value, 18);
            }

            public double MinGroundwaterDepth
            {
                get => this.ReadDataDouble(448UL, 20);
                set => this.WriteData(448UL, value, 20);
            }

            public byte MinGroundwaterDepthMonth
            {
                get => this.ReadDataByte(512UL, (byte)3);
                set => this.WriteData(512UL, value, (byte)3);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe68d439455fd9cceUL)]
        public class YearToValue : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe68d439455fd9cceUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Year = reader.Year;
                Value = reader.Value;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Year = Year;
                writer.Value = Value;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public ushort Year
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
                public ushort Year => ctx.ReadDataUShort(0UL, (ushort)0);
                public double Value => ctx.ReadDataDouble(64UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 0);
                }

                public ushort Year
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

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc1092d6c4c110e29UL)]
    public class MeasuredGroundwaterTableInformation : ICapnpSerializable
    {
        public const UInt64 typeId = 0xc1092d6c4c110e29UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            GroundwaterInformationAvailable = reader.GroundwaterInformationAvailable;
            GroundwaterInfo = reader.GroundwaterInfo?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Models.Monica.MeasuredGroundwaterTableInformation.DateToValue>(_));
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.GroundwaterInformationAvailable = GroundwaterInformationAvailable;
            writer.GroundwaterInfo.Init(GroundwaterInfo, (_s1, _v1) => _v1?.serialize(_s1));
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public bool GroundwaterInformationAvailable
        {
            get;
            set;
        }

        public IReadOnlyList<Mas.Models.Monica.MeasuredGroundwaterTableInformation.DateToValue> GroundwaterInfo
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
            public bool GroundwaterInformationAvailable => ctx.ReadDataBool(0UL, false);
            public IReadOnlyList<Mas.Models.Monica.MeasuredGroundwaterTableInformation.DateToValue.READER> GroundwaterInfo => ctx.ReadList(0).Cast(Mas.Models.Monica.MeasuredGroundwaterTableInformation.DateToValue.READER.create);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(1, 1);
            }

            public bool GroundwaterInformationAvailable
            {
                get => this.ReadDataBool(0UL, false);
                set => this.WriteData(0UL, value, false);
            }

            public ListOfStructsSerializer<Mas.Models.Monica.MeasuredGroundwaterTableInformation.DateToValue.WRITER> GroundwaterInfo
            {
                get => BuildPointer<ListOfStructsSerializer<Mas.Models.Monica.MeasuredGroundwaterTableInformation.DateToValue.WRITER>>(0);
                set => Link(0, value);
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x81b8ffeeb01d76f7UL)]
        public class DateToValue : ICapnpSerializable
        {
            public const UInt64 typeId = 0x81b8ffeeb01d76f7UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Date = CapnpSerializable.Create<Mas.Common.Date>(reader.Date);
                Value = reader.Value;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Date?.serialize(writer.Date);
                writer.Value = Value;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public Mas.Common.Date Date
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
                public Mas.Common.Date.READER Date => ctx.ReadStruct(0, Mas.Common.Date.READER.create);
                public double Value => ctx.ReadDataDouble(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public Mas.Common.Date.WRITER Date
                {
                    get => BuildPointer<Mas.Common.Date.WRITER>(0);
                    set => Link(0, value);
                }

                public double Value
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xffac0fa5c7156a5dUL)]
    public class SimulationParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0xffac0fa5c7156a5dUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            StartDate = CapnpSerializable.Create<Mas.Common.Date>(reader.StartDate);
            EndDate = CapnpSerializable.Create<Mas.Common.Date>(reader.EndDate);
            NitrogenResponseOn = reader.NitrogenResponseOn;
            WaterDeficitResponseOn = reader.WaterDeficitResponseOn;
            EmergenceFloodingControlOn = reader.EmergenceFloodingControlOn;
            EmergenceMoistureControlOn = reader.EmergenceMoistureControlOn;
            FrostKillOn = reader.FrostKillOn;
            UseAutomaticIrrigation = reader.UseAutomaticIrrigation;
            AutoIrrigationParams = CapnpSerializable.Create<Mas.Models.Monica.AutomaticIrrigationParameters>(reader.AutoIrrigationParams);
            UseNMinMineralFertilisingMethod = reader.UseNMinMineralFertilisingMethod;
            NMinFertiliserPartition = CapnpSerializable.Create<Mas.Rpc.Management.Params.MineralFertilization.Parameters>(reader.NMinFertiliserPartition);
            NMinApplicationParams = CapnpSerializable.Create<Mas.Models.Monica.NMinApplicationParameters>(reader.NMinApplicationParams);
            UseSecondaryYields = reader.UseSecondaryYields;
            UseAutomaticHarvestTrigger = reader.UseAutomaticHarvestTrigger;
            NumberOfLayers = reader.NumberOfLayers;
            LayerThickness = reader.LayerThickness;
            StartPVIndex = reader.StartPVIndex;
            JulianDayAutomaticFertilising = reader.JulianDayAutomaticFertilising;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            StartDate?.serialize(writer.StartDate);
            EndDate?.serialize(writer.EndDate);
            writer.NitrogenResponseOn = NitrogenResponseOn;
            writer.WaterDeficitResponseOn = WaterDeficitResponseOn;
            writer.EmergenceFloodingControlOn = EmergenceFloodingControlOn;
            writer.EmergenceMoistureControlOn = EmergenceMoistureControlOn;
            writer.FrostKillOn = FrostKillOn;
            writer.UseAutomaticIrrigation = UseAutomaticIrrigation;
            AutoIrrigationParams?.serialize(writer.AutoIrrigationParams);
            writer.UseNMinMineralFertilisingMethod = UseNMinMineralFertilisingMethod;
            NMinFertiliserPartition?.serialize(writer.NMinFertiliserPartition);
            NMinApplicationParams?.serialize(writer.NMinApplicationParams);
            writer.UseSecondaryYields = UseSecondaryYields;
            writer.UseAutomaticHarvestTrigger = UseAutomaticHarvestTrigger;
            writer.NumberOfLayers = NumberOfLayers;
            writer.LayerThickness = LayerThickness;
            writer.StartPVIndex = StartPVIndex;
            writer.JulianDayAutomaticFertilising = JulianDayAutomaticFertilising;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public Mas.Common.Date StartDate
        {
            get;
            set;
        }

        public Mas.Common.Date EndDate
        {
            get;
            set;
        }

        public bool NitrogenResponseOn
        {
            get;
            set;
        }

        = true;
        public bool WaterDeficitResponseOn
        {
            get;
            set;
        }

        = true;
        public bool EmergenceFloodingControlOn
        {
            get;
            set;
        }

        = true;
        public bool EmergenceMoistureControlOn
        {
            get;
            set;
        }

        = true;
        public bool FrostKillOn
        {
            get;
            set;
        }

        = true;
        public bool UseAutomaticIrrigation
        {
            get;
            set;
        }

        public Mas.Models.Monica.AutomaticIrrigationParameters AutoIrrigationParams
        {
            get;
            set;
        }

        public bool UseNMinMineralFertilisingMethod
        {
            get;
            set;
        }

        public Mas.Rpc.Management.Params.MineralFertilization.Parameters NMinFertiliserPartition
        {
            get;
            set;
        }

        public Mas.Models.Monica.NMinApplicationParameters NMinApplicationParams
        {
            get;
            set;
        }

        public bool UseSecondaryYields
        {
            get;
            set;
        }

        = true;
        public bool UseAutomaticHarvestTrigger
        {
            get;
            set;
        }

        public ushort NumberOfLayers
        {
            get;
            set;
        }

        = 20;
        public double LayerThickness
        {
            get;
            set;
        }

        = 0.1;
        public ushort StartPVIndex
        {
            get;
            set;
        }

        public ushort JulianDayAutomaticFertilising
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
            public Mas.Common.Date.READER StartDate => ctx.ReadStruct(0, Mas.Common.Date.READER.create);
            public Mas.Common.Date.READER EndDate => ctx.ReadStruct(1, Mas.Common.Date.READER.create);
            public bool NitrogenResponseOn => ctx.ReadDataBool(0UL, true);
            public bool WaterDeficitResponseOn => ctx.ReadDataBool(1UL, true);
            public bool EmergenceFloodingControlOn => ctx.ReadDataBool(2UL, true);
            public bool EmergenceMoistureControlOn => ctx.ReadDataBool(3UL, true);
            public bool FrostKillOn => ctx.ReadDataBool(4UL, true);
            public bool UseAutomaticIrrigation => ctx.ReadDataBool(5UL, false);
            public Mas.Models.Monica.AutomaticIrrigationParameters.READER AutoIrrigationParams => ctx.ReadStruct(2, Mas.Models.Monica.AutomaticIrrigationParameters.READER.create);
            public bool UseNMinMineralFertilisingMethod => ctx.ReadDataBool(6UL, false);
            public Mas.Rpc.Management.Params.MineralFertilization.Parameters.READER NMinFertiliserPartition => ctx.ReadStruct(3, Mas.Rpc.Management.Params.MineralFertilization.Parameters.READER.create);
            public Mas.Models.Monica.NMinApplicationParameters.READER NMinApplicationParams => ctx.ReadStruct(4, Mas.Models.Monica.NMinApplicationParameters.READER.create);
            public bool UseSecondaryYields => ctx.ReadDataBool(7UL, true);
            public bool UseAutomaticHarvestTrigger => ctx.ReadDataBool(8UL, false);
            public ushort NumberOfLayers => ctx.ReadDataUShort(16UL, (ushort)20);
            public double LayerThickness => ctx.ReadDataDouble(64UL, 0.1);
            public ushort StartPVIndex => ctx.ReadDataUShort(32UL, (ushort)0);
            public ushort JulianDayAutomaticFertilising => ctx.ReadDataUShort(48UL, (ushort)0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(2, 5);
            }

            public Mas.Common.Date.WRITER StartDate
            {
                get => BuildPointer<Mas.Common.Date.WRITER>(0);
                set => Link(0, value);
            }

            public Mas.Common.Date.WRITER EndDate
            {
                get => BuildPointer<Mas.Common.Date.WRITER>(1);
                set => Link(1, value);
            }

            public bool NitrogenResponseOn
            {
                get => this.ReadDataBool(0UL, true);
                set => this.WriteData(0UL, value, true);
            }

            public bool WaterDeficitResponseOn
            {
                get => this.ReadDataBool(1UL, true);
                set => this.WriteData(1UL, value, true);
            }

            public bool EmergenceFloodingControlOn
            {
                get => this.ReadDataBool(2UL, true);
                set => this.WriteData(2UL, value, true);
            }

            public bool EmergenceMoistureControlOn
            {
                get => this.ReadDataBool(3UL, true);
                set => this.WriteData(3UL, value, true);
            }

            public bool FrostKillOn
            {
                get => this.ReadDataBool(4UL, true);
                set => this.WriteData(4UL, value, true);
            }

            public bool UseAutomaticIrrigation
            {
                get => this.ReadDataBool(5UL, false);
                set => this.WriteData(5UL, value, false);
            }

            public Mas.Models.Monica.AutomaticIrrigationParameters.WRITER AutoIrrigationParams
            {
                get => BuildPointer<Mas.Models.Monica.AutomaticIrrigationParameters.WRITER>(2);
                set => Link(2, value);
            }

            public bool UseNMinMineralFertilisingMethod
            {
                get => this.ReadDataBool(6UL, false);
                set => this.WriteData(6UL, value, false);
            }

            public Mas.Rpc.Management.Params.MineralFertilization.Parameters.WRITER NMinFertiliserPartition
            {
                get => BuildPointer<Mas.Rpc.Management.Params.MineralFertilization.Parameters.WRITER>(3);
                set => Link(3, value);
            }

            public Mas.Models.Monica.NMinApplicationParameters.WRITER NMinApplicationParams
            {
                get => BuildPointer<Mas.Models.Monica.NMinApplicationParameters.WRITER>(4);
                set => Link(4, value);
            }

            public bool UseSecondaryYields
            {
                get => this.ReadDataBool(7UL, true);
                set => this.WriteData(7UL, value, true);
            }

            public bool UseAutomaticHarvestTrigger
            {
                get => this.ReadDataBool(8UL, false);
                set => this.WriteData(8UL, value, false);
            }

            public ushort NumberOfLayers
            {
                get => this.ReadDataUShort(16UL, (ushort)20);
                set => this.WriteData(16UL, value, (ushort)20);
            }

            public double LayerThickness
            {
                get => this.ReadDataDouble(64UL, 0.1);
                set => this.WriteData(64UL, value, 0.1);
            }

            public ushort StartPVIndex
            {
                get => this.ReadDataUShort(32UL, (ushort)0);
                set => this.WriteData(32UL, value, (ushort)0);
            }

            public ushort JulianDayAutomaticFertilising
            {
                get => this.ReadDataUShort(48UL, (ushort)0);
                set => this.WriteData(48UL, value, (ushort)0);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe4d6d0d9ae1553daUL)]
    public class CropModuleParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0xe4d6d0d9ae1553daUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            CanopyReflectionCoefficient = reader.CanopyReflectionCoefficient;
            ReferenceMaxAssimilationRate = reader.ReferenceMaxAssimilationRate;
            ReferenceLeafAreaIndex = reader.ReferenceLeafAreaIndex;
            MaintenanceRespirationParameter1 = reader.MaintenanceRespirationParameter1;
            MaintenanceRespirationParameter2 = reader.MaintenanceRespirationParameter2;
            MinimumNConcentrationRoot = reader.MinimumNConcentrationRoot;
            MinimumAvailableN = reader.MinimumAvailableN;
            ReferenceAlbedo = reader.ReferenceAlbedo;
            StomataConductanceAlpha = reader.StomataConductanceAlpha;
            SaturationBeta = reader.SaturationBeta;
            GrowthRespirationRedux = reader.GrowthRespirationRedux;
            MaxCropNDemand = reader.MaxCropNDemand;
            GrowthRespirationParameter1 = reader.GrowthRespirationParameter1;
            GrowthRespirationParameter2 = reader.GrowthRespirationParameter2;
            Tortuosity = reader.Tortuosity;
            AdjustRootDepthForSoilProps = reader.AdjustRootDepthForSoilProps;
            ExperimentalEnablePhenologyWangEngelTemperatureResponse = reader.ExperimentalEnablePhenologyWangEngelTemperatureResponse;
            ExperimentalEnablePhotosynthesisWangEngelTemperatureResponse = reader.ExperimentalEnablePhotosynthesisWangEngelTemperatureResponse;
            ExperimentalEnableHourlyFvCBPhotosynthesis = reader.ExperimentalEnableHourlyFvCBPhotosynthesis;
            ExperimentalEnableTResponseLeafExpansion = reader.ExperimentalEnableTResponseLeafExpansion;
            ExperimentalDisableDailyRootBiomassToSoil = reader.ExperimentalDisableDailyRootBiomassToSoil;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.CanopyReflectionCoefficient = CanopyReflectionCoefficient;
            writer.ReferenceMaxAssimilationRate = ReferenceMaxAssimilationRate;
            writer.ReferenceLeafAreaIndex = ReferenceLeafAreaIndex;
            writer.MaintenanceRespirationParameter1 = MaintenanceRespirationParameter1;
            writer.MaintenanceRespirationParameter2 = MaintenanceRespirationParameter2;
            writer.MinimumNConcentrationRoot = MinimumNConcentrationRoot;
            writer.MinimumAvailableN = MinimumAvailableN;
            writer.ReferenceAlbedo = ReferenceAlbedo;
            writer.StomataConductanceAlpha = StomataConductanceAlpha;
            writer.SaturationBeta = SaturationBeta;
            writer.GrowthRespirationRedux = GrowthRespirationRedux;
            writer.MaxCropNDemand = MaxCropNDemand;
            writer.GrowthRespirationParameter1 = GrowthRespirationParameter1;
            writer.GrowthRespirationParameter2 = GrowthRespirationParameter2;
            writer.Tortuosity = Tortuosity;
            writer.AdjustRootDepthForSoilProps = AdjustRootDepthForSoilProps;
            writer.ExperimentalEnablePhenologyWangEngelTemperatureResponse = ExperimentalEnablePhenologyWangEngelTemperatureResponse;
            writer.ExperimentalEnablePhotosynthesisWangEngelTemperatureResponse = ExperimentalEnablePhotosynthesisWangEngelTemperatureResponse;
            writer.ExperimentalEnableHourlyFvCBPhotosynthesis = ExperimentalEnableHourlyFvCBPhotosynthesis;
            writer.ExperimentalEnableTResponseLeafExpansion = ExperimentalEnableTResponseLeafExpansion;
            writer.ExperimentalDisableDailyRootBiomassToSoil = ExperimentalDisableDailyRootBiomassToSoil;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public double CanopyReflectionCoefficient
        {
            get;
            set;
        }

        public double ReferenceMaxAssimilationRate
        {
            get;
            set;
        }

        public double ReferenceLeafAreaIndex
        {
            get;
            set;
        }

        public double MaintenanceRespirationParameter1
        {
            get;
            set;
        }

        public double MaintenanceRespirationParameter2
        {
            get;
            set;
        }

        public double MinimumNConcentrationRoot
        {
            get;
            set;
        }

        public double MinimumAvailableN
        {
            get;
            set;
        }

        public double ReferenceAlbedo
        {
            get;
            set;
        }

        public double StomataConductanceAlpha
        {
            get;
            set;
        }

        public double SaturationBeta
        {
            get;
            set;
        }

        public double GrowthRespirationRedux
        {
            get;
            set;
        }

        public double MaxCropNDemand
        {
            get;
            set;
        }

        public double GrowthRespirationParameter1
        {
            get;
            set;
        }

        public double GrowthRespirationParameter2
        {
            get;
            set;
        }

        public double Tortuosity
        {
            get;
            set;
        }

        public bool AdjustRootDepthForSoilProps
        {
            get;
            set;
        }

        public bool ExperimentalEnablePhenologyWangEngelTemperatureResponse
        {
            get;
            set;
        }

        public bool ExperimentalEnablePhotosynthesisWangEngelTemperatureResponse
        {
            get;
            set;
        }

        public bool ExperimentalEnableHourlyFvCBPhotosynthesis
        {
            get;
            set;
        }

        public bool ExperimentalEnableTResponseLeafExpansion
        {
            get;
            set;
        }

        public bool ExperimentalDisableDailyRootBiomassToSoil
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
            public double CanopyReflectionCoefficient => ctx.ReadDataDouble(0UL, 0);
            public double ReferenceMaxAssimilationRate => ctx.ReadDataDouble(64UL, 0);
            public double ReferenceLeafAreaIndex => ctx.ReadDataDouble(128UL, 0);
            public double MaintenanceRespirationParameter1 => ctx.ReadDataDouble(192UL, 0);
            public double MaintenanceRespirationParameter2 => ctx.ReadDataDouble(256UL, 0);
            public double MinimumNConcentrationRoot => ctx.ReadDataDouble(320UL, 0);
            public double MinimumAvailableN => ctx.ReadDataDouble(384UL, 0);
            public double ReferenceAlbedo => ctx.ReadDataDouble(448UL, 0);
            public double StomataConductanceAlpha => ctx.ReadDataDouble(512UL, 0);
            public double SaturationBeta => ctx.ReadDataDouble(576UL, 0);
            public double GrowthRespirationRedux => ctx.ReadDataDouble(640UL, 0);
            public double MaxCropNDemand => ctx.ReadDataDouble(704UL, 0);
            public double GrowthRespirationParameter1 => ctx.ReadDataDouble(768UL, 0);
            public double GrowthRespirationParameter2 => ctx.ReadDataDouble(832UL, 0);
            public double Tortuosity => ctx.ReadDataDouble(896UL, 0);
            public bool AdjustRootDepthForSoilProps => ctx.ReadDataBool(960UL, false);
            public bool ExperimentalEnablePhenologyWangEngelTemperatureResponse => ctx.ReadDataBool(961UL, false);
            public bool ExperimentalEnablePhotosynthesisWangEngelTemperatureResponse => ctx.ReadDataBool(962UL, false);
            public bool ExperimentalEnableHourlyFvCBPhotosynthesis => ctx.ReadDataBool(963UL, false);
            public bool ExperimentalEnableTResponseLeafExpansion => ctx.ReadDataBool(964UL, false);
            public bool ExperimentalDisableDailyRootBiomassToSoil => ctx.ReadDataBool(965UL, false);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(16, 0);
            }

            public double CanopyReflectionCoefficient
            {
                get => this.ReadDataDouble(0UL, 0);
                set => this.WriteData(0UL, value, 0);
            }

            public double ReferenceMaxAssimilationRate
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public double ReferenceLeafAreaIndex
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }

            public double MaintenanceRespirationParameter1
            {
                get => this.ReadDataDouble(192UL, 0);
                set => this.WriteData(192UL, value, 0);
            }

            public double MaintenanceRespirationParameter2
            {
                get => this.ReadDataDouble(256UL, 0);
                set => this.WriteData(256UL, value, 0);
            }

            public double MinimumNConcentrationRoot
            {
                get => this.ReadDataDouble(320UL, 0);
                set => this.WriteData(320UL, value, 0);
            }

            public double MinimumAvailableN
            {
                get => this.ReadDataDouble(384UL, 0);
                set => this.WriteData(384UL, value, 0);
            }

            public double ReferenceAlbedo
            {
                get => this.ReadDataDouble(448UL, 0);
                set => this.WriteData(448UL, value, 0);
            }

            public double StomataConductanceAlpha
            {
                get => this.ReadDataDouble(512UL, 0);
                set => this.WriteData(512UL, value, 0);
            }

            public double SaturationBeta
            {
                get => this.ReadDataDouble(576UL, 0);
                set => this.WriteData(576UL, value, 0);
            }

            public double GrowthRespirationRedux
            {
                get => this.ReadDataDouble(640UL, 0);
                set => this.WriteData(640UL, value, 0);
            }

            public double MaxCropNDemand
            {
                get => this.ReadDataDouble(704UL, 0);
                set => this.WriteData(704UL, value, 0);
            }

            public double GrowthRespirationParameter1
            {
                get => this.ReadDataDouble(768UL, 0);
                set => this.WriteData(768UL, value, 0);
            }

            public double GrowthRespirationParameter2
            {
                get => this.ReadDataDouble(832UL, 0);
                set => this.WriteData(832UL, value, 0);
            }

            public double Tortuosity
            {
                get => this.ReadDataDouble(896UL, 0);
                set => this.WriteData(896UL, value, 0);
            }

            public bool AdjustRootDepthForSoilProps
            {
                get => this.ReadDataBool(960UL, false);
                set => this.WriteData(960UL, value, false);
            }

            public bool ExperimentalEnablePhenologyWangEngelTemperatureResponse
            {
                get => this.ReadDataBool(961UL, false);
                set => this.WriteData(961UL, value, false);
            }

            public bool ExperimentalEnablePhotosynthesisWangEngelTemperatureResponse
            {
                get => this.ReadDataBool(962UL, false);
                set => this.WriteData(962UL, value, false);
            }

            public bool ExperimentalEnableHourlyFvCBPhotosynthesis
            {
                get => this.ReadDataBool(963UL, false);
                set => this.WriteData(963UL, value, false);
            }

            public bool ExperimentalEnableTResponseLeafExpansion
            {
                get => this.ReadDataBool(964UL, false);
                set => this.WriteData(964UL, value, false);
            }

            public bool ExperimentalDisableDailyRootBiomassToSoil
            {
                get => this.ReadDataBool(965UL, false);
                set => this.WriteData(965UL, value, false);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcdff1b0306ea58cfUL)]
    public class SoilMoistureModuleParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0xcdff1b0306ea58cfUL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            CriticalMoistureDepth = reader.CriticalMoistureDepth;
            SaturatedHydraulicConductivity = reader.SaturatedHydraulicConductivity;
            SurfaceRoughness = reader.SurfaceRoughness;
            GroundwaterDischarge = reader.GroundwaterDischarge;
            HydraulicConductivityRedux = reader.HydraulicConductivityRedux;
            SnowAccumulationTresholdTemperature = reader.SnowAccumulationTresholdTemperature;
            KcFactor = reader.KcFactor;
            TemperatureLimitForLiquidWater = reader.TemperatureLimitForLiquidWater;
            CorrectionSnow = reader.CorrectionSnow;
            CorrectionRain = reader.CorrectionRain;
            SnowMaxAdditionalDensity = reader.SnowMaxAdditionalDensity;
            NewSnowDensityMin = reader.NewSnowDensityMin;
            SnowRetentionCapacityMin = reader.SnowRetentionCapacityMin;
            RefreezeParameter1 = reader.RefreezeParameter1;
            RefreezeParameter2 = reader.RefreezeParameter2;
            RefreezeTemperature = reader.RefreezeTemperature;
            SnowMeltTemperature = reader.SnowMeltTemperature;
            SnowPacking = reader.SnowPacking;
            SnowRetentionCapacityMax = reader.SnowRetentionCapacityMax;
            EvaporationZeta = reader.EvaporationZeta;
            XsaCriticalSoilMoisture = reader.XsaCriticalSoilMoisture;
            MaximumEvaporationImpactDepth = reader.MaximumEvaporationImpactDepth;
            MaxPercolationRate = reader.MaxPercolationRate;
            MoistureInitValue = reader.MoistureInitValue;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.CriticalMoistureDepth = CriticalMoistureDepth;
            writer.SaturatedHydraulicConductivity = SaturatedHydraulicConductivity;
            writer.SurfaceRoughness = SurfaceRoughness;
            writer.GroundwaterDischarge = GroundwaterDischarge;
            writer.HydraulicConductivityRedux = HydraulicConductivityRedux;
            writer.SnowAccumulationTresholdTemperature = SnowAccumulationTresholdTemperature;
            writer.KcFactor = KcFactor;
            writer.TemperatureLimitForLiquidWater = TemperatureLimitForLiquidWater;
            writer.CorrectionSnow = CorrectionSnow;
            writer.CorrectionRain = CorrectionRain;
            writer.SnowMaxAdditionalDensity = SnowMaxAdditionalDensity;
            writer.NewSnowDensityMin = NewSnowDensityMin;
            writer.SnowRetentionCapacityMin = SnowRetentionCapacityMin;
            writer.RefreezeParameter1 = RefreezeParameter1;
            writer.RefreezeParameter2 = RefreezeParameter2;
            writer.RefreezeTemperature = RefreezeTemperature;
            writer.SnowMeltTemperature = SnowMeltTemperature;
            writer.SnowPacking = SnowPacking;
            writer.SnowRetentionCapacityMax = SnowRetentionCapacityMax;
            writer.EvaporationZeta = EvaporationZeta;
            writer.XsaCriticalSoilMoisture = XsaCriticalSoilMoisture;
            writer.MaximumEvaporationImpactDepth = MaximumEvaporationImpactDepth;
            writer.MaxPercolationRate = MaxPercolationRate;
            writer.MoistureInitValue = MoistureInitValue;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public double CriticalMoistureDepth
        {
            get;
            set;
        }

        public double SaturatedHydraulicConductivity
        {
            get;
            set;
        }

        public double SurfaceRoughness
        {
            get;
            set;
        }

        public double GroundwaterDischarge
        {
            get;
            set;
        }

        public double HydraulicConductivityRedux
        {
            get;
            set;
        }

        public double SnowAccumulationTresholdTemperature
        {
            get;
            set;
        }

        public double KcFactor
        {
            get;
            set;
        }

        public double TemperatureLimitForLiquidWater
        {
            get;
            set;
        }

        public double CorrectionSnow
        {
            get;
            set;
        }

        public double CorrectionRain
        {
            get;
            set;
        }

        public double SnowMaxAdditionalDensity
        {
            get;
            set;
        }

        public double NewSnowDensityMin
        {
            get;
            set;
        }

        public double SnowRetentionCapacityMin
        {
            get;
            set;
        }

        public double RefreezeParameter1
        {
            get;
            set;
        }

        public double RefreezeParameter2
        {
            get;
            set;
        }

        public double RefreezeTemperature
        {
            get;
            set;
        }

        public double SnowMeltTemperature
        {
            get;
            set;
        }

        public double SnowPacking
        {
            get;
            set;
        }

        public double SnowRetentionCapacityMax
        {
            get;
            set;
        }

        public double EvaporationZeta
        {
            get;
            set;
        }

        public double XsaCriticalSoilMoisture
        {
            get;
            set;
        }

        public double MaximumEvaporationImpactDepth
        {
            get;
            set;
        }

        public double MaxPercolationRate
        {
            get;
            set;
        }

        public double MoistureInitValue
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
            public double CriticalMoistureDepth => ctx.ReadDataDouble(0UL, 0);
            public double SaturatedHydraulicConductivity => ctx.ReadDataDouble(64UL, 0);
            public double SurfaceRoughness => ctx.ReadDataDouble(128UL, 0);
            public double GroundwaterDischarge => ctx.ReadDataDouble(192UL, 0);
            public double HydraulicConductivityRedux => ctx.ReadDataDouble(256UL, 0);
            public double SnowAccumulationTresholdTemperature => ctx.ReadDataDouble(320UL, 0);
            public double KcFactor => ctx.ReadDataDouble(384UL, 0);
            public double TemperatureLimitForLiquidWater => ctx.ReadDataDouble(448UL, 0);
            public double CorrectionSnow => ctx.ReadDataDouble(512UL, 0);
            public double CorrectionRain => ctx.ReadDataDouble(576UL, 0);
            public double SnowMaxAdditionalDensity => ctx.ReadDataDouble(640UL, 0);
            public double NewSnowDensityMin => ctx.ReadDataDouble(704UL, 0);
            public double SnowRetentionCapacityMin => ctx.ReadDataDouble(768UL, 0);
            public double RefreezeParameter1 => ctx.ReadDataDouble(832UL, 0);
            public double RefreezeParameter2 => ctx.ReadDataDouble(896UL, 0);
            public double RefreezeTemperature => ctx.ReadDataDouble(960UL, 0);
            public double SnowMeltTemperature => ctx.ReadDataDouble(1024UL, 0);
            public double SnowPacking => ctx.ReadDataDouble(1088UL, 0);
            public double SnowRetentionCapacityMax => ctx.ReadDataDouble(1152UL, 0);
            public double EvaporationZeta => ctx.ReadDataDouble(1216UL, 0);
            public double XsaCriticalSoilMoisture => ctx.ReadDataDouble(1280UL, 0);
            public double MaximumEvaporationImpactDepth => ctx.ReadDataDouble(1344UL, 0);
            public double MaxPercolationRate => ctx.ReadDataDouble(1408UL, 0);
            public double MoistureInitValue => ctx.ReadDataDouble(1472UL, 0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(24, 0);
            }

            public double CriticalMoistureDepth
            {
                get => this.ReadDataDouble(0UL, 0);
                set => this.WriteData(0UL, value, 0);
            }

            public double SaturatedHydraulicConductivity
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public double SurfaceRoughness
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }

            public double GroundwaterDischarge
            {
                get => this.ReadDataDouble(192UL, 0);
                set => this.WriteData(192UL, value, 0);
            }

            public double HydraulicConductivityRedux
            {
                get => this.ReadDataDouble(256UL, 0);
                set => this.WriteData(256UL, value, 0);
            }

            public double SnowAccumulationTresholdTemperature
            {
                get => this.ReadDataDouble(320UL, 0);
                set => this.WriteData(320UL, value, 0);
            }

            public double KcFactor
            {
                get => this.ReadDataDouble(384UL, 0);
                set => this.WriteData(384UL, value, 0);
            }

            public double TemperatureLimitForLiquidWater
            {
                get => this.ReadDataDouble(448UL, 0);
                set => this.WriteData(448UL, value, 0);
            }

            public double CorrectionSnow
            {
                get => this.ReadDataDouble(512UL, 0);
                set => this.WriteData(512UL, value, 0);
            }

            public double CorrectionRain
            {
                get => this.ReadDataDouble(576UL, 0);
                set => this.WriteData(576UL, value, 0);
            }

            public double SnowMaxAdditionalDensity
            {
                get => this.ReadDataDouble(640UL, 0);
                set => this.WriteData(640UL, value, 0);
            }

            public double NewSnowDensityMin
            {
                get => this.ReadDataDouble(704UL, 0);
                set => this.WriteData(704UL, value, 0);
            }

            public double SnowRetentionCapacityMin
            {
                get => this.ReadDataDouble(768UL, 0);
                set => this.WriteData(768UL, value, 0);
            }

            public double RefreezeParameter1
            {
                get => this.ReadDataDouble(832UL, 0);
                set => this.WriteData(832UL, value, 0);
            }

            public double RefreezeParameter2
            {
                get => this.ReadDataDouble(896UL, 0);
                set => this.WriteData(896UL, value, 0);
            }

            public double RefreezeTemperature
            {
                get => this.ReadDataDouble(960UL, 0);
                set => this.WriteData(960UL, value, 0);
            }

            public double SnowMeltTemperature
            {
                get => this.ReadDataDouble(1024UL, 0);
                set => this.WriteData(1024UL, value, 0);
            }

            public double SnowPacking
            {
                get => this.ReadDataDouble(1088UL, 0);
                set => this.WriteData(1088UL, value, 0);
            }

            public double SnowRetentionCapacityMax
            {
                get => this.ReadDataDouble(1152UL, 0);
                set => this.WriteData(1152UL, value, 0);
            }

            public double EvaporationZeta
            {
                get => this.ReadDataDouble(1216UL, 0);
                set => this.WriteData(1216UL, value, 0);
            }

            public double XsaCriticalSoilMoisture
            {
                get => this.ReadDataDouble(1280UL, 0);
                set => this.WriteData(1280UL, value, 0);
            }

            public double MaximumEvaporationImpactDepth
            {
                get => this.ReadDataDouble(1344UL, 0);
                set => this.WriteData(1344UL, value, 0);
            }

            public double MaxPercolationRate
            {
                get => this.ReadDataDouble(1408UL, 0);
                set => this.WriteData(1408UL, value, 0);
            }

            public double MoistureInitValue
            {
                get => this.ReadDataDouble(1472UL, 0);
                set => this.WriteData(1472UL, value, 0);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb3e73f8c19afd787UL)]
    public class SoilOrganicModuleParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0xb3e73f8c19afd787UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            SomSlowDecCoeffStandard = reader.SomSlowDecCoeffStandard;
            SomFastDecCoeffStandard = reader.SomFastDecCoeffStandard;
            SmbSlowMaintRateStandard = reader.SmbSlowMaintRateStandard;
            SmbFastMaintRateStandard = reader.SmbFastMaintRateStandard;
            SmbSlowDeathRateStandard = reader.SmbSlowDeathRateStandard;
            SmbFastDeathRateStandard = reader.SmbFastDeathRateStandard;
            SmbUtilizationEfficiency = reader.SmbUtilizationEfficiency;
            SomSlowUtilizationEfficiency = reader.SomSlowUtilizationEfficiency;
            SomFastUtilizationEfficiency = reader.SomFastUtilizationEfficiency;
            AomSlowUtilizationEfficiency = reader.AomSlowUtilizationEfficiency;
            AomFastUtilizationEfficiency = reader.AomFastUtilizationEfficiency;
            AomFastMaxCtoN = reader.AomFastMaxCtoN;
            PartSOMFastToSOMSlow = reader.PartSOMFastToSOMSlow;
            PartSMBSlowToSOMFast = reader.PartSMBSlowToSOMFast;
            PartSMBFastToSOMFast = reader.PartSMBFastToSOMFast;
            PartSOMToSMBSlow = reader.PartSOMToSMBSlow;
            PartSOMToSMBFast = reader.PartSOMToSMBFast;
            CnRatioSMB = reader.CnRatioSMB;
            LimitClayEffect = reader.LimitClayEffect;
            AmmoniaOxidationRateCoeffStandard = reader.AmmoniaOxidationRateCoeffStandard;
            NitriteOxidationRateCoeffStandard = reader.NitriteOxidationRateCoeffStandard;
            TransportRateCoeff = reader.TransportRateCoeff;
            SpecAnaerobDenitrification = reader.SpecAnaerobDenitrification;
            ImmobilisationRateCoeffNO3 = reader.ImmobilisationRateCoeffNO3;
            ImmobilisationRateCoeffNH4 = reader.ImmobilisationRateCoeffNH4;
            Denit1 = reader.Denit1;
            Denit2 = reader.Denit2;
            Denit3 = reader.Denit3;
            HydrolysisKM = reader.HydrolysisKM;
            ActivationEnergy = reader.ActivationEnergy;
            HydrolysisP1 = reader.HydrolysisP1;
            HydrolysisP2 = reader.HydrolysisP2;
            AtmosphericResistance = reader.AtmosphericResistance;
            N2oProductionRate = reader.N2oProductionRate;
            InhibitorNH3 = reader.InhibitorNH3;
            PsMaxMineralisationDepth = reader.PsMaxMineralisationDepth;
            SticsParams = CapnpSerializable.Create<Mas.Models.Monica.SticsParameters>(reader.SticsParams);
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.SomSlowDecCoeffStandard = SomSlowDecCoeffStandard;
            writer.SomFastDecCoeffStandard = SomFastDecCoeffStandard;
            writer.SmbSlowMaintRateStandard = SmbSlowMaintRateStandard;
            writer.SmbFastMaintRateStandard = SmbFastMaintRateStandard;
            writer.SmbSlowDeathRateStandard = SmbSlowDeathRateStandard;
            writer.SmbFastDeathRateStandard = SmbFastDeathRateStandard;
            writer.SmbUtilizationEfficiency = SmbUtilizationEfficiency;
            writer.SomSlowUtilizationEfficiency = SomSlowUtilizationEfficiency;
            writer.SomFastUtilizationEfficiency = SomFastUtilizationEfficiency;
            writer.AomSlowUtilizationEfficiency = AomSlowUtilizationEfficiency;
            writer.AomFastUtilizationEfficiency = AomFastUtilizationEfficiency;
            writer.AomFastMaxCtoN = AomFastMaxCtoN;
            writer.PartSOMFastToSOMSlow = PartSOMFastToSOMSlow;
            writer.PartSMBSlowToSOMFast = PartSMBSlowToSOMFast;
            writer.PartSMBFastToSOMFast = PartSMBFastToSOMFast;
            writer.PartSOMToSMBSlow = PartSOMToSMBSlow;
            writer.PartSOMToSMBFast = PartSOMToSMBFast;
            writer.CnRatioSMB = CnRatioSMB;
            writer.LimitClayEffect = LimitClayEffect;
            writer.AmmoniaOxidationRateCoeffStandard = AmmoniaOxidationRateCoeffStandard;
            writer.NitriteOxidationRateCoeffStandard = NitriteOxidationRateCoeffStandard;
            writer.TransportRateCoeff = TransportRateCoeff;
            writer.SpecAnaerobDenitrification = SpecAnaerobDenitrification;
            writer.ImmobilisationRateCoeffNO3 = ImmobilisationRateCoeffNO3;
            writer.ImmobilisationRateCoeffNH4 = ImmobilisationRateCoeffNH4;
            writer.Denit1 = Denit1;
            writer.Denit2 = Denit2;
            writer.Denit3 = Denit3;
            writer.HydrolysisKM = HydrolysisKM;
            writer.ActivationEnergy = ActivationEnergy;
            writer.HydrolysisP1 = HydrolysisP1;
            writer.HydrolysisP2 = HydrolysisP2;
            writer.AtmosphericResistance = AtmosphericResistance;
            writer.N2oProductionRate = N2oProductionRate;
            writer.InhibitorNH3 = InhibitorNH3;
            writer.PsMaxMineralisationDepth = PsMaxMineralisationDepth;
            SticsParams?.serialize(writer.SticsParams);
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public double SomSlowDecCoeffStandard
        {
            get;
            set;
        }

        = 4.3E-05;
        public double SomFastDecCoeffStandard
        {
            get;
            set;
        }

        = 0.00014;
        public double SmbSlowMaintRateStandard
        {
            get;
            set;
        }

        = 0.001;
        public double SmbFastMaintRateStandard
        {
            get;
            set;
        }

        = 0.01;
        public double SmbSlowDeathRateStandard
        {
            get;
            set;
        }

        = 0.001;
        public double SmbFastDeathRateStandard
        {
            get;
            set;
        }

        = 0.01;
        public double SmbUtilizationEfficiency
        {
            get;
            set;
        }

        = 0.6;
        public double SomSlowUtilizationEfficiency
        {
            get;
            set;
        }

        = 0.4;
        public double SomFastUtilizationEfficiency
        {
            get;
            set;
        }

        = 0.5;
        public double AomSlowUtilizationEfficiency
        {
            get;
            set;
        }

        = 0.4;
        public double AomFastUtilizationEfficiency
        {
            get;
            set;
        }

        = 0.1;
        public double AomFastMaxCtoN
        {
            get;
            set;
        }

        = 1000;
        public double PartSOMFastToSOMSlow
        {
            get;
            set;
        }

        = 0.3;
        public double PartSMBSlowToSOMFast
        {
            get;
            set;
        }

        = 0.6;
        public double PartSMBFastToSOMFast
        {
            get;
            set;
        }

        = 0.6;
        public double PartSOMToSMBSlow
        {
            get;
            set;
        }

        = 0.015;
        public double PartSOMToSMBFast
        {
            get;
            set;
        }

        = 0.0002;
        public double CnRatioSMB
        {
            get;
            set;
        }

        = 6.7;
        public double LimitClayEffect
        {
            get;
            set;
        }

        = 0.25;
        public double AmmoniaOxidationRateCoeffStandard
        {
            get;
            set;
        }

        = 0.1;
        public double NitriteOxidationRateCoeffStandard
        {
            get;
            set;
        }

        = 0.9;
        public double TransportRateCoeff
        {
            get;
            set;
        }

        = 0.1;
        public double SpecAnaerobDenitrification
        {
            get;
            set;
        }

        = 0.1;
        public double ImmobilisationRateCoeffNO3
        {
            get;
            set;
        }

        = 0.5;
        public double ImmobilisationRateCoeffNH4
        {
            get;
            set;
        }

        = 0.5;
        public double Denit1
        {
            get;
            set;
        }

        = 0.2;
        public double Denit2
        {
            get;
            set;
        }

        = 0.8;
        public double Denit3
        {
            get;
            set;
        }

        = 0.9;
        public double HydrolysisKM
        {
            get;
            set;
        }

        = 0.00334;
        public double ActivationEnergy
        {
            get;
            set;
        }

        = 41000;
        public double HydrolysisP1
        {
            get;
            set;
        }

        = 4.259E-12;
        public double HydrolysisP2
        {
            get;
            set;
        }

        = 1.408E-12;
        public double AtmosphericResistance
        {
            get;
            set;
        }

        = 0.0025;
        public double N2oProductionRate
        {
            get;
            set;
        }

        = 0.5;
        public double InhibitorNH3
        {
            get;
            set;
        }

        = 1;
        public double PsMaxMineralisationDepth
        {
            get;
            set;
        }

        = 0.4;
        public Mas.Models.Monica.SticsParameters SticsParams
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
            public double SomSlowDecCoeffStandard => ctx.ReadDataDouble(0UL, 4.3E-05);
            public double SomFastDecCoeffStandard => ctx.ReadDataDouble(64UL, 0.00014);
            public double SmbSlowMaintRateStandard => ctx.ReadDataDouble(128UL, 0.001);
            public double SmbFastMaintRateStandard => ctx.ReadDataDouble(192UL, 0.01);
            public double SmbSlowDeathRateStandard => ctx.ReadDataDouble(256UL, 0.001);
            public double SmbFastDeathRateStandard => ctx.ReadDataDouble(320UL, 0.01);
            public double SmbUtilizationEfficiency => ctx.ReadDataDouble(384UL, 0.6);
            public double SomSlowUtilizationEfficiency => ctx.ReadDataDouble(448UL, 0.4);
            public double SomFastUtilizationEfficiency => ctx.ReadDataDouble(512UL, 0.5);
            public double AomSlowUtilizationEfficiency => ctx.ReadDataDouble(576UL, 0.4);
            public double AomFastUtilizationEfficiency => ctx.ReadDataDouble(640UL, 0.1);
            public double AomFastMaxCtoN => ctx.ReadDataDouble(704UL, 1000);
            public double PartSOMFastToSOMSlow => ctx.ReadDataDouble(768UL, 0.3);
            public double PartSMBSlowToSOMFast => ctx.ReadDataDouble(832UL, 0.6);
            public double PartSMBFastToSOMFast => ctx.ReadDataDouble(896UL, 0.6);
            public double PartSOMToSMBSlow => ctx.ReadDataDouble(960UL, 0.015);
            public double PartSOMToSMBFast => ctx.ReadDataDouble(1024UL, 0.0002);
            public double CnRatioSMB => ctx.ReadDataDouble(1088UL, 6.7);
            public double LimitClayEffect => ctx.ReadDataDouble(1152UL, 0.25);
            public double AmmoniaOxidationRateCoeffStandard => ctx.ReadDataDouble(1216UL, 0.1);
            public double NitriteOxidationRateCoeffStandard => ctx.ReadDataDouble(1280UL, 0.9);
            public double TransportRateCoeff => ctx.ReadDataDouble(1344UL, 0.1);
            public double SpecAnaerobDenitrification => ctx.ReadDataDouble(1408UL, 0.1);
            public double ImmobilisationRateCoeffNO3 => ctx.ReadDataDouble(1472UL, 0.5);
            public double ImmobilisationRateCoeffNH4 => ctx.ReadDataDouble(1536UL, 0.5);
            public double Denit1 => ctx.ReadDataDouble(1600UL, 0.2);
            public double Denit2 => ctx.ReadDataDouble(1664UL, 0.8);
            public double Denit3 => ctx.ReadDataDouble(1728UL, 0.9);
            public double HydrolysisKM => ctx.ReadDataDouble(1792UL, 0.00334);
            public double ActivationEnergy => ctx.ReadDataDouble(1856UL, 41000);
            public double HydrolysisP1 => ctx.ReadDataDouble(1920UL, 4.259E-12);
            public double HydrolysisP2 => ctx.ReadDataDouble(1984UL, 1.408E-12);
            public double AtmosphericResistance => ctx.ReadDataDouble(2048UL, 0.0025);
            public double N2oProductionRate => ctx.ReadDataDouble(2112UL, 0.5);
            public double InhibitorNH3 => ctx.ReadDataDouble(2176UL, 1);
            public double PsMaxMineralisationDepth => ctx.ReadDataDouble(2240UL, 0.4);
            public Mas.Models.Monica.SticsParameters.READER SticsParams => ctx.ReadStruct(0, Mas.Models.Monica.SticsParameters.READER.create);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(36, 1);
            }

            public double SomSlowDecCoeffStandard
            {
                get => this.ReadDataDouble(0UL, 4.3E-05);
                set => this.WriteData(0UL, value, 4.3E-05);
            }

            public double SomFastDecCoeffStandard
            {
                get => this.ReadDataDouble(64UL, 0.00014);
                set => this.WriteData(64UL, value, 0.00014);
            }

            public double SmbSlowMaintRateStandard
            {
                get => this.ReadDataDouble(128UL, 0.001);
                set => this.WriteData(128UL, value, 0.001);
            }

            public double SmbFastMaintRateStandard
            {
                get => this.ReadDataDouble(192UL, 0.01);
                set => this.WriteData(192UL, value, 0.01);
            }

            public double SmbSlowDeathRateStandard
            {
                get => this.ReadDataDouble(256UL, 0.001);
                set => this.WriteData(256UL, value, 0.001);
            }

            public double SmbFastDeathRateStandard
            {
                get => this.ReadDataDouble(320UL, 0.01);
                set => this.WriteData(320UL, value, 0.01);
            }

            public double SmbUtilizationEfficiency
            {
                get => this.ReadDataDouble(384UL, 0.6);
                set => this.WriteData(384UL, value, 0.6);
            }

            public double SomSlowUtilizationEfficiency
            {
                get => this.ReadDataDouble(448UL, 0.4);
                set => this.WriteData(448UL, value, 0.4);
            }

            public double SomFastUtilizationEfficiency
            {
                get => this.ReadDataDouble(512UL, 0.5);
                set => this.WriteData(512UL, value, 0.5);
            }

            public double AomSlowUtilizationEfficiency
            {
                get => this.ReadDataDouble(576UL, 0.4);
                set => this.WriteData(576UL, value, 0.4);
            }

            public double AomFastUtilizationEfficiency
            {
                get => this.ReadDataDouble(640UL, 0.1);
                set => this.WriteData(640UL, value, 0.1);
            }

            public double AomFastMaxCtoN
            {
                get => this.ReadDataDouble(704UL, 1000);
                set => this.WriteData(704UL, value, 1000);
            }

            public double PartSOMFastToSOMSlow
            {
                get => this.ReadDataDouble(768UL, 0.3);
                set => this.WriteData(768UL, value, 0.3);
            }

            public double PartSMBSlowToSOMFast
            {
                get => this.ReadDataDouble(832UL, 0.6);
                set => this.WriteData(832UL, value, 0.6);
            }

            public double PartSMBFastToSOMFast
            {
                get => this.ReadDataDouble(896UL, 0.6);
                set => this.WriteData(896UL, value, 0.6);
            }

            public double PartSOMToSMBSlow
            {
                get => this.ReadDataDouble(960UL, 0.015);
                set => this.WriteData(960UL, value, 0.015);
            }

            public double PartSOMToSMBFast
            {
                get => this.ReadDataDouble(1024UL, 0.0002);
                set => this.WriteData(1024UL, value, 0.0002);
            }

            public double CnRatioSMB
            {
                get => this.ReadDataDouble(1088UL, 6.7);
                set => this.WriteData(1088UL, value, 6.7);
            }

            public double LimitClayEffect
            {
                get => this.ReadDataDouble(1152UL, 0.25);
                set => this.WriteData(1152UL, value, 0.25);
            }

            public double AmmoniaOxidationRateCoeffStandard
            {
                get => this.ReadDataDouble(1216UL, 0.1);
                set => this.WriteData(1216UL, value, 0.1);
            }

            public double NitriteOxidationRateCoeffStandard
            {
                get => this.ReadDataDouble(1280UL, 0.9);
                set => this.WriteData(1280UL, value, 0.9);
            }

            public double TransportRateCoeff
            {
                get => this.ReadDataDouble(1344UL, 0.1);
                set => this.WriteData(1344UL, value, 0.1);
            }

            public double SpecAnaerobDenitrification
            {
                get => this.ReadDataDouble(1408UL, 0.1);
                set => this.WriteData(1408UL, value, 0.1);
            }

            public double ImmobilisationRateCoeffNO3
            {
                get => this.ReadDataDouble(1472UL, 0.5);
                set => this.WriteData(1472UL, value, 0.5);
            }

            public double ImmobilisationRateCoeffNH4
            {
                get => this.ReadDataDouble(1536UL, 0.5);
                set => this.WriteData(1536UL, value, 0.5);
            }

            public double Denit1
            {
                get => this.ReadDataDouble(1600UL, 0.2);
                set => this.WriteData(1600UL, value, 0.2);
            }

            public double Denit2
            {
                get => this.ReadDataDouble(1664UL, 0.8);
                set => this.WriteData(1664UL, value, 0.8);
            }

            public double Denit3
            {
                get => this.ReadDataDouble(1728UL, 0.9);
                set => this.WriteData(1728UL, value, 0.9);
            }

            public double HydrolysisKM
            {
                get => this.ReadDataDouble(1792UL, 0.00334);
                set => this.WriteData(1792UL, value, 0.00334);
            }

            public double ActivationEnergy
            {
                get => this.ReadDataDouble(1856UL, 41000);
                set => this.WriteData(1856UL, value, 41000);
            }

            public double HydrolysisP1
            {
                get => this.ReadDataDouble(1920UL, 4.259E-12);
                set => this.WriteData(1920UL, value, 4.259E-12);
            }

            public double HydrolysisP2
            {
                get => this.ReadDataDouble(1984UL, 1.408E-12);
                set => this.WriteData(1984UL, value, 1.408E-12);
            }

            public double AtmosphericResistance
            {
                get => this.ReadDataDouble(2048UL, 0.0025);
                set => this.WriteData(2048UL, value, 0.0025);
            }

            public double N2oProductionRate
            {
                get => this.ReadDataDouble(2112UL, 0.5);
                set => this.WriteData(2112UL, value, 0.5);
            }

            public double InhibitorNH3
            {
                get => this.ReadDataDouble(2176UL, 1);
                set => this.WriteData(2176UL, value, 1);
            }

            public double PsMaxMineralisationDepth
            {
                get => this.ReadDataDouble(2240UL, 0.4);
                set => this.WriteData(2240UL, value, 0.4);
            }

            public Mas.Models.Monica.SticsParameters.WRITER SticsParams
            {
                get => BuildPointer<Mas.Models.Monica.SticsParameters.WRITER>(0);
                set => Link(0, value);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf0c41d021228d929UL)]
    public class SoilTemperatureModuleParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0xf0c41d021228d929UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            NTau = reader.NTau;
            InitialSurfaceTemperature = reader.InitialSurfaceTemperature;
            BaseTemperature = reader.BaseTemperature;
            QuartzRawDensity = reader.QuartzRawDensity;
            DensityAir = reader.DensityAir;
            DensityWater = reader.DensityWater;
            DensityHumus = reader.DensityHumus;
            SpecificHeatCapacityAir = reader.SpecificHeatCapacityAir;
            SpecificHeatCapacityQuartz = reader.SpecificHeatCapacityQuartz;
            SpecificHeatCapacityWater = reader.SpecificHeatCapacityWater;
            SpecificHeatCapacityHumus = reader.SpecificHeatCapacityHumus;
            SoilAlbedo = reader.SoilAlbedo;
            SoilMoisture = reader.SoilMoisture;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.NTau = NTau;
            writer.InitialSurfaceTemperature = InitialSurfaceTemperature;
            writer.BaseTemperature = BaseTemperature;
            writer.QuartzRawDensity = QuartzRawDensity;
            writer.DensityAir = DensityAir;
            writer.DensityWater = DensityWater;
            writer.DensityHumus = DensityHumus;
            writer.SpecificHeatCapacityAir = SpecificHeatCapacityAir;
            writer.SpecificHeatCapacityQuartz = SpecificHeatCapacityQuartz;
            writer.SpecificHeatCapacityWater = SpecificHeatCapacityWater;
            writer.SpecificHeatCapacityHumus = SpecificHeatCapacityHumus;
            writer.SoilAlbedo = SoilAlbedo;
            writer.SoilMoisture = SoilMoisture;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public double NTau
        {
            get;
            set;
        }

        public double InitialSurfaceTemperature
        {
            get;
            set;
        }

        public double BaseTemperature
        {
            get;
            set;
        }

        public double QuartzRawDensity
        {
            get;
            set;
        }

        public double DensityAir
        {
            get;
            set;
        }

        public double DensityWater
        {
            get;
            set;
        }

        public double DensityHumus
        {
            get;
            set;
        }

        public double SpecificHeatCapacityAir
        {
            get;
            set;
        }

        public double SpecificHeatCapacityQuartz
        {
            get;
            set;
        }

        public double SpecificHeatCapacityWater
        {
            get;
            set;
        }

        public double SpecificHeatCapacityHumus
        {
            get;
            set;
        }

        public double SoilAlbedo
        {
            get;
            set;
        }

        public double SoilMoisture
        {
            get;
            set;
        }

        = 0.25;
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
            public double NTau => ctx.ReadDataDouble(0UL, 0);
            public double InitialSurfaceTemperature => ctx.ReadDataDouble(64UL, 0);
            public double BaseTemperature => ctx.ReadDataDouble(128UL, 0);
            public double QuartzRawDensity => ctx.ReadDataDouble(192UL, 0);
            public double DensityAir => ctx.ReadDataDouble(256UL, 0);
            public double DensityWater => ctx.ReadDataDouble(320UL, 0);
            public double DensityHumus => ctx.ReadDataDouble(384UL, 0);
            public double SpecificHeatCapacityAir => ctx.ReadDataDouble(448UL, 0);
            public double SpecificHeatCapacityQuartz => ctx.ReadDataDouble(512UL, 0);
            public double SpecificHeatCapacityWater => ctx.ReadDataDouble(576UL, 0);
            public double SpecificHeatCapacityHumus => ctx.ReadDataDouble(640UL, 0);
            public double SoilAlbedo => ctx.ReadDataDouble(704UL, 0);
            public double SoilMoisture => ctx.ReadDataDouble(768UL, 0.25);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(13, 0);
            }

            public double NTau
            {
                get => this.ReadDataDouble(0UL, 0);
                set => this.WriteData(0UL, value, 0);
            }

            public double InitialSurfaceTemperature
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public double BaseTemperature
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }

            public double QuartzRawDensity
            {
                get => this.ReadDataDouble(192UL, 0);
                set => this.WriteData(192UL, value, 0);
            }

            public double DensityAir
            {
                get => this.ReadDataDouble(256UL, 0);
                set => this.WriteData(256UL, value, 0);
            }

            public double DensityWater
            {
                get => this.ReadDataDouble(320UL, 0);
                set => this.WriteData(320UL, value, 0);
            }

            public double DensityHumus
            {
                get => this.ReadDataDouble(384UL, 0);
                set => this.WriteData(384UL, value, 0);
            }

            public double SpecificHeatCapacityAir
            {
                get => this.ReadDataDouble(448UL, 0);
                set => this.WriteData(448UL, value, 0);
            }

            public double SpecificHeatCapacityQuartz
            {
                get => this.ReadDataDouble(512UL, 0);
                set => this.WriteData(512UL, value, 0);
            }

            public double SpecificHeatCapacityWater
            {
                get => this.ReadDataDouble(576UL, 0);
                set => this.WriteData(576UL, value, 0);
            }

            public double SpecificHeatCapacityHumus
            {
                get => this.ReadDataDouble(640UL, 0);
                set => this.WriteData(640UL, value, 0);
            }

            public double SoilAlbedo
            {
                get => this.ReadDataDouble(704UL, 0);
                set => this.WriteData(704UL, value, 0);
            }

            public double SoilMoisture
            {
                get => this.ReadDataDouble(768UL, 0.25);
                set => this.WriteData(768UL, value, 0.25);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc5cb65e585742338UL)]
    public class SoilTransportModuleParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0xc5cb65e585742338UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            DispersionLength = reader.DispersionLength;
            Ad = reader.Ad;
            DiffusionCoefficientStandard = reader.DiffusionCoefficientStandard;
            NDeposition = reader.NDeposition;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.DispersionLength = DispersionLength;
            writer.Ad = Ad;
            writer.DiffusionCoefficientStandard = DiffusionCoefficientStandard;
            writer.NDeposition = NDeposition;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public double DispersionLength
        {
            get;
            set;
        }

        public double Ad
        {
            get;
            set;
        }

        public double DiffusionCoefficientStandard
        {
            get;
            set;
        }

        public double NDeposition
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
            public double DispersionLength => ctx.ReadDataDouble(0UL, 0);
            public double Ad => ctx.ReadDataDouble(64UL, 0);
            public double DiffusionCoefficientStandard => ctx.ReadDataDouble(128UL, 0);
            public double NDeposition => ctx.ReadDataDouble(192UL, 0);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(4, 0);
            }

            public double DispersionLength
            {
                get => this.ReadDataDouble(0UL, 0);
                set => this.WriteData(0UL, value, 0);
            }

            public double Ad
            {
                get => this.ReadDataDouble(64UL, 0);
                set => this.WriteData(64UL, value, 0);
            }

            public double DiffusionCoefficientStandard
            {
                get => this.ReadDataDouble(128UL, 0);
                set => this.WriteData(128UL, value, 0);
            }

            public double NDeposition
            {
                get => this.ReadDataDouble(192UL, 0);
                set => this.WriteData(192UL, value, 0);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb87956e2953771dbUL)]
    public class Voc : ICapnpSerializable
    {
        public const UInt64 typeId = 0xb87956e2953771dbUL;
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

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd9ed2c1c754d683eUL)]
        public class Emissions : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd9ed2c1c754d683eUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                SpeciesIdToIsopreneEmission = reader.SpeciesIdToIsopreneEmission?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Models.Monica.Voc.Emissions.SpeciesIdToEmission>(_));
                SpeciesIdToMonoterpeneEmission = reader.SpeciesIdToMonoterpeneEmission?.ToReadOnlyList(_ => CapnpSerializable.Create<Mas.Models.Monica.Voc.Emissions.SpeciesIdToEmission>(_));
                IsopreneEmission = reader.IsopreneEmission;
                MonoterpeneEmission = reader.MonoterpeneEmission;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.SpeciesIdToIsopreneEmission.Init(SpeciesIdToIsopreneEmission, (_s1, _v1) => _v1?.serialize(_s1));
                writer.SpeciesIdToMonoterpeneEmission.Init(SpeciesIdToMonoterpeneEmission, (_s1, _v1) => _v1?.serialize(_s1));
                writer.IsopreneEmission = IsopreneEmission;
                writer.MonoterpeneEmission = MonoterpeneEmission;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<Mas.Models.Monica.Voc.Emissions.SpeciesIdToEmission> SpeciesIdToIsopreneEmission
            {
                get;
                set;
            }

            public IReadOnlyList<Mas.Models.Monica.Voc.Emissions.SpeciesIdToEmission> SpeciesIdToMonoterpeneEmission
            {
                get;
                set;
            }

            public double IsopreneEmission
            {
                get;
                set;
            }

            public double MonoterpeneEmission
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
                public IReadOnlyList<Mas.Models.Monica.Voc.Emissions.SpeciesIdToEmission.READER> SpeciesIdToIsopreneEmission => ctx.ReadList(0).Cast(Mas.Models.Monica.Voc.Emissions.SpeciesIdToEmission.READER.create);
                public IReadOnlyList<Mas.Models.Monica.Voc.Emissions.SpeciesIdToEmission.READER> SpeciesIdToMonoterpeneEmission => ctx.ReadList(1).Cast(Mas.Models.Monica.Voc.Emissions.SpeciesIdToEmission.READER.create);
                public double IsopreneEmission => ctx.ReadDataDouble(0UL, 0);
                public double MonoterpeneEmission => ctx.ReadDataDouble(64UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 2);
                }

                public ListOfStructsSerializer<Mas.Models.Monica.Voc.Emissions.SpeciesIdToEmission.WRITER> SpeciesIdToIsopreneEmission
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Models.Monica.Voc.Emissions.SpeciesIdToEmission.WRITER>>(0);
                    set => Link(0, value);
                }

                public ListOfStructsSerializer<Mas.Models.Monica.Voc.Emissions.SpeciesIdToEmission.WRITER> SpeciesIdToMonoterpeneEmission
                {
                    get => BuildPointer<ListOfStructsSerializer<Mas.Models.Monica.Voc.Emissions.SpeciesIdToEmission.WRITER>>(1);
                    set => Link(1, value);
                }

                public double IsopreneEmission
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public double MonoterpeneEmission
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd11f8d1479e2f010UL)]
            public class SpeciesIdToEmission : ICapnpSerializable
            {
                public const UInt64 typeId = 0xd11f8d1479e2f010UL;
                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    SpeciesId = reader.SpeciesId;
                    Emission = reader.Emission;
                    applyDefaults();
                }

                public void serialize(WRITER writer)
                {
                    writer.SpeciesId = SpeciesId;
                    writer.Emission = Emission;
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public ulong SpeciesId
                {
                    get;
                    set;
                }

                public double Emission
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
                    public ulong SpeciesId => ctx.ReadDataULong(0UL, 0UL);
                    public double Emission => ctx.ReadDataDouble(64UL, 0);
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                        this.SetStruct(2, 0);
                    }

                    public ulong SpeciesId
                    {
                        get => this.ReadDataULong(0UL, 0UL);
                        set => this.WriteData(0UL, value, 0UL);
                    }

                    public double Emission
                    {
                        get => this.ReadDataDouble(64UL, 0);
                        set => this.WriteData(64UL, value, 0);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x80d5a7b782142e87UL)]
        public class SpeciesData : ICapnpSerializable
        {
            public const UInt64 typeId = 0x80d5a7b782142e87UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Id = reader.Id;
                EfMonos = reader.EfMonos;
                EfMono = reader.EfMono;
                EfIso = reader.EfIso;
                Theta = reader.Theta;
                Fage = reader.Fage;
                CtIs = reader.CtIs;
                CtMt = reader.CtMt;
                HaIs = reader.HaIs;
                HaMt = reader.HaMt;
                DsIs = reader.DsIs;
                DsMt = reader.DsMt;
                HdIs = reader.HdIs;
                HdMt = reader.HdMt;
                Hdj = reader.Hdj;
                Sdj = reader.Sdj;
                Kc25 = reader.Kc25;
                Ko25 = reader.Ko25;
                VcMax25 = reader.VcMax25;
                Qjvc = reader.Qjvc;
                Aekc = reader.Aekc;
                Aeko = reader.Aeko;
                Aejm = reader.Aejm;
                Aevc = reader.Aevc;
                SlaMin = reader.SlaMin;
                ScaleI = reader.ScaleI;
                ScaleM = reader.ScaleM;
                MFol = reader.MFol;
                Lai = reader.Lai;
                Sla = reader.Sla;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Id = Id;
                writer.EfMonos = EfMonos;
                writer.EfMono = EfMono;
                writer.EfIso = EfIso;
                writer.Theta = Theta;
                writer.Fage = Fage;
                writer.CtIs = CtIs;
                writer.CtMt = CtMt;
                writer.HaIs = HaIs;
                writer.HaMt = HaMt;
                writer.DsIs = DsIs;
                writer.DsMt = DsMt;
                writer.HdIs = HdIs;
                writer.HdMt = HdMt;
                writer.Hdj = Hdj;
                writer.Sdj = Sdj;
                writer.Kc25 = Kc25;
                writer.Ko25 = Ko25;
                writer.VcMax25 = VcMax25;
                writer.Qjvc = Qjvc;
                writer.Aekc = Aekc;
                writer.Aeko = Aeko;
                writer.Aejm = Aejm;
                writer.Aevc = Aevc;
                writer.SlaMin = SlaMin;
                writer.ScaleI = ScaleI;
                writer.ScaleM = ScaleM;
                writer.MFol = MFol;
                writer.Lai = Lai;
                writer.Sla = Sla;
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

            public double EfMonos
            {
                get;
                set;
            }

            public double EfMono
            {
                get;
                set;
            }

            public double EfIso
            {
                get;
                set;
            }

            public double Theta
            {
                get;
                set;
            }

            = 0.9;
            public double Fage
            {
                get;
                set;
            }

            = 1;
            public double CtIs
            {
                get;
                set;
            }

            public double CtMt
            {
                get;
                set;
            }

            public double HaIs
            {
                get;
                set;
            }

            public double HaMt
            {
                get;
                set;
            }

            public double DsIs
            {
                get;
                set;
            }

            public double DsMt
            {
                get;
                set;
            }

            public double HdIs
            {
                get;
                set;
            }

            = 284600;
            public double HdMt
            {
                get;
                set;
            }

            = 284600;
            public double Hdj
            {
                get;
                set;
            }

            = 220000;
            public double Sdj
            {
                get;
                set;
            }

            = 703;
            public double Kc25
            {
                get;
                set;
            }

            = 260;
            public double Ko25
            {
                get;
                set;
            }

            = 179;
            public double VcMax25
            {
                get;
                set;
            }

            = 80;
            public double Qjvc
            {
                get;
                set;
            }

            = 2;
            public double Aekc
            {
                get;
                set;
            }

            = 59356;
            public double Aeko
            {
                get;
                set;
            }

            = 35948;
            public double Aejm
            {
                get;
                set;
            }

            = 37000;
            public double Aevc
            {
                get;
                set;
            }

            = 58520;
            public double SlaMin
            {
                get;
                set;
            }

            = 20;
            public double ScaleI
            {
                get;
                set;
            }

            = 1;
            public double ScaleM
            {
                get;
                set;
            }

            = 1;
            public double MFol
            {
                get;
                set;
            }

            public double Lai
            {
                get;
                set;
            }

            public double Sla
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
                public double EfMonos => ctx.ReadDataDouble(64UL, 0);
                public double EfMono => ctx.ReadDataDouble(128UL, 0);
                public double EfIso => ctx.ReadDataDouble(192UL, 0);
                public double Theta => ctx.ReadDataDouble(256UL, 0.9);
                public double Fage => ctx.ReadDataDouble(320UL, 1);
                public double CtIs => ctx.ReadDataDouble(384UL, 0);
                public double CtMt => ctx.ReadDataDouble(448UL, 0);
                public double HaIs => ctx.ReadDataDouble(512UL, 0);
                public double HaMt => ctx.ReadDataDouble(576UL, 0);
                public double DsIs => ctx.ReadDataDouble(640UL, 0);
                public double DsMt => ctx.ReadDataDouble(704UL, 0);
                public double HdIs => ctx.ReadDataDouble(768UL, 284600);
                public double HdMt => ctx.ReadDataDouble(832UL, 284600);
                public double Hdj => ctx.ReadDataDouble(896UL, 220000);
                public double Sdj => ctx.ReadDataDouble(960UL, 703);
                public double Kc25 => ctx.ReadDataDouble(1024UL, 260);
                public double Ko25 => ctx.ReadDataDouble(1088UL, 179);
                public double VcMax25 => ctx.ReadDataDouble(1152UL, 80);
                public double Qjvc => ctx.ReadDataDouble(1216UL, 2);
                public double Aekc => ctx.ReadDataDouble(1280UL, 59356);
                public double Aeko => ctx.ReadDataDouble(1344UL, 35948);
                public double Aejm => ctx.ReadDataDouble(1408UL, 37000);
                public double Aevc => ctx.ReadDataDouble(1472UL, 58520);
                public double SlaMin => ctx.ReadDataDouble(1536UL, 20);
                public double ScaleI => ctx.ReadDataDouble(1600UL, 1);
                public double ScaleM => ctx.ReadDataDouble(1664UL, 1);
                public double MFol => ctx.ReadDataDouble(1728UL, 0);
                public double Lai => ctx.ReadDataDouble(1792UL, 0);
                public double Sla => ctx.ReadDataDouble(1856UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(30, 0);
                }

                public ulong Id
                {
                    get => this.ReadDataULong(0UL, 0UL);
                    set => this.WriteData(0UL, value, 0UL);
                }

                public double EfMonos
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }

                public double EfMono
                {
                    get => this.ReadDataDouble(128UL, 0);
                    set => this.WriteData(128UL, value, 0);
                }

                public double EfIso
                {
                    get => this.ReadDataDouble(192UL, 0);
                    set => this.WriteData(192UL, value, 0);
                }

                public double Theta
                {
                    get => this.ReadDataDouble(256UL, 0.9);
                    set => this.WriteData(256UL, value, 0.9);
                }

                public double Fage
                {
                    get => this.ReadDataDouble(320UL, 1);
                    set => this.WriteData(320UL, value, 1);
                }

                public double CtIs
                {
                    get => this.ReadDataDouble(384UL, 0);
                    set => this.WriteData(384UL, value, 0);
                }

                public double CtMt
                {
                    get => this.ReadDataDouble(448UL, 0);
                    set => this.WriteData(448UL, value, 0);
                }

                public double HaIs
                {
                    get => this.ReadDataDouble(512UL, 0);
                    set => this.WriteData(512UL, value, 0);
                }

                public double HaMt
                {
                    get => this.ReadDataDouble(576UL, 0);
                    set => this.WriteData(576UL, value, 0);
                }

                public double DsIs
                {
                    get => this.ReadDataDouble(640UL, 0);
                    set => this.WriteData(640UL, value, 0);
                }

                public double DsMt
                {
                    get => this.ReadDataDouble(704UL, 0);
                    set => this.WriteData(704UL, value, 0);
                }

                public double HdIs
                {
                    get => this.ReadDataDouble(768UL, 284600);
                    set => this.WriteData(768UL, value, 284600);
                }

                public double HdMt
                {
                    get => this.ReadDataDouble(832UL, 284600);
                    set => this.WriteData(832UL, value, 284600);
                }

                public double Hdj
                {
                    get => this.ReadDataDouble(896UL, 220000);
                    set => this.WriteData(896UL, value, 220000);
                }

                public double Sdj
                {
                    get => this.ReadDataDouble(960UL, 703);
                    set => this.WriteData(960UL, value, 703);
                }

                public double Kc25
                {
                    get => this.ReadDataDouble(1024UL, 260);
                    set => this.WriteData(1024UL, value, 260);
                }

                public double Ko25
                {
                    get => this.ReadDataDouble(1088UL, 179);
                    set => this.WriteData(1088UL, value, 179);
                }

                public double VcMax25
                {
                    get => this.ReadDataDouble(1152UL, 80);
                    set => this.WriteData(1152UL, value, 80);
                }

                public double Qjvc
                {
                    get => this.ReadDataDouble(1216UL, 2);
                    set => this.WriteData(1216UL, value, 2);
                }

                public double Aekc
                {
                    get => this.ReadDataDouble(1280UL, 59356);
                    set => this.WriteData(1280UL, value, 59356);
                }

                public double Aeko
                {
                    get => this.ReadDataDouble(1344UL, 35948);
                    set => this.WriteData(1344UL, value, 35948);
                }

                public double Aejm
                {
                    get => this.ReadDataDouble(1408UL, 37000);
                    set => this.WriteData(1408UL, value, 37000);
                }

                public double Aevc
                {
                    get => this.ReadDataDouble(1472UL, 58520);
                    set => this.WriteData(1472UL, value, 58520);
                }

                public double SlaMin
                {
                    get => this.ReadDataDouble(1536UL, 20);
                    set => this.WriteData(1536UL, value, 20);
                }

                public double ScaleI
                {
                    get => this.ReadDataDouble(1600UL, 1);
                    set => this.WriteData(1600UL, value, 1);
                }

                public double ScaleM
                {
                    get => this.ReadDataDouble(1664UL, 1);
                    set => this.WriteData(1664UL, value, 1);
                }

                public double MFol
                {
                    get => this.ReadDataDouble(1728UL, 0);
                    set => this.WriteData(1728UL, value, 0);
                }

                public double Lai
                {
                    get => this.ReadDataDouble(1792UL, 0);
                    set => this.WriteData(1792UL, value, 0);
                }

                public double Sla
                {
                    get => this.ReadDataDouble(1856UL, 0);
                    set => this.WriteData(1856UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcf0f425c8bd69fa2UL)]
        public class CPData : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcf0f425c8bd69fa2UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Kc = reader.Kc;
                Ko = reader.Ko;
                Oi = reader.Oi;
                Ci = reader.Ci;
                Comp = reader.Comp;
                VcMax = reader.VcMax;
                JMax = reader.JMax;
                Jj = reader.Jj;
                Jj1000 = reader.Jj1000;
                Jv = reader.Jv;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Kc = Kc;
                writer.Ko = Ko;
                writer.Oi = Oi;
                writer.Ci = Ci;
                writer.Comp = Comp;
                writer.VcMax = VcMax;
                writer.JMax = JMax;
                writer.Jj = Jj;
                writer.Jj1000 = Jj1000;
                writer.Jv = Jv;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Kc
            {
                get;
                set;
            }

            public double Ko
            {
                get;
                set;
            }

            public double Oi
            {
                get;
                set;
            }

            public double Ci
            {
                get;
                set;
            }

            public double Comp
            {
                get;
                set;
            }

            public double VcMax
            {
                get;
                set;
            }

            public double JMax
            {
                get;
                set;
            }

            public double Jj
            {
                get;
                set;
            }

            public double Jj1000
            {
                get;
                set;
            }

            public double Jv
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
                public double Kc => ctx.ReadDataDouble(0UL, 0);
                public double Ko => ctx.ReadDataDouble(64UL, 0);
                public double Oi => ctx.ReadDataDouble(128UL, 0);
                public double Ci => ctx.ReadDataDouble(192UL, 0);
                public double Comp => ctx.ReadDataDouble(256UL, 0);
                public double VcMax => ctx.ReadDataDouble(320UL, 0);
                public double JMax => ctx.ReadDataDouble(384UL, 0);
                public double Jj => ctx.ReadDataDouble(448UL, 0);
                public double Jj1000 => ctx.ReadDataDouble(512UL, 0);
                public double Jv => ctx.ReadDataDouble(576UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(10, 0);
                }

                public double Kc
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public double Ko
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }

                public double Oi
                {
                    get => this.ReadDataDouble(128UL, 0);
                    set => this.WriteData(128UL, value, 0);
                }

                public double Ci
                {
                    get => this.ReadDataDouble(192UL, 0);
                    set => this.WriteData(192UL, value, 0);
                }

                public double Comp
                {
                    get => this.ReadDataDouble(256UL, 0);
                    set => this.WriteData(256UL, value, 0);
                }

                public double VcMax
                {
                    get => this.ReadDataDouble(320UL, 0);
                    set => this.WriteData(320UL, value, 0);
                }

                public double JMax
                {
                    get => this.ReadDataDouble(384UL, 0);
                    set => this.WriteData(384UL, value, 0);
                }

                public double Jj
                {
                    get => this.ReadDataDouble(448UL, 0);
                    set => this.WriteData(448UL, value, 0);
                }

                public double Jj1000
                {
                    get => this.ReadDataDouble(512UL, 0);
                    set => this.WriteData(512UL, value, 0);
                }

                public double Jv
                {
                    get => this.ReadDataDouble(576UL, 0);
                    set => this.WriteData(576UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf246442c7aee0af5UL)]
        public class MicroClimateData : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf246442c7aee0af5UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Rad = reader.Rad;
                Rad24 = reader.Rad24;
                Rad240 = reader.Rad240;
                TFol = reader.TFol;
                TFol24 = reader.TFol24;
                TFol240 = reader.TFol240;
                Sunlitfoliagefraction = reader.Sunlitfoliagefraction;
                Sunlitfoliagefraction24 = reader.Sunlitfoliagefraction24;
                Co2concentration = reader.Co2concentration;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Rad = Rad;
                writer.Rad24 = Rad24;
                writer.Rad240 = Rad240;
                writer.TFol = TFol;
                writer.TFol24 = TFol24;
                writer.TFol240 = TFol240;
                writer.Sunlitfoliagefraction = Sunlitfoliagefraction;
                writer.Sunlitfoliagefraction24 = Sunlitfoliagefraction24;
                writer.Co2concentration = Co2concentration;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Rad
            {
                get;
                set;
            }

            public double Rad24
            {
                get;
                set;
            }

            public double Rad240
            {
                get;
                set;
            }

            public double TFol
            {
                get;
                set;
            }

            public double TFol24
            {
                get;
                set;
            }

            public double TFol240
            {
                get;
                set;
            }

            public double Sunlitfoliagefraction
            {
                get;
                set;
            }

            public double Sunlitfoliagefraction24
            {
                get;
                set;
            }

            public double Co2concentration
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
                public double Rad => ctx.ReadDataDouble(0UL, 0);
                public double Rad24 => ctx.ReadDataDouble(64UL, 0);
                public double Rad240 => ctx.ReadDataDouble(128UL, 0);
                public double TFol => ctx.ReadDataDouble(192UL, 0);
                public double TFol24 => ctx.ReadDataDouble(256UL, 0);
                public double TFol240 => ctx.ReadDataDouble(320UL, 0);
                public double Sunlitfoliagefraction => ctx.ReadDataDouble(384UL, 0);
                public double Sunlitfoliagefraction24 => ctx.ReadDataDouble(448UL, 0);
                public double Co2concentration => ctx.ReadDataDouble(512UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(9, 0);
                }

                public double Rad
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public double Rad24
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }

                public double Rad240
                {
                    get => this.ReadDataDouble(128UL, 0);
                    set => this.WriteData(128UL, value, 0);
                }

                public double TFol
                {
                    get => this.ReadDataDouble(192UL, 0);
                    set => this.WriteData(192UL, value, 0);
                }

                public double TFol24
                {
                    get => this.ReadDataDouble(256UL, 0);
                    set => this.WriteData(256UL, value, 0);
                }

                public double TFol240
                {
                    get => this.ReadDataDouble(320UL, 0);
                    set => this.WriteData(320UL, value, 0);
                }

                public double Sunlitfoliagefraction
                {
                    get => this.ReadDataDouble(384UL, 0);
                    set => this.WriteData(384UL, value, 0);
                }

                public double Sunlitfoliagefraction24
                {
                    get => this.ReadDataDouble(448UL, 0);
                    set => this.WriteData(448UL, value, 0);
                }

                public double Co2concentration
                {
                    get => this.ReadDataDouble(512UL, 0);
                    set => this.WriteData(512UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf95db11410e33efcUL)]
        public class PhotosynthT : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf95db11410e33efcUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Par = reader.Par;
                Par24 = reader.Par24;
                Par240 = reader.Par240;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Par = Par;
                writer.Par24 = Par24;
                writer.Par240 = Par240;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Par
            {
                get;
                set;
            }

            public double Par24
            {
                get;
                set;
            }

            public double Par240
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
                public double Par => ctx.ReadDataDouble(0UL, 0);
                public double Par24 => ctx.ReadDataDouble(64UL, 0);
                public double Par240 => ctx.ReadDataDouble(128UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(3, 0);
                }

                public double Par
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public double Par24
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }

                public double Par240
                {
                    get => this.ReadDataDouble(128UL, 0);
                    set => this.WriteData(128UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xee0b04cc3f52f33cUL)]
        public class FoliageT : ICapnpSerializable
        {
            public const UInt64 typeId = 0xee0b04cc3f52f33cUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                TempK = reader.TempK;
                TempK24 = reader.TempK24;
                TempK240 = reader.TempK240;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.TempK = TempK;
                writer.TempK24 = TempK24;
                writer.TempK240 = TempK240;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double TempK
            {
                get;
                set;
            }

            public double TempK24
            {
                get;
                set;
            }

            public double TempK240
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
                public double TempK => ctx.ReadDataDouble(0UL, 0);
                public double TempK24 => ctx.ReadDataDouble(64UL, 0);
                public double TempK240 => ctx.ReadDataDouble(128UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(3, 0);
                }

                public double TempK
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public double TempK24
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }

                public double TempK240
                {
                    get => this.ReadDataDouble(128UL, 0);
                    set => this.WriteData(128UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc281c6e5be483337UL)]
        public class EnzymeActivityT : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc281c6e5be483337UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                EfIso = reader.EfIso;
                EfMono = reader.EfMono;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.EfIso = EfIso;
                writer.EfMono = EfMono;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double EfIso
            {
                get;
                set;
            }

            public double EfMono
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
                public double EfIso => ctx.ReadDataDouble(0UL, 0);
                public double EfMono => ctx.ReadDataDouble(64UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 0);
                }

                public double EfIso
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public double EfMono
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe82d760b257daddbUL)]
        public class LeafEmissionT : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe82d760b257daddbUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                FoliageLayer = reader.FoliageLayer;
                Pho = CapnpSerializable.Create<Mas.Models.Monica.Voc.PhotosynthT>(reader.Pho);
                Fol = CapnpSerializable.Create<Mas.Models.Monica.Voc.FoliageT>(reader.Fol);
                EnzAct = CapnpSerializable.Create<Mas.Models.Monica.Voc.EnzymeActivityT>(reader.EnzAct);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.FoliageLayer = FoliageLayer;
                Pho?.serialize(writer.Pho);
                Fol?.serialize(writer.Fol);
                EnzAct?.serialize(writer.EnzAct);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public ushort FoliageLayer
            {
                get;
                set;
            }

            public Mas.Models.Monica.Voc.PhotosynthT Pho
            {
                get;
                set;
            }

            public Mas.Models.Monica.Voc.FoliageT Fol
            {
                get;
                set;
            }

            public Mas.Models.Monica.Voc.EnzymeActivityT EnzAct
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
                public ushort FoliageLayer => ctx.ReadDataUShort(0UL, (ushort)0);
                public Mas.Models.Monica.Voc.PhotosynthT.READER Pho => ctx.ReadStruct(0, Mas.Models.Monica.Voc.PhotosynthT.READER.create);
                public Mas.Models.Monica.Voc.FoliageT.READER Fol => ctx.ReadStruct(1, Mas.Models.Monica.Voc.FoliageT.READER.create);
                public Mas.Models.Monica.Voc.EnzymeActivityT.READER EnzAct => ctx.ReadStruct(2, Mas.Models.Monica.Voc.EnzymeActivityT.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 3);
                }

                public ushort FoliageLayer
                {
                    get => this.ReadDataUShort(0UL, (ushort)0);
                    set => this.WriteData(0UL, value, (ushort)0);
                }

                public Mas.Models.Monica.Voc.PhotosynthT.WRITER Pho
                {
                    get => BuildPointer<Mas.Models.Monica.Voc.PhotosynthT.WRITER>(0);
                    set => Link(0, value);
                }

                public Mas.Models.Monica.Voc.FoliageT.WRITER Fol
                {
                    get => BuildPointer<Mas.Models.Monica.Voc.FoliageT.WRITER>(1);
                    set => Link(1, value);
                }

                public Mas.Models.Monica.Voc.EnzymeActivityT.WRITER EnzAct
                {
                    get => BuildPointer<Mas.Models.Monica.Voc.EnzymeActivityT.WRITER>(2);
                    set => Link(2, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc8aeb5222ac5ef40UL)]
        public class LeafEmissions : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc8aeb5222ac5ef40UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Isoprene = reader.Isoprene;
                Monoterp = reader.Monoterp;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Isoprene = Isoprene;
                writer.Monoterp = Monoterp;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Isoprene
            {
                get;
                set;
            }

            public double Monoterp
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
                public double Isoprene => ctx.ReadDataDouble(0UL, 0);
                public double Monoterp => ctx.ReadDataDouble(64UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 0);
                }

                public double Isoprene
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public double Monoterp
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xce5b0091fd9acb21UL)]
    public class SticsParameters : ICapnpSerializable
    {
        public const UInt64 typeId = 0xce5b0091fd9acb21UL;
        void ICapnpSerializable.Deserialize(DeserializerState arg_)
        {
            var reader = READER.create(arg_);
            UseN2O = reader.UseN2O;
            UseNit = reader.UseNit;
            UseDenit = reader.UseDenit;
            CodeVnit = reader.CodeVnit;
            CodeTnit = reader.CodeTnit;
            CodeRationit = reader.CodeRationit;
            CodeHourlyWfpsNit = reader.CodeHourlyWfpsNit;
            CodePdenit = reader.CodePdenit;
            CodeRatiodenit = reader.CodeRatiodenit;
            CodeHourlyWfpsDenit = reader.CodeHourlyWfpsDenit;
            Hminn = reader.Hminn;
            Hoptn = reader.Hoptn;
            PHminnit = reader.PHminnit;
            PHmaxnit = reader.PHmaxnit;
            Nh4Min = reader.Nh4Min;
            PHminden = reader.PHminden;
            PHmaxden = reader.PHmaxden;
            Wfpsc = reader.Wfpsc;
            TdenitoptGauss = reader.TdenitoptGauss;
            ScaleTdenitopt = reader.ScaleTdenitopt;
            Kd = reader.Kd;
            KDesat = reader.KDesat;
            Fnx = reader.Fnx;
            Vnitmax = reader.Vnitmax;
            Kamm = reader.Kamm;
            Tnitmin = reader.Tnitmin;
            Tnitopt = reader.Tnitopt;
            Tnitop2 = reader.Tnitop2;
            Tnitmax = reader.Tnitmax;
            TnitoptGauss = reader.TnitoptGauss;
            ScaleTnitopt = reader.ScaleTnitopt;
            Rationit = reader.Rationit;
            CminPdenit = reader.CminPdenit;
            CmaxPdenit = reader.CmaxPdenit;
            MinPdenit = reader.MinPdenit;
            MaxPdenit = reader.MaxPdenit;
            Ratiodenit = reader.Ratiodenit;
            Profdenit = reader.Profdenit;
            Vpotdenit = reader.Vpotdenit;
            applyDefaults();
        }

        public void serialize(WRITER writer)
        {
            writer.UseN2O = UseN2O;
            writer.UseNit = UseNit;
            writer.UseDenit = UseDenit;
            writer.CodeVnit = CodeVnit;
            writer.CodeTnit = CodeTnit;
            writer.CodeRationit = CodeRationit;
            writer.CodeHourlyWfpsNit = CodeHourlyWfpsNit;
            writer.CodePdenit = CodePdenit;
            writer.CodeRatiodenit = CodeRatiodenit;
            writer.CodeHourlyWfpsDenit = CodeHourlyWfpsDenit;
            writer.Hminn = Hminn;
            writer.Hoptn = Hoptn;
            writer.PHminnit = PHminnit;
            writer.PHmaxnit = PHmaxnit;
            writer.Nh4Min = Nh4Min;
            writer.PHminden = PHminden;
            writer.PHmaxden = PHmaxden;
            writer.Wfpsc = Wfpsc;
            writer.TdenitoptGauss = TdenitoptGauss;
            writer.ScaleTdenitopt = ScaleTdenitopt;
            writer.Kd = Kd;
            writer.KDesat = KDesat;
            writer.Fnx = Fnx;
            writer.Vnitmax = Vnitmax;
            writer.Kamm = Kamm;
            writer.Tnitmin = Tnitmin;
            writer.Tnitopt = Tnitopt;
            writer.Tnitop2 = Tnitop2;
            writer.Tnitmax = Tnitmax;
            writer.TnitoptGauss = TnitoptGauss;
            writer.ScaleTnitopt = ScaleTnitopt;
            writer.Rationit = Rationit;
            writer.CminPdenit = CminPdenit;
            writer.CmaxPdenit = CmaxPdenit;
            writer.MinPdenit = MinPdenit;
            writer.MaxPdenit = MaxPdenit;
            writer.Ratiodenit = Ratiodenit;
            writer.Profdenit = Profdenit;
            writer.Vpotdenit = Vpotdenit;
        }

        void ICapnpSerializable.Serialize(SerializerState arg_)
        {
            serialize(arg_.Rewrap<WRITER>());
        }

        public void applyDefaults()
        {
        }

        public bool UseN2O
        {
            get;
            set;
        }

        public bool UseNit
        {
            get;
            set;
        }

        public bool UseDenit
        {
            get;
            set;
        }

        public byte CodeVnit
        {
            get;
            set;
        }

        = 1;
        public byte CodeTnit
        {
            get;
            set;
        }

        = 2;
        public byte CodeRationit
        {
            get;
            set;
        }

        = 2;
        public byte CodeHourlyWfpsNit
        {
            get;
            set;
        }

        = 2;
        public byte CodePdenit
        {
            get;
            set;
        }

        = 1;
        public byte CodeRatiodenit
        {
            get;
            set;
        }

        = 2;
        public byte CodeHourlyWfpsDenit
        {
            get;
            set;
        }

        = 2;
        public double Hminn
        {
            get;
            set;
        }

        = 0.3;
        public double Hoptn
        {
            get;
            set;
        }

        = 0.9;
        public double PHminnit
        {
            get;
            set;
        }

        = 4;
        public double PHmaxnit
        {
            get;
            set;
        }

        = 7.2;
        public double Nh4Min
        {
            get;
            set;
        }

        = 1;
        public double PHminden
        {
            get;
            set;
        }

        = 7.2;
        public double PHmaxden
        {
            get;
            set;
        }

        = 9.2;
        public double Wfpsc
        {
            get;
            set;
        }

        = 0.62;
        public double TdenitoptGauss
        {
            get;
            set;
        }

        = 47;
        public double ScaleTdenitopt
        {
            get;
            set;
        }

        = 25;
        public double Kd
        {
            get;
            set;
        }

        = 148;
        public double KDesat
        {
            get;
            set;
        }

        = 3;
        public double Fnx
        {
            get;
            set;
        }

        = 0.8;
        public double Vnitmax
        {
            get;
            set;
        }

        = 27.3;
        public double Kamm
        {
            get;
            set;
        }

        = 24;
        public double Tnitmin
        {
            get;
            set;
        }

        = 5;
        public double Tnitopt
        {
            get;
            set;
        }

        = 30;
        public double Tnitop2
        {
            get;
            set;
        }

        = 35;
        public double Tnitmax
        {
            get;
            set;
        }

        = 58;
        public double TnitoptGauss
        {
            get;
            set;
        }

        = 32.5;
        public double ScaleTnitopt
        {
            get;
            set;
        }

        = 16;
        public double Rationit
        {
            get;
            set;
        }

        = 0.0016;
        public double CminPdenit
        {
            get;
            set;
        }

        = 1;
        public double CmaxPdenit
        {
            get;
            set;
        }

        = 6;
        public double MinPdenit
        {
            get;
            set;
        }

        = 1;
        public double MaxPdenit
        {
            get;
            set;
        }

        = 20;
        public double Ratiodenit
        {
            get;
            set;
        }

        = 0.2;
        public double Profdenit
        {
            get;
            set;
        }

        = 20;
        public double Vpotdenit
        {
            get;
            set;
        }

        = 2;
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
            public bool UseN2O => ctx.ReadDataBool(0UL, false);
            public bool UseNit => ctx.ReadDataBool(1UL, false);
            public bool UseDenit => ctx.ReadDataBool(2UL, false);
            public byte CodeVnit => ctx.ReadDataByte(8UL, (byte)1);
            public byte CodeTnit => ctx.ReadDataByte(16UL, (byte)2);
            public byte CodeRationit => ctx.ReadDataByte(24UL, (byte)2);
            public byte CodeHourlyWfpsNit => ctx.ReadDataByte(32UL, (byte)2);
            public byte CodePdenit => ctx.ReadDataByte(40UL, (byte)1);
            public byte CodeRatiodenit => ctx.ReadDataByte(48UL, (byte)2);
            public byte CodeHourlyWfpsDenit => ctx.ReadDataByte(56UL, (byte)2);
            public double Hminn => ctx.ReadDataDouble(64UL, 0.3);
            public double Hoptn => ctx.ReadDataDouble(128UL, 0.9);
            public double PHminnit => ctx.ReadDataDouble(192UL, 4);
            public double PHmaxnit => ctx.ReadDataDouble(256UL, 7.2);
            public double Nh4Min => ctx.ReadDataDouble(320UL, 1);
            public double PHminden => ctx.ReadDataDouble(384UL, 7.2);
            public double PHmaxden => ctx.ReadDataDouble(448UL, 9.2);
            public double Wfpsc => ctx.ReadDataDouble(512UL, 0.62);
            public double TdenitoptGauss => ctx.ReadDataDouble(576UL, 47);
            public double ScaleTdenitopt => ctx.ReadDataDouble(640UL, 25);
            public double Kd => ctx.ReadDataDouble(704UL, 148);
            public double KDesat => ctx.ReadDataDouble(768UL, 3);
            public double Fnx => ctx.ReadDataDouble(832UL, 0.8);
            public double Vnitmax => ctx.ReadDataDouble(896UL, 27.3);
            public double Kamm => ctx.ReadDataDouble(960UL, 24);
            public double Tnitmin => ctx.ReadDataDouble(1024UL, 5);
            public double Tnitopt => ctx.ReadDataDouble(1088UL, 30);
            public double Tnitop2 => ctx.ReadDataDouble(1152UL, 35);
            public double Tnitmax => ctx.ReadDataDouble(1216UL, 58);
            public double TnitoptGauss => ctx.ReadDataDouble(1280UL, 32.5);
            public double ScaleTnitopt => ctx.ReadDataDouble(1344UL, 16);
            public double Rationit => ctx.ReadDataDouble(1408UL, 0.0016);
            public double CminPdenit => ctx.ReadDataDouble(1472UL, 1);
            public double CmaxPdenit => ctx.ReadDataDouble(1536UL, 6);
            public double MinPdenit => ctx.ReadDataDouble(1600UL, 1);
            public double MaxPdenit => ctx.ReadDataDouble(1664UL, 20);
            public double Ratiodenit => ctx.ReadDataDouble(1728UL, 0.2);
            public double Profdenit => ctx.ReadDataDouble(1792UL, 20);
            public double Vpotdenit => ctx.ReadDataDouble(1856UL, 2);
        }

        public class WRITER : SerializerState
        {
            public WRITER()
            {
                this.SetStruct(30, 0);
            }

            public bool UseN2O
            {
                get => this.ReadDataBool(0UL, false);
                set => this.WriteData(0UL, value, false);
            }

            public bool UseNit
            {
                get => this.ReadDataBool(1UL, false);
                set => this.WriteData(1UL, value, false);
            }

            public bool UseDenit
            {
                get => this.ReadDataBool(2UL, false);
                set => this.WriteData(2UL, value, false);
            }

            public byte CodeVnit
            {
                get => this.ReadDataByte(8UL, (byte)1);
                set => this.WriteData(8UL, value, (byte)1);
            }

            public byte CodeTnit
            {
                get => this.ReadDataByte(16UL, (byte)2);
                set => this.WriteData(16UL, value, (byte)2);
            }

            public byte CodeRationit
            {
                get => this.ReadDataByte(24UL, (byte)2);
                set => this.WriteData(24UL, value, (byte)2);
            }

            public byte CodeHourlyWfpsNit
            {
                get => this.ReadDataByte(32UL, (byte)2);
                set => this.WriteData(32UL, value, (byte)2);
            }

            public byte CodePdenit
            {
                get => this.ReadDataByte(40UL, (byte)1);
                set => this.WriteData(40UL, value, (byte)1);
            }

            public byte CodeRatiodenit
            {
                get => this.ReadDataByte(48UL, (byte)2);
                set => this.WriteData(48UL, value, (byte)2);
            }

            public byte CodeHourlyWfpsDenit
            {
                get => this.ReadDataByte(56UL, (byte)2);
                set => this.WriteData(56UL, value, (byte)2);
            }

            public double Hminn
            {
                get => this.ReadDataDouble(64UL, 0.3);
                set => this.WriteData(64UL, value, 0.3);
            }

            public double Hoptn
            {
                get => this.ReadDataDouble(128UL, 0.9);
                set => this.WriteData(128UL, value, 0.9);
            }

            public double PHminnit
            {
                get => this.ReadDataDouble(192UL, 4);
                set => this.WriteData(192UL, value, 4);
            }

            public double PHmaxnit
            {
                get => this.ReadDataDouble(256UL, 7.2);
                set => this.WriteData(256UL, value, 7.2);
            }

            public double Nh4Min
            {
                get => this.ReadDataDouble(320UL, 1);
                set => this.WriteData(320UL, value, 1);
            }

            public double PHminden
            {
                get => this.ReadDataDouble(384UL, 7.2);
                set => this.WriteData(384UL, value, 7.2);
            }

            public double PHmaxden
            {
                get => this.ReadDataDouble(448UL, 9.2);
                set => this.WriteData(448UL, value, 9.2);
            }

            public double Wfpsc
            {
                get => this.ReadDataDouble(512UL, 0.62);
                set => this.WriteData(512UL, value, 0.62);
            }

            public double TdenitoptGauss
            {
                get => this.ReadDataDouble(576UL, 47);
                set => this.WriteData(576UL, value, 47);
            }

            public double ScaleTdenitopt
            {
                get => this.ReadDataDouble(640UL, 25);
                set => this.WriteData(640UL, value, 25);
            }

            public double Kd
            {
                get => this.ReadDataDouble(704UL, 148);
                set => this.WriteData(704UL, value, 148);
            }

            public double KDesat
            {
                get => this.ReadDataDouble(768UL, 3);
                set => this.WriteData(768UL, value, 3);
            }

            public double Fnx
            {
                get => this.ReadDataDouble(832UL, 0.8);
                set => this.WriteData(832UL, value, 0.8);
            }

            public double Vnitmax
            {
                get => this.ReadDataDouble(896UL, 27.3);
                set => this.WriteData(896UL, value, 27.3);
            }

            public double Kamm
            {
                get => this.ReadDataDouble(960UL, 24);
                set => this.WriteData(960UL, value, 24);
            }

            public double Tnitmin
            {
                get => this.ReadDataDouble(1024UL, 5);
                set => this.WriteData(1024UL, value, 5);
            }

            public double Tnitopt
            {
                get => this.ReadDataDouble(1088UL, 30);
                set => this.WriteData(1088UL, value, 30);
            }

            public double Tnitop2
            {
                get => this.ReadDataDouble(1152UL, 35);
                set => this.WriteData(1152UL, value, 35);
            }

            public double Tnitmax
            {
                get => this.ReadDataDouble(1216UL, 58);
                set => this.WriteData(1216UL, value, 58);
            }

            public double TnitoptGauss
            {
                get => this.ReadDataDouble(1280UL, 32.5);
                set => this.WriteData(1280UL, value, 32.5);
            }

            public double ScaleTnitopt
            {
                get => this.ReadDataDouble(1344UL, 16);
                set => this.WriteData(1344UL, value, 16);
            }

            public double Rationit
            {
                get => this.ReadDataDouble(1408UL, 0.0016);
                set => this.WriteData(1408UL, value, 0.0016);
            }

            public double CminPdenit
            {
                get => this.ReadDataDouble(1472UL, 1);
                set => this.WriteData(1472UL, value, 1);
            }

            public double CmaxPdenit
            {
                get => this.ReadDataDouble(1536UL, 6);
                set => this.WriteData(1536UL, value, 6);
            }

            public double MinPdenit
            {
                get => this.ReadDataDouble(1600UL, 1);
                set => this.WriteData(1600UL, value, 1);
            }

            public double MaxPdenit
            {
                get => this.ReadDataDouble(1664UL, 20);
                set => this.WriteData(1664UL, value, 20);
            }

            public double Ratiodenit
            {
                get => this.ReadDataDouble(1728UL, 0.2);
                set => this.WriteData(1728UL, value, 0.2);
            }

            public double Profdenit
            {
                get => this.ReadDataDouble(1792UL, 20);
                set => this.WriteData(1792UL, value, 20);
            }

            public double Vpotdenit
            {
                get => this.ReadDataDouble(1856UL, 2);
                set => this.WriteData(1856UL, value, 2);
            }
        }
    }
}