@0xc75fa80819dba94e;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::models::monica");

using Date = import "../date.capnp".Date;

struct MineralFertiliserParameters {
    id          @0 :Text;
	name        @1 :Text;
    carbamid  @2 :Float64;    # [%]
	nh4       @3 :Float64;    # [%]
	no3       @4 :Float64;    # [%]
}

struct YieldComponent {
    organId         @0 :Int64 = -1;
	yieldPercentage @1 :Float64;
	yieldDryMatter  @2 :Float64;
}

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

struct AOMProperties {
    aomSlow                 @0 :Float64;        # C content in slowly decomposing added organic matter pool [kgC m-3]
    aomFast                 @1 :Float64;        # C content in rapidly decomposing added organic matter pool [kgC m-3]

    aomSlowDecRatetoSMBSlow @2 :Float64;        # Rate for slow AOM consumed by SMB Slow is calculated.
    aomSlowDecRatetoSMBFast @3 :Float64;        # Rate for slow AOM consumed by SMB Fast is calculated.
    aomFastDecRatetoSMBSlow @4 :Float64;        # Rate for fast AOM consumed by SMB Slow is calculated.
    aomFastDecRatetoSMBFast @5 :Float64;        # Rate for fast AOM consumed by SMB Fast is calculated.

    aomSlowDecCoeff         @6 :Float64;        # Is dependent on environment
    aomFastDecCoeff         @7 :Float64;        # Is dependent on environment

    aomSlowDecCoeffStandard @8 :Float64 = 1.0;  # Decomposition rate coefficient for slow AOM pool at standard conditions
    aomFastDecCoeffStandard @9 :Float64 = 1.0;  # Decomposition rate coefficient for fast AOM pool at standard conditions

    partAOMSlowtoSMBSlow    @10 :Float64;       # Partial transformation from AOM to SMB (soil microbiological biomass) for slow AOMs.
    partAOMSlowtoSMBFast    @11 :Float64;       # Partial transformation from AOM to SMB (soil microbiological biomass) for fast AOMs.

    cnRatioAOMSlow          @12 :Float64 = 1.0; # Used for calculation N-value if only C-value is known. Usually a constant value.
    cnRatioAOMFast          @13 :Float64 = 1.0; # C-N-Ratio is dependent on the nutritional condition of the plant.

    daysAfterApplication    @14 :UInt16 = 0;    # Fertilization parameter
    aomDryMatterContent     @15 :Float64;       # Fertilization parameter
    aomNH4Content           @16 :Float64;       # Fertilization parameter

    aomSlowDelta            @17 :Float64;       # Difference of AOM slow between to timesteps
    aomFastDelta            @18 :Float64;       # Difference of AOM fast between to timesteps

    incorporation           @19 :Bool = false;  # true if organic fertilizer is added with a subsequent incorporation.
	volatilization          @20 :Bool = true;   # true means it's a crop residue and won't participate in vo_volatilisation()
}

struct SoilLayerState {
    layerThickness      @0  :Float64 = 0.1;         # Soil layer's vertical extension [m]
    soilWaterFlux       @1  :Float64;               # Water flux at the upper boundary of the soil layer [l m-2]

    voAOMPool           @2  :List(AOMProperties);   # List of different added organic matter pools in soil layer

    somSlow             @3  :Float64;               # C content of soil organic matter slow pool [kg C m-3]
    somFast             @4  :Float64;               # content of soil organic matter fast pool size [kg C m-3]
    smbSlow             @5  :Float64;               # C content of soil microbial biomass slow pool size [kg C m-3]
    smbFast             @6  :Float64;               # C content of soil microbial biomass fast pool size [kg C m-3]

    # anorganische Stickstoff-Formen
    soilCarbamid        @7  :Float64;               # Soil layer's carbamide-N content [kg Carbamide-N m-3]
    soilNH4             @8  :Float64 = 0.0001;      # Soil layer's NH4-N content [kg NH4-N m-3]
    soilNO2             @9  :Float64 = 0.001;       # Soil layer's NO2-N content [kg NO2-N m-3]
    soilNO3             @10 :Float64 = 0.0001;      # Soil layer's NO3-N content [kg NO3-N m-3]
    soilFrozen          @11 :Bool = false;

    sps                 @12 :SoilParameters;
    soilMoistureM3      @13 :Float64 = 0.25;        # Soil layer's moisture content [m3 m-3]
    soilTemperature     @14 :Float64;               # Soil layer's temperature [°C]
}

struct OrganicMatterParameters {
    aomDryMatterContent         @0  :Float64; # Dry matter content of added organic matter [kg DM kg FM-1]
    aomNH4Content               @1  :Float64; # Ammonium content in added organic matter [kg N kg DM-1]
    aomNO3Content               @2  :Float64; # Nitrate content in added organic matter [kg N kg DM-1]
    aomCarbamidContent          @3  :Float64; # Carbamide content in added organic matter [kg N kg DM-1]

    aomSlowDecCoeffStandard     @4  :Float64; # Decomposition rate coefficient of slow AOM at standard conditions [d-1]
    aomFastDecCoeffStandard     @5  :Float64; # Decomposition rate coefficient of fast AOM at standard conditions [d-1]

    partAOMToAOMSlow            @6  :Float64; # Part of AOM that is assigned to the slowly decomposing pool [kg kg-1]
    partAOMToAOMFast            @7  :Float64; # Part of AOM that is assigned to the rapidly decomposing pool [kg kg-1]

    cnRatioAOMSlow              @8  :Float64; # C to N ratio of the slowly decomposing AOM pool []
    cnRatioAOMFast              @9  :Float64; # C to N ratio of the rapidly decomposing AOM pool []

