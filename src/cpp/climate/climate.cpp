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

#include <cstdlib>
#include <iostream>
#include <sstream>
#include <algorithm>
#include <cctype>
#include <cmath>
#include <cassert>
#include <map>
#include <list>
#include <mutex>
#include <string>
#include <limits>

#include "climate/climate.h"
#include "db/abstract-db-connections.h"
#include "tools/coord-trans.h"
#include "tools/algorithms.h"
#include "tools/datastructures.h"
#include "tools/helper.h"

using namespace std;
using namespace Climate;
using namespace Tools;

string ClimateStation::toString() const
{
	ostringstream s;
	s << "climate-station: name: " << name() << " id: " <<  id()
	<< " geoCoord: " << geoCoord().toString();
	return s.str();
}

//------------------------------------------------------------------------------



std::vector<LatLngCoord> ClimateSimulation::geoCoords() const
{
	std::vector<LatLngCoord> gcs(climateStations().size());
	for(unsigned int i = 0; i < gcs.size(); i++)
		gcs[i] = climateStations().at(i)->geoCoord();
	return gcs;
}

LatLngCoord ClimateSimulation::
climateStation2geoCoord(const string& stationName) const
{
  string lowerStationName = toLower(stationName);
  for(auto cs : _stations)
  {
    if(toLower(cs->name()).find(lowerStationName) != string::npos)
      return cs->geoCoord();
	}
	return LatLngCoord();
}

ClimateStation ClimateSimulation::
geoCoord2climateStation(const LatLngCoord& gc) const
{
  auto findStation = [&](const LatLngCoord& gc) -> pair<ClimateStation, bool> {
    for(auto s : _stations)
    {
      if(s->geoCoord() == gc)
        return make_pair(*s, true);
    }
    return make_pair(ClimateStation(), false);
  };

  auto p = findStation(gc);
  if(p.second)
    return p.first;

  auto p2 = findStation(getClosestClimateDataGeoCoord(gc));
  if(p2.second)
    return p2.first;

	return ClimateStation();
}

LatLngCoord ClimateSimulation::
getClosestClimateDataGeoCoord(const LatLngCoord& gc) const
{
  map<double, ClimateStationPtr> dist2cs;
  for(auto cs : _stations)
    dist2cs[gc.distanceTo(cs->geoCoord())] = cs;

  return dist2cs.empty() ? LatLngCoord() : dist2cs.begin()->second->geoCoord();
}

YearRange ClimateSimulation::availableYearRange()
{
  if(_yearRange.isValid())
    return _yearRange;

  //this should always lead to the least common denominator range
  //if the climate simulation is a container for multi-ensemble realizations
  //from different simulations
  int fromYear = 0, toYear = 9999;
  for(auto sc : _scenarios)
  {
    for(auto r : sc->realizations())
    {
      auto ayr = r->simulation()->availableYearRange();
      fromYear = max(fromYear, ayr.fromYear);
      toYear = min(toYear, ayr.toYear);
    }
  }
  return snapToRaster(YearRange(fromYear, toYear));
}

ClimateStation ClimateSimulation::climateStation(const string& stationName) const
{
  string lowerStationName = toLower(stationName);
  for(auto cs : _stations)
  {
    if(toLower(cs->name()).find(lowerStationName) != string::npos)
      return *cs;
	}
	return ClimateStation();
}

ClimateScenario* ClimateSimulation::scenario(const string& name) const
{
  for(auto s : _scenarios)
  {
    if(s->name() == name)
      return s.get();
	}
  return nullptr;
}

ClimateScenario* ClimateSimulation::scenarioById(const string& id) const
{
  for(auto s : _scenarios)
  {
    if(s->id() == id)
      return s.get();
  }
  return nullptr;
}


//------------------------------------------------------------------------------

namespace
{
  bool cmpClimateStationPtrs(ClimateStationPtr left, ClimateStationPtr right)
  {
		return (*left) < (*right);
	}

  struct ToLower : public unary_function<char, void>
  {
		void operator()(char& c){ c = tolower(c); }
	};
}

StarSimulation::StarSimulation(Db::DB* con)
: ClimateSimulation("star", "Star", con)
{
  setClimateStations();

  auto cs = make_shared<ClimateScenario>("---", this);
//  _realizations.push_back(new StarRealization(this, cs, connection().clone()));
  cs->setRealizations({make_shared<StarRealization>(this, cs.get(), connection().clone())});
  _scenarios.push_back(cs);
}

void StarSimulation::setClimateStations()
{
	connection().select("select latitude, longitude, dat, bezeichnung, hnn, id "
                      "from klimades");

	Db::MysqlDB* con = Db::toMysqlDB(&connection());
	MYSQL_ROW row;
	while((row = con->getMysqlRow()) != 0)
  {
    string name(row[3]);
    capitalizeInPlace(name);
    //cout << "name: " << name << endl;
    auto cs = make_shared<ClimateStation>(atoi(row[5]),
        LatLngCoord(atof(row[0]), atof(row[1])),
        atof(row[4]), name, this);
    cs->setDbName(row[2]);
    _stations.push_back(cs);
  }

  sort(_stations.begin(), _stations.end(), cmpClimateStationPtrs);
  connection().freeResultSet();
}

ClimateScenario* StarSimulation::defaultScenario() const
{
  return _scenarios.back().get();
}

//------------------------------------------------------------------------------

UserSqliteDBSimulation::UserSqliteDBSimulation(Db::DB* con)
: ClimateSimulation(toLower(con->abstractSchemaName()), capitalize(con->abstractSchemaName()), con)
{
	setClimateStations();

  auto cs = make_shared<ClimateScenario>("---", this);
//  _realizations.push_back(new UserSqliteDBRealization(this, cs, connection().clone()));
  cs->setRealizations({make_shared<UserSqliteDBRealization>(this, cs.get(), connection().clone())});
	_scenarios.push_back(cs);
}

void UserSqliteDBSimulation::setClimateStations()
{
  connection().select("select id, wgs84_lat, wgs84_lng, coordinate_system_short_name, "
                      "rect_coordinate_system_r, rect_coordinate_system_h "
											"from raster_point");

	Db::DBRow row;
	while(!(row = connection().getRow()).empty())
	{
    LatLngCoord llc;
    if(!row[1].empty() && !row[2].empty())
    {
      llc.lat = stod(row[1]);
      llc.lng = stod(row[2]);
    }
    else if(!row[3].empty() && !row[4].empty() && !row[5].empty())
      llc = RC2latLng(RectCoord(shortStringToCoordinateSystem(row[3]), stoi(row[4]), stoi(row[5])));

    if(!llc.isValid())
      continue;

		ostringstream ss;
		ss << "LatLng(" << llc.lat << "," << llc.lng << ")";
		string name(ss.str());
		//cout << "name: " << name << endl;
    auto cs = make_shared<ClimateStation>(satoi(row[0]),
        LatLngCoord(llc.lat, llc.lng),
        0.0, name, this);
    cs->setDbName("");
    _stations.push_back(cs);
	}

	sort(_stations.begin(), _stations.end(), cmpClimateStationPtrs);
	connection().freeResultSet();
}

ClimateScenario* UserSqliteDBSimulation::defaultScenario() const
{
  return _scenarios.back().get();
}

YearRange UserSqliteDBSimulation::availableYearRange()
{
  if(!_yearRange.isValid())
  {
    lock_guard<mutex> lock(_lockable);

    if(!_yearRange.isValid() &&
       !climateStations().empty())
    {
      auto firstCS = climateStations().front();

      ostringstream ss;
      ss << "SELECT min(year), max(year) "
         << "FROM data "
         << "WHERE raster_point_id = "
         << firstCS->id();

      connection().select(ss.str().c_str());

      Db::DBRow row;
      if(!(row = connection().getRow()).empty())
        _yearRange = snapToRaster(YearRange(satoi(row[0]), satoi(row[1])));
    }
  }

  return _yearRange;
}

//------------------------------------------------------------------------------

Star2Simulation::Star2Simulation(Db::DB* con)
: ClimateSimulation("star2", "Star2", con)
{
  setClimateStations();
  setScenariosAndRealizations();
}

void Star2Simulation::setScenariosAndRealizations()
{
  auto dbcps = Db::dbConnectionParameters();
  auto asn = connection().abstractSchemaName();
  string star2Section = dbcps.value("abstract-schema", asn) + "." + asn;

  string reals = Db::dbConnectionParameters().value(star2Section, "realizations", "1, 25, 50, 75, 100");
	vector<string> vsr = Tools::splitString(reals, ", ");
	vector<int> realizationNumbers;
  for(string s : vsr) { realizationNumbers.push_back(atoi(s.c_str())); }

  auto s2s = make_shared<Star2Scenario>("2k","2K", "2k_", this);
  Realizations rs;
  for(int realizationNo : realizationNumbers)
  {
    rs.push_back(make_shared<Star2Realization>(this, s2s.get(), connection().clone(),
                                               realizationNo));
  }
  s2s->setRealizations(rs);
//  _realizations.insert(_realizations.end(), rs.begin(), rs.end());
  _scenarios.push_back(s2s);

  rs.clear();
  s2s = make_shared<Star2Scenario>("0k","0K", "0k_", this);
  for(int realizationNo : realizationNumbers)
  {
    rs.push_back(make_shared<Star2Realization>(this, s2s.get(), connection().clone(),
                                               realizationNo));
  }
  s2s->setRealizations(rs);
//  _realizations.insert(_realizations.end(), rs.begin(), rs.end());
  _scenarios.push_back(s2s);
}

void Star2Simulation::setClimateStations()
{
	connection().select("select lat, lon, name, id from station");// where klim = 1");

	Db::MysqlDB* con = Db::toMysqlDB(&connection());
	MYSQL_ROW row;
	while((row = con->getMysqlRow()) != 0)
  {
    string name(row[2]);
    for_each(name.begin(), name.end(), ToLower());
    capitalizeInPlace(name);
    //cout << "name: " << name << endl;
    auto cs = make_shared<ClimateStation>(atoi(row[3]),
        LatLngCoord(atof(row[0]), atof(row[1])),
        0.0, name, this);
    cs->setDbName("");
    _stations.push_back(cs);
  }

  sort(_stations.begin(), _stations.end(), cmpClimateStationPtrs);
  connection().freeResultSet();
}

ClimateScenario* Star2Simulation::defaultScenario() const
{
  return scenarioById("2k");
}

//------------------------------------------------------------------------------

Star2MeasuredDataSimulation::Star2MeasuredDataSimulation(Db::DB* con)
: ClimateSimulation("star2measured", "Star2m", con)
{
  setClimateStations();

  auto cs = make_shared<ClimateScenario>("---", this);
//  _realizations.push_back(new Star2MeasuredDataRealization
//                          (this, cs, connection().clone()));
  cs->setRealizations({make_shared<Star2MeasuredDataRealization>(this, cs.get(), connection().clone())});
  _scenarios.push_back(cs);
}

