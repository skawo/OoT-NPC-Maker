
#include "../include/init.h"
#include "../include/movement.h"
#include "../include/scripts_defines.h"
#include "../include/h_rom.h"
#include "../include/h_scene.h"
#include "../include/h_scripts.h"
#include "../include/update.h"
#include "../include/npc_maker_user.h"

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

void Setup_Defaults(NpcMaker* en, PlayState* playState)
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
    en->hasStaticExDlists = false;

    en->FindNPCMakerFunction = &Scene_GetNpcMakerByID;
    en->FindActorFunction = &Scene_GetActorByID;
    en->FindCutscenePtrFunction = &Scene_GetCutscenePtr;
    en->FindSceneHeaderFunction = &Scene_GetHeaderPtr;
    en->FindPathPtrFunction = &Scene_GetPathPtr;
    en->GetInternalMsgFunc = &Message_Get;
    en->GetInternalMsgPtrFunc = &Message_GetMessageRAMAddr;
    en->GetInternalMsgDataPtrFunc = &Data_GetCustomMessage;

    for (int i = 0; i < 6; i++)
        en->CFuncs[i] = 0xFFFFFFFF;
    
    for (int i = 0; i < 8; i++)
        en->CFuncsWhen[i] = 0xFF;

    Movement_SetNextDelay(en);

    // Get the message address.
    en->dummyMesEntry = Rom_GetMessageEntry(DUMMY_MESSAGE);
    
    if (en->dummyMesEntry == NULL)
    {
        #if LOGGING > 0
            is64Printf("_%2d: WARNING: Did not find message %4x... \n", en->npcId, DUMMY_MESSAGE);
        #endif
    }

    return;
}

u32 Setup_LoadSection(NpcMaker* en, PlayState* playState, u8* buffer, u32 offset, u32 entryAddress,
                      u32* allocDest, u16* entriesNumberOut,  u32 entrySize, u32 nullSize, bool noCopy, s32 blockSize)
{
    #if LOGGING > 0
        is64Printf("_Loading a section.\n");
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
        // If we're loading from a RAM object, and the data is static, we might as well just use the RAM object data as is, without making a copy.
        if (en->getSettingsFromRAMObject & noCopy)
        {
            void* ptr = Rom_GetObjectDataPtr(en->actor.params, playState);
            *allocDest = (u32)AADDR(ptr, entryAddress + offset);
        }
        else
        {
            *allocDest = (u32)ZeldaArena_Malloc(blockSize);

            if (*allocDest == 0)
            {
                *entriesNumberOut = 0;

                #if LOGGING > 0
                    is64Printf("_Failed to allocate section!\n");
                #endif
            }
            else
                bcopy(buffer + offset, (u32*)*allocDest, blockSize);
        } 
    }
    else
    {
        #if LOGGING > 0
            is64Printf("_No entries defined for section.\n");
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
        #if LOGGING > 0
            is64Printf("_%2d: Could not allocate script variables!\n", en->npcId);
        #endif
    }
    else if (*ptr != NULL)
        Lib_MemSet((void*)*ptr, 4 * count, 0);
}

static u8* Setup_LoadEmbeddedOverlay(NpcMaker* en, PlayState* playState, u8* buffer, u32 offset, u32 len)
{
    #if LOGGING > 0
        is64Printf("_Allocating %2d bytes for embedded overlay.\n", len);
    #endif

    u32 ovlOffset = AVAL(buffer + offset + len, u32, -4);
    OverlayRelocationSection* ovl = (OverlayRelocationSection*)(buffer + offset + len - ovlOffset);

    u8* addr = ZeldaArena_Malloc(len + ovl->bssSize);
    
    #if LOGGING > 0
        is64Printf("_Copying overlay to 0x%8x\n", addr);
    #endif
    
    bcopy(buffer + offset, addr, len);

   
    ovlOffset = AVAL(addr + len, u32, -4);

    #if LOGGING > 0
        is64Printf("_Ovl Offset is at 0x%8x\n", ovlOffset);
    #endif

    ovl = (OverlayRelocationSection*)(addr + len - ovlOffset);

    
    #if LOGGING > 0
        is64Printf("_Relocating section is at 0x%8x\n", ovl);
        is64Printf("_Relocations num is %2d\n", ovl->nRelocations);
    #endif

    Overlay_Relocate(addr, ovl, (u32*)0x80800000);

    #if LOGGING > 0
        is64Printf("_Clearing bss...\n");
    #endif
    
    if (ovl->bssSize != 0)
        bzero((void*)addr + len, ovl->bssSize);    
    
    int size = (uintptr_t)&ovl->relocations[ovl->nRelocations] - (uintptr_t)ovl;
    bzero(ovl, size);   

    #if LOGGING > 0
        is64Printf("_Invalidating cache...\n");
    #endif

    osWritebackDCache(addr, len);
    osInvalICache(addr, len);

    return addr;
}

