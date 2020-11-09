@0xc75fa80819dba94e;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::models::monica");

using Date = import "date.capnp".Date;

struct MineralFertiliserParameters {
    id          @0 :Text;
	name        @1 :Text;
    voCarbamid  @2 :Float64;    # [%]
	voNH4       @3 :Float64;    # [%]
	voNO3       @4 :Float64;    # [%]
}

struct SoilParameters {
    vsSoilSandContent       @0  :Float64 = -1.0;    # Soil layer's sand content [kg kg-1] //{0.4}
	vsSoilClayContent       @1  :Float64 = -1.0;    # Soil layer's clay content [kg kg-1] (Ton) //{0.05}
	vsSoilpH                @2  :Float64 = 6.9;     # Soil pH value [] //{7.0}
	vsSoilStoneContent      @3  :Float64;           # Soil layer's stone content in soil [m3 m-3]
	vsLambda                @4  :Float64 = -1.0;    # Soil water conductivity coefficient [] //{0.5}
	vsFieldCapacity         @5  :Float64 = -1.0;    # {0.21} [m3 m-3]
	vsSaturation            @6  :Float64 = -1.0;    # {0.43} [m3 m-3]
	vsPermanentWiltingPoint @7  :Float64 = -1.0;    # {0.08} [m3 m-3]
	vs_SoilTexture          @8  :Text;
	vsSoilAmmonium          @9  :Float64 = 0.0005;  # soil ammonium content [kg NH4-N m-3]
	vsSoilNitrate           @10 :Float64 = 0.005;   # soil nitrate content [kg NO3-N m-3]
	vsSoil_CN_Ratio         @11 :Float64 = 10.0;
	vsSoilMoisturePercentFC @12 :Float64 = 100.0;
	vsSoilRawDensity        @13 :Float64 = -1.0;    # [kg m-3]
	vsSoilBulkDensity       @14 :Float64 = -1.0;    # [kg m-3]
	vsSoilOrganicCarbon     @15 :Float64 = -1.0;    # [kg kg-1]
	vsSoilOrganicMatter     @16 :Float64 = -1.0;    # [kg kg-1]
}

struct AOMProperties {
    voAOMSlow @0 :Float64 = 0.0;                   # C content in slowly decomposing added organic matter pool [kgC m-3]
    voAOMFast @1 :Float64 = 0.0;                   # C content in rapidly decomposing added organic matter pool [kgC m-3]

    voAOMSlowDecRatetoSMBSlow @2 :Float64 = 0.0;   # Rate for slow AOM consumed by SMB Slow is calculated.
    voAOMSlowDecRatetoSMBFast @3 :Float64 = 0.0;   # Rate for slow AOM consumed by SMB Fast is calculated.
    voAOMFastDecRatetoSMBSlow @4 :Float64 = 0.0;   # Rate for fast AOM consumed by SMB Slow is calculated.
    voAOMFastDecRatetoSMBFast @5 :Float64 = 0.0;   # Rate for fast AOM consumed by SMB Fast is calculated.

    voAOMSlowDecCoeff @6 :Float64 = 0.0;           # Is dependent on environment
    voAOMFastDecCoeff @7 :Float64 = 0.0;           # Is dependent on environment

    voAOMSlowDecCoeffStandard @8 :Float64 = 1.0;   # Decomposition rate coefficient for slow AOM pool at standard conditions
    voAOMFastDecCoeffStandard @9 :Float64 = 1.0;   # Decomposition rate coefficient for fast AOM pool at standard conditions

    voPartAOMSlowtoSMBSlow @10 :Float64 = 0.0;     # Partial transformation from AOM to SMB (soil microbiological biomass) for slow AOMs.
    voPartAOMSlowtoSMBFast @11 :Float64 = 0.0;     # Partial transformation from AOM to SMB (soil microbiological biomass) for fast AOMs.

    voCNRatioAOMSlow @12 :Float64 = 1.0;           # Used for calculation N-value if only C-value is known. Usually a constant value.
    voCNRatioAOMFast @13 :Float64 = 1.0;           # C-N-Ratio is dependent on the nutritional condition of the plant.

    voDaysAfterApplication @14 :UInt16 = 0;        # Fertilization parameter
    voAOM_DryMatterContent @15 :Float64 = 0.0;     # Fertilization parameter
    voAOM_NH4Content @16 :Float64 = 0.0;           # Fertilization parameter

    voAOMSlowDelta @17 :Float64 = 0.0;             # Difference of AOM slow between to timesteps
    voAOMFastDelta @18 :Float64 = 0.0;             # Difference of AOM fast between to timesteps

    incorporation @19 :Bool = false;               # true if organic fertilizer is added with a subsequent incorporation.
	noVolatilization @20 :Bool = true;             # true means it's a crop residue and won't participate in vo_volatilisation()
}

struct SoilLayerState {
    vsLayerThickness    @0  :Float64 = 0.1;         # Soil layer's vertical extension [m]
    vsSoilWaterFlux     @1  :Float64 = 0.0;         # Water flux at the upper boundary of the soil layer [l m-2]

    voAOMPool           @2  :List(AOMProperties);   # List of different added organic matter pools in soil layer

    vsSOMSlow           @3  :Float64 = 0.0;         # C content of soil organic matter slow pool [kg C m-3]
    vsSOMFast           @4  :Float64 = 0.0;         # content of soil organic matter fast pool size [kg C m-3]
    vsSMBSlow           @5  :Float64 = 0.0;         # C content of soil microbial biomass slow pool size [kg C m-3]
    vsSMBFast           @6  :Float64 = 0.0;         # C content of soil microbial biomass fast pool size [kg C m-3]

    # anorganische Stickstoff-Formen
    vsSoilCarbamid      @7  :Float64 = 0.0;         # Soil layer's carbamide-N content [kg Carbamide-N m-3]
    vsSoilNH4           @8  :Float64 = 0.0001;      # Soil layer's NH4-N content [kg NH4-N m-3]
    vsSoilNO2           @9  :Float64 = 0.001;       # Soil layer's NO2-N content [kg NO2-N m-3]
    vsSoilNO3           @10 :Float64 = 0.0001;      # Soil layer's NO3-N content [kg NO3-N m-3]
    vsSoilFrozen        @11 :Bool = false;

