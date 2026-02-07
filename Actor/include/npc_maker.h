#ifndef NPC_MAKER_H
#define NPC_MAKER_H

#define MAJOR_VERSION 1
#define MINOR_VERSION 0

#ifndef GAME_VERSION
    #define GAME_VERSION 0
#endif

#define GBI_GLANKK 1

#include "zocarina.h"
#include "game.h"
#include "actor.h"
#include "play_state.h"
#include "gfx.h"
#include "skin.h"
#include "sys_matrix.h"
#include "libu64/gfxprint.h"
#include "gfxalloc.h"
#include "z_lib.h"
#include "olib.h"
#include "ultra64/gbi.h"
#include "player.h"
#include "segmented_address.h"
#include "gfx_setupdl.h"
#include "libu64/overlay.h"
#include "zelda_arena.h"
#include "array_count.h"
#include "ocarina.h"
#include "sequence.h"
#include "regs.h"
#include "quake.h"
#include "rumble.h"
#include "effect.h"
#include "controller.h"
#include "libc64/qrand.h"
#include "save.h"
#include "audio.h"
#include "assets/objects/gameplay_keep/gameplay_keep.h"

#if GAME_VERSION == 0
    #ifndef LOGGING
        #define LOGGING 1
    #endif
    #ifndef DEBUG_STRUCT
        #define DEBUG_STRUCT 1
    #endif
    #ifndef LOG_VERSION
        #define LOG_VERSION 1
    #endif
#endif

#if GAME_VERSION == 1
    #ifndef LOGGING
        #define LOGGING 0
    #endif
    #ifndef DEBUG_STRUCT
        #define DEBUG_STRUCT 0
    #endif    
    #ifndef LOG_VERSION
        #define LOG_VERSION 0
    #endif    
#endif

#ifndef COLLISION_VIEWER
    #define COLLISION_VIEWER 1
#endif

#ifndef EXDLIST_EDITOR
    #define EXDLIST_EDITOR 1
#endif

#ifndef LOOKAT_EDITOR
    #define LOOKAT_EDITOR 1
#endif

#ifndef DIRECT_ROM_LOAD
    #define DIRECT_ROM_LOAD 0
#endif

#ifndef ZZROMTOOL
    #define ZZROMTOOL 1
#endif

#endif

