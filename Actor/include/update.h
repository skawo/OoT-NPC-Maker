#ifndef UPDATE_H
#define UPDATE_H

#include "npc_maker_types.h"

void Update_Misc(NpcMaker* en, GlobalContext* global);
void Update_TextureAnimations(NpcMaker *en, GlobalContext*global);
void Update_Animations(NpcMaker* en, GlobalContext* global);
void Update_HeadWaistRot(NpcMaker *en, GlobalContext*global);
void Update_HitsReaction(NpcMaker* en, GlobalContext* global);
void Update_Collision(NpcMaker* en, GlobalContext* global);
void Update_ModelAlpha(NpcMaker* en, GlobalContext* global);
void Update_Conversation(NpcMaker* en, GlobalContext* global);

#endif