    sps                 @12 :SoilParameters;
    vsSoilMoisture_m3   @13 :Float64 = 0.25;        # Soil layer's moisture content [m3 m-3]
    vsSoilTemperature   @14 :Float64 = 0.0;         # Soil layer's temperature [°C]
}

struct CropParameters {
    speciesParams   @0 :SpeciesParameters;
    cultivarParams  @1 :CultivarParameters;
}

struct SpeciesParameters {
    pcSpeciesId                                 @0  :Text;
    pcCarboxylationPathway                      @1  :UInt8;
    pcDefaultRadiationUseEfficiency             @2  :Float64;
    pcPartBiologicalNFixation                   @3  :Float64;
    pcInitialKcFactor                           @4  :Float64;
    pcLuxuryNCoeff                              @5  :Float64;
    pcMaxCropDiameter                           @6  :Float64;
    pcStageAtMaxHeight                          @7  :Float64;
    pcStageAtMaxDiameter                        @8  :Float64;
    pcMinimumNConcentration                     @9  :Float64;
    pcMinimumTemperatureForAssimilation         @10 :Float64;
    pcOptimumTemperatureForAssimilation         @11 :Float64;
    pcMaximumTemperatureForAssimilation         @12 :Float64;
    pcNConcentrationAbovegroundBiomass          @13 :Float64;
    pcNConcentrationB0                          @14 :Float64;
    pcNConcentrationPN                          @15 :Float64;
    pcNConcentrationRoot                        @16 :Float64;
    pcDevelopmentAccelerationByNitrogenStress   @17 :UInt16;
    pcFieldConditionModifier                    @18 :Float64 = 1.0;
    pcAssimilateReallocation                    @19 :Float64;

    pcBaseTemperature                           @20 :List(Float64);
    pcOrganMaintenanceRespiration               @21 :List(Float64);
    pcOrganGrowthRespiration                    @22 :List(Float64);
    pcStageMaxRootNConcentration                @23 :List(Float64);
    pcInitialOrganBiomass                       @24 :List(Float64);
    pcCriticalOxygenContent                     @25 :List(Float64);
	pcStageMobilFromStorageCoeff                @26 :List(Float64);

    pcAbovegroundOrgan                          @27 :List(Bool);
    pcStorageOrgan                              @28 :List(Bool);

    pcSamplingDepth                             @29 :Float64;
    pcTargetNSamplingDepth                      @30 :Float64;
    pcTargetN30                                 @31 :Float64;
    pcMaxNUptakeParam                           @32 :Float64;
    pcRootDistributionParam                     @33 :Float64;
    pcPlantDensity                              @34 :UInt16; # [plants m-2]
    pcRootGrowthLag                             @35 :Float64
    pcMinimumTemperatureRootGrowth              @36 :Float64;
    pcInitialRootingDepth                       @37 :Float64;
    pcRootPenetrationRate                       @38 :Float64;
    pcRootFormFactor                            @39 :Float64;
    pcSpecificRootLength                        @40 :Float64;
    pcStageAfterCut                             @41 :UInt16;
    pcLimitingTemperatureHeatStress             @42 :Float64;
    pcCuttingDelayDays                          @43 :UInt16;
    pcDroughtImpactOnFertilityFactor            @44 :Float64;

    EF_MONO                                     @45 :Float64 = 0.5;     # = MTsynt [ug gDW-1 h-1] Monoterpenes, which will be emitted right after synthesis
    EF_MONOS                                    @46 :Float64 = 0.5;     # = MTpool [ug gDW-1 h-1] Monoterpenes, which will be stored after synthesis in stores (mostly intra- oder intercellular space of leafs and then are being emitted; quasi evaporation)
    EF_ISO                                      @47 :Float64;           # Isoprene emission factor
    VCMAX25                                     @48 :Float64;           # maximum RubP saturated rate of carboxylation at 25oC for sun leaves (umol m-2 s-1)
    AEKC                                        @49 :Float64 = 65800.0; # activation energy for Michaelis-Menten constant for CO2 (J mol-1) | MONICA default=65800.0 | LDNDC default=59356.0
    AEKO                                        @50 :Float64 = 1400.0;  # activation energy for Michaelis-Menten constant for O2 (J mol-1) | MONICA default=65800.0 | LDNDC default=35948.0
    AEVC                                        @51 :Float64 = 68800.0; # activation energy for photosynthesis (J mol-1) | MONICA default=68800.0 | LDNDC default=58520.0
    KC25                                        @52 :Float64 = 460.0;   # Michaelis-Menten constant for CO2 at 25oC (umol mol-1 ubar-1) | MONICA default=460.0 | LDNDC default=260.0
    KO25                                        @53 :Float64 = 330.0;   # Michaelis-Menten constant for O2 at 25oC (mmol mol-1 mbar-1) | MONICA default=330.0 | LDNDC default=179.0

    pcTransitionStageLeafExp                    @54 :Int16 = -1; # [1-7]
}

struct CultivarParameters {
    pcCultivarId                            @0  :Text;
    pcDescription                           @1  :Text;
    cPerennial                              @2  :Bool;
    #std::string pc_PermanentCultivarId;
    pcMaxAssimilationRate                   @4  :Float64;
    pcMaxCropHeight                         @5  :Float64;
    pcResidueNRatio                         @6  :Float64;
    pcLT50cultivar                          @7  :Float64;

    pcCropHeightP1                          @8  :Float64;
    pcCropHeightP2                          @9  :Float64;
    pcCropSpecificMaxRootingDepth           @10 :Float64;

    pcAssimilatePartitioningCoeff           @11 :List(List(Float64));
    pcOrganSenescenceRate                   @12 :List(List(Float64));

    pcBaseDaylength                         @13 :List(Float64);
    pcOptimumTemperature                    @14 :List(Float64);
    pcDaylengthRequirement                  @15 :List(Float64);
    pcDroughtStressThreshold                @16 :List(Float64);
    pcSpecificLeafArea                      @17 :List(Float64);
    pcStageKcFactor                         @18 :List(Float64);
    pcStageTemperatureSum                   @19 :List(Float64);
    pcVernalisationRequirement              @20 :List(Float64);

    pcHeatSumIrrigationStart                @21 :Float64;
    pcHeatSumIrrigationEnd                  @22 :Float64;

