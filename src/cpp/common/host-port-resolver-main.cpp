/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg@zalf.de>

Maintainers:
Currently maintained by the authors.

This file is part of the ZALF model and simulation infrastructure.
Copyright (C) Leibniz Centre for Agricultural Landscape Research (ZALF)
*/

#include <iostream>

#include <kj/debug.h>
#include <kj/common.h>
#include <kj/main.h>
#include <kj/string.h>
#include <kj/vector.h>

#include "host-port-resolver.h"
#include "common.h"
#include "restorable-service-main.h"
#include "rpc-connection-manager.h"
#include "sole.hpp"

#include "common.capnp.h"

namespace mas::infrastructure::common {

class HostPortResolverMain : public RestorableServiceMain
{
public:
    HostPortResolverMain(kj::ProcessContext& context)
  : RestorableServiceMain(context, "Host-Port-Resolver v0.1", "Offers a service to resolves other services current host and port.")
  {}

  kj::MainBuilder::Validity startService() {
    KJ_LOG(INFO, "starting host-port-resolver service");

    auto ownedResolver = kj::heap<HostPortResolver>(ioContext.provider->getTimer(), name, description);
    auto resolver = ownedResolver.get();
    mas::schema::persistence::HostPortResolver::Client resolverClient = kj::mv(ownedResolver);
    KJ_LOG(INFO, "created host-port-resolver");

    startRestorerSetup(resolverClient, true);
    resolver->setRestorer(restorer, restorerClient);
    KJ_IF_MAYBE(scc, serviceContainerClient) resolver->setStorageContainer(*scc);

    auto registrarClient = resolver->createRegistrar();
    KJ_LOG(INFO, "created host-port-resolver registrar");

    auto ssr = restorer->saveStr(registrarClient).wait(ioContext.waitScope);
    if(outputSturdyRefs) std::cout << "registrarSR=" << ssr.sturdyRef.cStr() << std::endl;

    // Run forever, using regularly cleaning mappings, accepting connections and handling requests.
    auto gcmProm = resolver->garbageCollectMappings();
    gcmProm.then([](){ return kj::NEVER_DONE; }).wait(ioContext.waitScope);
    KJ_LOG(INFO, "stopped host-port-resolver service");
    return true;
  }

  kj::MainFunc getMain()
  {
    return addRestorableServiceOptions()
      .callAfterParsing(KJ_BIND_METHOD(*this, startService))
      .build();
  }
};

} // namespace mas::infrastructure::common

KJ_MAIN(mas::infrastructure::common::HostPortResolverMain)
