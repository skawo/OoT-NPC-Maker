#include "../include/npc_maker_user.h"
#include "../../common.h"

int NpcM_GetLanguage() 
{
    // To get support for translations, fill this in with an implementation that returns the current language ID (e.g. from the save file).
    return SAVE_LANGUAGE;
}

void* NpcM_LoadAnimation(NpcMaker* en, int animId, int objectId)
{
    // Implement loading external animations here
    return NULL;
}

int NpcM_GetAnimationSize(NpcMaker* en, int animId, int objectId)
{
    // Implement returning the size of an external animation here
    return 0;
}