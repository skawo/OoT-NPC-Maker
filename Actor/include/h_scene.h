#ifndef H_SCENE_H
#define H_SCENE_H

#include "npc_maker_types.h"

u32* Scene_GetHeaderPtr(PlayState* playState, int setup_index);
u32* Scene_GetCurrentHeaderPtr(PlayState*playState);
u32* Scene_GetCutscenePtr(PlayState* playState, int setup_index);
u32* Scene_GetCurrentCutscenePtr(PlayState*playState);
Path* Scene_GetPathPtr(PlayState* playState, s16 path_id);
u8 Scene_GetPathNodeCount(PlayState* playState, s16 path_id);
Vec3s* Scene_GetPathNodePos(PlayState* playState, s16 path_id, s16 node_id);
Vec3f get_node_data_from_path_f(PlayState* playState, s16 path_id, s16 node_id);
float Scene_GetPathSectionLen(PlayState* playState, s16 path_id, s16 start_node_id, bool ignore_y);
float Scene_GetPathLen(PlayState* playState, s16 path_id, s16 start_node_id, s16 end_node_id, bool ignore_y);
NpcMaker* Scene_GetNpcMakerByID(NpcMaker* en, PlayState*playState, u16 ID);
Actor* Scene_GetActorByID(int ID, PlayState* playState, Actor* closestTo, Actor* skip);

#endif