bool Setup_LoadSetup(NpcMaker* en, PlayState* playState)
{
    static u32 buf[4]; // has to be static, or Wii VC explodes
    bzero(&buf, 16);
    
    u16 settingsObjectId = en->actor.params;

    #if LOGGING > 0
        is64Printf("_Loading NPC Entry %2d from object %4d.\n", en->npcId, settingsObjectId);
    #endif

    #if DIRECT_ROM_LOAD == 1
        en->getSettingsFromRAMObject = Rom_IsObjectCompressed(settingsObjectId);
    #else
        en->getSettingsFromRAMObject = true;
    #endif

    if (en->getSettingsFromRAMObject)
    {
        #if LOGGING > 0
            is64Printf("_%2d: _Loading settings file into RAM...\n", en->npcId);
        #endif  

        int bankIndex = Rom_LoadObjectIfUnloaded(playState, settingsObjectId);
        
        if (!Object_IsLoaded(&playState->objectCtx, bankIndex))
            return false;
    }
    
    en->npcId = en->actor.shape.rot.z;  
    
    // Load number of entries from ROM...
    Rom_LoadDataFromObject(playState, settingsObjectId, &buf, 0, 4, en->getSettingsFromRAMObject);
    u32 numEntries = buf[0];
    
    // If the selected entry id is bigger than the number of entries, exit.
    if (en->npcId >= numEntries)
    {
        #if LOGGING > 0
            is64Printf("_NPC Entry %2d not found in file\n", en->npcId);
        #endif

        Actor_Kill(&en->actor);
        return false;
    }
    
    bzero(&buf, 16);
    
    // Load the entry offset...
    Rom_LoadDataFromObject(playState, settingsObjectId, &buf, 4 + (12 * en->npcId), 16, en->getSettingsFromRAMObject);

    u32 entryAddress = buf[0];
    u32 entrySizeCompr = buf[1];
    u32 entrySize = buf[2];
    u8* buffer;
    
    // If the entry offset is 0, the actor was nulled.
    if (entryAddress == 0 || entrySize == 0)
    {
        #if LOGGING > 0
            is64Printf("_NPC Entry %2d is null.\n", en->npcId);
        #endif

        Actor_Kill(&en->actor);
        return false;
    }
    
    // If compressed size is 0, then the actor is not compressed.
    if (entrySizeCompr)
    {
        Yaz0Header* bufferCompr = (Yaz0Header*)ZeldaArena_Malloc(entrySizeCompr);

        #if LOGGING > 0
            is64Printf("_%2d: Loading compressed entry, size bytes: 0x%08x\n", en->npcId, entrySizeCompr);
        #endif

        Rom_LoadDataFromObject(playState, settingsObjectId, bufferCompr, entryAddress, entrySizeCompr, en->getSettingsFromRAMObject);
        entrySize = bufferCompr->decSize;

        #if LOGGING > 0
            is64Printf("_%2d: Decompressed entry size: 0x%08x\n", en->npcId, entrySize);
        #endif

        buffer = ZeldaArena_Malloc(entrySize);
        Yaz0_DecompressImpl(bufferCompr, buffer);
        ZeldaArena_Free(bufferCompr);
    }
    else
    {
        #if LOGGING > 0
            is64Printf("_%2d: Loading entry size bytes: 0x%08x\n", en->npcId, entrySize);
        #endif

        buffer = ZeldaArena_Malloc(entrySize);
        Rom_LoadDataFromObject(playState, settingsObjectId, buffer, entryAddress, entrySize, en->getSettingsFromRAMObject);
    }

    // Copy data from the entry...
    bcopy(buffer, &en->settings, sizeof(NpcSettings));
    u32 offset = sizeof(NpcSettings);

    if (en->getSettingsFromRAMObject)
    {
        u32 len = AVAL(buffer, u32, offset);
        en->messagesDataOffset = entryAddress + offset + 16;
        en->numLanguages = AVAL(buffer, u32, offset + 8); 
        en->numMessages = AVAL(buffer, u32, offset + 12);         
        offset += len;
    }
    else
    {
        u8* msgBuf = NULL;
        u32 sectionLen = AVAL(buffer, u32, offset);
        en->numLanguages = AVAL(buffer, u32, offset + 8); 
        en->numMessages = AVAL(buffer, u32, offset + 12); 

        u8* dataStart = buffer + offset + 16;

        if (en->numMessages != 0)
        {
            int currentLang = NpcM_GetLanguage();
            
            if (currentLang >= en->numLanguages)
                currentLang = 0;

            if (en->numLanguages == 1)
            {
                // Single language: copy all data as-is
                u32 dataSize = sectionLen - 16;
                msgBuf = ZeldaArena_Malloc(dataSize);
                bcopy(dataStart, msgBuf, dataSize);
            }
            else
            {
                // Multiple languages: copy only the data for that language
                u32 headerSize = en->numMessages * sizeof(InternalMsgEntry);
                u32 langDataOffset = currentLang * headerSize;                

                InternalMsgEntry* langHeaders = (InternalMsgEntry*)(dataStart + langDataOffset);
                InternalMsgEntry* firstMsg = &langHeaders[0];
                InternalMsgEntry* lastMsg = &langHeaders[en->numMessages - 1];
                
                u32 msgDataSize = (lastMsg->offset + lastMsg->msgLen) - firstMsg->offset;
                u32 allocLen = headerSize + msgDataSize;

                msgBuf = ZeldaArena_Malloc(allocLen);

                // Copy headers
                bcopy(langHeaders, msgBuf, headerSize);

                // Copy message data
                u8* srcMsgData = dataStart + firstMsg->offset;
                bcopy(srcMsgData, msgBuf + headerSize, msgDataSize);

                // Adjust header offsets to account for new layout
                u32 offsetAdjustment = firstMsg->offset - headerSize;
                InternalMsgEntry* newHeaders = (InternalMsgEntry*)msgBuf;

                for (int i = 0; i < en->numMessages; i++)
                    newHeaders[i].offset -= offsetAdjustment;
            }
            
            en->messagesDataOffset = (u32)msgBuf;    
        }      

        offset += sectionLen;
    }

    SectionLoad sLoadList[] = 
    {
        {.allocDest = (u32*)&en->animations,  .entriesNumberOut = &en->numAnims,        .entrySize = sizeof(NpcAnimationEntry),  .nullBlockSize = NULL_ANIM_BLOCK_SIZE,      .noCopy = false},
        {.allocDest = (u32*)&en->extraDLists, .entriesNumberOut = &en->numExDLists,     .entrySize = sizeof(ExDListEntry),       .nullBlockSize = NULL_EXDLIST_BLOCK_SIZE,   .noCopy = false},
        {.allocDest = (u32*)&en->dListColors, .entriesNumberOut = &en->numExColors,     .entrySize = sizeof(ColorEntry),         .nullBlockSize = NULL_EX_COLORS_BLOCK_SIZE, .noCopy = true},
        {.allocDest = (u32*)&en->exSegData,   .entriesNumberOut = &en->exSegDataBlSize, .entrySize = 1,                          .nullBlockSize = NULL_SEG_BLOCK_SIZE,       .noCopy = true},
        {.allocDest = (u32*)&en->scripts,     .entriesNumberOut = NULL,                 .entrySize = 0,                          .nullBlockSize = NULL_SCRIPTS_BLOCK_SIZE,   .noCopy = true},
    };

    #if LOGGING > 0
        is64Printf("_%2d: _Loading sections: animations, extra display lists, colors, segment data, overlay, scripts.\n", en->npcId);
    #endif

    for (int i = 0; i < ARRAY_COUNT(sLoadList); i++)
    {
        // Load Overlay
        if (i == 4)
        {
            #if LOGGING > 0
                is64Printf("_Loading embedded overlay...\n");
            #endif

            int overlayLen = AVAL(buffer, u32, offset);

            #if LOGGING > 0
                is64Printf("_Size: 0x%8x", overlayLen);
            #endif

            if (overlayLen != 0xFFFFFFFF)
            {
                offset += 4;
                bcopy(buffer + offset, &en->CFuncs, 24);
                bcopy(buffer + offset + 24, &en->CFuncsWhen, 8);
                offset += 32;

                en->embeddedOverlay = Setup_LoadEmbeddedOverlay(en, playState, buffer, offset, overlayLen);

                #if LOGGING > 0
                    is64Printf("_Embedded overlay loaded!\n");
                #endif

                offset += overlayLen;
            }
            else
            {
                #if LOGGING > 0
                    is64Printf("_There is no embedded overlay.\n");
                #endif

                offset += 4;
            }
        }

        int size = (i == ARRAY_COUNT(sLoadList) - 1) ? entrySize - offset : -1;
        offset = Setup_LoadSection(en, 
                                   playState, 
                                   buffer, 
                                   offset, 
                                   entryAddress, 
                                   sLoadList[i].allocDest, 
                                   sLoadList[i].entriesNumberOut, 
                                   sLoadList[i].entrySize, 
                                   sLoadList[i].nullBlockSize, 
                                   sLoadList[i].noCopy, 
                                   size);
    }

    #if LOGGING > 0
        is64Printf("_%2d: Allocating script variables...\n", en->npcId);
    #endif  

    Setup_ScriptVars(en, (void*)&en->scriptVars, en->settings.numVars);
    Setup_ScriptVars(en, (void*)&en->scriptFVars, en->settings.numFVars);

    ZeldaArena_Free(buffer);
    return true;
}

