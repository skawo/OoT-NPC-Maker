@echo off
cd .\gcc\bin\
mips64-gcc -I "../mips64/include/z64hdr/oot_mq_debug" -I "../mips64/include/z64hdr/include/" -G 0 -O1 -fno-reorder-blocks -std=gnu99 -mtune=vr4300 -march=vr4300 -mabi=32 -c -mips3 -mno-explicit-relocs -mno-memcpy -mno-check-zero-division "../../test.c"
cd ..\mips64\include\z64hdr\
copy ..\..\..\bin\conf.ld entry.ld
..\..\..\bin\mips64-ld -L "..\..\..\..\gcc\mips64\include\z64hdr\common" -L "..\..\..\..\gcc\mips64\include\z64hdr\oot_mq_debug" -T syms.ld -T z64hdr_actor.ld --emit-relocs -o "../../../bin/test.elf" "../../../bin/test.o" 
cd ..\..\..\..\
.\gcc\bin\mips64-objdump -t ".\gcc\bin\test.o" | findstr /I "constructor destructor init update main dest draw init_vars initvars" >> output.txt
ls
nOVL\novl -vv -c -A 0x80800000 -o "test.ovl" "gcc/bin/test.elf" 
del ".\gcc\bin\test.elf"
del ".\gcc\bin\test.o"
echo done, output "test.ovl"