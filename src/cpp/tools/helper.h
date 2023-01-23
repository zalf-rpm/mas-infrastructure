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

#include <cmath>
#include <sstream>
#include <string>
#include <cstring>
#include <locale>
#include <vector>
#include <algorithm>

namespace Tools {
struct Errors {
  enum Type { ERR, WARN };

  Errors() {}
  Errors(std::string e) { errors.push_back(e); }
  Errors(Type t, std::string m) { if (t == ERR) errors.push_back(m); else warnings.push_back(m); }
  Errors(std::vector<std::string> es) : errors(es) {}

  inline bool success() const { return errors.empty(); }
  inline bool failure() const { return !success(); }

  void append(const Errors& es) {
    errors.insert(errors.end(), es.errors.begin(), es.errors.end());
    warnings.insert(warnings.end(), es.warnings.begin(), es.warnings.end());
  }

  void appendError(std::string err) { errors.push_back(err); }
  void appendWarning(std::string warn) { warnings.push_back(warn); }

  std::vector<std::string> errors;
  std::vector<std::string> warnings;
};

template<typename T>
struct EResult : public Errors {
  EResult() {}
  EResult(T result) : result(result) {}
  EResult(T result, std::string e) : Errors(e), result(result) {}
  EResult(T result, Type t, std::string m) : Errors(t, m), result(result) { }
  EResult(T result, std::vector<std::string> es) : Errors(es), result(result) {}
  EResult(T result, Errors es) : Errors(es), result(result) {}

  T result;
};

bool printPossibleErrors(const Errors& es, bool includeWarnings = false);

template<typename T>
T printPossibleErrors(const EResult<T>& er, bool includeWarnings = false) {
  if (printPossibleErrors(Errors(er))) return er.result;
  return T();
};

//---------------------------------------------------------------------------

//code adapted from: http://stackoverflow.com/questions/7690864/haskell-style-maybe-type-chaining-in-c11
template<typename T>
class Maybe {
public:
  Maybe() : _value(T()), _isNothing(true) {}
  Maybe(const T& a) : _value(a), _isNothing(false) {}
  Maybe(const Maybe& b) : _value(b._value), _isNothing(b._isNothing) {}

  T value() const { return _value; }
  void setValue(const T& v) { _value = v; _isNothing = false; }
  bool isValue() const { return !isNothing(); }
  bool isNothing() const { return _isNothing; }

  Maybe& operator=(const T& b) { return *this = Maybe(b); }

