#!/bin/sh
make clean
make -j 
echo y | wine "../../zzromtool.exe" "../../project.zzrp"
