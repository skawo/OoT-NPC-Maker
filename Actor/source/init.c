#include "../include/init.h"
#include "../include/movement.h"
#include "../include/scripts_defines.h"
#include "../include/h_rom.h"
#include "../include/h_scene.h"
#include "../include/h_scripts.h"
#include "../include/update.h"

static ColliderCylinderInit npcMakerCollision =
{
    .base = {
              .colType = COLTYPE_NONE,
              .atFlags = AT_NONE,
              .acFlags = AC_TYPE_ALL,
              .ocFlags1 = OC1_ON | OC1_TYPE_ALL,
              .ocFlags2 = OC1_TYPE_ALL,
              .shape = COLSHAPE_CYLINDER
            },
    .info = {
              .elemType = ELEMTYPE_UNK1,
              .toucher = {
                            .dmgFlags = 0x00000000,
                            .effect = 0x00,
                            .damage = 0x00,
                         },

              .bumper = {
                          .dmgFlags = 0xFFCFFFFF,
                          .effect = 0x00,
                          .defense = 0x00,
                        },
              .toucherFlags = TOUCH_ON,
              .bumperFlags = BUMP_ON,
              .ocElemFlags = OCELEM_ON,
            },
    .dim =  {
              .radius = 0x1000,
              .height = 0x1000,
              .yShift = 0x1000,
              .pos = {
                        .x = 0,
                        .y = 0,
                        .z = 0,
                     },
            }
};

void Setup_Defaults(NpcMaker* en, GlobalContext* globalCtx)
{
    en->exSegData = NULL;
    en->animations = NULL;
    en->extraDLists = NULL;
    en->dListColors = NULL;
    en->scripts = NULL;
    en->currentAnimId = -1;
    en->numAnims = 0;
    en->numExColors = 0;
    en->numExDLists = 0;
    en->exSegDataBlSize = 0;
    en->canMove = true;
    en->isMoving = false;
    en->stopped = true;
    en->currentDistToNextPos = 0xFFFFFFFF;
    en->distanceTotal = 0;
    en->traversedDistance = 0;
    en->lastTraversedDistance = 0;
    en->movementDelayCounter = 0;
    en->doBlinkingAnm = true;
    en->doTalkingAnm = true;
    en->cutsceneMovementSpeed = 0;
    en->lightNode = NULL;
    en->movementNextPos = en->actor.world.pos;
    en->autoAnims = true;
    en->isTalking = false;
    en->textboxDisplayed = false;
    en->talkingFinished = false;
    en->customMsgId = NO_CUSTOM_MESSAGE;
    en->messagesDataOffset = 0;
    en->textboxNum = -1;
    en->refActor = &en->actor;
    en->dummyMsgStart = 0xFFFFFFFF;
    Movement_SetNextDelay(en);

    // Get the dummy message.
    en->dummyMesEntry = Rom_GetMessageEntry(DUMMY_MESSAGE);
    
    if (en->dummyMesEntry == NULL)
    {
        #if LOGGING == 1
            osSyncPrintf("_%2d: Did not find message %4x... ", en->npcId, DUMMY_MESSAGE);
        #endif
        
        return;
    }
}

u32 Setup_LoadSection(u32* allocDest, u16* entriesNumberOut, u8* buffer, u32 offset, u32 entrySize, 
                       u32 nullSize, s32 blockSize)
{
    #if LOGGING == 1
        osSyncPrintf("_Loading a section.");
    #endif

    // If block size was passed in the argument, we don't calculate it.
    // This is specifically a workaround so we can reuse this function for loading the scripts.
    if (blockSize < 0)
    {
        *entriesNumberOut = (u16)AVAL(buffer, u32, offset); 
        offset += 4;

        blockSize = (*entriesNumberOut) * entrySize;
    }

    if (blockSize > nullSize)
    {
        *allocDest = (u32)ZeldaArena_Malloc(blockSize);

        if (*allocDest == 0)
        {
            *entriesNumberOut = 0;

            #if LOGGING == 1
                osSyncPrintf("_Failed to allocate section!");
            #endif
        }
        else
            bcopy(buffer + offset, (u32*)*allocDest, blockSize); 
    }
    else
    {
        #if LOGGING == 1
            osSyncPrintf("_No entries defined for section.");
        #endif        

        *entriesNumberOut = 0;
        *allocDest = 0;
    }   

    offset += blockSize;
    return offset;
}

