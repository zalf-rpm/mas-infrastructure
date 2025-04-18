/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Claas Nendel <claas.nendel@zalf.de>
Xenia Specka <xenia.specka@zalf.de>
Michael Berg <michael.berg@zalf.de>

Maintainers:
Currently maintained by the authors.

This file is part of the util library used by models created at the Institute of
Landscape Systems Analysis at the ZALF.
Copyright (C) Leibniz Centre for Agricultural Landscape Research (ZALF)
*/

#include "soil.h"

#include <map>
#include <memory>
#include <sstream>
#include <iostream>
#include <fstream>
#include <cmath>
#include <utility>
#include <mutex>
#include <climits>

#include <capnp/message.h>
#include <capnp/serialize.h>
#include <capnp/compat/json.h>
#include <kj/filesystem.h>
#include <kj/string.h>

#include "model/monica/soil_params.capnp.h"

#include "tools/algorithms.h"
#include "conversion.h"
#include "tools/debug.h"
#include "constants.h"

using namespace std;
using namespace Tools;
using namespace Soil;
using namespace json11;

//std::function<double(double)> Soil::transformIfPercent(const Json &j, const string& key) {
//    const auto &value = j[key];
//    if(value.is_array() && value.array_items().size() > 1
//       && value[1].is_string() && trim(value[1].string_value()) == "%") {
//      return [](double v) { return v / 100.0; };
//    }
//    return identity<double>;
//}

//std::function<double(double)> Soil::transformIfNotMeters(const Json &j, const string& key) {
//  const auto &value = j[key];
//  if(value.is_array() && value.array_items().size() > 1 && value[1].is_string()) {
//    auto unit = trim(value[1].string_value());
//    if (unit == "mm") return [](double v) { return v / 1000.0; };
//    else if (unit == "cm") return [](double v) { return v / 100.0; };
//    else if (unit == "dm") return [](double v) { return v / 10.0; };
//    else return identity<double>;
//  }
//  return identity<double>;
//}

void SoilParameters::serialize(mas::schema::model::monica::SoilParameters::Builder builder) const {
  builder.setSoilSandContent(vs_SoilSandContent);
  builder.setSoilClayContent(vs_SoilClayContent);
  builder.setSoilpH(vs_SoilpH);
  builder.setSoilStoneContent(vs_SoilStoneContent);
  builder.setLambda(vs_Lambda);
  builder.setFieldCapacity(vs_FieldCapacity);
  builder.setSaturation(vs_Saturation);
  builder.setPermanentWiltingPoint(vs_PermanentWiltingPoint);
  builder.setSoilTexture(vs_SoilTexture);
  builder.setSoilAmmonium(vs_SoilAmmonium);
  builder.setSoilNitrate(vs_SoilNitrate);
  builder.setSoilCNRatio(vs_Soil_CN_Ratio);
  builder.setSoilMoisturePercentFC(vs_SoilMoisturePercentFC);
  builder.setSoilRawDensity(_vs_SoilRawDensity);
  builder.setSoilBulkDensity(_vs_SoilBulkDensity);
  builder.setSoilOrganicCarbon(_vs_SoilOrganicCarbon);
  builder.setSoilOrganicMatter(_vs_SoilOrganicMatter);
}

void SoilParameters::deserialize(mas::schema::model::monica::SoilParameters::Reader reader) {
  vs_SoilSandContent = reader.getSoilSandContent();
  vs_SoilClayContent = reader.getSoilClayContent();
  vs_SoilpH = reader.getSoilpH();
  vs_SoilStoneContent = reader.getSoilStoneContent();
  vs_Lambda = reader.getLambda();
  vs_FieldCapacity = reader.getFieldCapacity();
  vs_Saturation = reader.getSaturation();
  vs_PermanentWiltingPoint = reader.getPermanentWiltingPoint();
  vs_SoilTexture = reader.getSoilTexture();
  vs_SoilAmmonium = reader.getSoilAmmonium();
  vs_SoilNitrate = reader.getSoilNitrate();
  vs_Soil_CN_Ratio = reader.getSoilCNRatio();
  vs_SoilMoisturePercentFC = reader.getSoilMoisturePercentFC();
  _vs_SoilRawDensity = reader.getSoilRawDensity();
  _vs_SoilBulkDensity = reader.getSoilBulkDensity();
  _vs_SoilOrganicCarbon = reader.getSoilOrganicCarbon();
  _vs_SoilOrganicMatter = reader.getSoilOrganicMatter();
}

SoilParameters::SoilParameters(json11::Json j) {
  merge(j);
}

