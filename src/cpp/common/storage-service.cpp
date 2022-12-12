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

#include "storage-service.h"

#include <chrono>
#include <thread>

#include <kj/async.h>
#include <kj/debug.h>
#include <kj/common.h>
#include <kj/encoding.h>
#include <kj/exception.h>
#include <kj/io.h>
#include <kj/map.h>
#include <kj/memory.h>
#include <kj/string.h>
#include <kj/thread.h>
#include <kj/vector.h>
#define KJ_MVCAP(var) var = kj::mv(var)

#include <capnp/any.h>
#include <capnp/capability.h>
#include <capnp/dynamic.h>
#include <capnp/ez-rpc.h>
#include "capnp/compat/json.h"
#include <capnp/list.h>
#include <capnp/message.h>
#include <capnp/rpc-twoparty.h>
#include <capnp/schema.h>
#include <capnp/serialize.h>
#include <capnp/serialize-packed.h>

#include "common.h"
#include "restorer.h"
#include "common/sole.hpp"
#include "sqlite/sqlite3.h"

//using namespace std;
using namespace mas::infrastructure::storage;

namespace _ { // private

  bool setEntryValue(
    mas::schema::storage::Store::Container::Entry::Value::Builder b, 
    mas::schema::storage::Store::Container::Entry::Value::Reader r,
    bool encodeAnyValueAsBase64Text = false,
    bool decodeAnyValueFromBase64Text = false) {
    bool wasAnyValueEncodedAsBase64Text = false;
    switch(r.which()){
      case mas::schema::storage::Store::Container::Entry::Value::BOOL_VALUE:
        b.setBoolValue(r.getBoolValue());
        break;
      case mas::schema::storage::Store::Container::Entry::Value::BOOL_LIST_VALUE: {
        auto lr = r.getBoolListValue();
        auto lb = b.initBoolListValue(lr.size());
        for(auto i : kj::indices(lr)) lb.set(i, lr[i]);
        break;
      }
      case mas::schema::storage::Store::Container::Entry::Value::INT_VALUE:
        b.setIntValue(r.getIntValue());
        break;
      case mas::schema::storage::Store::Container::Entry::Value::INT_LIST_VALUE: {
        auto lr = r.getIntListValue();
        auto lb = b.initIntListValue(lr.size());
        for(auto i : kj::indices(lr)) lb.set(i, lr[i]);
        break;
      }
      case mas::schema::storage::Store::Container::Entry::Value::FLOAT_VALUE:
        b.setFloatValue(r.getFloatValue());
        break;
      case mas::schema::storage::Store::Container::Entry::Value::FLOAT_LIST_VALUE: {
        auto lr = r.getFloatListValue();
        auto lb = b.initFloatListValue(lr.size());
        for(auto i : kj::indices(lr)) lb.set(i, lr[i]);
        break;
      }
      case mas::schema::storage::Store::Container::Entry::Value::TEXT_VALUE:
        if(decodeAnyValueFromBase64Text){
          auto bytes = kj::decodeBase64(r.getTextValue().asArray());
          kj::ArrayInputStream ais(bytes);
          capnp::PackedMessageReader ins(ais);
          auto value = ins.getRoot<mas::schema::storage::Store::Container::Entry::Value>();
          b.setAnyValue(value.getAnyValue());
        } else {
          b.setTextValue(r.getTextValue());
        }
        break;
      case mas::schema::storage::Store::Container::Entry::Value::TEXT_LIST_VALUE: {
        auto lr = r.getTextListValue();
        auto lb = b.initTextListValue(lr.size());
        for(auto i : kj::indices(lr)) lb.set(i, lr[i]);
        break;
      }
      case mas::schema::storage::Store::Container::Entry::Value::DATA_VALUE:
        b.setDataValue(r.getDataValue());
        break;
      case mas::schema::storage::Store::Container::Entry::Value::DATA_LIST_VALUE: {
        auto lr = r.getDataListValue();
        auto lb = b.initDataListValue(lr.size());
        for(auto i : kj::indices(lr)) lb.set(i, lr[i]);
        break;
      }
      case mas::schema::storage::Store::Container::Entry::Value::ANY_VALUE:
        if(encodeAnyValueAsBase64Text){
          capnp::MallocMessageBuilder message;
          auto builder = message.initRoot<mas::schema::storage::Store::Container::Entry::Value>();
          //setObjectValue(builder, r);
          builder.setAnyValue(r.getAnyValue());
          auto sizeInBytes = message.sizeInWords()*sizeof(capnp::word);
          kj::Array<kj::byte> bytes = kj::heapArray<kj::byte>(sizeInBytes);
          kj::ArrayOutputStream outs(bytes);
          capnp::writePackedMessage(outs, message);
          auto filledBytes = outs.getArray();
          auto base64 = kj::encodeBase64(filledBytes);
          b.setTextValue(base64);
          wasAnyValueEncodedAsBase64Text = true;
        } else {
          b.setAnyValue(r.getAnyValue());
        }
        break;
    }
    return wasAnyValueEncodedAsBase64Text;
  }

