@0xeef9ddc7a345de6d;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::model::monica");

using Go = import "/capnp/go.capnp";
$Go.package("monica");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/model/monica");

using Date = import "/date.capnp".Date;
using Mgmt = import "monica_management.capnp";
using Climate = import "/climate.capnp";

# -----------------------------------------------------------------------------
# crop related parameters
# -----------------------------------------------------------------------------

struct CropSpec {
    cropParams      @0 :CropParameters;
    residueParams   @1 :CropResidueParameters;
}

struct CropParameters {
    speciesParams   @0 :SpeciesParameters;
    cultivarParams  @1 :CultivarParameters;
}

struct SpeciesParameters {
    speciesId                                   @0  :Text;
    carboxylationPathway                        @1  :UInt8;
    defaultRadiationUseEfficiency               @2  :Float64;
    partBiologicalNFixation                     @3  :Float64;
    initialKcFactor                             @4  :Float64;
    luxuryNCoeff                                @5  :Float64;
    maxCropDiameter                             @6  :Float64;
    stageAtMaxHeight                            @7  :Float64;
    stageAtMaxDiameter                          @8  :Float64;
    minimumNConcentration                       @9  :Float64;
    minimumTemperatureForAssimilation           @10 :Float64;
    optimumTemperatureForAssimilation           @11 :Float64;
    maximumTemperatureForAssimilation           @12 :Float64;
    nConcentrationAbovegroundBiomass            @13 :Float64;
    nConcentrationB0                            @14 :Float64;
    nConcentrationPN                            @15 :Float64;
    nConcentrationRoot                          @16 :Float64;
    developmentAccelerationByNitrogenStress     @17 :UInt16;
    fieldConditionModifier                      @18 :Float64 = 1.0;
    assimilateReallocation                      @19 :Float64;

    baseTemperature                             @20 :List(Float64);
    organMaintenanceRespiration                 @21 :List(Float64);
    organGrowthRespiration                      @22 :List(Float64);
    stageMaxRootNConcentration                  @23 :List(Float64);
    initialOrganBiomass                         @24 :List(Float64);
    criticalOxygenContent                       @25 :List(Float64);
	stageMobilFromStorageCoeff                  @26 :List(Float64);

    abovegroundOrgan                            @27 :List(Bool);
    storageOrgan                                @28 :List(Bool);

    samplingDepth                               @29 :Float64;
    targetNSamplingDepth                        @30 :Float64;
    targetN30                                   @31 :Float64;
    maxNUptakeParam                             @32 :Float64;
    rootDistributionParam                       @33 :Float64;
    plantDensity                                @34 :UInt16; # [plants m-2]
    rootGrowthLag                               @35 :Float64;
    minimumTemperatureRootGrowth                @36 :Float64;
    initialRootingDepth                         @37 :Float64;
    rootPenetrationRate                         @38 :Float64;
    rootFormFactor                              @39 :Float64;
    specificRootLength                          @40 :Float64;
    stageAfterCut                               @41 :UInt16;
    limitingTemperatureHeatStress               @42 :Float64;
    cuttingDelayDays                            @43 :UInt16;
    droughtImpactOnFertilityFactor              @44 :Float64;

    efMono                                      @45 :Float64 = 0.5;     # = MTsynt [ug gDW-1 h-1] Monoterpenes, which will be emitted right after synthesis
    efMonos                                     @46 :Float64 = 0.5;     # = MTpool [ug gDW-1 h-1] Monoterpenes, which will be stored after synthesis in stores (mostly intra- oder intercellular space of leafs and then are being emitted; quasi evaporation)
    efIso                                       @47 :Float64;           # Isoprene emission factor
    vcMax25                                     @48 :Float64;           # maximum RubP saturated rate of carboxylation at 25oC for sun leaves (umol m-2 s-1)
    aekc                                        @49 :Float64 = 65800.0; # activation energy for Michaelis-Menten constant for CO2 (J mol-1) | MONICA default=65800.0 | LDNDC default=59356.0
    aeko                                        @50 :Float64 = 1400.0;  # activation energy for Michaelis-Menten constant for O2 (J mol-1) | MONICA default=65800.0 | LDNDC default=35948.0
    aevc                                        @51 :Float64 = 68800.0; # activation energy for photosynthesis (J mol-1) | MONICA default=68800.0 | LDNDC default=58520.0
    kc25                                        @52 :Float64 = 460.0;   # Michaelis-Menten constant for CO2 at 25oC (umol mol-1 ubar-1) | MONICA default=460.0 | LDNDC default=260.0
    ko25                                        @53 :Float64 = 330.0;   # Michaelis-Menten constant for O2 at 25oC (mmol mol-1 mbar-1) | MONICA default=330.0 | LDNDC default=179.0

    transitionStageLeafExp                      @54 :Int16 = -1; # [1-7]
}

struct CultivarParameters {
    cultivarId                              @0  :Text;
    description                             @1  :Text;
    perennial                               @2  :Bool;
    #std::string pc_PermanentCultivarId;
    maxAssimilationRate                     @3  :Float64;
    maxCropHeight                           @4  :Float64;
    residueNRatio                           @5  :Float64;
    lt50cultivar                            @6  :Float64;