Errors SoilParameters::merge(json11::Json j) {
  Errors es;

  set_double_value(vs_SoilSandContent, j, "Sand", transformIfPercent(j, "Sand"));
  set_double_value(vs_SoilClayContent, j, "Clay", transformIfPercent(j, "Clay"));
  set_double_value(vs_SoilpH, j, "pH");
  set_double_value(vs_SoilStoneContent, j, "Sceleton", transformIfPercent(j, "Sceleton"));
  set_double_value(vs_Lambda, j, "Lambda");
  set_double_value(vs_FieldCapacity, j, "FieldCapacity", transformIfPercent(j, "FieldCapacity"));
  set_double_value(vs_Saturation, j, "PoreVolume", transformIfPercent(j, "PoreVolume"));
  set_double_value(vs_PermanentWiltingPoint, j, "PermanentWiltingPoint",
                   transformIfPercent(j, "PermanentWiltingPoint"));
  set_string_value(vs_SoilTexture, j, "KA5TextureClass");
  set_double_value(vs_SoilAmmonium, j, "SoilAmmonium");
  set_double_value(vs_SoilNitrate, j, "SoilNitrate");
  set_double_value(vs_Soil_CN_Ratio, j, "CN");
  set_double_value(vs_SoilMoisturePercentFC, j, "SoilMoisturePercentFC");
  set_double_value(_vs_SoilRawDensity, j, "SoilRawDensity");
  set_double_value(_vs_SoilBulkDensity, j, "SoilBulkDensity");
  set_double_value(_vs_SoilOrganicCarbon, j, "SoilOrganicCarbon",
                   [](double soc) { return soc / 100.0; });
  set_double_value(_vs_SoilOrganicMatter, j, "SoilOrganicMatter",
                   transformIfPercent(j, "SoilOrganicMatter"));

  auto st = vs_SoilTexture;
  // use internally just uppercase chars
  vs_SoilTexture = Tools::toUpper(vs_SoilTexture);

  if (vs_SoilSandContent < 0 && !vs_SoilTexture.empty()) {
    auto res = KA5texture2sand(vs_SoilTexture);
    if (res.success()) vs_SoilSandContent = res.result;
    else es.append(res);
  }

  if (vs_SoilClayContent < 0 && !vs_SoilTexture.empty()) {
    auto res = KA5texture2clay(vs_SoilTexture);
    if (res.success()) vs_SoilClayContent = res.result;
    else es.append(res);
  }

  if (vs_SoilClayContent > 0 && vs_SoilSandContent > 0 && vs_SoilTexture.empty()) {
    vs_SoilTexture = sandAndClay2KA5texture(vs_SoilSandContent, vs_SoilClayContent);
  }

  // restrict sceleton to 80%, else FC, PWP and SAT could be calculated too low, so that the water transport algorithm gets unstable
  if (vs_SoilStoneContent > 0) vs_SoilStoneContent = min(vs_SoilStoneContent, 0.8);

  if (vs_FieldCapacity < 0 || vs_Saturation < 0 || vs_PermanentWiltingPoint < 0) {
    auto res = vs_SoilTexture.empty()
               ? fcSatPwpFromVanGenuchten(vs_SoilSandContent,
                                          vs_SoilClayContent,
                                          vs_SoilStoneContent,
                                          vs_SoilBulkDensity(),
                                          vs_SoilOrganicCarbon())
               : fcSatPwpFromKA5textureClass(vs_SoilTexture,
                                             vs_SoilStoneContent,
                                             vs_SoilRawDensity(),
                                             vs_SoilOrganicMatter());
    if (vs_FieldCapacity < 0) vs_FieldCapacity = res.fc;
    if (vs_Saturation < 0) vs_Saturation = res.sat;
    if (vs_PermanentWiltingPoint < 0) vs_PermanentWiltingPoint = res.pwp;
  }
  bool fcSatPwpSet = vs_FieldCapacity > 0 && vs_Saturation > 0 && vs_PermanentWiltingPoint > 0;

  // restrict FC, PWP and SAT else the water transport algorithm gets instable
  vs_FieldCapacity = max(0.05, vs_FieldCapacity);
  vs_PermanentWiltingPoint = max(0.01, vs_PermanentWiltingPoint);
  vs_Saturation = max(0.1, vs_Saturation);

  if (vs_Lambda < 0 && vs_SoilSandContent > 0 && vs_SoilClayContent > 0) {
    vs_Lambda = sandAndClay2lambda(vs_SoilSandContent, vs_SoilClayContent);
  }

  if (!vs_SoilTexture.empty() && KA5texture2sand(vs_SoilTexture).failure()) {
    es.appendError(kj::str("KA5TextureClass (", st, ") is unknown.").cStr());
  }
  //if (vs_SoilSandContent < 0 || vs_SoilSandContent > 1.0){
  //  es.appendError(kj::str("Sand content (", vs_SoilSandContent, ") is out of bounds [0, 1].").cStr());
  //}
  if (vs_SoilClayContent < 0 || vs_SoilClayContent > 1.0){
    es.appendError(kj::str("Clay content (", vs_SoilClayContent, ") is out of bounds [0, 1].").cStr());
  }
  if (vs_SoilpH < 0 || vs_SoilpH > 14){
    es.appendError(kj::str("pH value (", vs_SoilpH, ") is out of bounds [0, 14].").cStr());
  }
  if (vs_SoilStoneContent < 0 || vs_SoilStoneContent > 1.0){
    es.appendError(kj::str("Sceleton (", vs_SoilStoneContent, ") is out of bounds [0, 1].").cStr());
  }
  if (vs_FieldCapacity < 0 || vs_FieldCapacity > 1.0){
    es.appendError(kj::str("FieldCapacity (", vs_FieldCapacity, ") is out of bounds [0, 1].").cStr());
  }
  if (vs_Saturation < 0 || vs_Saturation > 1.0){
    es.appendError(kj::str("PoreVolume (", vs_Saturation, ") is out of bounds [0, 1].").cStr());
  }
  if (vs_PermanentWiltingPoint < 0 || vs_PermanentWiltingPoint > 1.0){
    es.appendError(kj::str("PermanentWiltingPoint (", vs_PermanentWiltingPoint, ") is out of bounds [0, 1].").cStr());
  }
  if (vs_SoilMoisturePercentFC < 0 || vs_SoilMoisturePercentFC > 100){
    es.appendError(kj::str("SoilMoisturePercentFC (", vs_SoilMoisturePercentFC, ") is out of bounds [0, 100].").cStr());
  }
  if (_vs_SoilBulkDensity < 0 && (_vs_SoilRawDensity < 0 || _vs_SoilRawDensity > 2000)){
    es.appendWarning(kj::str("SoilRawDensity (", _vs_SoilRawDensity, ") is out of bounds [0, 2000].").cStr());
  }
  if (_vs_SoilRawDensity < 0 && (_vs_SoilBulkDensity < 0 || _vs_SoilBulkDensity > 2000)){
    es.appendWarning(kj::str("SoilBulkDensity (", _vs_SoilBulkDensity, ") is out of bounds [0, 2000].").cStr());
  }
  if (_vs_SoilOrganicMatter < 0 && (_vs_SoilOrganicCarbon < 0 || _vs_SoilOrganicCarbon > 1.0)){
    es.appendError(kj::str("SoilOrganicCarbon content (", _vs_SoilOrganicCarbon, ") is out of bounds [0, 1].").cStr());
  }
  if (_vs_SoilOrganicCarbon < 0 && (_vs_SoilOrganicMatter < 0 || _vs_SoilOrganicMatter > 1.0)){
    es.appendError(kj::str("SoilOrganicMatter content (", _vs_SoilOrganicMatter, ") is out of bounds [0, 1].").cStr());
  }

  return es;
}