void Star2MeasuredDataSimulation::setClimateStations()
{
  connection().select("select lat, lon, name, id from station where klim = 1");

	Db::MysqlDB* con = Db::toMysqlDB(&connection());
	MYSQL_ROW row;
	while((row = con->getMysqlRow()) != 0)
  {
    string name(row[2]);
    for_each(name.begin(), name.end(), ToLower());
    capitalizeInPlace(name);
    //cout << "name: " << name << endl;
    auto cs = make_shared<ClimateStation>(atoi(row[3]),
        LatLngCoord(atof(row[0]), atof(row[1])),
        0.0, name, this);
    cs->setDbName("refzen");
    _stations.push_back(cs);
  }

  sort(_stations.begin(), _stations.end(), cmpClimateStationPtrs);
  connection().freeResultSet();
}

ClimateScenario* Star2MeasuredDataSimulation::defaultScenario() const
{
  return _scenarios.back().get();
}

//------------------------------------------------------------------------------

DDClimateDataServerSimulation::
DDClimateDataServerSimulation(const DDServerSetup& setupData, Db::DB* con)
	: ClimateSimulation(setupData.simulationId(), setupData.simulationName(), con),
		_setupData(setupData)
{
	if(_setupData.yearRange.isValid())
		_yearRange = _setupData.yearRange;

	setClimateStations();
	setScenariosAndRealizations();
}

void DDClimateDataServerSimulation::setScenariosAndRealizations()
{
  for(string sid : _setupData.scenarioIds())
	{
    auto sc = make_shared<ClimateScenario>(sid, this);
		Realizations rs;
    for(string rid : _setupData.realizationIds())
      rs.push_back(make_shared<DDClimateDataServerRealization>(rid, this, sc.get(),
                                                               connection().clone(),
                                                               _setupData));
		sc->setRealizations(rs);
//		_realizations.insert(_realizations.end(), rs.begin(), rs.end());
		_scenarios.push_back(sc);
	}
}

void DDClimateDataServerSimulation::setClimateStations()
{
	ostringstream ss;
  ss << "SELECT h.stat_id, h.stat_name, h.rwert5, h.hwert5, h.breite_dez, "
        "h.laenge_dez, h.nn, sl.dat_id, h.sl, sl.stat_ke "
        "FROM ";
  ss << _setupData.headerDbName() << "." << _setupData.headerTableName() << " as h "
     <<  "inner join ";
  ss << _setupData.stolistDbName() << "." << _setupData.stolistTableName() << " as sl "
     << "on h.stat_id = sl.stat_id "
        "where true ";
  //				 "where sl.stat_ke = 'Klim' ";
  if(_setupData.useErrorTable())
    ss << "and sl.dat_id not in "
          "(SELECT distinct dat_id "
       << "FROM " << _setupData.errorDbName() << "." << _setupData.errorTableName() << ") ";
  //excluded original wettreg 2006 stations with missing data or wrong data
  if(_setupData.simulationId() == "wettreg2006")
    ss << "and sl.dat_id not in (283, 385, 1120, 1623, 1861)";

  //cout << "climate-stations query: " << ss.str() << endl;
  connection().select(ss.str().c_str());

	bool commaDotConversionChecked = false;
	bool convertCommaToDot = false;
  bool purePrecipStationsFound = false;

  auto& con = connection();
  Db::DBRow row;
  while(!(row = con.getRow()).empty())
  {
    string name = toLower(row[1]);
    capitalizeInPlace(name);

    if(!commaDotConversionChecked)
    {
      convertCommaToDot = row[4].find(',') != string::npos;
			commaDotConversionChecked = true;
		}

    auto llc = convertCommaToDot ? LatLngCoord(Tools::stod_comma(row[4]), Tools::stod_comma(row[5]))
      : LatLngCoord(stod(row[4]), stod(row[5]));

    auto cs = make_shared<ClimateStation>(stoi(row[0]), llc, row[6].empty() ? 0.0 : stod(row[6]), name, this);
    cs->setDbName(row[7]);
    cs->setSL(ClimateStation::SL(row[8].empty() ? 1: stoi(row[8])));

    cs->setIsPrecipStation(toLower(row[9]) == "nied");
    purePrecipStationsFound = purePrecipStationsFound || cs->isPrecipStation();

    _stations.push_back(cs);
		//cout << "wrname: " << name << " : " << _stations.back()->toString() << endl;
	}
  con.freeResultSet();

  //assign the precipitation stations the closest full climate station where it gets the missing data from
  vector<ClimateStationPtr> fullClimateStations, precipStations;
  partition_copy(begin(_stations), end(_stations),
                 back_inserter(precipStations), back_inserter(fullClimateStations),
                 [](ClimateStationPtr cs){ return cs->isPrecipStation(); });

  map<ClimateStationPtr, ClimateStationPtr> pCS2fullCS;
  auto manualMappingByNames = Db::dbConnectionParameters().values
                              (_setupData.simulationId() + ".precip-to-climate-station-mapping");
  for(auto pn2fn : manualMappingByNames)
  {
    auto find = [&](string name) -> ClimateStationPtr
    {
      for(auto cs : _stations)
      {
        if(toLower(cs->name()).find(toLower(name)) != string::npos)
          return cs;
      }
      return nullptr;
    };
    pCS2fullCS[find(pn2fn.first)] = find(pn2fn.second);
  }

  for(auto pcs : precipStations)
  {
    ClimateStation* fullCS = nullptr;
    auto pCSIt = pCS2fullCS.find(pcs);
    if(pCSIt != pCS2fullCS.end())
      fullCS = pCSIt->second.get();
    else
    {
      double shortestDist = numeric_limits<double>::max();
      for(auto fcs : fullClimateStations)
      {
        double dist = pcs->geoCoord().distanceTo(fcs->geoCoord());
        if(dist < shortestDist)
        {
          fullCS = fcs.get();
          shortestDist = dist;
        }
      }
    }
    pcs->setFullClimateReferenceStation(fullCS);
  }

	sort(_stations.begin(), _stations.end(), cmpClimateStationPtrs);
}

ClimateScenario* DDClimateDataServerSimulation::defaultScenario() const
{
	return scenario(defaultScenarioId());
}

YearRange DDClimateDataServerSimulation::availableYearRange()
{
  if(!_yearRange.isValid())
  {
    lock_guard<mutex> lock(_lockable);

		if(!_yearRange.isValid() &&
			 !_setupData.realizationIds().empty() &&
			 !climateStations().empty())
    {
			string firstRId = _setupData.realizationIds().front();
      auto firstCS = climateStations().front();

			ostringstream ss;
			ss << "SELECT min(jahr), max(jahr) "
						"FROM "
				 << _setupData.dataDbName() << "." << _setupData.dataTableName() << " "
						"where szenario='" << defaultScenarioId() << "' and "
						"realisierung='" << firstRId << "' and "
						"dat_id = " << firstCS->dbName();

			connection().select(ss.str().c_str());

			Db::DBRow row;
			if(!(row = connection().getRow()).empty())
				_yearRange = snapToRaster(YearRange(satoi(row[0]), satoi(row[1])));
    }
  }

  return _yearRange;
}

//------------------------------------------------------------------------------

CLMSimulation::CLMSimulation(Db::DB* con)
	: ClimateSimulation("clm20-9", "CLM20-9", con),
		_avgClimateStationsSet([](const ClimateStation* left, const ClimateStation* right){	return left->id() < right->id(); })
{
	setClimateStations();
	setScenariosAndRealizations();
}

void CLMSimulation::setScenariosAndRealizations()
{
  auto dbcps = Db::dbConnectionParameters();
  auto asn = connection().abstractSchemaName();
  string clmSection = dbcps.value("abstract-schema", asn) + "." + asn;

  string reals = Db::dbConnectionParameters().value(clmSection, "realizations", "1, 2");
	vector<string> vsr = Tools::splitString(reals, ", ");

  auto sc = make_shared<ClimateScenario>("A1B", this);
	Realizations rs;
  for(string s : vsr)
	{
    rs.push_back(make_shared<CLMRealization>(this, sc.get(), s, connection().clone()));
	}
	sc->setRealizations(rs);
//	_realizations.insert(_realizations.end(), rs.begin(), rs.end());
	_scenarios.push_back(sc);

	rs.clear();
  sc = make_shared<ClimateScenario>("B1", this);
  for(string s : vsr)
	{
    rs.push_back(make_shared<CLMRealization>(this, sc.get(), s, connection().clone()));
	}
	sc->setRealizations(rs);
//	_realizations.insert(_realizations.end(), rs.begin(), rs.end());
	_scenarios.push_back(sc);
}

void CLMSimulation::setClimateStations()
{
	connection().select
	("SELECT h.stat_id, h.stat_name, h.rwert5, h.hwert5, h.breite_dez, "
	 "h.laenge_dez, h.nn, sl.dat_id "
	 "FROM clm20.header_clm20 h "
	 "inner join clm20.clm20_stolist sl on h.stat_id = sl.stat_id");

  set<int> lats, lngs;

	Db::DBRow row;
	while(!(row = connection().getRow()).empty())
  {
    auto cs = make_shared<ClimateStation>(stoi(row[0]), LatLngCoord(stod(row[4]), stod(row[5])),
        stod(row[6]), row[1], this);
		cs->setDbName(row[7]);
		_stations.push_back(cs);

    //find smallest and largest values
    lats.insert(int(cs->geoCoord().lat * 100.0));
    lngs.insert(int(cs->geoCoord().lng * 100.0));
	}

  //define position matrix which should be sparse (but isn't)
  //bearable in this case
  typedef SparseMatrix<ClimateStation*> LatLngPos;
  LatLngPos posMatrix(static_cast<ClimateStation*>(nullptr));

  //put climatestations into position matrix
  for(auto cs : climateStations())
  {
    int lat = int(cs->geoCoord().lat * 100.0);
    int lng = int(cs->geoCoord().lng * 100.0);
    posMatrix.setValueAt(lat, lng, cs.get());
  }

  //reduce "sparse" position matrix to minimal one
  typedef StdMatrix<ClimateStation*> LatLngPos2;
  LatLngPos2 posMatrix2(lats.size(), lngs.size(),
                        static_cast<ClimateStation*>(nullptr));
  int lat2 = 0;
  for(int lat : lats)
  {
    int lng2 = 0;
    for(int lng : lngs)
    {
      if(ClimateStation* cs =
         posMatrix.valueAt(lat, lng, static_cast<ClimateStation*>(nullptr)))
        posMatrix2[lat2][lng2++] = cs;
    }
    lat2++;
  }

  //find surrounding climatestations and remember them in map
  for(int lat = 0, latRange = posMatrix2.rows(); lat < latRange; lat++)
  {
    for(int lng = 0, lngRange = posMatrix2.cols(); lng < lngRange; lng++)
    {
      ClimateStation* centerStation = posMatrix2[lat][lng];
      set<const ClimateStation*>& css = _avgClimateStationsSet[centerStation];

      //north-west
      if(lat > 0 && lng > 0)
        css.insert(posMatrix2[lat-1][lng-1]);

      //north
      if(lat > 0)
        css.insert(posMatrix2[lat-1][lng]);

      //west
      if(lng > 0)
        css.insert(posMatrix2[lat][lng - 1]);

      //self
      css.insert(centerStation);

      //south-west
      if(lat < latRange - 1 && lng > 0)
        css.insert(posMatrix2[lat+1][lng-1]);

      //south
      if(lat < latRange - 1)
        css.insert(posMatrix2[lat+1][lng]);

      //south-east
      if(lat < latRange - 1 && lng < lngRange - 1)
        css.insert(posMatrix2[lat+1][lng+1]);

      //east
      if(lng < lngRange - 1)
        css.insert(posMatrix2[lat][lng+1]);

      //north-east
      if(lat > 0 && lng < lngRange - 1)
        css.insert(posMatrix2[lat-1][lng+1]);
    }
  }

	sort(_stations.begin(), _stations.end(), cmpClimateStationPtrs);
	connection().freeResultSet();
}