    cropHeightP1                            @7  :Float64;
    cropHeightP2                            @8  :Float64;
    cropSpecificMaxRootingDepth             @9  :Float64;

    assimilatePartitioningCoeff             @10 :List(List(Float64));
    organSenescenceRate                     @11 :List(List(Float64));

    baseDaylength                           @12 :List(Float64);
    optimumTemperature                      @13 :List(Float64);
    daylengthRequirement                    @14 :List(Float64);
    droughtStressThreshold                  @15 :List(Float64);
    specificLeafArea                        @16 :List(Float64);
    stageKcFactor                           @17 :List(Float64);
    stageTemperatureSum                     @18 :List(Float64);
    vernalisationRequirement                @19 :List(Float64);

    heatSumIrrigationStart                  @20 :Float64;
    heatSumIrrigationEnd                    @21 :Float64;

    criticalTemperatureHeatStress           @22 :Float64;
    beginSensitivePhaseHeatStress           @23 :Float64;
    endSensitivePhaseHeatStress             @24 :Float64;

    frostHardening                          @25 :Float64;
    frostDehardening                        @26 :Float64;
    lowTemperatureExposure                  @27 :Float64;
    respiratoryStress                       @28 :Float64;
    latestHarvestDoy                        @29 :Int16 = -1;

    organIdsForPrimaryYield                 @30 :List(YieldComponent);
    organIdsForSecondaryYield               @31 :List(YieldComponent);
    organIdsForCutting                      @32 :List(YieldComponent);

    earlyRefLeafExp                         @33 :Float64 = 12.0; # 12 = wheat (first guess)
    refLeafExp                              @34 :Float64 = 20.0; # 20 = wheat, 22 = maize (first guess)

    minTempDevWE                            @35 :Float64;
    optTempDevWE                            @36 :Float64;
    maxTempDevWE                            @37 :Float64;
    winterCrop                              @38 :Bool;
}

struct YieldComponent {
    organId         @0 :Int64 = -1;
	yieldPercentage @1 :Float64;
	yieldDryMatter  @2 :Float64;
}

struct AutomaticHarvestParameters {
    enum HarvestTime {
        maturity    @0;
        unknown     @1;
    }
    harvestTime         @0 :HarvestTime = unknown; # Harvest time parameter
    latestHarvestDOY    @1 :Int16 = -1;
}

struct NMinCropParameters {
    samplingDepth   @0 :Float64;
	nTarget         @1 :Float64;
	nTarget30       @2 :Float64;
}

# -----------------------------------------------------------------------------
# fertilizer parameters
# -----------------------------------------------------------------------------

#struct MineralFertilizerParameters {
#    id          @0 :Text;
#	name        @1 :Text;
#    carbamid    @2 :Float64; # [%]
#	nh4         @3 :Float64; # [%]
#	no3         @4 :Float64; # [%]
#}

struct NMinApplicationParameters {
    min         @0 :Float64;
    max         @1 :Float64;
    delayInDays @2 :UInt16;
}

#struct OrganicMatterParameters {
#    aomDryMatterContent         @0  :Float64; # Dry matter content of added organic matter [kg DM kg FM-1]
#    aomNH4Content               @1  :Float64; # Ammonium content in added organic matter [kg N kg DM-1]
#    aomNO3Content               @2  :Float64; # Nitrate content in added organic matter [kg N kg DM-1]
#    aomCarbamidContent          @3  :Float64; # Carbamide content in added organic matter [kg N kg DM-1]

#    aomSlowDecCoeffStandard     @4  :Float64; # Decomposition rate coefficient of slow AOM at standard conditions [d-1]
#    aomFastDecCoeffStandard     @5  :Float64; # Decomposition rate coefficient of fast AOM at standard conditions [d-1]

#    partAOMToAOMSlow            @6  :Float64; # Part of AOM that is assigned to the slowly decomposing pool [kg kg-1]
#    partAOMToAOMFast            @7  :Float64; # Part of AOM that is assigned to the rapidly decomposing pool [kg kg-1]

#    cnRatioAOMSlow              @8  :Float64; # C to N ratio of the slowly decomposing AOM pool []
#    cnRatioAOMFast              @9  :Float64; # C to N ratio of the rapidly decomposing AOM pool []

#    partAOMSlowToSMBSlow        @10 :Float64; # Part of AOM slow consumed by slow soil microbial biomass [kg kg-1]
#    partAOMSlowToSMBFast        @11 :Float64; # Part of AOM slow consumed by fast soil microbial biomass [kg kg-1]

#    nConcentration              @12 :Float64;
#}

#struct OrganicFertilizerParameters {
#    params  @0 :OrganicMatterParameters;
#    id      @1 :Text;
#	name    @2 :Text;
#}

struct CropResidueParameters {
    params      @0 :Mgmt.Params.OrganicFertilization.OrganicMatterParameters;
    species     @1 :Text;
    residueType @2 :Text;
}

# -----------------------------------------------------------------------------
# soil parameters
# -----------------------------------------------------------------------------

