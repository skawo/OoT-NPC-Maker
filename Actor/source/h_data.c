#include "../include/h_data.h"
#include "../include/h_rom.h"
#include "../include/npc_maker_user.h"

ExSegDataEntry* Data_GetExtraSegmentData(NpcMaker* en, int segmentId, int entry)
{
    ExSegDataEntry* segmentAddr = (ExSegDataEntry*)AADDR(en->exSegData, 4 + en->exSegData[segmentId]);
    return AADDR(segmentAddr, entry * sizeof(ExSegDataEntry));
}

InternalMsgEntry Data_GetCustomMessage(NpcMaker* en, PlayState* playState, int ID)
{
    InternalMsgEntry msgData;

    int language = NpcM_GetLanguage();

    if (language <= en->numLanguages - 1)
        ID += (language * en->numMessages);

    if (en->getSettingsFromRAMObject)
        Rom_LoadDataFromObject(playState, en->actor.params, &msgData, en->messagesDataOffset + (ID * sizeof(InternalMsgEntry)), sizeof(InternalMsgEntry), en->getSettingsFromRAMObject);
    else
        bcopy((void*)(en->messagesDataOffset + (ID * sizeof(InternalMsgEntry))), &msgData, sizeof(InternalMsgEntry));

    return msgData;
}