    partAOMSlowToSMBSlow        @10 :Float64; # Part of AOM slow consumed by slow soil microbial biomass [kg kg-1]
    partAOMSlowToSMBFast        @11 :Float64; # Part of AOM slow consumed by fast soil microbial biomass [kg kg-1]

    nConcentration              @12 :Float64;
}

struct CropResidueParameters {
    omps        @0 :OrganicMatterParameters;
    species     @1 :Text;
    residueType @2 :Text;
}

struct AutomaticHarvestParameters {
    enum HarvestTime {
        maturity    @0;
        unknown     @1;
    }
    harvestTime         @0 :HarvestTime = unknown; # Harvest time parameter
    latestHarvestDOY    @1 :Int16 = -1;
}


struct Crop {
    struct MaybeBool {
        value @0 :Bool;
    }

    dbId @0 :Int64 = -1;
    speciesName @1 :Text;
    cultivarName @2 :Text;
	seedDate @3 :Date;
	harvestDate @4 :Date;
    isWinterCrop @5 :MaybeBool;
	isPerennialCrop @6 :MaybeBool;
	cuttingDates @7 :List(Date);
    cropParams @8 :CropParameters;
    perennialCropParams @9 :CropParameters;
    residueParams @10 :CropResidueParameters;

    crossCropAdaptionFactor @11 :Float64 = 1.0;

    automaticHarvest @12 :Bool;
	automaticHarvestParams @13 :AutomaticHarvestParameters;
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
}


struct CropModuleState {
    struct ModuleParameters {
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
    }