struct SoilParameters {
    soilSandContent         @0  :Float64 = -1.0;    # Soil layer's sand content [kg kg-1] //{0.4}
	soilClayContent         @1  :Float64 = -1.0;    # Soil layer's clay content [kg kg-1] (Ton) //{0.05}
	soilpH                  @2  :Float64 = 6.9;     # Soil pH value [] //{7.0}
	soilStoneContent        @3  :Float64;           # Soil layer's stone content in soil [m3 m-3]
	lambda                  @4  :Float64 = -1.0;    # Soil water conductivity coefficient [] //{0.5}
	fieldCapacity           @5  :Float64 = -1.0;    # {0.21} [m3 m-3]
	saturation              @6  :Float64 = -1.0;    # {0.43} [m3 m-3]
	permanentWiltingPoint   @7  :Float64 = -1.0;    # {0.08} [m3 m-3]
	soilTexture             @8  :Text;
	soilAmmonium            @9  :Float64 = 0.0005;  # soil ammonium content [kg NH4-N m-3]
	soilNitrate             @10 :Float64 = 0.005;   # soil nitrate content [kg NO3-N m-3]
	soilCNRatio             @11 :Float64 = 10.0;
	soilMoisturePercentFC   @12 :Float64 = 100.0;
	soilRawDensity          @13 :Float64 = -1.0;    # [kg m-3]
	soilBulkDensity         @14 :Float64 = -1.0;    # [kg m-3]
	soilOrganicCarbon       @15 :Float64 = -1.0;    # [kg kg-1]
	soilOrganicMatter       @16 :Float64 = -1.0;    # [kg kg-1]
}

# -----------------------------------------------------------------------------
# irrigation parameters
# -----------------------------------------------------------------------------

#struct IrrigationParameters {
#    nitrateConcentration @0 :Float64;   # nitrate concentration [mg dm-3]
#	sulfateConcentration @1 :Float64;   # sulfate concentration [mg dm-3]
#}

struct AutomaticIrrigationParameters {
    params      @0 :Mgmt.Params.Irrigation.Parameters;
    amount      @1 :Float64 = 17.0;
	threshold   @2 :Float64 = 0.35;
}

# -----------------------------------------------------------------------------
# site related parameters
# -----------------------------------------------------------------------------

struct SiteParameters {
    latitude                                @0  :Float64 = 52.5; # ZALF latitude
    slope                                   @1  :Float64 = 0.01; # [m m-1]
    heightNN                                @2  :Float64 = 50.0; # [m]
    groundwaterDepth                        @3  :Float64 = 70.0; # [m]
    soilCNRatio                             @4  :Float64 = 10.0;
    drainageCoeff                           @5  :Float64 = 1.0;
    vqNDeposition                           @6  :Float64 = 30.0; # [kg N ha-1 y-1]
    maxEffectiveRootingDepth                @7  :Float64 = 2.0;  # [m]
    impenetrableLayerDepth                  @8  :Float64 = -1;   # [m]
    soilSpecificHumusBalanceCorrection      @9  :Float64;        # humus equivalents

    soilParameters                          @10 :List(SoilParameters);
}

struct EnvironmentParameters {
    struct YearToValue {
        year @0 :UInt16;
        value @1 :Float64;
    }

    albedo                      @0  :Float64 = 0.23;
    rcp                         @11 :Climate.RCP;
    atmosphericCO2              @1  :Float64;
    atmosphericCO2s             @2  :List(YearToValue);
    atmosphericO3               @3  :Float64;
    atmosphericO3s              @4  :List(YearToValue);
    windSpeedHeight             @5  :Float64 = 2.0;
    leachingDepth               @6  :Float64;
    timeStep                    @7  :Float64;

    maxGroundwaterDepth         @8  :Float64 = 18.0;
    minGroundwaterDepth         @9  :Float64 = 20.0;
    minGroundwaterDepthMonth    @10 :UInt8 = 3;
}

struct MeasuredGroundwaterTableInformation {
    struct DateToValue {
        date    @0 :Date;
        value   @1 :Float64;
    }
    
    groundwaterInformationAvailable @0 :Bool;
	groundwaterInfo                 @1 :List(DateToValue);
}

# -----------------------------------------------------------------------------
# simulation parameters
# -----------------------------------------------------------------------------

struct SimulationParameters {
    startDate                       @0  :Date;
    endDate                         @1  :Date;

    nitrogenResponseOn              @2  :Bool = true;
    waterDeficitResponseOn          @3  :Bool = true;
    emergenceFloodingControlOn      @4  :Bool = true;
    emergenceMoistureControlOn      @5  :Bool = true;
    frostKillOn                     @6  :Bool = true;

    useAutomaticIrrigation          @7  :Bool;
    autoIrrigationParams            @8  :AutomaticIrrigationParameters;

    useNMinMineralFertilisingMethod @9  :Bool;
    nMinFertiliserPartition         @10 :Mgmt.Params.MineralFertilization.Parameters;
    nMinApplicationParams           @11 :NMinApplicationParameters;

    useSecondaryYields              @12 :Bool = true;
    useAutomaticHarvestTrigger      @13 :Bool;

    numberOfLayers                  @14 :UInt16 = 20;
    layerThickness                  @15 :Float64 = 0.1;

    startPVIndex                    @16 :UInt16;
    julianDayAutomaticFertilising   @17 :UInt16;
}

