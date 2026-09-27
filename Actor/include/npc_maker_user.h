#ifndef H_USER_H
#define H_USER_H

#include "npc_maker_types.h"

int NpcM_GetLanguage();
void* NpcM_LoadAnimation(NpcMaker* en, int animId, int objectId);
int NpcM_GetAnimationSize(NpcMaker* en, int animId, int objectId);

#ifdef NPCM_Z64ROM
    #define NpcM_GetLanguage NpcM_Vtable->getLanguage
    #define NpcM_LoadAnimation NpcM_Vtable->loadAnimation
    #define NpcM_GetAnimationSize NpcM_Vtable->getAnimationSize
#endif

#endif