    frostKillOn                                 @0      :Bool;      
    soilColumn                                  @1      :SoilColumnState;
    perennialCropParams                         @2      :CropParameters;
    cropPs                                      @3      :ModuleParameters;
    speciesPs                                   @4      :SpeciesParameters;
    cultivarPs                                  @5      :CultivarParameters;      
    vsLatitude                                  @6      :Float64;  
    abovegroundBiomass                          @7      :Float64; # old OBMAS
    abovegroundBiomassOld                       @8      :Float64; # old OBALT
    abovegroundOrgan                            @9      :List(Bool); # old KOMP
    actualTranspiration                         @10     :Float64; 
    assimilatePartitioningCoeff                 @11     :List(List(Float64)); # old PRO
    assimilateReallocation                      @12     :Float64; 
    assimilates                                 @13     :Float64; 
    assimilationRate                            @14     :Float64; # old AMAX
    astronomicDayLenght                         @15     :Float64; # old DL
    baseDaylength                               @16     :List(Float64); # old DLBAS
    baseTemperature                             @17     :List(Float64); # old BAS
    beginSensitivePhaseHeatStress               @18     :Float64;
    belowgroundBiomass                          @19     :Float64; 
    belowgroundBiomassOld                       @20     :Float64;
    carboxylationPathway                        @21     :Int64; # old TEMPTYP
    clearDayRadiation                           @22     :Float64; # old DRC
    co2Method                                   @23     :UInt8 = 3;
    criticalNConcentration                      @24     :Float64; # old GEHMIN
    criticalOxygenContent                       @25     :List(Float64); # old LUKRIT
    criticalTemperatureHeatStress               @26     :Float64; 
    cropDiameter                                @27     :Float64;
    cropFrostRedux                              @28     :Float64 = 1.0;
    cropHeatRedux                               @29     :Float64 = 1.0;
    cropHeight                                  @30     :Float64;
    cropHeightP1                                @31     :Float64;
    cropHeightP2                                @32     :Float64;
    cropName                                    @33     :Text; # old FRUCHT$(AKF)
    cropNDemand                                 @34     :Float64; # old DTGESN
    cropNRedux                                  @35     :Float64 = 1.0; # old REDUK
    cropSpecificMaxRootingDepth                 @36     :Float64; # old WUMAXPF [m]
    cropWaterUptake                             @37     :List(Float64); # old TP
    currentTemperatureSum                       @38     :List(Float64); # old SUM
    currentTotalTemperatureSum                  @39     :Float64; # old FP
    currentTotalTemperatureSumRoot              @40     :Float64;
    pcCuttingDelayDays                          @41     :UInt16;
    daylengthFactor                             @42     :Float64; # old DAYL
    daylengthRequirement                        @43     :List(Float64); # old DEC
    daysAfterBeginFlowering                     @44     :UInt16;
    declination                                 @45     :Float64; # old EFF0
    defaultRadiationUseEfficiency               @46     :Float64;
    vmDepthGroundwaterTable                     @47     :UInt16; # old GRW
    developmentAccelerationByNitrogenStress     @48     :UInt64;
    developmentalStage                          @49     :UInt16; # old INTWICK
    noOfCropSteps                               @50     :UInt16; 
    droughtImpactOnFertility                    @51     :Float64 = 1.0;
    droughtImpactOnFertilityFactor              @52     :Float64;
    droughtStressThreshold                      @53     :List(Float64); # old DRYswell
    emergenceFloodingControlOn                  @54     :Bool = false;
    emergenceMoistureControlOn                  @55     :Bool = false;
    endSensitivePhaseHeatStress                 @56     :Float64;
    effectiveDayLength                          @57     :Float64; # old DLE
    errorStatus                                 @58     :Bool = false;
    errorMessage                                @59     :Text;
    evaporatedFromIntercept                     @60     :Float64;
    extraterrestrialRadiation                   @61     :Float64;
    fieldConditionModifier                      @62     :Float64;
    finalDevelopmentalStage                     @63     :UInt16;
    fixedN                                      @64     :Float64;
    #std::vector<double> vo_FreshSoilOrganicMatter @16 :List(Float64); # old NFOS
    frostDehardening                            @65     :Float64;
    frostHardening                              @66     :Float64;
    globalRadiation                             @67     :Float64;
    greenAreaIndex                              @68     :Float64;
    grossAssimilates                            @69     :Float64;
    grossPhotosynthesis                         @70     :Float64; # old GPHOT
    grossPhotosynthesisMol                      @71     :Float64;
    grossPhotosynthesisReferenceMol             @72     :Float64;
    grossPrimaryProduction                      @73     :Float64;
    growthCycleEnded                            @74     :Bool = false;
    growthRespirationAS                         @75     :Float64 = 0.0;
    heatSumIrrigationStart                      @76     :Float64;
    heatSumIrrigationEnd                        @77     :Float64;
    vsHeightNN                                  @78     :Float64;
    initialKcFactor                             @79     :Float64; # old Kcini
    initialOrganBiomass                         @80     :List(Float64);
    initialRootingDepth                         @81     :Float64;
    interceptionStorage                         @82     :Float64; 
    kcFactor                                    @83     :Float64 = 0.6; # old FKc
    leafAreaIndex                               @84     :Float64; # old LAI
    sunlitLeafAreaIndex                         @85     :List(Float64);
    shadedLeafAreaIndex                         @86     :List(Float64);
    lowTemperatureExposure                      @87     :Float64;
    limitingTemperatureHeatStress               @88     :Float64;
    lt50                                        @89     :Float64 = -3.0;
    lt50cultivar                                @90     :Float64;
    luxuryNCoeff                                @91     :Float64;
    maintenanceRespirationAS                    @92     :Float64;
    maxAssimilationRate                         @93     :Float64; # old MAXAMAX
    maxCropDiameter                             @94     :Float64;
    maxCropHeight                               @95     :Float64;
    maxNUptake                                  @96     :Float64; # old MAXUP
    maxNUptakeParam                             @97     :Float64;
    maxRootingDepth                             @98     :Float64; # old WURM
    minimumNConcentration                       @99     :Float64;
    minimumTemperatureForAssimilation           @100    :Float64; # old MINTMP
    optimumTemperatureForAssimilation           @101    :Float64;
    maximumTemperatureForAssimilation           @102    :Float64;
    minimumTemperatureRootGrowth                @103    :Float64;
    netMaintenanceRespiration                   @104    :Float64; # old MAINT
    netPhotosynthesis                           @105    :Float64; # old GTW
    netPrecipitation                            @106    :Float64;
    netPrimaryProduction                        @107    :Float64;
    pcNConcentrationAbovegroundBiomass          @108    :Float64; # initial value of old GEHOB
    nConcentrationAbovegroundBiomass            @109    :Float64; # old GEHOB
    nConcentrationAbovegroundBiomassOld         @110    :Float64; # old GEHALT
    nConcentrationB0                            @111    :Float64;
    nContentDeficit                             @112    :Float64;
    nConcentrationPN                            @113    :Float64; 
    pcNConcentrationRoot                        @114    :Float64; # initial value to WUGEH
    nConcentrationRoot                          @115    :Float64; # old WUGEH
    nConcentrationRootOld                       @116    :Float64; # old
    nitrogenResponseOn                          @117    :Bool; 
    numberOfDevelopmentalStages                 @118    :Float64;
    numberOfOrgans                              @119    :Float64; # old NRKOM
    nUptakeFromLayer                            @120    :List(Float64); # old PE
    optimumTemperature                          @121    :List(Float64);
    organBiomass                                @122    :List(Float64); # old WORG
    organDeadBiomass                            @123    :List(Float64); # old WDORG
    organGreenBiomass                           @124    :List(Float64);
    organGrowthIncrement                        @125    :List(Float64); # old GORG
    organGrowthRespiration                      @126    :List(Float64); # old MAIRT
    organIdsForPrimaryYield                     @127    :List(YieldComponent);
    organIdsForSecondaryYield                   @128    :List(YieldComponent);
    organIdsForCutting                          @129    :List(YieldComponent);
    organMaintenanceRespiration                 @130    :List(Float64); # old MAIRT
    organSenescenceIncremen                     @131    :List(Float64); # old DGORG
    organSenescenceRate                         @132    :List(List(Float64)); # old DEAD
    overcastDayRadiation                        @133    :Float64; # old DRO
    oxygenDeficit                               @134    :Float64; # old LURED
    partBiologicalNFixation                     @135    :Float64;
    perennial                                   @136    :Bool;
    photoperiodicDaylength                      @137    :Float64; # old DLP
    photActRadiationMean                        @138    :Float64; # old RDN
    plantDensity                                @139    :Float64; 
    potentialTranspiration                      @140    :Float64; 
    referenceEvapotranspiration                 @141    :Float64; 
    relativeTotalDevelopment                    @142    :Float64; 
    remainingEvapotranspiration                 @143    :Float64; 
    reserveAssimilatePool                       @144    :Float64; # old ASPOO
    residueNRatio                               @145    :Float64; 
    respiratoryStress                           @146    :Float64; 
    rootBiomass                                 @147    :Float64; # old WUMAS
    rootBiomassOld                              @148    :Float64; # old WUMALT
    rootDensity                                 @149    :List(Float64); # old WUDICH
    rootDiameter                                @150    :List(Float64); # old WRAD
    rootDistributionParam                       @151    :Float64;
    rootEffectivity                             @152    :List(Float64); # old WUEFF
    rootFormFactor                              @153    :Float64;
    rootGrowthLag                               @154    :Float64;
    rootingDepth                                @155    :UInt16; # old WURZ
    rootingDepthM                               @156    :Float64;
    rootingZone                                 @157    :UInt16;
    rootPenetrationRate                         @158    :Float64;
    vmSaturationDeficit                         @159    :Float64;
    soilCoverage                                @160    :Float64;
    vsSoilMineralNContent                       @161    :List(Float64); # old C1
    soilSpecificMaxRootingDepth                 @162    :Float64; # old WURZMAX [m]
    vsSoilSpecificMaxRootingDepth               @163    :Float64;
    specificLeafArea                            @164    :List(Float64); # old LAIFKT [ha kg-1]
    specificRootLength                          @165    :Float64;
    stageAfterCut                               @166    :UInt16; # //0-indexed
    stageAtMaxDiameter                          @167    :Float64;
    stageAtMaxHeight                            @168    :Float64;
    stageMaxRootNConcentration                  @169    :List(Float64); # old WGMAX
    stageKcFactor                               @170    :List(Float64); # old Kc
    stageTemperatureSum                         @171    :List(Float64); # old TSUM
    stomataResistance                           @172    :Float64; # old RSTOM
    pcStorageOrgan                              @173    :List(Bool);  
    storageOrgan                                @174    :UInt16 = 4;
    targetNConcentration                        @175    :Float64; # old GEHMAX
    timeStep                                    @176    :Float64 = 1.0; # old dt
    timeUnderAnoxia                             @177    :UInt64; 
    vsTortuosity                                @178    :Float64; # old AD
    totalBiomass                                @179    :Float64;
    totalBiomassNContent                        @180    :Float64; # old PESUM
    totalCropHeatImpact                         @181    :Float64;
    totalNInput                                 @182    :Float64;
    totalNUptake                                @183    :Float64; # old SUMPE
    totalRespired                               @184    :Float64;
    respiration                                 @185    :Float64;
    sumTotalNUptake                             @186    :Float64; # summation of all calculated NUptake; needed for sensitivity analysis
    totalRootLength                             @187    :Float64; # old WULAEN
    totalTemperatureSum                         @188    :Float64;
    temperatureSumToFlowering                   @189    :Float64;
    transpiration                               @190    :List(Float64); # old TP
    transpirationRedux                          @191    :List(Float64); # old TRRED
    transpirationDeficit                        @192    :Float64 = 1.0; # old TRREL
    vernalisationDays                           @193    :Float64;
    vernalisationFactor                         @194    :Float64; # old FV
    vernalisationRequirement                    @195    :List(Float64); # old VSCHWELL
    waterDeficitResponseOn                      @196    :Bool;