# -----------------------------------------------------------------------------
# MONICA module parameters
# -----------------------------------------------------------------------------

struct CropModuleParameters {
    canopyReflectionCoefficient                                     @0  :Float64;
    referenceMaxAssimilationRate                                    @1  :Float64;
    referenceLeafAreaIndex                                          @2  :Float64;
    maintenanceRespirationParameter1                                @3  :Float64;
    maintenanceRespirationParameter2                                @4  :Float64;
    minimumNConcentrationRoot                                       @5  :Float64;
    minimumAvailableN                                               @6  :Float64;
    referenceAlbedo                                                 @7  :Float64;
    stomataConductanceAlpha                                         @8  :Float64;
    saturationBeta                                                  @9  :Float64;
    growthRespirationRedux                                          @10 :Float64;
    maxCropNDemand                                                  @11 :Float64;
    growthRespirationParameter1                                     @12 :Float64;
    growthRespirationParameter2                                     @13 :Float64;
    tortuosity                                                      @14 :Float64;
    adjustRootDepthForSoilProps                                     @15 :Bool;  

    experimentalEnablePhenologyWangEngelTemperatureResponse         @16 :Bool;
    experimentalEnablePhotosynthesisWangEngelTemperatureResponse    @17 :Bool;
    experimentalEnableHourlyFvCBPhotosynthesis                      @18 :Bool;
    experimentalEnableTResponseLeafExpansion                        @19 :Bool;
    experimentalDisableDailyRootBiomassToSoil                       @20 :Bool;
    enableVernalisationFactorFix                                    @21 :Bool;
}

struct SoilMoistureModuleParameters {
    criticalMoistureDepth               @0  :Float64;
    saturatedHydraulicConductivity      @1  :Float64;
    surfaceRoughness                    @2  :Float64;
    groundwaterDischarge                @3  :Float64;
    hydraulicConductivityRedux          @4  :Float64;
    snowAccumulationTresholdTemperature @5  :Float64;
    kcFactor                            @6  :Float64;
    temperatureLimitForLiquidWater      @7  :Float64;
    correctionSnow                      @8  :Float64;
    correctionRain                      @9  :Float64;
    snowMaxAdditionalDensity            @10 :Float64;
    newSnowDensityMin                   @11 :Float64;
    snowRetentionCapacityMin            @12 :Float64;
    refreezeParameter1                  @13 :Float64;
    refreezeParameter2                  @14 :Float64;
    refreezeTemperature                 @15 :Float64;
    snowMeltTemperature                 @16 :Float64;
    snowPacking                         @17 :Float64;
    snowRetentionCapacityMax            @18 :Float64;
    evaporationZeta                     @19 :Float64;
    xsaCriticalSoilMoisture             @20 :Float64;
    maximumEvaporationImpactDepth       @21 :Float64;
    maxPercolationRate                  @22 :Float64;
    moistureInitValue                   @23 :Float64;
}

struct SoilOrganicModuleParameters {
    somSlowDecCoeffStandard             @0  :Float64 = 4.30e-5;     # 4.30e-5 [d-1], Bruun et al. 2003 4.3e-5
    somFastDecCoeffStandard             @1  :Float64 = 1.40e-4;     # 1.40e-4 [d-1], from DAISY manual 1.4e-4
    smbSlowMaintRateStandard            @2  :Float64 = 1.00e-3;     # 1.00e-3 [d-1], from DAISY manual original 1.8e-3
    smbFastMaintRateStandard            @3  :Float64 = 1.00e-2;     # 1.00e-2 [d-1], from DAISY manual
    smbSlowDeathRateStandard            @4  :Float64 = 1.00e-3;     # 1.00e-3 [d-1], from DAISY manual
    smbFastDeathRateStandard            @5  :Float64 = 1.00e-2;     # 1.00e-2 [d-1], from DAISY manual
    smbUtilizationEfficiency            @6  :Float64 = 0.60;        # 0.60 [], from DAISY manual 0.6
    somSlowUtilizationEfficiency        @7  :Float64 = 0.40;        # 0.40 [], from DAISY manual 0.4
    somFastUtilizationEfficiency        @8  :Float64 = 0.50;        # 0.50 [], from DAISY manual 0.5
    aomSlowUtilizationEfficiency        @9  :Float64 = 0.40;        # 0.40 [], from DAISY manual original 0.13
    aomFastUtilizationEfficiency        @10 :Float64 = 0.10;        # 0.10 [], from DAISY manual original 0.69
    aomFastMaxCtoN                      @11 :Float64 = 1000.0;      
    partSOMFastToSOMSlow                @12 :Float64 = 0.30;        # 0.30 [], Bruun et al. 2003
    partSMBSlowToSOMFast                @13 :Float64 = 0.60;        # 0.60 [], from DAISY manual
    partSMBFastToSOMFast                @14 :Float64 = 0.60;        # 0.60 [], from DAISY manual
    partSOMToSMBSlow                    @15 :Float64 = 0.0150;      # 0.0150 [], optimised
    partSOMToSMBFast                    @16 :Float64 = 0.0002;      # 0.0002 [], optimised
    cnRatioSMB                          @17 :Float64 = 6.70;        # 6.70 [], from DAISY manual
    limitClayEffect                     @18 :Float64 = 0.25;        # 0.25 [kg kg-1], from DAISY manual
    ammoniaOxidationRateCoeffStandard   @19 :Float64 = 1.0e-1;      # 1.0e-1 [d-1], from DAISY manual
    nitriteOxidationRateCoeffStandard   @20 :Float64 = 9.0e-1;      # 9.0e-1 [d-1], fudged by Florian Stange
    transportRateCoeff                  @21 :Float64 = 0.1;         # 0.1 [d-1], from DAISY manual
    specAnaerobDenitrification          @22 :Float64 = 0.1;         # 0.1 [g gas-N g CO2-C-1]
    immobilisationRateCoeffNO3          @23 :Float64 = 0.5;         # 0.5 [d-1]
    immobilisationRateCoeffNH4          @24 :Float64 = 0.5;         # 0.5 [d-1]
    denit1                              @25 :Float64 = 0.2;         # 0.2 Denitrification parameter
    denit2                              @26 :Float64 = 0.8;         # 0.8 Denitrification parameter
    denit3                              @27 :Float64 = 0.9;         # 0.9 Denitrification parameter
    hydrolysisKM                        @28 :Float64 = 0.00334;     # 0.00334 from Tabatabai 1973
    activationEnergy                    @29 :Float64 = 41000.0;     # 41000.0 from Gould et al. 1973
    hydrolysisP1                        @30 :Float64 = 4.259e-12;   # 4.259e-12 from Sadeghi et al. 1988
    hydrolysisP2                        @31 :Float64 = 1.408e-12;   # 1.408e-12 from Sadeghi et al. 1988
    atmosphericResistance               @32 :Float64 = 0.0025;      # 0.0025 [s m-1], from Sadeghi et al. 1988
    n2oProductionRate                   @33 :Float64 = 0.5;         # 0.5 [d-1]
    inhibitorNH3                        @34 :Float64 = 1.0;         # 1.0 [kg N m-3] NH3-induced inhibitor for nitrite oxidation
    psMaxMineralisationDepth            @35 :Float64 = 0.4;

