#!/bin/bash -x

# start dwd_core_ensemble service with singularity
# example usage: sbatch example_start_script.sh mas-services_0_7.sif /config/folder/path /log/path /tmp/path

# please note if the config toml of dwd_core_ensemble points to a different path for the data, this path should be mounted as well
# for now I assume that the data is in the same folder as the config

SINGULARITY_IMAGE=$1
MOUNT_DATA=$2
MOUNT_LOG=$3
MOUNT_TMP=$4
LOGOUT=/var/log
TMP_LOG=/tmp

ENV_VARS=autostart_dwd_core_ensemble=true,\
config_dwd_core_ensemble=${MOUNT_DATA},\
auto_restart=true\


singularity run --cleanenv --env ${ENV_VARS} -B \
$MOUNT_LOG:$LOGOUT,\
$MOUNT_TMP:$TMP_LOG,\
$MOUNT_DATA:$MOUNT_DATA \
${SINGULARITY_IMAGE} 