ClimateScenario* CLMSimulation::defaultScenario() const
{
	return scenario("A1B");
}

set<const ClimateStation*>
    CLMSimulation::avgClimateStationSet(const ClimateStation* c)
{
  return value(_avgClimateStationsSet, c);
}

YearRange CLMSimulation::availableYearRange()
{
  if(!_yearRange.isValid())
  {
    lock_guard<mutex> lock(_lockable);

    if(!_yearRange.isValid())
    {
      connection().select("SELECT min(jahr), max(jahr) "
													"FROM clm20.clm20_data where szenario='A1B' and "
                          "realisierung='1' and dat_id = 1");

			Db::DBRow row;
			if(!(row = connection().getRow()).empty())
				_yearRange = snapToRaster(YearRange(satoi(row[0]), satoi(row[1])));
    }
  }

  return _yearRange;
}

//------------------------------------------------------------------------------

ClimateRealizationPtr ClimateScenario::realizationPtr(const string& name) const
{
  for(auto r : realizations())
  {
    if(r->name() == name)
      return r;
  }
  return ClimateRealizationPtr();
}


//------------------------------------------------------------------------------

void ClimateRealization::fillCacheFor(const vector<AvailableClimateData>& acds,
                                      const LatLngCoord& gc,
                                      const Date& startDate,
                                      const Date& endDate)
{
  lock_guard<mutex> lock(_lockable);

	const LatLngCoord& cgc = simulation()->getClosestClimateDataGeoCoord(gc);
	vector<Cache>& cs = _geoCoord2cache[cgc];

	if(cs.size() < availableClimateDataSize())
		cs.resize(availableClimateDataSize());

	const ACDV& nicAcds = notInCache(cs, acds, startDate, endDate);
	const vector<ACDV>& cseAcds = commonStartEnd(cs, nicAcds, startDate, endDate);

	vector<ACDV>::const_iterator acdvi;
	for(acdvi = cseAcds.begin(); acdvi != cseAcds.end(); acdvi++)
		updateCaches(cs, *acdvi, cgc, startDate, endDate);
}

ACDV ClimateRealization::notInCache(const vector<Cache>& cs, const ACDV& acds,
                                    const Date& startDate,
                                    const Date& endDate) const
{
	ACDV res;
  for(ACDV::const_iterator acdi = acds.begin(); acdi != acds.end(); acdi++)
  {
		const Cache& c = cs.at(*acdi);
		if(!c.isInitialized() || c.startDate > startDate || c.endDate < endDate)
			res.push_back(*acdi);
	}
	return res;
}

vector<ACDV> ClimateRealization::commonStartEnd(const vector<Cache>& cs,
                                                const ACDV& acds,
                                                const Date& startDate,
                                                const Date& endDate) const
{
	vector<ACDV> res;
	typedef map<Date, vector<ACD> > MDVACD;
	typedef map<Date, MDVACD> MDMDVACD;
	MDMDVACD map;
  for(ACDV::const_iterator acdi = acds.begin(); acdi != acds.end(); acdi++)
  {
		const Cache& c = cs.at(*acdi);
		if(!c.isInitialized())
			map[startDate][endDate].push_back(*acdi);
		else
			map[c.startDate][c.endDate].push_back(*acdi);
	}

	MDMDVACD::const_iterator sdi;
	MDVACD::const_iterator edi;
  for(sdi = map.begin(); sdi != map.end(); sdi++)
    for(edi = sdi->second.begin(); edi != sdi->second.end(); edi++)
			res.push_back(vector<ACD>(edi->second.begin(), edi->second.end()));

	return res;
}

DataAccessor ClimateRealization::
dataAccessorFor(const ACDV& acds, const LatLngCoord& gc,
                const Date& startDate, const Date& endDate)
{
  //cout << "startDate: " << startDate.toString() <<
  //    " endDate: " << endDate.toString() << endl;

  //only try to fill caches if the realizations supports the requested range
  YearRange yr = simulation()->availableYearRange();
  if(yr.fromYear <= int(startDate.year()) && int(endDate.year()) <= yr.toYear)
  {
    fillCacheFor(acds, gc, startDate, endDate);

    lock_guard<mutex> lock(_lockable);

    const LatLngCoord& cgc = simulation()->getClosestClimateDataGeoCoord(gc);
    vector<Cache>& cs = _geoCoord2cache[cgc];

    int numberOfValues = startDate.numberOfDaysTo(endDate+1);
    DataAccessor bda(startDate, endDate);

		bool cacheError = false;
		ostringstream errorData;
		errorData << "Climate-Cache-Error" << endl;

    for(ACDV::const_iterator acdi = acds.begin(); acdi != acds.end(); acdi++)
    {
      Cache& c = cs[*acdi];
			if(c.isInitialized())
			{
				unsigned int o = c.offsetFor(startDate);
				bda.addClimateData(*acdi, vector<double>(c._cache.begin()+o,
																								 c._cache.begin()+o+numberOfValues));
			}
			else
			{
				cacheError = true;
				errorData << "ACD: " << *acdi
									<< " startDate: " << startDate.toString()
									<< " endDate: " << endDate.toString()
									<< endl;
			}
		}

		if(cacheError)
		{
			cout << errorData.str() << endl;
			return DataAccessor();
		}

    return bda;
  }

  return DataAccessor();
}

unsigned int Cache::getNewOffsetIndexFor(const Tools::Date& start)
{
	int delta = offsetFor(start);
	offsets.push_back(delta < 0 ? 0 : delta);
	return offsets.size() - 1;
}

//! for now we just make the cache grow infinitely
void ClimateRealization::updateCaches(vector<Cache>& cs, ACDV acds,
                                      const LatLngCoord& gc,
                                      const Date& startDate,
                                      const Date& endDate)
{
	const Cache& exampleCache = cs.at(acds.front());

	bool isNewCache = !exampleCache.isInitialized();

	Date sd = startDate, ed = endDate;

  if(!isNewCache)
  {
		//the cache contains already the the whole needed range of data
		if(startDate >= exampleCache.startDate && endDate <= exampleCache.endDate)
			return;

		//we got no sparse vectors, so have to extend the endDate until the
		//existing start date (could potentially be many elements
		//(might have to change)
		if(endDate < exampleCache.startDate || endDate <= exampleCache.endDate)
			ed = exampleCache.startDate - 1;

		if(startDate > exampleCache.endDate || startDate >= exampleCache.startDate)
			sd = exampleCache.endDate + 1;
	}

	//cout << "executing query" << endl;
	map<ACD, vector<double>*> acd2ds = executeQuery(acds, gc, sd, ed);
	map<ACD, vector<double>*>::const_iterator dsi;
  for(dsi = acd2ds.begin(); dsi != acd2ds.end(); dsi++)
  {
		vector<double>* ds = dsi->second;
		Cache& c = cs[dsi->first];
		unsigned int rowCount = ds->size();

		int lowerSlice = isNewCache ? rowCount : sd.numberOfDaysTo(c.startDate);
		int upperSlice = isNewCache ? 0 : c.endDate.numberOfDaysTo(ed);

		//update offsets (not necessary if the cache is new)
    if(!isNewCache)
    {
			int oldOffset = sd.numberOfDaysTo(c.startDate);
			if(oldOffset < 0)
				oldOffset = 0;

			//now update the offsets of all userHandles with the same location
			for(unsigned int i = 0; i < c.offsets.size(); i++)
				c.offsets[i] += oldOffset;
		}

		//prepend lower slice of rows
    if(lowerSlice > 0)
    {
			c._cache.insert(c._cache.begin(), ds->begin(), ds->begin() + lowerSlice);
			c.startDate = sd;

			if(isNewCache)
				c.endDate = ed;
		}
		//append upper slice of rows
    if(upperSlice > 0)
    {
			c._cache.insert(c._cache.end(), ds->end() - upperSlice, ds->end());
			c.endDate = ed;
		}

		//took ownership of data-vector
		delete ds;
	}
}

//------------------------------------------------------------------------------

//!helper functions to access and parse db-result set
namespace
{
	typedef ClimateStation::SL SL;

	//! month = -1 means fkorr value for whole year
  double fkorr(SL sl, int month = 0)
  {
		if(month < 0 || month > 12)
			return 0.0;

    static mutex lockable;
		static map<SL, map<int, double> > m;
		static bool initialized = false;
		if(!initialized)
		{
      lock_guard<mutex> lock(lockable);

			if(!initialized)
			{
				SL f = ClimateStation::f;
				m[f][1]=31.6; m[f][2]=33.5; m[f][3]=26.9; m[f][4]=18.3;
				m[f][5]=12.5; m[f][6]=10.4; m[f][7]=10.8; m[f][8]=10.5;
				m[f][9]=12.6; m[f][10]=15.5; m[f][11]=21.8; m[f][12]=26.5;
				m[f][-1]=18.2;

				SL lg = ClimateStation::lg;
				m[lg][1]=23.3; m[lg][2]=24.5; m[lg][3]=20.3; m[lg][4]=15.1;
				m[lg][5]=11.1; m[lg][6]=9.8; m[lg][7]=10.0; m[lg][8]=9.5;
				m[lg][9]=11.5; m[lg][10]=12.7; m[lg][11]=16.8; m[lg][12]=19.8;
				m[lg][-1]=14.6;

				SL mg = ClimateStation::mg;
				m[mg][1]=17.3; m[mg][2]=17.9; m[mg][3]=15.5; m[mg][4]=12.7;
				m[mg][5]=10.1; m[mg][6]=8.8; m[mg][7]=9.1; m[mg][8]=8.5;
				m[mg][9]=10.2; m[mg][10]=11.0; m[mg][11]=13.3; m[mg][12]=15.0;
				m[mg][-1]=12.0;

				SL sg = ClimateStation::sg;
				m[sg][1]=11.5; m[sg][2]=11.8; m[sg][3]=10.7; m[sg][4]=10.0;
				m[sg][5]=8.6; m[sg][6]=7.7; m[sg][7]=8.0; m[sg][8]=7.5;
				m[sg][9]=8.7; m[sg][10]=8.8; m[sg][11]=9.5; m[sg][12]=10.3;
				m[sg][-1]=9.3;

				initialized = true;
			}
		}

		return m[sl][month];
	}