  kj::Tuple<kj::String, bool> 
  getKeyAndSetValueFromDBRow(mas::schema::storage::Store::Container::Entry::Value::Builder b, sqlite3_stmt* stmt,
    bool encodeAnyValueAsBase64Text = false) {
    auto key = reinterpret_cast<const char*>(sqlite3_column_blob(stmt, 0));
    auto val = reinterpret_cast<const char*>(sqlite3_column_blob(stmt, 1));
    auto size = sqlite3_column_bytes(stmt, 1);
    auto bytes = kj::arrayPtr(reinterpret_cast<const kj::byte*>(val), size);
    kj::ArrayInputStream ais(bytes);
    capnp::PackedMessageReader ins(ais);
    auto value = ins.getRoot<mas::schema::storage::Store::Container::Entry::Value>();
    return kj::tuple(kj::str(key), ::_::setEntryValue(b, value, encodeAnyValueAsBase64Text));
  }


} // namespace private

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
  mas::schema::storage::Store::Client client{nullptr};
  mas::schema::common::Action::Client unregisterAction{nullptr};

  Impl(SqliteStorageService& self, mas::infrastructure::common::Restorer* restorer, kj::StringPtr filename, 
    kj::StringPtr name, kj::StringPtr description)
  : self(self)
  , name(kj::str(name))
  , filename(kj::str(filename)) {
    initDb();
    setRestorer(restorer);
  }

  ~Impl() noexcept(false)  {}

  void initDb() {
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

  void setRestorer(mas::infrastructure::common::Restorer* restorer){ 
    if(restorer != nullptr){
      this->restorer = restorer; 
      restorer->setRestoreCallback([this](kj::StringPtr containerId) -> capnp::Capability::Client {
        if(containerId == nullptr) return client;
        else return loadContainer(containerId);
      });
    }
  }

  kj::Tuple<ContainerClient, Container*> createContainer(kj::StringPtr id, kj::StringPtr name, kj::StringPtr description) {
    auto cont = kj::heap<Container>(self, id, name, description);
    auto ptr = cont.get();
    ContainerClient client = kj::mv(cont);
    ptr->setClient(client);
    auto& entry = containers.insert(kj::str(id), kj::tuple(kj::mv(client), ptr));
    return kj::tuple(ContainerClient(kj::get<0>(entry.value)), ptr);
  }


  kj::Tuple<ContainerClient, Container*> 
  createContainerDb(kj::StringPtr name, kj::StringPtr description, kj::StringPtr id = nullptr) {
    auto newId = id == nullptr ? kj::str(sole::uuid4().str().c_str()) : kj::str(id);
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

    return createContainer(newId.asPtr(), name, description);
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
        result = kj::get<0>(createContainer(id, name, description));
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
          result.add(kj::get<0>(createContainer(id, name, description)));
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

  
};

//-----------------------------------------------------------------------------

struct Container::Impl {
  Container *self{nullptr};
  mas::infrastructure::common::Restorer *restorer{nullptr};
  sqlite3 *db{nullptr};
  kj::String id;
  kj::String name{kj::str("Container")};
  kj::String description;
  mas::schema::storage::Store::Container::Client client{nullptr};
  kj::HashMap<kj::String, mas::schema::storage::Store::Container::Entry::Client> entries; 

  Impl(Container *self, mas::infrastructure::common::Restorer *restorer, sqlite3 *db, 
    kj::StringPtr id, kj::StringPtr name, kj::StringPtr description)
  : self(self)
  , restorer(restorer)
  , db(db)
  , id(kj::str(id))
  , name(kj::str(name))
  , description(kj::str(description)) {
    
  }

  ~Impl() noexcept(false)  {}
  
  bool addObject(kj::StringPtr key, mas::schema::storage::Store::Container::Entry::Value::Reader value,
    bool decodeAnyValueFromBase64Text = false) {
    auto insertStmt = kj::str("INSERT INTO '", id, "' (key, value) VALUES ('", key, "', ?);");
    sqlite3_stmt* stmt = nullptr;
    auto rc = sqlite3_prepare_v2(db, insertStmt.cStr(), -1, &stmt, NULL);
    bool success = false;
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't prepare statement.", insertStmt);
    } else {
      capnp::MallocMessageBuilder message;
      auto builder = message.initRoot<mas::schema::storage::Store::Container::Entry::Value>();
      ::_::setEntryValue(builder, value, false, decodeAnyValueFromBase64Text);
      auto sizeInBytes = message.sizeInWords()*sizeof(capnp::word);
      kj::Array<kj::byte> bytes = kj::heapArray<kj::byte>(sizeInBytes);
      kj::ArrayOutputStream outs(bytes);
      capnp::writePackedMessage(outs, message);
      auto filledBytes = outs.getArray();
      sqlite3_bind_blob(stmt, 1, filledBytes.begin(), filledBytes.size(), SQLITE_STATIC);
      rc = sqlite3_step(stmt);
      success = rc == SQLITE_DONE;
      if(!success) {
        KJ_LOG(ERROR, "Can't insert object.", insertStmt);
      }
      sqlite3_finalize(stmt);
    }
    return success;
  }

  
  void downloadEntries(capnp::List<mas::schema::common::Pair<capnp::Text, mas::schema::storage::Store::Container::Entry::Value>, capnp::Kind::STRUCT>::Builder list,
    bool encodeAnyValueAsBase64Text = false,
    capnp::List<bool, capnp::Kind::PRIMITIVE>::Builder isAnyValue = nullptr) {

    auto selectStmt = kj::str("SELECT key, value FROM '", id, "' ORDER BY key");
    sqlite3_stmt* stmt = nullptr;
    auto rc = sqlite3_prepare_v2(db, selectStmt.cStr(), -1, &stmt, NULL);
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't prepare statement.", selectStmt);
    } else {
      int i = 0;
      // in case of to be encoded any values, we need to know which ones are encoded and sizes have to match
      KJ_ASSERT(!encodeAnyValueAsBase64Text || list.size() == isAnyValue.size());
      while(sqlite3_step(stmt) == SQLITE_ROW && i < list.size()) {
        if(encodeAnyValueAsBase64Text){
          auto keyAndSucc = ::_::getKeyAndSetValueFromDBRow(list[i].initSnd(), stmt, true);
          list[i].setFst(kj::get<0>(keyAndSucc));
          isAnyValue.set(i, kj::get<1>(keyAndSucc));
        } else {
          auto keyAndSucc = ::_::getKeyAndSetValueFromDBRow(list[i].initSnd(), stmt);
          list[i].setFst(kj::get<0>(keyAndSucc));
        }
        i++;
      } 
      sqlite3_finalize(stmt);
    }
  }


  void listEntries(capnp::List<mas::schema::storage::Store::Container::Entry, capnp::Kind::INTERFACE>::Builder list,
    bool encodeAnyValueAsBase64Text = false,
    capnp::List<bool, capnp::Kind::PRIMITIVE>::Builder isAnyValue = nullptr) {
    auto selectStmt = kj::str("SELECT key FROM '", id, "' ORDER BY key");
    sqlite3_stmt* stmt = nullptr;
    auto rc = sqlite3_prepare_v2(db, selectStmt.cStr(), -1, &stmt, NULL);
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't prepare statement.", selectStmt);
    } else {
      int i = 0;
      // in case of to be encoded any values, we need to know which ones are encoded and sizes have to match
      KJ_ASSERT(!encodeAnyValueAsBase64Text || list.size() == isAnyValue.size());
      while(sqlite3_step(stmt) == SQLITE_ROW && i < list.size()) {
        auto key = reinterpret_cast<const char*>(sqlite3_column_text(stmt, 0));
        // entry already cached?
        KJ_IF_MAYBE(entry, entries.find(key)){
          list[i] = *entry;
        } else { // no, create one
          mas::schema::storage::Store::Container::Entry::Client e = kj::heap<Entry>(self, key);
          entries.insert(kj::str(key), e);
          list[i] = e;
        }
        i++;
      } 
      sqlite3_finalize(stmt);
    }
  }


  bool removeEntry(kj::StringPtr key) {
    auto deleteStmt = kj::str("DELETE FROM '", id, "' WHERE key = '", key, "'");
    char* errMsg;
    auto rc = sqlite3_exec(db, deleteStmt.cStr(), NULL, NULL, &errMsg);
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't delete object.", deleteStmt, "Error:", errMsg);
      sqlite3_free(errMsg);
      return false;
    }
    return true;
  }


  size_t countEntries() {
    auto selectStmt = kj::str("SELECT count(key) FROM '", id, "'");
    sqlite3_stmt* stmt = nullptr;
    auto rc = sqlite3_prepare_v2(db, selectStmt.cStr(), -1, &stmt, NULL);
    size_t count = 0;
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't prepare statement.", selectStmt);
    } else {
      rc = sqlite3_step(stmt);
      if(rc == SQLITE_ROW) {
        count = (size_t)sqlite3_column_int64(stmt, 0);
      } else {
        KJ_LOG(ERROR, "Can't get entry.", selectStmt);
      }
      sqlite3_finalize(stmt);
    }
    return count;
  }

  bool clearContainer() {
    auto deleteStmt = kj::str("DELETE * FROM '", id, "'");
    char* errMsg;
    auto rc = sqlite3_exec(db, deleteStmt.cStr(), NULL, NULL, &errMsg);
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't delete all entries from container.", deleteStmt, "Error:", errMsg);
      sqlite3_free(errMsg);
      return false;
    }
    return true;
  }

};

