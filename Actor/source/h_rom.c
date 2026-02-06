#include "../include/h_rom.h"

bool Rom_IsObjectCompressed(int objId)
{
    RomFile* rv = Rom_GetObjectVROMAddr(objId);
    return Rom_GetPhysicalROMAddrFromVirtual(rv->vromStart).vromEnd != 0;
}

inline RomFile* Rom_GetObjectVROMAddr(int objId)
{
    return (RomFile*)&objectTable[objId];
}

RomFile Rom_GetPhysicalROMAddrFromVirtual(u32 virtual)
{
    DmaEntry* dma;
    RomFile out = { .vromStart = 0xFFFFFFFF, .vromEnd = 0xFFFFFFFF };

    for (dma = (DmaEntry*)dmaData; ; ++dma)
    {
        if (dma->romStart == (uintptr_t)virtual)
        {
            out.vromStart = dma->romStart;
            out.vromEnd   = dma->romEnd;
            break;
        }
    }

    return out;
}

// Loads data from given object directly from the cartridge (if it's not compressed!)
void Rom_LoadObject(int objId, void *dest)
{
    RomFile* obj = Rom_GetObjectVROMAddr(objId);
    DMA_REQUEST_SYNC(dest, obj->vromStart, obj->vromEnd - obj->vromStart, __FILE__, __LINE__);
}

// Loads data from given object directly from the cartridge (if it's not compressed!)
void Rom_LoadDataFromObjectFromROM(int objId, void* ram, u32 fileOffs, size_t size)
{
    RomFile* obj = Rom_GetObjectVROMAddr(objId);

    if (obj->vromStart == 0)
    {
        #if LOGGING > 0
            is64Printf("_Object %4d was not found!\n", objId);
        #endif       

        return;
    }

    if (size > obj->vromEnd - (obj->vromStart + fileOffs))
        size = obj->vromEnd - (obj->vromStart + fileOffs);

    u32 start = obj->vromStart + fileOffs;

    u8 buf[16];
    void* dest = ram;
    size_t sz = size;

    if (size < 16)
    {
        sz = 16;
        dest = buf;
    }

    #if LOGGING > 0
        is64Printf("_Loading 0x%08x bytes from ROM at 0x%08x\n", size, start);
    #endif    

    DMA_REQUEST_SYNC(dest, start, sz, __FILE__, __LINE__);

    if (size < 16)
        bcopy(buf, ram, size);
}

void Rom_LoadDataFromObject(PlayState* playState, int objId, void* dest, u32 fileOffs, size_t size, bool fromRam)
{
    if (fromRam)
    {
        void* ptr = Rom_GetObjectDataPtr(objId, playState);

        if (ptr == NULL)
        {
            #if LOGGING > 0
                is64Printf("_The file to load from was not found in RAM!\n");
            #endif   

            return;
        }

        bcopy(AADDR(ptr, fileOffs), dest, size);
    }
    else
        Rom_LoadDataFromObjectFromROM(objId, dest, fileOffs, size);
}

s32 Rom_LoadObjectIfUnloaded(PlayState* playState, s16 objId)
{
    s32 bankIndex = -1;

    if (objId <= 0)
        return bankIndex;

    #if LOGGING > 0
        is64Printf("_Loading object %4d...\n", objId);
    #endif   
    
    bankIndex = Object_GetSlot(&playState->objectCtx, objId);

    if (bankIndex < 0)
    {
        u32 numPersistent = playState->objectCtx.numPersistentEntries;
        bankIndex = Object_SpawnPersistent(&playState->objectCtx, objId);
        playState->objectCtx.numPersistentEntries = numPersistent;
    }
    else
    {
        #if LOGGING > 0
            is64Printf("_It's already loaded.\n");
        #endif         
    }

    return bankIndex;
}

bool Rom_SetObjectToActor(Actor* en, PlayState* playState, u16 object, s32 fileStart)
{
    if (object == 0)
        return true;
    
    int bankIndex = Object_GetSlot(&playState->objectCtx, object);

    if (Object_IsLoaded(&playState->objectCtx, bankIndex))
    {
        en->objectSlot = bankIndex;
        gSegments[6] = OS_K0_TO_PHYSICAL(playState->objectCtx.slots[en->objectSlot].segment) + fileStart;
        return true;
    }
    else
        return false;
}

void* Rom_GetObjectDataPtr(u16 objId, PlayState* playState)
{
    int index = Object_GetSlot(&playState->objectCtx, objId);

    if (index < 0)
        return NULL;

    return playState->objectCtx.slots[index].segment;
}

MessageTableEntry* Rom_GetMessageEntry(s16 msgId)
{
    for (int i = 0; i < 65535; i++)
    {
        MessageTableEntry* MsgE = (MessageTableEntry*)&messageTable[i];

        if (MsgE->textId == msgId)
            return MsgE;
    }

    return NULL;
}

void Message_Overwrite(NpcMaker* en, PlayState* playState, s16 msgId)
{
    Message_Get(en, playState, msgId, playState->msgCtx.font.msgBuf);
}

void Message_Get(NpcMaker* en, PlayState* playState, s16 msgId, void* buffer)
{
    if (!en->messagesDataOffset)
        return;

    InternalMsgEntry msgdata = Data_GetCustomMessage(en, playState, msgId);

    if (en->getSettingsFromRAMObject)
        Rom_LoadDataFromObject(playState, en->actor.params, buffer, en->messagesDataOffset + msgdata.offset, msgdata.msgLen, en->getSettingsFromRAMObject);
    else
        bcopy((void*)(en->messagesDataOffset + msgdata.offset), buffer, msgdata.msgLen);
}

void* Message_GetMessageRAMAddr(NpcMaker* en, PlayState* playState, s16 msgId)
{
    if (!en->messagesDataOffset)
        return NULL;

    InternalMsgEntry msgdata = Data_GetCustomMessage(en, playState, msgId);
    void* ptr = 0;
    
    if (en->getSettingsFromRAMObject)
        ptr = Rom_GetObjectDataPtr(en->actor.params, playState);

    return AADDR(ptr, en->messagesDataOffset + msgdata.offset);
}