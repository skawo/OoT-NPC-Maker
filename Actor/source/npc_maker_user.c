#include "../include/npc_maker_user.h"
#include <hol/common.h>

// z64rom hosts these functions in lib_user
#ifndef NPCM_Z64ROM

int NpcM_GetLanguage()
{
    return SAVE_LANGUAGE;
}

void* NpcM_LoadAnimation(NpcMaker* en, int animId, int objectId)
{
    return LoadFromHeaderObjectToDest(objectId, animId, en->userLoadAnimBuf, en->userLoadAnimBuf, false, NULL);
}

int NpcM_GetAnimationSize(NpcMaker* en, int animId, int objectId)
{
    int outSize = 0;
    LoadFromHeaderObjectToDest(objectId, animId, NULL, NULL, true, &outSize);
    return outSize;
}

#endif