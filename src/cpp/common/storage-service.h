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

namespace mas {
namespace infrastructure {

  namespace common { class Restorer; }

namespace storage {

class SqliteStorageService final : public mas::schema::storage::Store::Server
{
public:
  SqliteStorageService(kj::StringPtr filename, kj::StringPtr name, kj::StringPtr description, 
    mas::infrastructure::common::Restorer* restorer = nullptr);

  virtual ~SqliteStorageService() noexcept(false);

  kj::Promise<void> info(InfoContext context) override;

  kj::Promise<void> save(SaveContext context) override;

  kj::Promise<void> newContainer(NewContainerContext context) override;

  kj::Promise<void> containerWithId(ContainerWithIdContext context) override;

  kj::Promise<void> listContainers(ListContainersContext context) override;

  kj::Promise<void> removeContainer(RemoveContainerContext context) override;

  kj::Promise<void> importContainer(ImportContainerContext context) override;

  mas::schema::storage::Store::Client getClient();
  void setClient(mas::schema::storage::Store::Client c);

  mas::schema::common::Action::Client getUnregisterAction();
  void setUnregisterAction(mas::schema::common::Action::Client unreg);

  void setRestorer(mas::infrastructure::common::Restorer* r);

  void initFromStorageContainer();

private:
  struct Impl;
  kj::Own<Impl> impl;

  friend class Container;
  friend class Object;
};

//-----------------------------------------------------------------------------

class Container final : public mas::schema::storage::Store::Container::Server {
public:
  Container(SqliteStorageService& s, kj::StringPtr id, kj::StringPtr name, kj::StringPtr description);

  virtual ~Container() noexcept(false);

  kj::Promise<void> info(InfoContext context) override;

  kj::Promise<void> save(SaveContext context) override;

  kj::Promise<void> export_(ExportContext context) override;

  kj::Promise<void> downloadEntries(DownloadEntriesContext context) override;

  kj::Promise<void> listEntries(ListEntriesContext context) override;

  kj::Promise<void> getEntry(GetEntryContext context) override;

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

//-----------------------------------------------------------------------------

class Entry final : public mas::schema::storage::Store::Container::Entry::Server {
public:
  Entry(Container *container, kj::StringPtr key);

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
} // namespace infrastructure
} // namespace mas