void Setup_ScriptVars(NpcMaker* en, void** ptr, u32 count)
{
    if (count != 0)
        *ptr = ZeldaArena_Malloc(4 * count);
    else
        *ptr = NULL;   

    if (*ptr == NULL && count != 0)
    {
        #if LOGGING == 1
            osSyncPrintf("_%2d: Could not allocate script variables!", en->npcId);
        #endif
    }
    else if (*ptr != NULL)
        Lib_MemSet((void*)*ptr, 4 * count, 0);
}
 

bool Setup_LoadSetup(NpcMaker* en, GlobalContext* globalCtx)
{
    u16 settingsObjectId = en->actor.params;
    en->npcId = en->actor.shape.rot.z;

    #if LOGGING == 1
        osSyncPrintf("_Loading NPC Entry %2d from object %4d.", en->npcId, settingsObjectId);
    #endif

    en->settingsCompressed = Rom_IsObjectCompressed(settingsObjectId);

    if (en->settingsCompressed)
    {
        #if LOGGING == 1
            osSyncPrintf("_Settings object file is compressed! Loading it into RAM...");
        #endif  

        Rom_LoadObjectIfUnloaded(globalCtx, settingsObjectId);
    }

    u32 buf;

    // Load number of entries from ROM...
    Rom_LoadDataFromObject(globalCtx, settingsObjectId, &buf, 0, 4, en->settingsCompressed);
    u32 numEntries = buf;

    // If the selected entry id is bigger than the number of entries, exit.
    if (en->npcId >= numEntries)
    {
        #if LOGGING == 1
            osSyncPrintf("_NPC Entry %2d not found in file", en->npcId);
        #endif

        return false;
    }

    // Load the entry offset...
    Rom_LoadDataFromObject(globalCtx, settingsObjectId, &buf, 4 + (4 * en->npcId), 4, en->settingsCompressed);
    u32 entryAddress = buf;

    // If the entry offset is 0, the actor was nulled.
    if (entryAddress == 0)
    {
        #if LOGGING == 1
            osSyncPrintf("_NPC Entry %2d is null.", en->npcId);
        #endif

        return false;
    }

    // Get entry size...
    Rom_LoadDataFromObject(globalCtx, settingsObjectId, &buf, entryAddress, 4, en->settingsCompressed);
    u32 entrySize = buf;

    // We load the whole entry here to avoid multiple tiny reads from ROM.
    u8* buffer = ZeldaArena_Malloc(entrySize);

    #if LOGGING == 1
        osSyncPrintf("_%2d: Loading entry size bytes: %08x", en->npcId, entrySize);
    #endif

    Rom_LoadDataFromObject(globalCtx, settingsObjectId, buffer, entryAddress + 4, entrySize, en->settingsCompressed);

    // Copy data from the entry...
    bcopy(buffer, &en->settings, sizeof(NpcSettings));
    u32 offset = sizeof(NpcSettings);

    en->messagesDataOffset = AVAL(buffer, u32, offset + 4);
    offset += AVAL(buffer, u32, offset);

    SectionLoad sLoadList[] = 
    {
        {.allocDest = (u32*)&en->animations,  .entriesNumberOut = &en->numAnims,        .entrySize = sizeof(NpcAnimationEntry),  .nullBlockSize = NULL_ANIM_BLOCK_SIZE},
        {.allocDest = (u32*)&en->extraDLists, .entriesNumberOut = &en->numExDLists,     .entrySize = sizeof(ExDListEntry),       .nullBlockSize = NULL_EXDLIST_BLOCK_SIZE},
        {.allocDest = (u32*)&en->dListColors, .entriesNumberOut = &en->numExColors,     .entrySize = sizeof(ColorEntry),         .nullBlockSize = NULL_EX_COLORS_BLOCK_SIZE},
        {.allocDest = (u32*)&en->exSegData,   .entriesNumberOut = &en->exSegDataBlSize, .entrySize = 1,                          .nullBlockSize = NULL_SEG_BLOCK_SIZE},
        {.allocDest = (u32*)&en->scripts,     .entriesNumberOut = NULL,                 .entrySize = 0,                          .nullBlockSize = NULL_SCRIPTS_BLOCK_SIZE},
    };

    #if LOGGING == 1
        osSyncPrintf("_Loading sections: animations, extra display lists, colors, segment data, scripts.");
    #endif

    for (int i = 0; i < ARRAY_COUNT(sLoadList); i++)
    {
        int size = (i == ARRAY_COUNT(sLoadList) - 1) ? entrySize - offset : -1;
        offset = Setup_LoadSection(sLoadList[i].allocDest, sLoadList[i].entriesNumberOut, buffer, offset, sLoadList[i].entrySize, sLoadList[i].nullBlockSize, size);
    }

    #if LOGGING == 1
        osSyncPrintf("_%2d: Allocating script variables...", en->npcId);
    #endif  

    Setup_ScriptVars(en, (void*)&en->scriptVars, en->settings.numVars);
    Setup_ScriptVars(en, (void*)&en->scriptFVars, en->settings.numFVars);

    ZeldaArena_Free(buffer);
    return true;
}

