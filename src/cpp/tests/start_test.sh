#!/bin/bash

to=$1
#writers=`seq -s, $to`

# start channel
#_cmake_linux_release/channel -h10.10.24.218 -p5555 -rin -w$writers &
#chan_pid=$!
#echo "chan pid: " $chan_pid

for i in `seq $to`
do 
	#echo $i
	_cmake_linux_release/test_writer capnp://insecure@10.10.24.218:5555/$i &
	#_cmake_linux_debug/test_writer $sr/$i &
done

# start reader
#time _cmake_linux_release/test_reader capnp://insecure@10.10.24.218:5555/in

#kill $chan_pid