bool Setup_Objects(NpcMaker* en, PlayState* playState)
{
    // Loading and setting the main object ID.
    if (en->settings.objectId > 0)
    {
        Rom_LoadObjectIfUnloaded(playState, en->settings.objectId);
        if (!Rom_SetObjectToActor(&en->actor, playState, en->settings.objectId, en->settings.fileStart))
            return false;
    }

    for (int i = 0; i < en->numAnims; i++)
        Rom_LoadObjectIfUnloaded(playState, en->animations[i].objectId);

    for (int i = 0; i < en->numExDLists; i++)
        Rom_LoadObjectIfUnloaded(playState, en->extraDLists[i].objectId);

    for (int i = NULL_SEG_BLOCK_SIZE; i < en->exSegDataBlSize; i += 12)
    {
        ExSegDataEntry* ex = (ExSegDataEntry*)AADDR(en->exSegData, i);
        Rom_LoadObjectIfUnloaded(playState, ex->objectId);
    }
    
    return true;
}

void Setup_Misc(NpcMaker* en, PlayState* playState)
{
    #if LOGGING > 0
        is64Printf("_%2d: Setting up collision with radius %04d, height %04d, yoffs %04d\n", 
                     en->npcId, en->settings.collisionRadius, en->settings.collisionHeight, en->settings.collisionyShift);
    #endif
    
    // Only one of these can be enabled
    if (en->settings.showDlistEditorDebugOn && en->settings.showLookAtEditorDebugOn)
        en->settings.showLookAtEditorDebugOn = false;

    Collider_InitCylinder(playState, &en->collider);
    Collider_SetCylinder(playState, &en->collider, &en->actor, &npcMakerCollision);
    en->collider.dim.radius = en->settings.collisionRadius;
    en->collider.dim.height = en->settings.collisionHeight;
    en->collider.dim.yShift = en->settings.collisionyShift;
    Collider_UpdateCylinder(&en->actor, &en->collider);
    
    if (en->settings.castsShadow)
    {
        #if LOGGING > 0
            is64Printf("_%2d: Setting up a shadow with radius %04d.\n", en->npcId, en->settings.shadowRadius);
        #endif

        ActorShape_Init(&en->actor.shape, 0.0f, ActorShadow_DrawCircle, en->settings.shadowRadius);
        en->actor.shape.shadowAlpha = SHADOW_ALPHA;
    }

    Actor_SetScale(&en->actor, en->settings.modelScale);

    //Setting the starting path, if any.
    Setup_Path(en, playState, en->settings.pathId);

    if (en->settings.generatesLight)
        en->lightNode = LightContext_InsertLight(playState, &playState->lightCtx, &en->light);


    en->actor.uncullZoneForward = en->settings.UncullFwd;
    en->actor.uncullZoneDownward = en->settings.UncullDwn;
    en->actor.uncullZoneScale = en->settings.UncullScale;

    #pragma region Actor flags

        if (!en->settings.alwaysActive)
            en->actor.flags &= ~ALWAYS_ACTIVE_MASK;

        if (en->settings.isTargettable)
            en->actor.flags |= TARGETTABLE_MASK;

        if (en->settings.pushesSwitches)
            en->actor.flags |= PUSH_SWITCHES_MASK;

        if (en->settings.alwaysDrawn)
            en->actor.flags |= ALWAYS_DRAWN_MASK;

        if (en->settings.visibleWithLens)
            en->actor.flags |= DRAWN_WITH_LENS_MASK;

        if (en->settings.existsInAllRooms)
            en->actor.room = -1;

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
        #if LOGGING > 0
            is64Printf("_%2d: Allocating space for scripts: 0x%8x\n", en->npcId, en->scripts->numScripts * sizeof(ScriptInstance));
        #endif

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
            en->scriptInstances[i].active = 1;
            
            Scripts_FreeTemp(&en->scriptInstances[i]);
        }

        #if LOGGING > 0
            is64Printf("_%2d: Script init complete.\n", en->npcId);
        #endif            
    }

    #pragma endregion
}

