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
import capnp
import os
from pathlib import Path
import subprocess as sp
import sys
import uuid
from zalfmas_common import common
import zalfmas_capnp_schemas
from zalfmas_fbp.run import channels as chans, ports as fbp_ports

capnp_path = Path(os.path.dirname(zalfmas_capnp_schemas.__file__))
sys.path.append(str(capnp_path))
import common_capnp
import fbp_capnp

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
                                            "in_out|" + first_writer_sr, name="in_out"))
        channels.append(chans.start_channel(config["path_to_channel"],
                                            "port_infos_up|" + first_writer_sr, name="port_infos_up",
                                            port=9991,
                                            reader_srts="r_in"))
        channels.append(chans.start_channel(config["path_to_channel"],
                                            "port_infos_down|" + first_writer_sr, name="port_infos_down",
                                            port=9992,
                                            reader_srts="r_in"))

        port_infos_writer_up = port_infos_writer_down = None
        in_ports = []
        out_ports = []
        for i in range(3):
            p = (await first_reader.read()).value.as_struct(common_capnp.Pair)
            c_id = p.fst.as_text()
            info = p.snd.as_struct(fbp_capnp.Channel.StartupInfo)
            print("channel:", c_id, "reader_sr:", info.readerSRs[0], "writer_sr:", info.writerSRs[0])
            if c_id == "in_out":
                in_ports.append({"name": "in", "sr": info.readerSRs[0]})
                out_ports.append({"name": "out", "sr": info.writerSRs[0]})
            elif c_id == "port_infos_up":
                port_infos_writer_up = await con_man.try_connect(info.writerSRs[0], cast_as=fbp_capnp.Channel.Writer)
            elif c_id == "port_infos_down":
                port_infos_writer_down = await con_man.try_connect(info.writerSRs[0], cast_as=fbp_capnp.Channel.Writer)

        # write the config to the config channel
        await port_infos_writer_up.write(value=fbp_capnp.PortInfos.new_message(outPorts=out_ports))
        await port_infos_writer_down.write(value=fbp_capnp.PortInfos.new_message(inPorts=in_ports))

        # wait for the channels to finish
        read_file_component = await con_man.try_connect("capnp://localhost:5111/read_file", cast_as=fbp_capnp.Component)
        success = await read_file_component.start("capnp://10.10.24.203:9991/r_in")

        console_output_component = await con_man.try_connect("capnp://localhost:5112/console_output", cast_as=fbp_capnp.Component)
        success = await console_output_component.start("capnp://10.10.24.203:9992/r_in")

        channels[0].wait()

    except Exception as e:
        #for process in process_id_to_process.values():
        #    process.terminate()

        for channel in channels:
            channel.terminate()

        print(f"exception terminated {os.path.basename(__file__)} early. Exception:", e)

    print("finished")

if __name__ == '__main__':
    asyncio.run(capnp.run(main(standalone_config)))
