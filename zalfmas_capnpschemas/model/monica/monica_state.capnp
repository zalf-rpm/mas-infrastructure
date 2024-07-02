@0x86ea47c297746539;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::model::monica");

using Go = import "/capnp/go.capnp";
$Go.package("monica");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/model/monica");

using Date = import "/date.capnp".Date;
using Params = import "monica_params.capnp";
using Mgmt = import "monica_management.capnp";

struct MaybeBool {
    value @0 :Bool;
}

struct RuntimeState {
    modelState  @0 :MonicaModelState;
    #critPos     @1 :UInt16; # crop rotation iterator position
    #cmitPos     @2 :UInt16; # cultivation method iterator position
}

struct CropState {
    speciesName             @1  :Text;
    cultivarName            @2  :Text;
	seedDate                @3  :Date;
	harvestDate             @4  :Date;
    isWinterCrop            @5  :MaybeBool;
	isPerennialCrop         @6  :MaybeBool;
	cuttingDates            @7  :List(Date);
    cropParams              @8  :Params.CropParameters;
    perennialCropParams     @9  :Params.CropParameters;
    residueParams           @10 :Params.CropResidueParameters;
    crossCropAdaptionFactor @11 :Float64 = 1.0;
    automaticHarvest        @12 :Bool;
	automaticHarvestParams  @0  :Params.AutomaticHarvestParameters;
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
	noVolatilization        @20 :Bool = true;   # true means it's a crop residue and won't participate in vo_volatilisation()
}



struct SoilColumnState {
    struct DelayedNMinApplicationParams {
      fp                        @0 :Mgmt.Params.MineralFertilization.Parameters;
      samplingDepth             @1 :Float64;
      cropNTarget               @2 :Float64;
      cropNTarget30             @3 :Float64;
      fertiliserMinApplication  @4 :Float64;
      fertiliserMaxApplication  @5 :Float64;
      topDressingDelay          @6 :Float64;
    }

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
    vfTopDressingPartition      @10 :Mgmt.Params.MineralFertilization.Parameters;
    vfTopDressingDelay          @11 :UInt16;
    cropModule                  @12 :CropModuleState;
    delayedNMinApplications     @13 :List(DelayedNMinApplicationParams);
    pmCriticalMoistureDepth     @14 :Float64;

    layers                      @15 :List(SoilLayerState);
}

struct SoilLayerState {
    layerThickness      @0  :Float64 = 0.1;                 # Soil layer's vertical extension [m]
    soilWaterFlux       @1  :Float64;                       # Water flux at the upper boundary of the soil layer [l m-2]

    voAOMPool           @2  :List(AOMProperties);   # List of different added organic matter pools in soil layer

    somSlow             @3  :Float64;                       # C content of soil organic matter slow pool [kg C m-3]
    somFast             @4  :Float64;                       # content of soil organic matter fast pool size [kg C m-3]
    smbSlow             @5  :Float64;                       # C content of soil microbial biomass slow pool size [kg C m-3]
    smbFast             @6  :Float64;                       # C content of soil microbial biomass fast pool size [kg C m-3]

    # anorganische Stickstoff-Formen
    soilCarbamid        @7  :Float64;                       # Soil layer's carbamide-N content [kg Carbamide-N m-3]
    soilNH4             @8  :Float64 = 0.0001;              # Soil layer's NH4-N content [kg NH4-N m-3]
    soilNO2             @9  :Float64 = 0.001;               # Soil layer's NO2-N content [kg NO2-N m-3]
    soilNO3             @10 :Float64 = 0.0001;              # Soil layer's NO3-N content [kg NO3-N m-3]
    soilFrozen          @11 :Bool = false;

    sps                 @12 :Params.SoilParameters;
    soilMoistureM3      @13 :Float64 = 0.25;                # Soil layer's moisture content [m3 m-3]
    soilTemperature     @14 :Float64;                       # Soil layer's temperature [°C]
}

struct MonicaModelState {
    sitePs                          @0  :Params.SiteParameters;
    envPs                           @2  :Params.EnvironmentParameters;
    cropPs                          @3  :Params.CropModuleParameters;
    simPs                           @7  :Params.SimulationParameters;

