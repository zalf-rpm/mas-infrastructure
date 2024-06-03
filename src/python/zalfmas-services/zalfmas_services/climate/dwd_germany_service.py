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
# This file has been created at the Institute of
# Landscape Systems Analysis at the ZALF.
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)

import asyncio
import capnp
import os
from pathlib import Path
import sys

from zalfmas_common.climate import common_climate_data_capnp_impl as ccdi
from zalfmas_common import common
from zalfmas_common import service as serv
from zalfmas_common.climate import csv_file_based as csv_based
from zalfmas_common import fbp
import zalfmas_capnpschemas

sys.path.append(os.path.dirname(zalfmas_capnpschemas.__file__))
import climate_capnp
import common_capnp
import fbp_capnp
import geo_capnp
import registry_capnp as reg_capnp


async def fbp_component(config: dict, service: ccdi.Service):
    ports, close_out_ports = await fbp.connect_ports(config)
    mode = config["mode"]

    def iso_to_cdate(iso_date_str):
        ds = iso_date_str.split("-")
        return {"year": int(ds[0]), "month": int(ds[1]), "day": int(ds[2])}

    dataset: csv_based.Dataset = (await service.getAvailableDatasets()).datasets[0].data

    while ports["inp"] and ports["outp"] and service:
        try:
            in_msg = await ports["inp"].read()
            if in_msg.which() == "done":
                ports["inp"] = None
                continue

            in_ip = in_msg.value.as_struct(fbp_capnp.IP)
            attr = common.get_fbp_attr(in_ip, config["latlon_attr"])
            if attr:
                coord = attr.as_struct(geo_capnp.LatLonCoord)
            else:
                coord = in_ip.content.as_struct(geo_capnp.LatLonCoord)
            start_date = common.get_fbp_attr(in_ip, config["start_date_attr"]).as_text()
            end_date = common.get_fbp_attr(in_ip, config["end_date_attr"]).as_text()

            timeseries_p: csv_based.TimeSeries = dataset.closestTimeSeriesAt(coord).timeSeries
            timeseries = await (timeseries_p.subrange(iso_to_cdate(start_date),
                                                      iso_to_cdate(end_date))).timeSeries

            res = timeseries
            if mode == "sturdyref":
                res = await timeseries.save()
            elif mode == "capability":
                res = timeseries
            elif mode == "data":
                res = climate_capnp.TimeSeriesData.new_message()
                res.isTransposed = False
                header = timeseries.header().header
                se_date = timeseries.range()
                resolution = timeseries.resolution().resolution
                res.data = (await timeseries.data()).data
                res.header = header
                se_date = se_date
                res.startDate = se_date.startDate
                res.endDate = se_date.endDate
                res.resolution = resolution

            # print(res.data().wait())
            out_ip = fbp_capnp.IP.new_message()
            if not config["to_attr"]:
                out_ip.content = res
            common.copy_and_set_fbp_attrs(in_ip, out_ip, **({config["to_attr"]: res} if config["to_attr"] else {}))
            await ports["outp"].write(value=out_ip)

        except Exception as e:
            print(f"{os.path.basename(__file__)} Exception:", e)

    await close_out_ports()
    print(f"{os.path.basename(__file__)}: process finished")


def create_meta_plus_datasets(path_to_data_dir, interpolator, rowcol_to_latlon, restorer):
    datasets = []
    metadata = climate_capnp.Metadata.new_message(
        entries=[
            {"historical": None},
            {"start": {"year": 1990, "month": 1, "day": 1}},
            {"end": {"year": 2019, "month": 12, "day": 31}}
        ]
    )
    metadata.info = ccdi.MetadataInfo(metadata)
    datasets.append(climate_capnp.MetaPlusData.new_message(
        meta=metadata,
        data=csv_based.Dataset(metadata, path_to_data_dir, interpolator, rowcol_to_latlon,
                               header_map={"windspeed": "wind"},
                               row_col_pattern="row-{row}/col-{col}.csv",
                               name="DWD Germany 1991-2019",
                               description="ZALF DWD Germany data from 1991-2019 in MONICA CSV format.",
                               restorer=restorer)
    ))
    return datasets


async def main(path_to_data, serve_bootstrap=True, host=None, port=None,
               id=None, name="DWD - historical - 1991-2019", description=None, srt=None):
    config = {
        "path_to_data": path_to_data,
        "port": port,
        "host": host,
        "id": id,
        "name": name,
        "description": description,
        "serve_bootstrap": serve_bootstrap,
        "in_sr": None,
        "out_sr": None,
        "fbp": True,
        "to_attr": None,  # "climate",
        "latlon_attr": "latlon",
        "start_date_attr": "startDate",
        "end_date_attr": "endDate",
        "mode": "sturdyref",  # sturdyref | capability | data
        "srt": srt
    }
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    restorer = common.Restorer()
    interpolator, rowcol_to_latlon = ccdi.create_lat_lon_interpolator_from_json_coords_file(
        config["path_to_data"] + "/" + "latlon-to-rowcol.json")
    meta_plus_data = create_meta_plus_datasets(config["path_to_data"] + "/germany", interpolator, rowcol_to_latlon,
                                               restorer)
    service = ccdi.Service(meta_plus_data, id=config["id"], name=config["name"], description=config["description"],
                           restorer=restorer)
    if config["fbp"]:
        await fbp_component(config, climate_capnp.Service._new_client(service))
    else:
        await serv.init_and_run_service({"service": service}, config["host"], config["port"],
                                        serve_bootstrap=config["serve_bootstrap"],
                                        name_to_service_srs={"service": config["srt"]},
                                        restorer=restorer)


if __name__ == '__main__':
    asyncio.run(capnp.run(main(
        "/run/user/1000/gvfs/sftp:host=login01.cluster.zalf.de,user=rpm/beegfs/common/data/climate/dwd/csvs")))