    pcCriticalTemperatureHeatStress         @23 :Float64;
    pcBeginSensitivePhaseHeatStress         @24 :Float64;
    pcEndSensitivePhaseHeatStress           @25 :Float64;

    pcFrostHardening                        @26 :Float64;
    pcFrostDehardening                      @27 :Float64;
    pcLowTemperatureExposure                @28 :Float64;
    pcRespiratoryStress                     @29 :Float64;
    pcLatestHarvestDoy                      @30 :Int16 = -1;

    pcOrganIdsForPrimaryYield               @31 :List(YieldComponent);
    pcOrganIdsForSecondaryYield             @32 :List(YieldComponent);
    pcOrganIdsForCutting                    @33 :List(YieldComponent);

    pcEarlyRefLeafExp                       @34 :Float64 = 12.0; # 12 = wheat (first guess)
    pcRefLeafExp                            @35 :Float64 = 20.0; # 20 = wheat, 22 = maize (first guess)

    pcMinTempDevWE                          @36 :Float64;
    pcOptTempDevWE                          @37 :Float64;
    pcMaxTempDevWE                          @39 :Float64;
}


struct CropModuleState {
    struct ModuleParameters {
        pcCanopyReflectionCoefficient                           @0  :Float64;
        pcReferenceMaxAssimilationRate                          @1  :Float64;
        pcReferenceLeafAreaIndex                                @2  :Float64;
        pcMaintenanceRespirationParameter1                      @3  :Float64;
        pcMaintenanceRespirationParameter2                      @4  :Float64;
        pcMinimumNConcentrationRoot                             @5  :Float64;
        pcMinimumAvailableN                                     @6  :Float64;
        pcReferenceAlbedo                                       @7  :Float64;
        pcStomataConductanceAlpha                               @8  :Float64;
        pcSaturationBeta                                        @9  :Float64;
        pcGrowthRespirationRedux                                @10 :Float64;
        pcMaxCropNDemand                                        @11 :Float64;
        pcGrowthRespirationParameter1                           @12 :Float64;
        pcGrowthRespirationParameter2                           @13 :Float64;
        pcTortuosity                                            @14 :Float64;
        pcAdjustRootDepthForSoilProps                           @15 :Bool;  

        __enablePhenologyWangEngelTemperatureResponse__         @16 :Bool;
        __enablePhotosynthesisWangEngelTemperatureResponse__    @17 :Bool;
        __enableHourlyFvCBPhotosynthesis__                      @18 :Bool;
        __enableTResponseLeafExpansion__                        @19 :Bool;
        __disableDailyRootBiomassToSoil__                       @20 :Bool;
    }