    eva2usage                                   @197    :UInt8;
    eva2primaryYieldComponents                  @198    :List(YieldComponent);
    eva2secondaryYieldComponents                @199    :List(YieldComponent);   
    dyingOut                                    @200    :Bool;
    accumulatedETa                              @201    :Float64;
    accumulatedTranspiration                    @202    :Float64;
    accumulatedPrimaryCropYield                 @203    :Float64;
    sumExportedCutBiomass                       @204    :Float64;
    exportedCutBiomass                          @205    :Float64;
    sumResidueCutBiomass                        @206    :Float64;
    residueCutBiomass                           @207    :Float64;

    cuttingDelayDays                            @208    :UInt16;
    vsMaxEffectiveRootingDepth                  @209    :Float64;
    vsImpenetrableLayerDept                     @210    :Float64; # h

    anthesisDay                                 @211    :Int16 = -1;
    maturityDay                                 @212    :Int16 = -1;

    maturityReached                             @213    :Bool;

    # VOC members   
    stepSize24                                  @214    :UInt16 = 24;
    stepSize240                                 @215    :UInt16 = 240;
    rad24                                       @216    :List(Float64); 
    rad240                                      @217    :Float64; 
    tfol24                                      @218    :Float64; 
    tfol240                                     @219    :Float64;
    index24                                     @220    :UInt16; 
    index240                                    @221    :UInt16;
    full24                                      @222    :Bool; 
    full240                                     @223    :Bool;

    guentherEmissions                           @224    :Voc.Emissions;
    jjvEmissions                                @225    :Voc.Emissions;
    vocSpecies                                  @226    :Voc.SpeciesData;
    cropPhotosynthesisResults                   @227    :Voc.CPData;

    #std::function<void(std::string)> _fireEvent;   
    #std::function<void(std::map<size_t, double>, double)> _addOrganicMatter;

    o3ShortTermDamage                           @228    :Float64 = 1.0;
    o3LongTermDamage                            @229    :Float64 = 1.0;
    o3Senescence                                @230    :Float64 = 1.0;
    o3SumUptake                                 @231    :Float64;
    o3WStomatalClosure                          @232    :Float64 = 1.0;

    assimilatePartCoeffsReduced                 @233    :Bool;
    ktkc                                        @234    :Float64; # old KTkc
    ktko                                        @235    :Float64; # old KTko
}

struct Voc {

    const kilo          :Float64 = 1000.0;
    const milli         :Float64 = 0.001;

	# conv constants
	const nmol2umol     :Float64 = Voc.kilo;               # nmol to umol
	const umol2nmol     :Float64 = 0.001;               # (= 1.0 / NMOL2UMOL) umol to nmol
	const mol2mmol      :Float64 = Voc.milli;
	const mmol2mol      :Float64 = 1000.0;              # (= 1.0 / MOL2MMOL)
	const fPar          :Float64 = 0.45;                # conversion factor for global into PAR (Monteith 1965, Meek et al. 1984)
	const d2k           :Float64 = 273.15;              # kelvin at zero degree celsius
	const g2kg          :Float64 = 1000.0;              # 0.001 kg per g
	const umol2w        :Float64 = 4.57;                # conversion factor from Watt in umol PAR (Cox et al. 1998)
	const w2umol        :Float64 = 0.2188183807439825;  # (= 1.0 / UMOL2W) conversion factor from umol PAR in Watt (Cox et al. 1998)
	const ng2ug         :Float64 = 1000.0;              # conversion factor from nano to micro (gramm)
	const ug2ng         :Float64 = 0.001;                # (= 1.0 / NG2UG) 

	# phys constants
	const rGas          :Float64 = 8.3143;              # general gas constant  [J mol-1 K-1]

