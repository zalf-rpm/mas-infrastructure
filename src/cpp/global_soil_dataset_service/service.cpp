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
#include "service.h"

#include <string>
#include <vector>

#include <kj/debug.h>
#include <kj/common.h>
#include <kj/main.h>
#include <kj/string.h>

#include "common/common.h"

using namespace mas::infrastructure::soil;

kj::Promise<void> Profile::data(DataContext context) {

  return kj::READY_NOW;
}

kj::Promise<void> Profile::geoLocation(GeoLocationContext context) {

  return kj::READY_NOW;
}

kj::Promise<void> Stream::nextProfiles(NextProfilesContext context) {

  return kj::READY_NOW;
}

kj::Promise<void> Service::checkAvailableParameters(CheckAvailableParametersContext context) {

  return kj::READY_NOW;
}

kj::Promise<void> Service::getAllAvailableParameters(GetAllAvailableParametersContext context) {

  return kj::READY_NOW;
}

kj::Promise<void> Service::closestProfilesAt(ClosestProfilesAtContext context) {

  return kj::READY_NOW;
}

kj::Promise<void> Service::streamAllProfiles(StreamAllProfilesContext context) {

  return kj::READY_NOW;
}


//  kj::Promise<void> m(MContext context) override {
//    auto hello = context.getParams().getHello();
//    if (i % 10000 == 0) {
//      std::cout << ".";
//      std::cout.flush();
//    }
//    i++;
//    if (hello == "done") exit(0);
//    return kj::READY_NOW;
//  }