	//kind of precipitation
	enum PArt { Regen = 0, Mischniederschlag = 2, Schnee = 3 };

	//kind of precipitation extended
	enum PArtPlus { rs = 0, rw = 1, mn = Mischniederschlag, s = Schnee };

  PArtPlus createPArtPlus(PArt pa, int month)
  {
    if(pa == Regen)
    {
			if(4 <= month && month <= 9)
				return rs;
			else
				return rw;
		}
		return PArtPlus(pa);
	}

  PArt PArt4tmit(double tmit, bool forSaxony = false)
  {
    if(forSaxony)
    {
      return tmit > 3.0 ?
          Regen : -0.4 <= tmit && tmit <= 3.0 ?
          Mischniederschlag : Schnee;
		}

    return tmit > 3.0 ?
        Regen : -0.7 <= tmit && tmit <= 3.0 ?
        Mischniederschlag : Schnee;
	}

	//! b koefficient
  double bKoeff(SL sl, PArtPlus pap)
  {
    static mutex lockable;
		static map<SL, map<PArtPlus, double> > m;
		static bool initialized = false;
		if(!initialized)
		{
      lock_guard<mutex> lock(lockable);

			if(!initialized)
			{
				SL f = ClimateStation::f;
				m[f][rs]=0.345; m[f][rw]=0.34; m[f][mn]=0.535; m[f][s]=0.72;

				SL lg = ClimateStation::lg;
				m[lg][rs]=0.31; m[lg][rw]=0.28; m[lg][mn]=0.39; m[lg][s]=0.51;

				SL mg = ClimateStation::mg;
				m[mg][rs]=0.28; m[mg][rw]=0.24; m[mg][mn]=0.305; m[mg][s]=0.33;

				SL sg = ClimateStation::sg;
				m[sg][rs]=0.245; m[sg][rw]=0.19; m[sg][mn]=0.185; m[sg][s]=0.21;

				initialized = true;
			}
		}

		return m[sl][pap];

	}

	//! epsilon koefficient
  double epsilonKoeff(PArtPlus pap)
  {
		double res = 0;
    switch(pap)
    {
    case rs: res = 0.38; break;
    case rw: res = 0.46; break;
    case mn: res = 0.55; break;
    case s: res = 0.82; break;
		}
		return res;
	}

  struct Fun
  {
		virtual ~Fun(){}
		virtual double operator()(MYSQL_ROW row) const = 0;
		virtual double operator()(const Db::DBRow& row) const = 0;
	};

  struct ParseAsDouble : public Fun
  {
		int _pos;
		ParseAsDouble(int pos) : _pos(pos) {}
		virtual ~ParseAsDouble(){}

		double operator()(MYSQL_ROW row) const
		{
			return atof(row[_pos]);
		}

		double operator()(const Db::DBRow& row) const
    {
      return stof(row.at(_pos));
		}
	};

  struct CalcStarGlobrad : public Fun
  {
    int _pos; bool _asMJpm2pd;
    CalcStarGlobrad(int pos, bool asMJpm2pd = true) :
        _pos(pos), _asMJpm2pd(asMJpm2pd) {}
		virtual ~CalcStarGlobrad(){}

		double operator()(MYSQL_ROW row) const
		{
			//100.0*100.0/1000000.0 -> 1/100
			//double gr = std::atof(row[_pos])*4.1868;
			double gr = atof(row[_pos]);
			return _asMJpm2pd ? gr / 100.0 : gr;
		}

		double operator()(const Db::DBRow& row) const
    {
      //100.0*100.0/1000000.0 -> 1/100
      //double gr = std::atof(row[_pos])*4.1868;
			double gr = satof(row.at(_pos));
      return _asMJpm2pd ? gr / 100.0 : gr;
		}
	};

  struct CalcWettRegGlobrad : public Fun
  {
		int _posSun, _posYd; double _lat;
		CalcWettRegGlobrad(int posSun, int posYd, double lat)
		: _posSun(posSun), _posYd(posYd), _lat(lat) {}
		virtual ~CalcWettRegGlobrad(){}

		double operator()(MYSQL_ROW row) const
		{
			return Tools::sunshine2globalRadiation(atoi(row[_posYd]),
																						 atof(row[_posSun]),
																						 _lat);
		}

		double operator()(const Db::DBRow& row) const //[MJ/m²/d]
    {
			return Tools::sunshine2globalRadiation(satoi(row.at(_posYd)),
																						 satof(row.at(_posSun)),
			                                       _lat);
		}
	};

	struct CalcRemoGlobrad : public Fun
	{
		int _posCloudAmount, _posDoy;
		double _lat, _hnn;
		CalcRemoGlobrad(int posCloudAmount, int posDoy, double lat, double heightNN)
			: _posCloudAmount(posCloudAmount),
				_posDoy(posDoy),
				_lat(lat),
				_hnn(heightNN)
		{}
		virtual ~CalcRemoGlobrad(){}

		double operator()(MYSQL_ROW row) const
		{
			return Tools::cloudAmount2globalRadiation(atoi(row[_posDoy]),
																								atof(row[_posCloudAmount]),
																								_lat, _hnn);
		}

		double operator()(const Db::DBRow& row) const //[MJ/m²/d]
		{
			return Tools::cloudAmount2globalRadiation(satoi(row.at(_posDoy)),
																								satof(row.at(_posCloudAmount)),
																								_lat, _hnn);
		}
	};

  struct CalcCorrWRAndCLMPrecip : public Fun
  {
		int _posPrecip; int _posTavg; int _posMonth; SL _sl;
		CalcCorrWRAndCLMPrecip(int posPrecip, int posTavg, int posMonth, SL sl)
		: _posPrecip(posPrecip), _posTavg(posTavg), _posMonth(posMonth), _sl(sl) {}
		virtual ~CalcCorrWRAndCLMPrecip(){}

		double operator()(MYSQL_ROW row) const
		{
			int month = atoi(row[_posMonth]);
			PArtPlus pap = createPArtPlus(PArt4tmit(atof(row[_posTavg])), month);
			double P = atof(row[_posPrecip]);
			double b = bKoeff(_sl, pap);
			double epsilon = epsilonKoeff(pap);
			return P+b*pow(P, epsilon);
		}

		double operator()(const Db::DBRow& row) const
    {
			int month = satoi(row.at(_posMonth));
			PArtPlus pap = createPArtPlus(PArt4tmit(satof(row.at(_posTavg))), month);
			double P = satof(row.at(_posPrecip));
			double b = bKoeff(_sl, pap);
			double epsilon = epsilonKoeff(pap);
			return P+b*pow(P, epsilon);
		}
	};
}

//------------------------------------------------------------------------------

DataAccessor StarRealization::
dataAccessorFor(const vector<AvailableClimateData>& acds,
                        const string& stationName,
                        const Date& startDate,
                        const Date& endDate)
{
	return ClimateRealization::
	dataAccessorFor(acds, simulation()->climateStation2geoCoord(stationName),
	                startDate, endDate);
}

map<ACD, vector<double>*>
StarRealization::executeQuery(const ACDV& acds,
                              const LatLngCoord& gc, const Date& startDate,
                              const Date& endDate) const
{
	const ClimateStation& cs = simulation()->geoCoord2climateStation(gc);

	ostringstream query; query << "select ";
	int c = 0;
	vector<Fun*> fs;

  for(ACDV::const_iterator acdi = acds.begin(); acdi != acds.end(); acdi++)
  {
    ACD acd = *acdi;
    switch(acd)
    {
    case Climate::globrad:
      query << availableClimateData2StarDBColName(acd);
      fs.push_back(new CalcStarGlobrad(c++));
      break;
    default:
      query << availableClimateData2StarDBColName(acd);
      fs.push_back(new ParseAsDouble(c++));
		}
		query << (acdi+1 != acds.end() ? ", " : " ");
	}

	string dbDate =
			"concat(jahr, \'-\', "
			"if(mo<10,concat(\'0\',mo),mo), \'-\', "
			"if(tag<10,concat(\'0\',tag),tag))";

	query << "from " << cs.dbName() << " "
					 "where " << dbDate << " >= '" << connection().toDBDate(startDate) << "' "
					 "and " << dbDate << " <= '" << connection().toDBDate(endDate) << "' "
					 "and not (mo = 2 and tag = 29) "
					 "order by jahr, mo, tag";

  //cout << "query: " << query.str() << endl;
  connection().select(query.str().c_str());

	int rowCount = connection().getNumberOfRows();
	map<ACD, vector<double>*> acd2ds;
  for(ACD acd : acds)
  {
    acd2ds[acd] = new vector<double>(rowCount);
  }

	int count = 0;
	Db::MysqlDB* con = Db::toMysqlDB(&connection());
	MYSQL_ROW row;
	while((row = con->getMysqlRow()) != 0)
	{
		int c = 0;
    for(ACD acd : acds)
    {
      (*(acd2ds[acd]))[count] = (*(fs.at(c++)))(row);
    }
		count++;
	}

	for(unsigned int i = 0; i < fs.size(); i++)
		delete fs.at(i);

	return acd2ds;
}

//------------------------------------------------------------------------------

DataAccessor UserSqliteDBRealization::
dataAccessorFor(const vector<AvailableClimateData>& acds,
												const string& stationName,
												const Date& startDate,
												const Date& endDate)
{
	return ClimateRealization::
	dataAccessorFor(acds, simulation()->climateStation2geoCoord(stationName),
									startDate, endDate);
}

map<ACD, vector<double>*>
UserSqliteDBRealization::executeQuery(const ACDV& acds,
																		const LatLngCoord& gc, const Date& startDate,
																		const Date& endDate) const
{
	const ClimateStation& cs = simulation()->geoCoord2climateStation(gc);

	ostringstream query; query << "select ";
	int c = 0;
	vector<Fun*> fs;

	for(ACDV::const_iterator acdi = acds.begin(); acdi != acds.end(); acdi++)
	{
		ACD acd = *acdi;
    auto colname = availableClimateData2UserSqliteDBColNameAndScaleFactor(acd);
		switch(acd)
		{
		case Climate::globrad:
			query << colname;
			fs.push_back(new CalcStarGlobrad(c++));
			break;
		default:
			query << colname;
			fs.push_back(new ParseAsDouble(c++));
		}
		query << (acdi+1 != acds.end() ? ", " : " ");
	}

	string dbDate =
      "date(year || \'-\' || "
			"case when month<10 then \'0\' || month else month end || \'-\' || "
      "case when day<10 then \'0\' || day  else day end)";

  query << "from data "
        << "where " << dbDate << " between date('" << connection().toDBDate(startDate) << "') "
        << "and date('" << connection().toDBDate(endDate) << "') "
        << "and not (month = 2 and day = 29) "
        << "and raster_point_id = " << cs.id() << " "
        << "order by year, month, day";

//	cout << "query: " << query.str() << endl;
	connection().select(query.str().c_str());

	int rowCount = connection().getNumberOfRows();
	map<ACD, vector<double>*> acd2ds;
  for(ACD acd : acds)
	{
		acd2ds[acd] = new vector<double>(rowCount);
	}

	Db::DBRow row;
	int count = 0;
	while(!(row = connection().getRow()).empty())
	{
		int c = 0;
    for(ACD acd : acds)
      (*(acd2ds[acd]))[count] = (*(fs.at(c++)))(row);

		count++;
	}

	for(unsigned int i = 0; i < fs.size(); i++)
		delete fs.at(i);

	return acd2ds;
}

