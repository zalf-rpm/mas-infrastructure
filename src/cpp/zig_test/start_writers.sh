#!/bin/bash

sr=$1
to=$2
for i in `seq $to`
do 
	#echo $i
	common/_cmake_release/test_writer -c100000 $sr/$i &
done