	# chem constants
	const mc            :Float64 = 12.0;                # molecular weight of carbon  [g mol-1]
	const cIso          :Float64 = 5.0;                 # number of carbons in Isoprene (C5H8)
	const cMono         :Float64 = 10.0;                # number of carbons in Monoterpene (C10H16)

	# time constants
	const secInMin      :UInt8 = 60;                    # minute to seconds
	const minInHr       :UInt8 = 60;                    # hour to minutes
	const hrInDay       :UInt8 = 24;                    # day to hours
	const monthsInYear  :UInt8 = 12;                    # year to months
	const secInHr       :UInt16 = 3600;                 # hour to seconds
	const minInDay      :UInt16 = 1440;                 # day to minutes
	const secInDay      :UInt32 = 86400;                # day to seconds

	# meteo constants
	const po2           :Float64 = 0.208;               # volumentric percentage of oxygen in the canopy air

	# voc module specific constants
	const abso          :Float64 = 0.860;               # absorbance factor, Collatz et al. 1991 
	const alpha         :Float64 = 0.0027;              # light modifier, Guenther et al. 1993; (Staudt et al. 2004 f. Q.ilex: 0.0041, Bertin et al. 1997 f. Q.ilex: 0.00147, Harley et al. 2004 f. M.indica: 0.00145) 
	const beta          :Float64 = 0.09;                # monoterpene scaling factor, Guenther et al. 1995 (cit. in Guenther 1999 says this value originates from Guenther 1993) 
	const c1            :Float64 = 0.17650;             #fw: 0.17650e-3 in Grote et al. 2014 to preserve the constant relation to C2; #  fraction of electrons used from excess electron transport (−), from Sun et al. 2012 dataset, Grote et al. 2014 
	const c2            :Float64 = 0.00280;             #fw: 0.00280e-3 (umol m-2 s-1) in Grote et al. 2014; here in nmol m-2 s-1; # fraction of electrons used from photosynthetic electron transport (−),from Sun et al. 2012 dataset, Grote et al. 2014 
	const ceoIso        :Float64 = 2.0;                 # emission-class dependent empirical coefficient for temperature acitivity factor of isoprene from MEGAN v2.1 (Guenther et al. 2012)
	const ceoMono       :Float64 = 1.83;                # emission-class dependent empirical coefficient for temperature acitivity factor of a-pinene, b-pinene, oMT, Limonen, etc from MEGAN v2.1 (Guenther et al. 2012)
	const ct1           :Float64 = 95000.0;             # first temperature modifier (J mol-1), Guenther et al. 1993; (Staudt et al. 2004 f. Q.ilex: 87620, Bertin et al. 1997 f. Q.ilex: 74050, Harley et al. f. M.indica: 124600, in WIMOVAC 95100) 
	const ct2           :Float64 = 230000.0;            # second temperature modifier (J mol-1), Guenther et al. 1993; (Staudt et al. 2004 f. Q.ilex: 188200, Bertin et al. 1997 f. Q.ilex: 638600, Harley et al. f. M.indica: 254800) 
	const cl1           :Float64 = 1.066;               # radiation modifier, Guenther et al. 1993; (Staudt et al. 2004 f. Q.ilex: 1.04, Bertin et al. 1997 f. Q.ilex: 1.21, Harley et al. 2004 f. M.indica: 1.218) 
	const gammaMax      :Float64 = 34.0;                # saturating amount of electrons that can be supplied from other sources (μmol m−2 s−1), Sun et al. 2012 dataset, Grote et al. 2014 (delta J_sat in paper)
	const ppfd0         :Float64 = 1000.0;              # reference photosynthetically active quantum flux density/light density (standard conditions) (umol m-2 s-1), Guenther et al. 1993 
	const temp0         :Float64 = 298.15;              # (= 25.0 + D2K) reference (leaf) temperature (standard conditions) (K), Guenther et al. 1993 
	const topT          :Float64 = 314.0;               # temperature with maximum emission (K), Guenther et al. 1993; (Staudt et al. 2004 f. Q.ilex: 317, Bertin et al. 1997 f. Q.ilex: 311.6, Harley et al. f. M.indica: 313.4, in WIMOVAC 311.83) 
	const tRef          :Float64 = 303.15;              # (= 30.0 + D2K) reference temperature (K), Guenther et al. 1993 
		
	# photofarquhar specific constants
	const tk25 :Float64 = 298.16;

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


struct SoilColumnState {
    vsSurfaceWaterStorage       @0  :Float64;       # Content of above-ground water storage [mm]
    vsInterceptionStorage       @1  :Float64;       # Amount of intercepted water on crop surface [mm]
    vmGroundwaterTable          @2  :UInt16;        # Layer of current groundwater table
    vsFluxAtLowerBoundary       @3  :Float64;       # Water flux out of bottom layer
    vqCropNUptake               @4  :Float64;       # Daily amount of N taken up by the crop [kg m-2]
    vtSoilSurfaceTemperature    @5  :Float64;
    vmSnowDepth                 @6  :Float64;
    psMaxMineralisationDepth    @7  :Float64 = 0.4;
    vsNumberOfOrganicLayers     @8  :Float64;       # Number of organic layers.
    vfTopDressing               @9  :Float64;
    vfTopDressingPartition      @10 :MineralFertiliserParameters;
    vfTopDressingDelay          @11 :UInt16;
    cropModule                  @12 :CropModuleState;
    #std::list<std::function<double()>> _delayedNMinApplications;
    pmCriticalMoistureDepth     @13  :Float64;
}


struct SoilTemperatureModuleState {
    struct ModuleParameters {
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

