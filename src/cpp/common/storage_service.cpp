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

#include <chrono>
#include <thread>

#include <kj/debug.h>
#include <kj/thread.h>
#include <kj/common.h>
#include <kj/async.h>
#include <kj/exception.h>
#include <kj/map.h>
#include <kj/memory.h>
#include <kj/string.h>
#include <kj/vector.h>
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
    auto createTableStmt = kj::str(
      "CREATE TABLE IF NOT EXISTS '", newId, "' ("
      "'key' TEXT NOT NULL PRIMARY KEY,"
      "'value' TEXT NOT NULL"
      ")");
    char* errMsg;
    // create table
    auto rc = sqlite3_exec(db, createTableStmt.cStr(), NULL, NULL, &errMsg);
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't create table.", createTableStmt, "Error:", errMsg);
      sqlite3_free(errMsg);
    }

    // insert container into containers table
    auto insertStmt = kj::str("INSERT INTO containers (id, name, description) VALUES "
      "('", newId, "', '", name, "', '", description, "')");
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

  ContainerClient loadContainer(kj::StringPtr id) {
    auto selectStmt = kj::str("SELECT id, name, description FROM 'containers' WHERE id = '", id, "';");
    sqlite3_stmt* stmt = nullptr;
    auto rc = sqlite3_prepare_v2(db, selectStmt.cStr(), -1, &stmt, NULL);
    ContainerClient result(nullptr);
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't prepare statement.", selectStmt);
    } else {
      rc = sqlite3_step(stmt);
      if(rc == SQLITE_ROW) {
        auto id = reinterpret_cast<const char*>(sqlite3_column_text(stmt, 0));
        auto name = reinterpret_cast<const char*>(sqlite3_column_text(stmt, 1));
        auto description = reinterpret_cast<const char*>(sqlite3_column_text(stmt, 2));
        auto cont = kj::heap<Container>(self, id, name, description);
        auto ptr = cont.get();
        ContainerClient client = kj::mv(cont);
        auto& entry = containers.insert(kj::str(id), kj::tuple(kj::mv(client), ptr));
        result = kj::get<0>(entry.value);
      } else {
        KJ_LOG(ERROR, "Can't get object.", selectStmt);
      }
      sqlite3_finalize(stmt);
    }
    return result;
  }

  kj::Vector<ContainerClient> listContainers() {
    auto selectStmt = kj::str("SELECT id, name, description FROM 'containers'");
    sqlite3_stmt* stmt = nullptr;
    auto rc = sqlite3_prepare_v2(db, selectStmt.cStr(), -1, &stmt, NULL);
    kj::Vector<ContainerClient> result;
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't prepare statement.", selectStmt);
    } else {
      while(sqlite3_step(stmt) == SQLITE_ROW) {
        auto id = reinterpret_cast<const char*>(sqlite3_column_text(stmt, 0));
        auto name = reinterpret_cast<const char*>(sqlite3_column_text(stmt, 1));
        auto description = reinterpret_cast<const char*>(sqlite3_column_text(stmt, 2));
        KJ_IF_MAYBE(entry, containers.find(id)){
          result.add(kj::get<0>(*entry));    
        } else {
          auto cont = kj::heap<Container>(self, id, name, description);
          auto ptr = cont.get();
          ContainerClient client = kj::mv(cont);
          auto& e = containers.insert(kj::str(id), kj::tuple(kj::mv(client), ptr));
          result.add(kj::get<0>(e.value));
        }
      }
      sqlite3_finalize(stmt);
    }
    return kj::mv(result);
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

  bool clearContainer(kj::StringPtr id) {
    auto deleteStmt = kj::str("DELETE * FROM '", id, "'");
    char* errMsg;
    auto rc = sqlite3_exec(db, deleteStmt.cStr(), NULL, NULL, &errMsg);
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't delete all data from container.", deleteStmt, "Error:", errMsg);
      sqlite3_free(errMsg);
      return false;
    }
    return true;
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
  } else {
    context.getResults().setContainer(impl->loadContainer(id));
  }
  return kj::READY_NOW;
}

