/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg-mohnicke@zalf.de>

Maintainers:
Currently maintained by the authors.

This file is part of the ZALF model and simulation infrastructure.
Copyright (C) Leibniz Centre for Agricultural Landscape Research (ZALF)
*/

#pragma once

#include "storage.capnp.h"

namespace mas::infrastructure {

namespace common { class Restorer; }

namespace storage {

const char *const LAST_STORAGE_SERVICE_KEY_NAME = "last_storage_service_sr_token";
const char *const RESTORE_STORAGE_ITSELF_TOKEN_VALUE = "storage_service_itself";

class SqliteStorageService final : public mas::schema::storage::Store::Server {
public:
  SqliteStorageService(kj::StringPtr filename, kj::StringPtr name, kj::StringPtr description,
                       mas::infrastructure::common::Restorer *restorer = nullptr);

  ~SqliteStorageService();

  kj::Promise<void> info(InfoContext context) override;

  kj::Promise<void> save(SaveContext context) override;

  kj::Promise<void> newContainer(NewContainerContext context) override;

  kj::Promise<void> containerWithId(ContainerWithIdContext context) override;

  kj::Promise<void> listContainers(ListContainersContext context) override;

  kj::Promise<void> removeContainer(RemoveContainerContext context) override;

  kj::Promise<void> importContainer(ImportContainerContext context) override;

  mas::schema::storage::Store::Client getClient();

  void setClient(mas::schema::storage::Store::Client c);

  //mas::schema::common::Action::Client getUnregisterAction();
  //void setUnregisterAction(mas::schema::common::Action::Client unreg);

  void setRestorer(mas::infrastructure::common::Restorer *r);

  //void initFromStorageContainer();

private:
  struct Impl;
  kj::Own<Impl> impl;

  friend class Container;

  friend class Object;
};

class Container final : public mas::schema::storage::Store::Container::Server {
public:
  Container(SqliteStorageService &s, kj::StringPtr id, kj::StringPtr name, kj::StringPtr description);

  virtual ~Container() = default;

  kj::Promise<void> info(InfoContext context) override;

  kj::Promise<void> save(SaveContext context) override;

  kj::Promise<void> export_(ExportContext context) override;

  kj::Promise<void> downloadEntries(DownloadEntriesContext context) override;

  kj::Promise<void> listEntries(ListEntriesContext context) override;

  kj::Promise<void> getEntry(GetEntryContext context) override;

  kj::Promise<void> addEntry(AddEntryContext context) override;

  kj::Promise<void> removeEntry(RemoveEntryContext context) override;

  kj::Promise<void> clear(ClearContext context) override;

  mas::schema::storage::Store::Container::Client getClient();

  void setClient(mas::schema::storage::Store::Container::Client c);

private:
  struct Impl;
  kj::Own<Impl> impl;

  friend class SqliteStorageService;

  friend class Entry;
};

class Entry final : public mas::schema::storage::Store::Container::Entry::Server {
public:
  Entry(Container *container, kj::StringPtr key, bool isUnset);

  virtual ~Entry() noexcept(false);

  kj::Promise<void> getKey(GetKeyContext context) override;

  kj::Promise<void> getValue(GetValueContext context) override;

  kj::Promise<void> setValue(SetValueContext context) override;

private:
  struct Impl;
  kj::Own<Impl> impl;

  friend class SqliteStorageService;
};

} // namespace storage
} // namespace mas::infrastructure
