BUILDDIR = build
SOURCEDIR = source

PROGRAM = npc_maker
OVLOUT = actor.zovl
ADDRESS = 0x80800000

ELF = $(BUILDDIR)/$(PROGRAM).elf
SRCS := $(shell find $(SOURCEDIR) -name '*.c')
PARTS:=$(SRCS:.c=.o)
OBJECTS := $(addprefix $(BUILDDIR)/,$(notdir $(PARTS)))

CC   = mips64-gcc
LD   = mips64-ld
NOVL = novl

CFLAGS = -G 0 -Os -I/opt/n64/mips64/include/z64hdr -I/opt/n64/mips64/include/z64hdr/include --std=gnu99 -mtune=vr4300 -mabi=32 -mips3 -fno-builtin -mno-explicit-relocs -mno-memcpy -mno-check-zero-division -Wmissing-field-initializers -Wall -Wno-unknown-pragmas
NOVLFLAGS = -c -A $(ADDRESS) -o $(OVLOUT)
DEFINES = -DGAME_VERSION=$(GAME_VERSION) -DLOGGING=$(LOGGING) -DCOLLISION_VIEWER=$(COLLISION_VIEWER) -DLOG_TO_SCREEN=$(LOG_TO_SCREEN) -DZZROMTOOL=$(ZZROMTOOL)

default: debug

debug: LOGGING = 1
debug: LOG_TO_SCREEN = 1
debug: ZZROMTOOL = 1
debug: COLLISION_VIEWER = 1
debug: GAME_VERSION = 0 
debug: LDFLAGS = -L/opt/n64/mips64/include/z64hdr/common -L/opt/n64/mips64/include/z64hdr/oot_mq_debug/ -T /opt/n64/mips64/include/z64hdr/oot_mq_debug/z64hdr.ld --emit-relocs
debug: create_dir $(PROGRAM).ovl

10: LOG_TO_SCREEN = 0
10: ZZROMTOOL = 0
10: COLLISION_VIEWER = 1
10: LOGGING = 0
10: GAME_VERSION = 1 
10: LDFLAGS = -L/opt/n64/mips64/include/z64hdr/common -L/opt/n64/mips64/include/z64hdr/oot_u10/ -T /opt/n64/mips64/include/z64hdr/oot_u10/z64hdr.ld --emit-relocs
10: create_dir $(PROGRAM).ovl

clean: 
	rm -fv $(ELF) $(OVLOUT) $(OBJECTS)
	



create_dir:
	@mkdir -p $(BUILDDIR)
	
$(PROGRAM).ovl: $(PARTS)
	$(LD) -o $(ELF) $(OBJECTS) $(LDFLAGS)
	$(NOVL) $(NOVLFLAGS) $(BUILDDIR)/$(PROGRAM).elf
	
%.o: %.c
	$(CC) -c $(CFLAGS) $< $(DEFINES) -o $(BUILDDIR)/$(notdir $@)