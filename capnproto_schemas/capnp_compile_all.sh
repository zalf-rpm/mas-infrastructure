#!/bin/sh

if [ $# -ge 1 ] 
then
	LANG=$1
else
	echo "language missing: c++ | csharp | go | java"
	echo "usage: capnp_compile_all.sh lang [path-to-capnp [path-to-capnpc-lang]]"
	exit
fi

if [ $# -ge 2 ]
then
	PATH_TO_CAPNP=$2
else
	echo "used path to 'capnp' is:" `which capnp`
	PATH_TO_CAPNP=capnp
fi

if [ $# -ge 3 ]
then
	PATH_TO_CAPNPC_LANG=$3
else
	echo "used path to 'capnp-"$LANG"' is:" `which capnpc-${LANG}`
	PATH_TO_CAPNPC_LANG=$LANG
fi




mkdir -p gen/$LANG/test ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/test zalfmas_capnp_schemas/x.capnp
mkdir -p gen/$LANG/test ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/test zalfmas_capnp_schemas/a.capnp
mkdir -p gen/$LANG/climate ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/climate zalfmas_capnp_schemas/climate.capnp
mkdir -p gen/$LANG/cluster ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/cluster zalfmas_capnp_schemas/cluster_admin_service.capnp
mkdir -p gen/$LANG/common ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/common zalfmas_capnp_schemas/common.capnp
mkdir -p gen/$LANG/config ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/config zalfmas_capnp_schemas/config.capnp
mkdir -p gen/$LANG/crop ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/crop zalfmas_capnp_schemas/crop.capnp
mkdir -p gen/$LANG/common ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/common zalfmas_capnp_schemas/date.capnp
mkdir -p gen/$LANG/fbp ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/fbp zalfmas_capnp_schemas/fbp.capnp
#mkdir -p gen/$LANG/frontend ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/frontend zalfmas_capnp_schemas/frontend.capnp
mkdir -p gen/$LANG/geo ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/geo zalfmas_capnp_schemas/geo.capnp
mkdir -p gen/$LANG/grid ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/grid zalfmas_capnp_schemas/grid.capnp
mkdir -p gen/$LANG/jobs ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/jobs zalfmas_capnp_schemas/jobs.capnp
mkdir -p gen/$LANG/management ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/management zalfmas_capnp_schemas/management.capnp
mkdir -p gen/$LANG/model ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/model zalfmas_capnp_schemas/model.capnp
mkdir -p gen/$LANG/persistence ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/persistence zalfmas_capnp_schemas/persistence.capnp
mkdir -p gen/$LANG/registry ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/registry zalfmas_capnp_schemas/registry.capnp
mkdir -p gen/$LANG/service ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/service zalfmas_capnp_schemas/service.capnp
mkdir -p gen/$LANG/soil ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/soil zalfmas_capnp_schemas/soil.capnp
mkdir -p gen/$LANG/storage ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/storage zalfmas_capnp_schemas/storage.capnp
#mkdir -p gen/$LANG/vr ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG/vr vr.capnp

mkdir -p gen/$LANG/model/weberest ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG zalfmas_capnp_schemas/model/weberest/web-berest-data-import.capnp
mkdir -p gen/$LANG/model/yieldstat ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG zalfmas_capnp_schemas/model/yieldstat/yieldstat.capnp
mkdir -p gen/$LANG/model/monica ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG zalfmas_capnp_schemas/model/monica/monica_management.capnp
mkdir -p gen/$LANG/model/monica ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG zalfmas_capnp_schemas/model/monica/monica_params.capnp
mkdir -p gen/$LANG/model/monica ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG zalfmas_capnp_schemas/model/monica/monica_state.capnp
mkdir -p gen/$LANG/model/monica ; $PATH_TO_CAPNP compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o$PATH_TO_CAPNPC_LANG:./gen/$LANG zalfmas_capnp_schemas/model/monica/soil_params.capnp
