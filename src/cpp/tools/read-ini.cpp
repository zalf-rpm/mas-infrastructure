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

#include "read-ini.h"

#include <fstream>
#include <cstdlib>
#include <iostream>

#include "algorithms.h"

using namespace Tools;
using namespace std;

IniParameterMap::IniParameterMap(const string& pathToIniFile)
: _pathToIniFile(pathToIniFile) {
  parseIniFile();
}

void IniParameterMap::parseIniFile() {
  ifstream ifs(pathToIniFile().c_str(), ifstream::in);

  char line[1024];

  while (ifs.good()) {
    ifs.getline(line, 1024);
    parseAndInsert(string(line));
  }

  ifs.close();
}

void IniParameterMap::parseAndInsert(const string& line) {
  //cout << "line: " << line << endl;
  string tline = trim(line);
  if(tline.empty()) return;
  //cout << "trimmed line: " << tline << endl;

  //skip comments
  if(tline.at(0) == ';')
    return;

  //is section header
  if(tline.at(0) == '[') {
    _currentSection = tline.substr(1, tline.length()-2);
    //cout << "currentSection: |" << _currentSection << "|" << endl;
  }
  else { //is a values line
    size_t sepPos = line.find_first_of("=");
  string name = trim(line);
  string value;
  if(sepPos != string::npos) {
    name = trim(line.substr(0, sepPos));
    value = trim(line.substr(sepPos + 1));
  }
        
  //cout << "name: |" << name << "| = value: |" << value << "|" << endl;

    (*this)[_currentSection].insert(make_pair(name, value));
  }
}

string IniParameterMap::value(const string& section, const string& key,
                const std::string& def) const {
  string value(def);
  Sections2NVMaps::const_iterator cit = find(section);
  if(cit != end()) {
    Names2Values::const_iterator cit2 = cit->second.find(key);
    if(cit2 != cit->second.end()) value = cit2->second;
  }
  return value;
}

const Names2Values& IniParameterMap::values(const std::string& section) const {
  static Names2Values empty;
  Sections2NVMaps::const_iterator ci = find(section);
  return ci == end() ? empty : ci->second;
}

int IniParameterMap::valueAsInt(const std::string& section, 
                const std::string& key, int def) const {
  string res = value(section, key, "");
  return res.empty() ? def : stoi(res);
}

double IniParameterMap::valueAsDouble(const std::string& section,
                    const std::string& key, double def) const {
  string res = value(section, key, "");
  return res.empty() ? def : stof(res);
}

bool IniParameterMap::valueAsBool(const std::string& section,
                  const std::string& key,
                  bool def) const {
  string res = value(section, key, "");
  return res.empty() ? def : res == "true";
}

Date IniParameterMap::valueAsRelativeDate(const std::string& section,
                      const std::string& key,
                      Date def) const {
  return valueAsDate(section, key, def).toRelativeDate();
}

Date IniParameterMap::valueAsDate(const std::string& section,
                  const std::string& key,
                  Date def) const {
  string res = value(section, key, "");
  return res.empty() ? def : Date::fromIsoDateString(res);
}
