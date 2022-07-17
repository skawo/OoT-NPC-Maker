#ifndef H_ROM_H
#define H_ROM_H

#include "npc_maker_types.h"

extern RomFile* objectTable;

#ifdef ZZROMTOOL
    #define dmaData 0x800097E0
#else
    #define dmaData gDmaDataTable   
#endif 

#ifdef ZZROMTOOL
    #define objectTable (*(RomSection(*)[]) 0x801281C0)
#else
    #define objectTable (*(RomSection(*)[]) gObjectTable) 
#endif 

#ifdef _Z64HDR_MQ_DEBUG_
    #define messageTable (*(MessageEntry(*)[]) 0x8014B320)
#endif

#ifdef _Z64HDR_U10_
    #define messageTable (*(MessageEntry(*)[]) 0x8010EA8C)
#endif


void Rom_LoadObject(int objId, void *dram_addr);
void Rom_LoadDataFromObjectFromROM(int objId, void* dram_addr, u32 offset_into_file, size_t size);
void Rom_LoadObjectIfUnloaded(GlobalContext* global, s16 object_id);
bool Rom_SetObjectToActor(Actor* en, GlobalContext*global, u16 object, s32 fileStart);
void* Rom_GetObjectDataPtr(u16 object_id, GlobalContext*global);
MessageEntry* Rom_GetMessageEntry(s16 msg_id);
InternalMsgEntry Data_GetCustomMessage(NpcMaker* en, GlobalContext* global, int id);
bool Rom_IsObjectCompressed(int objId);
RomSection Rom_GetObjectROMAddr(int objId);
RomSection Rom_GetPhysicalROMAddrFromVirtual(u32 virtual);
void Rom_LoadDataFromObject(GlobalContext* global, int objId, void* dram_addr, u32 offset_into_file, size_t size, bool from_ram);
void Message_Overwrite(NpcMaker* en, GlobalContext* globalCtx, s16 msgId);

#endif