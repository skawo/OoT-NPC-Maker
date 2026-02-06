# ===== Detect OS for BINDIR =====
ifeq ($(OS),Windows_NT)
BINDIR = bin
else
BINDIR = binmono
endif

# ===== Directories =====
OUTDIR    = build
SOURCEDIR = source

# SDKROOT (from environment or default)
SDKROOT   ?= $(OUTDIR)
MIPSROOT   = $(SDKROOT)/mips64
ZOCARINA   = $(MIPSROOT)/include/zocarina

# ===== Program =====
PROGRAM = npc_maker
OVLOUT  = actor.zovl
ADDRESS = 0x80800000
ELF     = $(OUTDIR)/$(PROGRAM).elf

SRCS    := $(wildcard $(SOURCEDIR)/**/*.c) $(wildcard $(SOURCEDIR)/*.c)
PARTS   := $(SRCS:.c=.o)
OBJECTS := $(addprefix $(OUTDIR)/,$(notdir $(PARTS)))

# ===== Tools =====
CC      = $(SDKROOT)/$(BINDIR)/mips64-gcc
CC1_DIR = $(SDKROOT)/$(BINDIR)/
LD      = $(SDKROOT)/$(BINDIR)/mips64-ld
NOVL    = $(SDKROOT)/$(BINDIR)/nOVL

# ===== Flags =====
CFLAGS = -G 0 $(OPTIMIZATION) $(ZOCARINAPATHS) \
    --std=gnu99 -mtune=vr4300 -mabi=32 -mips3 \
    -mno-split-addresses -fno-builtin -fno-reorder-blocks \
    -ffast-math -mno-explicit-relocs -nostdinc \
    -fno-unsafe-math-optimizations -fno-optimize-sibling-calls \
    -mno-memcpy -mno-check-zero-division -fno-common -funsigned-char \
    -Wmissing-field-initializers -Wall -Wno-unknown-pragmas

NOVLFLAGS = -c -A $(ADDRESS) -o $(OVLOUT)

DEFINES = -D_LANGUAGE_C \
          -DPLATFORM_N64=1 \
          -DPLATFORM_GC=0 \
          -DPLATFORM_IQUE=0 \
          -DOOT_VERSION=NTSC_1_0 \
          -DOOT_REVISION=0 \
          -DOOT_REGION=REGION_US \
          -DLIBULTRA_VERSION=LIBULTRA_VERSION_I \
          -DLIBULTRA_PATCH=1 \
          -DDEBUG_FEATURES=0 \
          -DNDEBUG \
          -DF3DEX_GBI_2 \
          -DDIRECT_ROM_LOAD=1 \
          -DGAME_VERSION=$(GAME_VERSION) \
          -DLOOKAT_EDITOR=$(LOOKAT_EDITOR) \
          -DLOGGING=$(LOGGING) \
          -DCOLLISION_VIEWER=$(COLLISION_VIEWER) \
          -DDEBUG_STRUCT=$(DEBUG_STRUCT) \
          -DZZROMTOOL=$(ZZROMTOOL) \
          -DLOG_VERSION=$(LOG_VERSION) \
          -DEXDLIST_EDITOR=$(EXDLIST_EDITOR)

# ===== Default target =====
default: 10

# ===== Debug builds =====
debug: LOGGING = 1
debug: LOG_VERSION = 1
debug: DEBUG_STRUCT = 1
debug: EXDLIST_EDITOR = 1
debug: ZZROMTOOL = 1
debug: COLLISION_VIEWER = 1
debug: GAME_VERSION = 0
debug: LOOKAT_EDITOR = 1
debug: OPTIMIZATION = -Os
debug: ZOCARINAPATHS = -I"$(ZOCARINA)/include" -I"$(ZOCARINA)/include/libc" -I"$(ZOCARINA)/version/oot_mq_debug"
debug: LDFLAGS = -L"$(CURDIR)" -T entry.ld -L"$(ZOCARINA)/version/oot_mq_debug" -T "$(ZOCARINA)/gc-eu-mq-dbg.ld" --emit-relocs
debug: $(OVLOUT)

debugr: LOGGING = 0
debugr: LOG_VERSION = 0
debugr: DEBUG_STRUCT = 0
debugr: EXDLIST_EDITOR = 0
debugr: ZZROMTOOL = 0
debugr: COLLISION_VIEWER = 0
debugr: GAME_VERSION = 0
debugr: LOOKAT_EDITOR = 0
debugr: OPTIMIZATION = -Os
debugr: ZOCARINAPATHS = -I"$(ZOCARINA)/include" -I"$(ZOCARINA)/include/libc" -I"$(ZOCARINA)/version/oot_mq_debug"
debugr: LDFLAGS = -L"$(CURDIR)" -T entry.ld -L"$(ZOCARINA)/version/oot_mq_debug" -T "$(ZOCARINA)/gc-eu-mq-dbg.ld" --emit-relocs
debugr: $(OVLOUT)

# ===== OOT 1.0 =====
10: LOGGING = 0
10: LOG_VERSION = 0
10: DEBUG_STRUCT = 1
10: EXDLIST_EDITOR = 0
10: ZZROMTOOL = 0
10: COLLISION_VIEWER = 0
10: GAME_VERSION = 1
10: LOOKAT_EDITOR = 0
10: OPTIMIZATION = -Os
10: ZOCARINAPATHS = -I"$(ZOCARINA)/include" -I"$(ZOCARINA)/include/libc" -I"$(ZOCARINA)/version/ntsc-1.0"
10: LDFLAGS = -L"$(CURDIR)" -T entry.ld -L"$(ZOCARINA)/version/ntsc-1.0" -T "$(ZOCARINA)/ntsc-1.0.ld" --emit-relocs
10: $(OVLOUT)

# ===== OOT 1.0 Debug =====
10D: LOGGING = 0
10D: LOG_VERSION = 1
10D: DEBUG_STRUCT = 1
10D: EXDLIST_EDITOR = 1
10D: ZZROMTOOL = 0
10D: COLLISION_VIEWER = 1
10D: GAME_VERSION = 1
10D: LOOKAT_EDITOR = 1
10D: OPTIMIZATION = -Os
10D: ZOCARINAPATHS = -I"$(ZOCARINA)/include" -I"$(ZOCARINA)/include/libc" -I"$(ZOCARINA)/version/ntsc-1.0"
10D: LDFLAGS = -L"$(CURDIR)" -T entry.ld -L"$(ZOCARINA)/version/ntsc-1.0" -T "$(ZOCARINA)/ntsc-1.0.ld" --emit-relocs
10D: $(OVLOUT)

# ===== OOT 1.0 Debug + Logging =====
10DLOG: LOGGING = 1
10DLOG: LOG_VERSION = 1
10DLOG: DEBUG_STRUCT = 1
10DLOG: EXDLIST_EDITOR = 1
10DLOG: ZZROMTOOL = 0
10DLOG: COLLISION_VIEWER = 1
10DLOG: GAME_VERSION = 1
10DLOG: LOOKAT_EDITOR = 1
10DLOG: OPTIMIZATION = -Os
10DLOG: ZOCARINAPATHS = -I"$(ZOCARINA)/include" -I"$(ZOCARINA)/include/libc" -I"$(ZOCARINA)/version/ntsc-1.0"
10DLOG: LDFLAGS = -L"$(CURDIR)" -T entry.ld -L"$(ZOCARINA)/version/ntsc-1.0" -T "$(ZOCARINA)/ntsc-1.0.ld" --emit-relocs
10DLOG: $(OVLOUT)

# ===== Build rules =====
$(OUTDIR):
	@mkdir -p "$@"

$(OVLOUT): $(ELF)
	@ "$(NOVL)" $(NOVLFLAGS) "$(ELF)"

$(ELF): $(OBJECTS) | $(OUTDIR)
	@ "$(LD)" -o "$@" $(OBJECTS) $(LDFLAGS)

$(OUTDIR)/%.o: $(SOURCEDIR)/%.c | $(OUTDIR)
	@ "$(CC)" -c -B"$(CC1_DIR)" $(CFLAGS) $(DEFINES) "$<" -o "$@"

# ===== Clean =====
clean:
	@ rm -f "$(ELF)" "$(OVLOUT)" $(OBJECTS)

.PHONY: default debug debugr 10 10D 10DLOG clean
