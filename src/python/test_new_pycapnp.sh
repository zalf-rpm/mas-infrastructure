#!/bin/bash

for i in {1..1}
do
  echo "round $i"
  for _ in {1..900}
  do
    /home/berg/miniconda3/envs/new_pycapnp_test/bin/python client_test.py &
  done
  #sleep 10
done