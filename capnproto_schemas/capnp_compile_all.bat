set PROGLANG=%1

mkdir gen\%PROGLANG%\test 
if "%PROGLANG%"=="go" (
    capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/test zalfmas_capnp_schemas/x.capnp zalfmas_capnp_schemas/a.capnp
) else (
    capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/test zalfmas_capnp_schemas/x.capnp
    capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/test zalfmas_capnp_schemas/a.capnp
)
mkdir gen\%PROGLANG%\climate   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/climate zalfmas_capnp_schemas/climate.capnp
mkdir gen\%PROGLANG%\cluster   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/cluster zalfmas_capnp_schemas/cluster_admin_service.capnp
mkdir gen\%PROGLANG%\common   
if "%PROGLANG%"=="go" (
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/common zalfmas_capnp_schemas/common.capnp zalfmas_capnp_schemas/date.capnp
) else (
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/common zalfmas_capnp_schemas/common.capnp
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/common zalfmas_capnp_schemas/date.capnp
)
mkdir gen\%PROGLANG%\config   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/config zalfmas_capnp_schemas/config.capnp
mkdir gen\%PROGLANG%\crop   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/crop zalfmas_capnp_schemas/crop.capnp
mkdir gen\%PROGLANG%\fbp   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/fbp zalfmas_capnp_schemas/fbp.capnp
rem mkdir -p gen/%PROGLANG%/frontend   
rem  capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/frontend zalfmas_capnp_schemas/frontend.capnp
mkdir gen\%PROGLANG%\geo   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/geo zalfmas_capnp_schemas/geo.capnp
mkdir gen\%PROGLANG%\grid   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/grid zalfmas_capnp_schemas/grid.capnp
mkdir gen\%PROGLANG%\jobs   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/jobs zalfmas_capnp_schemas/jobs.capnp
mkdir gen\%PROGLANG%\management   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/management zalfmas_capnp_schemas/management.capnp
mkdir gen\%PROGLANG%\model   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/model zalfmas_capnp_schemas/model.capnp
mkdir gen\%PROGLANG%\persistence   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/persistence zalfmas_capnp_schemas/persistence.capnp
mkdir gen\%PROGLANG%\registry   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/registry zalfmas_capnp_schemas/registry.capnp
mkdir gen\%PROGLANG%\service   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/service zalfmas_capnp_schemas/service.capnp
mkdir gen\%PROGLANG%\soil   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/soil zalfmas_capnp_schemas/soil.capnp
mkdir gen\%PROGLANG%\storage   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/storage zalfmas_capnp_schemas/storage.capnp
rem mkdir -p gen/%PROGLANG%/vr   
rem  capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG%/vr vr.capnp

mkdir gen\%PROGLANG%\model\weberest   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG% zalfmas_capnp_schemas/model/weberest/web-berest-data-import.capnp
mkdir gen\%PROGLANG%\model\yieldstat   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG% zalfmas_capnp_schemas/model/yieldstat/yieldstat.capnp
mkdir gen\%PROGLANG%\model\monica   
rem check if program language is go
if "%PROGLANG%"=="go" (
    capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG% zalfmas_capnp_schemas/model/monica/monica_management.capnp zalfmas_capnp_schemas/model/monica/monica_params.capnp zalfmas_capnp_schemas/model/monica/monica_state.capnp zalfmas_capnp_schemas/model/monica/soil_params.capnp
) else (
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG% zalfmas_capnp_schemas/model/monica/monica_management.capnp 
mkdir gen\%PROGLANG%\model\monica   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG% zalfmas_capnp_schemas/model/monica/monica_params.capnp
mkdir gen\%PROGLANG%\model\monica   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG% zalfmas_capnp_schemas/model/monica/monica_state.capnp
mkdir gen\%PROGLANG%\model\monica   
 capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -o%PROGLANG%:./gen/%PROGLANG% zalfmas_capnp_schemas/model/monica/soil_params.capnp
)