#include "../include/h_data.h"
#include "../include/h_rom.h"

ExSegDataEntry* Data_GetExtraSegmentData(NpcMaker* en, int segmentId, int entry)
{
    ExSegDataEntry* segmentAddr = (ExSegDataEntry*)AADDR(en->exSegData, 4 + en->exSegData[segmentId]);
    return AADDR(segmentAddr, entry * sizeof(ExSegDataEntry));
}

InternalMsgEntry Data_GetCustomMessage(NpcMaker* en, GlobalContext* globalCtx, int ID)
{
    InternalMsgEntry msgData;
    Rom_LoadDataFromObject(globalCtx, en->actor.params, &msgData, en->messagesDataOffset + (ID * sizeof(InternalMsgEntry)), sizeof(InternalMsgEntry), en->settingsCompressed);
    return msgData;
}