    frostKillOn                                 @0      :Bool;      
    soilColumn                                  @1      :SoilColumnState;
    perennialCropParams                         @2      :CropParameters;
    cropPs                                      @3      :ModuleParameters;
    speciesPs                                   @4      :SpeciesParameters;
    cultivarPs                                  @5      :CultivarParameters;      
    vsLatitude                                  @6      :Float64;  
    vcAbovegroundBiomass                        @7      :Float64; # old OBMAS
    vcAbovegroundBiomassOld                     @8      :Float64; # old OBALT
    pcAbovegroundOrgan                          @9      :List(Bool); # old KOMP
    vcActualTranspiration                       @10     :Float64; 
    pcAssimilatePartitioningCoeff               @11     :List(List(Float64)); # old PRO
    pcAssimilateReallocation                    @12     :Float64; 
    vcAssimilates                               @13     :Float64; 
    vcAssimilationRate                          @14     :Float64; # old AMAX
    vcAstronomicDayLenght                       @15     :Float64; # old DL
    pcBaseDaylength                             @16     :List(Float64); # old DLBAS
    pcBaseTemperature                           @17     :List(Float64); # old BAS
    pcBeginSensitivePhaseHeatStress             @18     :Float64;
    vcBelowgroundBiomass                        @19     :Float64; 
    vcBelowgroundBiomassOld                     @20     :Float64;
    pcCarboxylationPathway                      @21     :Int64; # old TEMPTYP
    vcClearDayRadiation                         @22     :Float64; # old DRC
    pcCO2Method                                 @23     :UInt8 = 3;
    vcCriticalNConcentration                    @24     :Float64; # old GEHMIN
    pcCriticalOxygenContent                     @25     :List(Float64); # old LUKRIT
    pcCriticalTemperatureHeatStress             @26     :Float64; 
    vcCropDiameter                              @27     :Float64;
    vcCropFrostRedux                            @28     :Float64 = 1.0;
    vcCropHeatRedux                             @29     :Float64 = 1.0;
    vcCropHeight                                @30     :Float64;
    pcCropHeightP1                              @31     :Float64;
    pcCropHeightP2                              @32     :Float64;
    pcCropName                                  @33     :Text; # old FRUCHT$(AKF)
    vcCropNDemand                               @34     :Float64; # old DTGESN
    vcCropNRedux                                @35     :Float64 = 1.0; # old REDUK
    pcCropSpecificMaxRootingDepth               @36     :Float64; # old WUMAXPF [m]
    vcCropWaterUptake                           @37     :List(Float64); # old TP
    vcCurrentTemperatureSum                     @38     :List(Float64); # old SUM
    vcCurrentTotalTemperatureSum                @39     :Float64; # old FP
    vcCurrentTotalTemperatureSumRoot            @40     :Float64;
    pcCuttingDelayDays                          @41     :UInt16;
    vcDaylengthFactor                           @42     :Float64; # old DAYL
    pcDaylengthRequirement                      @43     :List(Float64); # old DEC
    vcDaysAfterBeginFlowering                   @44     :UInt16;
    vcDeclination                               @45     :Float64; # old EFF0
    pcDefaultRadiationUseEfficiency             @46     :Float64;
    vmDepthGroundwaterTable                     @47     :UInt16; # old GRW
    pcDevelopmentAccelerationByNitrogenStress   @48     :UInt64;
    vcDevelopmentalStage                        @49     :UInt16; # old INTWICK
    noOfCropSteps                               @50     :UInt16; 
    vcDroughtImpactOnFertility                  @51     :Float64 = 1.0;
    pcDroughtImpactOnFertilityFactor            @52     :Float64;
    pcDroughtStressThreshold                    @53     :List(Float64); # old DRYswell
    pcEmergenceFloodingControlOn                @54     :Bool = false;
    pcEmergenceMoistureControlOn                @55     :Bool = false;
    pcEndSensitivePhaseHeatStress               @56     :Float64;
    vcEffectiveDayLength                        @57     :Float64; # old DLE
    vcErrorStatus                               @58     :Bool = false;
    vcErrorMessage                              @59     :Text;
    vcEvaporatedFromIntercept                   @60     :Float64;
    vcExtraterrestrialRadiation                 @61     :Float64;
    pcFieldConditionModifier                    @62     :Float64;
    vcFinalDevelopmentalStage                   @63     :UInt16;
    vcFixedN                                    @64     :Float64;
    #std::vector<double> vo_FreshSoilOrganicMatter @16 :List(Float64); # old NFOS
    pcFrostDehardening                          @65     :Float64;
    pcFrostHardening                            @66     :Float64;
    vcGlobalRadiation                           @67     :Float64;
    vcGreenAreaIndex                            @68     :Float64;
    vcGrossAssimilates                          @69     :Float64;
    vcGrossPhotosynthesis                       @70     :Float64; # old GPHOT
    vcGrossPhotosynthesis_mol                   @71     :Float64;
    vcGrossPhotosynthesisReferenceMol           @72     :Float64;
    vcGrossPrimaryProduction                    @73     :Float64;
    vcGrowthCycleEnded                          @74     :Bool = false;
    vcGrowthRespirationAS                       @75     :Float64 = 0.0;
    pcHeatSumIrrigationStart                    @76     :Float64;
    pcHeatSumIrrigationEnd                      @77     :Float64;
    vsHeightNN                                  @78     :Float64;
    pcInitialKcFactor                           @79     :Float64; # old Kcini
    pcInitialOrganBiomass                       @80     :List(Float64);
    pcInitialRootingDepth                       @81     :Float64;
    vcInterceptionStorage                       @82     :Float64; 
    vcKcFactor                                  @83     :Float64 = 0.6; # old FKc
    vcLeafAreaIndex                             @84     :Float64; # old LAI
    vcSunlitLeafAreaIndex                       @85     :List(Float64);
    vcShadedLeafAreaIndex                       @86     :List(Float64);
    pcLowTemperatureExposure                    @87     :Float64;
    pcLimitingTemperatureHeatStress             @88     :Float64;
    vcLT50                                      @89     :Float64 = -3.0;
    pcLT50cultivar                              @90     :Float64;
    pcLuxuryNCoeff                              @91     :Float64;
    vcMaintenanceRespirationAS                  @92     :Float64;
    pcMaxAssimilationRate                       @93     :Float64; # old MAXAMAX
    pcMaxCropDiameter                           @94     :Float64;
    pcMaxCropHeight                             @95     :Float64;
    vcMaxNUptake                                @96     :Float64; # old MAXUP
    pcMaxNUptakeParam                           @97     :Float64;
    vcMaxRootingDepth                           @98     :Float64; # old WURM
    pcMinimumNConcentration                     @99     :Float64;
    pcMinimumTemperatureForAssimilation         @100    :Float64; # old MINTMP
    pcOptimumTemperatureForAssimilation         @101    :Float64;
    pcMaximumTemperatureForAssimilation         @102    :Float64;
    pcMinimumTemperatureRootGrowth              @103    :Float64;
    vcNetMaintenanceRespiration                 @104    :Float64; # old MAINT
    vcNetPhotosynthesis                         @105    :Float64; # old GTW
    vcNetPrecipitation                          @106    :Float64;
    vcNetPrimaryProduction                      @107    :Float64;
    pcNConcentrationAbovegroundBiomass          @108    :Float64; # initial value of old GEHOB
    vcNConcentrationAbovegroundBiomass          @109    :Float64; # old GEHOB
    vcNConcentrationAbovegroundBiomassOld       @110    :Float64; # old GEHALT
    pcNConcentrationB0                          @111    :Float64;
    vcNContentDeficit                           @112    :Float64;
    pcNConcentrationPN                          @113    :Float64; 
    pcNConcentrationRoot                        @114    :Float64; # initial value to WUGEH
    vcNConcentrationRoot                        @115    :Float64; # old WUGEH
    vcNConcentrationRootOld                     @116    :Float64; # old
    pcNitrogenResponseOn                        @117    :Bool; 
    pcNumberOfDevelopmentalStages               @118    :Float64;
    pcNumberOfOrgans                            @119    :Float64; # old NRKOM
    vcNUptakeFromLayer                          @120    :List(Float64); # old PE
    pcOptimumTemperature                        @121    :List(Float64);
    vcOrganBiomass                              @122    :List(Float64); # old WORG
    vcOrganDeadBiomass                          @123    :List(Float64); # old WDORG
    vcOrganGreenBiomass                         @124    :List(Float64);
    vcOrganGrowthIncrement                      @125    :List(Float64); # old GORG
    pcOrganGrowthRespiration                    @126    :List(Float64); # old MAIRT
    pcOrganIdsForPrimaryYield                   @127    :List(YieldComponent);
    pcOrganIdsForSecondaryYield                 @128    :List(YieldComponent);
    pcOrganIdsForCutting                        @129    :List(YieldComponent);
    pcOrganMaintenanceRespiration               @130    :List(Float64); # old MAIRT
    vcOrganSenescenceIncremen                   @131    :List(Float64); # old DGORG
    pcOrganSenescenceRate                       @132    :List(List(Float64)); # old DEAD
    vcOvercastDayRadiation                      @133    :Float64; # old DRO
    vcOxygenDeficit                             @134    :Float64; # old LURED
    pcPartBiologicalNFixation                   @135    :Float64;
    pcPerennial                                 @136    :Bool;
    vcPhotoperiodicDaylength                    @137    :Float64; # old DLP
    vcPhotActRadiationMean                      @138    :Float64; # old RDN
    pcPlantDensity                              @139    :Float64; 
    vcPotentialTranspiration                    @140    :Float64; 
    vcReferenceEvapotranspiration               @141    :Float64; 
    vcRelativeTotalDevelopment                  @142    :Float64; 
    vcRemainingEvapotranspiration               @143    :Float64; 
    vcReserveAssimilatePool                     @144    :Float64; # old ASPOO
    pcResidueNRatio                             @145    :Float64; 
    pcRespiratoryStress                         @146    :Float64; 
    vcRootBiomass                               @147    :Float64; # old WUMAS
    vcRootBiomassOld                            @148    :Float64; # old WUMALT
    vcRootDensity                               @149    :List(Float64); # old WUDICH
    vcRootDiameter                              @150    :List(Float64); # old WRAD
    pcRootDistributionParam                     @151    :Float64;
    vcRootEffectivity                           @152    :List(Float64); # old WUEFF
    pcRootFormFactor                            @153    :Float64;
    pcRootGrowthLag                             @154    :Float64;
    vcRootingDepth                              @155    :UInt16; # old WURZ
    vcRootingDepthM                             @156    :Float64;
    vcRootingZone                               @157    :UInt16;
    pcRootPenetrationRate                       @158    :Float64;
    vmSaturationDeficit                         @159    :Float64;
    vcSoilCoverage                              @160    :Float64;
    vsSoilMineralNContent                       @161    :List(Float64); # old C1
    vcSoilSpecificMaxRootingDepth               @162    :Float64; # old WURZMAX [m]
    vsSoilSpecificMaxRootingDepth               @163    :Float64;
    pcSpecificLeafArea                          @164    :List(Float64); # old LAIFKT [ha kg-1]
    pcSpecificRootLength                        @165    :Float64;
    pcStageAfterCut                             @166    :UInt16; # //0-indexed
    pcStageAtMaxDiameter                        @167    :Float64;
    pcStageAtMaxHeight                          @168    :Float64;
    pcStageMaxRootNConcentration                @169    :List(Float64); # old WGMAX
    pcStageKcFactor                             @170    :List(Float64); # old Kc
    pcStageTemperatureSum                       @171    :List(Float64); # old TSUM
    vcStomataResistance                         @172    :Float64; # old RSTOM
    pcStorageOrgan                              @173    :List(Bool);  
    vcStorageOrgan                              @174    :UInt16 = 4;
    vcTargetNConcentration                      @175    :Float64; # old GEHMAX
    vcTimeStep                                  @176    :Float64 = 1.0; # old dt
    vcTimeUnderAnoxia                           @177    :UInt64; 
    vsTortuosity                                @178    :Float64; # old AD
    vcTotalBiomass                              @179    :Float64;
    vcTotalBiomassNContent                      @180    :Float64; # old PESUM
    vcTotalCropHeatImpact                       @181    :Float64;
    vcTotalNInput                               @181    :Float64;
    vcTotalNUptake                              @182    :Float64; # old SUMPE
    vcTotalRespired                             @183    :Float64;
    vcRespiration                               @184    :Float64;
    vcSumTotalNUptake                           @185    :Float64; # summation of all calculated NUptake; needed for sensitivity analysis
    vcTotalRootLength                           @186    :Float64; # old WULAEN
    vcTotalTemperatureSum                       @187    :Float64;
    vcTemperatureSumToFlowering                 @188    :Float64;
    vcTranspiration                             @189    :List(Float64); # old TP
    vcTranspirationRedux                        @190    :List(Float64); # old TRRED
    vcTranspirationDeficit                      @191    :Float64 = 1.0; # old TRREL
    vcVernalisationDays                         @192    :Float64;
    vcVernalisationFactor                       @192    :Float64; # old FV
    pcVernalisationRequirement                  @193    :List(Float64); # old VSCHWELL
    pcWaterDeficitResponseOn                    @194    :Bool;

