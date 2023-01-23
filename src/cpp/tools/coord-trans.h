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

#ifndef COORDTRANS_H_
#define COORDTRANS_H_

#include <vector>
#include <cmath>
#include <cassert>
#include <memory>
#include <climits>
#include <iostream>
#include <map>

#include "proj_api.h"

namespace Tools
{

struct CoordConversionParams
{
  double sourceConversionFactor;
  double targetConversionFactor;
  bool switch2DCoordinates;
  std::string projectionParams;
};


struct CoordinateSystemData
{
  std::string name, shortName;
  CoordConversionParams proj4Params;
};
typedef std::shared_ptr<CoordinateSystemData> CoordinateSystemDataPtr;

//can be used as value object
struct CoordinateSystem
{
  CoordinateSystem(int id = 0) : id(id) {}
  CoordinateSystem(int id, CoordinateSystemDataPtr data) : id(id), data(data) {}

  inline bool operator==(const CoordinateSystem& other) const
  {
    return id == other.id && data.get() == other.data.get();
  }

  inline bool operator!=(const CoordinateSystem& other) const
  {
    return id != other.id || data.get() != other.data.get();
  }

  inline bool operator<(const CoordinateSystem& other) const
  {
    return id < other.id;
  }

  bool isValid() const { return id > 0 || data.get() != NULL; }

  int id;
  CoordinateSystemDataPtr data;
};

inline CoordConversionParams coordConversionParams(CoordinateSystem cs)
{
  if(cs.data)
    return cs.data->proj4Params;

  return CoordConversionParams();
}


/*
enum CoordinateSystem
{
  LatLng_EPSG4326 = 0,
  UTM21S_EPSG32721,
  GK5_EPSG31469,
  UTM32N_EPSG25832,
  UndefinedCoordinateSystem
};
*/

std::string coordinateSystemToString(CoordinateSystem cs);

std::string coordinateSystemToShortString(CoordinateSystem cs);

CoordinateSystem shortStringToCoordinateSystem(std::string cs, CoordinateSystem def = CoordinateSystem());

template<typename T>
struct Coord2D
{
  Coord2D() : coordinateSystem(CoordinateSystem()) {}
  Coord2D(CoordinateSystem cs) : coordinateSystem(cs) {}
  virtual T firstDimension() const = 0;
  virtual T secondDimension() const = 0;
  CoordinateSystem coordinateSystem;
};

struct RectCoord : public Coord2D<double>
{
  RectCoord() : Coord2D(CoordinateSystem()), r(0), h(0) { }

  RectCoord(CoordinateSystem cs) : Coord2D(cs), r(0), h(0) { }

  //		RectCoord(double r, double h) : Coord2D(GK5_EPSG31469), r(r), h(h) { }

  RectCoord(CoordinateSystem cs, double r, double h)
    : Coord2D(cs), r(r), h(h) { }

  virtual double firstDimension() const { return r; }
  virtual double secondDimension() const { return h; }

  RectCoord operator+(const RectCoord& other) const
  {
    assert(coordinateSystem == other.coordinateSystem);
    return RectCoord(coordinateSystem, r + other.r, h + other.h);
  }

  RectCoord operator-(const RectCoord & other) const
  {
    return (*this) + (other*-1);
  }

  RectCoord operator+(double value) const
  {
    return RectCoord(coordinateSystem, r + value, h + value);
  }

  RectCoord operator-(double value) const
  {
    return (*this) + (-1*value);
  }

  RectCoord operator*(double value) const
  {
    return RectCoord(coordinateSystem, r*value, h*value);
  }

  RectCoord operator/(double value) const
  {
    return (*this) * (1.0 / value);
  }

  double distanceTo(const RectCoord & other) const
  {
    return std::sqrt(((r - other.r)*(r - other.r)) + ((h - other.h)*(h - other.h)));
  }

  std::string toString() const;

  bool isZero() const
  {
    return int(r) == 0 && int(h) == 0;
  }

  double r;
  double h;
};

//----------------------------------------------------------------------------

/*! A Geocoordinate Pair (latitude, longitude)
   * geocoordinates
   */
struct LatLngCoord : public Coord2D<double>
{
  static CoordinateSystem latLngCoordinateSystem();

  LatLngCoord() : Coord2D(latLngCoordinateSystem()), lat(-9999.0), lng(-9999.0) {}

  LatLngCoord(CoordinateSystem)
    : Coord2D(latLngCoordinateSystem()), lat(-9999.0), lng(-9999.0)
  {}