json11::Json SoilParameters::to_json() const {
  return J11Object{
      {"type",                  "SoilParameters"},
      {"Sand",                  J11Array{vs_SoilSandContent, "% [0-1]"}},
      {"Clay",                  J11Array{vs_SoilClayContent, "% [0-1]"}},
      {"pH",                    vs_SoilpH},
      {"Sceleton",              J11Array{vs_SoilStoneContent, "vol% [0-1] (m3 m-3)"}},
      {"Lambda",                vs_Lambda},
      {"FieldCapacity",         J11Array{vs_FieldCapacity, "vol% [0-1] (m3 m-3)"}},
      {"PoreVolume",            J11Array{vs_Saturation, "vol% [0-1] (m3 m-3)"}},
      {"PermanentWiltingPoint", J11Array{vs_PermanentWiltingPoint, "vol% [0-1] (m3 m-3)"}},
      {"KA5TextureClass",       vs_SoilTexture},
      {"SoilAmmonium",          J11Array{vs_SoilAmmonium, "kg NH4-N m-3"}},
      {"SoilNitrate",           J11Array{vs_SoilNitrate, "kg NO3-N m-3"}},
      {"CN",                    vs_Soil_CN_Ratio},
      {"SoilRawDensity",        J11Array{_vs_SoilRawDensity, "kg m-3"}},
      {"SoilBulkDensity",       J11Array{_vs_SoilBulkDensity, "kg m-3"}},
      {"SoilOrganicCarbon",     J11Array{_vs_SoilOrganicCarbon * 100.0, "mass% [0-100]"}},
      {"SoilOrganicMatter",     J11Array{_vs_SoilOrganicMatter, "mass% [0-1]"}},
      {"SoilMoisturePercentFC", J11Array{vs_SoilMoisturePercentFC, "% [0-100]"}}};
}

void CapillaryRiseRates::addRate(const std::string& soilType, size_t distance, double value) {
  //    std::cout << "Add cap rate: " << bodart.c_str() << "\tdist: " << distance << "\tvalue: " << value << std::endl;
  //cap_rates_map.insert(std::pair<std::string,std::map<int,double> >(bodart,std::pair<int,double>(distance,value)));
  capillaryRiseRates[soilType][distance] = value;
}

/**
   * Returns capillary rise rate for given soil type and distance to ground water.
   */
double CapillaryRiseRates::getRate(const std::string& soilType, size_t distance) const {
  auto it = capillaryRiseRates.find(soilType);
  if (it == capillaryRiseRates.end()) {
    it = capillaryRiseRates.find(soilType.substr(0, 3));
    if (it == capillaryRiseRates.end()) {
      it = capillaryRiseRates.find(soilType.substr(0, 2));
      if (it == capillaryRiseRates.end()) return 0.0;
    }
  }
  auto it2 = it->second.find(distance);
  if (it2 == it->second.end()) return 0.0;
  return it2->second;
}

const CapillaryRiseRates &Soil::readCapillaryRiseRates() {
  static mutex lockable;
  static bool initialized = false;
  static CapillaryRiseRates cap_rates;

  if (!initialized) {
    lock_guard<mutex> lock(lockable);

    if (!initialized) {
      auto cacheAllData = [](mas::schema::soil::CapillaryRiseRate::Reader data) {
        for (const auto &scd: data.getList()) {
          cap_rates.addRate(Tools::toUpper(scd.getSoilType()), scd.getDistance(), scd.getRate());
        }
      };

      auto fs = kj::newDiskFilesystem();
#ifdef _WIN32
      auto pathToMonicaParamsSoil = fs->getCurrentPath().evalWin32(replaceEnvVars("${MONICA_PARAMETERS}\\soil\\"));
#else
      auto pathToMonicaParamsSoil = fs->getCurrentPath().eval(replaceEnvVars("${MONICA_PARAMETERS}/soil/"));
#endif
      try {
        KJ_IF_MAYBE(file, fs->getRoot().tryOpenFile(pathToMonicaParamsSoil.append("CapillaryRiseRates.sercapnp"))) {
          auto allBytes = (*file)->readAllBytes();
          kj::ArrayInputStream aios(allBytes);
          capnp::InputStreamMessageReader message(aios);
          cacheAllData(message.getRoot<mas::schema::soil::CapillaryRiseRate>());
        } else KJ_IF_MAYBE(file2, fs->getRoot().tryOpenFile(pathToMonicaParamsSoil.append("CapillaryRiseRates.json"))) {
          capnp::JsonCodec json;
          capnp::MallocMessageBuilder msg;
          auto builder = msg.initRoot<mas::schema::soil::CapillaryRiseRate>();
          json.decode((*file2)->readAllBytes().asChars(), builder);
          cacheAllData(builder.asReader());
        }

        initialized = true;
      } catch (const kj::Exception& e) {
        cout << "Error: couldn't read CapillaryRiseRates.sercapnp or CapillaryRiseRates.json from folder "
             << pathToMonicaParamsSoil.toString().cStr() << " ! Exception: " << e.getDescription().cStr() << endl;
      }
      initialized = true;
    }
  }

  return cap_rates;
}