    eva2_usage                                  @195    :UInt8;
    eva2_primaryYieldComponents                 @196    :List(YieldComponent);
    eva2_secondaryYieldComponents               @197    :List(YieldComponent);   
    dyingOut                                    @198    :Bool;
    vcAccumulatedETa                            @199    :Float64;
    vcAccumulatedTranspiration                  @200    :Float64;
    vcAccumulatedPrimaryCropYield               @201    :Float64;
    vcSumExportedCutBiomass                     @202    :Float64;
    vcExportedCutBiomass                        @203    :Float64;
    vcSumResidueCutBiomass                      @204    :Float64;
    vcResidueCutBiomass                         @205    :Float64;

    vc_CuttingDelayDays                         @206    :UInt16;
    vsMaxEffectiveRootingDepth                  @207    :Float64;
    vsImpenetrableLayerDept                     @208    :Float64; # h

    vcAnthesisDay                               @209    :Int16 = -1;
    vcMaturityDay                               @210    :Int16 = -1;

    vcMaturityReached                           @211    :Bool;

    # VOC members   
    stepSize24                                  @212    :UInt16 = 24;
    stepSize240                                 @213    :UInt16 = 240;
    rad24                                       @214    :List(Float64); 
    rad240                                      @215    :Float64; 
    tfol24                                      @216    :Float64; 
    tfol240                                     @217    :Float64;
    index24                                     @218    :UInt16; 
    index240                                    @219    :UInt16;
    full24                                      @220    :Bool; 
    full240                                     @221    :Bool;

    guentherEmissions                           @222    :Voc.Emissions;
    jjvEmissions                                @223    :Voc.Emissions;
    vocSpecies                                  @224    :Voc.SpeciesData;
    cropPhotosynthesisResults                   @225    :Voc.CPData;

    #std::function<void(std::string)> _fireEvent;   
    #std::function<void(std::map<size_t, double>, double)> _addOrganicMatter;

    vcO3ShortTermDamage                         @226    :Float64 = 1.0;
    vcO3LongTermDamage                          @227    :Float64 = 1.0;
    vcO3Senescence                              @228    :Float64 = 1.0;
    vcO3SumUptake                               @229    :Float64;
    vcO3WStomatalClosure                        @230    :Float64 = 1.0;

    assimilatePartCoeffsReduced                 @231    :Bool;
    vcKTkc                                      @232    :Float64; # old KTkc
    vcKTko                                      @233    :Float64; # old KTko
}

struct Voc {
    struct Emissions {

    }

    struct SpeciesData {

    }