    sticsParams                         @36 :SticsParameters;
}

struct SoilTemperatureModuleParameters {
    nTau                        @0  :Float64;
    initialSurfaceTemperature   @1  :Float64;
    baseTemperature             @2  :Float64;
    quartzRawDensity            @3  :Float64;
    densityAir                  @4  :Float64;
    densityWater                @5  :Float64;
    densityHumus                @6  :Float64;
    specificHeatCapacityAir     @7  :Float64;
    specificHeatCapacityQuartz  @8  :Float64;
    specificHeatCapacityWater   @9  :Float64;
    specificHeatCapacityHumus   @10 :Float64;
    soilAlbedo                  @11 :Float64;
    soilMoisture                @12 :Float64 = 0.25;
}

struct SoilTransportModuleParameters {
    dispersionLength                @0 :Float64;
    ad                              @1 :Float64;
    diffusionCoefficientStandard    @2 :Float64;
    nDeposition                     @3 :Float64;
}

# -----------------------------------------------------------------------------
# special module parameters
# -----------------------------------------------------------------------------

struct Voc {

#    const kilo          :Float64 = 1000.0;
#    const milli         :Float64 = 0.001;

	# conv constants
#	const nmol2umol     :Float64 = Voc.kilo;               # nmol to umol
#	const umol2nmol     :Float64 = 0.001;               # (= 1.0 / NMOL2UMOL) umol to nmol
#	const mol2mmol      :Float64 = Voc.milli;
#	const mmol2mol      :Float64 = 1000.0;              # (= 1.0 / MOL2MMOL)
#	const fPar          :Float64 = 0.45;                # conversion factor for global into PAR (Monteith 1965, Meek et al. 1984)
#	const d2k           :Float64 = 273.15;              # kelvin at zero degree celsius
#	const g2kg          :Float64 = 1000.0;              # 0.001 kg per g
#	const umol2w        :Float64 = 4.57;                # conversion factor from Watt in umol PAR (Cox et al. 1998)
#	const w2umol        :Float64 = 0.2188183807439825;  # (= 1.0 / UMOL2W) conversion factor from umol PAR in Watt (Cox et al. 1998)
#	const ng2ug         :Float64 = 1000.0;              # conversion factor from nano to micro (gramm)
#	const ug2ng         :Float64 = 0.001;                # (= 1.0 / NG2UG) 

	# phys constants
#	const rGas          :Float64 = 8.3143;              # general gas constant  [J mol-1 K-1]

	# chem constants
#	const mc            :Float64 = 12.0;                # molecular weight of carbon  [g mol-1]
#	const cIso          :Float64 = 5.0;                 # number of carbons in Isoprene (C5H8)
#	const cMono         :Float64 = 10.0;                # number of carbons in Monoterpene (C10H16)

