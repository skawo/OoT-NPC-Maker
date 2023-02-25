#include "../include/h_math.h"

void Math_AffectMatrixByRot(s16 rot, Vec3f* vector, Actor* rel)
{
    Vec3f temp;

    Matrix_Push();

    if (rel != NULL)
        Matrix_SetTranslateRotateYXZ(rel->world.pos.x, rel->world.pos.y, rel->world.pos.z, &rel->shape.rot);

    Matrix_RotateY((rot / 32768.0f) * M_PI, 0);
    Matrix_MultVec3f(vector, &temp);
    Matrix_Pop();

    Math_Vec3f_Copy(vector, &temp);
}

inline s16 Math_RandGetBetween(s16 min, s16 max)
{
    return Rand_S16Offset(min, max - min);
}

inline void Math_Vec3s_Sum(Vec3s* a, Vec3s* b, Vec3s* dst) 
{
    dst->x = a->x + b->x;
    dst->y = a->y + b->y;
    dst->z = a->z + b->z;
}