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

#include "climate-file-io.h"

#include <map>
#include <sstream>
#include <iostream>
#include <fstream>
#include <utility>
#include <cassert>
#include <mutex>
#include <cstdlib>

#include <kj/filesystem.h>
#include "kj/compat/gzip.h"

#include "climate-common.h"
#include "tools/algorithms.h"
#include "tools/debug.h"

#include "common/common.h"

using namespace std;
using namespace Tools;
using namespace Climate;

CSVViaHeaderOptions::CSVViaHeaderOptions()
  : separator(",") {}
  
CSVViaHeaderOptions::CSVViaHeaderOptions(json11::Json j) {
  merge(j);
}

Tools::Errors CSVViaHeaderOptions::merge(json11::Json j) {
  Tools::Errors errors;

  map<string, string> headerNames;
  for (const auto& p : j["header-to-acd-names"].object_items()) {
    if (p.second.is_array() && p.second.array_items().size() == 3) {
      headerNames[p.first] = p.second[0].string_value();
      convert[p.first] = make_pair(p.second[1].string_value(), p.second[2].number_value());
    } else {
      headerNames[p.first] = p.second.string_value();
    }
  }
  headerName2ACDName = headerNames;
  set_string_valueD(separator, j, "csv-separator", ",");

  set_iso_date_value(startDate, j, "start-date");
  set_iso_date_value(endDate, j, "end-date");

  set_double_value(latitude, j, "latitude");
  //is being used and necessary when globrad is not available and calculated from sunshine hours

  lineNoOfDataStart = int_valueD(j, "line-no-of-data-start", lineNoOfDataStart);

  set_string_vector(header, j, "header");
  if (header.empty()) {
    noOfHeaderLines = int_valueD(j, "no-of-climate-file-header-lines", noOfHeaderLines);
    lineNoOfHeaderLine = int_valueD(j, "line-no-of-header-line", lineNoOfHeaderLine);
    if (lineNoOfHeaderLine < 0) lineNoOfHeaderLine = 1;
    if (lineNoOfDataStart < 0) lineNoOfDataStart = lineNoOfHeaderLine + noOfHeaderLines;
  } else {
    lineNoOfHeaderLine = -1;
    if (lineNoOfDataStart < 0) lineNoOfDataStart = 1;
  }

  lineNoOfDataEnd = int_valueD(j, "line-no-of-data-end", lineNoOfDataEnd);
  if (lineNoOfDataEnd > 0 && lineNoOfDataEnd < lineNoOfDataStart){
    lineNoOfDataEnd = -1;
    errors.appendWarning("line-no-of-data-end must be greater than line-no-of-data-start. Ignoring line-no-of-data-end.");
  }

  return{};
}

json11::Json CSVViaHeaderOptions::to_json() const {
  J11Object headerNames;
  for (const auto& p : headerName2ACDName) {
    auto it = convert.find(p.first);
    if (it != convert.end()) {
      headerNames[p.first] = J11Array{ p.second, it->second.first, it->second.second };
    } else {
      headerNames[p.first] = p.second;
    }
  }

  J11Object convert_;
  for (const auto& p : convert) convert_[p.first] = J11Array{ p.second.first, p.second.second };

  return json11::Json::object
  { {"type", "CSVViaHeaderOptions"}
  ,{"csv-separator", separator}
  ,{"start-date", startDate.toIsoDateString()}
  ,{"end-date", endDate.toIsoDateString()}
  ,{"no-of-climate-file-header-lines", noOfHeaderLines}
  ,{"line-no-of-header-line", lineNoOfHeaderLine}
  ,{"line-no-of-data-start", lineNoOfDataStart}
  ,{"line-no-of-data-end", lineNoOfDataEnd}
  ,{"header-to-acd-names", headerNames}
  ,{"latitude", latitude}
  ,{"header", header}
  };
}

