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

#include "common.capnp.h"
#include "storage.capnp.h"
#include "registry.capnp.h"

namespace mas { 
namespace infrastructure { 
namespace storage {

class StorageServiceMain
{
public:
  StorageServiceMain(kj::ProcessContext &context) 
  : restorer(kj::heap<mas::infrastructure::common::Restorer>())
  , conMan(restorer.get())
  , context(context)
  , ioContext(kj::setupAsyncIo()) 
  {}

  kj::MainBuilder::Validity setName(kj::StringPtr n) { name = kj::str(n); return true; }

  kj::MainBuilder::Validity setDescription(kj::StringPtr d) { description = kj::str(d); return true; }

  kj::MainBuilder::Validity setHost(kj::StringPtr name) { host = kj::str(name); return true; }

  kj::MainBuilder::Validity setLocalHost(kj::StringPtr h) { localHost = kj::str(h); return true; }

  kj::MainBuilder::Validity setPort(kj::StringPtr name) { port = std::max(0, std::stoi(name.cStr())); return true; }

  kj::MainBuilder::Validity setCheckPort(kj::StringPtr portStr) { checkPort = portStr.parseAs<int>(); return true; }

  kj::MainBuilder::Validity setCheckIP(kj::StringPtr ip) { checkIP = kj::str(ip); return true; }

  kj::MainBuilder::Validity setPathToDB(kj::StringPtr path) { pathToDB = kj::str(path); return true; }

  kj::MainBuilder::Validity setRestorerContainerSR(kj::StringPtr sr) { registrarSR = kj::str(sr); return true; }

  kj::MainBuilder::Validity setRegistrarSR(kj::StringPtr sr) { registrarSR = kj::str(sr); return true; }

  kj::MainBuilder::Validity setRegName(kj::StringPtr n) { regName = kj::str(n); return true; }

  kj::MainBuilder::Validity setRegCategory(kj::StringPtr cat) { regCategory = kj::str(cat); return true; }

  kj::MainBuilder::Validity startService()
  {
    KJ_LOG(INFO, "starting SQLite storage service");

    auto& restorerRef = *restorer;
    mas::schema::persistence::Restorer::Client restorerClient = kj::mv(restorer);
    auto service = kj::heap<SqliteStorageService>(&restorerRef, pathToDB, name.asPtr(), description.asPtr());
    auto& serviceRef = *service;
    mas::schema::storage::Store::Client serviceClient = kj::mv(service);

    //connect to restorer container
    mas::schema::storage::Store::Container::Client restoreContainerClient = nullptr;
    if(restorerContainerSR.size() == 0){
      // get a new container
      auto req = serviceClient.newContainerRequest();
      req.setName("Restorer data");
      restoreContainerClient = req.send().wait(ioContext.waitScope).getContainer();
      // set this container as the store for the restorer
      restorerRef.setStore(restoreContainerClient);
      // get a sturdy ref to the new container (which will also store the sturdy ref in the new container)
      auto sr = kj::get<0>(restoreContainerClient.saveRequest().send().wait(ioContext.waitScope));
      KJ_LOG(INFO, "Sturdyref to newly create container for restorer:", sr);
    } else {
      restoreContainerClient = conMan.tryConnectB(ioContext, restorerContainerSR).castAs<mas::schema::storage::Store::Container>();
      restorerRef.setStore(restoreContainerClient);
    }
    KJ_LOG(INFO, "created service");
    
    KJ_LOG(INFO, "trying to bind to", host, port);
    auto portPromise = conMan.bind(ioContext, restorerClient, host, port);
    auto succAndIP = infrastructure::common::getLocalIP(checkIP, checkPort);
    if(kj::get<0>(succAndIP)) restorerRef.setHost(kj::get<1>(succAndIP));
    else restorerRef.setHost(localHost);
    auto port = portPromise.wait(ioContext.waitScope);
    restorerRef.setPort(port);
    KJ_LOG(INFO, "bound to", host, port);

    auto restorerSR = restorerRef.sturdyRefStr("");
    auto serviceSR = kj::get<0>(restorerRef.saveStr(serviceClient).wait(ioContext.waitScope));
    KJ_LOG(INFO, serviceSR);
    KJ_LOG(INFO, restorerSR);

    mas::schema::common::Action::Client unregister(nullptr);
    //mas::schema::persistence::SturdyRef::Reader reregSR(nullptr);
    mas::schema::registry::Registrar::Client registrar(nullptr);
    if(registrarSR.size() > 0) {
      KJ_LOG(INFO, "trying to register at", registrarSR);
      registrar = conMan.tryConnectB(ioContext, registrarSR).castAs<mas::schema::registry::Registrar>();
      auto request = registrar.registerRequest();
      request.setCap(serviceClient);
      request.setRegName(regName.size() == 0 ? name.asPtr() : regName.asPtr());
      request.setCategoryId(regCategory);
      auto xd = request.initXDomain();
      restorerRef.setVatId(xd.initVatId());
      xd.setRestorer(restorerClient);
      try {
        auto response = request.send().wait(ioContext.waitScope);
        if(response.hasUnreg()) { 
          unregister = response.getUnreg();
          serviceRef.setUnregisterAction(unregister);
        }
        //if(response.hasReregSR()) reregSR = response.getReregSR();
        KJ_LOG(INFO, "registered at", registrarSR);
      } catch(kj::Exception e) {
        KJ_LOG(ERROR, "Error sending register message to Registrar! Error", e.getDescription().cStr());
      }
    }

    // Run forever, accepting connections and handling requests.
    kj::NEVER_DONE.wait(ioContext.waitScope);
  }