	# time constants
#	const secInMin      :UInt8 = 60;                    # minute to seconds
#	const minInHr       :UInt8 = 60;                    # hour to minutes
#	const hrInDay       :UInt8 = 24;                    # day to hours
#	const monthsInYear  :UInt8 = 12;                    # year to months
#	const secInHr       :UInt16 = 3600;                 # hour to seconds
#	const minInDay      :UInt16 = 1440;                 # day to minutes
#	const secInDay      :UInt32 = 86400;                # day to seconds

	# meteo constants
#	const po2           :Float64 = 0.208;               # volumentric percentage of oxygen in the canopy air

	# voc module specific constants
#	const abso          :Float64 = 0.860;               # absorbance factor, Collatz et al. 1991 
#	const alpha         :Float64 = 0.0027;              # light modifier, Guenther et al. 1993; (Staudt et al. 2004 f. Q.ilex: 0.0041, Bertin et al. 1997 f. Q.ilex: 0.00147, Harley et al. 2004 f. M.indica: 0.00145) 
#	const beta          :Float64 = 0.09;                # monoterpene scaling factor, Guenther et al. 1995 (cit. in Guenther 1999 says this value originates from Guenther 1993) 
#	const c1            :Float64 = 0.17650;             #fw: 0.17650e-3 in Grote et al. 2014 to preserve the constant relation to C2; #  fraction of electrons used from excess electron transport (−), from Sun et al. 2012 dataset, Grote et al. 2014 
#	const c2            :Float64 = 0.00280;             #fw: 0.00280e-3 (umol m-2 s-1) in Grote et al. 2014; here in nmol m-2 s-1; # fraction of electrons used from photosynthetic electron transport (−),from Sun et al. 2012 dataset, Grote et al. 2014 
#	const ceoIso        :Float64 = 2.0;                 # emission-class dependent empirical coefficient for temperature acitivity factor of isoprene from MEGAN v2.1 (Guenther et al. 2012)
#	const ceoMono       :Float64 = 1.83;                # emission-class dependent empirical coefficient for temperature acitivity factor of a-pinene, b-pinene, oMT, Limonen, etc from MEGAN v2.1 (Guenther et al. 2012)
#	const ct1           :Float64 = 95000.0;             # first temperature modifier (J mol-1), Guenther et al. 1993; (Staudt et al. 2004 f. Q.ilex: 87620, Bertin et al. 1997 f. Q.ilex: 74050, Harley et al. f. M.indica: 124600, in WIMOVAC 95100) 
#	const ct2           :Float64 = 230000.0;            # second temperature modifier (J mol-1), Guenther et al. 1993; (Staudt et al. 2004 f. Q.ilex: 188200, Bertin et al. 1997 f. Q.ilex: 638600, Harley et al. f. M.indica: 254800) 
#	const cl1           :Float64 = 1.066;               # radiation modifier, Guenther et al. 1993; (Staudt et al. 2004 f. Q.ilex: 1.04, Bertin et al. 1997 f. Q.ilex: 1.21, Harley et al. 2004 f. M.indica: 1.218) 
#	const gammaMax      :Float64 = 34.0;                # saturating amount of electrons that can be supplied from other sources (μmol m−2 s−1), Sun et al. 2012 dataset, Grote et al. 2014 (delta J_sat in paper)
#	const ppfd0         :Float64 = 1000.0;              # reference photosynthetically active quantum flux density/light density (standard conditions) (umol m-2 s-1), Guenther et al. 1993 
#	const temp0         :Float64 = 298.15;              # (= 25.0 + D2K) reference (leaf) temperature (standard conditions) (K), Guenther et al. 1993 
#	const topT          :Float64 = 314.0;               # temperature with maximum emission (K), Guenther et al. 1993; (Staudt et al. 2004 f. Q.ilex: 317, Bertin et al. 1997 f. Q.ilex: 311.6, Harley et al. f. M.indica: 313.4, in WIMOVAC 311.83) 
#	const tRef          :Float64 = 303.15;              # (= 30.0 + D2K) reference temperature (K), Guenther et al. 1993 
		
	# photofarquhar specific constants
#	const tk25 :Float64 = 298.16;

    struct Emissions {
        struct SpeciesIdToEmission {
            speciesId   @0 :UInt64;
            emission    @1 :Float64;
        }

        speciesIdToIsopreneEmission     @0 :List(SpeciesIdToEmission);  # [umol m-2Ground ts-1] isoprene emissions per timestep and plant
		speciesIdToMonoterpeneEmission  @1 :List(SpeciesIdToEmission);  # [umol m-2Ground ts-1] monoterpene emissions per timestep and plant

		isopreneEmission                @2 :Float64;                    # [umol m-2Ground ts-1] isoprene emissions per timestep
		monoterpeneEmission             @3 :Float64;                    # [umol m-2Ground ts-1] monoterpene emissions per timestep
    }

    struct SpeciesData {
        id          @0  :UInt64;

		# common
		efMonos     @1  :Float64;               # emission rate of stored terpenes under standard conditions (ug gDW-1 h-1)
		efMono      @2  :Float64;               # monoterpene emission rate under standard conditions (ug gDW-1 h-1)
		efIso       @3  :Float64;               # isoprene emission rate under standard conditions (ug gDW-1 h-1)