    groundwaterInformation          @8  :Params.MeasuredGroundwaterTableInformation;

    soilColumn                      @9  :SoilColumnState; # main soil data structure
    soilTemperature                 @10 :SoilTemperatureModuleState; # temperature code
    soilMoisture                    @11 :SoilMoistureModuleState; # moisture code
    soilOrganic                     @12 :SoilOrganicModuleState; # organic code
    soilTransport                   @13 :SoilTransportModuleState; # transport code
    currentCropModule               @15 :CropModuleState; # crop code for possibly planted crop

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
    accuOxygenStress                @14 :Float64;

    vwAtmosphericCO2Concentration   @6  :Float64;
    vwAtmosphericO3Concentration    @5  :Float64;
    vsGroundwaterDepth              @4  :Float64;

    cultivationMethodCount          @1  :UInt16;
}

struct CropModuleState {
    frostKillOn                                 @0      :Bool;      
    speciesParams                               @228    :Params.SpeciesParameters;
    cultivarParams                              @229    :Params.CultivarParameters;
    residueParams                               @230    :Params.CropResidueParameters;
    isWinterCrop                                @231    :Bool;
    vsLatitude                                  @6      :Float64;  
    abovegroundBiomass                          @7      :Float64; # old OBMAS
    abovegroundBiomassOld                       @8      :Float64; # old OBALT
    pcAbovegroundOrgan                            @9    :List(Bool); # old KOMP
    actualTranspiration                         @10     :Float64; 
    pcAssimilatePartitioningCoeff                 @11   :List(List(Float64)); # old PRO
    pcAssimilateReallocation                      @12   :Float64; 
    assimilates                                 @13     :Float64; 
    assimilationRate                            @14     :Float64; # old AMAX
    astronomicDayLenght                         @15     :Float64; # old DL
    pcBaseDaylength                               @16   :List(Float64); # old DLBAS
    pcBaseTemperature                             @17   :List(Float64); # old BAS
    pcBeginSensitivePhaseHeatStress               @18   :Float64;
    belowgroundBiomass                          @19     :Float64; 
    belowgroundBiomassOld                       @20     :Float64;
    pcCarboxylationPathway                        @21   :Int64; # old TEMPTYP
    clearDayRadiation                           @22     :Float64; # old DRC
    pcCo2Method                                   @23   :UInt8 = 3;
    criticalNConcentration                      @24     :Float64; # old GEHMIN
    pcCriticalOxygenContent                       @25   :List(Float64); # old LUKRIT
    pcCriticalTemperatureHeatStress               @26   :Float64; 
    cropDiameter                                @27     :Float64;
    cropFrostRedux                              @28     :Float64 = 1.0;
    cropHeatRedux                               @29     :Float64 = 1.0;
    cropHeight                                  @30     :Float64;
    pcCropHeightP1                                @31   :Float64;
    pcCropHeightP2                                @32   :Float64;
    pcCropName                                    @33   :Text; # old FRUCHT$(AKF)
    cropNDemand                                 @34     :Float64; # old DTGESN
    cropNRedux                                  @35     :Float64 = 1.0; # old REDUK
    pcCropSpecificMaxRootingDepth                 @36   :Float64; # old WUMAXPF [m]
    cropWaterUptake                             @37     :List(Float64); # old TP
    currentTemperatureSum                       @38     :List(Float64); # old SUM
    currentTotalTemperatureSum                  @39     :Float64; # old FP
    currentTotalTemperatureSumRoot              @40     :Float64;
    pcCuttingDelayDays                          @41     :UInt16;
    daylengthFactor                             @42     :Float64; # old DAYL
    pcDaylengthRequirement                        @43   :List(Float64); # old DEC
    daysAfterBeginFlowering                     @44     :UInt16;
    declination                                 @45     :Float64; # old EFF0
    pcDefaultRadiationUseEfficiency               @46   :Float64;
    vmDepthGroundwaterTable                     @47     :UInt16; # old GRW
    pcDevelopmentAccelerationByNitrogenStress     @48   :UInt64;
    developmentalStage                          @49     :UInt16; # old INTWICK
    noOfCropSteps                               @50     :UInt16; 
    droughtImpactOnFertility                    @51     :Float64 = 1.0;
    pcDroughtImpactOnFertilityFactor              @52   :Float64;
    pcDroughtStressThreshold                      @53   :List(Float64); # old DRYswell
    pcEmergenceFloodingControlOn                  @54   :Bool = false;
    pcEmergenceMoistureControlOn                  @55   :Bool = false;
    pcEndSensitivePhaseHeatStress                 @56   :Float64;
    effectiveDayLength                          @57     :Float64; # old DLE
    errorStatus                                 @58     :Bool = false;
    errorMessage                                @59     :Text;
    evaporatedFromIntercept                     @60     :Float64;
    extraterrestrialRadiation                   @61     :Float64;
    pcFieldConditionModifier                      @62   :Float64;
    finalDevelopmentalStage                     @63     :UInt16;
    fixedN                                      @64     :Float64;
    #std::vector<double> vo_FreshSoilOrganicMatter @16 :List(Float64); # old NFOS
    pcFrostDehardening                            @65   :Float64;
    pcFrostHardening                              @66   :Float64;
    globalRadiation                             @67     :Float64;
    greenAreaIndex                              @68     :Float64;
    grossAssimilates                            @69     :Float64;
    grossPhotosynthesis                         @70     :Float64; # old GPHOT
    grossPhotosynthesisMol                      @71     :Float64;
    grossPhotosynthesisReferenceMol             @72     :Float64;
    grossPrimaryProduction                      @73     :Float64;
    growthCycleEnded                            @74     :Bool = false;
    growthRespirationAS                         @75     :Float64 = 0.0;
    pcHeatSumIrrigationStart                      @76   :Float64;
    pcHeatSumIrrigationEnd                        @77   :Float64;
    vsHeightNN                                  @78     :Float64;
    pcInitialKcFactor                             @79   :Float64; # old Kcini
    pcInitialOrganBiomass                         @80   :List(Float64);
    pcInitialRootingDepth                         @81   :Float64;
    interceptionStorage                         @82     :Float64; 
    kcFactor                                    @83     :Float64 = 0.6; # old FKc
    leafAreaIndex                               @84     :Float64; # old LAI
    sunlitLeafAreaIndex                         @85     :List(Float64);
    shadedLeafAreaIndex                         @86     :List(Float64);
    pcLowTemperatureExposure                      @87   :Float64;
    pcLimitingTemperatureHeatStress               @88   :Float64;
    lt50                                        @89     :Float64 = -3.0;
    lt50m                                       @233    :Float64 = -3.0;
    pcLt50cultivar                                @90   :Float64;
    pcLuxuryNCoeff                                @91   :Float64;
    maintenanceRespirationAS                    @92     :Float64;
    pcMaxAssimilationRate                         @93   :Float64; # old MAXAMAX
    pcMaxCropDiameter                             @94   :Float64;
    pcMaxCropHeight                               @95   :Float64;
    maxNUptake                                  @96     :Float64; # old MAXUP
    pcMaxNUptakeParam                             @97   :Float64;
    pcMaxRootingDepth                             @98   :Float64; # old WURM
    pcMinimumNConcentration                       @99   :Float64;
    pcMinimumTemperatureForAssimilation           @100  :Float64; # old MINTMP
    pcOptimumTemperatureForAssimilation           @101  :Float64;
    pcMaximumTemperatureForAssimilation           @102  :Float64;
    pcMinimumTemperatureRootGrowth                @103  :Float64;
    netMaintenanceRespiration                   @104    :Float64; # old MAINT
    netPhotosynthesis                           @105    :Float64; # old GTW
    netPrecipitation                            @106    :Float64;
    netPrimaryProduction                        @107    :Float64;
    pcNConcentrationAbovegroundBiomass          @108    :Float64; # initial value of old GEHOB
    nConcentrationAbovegroundBiomass            @109    :Float64; # old GEHOB
    nConcentrationAbovegroundBiomassOld         @110    :Float64; # old GEHALT
    pcNConcentrationB0                            @111  :Float64;
    nContentDeficit                             @112    :Float64;
    pcNConcentrationPN                            @113  :Float64; 
    pcNConcentrationRoot                        @114    :Float64; # initial value to WUGEH
    nConcentrationRoot                          @115    :Float64; # old WUGEH
    nConcentrationRootOld                       @116    :Float64; # old
    pcNitrogenResponseOn                          @117  :Bool; 
    pcNumberOfDevelopmentalStages                 @118  :Float64;
    pcNumberOfOrgans                              @119  :Float64; # old NRKOM
    nUptakeFromLayer                            @120    :List(Float64); # old PE
    pcOptimumTemperature                          @121  :List(Float64);
    organBiomass                                @122    :List(Float64); # old WORG
    organDeadBiomass                            @123    :List(Float64); # old WDORG
    organGreenBiomass                           @124    :List(Float64);
    organGrowthIncrement                        @125    :List(Float64); # old GORG
    pcOrganGrowthRespiration                      @126  :List(Float64); # old MAIRT
    pcOrganIdsForPrimaryYield                     @127  :List(Params.YieldComponent);
    pcOrganIdsForSecondaryYield                   @128  :List(Params.YieldComponent);
    pcOrganIdsForCutting                          @129  :List(Params.YieldComponent);
    pcOrganMaintenanceRespiration                 @130  :List(Float64); # old MAIRT
    organSenescenceIncrement                    @131    :List(Float64); # old DGORG
    pcOrganSenescenceRate                         @132  :List(List(Float64)); # old DEAD
    overcastDayRadiation                        @133    :Float64; # old DRO
    oxygenDeficit                               @134    :Float64; # old LURED
    pcPartBiologicalNFixation                     @135  :Float64;
    pcPerennial                                   @136  :Bool;
    photoperiodicDaylength                      @137    :Float64; # old DLP
    photActRadiationMean                        @138    :Float64; # old RDN
    pcPlantDensity                                @139  :Float64; 
    potentialTranspiration                      @140    :Float64; 
    referenceEvapotranspiration                 @141    :Float64; 
    relativeTotalDevelopment                    @142    :Float64; 
    remainingEvapotranspiration                 @143    :Float64; 
    reserveAssimilatePool                       @144    :Float64; # old ASPOO
    pcResidueNRatio                               @145  :Float64; 
    pcRespiratoryStress                           @146  :Float64; 
    rootBiomass                                 @147    :Float64; # old WUMAS
    rootBiomassOld                              @148    :Float64; # old WUMALT
    rootDensity                                 @149    :List(Float64); # old WUDICH
    rootDiameter                                @150    :List(Float64); # old WRAD
    pcRootDistributionParam                       @151  :Float64;
    rootEffectivity                             @152    :List(Float64); # old WUEFF
    pcRootFormFactor                              @153  :Float64;
    pcRootGrowthLag                               @154  :Float64;
    rootingDepth                                @155    :UInt16; # old WURZ
    rootingDepthM                               @156    :Float64;
    rootingZone                                 @157    :UInt16;
    pcRootPenetrationRate                         @158  :Float64;
    vmSaturationDeficit                         @159    :Float64;
    soilCoverage                                @160    :Float64;
    vsSoilMineralNContent                       @161    :List(Float64); # old C1
    soilSpecificMaxRootingDepth                 @162    :Float64; # old WURZMAX [m]
    vsSoilSpecificMaxRootingDepth               @163    :Float64;
    pcSpecificLeafArea                            @164  :List(Float64); # old LAIFKT [ha kg-1]
    pcSpecificRootLength                          @165  :Float64;
    pcStageAfterCut                               @166  :UInt16; # //0-indexed
    pcStageAtMaxDiameter                          @167  :Float64;
    pcStageAtMaxHeight                            @168  :Float64;
    pcStageMaxRootNConcentration                  @169  :List(Float64); # old WGMAX
    pcStageKcFactor                               @170  :List(Float64); # old Kc
    pcStageTemperatureSum                         @171  :List(Float64); # old TSUM
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
    pcVernalisationRequirement                    @195  :List(Float64); # old VSCHWELL
    pcWaterDeficitResponseOn                      @196  :Bool;

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
    rad240                                      @217    :List(Float64);  
    tfol24                                      @218    :List(Float64); 
    tfol240                                     @219    :List(Float64); 
    index24                                     @220    :UInt16; 
    index240                                    @221    :UInt16;
    full24                                      @222    :Bool; 
    full240                                     @223    :Bool;