bool SoilParameters::isValid() const {
  bool is_valid = true;

  if (vs_FieldCapacity < 0) {
    debug() << "SoilParameters::Error: No field capacity defined in database for " << vs_SoilTexture
            << " , RawDensity: " << _vs_SoilRawDensity << endl;
    is_valid = false;
  }
  if (vs_Saturation < 0) {
    debug() << "SoilParameters::Error: No saturation defined in database for " << vs_SoilTexture << " , RawDensity: "
            << _vs_SoilRawDensity << endl;
    is_valid = false;
  }
  if (vs_PermanentWiltingPoint < 0) {
    debug() << "SoilParameters::Error: No saturation defined in database for " << vs_SoilTexture << " , RawDensity: "
            << _vs_SoilRawDensity << endl;
    is_valid = false;
  }

  if (vs_SoilSandContent < 0) {
    debug() << "SoilParameters::Error: Invalid soil sand content: " << vs_SoilSandContent << endl;
    is_valid = false;
  }

  if (vs_SoilClayContent < 0) {
    debug() << "SoilParameters::Error: Invalid soil clay content: " << vs_SoilClayContent << endl;
    is_valid = false;
  }

  if (vs_SoilpH < 0) {
    debug() << "SoilParameters::Error: Invalid soil ph value: " << vs_SoilpH << endl;
    is_valid = false;
  }

  if (vs_SoilStoneContent < 0) {
    debug() << "SoilParameters::Error: Invalid soil stone content: " << vs_SoilStoneContent << endl;
    is_valid = false;
  }

  if (vs_Saturation < 0) {
    debug() << "SoilParameters::Error: Invalid value for saturation: " << vs_Saturation << endl;
    is_valid = false;
  }

  if (vs_PermanentWiltingPoint < 0) {
    debug() << "SoilParameters::Error: Invalid value for permanent wilting point: " << vs_PermanentWiltingPoint << endl;
    is_valid = false;
  }
  /*
  if (_vs_SoilRawDensity<0) {
      cout << "SoilParameters::Error: Invalid soil raw density: "<< _vs_SoilRawDensity << endl;
      is_valid = false;
  }
  */
  return is_valid;
}

/**
 * @brief Returns raw density of soil
 * @return raw density of soil
 */
double SoilParameters::vs_SoilRawDensity() const {
  auto srd =
      _vs_SoilRawDensity < 0
      ? ((_vs_SoilBulkDensity / 1000.0) - (0.009 * 100.0 * vs_SoilClayContent)) * 1000.0
      : _vs_SoilRawDensity;

  return srd;
}

/**
* @brief Getter for soil bulk density.
* @return bulk density
*/
double SoilParameters::vs_SoilBulkDensity() const {
  auto sbd =
      _vs_SoilBulkDensity < 0
      ? ((_vs_SoilRawDensity / 1000.0) + (0.009 * 100.0 * vs_SoilClayContent)) * 1000.0
      : _vs_SoilBulkDensity;

  return sbd;
}

/**
 * @brief Returns soil organic carbon.
 * @return soil organic carbon
 */
double SoilParameters::vs_SoilOrganicCarbon() const {
  return _vs_SoilOrganicCarbon < 0
         ? _vs_SoilOrganicMatter * OrganicConstants::po_SOM_to_C
         : _vs_SoilOrganicCarbon;
}

/**
 * @brief Getter for soil organic matter.
 * @return Soil organic matter
 */
double SoilParameters::vs_SoilOrganicMatter() const {
  return _vs_SoilOrganicMatter < 0
         ? _vs_SoilOrganicCarbon / OrganicConstants::po_SOM_to_C
         : _vs_SoilOrganicMatter;
}

/**
 * @brief Returns lambda from soil texture
 *
 * @param lambda
 *
 * @return
 */
double SoilParameters::sandAndClay2lambda(double sand, double clay) {
  return ::sandAndClay2lambda(sand, clay);
}



std::pair<SoilPMs, Errors> Soil::createEqualSizedSoilPMs(const Tools::J11Array &jsonSoilPMs, double layerThickness, int numberOfLayers) {
  Errors errors;

  SoilPMs soilPMs;
  int layerCount = 0;
  for (size_t spi = 0, spsCount = jsonSoilPMs.size(); spi < spsCount; spi++) {
    const Json& sp = jsonSoilPMs.at(spi);

    //repeat layers if there is an associated Thickness parameter
    string err;
    int repeatLayer = 1;
    if (!sp["Thickness"].is_null()) {
      auto transf = transformIfNotMeters(sp, "Thickness");
      auto lt = transf(double_valueD(sp, "Thickness", layerThickness));
      auto noOfMonicaLayers = Tools::roundRT<int>(lt / layerThickness, 0);
      repeatLayer = min(max(1, noOfMonicaLayers), numberOfLayers - layerCount);
    }

    //simply repeat the last layer as often as necessary to fill the 20 layers
    if (spi + 1 == spsCount) repeatLayer = numberOfLayers - layerCount;

    for (int i = 1; i <= repeatLayer; i++) {
      SoilParameters sps;
      auto es = sps.merge(sp);
      soilPMs.push_back(sps);
      if (es.failure()) errors.append(es);
    }

    layerCount += repeatLayer;
  }

  return make_pair(soilPMs, errors);
}

std::pair<SoilPMs, Errors> Soil::createSoilPMs(const J11Array &jsonSoilPMs) {
  Errors errors;

  SoilPMs soilPMs;
  int layerCount = 0;
  for (const auto & sp : jsonSoilPMs) {
    SoilParameters sps;
    auto es = sps.merge(sp);
    auto transf = transformIfNotMeters(sp, "Thickness");
    auto lt = transf(double_valueD(sp, "Thickness", 0.1));
    sps.thickness = lt;
    soilPMs.push_back(sps);
    if (es.failure()) errors.append(es);
  }

  return make_pair(soilPMs, errors);
}

/*
string Soil::soilProfileId2KA5Layers(const string& abstractDbSchema,
                                     int soilProfileId)
{
  static mutex lockable;

  typedef map<int, string> Map;
  typedef map<string, Map> Map2;
  static bool initialized = false;
  static Map2 m;

  //yet unloaded schema
  if(initialized && m.find(abstractDbSchema) == m.end())
    initialized = false;

  if(!initialized)
  {
    lock_guard<mutex> lock(lockable);

    if(!initialized)
    {
      DBPtr con(newConnection(abstractDbSchema));
      con->setCharacterSet("utf8");
      DBRow row;

      Map& m2 = m[abstractDbSchema];

      con->select("SELECT id, KA5_texture_class "
          "from soil_profile "
                  "order by id, layer_depth");
      while(!(row = con->getRow()).empty())
      {
        string pre = m2[satoi(row[0])].empty() ? "" : "|";
        m2[satoi(row[0])].append(pre).append(row[1]);
      }

      initialized = true;
    }
  }

  auto ci2 = m.find(abstractDbSchema);
  if(ci2 != m.end())
  {
    Map& m2 = ci2->second;
    Map::const_iterator ci = m2.find(soilProfileId);
    return ci != m2.end() ? ci->second : "Soil profile not found!";
  }

  return "Soil profile database not found!";
}
*/