    struct CPData {

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
    vtSoilSurfaceTemperature    @0  :Float64;
    soilColumn                  @1  :SoilColumnState;
    monica                      @2  :MonicaModelState;
    soilColumnVtGroundLayer     @3  :SoilLayerState;
    soilColumnVtBottomLayer     @4  :SoilLayerState;
    vtNumberOfLayers            @5  :UInt16;
    vsNumberOfLayers            @6  :UInt16;
    vsSoilMoisture_const        @7  :List(Float64);
    vtSoilTemperature           @8  :List(Float64);
    vtV                         @9  :List(Float64);
    vtVolumeMatrix              @10 :List(Float64);
    vtVolumeMatrixOld           @11 :List(Float64);
    vtB                         @12 :List(Float64);
    vtMatrixPrimaryDiagonal     @13 :List(Float64);
    vtMatrixSecundaryDiagonal   @14 :List(Float64);
    vtHeatFlow                  @15 :Float64;
    vtHeatConductivity          @16 :List(Float64);
    vtHeatConductivityMean      @17 :List(Float64);
    vtHeatCapacity              @18 :List(Float64);
    dampingFactor               @19 :Float64 = 0.8;
}

struct SiteParameters {
    vsLatitude                              @0  :Float64 = 52.5; # ZALF latitude
    vsSlope                                 @1  :Float64 = 0.01; # [m m-1]
    vsHeightNN                              @2  :Float64 = 50.0; # [m]
    vsGroundwaterDepth                      @3  :Float64 = 70.0; # [m]
    vsSoilCNRatio                           @4  :Float64 = 10.0;
    vsDrainageCoeff                         @5  :Float64 = 1.0;
    vqNDeposition                           @6  :Float64 = 30.0; # [kg N ha-1 y-1]
    vsMaxEffectiveRootingDepth              @7  :Float64 = 2.0;  # [m]
    vsImpenetrableLayerDepth                @8  :Float64 = -1;   # [m]
    vsSoilSpecificHumusBalanceCorrection    @9  :Float64;        # humus equivalents

    vsSoilParameters                        @10 :List(SoilParameters);
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
        poSOMSlowDecCoeffStandard           @0  :Float64 = 4.30e-5;     # 4.30e-5 [d-1], Bruun et al. 2003 4.3e-5
		poSOMFastDecCoeffStandard           @1  :Float64 = 1.40e-4;     # 1.40e-4 [d-1], from DAISY manual 1.4e-4
		poSMBSlowMaintRateStandard          @2  :Float64 = 1.00e-3;     # 1.00e-3 [d-1], from DAISY manual original 1.8e-3
		poSMBFastMaintRateStandard          @3  :Float64 = 1.00e-2;     # 1.00e-2 [d-1], from DAISY manual
		poSMBSlowDeathRateStandard          @4  :Float64 = 1.00e-3;     # 1.00e-3 [d-1], from DAISY manual
		poSMBFastDeathRateStandard          @5  :Float64 = 1.00e-2;     # 1.00e-2 [d-1], from DAISY manual
		poSMBUtilizationEfficiency          @6  :Float64 = 0.60;        # 0.60 [], from DAISY manual 0.6
		poSOMSlowUtilizationEfficiency      @7  :Float64 = 0.40;        # 0.40 [], from DAISY manual 0.4
		poSOMFastUtilizationEfficiency      @8  :Float64 = 0.50;        # 0.50 [], from DAISY manual 0.5
		poAOMSlowUtilizationEfficiency      @9  :Float64 = 0.40;        # 0.40 [], from DAISY manual original 0.13
		poAOMFastUtilizationEfficiency      @10 :Float64 = 0.10;        # 0.10 [], from DAISY manual original 0.69
		poAOMFastMaxCtoN                    @11 :Float64 = 1000.0;      
		poPartSOMFastToSOMSlow              @12 :Float64 = 0.30;        # 0.30 [], Bruun et al. 2003
		poPartSMBSlowToSOMFast              @13 :Float64 = 0.60;        # 0.60 [], from DAISY manual
		poPartSMBFastToSOMFast              @14 :Float64 = 0.60;        # 0.60 [], from DAISY manual
		poPartSOMToSMBSlow                  @15 :Float64 = 0.0150;      # 0.0150 [], optimised
		poPartSOMToSMBFast                  @16 :Float64 = 0.0002;      # 0.0002 [], optimised
		poCNRatioSMB                        @17 :Float64 = 6.70;        # 6.70 [], from DAISY manual
		poLimitClayEffect                   @18 :Float64 = 0.25;        # 0.25 [kg kg-1], from DAISY manual
		poAmmoniaOxidationRateCoeffStandard @19 :Float64 = 1.0e-1;      # 1.0e-1 [d-1], from DAISY manual
		poNitriteOxidationRateCoeffStandard @20 :Float64 = 9.0e-1;      # 9.0e-1 [d-1], fudged by Florian Stange
		poTransportRateCoeff                @21 :Float64 = 0.1;         # 0.1 [d-1], from DAISY manual
		poSpecAnaerobDenitrification        @22 :Float64 = 0.1;         # 0.1 [g gas-N g CO2-C-1]
		poImmobilisationRateCoeffNO3        @23 :Float64 = 0.5;         # 0.5 [d-1]
		poImmobilisationRateCoeffNH4        @24 :Float64 = 0.5;         # 0.5 [d-1]
		poDenit1                            @25 :Float64 = 0.2;         # 0.2 Denitrification parameter
		poDenit2                            @26 :Float64 = 0.8;         # 0.8 Denitrification parameter
		poDenit3                            @27 :Float64 = 0.9;         # 0.9 Denitrification parameter
		poHydrolysisKM                      @28 :Float64 = 0.00334;     # 0.00334 from Tabatabai 1973
		poActivationEnergy                  @29 :Float64 = 41000.0;     # 41000.0 from Gould et al. 1973
		poHydrolysisP1                      @30 :Float64 = 4.259e-12;   # 4.259e-12 from Sadeghi et al. 1988
		poHydrolysisP2                      @31 :Float64 = 1.408e-12;   # 1.408e-12 from Sadeghi et al. 1988
		poAtmosphericResistance             @32 :Float64 = 0.0025;      # 0.0025 [s m-1], from Sadeghi et al. 1988
		poN2OProductionRate                 @33 :Float64 = 0.5;         # 0.5 [d-1]
		poInhibitorNH3                      @34 :Float64 = 1.0;         # 1.0 [kg N m-3] NH3-induced inhibitor for nitrite oxidation
		psMaxMineralisationDepth            @35 :Float64 = 0.4;

