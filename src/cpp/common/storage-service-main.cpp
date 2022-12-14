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

#include <kj/debug.h>
#include <kj/common.h>
#include <kj/main.h>
#include <kj/string.h>
#include <kj/vector.h>

#include "rpc-connection-manager.h"
#include "common.h"
#include "restorer.h"
#include "sole.hpp"

#include "storage-service.h"
#include "restorable-service-main.h"

#include "common.capnp.h"
#include "storage.capnp.h"
#include "registry.capnp.h"

namespace mas { 
namespace infrastructure { 
namespace storage {

using RSM = mas::infrastructure::common::RestorableServiceMain;

class StorageServiceMain : public RSM
{
public:
  StorageServiceMain(kj::ProcessContext &context) 
  : RSM(context, "SQLite Storage Service v0.1", "Offers a SQLite backed storage service.")
  {}

  kj::MainBuilder::Validity setPathToDB(kj::StringPtr path) { pathToDB = kj::str(path); return true; }

  kj::MainBuilder::Validity startService()
  {
    KJ_LOG(INFO, "starting SQLite storage service");

    // create storage service
    auto store = kj::heap<SqliteStorageService>(pathToDB, name.asPtr(), description.asPtr());
    auto& storeRef = *store;
    mas::schema::storage::Store::Client storeClient = kj::mv(store);

    // make sure we a storage container for the storage service itself
    kj::Maybe<mas::schema::storage::Store::Container::Client> restorerContainerClient;
    // no container id or sturdy ref given, create a new container and return its id
    if(restorerContainerId.size() == 0 && restorerContainerSR.size() == 0){
      // get a new container
      auto req = storeClient.newContainerRequest();
      req.setName("Restorer data");
      auto rcc = req.send().wait(ioContext.waitScope).getContainer();
      // get the id of the container
      auto containerId = rcc.infoRequest().send().wait(ioContext.waitScope).getId();
      restorerContainerClient = rcc;
      KJ_LOG(INFO, "Id of newly create container for restorer:", containerId);
    } else if(restorerContainerId.size() > 0) { // we got an container id for the local store, get container
      auto req = storeClient.containerWithIdRequest();
      req.setId(restorerContainerId);
      restorerContainerClient = req.send().wait(ioContext.waitScope).getContainer();
    } 

    // start the restorer service 
    auto restorer = kj::get<1>(startRestorableParts(storeClient, restorerContainerClient));
    storeRef.setRestorer(restorer);

    kj::String serviceSR;
    KJ_IF_MAYBE(rcc, restorerContainerClient) {
      auto req = rcc->getEntryRequest();
      req.setKey("storage_service_sr");
      auto entryProm = req.send().getEntry();
      auto value = entryProm.getValueRequest().send().wait(ioContext.waitScope);
      if(value.getIsUnset()) {
        auto ssr = restorer->saveStr(storeClient).wait(ioContext.waitScope);
        serviceSR = kj::mv(ssr.sturdyRef);
        auto svReq = entryProm.setValueRequest();
        svReq.initValue().setTextValue(kj::mv(ssr.srToken));
        auto succ = svReq.send().wait(ioContext.waitScope).getSuccess();
        KJ_LOG(INFO, "set storage service sturdy ref in restorer container: ", succ);
      } else {
        auto serviceSRToken = kj::str(value.getValue().getTextValue());
        serviceSR = restorer->saveStr(storeClient, serviceSRToken, nullptr, false, nullptr, false).wait(ioContext.waitScope).sturdyRef;
      }
    } else {
      // get the sturdy ref to the storage service itself    
      serviceSR = restorer->saveStr(storeClient, nullptr, nullptr, false, nullptr, true).wait(ioContext.waitScope).sturdyRef;
    }
    KJ_LOG(INFO, serviceSR);
    KJ_LOG(INFO, "created storage service");
    
    // Run forever, accepting connections and handling requests.
    kj::NEVER_DONE.wait(ioContext.waitScope);
  }

  kj::MainFunc getMain()
  {
    return addRestorableServiceOptions()
      .addOptionWithArg({'f', "filename"}, KJ_BIND_METHOD(*this, setPathToDB),
                        "<sqlite-db-filename (default: storage_service.sqlite)>", "Path to storage service' sqlite db.")
      .callAfterParsing(KJ_BIND_METHOD(*this, startService))
      .build();
  }

private:
  kj::String pathToDB{kj::str("storage_service.sqlite")};
};

} // namespace storage
} // namespace infrastructure
} // namespace mas

KJ_MAIN(mas::infrastructure::storage::StorageServiceMain)