		# jjv
		theta       @4  :Float64 = 0.9;         # curvature parameter
		fage        @5  :Float64 = 1.0;         # relative decrease of emission synthesis per foliage age class
		ctIs        @6  :Float64;               # scaling constant for temperature sensitivity of isoprene synthase.
		ctMt        @7  :Float64;               # scaling constant for temperature sensitivity

		haIs        @8  :Float64;               # activation energy for isoprene synthase (J mol-1)
		haMt        @9  :Float64;               # activation energy for GDP synthase (J mol-1)

		dsIs        @10 :Float64;               # entropy term for isoprene synthase sensitivity to temperature (J:mol-1:K-1)
		dsMt        @11 :Float64;               # entropy term for GDP synthase sensitivity to temperature (J:mol-1:K-1)
		hdIs        @12 :Float64 = 284600.0;    # deactivation energy for isoprene synthase (J mol-1)
		hdMt        @13 :Float64 = 284600.0;    # deactivation energy for monoterpene synthase (J mol-1)

		hdj         @14 :Float64 = 220000.0;    # fw: "HDJ": curvature parameter of jMax (J mol-1) (Kattge et al. 2007: 200000; Farquhar et al. 1980: 220000; Harley et al. 1992: 201000) 
		sdj         @15 :Float64 = 703.0;       # fw: "SDJ": electron transport temperature response parameter (Kattge et al. 2007: 647; Farquhar et al. 1980: 710; Harley et al. 1992: 650)
		kc25        @16 :Float64 = 260.0;       # Michaelis-Menten constant for CO2 at 25oC (umol mol-1 ubar-1)
		ko25        @17 :Float64 = 179.0;       # Michaelis-Menten constant for O2 at 25oC (mmol mol-1 mbar-1)
		vcMax25     @18 :Float64 = 80.0;        # corn: 13.1 | maximum RubP saturated rate of carboxylation at 25oC for sun leaves (umol m-2 s-1)
		qjvc        @19 :Float64 = 2.0;         # relation between maximum electron transport rate and RubP saturated rate of carboxylation (--)

		aekc        @20 :Float64 = 59356;       # for corn | activation energy for Michaelis-Menten constant for CO2 (J mol-1)
		aeko        @21 :Float64 = 35948;       # for corn | activation energy for Michaelis-Menten constant for O2 (J mol-1)
		aejm        @22 :Float64 = 37000;       # for corn | activation energy for electron transport (J mol-1) 
		aevc        @23 :Float64 = 58520;       # for corn | activation energy for photosynthesis (J mol-1)
		slaMin      @24 :Float64 = 20;          # for corn | specific leaf area under full light (m2 kg-1)
		
		scaleI      @25 :Float64 = 1.0;
		scaleM      @26 :Float64 = 1.0;

		mFol        @27 :Float64;
		# species and canopy layer specific foliage biomass(dry weight).
		# physiology  mFol_vtfl  mFol_vtfl  double  V : F  0.0  kg : m^-2
		# vc_OrganBiomass[LEAF]

		lai         @28 :Float64;
		# species specific leaf area index.
		# physiology  lai_vtfl  lai_vtfl  double  V : F  0.0  m ^ 2 : m^-2
		# CropGrowth::get_LeafAreaIndex()

		sla         @29 :Float64;
		# specific foliage area (m2 kgDW-1).
		# vegstructure  specific_foliage_area sla_vtfl  double  V : F  0.0  m ^ 2 : g : 10 ^ -3
		# specific leaf area (pc_SpecificLeafArea / cps.cultivarParams.pc_SpecificLeafArea) pc_SpecificLeafArea[vc_DevelopmentalStage]
    }

    struct CPData {
        kc      @0  :Float64; # Michaelis - Menten constant for CO2 reaction of rubisco per canopy layer(umol mol - 1 ubar - 1)
		ko      @1  :Float64; # Michaelis - Menten constant for O2 reaction of rubisco per canopy layer(umol mol - 1 ubar - 1)
		oi      @2  :Float64; # species and layer specific intercellular concentration of CO2 (umol mol-1)
		ci      @3  :Float64; # leaf internal O2 concentration per canopy layer(umol m - 2)
		comp    @4  :Float64; # CO2 compensation point at 25oC per canopy layer (umol m-2)
		vcMax   @5  :Float64; # actual activity state of rubisco  per canopy layer (umol m-2 s-1)
		jMax    @6  :Float64; #  actual electron transport capacity per canopy layer(umol m - 2 s - 1)
		jj      @7  :Float64; # umol m-2 s-1 ... electron provision (unit leaf area)
		jj1000  @8  :Float64; # umol m-2 s-1 ... electron provision (unit leaf area) under normalized conditions 
		jv      @9  :Float64; # umol m-2 s-1 ... used electron transport for photosynthesis (unit leaf area)
    }

    struct MicroClimateData
	{
		# common
        rad                     @0  :Float64;
		# radiation per canopy layer(W m - 2)
		# microclimate  rad_fl rad_fl  double  F  0.0  W:m^-2
		
        rad24                   @1  :Float64;
		# radiation regime over the last 24hours(W m - 2)
		# microclimate  rad24_fl rad24_fl  double  F  0.0  W:m^-2
		
