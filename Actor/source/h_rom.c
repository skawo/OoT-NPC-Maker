#include "../include/h_rom.h"

inline bool Rom_IsObjectCompressed(int objId)
{
    return Rom_GetObjectROMAddr(objId).End != 0;
}

RomSection Rom_GetObjectROMAddr(int objId)
{
    RomSection* obj = &objectTable[objId];
    return Rom_GetPhysicalROMAddrFromVirtual(obj->Start);
}

RomSection Rom_GetPhysicalROMAddrFromVirtual(u32 virtual)
{
    struct 
    {
        u32 Vstart;
        u32 Vend;
        u32 Pstart;
        u32 Pend;
    } *dma;

    RomSection out = { .Start = 0xFFFFFFFF, .End = 0xFFFFFFFF};
    
    for (dma = (void*)dmaData; ; ++dma)
    {
        if (dma->Vstart == virtual)
        {
            out.Start = dma->Pstart;
            out.End = dma->Pend;
            break;
        }
    }

    return out;
}

// Loads data from given object directly from the cartridge (if it's not compressed!)
void Rom_LoadObject(int objId, void *dest)
{
    RomSection obj = Rom_GetObjectROMAddr(objId);

    #ifdef OOT_MQ_DEBUG_PAL
        DmaMgr_SendRequest1(dest, obj.Start, obj.End - obj.Start, "", __LINE__);
    #else
        DmaMgr_SendRequest1(dest, obj.Start, obj.End - obj.Start);
    #endif
}

// Loads data from given object directly from the cartridge (if it's not compressed!)
void Rom_LoadDataFromObjectFromROM(int objId, void* dest, u32 fileOffs, size_t size)
{
    RomSection obj = Rom_GetObjectROMAddr(objId);

    if (obj.Start == 0)
    {
        #if LOGGING == 1
            osSyncPrintf("_Object %4d was not found!", objId);
        #endif       

        return;
    }

    if (size > obj.End - (obj.Start + fileOffs))
        size = obj.End - (obj.Start + fileOffs);

    u32 start = obj.Start + fileOffs;

    #if LOGGING == 1
        osSyncPrintf("_Loading 0x%08x bytes from ROM at 0x%08x", size, start);
    #endif    

    #ifdef OOT_MQ_DEBUG_PAL
        DmaMgr_SendRequest1(dest, start, size, "", __LINE__);
    #else
        DmaMgr_SendRequest1(dest, start, size);
    #endif

}

void Rom_LoadDataFromObject(PlayState* playState, int objId, void* dest, u32 fileOffs, size_t size, bool fromRam)
{
    if (fromRam)
    {
        void* ptr = Rom_GetObjectDataPtr(objId, playState);

        if (ptr == NULL)
        {
            #if LOGGING == 1
                osSyncPrintf("_The file to load from was not found in RAM!");
            #endif   

            return;
        }

        bcopy(AADDR(ptr, fileOffs), dest, size);
    }
    else
        Rom_LoadDataFromObjectFromROM(objId, dest, fileOffs, size);
}

void Rom_LoadObjectIfUnloaded(PlayState* playState, s16 objId)
{
    if (objId < 0)
        return;

    #if LOGGING == 1
        osSyncPrintf("_Loading object %4d...", objId);
    #endif   

    if (!Object_IsLoaded(&playState->objectCtx, Object_GetIndex(&playState->objectCtx, objId)))
        Object_Spawn(&playState->objectCtx, objId);
    else
    {
        #if LOGGING == 1
            osSyncPrintf("_It's already loaded.");
        #endif         
    }
}

bool Rom_SetObjectToActor(Actor* en, PlayState* playState, u16 object, s32 fileStart)
{
    int bankIndex = Object_GetIndex(&playState->objectCtx, object);

    if (Object_IsLoaded(&playState->objectCtx, bankIndex))
    {
        en->objBankIndex = bankIndex;
        gSegments[6] = VIRTUAL_TO_PHYSICAL(playState->objectCtx.status[en->objBankIndex].segment) + fileStart;
        return true;
    }
    else
        return false;
}

void* Rom_GetObjectDataPtr(u16 objId, PlayState* playState)
{
	int index = Object_GetIndex(&playState->objectCtx, objId);

	if (index < 0)
		return NULL;

    return playState->objectCtx.status[index].segment;
}

MessageEntry* Rom_GetMessageEntry(s16 msgId)
{
    for (int i = 0; i < 65535; i++)
    {
        MessageEntry* MsgE = AADDR(messageTable, i * sizeof(MessageEntry));

        if (MsgE->msg_id == msgId)
            return MsgE;
    }

    return NULL;
}

void Message_Overwrite(NpcMaker* en, PlayState* playState, s16 msgId)
{
    InternalMsgEntry msgdata = Data_GetCustomMessage(en, playState, msgId);
    Rom_LoadDataFromObject(playState, en->actor.params, &playState->msgCtx.font.msgBuf, en->messagesDataOffset + msgdata.offset, msgdata.msgLen, en->getSettingsFromRAMObject);
}