void Setup_Objects(NpcMaker* en, GlobalContext* globalCtx)
{
    // Loading and setting the main object ID.
    if (en->settings.objectId > 0)
    {
        Rom_LoadObjectIfUnloaded(globalCtx, en->settings.objectId);
        Rom_SetObjectToActor(&en->actor, globalCtx, en->settings.objectId);
    }

    for (int i = 0; i < en->numAnims; i++)
        Rom_LoadObjectIfUnloaded(globalCtx, en->animations[i].objectId);

    for (int i = 0; i < en->numExDLists; i++)
        Rom_LoadObjectIfUnloaded(globalCtx, en->extraDLists[i].objectId);

    for (int i = NULL_SEG_BLOCK_SIZE; i < en->exSegDataBlSize; i+=8)
    {
        ExSegDataEntry* ex = (ExSegDataEntry*)AADDR(en->exSegData, i);
        Rom_LoadObjectIfUnloaded(globalCtx, ex->objectId);
    }
}

void Setup_Misc(NpcMaker* en, GlobalContext* globalCtx)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: Setting up collision with radius %04d, height %04d, yoffs %04d", 
                     en->npcId, en->settings.collisionRadius, en->settings.collisionHeight, en->settings.collisionyShift);
    
    if (en->actor.shape.rot.x == 1)
        en->dbgDrawToScreen = true;
    #endif

    en->actor.shape.rot.x = 0;

    Collider_InitCylinder(globalCtx, &en->collider);
    Collider_SetCylinder(globalCtx, &en->collider, &en->actor, &npcMakerCollision);
    en->collider.dim.radius = en->settings.collisionRadius;
    en->collider.dim.height = en->settings.collisionHeight;
    en->collider.dim.yShift = en->settings.collisionyShift;
    Collider_UpdateCylinder(&en->actor, &en->collider);
    
    if (en->settings.castsShadow)
    {
        #if LOGGING == 1
            osSyncPrintf("_%2d: Setting up a shadow with radius %04d.", en->npcId, en->settings.shadowRadius);
        #endif

        ActorShape_Init(&en->actor.shape, 0.0f, ActorShadow_DrawCircle, en->settings.shadowRadius);
        en->actor.shape.shadowAlpha = SHADOW_ALPHA;
    }

    Actor_SetScale(&en->actor, en->settings.modelScale);

    //Setting the starting path, if any.
    Setup_Path(en, globalCtx, en->settings.pathId);

    if (en->settings.generatesLight)
        en->lightNode = LightContext_InsertLight(globalCtx, &globalCtx->lightCtx, &en->light);

    #pragma region Actor flags

        if (en->settings.isTargettable)
            en->actor.flags |= TARGETTABLE_MASK;

        if (en->settings.pushesSwitches)
            en->actor.flags |= PUSH_SWITCHES_MASK;

        if (en->settings.alwaysActive)
            en->actor.flags |= ALWAYS_ACTIVE_MASK;

        if (en->settings.alwaysDrawn)
            en->actor.flags |= ALWAYS_DRAWN_MASK;

        if (en->settings.visibleWithLens)
            en->actor.flags |= DRAWN_WITH_LENS_MASK;

        en->actor.flags |= NO_LIGHT_BIND;

        en->actor.targetMode = en->settings.targetDistance;
        // If the actor is meant to be pushable, we set its mass lower.
        en->actor.colChkInfo.mass = en->settings.mass;

        // Gravity is the inverse of what's set in the editor.
        en->actor.gravity = en->settings.hasCollision ? -en->settings.gravity : 0;
        en->collider.base.colType = en->settings.effectIfAttacked;
        en->curAlpha = en->actor.xzDistToPlayer > FADE_OUT_DISTANCE ? 0 : en->settings.alpha;

        if (en->settings.reactsToAttacks)
            en->collider.base.acFlags |= AC_ON;

    #pragma endregion

    #pragma region Scripts

    if (en->scripts != NULL)
    {
        en->scriptInstances = ZeldaArena_Malloc(en->scripts->numScripts * sizeof(ScriptInstance));
        u32* offset = AADDR(en->scripts, 4 + (4 * en->scripts->numScripts));

        for (int i = 0; i < en->scripts->numScripts; i++)
        {
            en->scriptInstances[i].scriptPtr = AADDR(offset, (u32)en->scripts->scriptDataOffsets[i]);
            en->scriptInstances[i].curInstrNum = 0;
            en->scriptInstances[i].waitTimer = 0;
            en->scriptInstances[i].startInstrNum = 0;
            en->scriptInstances[i].responsesInstrNum = -1;
            en->scriptInstances[i].jumpToWhenReponded = -1;
            en->scriptInstances[i].spotted = 0;
            en->scriptInstances[i].jumpToWhenSpottedInstrNum = -1;
            
            Scripts_FreeTemp(&en->scriptInstances[i]);
        }
    }

    #pragma endregion
}

