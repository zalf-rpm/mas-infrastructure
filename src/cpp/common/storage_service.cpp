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
#include <kj/map.h>
#include <kj/memory.h>
#include <kj/string.h>
#define KJ_MVCAP(var) var = kj::mv(var)

#include <capnp/any.h>
#include <capnp/capability.h>
#include <capnp/dynamic.h>
#include <capnp/ez-rpc.h>
#include <capnp/list.h>
#include <capnp/message.h>
#include <capnp/rpc-twoparty.h>
#include <capnp/schema.h>
#include <capnp/serialize.h>

#include "common.h"
#include "common/sole.hpp"
#include "sqlite/sqlite3.h"

//using namespace std;
using namespace mas::infrastructure::storage;

//-----------------------------------------------------------------------------

typedef mas::schema::storage::Store::Container::Client ContainerClient;

struct SqliteStorageService::Impl {
  SqliteStorageService& self;
  mas::infrastructure::common::Restorer* restorer{nullptr};
  kj::String id;
  kj::String name{kj::str("Storage service")};
  kj::String description;
  kj::String filename;
  sqlite3* db{nullptr};
  bool isConnected{false};
  kj::HashMap<kj::String, kj::Tuple<ContainerClient, Container*>> containers;

  Impl(SqliteStorageService& self, mas::infrastructure::common::Restorer* restorer, kj::StringPtr filename, kj::StringPtr name)
    : self(self)
    , restorer(restorer)
    , name(kj::str(name))
    , filename(kj::str(filename)) {
    KJ_REQUIRE(restorer, "restorer must not be null");
    init();
  }

  ~Impl() noexcept(false)  {}

  void init() {
    kj::String utf8Filename = kj::str(filename); // linux default codepage is utf-8 
#ifdef WIN32
    utf8filename = kj::str(Tools::winStringSystemCodepageToutf8(filename.C_str()));
#endif
    int rc = sqlite3_open(utf8Filename.cStr(), &db);
    isConnected = rc == SQLITE_OK;
    if(rc) {
      KJ_LOG(ERROR, "Can't open sqlite database.", filename, "Error:", sqlite3_errmsg(db));
      sqlite3_close(db);
      exit(1);
    }

    //addNeededSQLFunctions();
  }

  ContainerClient createContainer(kj::StringPtr name, kj::StringPtr description) {
    auto newId = kj::str(sole::uuid4().str().c_str());
    auto createTableStmt = kj::str("CREATE TABLE IF NOT EXISTS '", newId, "' ("
      "'key' TEXT NOT NULL PRIMARY KEY,"
      "'value' TEXT NOT NULL"
      ");");
    char* errMsg;
    // create table
    auto rc = sqlite3_exec(db, createTableStmt.cStr(), NULL, NULL, &errMsg);
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't create table.", createTableStmt, "Error:", errMsg);
      sqlite3_free(errMsg);
    }

    // insert container into containers table
    auto insertStmt = kj::str("INSERT INTO containers (id, name, description) VALUES "
      "('", newId, "', '", name, "', '", description, "');");
    rc = sqlite3_exec(db, insertStmt.cStr(), NULL, NULL, &errMsg);
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't insert container.", insertStmt, "Error:", errMsg);
      sqlite3_free(errMsg);
    }

    auto cont = kj::heap<Container>(self, newId.asPtr(), name, description);
    auto ptr = cont.get();
    ContainerClient client = kj::mv(cont);
    auto& entry = containers.insert(kj::mv(newId), kj::tuple(kj::mv(client), ptr));
    return kj::get<0>(entry.value);
  }


  bool removeContainer(kj::StringPtr id) {
    KJ_IF_MAYBE(entry, containers.find(id)) {
      auto deleteStmt = kj::str("DELETE FROM containers WHERE id = '", id, "';");
      char* errMsg;
      auto rc = sqlite3_exec(db, deleteStmt.cStr(), NULL, NULL, &errMsg);
      if(rc != SQLITE_OK) {
        KJ_LOG(ERROR, "Can't delete container.", deleteStmt, "Error:", errMsg);
        sqlite3_free(errMsg);
      }

      auto dropTableStmt = kj::str("DROP TABLE IF EXISTS '", id, "';");
      rc = sqlite3_exec(db, dropTableStmt.cStr(), NULL, NULL, &errMsg);
      if(rc != SQLITE_OK) {
        KJ_LOG(ERROR, "Can't drop table.", dropTableStmt, "Error:", errMsg);
        sqlite3_free(errMsg);
      }

      containers.erase(id);
      return true;
    }
    else return false;
  }


};

