#include "../include/movement.h"
#include "../include/init.h"
#include "../include/h_movement.h"
#include "../include/h_scene.h"
#include "../include/h_doors.h"
#include "../include/h_rom.h"
#include "../include/h_math.h"
#include "../include/h_scripts.h"

void Movement_OpenDoors(NpcMaker* en, PlayState* playState)
{
    Actor* door = playState->actorCtx.actorLists[ACTORCAT_DOOR].head;

    while (door != NULL)
    {
        // Needs to check whether the initial position is below the margin, because shutter doors move up when being opened, which causes issues.
        // Unfortunately, it seems shutter doors don't set their pos_init automatically, which is incredibly annoying.
        // So, open_door does that, but we have to check whether either distance is less than the margin for that first check when the initial pos hasn't yet been set.
        float dist = Math_Vec3f_DistXYZ(&en->actor.world.pos, &door->world.pos);
        float dist2 = Math_Vec3f_DistXYZ(&en->actor.world.pos, &door->home.pos);

        if (dist <= DOOR_OPEN_DIST_MARGIN || dist2 <= DOOR_OPEN_DIST_MARGIN)
            Doors_Open(&en->actor, door, playState);
        else
            Doors_Close(&en->actor, door, playState);

        door = door->next;
    }
}

void Movement_MoveTowardsNextPos(NpcMaker* en, PlayState* playState, float speed, movement_type movementType, bool ignoreY, bool setAnims)
{
    // We don't move in these instances. Can't move if movement type is timed path since that snaps the NPC into place.
    if (!en->canMove ||
        !speed ||  
         en->stopped || 
         en->listeningToSong || 
         en->wasHit || 
         movementType == MOVEMENT_TIMED_PATH)
    {
        en->actor.speedXZ = 0;
        Movement_Apply(&en->actor, NULL);
        return;
    }

    // The option to not consider Y axis exists, because placing pathnodes on the ground is difficult.
    en->currentDistToNextPos = DIST_TO_NEXT_POS(en, ignoreY);

    // If we're moving and we're close enough to the end, or the distance travelled has exceeded the initial distance calculated, we don't move.
    if (movementType != MOVEMENT_CUTSCENE && Movement_HasReachedDestination(en, MOVEMENT_DISTANCE_EQUAL_MARGIN))
    {
        Movement_StopMoving(en, playState, setAnims);
        Movement_Apply(&en->actor, NULL);
        return;
    }
    else
    {
        // If we're not moving, and the distance hasn't been reached, we start moving.
        if (!en->isMoving)
        {
            en->isMoving = true;
            en->distanceTotal = en->currentDistToNextPos;
            en->traversedDistance = 0;
            en->lastTraversedDistance = 0;
            en->movementStartPos = en->actor.world.pos;
            
            #if LOGGING > 0
                is64Printf("_Started movement.\n");
            #endif  
        }

        if (setAnims && en->currentAnimId != ANIM_WALK)
            Setup_Animation(en, playState, ANIM_WALK, true, false, false, !en->autoAnims, false);

        // Calculate the direction. Set the rotation faced immediately to the smoothed direction.
        Math_SmoothStepToS(&en->actor.world.rot.y, 
                            Math_Vec3f_Yaw(&en->actor.world.pos, &en->movementNextPos), 
                            MAX(1, MOVEMENT_SMOOTHEN_ROTATION_SCALE + en->settings.smoothingConstant), 
                            MOVEMENT_SMOOTHEN_ROTATION_MAX,
                            MOVEMENT_SMOOTHEN_ROTATION_MIN);

        en->actor.shape.rot.y = en->actor.world.rot.y;

        // When we ignore Y, we can just set the speed to move in the calculated direction and away we go.
        // Otherwise, we calculate the vector to move by in this frame.
        if (ignoreY)
        {
            en->actor.speedXZ = speed;
            Movement_Apply(&en->actor, NULL);
        }
        else
        {
            en->actor.speedXZ = 0;
            Vec3f normalized = Movement_CalcVector(&en->actor.world.pos, &en->movementNextPos, speed);
            Movement_Apply(&en->actor, &normalized);
        }
    }
}