SoilPMsPtr Soil::soilParametersFromHermesFile(int soilId,
                                                    const string &pathToFile,
                                                    int layerThicknessCm,
                                                    int maxDepthCm,
                                                    double soil_ph,
                                                    double drainage_coeff) {
  debug() << pathToFile.c_str() << endl;
  int maxNoOfLayers = int(double(maxDepthCm) / double(layerThicknessCm));

  static mutex lockable;

  typedef map<int, SoilPMsPtr> Map;
  static bool initialized = false;
  static Map spss;
  if (!initialized) {
    lock_guard<mutex> lock(lockable);

    if (!initialized) {
      ifstream ifs(pathToFile.c_str(), ios::binary);
      string s;

      //skip first line(s)
      getline(ifs, s);

      int currenth = 1;
      while (getline(ifs, s)) {
        //cout << "s: " << s << endl;
        if (trim(s) == "end")
          break;

        //BdID Corg Bart UKT LD Stn C/N C/S Hy Wmx AzHo
        int ti;
        string ba, ts;
        int id, hu, ld, stone, cn, hcount;
        double corg, wmax;
        istringstream ss(s);
        ss >> id >> corg >> ba >> hu >> ld >> stone >> cn >> ts
           >> ti >> wmax >> hcount;

        //double vs_SoilSpecificMaxRootingDepth = wmax / 10.0; //[dm] --> [m]

        hu *= 10;
        //Reset horizont count to start new soil definition
        if (hcount > 0)
          currenth = 1;

        auto spsi = spss.find(soilId);
        SoilPMsPtr sps;
        if (spsi == spss.end()) {
          spss.insert(make_pair(soilId, sps = std::make_shared<SoilPMs>()));
        } else {
          sps = spsi->second;
        }
        assert(sps->size() <= INT_MAX); // assert that size of the vector can be converted into an int 
        int numSoilPMs = (int) sps->size();
        int ho = numSoilPMs * layerThicknessCm;
        int hsize = max(0, hu - ho);
        int subhcount = int(Tools::round(double(hsize) / double(layerThicknessCm)));
        if (currenth == hcount && (numSoilPMs + subhcount) < maxNoOfLayers)
          subhcount += maxNoOfLayers - numSoilPMs - subhcount;

        if ((ba != "Ss") && (ba != "Sl2") && (ba != "Sl3") && (ba != "Sl4") &&
            (ba != "Slu") && (ba != "St2") && (ba != "St3") && (ba != "Su2") &&
            (ba != "Su3") && (ba != "Su4") && (ba != "Ls2") && (ba != "Ls3") &&
            (ba != "Ls4") && (ba != "Lt2") && (ba != "Lt3") && (ba != "Lts") &&
            (ba != "Lu") && (ba != "Uu") && (ba != "Uls") && (ba != "Us") &&
            (ba != "Ut2") && (ba != "Ut3") && (ba != "Ut4") && (ba != "Tt") &&
            (ba != "Tl") && (ba != "Tu2") && (ba != "Tu3") && (ba != "Tu4") &&
            (ba != "Ts2") && (ba != "Ts3") && (ba != "Ts4") && (ba != "fS") &&
            (ba != "fS") && (ba != "fSms") && (ba != "fSgs") && (ba != "mS") &&
            (ba != "mSfs") && (ba != "mSgs") && (ba != "gS") && (ba != "Hh") &&
            (ba != "Hn")) {
          cerr << "No valid texture class defined: " << ba.c_str() << endl;
          exit(1);
        }

        SoilParameters p;
        p.set_vs_SoilOrganicCarbon(corg / 100.0); //[kg C 100kg] --> [kg C kg-1]
        auto ec = KA5texture2clay(ba);
        if (ec.success()) {
          p.vs_SoilClayContent = ec.result;

          auto erd = bulkDensityClass2rawDensity(ld, ec.result);
          if (erd.success())
            p.set_vs_SoilRawDensity(erd.result * 1000.0);
          else
            for (const auto& e: erd.errors)
              cout << e << endl;
        } else {
          for (const auto& e: ec.errors)
            cout << e << endl;
        }
        auto es = KA5texture2sand(ba);
        if (es.success())
          p.vs_SoilSandContent = es.result;
        else
          for (const auto& e: ec.errors)
            cout << e << endl;
        p.vs_SoilStoneContent = stone / 100.0;
        p.vs_Lambda = sandAndClay2lambda(p.vs_SoilSandContent, p.vs_SoilClayContent);
        p.vs_SoilTexture = ba;

        if (soil_ph != -1.0)
          p.vs_SoilpH = soil_ph;

        if (drainage_coeff != -1.0)
          p.vs_Lambda = drainage_coeff;

        // initialization of saturation, field capacity and perm. wilting point
        soilCharacteristicsKA5(p);

        bool valid_soil_params = p.isValid();
        if (!valid_soil_params) {
          cout << "Error in soil parameters. Aborting now simulation";
          exit(-1);
        }

        for (int i = 0; i < subhcount; i++)
          sps->push_back(p);
        currenth++;
      }

      initialized = true;

    }
  }

  static SoilPMsPtr nothing = make_shared<SoilPMs>();
  auto ci = spss.find(soilId);
  return ci != spss.end() ? ci->second : nothing;
}