    guentherEmissions                           @224    :Params.Voc.Emissions;
    jjvEmissions                                @225    :Params.Voc.Emissions;
    vocSpecies                                  @226    :Params.Voc.SpeciesData;
    cropPhotosynthesisResults                   @227    :Params.Voc.CPData;

    o3ShortTermDamage                           @199    :Float64 = 1.0;
    o3LongTermDamage                            @198    :Float64 = 1.0;
    o3Senescence                                @197    :Float64 = 1.0;
    o3SumUptake                                 @5      :Float64;
    o3WStomatalClosure                          @4      :Float64 = 1.0;

    assimilatePartCoeffsReduced                 @3      :Bool;
    ktkc                                        @2      :Float64; # old KTkc
    ktko                                        @1      :Float64; # old KTko

    stemElongationEventFired                    @232    :Bool;
}

struct SnowModuleState {
    snowDensity                             @1  :Float64;   # Snow density [kg dm-3]
    snowDepth                               @2  :Float64;   # Snow depth [mm]
    frozenWaterInSnow                       @3  :Float64;   # [mm]
    liquidWaterInSnow                       @4  :Float64;   # [mm]
    waterToInfiltrate                       @5  :Float64;   # [mm]
    maxSnowDepth                            @6  :Float64;   # [mm]
    accumulatedSnowDepth                    @7  :Float64;   # [mm]
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
    snowRetentionCapacityMax                @0  :Float64;  
}