bool Movement_RideActor(NpcMaker* en, PlayState* playState)
{
    if (en->settings.riddenNPCId >= 0)
    {
        en->riddenNpc = Scene_GetNpcMakerByID(en, playState, en->settings.riddenNPCId);

        if (en->riddenNpc == NULL)
            return false;

        // If not talked to, check for if the ridden NPC was hit and riding NPC is supposed to react to attacks.
        if (!en->isTalking)
        {
            if (en->riddenNpc->wasHit && en->settings.reactsToAttacks)
            {
                // Play necessary SFX and animation if they are.
                if (!en->wasHit)
                {
                    if (en->settings.sfxIfAttacked >= 0)
                        Audio_PlayActorSfx2(&en->actor, en->settings.sfxIfAttacked);

                    // Play the attacked animation and setup info that we've been hit.
                    Setup_Animation(en, playState, ANIM_ATTACKED, true, true, false, !en->autoAnims, false);
                    en->wasHit = true;
                    en->canMove = false;
                }
            }
            else
            {
                // Stop the hit state.
                if (!en->riddenNpc->wasHit && en->wasHit)
                {
                    en->wasHit = false;
                    en->canMove = true;
                }

                // Set the appropriate animation...
                if (en->riddenNpc->isMoving)
                    Setup_Animation(en, playState, ANIM_WALK, true, false, false, !en->autoAnims, false);
                else
                    Setup_Animation(en, playState, ANIM_IDLE, true, false, false, !en->autoAnims, false);
            }
        }

        // Carry over the necessary information...
        en->actor.world.pos = en->riddenNpc->actor.world.pos;
        en->actor.shape.rot = en->riddenNpc->actor.shape.rot;
        en->riddenNpc->canMove = en->canMove;
        en->riddenNpc->isTalking = en->isTalking;

        // Stop moving if the either NPC has stopped moving.
        if (!en->canMove || !en->riddenNpc->canMove)
        {
            if (en->isMoving)
                Movement_StopMoving(en, playState, true);

            if (en->riddenNpc->isMoving)
            {
                // Object needs to be set to the ridden NPC's, otherwise the animations will not work properly.
                Rom_SetObjectToActor(&en->riddenNpc->actor, playState, en->riddenNpc->settings.objectId, en->riddenNpc->settings.fileStart);
                Movement_StopMoving(en->riddenNpc, playState, true);
                Rom_SetObjectToActor(&en->actor, playState, en->settings.objectId, en->settings.fileStart);
            }
        }

        return true;
    }
    else
        return false;    
}

bool Movement_PickUp(NpcMaker* en, PlayState* playState)
{
    switch (en->pickedUpState)
    {
        case STATE_IDLE:
        {
            // If player is holding us, change state to picked-up, disable collision and targetting.
            if (GET_PLAYER(playState)->heldActor == &en->actor)
            {
                en->pickedUpState = STATE_PICKED_UP;
                en->hadCollision = en->settings.hasCollision;
                en->settings.hasCollision = false;
                en->actor.flags &= ~TARGETTABLE_MASK;      
            }

            break; 
        }
        // If player stopped holding us, we've been thrown. Restore collision and targetting.
        case STATE_PICKED_UP:
        {
            if (GET_PLAYER(playState)->heldActor != &en->actor)
            {
                en->pickedUpState = STATE_THROWN;
                en->settings.hasCollision = en->hadCollision;

                if (en->settings.isTargettable)
                    en->actor.flags |= TARGETTABLE_MASK;   
            }

            break; 
        }
        // If we've been thrown and are on the ground, we've landed.
        case STATE_THROWN:
        {
            if (en->actor.world.pos.y == en->actor.floorHeight)
                en->pickedUpState = STATE_LANDED;
            else
            {
                // Else, move in the thrown direction with speed calculated relative to mass.
                en->actor.speedXZ = MIN(MAX_THROW_VELOCITY, MIN_THROW_VELOCITY + (255 - en->actor.colChkInfo.mass));
                Movement_Apply(&en->actor, NULL);      
            }

            break;
        }
        // If we've landed last frame, return to being idle.
        case STATE_LANDED:
        {
            en->pickedUpState = STATE_IDLE;
            break;
        }
    }

    return en->pickedUpState != STATE_IDLE;
}