void Setup_Path(NpcMaker* en, GlobalContext* globalCtx, int pathId)
{
    en->settings.pathId = pathId;
    en->curPathNumNodes = 0;

    if (pathId != INVALID_PATH)
        en->curPathNumNodes = Scene_GetPathNodeCount(globalCtx, PATH_ID(en));
    else
    {
        en->curPathNode = INVALID_NODE;

        #if LOGGING == 1
            osSyncPrintf("_%2d: Tried to setup an invalid path.", en->npcId);
        #endif  

        return;
    }

    if (en->curPathNumNodes == 0)
    {
        #if LOGGING == 1
            osSyncPrintf("_%2d: Requested path doesn't exist, or path list was not found.", en->npcId);
        #endif     

        en->curPathNode = INVALID_NODE;
        en->settings.pathId = INVALID_PATH;
    }
    else
    {
        // Setting the starting path node ID, if it's defined.
        en->curPathNode = START_NODE(en);
    }
}

void Setup_Model(NpcMaker* en, GlobalContext* globalCtx)
{
    if (en->settings.objectId > 0)
    {
        // We assume the model is in Segment 6.
        en->settings.skeleton = OFFSET_ADDRESS(6, en->settings.skeleton);

        switch (en->settings.drawType)
        {
            case OPA_MATRIX:
            case XLU_MATRIX:
            {
                SkelAnime_InitFlex(globalCtx,
                                   &en->skin.skelAnime,
                                   (void*)en->settings.skeleton,
                                   0, 0, 0, 0); 
                                 
                break;
            }
            case OPA_NONMATRIX:
            case XLU_NONMATRIX:
            {
                SkelAnime_Init(globalCtx,
                               &en->skin.skelAnime,
                               (void*)en->settings.skeleton,
                               0, 0, 0, 0); 
                                 
                break;
            }
            case SKIN:
            {
                //z_skelanime_init_weighted
                func_800A663C(globalCtx,
                              &en->skin,
                              (void*)en->settings.skeleton,
                              0); 
                                          
                break;
            }
            default: break;
        }
    }

    if (en->animations[ANIM_IDLE].offset != 0)
    {
        Setup_Animation(en, globalCtx, ANIM_IDLE, false, false, true, false);
        Update_Animations(en, globalCtx);
    }
}