//------------------------------------------------------------------------------

DataAccessor Star2Realization::
dataAccessorFor(const vector<AvailableClimateData>& acds,
                        const string& stationName,
                        const Date& startDate,
                        const Date& endDate)
{
  return ClimateRealization::
  dataAccessorFor(acds, simulation()->climateStation2geoCoord(stationName),
                  startDate, endDate);
}

map<ACD, vector<double>*>
Star2Realization::executeQuery(const ACDV& acds,
                               const LatLngCoord& gc, const Date& startDate,
                               const Date& endDate) const
{
  const ClimateStation& cs = simulation()->geoCoord2climateStation(gc);

  ostringstream query, query2; query << "select ";
  int c = 0;
  vector<Fun*> fs;

  for(ACDV::const_iterator acdi = acds.begin(); acdi != acds.end(); acdi++)
  {
    ACD acd = *acdi;
    switch(acd)
    {
    case Climate::globrad:
      query << availableClimateData2StarDBColName(acd);
      fs.push_back(new CalcStarGlobrad(c++));
      break;
    default:
      query << availableClimateData2StarDBColName(acd);
      fs.push_back(new ParseAsDouble(c++));
    }
    query << ", ";//(acdi+1 != acds.end() ? ", " : " ");
  }
  query << " tag as _tag, mo as _mo, jahr as _jahr ";

  string dbDate =
      "concat(jahr, \'-\', "
      "if(mo<10,concat(\'0\',mo),mo), \'-\', "
      "if(tag<10,concat(\'0\',tag),tag))";

  query2 << query.str();

	query << "from " << scenario()->id() << "_" << id() << " "
					 "where " << dbDate << " >= '" << connection().toDBDate(startDate) << "' "
					 "and " << dbDate << " <= '" << connection().toDBDate(endDate) << "' "
					 "and not (mo = 2 and tag = 29) "
					 "and id = " << cs.id();

	query2 << "from refzen "
						"where " << dbDate << " >= '" << connection().toDBDate(startDate) << "' "
						"and " << dbDate << " <= '" << connection().toDBDate(endDate) << "' "
						"and not (mo = 2 and tag = 29) "
						"and id = " << cs.id();

  query << " union " << query2.str() << " "
					 "order by _jahr, _mo, _tag";

  //cout << "query: " << query.str() << endl;
  connection().select(query.str().c_str());

  int rowCount = connection().getNumberOfRows();
  map<ACD, vector<double>*> acd2ds;
  for(ACD acd : acds)
  {
    acd2ds[acd] = new vector<double>(rowCount);
  }

	int count = 0;
	Db::MysqlDB* con = Db::toMysqlDB(&connection());
	MYSQL_ROW row;
	while((row = con->getMysqlRow()) != 0)
	{
    int c = 0;
    for(ACD acd : acds)
    {
      (*(acd2ds[acd]))[count] = (*(fs.at(c++)))(row);
    }
    count++;
  }

  for(unsigned int i = 0; i < fs.size(); i++)
    delete fs.at(i);

  return acd2ds;
}

//------------------------------------------------------------------------------

DataAccessor Star2MeasuredDataRealization::
dataAccessorFor(const vector<AvailableClimateData>& acds,
                        const string& stationName,
                        const Date& startDate,
                        const Date& endDate)
{
  return ClimateRealization::
  dataAccessorFor(acds, simulation()->climateStation2geoCoord(stationName),
                  startDate, endDate);
}

map<ACD, vector<double>*>
    Star2MeasuredDataRealization::executeQuery(const ACDV& acds,
                                               const LatLngCoord& gc,
                                               const Date& startDate,
                                               const Date& endDate) const
{
  const ClimateStation& cs = simulation()->geoCoord2climateStation(gc);

  ostringstream query; query << "select ";
  int c = 0;
  vector<Fun*> fs;

  for(ACDV::const_iterator acdi = acds.begin(); acdi != acds.end(); acdi++)
  {
    ACD acd = *acdi;
    switch(acd)
    {
    case Climate::globrad:
      query << availableClimateData2StarDBColName(acd);
      fs.push_back(new CalcStarGlobrad(c++));
      break;
    default:
      query << availableClimateData2StarDBColName(acd);
      fs.push_back(new ParseAsDouble(c++));
    }
    query << (acdi+1 != acds.end() ? ", " : " ");
  }

  string dbDate =
      "concat(jahr, \'-\', "
      "if(mo<10,concat(\'0\',mo),mo), \'-\', "
      "if(tag<10,concat(\'0\',tag),tag))";

	query << "from " << cs.dbName() << " "
					 "where " << dbDate << " >= '" << connection().toDBDate(startDate) << "' "
					 "and " << dbDate << " <= '" << connection().toDBDate(endDate) << "' "
					 "and not (mo = 2 and tag = 29) "
					 "and id = " << cs.id() << " "
					 "order by jahr, mo, tag";

  //cout << "query: " << query.str() << endl;

  connection().select(query.str().c_str());

  int rowCount = connection().getNumberOfRows();
  map<ACD, vector<double>*> acd2ds;
  for(ACD acd : acds)
  {
    acd2ds[acd] = new vector<double>(rowCount);
  }

	int count = 0;
	Db::MysqlDB* con = Db::toMysqlDB(&connection());
	MYSQL_ROW row;
	while((row = con->getMysqlRow()) != 0)
	{
    int c = 0;
    for(ACD acd : acds)
    {
      (*(acd2ds[acd]))[count] = (*(fs.at(c++)))(row);
    }
    count++;
  }

  for(unsigned int i = 0; i < fs.size(); i++)
    delete fs.at(i);

  return acd2ds;
}

//------------------------------------------------------------------------------

DataAccessor DDClimateDataServerRealization::
dataAccessorFor(const vector<AvailableClimateData>& acds,
								const string& stationName, const Date& startDate,
								const Date& endDate)
{
	return ClimateRealization::
			dataAccessorFor(acds, simulation()->climateStation2geoCoord(stationName),
											startDate, endDate);
}

map<ACD, vector<double>*>
DDClimateDataServerRealization::executeQuery(const ACDV& acds,
																						 const LatLngCoord& gc,
																						 const Date& startDate,
																						 const Date& endDate) const
{
	const ClimateStation& cs = simulation()->geoCoord2climateStation(gc);

	string dbDate =
      "concat(f.jahr, \'-\', "
      "if(f.monat<10,concat(\'0\',f.monat),f.monat), \'-\', "
      "if(f.tag<10,concat(\'0\',f.tag),f.tag))";

	ostringstream query; query << "select ";
	int c = 0;
	vector<Fun*> fs;
	for(ACDV::const_iterator acdi = acds.begin(); acdi != acds.end(); acdi++)
	{
		ACD acd = *acdi;
		switch(acd)
		{
		case Climate::globrad:
		{
			if(_setupData.simulationId() == "remo")
			{
        query << "f.nn, dayofyear(" << dbDate << ") as dy";
				int posCloudAmount = c++; int posDoy = c++;
				fs.push_back(new CalcRemoGlobrad(posCloudAmount, posDoy,
																				 cs.geoCoord().lat, cs.nn()));
			}
			else if(_setupData.simulationId() == "raklida5")
			{
				query << "f.rgj / 100.0 as globrad";
				fs.push_back(new ParseAsDouble(c++));
			}
			else
			{
        query << "f.sd, dayofyear(" << dbDate << ") as dy";
				int posSun = c++; int posYd = c++;
				fs.push_back(new CalcWettRegGlobrad(posSun, posYd, cs.geoCoord().lat));
			}
			break;
		}
		case Climate::precip:
		{
			if(_setupData.simulationId() == "remo")
			{
        query << "f.rr_drift";
				fs.push_back(new ParseAsDouble(c++));
			}
			else if(_setupData.simulationId() == "raklida5")
			{
				query << "f.rr_corr";
				fs.push_back(new ParseAsDouble(c++));
			}
			else
			{
        string csPrefix = cs.isPrecipStation() && cs.fullClimateReferenceStation()
                          ? "p" : "f";
        query << csPrefix << ".rr, f.tm, f.monat";
				int posPrecip = c++; int posTavg = c++; int posMonat = c++;
				fs.push_back(new CalcCorrWRAndCLMPrecip(posPrecip, posTavg, posMonat,
																								cs.sl()));
			}
			break;
		}
		case Climate::sunhours:
		{
			if(_setupData.simulationId() == "remo")
				query << "0 as sd";
			else
        query << "f." << availableClimateData2CLMDBColName(acd);
			fs.push_back(new ParseAsDouble(c++));
			break;
		}
		default:
      query << "f." << availableClimateData2CLMDBColName(acd);
			fs.push_back(new ParseAsDouble(c++));
			break;
		}
		query << (acdi+1 != acds.end() ? ", " : " ");
	}
  query << "from "
        << _setupData.dataDbName() << "." << _setupData.dataTableName() << " as f ";
  if(cs.isPrecipStation() && cs.fullClimateReferenceStation())
  {
    query << "inner join " << _setupData.dataDbName()
          << "." << _setupData.dataTableName() << " as p ";
    query << "on f.szenario = p.szenario "
             "and f.realisierung = p.realisierung "
             "and f.tag = p.tag "
             "and f.monat = p.monat "
             "and f.jahr = p.jahr ";
  }
  query << "where f.szenario = '" << _scenario->name() << "' "
        << "and f.realisierung = '" << id() << "' ";
  if(cs.isPrecipStation() && cs.fullClimateReferenceStation())
    query << "and f.dat_id = " << cs.fullClimateReferenceStation()->dbName() << " "
          << "and p.dat_id = " << cs.dbName() << " ";
  else
    query << "and f.dat_id = " << cs.dbName() << " ";
  query << "and " << dbDate << " >= '" << connection().toDBDate(startDate) << "' "
        << "and " << dbDate << " <= '" << connection().toDBDate(endDate) << "' "
        << "and not (f.monat = 2 and f.tag = 29) "
           "order by f.jahr, f.monat, f.tag";

  //cout << "select: " << query.str() << endl;
	connection().select(query.str().c_str());

	int rowCount = connection().getNumberOfRows();
	map<ACD, vector<double>*> acd2ds;
  for(ACD acd : acds)
	{
		acd2ds[acd] = new vector<double>(rowCount);
	}

  Db::DBRow row;
  int count = 0;
//	Db::MysqlDB* con = Db::toMysqlDB(&connection());
//	MYSQL_ROW row;
//	while((row = con->getMysqlRow()) != 0)
  while(!(row = connection().getRow()).empty())
	{
		int c = 0;
    for(ACD acd : acds)
		{
			(*(acd2ds[acd]))[count] = (*(fs.at(c++)))(row);
		}
		count++;
	}

	for(unsigned int i = 0; i < fs.size(); i++)
		delete fs.at(i);

	return acd2ds;
}

