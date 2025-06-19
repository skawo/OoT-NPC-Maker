#ifndef NPC_MAKER_H
#define NPC_MAKER_H

#define MAJOR_VERSION 0
#define MINOR_VERSION 617

#ifndef GAME_VERSION
    #define GAME_VERSION 0
#endif

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
    #include <z64hdr/oot_mq_debug/z64hdr.h>
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
    #include <z64hdr/oot_u10/z64hdr.h>
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