void Setup_Animation(NpcMaker* en, GlobalContext* globalCtx, int animId, bool interpolate, bool playOnce, bool forceSet, bool doNothing)
{
    if (doNothing)
        return;

    if (en->animations == NULL)
    {
        #if LOGGING == 1
            osSyncPrintf("_%2d: Animations are undefined, or couldn't be allocated.", en->npcId, animId);
        #endif      

        return; 
    }

    if (en->currentAnimId != animId || forceSet)
    {
        if (en->numAnims <= animId)
        {
            #if LOGGING == 1
                osSyncPrintf("_%2d: Tried to set animation ID %02d, but it wasn't defined.", en->npcId, animId);
            #endif

            return;
        }

        #if LOGGING == 1
            osSyncPrintf("_%2d: Setting animation ID: %02d", en->npcId, animId);
        #endif

        en->currentAnimId = animId;
        NpcAnimationEntry anim = en->animations[en->currentAnimId];
        
        bool was_set = Setup_AnimationImpl(&en->actor, 
                                             globalCtx, 
                                             &en->skin.skelAnime, 
                                             anim.offset, 
                                             en->settings.animationType, 
                                             R_OBJECT(en, anim.objectId),
                                             en->settings.objectId, 
                                             anim.startFrame, 
                                             anim.endFrame, 
                                             anim.speed, 
                                             interpolate, 
                                             playOnce);

        if (was_set)
            en->animationFinished = false;
    }
}

bool Setup_AnimationImpl(Actor* actor, GlobalContext* globalCtx, SkelAnime* skelanime, int animAddr, int animType, int object, int actorObject,
                           int animStart, int animEnd, float speed, bool interpolate, bool playOnce)
{
#pragma region AnimMode
        /*
            if (anim.start_frame != 0 || anim.end_frame != 255)
            {
                anim_mode = interpolate ? 
                                    play_once ? ANIMMODE_ONCE_INTERP : ANIMMODE_LOOP_PARTIAL_INTERP 
                                    : 
                                    play_once ? ANIMMODE_ONCE : ANIMMODE_LOOP_PARTIAL;
            }
            else
            {
                anim_mode = interpolate ? 
                                    play_once ? ANIMMODE_ONCE_INTERP : ANIMMODE_LOOP_INTERP 
                                    : 
                                    play_once ? ANIMMODE_ONCE : ANIMMODE_LOOP;            
            }
        */

        int animMode;
        
        if (animStart != 0 || animEnd != 255)
            animMode = ANIMMODE_LOOP_PARTIAL + interpolate - (2 * playOnce);
        else
            animMode = interpolate + (2 * playOnce);

#pragma endregion

        switch (animType)
        {
            case ANIMTYPE_LINK:
            {
                animAddr = OFFSET_ADDRESS(4, animAddr);
                
                #if LOGGING == 1
                    osSyncPrintf("_Link animation type at %08x, animation mode %01d", animAddr, animMode);
                #endif

                int endFrame = MIN(Animation_GetLastFrame((void*)animAddr), animEnd);
                int startFrame = MIN(endFrame, animStart);

                LinkAnimation_Change(globalCtx,
                                     skelanime,
                                     (void*)animAddr,
                                     speed,
                                     startFrame,
                                     endFrame, 
                                     animMode,
                                     interpolate ? -4 : -1);

                skelanime->curFrame = animStart;
                skelanime->endFrame = endFrame;

                break;
            }
            case ANIMTYPE_NORMAL:
            {
                animAddr = OFFSET_ADDRESS(6, animAddr);

                #if LOGGING == 1
                    osSyncPrintf("_Normal animation type at %08x, animation mode %01d", animAddr, animMode);
                #endif

                if (actorObject != object && object > 0)
                {
                    if (!Rom_SetObjectToActor(actor, globalCtx, object))
                    {
                        #if LOGGING == 1
                            osSyncPrintf("_Animation needs object %08x, but it's not loaded, so the animation won't play", object);
                        #endif

                        return false;
                    }
                }

                int endFrame = MIN(Animation_GetLastFrame((void*)animAddr), animEnd);
                int startFrame = MIN(endFrame, animStart);
                int animLen = animEnd - startFrame;

                Animation_Change(skelanime,
                                 (void*)animAddr,
                                 speed,
                                 startFrame,
                                 animLen,
                                 animMode,
                                 interpolate ? -4 : -1);

                skelanime->curFrame = animStart;
                skelanime->endFrame = endFrame;

                if (actorObject != object && actorObject > 0)
                    Rom_SetObjectToActor(actor, globalCtx, actorObject);

                break;
            }
            default: break;
        }

        return true;
}