SqliteStorageService::SqliteStorageService(mas::infrastructure::common::Restorer* restorer, kj::StringPtr filename, 
  kj::StringPtr name)
: impl(kj::heap<Impl>(*this, restorer, filename, name)) {
}

SqliteStorageService::~SqliteStorageService() {}
  
kj::Promise<void> SqliteStorageService::info(InfoContext context) {
  KJ_LOG(INFO, "info message received");
  auto rs = context.getResults();
  rs.setId(impl->id);
  rs.setName(impl->name);
  rs.setDescription(impl->description);
  return kj::READY_NOW;
}


kj::Promise<void> SqliteStorageService::save(SaveContext context) {
  KJ_LOG(INFO, "save message received");
  // if(_restorer)
  // {
  //   //_restorer->save(_client, context.getResults().initSturdyRef(), context.getResults().initUnsaveSR());
  // }
  return kj::READY_NOW;
}


kj::Promise<void> SqliteStorageService::newContainer(NewContainerContext context) {
  KJ_LOG(INFO, "newContainer message received");
  auto info = context.getParams();
  context.getResults().setContainer(impl->createContainer(info.getName(), info.getDescription()));
  return kj::READY_NOW;
}

kj::Promise<void> SqliteStorageService::containerWithId(ContainerWithIdContext context) {
  KJ_LOG(INFO, "containerWithId message received");
  auto id = context.getParams().getId();
  KJ_IF_MAYBE(entry, impl->containers.find(id)) {
    context.getResults().setContainer(kj::get<0>(*entry));
  }
  return kj::READY_NOW;
}

kj::Promise<void> SqliteStorageService::listContainers(ListContainersContext context) {
  KJ_LOG(INFO, "listContainers message received");
  auto rs = context.getResults();
  auto list = rs.initContainers(impl->containers.size());
  size_t i = 0;
  for(auto& entry : impl->containers) {
    list.set(i++, kj::get<0>(entry.value));
  }
  return kj::READY_NOW;
}

kj::Promise<void> SqliteStorageService::removeContainer(RemoveContainerContext context) {
  KJ_LOG(INFO, "removeContainer message received");
  auto id = context.getParams().getId();
  context.getResults().setSuccess(impl->removeContainer(id));
  return kj::READY_NOW;
}


//-----------------------------------------------------------------------------

struct Container::Impl {
  SqliteStorageService& store;
  kj::String id;
  kj::String name{kj::str("Container")};
  kj::String description;

  Impl(SqliteStorageService& store, kj::StringPtr id, kj::StringPtr name, kj::StringPtr description)
    : store(store)
    , id(kj::str(id))
    , name(kj::str(name))
    , description(kj::str(description)) {
  }

  ~Impl() noexcept(false)  {}