kj::Promise<void> SqliteStorageService::listContainers(ListContainersContext context) {
  KJ_LOG(INFO, "listContainers message received");
  auto rs = context.getResults();
  auto containers = impl->listContainers();
  auto list = rs.initContainers(containers.size());
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

  void setObjectValue(
    mas::schema::storage::Store::Container::Object::Value::Builder b, 
    mas::schema::storage::Store::Container::Object::Value::Reader r){
    switch(r.which()){
      case mas::schema::storage::Store::Container::Object::Value::BOOL_VALUE:
        b.setBoolValue(r.getBoolValue());
        break;
      case mas::schema::storage::Store::Container::Object::Value::INT_VALUE:
        b.setIntValue(r.getIntValue());
        break;
      case mas::schema::storage::Store::Container::Object::Value::FLOAT_VALUE:
        b.setFloatValue(r.getFloatValue());
        break;
      case mas::schema::storage::Store::Container::Object::Value::TEXT_VALUE:
        b.setTextValue(r.getTextValue());
        break;
      case mas::schema::storage::Store::Container::Object::Value::DATA_VALUE:
        b.setDataValue(r.getDataValue());
        break;
      case mas::schema::storage::Store::Container::Object::Value::ANY_VALUE:
        b.setAnyValue(r.getAnyValue());
        break;
    }
  }

  bool addObject(mas::schema::storage::Store::Container::Object::Reader obj) {
    auto insertStmt = kj::str("INSERT INTO '", id, "' (key, value) VALUES ('", obj.getKey(), "', ?);");
    sqlite3_stmt* stmt = nullptr;
    auto rc = sqlite3_prepare_v2(store.impl->db, insertStmt.cStr(), -1, &stmt, NULL);
    bool success = false;
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't prepare statement.", insertStmt);
    } else {
      capnp::MallocMessageBuilder message;
      auto builder = message.initRoot<mas::schema::storage::Store::Container::Object::Value>();
      setObjectValue(builder, obj.getValue());
      auto flatArray = capnp::messageToFlatArray(message);
	    auto bytes = flatArray.asBytes();
      sqlite3_bind_blob(stmt, 1, bytes.begin(), bytes.size(), SQLITE_STATIC);
      rc = sqlite3_step(stmt);
      success = rc == SQLITE_DONE;
      if(!success) {
        KJ_LOG(ERROR, "Can't insert object.", insertStmt);
      }
      sqlite3_finalize(stmt);
    }
    return success;
  }

  void setObjectFromRow(mas::schema::storage::Store::Container::Object::Builder b, sqlite3_stmt* stmt) {
    auto key = reinterpret_cast<const char*>(sqlite3_column_blob(stmt, 0));
    auto val = reinterpret_cast<const char*>(sqlite3_column_blob(stmt, 1));
    auto size = sqlite3_column_bytes(stmt, 1);
    capnp::FlatArrayMessageReader reader(kj::ArrayPtr<const capnp::word>(reinterpret_cast<const capnp::word*>(val), size/sizeof(capnp::word)));
    auto value = reader.getRoot<mas::schema::storage::Store::Container::Object::Value>();
    b.setKey(key);
    setObjectValue(b.initValue(), value);
  }

  void getObject(kj::StringPtr key, mas::schema::storage::Store::Container::Object::Builder objBuilder) {
    auto selectStmt = kj::str("SELECT key, value FROM '", id, "' WHERE key = '", key, "';");
    sqlite3_stmt* stmt = nullptr;
    auto rc = sqlite3_prepare_v2(store.impl->db, selectStmt.cStr(), -1, &stmt, NULL);
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't prepare statement.", selectStmt);
    } else {
      rc = sqlite3_step(stmt);
      if(rc == SQLITE_ROW) {
        setObjectFromRow(objBuilder, stmt);
      } else {
        KJ_LOG(ERROR, "Can't get object.", selectStmt);
      }
      sqlite3_finalize(stmt);
    }
  }

  void listObjects(capnp::List<mas::schema::storage::Store::Container::Object, capnp::Kind::STRUCT>::Builder list) {
    auto selectStmt = kj::str("SELECT key, value FROM '", id, "' ORDER BY key");
    sqlite3_stmt* stmt = nullptr;
    auto rc = sqlite3_prepare_v2(store.impl->db, selectStmt.cStr(), -1, &stmt, NULL);
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't prepare statement.", selectStmt);
    } else {
      int i = 0;
      while(sqlite3_step(stmt) == SQLITE_ROW && i < list.size()) {
        setObjectFromRow(list[i++], stmt);
      } 
      sqlite3_finalize(stmt);
    }
  }

  bool removeObject(kj::StringPtr key) {
    auto deleteStmt = kj::str("DELETE FROM '", id, "' WHERE key = '", key, "'");
    char* errMsg;
    auto rc = sqlite3_exec(store.impl->db, deleteStmt.cStr(), NULL, NULL, &errMsg);
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't delete object.", deleteStmt, "Error:", errMsg);
      sqlite3_free(errMsg);
      return false;
    }
    return true;
  }

  size_t countObjects() {
    auto selectStmt = kj::str("SELECT count(key) FROM '", id, "'");
    sqlite3_stmt* stmt = nullptr;
    auto rc = sqlite3_prepare_v2(store.impl->db, selectStmt.cStr(), -1, &stmt, NULL);
    size_t count = 0;
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't prepare statement.", selectStmt);
    } else {
      rc = sqlite3_step(stmt);
      if(rc == SQLITE_ROW) {
        count = (size_t)sqlite3_column_int64(stmt, 0);
      } else {
        KJ_LOG(ERROR, "Can't get object.", selectStmt);
      }
      sqlite3_finalize(stmt);
    }
    return count;
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
  auto count = impl->countObjects();
  impl->listObjects(context.getResults().initObjects(count));
  return kj::READY_NOW;
}


kj::Promise<void> Container::getObject(GetObjectContext context) {
  KJ_LOG(INFO, "getObject message received");
  impl->getObject(context.getParams().getKey(), context.getResults().initObject());
  return kj::READY_NOW;
}


kj::Promise<void> Container::addObject(AddObjectContext context) {
  KJ_LOG(INFO, "addObject message received");
  context.getResults().setSuccess(impl->addObject(context.getParams().getObject()));
  return kj::READY_NOW;
}


kj::Promise<void> Container::removeObject(RemoveObjectContext context) {
  KJ_LOG(INFO, "removeObject message received");
  context.getResults().setSuccess(impl->removeObject(context.getParams().getKey()));
  return kj::READY_NOW;
}


kj::Promise<void> Container::clear(ClearContext context) {
  KJ_LOG(INFO, "clear message received");
  context.getResults().setSuccess(impl->store.impl->clearContainer(impl->id));
  return kj::READY_NOW;
}