  Maybe& operator=(const Maybe& b)  {
    _value = b._value;
    _isNothing = b._isNothing;
    return *this;
  }
private:
  T _value;
  bool _isNothing = false;
};

//----------------------------------------------------------------------------

template<typename T, typename S>
struct Cast { S operator()(const T& in) const { return S(in); } };

//! Identity functor
template<class T>
struct Id { T operator()(const T& in) const { return in; } };

//! functor returning the first value of a given pair
template<class Pair>
struct ExtractFirst {
  typename Pair::first_type
    operator()(const Pair& p) const { return p.first; }
};

//! functor returning the second value of a given pair
template<class Pair>
struct ExtractSecond {
  typename Pair::second_type
    operator()(const Pair& p) const { return p.second; }
};

inline double stod_comma(std::string s) {
  size_t pos = s.find_first_of(',');
  if (pos != std::string::npos) return std::stod(s.replace(pos, 1, "."));

  return std::stod(s);
}

template<typename T>
bool flt_equal_eps(T f1, T f2, T eps = std::numeric_limits<T>::epsilon()) {
  return ((std::fabs(f1) < eps) && (std::fabs(f2) < eps))
    || std::fabs(T(f1 - f2)) < eps;
}

template<typename T> bool flt_equal_zero(T f, T eps = std::numeric_limits<T>::epsilon()) {
  return  flt_equal_eps(T(0.0), f, eps);
}

template<typename T>
inline T const& bound_max(T const& val, T const& max) {
  return val > max ? max : val;
}

template<typename T> inline T sqr(T const& n) { return  n * n; }

inline bool fuzzyIsNull(double d) {
  return std::abs(d) <= 0.000000000001;
}

inline bool fuzzyIsNull(float f) {
  return std::abs(f) <= 0.00001f;
}

inline bool fuzzyCompare(double p1, double p2) {
  using std::abs;
  using std::min;
  return abs(p1 - p2) <= 0.000000000001 * min(abs(p1), abs(p2));
}

inline bool fuzzyCompare(float p1, float p2) {
  using std::abs;
  using std::min;
  return abs(p1 - p2) <= 0.00001f * min(abs(p1), abs(p2));
}

//! get value from map with default value if not found
template<class Map, typename KT, typename VT>
VT valueD(const Map& map, KT key, VT def) {
  typename Map::const_iterator ci = map.find(key);
  return ci == map.end() ? def : ci->second;
}

//! get value from map with default-constructed value if not found
template<class Map, typename KT>
typename Map::mapped_type value(const Map& map, KT key) {
  return valueD(map, key, typename Map::mapped_type());
}

//! short cut to value(map, key)
template<class Map, typename KT>
typename Map::mapped_type operator&(const Map& map, KT key) {
  return value(map, key);
}

//! get value from two nested maps with given default if nothing found
template<class Map, typename KT, typename KT2, typename VT2>
VT2 valueD(const Map& map, KT key, KT2 key2, VT2 def) {
  typename Map::const_iterator ci = map.find(key);
  if (ci != map.end())   {
    typedef typename Map::mapped_type VT;
    typename VT::const_iterator ci2 = ci->second.find(key2);
    if (ci2 != ci->second.end()) return ci2->second;
  }
  return def;
}

//! get value from two nested maps with default constructed default if not found
template<class Map, typename KT, typename KT2>
typename Map::mapped_type::mapped_type value(const Map& map, KT key, KT2 key2) {
  return valueD(map, key, key2, typename Map::mapped_type::mapped_type());
}

//! get value from three nested maps with given default of nothing found
template<class Map, typename KT, typename KT2, typename KT3, typename VT3>
VT3 valueD(const Map& map, KT key, KT2 key2, KT3 key3, VT3 def) {
  typename Map::const_iterator ci = map.find(key);
  if (ci != map.end()) {
    typedef typename Map::mapped_type VT;
    typename VT::const_iterator ci2 = ci->second.find(key2);
    if (ci2 != ci->second.end()) {
     typedef typename VT::mapped_type VT2;
     typename VT2::const_iterator ci3 = ci2->second.find(key3);
     if (ci3 != ci2->end()) return ci3->second;
    }
  }
  return def;
}

//! get value from three nested maps with default constructed default if nothing found
template<class Map, typename KT, typename KT2, typename KT3>
typename Map::mapped_type::mapped_type::mapped_type
  value(const Map& map, KT key, KT2 key2, KT3 key3) {
  return valueD(map, key, key2, key3, typename Map::mapped_type::mapped_type::mapped_type());
}

//----------------------------------------------------------------------------

//! square the argument
template<typename T> inline T sq(T x) { return x * x; }

//----------------------------------------------------------------------------

template<typename Container>
Container range(int from, int to = 0, int step = 1) {
  Container c;
  for (int i = from; i <= to; i += step) c.insert(c.end(), typename Container::value_type(i));
  return c;
}

//----------------------------------------------------------------------------

template<typename T>
bool between(T low, T value, T high) {
  return low <= value && value <= high;
}

//----------------------------------------------------------------------------

/*!
  * create a vector out of the template argument enum and it's size
  * enum values have to be continous and start at 0
  */
template<typename E, int size>
const std::vector<E>& vectorOfEs() {
  static std::vector<E> v(size);
  static bool initialized = false;
  if (!initialized) {
    for (int i = 0; i < size; i++) v[i] = E(i);
    initialized = true;
  }
  return v;
}

//----------------------------------------------------------------------------

// listBuilder has to have the same size as collection
template<typename Collection, typename CapnpListBuilder>
void setCapnpList(const Collection& values, CapnpListBuilder listBuilder) {
  uint i = 0;
  for (auto value : values) listBuilder.set(i++, value);
}

//----------------------------------------------------------------------------

// listBuilder has to have the same size as collection
template<typename Collection, typename CapnpListBuilder>
void setComplexCapnpList(const Collection& values, CapnpListBuilder listBuilder) {
  uint i = 0;
  for (const auto& value : values) value.serialize(listBuilder[i++]);
}

//----------------------------------------------------------------------------

template<typename Collection, typename CapnpListReader>
void setFromCapnpList(Collection& values, CapnpListReader listReader) {
  values.resize(listReader.size());
  uint i = 0;
  for (auto value : listReader) values[i++] = value;
}

//----------------------------------------------------------------------------

template<typename Collection, typename CapnpListReader>
void setFromComplexCapnpList(Collection& values, CapnpListReader listReader) {
  values.resize(listReader.size());
  uint i = 0;
  for (auto& value : values) value.deserialize(listReader[i++]);
}

//----------------------------------------------------------------------------

//  template<typename T, bool isStdFundamental>
//  struct ToString
//  {
//  std::string operator()(const T& object){ return object.toString(); }
//  };

//  template<typename T>
//  struct ToString<T, true>
//  {
//  std::string operator()(const T& object)
//  {
//   std::ostringstream s;
//   s << object;
//   return s.str();
//  }
//  };

//  template<typename T>
//  struct ToString<T, false>
//  {
//  std::string operator()(const T& object)
//  {
//   std::ostringstream s;
//   s << "[";
//   typename T::const_iterator end = object.end();
//   for(typename T::const_iterator ci = object.begin(); ci != end; ci++)
//    s << *ci << (ci+1 == end ? "]" : ",");
//   return s.str();
//  }
//  };

//  template<typename T>
//  std::string toString(T t)
//  {
//  const bool isStdF = Loki::TypeTraits<T>::isStdFundamental;
//  return ToString<T, isStdF>()(t);
//  }

template<typename T>
std::string toString(T t, std::string indent = std::string(), bool detailed = false) { return t.toString(indent, detailed); }

template<typename T>
std::string toString(T* t, std::string indent = std::string(), bool detailed = false) { return t->toString(indent, detailed); }

//  template<typename Container>
//  std::string toString(const Container& c)
//  {
//   std::ostringstream s;
//   s << "[";
//   typename Container::const_iterator end = c.end();
//   for(typename Container::const_iterator ci = c.begin(); ci != end; ci++)
//    s << *ci << (ci+1 == end ? "]" : ",");
//   return s.str();
//  }

inline std::string toString(int i) { std::ostringstream s; s << i; return s.str(); }

inline std::string toString(double d) { std::ostringstream s; s << d; return s.str(); }

inline std::string toString(float f) { std::ostringstream s; s << f; return s.str(); }

inline std::string toString(bool b) { std::ostringstream s; s << b; return s.str(); }

//----------------------------------------------------------------------------

//makes year a multiple of second parameter
inline int multipleOfDown(int value, int mult = 5) {
  return value - value % mult;
}

inline int multipleOfUp(int value, int mult = 5) {
  return value + (value % mult == 0 ? 0 : mult - value % mult);
}

//-----------------------------------------------------------------------

inline std::string toLower(const std::string& str) {
  std::locale loc;
  std::string lowerStr;
  for (auto elem : str) lowerStr.append(1, std::tolower(elem, loc));
  return lowerStr;
}

inline std::string toUpper(const std::string& str) {
  std::locale loc;
  std::string upperStr;
  for (auto elem : str) upperStr.append(1, std::toupper(elem, loc));
  return upperStr;
}

//-------------------------------------------------------------------------

std::string replace(std::string s, std::string findStr, std::string replStr);

inline int satoi(const std::string& s, int def = 0) {
  return s.empty() ? def : std::stoi(s);
}

inline double satof(const std::string& s, double def = 0.0) {
  return s.empty() ? def : std::stof(s);
}

bool stob(const std::string& s, bool def = false);

inline bool satob(const std::string& s, bool def = false) { return stob(s, def); }

inline std::string surround(std::string with, std::string str) { return with + str + with; };

EResult<std::string> readFile(std::string path);

std::pair<std::string, std::string> splitPathToFile(const std::string& pathToFile);

bool isAbsolutePath(const std::string& path);

std::string fixSystemSeparator(std::string path);

bool ensureDirExists(const std::string& path);

bool directoryExist(const std::string& path);

std::string rimRight(const std::string& str, const std::string& charSet);

std::string replaceEnvVars(std::string path);

std::string winStringSystemCodepageToutf8(const std::string& str);

inline std::string pathSeparator()
{
  return
#ifdef __unix__
    "/";
#else
    "\\";
#endif
}

} //namespace Tools