  void addObject(mas::schema::storage::Store::Container::Object::Reader obj) {
    auto insertStmt = kj::str("INSERT INTO '", id, "' (key, value) VALUES ('", obj.getKey(), "', ?);");
    sqlite3_stmt* stmt = nullptr;
    auto rc = sqlite3_prepare_v2(store.impl->db, insertStmt.cStr(), -1, &stmt, NULL);
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't prepare statement.", insertStmt);
    } else {
      auto canonical = capnp::canonicalize(obj.getValue());
      auto canBytes = canonical.asBytes();
      sqlite3_bind_blob(stmt, 1, canBytes.begin(), canBytes.size(), SQLITE_STATIC);
      rc = sqlite3_step(stmt);
      if(rc != SQLITE_DONE) {
        KJ_LOG(ERROR, "Can't insert object.", insertStmt);
      }
      sqlite3_finalize(stmt);
    }
  }

  void getObject(kj::StringPtr key, mas::schema::storage::Store::Container::Object::Builder obj) {
    auto selectStmt = kj::str("SELECT value FROM '", id, "' WHERE key = '", key, "';");
    sqlite3_stmt* stmt = nullptr;
    auto rc = sqlite3_prepare_v2(store.impl->db, selectStmt.cStr(), -1, &stmt, NULL);
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't prepare statement.", selectStmt);
    } else {
      rc = sqlite3_step(stmt);
      if(rc == SQLITE_ROW) {
        auto bytes = kj::arrayPtr(reinterpret_cast<const capnp::word*>(sqlite3_column_blob(stmt, 0)), sqlite3_column_bytes(stmt, 0));
        auto msg = capnp::FlatArrayMessageReader(bytes);
        auto value = msg.getRoot<mas::schema::storage::Store::Container::Object::Value>();
        obj.setKey(key);
        auto val = obj.initValue();
        if(value.isBoolValue()) val.setBoolValue(value.getBoolValue());
        if(value.isIntValue()) val.setIntValue(value.getIntValue());
        if(value.isFloatValue()) val.setFloatValue(value.getFloatValue());
        if(value.isTextValue()) val.setTextValue(value.getTextValue());
        if(value.isDataValue()) val.setDataValue(value.getDataValue());
        if(value.isAnyValue()) val.setAnyValue(value.getAnyValue());
      } else {
        KJ_LOG(ERROR, "Can't get object.", selectStmt);
      }
      sqlite3_finalize(stmt);
    }
  }


};

Container::Container(SqliteStorageService& store, kj::StringPtr id, kj::StringPtr name, kj::StringPtr description)
: impl(kj::heap<Impl>(store, id, name, description)) {
}

Container::~Container() {}

kj::Promise<void> Container::info(InfoContext context) {
  KJ_LOG(INFO, "info message received");
  auto rs = context.getResults();
  rs.setId(impl->id);
  rs.setName(impl->name);
  rs.setDescription(impl->description);
  return kj::READY_NOW;
}


kj::Promise<void> Container::save(SaveContext context) {
  KJ_LOG(INFO, "save message received");
  //_store->save(_client, context.getResults().initSturdyRef(), context.getResults().initUnsaveSR());
  return kj::READY_NOW;
}

kj::Promise<void> Container::importData(ImportDataContext context) {
  KJ_LOG(INFO, "importData message received");

  return kj::READY_NOW;
}


kj::Promise<void> Container::exportData(ExportDataContext context) {
  KJ_LOG(INFO, "exportData message received");
  
  return kj::READY_NOW;
}


kj::Promise<void> Container::listObjects(ListObjectsContext context) {
  KJ_LOG(INFO, "listObjects message received");
  
  return kj::READY_NOW;
}


kj::Promise<void> Container::getObject(GetObjectContext context) {
  KJ_LOG(INFO, "getObject message received");
  
  return kj::READY_NOW;
}


kj::Promise<void> Container::addObject(AddObjectContext context) {
  KJ_LOG(INFO, "addObject message received");

  return kj::READY_NOW;
}


kj::Promise<void> Container::removeObject(RemoveObjectContext context) {
  KJ_LOG(INFO, "removeObject message received");
  
  return kj::READY_NOW;
}


kj::Promise<void> Container::clear(ClearContext context) {
  KJ_LOG(INFO, "clear message received");
  
  return kj::READY_NOW;
}