  LatLngCoord(double lat, double lng)
    : Coord2D(latLngCoordinateSystem()), lat(lat), lng(lng)
  {}

  LatLngCoord(CoordinateSystem, double lat, double lng)
    : Coord2D(latLngCoordinateSystem()), lat(lat), lng(lng)
  {}

  virtual double firstDimension() const { return lat; }
  virtual double secondDimension() const { return lng; }

  bool operator==(const LatLngCoord& other) const;

  bool operator<(const LatLngCoord & other) const;

  LatLngCoord operator+(const LatLngCoord& other) const;

  LatLngCoord operator-(const LatLngCoord& other) const
  {
    return (*this) + (other * -1);
  }

  void operator+=(const LatLngCoord& other);

  void operator-=(const LatLngCoord& other){ (*this) += (other * -1); }

  LatLngCoord operator*(double scaleFactor) const;

  double distanceTo(const LatLngCoord & other) const;

  std::string toCanonicalString(std::string leftBound = std::string(),
                                std::string fill = std::string(),
                                std::string rightBound = std::string()) const;

  std::string toString() const;

  bool isZero() const { return int(lat) == 0 && int(lng) == 0; }

  bool isValid() const { return int(lat) != -9999 && int(lng) != -9999; }

  double lat{-9999.0};
  double lng{-9999.0};
  static const double eps;
};



//----------------------------------------------------------------------------



template<typename SourceCoordType, typename TargetCoordType>
std::vector<TargetCoordType>
sourceProj2targetProj(const std::vector<SourceCoordType>& sourceCoords,
                      CoordinateSystem targetCoordinateSystem);

template<typename SourceCoordType, typename TargetCoordType>
TargetCoordType singleSourceProj2targetProj(SourceCoordType s,
                                            CoordinateSystem targetCoordinateSystem);

//	template<typename SP, typename TP>
//	std::vector<typename TP::CoordType>
//	sourceProj2targetProj(const std::vector<typename SP::CoordType>& sourceCoords);

//	template<typename SP, typename TP>
//	typename TP::CoordType sourceProj2targetProj(typename SP::CoordType s);

//Lat/Lng to GK5

//	inline std::vector<RectCoord> latLng2GK5(const std::vector<LatLngCoord>& lls)
//	{ return sourceProj2targetProj<LatLng_EPSG4326_Params, GK5_Params>(lls); }

//	inline RectCoord latLng2GK5(LatLngCoord llc)
//	{ return sourceProj2targetProj<LatLng_EPSG4326_Params, GK5_Params>(llc); }


inline std::vector<RectCoord> latLng2RC(const std::vector<LatLngCoord>& lls,
                                        CoordinateSystem targetCoordinateSystem)
{
  return sourceProj2targetProj<LatLngCoord, RectCoord>(lls, targetCoordinateSystem);
}

inline RectCoord latLng2RC(LatLngCoord llc, CoordinateSystem targetCoordinateSystem)
{
  return singleSourceProj2targetProj<LatLngCoord, RectCoord>(llc, targetCoordinateSystem);
}

//	inline std::vector<RectCoord> latLng2RC(const std::vector<LatLngCoord>& lls)
//	{ return sourceProj2targetProj<LatLng_EPSG4326_Params, GK5_Params>(lls); }

//	inline RectCoord latLng2RC(LatLngCoord llc)
//	{ return sourceProj2targetProj<LatLng_EPSG4326_Params, GK5_Params>(llc); }

//Gk5 to Lat/Lng
//	inline std::vector<LatLngCoord> GK52latLng(const std::vector<RectCoord>& rcs)
//	{ return sourceProj2targetProj<GK5_Params, LatLng_EPSG4326_Params>(rcs); }

//	inline LatLngCoord GK52latLng(RectCoord rcc)
//	{ return sourceProj2targetProj<GK5_Params, LatLng_EPSG4326_Params>(rcc); }

inline std::vector<LatLngCoord> RC2latLng(const std::vector<RectCoord>& rcs)
{
  return sourceProj2targetProj<RectCoord, LatLngCoord>(rcs, shortStringToCoordinateSystem("latlng"));
}

inline LatLngCoord RC2latLng(RectCoord rcc)
{
  return singleSourceProj2targetProj<RectCoord, LatLngCoord>(rcc, shortStringToCoordinateSystem("latlng"));
}

//Lat/Lng to UTM21S
//	inline std::vector<UTM21SCoord> latLng2UTM21S(const std::vector<LatLngCoord>& lls)
//	{ return sourceProj2targetProj<LatLng_EPSG4326_Params, UTM21S_EPSG32721_Params>(lls); }

//	inline UTM21SCoord latLng2UTM21S(LatLngCoord llc)
//	{ return sourceProj2targetProj<LatLng_EPSG4326_Params, UTM21S_EPSG32721_Params>(llc); }

//UTM21S to Lat/Lng
//	inline std::vector<LatLngCoord> UTM21S2latLng(const std::vector<UTM21SCoord>& utms)
//	{ return sourceProj2targetProj<UTM21S_EPSG32721_Params, LatLng_EPSG4326_Params>(utms); }

//	inline LatLngCoord UTM21S2latLng(UTM21SCoord utmc)
//	{ return sourceProj2targetProj<UTM21S_EPSG32721_Params, LatLng_EPSG4326_Params>(utmc); }

bool contains(std::vector<LatLngCoord> tlTrBrBlRect, LatLngCoord point);
}