    soilSurfaceTemperature      @0  :Float64;
    soilColumn                  @1  :SoilColumnState;
    monica                      @2  :MonicaModelState;
    soilColumnVtGroundLayer     @3  :SoilLayerState;
    soilColumnVtBottomLayer     @4  :SoilLayerState;
    numberOfLayers              @5  :UInt16;
    vsNumberOfLayers            @6  :UInt16;
    vsSoilMoistureConst         @7  :List(Float64);
    soilTemperature             @8  :List(Float64);
    v                           @9  :List(Float64);
    volumeMatrix                @10 :List(Float64);
    volumeMatrixOld             @11 :List(Float64);
    b                           @12 :List(Float64);
    matrixPrimaryDiagonal       @13 :List(Float64);
    matrixSecundaryDiagonal     @14 :List(Float64);
    heatFlow                    @15 :Float64;
    heatConductivity            @16 :List(Float64);
    heatConductivityMean        @17 :List(Float64);
    heatCapacity                @18 :List(Float64);
    dampingFactor               @19 :Float64 = 0.8;
}

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

struct Stics {
    struct Parameters {
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
}

struct SoilOrganicModuleState {
    struct ModuleParameters {
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
		cmRatioSMB                          @17 :Float64 = 6.70;        # 6.70 [], from DAISY manual
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

        sticsParams                         @36 :Stics.Parameters;
    }

    soilColumn                          @0  :SoilColumnState;
    siteParams                          @1  :SiteParameters;
    organicPs                           @2  :ModuleParameters;

    vsNumberOfLayers                    @3  :UInt16;
    vsNumberOfOrganicLayers             @4  :UInt16;
    addedOrganicMatter                  @5  :Bool;
    irrigationAmount                    @6  :Float64;
    actAmmoniaOxidationRate             @7  :List(Float64); # [kg N m-3 d-1]
    actNitrificationRate                @8  :List(Float64); # [kg N m-3 d-1]
    actDenitrificationRate              @9  :List(Float64);  # [kg N m-3 d-1]
    aomFastDeltaSum                     @10 :List(Float64);
    aomFastInput                        @11 :List(Float64); # AOMfast pool change by direct input [kg C m-3]
    aomFastSum                          @12 :List(Float64);
    aomSlowDeltaSum                     @13 :List(Float64);
    aomSlowInput                        @14 :List(Float64); # AOMslow pool change by direct input [kg C m-3]
    aomSlowSum                          @15 :List(Float64);
    cBalance                            @16 :List(Float64);
    decomposerRespiration               @17 :Float64;
    errorMessage                        @18 :Text;
    inertSoilOrganicC                   @19 :List(Float64);
    n2oProduced                         @20 :Float64; # [kg-N2O-N/ha]
    n2oProducedNit                      @21 :Float64; # [kg-N2O-N/ha]
    n2oProducedDenit                    @22 :Float64; # [kg-N2O-N/ha]
    netEcosystemExchange                @23 :Float64;
    netEcosystemProduction              @24 :Float64;
    netNMineralisation                  @25 :Float64;
    netNMineralisationRate              @26 :List(Float64);
    totalNH3Volatilised                 @27 :Float64;
    nh3Volatilised                      @28 :Float64;
    smbCO2EvolutionRate                 @29 :List(Float64);
    smbFastDelta                        @30 :List(Float64);
    smbSlowDelta                        @31 :List(Float64);
    vsSoilMineralNContent               @32 :List(Float64);
    soilOrganicC                        @33 :List(Float64);
    somFastDelta                        @34 :List(Float64);
    somFastInput                        @35 :List(Float64); # SOMfast pool change by direct input [kg C m-3]
    somSlowDelta                        @36 :List(Float64);
    sumDenitrification                  @37 :Float64;       # kg-N/m2
    sumNetNMineralisation               @38 :Float64;
    sumN2OProduced                      @39 :Float64;
    sumNH3Volatilised                   @40 :Float64;
    totalDenitrification                @41 :Float64;

    incorporation                       @42 :Bool;
    cropModule                          @43 :CropModuleState;
}

struct EnvironmentParameters {
    struct YearToValue {
        year @0 :UInt16;
        value @1 :Float64;
    }

    albedo                      @0  :Float64 = 0.23;
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

struct SoilMoistureModuleState {
    struct ModuleParameters {
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

    struct SnowComponentState {
        soilColumn                              @0  :SoilColumnState;

        snowDensity                             @1  :Float64;   # Snow density [kg dm-3]
        snowDepth                               @2  :Float64;   # Snow depth [mm]
        frozenWaterInSnow                       @3  :Float64;   # [mm]
        liquidWaterInSnow                       @4  :Float64;   # [mm]
        waterToInfiltrate                       @5  :Float64;   # [mm]
        maxSnowDepth                            @6  :Float64;   # [mm]
        accumulatedSnowDepth                    @7  :Float64;   # [mm]
        
        # extern or user defined snow parameter
        snowmeltTemperature                     @8  :Float64;   # Base temperature for snowmelt [°C]
        snowAccumulationThresholdTemperature    @9  :Float64;
        temperatureLimitForLiquidWater          @10 :Float64;   # Lower temperature limit of liquid water in snow
        correctionRain                          @11 :Float64;   # Correction factor for rain (no correction used here)
        correctionSnow                          @12 :Float64;   # Correction factor for snow (value used in COUP by Lars Egil H.)
        refreezeTemperature                     @13 :Float64;   # Base temperature for refreeze [°C]
        refreezeP1                              @14 :Float64;   # Refreeze parameter (Karvonen's value)
        refreezeP2                              @15 :Float64;   # Refreeze exponent (Karvonen's value)
        newSnowDensityMin                       @16 :Float64;   # Minimum density of new snow
        snowMaxAdditionalDensity                @17 :Float64;   # Maximum additional density of snow (max rho = 0.35, Karvonen)
        snowPacking                             @18 :Float64;   # Snow packing factor (calibrated by Helge Bonesmo)
        snowRetentionCapacityMin                @19 :Float64;   # Minimum liquid water retention capacity in snow [mm]
        snowRetentionCapacityMax                @20 :Float64;  
    }

