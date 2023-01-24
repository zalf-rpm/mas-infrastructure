/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg@zalf.de>

Maintainers:
Currently maintained by the authors.

This file is part of the util library used by models created at the Institute of
Landscape Systems Analysis at the ZALF.
Copyright (C) Leibniz Centre for Agricultural Landscape Research (ZALF)
*/

#include <sstream>
#include <algorithm>
#include <iostream>
#include <map>
#include <cstdio>
#include <mutex>

#include "orm--.h"
#include "tools/algorithms.h"

using namespace Db;
using namespace std;
using namespace Tools;

string Identifiable::toString(const string& /*indent*/, bool /*detailed*/) const
{
	ostringstream s;
	s << id << ": " << name;
	return s.str();
}

string Describable::toString(const string& indent, bool /*detailed*/) const
{
	ostringstream s;
	s << Identifiable::toString(indent) << ", " << description;
	return s.str();
}
