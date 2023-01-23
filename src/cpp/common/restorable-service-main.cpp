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

#include "restorable-service-main.h"

#include <iostream>

#include <kj/debug.h>

#include "../tools/algorithms.h"

#include "common.h"

#include "common.capnp.h"
#include "storage.capnp.h"
#include "registry.capnp.h"

using namespace mas::infrastructure::common;

RestorableServiceMain::RestorableServiceMain(kj::ProcessContext &context, kj::StringPtr serviceVersion, 
  kj::StringPtr serviceBriefDescription, kj::StringPtr serviceExtendedDescription) 
: mainBuilder(context, serviceVersion, serviceBriefDescription, serviceExtendedDescription)
, context(context)
, ioContext(kj::setupAsyncIo()) 
{}

kj::MainBuilder::Validity RestorableServiceMain::setName(kj::StringPtr n) { name = kj::str(n); return true; }

kj::MainBuilder::Validity RestorableServiceMain::setDescription(kj::StringPtr d) { description = kj::str(d); return true; }

kj::MainBuilder::Validity RestorableServiceMain::setHost(kj::StringPtr name) { host = kj::str(name); return true; }

kj::MainBuilder::Validity RestorableServiceMain::setLocalHost(kj::StringPtr h) { localHost = kj::str(h); return true; }

kj::MainBuilder::Validity RestorableServiceMain::setPort(kj::StringPtr name) { port = kj::max(0, name.parseAs<int>()); return true; }

kj::MainBuilder::Validity RestorableServiceMain::setCheckPort(kj::StringPtr portStr) { checkPort = portStr.parseAs<int>(); return true; }

kj::MainBuilder::Validity RestorableServiceMain::setCheckIP(kj::StringPtr ip) { checkIP = kj::str(ip); return true; }

kj::MainBuilder::Validity RestorableServiceMain::setRestorerContainerSR(kj::StringPtr sr) { restorerContainerSR = kj::str(sr); return true; }

kj::MainBuilder::Validity RestorableServiceMain::setServiceContainerSR(kj::StringPtr sr) { serviceContainerSR = kj::str(sr); return true; }

kj::MainBuilder::Validity RestorableServiceMain::setRegistrarSR(kj::StringPtr sr) { registrarSR = kj::str(sr); return true; }

kj::MainBuilder::Validity RestorableServiceMain::setRegName(kj::StringPtr n) { regName = kj::str(n); return true; }

kj::MainBuilder::Validity RestorableServiceMain::setRegCategory(kj::StringPtr cat) { regCategory = kj::str(cat); return true; }

kj::MainBuilder::Validity RestorableServiceMain::setOutputSturdyRefs() { outputSturdyRefs = true; return true; }

kj::MainBuilder::Validity RestorableServiceMain::setOutputSRWriterSR(kj::StringPtr idAndSR) { 
  auto v = splitString(idAndSR, "|");
  if(v.size() == 2) {
    outputSRWriterId = kj::mv(v[0]);
    outputSRWriterSR = kj::mv(v[1]);
  } else if(v.size() == 1) outputSRWriterSR = kj::mv(v[0]);
  return true; 
}

kj::MainBuilder::Validity RestorableServiceMain::setInitServiceFromContainer(kj::StringPtr init) { 
  initRestorerFromContainer = init == "true"; 
  initServiceFromContainer = init == "true";
  return true; 
}

