#ifndef MOVEMENT_H
#define MOVEMENT_H

#include "npc_maker_types.h"

#define DIST_TO_NEXT_POS(en, ignoreY) ignoreY ? Math_Vec3f_DistXZ(&en->actor.world.pos, &en->movementNextPos) : Math_Vec3f_DistXYZ(&en->actor.world.pos, &en->movementNextPos)

void Movement_OpenDoors(NpcMaker* en, PlayState* playState);
void Movement_MoveTowardsNextPos(NpcMaker* en, PlayState* playState, float speed, movement_type movement_type, bool ignore_y_coord, bool set_anims);
bool Movement_RideActor(NpcMaker* en, PlayState* playState);
bool Movement_PickUp(NpcMaker* en, PlayState* playState);
void Movement_Main(NpcMaker* en, PlayState* playState, movement_type movement_type, bool ignore_y_coord, bool set_anims);
void Movement_SetNextDelay(NpcMaker* en);
void Movement_SetNextPos(NpcMaker* en, Vec3f* next_pos);
void Movement_StopMoving(NpcMaker* en, PlayState* playState, bool stop_anim);
bool Movement_HasReachedDestination(NpcMaker* en, float distance_margin);

#endif