        sticsParams @ Stics.Parameters;
    }

    soilColumn                          @0  :SoilColumnState;
    siteParams                          @1  :SiteParameters;
    organicPs                           @2  :ModuleParameters;

    vsNumberOfLayers                    @3  :UInt16;
    vsNumberOfOrganicLayers             @4  :UInt16;
    addedOrganicMatter                  @5  :Bool;
    irrigationAmount                    @6  :Float64;
    voActAmmoniaOxidationRate           @7  :List(Float64); # [kg N m-3 d-1]
    voActNitrificationRate              @8  :List(Float64); # [kg N m-3 d-1]
    voActDenitrificationRate            @9  :List(Float64);  # [kg N m-3 d-1]
    voAOMFastDeltaSum                   @10 :List(Float64);
    voAOMFastInput                      @11 :List(Float64); # AOMfast pool change by direct input [kg C m-3]
    voAOMFastSum                        @12 :List(Float64);
    voAOMSlowDeltaSum                   @13 :List(Float64);
    voAOMSlowInput                      @14 :List(Float64); # AOMslow pool change by direct input [kg C m-3]
    voAOMSlowSum                        @15 :List(Float64);
    voCBalance                          @16 :List(Float64);
    voDecomposerRespiration             @17 :Float64;
    voErrorMessage                      @18 :Text;
    voInertSoilOrganicC                 @19 :List(Float64);
    voN2OProduced                       @20 :Float64; # [kg-N2O-N/ha]
    voN2OProducedNit                    @21 :Float64; # [kg-N2O-N/ha]
    voN2OProducedDenit                  @22 :Float64; # [kg-N2O-N/ha]
    voNetEcosystemExchange              @23 :Float64;
    voNetEcosystemProduction            @24 :Float64;
    voNetNMineralisation                @25 :Float64;
    voNetNMineralisationRate            @26 :List(Float64);
    voTotalNH3Volatilised               @27 :Float64;
    voNH3Volatilised                    @28 :Float64;
    voSMBCO2EvolutionRate               @29 :List(Float64);
    voSMBFastDelta                      @30 :List(Float64);
    voSMBSlowDelta                      @31 :List(Float64);
    vsSoilMineralNContent               @32 :List(Float64);
    voSoilOrganicC                      @33 :List(Float64);
    voSOMFastDelta                      @34 :List(Float64);
    voSOMFastInput                      @35 :List(Float64); # SOMfast pool change by direct input [kg C m-3]
    voSOMSlowDelta                      @36 :List(Float64);
    voSumDenitrification                @37 :Float64;       # kg-N/m2
    voSumNetNMineralisation             @38 :Float64;
    voSumN2OProduced                    @39 :Float64;
    voSumNH3Volatilised                 @40 :Float64;
    voTotalDenitrification              @41 :Float64;

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


struct SoilMoistureModuleState {
    struct SnowComponentState {
        soilColumn                              @0  :SoilColumn;

        vmSnowDensity                           @1  :Float64;   # Snow density [kg dm-3]
        vmSnowDepth                             @2  :Float64;   # Snow depth [mm]
        vmFrozenWaterInSnow                     @3  :Float64;   # [mm]
        vmLiquidWaterInSnow                     @4  :Float64;   # [mm]
        vmWaterToInfiltrate                     @5  :Float64;   # [mm]
        vmMaxSnowDepth                          @6  :Float64;   # [mm]
        vmAccumulatedSnowDepth                  @7  :Float64;   # [mm]

        # extern or user defined snow parameter
        vmSnowmeltTemperature                   @8  :Float64;   # Base temperature for snowmelt [°C]
        vmSnowAccumulationThresholdTemperature  @9  :Float64;
        vmTemperatureLimitForLiquidWater        @10 :Float64;   # Lower temperature limit of liquid water in snow
        vmCorrectionRain                        @11 :Float64;   # Correction factor for rain (no correction used here)
        vmCorrectionSnow                        @12 :Float64;   # Correction factor for snow (value used in COUP by Lars Egil H.)
        vmRefreezeTemperature                   @13 :Float64;   # Base temperature for refreeze [°C]
        vmRefreezeP1                            @14 :Float64;   # Refreeze parameter (Karvonen's value)
        vmRefreezeP2                            @15 :Float64;   # Refreeze exponent (Karvonen's value)
        vmNewSnowDensityMin                     @16 :Float64;   # Minimum density of new snow
        vmSnowMaxAdditionalDensity              @17 :Float64;   # Maximum additional density of snow (max rho = 0.35, Karvonen)
        vmSnowPacking                           @18 :Float64;   # Snow packing factor (calibrated by Helge Bonesmo)
        vmSnowRetentionCapacityMin              @19 :Float64;   # Minimum liquid water retention capacity in snow [mm]
        vmSnowRetentionCapacityMax              @20 :Float64;  
    }

    struct FrostComponentState {
        soilColumn                      @0  :SoilColumn;
        vm_FrostDepth                   @1  :Float64;
        vmAccumulatedFrostDepth         @2  :Float64;
        vmNegativeDegreeDays            @3  :Float64;       # Counts negative degree-days under snow
        vmThawDepth                     @4  :Float64;
        vmFrostDays                     @5  :UInt16;
        vmLambdaRedux                   @6  :List(Float64); # Reduction factor for Lambda []
        vmTemperatureUnderSnow          @7  :Float64;

        # user defined or data base parameter
        vmHydraulicConductivityRedux    @8  :Float64;
        ptTimeStep                      @9  :Float64;

        pmHydraulicConductivityRedux    @10 :Float64;
    }

    soilColumn                      @0  :SoilColumnState;
    siteParameters                  @1  :SiteParameters;
    monica                          @3  :MonicaModelState;
    smPs                            @4  :ModuleParameters;
    envPs                           @5  :EnvironmentParameters;
    cropPs                          @6  :CropModuleState.ModuleParameters;
	
    numberOfLayers                  @7  :UInt16;
	vsNumberOfLayers                @8  :UInt16;