void Movement_Main(NpcMaker* en, PlayState* playState, movement_type movementType, bool ignoreY, bool setAnims)
{
    if (Movement_RideActor(en, playState) || Movement_PickUp(en, playState) || !en->canMove)
        return;

    // Before we start moving, we grab the current position for use later.
    Vec3f pos_before_movement = en->actor.world.pos;

    // If the actor is set to do so, open doors.
    // This has to happen after the first frame of a scene, or the game crashes, which is why it's placed up here.
    if (en->settings.opensDoors)
        Movement_OpenDoors(en, playState);

    float speed = en->settings.movementSpeed;

    switch (movementType)
    {
        // In this case, we wait until we reach the calculated random interval of time.
        // Then, we calculate a random vector and set the next position to it.
        // We also update the random interval for the next time.
        case MOVEMENT_ROAM:
        {
            bool tooFarFromBase = Movement_CalcDist(&en->actor.world.pos, &en->actor.home.pos, en->settings.ignorePathYAxis) > en->settings.maxRoamDist;
            bool reachedDest = Movement_HasReachedDestination(en, MOVEMENT_DISTANCE_EQUAL_MARGIN);

            if (!en->isMoving || (en->roamMovementDelay == 1 && reachedDest))
            {
                en->movementDelayCounter++;

                if (en->roamMovementDelay <= en->movementDelayCounter)
                {
                    Vec3f DiffVect;

                    if (tooFarFromBase)
                    {
                        // If too far from base, walk towards the base.
                        s16 yaw = Math_Vec3f_Yaw(&en->actor.world.pos, &en->actor.home.pos);
                        en->actor.world.rot.y = yaw;

                        DiffVect = (Vec3f){en->settings.movementDistance * Math_SinS(yaw), 
                                           0,
                                           en->settings.movementDistance * Math_CosS(yaw)};
                    }
                    else
                    {
                        DiffVect.x = Math_RandGetBetween(-en->settings.movementDistance, en->settings.movementDistance);
                        DiffVect.z = Math_RandGetBetween(-en->settings.movementDistance, en->settings.movementDistance);

                        if (ignoreY)
                            DiffVect.y = 0;
                        else
                            DiffVect.y = Math_RandGetBetween(-en->settings.movementDistance, en->settings.movementDistance);
                    }

                    Math_Vec3f_Sum(&en->actor.world.pos, &DiffVect, &en->movementNextPos);
                    Movement_SetNextPos(en, &en->movementNextPos);

                    en->movementDelayCounter = 0;
                    Movement_SetNextDelay(en);
                }
            }
            // Checking if we've bumbled into something or if we've reached the destination or travelled far enough. If we have, stop moving.
            else if (en->traversedDistance - en->lastTraversedDistance < speed || tooFarFromBase || reachedDest)
                Movement_StopMoving(en, playState, setAnims);

            break;
        }
        // In this case, we simply set the next position to be Link's.
        // If the actor is close enough, we stop.
        case MOVEMENT_FOLLOW:
        {
            float minDist = GET_PLAYER(playState)->cylinder.dim.radius + en->settings.collisionRadius + en->settings.movementDistance;

            if (en->actor.xzDistToPlayer > minDist || en->actor.yDistToPlayer > minDist)
                Movement_SetNextPos(en, &GET_PLAYER(playState)->actor.world.pos);
            else
                Movement_StopMoving(en, playState, setAnims);
            
            break;
        }
        // In this case, if the actor is close enough, we set a point away from Link.
        case MOVEMENT_RUN_AWAY:
        {
            float minDist = en->settings.collisionRadius * LINK_RUNAWAY_RADIUS_MULTIPLIER;       

            if (en->actor.xzDistToPlayer < minDist && !en->isMoving)
            {
                Vec3f vector = {en->settings.movementDistance * Math_SinS(GET_PLAYER(playState)->actor.world.rot.y), 
                                0, 
                                en->settings.movementDistance * Math_CosS(GET_PLAYER(playState)->actor.world.rot.y)};
                                  
                Math_Vec3f_Sum(&en->actor.world.pos, &vector, &en->movementNextPos);
                Movement_SetNextPos(en, &en->movementNextPos);
            }
            else if (Movement_HasReachedDestination(en, MOVEMENT_DISTANCE_EQUAL_MARGIN))
                Movement_StopMoving(en, playState, setAnims);
        
            break;
        }
        // In this case, we calculate a point on the path according to current time.
        // This movement type moves the actor on its own.
        case MOVEMENT_TIMED_PATH:
        {
            // We don't do anything if the Path ID is 0 (we're using SharpOcarina's path IDs here, and path 0 doesn't exist) or if the path list address wasn't found.
            // EDIT: SharpOcarina has changed the path indexes, so this is now wrong, but it can't be changed, or it'll break all previously made actors.
            // Great!
            if (en->settings.pathId == INVALID_PATH || playState->setupPathList == NULL || en->curPathNode < 0)
                break;

            // First, we get the total time the path takes, and how much time is left until the end time.
            u32 timeTotal = Movement_GetTotalPathTime(en, playState);
            u32 timeLeft = Movement_GetRemainingPathTime(en, playState);

            if (timeTotal == 0)
                break;
            else
            {
                // We calculate the total path distance, and how much we've completed of it.
                float percentCompl = ((float)(timeTotal - timeLeft) / (float)timeTotal);
                float curSectionProgress = 0;

                // If time indicates we shouldn't yet be moving, we set node to start node
                if (percentCompl <= 0)
                    en->curPathNode = START_NODE(en);    
                // If time indicates we should have already completed the path, we set node to end node
                else if (percentCompl > 1)
                {
                    en->curPathNode = END_NODE(en);
                    curSectionProgress = 1;
                }
                else
                {
                    float totalPathLen = Scene_GetPathLen(playState, 
                                                          PATH_ID(en), 
                                                          START_NODE(en),
                                                          END_NODE(en),
                                                          ignoreY);
                                                    
                    // Next, we calculate the distance we've traversed by multiplying the whole path by the percent of it we've completed.
                    float curProgress = totalPathLen * percentCompl;
                    float pathLen = 0;
                    float sectionLen = 0;
                    int i = 0;

                    // Then, we start calculating where on the path we should be, by getting the length of every node it has and adding it up until we exceed the 
                    // distance we're supposed to have traversed.
                    for (i = 0; i < en->curPathNumNodes - 1; i++)
                    {
                        sectionLen = Scene_GetPathSectionLen(playState, PATH_ID(en), i, ignoreY);
                        pathLen += sectionLen;

                        if (pathLen >= curProgress)
                            break;
                    }

                    en->curPathNode = i;
                    curSectionProgress = 1 - ((float)(pathLen - curProgress) / (float)sectionLen);
                }

                // We know the node we're on now, so we get the start and end points for it...
                Vec3s* sectionStart = Scene_GetPathNodePos(playState, PATH_ID(en), en->curPathNode);
                Vec3s* sectionEnd;

                if (en->curPathNode + 1 >= en->curPathNumNodes)
                    sectionEnd = sectionStart;
                else
                    sectionEnd = Scene_GetPathNodePos(playState, PATH_ID(en), en->curPathNode + 1);

                if (sectionStart != NULL)
                {
                    // Calculate position by adding difference on axis * percentage to the axis
                    en->movementNextPos.x = sectionStart->x + ((sectionEnd->x - sectionStart->x) * curSectionProgress);
                    en->movementNextPos.z = sectionStart->z + ((sectionEnd->z - sectionStart->z) * curSectionProgress);

                    if (ignoreY)
                        en->movementNextPos.y = en->actor.world.pos.y;
                    else
                        en->movementNextPos.y = sectionStart->y + ((sectionEnd->y - sectionStart->y) * curSectionProgress);

                    if (!en->isMoving)
                    {
                        // If we've not yet started moving, set walking animation, and position yourself on path towards the next node.
                        Vec3f fSectionEnd = {sectionEnd->x, sectionEnd->y, sectionEnd->z}; 
                        en->actor.shape.rot.y = Math_Vec3f_Yaw(&en->movementNextPos, &fSectionEnd);
                        en->isMoving = true;
                    }
                    else
                    {
                        // We need to calculate if we've actually moved, or we'll rotate in place due to the smoothing.
                        float dist = DIST_TO_NEXT_POS(en, ignoreY);

                        if (dist > 0)
                        {
                            Setup_Animation(en, playState, ANIM_WALK, true, false, false, !en->autoAnims, false); 

                            // Multiply animation speed according to how quickly time is passing. 
                            u32 time_diff = gSaveContext.dayTime < en->lastDayTime ? 0xFFFF - en->lastDayTime : gSaveContext.dayTime - en->lastDayTime;

                            float anim_speed_mult = MIN((float)time_diff / (float)time_diff, 5);
                            en->skin.skelAnime.playSpeed = en->animations[en->currentAnimId].speed * anim_speed_mult;

                            if (en->settings.smoothingConstant >= 0)
                                Movement_RotTowards(&en->actor.shape.rot.y, Math_Vec3f_Yaw(&en->actor.world.pos, &en->movementNextPos), 0);
                            else
                                en->actor.shape.rot.y = Math_Vec3f_Yaw(&en->actor.world.pos, &en->movementNextPos);
                        }
                        else
                            Setup_Animation(en, playState, ANIM_IDLE, true, false, false, !en->autoAnims, false);
                    }

                    // Actually move the character.
                    en->actor.world.pos = en->movementNextPos;
                }
                break;
            }
        }
        // In this case, we set the next position to the next point on the path.
        case MOVEMENT_PATH:
        {
            // We don't do anything if the Path ID is 0 (we're using SharpOcarina's path IDs here, and path 0 doesn't exist) or if the path list address wasn't found.
            if (en->settings.pathId == INVALID_PATH || playState->setupPathList == NULL || en->curPathNode < 0)
                break;

            // If we're moving, we check if we've reached the destination or travelled far enough. If so, we stop moving, but we do not change
            // the animation back to idle yet, in case we're not actually fully stopping (there are further nodes, or the path is set to loop)
            if (Movement_HasReachedDestination(en, MOVEMENT_DISTANCE_EQUAL_MARGIN + en->settings.smoothingConstant))
            {
                Movement_StopMoving(en, playState, false);

                // If the current path node is the last, or the last in the loop, we check if looping is enabled.
                // If looping is enabled, we set the next path node to loop start, or 0 if undefined. We also set the delay before the path loop restarts.
                // If looping isn't enabled, or we have a delay time set for the loop, THAT's when we change animation to idle.
                // If the current path node isn't the last, nor last in the loop, we move onto the next one.
                if (en->curPathNode == END_NODE(en) || en->curPathNode + 1 == en->curPathNumNodes)
                {
                    if (en->settings.loopPath)
                    {
                        en->curPathNode = START_NODE(en);
                        en->movementDelayCounter = en->settings.movementDelay;

                        if (en->movementDelayCounter != 0)
                            Setup_Animation(en, playState, ANIM_IDLE, true, false, false, !en->autoAnims, false); 
                    }
                    else
                    {
                        en->curPathNode = STOPPED_NODE;
                        Setup_Animation(en, playState, ANIM_IDLE, true, false, false, !en->autoAnims, false); 
                        break;
                    }
                }
                else
                    en->curPathNode++;                
            }

            if (!en->isMoving)
            {
                // Next, we wait for the delay to expire...
                if (en->movementDelayCounter)
                {    
                    en->movementDelayCounter--;
                    break;
                }
                // And set the next node position.
                else
                {   
                    Vec3s* next_node = Scene_GetPathNodePos(playState, PATH_ID(en), en->curPathNode);

                    if (next_node == NULL)
                    {
                        #if LOGGING > 0
                            is64Printf("_Error: Next node was NULL.\n");
                        #endif                         

                        break;
                    }

                    Math_Vec3s_ToVec3f(&en->movementNextPos, next_node);
                    Movement_SetNextPos(en, &en->movementNextPos);
                }
            }
                
            break;
        }
        // In this case, we handle cutscene movement by waiting for the next event and setting the next position and animation according
        // to that.
        case MOVEMENT_CUTSCENE:
        {
            CsCmdActorAction* curActionPtr = playState->csCtx.npcActions[CUTSCENE_ID(en)];
            speed = en->cutsceneMovementSpeed;

            if (curActionPtr != NULL)
            {
                int curFrame = playState->csCtx.frames;
                int framesRemain = curActionPtr->endFrame + 2 - curFrame;

                if (curActionPtr->startFrame < 2 && curFrame > 0)
                    curActionPtr->startFrame = 2;

                if (curActionPtr->startFrame + 1 == curFrame)
                {
                    // Set the animation based on the current action.
                    Setup_Animation(en, playState, curActionPtr->action - 1, true, false, false, false, false);
                    
                    if (en->settings.smoothingConstant < 0)
                    {
                        en->actor.world.pos.x = curActionPtr->startPos.x;
                        en->actor.world.pos.y = curActionPtr->startPos.y;
                        en->actor.world.pos.z = curActionPtr->startPos.z;
                    }

                    // Set start position...
                    en->movementStartPos = en->actor.world.pos;

                    // Set the next position...
                    Vec3f nextPos = {curActionPtr->endPos.x, 
                                     en->settings.ignorePathYAxis ? en->actor.world.pos.y : curActionPtr->endPos.y, 
                                     curActionPtr->endPos.z};
                                        
                    Movement_SetNextPos(en, &nextPos);

                    // Set the speed here and save it for next time.
                    float dist = Math_Vec3f_DistXYZ(&en->movementStartPos, &en->movementNextPos);
                    en->cutsceneMovementSpeed = (dist / (float)(framesRemain == 0 ? 1 : framesRemain));
                    speed = en->cutsceneMovementSpeed;

                    en->stopped = false;
                    en->distanceTotal = DIST_TO_NEXT_POS(en, ignoreY);
                    en->traversedDistance = 0;

                }
                else if (curActionPtr->endFrame + 1 == curFrame)
                    Movement_StopMoving(en, playState, true);
            }
            else
            {
                Movement_StopMoving(en, playState, true);
                speed = 0;
            }            

            break;
        }
        case MOVEMENT_MISC: return;
        case MOVEMENT_NONE:
        default:
        {
            if (!en->stopped || en->isMoving)
                Movement_StopMoving(en, playState, setAnims);

            break;
        }
    }

    // We actually move there.
    Movement_MoveTowardsNextPos(en, playState, speed, movementType, ignoreY, setAnims);

    // Next, we check how much we've moved and add it to the travelled distance.
    en->lastTraversedDistance = en->traversedDistance;
    en->traversedDistance += Math_Vec3f_DistXYZ(&en->actor.world.pos, &pos_before_movement);
}

