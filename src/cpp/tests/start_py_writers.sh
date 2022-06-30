#!/bin/bash

to=$1
for i in `seq $to`
do 
	#echo $i
	python test_writer.py &
	#_cmake_linux_debug/test_writer $sr/$i &
done

