import capnp
import capnproto_schemas.a_capnp as a_capnp

a_cap = capnp.TwoPartyClient("localhost:11111").bootstrap().cast_as(a_capnp.A)
txt = a_cap.method("______________PARAM______________").wait().res
print(txt)