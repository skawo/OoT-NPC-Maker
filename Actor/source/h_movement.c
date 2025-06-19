#include "../include/h_movement.h"

u32 Movement_GetTotalPathTime(NpcMaker* en, PlayState* playState)
{
    if (en->settings.timedPathEnd > en->settings.timedPathStart)
        return en->settings.timedPathEnd - en->settings.timedPathStart;
    else
        return (0xFFFF - en->settings.timedPathStart) + en->settings.timedPathEnd;    
}

u32 Movement_GetRemainingPathTime(NpcMaker* en, PlayState* playState)
{
    if (en->settings.timedPathEnd > en->settings.timedPathStart || en->settings.timedPathEnd >= gSaveContext.dayTime)
        return MAX(0, en->settings.timedPathEnd - gSaveContext.dayTime);
    else
        return (0xFFFF - gSaveContext.dayTime) + en->settings.timedPathEnd;
}

void Movement_Apply(Actor* act, Vec3f* movementVec)
{
    // If a movement vector was passed, we move using it.
    if (movementVec != NULL)
        Math_Vec3f_Sum(&act->world.pos, movementVec, &act->world.pos);

    Actor_MoveForward(act);    
}

Vec3f Movement_CalcVector(Vec3f* start, Vec3f* end, float speed)
{
    const float EPSILON = 1e-6f;
    Vec3f vector = (Vec3f){0,0,0};
    
    float dist = Math_Vec3f_DistXYZAndStoreDiff(start, end, &vector);

    if (dist > EPSILON && speed < dist)
    {
        vector.x = (vector.x / dist) * speed;
        vector.y = (vector.y / dist) * speed;
        vector.z = (vector.z / dist) * speed;  
    }

    return vector;
}

float Movement_CalcDist(Vec3f* posA, Vec3f* posB, bool ignoreY)
{
    if (ignoreY)
        return Math_Vec3f_DistXZ(posA, posB);
    else
        return Math_Vec3f_DistXYZ(posA, posB);    
}

s16 Movement_RotTowards(s16* src, s16 target, u32 speed)
{
    u32 speed_max = speed == 0 ? MOVEMENT_ROTATION_MAX : speed;
    u32 speed_min = speed == 0 ? MOVEMENT_ROTATION_MIN : speed / 10;

    return Math_SmoothStepToS(src, target, 
                              MOVEMENT_ROTATION_SCALE,
                              speed_max,
                              speed_min);
}

s16 Movement_StepToZero(float* steppedToZero, s16* stepped, float step)
{
    s16 outStep = 0;

    if ((*steppedToZero >= 0 && *steppedToZero - step < 0) || (*steppedToZero < 0 && (*steppedToZero + step > 0)))
        outStep = -*steppedToZero;
    else if (*steppedToZero < 0)
        outStep = step;
    else if (*steppedToZero > 0)
        outStep = -step;

    *steppedToZero += outStep;
    *stepped += outStep;

    return outStep;
}