/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg@zalf.de>

Maintainers:
Currently maintained by the authors.

Copyright (C) Leibniz Centre for Agricultural Landscape Research (ZALF)
*/

#include "storage_service.h"

#include <kj/debug.h>
#include <kj/thread.h>
#include <kj/common.h>
#include <kj/async.h>
#include <kj/exception.h>
#define KJ_MVCAP(var) var = kj::mv(var)

#include <capnp/capability.h>
#include <capnp/ez-rpc.h>
#include <capnp/message.h>
#include <capnp/schema.h>
#include <capnp/dynamic.h>
#include <capnp/list.h>
#include <capnp/rpc-twoparty.h>
#include <capnp/any.h>

#include "common/sole.hpp"

using namespace std;
using namespace mas::infrastructure::storage;

//-----------------------------------------------------------------------------

struct Store::Impl
{
  mas::infrastructure::common::Restorer* _restorer{nullptr};
  kj::String id;
  kj::String name{kj::str("Storage service")};
  kj::String description;

  Impl(mas::infrastructure::common::Restorer* restorer, kj::StringPtr name)
    : _restorer(restorer)
    , name(kj::str(name))
  {
    KJ_REQUIRE(restorer, "restorer must not be null");
  }

  ~Impl() noexcept(false)  {}
};

Store::Store(mas::infrastructure::common::Restorer* restorer, kj::StringPtr name)
: impl(kj::heap<Impl>(restorer, name))
{
}

kj::Promise<void> Store::info(InfoContext context) 
{
  KJ_LOG(INFO, "info message received");
  auto rs = context.getResults();
  rs.setId(impl->id);
  rs.setName(impl->name);
  rs.setDescription(impl->description);
  return kj::READY_NOW;
}


kj::Promise<void> Store::save(SaveContext context) 
{
  KJ_LOG(INFO, "save message received");
  // if(_restorer)
  // {
  //   //_restorer->save(_client, context.getResults().initSturdyRef(), context.getResults().initUnsaveSR());
  // }
  return kj::READY_NOW;
}


kj::Promise<void> Store::newContainer(NewContainerContext context)
{
  KJ_LOG(INFO, "newContainer message received");
  // auto r = kj::heap<Reader>(*this);
  // auto id = r->id();
  // AnyPointerChannel::ChanReader::Client rc = kj::mv(r);
  // _readers.insert(kj::str(id), rc);
  // context.getResults().setR(rc);
  return kj::READY_NOW;
}

kj::Promise<void> Store::containerWithId(ContainerWithIdContext context)
{
  KJ_LOG(INFO, "containerWithId message received");
  // auto w = kj::heap<Writer>(*this);
  // auto id = w->id();
  // AnyPointerChannel::ChanWriter::Client wc = kj::mv(w);
  // _writers.insert(kj::str(id), wc);
  // context.getResults().setW(wc);
  return kj::READY_NOW;
}

kj::Promise<void> Store::listContainers(ListContainersContext context)
{
  KJ_LOG(INFO, "listContainers message received");
  // auto w = kj::heap<Writer>(*this);
  // auto id = w->id();
  // AnyPointerChannel::ChanWriter::Client wc = kj::mv(w);
  // _writers.insert(kj::str(id), wc);
  // context.getResults().setW(wc);
  return kj::READY_NOW;
}

kj::Promise<void> Store::removeContainer(RemoveContainerContext context)
{
  KJ_LOG(INFO, "removeContainer message received");
  // auto w = kj::heap<Writer>(*this);
  // auto id = w->id();
  // AnyPointerChannel::ChanWriter::Client wc = kj::mv(w);
  // _writers.insert(kj::str(id), wc);
  // context.getResults().setW(wc);
  return kj::READY_NOW;
}


//-----------------------------------------------------------------------------

struct Container::Impl
{
  Store& store;
  kj::String id;
  kj::String name{kj::str("Container")};
  kj::String description;

  Impl(Store& store, kj::StringPtr name)
    : store(store)
    , id(kj::str(sole::uuid4().str().c_str()))
    , name(kj::str(name))
  {
  }

  ~Impl() noexcept(false)  {}
};

Container::Container(Store& store, kj::StringPtr name)
: impl(kj::heap<Impl>(store, name))
{
}

kj::Promise<void> Container::info(InfoContext context) //override
{
  KJ_LOG(INFO, "info message received");
  auto rs = context.getResults();
  rs.setId(impl->id);
  rs.setName(impl->name);
  rs.setDescription(impl->description);
  return kj::READY_NOW;
}


kj::Promise<void> Container::save(SaveContext context) 
{
  KJ_LOG(INFO, "save message received");
  //_store->save(_client, context.getResults().initSturdyRef(), context.getResults().initUnsaveSR());
  return kj::READY_NOW;
}

kj::Promise<void> Container::importData(ImportDataContext context) 
{
  KJ_LOG(INFO, "importData message received");

  return kj::READY_NOW;
}


kj::Promise<void> Container::exportData(ExportDataContext context) 
{
  KJ_LOG(INFO, "exportData message received");
  
  return kj::READY_NOW;
}


kj::Promise<void> Container::listObjects(ListObjectsContext context) 
{
  KJ_LOG(INFO, "listObjects message received");
  
  return kj::READY_NOW;
}


kj::Promise<void> Container::getObject(GetObjectContext context) 
{
  KJ_LOG(INFO, "getObject message received");
  
  return kj::READY_NOW;
}


kj::Promise<void> Container::addObject(AddObjectContext context) 
{
  KJ_LOG(INFO, "addObject message received");
  
  return kj::READY_NOW;
}


kj::Promise<void> Container::removeObject(RemoveObjectContext context) 
{
  KJ_LOG(INFO, "removeObject message received");
  
  return kj::READY_NOW;
}


kj::Promise<void> Container::clear(ClearContext context) 
{
  KJ_LOG(INFO, "clear message received");
  
  return kj::READY_NOW;
}