struct FrostModuleState {
    frostDepth                      @1  :Float64;
    accumulatedFrostDepth           @2  :Float64;
    negativeDegreeDays              @3  :Float64;       # Counts negative degree-days under snow
    thawDepth                       @4  :Float64;
    frostDays                       @5  :UInt16;
    lambdaRedux                     @6  :List(Float64); # Reduction factor for Lambda []
    temperatureUnderSnow            @7  :Float64;
    hydraulicConductivityRedux      @8  :Float64;
    ptTimeStep                      @9  :Float64;
    pmHydraulicConductivityRedux    @0  :Float64;
}

struct SoilMoistureModuleState {
    moduleParams                    @3  :Params.SoilMoistureModuleParameters;
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
    vwWindSpeed                     @5  :Float64;               # [m s-1]
    vwWindSpeedHeight               @4  :Float64;               # [m]
    xSACriticalSoilMoisture         @2  :Float64;
    snowComponent                   @1  :SnowModuleState;
    frostComponent                  @0  :FrostModuleState;
}

struct SoilOrganicModuleState {
    moduleParams                        @2  :Params.SoilOrganicModuleParameters;
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
    totalDenitrification                @1 :Float64;
    incorporation                       @0 :Bool;
}

struct SoilTemperatureModuleState {
    soilSurfaceTemperature      @0  :Float64;
    soilColumnVtGroundLayer     @3  :SoilLayerState;
    soilColumnVtBottomLayer     @4  :SoilLayerState;
    moduleParams                @1  :Params.SoilTemperatureModuleParameters;
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
    dampingFactor               @2 :Float64 = 0.8;
}

struct SoilTransportModuleState {
    moduleParams            @1  :Params.SoilTransportModuleParameters;
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
    pcMinimumAvailableN     @0 :Float64;       # kg m-2
}

struct ICData {
    # intercropping data
    union {
        noCrop          @0 :Void;
        height          @1 :Float64;
        lait            @2 :Float64;
    }
}

