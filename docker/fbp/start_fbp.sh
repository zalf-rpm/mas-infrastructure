#!/bin/sh

# get command line arguments

if [ $# -eq 0 ]; then
    echo "No arguments supplied"
    exit 1
fi

FBP_COMPONENT=$1
OTHER_ARGS=${@:2}

# start the FBP component

cd /envs
# execute component with args
#example poetry run python -u /mas-infrastructure/src/python/zalfmas-fbp/zalfmas_fbp/components/console_output.py in_sr=capnp://_VcNoyHw5kwHS62qx_uIHqrnhYXUPEmfegbtIY9rf4I@10.10.25.19:9922/r_out
poetry run python -u /mas-infrastructure/src/python/zalfmas-fbp/zalfmas_fbp/components/$FBP_COMPONENT $OTHER_ARGS


