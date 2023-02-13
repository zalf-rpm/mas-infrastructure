# model/monica/monica_state.capnp
@0x86ea47c297746539;
$import "/capnp/c++.capnp".namespace("mas::schema::model::monica");
$import "/capnp/go.capnp".package("monica");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/model/monica");
struct MaybeBool @0xd8af9210839bc071 {  # 8 bytes, 0 ptrs
  value @0 :Bool;  # bits[0, 1)
}
struct RuntimeState @0xd599d06dc405571a {  # 0 bytes, 1 ptrs
  modelState @0 :MonicaModelState;  # ptr[0]
}
struct CropState @0x8b008567c93f7c7d {  # 16 bytes, 11 ptrs
  speciesName @1 :Text;  # ptr[1]
  cultivarName @2 :Text;  # ptr[2]
  seedDate @3 :import "/date.capnp".Date;  # ptr[3]
  harvestDate @4 :import "/date.capnp".Date;  # ptr[4]
  isWinterCrop @5 :MaybeBool;  # ptr[5]
  isPerennialCrop @6 :MaybeBool;  # ptr[6]
  cuttingDates @7 :List(import "/date.capnp".Date);  # ptr[7]
  cropParams @8 :import "/model/monica/monica_params.capnp".CropParameters;  # ptr[8]
  perennialCropParams @9 :import "/model/monica/monica_params.capnp".CropParameters;  # ptr[9]
  residueParams @10 :import "/model/monica/monica_params.capnp".CropResidueParameters;  # ptr[10]
  crossCropAdaptionFactor @11 :Float64 = 1;  # bits[0, 64)
  automaticHarvest @12 :Bool;  # bits[64, 65)
  automaticHarvestParams @0 :import "/model/monica/monica_params.capnp".AutomaticHarvestParameters;  # ptr[0]
}
struct AOMProperties @0xe3512e62df901c18 {  # 152 bytes, 0 ptrs
  aomSlow @0 :Float64;  # bits[0, 64)
  aomFast @1 :Float64;  # bits[64, 128)
  aomSlowDecRatetoSMBSlow @2 :Float64;  # bits[128, 192)
  aomSlowDecRatetoSMBFast @3 :Float64;  # bits[192, 256)
  aomFastDecRatetoSMBSlow @4 :Float64;  # bits[256, 320)
  aomFastDecRatetoSMBFast @5 :Float64;  # bits[320, 384)
  aomSlowDecCoeff @6 :Float64;  # bits[384, 448)
  aomFastDecCoeff @7 :Float64;  # bits[448, 512)
  aomSlowDecCoeffStandard @8 :Float64 = 1;  # bits[512, 576)
  aomFastDecCoeffStandard @9 :Float64 = 1;  # bits[576, 640)
  partAOMSlowtoSMBSlow @10 :Float64;  # bits[640, 704)
  partAOMSlowtoSMBFast @11 :Float64;  # bits[704, 768)
  cnRatioAOMSlow @12 :Float64 = 1;  # bits[768, 832)
  cnRatioAOMFast @13 :Float64 = 1;  # bits[832, 896)
  daysAfterApplication @14 :UInt16;  # bits[896, 912)
  aomDryMatterContent @15 :Float64;  # bits[960, 1024)
  aomNH4Content @16 :Float64;  # bits[1024, 1088)
  aomSlowDelta @17 :Float64;  # bits[1088, 1152)
  aomFastDelta @18 :Float64;  # bits[1152, 1216)
  incorporation @19 :Bool;  # bits[912, 913)
  noVolatilization @20 :Bool = true;  # bits[913, 914)
}
struct SoilColumnState @0xef3e4198d3e35596 {  # 88 bytes, 4 ptrs
  vsSurfaceWaterStorage @0 :Float64;  # bits[0, 64)
  vsInterceptionStorage @1 :Float64;  # bits[64, 128)
  vmGroundwaterTable @2 :UInt16;  # bits[128, 144)
  vsFluxAtLowerBoundary @3 :Float64;  # bits[192, 256)
  vqCropNUptake @4 :Float64;  # bits[256, 320)
  vtSoilSurfaceTemperature @5 :Float64;  # bits[320, 384)
  vmSnowDepth @6 :Float64;  # bits[384, 448)
  psMaxMineralisationDepth @7 :Float64 = 0.4;  # bits[448, 512)
  vsNumberOfOrganicLayers @8 :Float64;  # bits[512, 576)
  vfTopDressing @9 :Float64;  # bits[576, 640)
  vfTopDressingPartition @10 :import "/model/monica/monica_management.capnp".Params.MineralFertilization.Parameters;  # ptr[0]
  vfTopDressingDelay @11 :UInt16;  # bits[144, 160)
  cropModule @12 :CropModuleState;  # ptr[1]
  delayedNMinApplications @13 :List(DelayedNMinApplicationParams);  # ptr[2]
  pmCriticalMoistureDepth @14 :Float64;  # bits[640, 704)
  layers @15 :List(SoilLayerState);  # ptr[3]
  struct DelayedNMinApplicationParams @0xd1edcf54f4edf638 {  # 48 bytes, 1 ptrs
    fp @0 :import "/model/monica/monica_management.capnp".Params.MineralFertilization.Parameters;  # ptr[0]
    samplingDepth @1 :Float64;  # bits[0, 64)
    cropNTarget @2 :Float64;  # bits[64, 128)
    cropNTarget30 @3 :Float64;  # bits[128, 192)
    fertiliserMinApplication @4 :Float64;  # bits[192, 256)
    fertiliserMaxApplication @5 :Float64;  # bits[256, 320)
    topDressingDelay @6 :Float64;  # bits[320, 384)
  }
}
struct SoilLayerState @0xdd1e0c7c94dc4211 {  # 104 bytes, 2 ptrs
  layerThickness @0 :Float64 = 0.1;  # bits[0, 64)
  soilWaterFlux @1 :Float64;  # bits[64, 128)
  voAOMPool @2 :List(AOMProperties);  # ptr[0]
  somSlow @3 :Float64;  # bits[128, 192)
  somFast @4 :Float64;  # bits[192, 256)
  smbSlow @5 :Float64;  # bits[256, 320)
  smbFast @6 :Float64;  # bits[320, 384)
  soilCarbamid @7 :Float64;  # bits[384, 448)
  soilNH4 @8 :Float64 = 0.0001;  # bits[448, 512)
  soilNO2 @9 :Float64 = 0.001;  # bits[512, 576)
  soilNO3 @10 :Float64 = 0.0001;  # bits[576, 640)
  soilFrozen @11 :Bool;  # bits[640, 641)
  sps @12 :import "/model/monica/monica_params.capnp".SoilParameters;  # ptr[1]
  soilMoistureM3 @13 :Float64 = 0.25;  # bits[704, 768)
  soilTemperature @14 :Float64;  # bits[768, 832)
}
struct MonicaModelState @0xab56969492d293b3 {  # 144 bytes, 15 ptrs
  sitePs @0 :import "/model/monica/monica_params.capnp".SiteParameters;  # ptr[0]
  envPs @2 :import "/model/monica/monica_params.capnp".EnvironmentParameters;  # ptr[1]
  cropPs @3 :import "/model/monica/monica_params.capnp".CropModuleParameters;  # ptr[2]
  simPs @7 :import "/model/monica/monica_params.capnp".SimulationParameters;  # ptr[3]
  groundwaterInformation @8 :import "/model/monica/monica_params.capnp".MeasuredGroundwaterTableInformation;  # ptr[4]
  soilColumn @9 :SoilColumnState;  # ptr[5]
  soilTemperature @10 :SoilTemperatureModuleState;  # ptr[6]
  soilMoisture @11 :SoilMoistureModuleState;  # ptr[7]
  soilOrganic @12 :SoilOrganicModuleState;  # ptr[8]
  soilTransport @13 :SoilTransportModuleState;  # ptr[9]
  currentCropModule @15 :CropModuleState;  # ptr[10]
  sumFertiliser @16 :Float64;  # bits[320, 384)
  sumOrgFertiliser @17 :Float64;  # bits[384, 448)
  dailySumFertiliser @18 :Float64;  # bits[448, 512)
  dailySumOrgFertiliser @19 :Float64;  # bits[512, 576)
  dailySumOrganicFertilizerDM @20 :Float64;  # bits[576, 640)
  sumOrganicFertilizerDM @21 :Float64;  # bits[640, 704)
  humusBalanceCarryOver @22 :Float64;  # bits[704, 768)
  dailySumIrrigationWater @23 :Float64;  # bits[768, 832)
  optCarbonExportedResidues @24 :Float64;  # bits[832, 896)
  optCarbonReturnedResidues @25 :Float64;  # bits[896, 960)
  currentStepDate @26 :import "/date.capnp".Date;  # ptr[11]
  climateData @27 :List(List(ACDToValue));  # ptr[12]
  currentEvents @28 :List(Text);  # ptr[13]
  previousDaysEvents @29 :List(Text);  # ptr[14]
  clearCropUponNextDay @30 :Bool;  # bits[16, 17)
  daysWithCrop @31 :UInt16;  # bits[32, 48)
  accuNStress @32 :Float64;  # bits[960, 1024)
  accuWaterStress @33 :Float64;  # bits[1024, 1088)
  accuHeatStress @34 :Float64;  # bits[1088, 1152)
  accuOxygenStress @14 :Float64;  # bits[256, 320)
  vwAtmosphericCO2Concentration @6 :Float64;  # bits[192, 256)
  vwAtmosphericO3Concentration @5 :Float64;  # bits[128, 192)
  vsGroundwaterDepth @4 :Float64;  # bits[64, 128)
  cultivationMethodCount @1 :UInt16;  # bits[0, 16)
  struct ACDToValue @0x98e203c76f83d365 {  # 16 bytes, 0 ptrs
    acd @0 :UInt16;  # bits[0, 16)
    value @1 :Float64;  # bits[64, 128)
  }
}
struct CropModuleState @0x811d54ac7debc21e {  # 1248 bytes, 50 ptrs
  frostKillOn @0 :Bool;  # bits[0, 1)
  speciesParams @228 :import "/model/monica/monica_params.capnp".SpeciesParameters;  # ptr[47]
  cultivarParams @229 :import "/model/monica/monica_params.capnp".CultivarParameters;  # ptr[48]
  residueParams @230 :import "/model/monica/monica_params.capnp".CropResidueParameters;  # ptr[49]
  isWinterCrop @231 :Bool;  # bits[7285, 7286)
  vsLatitude @6 :Float64;  # bits[320, 384)
  abovegroundBiomass @7 :Float64;  # bits[384, 448)
  abovegroundBiomassOld @8 :Float64;  # bits[448, 512)
  pcAbovegroundOrgan @9 :List(Bool);  # ptr[0]
  actualTranspiration @10 :Float64;  # bits[512, 576)
  pcAssimilatePartitioningCoeff @11 :List(List(Float64));  # ptr[1]
  pcAssimilateReallocation @12 :Float64;  # bits[576, 640)
  assimilates @13 :Float64;  # bits[640, 704)
  assimilationRate @14 :Float64;  # bits[704, 768)
  astronomicDayLenght @15 :Float64;  # bits[768, 832)
  pcBaseDaylength @16 :List(Float64);  # ptr[2]
  pcBaseTemperature @17 :List(Float64);  # ptr[3]
  pcBeginSensitivePhaseHeatStress @18 :Float64;  # bits[832, 896)
  belowgroundBiomass @19 :Float64;  # bits[896, 960)
  belowgroundBiomassOld @20 :Float64;  # bits[960, 1024)
  pcCarboxylationPathway @21 :Int64;  # bits[1024, 1088)
  clearDayRadiation @22 :Float64;  # bits[1088, 1152)
  pcCo2Method @23 :UInt8 = 3;  # bits[8, 16)
  criticalNConcentration @24 :Float64;  # bits[1152, 1216)
  pcCriticalOxygenContent @25 :List(Float64);  # ptr[4]
  pcCriticalTemperatureHeatStress @26 :Float64;  # bits[1216, 1280)
  cropDiameter @27 :Float64;  # bits[1280, 1344)
  cropFrostRedux @28 :Float64 = 1;  # bits[1344, 1408)
  cropHeatRedux @29 :Float64 = 1;  # bits[1408, 1472)
  cropHeight @30 :Float64;  # bits[1472, 1536)
  pcCropHeightP1 @31 :Float64;  # bits[1536, 1600)
  pcCropHeightP2 @32 :Float64;  # bits[1600, 1664)
  pcCropName @33 :Text;  # ptr[5]
  cropNDemand @34 :Float64;  # bits[1664, 1728)
  cropNRedux @35 :Float64 = 1;  # bits[1728, 1792)
  pcCropSpecificMaxRootingDepth @36 :Float64;  # bits[1792, 1856)
  cropWaterUptake @37 :List(Float64);  # ptr[6]
  currentTemperatureSum @38 :List(Float64);  # ptr[7]
  currentTotalTemperatureSum @39 :Float64;  # bits[1856, 1920)
  currentTotalTemperatureSumRoot @40 :Float64;  # bits[1920, 1984)
  pcCuttingDelayDays @41 :UInt16;  # bits[16, 32)
  daylengthFactor @42 :Float64;  # bits[1984, 2048)
  pcDaylengthRequirement @43 :List(Float64);  # ptr[8]
  daysAfterBeginFlowering @44 :UInt16;  # bits[32, 48)
  declination @45 :Float64;  # bits[2048, 2112)
  pcDefaultRadiationUseEfficiency @46 :Float64;  # bits[2112, 2176)
  vmDepthGroundwaterTable @47 :UInt16;  # bits[48, 64)
  pcDevelopmentAccelerationByNitrogenStress @48 :UInt64;  # bits[2176, 2240)
  developmentalStage @49 :UInt16;  # bits[2240, 2256)
  noOfCropSteps @50 :UInt16;  # bits[2256, 2272)
  droughtImpactOnFertility @51 :Float64 = 1;  # bits[2304, 2368)
  pcDroughtImpactOnFertilityFactor @52 :Float64;  # bits[2368, 2432)
  pcDroughtStressThreshold @53 :List(Float64);  # ptr[9]
  pcEmergenceFloodingControlOn @54 :Bool;  # bits[2, 3)
  pcEmergenceMoistureControlOn @55 :Bool;  # bits[3, 4)
  pcEndSensitivePhaseHeatStress @56 :Float64;  # bits[2432, 2496)
  effectiveDayLength @57 :Float64;  # bits[2496, 2560)
  errorStatus @58 :Bool;  # bits[4, 5)
  errorMessage @59 :Text;  # ptr[10]
  evaporatedFromIntercept @60 :Float64;  # bits[2560, 2624)
  extraterrestrialRadiation @61 :Float64;  # bits[2624, 2688)
  pcFieldConditionModifier @62 :Float64;  # bits[2688, 2752)
  finalDevelopmentalStage @63 :UInt16;  # bits[2272, 2288)
  fixedN @64 :Float64;  # bits[2752, 2816)
  pcFrostDehardening @65 :Float64;  # bits[2816, 2880)
  pcFrostHardening @66 :Float64;  # bits[2880, 2944)
  globalRadiation @67 :Float64;  # bits[2944, 3008)
  greenAreaIndex @68 :Float64;  # bits[3008, 3072)
  grossAssimilates @69 :Float64;  # bits[3072, 3136)
  grossPhotosynthesis @70 :Float64;  # bits[3136, 3200)
  grossPhotosynthesisMol @71 :Float64;  # bits[3200, 3264)
  grossPhotosynthesisReferenceMol @72 :Float64;  # bits[3264, 3328)
  grossPrimaryProduction @73 :Float64;  # bits[3328, 3392)
  growthCycleEnded @74 :Bool;  # bits[5, 6)
  growthRespirationAS @75 :Float64;  # bits[3392, 3456)
  pcHeatSumIrrigationStart @76 :Float64;  # bits[3456, 3520)
  pcHeatSumIrrigationEnd @77 :Float64;  # bits[3520, 3584)
  vsHeightNN @78 :Float64;  # bits[3584, 3648)
  pcInitialKcFactor @79 :Float64;  # bits[3648, 3712)
  pcInitialOrganBiomass @80 :List(Float64);  # ptr[11]
  pcInitialRootingDepth @81 :Float64;  # bits[3712, 3776)
  interceptionStorage @82 :Float64;  # bits[3776, 3840)
  kcFactor @83 :Float64 = 0.6;  # bits[3840, 3904)
  leafAreaIndex @84 :Float64;  # bits[3904, 3968)
  sunlitLeafAreaIndex @85 :List(Float64);  # ptr[12]
  shadedLeafAreaIndex @86 :List(Float64);  # ptr[13]
  pcLowTemperatureExposure @87 :Float64;  # bits[3968, 4032)
  pcLimitingTemperatureHeatStress @88 :Float64;  # bits[4032, 4096)
  lt50 @89 :Float64 = -3;  # bits[4096, 4160)
  lt50m @233 :Float64 = -3;  # bits[9920, 9984)
  pcLt50cultivar @90 :Float64;  # bits[4160, 4224)
  pcLuxuryNCoeff @91 :Float64;  # bits[4224, 4288)
  maintenanceRespirationAS @92 :Float64;  # bits[4288, 4352)
  pcMaxAssimilationRate @93 :Float64;  # bits[4352, 4416)
  pcMaxCropDiameter @94 :Float64;  # bits[4416, 4480)
  pcMaxCropHeight @95 :Float64;  # bits[4480, 4544)
  maxNUptake @96 :Float64;  # bits[4544, 4608)
  pcMaxNUptakeParam @97 :Float64;  # bits[4608, 4672)
  pcMaxRootingDepth @98 :Float64;  # bits[4672, 4736)
  pcMinimumNConcentration @99 :Float64;  # bits[4736, 4800)
  pcMinimumTemperatureForAssimilation @100 :Float64;  # bits[4800, 4864)
  pcOptimumTemperatureForAssimilation @101 :Float64;  # bits[4864, 4928)
  pcMaximumTemperatureForAssimilation @102 :Float64;  # bits[4928, 4992)
  pcMinimumTemperatureRootGrowth @103 :Float64;  # bits[4992, 5056)
  netMaintenanceRespiration @104 :Float64;  # bits[5056, 5120)
  netPhotosynthesis @105 :Float64;  # bits[5120, 5184)
  netPrecipitation @106 :Float64;  # bits[5184, 5248)
  netPrimaryProduction @107 :Float64;  # bits[5248, 5312)
  pcNConcentrationAbovegroundBiomass @108 :Float64;  # bits[5312, 5376)
  nConcentrationAbovegroundBiomass @109 :Float64;  # bits[5376, 5440)
  nConcentrationAbovegroundBiomassOld @110 :Float64;  # bits[5440, 5504)
  pcNConcentrationB0 @111 :Float64;  # bits[5504, 5568)
  nContentDeficit @112 :Float64;  # bits[5568, 5632)
  pcNConcentrationPN @113 :Float64;  # bits[5632, 5696)
  pcNConcentrationRoot @114 :Float64;  # bits[5696, 5760)
  nConcentrationRoot @115 :Float64;  # bits[5760, 5824)
  nConcentrationRootOld @116 :Float64;  # bits[5824, 5888)
  pcNitrogenResponseOn @117 :Bool;  # bits[6, 7)
  pcNumberOfDevelopmentalStages @118 :Float64;  # bits[5888, 5952)
  pcNumberOfOrgans @119 :Float64;  # bits[5952, 6016)
  nUptakeFromLayer @120 :List(Float64);  # ptr[14]
  pcOptimumTemperature @121 :List(Float64);  # ptr[15]
  organBiomass @122 :List(Float64);  # ptr[16]
  organDeadBiomass @123 :List(Float64);  # ptr[17]
  organGreenBiomass @124 :List(Float64);  # ptr[18]
  organGrowthIncrement @125 :List(Float64);  # ptr[19]
  pcOrganGrowthRespiration @126 :List(Float64);  # ptr[20]
  pcOrganIdsForPrimaryYield @127 :List(import "/model/monica/monica_params.capnp".YieldComponent);  # ptr[21]
  pcOrganIdsForSecondaryYield @128 :List(import "/model/monica/monica_params.capnp".YieldComponent);  # ptr[22]
  pcOrganIdsForCutting @129 :List(import "/model/monica/monica_params.capnp".YieldComponent);  # ptr[23]
  pcOrganMaintenanceRespiration @130 :List(Float64);  # ptr[24]
  organSenescenceIncrement @131 :List(Float64);  # ptr[25]
  pcOrganSenescenceRate @132 :List(List(Float64));  # ptr[26]
  overcastDayRadiation @133 :Float64;  # bits[6016, 6080)
  oxygenDeficit @134 :Float64;  # bits[6080, 6144)
  pcPartBiologicalNFixation @135 :Float64;  # bits[6144, 6208)
  pcPerennial @136 :Bool;  # bits[7, 8)
  photoperiodicDaylength @137 :Float64;  # bits[6208, 6272)
  photActRadiationMean @138 :Float64;  # bits[6272, 6336)
  pcPlantDensity @139 :Float64;  # bits[6336, 6400)
  potentialTranspiration @140 :Float64;  # bits[6400, 6464)
  referenceEvapotranspiration @141 :Float64;  # bits[6464, 6528)
  relativeTotalDevelopment @142 :Float64;  # bits[6528, 6592)
  remainingEvapotranspiration @143 :Float64;  # bits[6592, 6656)
  reserveAssimilatePool @144 :Float64;  # bits[6656, 6720)
  pcResidueNRatio @145 :Float64;  # bits[6720, 6784)
  pcRespiratoryStress @146 :Float64;  # bits[6784, 6848)
  rootBiomass @147 :Float64;  # bits[6848, 6912)
  rootBiomassOld @148 :Float64;  # bits[6912, 6976)
  rootDensity @149 :List(Float64);  # ptr[27]
  rootDiameter @150 :List(Float64);  # ptr[28]
  pcRootDistributionParam @151 :Float64;  # bits[6976, 7040)
  rootEffectivity @152 :List(Float64);  # ptr[29]
  pcRootFormFactor @153 :Float64;  # bits[7040, 7104)
  pcRootGrowthLag @154 :Float64;  # bits[7104, 7168)
  rootingDepth @155 :UInt16;  # bits[2288, 2304)
  rootingDepthM @156 :Float64;  # bits[7168, 7232)
  rootingZone @157 :UInt16;  # bits[7232, 7248)
  pcRootPenetrationRate @158 :Float64;  # bits[7296, 7360)
  vmSaturationDeficit @159 :Float64;  # bits[7360, 7424)
  soilCoverage @160 :Float64;  # bits[7424, 7488)
  vsSoilMineralNContent @161 :List(Float64);  # ptr[30]
  soilSpecificMaxRootingDepth @162 :Float64;  # bits[7488, 7552)
  vsSoilSpecificMaxRootingDepth @163 :Float64;  # bits[7552, 7616)
  pcSpecificLeafArea @164 :List(Float64);  # ptr[31]
  pcSpecificRootLength @165 :Float64;  # bits[7616, 7680)
  pcStageAfterCut @166 :UInt16;  # bits[7248, 7264)
  pcStageAtMaxDiameter @167 :Float64;  # bits[7680, 7744)
  pcStageAtMaxHeight @168 :Float64;  # bits[7744, 7808)
  pcStageMaxRootNConcentration @169 :List(Float64);  # ptr[32]
  pcStageKcFactor @170 :List(Float64);  # ptr[33]
  pcStageTemperatureSum @171 :List(Float64);  # ptr[34]
  stomataResistance @172 :Float64;  # bits[7808, 7872)
  pcStorageOrgan @173 :List(Bool);  # ptr[35]
  storageOrgan @174 :UInt16 = 4;  # bits[7264, 7280)
  targetNConcentration @175 :Float64;  # bits[7872, 7936)
  timeStep @176 :Float64 = 1;  # bits[7936, 8000)
  timeUnderAnoxia @177 :UInt64;  # bits[8000, 8064)
  vsTortuosity @178 :Float64;  # bits[8064, 8128)
  totalBiomass @179 :Float64;  # bits[8128, 8192)
  totalBiomassNContent @180 :Float64;  # bits[8192, 8256)
  totalCropHeatImpact @181 :Float64;  # bits[8256, 8320)
  totalNInput @182 :Float64;  # bits[8320, 8384)
  totalNUptake @183 :Float64;  # bits[8384, 8448)
  totalRespired @184 :Float64;  # bits[8448, 8512)
  respiration @185 :Float64;  # bits[8512, 8576)
  sumTotalNUptake @186 :Float64;  # bits[8576, 8640)
  totalRootLength @187 :Float64;  # bits[8640, 8704)
  totalTemperatureSum @188 :Float64;  # bits[8704, 8768)
  temperatureSumToFlowering @189 :Float64;  # bits[8768, 8832)
  transpiration @190 :List(Float64);  # ptr[36]
  transpirationRedux @191 :List(Float64);  # ptr[37]
  transpirationDeficit @192 :Float64 = 1;  # bits[8832, 8896)
  vernalisationDays @193 :Float64;  # bits[8896, 8960)
  vernalisationFactor @194 :Float64;  # bits[8960, 9024)
  pcVernalisationRequirement @195 :List(Float64);  # ptr[38]
  pcWaterDeficitResponseOn @196 :Bool;  # bits[7280, 7281)
  dyingOut @200 :Bool;  # bits[7281, 7282)
  accumulatedETa @201 :Float64;  # bits[9216, 9280)
  accumulatedTranspiration @202 :Float64;  # bits[9280, 9344)
  accumulatedPrimaryCropYield @203 :Float64;  # bits[9344, 9408)
  sumExportedCutBiomass @204 :Float64;  # bits[9408, 9472)
  exportedCutBiomass @205 :Float64;  # bits[9472, 9536)
  sumResidueCutBiomass @206 :Float64;  # bits[9536, 9600)
  residueCutBiomass @207 :Float64;  # bits[9600, 9664)
  cuttingDelayDays @208 :UInt16;  # bits[9664, 9680)
  vsMaxEffectiveRootingDepth @209 :Float64;  # bits[9728, 9792)
  vsImpenetrableLayerDept @210 :Float64;  # bits[9792, 9856)
  anthesisDay @211 :Int16 = -1;  # bits[9680, 9696)
  maturityDay @212 :Int16 = -1;  # bits[9696, 9712)
  maturityReached @213 :Bool;  # bits[7282, 7283)
  stepSize24 @214 :UInt16 = 24;  # bits[9712, 9728)
  stepSize240 @215 :UInt16 = 240;  # bits[9856, 9872)
  rad24 @216 :List(Float64);  # ptr[39]
  rad240 @217 :List(Float64);  # ptr[40]
  tfol24 @218 :List(Float64);  # ptr[41]
  tfol240 @219 :List(Float64);  # ptr[42]
  index24 @220 :UInt16;  # bits[9872, 9888)
  index240 @221 :UInt16;  # bits[9888, 9904)
  full24 @222 :Bool;  # bits[7283, 7284)
  full240 @223 :Bool;  # bits[7284, 7285)
  guentherEmissions @224 :import "/model/monica/monica_params.capnp".Voc.Emissions;  # ptr[43]
  jjvEmissions @225 :import "/model/monica/monica_params.capnp".Voc.Emissions;  # ptr[44]
  vocSpecies @226 :import "/model/monica/monica_params.capnp".Voc.SpeciesData;  # ptr[45]
  cropPhotosynthesisResults @227 :import "/model/monica/monica_params.capnp".Voc.CPData;  # ptr[46]
  o3ShortTermDamage @199 :Float64 = 1;  # bits[9152, 9216)
  o3LongTermDamage @198 :Float64 = 1;  # bits[9088, 9152)
  o3Senescence @197 :Float64 = 1;  # bits[9024, 9088)
  o3SumUptake @5 :Float64;  # bits[256, 320)
  o3WStomatalClosure @4 :Float64 = 1;  # bits[192, 256)
  assimilatePartCoeffsReduced @3 :Bool;  # bits[1, 2)
  ktkc @2 :Float64;  # bits[128, 192)
  ktko @1 :Float64;  # bits[64, 128)
  stemElongationEventFired @232 :Bool;  # bits[7286, 7287)
}
struct SnowModuleState @0xa4da01d10b3b6acd {  # 160 bytes, 0 ptrs
  snowDensity @1 :Float64;  # bits[64, 128)
  snowDepth @2 :Float64;  # bits[128, 192)
  frozenWaterInSnow @3 :Float64;  # bits[192, 256)
  liquidWaterInSnow @4 :Float64;  # bits[256, 320)
  waterToInfiltrate @5 :Float64;  # bits[320, 384)
  maxSnowDepth @6 :Float64;  # bits[384, 448)
  accumulatedSnowDepth @7 :Float64;  # bits[448, 512)
  snowmeltTemperature @8 :Float64;  # bits[512, 576)
  snowAccumulationThresholdTemperature @9 :Float64;  # bits[576, 640)
  temperatureLimitForLiquidWater @10 :Float64;  # bits[640, 704)
  correctionRain @11 :Float64;  # bits[704, 768)
  correctionSnow @12 :Float64;  # bits[768, 832)
  refreezeTemperature @13 :Float64;  # bits[832, 896)
  refreezeP1 @14 :Float64;  # bits[896, 960)
  refreezeP2 @15 :Float64;  # bits[960, 1024)
  newSnowDensityMin @16 :Float64;  # bits[1024, 1088)
  snowMaxAdditionalDensity @17 :Float64;  # bits[1088, 1152)
  snowPacking @18 :Float64;  # bits[1152, 1216)
  snowRetentionCapacityMin @19 :Float64;  # bits[1216, 1280)
  snowRetentionCapacityMax @0 :Float64;  # bits[0, 64)
}
struct FrostModuleState @0xb4f16ea3144d85a6 {  # 72 bytes, 1 ptrs
  frostDepth @1 :Float64;  # bits[64, 128)
  accumulatedFrostDepth @2 :Float64;  # bits[128, 192)
  negativeDegreeDays @3 :Float64;  # bits[192, 256)
  thawDepth @4 :Float64;  # bits[256, 320)
  frostDays @5 :UInt16;  # bits[320, 336)
  lambdaRedux @6 :List(Float64);  # ptr[0]
  temperatureUnderSnow @7 :Float64;  # bits[384, 448)
  hydraulicConductivityRedux @8 :Float64;  # bits[448, 512)
  ptTimeStep @9 :Float64;  # bits[512, 576)
  pmHydraulicConductivityRedux @0 :Float64;  # bits[0, 64)
}
struct SoilMoistureModuleState @0xcd05962719bf7ec8 {  # 320 bytes, 22 ptrs
  moduleParams @3 :import "/model/monica/monica_params.capnp".SoilMoistureModuleParameters;  # ptr[2]
  numberOfLayers @6 :UInt16;  # bits[192, 208)
  vsNumberOfLayers @7 :UInt16;  # bits[208, 224)
  actualEvaporation @8 :Float64;  # bits[256, 320)
  actualEvapotranspiration @9 :Float64;  # bits[320, 384)
  actualTranspiration @10 :Float64;  # bits[384, 448)
  availableWater @11 :List(Float64);  # ptr[3]
  capillaryRise @12 :Float64;  # bits[448, 512)
  capillaryRiseRate @13 :List(Float64);  # ptr[4]
  capillaryWater @14 :List(Float64);  # ptr[5]
  capillaryWater70 @15 :List(Float64);  # ptr[6]
  evaporation @16 :List(Float64);  # ptr[7]
  evapotranspiration @17 :List(Float64);  # ptr[8]
  fieldCapacity @18 :List(Float64);  # ptr[9]
  fluxAtLowerBoundary @19 :Float64;  # bits[512, 576)
  gravitationalWater @20 :List(Float64);  # ptr[10]
  grossPrecipitation @21 :Float64;  # bits[576, 640)
  groundwaterAdded @22 :Float64;  # bits[640, 704)
  groundwaterDischarge @23 :Float64;  # bits[704, 768)
  groundwaterTable @24 :UInt16;  # bits[224, 240)
  heatConductivity @25 :List(Float64);  # ptr[11]
  hydraulicConductivityRedux @26 :Float64;  # bits[768, 832)
  infiltration @27 :Float64;  # bits[832, 896)
  interception @28 :Float64;  # bits[896, 960)
  vcKcFactor @29 :Float64 = 0.6;  # bits[960, 1024)
  lambda @30 :List(Float64);  # ptr[12]
  lambdaReduced @31 :Float64;  # bits[1024, 1088)
  vsLatitude @32 :Float64;  # bits[1088, 1152)
  layerThickness @33 :List(Float64);  # ptr[13]
  pmLayerThickness @34 :Float64;  # bits[1152, 1216)
  pmLeachingDepth @35 :Float64;  # bits[1216, 1280)
  pmLeachingDepthLayer @36 :UInt16;  # bits[240, 256)
  vwMaxAirTemperature @37 :Float64;  # bits[1280, 1344)
  pmMaxPercolationRate @38 :Float64;  # bits[1344, 1408)
  vwMeanAirTemperature @39 :Float64;  # bits[1408, 1472)
  vwMinAirTemperature @40 :Float64;  # bits[1472, 1536)
  vcNetPrecipitation @41 :Float64;  # bits[1536, 1600)
  vwNetRadiation @42 :Float64;  # bits[1600, 1664)
  permanentWiltingPoint @43 :List(Float64);  # ptr[14]
  vcPercentageSoilCoverage @44 :Float64;  # bits[1664, 1728)
  percolationRate @45 :List(Float64);  # ptr[15]
  vwPrecipitation @46 :Float64;  # bits[1728, 1792)
  referenceEvapotranspiration @47 :Float64 = 6;  # bits[1792, 1856)
  relativeHumidity @48 :Float64;  # bits[1856, 1920)
  residualEvapotranspiration @49 :List(Float64);  # ptr[16]
  saturatedHydraulicConductivity @50 :List(Float64);  # ptr[17]
  soilMoisture @51 :List(Float64);  # ptr[18]
  soilMoisturecrit @52 :Float64;  # bits[1920, 1984)
  soilMoistureDeficit @53 :Float64;  # bits[1984, 2048)
  soilPoreVolume @54 :List(Float64);  # ptr[19]
  vcStomataResistance @55 :Float64;  # bits[2048, 2112)
  surfaceRoughness @56 :Float64;  # bits[2112, 2176)
  surfaceRunOff @57 :Float64;  # bits[2176, 2240)
  sumSurfaceRunOff @58 :Float64;  # bits[2240, 2304)
  surfaceWaterStorage @59 :Float64;  # bits[2304, 2368)
  ptTimeStep @60 :Float64;  # bits[2368, 2432)
  totalWaterRemoval @61 :Float64;  # bits[2432, 2496)
  transpiration @62 :List(Float64);  # ptr[20]
  transpirationDeficit @63 :Float64;  # bits[2496, 2560)
  waterFlux @64 :List(Float64);  # ptr[21]
  vwWindSpeed @5 :Float64;  # bits[128, 192)
  vwWindSpeedHeight @4 :Float64;  # bits[64, 128)
  xSACriticalSoilMoisture @2 :Float64;  # bits[0, 64)
  snowComponent @1 :SnowModuleState;  # ptr[1]
  frostComponent @0 :FrostModuleState;  # ptr[0]
}
struct SoilOrganicModuleState @0xd594e64f6b5f461d {  # 128 bytes, 22 ptrs
  moduleParams @2 :import "/model/monica/monica_params.capnp".SoilOrganicModuleParameters;  # ptr[0]
  vsNumberOfLayers @3 :UInt16;  # bits[16, 32)
  vsNumberOfOrganicLayers @4 :UInt16;  # bits[32, 48)
  addedOrganicMatter @5 :Bool;  # bits[1, 2)
  irrigationAmount @6 :Float64;  # bits[128, 192)
  actAmmoniaOxidationRate @7 :List(Float64);  # ptr[1]
  actNitrificationRate @8 :List(Float64);  # ptr[2]
  actDenitrificationRate @9 :List(Float64);  # ptr[3]
  aomFastDeltaSum @10 :List(Float64);  # ptr[4]
  aomFastInput @11 :List(Float64);  # ptr[5]
  aomFastSum @12 :List(Float64);  # ptr[6]
  aomSlowDeltaSum @13 :List(Float64);  # ptr[7]
  aomSlowInput @14 :List(Float64);  # ptr[8]
  aomSlowSum @15 :List(Float64);  # ptr[9]
  cBalance @16 :List(Float64);  # ptr[10]
  decomposerRespiration @17 :Float64;  # bits[192, 256)
  errorMessage @18 :Text;  # ptr[11]
  inertSoilOrganicC @19 :List(Float64);  # ptr[12]
  n2oProduced @20 :Float64;  # bits[256, 320)
  n2oProducedNit @21 :Float64;  # bits[320, 384)
  n2oProducedDenit @22 :Float64;  # bits[384, 448)
  netEcosystemExchange @23 :Float64;  # bits[448, 512)
  netEcosystemProduction @24 :Float64;  # bits[512, 576)
  netNMineralisation @25 :Float64;  # bits[576, 640)
  netNMineralisationRate @26 :List(Float64);  # ptr[13]
  totalNH3Volatilised @27 :Float64;  # bits[640, 704)
  nh3Volatilised @28 :Float64;  # bits[704, 768)
  smbCO2EvolutionRate @29 :List(Float64);  # ptr[14]
  smbFastDelta @30 :List(Float64);  # ptr[15]
  smbSlowDelta @31 :List(Float64);  # ptr[16]
  vsSoilMineralNContent @32 :List(Float64);  # ptr[17]
  soilOrganicC @33 :List(Float64);  # ptr[18]
  somFastDelta @34 :List(Float64);  # ptr[19]
  somFastInput @35 :List(Float64);  # ptr[20]
  somSlowDelta @36 :List(Float64);  # ptr[21]
  sumDenitrification @37 :Float64;  # bits[768, 832)
  sumNetNMineralisation @38 :Float64;  # bits[832, 896)
  sumN2OProduced @39 :Float64;  # bits[896, 960)
  sumNH3Volatilised @40 :Float64;  # bits[960, 1024)
  totalDenitrification @1 :Float64;  # bits[64, 128)
  incorporation @0 :Bool;  # bits[0, 1)
}
struct SoilTemperatureModuleState @0xbd3e199eb9b03758 {  # 32 bytes, 14 ptrs
  soilSurfaceTemperature @0 :Float64;  # bits[0, 64)
  soilColumnVtGroundLayer @3 :SoilLayerState;  # ptr[1]
  soilColumnVtBottomLayer @4 :SoilLayerState;  # ptr[2]
  moduleParams @1 :import "/model/monica/monica_params.capnp".SoilTemperatureModuleParameters;  # ptr[0]
  numberOfLayers @5 :UInt16;  # bits[128, 144)
  vsNumberOfLayers @6 :UInt16;  # bits[144, 160)
  vsSoilMoistureConst @7 :List(Float64);  # ptr[3]
  soilTemperature @8 :List(Float64);  # ptr[4]
  v @9 :List(Float64);  # ptr[5]
  volumeMatrix @10 :List(Float64);  # ptr[6]
  volumeMatrixOld @11 :List(Float64);  # ptr[7]
  b @12 :List(Float64);  # ptr[8]
  matrixPrimaryDiagonal @13 :List(Float64);  # ptr[9]
  matrixSecundaryDiagonal @14 :List(Float64);  # ptr[10]
  heatFlow @15 :Float64;  # bits[192, 256)
  heatConductivity @16 :List(Float64);  # ptr[11]
  heatConductivityMean @17 :List(Float64);  # ptr[12]
  heatCapacity @18 :List(Float64);  # ptr[13]
  dampingFactor @2 :Float64 = 0.8;  # bits[64, 128)
}
struct SoilTransportModuleState @0xb1760f65e652e737 {  # 48 bytes, 12 ptrs
  moduleParams @1 :import "/model/monica/monica_params.capnp".SoilTransportModuleParameters;  # ptr[0]
  convection @2 :List(Float64);  # ptr[1]
  cropNUptake @3 :Float64;  # bits[64, 128)
  diffusionCoeff @4 :List(Float64);  # ptr[2]
  dispersion @5 :List(Float64);  # ptr[3]
  dispersionCoeff @6 :List(Float64);  # ptr[4]
  vsLeachingDepth @7 :Float64;  # bits[128, 192)
  leachingAtBoundary @8 :Float64;  # bits[192, 256)
  vsNDeposition @9 :Float64;  # bits[256, 320)
  vcNUptakeFromLayer @10 :List(Float64);  # ptr[5]
  poreWaterVelocity @11 :List(Float64);  # ptr[6]
  vsSoilMineralNContent @12 :List(Float64);  # ptr[7]
  soilNO3 @13 :List(Float64);  # ptr[8]
  soilNO3aq @14 :List(Float64);  # ptr[9]
  timeStep @15 :Float64 = 1;  # bits[320, 384)
  totalDispersion @16 :List(Float64);  # ptr[10]
  percolationRate @17 :List(Float64);  # ptr[11]
  pcMinimumAvailableN @0 :Float64;  # bits[0, 64)
}
struct ICData @0xf03d8fd1bbe75519 {  # 16 bytes, 0 ptrs
  union {  # tag bits [0, 16)
    noCrop @0 :Void;  # bits[0, 0), union tag = 0
    height @1 :Float64;  # bits[64, 128), union tag = 1
    lait @2 :Float64;  # bits[64, 128), union tag = 2
  }
}