Tools::EResult<Climate::DataAccessor>
Climate::readClimateDataFromCSVInputStreamViaHeaders(istream& is,
                                                     CSVViaHeaderOptions options,
                                                     bool strictDateChecking) {
  if (!is.good()) {
    return {DataAccessor(), "Input stream not good!"};
  }

  vector<ACD> header;
  string s;
  if (options.noOfHeaderLines > 0 || !options.header.empty()){
    for (int i = 1; i <= options.lineNoOfHeaderLine; ++i) getline(is, s);

    vector<string> r = options.header.empty() ? splitString(s, options.separator) : options.header;
    if (r.back().empty()) r.pop_back();

    //remove possible \r at the end of the last element, when reading Windows files under linux
    if (r.back().back() == '\r') r.back().pop_back();
    auto n2acd = name2acd();
    for (const auto& colName : r) {
      auto tcn = trim(colName);
      auto replColName = options.headerName2ACDName[tcn];
      auto acdi = n2acd.find(replColName.empty() ? tcn : replColName);
      header.push_back(acdi == n2acd.end() ? skip : acdi->second);

      if (!options.convert.empty()) {
        auto it = options.convert.find(tcn);
        if (it != options.convert.end()) {
          auto acd = n2acd[replColName.empty() ? tcn : replColName];
          auto op = it->second.first;
          double value = it->second.second;
          if (op == "*") options.convertFn[acd] = [=](double d) { return d * value; };
          else if (op == "/") options.convertFn[acd] = [=](double d) { return d / value; };
          else if (op == "+") options.convertFn[acd] = [=](double d) { return d + value; };
          else if (op == "-") options.convertFn[acd] = [=](double d) { return d - value; };
        }
      }
    }
  }

  if (header.empty()) {
    stringstream oss;
    oss << "Couldn't match any column names to internally used names. "
      << "Read CSV header line was: " << s;
    return {DataAccessor(), oss.str()};
  }

  return readClimateDataFromCSVInputStream(is.seekg(0), header, options, strictDateChecking);
}

Tools::EResult<Climate::DataAccessor>
Climate::readClimateDataFromCSVFileViaHeaders(std::string pathToFile,
                                              const CSVViaHeaderOptions& options,
                                              bool strictDateChecking) {
  pathToFile = fixSystemSeparator(pathToFile);
  if (pathToFile.substr(pathToFile.size() - 3) == ".gz") {
    auto fs = kj::newDiskFilesystem();
    auto file = isAbsolutePath(pathToFile)
                ? fs->getRoot().openFile(fs->getCurrentPath().eval(pathToFile))
                : fs->getRoot().openFile(kj::Path::parse(pathToFile));
#ifdef _WIN32
    KJ_IF_MAYBE(fh, file->getWin32Handle()){
      kj::HandleInputStream his(*fh);
      kj::GzipInputStream gis(his);
      auto all = gis.readAllText();
      std::istringstream iss(all.cStr());
      return readClimateDataFromCSVInputStreamViaHeaders(iss, options);
    }
#else
    KJ_IF_MAYBE(fd, file->getFd()) {
      kj::FdInputStream fis(*fd);
      kj::GzipInputStream gis(fis);
      auto all = gis.readAllText();
      std::istringstream iss(all.cStr());
      return readClimateDataFromCSVInputStreamViaHeaders(iss, options, strictDateChecking);
    }
#endif
    stringstream oss;
    oss << "Could not open climate file " << pathToFile << ".";
    return {DataAccessor(), oss.str()};
  } else {
    ifstream ifs(pathToFile.c_str());
    if (!ifs.good()) {
      stringstream oss;
      oss << "Could not open climate file " << pathToFile << ".";
      return {DataAccessor(), oss.str()};
    }
    return readClimateDataFromCSVInputStreamViaHeaders(ifs, options, strictDateChecking);
  }
}


