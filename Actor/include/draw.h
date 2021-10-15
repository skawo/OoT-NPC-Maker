#ifndef DRAW_H
#define DRAW_H

#include "npc_maker_types.h"

void Draw_Debug(NpcMaker* en, GlobalContext* global);
void Draw_Lights(NpcMaker* en, GlobalContext* global, Vec3f* translation);
void Draw_LightsRebind(NpcMaker* en, GlobalContext* globalCtx);
void Draw_SetAxis(u8 axis, s16 value, Vec3s* rotation);
//void Draw_ExtDList(NpcMaker *en, GlobalContext* globalCtx, s32 object, Color_RGB8 envColor, u32 dList);
void Draw_AffectMatrix(ExDListEntry dlist, Vec3f* translation, Vec3s* rotation);
void Draw_PostLimbDraw(GlobalContext* global, s32 limb, Gfx** dlist_addr_ptr, Vec3s* rot, void* instance, Gfx** gfxP);
s32 Draw_PostLimbDrawSkin(Actor* instance, GlobalContext* global, s32 limb, PSkinAwb* skelanime);
s32 Draw_OverrideLimbDraw(GlobalContext* global, s32 limb_number, Gfx** dlist_addr_ptr, Vec3f* translation, Vec3s* rotation, void* instance, Gfx** gfxP);
void Draw_SetGlobalEnvColor(NpcMaker* en, GlobalContext* global);
void Draw_SetupSegments(NpcMaker* en, GlobalContext* global);
void set_env_color(NpcMaker* en, GlobalContext* global);
void Draw_Model(NpcMaker* en, GlobalContext* global);
void Draw_CalcFocusPos(GlobalContext* global, s32 limb, NpcMaker* en);
void Draw_SetEnvColor(Gfx** destP, Color_RGB8 color, u8 alpha);

#endif