void Setup_Path(NpcMaker* en, PlayState* playState, int pathId)
{
    en->settings.pathId = pathId;
    en->curPathNumNodes = 0;

    if (pathId != INVALID_PATH)
        en->curPathNumNodes = Scene_GetPathNodeCount(playState, PATH_ID(en));
    else
    {
        en->curPathNode = INVALID_NODE;

        #if LOGGING > 0
            is64Printf("_%2d: Tried to setup an invalid path.\n", en->npcId);
        #endif  

        return;
    }

    if (en->curPathNumNodes == 0)
    {
        #if LOGGING > 0
            is64Printf("_%2d: Requested path doesn't exist, or path list was not found.\n", en->npcId);
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

void Setup_Model(NpcMaker* en, PlayState* playState)
{
    #if LOGGING > 0
        is64Printf("_%2d: Setting up model.\n", en->npcId);
    #endif
    
    if (en->settings.objectId > 0)
    {
        // We assume the model is in Segment 6.
        en->settings.skeleton = OFFSET_ADDRESS(6, en->settings.skeleton);

        #if LOGGING > 0
            is64Printf("_%2d: Setting up skeleton at 0x%08x.\n", en->npcId, en->settings.skeleton);
        #endif        

        switch (en->settings.drawType)
        {
            case OPA_MATRIX:
            case XLU_MATRIX:
            {
                SkelAnime_InitFlex(playState,
                                   &en->skin.skelAnime,
                                   (void*)en->settings.skeleton,
                                   0, 0, 0, 0); 
                                 
                break;
            }
            case OPA_NONMATRIX:
            case XLU_NONMATRIX:
            {
                SkelAnime_Init(playState,
                               &en->skin.skelAnime,
                               (void*)en->settings.skeleton,
                               0, 0, 0, 0); 
                                 
                break;
            }
            case SKIN:
            {
                //z_skelanime_init_weighted
                Skin_Init(playState,
                              &en->skin,
                              (void*)en->settings.skeleton,
                              0); 
                                          
                break;
            }
            default: break;
        }
    }

    #if LOGGING > 0
        is64Printf("_%2d: Setting default animation.\n", en->npcId);
    #endif

    if (en->animations[ANIM_IDLE].offset != 0)
    {
        Setup_Animation(en, playState, ANIM_IDLE, false, false, true, false, false);
        Update_Animations(en, playState);
    }

    #if LOGGING > 0
        is64Printf("_%2d: Detecting static ExDlists.\n", en->npcId);
    #endif

    if (en->settings.showDlistEditorDebugOn)
    {
        en->hasStaticExDlists = true;
    }
    else
    {
        for (int i = 0; i < en->numExDLists; i++)
        {
            ExDListEntry dlist = en->extraDLists[i];

            if (dlist.limb < 0)
            {
                en->hasStaticExDlists = true;
                break;
            }
        }
    }

    #if LOGGING > 0
        is64Printf("_%2d: Model initialized.\n", en->npcId);
    #endif
}

void Setup_Animation(NpcMaker* en, PlayState* playState, int animId, bool interpolate, bool playOnce, bool forceSet, bool doNothing, bool external)
{
    if (doNothing)
        return;

    if (en->animations == NULL)
    {
        #if LOGGING > 0
            is64Printf("_%2d: Animations are undefined, or couldn't be allocated.\n", en->npcId, animId);
        #endif      

        return; 
    }

    if (en->currentAnimId != animId || forceSet)
    {
        if (en->numAnims <= animId)
        {
            #if LOGGING > 0
                is64Printf("_%2d: Tried to set animation ID %02d, but it wasn't defined.\n", en->npcId, animId);
            #endif

            return;
        }

        #if LOGGING > 0
            is64Printf("_%2d: Setting animation ID: %02d\n", en->npcId, animId);
        #endif

        NpcAnimationEntry anim = en->animations[animId];
        
        bool was_set = Setup_AnimationImpl(&en->actor, 
                                             playState, 
                                             &en->skin.skelAnime, 
                                             anim.offset, 
                                             en->settings.animationType, 
                                             R_OBJECT(en, anim.objectId),
                                             anim.fileStart,
                                             (R_FILESTART(en, anim.fileStart)),
                                             en->settings.objectId, 
                                             en->settings.fileStart,
                                             anim.startFrame, 
                                             anim.endFrame, 
                                             anim.speed, 
                                             -en->settings.animInterpFrames,
                                             interpolate, 
                                             playOnce,
                                             external);

        if (was_set)
        {
            en->currentAnimId = animId;
            en->animationFinished = false;
        }
    }
}

bool Setup_AnimationImpl(Actor* actor, PlayState* playState, SkelAnime* skelanime, int animAddr, int animType, int object, int fileStart, int rFileStart, int actorObject, int actorObjectFileStart,
                           int animStart, int animEnd, float speed, int interpolateFrames, bool interpolate, bool playOnce, bool external)
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
                
                #if LOGGING > 1
                    is64Printf("_Link animation type at 0x%08x, animation mode %01d\n", animAddr, animMode);
                #endif

                int endFrame = MIN(Animation_GetLastFrame((void*)animAddr), animEnd);
                int startFrame = MIN(endFrame, animStart);

                LinkAnimation_Change(playState,
                                     skelanime,
                                     (void*)animAddr,
                                     speed,
                                     startFrame,
                                     endFrame, 
                                     animMode,
                                     interpolate ? interpolateFrames : -1);

                skelanime->curFrame = animStart;
                skelanime->endFrame = endFrame;

                break;
            }
            case ANIMTYPE_NORMAL:
            {
                animAddr = OFFSET_ADDRESS(6, animAddr);

                #if LOGGING > 1
                    is64Printf("_Normal animation type at 0x%08x, animation mode %01d\n", animAddr, animMode);
                #endif

                if (external || (actorObject != object && object > 0) || (fileStart != OBJECT_CURRENT))
                {
                    if (!Rom_SetObjectToActor(actor, playState, object, rFileStart))
                    {
                        #if LOGGING > 0
                            is64Printf("_Animation needs object 0x%08x, but it's not loaded, so the animation won't play\n", object);
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
                                 interpolate ? interpolateFrames : -1);

                skelanime->curFrame = animStart;
                skelanime->endFrame = endFrame;

                if (external || (actorObject != object && actorObject > 0) || (fileStart != OBJECT_CURRENT))
                    Rom_SetObjectToActor(actor, playState, actorObject, actorObjectFileStart);

                break;
            }
            default: break;
        }

        return true;
}