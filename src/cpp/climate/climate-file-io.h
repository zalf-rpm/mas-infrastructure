/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg@zalf.de>

Maintainers:
Currently maintained by the authors.

This file is part of the util library used by models created at the Institute of
Landscape Systems Analysis at the ZALF.
Copyright (C) Leibniz Centre for Agricultural Landscape Research (ZALF)
*/

#pragma once

#include <string>
#include <vector>
#include <istream>

#include "kj/io.h"
#include "kj/function.h"

#include "json11/json11-helper.h"
#include "tools/helper.h"
#include "climate-common.h"
#include "common/dll-exports.h"

namespace Climate {

//inline std::vector<ACD> defaultHeader()
//{
//	return{deDate, tmin, tavg, tmax, precip, globrad, relhumid, wind};
//}

struct CSVViaHeaderOptions : public Tools::Json11Serializable {
  CSVViaHeaderOptions();

  CSVViaHeaderOptions(json11::Json object);

  virtual Tools::Errors merge(json11::Json j);

  virtual json11::Json to_json() const;

  std::string separator;
  Tools::Date startDate;
  Tools::Date endDate;
  int noOfHeaderLines{1};
  int lineNoOfHeaderLine{-1};
  int lineNoOfDataStart{-2};
  int lineNoOfDataEnd{-1};
  double latitude{0};
  std::vector<std::string> header;
  std::map<std::string, std::string> headerName2ACDName;
  std::map<std::string, std::pair<std::string, double>> convert;
  std::map<Climate::ACD, std::function<double(double)>> convertFn;
  std::string datePattern;
};

Tools::EResult<Climate::DataAccessor>
readClimateDataFromCSVInputStreamViaHeaders(std::istream &inputStream,
                                            CSVViaHeaderOptions options = CSVViaHeaderOptions(),
                                            bool strictDateChecking = true);

Tools::EResult<Climate::DataAccessor>
readClimateDataFromCSVInputStreamViaHeaders(kj::InputStream &inputStream,
                                            CSVViaHeaderOptions options = CSVViaHeaderOptions(),
                                            bool strictDateChecking = true);

Tools::EResult<Climate::DataAccessor>
readClimateDataFromCSVFileViaHeaders(std::string pathToFile,
                                     const CSVViaHeaderOptions& options = CSVViaHeaderOptions(),
                                     bool strictDateChecking = true);

Tools::EResult<Climate::DataAccessor>
readClimateDataFromCSVFilesViaHeaders(const std::vector<std::string>& pathsToFiles,
                                      const CSVViaHeaderOptions& options = CSVViaHeaderOptions());

Tools::EResult<Climate::DataAccessor>
readClimateDataFromCSVStringViaHeaders(const std::string& csvString,
                                       CSVViaHeaderOptions options = CSVViaHeaderOptions());

Tools::EResult<Climate::DataAccessor>
readClimateDataFromCSVInputStream(std::istream &inputStream,
                                  std::vector<ACD> header,
                                  const CSVViaHeaderOptions& options,
                                  bool strictDateChecking = true);

} // namespace Climate

extern "C" DLL_API char *Climate_readClimateDataFromCSVStringViaHeaders(const char *csvString, const char *options);
extern "C" DLL_API void Climate_freeCString(char *str);