Tools::EResult<Climate::DataAccessor>
Climate::readClimateDataFromCSVFilesViaHeaders(const std::vector<std::string>& pathsToFiles,
                                               const CSVViaHeaderOptions& options) {
  Errors es;
  Climate::DataAccessor finalDA;

  for (const auto& pathToFile : pathsToFiles) {
    if (pathToFile.find("capnp") == 0) continue; // skip capnproto sturdy refs

    auto eda = readClimateDataFromCSVFileViaHeaders(pathToFile, options, false);

    if (!finalDA.isValid()) {
      finalDA = kj::mv(eda.result);
    } else {
      finalDA.mergeClimateData(kj::mv(eda.result), true);
    }
    if (eda.failure()) es.append(eda);
  }

  if (options.startDate.isValid() && options.endDate.isValid()) {
    int noOfDays = options.endDate - options.startDate + 1;
    if (finalDA.noOfStepsPossible() < size_t(noOfDays)) {
      stringstream oss;
      oss << "Read time-series data between " << options.startDate.toIsoDateString()
        << " and " << options.endDate.toIsoDateString()
        << " (" << noOfDays << " days) is incomplete. There are just "
        << finalDA.noOfStepsPossible() << " days in read dataset.";
      es.append(oss.str());
      return {DataAccessor(), es};
    }
  }

  return {finalDA, es};
}

Tools::EResult<Climate::DataAccessor>
Climate::readClimateDataFromCSVStringViaHeaders(const std::string& csvString,
                                                CSVViaHeaderOptions options) {
  istringstream iss(csvString);
  if (!iss.good()) return {DataAccessor(), "Could not access input string stream!"};
  return readClimateDataFromCSVInputStreamViaHeaders(iss, kj::mv(options));
}


