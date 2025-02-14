import asyncio
import random

import capnp
import os
import sys
import time
from zalfmas_common import common
import a_capnp
import zalfmas_capnp_schemas
sys.path.append(os.path.dirname(zalfmas_capnp_schemas.__file__))
import common_capnp
import model_capnp
import climate_capnp
import soil_capnp
import x_capnp


async def main_():
    conman = common.ConnectionManager()
    #soil_service = await conman.connect("capnp://localhost:9981/buek200", cast_as=soil_capnp.Service, retry_secs=1)
    monica_service = await conman.connect("capnp://localhost:9920/monica", cast_as=model_capnp.EnvInstance)
    time_series = await conman.connect("capnp://localhost:9991/klotzsche", cast_as=climate_capnp.TimeSeries)

    with open("env.json", "r") as _:
        env_t_str = _.read()

    rr = monica_service.run_request()
    env = rr.env
    rr.env.timeSeries = time_series
    # rr.env.soilProfile = wst_sr_to_caps[var["sr"]]["soil_profile"]
    rest = env.rest.as_struct(common_capnp.StructuredText)
    rest.value = env_t_str
    rest.structure.json = None
    # rr.env.rest = common_capnp.StructuredText.new_message(value=json.dumps(env_template),
    #                                                   structure={"json": None})
    res = await rr.send()
    st = res.result.as_struct(common_capnp.StructuredText)
    print(st.value, "len(st.value):", len(st.value))
    pass


async def main():
    con_man = common.ConnectionManager()
    if False:
        y = await con_man.try_connect("capnp://localhost:9920/monica", x_capnp.Y, retry_secs=1)
        for i in range(100):
            req = y.m_request()
            r = random.random()
            time.sleep(r)
            req.hello = f"Michael {i}"
            print("r:", r, "i:", i, flush=True)
            await req.send()
        #await y.m("Michael")
        #st = res.result.as_struct(common_capnp.StructuredText)
        return

    m = await con_man.try_connect("capnp://localhost:9920/monica", model_capnp.EnvInstance, retry_secs=1)
    ts = await con_man.try_connect("capnp://localhost:9991/klotzsche", climate_capnp.TimeSeries)
    s = await con_man.try_connect("capnp://localhost:9981/buek200", soil_capnp.Service)
    id = (await s.info()).id
    print("soil id:", id, flush=True)
    with open("env.json", "r") as _:
        env_str = _.read()
    for i in range(100):
        req = m.run_request()
        r = random.random()
        time.sleep(r)
        print("r:", r, "i:", i, flush=True)
        env = req.env
        env.timeSeries = ts
        #env.soilProfile = s
        #rest = env.init("rest").as_struct(common_capnp.StructuredText)
        rest = env.rest.as_struct(common_capnp.StructuredText)
        rest.value = env_str
        rest.structure.json = None
        #req.env.rest = common_capnp.StructuredText.new_message(value="blablabla", structure={"json": None})
        #req.param.rest.structure.json = None
        res = await req.send()
        #res = await m.run({"rest": common_capnp.StructuredText.new_message(value="blabla")})
        st = res.result.as_struct(common_capnp.StructuredText)
        stv = st.value
        print("i:", i, "len(res):", len(stv), flush=True)

if __name__ == '__main__':
    asyncio.run(capnp.run(main()))