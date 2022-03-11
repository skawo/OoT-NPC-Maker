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

CFLAGS = -G 0 $(OPTIMIZATION) -I/opt/n64/mips64/include/z64hdr -I/opt/n64/mips64/include/z64hdr/include --std=gnu99 -mtune=vr4300 -mabi=32 -mips3 -fno-builtin -mno-explicit-relocs -mno-memcpy -mno-check-zero-division -Wmissing-field-initializers -Wall -Wno-unknown-pragmas
NOVLFLAGS = -c -A $(ADDRESS) -o $(OVLOUT)
DEFINES = -DGAME_VERSION=$(GAME_VERSION) -DLOGGING=$(LOGGING) -DCOLLISION_VIEWER=$(COLLISION_VIEWER) -DDEBUG_STRUCT=$(DEBUG_STRUCT) -DZZROMTOOL=$(ZZROMTOOL) -DLOG_VERSION=$(LOG_VERSION) -DEXDLIST_EDITOR=$(EXDLIST_EDITOR) -DWIIVC_COMPAT=$(WIIVC_COMPAT)

default: debug

debug: LOGGING = 1
debug: LOG_VERSION = 1
debug: DEBUG_STRUCT = 1
debug: EXDLIST_EDITOR = 1
debug: ZZROMTOOL = 1
debug: COLLISION_VIEWER = 1
debug: GAME_VERSION = 0 
debug: WIIVC_COMPAT = 0
debug: OPTIMIZATION = -Os
debug: LDFLAGS = -L/opt/n64/mips64/include/z64hdr/common -L/opt/n64/mips64/include/z64hdr/oot_mq_debug/ -T /opt/n64/mips64/include/z64hdr/oot_mq_debug/z64hdr.ld --emit-relocs
debug: create_dir $(PROGRAM).ovl

debugVC: LOGGING = 1
debugVC: LOG_VERSION = 1
debugVC: DEBUG_STRUCT = 1
debugVC: EXDLIST_EDITOR = 1
debugVC: ZZROMTOOL = 1
debugVC: COLLISION_VIEWER = 1
debugVC: GAME_VERSION = 0 
debugVC: WIIVC_COMPAT = 1
debugVC: OPTIMIZATION = -O1
debugVC: LDFLAGS = -L/opt/n64/mips64/include/z64hdr/common -L/opt/n64/mips64/include/z64hdr/oot_mq_debug/ -T /opt/n64/mips64/include/z64hdr/oot_mq_debug/z64hdr.ld --emit-relocs
debugVC: create_dir $(PROGRAM).ovl

10: LOGGING = 0
10: LOG_VERSION = 0
10: DEBUG_STRUCT = 0
10: EXDLIST_EDITOR = 0
10: ZZROMTOOL = 0
10: COLLISION_VIEWER = 1
10: GAME_VERSION = 1 
10: WIIVC_COMPAT = 0
10: OPTIMIZATION = -Os
10: LDFLAGS = -L/opt/n64/mips64/include/z64hdr/common -L/opt/n64/mips64/include/z64hdr/oot_u10/ -T /opt/n64/mips64/include/z64hdr/oot_u10/z64hdr.ld --emit-relocs
10: create_dir $(PROGRAM).ovl

10VC: LOGGING = 0
10VC: LOG_VERSION = 0
10VC: DEBUG_STRUCT = 0
10VC: EXDLIST_EDITOR = 0
10VC: ZZROMTOOL = 0
10VC: COLLISION_VIEWER = 1
10VC: GAME_VERSION = 1 
10VC: WIIVC_COMPAT = 1
10VC: OPTIMIZATION = -O1
10VC: LDFLAGS = -L/opt/n64/mips64/include/z64hdr/common -L/opt/n64/mips64/include/z64hdr/oot_u10/ -T /opt/n64/mips64/include/z64hdr/oot_u10/z64hdr.ld --emit-relocs
10VC: create_dir $(PROGRAM).ovl

10D: LOGGING = 0
10D: LOG_VERSION = 1
10D: DEBUG_STRUCT = 1
10D: EXDLIST_EDITOR = 1
10D: ZZROMTOOL = 0
10D: COLLISION_VIEWER = 1
10D: GAME_VERSION = 1 
10D: WIIVC_COMPAT = 0
10D: OPTIMIZATION = -Os
10D: LDFLAGS = -L/opt/n64/mips64/include/z64hdr/common -L/opt/n64/mips64/include/z64hdr/oot_u10/ -T /opt/n64/mips64/include/z64hdr/oot_u10/z64hdr.ld --emit-relocs
10D: create_dir $(PROGRAM).ovl

10DVC: LOGGING = 0
10DVC: LOG_VERSION = 1
10DVC: DEBUG_STRUCT = 1
10DVC: EXDLIST_EDITOR = 1
10DVC: ZZROMTOOL = 0
10DVC: COLLISION_VIEWER = 1
10DVC: GAME_VERSION = 1 
10DVC: WIIVC_COMPAT = 1
10DVC: OPTIMIZATION = -O1
10DVC: LDFLAGS = -L/opt/n64/mips64/include/z64hdr/common -L/opt/n64/mips64/include/z64hdr/oot_u10/ -T /opt/n64/mips64/include/z64hdr/oot_u10/z64hdr.ld --emit-relocs
10D: create_dir $(PROGRAM).ovl


clean: 
	rm -fv $(ELF) $(OVLOUT) $(OBJECTS)
	



create_dir:
	@mkdir -p $(BUILDDIR)
	
$(PROGRAM).ovl: $(PARTS)
	$(LD) -o $(ELF) $(OBJECTS) $(LDFLAGS)
	$(NOVL) $(NOVLFLAGS) $(BUILDDIR)/$(PROGRAM).elf
	
%.o: %.c
	$(CC) -c $(CFLAGS) $< $(DEFINES) -o $(BUILDDIR)/$(notdir $@)