    struct FrostComponentState {
        soilColumn                      @0  :SoilColumnState;
        frostDepth                      @1  :Float64;
        accumulatedFrostDepth           @2  :Float64;
        negativeDegreeDays              @3  :Float64;       # Counts negative degree-days under snow
        thawDepth                       @4  :Float64;
        frostDays                       @5  :UInt16;
        lambdaRedux                     @6  :List(Float64); # Reduction factor for Lambda []
        temperatureUnderSnow            @7  :Float64;

        # user defined or data base parameter
        hydraulicConductivityRedux      @8  :Float64;
        ptTimeStep                      @9  :Float64;

        pmHydraulicConductivityRedux    @10 :Float64;
    }

    soilColumn                      @0  :SoilColumnState;
    siteParameters                  @1  :SiteParameters;
    monica                          @2  :MonicaModelState;
    smPs                            @3  :ModuleParameters;
    envPs                           @4  :EnvironmentParameters;
    cropPs                          @5  :CropModuleState.ModuleParameters;
	
    numberOfLayers                  @6  :UInt16;
	vsNumberOfLayers                @7  :UInt16;

    actualEvaporation               @8  :Float64;               # Sum of evaporation of all layers [mm]
    actualEvapotranspiration        @9  :Float64;               # Sum of evaporation and transpiration of all layers [mm]
    actualTranspiration             @10 :Float64;               # Sum of transpiration of all layers [mm]
    availableWater                  @11 :List(Float64);         # Soil available water in [mm]
    capillaryRise                   @12 :Float64;               # Capillary rise [mm]
    capillaryRiseRate               @13 :List(Float64);         # Capillary rise rate from database in dependence of groundwater distance and texture [m d-1]
    capillaryWater                  @14 :List(Float64);         # soil capillary water in [mm]
    capillaryWater70                @15 :List(Float64);         # 70% of soil capillary water in [mm]
    evaporation                     @16 :List(Float64);         # Evaporation of layer [mm]
    evapotranspiration              @17 :List(Float64);         # Evapotranspiration of layer [mm]
    fieldCapacity                   @18 :List(Float64);         # Soil water content at Field Capacity
    fluxAtLowerBoundary             @19 :Float64;               # Water flux out of bottom layer [mm]

    gravitationalWater              @20 :List(Float64);         # Soil water content drained by gravitation only [mm]
    grossPrecipitation              @21 :Float64;               # Precipitation amount that falls on soil and vegetation [mm]
    groundwaterAdded                @22 :Float64;
    groundwaterDischarge            @23 :Float64;
    groundwaterTable                @24 :UInt16;                # Layer of groundwater table []
    heatConductivity                @25 :List(Float64);         # Heat conductivity of layer [J m-1 d-1]
    hydraulicConductivityRedux      @26 :Float64;               # Reduction factor for hydraulic conductivity [0.1 - 9.9]
    infiltration                    @27 :Float64;               # Amount of water that infiltrates into top soil layer [mm]
    interception                    @28 :Float64;               # [mm], water that is intercepted by the crop and evaporates from it's surface; not accountable for soil water budget
    vcKcFactor                      @29 :Float64 = 0.6;
    lambda                          @30 :List(Float64);         # Empirical soil water conductivity parameter []
    lambdaReduced                   @31 :Float64;
    vsLatitude                      @32 :Float64;
    layerThickness                  @33 :List(Float64);
    pmLayerThickness                @34 :Float64;
    pmLeachingDepth                 @35 :Float64;
    pmLeachingDepthLayer            @36 :UInt16;
    vwMaxAirTemperature             @37 :Float64;               # [°C]
    pmMaxPercolationRate            @38 :Float64;               # [mm d-1]
    vwMeanAirTemperature            @39 :Float64;               # [°C]
    vwMinAirTemperature             @40 :Float64;               # [°C]
    vcNetPrecipitation              @41 :Float64;               # Precipitation amount that is not intercepted by vegetation [mm]
    vwNetRadiation                  @42 :Float64;               # [MJ m-2]
    permanentWiltingPoint           @43 :List(Float64);         # Soil water content at permanent wilting point [m3 m-3]
    vcPercentageSoilCoverage        @44 :Float64;               # [m2 m-2]
    percolationRate                 @45 :List(Float64);         # Percolation rate per layer [mm d-1]
    vwPrecipitation                 @46 :Float64;               # Precipition taken from weather data [mm]
    referenceEvapotranspiration     @47 :Float64 = 6.0;         # Evapotranspiration of a 12mm cut grass crop at sufficient water supply [mm]
    relativeHumidity                @48 :Float64;               # [m3 m-3]
    residualEvapotranspiration      @49 :List(Float64);         # Residual evapotranspiration in [mm]
    saturatedHydraulicConductivity  @50 :List(Float64);         # Saturated hydraulic conductivity [mm d-1]

    soilMoisture                    @51 :List(Float64);         # Result - Soil moisture of layer [m3 m-3]
    soilMoisturecrit                @52 :Float64;
    soilMoistureDeficit             @53 :Float64;               # Soil moisture deficit [m3 m-3]
    soilPoreVolume                  @54 :List(Float64);         # Total soil pore volume [m3]; same as vs_Saturation
    vcStomataResistance             @55 :Float64;
    surfaceRoughness                @56 :Float64;               # Average amplitude of surface micro-elevations to hold back water in ponds [m]
    surfaceRunOff                   @57 :Float64;               # Amount of water running off on soil surface [mm]
    sumSurfaceRunOff                @58 :Float64;               # internal accumulation variable
    surfaceWaterStorage             @59 :Float64;               #  Simulates a virtual layer that contains the surface water [mm]
    ptTimeStep                      @60 :Float64;
    totalWaterRemoval               @61 :Float64;               # Total water removal of layer [m3]
    transpiration                   @62 :List(Float64);         # Transpiration of layer [mm]
    transpirationDeficit            @63 :Float64;
    waterFlux                       @64 :List(Float64);         # Soil water flux at the layer's upper boundary[mm d-1]
    vwWindSpeed                     @65 :Float64;               # [m s-1]
    vwWindSpeedHeight               @66 :Float64;               # [m]
    xSACriticalSoilMoisture         @67 :Float64;

