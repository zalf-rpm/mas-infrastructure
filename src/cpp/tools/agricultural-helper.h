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

#ifndef _AGRICULTURAL_HELPER_H
#define	_AGRICULTURAL_HELPER_H

#include <string>
#include <vector>

namespace Tools
{
	//! create stt from stt code
	std::string sttFromCode(int sttCode);

	std::vector<std::string> sttsFromCode(int sttCode);

	//! splits stt code into its parts
	std::vector<int> splitSttCode(int sttCode);

	int sttCodeFromStt(std::string stt);
}

#endif	/* _AGRICULTURAL_HELPER_H */