RPSCDRes Soil::readPrincipalSoilCharacteristicData(const string& soilType, double rawDensity) {
  static mutex lockable;
  typedef map<int, RPSCDRes> M1;
  typedef map<string, M1> M2;
  static M2 m;
  static bool initialized = false;

  RPSCDRes errorRes;

  if (!initialized) {
    lock_guard<mutex> lock(lockable);

    if (!initialized) {
      auto cacheAllData = [](mas::schema::soil::SoilCharacteristicData::Reader data) {
        for (const auto &scd: data.getList()) {
          double ac = scd.getAirCapacity();
          double fc = scd.getFieldCapacity();
          double nfc = scd.getNFieldCapacity();

          RPSCDRes r(true);
          r.sat = ac + fc;
          r.fc = fc;
          r.pwp = fc - nfc;

          m[Tools::toUpper(scd.getSoilType())][int(scd.getSoilRawDensity() / 100.0)] = r;
        }
      };

      auto fs = kj::newDiskFilesystem();
#ifdef _WIN32
      auto pathToMonicaParamsSoil = fs->getCurrentPath().evalWin32(replaceEnvVars("${MONICA_PARAMETERS}\\soil\\"));
#else
      auto pathToMonicaParamsSoil = fs->getCurrentPath().eval(replaceEnvVars("${MONICA_PARAMETERS}/soil/"));
#endif
      try {
        KJ_IF_MAYBE(file, fs->getRoot().tryOpenFile(pathToMonicaParamsSoil.append("SoilCharacteristicData.sercapnp"))) {
          auto allBytes = (*file)->readAllBytes();
          kj::ArrayInputStream aios(allBytes);
          capnp::InputStreamMessageReader message(aios);
          cacheAllData(message.getRoot<mas::schema::soil::SoilCharacteristicData>());
        } else KJ_IF_MAYBE(file2,
                           fs->getRoot().tryOpenFile(pathToMonicaParamsSoil.append("SoilCharacteristicData.json"))) {
          capnp::JsonCodec json;
          capnp::MallocMessageBuilder msg;
          auto builder = msg.initRoot<mas::schema::soil::SoilCharacteristicData>();
          json.decode((*file2)->readAllBytes().asChars(), builder);
          cacheAllData(builder.asReader());
        }

        initialized = true;
      }
      catch (const kj::Exception& e) {
        cout << "Error: couldn't read SoilCharacteristicData.sercapnp or SoilCharacteristicData.json from folder "
             << pathToMonicaParamsSoil.toString().cStr() << " ! Exception: " << e.getDescription().cStr() << endl;
      }
    }
  }

  auto ci = m.find(soilType);
  if (ci != m.end()) {
    int rd10 = int(rawDensity * 10);
    int delta = rd10 < 15 ? 2 : -2;

    M1::const_iterator ci2;
    //if we didn't find values for a given raw density, e.g. 1.1 (= 11)
    //we try to find the closest next one (up (1.1) or down (1.9))
    while ((ci2 = ci->second.find(rd10)) == ci->second.end() &&
           (11 <= rd10 && rd10 <= 19))
      rd10 += delta;

    return ci2 != ci->second.end() ? ci2->second : errorRes;
  }

  return errorRes;
}

RPSCDRes Soil::readSoilCharacteristicModifier(const string& soilType, double organicMatter) {
  static mutex lockable;
  typedef map<int, RPSCDRes> M1;
  typedef map<string, M1> M2;
  static M2 m;
  static bool initialized = false;
  if (!initialized) {
    lock_guard<mutex> lock(lockable);

    if (!initialized) {
      auto cacheAllData = [](mas::schema::soil::SoilCharacteristicModifier::Reader data) {
        for (const auto &scd: data.getList()) {
          double ac = scd.getAirCapacity();
          double fc = scd.getFieldCapacity();
          double nfc = scd.getNFieldCapacity();

          RPSCDRes r(true);
          r.sat = ac + fc;
          r.fc = fc;
          r.pwp = fc - nfc;

          m[Tools::toUpper(scd.getSoilType())][int(scd.getOrganicMatter() * 10)] = r;
        }
      };

      auto fs = kj::newDiskFilesystem();
#ifdef _WIN32
      auto pathToMonicaParamsSoil = fs->getCurrentPath().evalWin32(replaceEnvVars("${MONICA_PARAMETERS}\\soil\\"));
#else
      auto pathToMonicaParamsSoil = fs->getCurrentPath().eval(replaceEnvVars("${MONICA_PARAMETERS}/soil/"));
#endif
      try {
        KJ_IF_MAYBE(file,
                    fs->getRoot().tryOpenFile(pathToMonicaParamsSoil.append("SoilCharacteristicModifier.sercapnp"))) {
          auto allBytes = (*file)->readAllBytes();
          kj::ArrayInputStream aios(allBytes);
          capnp::InputStreamMessageReader message(aios);
          cacheAllData(message.getRoot<mas::schema::soil::SoilCharacteristicModifier>());
        } else KJ_IF_MAYBE(file2, fs->getRoot().tryOpenFile(
            pathToMonicaParamsSoil.append("SoilCharacteristicModifier.json"))) {
          capnp::JsonCodec json;
          capnp::MallocMessageBuilder msg;
          auto builder = msg.initRoot<mas::schema::soil::SoilCharacteristicModifier>();
          json.decode((*file2)->readAllBytes().asChars(), builder);
          cacheAllData(builder.asReader());
        }

        initialized = true;
      }
      catch (const kj::Exception& e) {
        cout
            << "Error: couldn't read SoilCharacteristicModifier.sercapnp or SoilCharacteristicModifier.json from folder "
            << pathToMonicaParamsSoil.toString().cStr() << " ! Exception: " << e.getDescription().cStr() << endl;
      }
    }
  }

  auto ci = m.find(Tools::toUpper(soilType));
  if (ci != m.end()) {
    auto ci2 = ci->second.find(int(organicMatter * 10));
    return ci2 != ci->second.end() ? ci2->second : RPSCDRes();
  }

  return {};
}

void Soil::soilCharacteristicsKA5(SoilParameters &sp) {
  auto res = fcSatPwpFromKA5textureClass(sp.vs_SoilTexture,
                                         sp.vs_SoilStoneContent,
                                         sp.vs_SoilRawDensity(),
                                         sp.vs_SoilOrganicMatter());
  sp.vs_FieldCapacity = res.fc;
  sp.vs_Saturation = res.sat;
  sp.vs_PermanentWiltingPoint = res.pwp;
}

