import zmq

context = zmq.Context()
socket = context.socket(zmq.PULL)
socket.connect("tcp://localhost:7777")

i = 0
while True:
    msg = socket.recv_string() #encoding="latin-1"
    if msg == "done":
        break
    if i % 10000 == 0:
        print(".", flush=True, end="")
    i += 1
