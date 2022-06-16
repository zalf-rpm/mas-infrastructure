/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg-mohnicke@zalf.de>

Maintainers:
Currently maintained by the authors.

This file is part of the MONICA model.
Copyright (C) Leibniz Centre for Agricultural Landscape Research (ZALF)
*/

#pragma once

#include <kj/debug.h>
#include <kj/common.h>
#include <kj/string.h>
#include <kj/vector.h>
#include <kj/map.h>

#include <capnp/rpc-twoparty.h>
#include <kj/thread.h>

#include <functional>
#include <string>
#include <vector>

//#include "model.capnp.h"
#include "common.capnp.h"
#include "persistence.capnp.h"

namespace mas {
  namespace rpc {
    namespace common {

      class Restorer final : public mas::schema::persistence::Restorer::Server
      {
        public:
        Restorer() {}

        virtual ~Restorer() noexcept(false) {}

        // restore @0 (srToken :Text) -> (cap :Capability);
        kj::Promise<void> restore(RestoreContext context) override;

        int getPort() const { return _port; }
        void setPort(int p) { _port = p; }

        std::string getHost() const { return _host; }
        void setHost(std::string h) { _host = h; }

        std::string sturdyRef(std::string srToken = "") const;

        std::pair<std::string, std::string> save(capnp::Capability::Client cap, std::string srToken = std::string(),
          bool createUnsave = true);

        void unsave(std::string srToken);

      private:
        std::string _host{ "" };
        int _port{ 0 };
        kj::HashMap<kj::String, capnp::Capability::Client> _issuedSRTokens;
        std::vector<std::function<void()>> _actions;
      };

      //-----------------------------------------------------------------------------

      class Identifiable final : public mas::schema::common::Identifiable::Server {
      public:
        Identifiable(std::string id = "", std::string name = "", std::string description = "");

        virtual ~Identifiable() noexcept(false) {}

        kj::Promise<void> info(InfoContext context) override;

      private:
        std::string _id{ "" };
        std::string _name{ "" };
        std::string _description{ "" };
      };

      //-----------------------------------------------------------------------------

      class CallbackImpl final : public mas::schema::common::Callback::Server {
      public:
        CallbackImpl(std::function<void()> callback, 
                    bool execCallbackOnDel = false,
                    std::string id = "<-");

        virtual ~CallbackImpl() noexcept(false);

        kj::Promise<void> call(CallContext context) override;

      private:
        std::string id{ "<-" };
        std::function<void()> callback;
        bool execCallbackOnDel{ false };
        bool alreadyCalled{ false };
      };

      //-----------------------------------------------------------------------------

      class Action final : public mas::schema::common::Action::Server {
      public:
        Action(std::function<void()> action, 
                    bool execActionOnDel = false,
                    std::string id = "<-");

        virtual ~Action() noexcept(false);

        kj::Promise<void> do_(DoContext context) override;

      private:
        std::string id{ "<-" };
        std::function<void()> action;
        bool execActionOnDel{ false };
        bool alreadyCalled{ false };
      };

      //-----------------------------------------------------------------------------

      class CapHolderImpl final : public mas::schema::common::CapHolder<capnp::AnyPointer>::Server {
      public:
        CapHolderImpl(capnp::Capability::Client cap,
                      kj::String sturdyRef,
                      bool releaseOnDel = false,
                      std::string id = "-");

        virtual ~CapHolderImpl() noexcept(false);

        kj::Promise<void> cap(CapContext context) override;

        kj::Promise<void> release(ReleaseContext context) override;

        //kj::Promise<void> save(SaveContext context) override;

      private:
        std::string id{ "-" };
        capnp::Capability::Client _cap;
        kj::String sturdyRef;
        bool releaseOnDel{ false };
        bool alreadyReleased{ false };
      };

      //-----------------------------------------------------------------------------

      class CapHolderListImpl final : public mas::schema::common::CapHolder<capnp::AnyPointer>::Server {
      public:
        CapHolderListImpl(kj::Vector<capnp::Capability::Client>&& caps,
                          kj::String sturdyRef,
                          bool releaseOnDel = false,
                          std::string id = "[-]");

        virtual ~CapHolderListImpl() noexcept(false);

        kj::Promise<void> cap(CapContext context) override;

        kj::Promise<void> release(ReleaseContext context) override;

        //kj::Promise<void> save(SaveContext context) override;

      private:
        std::string id{ "[-]" };
        kj::Vector<capnp::Capability::Client> caps;
        kj::String sturdyRef;
        bool releaseOnDel{ false };
        bool alreadyReleased{ false };
      };

      //-----------------------------------------------------------------------

      kj::Maybe<capnp::AnyPointer::Reader> getIPAttr(schema::common::IP::Reader ip, kj::StringPtr attrName);
       
      kj::Maybe<capnp::AnyPointer::Builder> 
      copyAndSetIPAttrs(schema::common::IP::Reader oldIp, schema::common::IP::Builder newIp, 
        kj::StringPtr newAttrName = nullptr);//, kj::Maybe<capnp::AnyPointer::Reader> newValue = nullptr);
    } 
  }
}
