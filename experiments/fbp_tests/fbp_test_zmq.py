import sys
import os
from datetime import date, timedelta
import json
import time
import zmq


def process(config): 
    
    context = zmq.Context()
    in_socket = context.socket(zmq.PULL)
    in_socket.bind("tcp://*:" + config["process_in_port"])
    out_socket = context.socket(zmq.PUSH)
    out_socket.bind("tcp://*:" + config["process_out_port"])
    
    while(True):
        data = in_socket.recv_string()
        #print("received", data)
        out_socket.send_string(data)


def consume(config):

    context = zmq.Context()
    socket = context.socket(zmq.PULL)
    socket.connect("tcp://localhost:" + config["process_out_port"])

    count = 0
    while(True):
        data = socket.recv_string()
        if count == 0:
            start = time.time()

        count = count + 1
        if count % 1000 == 0:
            print(data, "count:", count)

        if count == 100000:
            print("received", count, "messages in", time.time()-start, "s")

def produce(config):

    context = zmq.Context()
    socket = context.socket(zmq.PUSH)
    socket.connect("tcp://localhost:" + config["process_in_port"])
    
    start = time.time()

    outer = 1000
    sub = 100
    for i in range(outer*sub):
        socket.send_string("data " + str(i))
        #print("sent","data " + str(i))

    end = time.time()
    print("sent", outer*sub, "messages in", end-start, "s")


def main():

    # start = [Process, Consumer, Producer]
    config = {
        "process_in_port": "10011",
        "process_out_port": "10012",
        "server": "*",
        "start": "Process"
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v

    cs = config["start"]
    if cs == "Process":
        process(config)
    elif cs == "Consumer":
        consume(config)
    elif cs == "Producer":
        produce(config)

if __name__ == '__main__':
    main()