        rad240                  @2  :Float64;
		# radiation regime over the last 10days (W m-2)
		# microclimate  rad240_fl rad240_fl  double  F  0.0  W:m^-2
		
        tFol                    @3  :Float64;
		# foliage temperature per canopy layer(oC)
		# microclimate  tFol_fl tFol_fl  double  F  0.0  oC
		
        tFol24                  @4  :Float64;
		# temperature regime over the last 24hours
		# microclimate  tFol24_fl tFol24_fl  double  F  0.0  oC
		
        tFol240                 @5  :Float64;
        # temperature regime over the last 10days
		# microclimate  tFol240_fl tFol240_fl  double  F  0.0  oC
		
		#jjv
		sunlitfoliagefraction   @6  :Float64;
		# fraction of sunlit foliage per canopy layer
		# microclimate  ts_sunlitfoliagefraction_fl ts_sunlitfoliagefraction_fl  double  F  0.0 ?
		
        sunlitfoliagefraction24 @7  :Float64;
        # fraction of sunlit foliage over the past 24 hours per canopy layer
		# microclimate  sunlitfoliagefraction24_fl  sunlitfoliagefraction24_fl  double  F  0.0 ?
		
		co2concentration        @8  :Float64;
	}

    struct PhotosynthT {
		par     @0 :Float64;    # photosynthetic active radiation (umol m-2 s-1)
		par24   @1 :Float64;    # 1 day aggregated photosynthetic active radiation (umol m-2 s-1)
		par240  @2 :Float64;    # 10 days aggregated photosynthetic active radiation (umol m-2 s-1)
	}

	struct FoliageT {
		tempK       @0 :Float64;    # foliage temperature within a canopy layer (K)
		tempK24     @1 :Float64;    # 1 day aggregated foliage temperature within a canopy layer (K)
		tempK240    @2 :Float64;    # 10 days aggregated foliage temperature within a canopy layer (K)
	}

	struct EnzymeActivityT {
		efIso   @0 :Float64;    # emission factor of isoprene(ug gDW-1 h-1)
		efMono  @1 :Float64;    # emission factor of monoterpenes (ug gDW-1 h-1)
	}

    struct LeafEmissionT {
		foliageLayer    @0 :UInt16;

		pho             @1 :PhotosynthT;
		fol             @2 :FoliageT;
		enzAct          @3 :EnzymeActivityT;
	}

	struct LeafEmissions {
		isoprene    @0 :Float64;    # isoprene emission (ug m-2ground h-1)
		monoterp    @1 :Float64;    # monoterpene emission (ug m-2ground h-1)
	}
}

struct SticsParameters {
    useN2O                  @0  :Bool;
    useNit                  @1  :Bool;
    useDenit                @2  :Bool;
    codeVnit                @3  :UInt8 = 1;
    codeTnit                @4  :UInt8 = 2;
    codeRationit            @5  :UInt8 = 2;
    codeHourlyWfpsNit       @6  :UInt8 = 2;
    codePdenit              @7  :UInt8 = 1;
    codeRatiodenit          @8  :UInt8 = 2;
    codeHourlyWfpsDenit     @9  :UInt8 = 2;
    hminn                   @10 :Float64 = 0.3;
    hoptn                   @11 :Float64 = 0.9;
    pHminnit                @12 :Float64 = 4.0; 
    pHmaxnit                @13 :Float64 = 7.2;
    nh4Min                  @14 :Float64 = 1.0;     # [mg NH4-N/kg soil]
    pHminden                @15 :Float64 = 7.2; 
    pHmaxden                @16 :Float64 = 9.2;
    wfpsc                   @17 :Float64 = 0.62; 
    tdenitoptGauss          @18 :Float64 = 47;      # [°C]
    scaleTdenitopt          @19 :Float64 = 25;      # [°C]
    kd                      @20 :Float64 = 148;     # [mg NO3-N/L]
    kDesat                  @21 :Float64 = 3.0;     # [1/day]
    fnx                     @22 :Float64 = 0.8;     # [1/day]
    vnitmax                 @23 :Float64 = 27.3;    # [mg NH4-N/kg soil/day]
    kamm                    @24 :Float64 = 24;      # [mg NH4-N/L]
    tnitmin                 @25 :Float64 = 5.0;     # [°C]
    tnitopt                 @26 :Float64 = 30.0;    # [°C]
    tnitop2                 @27 :Float64 = 35.0;    # [°C]
    tnitmax                 @28 :Float64 = 58.0;    # [°C]
    tnitoptGauss            @29 :Float64 = 32.5;    # [°C]
    scaleTnitopt            @30 :Float64 = 16.0;    # [°C]
    rationit                @31 :Float64 = 0.0016; 
    cminPdenit              @32 :Float64 = 1.0;     # [% [0-100]]
    cmaxPdenit              @33 :Float64 = 6.0;     # [% [0-100]]
    minPdenit               @34 :Float64 = 1.0;     # [mg N/Kg soil/day]
    maxPdenit               @35 :Float64 = 20.0;    # [mg N/kg soil/day]
    ratiodenit              @36 :Float64 = 0.2; 
    profdenit               @37 :Float64 = 20;      # [cm]
    vpotdenit               @38 :Float64 = 2.0;     # [kg N/ha/day]
}
