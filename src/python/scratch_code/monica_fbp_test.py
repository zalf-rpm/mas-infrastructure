#!/usr/bin/python
# -*- coding: UTF-8

# This Source Code Form is subject to the terms of the Mozilla Public
# License, v. 2.0. If a copy of the MPL was not distributed with this
# file, You can obtain one at http://mozilla.org/MPL/2.0/. */

# Authors:
# Michael Berg-Mohnicke <michael.berg-mohnicke@zalf.de>
#
# Maintainers:
# Currently maintained by the authors.
#
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)

import asyncio
import threading
from collections import defaultdict
import capnp
import json
import os
from pathlib import Path
import subprocess as sp
import sys
import uuid
from zalfmas_common import common
from zalfmas_common.climate import csv_file_based as csv_based
import zalfmas_capnp_schemas
from zalfmas_common import service as serv
from zalfmas_common.model import monica_io
from zalfmas_fbp.run import channels as chans, ports as fbp_ports
capnp_path = Path(os.path.dirname(zalfmas_capnp_schemas.__file__))
sys.path.append(str(capnp_path))
import climate_capnp
import common_capnp
import fbp_capnp
import model_capnp
import crop_capnp

sys.path.append(str(capnp_path / "model" / "monica"))
import monica_management_capnp as mgmt_capnp
import monica_params_capnp

def start_component(path_to_component, writer_sr, name=None, verbose=False):
    return sp.Popen([path_to_component,
                    f"--name=comp_{name if name else str(uuid.uuid4())}",
                    f"--startup_info_writer_sr={writer_sr}",
                    ] + (["--verbose"] if verbose else []))

standalone_config = {
    "path_to_channel": "/home/berg/GitHub/monica/_cmake_debug/common/channel",
}
async def main(config: dict):
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    con_man = common.ConnectionManager()
    channels = []

    try:
        first_chan, first_reader_sr, first_writer_sr = chans.start_first_channel(config["path_to_channel"])
        channels.append(first_chan)
        first_reader = await con_man.try_connect(first_reader_sr, cast_as=fbp_capnp.Channel.Reader)

        # create the three channels for the three ports
        channels.append(chans.start_channel(config["path_to_channel"],
                                            "env_in|" + first_writer_sr, name="env"))
        channels.append(chans.start_channel(config["path_to_channel"],
                                            "result_out|" + first_writer_sr, name="result"))
        channels.append(chans.start_channel(config["path_to_channel"],
                                            "port_infos|" + first_writer_sr, name="port_infos",
                                            port=9991,
                                            reader_srts="r_in"))

        port_srs = {"in": {}, "out": {}}
        port_infos_reader_sr = None
        port_infos_writer = None
        port_infos_msg = fbp_capnp.PortInfos.new_message()
        inPorts = []
        outPorts = []
        for i in range(3):
            p = (await first_reader.read()).value.as_struct(common_capnp.Pair)
            c_id = p.fst.as_text()
            info = p.snd.as_struct(fbp_capnp.Channel.StartupInfo)
            print("channel:", c_id, "reader_sr:", info.readerSRs[0], "writer_sr:", info.writerSRs[0])
            if c_id[-3:] == "_in":
                port_name = c_id[:-3]
                inPorts.append({"name": port_name, "sr": info.readerSRs[0]})
                port_srs["in"][port_name] = info.writerSRs[0]
            elif c_id[-4:] == "_out":
                port_name = c_id[:-4]
                outPorts.append({"name": port_name, "sr": info.writerSRs[0]})
                port_srs["out"][port_name] = info.readerSRs[0]
            else:
                port_infos_writer = await con_man.try_connect(info.writerSRs[0], cast_as=fbp_capnp.Channel.Writer)
                port_infos_reader_sr = info.readerSRs[0]
        port_infos_msg.inPorts = inPorts
        port_infos_msg.outPorts = outPorts

        # write the config to the config channel
        await port_infos_writer.write(value=port_infos_msg)

        with open("/home/berg/GitHub/mas-infrastructure/src/python/scratch_code/env_1.json") as _:
            env_json = json.load(_)

        env_writer = await con_man.try_connect(port_srs["in"]["env"], cast_as=fbp_capnp.Channel.Writer)
        env = model_capnp.Env.new_message(rest=common_capnp.StructuredText.new_message(value=json.dumps(env_json),
                                                      structure={"json": None}))
        await env_writer.write(value=fbp_capnp.IP.new_message(content=env))
        print("send env on env channel")

        output_reader = await con_man.try_connect(port_srs["out"]["result"], cast_as=fbp_capnp.Channel.Reader)
        out_msg = await output_reader.read()
        if out_msg.which() == "value":
            out_ip = out_msg.value.as_struct(fbp_capnp.IP)
            c = out_ip.content.as_struct(common_capnp.StructuredText)
            print(json.loads(c.value))
        else:
            print("received done on output channel")

        for channel in channels:
            channel.terminate()
        print(f"{os.path.basename(__file__)}: all channels terminated")

    except Exception as e:
        #for process in process_id_to_process.values():
        #    process.terminate()

        for channel in channels:
            channel.terminate()

        print(f"exception terminated {os.path.basename(__file__)} early. Exception:", e)


if __name__ == '__main__':
    asyncio.run(capnp.run(main(standalone_config)))