    actualEvaporation               @9  :Float64;               # Sum of evaporation of all layers [mm]
    actualEvapotranspiration        @10 :Float64;               # Sum of evaporation and transpiration of all layers [mm]
    actualTranspiration             @11 :Float64;               # Sum of transpiration of all layers [mm]
    availableWater                  @12 :List(Float64);         # Soil available water in [mm]
    capillaryRise                   @13 :Float64;               # Capillary rise [mm]
    capillaryRiseRate               @14 :List(Float64);         # Capillary rise rate from database in dependence of groundwater distance and texture [m d-1]
    capillaryWater                  @15 :List(Float64);         # soil capillary water in [mm]
    capillaryWater70                @16 :List(Float64);         # 70% of soil capillary water in [mm]
    evaporation                     @17 :List(Float64);         # Evaporation of layer [mm]
    evapotranspiration              @18 :List(Float64);         # Evapotranspiration of layer [mm]
    fieldCapacity                   @19 :List(Float64);         # Soil water content at Field Capacity
    fluxAtLowerBoundary             @20 :Float64;               # Water flux out of bottom layer [mm]

    gravitationalWater              @21 :List(Float64);         # Soil water content drained by gravitation only [mm]
    grossPrecipitation              @22 :Float64;               # Precipitation amount that falls on soil and vegetation [mm]
    groundwaterAdded                @23 :Float64;
    groundwaterDischarge            @24 :Float64;
    groundwaterTable                @25 :UInt16;                # Layer of groundwater table []
    heatConductivity                @26 :List(Float64);         # Heat conductivity of layer [J m-1 d-1]
    hydraulicConductivityRedux      @27 :Float64;               # Reduction factor for hydraulic conductivity [0.1 - 9.9]
    infiltration                    @28 :Float64;               # Amount of water that infiltrates into top soil layer [mm]
    interception                    @29 :Float64;               # [mm], water that is intercepted by the crop and evaporates from it's surface; not accountable for soil water budget
    vcKcFactor                      @30 :Float64 = 0.6;
    lambda                          @31 :List(Float64);         # Empirical soil water conductivity parameter []
    lambdaReduced                   @32 :Float64;
    vsLatitude                      @33 :Float64;
    layerThickness                  @34 :List(Float64);
    pmLayerThickness                @35 :Float64;
    pmLeachingDepth                 @36 :Float64;
    pmLeachingDepthLayer            @37 :UInt16;
    vwMaxAirTemperature             @38 :Float64;               # [°C]
    pmMaxPercolationRate            @39 :Float64;               # [mm d-1]
    vwMeanAirTemperature            @40 :Float64;               # [°C]
    vwMinAirTemperature             @41 :Float64;               # [°C]
    vcNetPrecipitation              @42 :Float64;               # Precipitation amount that is not intercepted by vegetation [mm]
    vwNetRadiation                  @43 :Float64;               # [MJ m-2]
    permanentWiltingPoint           @44 :List(Float64);         # Soil water content at permanent wilting point [m3 m-3]
    vcPercentageSoilCoverage        @45 :Float64;               # [m2 m-2]
    percolationRate                 @46 :List(Float64);         # Percolation rate per layer [mm d-1]
    vwPrecipitation                 @47 :Float64;               # Precipition taken from weather data [mm]
    referenceEvapotranspiration     @48 :Float64 = 6.0;         # Evapotranspiration of a 12mm cut grass crop at sufficient water supply [mm]
    relativeHumidity                @49 :Float64;               # [m3 m-3]
    residualEvapotranspiration      @50 :List(Float64);         # Residual evapotranspiration in [mm]
    saturatedHydraulicConductivity  @51 :List(Float64);         # Saturated hydraulic conductivity [mm d-1]

    soilMoisture                    @52 :List(Float64);         # Result - Soil moisture of layer [m3 m-3]
    soilMoisture_crit               @53 :Float64;
    soilMoistureDeficit             @54 :Float64;               # Soil moisture deficit [m3 m-3]
    soilPoreVolume                  @55 :List(Float64);         # Total soil pore volume [m3]; same as vs_Saturation
    vcStomataResistance             @56 :Float64;
    surfaceRoughness                @57 :Float64;               # Average amplitude of surface micro-elevations to hold back water in ponds [m]
    surfaceRunOff                   @58 :Float64;               # Amount of water running off on soil surface [mm]
    sumSurfaceRunOff                @59 :Float64;               # internal accumulation variable
    surfaceWaterStorage             @60 :Float64;               #  Simulates a virtual layer that contains the surface water [mm]
    ptTimeStep                      @61 :Float64;
    totalWaterRemoval               @62 :Float64;               # Total water removal of layer [m3]
    transpiration                   @63 :List(Float64);         # Transpiration of layer [mm]
    transpirationDeficit            @64 :Float64;
    waterFlux                       @65 :List(Float64);         # Soil water flux at the layer's upper boundary[mm d-1]
    vwWindSpeed                     @66 :Float64;               # [m s-1]
    vwWindSpeedHeight               @67 :Float64;               # [m]
    xSACriticalSoilMoisture         @68 :Float64;

    snowComponent                   @69 :SnowComponentState;
    frostComponent                  @70 :FrostComponentState;
    cropModule                      @71 :CropModuleState;
}

struct SoilTransportModuleState {
    struct ModuleParameters {
        dispersionLength                @0 :Float64;
		AD                              @1 :Float64;
		diffusionCoefficientStandard    @2 :Float64;
		NDeposition                     @3 :Float64;
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
	NH4         @3 :Float64; # [%]
	NO3         @4 :Float64; # [%]
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
    smPs                            @1  :SoilMoisture.ModuleParameters;
    envPs                           @2  :EnvironmentParameters;
    cropPs                          @3  :CropGrowth.ModuleParameters;
    soilTempPs                      @4  :SoilTemperature.ModuleParameters;
    soilTransPs                     @5  :SoilTransport.ModuleParameters;
    soilOrganicPs                   @6  :SoilOrganic.ModuleParameters;
    simPs                           @7  :SimulationParameters;

    groundwaterInformation          @8  :MeasuredGroundwaterTableInformation;

    soilColumn                      @9  :SoilColumn; # main soil data structure
    soilTemperature                 @10 :SoilTemperature; # temperature code
    soilMoisture                    @11 :SoilMoisture; # moisture code
    soilOrganic                     @12 :SoilOrganic; # organic code
    soilTransport                   @13 :SoilTransport; # transport code
    currentCrop                     @14 :Crop; # currently possibly planted crop
    currentCropGrowth               @15 :CropGrowth; # crop code for possibly planted crop

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