Tools::EResult<Climate::DataAccessor>
Climate::readClimateDataFromCSVInputStream(std::istream& is,
                                           std::vector<ACD> header,
                                           const CSVViaHeaderOptions& options,
                                           bool strictDateChecking) {
  Errors es;

  string separator = options.separator;
  Date startDate = options.startDate;
  Date endDate = options.endDate;
  std::map<ACD, std::function<double(double)>> convert = options.convertFn;

  if (!is.good()) return {DataAccessor(), "Climate data error: Couldn't read climate data! (Input stream not good.)"};

  bool isStartDateValid = startDate.isValid();
  bool isEndDateValid = endDate.isValid();

  //we store all data in a map to also manage csv files with wrong order
  map<Date, map<ACD, double>> data;

  string s;
  int lineNo = 1;
  while (getline(is, s)) {
    if (lineNo < options.lineNoOfDataStart) {
      ++lineNo;
      continue;
    }
    if (options.lineNoOfDataEnd > 0 && lineNo > options.lineNoOfDataEnd) break;

    vector<string> r = splitString(s, separator);
    if (r.back().empty()) r.pop_back();
    //remove possible \r at the end of the last element, when reading Windows files under linux
    if (r.back().back() == '\r') r.back().pop_back();
    size_t rSize = r.size();
    size_t hSize = header.size();
    if (rSize < header.size()) {
      stringstream oss;
      oss
        << "Climate data: Skipping line: " << endl
        << s << endl
        << "because of less (" << r.size() << ") than expected ("
        << header.size() << ") elements.";
      debug() << oss.str();
      es.appendWarning(oss.str());
      continue;
    }

    Date date;
    vector<bool> usedVs(availableClimateDataSize(), false);
    vector<double> vs(availableClimateDataSize(), false);
    try {
      for (size_t i = 0; i < hSize; i++) {
        ACD acdi = header.at(i);
        bool doConvert = !convert.empty() && convert[acdi];
        switch (acdi) {
          case day: date.setDay((uint8_t)stoul(r.at(i))); break;
          case month: date.setMonth((uint8_t)stoul(r.at(i))); break;
          case year: date.setYear((uint16_t)stoul(r.at(i))); break;
          case isoDate: date = Date::fromIsoDateString(r.at(i)); break;
          case deDate:
          {
            auto dmy = splitString(r.at(i), ".");
            if (dmy.size() == 3) {
              date.setDay((uint8_t)stoul(dmy.at(0)));
              date.setMonth((uint8_t)stoul(dmy.at(1)));
              date.setYear((uint16_t)stoul(dmy.at(2)));
            }
            break;
          }
          case skip: break; //ignore element
          default:
          {
            auto v = stod(r.at(i));
            vs[acdi] = doConvert ? convert[acdi](v) : v;
            usedVs[acdi] = true;
          }
        }
      }
    } catch (const invalid_argument& e) {
      stringstream oss;
      oss << "Climate data error: Error converting one of the (climate) elements in the line: " << endl << s;
      es.appendError(oss.str());
      return {DataAccessor(), es};
    }

    if (isStartDateValid && date < startDate) continue;
    if (isEndDateValid && date > endDate) continue;

    if (!date.isValid()) {
      stringstream oss;
      oss << "Climate data error: Date is missing or not valid. Ignoring line: " << endl << s;
      debug() << oss.str();
      es.appendWarning(oss.str());
      continue;
    }

    if (!usedVs[tmin] ||
        !usedVs[tmax] ||
        !usedVs[precip]) {
      stringstream oss;
      oss << "Climate data error: One of [tmin, tmax, precip] is missing. Ignoring line: " << endl << s;
      debug() << oss.str();
      es.appendWarning(oss.str());
      continue;
    }

    //if we miss the average air temperature, but have the daily minimum and maximum, then calculate the average from these
    if (!usedVs[tavg]) {
      vs[tavg] = (vs[tmin] + vs[tmax]) / 2.0;
      usedVs[tavg] = true;
    }

    if (!usedVs[globrad] && usedVs[sunhours]) {
      vs[globrad] = Tools::sunshine2globalRadiation(date.julianDay(), vs[sunhours], options.latitude, true);
      usedVs[globrad] = true;
    } else if (!usedVs[globrad]) {
      stringstream oss;
      oss << "Climate data error: Globrad and sunhours is missing. Ignoring line: " << endl << s;
      debug() << oss.str();
      es.appendWarning(oss.str());
      continue;
    }

    map<ACD, double> vsm;
    for (size_t i = 0, size = usedVs.size(); i < size; i++) if (usedVs[i]) vsm[ACD(i)] = vs[i];
    assert(vsm.size() >= 5);
    data[date] = vsm;
  }

  if (data.empty()) {
    es.appendError("Climate data error: No data could be read from file!");
    return {DataAccessor(), es};
  }

  //if we have no dates or don't do strict date checking for multiple files, set the start/end data according to read data
  if (!isStartDateValid || !strictDateChecking) startDate = data.begin()->first;
  if (!isEndDateValid || !strictDateChecking) endDate = data.rbegin()->first;

  int noOfDays = endDate - startDate + 1;
  if (strictDateChecking && data.size() < size_t(noOfDays)) {
    stringstream oss;
    oss
      << "Climate data error: Read timeseries data between " << startDate.toIsoDateString()
      << " and " << endDate.toIsoDateString()
      << " (" << noOfDays << " days) is incomplete. There are just "
      << data.size() << " days in read dataset!";
    es.appendError(oss.str());
    return {DataAccessor(), es};
  }

  // rewrite data into vectors of single elements
  map<ACD, vector<double>> daData;
  for (Date d = startDate, ed = endDate; d <= ed; d++) {
    for (const auto& p : data[d]) daData[p.first].push_back(p.second);
  }

  // check if all vectors have the same length
  size_t sizes = 0;
  for (const auto& p : daData) sizes += p.second.size();

  if (!daData.empty() && sizes % daData.size() != 0) {
    es.appendError("Climate data error: At least one of the climate elements has less elements than the others!");
    return {DataAccessor(), es};
  }

  Climate::DataAccessor da(startDate, endDate);
  for (const auto& p : daData) {
    if (!p.second.empty()) {
      da.addClimateData(p.first, p.second);
    }
  }

  return {da, es};
}

char* Climate_readClimateDataFromCSVStringViaHeaders(const char* csvString, const char* options) {
  string resStr = Climate::readClimateDataFromCSVStringViaHeaders(csvString, CSVViaHeaderOptions(parseJsonString(options).result)).result.to_json().dump();
  return strdup(resStr.c_str());
}

void Climate_freeCString(char* str) {
  free(str);
}