FcSatPwp Soil::fcSatPwpFromKA5textureClass(std::string texture,
                                           double stoneContent,
                                           double soilRawDensity,
                                           double soilOrganicMatter) {
  debug() << "soilCharacteristicsKA5" << endl;
  texture = Tools::toUpper(texture);

  FcSatPwp res;

  if (!texture.empty()) {
    double srd = soilRawDensity / 1000.0; // [kg m-3] -> [g cm-3]
    double som = soilOrganicMatter * 100.0; // [kg kg-1] -> [%]

    // ***************************************************************************
    // *** The following boundaries are extracted from:            ***
    // *** Wessolek, G., M. Kaupenjohann, M. Renger (2009) Bodenphysikalische  ***
    // *** Kennwerte und Berechnungsverfahren für die Praxis. Bodenökologie  ***
    // *** und Bodengenese 40, Selbstverlag Technische Universität Berlin    ***
    // *** (Tab. 4).                               ***
    // ***************************************************************************

    double srd_lowerBound = 0.0;
    double srd_upperBound = 0.0;
    if (srd < 1.1) {
      srd_lowerBound = 1.1;
      srd_upperBound = 1.1;
    } else if ((srd >= 1.1) && (srd < 1.3)) {
      srd_lowerBound = 1.1;
      srd_upperBound = 1.3;
    } else if ((srd >= 1.3) && (srd < 1.5)) {
      srd_lowerBound = 1.3;
      srd_upperBound = 1.5;
    } else if ((srd >= 1.5) && (srd < 1.7)) {
      srd_lowerBound = 1.5;
      srd_upperBound = 1.7;
    } else if ((srd >= 1.7) && (srd < 1.9)) {
      srd_lowerBound = 1.7;
      srd_upperBound = 1.9;
    } else if (srd >= 1.9) {
      srd_lowerBound = 1.9;
      srd_upperBound = 1.9;
    }

    // special treatment for "torf" soils
    if (texture == "HH" || texture == "HN") {
      srd_lowerBound = -1;
      srd_upperBound = -1;
    }

    // Boundaries for linear interpolation
    auto lbRes = readPrincipalSoilCharacteristicData(texture, srd_lowerBound);
    double sat_lowerBound = lbRes.sat;
    double fc_lowerBound = lbRes.fc;
    double pwp_lowerBound = lbRes.pwp;

    auto ubRes = readPrincipalSoilCharacteristicData(texture, srd_upperBound);
    double sat_upperBound = ubRes.sat;
    double fc_upperBound = ubRes.fc;
    double pwp_upperBound = ubRes.pwp;

    if (lbRes.initialized && ubRes.initialized) {
      //  cout << "Soil Raw Density:\t" << vs_SoilRawDensity << endl;
      //  cout << "Saturation:\t\t" << vs_SaturationLowerBoundary << "\t" << vs_SaturationUpperBoundary << endl;
      //  cout << "Field Capacity:\t" << vs_FieldCapacityLowerBoundary << "\t" << vs_FieldCapacityUpperBoundary << endl;
      //  cout << "PermanentWP:\t" << vs_PermanentWiltingPointLowerBoundary << "\t" << vs_PermanentWiltingPointUpperBoundary << endl;
      //  cout << "Soil Organic Matter:\t" << vs_SoilOrganicMatter << endl;

      // ***************************************************************************
      // *** The following boundaries are extracted from:            ***
      // *** Wessolek, G., M. Kaupenjohann, M. Renger (2009) Bodenphysikalische  ***
      // *** Kennwerte und Berechnungsverfahren für die Praxis. Bodenökologie  ***
      // *** und Bodengenese 40, Selbstverlag Technische Universität Berlin    ***
      // *** (Tab. 5).                               ***
      // ***************************************************************************

      double som_lowerBound = 0.0;
      double som_upperBound = 0.0;

      if (som >= 0.0 && som < 1.0) {
        som_lowerBound = 0.0;
        som_upperBound = 0.0;
      } else if (som >= 1.0 && som < 1.5) {
        som_lowerBound = 0.0;
        som_upperBound = 1.5;
      } else if (som >= 1.5 && som < 3.0) {
        som_lowerBound = 1.5;
        som_upperBound = 3.0;
      } else if (som >= 3.0 && som < 6.0) {
        som_lowerBound = 3.0;
        som_upperBound = 6.0;
      } else if (som >= 6.0 && som < 11.5) {
        som_lowerBound = 6.0;
        som_upperBound = 11.5;
      } else if (som >= 11.5) {
        som_lowerBound = 11.5;
        som_upperBound = 11.5;
      }

      // special treatment for "torf" soils
      if (texture == "HH" || texture == "HN") {
        som_lowerBound = 0.0;
        som_upperBound = 0.0;
      }

      // Boundaries for linear interpolation
      double fc_mod_lowerBound = 0.0;
      double sat_mod_lowerBound = 0.0;
      double pwp_mod_lowerBound = 0.0;
      // modifier values are given only for organic matter > 1.0% (class h2)
      if (som_lowerBound != 0.0) {
        auto lbRes2 = readSoilCharacteristicModifier(texture, som_lowerBound);
        sat_mod_lowerBound = lbRes2.sat;
        fc_mod_lowerBound = lbRes2.fc;
        pwp_mod_lowerBound = lbRes2.pwp;
      }

      double fc_mod_upperBound = 0.0;
      double sat_mod_upperBound = 0.0;
      double pwp_mod_upperBound = 0.0;
      if (som_upperBound != 0.0) {
        auto ubRes2 = readSoilCharacteristicModifier(texture, som_upperBound);
        sat_mod_upperBound = ubRes2.sat;
        fc_mod_upperBound = ubRes2.fc;
        pwp_mod_upperBound = ubRes2.pwp;
      }

      //   cout << "Saturation-Modifier:\t" << sat_mod_lowerBound << "\t" << sat_mod_upperBound << endl;
      //   cout << "Field capacity-Modifier:\t" << fc_mod_lowerBound << "\t" << fc_mod_upperBound << endl;
      //   cout << "PWP-Modifier:\t" << pwp_mod_lowerBound << "\t" << pwp_mod_upperBound << endl;

      // Linear interpolation
      double fc_unmod = fc_lowerBound;
      if (fc_upperBound < 0.5 && fc_lowerBound >= 1.0) fc_unmod = fc_lowerBound;
      else if (fc_lowerBound < 0.5 && fc_upperBound >= 1.0) fc_unmod = fc_upperBound;
      else if (srd_upperBound != srd_lowerBound) {
        fc_unmod = (srd - srd_lowerBound) /
                   (srd_upperBound - srd_lowerBound) *
                   (fc_upperBound - fc_lowerBound) + fc_lowerBound;
      }

      double sat_unmod = sat_lowerBound;
      if (sat_upperBound < 0.5 && sat_lowerBound >= 1.0) sat_unmod = sat_lowerBound;
      else if (sat_lowerBound < 0.5 && sat_upperBound >= 1.0) sat_unmod = sat_upperBound;
      else if (srd_upperBound != srd_lowerBound) {
        sat_unmod = (srd - srd_lowerBound) /
                    (srd_upperBound - srd_lowerBound) *
                    (sat_upperBound - sat_lowerBound) + sat_lowerBound;
      }

      double pwp_unmod = pwp_lowerBound;
      if (pwp_upperBound < 0.5 && pwp_lowerBound >= 1.0) pwp_unmod = pwp_lowerBound;
      else if (pwp_lowerBound < 0.5 && pwp_upperBound >= 1.0) pwp_unmod = pwp_upperBound;
      else if (srd_upperBound != srd_lowerBound) {
        pwp_unmod = (srd - srd_lowerBound) /
                    (srd_upperBound - srd_lowerBound) *
                    (pwp_upperBound - pwp_lowerBound) + pwp_lowerBound;
      }

      //in this case upper and lower boundary are equal, so doesn't matter.
      double fc_mod = fc_mod_lowerBound;
      double sat_mod = sat_mod_lowerBound;
      double pwp_mod = pwp_mod_lowerBound;
      if (som_upperBound != som_lowerBound) {
        fc_mod = (som - som_lowerBound) /
                 (som_upperBound - som_lowerBound) *
                 (fc_mod_upperBound - fc_mod_lowerBound) + fc_mod_lowerBound;

        sat_mod = (som - som_lowerBound) /
                  (som_upperBound - som_lowerBound) *
                  (sat_mod_upperBound - sat_mod_lowerBound) + sat_mod_lowerBound;

        pwp_mod = (som - som_lowerBound) /
                  (som_upperBound - som_lowerBound) *
                  (pwp_mod_upperBound - pwp_mod_lowerBound) + pwp_mod_lowerBound;
      }

      // Modifying the principal values by organic matter
      res.fc = (fc_unmod + fc_mod) / 100.0; // [m3 m-3]
      res.sat = (sat_unmod + sat_mod) / 100.0; // [m3 m-3]
      res.pwp = (pwp_unmod + pwp_mod) / 100.0; // [m3 m-3]

      // Modifying the principal values by stone content
      res.fc *= (1.0 - stoneContent);
      res.sat *= (1.0 - stoneContent);
      res.pwp *= (1.0 - stoneContent);
    }
  }

  debug() << "SoilTexture:\t\t\t" << texture << endl;
  debug() << "Saturation:\t\t\t" << res.sat << endl;
  debug() << "FieldCapacity:\t\t" << res.fc << endl;
  debug() << "PermanentWiltingPoint:\t" << res.pwp << endl << endl;

  return res;
}


