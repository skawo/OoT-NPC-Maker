#ifndef UPDATE_H
#define UPDATE_H

#include "npc_maker_types.h"

void Update_Misc(NpcMaker* en, PlayState* playState);
void Update_TextureAnimations(NpcMaker *en, PlayState*playState);
void Update_Animations(NpcMaker* en, PlayState* playState);
void Update_HeadWaistRot(NpcMaker *en, PlayState*playState);
void Update_HitsReaction(NpcMaker* en, PlayState* playState);
void Update_Collision(NpcMaker* en, PlayState* playState);
void Update_ModelAlpha(NpcMaker* en, PlayState* playState);
void Update_Conversation(NpcMaker* en, PlayState* playState);

#endif