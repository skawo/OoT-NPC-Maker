#ifndef H_ROM_H
#define H_ROM_H

#include "npc_maker_types.h"

extern RomFile* objectTable;

#if ZZROMTOOL == 1
    #define dmaData 0x800097E0
#else
    #define dmaData gDmaDataTable   
#endif 

#if ZZROMTOOL == 1
    #define objectTable (*(RomFile(*)[]) 0x801281C0)
#else
    #ifdef OOT_MQ_DEBUG_PAL
        #define objectTable (*(RomFile(*)[]) gObjectTable) 
    #else
        #define objectTable (*(RomFile(*)[]) 0x800F8FF8)
    #endif
#endif 

#ifdef OOT_MQ_DEBUG_PAL
    #define messageTable (*(MessageEntry(*)[]) 0x8014B320)
#else
    #define messageTable (*(MessageEntry(*)[]) 0x8010EA8C)
#endif


void Rom_LoadObject(int objId, void *dram_addr);
void Rom_LoadDataFromObjectFromROM(int objId, void* dram_addr, u32 offset_into_file, size_t size);
s32 Rom_LoadObjectIfUnloaded(PlayState* playState, s16 object_id);
bool Rom_SetObjectToActor(Actor* en, PlayState*playState, u16 object, s32 fileStart);
void* Rom_GetObjectDataPtr(u16 object_id, PlayState*playState);
MessageEntry* Rom_GetMessageEntry(s16 msg_id);
InternalMsgEntry Data_GetCustomMessage(NpcMaker* en, PlayState* playState, int id);
bool Rom_IsObjectCompressed(int objId);
RomFile* Rom_GetObjectVROMAddr(int objId);
RomSection Rom_GetPhysicalROMAddrFromVirtual(u32 virtual);
void Rom_LoadDataFromObject(PlayState* playState, int objId, void* dram_addr, u32 offset_into_file, size_t size, bool from_ram);
void Message_Overwrite(NpcMaker* en, PlayState* playState, s16 msgId);
void Message_Get(NpcMaker* en, PlayState* playState, s16 msgId, void* buffer);
void* Message_GetMessageRAMAddr(NpcMaker* en, PlayState* playState, s16 msgId);

#endif