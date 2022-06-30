#!/bin/bash

sr=$1
to=$2
for i in `seq $to`
do 
	#echo $i
	_cmake_linux_release/test_writer -c100000 $sr/$i &
	#_cmake_linux_debug/test_writer $sr/$i &
done