    snowComponent                   @68 :SnowComponentState;
    frostComponent                  @69 :FrostComponentState;
    cropModule                      @70 :CropModuleState;
}

struct SoilTransportModuleState {
    struct ModuleParameters {
        dispersionLength                @0 :Float64;
		ad                              @1 :Float64;
		diffusionCoefficientStandard    @2 :Float64;
		nDeposition                     @3 :Float64;
    }

    soilColumn              @0  :SoilColumnState;
    stPs                    @1  :ModuleParameters;
    convection              @2  :List(Float64);
    cropNUptake             @3  :Float64;
    diffusionCoeff          @4  :List(Float64);
    dispersion              @5  :List(Float64);
    dispersionCoeff         @6  :List(Float64);
    vsLeachingDepth         @7  :Float64;       # [m]
    leachingAtBoundary      @8  :Float64;
    vsNDeposition           @9  :Float64;       # [kg N ha-1 y-1]
    vcNUptakeFromLayer      @10 :List(Float64); # Pflanzenaufnahme aus der Tiefe Z; C1 N-Konzentration [kg N ha-1]
    poreWaterVelocity       @11 :List(Float64);
    vsSoilMineralNContent   @12 :List(Float64);
    soilNO3                 @13 :List(Float64);
    soilNO3aq               @14 :List(Float64);
    timeStep                @15 :Float64 = 1.0;
    totalDispersion         @16 :List(Float64);
    percolationRate         @17 :List(Float64); # Soil water flux from above [mm d-1]

    pcMinimumAvailableN     @18 :Float64;       # kg m-2

    cropModule              @19 :CropModuleState;
}

struct AutomaticIrrigationParameters {
    amount      @0 :Float64 = 17.0;
	threshold   @1 :Float64 = 0.35;
}


struct NMinFertilizerParameters {
    id          @0 :Text;
	name        @1 :Text;
    carbamid    @2 :Float64; # [%]
	nh4         @3 :Float64; # [%]
	no3         @4 :Float64; # [%]
}

struct NMinUserParameters {
    min         @0 :Float64;
    max         @1 :Float64;
    delayInDays @2 :UInt16;
}


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
    nMinFertiliserPartition         @10 :MineralFertiliserParameters;
    nMinUserParams                  @11 :NMinUserParameters;

    useSecondaryYields              @12 :Bool = true;
    useAutomaticHarvestTrigger      @13 :Bool;

    numberOfLayers                  @14 :UInt16 = 20;
    layerThickness                  @15 :Float64 = 0.1;

    startPVIndex                    @16 :UInt16;
    julianDayAutomaticFertilising   @17 :UInt16;
}


struct MonicaModelState {
    sitePs                          @0  :SiteParameters;
    smPs                            @1  :SoilMoistureModuleState.ModuleParameters;
    envPs                           @2  :EnvironmentParameters;
    cropPs                          @3  :CropModuleState.ModuleParameters;
    soilTempPs                      @4  :SoilTemperatureModuleState.ModuleParameters;
    soilTransPs                     @5  :SoilTransportModuleState.ModuleParameters;
    soilOrganicPs                   @6  :SoilOrganicModuleState.ModuleParameters;
    simPs                           @7  :SimulationParameters;

    groundwaterInformation          @8  :MeasuredGroundwaterTableInformation;

    soilColumn                      @9  :SoilColumnState; # main soil data structure
    soilTemperature                 @10 :SoilTemperatureModuleState; # temperature code
    soilMoisture                    @11 :SoilMoistureModuleState; # moisture code
    soilOrganic                     @12 :SoilOrganicModuleState; # organic code
    soilTransport                   @13 :SoilTransportModuleState; # transport code
    currentCrop                     @14 :Crop; # currently possibly planted crop
    currentCropGrowth               @15 :CropModuleState; # crop code for possibly planted crop

    sumFertiliser                   @16 :Float64; 
    sumOrgFertiliser                @17 :Float64; 

    dailySumFertiliser              @18 :Float64; 
    dailySumOrgFertiliser           @19 :Float64; 

    dailySumOrganicFertilizerDM     @20 :Float64;
    sumOrganicFertilizerDM          @21 :Float64;

    humusBalanceCarryOver           @22 :Float64;

    dailySumIrrigationWater         @23 :Float64;

    optCarbonExportedResidues       @24 :Float64;
    optCarbonReturnedResidues       @25 :Float64;

    currentStepDate                 @26 :Date;

    struct ACDToValue {
        acd     @0 :UInt16;
        value   @1 :Float64;
    }
    climateData                     @27 :List(List(ACDToValue));
    currentEvents                   @28 :List(Text);
    previousDaysEvents              @29 :List(Text);

    clearCropUponNextDay            @30 :Bool;

    daysWithCrop                    @31 :UInt16;
    accuNStress                     @32 :Float64;
    accuWaterStress                 @33 :Float64;
    accuHeatStress                  @34 :Float64;
    accuOxygenStress                @35 :Float64;

    vwAtmosphericCO2Concentration   @36 :Float64;
    vwAtmosphericO3Concentration    @37 :Float64;
    vsGroundwaterDepth              @38 :Float64;

    cultivationMethodCount          @39 :UInt16;
}