// ----------------------------------------------------------------------

struct Entry::Impl {
  Container::Impl *cImpl{nullptr};
  kj::String key;

  Impl(Container::Impl *ci, kj::StringPtr key)
  : cImpl(ci)
  , key(kj::str(key))
  {}    

  ~Impl() noexcept(false)  {}

  void getEntryValue(mas::schema::storage::Store::Container::Entry::Value::Builder valueBuilder) {
    auto selectStmt = kj::str("SELECT value FROM '", cImpl->id, "' WHERE key = '", key, "';");
    sqlite3_stmt* stmt = nullptr;
    auto rc = sqlite3_prepare_v2(cImpl->db, selectStmt.cStr(), -1, &stmt, NULL);
    if(rc != SQLITE_OK) {
      KJ_LOG(ERROR, "Can't prepare statement.", selectStmt);
    } else {
      rc = sqlite3_step(stmt);
      if(rc == SQLITE_ROW) {
        ::_::getKeyAndSetValueFromDBRow(valueBuilder, stmt);
      } else {
        KJ_LOG(ERROR, "Can't get object.", selectStmt);
      }
      sqlite3_finalize(stmt);
    }
  }


};


// ----------------------------------------------------------------------

SqliteStorageService::SqliteStorageService(kj::StringPtr filename, kj::StringPtr name, 
  kj::StringPtr description, mas::infrastructure::common::Restorer* restorer)
