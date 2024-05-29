# model/monica/monica_params.capnp
@0xeef9ddc7a345de6d;
$import "/capnp/c++.capnp".namespace("mas::schema::model::monica");
$import "/capnp/go.capnp".package("monica");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/model/monica");
struct CropSpec @0xa74f5574681f9d55 {  # 0 bytes, 2 ptrs
  cropParams @0 :CropParameters;  # ptr[0]
  residueParams @1 :CropResidueParameters;  # ptr[1]
}
struct CropParameters @0x8ac5cfb21988c168 {  # 0 bytes, 2 ptrs
  speciesParams @0 :SpeciesParameters;  # ptr[0]
  cultivarParams @1 :CultivarParameters;  # ptr[1]
}
struct SpeciesParameters @0xd2d587c796186e8b {  # 328 bytes, 10 ptrs
  speciesId @0 :Text;  # ptr[0]
  carboxylationPathway @1 :UInt8;  # bits[0, 8)
  defaultRadiationUseEfficiency @2 :Float64;  # bits[64, 128)
  partBiologicalNFixation @3 :Float64;  # bits[128, 192)
  initialKcFactor @4 :Float64;  # bits[192, 256)
  luxuryNCoeff @5 :Float64;  # bits[256, 320)
  maxCropDiameter @6 :Float64;  # bits[320, 384)
  stageAtMaxHeight @7 :Float64;  # bits[384, 448)
  stageAtMaxDiameter @8 :Float64;  # bits[448, 512)
  minimumNConcentration @9 :Float64;  # bits[512, 576)
  minimumTemperatureForAssimilation @10 :Float64;  # bits[576, 640)
  optimumTemperatureForAssimilation @11 :Float64;  # bits[640, 704)
  maximumTemperatureForAssimilation @12 :Float64;  # bits[704, 768)
  nConcentrationAbovegroundBiomass @13 :Float64;  # bits[768, 832)
  nConcentrationB0 @14 :Float64;  # bits[832, 896)
  nConcentrationPN @15 :Float64;  # bits[896, 960)
  nConcentrationRoot @16 :Float64;  # bits[960, 1024)
  developmentAccelerationByNitrogenStress @17 :UInt16;  # bits[16, 32)
  fieldConditionModifier @18 :Float64 = 1;  # bits[1024, 1088)
  assimilateReallocation @19 :Float64;  # bits[1088, 1152)
  baseTemperature @20 :List(Float64);  # ptr[1]
  organMaintenanceRespiration @21 :List(Float64);  # ptr[2]
  organGrowthRespiration @22 :List(Float64);  # ptr[3]
  stageMaxRootNConcentration @23 :List(Float64);  # ptr[4]
  initialOrganBiomass @24 :List(Float64);  # ptr[5]
  criticalOxygenContent @25 :List(Float64);  # ptr[6]
  stageMobilFromStorageCoeff @26 :List(Float64);  # ptr[7]
  abovegroundOrgan @27 :List(Bool);  # ptr[8]
  storageOrgan @28 :List(Bool);  # ptr[9]
  samplingDepth @29 :Float64;  # bits[1152, 1216)
  targetNSamplingDepth @30 :Float64;  # bits[1216, 1280)
  targetN30 @31 :Float64;  # bits[1280, 1344)
  maxNUptakeParam @32 :Float64;  # bits[1344, 1408)
  rootDistributionParam @33 :Float64;  # bits[1408, 1472)
  plantDensity @34 :UInt16;  # bits[32, 48)
  rootGrowthLag @35 :Float64;  # bits[1472, 1536)
  minimumTemperatureRootGrowth @36 :Float64;  # bits[1536, 1600)
  initialRootingDepth @37 :Float64;  # bits[1600, 1664)
  rootPenetrationRate @38 :Float64;  # bits[1664, 1728)
  rootFormFactor @39 :Float64;  # bits[1728, 1792)
  specificRootLength @40 :Float64;  # bits[1792, 1856)
  stageAfterCut @41 :UInt16;  # bits[48, 64)
  limitingTemperatureHeatStress @42 :Float64;  # bits[1856, 1920)
  cuttingDelayDays @43 :UInt16;  # bits[1920, 1936)
  droughtImpactOnFertilityFactor @44 :Float64;  # bits[1984, 2048)
  efMono @45 :Float64 = 0.5;  # bits[2048, 2112)
  efMonos @46 :Float64 = 0.5;  # bits[2112, 2176)
  efIso @47 :Float64;  # bits[2176, 2240)
  vcMax25 @48 :Float64;  # bits[2240, 2304)
  aekc @49 :Float64 = 65800;  # bits[2304, 2368)
  aeko @50 :Float64 = 1400;  # bits[2368, 2432)
  aevc @51 :Float64 = 68800;  # bits[2432, 2496)
  kc25 @52 :Float64 = 460;  # bits[2496, 2560)
  ko25 @53 :Float64 = 330;  # bits[2560, 2624)
  transitionStageLeafExp @54 :Int16 = -1;  # bits[1936, 1952)
}
struct CultivarParameters @0xf206f12e39ab7f9b {  # 176 bytes, 15 ptrs
  cultivarId @0 :Text;  # ptr[0]
  description @1 :Text;  # ptr[1]
  perennial @2 :Bool;  # bits[0, 1)
  maxAssimilationRate @3 :Float64;  # bits[64, 128)
  maxCropHeight @4 :Float64;  # bits[128, 192)
  residueNRatio @5 :Float64;  # bits[192, 256)
  lt50cultivar @6 :Float64;  # bits[256, 320)
  cropHeightP1 @7 :Float64;  # bits[320, 384)
  cropHeightP2 @8 :Float64;  # bits[384, 448)
  cropSpecificMaxRootingDepth @9 :Float64;  # bits[448, 512)
  assimilatePartitioningCoeff @10 :List(List(Float64));  # ptr[2]
  organSenescenceRate @11 :List(List(Float64));  # ptr[3]
  baseDaylength @12 :List(Float64);  # ptr[4]
  optimumTemperature @13 :List(Float64);  # ptr[5]
  daylengthRequirement @14 :List(Float64);  # ptr[6]
  droughtStressThreshold @15 :List(Float64);  # ptr[7]
  specificLeafArea @16 :List(Float64);  # ptr[8]
  stageKcFactor @17 :List(Float64);  # ptr[9]
  stageTemperatureSum @18 :List(Float64);  # ptr[10]
  vernalisationRequirement @19 :List(Float64);  # ptr[11]
  heatSumIrrigationStart @20 :Float64;  # bits[512, 576)
  heatSumIrrigationEnd @21 :Float64;  # bits[576, 640)
  criticalTemperatureHeatStress @22 :Float64;  # bits[640, 704)
  beginSensitivePhaseHeatStress @23 :Float64;  # bits[704, 768)
  endSensitivePhaseHeatStress @24 :Float64;  # bits[768, 832)
  frostHardening @25 :Float64;  # bits[832, 896)
  frostDehardening @26 :Float64;  # bits[896, 960)
  lowTemperatureExposure @27 :Float64;  # bits[960, 1024)
  respiratoryStress @28 :Float64;  # bits[1024, 1088)
  latestHarvestDoy @29 :Int16 = -1;  # bits[16, 32)
  organIdsForPrimaryYield @30 :List(YieldComponent);  # ptr[12]
  organIdsForSecondaryYield @31 :List(YieldComponent);  # ptr[13]
  organIdsForCutting @32 :List(YieldComponent);  # ptr[14]
  earlyRefLeafExp @33 :Float64 = 12;  # bits[1088, 1152)
  refLeafExp @34 :Float64 = 20;  # bits[1152, 1216)
  minTempDevWE @35 :Float64;  # bits[1216, 1280)
  optTempDevWE @36 :Float64;  # bits[1280, 1344)
  maxTempDevWE @37 :Float64;  # bits[1344, 1408)
  winterCrop @38 :Bool;  # bits[1, 2)
}
struct YieldComponent @0xdbfe301c0ddefe4e {  # 24 bytes, 0 ptrs
  organId @0 :Int64 = -1;  # bits[0, 64)
  yieldPercentage @1 :Float64;  # bits[64, 128)
  yieldDryMatter @2 :Float64;  # bits[128, 192)
}
struct AutomaticHarvestParameters @0xc5f724bd00c2f628 {  # 8 bytes, 0 ptrs
  harvestTime @0 :HarvestTime = unknown;  # bits[0, 16)
  latestHarvestDOY @1 :Int16 = -1;  # bits[16, 32)
  enum HarvestTime @0x990bdcf2be83b604 {
    maturity @0;
    unknown @1;
  }
}
struct NMinCropParameters @0xea9236083718fdc2 {  # 24 bytes, 0 ptrs
  samplingDepth @0 :Float64;  # bits[0, 64)
  nTarget @1 :Float64;  # bits[64, 128)
  nTarget30 @2 :Float64;  # bits[128, 192)
}
struct NMinApplicationParameters @0xde7576c640b5ad18 {  # 24 bytes, 0 ptrs
  min @0 :Float64;  # bits[0, 64)
  max @1 :Float64;  # bits[64, 128)
  delayInDays @2 :UInt16;  # bits[128, 144)
}
struct CropResidueParameters @0x8491dc2c2f94f1d1 {  # 0 bytes, 3 ptrs
  params @0 :import "/model/monica/monica_management.capnp".Params.OrganicFertilization.OrganicMatterParameters;  # ptr[0]
  species @1 :Text;  # ptr[1]
  residueType @2 :Text;  # ptr[2]
}
struct SoilParameters @0xb42137d4b8ba3ef6 {  # 128 bytes, 1 ptrs
  soilSandContent @0 :Float64 = -1;  # bits[0, 64)
  soilClayContent @1 :Float64 = -1;  # bits[64, 128)
  soilpH @2 :Float64 = 6.9;  # bits[128, 192)
  soilStoneContent @3 :Float64;  # bits[192, 256)
  lambda @4 :Float64 = -1;  # bits[256, 320)
  fieldCapacity @5 :Float64 = -1;  # bits[320, 384)
  saturation @6 :Float64 = -1;  # bits[384, 448)
  permanentWiltingPoint @7 :Float64 = -1;  # bits[448, 512)
  soilTexture @8 :Text;  # ptr[0]
  soilAmmonium @9 :Float64 = 0.0005;  # bits[512, 576)
  soilNitrate @10 :Float64 = 0.005;  # bits[576, 640)
  soilCNRatio @11 :Float64 = 10;  # bits[640, 704)
  soilMoisturePercentFC @12 :Float64 = 100;  # bits[704, 768)
  soilRawDensity @13 :Float64 = -1;  # bits[768, 832)
  soilBulkDensity @14 :Float64 = -1;  # bits[832, 896)
  soilOrganicCarbon @15 :Float64 = -1;  # bits[896, 960)
  soilOrganicMatter @16 :Float64 = -1;  # bits[960, 1024)
}
struct AutomaticIrrigationParameters @0x8890f17a143c6896 {  # 16 bytes, 1 ptrs
  params @0 :import "/model/monica/monica_management.capnp".Params.Irrigation.Parameters;  # ptr[0]
  amount @1 :Float64 = 17;  # bits[0, 64)
  threshold @2 :Float64 = 0.35;  # bits[64, 128)
}
struct SiteParameters @0xb599bbd2f1465f9c {  # 80 bytes, 1 ptrs
  latitude @0 :Float64 = 52.5;  # bits[0, 64)
  slope @1 :Float64 = 0.01;  # bits[64, 128)
  heightNN @2 :Float64 = 50;  # bits[128, 192)
  groundwaterDepth @3 :Float64 = 70;  # bits[192, 256)
  soilCNRatio @4 :Float64 = 10;  # bits[256, 320)
  drainageCoeff @5 :Float64 = 1;  # bits[320, 384)
  vqNDeposition @6 :Float64 = 30;  # bits[384, 448)
  maxEffectiveRootingDepth @7 :Float64 = 2;  # bits[448, 512)
  impenetrableLayerDepth @8 :Float64 = -1;  # bits[512, 576)
  soilSpecificHumusBalanceCorrection @9 :Float64;  # bits[576, 640)
  soilParameters @10 :List(SoilParameters);  # ptr[0]
}
struct EnvironmentParameters @0xc0ff4a277ca4be0a {  # 72 bytes, 2 ptrs
  albedo @0 :Float64 = 0.23;  # bits[0, 64)
  rcp @11 :import "/climate.capnp".RCP;  # bits[528, 544)
  atmosphericCO2 @1 :Float64;  # bits[64, 128)
  atmosphericCO2s @2 :List(YearToValue);  # ptr[0]
  atmosphericO3 @3 :Float64;  # bits[128, 192)
  atmosphericO3s @4 :List(YearToValue);  # ptr[1]
  windSpeedHeight @5 :Float64 = 2;  # bits[192, 256)
  leachingDepth @6 :Float64;  # bits[256, 320)
  timeStep @7 :Float64;  # bits[320, 384)
  maxGroundwaterDepth @8 :Float64 = 18;  # bits[384, 448)
  minGroundwaterDepth @9 :Float64 = 20;  # bits[448, 512)
  minGroundwaterDepthMonth @10 :UInt8 = 3;  # bits[512, 520)
  struct YearToValue @0xe68d439455fd9cce {  # 16 bytes, 0 ptrs
    year @0 :UInt16;  # bits[0, 16)
    value @1 :Float64;  # bits[64, 128)
  }
}
struct MeasuredGroundwaterTableInformation @0xc1092d6c4c110e29 {  # 8 bytes, 1 ptrs
  groundwaterInformationAvailable @0 :Bool;  # bits[0, 1)
  groundwaterInfo @1 :List(DateToValue);  # ptr[0]
  struct DateToValue @0x81b8ffeeb01d76f7 {  # 8 bytes, 1 ptrs
    date @0 :import "/date.capnp".Date;  # ptr[0]
    value @1 :Float64;  # bits[0, 64)
  }
}
struct SimulationParameters @0xffac0fa5c7156a5d {  # 16 bytes, 5 ptrs
  startDate @0 :import "/date.capnp".Date;  # ptr[0]
  endDate @1 :import "/date.capnp".Date;  # ptr[1]
  nitrogenResponseOn @2 :Bool = true;  # bits[0, 1)
  waterDeficitResponseOn @3 :Bool = true;  # bits[1, 2)
  emergenceFloodingControlOn @4 :Bool = true;  # bits[2, 3)
  emergenceMoistureControlOn @5 :Bool = true;  # bits[3, 4)
  frostKillOn @6 :Bool = true;  # bits[4, 5)
  useAutomaticIrrigation @7 :Bool;  # bits[5, 6)
  autoIrrigationParams @8 :AutomaticIrrigationParameters;  # ptr[2]
  useNMinMineralFertilisingMethod @9 :Bool;  # bits[6, 7)
  nMinFertiliserPartition @10 :import "/model/monica/monica_management.capnp".Params.MineralFertilization.Parameters;  # ptr[3]
  nMinApplicationParams @11 :NMinApplicationParameters;  # ptr[4]
  useSecondaryYields @12 :Bool = true;  # bits[7, 8)
  useAutomaticHarvestTrigger @13 :Bool;  # bits[8, 9)
  numberOfLayers @14 :UInt16 = 20;  # bits[16, 32)
  layerThickness @15 :Float64 = 0.1;  # bits[64, 128)
  startPVIndex @16 :UInt16;  # bits[32, 48)
  julianDayAutomaticFertilising @17 :UInt16;  # bits[48, 64)
}
struct CropModuleParameters @0xe4d6d0d9ae1553da {  # 128 bytes, 0 ptrs
  canopyReflectionCoefficient @0 :Float64;  # bits[0, 64)
  referenceMaxAssimilationRate @1 :Float64;  # bits[64, 128)
  referenceLeafAreaIndex @2 :Float64;  # bits[128, 192)
  maintenanceRespirationParameter1 @3 :Float64;  # bits[192, 256)
  maintenanceRespirationParameter2 @4 :Float64;  # bits[256, 320)
  minimumNConcentrationRoot @5 :Float64;  # bits[320, 384)
  minimumAvailableN @6 :Float64;  # bits[384, 448)
  referenceAlbedo @7 :Float64;  # bits[448, 512)
  stomataConductanceAlpha @8 :Float64;  # bits[512, 576)
  saturationBeta @9 :Float64;  # bits[576, 640)
  growthRespirationRedux @10 :Float64;  # bits[640, 704)
  maxCropNDemand @11 :Float64;  # bits[704, 768)
  growthRespirationParameter1 @12 :Float64;  # bits[768, 832)
  growthRespirationParameter2 @13 :Float64;  # bits[832, 896)
  tortuosity @14 :Float64;  # bits[896, 960)
  adjustRootDepthForSoilProps @15 :Bool;  # bits[960, 961)
  experimentalEnablePhenologyWangEngelTemperatureResponse @16 :Bool;  # bits[961, 962)
  experimentalEnablePhotosynthesisWangEngelTemperatureResponse @17 :Bool;  # bits[962, 963)
  experimentalEnableHourlyFvCBPhotosynthesis @18 :Bool;  # bits[963, 964)
  experimentalEnableTResponseLeafExpansion @19 :Bool;  # bits[964, 965)
  experimentalDisableDailyRootBiomassToSoil @20 :Bool;  # bits[965, 966)
  enableVernalisationFactorFix @21 :Bool;  # bits[966, 967)
}
struct SoilMoistureModuleParameters @0xcdff1b0306ea58cf {  # 192 bytes, 0 ptrs
  criticalMoistureDepth @0 :Float64;  # bits[0, 64)
  saturatedHydraulicConductivity @1 :Float64;  # bits[64, 128)
  surfaceRoughness @2 :Float64;  # bits[128, 192)
  groundwaterDischarge @3 :Float64;  # bits[192, 256)
  hydraulicConductivityRedux @4 :Float64;  # bits[256, 320)
  snowAccumulationTresholdTemperature @5 :Float64;  # bits[320, 384)
  kcFactor @6 :Float64;  # bits[384, 448)
  temperatureLimitForLiquidWater @7 :Float64;  # bits[448, 512)
  correctionSnow @8 :Float64;  # bits[512, 576)
  correctionRain @9 :Float64;  # bits[576, 640)
  snowMaxAdditionalDensity @10 :Float64;  # bits[640, 704)
  newSnowDensityMin @11 :Float64;  # bits[704, 768)
  snowRetentionCapacityMin @12 :Float64;  # bits[768, 832)
  refreezeParameter1 @13 :Float64;  # bits[832, 896)
  refreezeParameter2 @14 :Float64;  # bits[896, 960)
  refreezeTemperature @15 :Float64;  # bits[960, 1024)
  snowMeltTemperature @16 :Float64;  # bits[1024, 1088)
  snowPacking @17 :Float64;  # bits[1088, 1152)
  snowRetentionCapacityMax @18 :Float64;  # bits[1152, 1216)
  evaporationZeta @19 :Float64;  # bits[1216, 1280)
  xsaCriticalSoilMoisture @20 :Float64;  # bits[1280, 1344)
  maximumEvaporationImpactDepth @21 :Float64;  # bits[1344, 1408)
  maxPercolationRate @22 :Float64;  # bits[1408, 1472)
  moistureInitValue @23 :Float64;  # bits[1472, 1536)
}
struct SoilOrganicModuleParameters @0xb3e73f8c19afd787 {  # 288 bytes, 1 ptrs
  somSlowDecCoeffStandard @0 :Float64 = 4.3e-05;  # bits[0, 64)
  somFastDecCoeffStandard @1 :Float64 = 0.00014;  # bits[64, 128)
  smbSlowMaintRateStandard @2 :Float64 = 0.001;  # bits[128, 192)
  smbFastMaintRateStandard @3 :Float64 = 0.01;  # bits[192, 256)
  smbSlowDeathRateStandard @4 :Float64 = 0.001;  # bits[256, 320)
  smbFastDeathRateStandard @5 :Float64 = 0.01;  # bits[320, 384)
  smbUtilizationEfficiency @6 :Float64 = 0.6;  # bits[384, 448)
  somSlowUtilizationEfficiency @7 :Float64 = 0.4;  # bits[448, 512)
  somFastUtilizationEfficiency @8 :Float64 = 0.5;  # bits[512, 576)
  aomSlowUtilizationEfficiency @9 :Float64 = 0.4;  # bits[576, 640)
  aomFastUtilizationEfficiency @10 :Float64 = 0.1;  # bits[640, 704)
  aomFastMaxCtoN @11 :Float64 = 1000;  # bits[704, 768)
  partSOMFastToSOMSlow @12 :Float64 = 0.3;  # bits[768, 832)
  partSMBSlowToSOMFast @13 :Float64 = 0.6;  # bits[832, 896)
  partSMBFastToSOMFast @14 :Float64 = 0.6;  # bits[896, 960)
  partSOMToSMBSlow @15 :Float64 = 0.015;  # bits[960, 1024)
  partSOMToSMBFast @16 :Float64 = 0.0002;  # bits[1024, 1088)
  cnRatioSMB @17 :Float64 = 6.7;  # bits[1088, 1152)
  limitClayEffect @18 :Float64 = 0.25;  # bits[1152, 1216)
  ammoniaOxidationRateCoeffStandard @19 :Float64 = 0.1;  # bits[1216, 1280)
  nitriteOxidationRateCoeffStandard @20 :Float64 = 0.9;  # bits[1280, 1344)
  transportRateCoeff @21 :Float64 = 0.1;  # bits[1344, 1408)
  specAnaerobDenitrification @22 :Float64 = 0.1;  # bits[1408, 1472)
  immobilisationRateCoeffNO3 @23 :Float64 = 0.5;  # bits[1472, 1536)
  immobilisationRateCoeffNH4 @24 :Float64 = 0.5;  # bits[1536, 1600)
  denit1 @25 :Float64 = 0.2;  # bits[1600, 1664)
  denit2 @26 :Float64 = 0.8;  # bits[1664, 1728)
  denit3 @27 :Float64 = 0.9;  # bits[1728, 1792)
  hydrolysisKM @28 :Float64 = 0.00334;  # bits[1792, 1856)
  activationEnergy @29 :Float64 = 41000;  # bits[1856, 1920)
  hydrolysisP1 @30 :Float64 = 4.259e-12;  # bits[1920, 1984)
  hydrolysisP2 @31 :Float64 = 1.408e-12;  # bits[1984, 2048)
  atmosphericResistance @32 :Float64 = 0.0025;  # bits[2048, 2112)
  n2oProductionRate @33 :Float64 = 0.5;  # bits[2112, 2176)
  inhibitorNH3 @34 :Float64 = 1;  # bits[2176, 2240)
  psMaxMineralisationDepth @35 :Float64 = 0.4;  # bits[2240, 2304)
  sticsParams @36 :SticsParameters;  # ptr[0]
}
struct SoilTemperatureModuleParameters @0xf0c41d021228d929 {  # 104 bytes, 0 ptrs
  nTau @0 :Float64;  # bits[0, 64)
  initialSurfaceTemperature @1 :Float64;  # bits[64, 128)
  baseTemperature @2 :Float64;  # bits[128, 192)
  quartzRawDensity @3 :Float64;  # bits[192, 256)
  densityAir @4 :Float64;  # bits[256, 320)
  densityWater @5 :Float64;  # bits[320, 384)
  densityHumus @6 :Float64;  # bits[384, 448)
  specificHeatCapacityAir @7 :Float64;  # bits[448, 512)
  specificHeatCapacityQuartz @8 :Float64;  # bits[512, 576)
  specificHeatCapacityWater @9 :Float64;  # bits[576, 640)
  specificHeatCapacityHumus @10 :Float64;  # bits[640, 704)
  soilAlbedo @11 :Float64;  # bits[704, 768)
  soilMoisture @12 :Float64 = 0.25;  # bits[768, 832)
}
struct SoilTransportModuleParameters @0xc5cb65e585742338 {  # 32 bytes, 0 ptrs
  dispersionLength @0 :Float64;  # bits[0, 64)
  ad @1 :Float64;  # bits[64, 128)
  diffusionCoefficientStandard @2 :Float64;  # bits[128, 192)
  nDeposition @3 :Float64;  # bits[192, 256)
}
struct Voc @0xb87956e2953771db {  # 0 bytes, 0 ptrs
  struct Emissions @0xd9ed2c1c754d683e {  # 16 bytes, 2 ptrs
    speciesIdToIsopreneEmission @0 :List(SpeciesIdToEmission);  # ptr[0]
    speciesIdToMonoterpeneEmission @1 :List(SpeciesIdToEmission);  # ptr[1]
    isopreneEmission @2 :Float64;  # bits[0, 64)
    monoterpeneEmission @3 :Float64;  # bits[64, 128)
    struct SpeciesIdToEmission @0xd11f8d1479e2f010 {  # 16 bytes, 0 ptrs
      speciesId @0 :UInt64;  # bits[0, 64)
      emission @1 :Float64;  # bits[64, 128)
    }
  }
  struct SpeciesData @0x80d5a7b782142e87 {  # 240 bytes, 0 ptrs
    id @0 :UInt64;  # bits[0, 64)
    efMonos @1 :Float64;  # bits[64, 128)
    efMono @2 :Float64;  # bits[128, 192)
    efIso @3 :Float64;  # bits[192, 256)
    theta @4 :Float64 = 0.9;  # bits[256, 320)
    fage @5 :Float64 = 1;  # bits[320, 384)
    ctIs @6 :Float64;  # bits[384, 448)
    ctMt @7 :Float64;  # bits[448, 512)
    haIs @8 :Float64;  # bits[512, 576)
    haMt @9 :Float64;  # bits[576, 640)
    dsIs @10 :Float64;  # bits[640, 704)
    dsMt @11 :Float64;  # bits[704, 768)
    hdIs @12 :Float64 = 284600;  # bits[768, 832)
    hdMt @13 :Float64 = 284600;  # bits[832, 896)
    hdj @14 :Float64 = 220000;  # bits[896, 960)
    sdj @15 :Float64 = 703;  # bits[960, 1024)
    kc25 @16 :Float64 = 260;  # bits[1024, 1088)
    ko25 @17 :Float64 = 179;  # bits[1088, 1152)
    vcMax25 @18 :Float64 = 80;  # bits[1152, 1216)
    qjvc @19 :Float64 = 2;  # bits[1216, 1280)
    aekc @20 :Float64 = 59356;  # bits[1280, 1344)
    aeko @21 :Float64 = 35948;  # bits[1344, 1408)
    aejm @22 :Float64 = 37000;  # bits[1408, 1472)
    aevc @23 :Float64 = 58520;  # bits[1472, 1536)
    slaMin @24 :Float64 = 20;  # bits[1536, 1600)
    scaleI @25 :Float64 = 1;  # bits[1600, 1664)
    scaleM @26 :Float64 = 1;  # bits[1664, 1728)
    mFol @27 :Float64;  # bits[1728, 1792)
    lai @28 :Float64;  # bits[1792, 1856)
    sla @29 :Float64;  # bits[1856, 1920)
  }
  struct CPData @0xcf0f425c8bd69fa2 {  # 80 bytes, 0 ptrs
    kc @0 :Float64;  # bits[0, 64)
    ko @1 :Float64;  # bits[64, 128)
    oi @2 :Float64;  # bits[128, 192)
    ci @3 :Float64;  # bits[192, 256)
    comp @4 :Float64;  # bits[256, 320)
    vcMax @5 :Float64;  # bits[320, 384)
    jMax @6 :Float64;  # bits[384, 448)
    jj @7 :Float64;  # bits[448, 512)
    jj1000 @8 :Float64;  # bits[512, 576)
    jv @9 :Float64;  # bits[576, 640)
  }
  struct MicroClimateData @0xf246442c7aee0af5 {  # 72 bytes, 0 ptrs
    rad @0 :Float64;  # bits[0, 64)
    rad24 @1 :Float64;  # bits[64, 128)
    rad240 @2 :Float64;  # bits[128, 192)
    tFol @3 :Float64;  # bits[192, 256)
    tFol24 @4 :Float64;  # bits[256, 320)
    tFol240 @5 :Float64;  # bits[320, 384)
    sunlitfoliagefraction @6 :Float64;  # bits[384, 448)
    sunlitfoliagefraction24 @7 :Float64;  # bits[448, 512)
    co2concentration @8 :Float64;  # bits[512, 576)
  }
  struct PhotosynthT @0xf95db11410e33efc {  # 24 bytes, 0 ptrs
    par @0 :Float64;  # bits[0, 64)
    par24 @1 :Float64;  # bits[64, 128)
    par240 @2 :Float64;  # bits[128, 192)
  }
  struct FoliageT @0xee0b04cc3f52f33c {  # 24 bytes, 0 ptrs
    tempK @0 :Float64;  # bits[0, 64)
    tempK24 @1 :Float64;  # bits[64, 128)
    tempK240 @2 :Float64;  # bits[128, 192)
  }
  struct EnzymeActivityT @0xc281c6e5be483337 {  # 16 bytes, 0 ptrs
    efIso @0 :Float64;  # bits[0, 64)
    efMono @1 :Float64;  # bits[64, 128)
  }
  struct LeafEmissionT @0xe82d760b257daddb {  # 8 bytes, 3 ptrs
    foliageLayer @0 :UInt16;  # bits[0, 16)
    pho @1 :PhotosynthT;  # ptr[0]
    fol @2 :FoliageT;  # ptr[1]
    enzAct @3 :EnzymeActivityT;  # ptr[2]
  }
  struct LeafEmissions @0xc8aeb5222ac5ef40 {  # 16 bytes, 0 ptrs
    isoprene @0 :Float64;  # bits[0, 64)
    monoterp @1 :Float64;  # bits[64, 128)
  }
}
struct SticsParameters @0xce5b0091fd9acb21 {  # 240 bytes, 0 ptrs
  useN2O @0 :Bool;  # bits[0, 1)
  useNit @1 :Bool;  # bits[1, 2)
  useDenit @2 :Bool;  # bits[2, 3)
  codeVnit @3 :UInt8 = 1;  # bits[8, 16)
  codeTnit @4 :UInt8 = 2;  # bits[16, 24)
  codeRationit @5 :UInt8 = 2;  # bits[24, 32)
  codeHourlyWfpsNit @6 :UInt8 = 2;  # bits[32, 40)
  codePdenit @7 :UInt8 = 1;  # bits[40, 48)
  codeRatiodenit @8 :UInt8 = 2;  # bits[48, 56)
  codeHourlyWfpsDenit @9 :UInt8 = 2;  # bits[56, 64)
  hminn @10 :Float64 = 0.3;  # bits[64, 128)
  hoptn @11 :Float64 = 0.9;  # bits[128, 192)
  pHminnit @12 :Float64 = 4;  # bits[192, 256)
  pHmaxnit @13 :Float64 = 7.2;  # bits[256, 320)
  nh4Min @14 :Float64 = 1;  # bits[320, 384)
  pHminden @15 :Float64 = 7.2;  # bits[384, 448)
  pHmaxden @16 :Float64 = 9.2;  # bits[448, 512)
  wfpsc @17 :Float64 = 0.62;  # bits[512, 576)
  tdenitoptGauss @18 :Float64 = 47;  # bits[576, 640)
  scaleTdenitopt @19 :Float64 = 25;  # bits[640, 704)
  kd @20 :Float64 = 148;  # bits[704, 768)
  kDesat @21 :Float64 = 3;  # bits[768, 832)
  fnx @22 :Float64 = 0.8;  # bits[832, 896)
  vnitmax @23 :Float64 = 27.3;  # bits[896, 960)
  kamm @24 :Float64 = 24;  # bits[960, 1024)
  tnitmin @25 :Float64 = 5;  # bits[1024, 1088)
  tnitopt @26 :Float64 = 30;  # bits[1088, 1152)
  tnitop2 @27 :Float64 = 35;  # bits[1152, 1216)
  tnitmax @28 :Float64 = 58;  # bits[1216, 1280)
  tnitoptGauss @29 :Float64 = 32.5;  # bits[1280, 1344)
  scaleTnitopt @30 :Float64 = 16;  # bits[1344, 1408)
  rationit @31 :Float64 = 0.0016;  # bits[1408, 1472)
  cminPdenit @32 :Float64 = 1;  # bits[1472, 1536)
  cmaxPdenit @33 :Float64 = 6;  # bits[1536, 1600)
  minPdenit @34 :Float64 = 1;  # bits[1600, 1664)
  maxPdenit @35 :Float64 = 20;  # bits[1664, 1728)
  ratiodenit @36 :Float64 = 0.2;  # bits[1728, 1792)
  profdenit @37 :Float64 = 20;  # bits[1792, 1856)
  vpotdenit @38 :Float64 = 2;  # bits[1856, 1920)
}