//------------------------------------------------------------------------------

DataAccessor CLMRealization::
dataAccessorFor(const vector<AvailableClimateData>& acds,
                const string& stationName, const Date& startDate,
                const Date& endDate)
{
	return ClimateRealization::
	dataAccessorFor(acds, simulation()->climateStation2geoCoord(stationName),
	                startDate, endDate);
}

map<ACD, vector<double>*>
CLMRealization::executeQuery(const ACDV& acds,
                             const LatLngCoord& gc,
                             const Date& startDate,
                             const Date& endDate) const
{
	const ClimateStation& cs = simulation()->geoCoord2climateStation(gc);

	string dbDate =
      "concat(jahr, \'-\', "
      "if(monat<10,concat(\'0\',monat),monat), \'-\', "
      "if(tag<10,concat(\'0\',tag),tag))";

	ostringstream query; query << "select ";
	int c = 0;
	vector<Fun*> fs;
  for(ACDV::const_iterator acdi = acds.begin(); acdi != acds.end(); acdi++)
  {
    ACD acd = *acdi;
    switch(acd)
    {
    case Climate::globrad:
      {
        query << "avg(sd), avg(dayofyear(" << dbDate << ")) as dy";
				int posSun = c++; int posYd = c++;
				fs.push_back(new CalcWettRegGlobrad(posSun, posYd, cs.geoCoord().lat));
				break;
			}
    case day:
    case month:
    case year:
      query << availableClimateData2CLMDBColName(acd);
      fs.push_back(new ParseAsDouble(c++));
      break;
    case precip:
      query << "avg(if("
          << availableClimateData2CLMDBColName(precip) << " < -998, "
          << availableClimateData2CLMDBColName(precipOrig) << ", "
          << availableClimateData2CLMDBColName(precip) << "))";
      fs.push_back(new ParseAsDouble(c++));
      break;
    default:
      query << "avg(" << availableClimateData2CLMDBColName(acd) << ")";
      fs.push_back(new ParseAsDouble(c++));
      break;
    }
    query << (acdi+1 != acds.end() ? ", " : " ");
	}
  CLMSimulation* sim = static_cast<CLMSimulation*>(simulation());
  ostringstream stationList;
  stationList << "(";
  for(const ClimateStation* c : sim->avgClimateStationSet(&cs))
  {
    stationList << c->dbName() << ",";
  }
  string sl = stationList.str();
  sl.at(sl.length()-1) = ')';

	query <<
					 "from clm20.clm20_data "
					 "where szenario = '" << _scenario->name() << "' "
					 "and realisierung = '" << _realizationNo << "' "
					 "and dat_id in " << sl << " "//cs.dbName() << " "
					 "and " << dbDate << " >= '" << connection().toDBDate(startDate) << "' "
					 "and " << dbDate << " <= '" << connection().toDBDate(endDate) << "' "
					 "and not (monat = 2 and tag = 29) "
					 "group by szenario, realisierung, tag, monat, jahr "
					 "order by jahr, monat, tag";

  //cout << "select: " << query.str() << endl;
	connection().select(query.str().c_str());

	int rowCount = connection().getNumberOfRows();
	map<ACD, vector<double>*> acd2ds;
  for(ACD acd : acds)
  {
    acd2ds[acd] = new vector<double>(rowCount);
  }

	int count = 0;
	Db::MysqlDB* con = Db::toMysqlDB(&connection());
	MYSQL_ROW row;
	while((row = con->getMysqlRow()) != 0)
  {
		int c = 0;
    for(ACD acd : acds)
    {
      (*(acd2ds[acd]))[count] = (*(fs.at(c++)))(row);
    }
		count++;
	}

	for(unsigned int i = 0; i < fs.size(); i++)
		delete fs.at(i);

	return acd2ds;
}

//------------------------------------------------------------------------------

ClimateDataManager& Climate::climateDataManager()
{
	static ClimateDataManager cdm;
  static mutex lockable;
  static bool initialized = false;
  if(!initialized)
  {
    lock_guard<mutex> lock(lockable);

    if(!initialized)
    {
			const Names2Values& n2vs =
          Db::dbConnectionParameters().values("active-climate-db-schemas");
			set<string> s;
      for(auto n2v : n2vs)
        s.insert(n2v.first);
//            transform(n2vs.begin(), n2vs.end(), inserter(s, s.begin()),
//								std::bind(&Names2Values::value_type::first, std::placeholders::_1));
			cdm.loadAvailableSimulations(s);
      initialized = true;
    }
  }

	return cdm;
}

ClimateSimulationPtr Climate::createSimulationFromSetupData(const IniParameterMap& dbParams,
                                                     const string& abstractSchema)
{
  string dbSection = dbParams.value("abstract-schema", abstractSchema);
  string type = dbParams.value(dbSection, "type");

  ClimateSimulationPtr sim;

  if(type == "landcare-climate-data-server")
  {
    string setupSection = dbSection + "." + abstractSchema;
    DDServerSetup setup(dbParams.values(setupSection));
    if(setup.setupComplete())
      sim = make_shared<DDClimateDataServerSimulation>(setup, Db::newConnection(abstractSchema));
  }
  else if(type == "user-sqlite-climate-db")
    sim = make_shared<UserSqliteDBSimulation>(Db::newConnection(abstractSchema));

  return sim;
}

DDServerSetup::DDServerSetup(std::map<string, string> setupSectionMap)
{
  _simulationId = setupSectionMap["simulation-id"];
  _simulationName = setupSectionMap["simulation-name"];
  _headerDbName = setupSectionMap["header-db-name"];
  _headerTableName = setupSectionMap["header-table-name"];
  _stolistDbName = setupSectionMap["stolist-db-name"];
  _stolistTableName = setupSectionMap["stolist-table-name"];
  _dataDbName = setupSectionMap["data-db-name"];
  _dataTableName = setupSectionMap["data-table-name"];
  _errorDbName = setupSectionMap["error-db-name"];
  _errorTableName = setupSectionMap["error-table-name"];
  for(auto scId : Tools::splitString(setupSectionMap["scenarios"], ", "))
    _scenarioIds.push_back(scId);
  for(auto rId : Tools::splitString(setupSectionMap["realizations"], ", "))
    _realizationIds.push_back(rId);
  vector<string> sYearRange = Tools::splitString(setupSectionMap["years-from-to"], ", ");
  if(sYearRange.size() == 2)
    yearRange = YearRange(stoi(sYearRange.front()), stoi(sYearRange.back()));

  if(_simulationId.empty() ||
     _simulationName.empty() ||
     _headerTableName.empty() ||
     _stolistTableName.empty() ||
     _dataDbName.empty() ||
     _dataTableName.empty() ||
     _scenarioIds.empty() ||
     _realizationIds.empty())
  {
    cout << "Setup of climate simulation data for DD climate data server failed! Ignoring this simulation!" << endl;
    cout << "Setup section map was: ";
    for(auto p : setupSectionMap)
      cout << p.first << " -> " << p.second << endl;
  }
  else
    _setupComplete = true;
}

vector<ClimateSimulationPtr> ClimateDataManager::loadSimulation(string abstractSchema)
{
  using namespace Db;

  vector<ClimateSimulationPtr> sims;

  //first load hardcoded simulations
  if(abstractSchema == "clm20-9")
    sims.push_back(make_shared<CLMSimulation>(newConnection("clm20-9")));
  else if(abstractSchema == "star")
    sims.push_back(make_shared<StarSimulation>(newConnection("star")));
  else if(abstractSchema == "star2")
  {
    sims.push_back(make_shared<Star2Simulation>(newConnection("star2")));
    sims.push_back(make_shared<Star2MeasuredDataSimulation>(newConnection("star2")));
  }
  else
  {
    if(auto sim = createSimulationFromSetupData(dbConnectionParameters(), abstractSchema))
      sims.push_back(sim);
  }

  return sims;
}


void ClimateDataManager::loadAvailableSimulations(set<string> ass)
{
  auto storeSims = [&](const vector<ClimateSimulationPtr>& css)
  {
    for(auto sim : css)
      _abstractSchema2simulation[sim->id()] = sim;;
  };

  auto dbParams = Db::dbConnectionParameters();
  for(string abstractSchema : ass)
  {
    string dbSection = dbParams.value("abstract-schema", abstractSchema);
    string type = dbParams.value(dbSection, "type");

    if(type == "multi-ensemble-simulations")
    {
      auto ncs = make_shared<ClimateSimulation>();
      auto nsc = make_shared<ClimateScenario>("---", ncs.get());
      ncs->addScenario(nsc);

      Stations climateStations;

      string setupSection = dbSection + "." + abstractSchema;
      for(auto p : dbParams.values(setupSection))
      {
        auto key = p.first;
        auto value = p.second;
        if(key == "simulation-id")
          ncs->setId(value);
        else if(key == "simulation-name")
          ncs->setName(value);
        else
        {
          auto simScenReal = splitString(value, " ,");
          //cout << "simScenReal: " << value << endl;
          if(!simScenReal.empty() && simScenReal.size() == 3)
          {
            auto sim = simScenReal.at(0);
            auto addRealizationAndClimateStations = [&](ClimateSimulationPtr cs)
            {
              if(auto sc = cs->scenario(simScenReal.at(1)))
                if(auto r = sc->realizationPtr(simScenReal.at(2)))
                  nsc->addRealizations({r});

              map<int, ClimateStationPtr> id2fullClimateStation;

              for(auto s : cs->climateStations())
              {
                //we already added a full climate station
                //when copying a precipitation station
                if(id2fullClimateStation.find(s->id()) != id2fullClimateStation.end())
                  continue;

                auto csCopy = make_shared<ClimateStation>(*s);
                csCopy->setSimulation(ncs.get());

                if(csCopy->isPrecipStation())
                {
                  auto id2fcsit = id2fullClimateStation.find(s->id());
                  ClimateStationPtr fullCS;
                  if(id2fcsit == id2fullClimateStation.end())
                  {
                    fullCS = make_shared<ClimateStation>(*csCopy->fullClimateReferenceStation());
                    fullCS->setSimulation(ncs.get());

                    //add new full climate station copy
                    id2fullClimateStation[fullCS->id()] = fullCS;
                    climateStations.push_back(fullCS);
                  }
                  else
                    fullCS = id2fcsit->second;

                  //add full reference station to precipition station copy
                  csCopy->setFullClimateReferenceStation(fullCS.get());
                }
                else
                  id2fullClimateStation[csCopy->id()] = csCopy;

                climateStations.push_back(csCopy);
              }
            };

            auto csi = _abstractSchema2simulation.find(sim);
            if(csi != _abstractSchema2simulation.end())
              addRealizationAndClimateStations(csi->second);
            else
            {
              auto css = loadSimulation(sim);
              storeSims(css);
              for(auto cs : css)
                if(cs->id() == sim)
                  addRealizationAndClimateStations(cs);
            }
          }
        }
      }

      if(ncs->id().empty())
        ncs->setId(abstractSchema);
      if(ncs->name().empty())
        ncs->setName(capitalize(abstractSchema));

      ncs->setClimateStations(climateStations);

      if(!nsc->realizations().empty())
        storeSims({ncs});
    }
    else
      storeSims(loadSimulation(abstractSchema));
  }
}