//template implementations
//-------------------------------

template<typename SCT, typename TCT>
std::vector<TCT>
Tools::sourceProj2targetProj(const std::vector<SCT>& scs, CoordinateSystem targetCS)
{
	if(scs.empty())
		return std::vector<TCT>();

	struct AutoDeleter
	{
		std::map<std::string, projPJ> cache;
		AutoDeleter() {}
		~AutoDeleter() { for(auto p : cache) pj_free(p.second); }
	};
	static AutoDeleter ad;
	auto& cache = ad.cache;

  CoordConversionParams sccp = coordConversionParams(scs.front().coordinateSystem);
  CoordConversionParams tccp = coordConversionParams(targetCS);

  unsigned int nocs = (unsigned int)scs.size(); //no of coordinates
  double* xs = new double[nocs];
  double* ys = new double[nocs];
  double* zs = new double[nocs];

  double scv = sccp.sourceConversionFactor;
  bool ss2Dc = sccp.switch2DCoordinates;
  for(unsigned int i = 0; i < nocs; i++)
  {
    xs[i] = ss2Dc ? scs.at(i).secondDimension() * scv
                  : scs.at(i).firstDimension() * scv;
    ys[i] = ss2Dc ? scs.at(i).firstDimension() * scv
                  : scs.at(i).secondDimension() * scv;
    zs[i] = 0;
  }

	auto sci = cache.find(sccp.projectionParams);
	if(sci == cache.end())
	{
		cache[sccp.projectionParams] = pj_init_plus(sccp.projectionParams.c_str());
		sci = cache.find(sccp.projectionParams);
	}
	auto tci = cache.find(tccp.projectionParams);
	if(tci == cache.end())
	{
		cache[tccp.projectionParams] = pj_init_plus(tccp.projectionParams.c_str());
		tci = cache.find(tccp.projectionParams);
	}

	projPJ sourcePJ = sci->second;
	projPJ targetPJ = tci->second;

	//projPJ sourcePJ = pj_init_plus(sccp.projectionParams.c_str());
	//projPJ targetPJ = pj_init_plus(tccp.projectionParams.c_str());

  if(sourcePJ && targetPJ){
    int error = pj_transform(sourcePJ, targetPJ, nocs, 0, xs, ys, zs);
    if(error)
    {
      std::cerr << "error: " << error << std::endl;
      return std::vector<TCT>();
    }
  }

  std::vector<TCT> tcs(nocs);
  double tcv = tccp.targetConversionFactor;
  bool ts2Dc = tccp.switch2DCoordinates;
  for(unsigned int i = 0; i < nocs; i++)
    tcs[i] = ts2Dc ? TCT(targetCS, ys[i]*tcv, xs[i]*tcv)
                   : TCT(targetCS, xs[i]*tcv, ys[i]*tcv);

	//if(sourcePJ)
	//  pj_free(sourcePJ);
	//if(targetPJ)
	//  pj_free(targetPJ);

  delete[] xs;
  delete[] ys;
  delete[] zs;

	//static int count = 0;
	//count++;
	//if(count % 1000 == 0)
	//	std::cout << count << " ";

  return !sourcePJ || !targetPJ ? std::vector<TCT>() : tcs;
}

template<typename SCT, typename TCT>
TCT Tools::singleSourceProj2targetProj(SCT sc, CoordinateSystem targetCS)
{
  auto v = sourceProj2targetProj<SCT, TCT>(std::vector<SCT>(1, sc), targetCS);
  return v.empty() ? TCT(targetCS) : v.front();
}




#endif