FcSatPwp Soil::fcSatPwpFromVanGenuchten(double sandContent,
                                        double clayContent,
                                        double stoneContent,
                                        double soilBulkDensity,
                                        double soilOrganicCarbon) {
  FcSatPwp res;

  //cout << "Permanent Wilting Point is calculated from van Genuchten parameters" << endl;
  res.pwp = (0.015 + 0.5 * clayContent + 1.4 * soilOrganicCarbon) * (1.0 - stoneContent);

  res.sat = (0.81 - 0.283 * (soilBulkDensity / 1000.0) + 0.1 * clayContent) * (1.0 - stoneContent);

  //  cout << "Field capacity is calculated from van Genuchten parameters" << endl;
  double thetaR = res.pwp;
  double thetaS = res.sat;

  double vanGenuchtenAlpha = exp(-2.486
                                 + 2.5 * sandContent
                                 - 35.1 * soilOrganicCarbon
                                 - 2.617 * (soilBulkDensity / 1000.0)
                                 - 2.3 * clayContent);

  double vanGenuchtenM = 1.0;

  double vanGenuchtenN = exp(0.053
                             - 0.9 * sandContent
                             - 1.3 * clayContent
                             + 1.5 * (pow(sandContent, 2.0)));

  //***** Van Genuchten retention curve to calculate volumetric water content at
  //***** moisture equivalent (Field capacity definition KA5)

  double fieldCapacity_pF = 2.1;
  if (sandContent > 0.48 && sandContent <= 0.9 && clayContent <= 0.12) fieldCapacity_pF = 2.1 - (0.476 * (sandContent - 0.48));
  else if (sandContent > 0.9 && clayContent <= 0.05) fieldCapacity_pF = 1.9;
  else if (clayContent > 0.45) fieldCapacity_pF = 2.5;
  else if (clayContent > 0.30 && sandContent < 0.2) fieldCapacity_pF = 2.4;
  else if (clayContent > 0.35) fieldCapacity_pF = 2.3;
  else if (clayContent > 0.25 && sandContent < 0.1) fieldCapacity_pF = 2.3;
  else if (clayContent > 0.17 && sandContent > 0.68) fieldCapacity_pF = 2.2;
  else if (clayContent > 0.17 && sandContent < 0.33) fieldCapacity_pF = 2.2;
  else if (clayContent > 0.08 && sandContent < 0.27) fieldCapacity_pF = 2.2;
  else if (clayContent > 0.25 && sandContent < 0.25) fieldCapacity_pF = 2.2;

  double matricHead = pow(10, fieldCapacity_pF);

  res.fc = (thetaR + ((thetaS - thetaR) /
      (pow(1.0 + pow(vanGenuchtenAlpha * matricHead, vanGenuchtenN), vanGenuchtenM))))
          * (1.0 - stoneContent);

  return res;
}
