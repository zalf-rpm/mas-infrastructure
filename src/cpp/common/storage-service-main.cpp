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

  kj::MainBuilder::Validity setRestorerContainerId(kj::StringPtr id) { restorerContainerId = kj::str(id); return true; }

  kj::MainBuilder::Validity setServiceContainerId(kj::StringPtr id) { serviceContainerId = kj::str(id); return true; }

  kj::MainBuilder::Validity startService()
  {
    KJ_LOG(INFO, "Starting SQLite storage service.");

    // create storage service
    auto ownedStorageService = kj::heap<SqliteStorageService>(pathToDB, name, description);
    auto storageService = ownedStorageService.get();
    mas::schema::storage::Store::Client storageServiceClient = kj::mv(ownedStorageService);

    // make sure we have a storage container for the restorer of the storage service itself
    // no container id or sturdy ref given, create a new container and return its id
    if(restorerContainerId.size() == 0 && restorerContainerSR.size() == 0){
      // get a new container
      auto req = storageServiceClient.newContainerRequest();
      req.setName(kj::str("Restorer data (", name, ")"));
      auto rcc = req.send().wait(ioContext.waitScope).getContainer();
      // get the id of the container
      auto containerId = rcc.infoRequest().send().wait(ioContext.waitScope).getId();
      restorerContainerClient = rcc;
      KJ_LOG(INFO, "Id of newly create container for restorer:", containerId);
    } else if(restorerContainerId.size() > 0) { // we got an container id for the local store, get container
      auto req = storageServiceClient.containerWithIdRequest();
      req.setId(restorerContainerId);
      restorerContainerClient = req.send().wait(ioContext.waitScope).getContainer();
    } 

    // make sure we have a storage container for the storage service itself
    // no container id or sturdy ref given, create a new container and return its id
    if(serviceContainerId.size() == 0 && serviceContainerSR.size() == 0){
      // get a new container
      auto req = storageServiceClient.newContainerRequest();
      req.setName(kj::str("Service data (", name, ")"));
      auto sccP = req.send().getContainer();
      // get the id of the container
      auto containerId = sccP.infoRequest().send().wait(ioContext.waitScope).getId();
      serviceContainerClient = sccP;
      KJ_LOG(INFO, "Id of newly create container for storage service:", containerId);
    } else if(serviceContainerId.size() > 0) { // we got an container id for the local store, get container
      auto req = storageServiceClient.containerWithIdRequest();
      req.setId(serviceContainerId);
      serviceContainerClient = req.send().wait(ioContext.waitScope).getContainer();
    } 

    // start the restorer service 
    startRestorerSetup(storageServiceClient);
    storageService->setRestorer(restorer);

    // restore the service under the last used sturdy ref (token), if we got a service storage container
    kj::String serviceSR;
    KJ_IF_MAYBE(scc, serviceContainerClient) {
      auto req = scc->getEntryRequest();
      req.setKey(LAST_STORAGE_SERVICE_KEY_NAME);
      auto entryProm = req.send().getEntry();
      auto value = entryProm.getValueRequest().send().wait(ioContext.waitScope);
      if(value.getIsUnset()) { // there was no set last token, so we need to create a new one
        auto ssr = restorer->saveStr(storageServiceClient, nullptr, nullptr, false, RESTORE_STORAGE_ITSELF_TOKEN_VALUE, true).wait(ioContext.waitScope);
        // keep the sturdy ref around for output to the user
        serviceSR = kj::mv(ssr.sturdyRef);
        // and save the actual token as the one we used
        auto svReq = entryProm.setValueRequest();
        svReq.initValue().setTextValue(kj::mv(ssr.srToken));
        auto succ = svReq.send().wait(ioContext.waitScope).getSuccess();
        KJ_LOG(INFO, "Storing sturdy ref token for service restart was successful: ", succ);
      } else { // there was a previously stored token, so just create a sturdy ref for output from it
        auto serviceSRToken = kj::str(value.getValue().getTextValue());
        serviceSR = restorer->saveStr(storageServiceClient, serviceSRToken, nullptr, false, nullptr, false).wait(ioContext.waitScope).sturdyRef;
      }
    } else { // we got no storage connected, so just create a new sturdy ref for this incarnation of the service
      serviceSR = restorer->saveStr(storageServiceClient, nullptr, nullptr, false, nullptr, false).wait(ioContext.waitScope).sturdyRef;
    }
    KJ_LOG(INFO, serviceSR);

    KJ_LOG(INFO, "Created storage service.");
    
    // Run forever, accepting connections and handling requests.
    kj::NEVER_DONE.wait(ioContext.waitScope);
  }

  kj::MainFunc getMain()
  {
    return addRestorableServiceOptions()
      .addOptionWithArg({'f', "filename"}, KJ_BIND_METHOD(*this, setPathToDB),
                        "<sqlite-db-filename (default: storage_service.sqlite)>", "Path to storage service' sqlite db.")
      .addOptionWithArg({"storage_container_id"}, KJ_BIND_METHOD(*this, setRestorerContainerId),
                        "<sturdy_ref (default: not set = create new local container)>", "Sturdy ref to container for this restorer.")
      .addOptionWithArg({"service_container_id"}, KJ_BIND_METHOD(*this, setServiceContainerId),
                        "<sturdy_ref (default: not set = create new local container)>", "Sturdy ref to container for this service.")
      .callAfterParsing(KJ_BIND_METHOD(*this, startService))
      .build();
  }

private:
  kj::String pathToDB{kj::str("storage_service.sqlite")};
  kj::String restorerContainerId;
  kj::String serviceContainerId;
};

} // namespace storage
} // namespace infrastructure
} // namespace mas

KJ_MAIN(mas::infrastructure::storage::StorageServiceMain)