//void ClimateDataManager::loadAvailableSimulations(set<string> ass)
//{
//	using namespace Db;
//	bool isMexicoMode = false; //true;
//	if(!isMexicoMode)
//	{
//		if(ass.find("clm20-9") != ass.end())
//			_simulations.push_back(new CLMSimulation(newConnection("clm20-9")));
//		if(ass.find("clm20") != ass.end())
//			_simulations.push_back(newDDClm20());
//		if(ass.find("star") != ass.end())
//			_simulations.push_back(new StarSimulation(newConnection("star")));
//		if(ass.find("star2") != ass.end())
//		{
//			_simulations.push_back(new Star2Simulation(newConnection("star2")));
//			_simulations.push_back(new Star2MeasuredDataSimulation(newConnection("star2")));
//		}
//		if(ass.find("wettreg2006") != ass.end())
//		{
//			//put in front to designate the default
//			_simulations.insert(_simulations.begin(), newDDWettReg2006());
//		}
//		if(ass.find("wettreg2010") != ass.end())
//		{
//			_simulations.push_back(newDDWettReg2010());
//		}
//		if(ass.find("remo") != ass.end())
//			_simulations.push_back(newDDRemo());
//		if(ass.find("werex4") != ass.end())
//			_simulations.push_back(newDDWerex4());
//		if(ass.find("werex5_eh5_l1") != ass.end())
//			_simulations.push_back(newDDWerex5_eh5_l1());
//		if(ass.find("werex5_eh5_l1_clm") != ass.end())
//			_simulations.push_back(newDDWerex5_eh5_l1_clm());
//		if(ass.find("werex5_eh5_l2") != ass.end())
//			_simulations.push_back(newDDWerex5_eh5_l2());
//		if(ass.find("werex5_eh5_l2_clm") != ass.end())
//			_simulations.push_back(newDDWerex5_eh5_l2_clm());
//		if(ass.find("werex5_eh5_l3") != ass.end())
//			_simulations.push_back(newDDWerex5_eh5_l3());
//		if(ass.find("werex5_eh5_l3_racmo") != ass.end())
//			_simulations.push_back(newDDWerex5_eh5_l3_racmo());
//		if(ass.find("werex5_eh5_l3_remo") != ass.end())
//			_simulations.push_back(newDDWerex5_eh5_l3_remo());
//		if(ass.find("werex5_hc3c_l1_a1b") != ass.end())
//			_simulations.push_back(newDDWerex5_hc3c_l1_a1b());
//		if(ass.find("werex5_hc3c_l1_e1") != ass.end())
//			_simulations.push_back(newDDWerex5_hc3c_l1_e1());
//	}

//	if(ass.find("echam5") != ass.end())
//		_simulations.push_back(newDDEcham5());
//	if(ass.find("echam6") != ass.end())
//			_simulations.push_back(newDDEcham6());
//	if(ass.find("hrm3") != ass.end())
//	{
//		_simulations.push_back(newDDHrm3(YearRange(1971, 2000)));
//		_simulations.push_back(newDDHrm3(YearRange(2041, 2070)));
//	}
//	if(ass.find("cru") != ass.end())
//		_simulations.push_back(newDDCru());
//  if(ass.find("dwd-nrw") != ass.end())
//    _simulations.push_back(newDDDwdNrw());

//	if(!isMexicoMode)
//		if(ass.find("carbiocial-climate") != ass.end())
//      _simulations.push_back(new UserSqliteDBSimulation(newConnection("carbiocial-climate")));
//}


vector<ClimateSimulation*> ClimateDataManager::allClimateSimulations() const
{
  vector<ClimateSimulation*> css;
  for(auto p : _abstractSchema2simulation)
    css.push_back(p.second.get());
  return css;
}

ClimateSimulation* ClimateDataManager::defaultSimulation() const
{
  const auto& css = allClimateSimulations();
  return css.empty() ? nullptr : css.front();
}

//------------------------------------------------------------------------------

//DDClimateDataServerSimulation* Climate::newDDWettReg2006(string userRs)
//{
//	DDServerSetup setup("wettreg2006", "WettReg2006", "header", "wettreg_stolist",
//											"wettreg2006", "wettreg_data", "wettreg_fehler_regklam");
//	setup._scenarioIds.push_back("A1B");
//	setup._scenarioIds.push_back("A2");
//	setup._scenarioIds.push_back("B1");

//	string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "wettreg2006", "tro_a, nor_a, feu_a") : userRs;
//	vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//	return new DDClimateDataServerSimulation(setup, Db::newConnection("wettreg2006"));
//}

//DDClimateDataServerSimulation* Climate::newDDWettReg2010(string userRs)
//{
//	DDServerSetup setup("wettreg2010", "WettReg2010", "header", "wettreg2010_stolist",
//											"wettreg2010", "wettreg2010_data");
//	setup._scenarioIds.push_back("A1B");
//	setup._scenarioIds.push_back("B1");
//	setup._scenarioIds.push_back("A2");

//	string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "wettreg2010", "00, 55, 99") : userRs;
//	vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//	return new DDClimateDataServerSimulation(setup, Db::newConnection("wettreg2010"));
//}

//DDClimateDataServerSimulation* Climate::newDDRemo()
//{
//	DDServerSetup setup("remo", "REMO", "header_remo", "remo_stolist",
//											"remo", "remo_data", "remo_fehler_regklam");
//	setup._scenarioIds.push_back("A1B");
//	setup._scenarioIds.push_back("B1");
//	setup._realizationIds.push_back("1");
//	return new DDClimateDataServerSimulation(setup, Db::newConnection("remo"));
//}

//DDClimateDataServerSimulation* Climate::newDDWerex4(string userRs)
//{
//	DDServerSetup setup("werex4", "WEREX4", "header", "werex4_stolist",
//											"werex4", "werex4_data", "werex4_fehler_regklam");
//	setup._scenarioIds.push_back("A1B");
//	setup._scenarioIds.push_back("A2");
//	setup._scenarioIds.push_back("B1");

//	string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "werex4", "tro, nor, feu") : userRs;
//	vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//	return new DDClimateDataServerSimulation(setup, Db::newConnection("werex4"));
//}

//DDClimateDataServerSimulation* Climate::newDDWerex5_eh5_l1(string userRs)
//{
//	DDServerSetup setup("werex5_eh5_l1", "WEREX5-EH5-L1", "header", "werex5_stolist",
//											"werex5", "werex5_eh5_l1");
//	setup._scenarioIds.push_back("A1B");
//	setup._scenarioIds.push_back("E1");

//	string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "werex5_eh5_l1", "77") : userRs;
//	vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//	return new DDClimateDataServerSimulation(setup, Db::newConnection("werex5"));
//}

//DDClimateDataServerSimulation* Climate::newDDWerex5_eh5_l1_clm(string userRs)
//{
//	DDServerSetup setup("werex5_eh5_l1_clm", "WEREX5-EH5-L1-CLM", "header", "werex5_stolist",
//											"werex5", "werex5_eh5_l1_clm");
//	setup._scenarioIds.push_back("A1B");

//	string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "werex5_eh5_l1_clm", "55") : userRs;
//	vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//	return new DDClimateDataServerSimulation(setup, Db::newConnection("werex5"));
//}

//DDClimateDataServerSimulation* Climate::newDDWerex5_eh5_l2(string userRs)
//{
//	DDServerSetup setup("werex5_eh5_l2", "WEREX5-EH5-L2", "header", "werex5_stolist",
//											"werex5", "werex5_eh5_l2");
//	setup._scenarioIds.push_back("A1B");

//	string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "werex5_eh5_l2", "44") : userRs;
//	vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//	return new DDClimateDataServerSimulation(setup, Db::newConnection("werex5"));
//}

//DDClimateDataServerSimulation* Climate::newDDWerex5_eh5_l2_clm(string userRs)
//{
//	DDServerSetup setup("werex5_eh5_l2_clm", "WEREX5-EH5-L2-CLM", "header", "werex5_stolist",
//											"werex5", "werex5_eh5_l2_clm");
//	setup._scenarioIds.push_back("A1B");
//	setup._scenarioIds.push_back("E1");

//	string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "werex5_eh5_l2_clm", "33") : userRs;
//	vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//	return new DDClimateDataServerSimulation(setup, Db::newConnection("werex5"));
//}

//DDClimateDataServerSimulation* Climate::newDDWerex5_eh5_l3(string userRs)
//{
//	DDServerSetup setup("werex5_eh5_l3", "WEREX5-EH5-L3", "header", "werex5_stolist",
//											"werex5", "werex5_eh5_l3");
//	setup._scenarioIds.push_back("A1B");

//	string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "werex5_eh5_l3", "33") : userRs;
//	vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//	return new DDClimateDataServerSimulation(setup, Db::newConnection("werex5"));
//}

//DDClimateDataServerSimulation* Climate::newDDWerex5_eh5_l3_racmo(string userRs)
//{
//	DDServerSetup setup("werex5_eh5_l3_racmo", "WEREX5-EH5-L3-RACMO", "header", "werex5_stolist",
//											"werex5", "werex5_eh5_l3_racmo");
//	setup._scenarioIds.push_back("A1B");

//	string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "werex5_eh5_l3_racmo", "00") : userRs;
//	vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//	return new DDClimateDataServerSimulation(setup, Db::newConnection("werex5"));
//}

//DDClimateDataServerSimulation* Climate::newDDWerex5_eh5_l3_remo(string userRs)
//{
//	DDServerSetup setup("werex5_eh5_l3_remo", "WEREX5-EH5-L3-REMO", "header", "werex5_stolist",
//											"werex5", "werex5_eh5_l3_remo");
//	setup._scenarioIds.push_back("A1B");

//	string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "werex5_eh5_l3_remo", "88") : userRs;
//	vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//	return new DDClimateDataServerSimulation(setup, Db::newConnection("werex5"));
//}

//DDClimateDataServerSimulation* Climate::newDDWerex5_hc3c_l1_a1b(string userRs)
//{
//	DDServerSetup setup("werex5_hc3c_l1_a1b", "WEREX5-HC3C-L1-A1B", "header", "werex5_stolist",
//											"werex5", "werex5_hc3c_l1");
//	setup._scenarioIds.push_back("A1B");

//	string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "werex5_eh5_l1_a1b", "00") : userRs;
//	vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//	return new DDClimateDataServerSimulation(setup, Db::newConnection("werex5"));
//}

