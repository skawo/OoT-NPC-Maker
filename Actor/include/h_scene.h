#ifndef H_SCENE_H
#define H_SCENE_H

#include "npc_maker_types.h"

u32* Scene_GetHeaderPtr(GlobalContext* global, int setup_index);
u32* Scene_GetCurrentHeaderPtr(GlobalContext*global);
u32* Scene_GetCutscenePtr(GlobalContext* global, int setup_index);
u32* Scene_GetCurrentCutscenePtr(GlobalContext*global);
Path* Scene_GetPathPtr(GlobalContext* global, s16 path_id);
u8 Scene_GetPathNodeCount(GlobalContext* global, s16 path_id);
Vec3s* Scene_GetPathNodePos(GlobalContext* global, s16 path_id, s16 node_id);
Vec3f get_node_data_from_path_f(GlobalContext* global, s16 path_id, s16 node_id);
float Scene_GetPathSectionLen(GlobalContext* global, s16 path_id, s16 start_node_id, bool ignore_y);
float Scene_GetPathLen(GlobalContext* global, s16 path_id, s16 start_node_id, s16 end_node_id, bool ignore_y);
NpcMaker* Scene_GetNpcMakerByID(NpcMaker* en, GlobalContext*global, u16 ID);
Actor* Scene_GetActorByID(int ID, GlobalContext* globalCtx, Actor* closestTo, Actor* skip);


#endif