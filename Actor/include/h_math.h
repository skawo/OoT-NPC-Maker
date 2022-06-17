#ifndef H_MATh_H
#define H_MATh_H

#include "npc_maker_types.h"

void Math_AffectMatrixByRot(s16 rot, Vec3f* vector, Actor* rel);
s16 Math_RandGetBetween(s16 min, s16 max);
void Math_Vec3s_Sum(Vec3s* a, Vec3s* b, Vec3s* out);

#endif 