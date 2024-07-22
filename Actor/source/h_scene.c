#include "../include/h_scene.h"
#include "../include/h_movement.h"

u32* Scene_GetHeaderPtr(PlayState* playState, int setupId)
{
    u32* sceneHeader = (u32*)playState->sceneSegment;

    if (*sceneHeader == 0x18000000 && setupId != 0)
    {
        u32* headers_list = (u32*)*(sceneHeader + 1);

        for (int i = setupId - 1; i > -1; i--)
        {
            u32* addr = (u32*)SEGMENTED_TO_VIRTUAL((u32)(headers_list + i));

            if (*addr != 0)
            {
                sceneHeader = (u32*)SEGMENTED_TO_VIRTUAL((u32)*addr);
                break;
            }
        }
    }

    return sceneHeader;
}

inline u32* Scene_GetCurrentHeaderPtr(PlayState* playState)
{
    return Scene_GetHeaderPtr(playState, gSaveContext.sceneLayer);
}

u32* Scene_GetCutscenePtr(PlayState* playState, int setupId)
{
    u32* cutscenePtr = Scene_GetHeaderPtr(playState, setupId);

    for (int i = 0; i < 25; i++)
    {
        if (*((u8*)(cutscenePtr + i * 2)) == 0x14)
            break;

        if (*(cutscenePtr + (i * 2)) == 0x17000000)
            return (u32*)*(cutscenePtr + (i * 2) + 1);
    }

    return NULL;
}

inline u32* Scene_GetCurrentCutscenePtr(PlayState* playState)
{
    return Scene_GetCutscenePtr(playState, gSaveContext.sceneLayer);
}

Path* Scene_GetPathPtr(PlayState* playState, s16 pathId)
{
    if (playState->setupPathList == NULL)
        return NULL;

    return &playState->setupPathList[pathId];
}

u8 Scene_GetPathNodeCount(PlayState* playState, s16 pathId)
{
    if (playState->setupPathList == NULL)
        return 0;

    Path* pathPtr = Scene_GetPathPtr(playState, pathId);

    if (pathPtr == NULL)
        return 0;

    return pathPtr->count;
}

Vec3s* Scene_GetPathNodePos(PlayState* playState, s16 pathId, s16 nodeId)
{
    Path* pathPtr = Scene_GetPathPtr(playState, pathId);

    if (pathPtr == NULL)
        return NULL;

    if (pathPtr->count < nodeId)
        return NULL;
    else
    {
        Vec3s* out = SEGMENTED_TO_VIRTUAL(pathPtr->points);
        return &out[nodeId];
    }
}

float Scene_GetPathSectionLen(PlayState* playState, s16 pathId, s16 startNodeId, bool ignoreY)
{
    Vec3s* start = Scene_GetPathNodePos(playState, pathId, startNodeId);
    Vec3s* end = Scene_GetPathNodePos(playState, pathId, startNodeId + 1);

    if (start == NULL || end == NULL)
        return 0;

    Vec3f start_f;
    Vec3f end_f;

    Math_Vec3s_ToVec3f(&start_f, start);
    Math_Vec3s_ToVec3f(&end_f, end);
    return Movement_CalcDist(&start_f, &end_f, ignoreY);
}

float Scene_GetPathLen(PlayState* playState, s16 pathId, s16 startNodeId, s16 endNodeId, bool ignoreY)
{
    float out = 0;

    for (u32 i = startNodeId; i < endNodeId; i++)
        out += Scene_GetPathSectionLen(playState, pathId, i, ignoreY);

    return out;
}

NpcMaker* Scene_GetNpcMakerByID(NpcMaker* en, PlayState* playState, u16 ID)
{
    if (en->npcId == ID)
        return en;
    
    NpcMaker* npc = (NpcMaker*)playState->actorCtx.actorLists[ACTORCAT_NPC].head;
    bool found = false;

    while (npc)
    {
        if (npc->actor.id == en->actor.id)
        {
            if (npc->npcId == ID)
            {
                found = true;
                break;
            }
        }

        npc = (NpcMaker*)npc->actor.next;
    }

    if (!found)
        return NULL;
    else
        return npc;
}

Actor* Scene_GetActorByID(int ID, PlayState* playState, Actor* closestTo, Actor* skip)
{
    Actor* out = skip;
    float dist = __UINT32_MAX__;

    for (int i = ACTORCAT_SWITCH; i <= ACTORCAT_CHEST; i++)
    {
        Actor* act = playState->actorCtx.actorLists[i].head;

        while (act)
        {
            if (act->id == ID)
            {
                if (closestTo == NULL)
                    return act;
                else
                {
                    float distToActor = Math_Vec3f_DistXYZ(&closestTo->world.pos, &act->world.pos);

                    if ((distToActor < dist && act != closestTo) && (skip == NULL || skip != act))
                    {
                        dist = distToActor;
                        out = act;
                    }
                }
            }

            act = act->next;
        }
    }

    return out;    
}