//DDClimateDataServerSimulation* Climate::newDDWerex5_hc3c_l1_e1(string userRs)
//{
//	DDServerSetup setup("werex5_hc3c_l1_e1", "WEREX5-HC3C-L1-E1", "header", "werex5_stolist",
//											"werex5", "werex5_hc3c_l1");
//	setup._scenarioIds.push_back("E1");

//	string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "werex5_eh5_l1_e1", "44") : userRs;
//	vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//	return new DDClimateDataServerSimulation(setup, Db::newConnection("werex5"));
//}

//DDClimateDataServerSimulation* Climate::newDDClm20(string userRs)
//{
//	DDServerSetup setup("clm20", "CLM20", "header_clm20", "clm20_stolist",
//											"clm20", "clm20_data", "clm20_fehler_regklam");
//	setup._scenarioIds.push_back("A1B");
//	setup._scenarioIds.push_back("B1");

//	string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "clm20", "1, 2") : userRs;
//	vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//	return new DDClimateDataServerSimulation(setup, Db::newConnection("clm20"));
//}

//DDClimateDataServerSimulation* Climate::newDDEcham5(string userRs)
//{
//	DDServerSetup setup("echam5", "ECHAM5", "header_echam5", "echam5_stolist",
//											"project_mexiko", "echam5_data", string(), "project_mexiko");
//	setup._scenarioIds.push_back("A1B");
//	setup._scenarioIds.push_back("A2");

//	string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "echam5", "1") : userRs;
//	vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//	return new DDClimateDataServerSimulation(setup, Db::newConnection("echam5"));
//}

//DDClimateDataServerSimulation* Climate::newDDEcham6(string userRs)
//{
//	DDServerSetup setup("echam6", "ECHAM6", "header_echam6", "echam6_stolist",
//											"project_mexiko", "echam6_data", string(), "project_mexiko");
//	setup._scenarioIds.push_back("rcp85");

//	string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "echam6", "1") : userRs;
//	vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//	return new DDClimateDataServerSimulation(setup, Db::newConnection("echam6"));
//}

//DDClimateDataServerSimulation* Climate::newDDHrm3(YearRange yr, string userRs)
//{
//	ostringstream ss;
//	ss << "HRM3-" << yr.fromYear << "/" << yr.toYear;
//	DDServerSetup setup("hrm3", ss.str(), "header_hrm3", "hrm3_stolist",
//											"project_mexiko", "hrm3_data", string(), "project_mexiko");
//	setup.yearRange = yr;
//	setup._scenarioIds.push_back("A2");

//	string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "hrm3", "1") : userRs;
//	vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//	return new DDClimateDataServerSimulation(setup, Db::newConnection("hrm3"));
//}


//DDClimateDataServerSimulation* Climate::newDDCru(string userRs)
//{
//	DDServerSetup setup("cru", "CRU", "header_cru", "cru_stolist",
//											"project_mexiko", "cru_data", string(), "project_mexiko");
//	setup._scenarioIds.push_back("CRU");

//	string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "cru", "3.1") : userRs;
//	vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//	return new DDClimateDataServerSimulation(setup, Db::newConnection("cru"));
//}

//DDClimateDataServerSimulation* Climate::newDDDwdNrw(string userRs)
//{
//  DDServerSetup setup("dwd-nrw", "DWD-NRW", "header", "dwd_nrw_stolist",
//                      "landcare", "dwd_nrw_data", string(), "landcare");
//  setup._scenarioIds.push_back("OBS");

//  string rs = userRs.empty() ? Db::dbConnectionParameters().value("used-realizations", "dwd-nrw", "00") : userRs;
//  vector<string> vsr = Tools::splitString(rs, ", ");
//  for(string s : vsr) { setup._realizationIds.push_back(s); }

//  return new DDClimateDataServerSimulation(setup, Db::newConnection("dwd-nrw"));
//}


//------------------------------------------------------------------------------

void Climate::testClimate()
{
	//typedef Climate<AllDBData> Climate;

	vector<string> stations;
	/*
	typedef StarClimate<DBData> StarClimate;
 	StarClimate climate;
	stations.push_back("drd_");
	stations.push_back("ang_");
	stations.push_back("pre_");
	stations.push_back("mub_");
	stations.push_back("gri_");
	stations.push_back("bro_");
	//*/

	/*
	typedef WettRegClimate<AllDBData> WettRegClimate;
	WettRegClimate climate(WettRegClimate::Scenario::A1B(), WettRegClimate::dry);
	stations.push_back("DRESDEN");
	stations.push_back("ANGERMUENDE");
	stations.push_back("MUENCHEBERG");
	stations.push_back("BROCKEN");
	//*/

	/*
	vector<string>::const_iterator sit;
	for(sit = stations.begin(); sit != stations.end(); sit++){
		string station(*sit);
		cout << "station: " << station << endl;

		Date s0(1,1,2000); Date e0(31,1,2000);
		cout << "caching range: " << s0.toMysqlString()
		<< " - " << e0.toMysqlString() << endl;
		Climate::LWDataAccessor da0 = climate.dataAccessorFor(station, s0, e0);
		const Climate::Cache& c = da0._cache;
		cout << "c.startDate: " << c.toDBDate(startDate)
		<< " c.endDate: " << c.toDBDate(endDate) << endl;
		assert(c.startDate == s0 && c.endDate == e0);
		cout << "c.size(): " << c.size() << endl;
		assert(c.size() == 31);
		cout << "da0.offset: " << da0.offset() << endl;
		assert(da0.offset() == 0);

		Date s1(1,1,2002); Date e1(31,12,2002);
		cout << "caching range: " << s1.toMysqlString()
		<< " - " << e1.toMysqlString() << endl;
		Climate::LWDataAccessor da1 = climate.dataAccessorFor(station, s1, e1);
		cout << "c.startDate: " << c.toDBDate(startDate)
		<< " c.endDate: " << c.toDBDate(endDate) << endl;
		assert(c.startDate == s0 && c.endDate == e1);
		cout << "c.size(): " << c.size() << endl;
		assert(c.size() == 31 + (365 - 31) + 365 + 365);
		cout << "da1.offset: " << da1.offset() << endl;
		assert(da1.offset() == 31 + (365 - 31) + 365);
		cout << "da0.offset: " << da0.offset() << endl;
		assert(da0.offset() == 0);

		Date s2(20,12,1999); Date e2(25,12,1999);
		cout << "caching range: " << s2.toMysqlString()
		<< " - " << e2.toMysqlString() << endl;
		Climate::LWDataAccessor da2 = climate.dataAccessorFor(station, s2, e2);
		cout << "c.startDate: " << c.toDBDate(startDate)
		<< " c.endDate: " << c.toDBDate(endDate) << endl;
		assert(c.startDate == s2 && c.endDate == e1);
		cout << "c.size(): " << c.size() << endl;
		assert(c.size() == 6 + 6 + 31 + (365 - 31) + 365 + 365);
		cout << "da2.offset: " << da2.offset() << endl;
		assert(da2.offset() == 0);
		cout << "da1.offset: " << da1.offset() << endl;
		assert(da1.offset() == 6 + 6 + 31 + (365 - 31) + 365);
		cout << "da0.offset: " << da0.offset() << endl;
		assert(da0.offset() == 6 + 6);

		Date s3(10,12,1999); Date e3(25,12,2002);
		cout << "caching range: " << s3.toMysqlString()
		<< " - " << e3.toMysqlString() << endl;
		Climate::LWDataAccessor da3 = climate.dataAccessorFor(station, s3, e3);
		cout << "c.startDate: " << c.toDBDate(startDate)
		<< " c.endDate: " << c.toDBDate(endDate) << endl;
		assert(c.startDate == s3 && c.endDate == e1);
		cout << "c.size(): " << c.size() << endl;
		assert(c.size() == 10 + 6 + 6 + 31 + (365 - 31) + 365 + 365);
		cout << "da3.offset: " << da3.offset() << endl;
		assert(da3.offset() == 0);
		cout << "da2.offset: " << da2.offset() << endl;
		assert(da2.offset() == 10);
		cout << "da1.offset: " << da1.offset() << endl;
		assert(da1.offset() == 10 + 6 + 6 + 31 + (365 - 31) + 365);
		cout << "da0.offset: " << da0.offset() << endl;
		assert(da0.offset() == 10 + 6 + 6);

		Date s4(1,1,2001); Date e4(15,2,2003);
		cout << "caching range: " << s4.toMysqlString()
		<< " - " << e4.toMysqlString() << endl;
		Climate::LWDataAccessor da4 = climate.dataAccessorFor(station, s4, e4);
		cout << "c.startDate: " << c.toDBDate(startDate)
		<< " c.endDate: " << c.toDBDate(endDate) << endl;
		assert(c.startDate == s3 && c.endDate == e4);
		cout << "c.size(): " << c.size() << endl;
		assert(c.size() == 10 + 6 + 6 + 31 + (365 - 31) + 365 + 365 + 46);
		cout << "da4.offset: " << da4.offset() << endl;
		assert(da4.offset() == 10 + 6 + 6 + 365);
		cout << "da3.offset: " << da3.offset() << endl;
		assert(da3.offset() == 0);
		cout << "da2.offset: " << da2.offset() << endl;
		assert(da2.offset() == 10);
		cout << "da1.offset: " << da1.offset() << endl;
		assert(da1.offset() == 10 + 6 + 6 + 31 + (365 - 31) + 365);
		cout << "da0.offset: " << da0.offset() << endl;
		assert(da0.offset() == 10 + 6 + 6);

		Date s5(30,6,2001); Date e5(14,12,2002);
		cout << "caching range: " << s5.toMysqlString()
		<< " - " << e5.toMysqlString() << endl;
		Climate::LWDataAccessor da5 = climate.dataAccessorFor(station, s5, e5);
		cout << "c.startDate: " << c.toDBDate(startDate)
		<< " c.endDate: " << c.toDBDate(endDate) << endl;
		assert(c.startDate == s3 && c.endDate == e4);
		cout << "c.size(): " << c.size() << endl;
		assert(c.size() == 10 + 6 + 6 + 31 + (365 - 31) + 365 + 365 + 46);
		cout << "da5.offset: " << da5.offset() << endl;
		assert(da5.offset() == 10 + 6 + 6 + 365 + 31 + 28 + 31 + 30 + 31 + 29);
		cout << "da4.offset: " << da4.offset() << endl;
		assert(da4.offset() == 10 + 6 + 6 + 365);
		cout << "da3.offset: " << da3.offset() << endl;
		assert(da3.offset() == 0);
		cout << "da2.offset: " << da2.offset() << endl;
		assert(da2.offset() == 10);
		cout << "da1.offset: " << da1.offset() << endl;
		assert(da1.offset() == 10 + 6 + 6 + 31 + (365 - 31) + 365);
		cout << "da0.offset: " << da0.offset() << endl;
		assert(da0.offset() == 10 + 6 + 6);
	}
	*/
}
