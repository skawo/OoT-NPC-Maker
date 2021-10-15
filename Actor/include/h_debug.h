#ifndef H_DEBUG_H
#define H_DEBUG_H

#include "npc_maker_types.h"
 
ReadableTime Time_Convert(u16 time);
void printf_IS_RAM(NpcMaker* en, const char* fmt, ...);

#endif