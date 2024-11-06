#ifndef DRAW_H
#define DRAW_H

#include "npc_maker_types.h"

void Draw_Debug(NpcMaker* en, PlayState* playState);
void Draw_Lights(NpcMaker* en, PlayState* playState, Vec3f* translation);
void Draw_LightsRebind(NpcMaker* en, PlayState* playState);
void Draw_SetAxis(u8 axis, s16 value, Vec3s* rotation);
//void Draw_ExtDList(NpcMaker *en, PlayState* playState, s32 object, Color_RGB8 envColor, u32 dList);
void Draw_AffectMatrix(ExDListEntry dlist, Vec3f* translation, Vec3s* rotation);
void Draw_PostLimbDraw(PlayState* playState, s32 limb, Gfx** dlist_addr_ptr, Vec3s* rot, void* instance, Gfx** gfxP);
s32 Draw_PostLimbDrawSkin(Actor* instance, PlayState* playState, s32 limb, Skin* skelanime);
s32 Draw_OverrideLimbDraw(PlayState* playState, s32 limb_number, Gfx** dlist_addr_ptr, Vec3f* translation, Vec3s* rotation, void* instance, Gfx** gfxP);
void Draw_SetGlobalEnvColor(NpcMaker* en, PlayState* playState);
void Draw_SetupSegments(NpcMaker* en, PlayState* playState);
void set_env_color(NpcMaker* en, PlayState* playState);
void Draw_Model(NpcMaker* en, PlayState* playState);
void Draw_CalcFocusPos(PlayState* playState, s32 limb, NpcMaker* en);
void Draw_SetEnvColor(Gfx** destP, Color_RGB8 color, u8 alpha);
void Draw_ExtDListInt(NpcMaker *en, PlayState* playState, ExDListEntry* dList, Gfx** dest);

float NpcMaker_RunCFunc(NpcMaker* en, PlayState* playState, u32 offset, float* Args);

#endif