/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg@zalf.de>

Maintainers:
Currently maintained by the authors.

This file is part of the MONICA model.
Copyright (C) Leibniz Centre for Agricultural Landscape Research (ZALF)
*/
#include "soil/soil.capnp.h"
#include "common/common.h"

namespace mas::infrastructure::soil {

class Profile final : public mas::schema::soil::Profile::Server, public mas::infrastructure::common::Identifiable {
public:
  Profile() = default;

  virtual ~Profile() noexcept(false) = default;

  //data        @0 () -> ProfileData;
  kj::Promise<void> data(DataContext context) override;

  //geoLocation @1 () -> Geo.LatLonCoord;
  kj::Promise<void> geoLocation(GeoLocationContext context) override;

};

class Stream final : public mas::schema::soil::Service::Stream::Server {
public:
  Stream() = default;

  virtual ~Stream() noexcept(false) = default;

  //nextProfiles @0 (maxCount :Int64 = 100) -> (profiles :List(Profile));
  kj::Promise<void> nextProfiles(NextProfilesContext context) override;
};

class Service final : public mas::schema::soil::Service::Server {
public:
  Service() = default;

  virtual ~Service() noexcept(false) = default;

  //checkAvailableParameters @0 Query -> Query.Result;
  kj::Promise<void> checkAvailableParameters(CheckAvailableParametersContext context) override;

  //getAllAvailableParameters @1 (onlyRawData :Bool) -> (mandatory :List(PropertyName), optional :List(PropertyName));
  kj::Promise<void> getAllAvailableParameters(GetAllAvailableParametersContext context) override;

  //closestProfilesAt @2 (coord :Geo.LatLonCoord, query :Query) -> (profiles :List(Profile));
  kj::Promise<void> closestProfilesAt(ClosestProfilesAtContext context) override;

  //streamAllProfiles @3 Query -> (allProfiles :Stream);
  kj::Promise<void> streamAllProfiles(StreamAllProfilesContext context) override;
};

}