inline void Movement_SetNextDelay(NpcMaker* en)
{
    if (en->settings.movementDelay == 0)
        en->roamMovementDelay = 0;
    else
        en->roamMovementDelay = Math_RandGetBetween(MINIMUM_RANDOM_DELAY, MINIMUM_RANDOM_DELAY + en->settings.movementDelay);
}

void Movement_SetNextPos(NpcMaker* en, Vec3f* next_pos)
{
    en->isMoving = false;
    en->stopped = false;
    en->movementNextPos = *next_pos;

    #if LOGGING > 1
        is64Printf("_Set next position at %08x, %08x, %08x.\n", en->movementNextPos.x, en->movementNextPos.y, en->movementNextPos.z);
    #endif  
}

void Movement_StopMoving(NpcMaker* en, PlayState* playState, bool stopAnim)
{
    if (stopAnim)
        Setup_Animation(en, playState, ANIM_IDLE, true, false, false, !en->autoAnims, false);

    en->stopped = true;
    en->isMoving = false;
    en->actor.speedXZ = 0;
    en->currentDistToNextPos = 0xFFFFFFFF; 
}

bool Movement_HasReachedDestination(NpcMaker* en, float distance_margin)
{
    bool result = ((en->isMoving && 
                   (en->currentDistToNextPos < distance_margin)) || 
                   (en->isMoving && en->traversedDistance >= en->distanceTotal));

    #if LOGGING > 0
        if (result)
            is64Printf("_Reached end position or travelled far enough.\n");
    #endif  

    return result;
}