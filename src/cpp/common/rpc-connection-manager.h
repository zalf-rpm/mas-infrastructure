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

#include <kj/async-io.h>
#include <kj/string.h>
#include <kj/tuple.h>

#include <capnp/capability.h>

namespace mas {
namespace infrastructure {
namespace common {

//-----------------------------------------------------------------------------

class Restorer;

class ConnectionManager final {
public:
	ConnectionManager(Restorer* restorer = nullptr);
	~ConnectionManager() noexcept(false);

	kj::StringPtr getLocallyUsedHost() const;
	void setLocallyUsedHost(kj::StringPtr h);

	kj::Promise<capnp::Capability::Client> tryConnect(kj::AsyncIoContext& ioc, kj::StringPtr sturdyRefStr, 
		int retryCount = 10, int retrySecs = 5, bool printRetryMsgs = true);

	capnp::Capability::Client tryConnectB(kj::AsyncIoContext& ioc, kj::StringPtr sturdyRefStr, 
		int retryCount = 10, int retrySecs = 5, bool printRetryMsgs = true);

	kj::Promise<capnp::Capability::Client> connect(kj::AsyncIoContext& ioContext, kj::StringPtr sturdyRefStr);

	kj::Promise<kj::uint> bind(kj::AsyncIoContext& ioContext, 
		capnp::Capability::Client mainInterface, kj::StringPtr host, kj::uint port = 0U);

private:
	struct Impl;
	kj::Own<Impl> impl;
};

//-----------------------------------------------------------------------------

kj::Tuple<bool, kj::String> getLocalIP(kj::StringPtr connectToHost = "8.8.8.8", kj::uint connectToPort = 53);

} // namespace common
} // namespace infrastructure
} // namespace mas