: impl(kj::heap<Impl>(*this, restorer, filename, name, description)) {
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
  if(impl->restorer) {
    return impl->restorer->save(impl->client, context.getResults().initSturdyRef(), context.getResults().initUnsaveSR());
  }
  return kj::READY_NOW;
}


kj::Promise<void> SqliteStorageService::newContainer(NewContainerContext context) {
  KJ_LOG(INFO, "newContainer message received");
  auto info = context.getParams();
  context.getResults().setContainer(kj::get<0>(impl->createContainerDb(info.getName(), info.getDescription())));
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


kj::Promise<void> SqliteStorageService::importContainer(ImportContainerContext context) {
  KJ_LOG(INFO, "importContainer message received");
  capnp::JsonCodec json;
  capnp::MallocMessageBuilder msg;
  auto builder = msg.initRoot<mas::schema::storage::Store::ImportExportData>();
  json.decode(context.getParams().getJson(), builder);
  auto t = impl->createContainerDb(builder.getInfo().getName(), builder.getInfo().getDescription(), builder.getInfo().getId());
  auto container = kj::get<1>(t);
  auto entries = builder.getEntries().asReader();
  auto isAnyValue = builder.getIsAnyValue().asReader();
  for(auto i : kj::indices(entries)) {
    container->impl->addObject(entries[i].getFst(), entries[i].getSnd(), isAnyValue[i]);
  }
  context.getResults().setContainer(kj::get<0>(t));
  return kj::READY_NOW;
}

mas::schema::storage::Store::Client SqliteStorageService::getClient() { return impl->client; }
void SqliteStorageService::setClient(mas::schema::storage::Store::Client c) { impl->client = c; }

mas::schema::common::Action::Client SqliteStorageService::getUnregisterAction() { return impl->unregisterAction; }
void SqliteStorageService::setUnregisterAction(mas::schema::common::Action::Client unreg) { impl->unregisterAction = unreg; }

void SqliteStorageService::setRestorer(mas::infrastructure::common::Restorer* restorer){ 
  impl->setRestorer(restorer);
}

void SqliteStorageService::initFromStorageContainer(){

}

// ----------------------------------------------------------------------

Container::Container(SqliteStorageService& store, kj::StringPtr id, kj::StringPtr name, kj::StringPtr description)
: impl(kj::heap<Impl>(this, store.impl->restorer, store.impl->db, id, name, description)) {
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
  if(impl->restorer) {
    return impl->restorer->save(impl->client, context.getResults().initSturdyRef(), context.getResults().initUnsaveSR(),
      nullptr, nullptr, true, impl->id);
  }
  return kj::READY_NOW;
}


kj::Promise<void> Container::export_(ExportContext context) {
  KJ_LOG(INFO, "export message received");
  capnp::JsonCodec json;
  json.setPrettyPrint(true);
  capnp::MallocMessageBuilder msg;
  auto builder = msg.initRoot<mas::schema::storage::Store::ImportExportData>();
  auto info = builder.initInfo();
  info.setId(impl->id);
  info.setName(impl->name);
  info.setDescription(impl->description);
  // encode anyvalues as base64 text
  auto noOfEntries = impl->countEntries();
  impl->downloadEntries(builder.initEntries(noOfEntries), true, builder.initIsAnyValue(noOfEntries));
  auto expStr = json.encode(builder.asReader());
  context.getResults().setJson(expStr);
  return kj::READY_NOW;
}


kj::Promise<void> Container::downloadEntries(DownloadEntriesContext context) {
  KJ_LOG(INFO, "downloadEntries message received");
  auto count = impl->countEntries();
  impl->downloadEntries(context.getResults().initEntries(count));
  return kj::READY_NOW;
}


kj::Promise<void> Container::listEntries(ListEntriesContext context) {
  KJ_LOG(INFO, "listEntries message received");
  auto count = impl->countEntries();
  impl->listEntries(context.getResults().initEntries(count));
  return kj::READY_NOW;
}


kj::Promise<void> Container::getEntry(GetEntryContext context) {
  KJ_LOG(INFO, "getEntries message received");
  auto rs = context.getResults();
  auto key = context.getParams().getKey();
  KJ_IF_MAYBE(entry, impl->entries.find(key)){
    rs.setEntry(*entry);
    rs.setIsNew(false);
  } else {
    rs.setIsNew(true);
    mas::schema::storage::Store::Container::Entry::Client newEntry = kj::heap<Entry>(this, key);
    impl->entries.insert(kj::str(key), newEntry);
    rs.setEntry(newEntry);
  }
  return kj::READY_NOW;
}


kj::Promise<void> Container::removeEntry(RemoveEntryContext context) {
  KJ_LOG(INFO, "removeObject message received");
  context.getResults().setSuccess(impl->removeEntry(context.getParams().getKey()));
  return kj::READY_NOW;
}


kj::Promise<void> Container::clear(ClearContext context) {
  KJ_LOG(INFO, "clear message received");
  context.getResults().setSuccess(impl->clearContainer());
  return kj::READY_NOW;
}


mas::schema::storage::Store::Container::Client Container::getClient() { return impl->client; }
void Container::setClient(mas::schema::storage::Store::Container::Client c) { impl->client = c; }


// ----------------------------------------------------------------------

Entry::Entry(Container *c, kj::StringPtr key)
: impl(kj::heap<Impl>(c->impl, key)) {
}

Entry::~Entry() {}

kj::Promise<void> Entry::getKey(GetKeyContext context) {
  KJ_LOG(INFO, "getKey message received");
  context.getResults().setKey(impl->key);
  return kj::READY_NOW;
}


kj::Promise<void> Entry::getValue(GetValueContext context) {
  KJ_LOG(INFO, "getValue message received");
  impl->getEntryValue(context.getResults().initValue());
  return kj::READY_NOW;
}


kj::Promise<void> Entry::setValue(SetValueContext context) {
  KJ_LOG(INFO, "setValue message received");


  return kj::READY_NOW;
}