void RestorableServiceMain::startRestorerSetup(mas::schema::common::Identifiable::Client serviceClient)
{
  KJ_LOG(INFO, "Starting restorer setup.");
  
  auto ownedRestorer = kj::heap<mas::infrastructure::common::Restorer>();
  restorer = ownedRestorer.get();
  conMan = kj::heap<mas::infrastructure::common::ConnectionManager>(restorer);
  restorerClient = kj::mv(ownedRestorer);
  KJ_ASSERT(restorer != nullptr);
  KJ_LOG(INFO, "Created restorer.");
  
  if (outputSRWriterSR.size() > 0) {
    outputSRWriterClient = conMan->tryConnectB(ioContext, outputSRWriterSR).castAs<TextPairChannel::ChanWriter>();
  }

  // if a restorer container stury ref is given, try to connect to it
  if (restorerContainerClient == nullptr && restorerContainerSR.size() > 0) {
    restorerContainerClient = conMan->tryConnectB(ioContext, restorerContainerSR).castAs<mas::schema::storage::Store::Container>();
  }

  // if a service container sturdy ref is given, try to connect to it
  if (serviceContainerClient == nullptr && serviceContainerSR.size() > 0) {
    serviceContainerClient = conMan->tryConnectB(ioContext, serviceContainerSR).castAs<mas::schema::storage::Store::Container>();
  }

  // set the restorers storage container, which also will try to load a previously stored port if initRestorerFromContainer is true
  KJ_IF_MAYBE(rcc, restorerContainerClient){ 
    restorer->setStorageContainer(*rcc);
    if(initRestorerFromContainer){
      if(port == 0) { 
        restorer->initPortFromContainer().wait(ioContext.waitScope);
        port = restorer->getPort();
      }
      restorer->initVatIdFromContainer().wait(ioContext.waitScope);
    }  
  } 

  // bind restorer 
  KJ_LOG(INFO, "Trying to bind restorer to", host, port);
  auto portPromise = conMan->bind(ioContext, restorerClient, host, port);
  auto succAndIP = infrastructure::common::getLocalIP(checkIP, checkPort);
  if(kj::get<0>(succAndIP)){
    restorer->setHost(kj::get<1>(succAndIP));
    conMan->setLocallyUsedHost(kj::get<1>(succAndIP));
  } else {
    restorer->setHost(localHost);
    conMan->setLocallyUsedHost(localHost);
  }
  auto port = portPromise.then([this](auto port){ 
    return restorer->setPort(port).then([port](){ 
        return port; 
      }, [](auto&& e){
        KJ_LOG(ERROR, "Error while trying to set port.", e);
        return 0;
      }
    );
  }).wait(ioContext.waitScope); 
  KJ_LOG(INFO, "Bound restorer to", host, port);
  
  // print the restorers sturdy ref
  auto restorerSR = restorer->sturdyRefStr("");
  if(outputSturdyRefs && restorerSR.size() > 0) std::cout << "restorerSR=" << restorerSR.cStr() << std::endl;
  KJ_IF_MAYBE(out, outputSRWriterClient){
     auto req = out->writeRequest();
     auto val = req.initValue();
     //val.setFst("")
     //req.send().wait(ioContext.waitScope);
  }

  //mas::schema::persistence::SturdyRef::Reader reregSR(nullptr);
  mas::schema::registry::Registrar::Client registrar(nullptr);
  if(registrarSR.size() > 0) {
    KJ_LOG(INFO, "Trying to register service at:", registrarSR);
    registrar = conMan->tryConnectB(ioContext, registrarSR).castAs<mas::schema::registry::Registrar>();
    auto request = registrar.registerRequest();
    request.setCap(serviceClient);
    request.setRegName(regName.size() == 0 ? name.asPtr() : regName.asPtr());
    request.setCategoryId(regCategory);
    auto xd = request.initXDomain();
    restorer->setVatId(xd.initVatId());
    xd.setRestorer(restorerClient);
    try {
      auto response = request.send().wait(ioContext.waitScope);
      if(response.hasUnreg()) serviceUnregisterAction = response.getUnreg();
      //if(response.hasReregSR()) reregSR = response.getReregSR();
      KJ_LOG(INFO, "Registered service at:", registrarSR);
    } catch(kj::Exception e) {
      KJ_LOG(ERROR, "Error sending register message to Registrar! Error", e.getDescription().cStr());
    }
  }
}

kj::MainBuilder& RestorableServiceMain::addRestorableServiceOptions()
{
  return mainBuilder
    .addOptionWithArg({'n', "name"}, KJ_BIND_METHOD(*this, setName),
                      "<storage-service-name (default: SQLite Storage Service)>", "Name of service.")
    .addOptionWithArg({"description"}, KJ_BIND_METHOD(*this, setDescription),
                      "<storage-service-description (default: "")>", "Description of service.")
    .addOptionWithArg({"init_from_storage"}, KJ_BIND_METHOD(*this, setInitServiceFromContainer),
                      "<true | false (default: true)>", "Initialize service from storage.")
    .addOptionWithArg({'h', "host"}, KJ_BIND_METHOD(*this, setHost),
                      "<host-IP>", "Set host IP.")
    .addOptionWithArg({'p', "port"}, KJ_BIND_METHOD(*this, setPort),
                      "<port>", "Set port.")
    .addOptionWithArg({"restorer_container_sr"}, KJ_BIND_METHOD(*this, setRestorerContainerSR),
                      "<sturdy_ref>", "Sturdy ref to container for this restorer.")
    .addOptionWithArg({"service_container_sr"}, KJ_BIND_METHOD(*this, setServiceContainerSR),
                      "<sturdy_ref>", "Sturdy ref to container for this service.")
    .addOptionWithArg({"registrar_sr"}, KJ_BIND_METHOD(*this, setRegistrarSR),
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
    .addOption({"output_srs"}, KJ_BIND_METHOD(*this, setOutputSturdyRefs),
                "Output the sturdy refs to the restorer and service to stdout.")
    .addOptionWithArg({"output_srs_writer_sr"}, KJ_BIND_METHOD(*this, setOutputSRWriterSR),
                      "<ID>|<sturdy_ref>", "ID to identify and sturdy ref to an output channel writer capability.");
}