  kj::MainFunc getMain()
  {
    return kj::MainBuilder(context, "SQLite Storage Service v0.1", "Offers a SQLite backed storage service.")
      .addOptionWithArg({'n', "name"}, KJ_BIND_METHOD(*this, setName),
                        "<storage-service-name (default: SQLite Storage Service)>", "Name of service.")
      .addOptionWithArg({"description"}, KJ_BIND_METHOD(*this, setDescription),
                        "<storage-service-description (default: "")>", "Description of service.")
      .addOptionWithArg({'f', "filename"}, KJ_BIND_METHOD(*this, setPathToDB),
                        "<sqlite-db-filename (default: storage_service.sqlite)>", "Path to storage service' sqlite db.")
      .addOptionWithArg({'h', "host"}, KJ_BIND_METHOD(*this, setHost),
                        "<host-IP>", "Set host IP.")
      .addOptionWithArg({'p', "port"}, KJ_BIND_METHOD(*this, setPort),
                        "<port>", "Set port.")
      .addOptionWithArg({'s', "storage_container_sr"}, KJ_BIND_METHOD(*this, setRestorerContainerSR),
                        "<sturdy_ref (default: create new local container)>", "Sturdy ref to container for this restorer.")
      .addOptionWithArg({'r', "registrar_sr"}, KJ_BIND_METHOD(*this, setRegistrarSR),
                        "<sturdy_ref>", "Sturdy ref to registrar.")
      .addOptionWithArg({"reg_name"}, KJ_BIND_METHOD(*this, setRegName),
                        "<register name (default: --name)>", "Name to register service under.")
      .addOptionWithArg({"reg_category"}, KJ_BIND_METHOD(*this, setRegCategory),
                        "<category (default: monica)>", "Name of the category to register at.")
      .addOptionWithArg({"local_host (default: localhost)"}, KJ_BIND_METHOD(*this, setLocalHost),
                        "<IP_or_host_address>", "Use this host for sturdy reference creation.")
      .addOptionWithArg({"check_IP"}, KJ_BIND_METHOD(*this, setCheckIP),
                        "<IPv4 (default: 8.8.8.8)>", "IP to connect to in order to find local outside IP.")
      .addOptionWithArg({"check_port"}, KJ_BIND_METHOD(*this, setCheckPort),
                        "<port (default: 53)>", "Port to connect to in order to find local outside IP.")
      .callAfterParsing(KJ_BIND_METHOD(*this, startService))
      .build();
  }

private:
  kj::Own<mas::infrastructure::common::Restorer> restorer;
  mas::infrastructure::common::ConnectionManager conMan;
  kj::ProcessContext &context;
  kj::AsyncIoContext ioContext;
  kj::String name{kj::str("SQLite Storage Service")};
  kj::String description;
  kj::String host{kj::str("*")};
  kj::String localHost{kj::str("localhost")};
  int port{0};
  kj::String checkIP;
  int checkPort{0};
  kj::String pathToDB{kj::str("storage_service.sqlite")};
  kj::String restorerContainerSR;
  kj::String registrarSR;
  kj::String regName;
  kj::String regCategory{kj::str("storage")};
};

} // namespace storage
} // namespace infrastructure
} // namespace mas

KJ_MAIN(mas::infrastructure::storage::StorageServiceMain)
