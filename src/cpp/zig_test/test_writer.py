import zmq

context = zmq.Context()
socket = context.socket(zmq.PUSH) # pylint: disable=no-member
socket.connect("tcp://localhost:6666")

for i in range(1, 1000000):
    socket.send_string("Hello_"+str(i))

socket.send_string("done")
