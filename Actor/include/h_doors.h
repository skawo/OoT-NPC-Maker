#ifndef H_DOORS_H
#define H_DOORS_H

#include "npc_maker_types.h"

void Doors_UpdateDummy(Actor* door, GlobalContext* global);
void Doors_Close(Actor* npc, Actor* door, GlobalContext* global);
void Doors_Open(Actor* npc, Actor* door, GlobalContext* global);

#define NPCID(door) door->colorFilterParams
#define ACTORID(door) door->colChkInfo.health

#endif