#ifndef H_MOVEMENT
#define H_MOVEMENT

#include "npc_maker_types.h"

s16 Movement_RotTowards(s16* src, s16 target, u32 speed);
u32 Movement_GetTotalPathTime(NpcMaker* en, PlayState* playState);
u32 Movement_GetRemainingPathTime(NpcMaker* en, PlayState* playState);
void Movement_Apply(Actor* act, Vec3f* movement_vector);
Vec3f Movement_CalcVector(Vec3f* start, Vec3f* end, float speed);
s16 Movement_StepToZero(float* step_counter, s16* stepped, float step);
float Movement_CalcDist(Vec3f* position_a, Vec3f* position_b, bool ignore_y);

#endif