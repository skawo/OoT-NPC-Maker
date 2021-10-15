#include "../include/h_math.h"

void Math_AffectMatrixByRot(s16 rot, Vec3f* vector)
{
    Vec3f temp;

    Matrix_Push();
    Matrix_RotateY((rot / 32768.0f) * M_PI, 0);
    Matrix_MultVec3f(vector, &temp);
    Matrix_Pop();

    Math_Vec3f_Copy(vector, &temp);
}

inline s16 Math_RandGetBetween(s16 min, s16 max)
{
    return Rand_S16Offset(min, max - min);
}