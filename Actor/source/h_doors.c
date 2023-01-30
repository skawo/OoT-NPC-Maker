#include "../include/h_doors.h"

void Doors_UpdateDummy(Actor* door, PlayState* playState)
{
    //Failsafe.
    if (Actor_FindNearby(playState, door, door->colChkInfo.health, ACTORCAT_NPC, DOOR_OPEN_DIST_MARGIN * 3) == NULL)
        Doors_Close(door, door, playState);
}

void Doors_Close(Actor* npc, Actor* door, PlayState* playState)
{
    u32* savedUpdate = (u32*)&door->speedXZ;

    if (door->update == (void*)&Doors_UpdateDummy)
    {
        if (npc != NULL && npc != door)
        {
            int id = ((NpcMaker*)npc)->npcId;

            if (id != NPCID(door))
                return;
        }

        switch (door->id)
        {
            case ACTOR_EN_DOOR:
            {
                SkelAnime* door_skelanime = AADDR(door, 0x14C);
                Math_ApproachS(&door_skelanime->jointTable[4].z, 0, 1, DOOR_OPEN_SHUT_SPEED);

                if (door_skelanime->jointTable[4].z == 0)
                {
                    door->update = (void*)*savedUpdate;
                    NPCID(door) = 0xFFFF;
                    Audio_PlayActorSfx2(door, NA_SE_EV_DOOR_CLOSE);
                }
                break;
            }
            case ACTOR_DOOR_SHUTTER:
            {
                if (door->world.pos.y == (door->home.pos.y + SLIDE_DOOR_OPEN_DIST))
                    Audio_PlayActorSfx2(door, NA_SE_EV_SLIDE_DOOR_CLOSE);

                if (door->world.pos.y < door->home.pos.y + 20)
                {
                    door->floorHeight = door->world.pos.y;
                    Actor_SpawnFloorDustRing(playState, door, &door->world.pos, 45.0f, 0xA, 8.0f, 0x1F4, 0xA, 0);
                }

                Math_ApproachF(&door->world.pos.y, door->home.pos.y, 1, SLIDE_DOOR_OPEN_SHUT_SPEED);

                if (door->world.pos.y == door->home.pos.y)
                {
                    door->update = (void*)*savedUpdate;
                    NPCID(door) = 0xFFFF;
                    Audio_PlayActorSfx2(door, NA_SE_EV_STONE_BOUND);
                }

                break;
            }
            default: break;
        }
    }

}

void Doors_Open(Actor* npc, Actor* door, PlayState* playState)
{
    u32* savedUpdate = (u32*)&door->speedXZ;

    switch (door->id)
    {
        case ACTOR_EN_DOOR:
        {
            SkelAnime* door_skelanime = AADDR(door, sizeof(Actor));

            if (door->update != (void*)&Doors_UpdateDummy)
            {
                // Abusing the xz_speed, health and damage color here to store info.
                *savedUpdate = (u32)door->update;
                door->update = (void*)&Doors_UpdateDummy;
                NPCID(door) = ((NpcMaker*)npc)->npcId;
                ACTORID(door) = npc->id;

                Audio_PlayActorSfx2(door, NA_SE_OC_DOOR_OPEN);
            }

            Math_ApproachS(&door_skelanime->jointTable[4].z, door->shape.rot.y < 0 ? DOOR_OPEN_DEST_ROT : -DOOR_OPEN_DEST_ROT, 1, DOOR_OPEN_SHUT_SPEED);
            break;
        }
        case ACTOR_DOOR_SHUTTER:
        {
            // Abusing the xz_speed, health and damage color here to store info.
            if (door->update != (void*)&Doors_UpdateDummy)
            {
                door->home.pos = door->world.pos;
                *savedUpdate = (u32)door->update;
                door->update = (void*)&Doors_UpdateDummy;
                NPCID(door) = ((NpcMaker*)npc)->npcId;
                ACTORID(door) = npc->id;

                Audio_PlayActorSfx2(door, NA_SE_EV_SLIDE_DOOR_OPEN);
            }

            Math_ApproachF(&door->world.pos.y, door->home.pos.y + SLIDE_DOOR_OPEN_DIST, 1, SLIDE_DOOR_OPEN_SHUT_SPEED);
            break;
        }
        default: break;
    }
}