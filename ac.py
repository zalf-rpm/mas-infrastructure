import capnp
import a_capnp

a_cap = capnp.TwoPartyClient("localhost:11111").bootstrap().cast_as(a_capnp.A)
txt = a_cap.method().wait().res
print(txt)