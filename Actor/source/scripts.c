#include "../include/scripts.h"
#include "../include/scripts_defines.h"
#include "../include/h_scripts.h"
#include "../include/init.h"
#include "../include/movement.h"
#include "../include/h_movement.h"
#include "../include/h_scene.h"
#include "../include/h_data.h"
#include "../include/h_rom.h"
#include "../include/h_math.h"
#include "../include/draw.h"
#include "../include/scripts_data.h"

// Used as the update function for the player whenever animate mode is set.
// The actor which turns on the animate mode MUST unset it in the same state as when the animate mode was set!
// (e.g if animate mode is set while textbox is on screen, it must be unset while textbox is on screen)
void Scripts_PlayerAnimateMode(Player* pl, PlayState* playState)
{
    LinkAnimation_Update(playState, &pl->skelAnime);
}

inline ScrInstr* Scripts_GetInstrPtr(ScriptInstance* script, u32 instruction_num)
{
    u32 curOffs = AVAL(script->scriptPtr, u16, SCRIPT_INSTR_SIZE * instruction_num);
    return (ScrInstr*)AADDR(script->scriptPtr, curOffs * 4);
}

void Scripts_Main(NpcMaker* en, PlayState* playState)
{
    #if LOGGING > 2
        is64Printf("_%2d: ******* Scripts ******* \n", en->npcId, playState->gameplayFrames);
    #endif
    
    for (int i = 0; i < en->scripts->numScripts; i++)
    {
        #if LOGGING > 3
            en->curScriptNum = i;
        #endif

        ScriptInstance* script = &en->scriptInstances[i];

        if (script->scriptPtr != NULL)
        {
            if (script->active)
            {
                // Player responding to a textbox may desync from the script (especially if the player mashes the button), 
                // so we need to check for this before executing anything.
                Scripts_ResponseInstruction(en, playState, script);

                // Player getting spotted due to a search particle.
                if (script->spotted && script->jumpToWhenSpottedInstrNum >= 0)
                {
                    script->curInstrNum = script->jumpToWhenSpottedInstrNum;
                    script->spotted = 0;
                    script->jumpToWhenSpottedInstrNum = -1;
                }

                if (script->waitTimer != 0)
                {
                    #if LOGGING > 3
                        is64Printf("_[%2d, %1d]: WAITING for %02d more frames.\n", en->npcId, en->curScriptNum, script->waitTimer);
                    #endif

                    script->waitTimer--;
                }
                else
                    while(Scripts_Execute(en, playState, script));
            }
        }   
    }

    #if LOGGING > 2
        is64Printf("_%2d: ******* Scripts End *******\n", en->npcId, playState->gameplayFrames);
    #endif
}

void Scripts_ResponseInstruction(NpcMaker* en, PlayState* playState, ScriptInstance* script)
{
    if (script->responsesInstrNum < 0)
        return;

    ScrInstrResponsesSet* instruction = (ScrInstrResponsesSet*)Scripts_GetInstrPtr(script, script->responsesInstrNum);

    // Wait until the selection pops up and player has responded to it...
    // Player talk state, player responded to textbox
    if (Message_GetState(&playState->msgCtx) == TEXT_STATE_CHOICE && Message_ShouldAdvance(playState))
        script->jumpToWhenReponded = instruction->respInstrNum[playState->msgCtx.choiceIndex];

    // If waiting for response, and a selection has been picked, jump to the designated instruction.
    if (en->isWaitingForResponse && script->jumpToWhenReponded >= 0)
    {
        if (script->jumpToWhenReponded == SCRIPT_RETURN)
            script->curInstrNum = script->startInstrNum;
        else
            script->curInstrNum = script->jumpToWhenReponded;

        script->jumpToWhenReponded = -1;
        script->responsesInstrNum = -1;
        en->isWaitingForResponse = false;
    }
}

void* ScriptFuncs[] = 
{   &Scripts_InstructionIf,                 // IF
    &Scripts_InstructionIf,                 // WHILE
    &Scripts_InstructionAwait,              // AWAIT
    &Scripts_InstructionSet,                // SET
    &Scripts_InstructionNop,                // TALK
    &Scripts_InstructionForceTalk,          // FORCE_TALK
    &Scripts_InstructionEnableTrade,        // TRADE
    &Scripts_InstructionEnableTalking,      // ENABLE_TALKING
    &Scripts_InstructionShowTextbox,        // SHOW_TEXTBOX
    &Scripts_InstructionShowTextbox,        // SHOW_TEXTBOX_SP
    &Scripts_InstructionCloseTextbox,       // CLOSE_TEXTBOX
    &Scripts_InstructionItem,               // ITEM
    &Scripts_InstructionPlay,               // PLAY
    &Scripts_InstructionScript,             // SCRIPT
    &Scripts_InstructionKill,               // KILL
    &Scripts_InstructionSpawn,              // SPAWN
    &Scripts_InstructionWarp,               // WARP
    &Scripts_InstructionRotation,           // ROTATION
    &Scripts_InstructionPosition,           // POSITION
    &Scripts_InstructionScale,              // SCALE
    &Scripts_InstructionFace,               // FACE
    &Scripts_InstructionParticle,           // PARTICLE
    &Scripts_InstructionOcarina,            // OCARINA
    &Scripts_InstructionNop,                // PICKUP
    &Scripts_InstructionNop,                // RETURN
    &Scripts_InstructionGoto,               // GOTO
    &Scripts_InstructionNop,                // LABEL
    &Scripts_InstructionSave,               // SAVE
    &Scripts_InstructionFadeIn,             // FADEIN
    &Scripts_InstructionFadeOut,            // FADEOUT
    &Scripts_InstructionQuake,              // QUAKE
    &Scripts_InstructionCCall,              // CCALL
    &Scripts_InstructionGet,                // GET
    &Scripts_InstructionGotoVar,            // GOTOVAR
    &Scripts_InstructionStop,               // STOP
    &Scripts_InstructionNop,                // NOP
};

bool Scripts_Execute(NpcMaker* en, PlayState* playState, ScriptInstance* script)
{
    ScrInstr* instruction = Scripts_GetInstrPtr(script, script->curInstrNum);

    typedef bool ScriptFunc(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstr* in);
    ScriptFunc* f = (ScriptFunc*)ScriptFuncs[instruction->id];
    return f(en, playState, script, instruction);
}

bool Scripts_InstructionSave(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstr* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: SAVE\n", en->npcId, en->curScriptNum);
    #endif      

    Play_SaveSceneFlags(playState);
    Sram_WriteSave(&playState->sramCtx);

    script->curInstrNum++; 
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionGet(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrGetExtVar* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d], : GET\n", en->npcId, en->curScriptNum);
    #endif          

    switch (in->subid)
    {
        case GET_EXT_VAR:
        case GET_EXT_VARF:
        {
            u32 actor_id = Scripts_GetVarval(en, playState, in->varTypeActorNum, in->ActorNum, false);
            float out = 0;

            if (in->subid == GET_EXT_VAR)
                out = Scene_GetNpcMakerByID(en, playState, actor_id)->scriptVars[in->extvarnum - 1];
            else
                out = Scene_GetNpcMakerByID(en, playState, actor_id)->scriptFVars[in->extvarnum - 1];

            u32 valt;
            void* addr = Scripts_RamSubIdSetup(en, playState, in->DestVar.ui32, SUBT_GLOBAL8 - 2 + in->varTypeDestVar, &valt);

            switch (valt)
            {
                case INT8:      AVAL(addr, s8, 0) = out; break;
                case INT16:     AVAL(addr, s16, 0) = out; break;
                case INT32:     AVAL(addr, s32, 0) = out; break;
                case FLOAT:     AVAL(addr, float, 0) = out; break;
                default: break;
            }            
        }
        default: break;
    }

    script->curInstrNum++; 
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionCCall(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrCCall* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d], : CCALL\n", en->npcId, en->curScriptNum);
    #endif      

    float args[in->numArgs]; 

    if (in->numArgs)
    {
        for (int i = 0; i < in->numArgs; i++)
            args[i] = Scripts_GetVarval(en, playState, ((in->varTypeArgs[i / 2]) >> (i % 2 ? 0 : 4)) & 0xF, in->Arg[i], false); 
    }

    float out = NpcMaker_RunCFunc(en, playState, in->funcOffs, in->numArgs ? args : NULL); 
    
    if (in->varType > 1)
    {
        u32 valt;
        void* addr = Scripts_RamSubIdSetup(en, playState, in->DestVar.ui32, SUBT_GLOBAL8 - 2 + in->varType, &valt);

        switch (valt)
        {
            case INT8:      AVAL(addr, s8, 0) = out; break;
            case INT16:     AVAL(addr, s16, 0) = out; break;
            case INT32:     AVAL(addr, s32, 0) = out; break;
            case FLOAT:     AVAL(addr, float, 0) = out; break;
            default: break;
        }
    }

    script->curInstrNum++; 
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionQuake(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrQuake* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: QUAKE\n", en->npcId, en->curScriptNum);
    #endif      

    float speed = Scripts_GetVarval(en, playState, in->varTypeSpeed, in->speed, false);
    float dur = Scripts_GetVarval(en, playState, in->varTypeDuration, in->duration, false);
    float type =  Scripts_GetVarval(en, playState, in->varTypeType, in->type, false);
    
    float x =  Scripts_GetVarval(en, playState, in->varTypeX, in->x, false);
    float y =  Scripts_GetVarval(en, playState, in->varTypeY, in->y, false);
    float zrot =  Scripts_GetVarval(en, playState, in->varTypeZRot, in->zrot, false);
    float zoom =  Scripts_GetVarval(en, playState, in->varTypeZoom, in->zoom, false);

    s16 quakeId = Quake_Add(GET_ACTIVE_CAM(playState), type);
    Quake_SetSpeed(quakeId, speed);
    Quake_SetQuakeValues(quakeId, x, y, zoom, zrot);
    Quake_SetCountdown(quakeId, dur);

    Rumble_Request(en->actor.xyzDistToPlayerSq, 255, dur, 150);

    script->curInstrNum++; 
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionFadeIn(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrFade* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: FADEIN\n", en->npcId, en->curScriptNum);
    #endif      

    if (playState->envCtx.screenFillColor[3] != 0)
    {
        float rate = Scripts_GetVarval(en, playState, in->varTypeRate, in->rate, false);

        if (rate >= playState->envCtx.screenFillColor[3])
        {
            playState->envCtx.screenFillColor[3] = 0;
            playState->envCtx.fillScreen = 0;
        }
        else
        {
            playState->envCtx.screenFillColor[3] -= rate;
            return SCRIPT_STOP;
        }
    }

    script->curInstrNum++; 
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionFadeOut(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrFade* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: FADEOUT\n", en->npcId, en->curScriptNum);
    #endif      

    bool firstRun = Scripts_SetupTemp(script, in);

    if (firstRun)
    {
        playState->envCtx.screenFillColor[0] = Scripts_GetVarval(en, playState, in->varTypeR, in->R, false);
        playState->envCtx.screenFillColor[1] = Scripts_GetVarval(en, playState, in->varTypeG, in->G, false);
        playState->envCtx.screenFillColor[2] = Scripts_GetVarval(en, playState, in->varTypeB, in->B, false);
        playState->envCtx.fillScreen = 1;
    }
    
    int alphaCur = playState->envCtx.screenFillColor[3];

    if (alphaCur != 255)
    {
        float rate = Scripts_GetVarval(en, playState, in->varTypeRate, in->rate, false);

        if (rate + alphaCur >= 255)
            playState->envCtx.screenFillColor[3] = 255;
        else
        {
            playState->envCtx.screenFillColor[3] += rate;
            return SCRIPT_STOP;
        }
    }

    return Scripts_FreeAndContinue(script);
}


bool Scripts_InstructionNop(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstr* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: NOP\n", en->npcId, en->curScriptNum);
    #endif      
    
    script->curInstrNum++; 
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionCloseTextbox(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstr* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: CLOSE TEXTBOX\n", en->npcId, en->curScriptNum);
    #endif      
    
    playState->msgCtx.msgMode = MSGMODE_TEXT_CLOSING; 
    script->curInstrNum++; 
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionForceTalk(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstr* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: FORCE TALK\n", en->npcId, en->curScriptNum);
    #endif  
    
    en->isTalking = true; 
    playState->talkWithPlayer(playState, &en->actor); 
    script->curInstrNum++; 
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionParticle(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrParticle* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: PARTICLE\n", en->npcId, en->curScriptNum);
    #endif

    Vec3f pos = Scripts_GetVarvalVec3f(en, playState, (Vartype[]){in->posXType, in->posYType, in->posZType}, (ScriptVarval[]){in->posX, in->posY, in->posZ}, 1);

    Actor* subject = &en->actor;

    if (in->posType >= 3)
        subject = en->refActor;

    if (in->posType)
    {
        if (in->posType % 2)
        {
            if (in->type != PARTICLE_FIRE_TAIL)
                Math_Vec3f_Sum(&pos, &subject->world.pos, &pos);
        } 
        else 
        {
            Math_AffectMatrixByRot(subject->shape.rot.y, &pos, NULL);
            Math_Vec3f_Sum(&pos, &subject->world.pos, &pos);
        }
    }


    Vec3f accel = Scripts_GetVarvalVec3f(en, playState, (Vartype[]){in->accelXType, in->accelYType, in->accelZType}, (ScriptVarval[]){in->accelX, in->accelY, in->accelZ}, 100);
    Vec3f vel = Scripts_GetVarvalVec3f(en, playState, (Vartype[]){in->velXType, in->velYType, in->velZType}, (ScriptVarval[]){in->velX, in->velY, in->velZ}, 100);
    Color_RGBA8 prim = Scripts_GetVarvalRGBA(en, playState, (Vartype[]){in->primRType, in->primGType, in->primBType, in->primAType}, (ScriptVarval[]){in->primR, in->primG, in->primB, in->primA});
    Color_RGBA8 env = Scripts_GetVarvalRGBA(en, playState, (Vartype[]){in->envRType, in->envGType, in->envBType, in->envAType}, (ScriptVarval[]){in->envR, in->envG, in->envB, in->envA});
    float scale = Scripts_GetVarval(en, playState, in->scaleType, in->scale, true);
    float scaleUpd = Scripts_GetVarval(en, playState, in->scaleUpdType, in->scaleUpdate, true);
    float life = Scripts_GetVarval(en, playState, in->lifeType, in->life, true);
    float var = Scripts_GetVarval(en, playState, in->varType, in->var, false);

    switch (in->type)
    {
        case PARTICLE_DUST:             EffectSsDust_Spawn(playState, 0, &pos, &vel, &accel, &prim, &env, scale, scaleUpd, life, 0); break;
        case PARTICLE_EXPLOSION:        EffectSsBomb2_SpawnLayered(playState, &pos, &vel, &accel, scale, scaleUpd); break;
        case PARTICLE_SPARK:            EffectSsGSpk_SpawnAccel(playState, &en->actor, &pos, &vel, &accel, &prim, &env, scale, scaleUpd); break;
        case PARTICLE_BUBBLE:           EffectSsDtBubble_SpawnCustomColor(playState, &pos, &vel, &accel, &prim, &env, scale, life, var); break;
        case PARTICLE_WATER_SPLASH:     EffectSsSibuki_SpawnBurst(playState, &pos); break;
        case PARTICLE_SMOKE:            EffectSsSibuki2_Spawn(playState, &pos, &vel, &accel, scale); break;
        case PARTICLE_ICE_CHUNK:        EffectSsEnIce_Spawn(playState, &pos, scale, &vel, &accel, &prim, &env, life); break;
        case PARTICLE_ICE_BURST:        EffectSsIcePiece_SpawnBurst(playState, &pos, scale); break;
        case PARTICLE_RED_FLAME:        EffectSsKFire_Spawn(playState, &pos, &vel, &accel, scale, 100); break;
        case PARTICLE_BLUE_FLAME:       EffectSsKFire_Spawn(playState, &pos, &vel, &accel, scale, 0); break;
        case PARTICLE_ELECTRICITY:      EffectSsFhgFlash_SpawnShock(playState, &en->actor, &pos, scale, 0); break;
        case PARTICLE_FOCUSED_STAR:     EffectSsKiraKira_SpawnFocused(playState, &pos, &vel, &accel, &prim, &env, scale, life); break;
        case PARTICLE_DISPERSED_STAR:   EffectSsKiraKira_SpawnDispersed(playState, &pos, &vel, &accel, &prim, &env, scale, life); break;
        case PARTICLE_BURN_MARK:        EffectSsDeadDs_Spawn(playState, &pos, &vel, &accel, scale, scaleUpd, var, life); break;
        case PARTICLE_RING:             EffectSsBlast_Spawn(playState, &pos, &vel, &accel, &prim, &env, scale, scaleUpd, var, life); break;
        case PARTICLE_FLAME:            EffectSsDeadDb_Spawn(playState, &pos, &vel, &accel, scale, scaleUpd, prim.r, prim.g, prim.b, prim.a, env.r, env.g, env.b, env.a, life, 0); break;
        case PARTICLE_FIRE_TAIL:        EffectSsFireTail_Spawn(playState, subject, &pos, scale, &vel, 0, &prim, &env, var, -1, life); break;
        case PARTICLE_HIT_MARK_FLASH:     
        case PARTICLE_HIT_MARK_DUST:  
        case PARTICLE_HIT_MARK_BURST:  
        case PARTICLE_HIT_MARK_SPARK:  
                                        EffectSsHitMark_Spawn(playState, in->type - PARTICLE_HIT_MARK_FLASH, scale, &pos); break;
                                        
        // Requires OBJECT_FHG
        case PARTICLE_LIGHT_POINT:      EffectSsFhgFlash_SpawnLightBall(playState, &pos, &vel, &accel, scale, var); break; 
        // Requires OBJECT_YABUSAME_POINT
        case PARTICLE_SCORE:            EffectSsExtra_Spawn(playState, &pos, &vel, &accel, scale, var); break;
        // Requires OBJECT_DODONGO
        case PARTICLE_DODONGO_FIRE:     EffectSsDFire_Spawn(playState, &pos, &vel, &accel, scale, scaleUpd, var, prim.r, life); break;
        // Requires OBJECT_FZ
        case PARTICLE_FREEZARD_SMOKE:   EffectSsIceSmoke_Spawn(playState, &pos, &vel, &accel, scale); break;

        case PARTICLE_LIGHTNING:        
        {
            s16 yaw = Scripts_GetVarval(en, playState, in->yawType, in->yaw, true);
            EffectSsLightning_Spawn(playState, &pos, &prim, &env, scale, yaw, life, var);
            break;
        }
        case PARTICLE_DISPLAY_LIST: 
        {
            s16 exDlistIndex = Scripts_GetVarval(en, playState, in->dListType, in->dList, true);
            ExDListEntry exDList;
            Gfx* offset = NULL;

            if (exDlistIndex < 0)
            {
                // Hahen has a default object.
                exDList.objectId = -1;
                exDList.scale = 35.0f;
            }
            else
            {
                exDList = en->extraDLists[(int)exDlistIndex];
                exDList.objectId = R_OBJECT(en, exDList.objectId);
                offset = (Gfx*)(OFFSET_ADDRESS(6, exDList.offset));
            }

            EffectSsHahen_SpawnBurst(playState, &pos, scale, 0, exDList.scale, scaleUpd, var, exDList.objectId, life, offset);
            break;
        }
        case PARTICLE_SEARCH_EFFECT:
        {
            script->jumpToWhenSpottedInstrNum = in->foundInstrNum;  

            Math_AffectMatrixByRot(en->actor.shape.rot.y + en->limbRotA, &vel, NULL);         
            EffectSsSolderSrchBall_Spawn(playState, &pos, &vel, &accel, 0, &script->spotted);
            break;
        }
    }
    script->curInstrNum++;
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionIf(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrIf* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: IF/WHILE with subtype %02d.\n", en->npcId, en->curScriptNum, in->subId);
    #endif

    u16 branch = script->curInstrNum + 1;

    switch (in->subId)
    {
        case IF_FLAG_INF:
        case IF_FLAG_EVENT:
        case IF_FLAG_SWITCH:
        case IF_FLAG_SCENE:
        case IF_FLAG_TREASURE:
        case IF_FLAG_ROOM_CLEAR:
        case IF_FLAG_SCENE_COLLECT:
        case IF_FLAG_TEMPORARY:                 
        case IF_FLAG_INTERNAL:                  branch = Scripts_IfFlag(en, playState, in); break;
                                            
        case IF_LINK_IS_ADULT:                  branch = Scripts_IfBool(en, playState, !playState->linkAgeOnLoad, in); break;
        case IF_IS_DAY:                         branch = Scripts_IfBool(en, playState, MORNING_TIME < gSaveContext.dayTime && NIGHT_TIME > gSaveContext.dayTime, in); break;
        case IF_IS_TALKING:                     branch = Scripts_IfBool(en, playState, en->isTalking, in); break;
        case IF_PLAYER_HAS_EMPTY_BOTTLE:        branch = Scripts_IfBool(en, playState, Inventory_HasEmptyBottle(), in); break;
        case IF_IN_CUTSCENE:                    branch = Scripts_IfBool(en, playState, playState->csCtx.segment != NULL, in); break;
        case IF_TEXTBOX_ON_SCREEN:              branch = Scripts_IfBool(en, playState, Message_GetState(&playState->msgCtx), in); break;    
        case IF_TEXTBOX_DRAWING:                branch = Scripts_IfBool(en, playState, playState->msgCtx.msgMode == MSGMODE_TEXT_DISPLAYING, in); break; 
        case IF_PLAYER_HAS_MAGIC:               branch = Scripts_IfBool(en, playState, gSaveContext.isMagicAcquired, in); break; 
        case IF_ATTACKED:                       branch = Scripts_IfBool(en, playState, en->wasHitThisFrame, in); break; 
        case IF_REF_ACTOR_EXISTS:               branch = Scripts_IfBool(en, playState, en->refActor != NULL, in); break; 
        case IF_PICKUP_IDLE:
        case IF_PICKUP_PICKED_UP:
        case IF_PICKUP_THROWN:
        case IF_PICKUP_LANDED:                  branch = Scripts_IfBool(en, playState, en->pickedUpState == (in->subId - IF_PICKUP_IDLE), in); break; 
        case IF_IS_SPEAKING:                    branch = Scripts_IfBool(en, playState, en->isTalking, in); break; 
        case IF_LENS_OF_TRUTH_ON:               branch = Scripts_IfBool(en, playState, playState->actorCtx.lensActive, in); break; 

        case IF_PLAYER_RUPEES:                  branch = Scripts_IfValue(en, playState, gSaveContext.rupees, in, INT16); break;
        case IF_SCENE_ID:                       branch = Scripts_IfValue(en, playState, playState->sceneId, in, INT16); break;
        case IF_ROOM_ID:                        branch = Scripts_IfValue(en, playState, playState->roomCtx.curRoom.num, in, INT8); break;
        case IF_PLAYER_SKULLTULAS:              branch = Scripts_IfValue(en, playState, gSaveContext.inventory.gsTokens, in, INT16); break;
        case IF_PATH_NODE:                      branch = Scripts_IfValue(en, playState, en->curPathNode, in, INT16); break;
        case IF_ANIMATION_FRAME:                branch = Scripts_IfValue(en, playState, (u16)en->skin.skelAnime.curFrame, in, INT16); break;
        case IF_CUTSCENE_FRAME:                 branch = Scripts_IfValue(en, playState, playState->csCtx.frames, in, INT16); break;
        case IF_PLAYER_HEALTH:                  branch = Scripts_IfValue(en, playState, gSaveContext.health, in, INT16); break;       
        case IF_PLAYER_MAGIC:                   branch = Scripts_IfValue(en, playState, gSaveContext.magic, in, INT16); break;     

#if DEBUG_STRUCT == 1
        case IF_DEBUG_VAR:                      branch = Scripts_IfValue(en, playState, en->dbgVar, in, INT32); break;    
        case IF_DEBUG_VARF:                     branch = Scripts_IfValue(en, playState, en->fDbgVar, in, FLOAT); break;    
#else
        case IF_DEBUG_VAR:                      
        case IF_DEBUG_VARF:                     branch = false; break;  
#endif

        case IF_PLAYER_BOMBS:                   
        case IF_PLAYER_BOMBCHUS:                   
        case IF_PLAYER_ARROWS:                      
        case IF_PLAYER_DEKUNUTS:               
        case IF_PLAYER_DEKUSTICKS:                  
        case IF_PLAYER_BEANS:                    
        case IF_PLAYER_SEEDS:                   branch = Scripts_IfValue(en, playState, gSaveContext.inventory.ammo[inventory_set_slots[in->subId - IF_PLAYER_BOMBS][1]], in, INT16); break;      

        case IF_STICK_X:                        
        {
            ScrInstrIf nn;
            bcopy(in, &nn, sizeof(ScrInstrIf));
            u8 controller = 0;

            if (nn.value.flo < 0)
            {
                while (nn.value.flo <= -0x10000)
                {
                    nn.value.flo += 0x10000;
                    controller++;
                }
            }
            else
            {
                while (nn.value.flo >= 0x10000)
                {
                    nn.value.flo -= 0x10000;
                    controller++;
                }                
            }

            branch = Scripts_IfValue(en, playState, playState->state.input[controller].cur.stick_x, &nn, INT8); 
            break;
        }
        case IF_STICK_Y:                        
        {
            ScrInstrIf nn;
            bcopy(in, &nn, sizeof(ScrInstrIf));
            u8 controller = 0;

            if (nn.value.flo < 0)
            {
                while (nn.value.flo <= -0x10000)
                {
                    nn.value.flo += 0x10000;
                    controller++;
                }
            }
            else
            {
                while (nn.value.flo >= 0x10000)
                {
                    nn.value.flo -= 0x10000;
                    controller++;
                }                
            }

            branch = Scripts_IfValue(en, playState, playState->state.input[controller].cur.stick_y, &nn, INT8); 
            break;
        }
        case IF_CURRENT_STATE:                  
        {
            // Checks current state derived from collision.
            bool t = en->actor.bgCheckFlags & (1 << (int)Scripts_GetVarval(en, playState, in->vartype, in->value, false));
            branch = (t ? in->trueInstrNum : in->falseInstrNum);
            break;
        }
        case IF_ITEM_BEING_TRADED:              
        {
            // If we're not in the trading radius, and not talking, then we're definitely not trading anything.
            if (!en->canTrade && !en->isTalking)
                branch = in->falseInstrNum;
            // Otherwise, check if the item being traded matches
            else
                branch = Scripts_IfValue(en, playState, GET_PLAYER(playState)->exchangeItemId, in, INT32); 

            // If this has happened, then a textbox is gonna be shown, but without the NPC being marked as talked to.
            // This is why we need to artificially set this.
            if (branch == in->trueInstrNum)
            {
                en->isTalking = true;
                en->textboxDisplayed = false;
                en->talkingFinished = false;
            }

            break;
        }
        case IF_TRADE_STATUS:                   
        {
            u32 tradeStatusToCheck = Scripts_GetVarval(en, playState, in->vartype, in->value, false);
            s8 curTraded = GET_PLAYER(playState)->exchangeItemId;

            branch = in->falseInstrNum;

            // Branch to the instruction if taking to the NPC (trading counts as talking) and trading status matches
            if (en->isTalking)
            {
                if (en->tradeItem == curTraded && tradeStatusToCheck == TRADE_SUCCESSFUL)
                    branch = in->trueInstrNum;
                else if (curTraded == EXCH_ITEM_NONE && tradeStatusToCheck == TRADE_TALKED_TO)
                    branch = in->trueInstrNum;
                else if (curTraded != EXCH_ITEM_NONE && tradeStatusToCheck == TRADE_FAILURE)
                    branch = in->trueInstrNum;
            }
            
            break;
        }
        case IF_PLAYER_MASK: branch = Scripts_IfValue(en, playState, GET_PLAYER(playState)->currentMask, in, UINT8); break;
        case IF_TIME_OF_DAY: branch = Scripts_IfValue(en, playState, gSaveContext.dayTime, in, UINT16); break;
        case IF_ANIMATION: branch = Scripts_IfValue(en, playState, en->currentAnimId, in, UINT16); break;
        case IF_PLAYER_HAS_INVENTORY_ITEM: 
        {
            u32 item = (u32)Scripts_GetVarval(en, playState, in->vartype, in->value, false);

            if (IS_BOTTLE_ITEM(item))
                branch = Scripts_IfBool(en, playState, Inventory_HasSpecificBottle(item), in);
            else
            {
                switch (item)
                {
                    case ITEM_BOTTLE:                   branch = Scripts_IfBool(en, playState, Inventory_HasEmptyBottle(), in); break;
                    case UPGRADE_MAGIC:                 branch = Scripts_IfBool(en, playState, gSaveContext.isMagicAcquired, in); break; 
                    case UPGRADE_DOUBLE_MAGIC:          branch = Scripts_IfBool(en, playState, gSaveContext.isDoubleMagicAcquired, in); break; 
                    case UPGRADE_DOUBLE_DEFENCE:        branch = Scripts_IfBool(en, playState, gSaveContext.isDoubleDefenseAcquired, in); break; 
                    default:                            branch = Scripts_IfBool(en, playState, INV_CONTENT(item) == item, in); 
                }
            }

            break; 
        }
        case IF_PLAYER_HAS_QUEST_ITEM: branch = Scripts_IfBool(en, playState, CHECK_QUEST_ITEM((u32)Scripts_GetVarval(en, playState, in->vartype, in->value, false)), in); break; 
        case IF_PLAYER_HAS_DUNGEON_ITEM:
        {
            ScrInstrDoubleIf* instr = (ScrInstrDoubleIf*)in;
            u32 dungeon = Scripts_GetVarval(en, playState, instr->varType1, instr->value1, false);
            u32 item = Scripts_GetVarval(en, playState, instr->varType2, instr->value2, false);

            branch = Scripts_IfBoolTwoValues(en, playState, CHECK_DUNGEON_ITEM(item, dungeon), in); 
            break; 
        }
        case IF_BUTTON_PRESSED: 
        {
            u32 btn = (u32)Scripts_GetVarval(en, playState, in->vartype, in->value, false);
            u8 controller = btn >> 16;
            btn &= 0xFFFF;

            bool pressed = CHECK_BTN_ALL(playState->state.input[controller].press.button, btn);
            branch = Scripts_IfBool(en, playState, pressed, in); 
            break;
        }
        case IF_BUTTON_HELD: 
        {
            u32 btn = (u32)Scripts_GetVarval(en, playState, in->vartype, in->value, false);
            u8 controller = btn >> 16;
            btn &= 0xFFFF;

            bool held = CHECK_BTN_ALL(playState->state.input[controller].cur.button, btn);
            branch = Scripts_IfBool(en, playState, held, in); 
            break;
        }
        case IF_TARGETTED:                  branch = Scripts_IfBool(en, playState, playState->actorCtx.targetCtx.targetedActor == &en->actor, in); break;
        case IF_DISTANCE_FROM_PLAYER:       branch = Scripts_IfValue(en, playState, en->actor.xzDistToPlayer - GET_PLAYER(playState)->cylinder.dim.radius - en->settings.collisionRadius, in, FLOAT); break;
        case IF_DISTANCE_FROM_REF_ACTOR:    branch = Scripts_IfValue(en, playState, Math_Vec3f_DistXZ(&en->actor.world.pos, &en->refActor->world.pos), in, FLOAT); break;     
        case IF_EXT_VAR:
        {
            ScrInstrExtVarIf* instr = (ScrInstrExtVarIf*)in;
            u32 actor_id = Scripts_GetVarval(en, playState, instr->actorNumVarType, instr->actorNum, false);
            NpcMaker* ex_actor = Scene_GetNpcMakerByID(en, playState, actor_id);

            if (ex_actor == NULL)
                branch = instr->falseInstrNum;
            else
                branch = Scripts_IfExtVar(en, playState, (float)ex_actor->scriptVars[instr->extVarNum - 1], in, INT32);
                
            break;
        }
        case IF_EXT_VARF:
        {
            ScrInstrExtVarIf* instr = (ScrInstrExtVarIf*)in;
            u32 actor_id = Scripts_GetVarval(en, playState, instr->actorNumVarType, instr->actorNum, false);
            NpcMaker* ex_actor = Scene_GetNpcMakerByID(en, playState, actor_id);

            if (ex_actor == NULL)
                branch = instr->falseInstrNum;
            else
                branch = Scripts_IfExtVar(en, playState, ex_actor->scriptFVars[instr->extVarNum - 1], in, FLOAT);
                
            break;
        }
        case IF_DAMAGED_BY:
        {            
            int i = 0;

            if (en->collider.info.acHitInfo != 0)
            {
                u32 flags = en->collider.info.acHitInfo->toucher.dmgFlags;

                for (i = 0; i < 0x20; i++, flags >>= 1) 
                {
                    if (flags == 1) 
                        break;
                }
            }    

            branch = Scripts_IfValue(en, playState, i, in, INT32); 
            break;
        }
        case IF_CCALL:
        {
            ScrInstrIfCCall* instr = (ScrInstrIfCCall*)in;

            float args[instr->numArgs];

            if (instr->numArgs)
            {
                for (int i = 0; i < instr->numArgs; i++)
                    args[i] = Scripts_GetVarval(en, playState, ((instr->varTypeArgs[i / 2]) >> (i % 2 ? 0 : 4)) & 0xF, instr->Arg[i], false);
            }

            float out = NpcMaker_RunCFunc(en, playState, instr->funcOffs, instr->numArgs ? args : NULL);
            
            if (!instr->isBool)
                branch = Scripts_IfValueCommon(en, playState, out, FLOAT, instr->condition, instr->varType, instr->value, instr->trueInstrNum, instr->falseInstrNum);
            else
                branch = Scripts_IfCommon(en, playState, out, instr->condition, instr->trueInstrNum, instr->falseInstrNum);

            break;   
        }
        case SUBT_RANDOM:
        {
            ScrInstrDoubleIf* instr = (ScrInstrDoubleIf*)in;
            s16 value = Scripts_GetVarval(en, playState, instr->varType1, instr->value1, true);
            branch = Scripts_IfTwoValues(en, playState, value, in, INT16);
            break;
        }
        case IF_ACTOR_EXISTS:
        {
            void* actor = Scripts_GetActorByType(en, playState, in->condition, in->vartype, in->value);
            branch = (actor != NULL) ? in->trueInstrNum : in->falseInstrNum;
            break;
        }
        case SUBT_GLOBAL8:
        case SUBT_GLOBAL16:
        case SUBT_GLOBAL32:
        case SUBT_GLOBALF:
        case SUBT_ACTOR8:
        case SUBT_ACTOR16:
        case SUBT_ACTOR32:
        case SUBT_ACTORF:
        case SUBT_SAVE8:
        case SUBT_SAVE16:
        case SUBT_SAVE32:
        case SUBT_SAVEF:
        case SUBT_VAR:
        case SUBT_VARF:
        {
            ScrInstrDoubleIf* instr = (ScrInstrDoubleIf*)in;

            u32 valt;
            void* addr = Scripts_RamSubIdSetup(en, playState, instr->value1.ui32, instr->subId, &valt);
            
            if (addr == NULL)
                break;

            float value = 0;

            switch (valt)
            {
                case INT8:      value = AVAL(addr, s8, 0); break;
                case INT16:     value = AVAL(addr, s16, 0); break;
                case INT32:     value = AVAL(addr, s32, 0); break;
                case FLOAT:     value = AVAL(addr, float, 0); break;
                default:        value = 0; break;
            }

            branch = Scripts_IfTwoValues(en, playState, value, in, valt);
            break;
        }
    }


    script->curInstrNum = branch;
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionAwait(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrAwait* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: AWAIT with subtype %02d.\n", en->npcId, en->curScriptNum, in->subId);
    #endif
    
    bool firstRun = Scripts_SetupTemp(script, in);

    // On the first run, generate a new random number and store it.
    if (firstRun)
    {
        script->tempValues[0] = Rand_Next();
        script->tempValues[1] = script->tempValues[0];
    }
    // On every next run, generate the next number and store it, then restore the number from the first run.
    // This is so that if a random value await is used, the same random value will be checked every time, and not a random value generated every frame.
    // After the function runs, we restore the seed that's generated before seeding with the first-run seed,
    // so that the RNG doesn't "pause" in the game while await is running.
    else
    {
        script->tempValues[1] = Rand_Next();
        Rand_Seed(script->tempValues[0]);
    }

    bool conditionMet = true;

    switch (in->subId)
    {
        case AWAIT_FRAMES:                          
        {
            script->waitTimer = Scripts_GetVarval(en, playState, in->varType, in->value, false); 
            script->waitTimer *= (float)((float)3 / (float)R_UPDATE_RATE);
            
            script->curInstrNum++; 
            Rand_Seed(script->tempValues[1]);
            Rand_Next();
            Scripts_FreeTemp(script);
            return SCRIPT_STOP;
        }
        case AWAIT_RESPONSE:                
        {
            Rand_Seed(script->tempValues[1]);
            Rand_Next();
            en->isWaitingForResponse = true;
            return SCRIPT_STOP;
        }
        case AWAIT_FLAG_INF:
        case AWAIT_FLAG_EVENT:
        case AWAIT_FLAG_SWITCH:
        case AWAIT_FLAG_SCENE:
        case AWAIT_FLAG_TREASURE:
        case AWAIT_FLAG_ROOM_CLEAR:
        case AWAIT_FLAG_SCENE_COLLECT:
        case AWAIT_FLAG_TEMPORARY:
        case AWAIT_FLAG_INTERNAL:
        {
            ScrInstrIf instr = (ScrInstrIf){.condition = in->condition, 
                                            .subId = in->subId,
                                            .value = in->value,
                                            .falseInstrNum = 0,
                                            .trueInstrNum = 1};
 
            conditionMet = Scripts_IfFlag(en, playState, &instr);
            break;
        }
        case AWAIT_FOREVER:                         conditionMet = false; break;
        case AWAIT_MOVEMENT_PATH_END:               conditionMet = Scripts_AwaitBool(en, playState, (en->stopped && (en->curPathNode == STOPPED_NODE)), C_TRUE); break;
        case AWAIT_TALKING_END:                     conditionMet = Scripts_AwaitBool(en, playState, en->talkingFinished, C_TRUE); break;
        case AWAIT_TEXTBOX_ON_SCREEN:               conditionMet = Scripts_AwaitBool(en, playState, Message_GetState(&playState->msgCtx) != TEXT_STATE_NONE, in->condition); break;
        case AWAIT_TEXTBOX_DRAWING:                 conditionMet = Scripts_AwaitBool(en, playState, playState->msgCtx.msgMode == MSGMODE_TEXT_DISPLAYING, in->condition); break;
        case AWAIT_TEXTBOX_DISMISSED:               conditionMet = Scripts_AwaitBool(en, playState, playState->msgCtx.msgMode == MSGMODE_TEXT_CLOSING, C_TRUE); break;
        case AWAIT_PATH_NODE:                       conditionMet = Scripts_AwaitValue(en, playState, en->curPathNode, INT16, in->condition, in->varType, in->value); break;
        case AWAIT_ANIMATION_FRAME:                 conditionMet = Scripts_AwaitValue(en, playState, en->skin.skelAnime.curFrame, UINT32, in->condition, in->varType, in->value); break;
        case AWAIT_CUTSCENE_FRAME:                  conditionMet = Scripts_AwaitValue(en, playState, playState->csCtx.frames, UINT16, in->condition, in->varType, in->value); break;
        case AWAIT_TIME_OF_DAY:                     conditionMet = Scripts_AwaitValue(en, playState, gSaveContext.dayTime, UINT16, in->condition, in->varType, in->value); break;
        case AWAIT_TEXTBOX_NUM:                     conditionMet = Scripts_AwaitValue(en, playState, en->textboxNum + 1, INT8, C_MOREOREQ, in->varType, in->value); break;
        case AWAIT_STICK_X:                         
        {
            ScriptVarval v = in->value;
            u8 controller = 0;
            
            if (v.flo < 0)
            {
                while (v.flo <= -0x10000)
                {
                    v.flo += 0x10000;
                    controller++;
                }
            }
            else
            {
                while (v.flo >= 0x10000)
                {
                    v.flo -= 0x10000;
                    controller++;
                }                
            }

            conditionMet = Scripts_AwaitValue(en, playState, playState->state.input[controller].cur.stick_x, INT8, in->condition, in->varType, v); 
            break;

        }
        case AWAIT_STICK_Y:                         
        {
            ScriptVarval v = in->value;
            u8 controller = 0;
            
            if (v.flo < 0)
            {
                while (v.flo <= -0x10000)
                {
                    v.flo += 0x10000;
                    controller++;
                }
            }
            else
            {
                while (v.flo >= 0x10000)
                {
                    v.flo -= 0x10000;
                    controller++;
                }                
            }

            conditionMet = Scripts_AwaitValue(en, playState, playState->state.input[controller].cur.stick_y, INT8, in->condition, in->varType, v); 
            break;
        }
        case AWAIT_BUTTON_HELD:                     
        {
            u32 btn = (u32)Scripts_GetVarval(en, playState, in->varType, in->value, false);
            u8 controller = btn >> 16;
            btn &= 0xFFFF;

            conditionMet = CHECK_BTN_ALL(playState->state.input[controller].cur.button, btn); 
            break;
        }
        case AWAIT_BUTTON_PRESSED:                  
        {
            u32 btn = (u32)Scripts_GetVarval(en, playState, in->varType, in->value, false);
            u8 controller = btn >> 16;
            btn &= 0xFFFF;

            conditionMet = CHECK_BTN_ALL(playState->state.input[controller].press.button, btn); 
            break;
        }
        case AWAIT_ANIMATION_END:                   
        {
            if (firstRun)
                script->tempValues[2] = (s32)en->skin.skelAnime.animation;

            conditionMet = en->animationFinished || ((s32)en->skin.skelAnime.animation != script->tempValues[2]);     
            break;           
        }
        case AWAIT_PLAYER_ANIMATION_END:                   
        {
            if (firstRun)
                script->tempValues[2] = (s32)GET_PLAYER(playState)->skelAnime.animation;

            conditionMet = (GET_PLAYER(playState)->skelAnime.curFrame >= GET_PLAYER(playState)->skelAnime.endFrame - 1 - GET_PLAYER(playState)->skelAnime.playSpeed) || ((s32)GET_PLAYER(playState)->skelAnime.animation != script->tempValues[2]);
            break;               
        }
        case AWAIT_EXT_VAR: 
        case AWAIT_EXT_VARF: 
        {
            ScrInstrExtVarAwait* instr = (ScrInstrExtVarAwait*)in;

            u32 actor_id = Scripts_GetVarval(en, playState, instr->actorNumVarType, instr->actorNum, false);
            NpcMaker* exActor = Scene_GetNpcMakerByID(en, playState, actor_id);

            if (in->subId == AWAIT_EXT_VAR)
                conditionMet = Scripts_AwaitValue(en, playState, exActor->scriptVars[instr->extVarNum - 1], INT32, instr->condition, instr->varType, instr->value);
            else
                conditionMet = Scripts_AwaitValue(en, playState, exActor->scriptFVars[instr->extVarNum - 1], FLOAT, instr->condition, instr->varType, instr->value);

            break;
        }
        case AWAIT_CURRENT_STATE:
        {
            conditionMet = en->actor.bgCheckFlags & (1 << (int)Scripts_GetVarval(en, playState, in->varType, in->value, false));
            break;
        }
        case AWAIT_CCALL:
        {
            ScrInstrAwaitCCall* instr = (ScrInstrAwaitCCall*)in;

            float args[instr->numArgs]; 

            if (instr->numArgs)
            {
                for (int i = 0; i < instr->numArgs; i++)
                    args[i] = Scripts_GetVarval(en, playState, ((instr->varTypeArgs[i / 2]) >> (i % 2 ? 0 : 4)) & 0xF, instr->Arg[i], false);
            }

            float out = NpcMaker_RunCFunc(en, playState, instr->funcOffs, args); 
            conditionMet = Scripts_AwaitValue(en, playState, out, instr->isBool ? BOOL : FLOAT, instr->condition, instr->varType, instr->value);
            break;
        }
        case AWAIT_ACTOR_EXISTS:
        {
            void* actor = Scripts_GetActorByType(en, playState, in->condition, in->varType, in->value);
            conditionMet = (actor != NULL);
            break;
        }
        case SUBT_GLOBAL8:
        case SUBT_GLOBAL16:
        case SUBT_GLOBAL32:
        case SUBT_GLOBALF:
        case SUBT_ACTOR8:
        case SUBT_ACTOR16:
        case SUBT_ACTOR32:
        case SUBT_ACTORF:
        case SUBT_SAVE8:
        case SUBT_SAVE16:
        case SUBT_SAVE32:
        case SUBT_SAVEF:
        case SUBT_VAR:
        case SUBT_VARF:
        {
            ScrInstrDoubleAwait* instr = (ScrInstrDoubleAwait*)in;

            u32 valType;
            float value = 0;
            void* addr = Scripts_RamSubIdSetup(en, playState, instr->value.ui32, instr->subId, &valType);
            
            if (addr == NULL)
                break;

            switch (valType)
            {
                case INT8:      value = AVAL(addr, s8, 0); break;
                case INT16:     value = AVAL(addr, s16, 0); break;
                case INT32:     value = AVAL(addr, s32, 0); break;
                case FLOAT:     value = AVAL(addr, float, 0); break;
                default:        value = 0; break;
            }

            conditionMet = Scripts_AwaitValue(en, playState, value, valType, instr->condition, instr->varType2, instr->value2);
            break;
        }
    }

    // Restoring the new random seed. 
    Rand_Seed(script->tempValues[1]);
    Rand_Next();

    if (conditionMet)
        return Scripts_FreeAndContinue(script);
    else
        return SCRIPT_STOP;
}

bool Scripts_InstructionGoto(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrGoto* in)
{
    #if LOGGING > 3
        if (in->instrNum == 65535)
            is64Printf("_[%2d, %1d]: RETURN\n", en->npcId, en->curScriptNum);
        else
            is64Printf("_[%2d, %1d]: GOTO going to %04d.\n", en->npcId, en->curScriptNum, in->instrNum);
    #endif

    script->curInstrNum = in->instrNum == SCRIPT_RETURN ? script->startInstrNum : in->instrNum;
    return in->instrNum != SCRIPT_RETURN;
}

bool Scripts_InstructionGotoVar(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrGotoVar* in)
{
    u32 instrNum = Scripts_GetVarval(en, playState, in->vartype, in->value, false);

    #if LOGGING > 3
        if (instrNum == 65535)
            is64Printf("_[%2d, %1d]: VARIABLE INDUCED RETURN\n", en->npcId, en->curScriptNum);
        else
            is64Printf("_[%2d, %1d]: GOTOVAR going to %04d.\n", en->npcId, en->curScriptNum, instrNum);
    #endif

    script->curInstrNum = instrNum == SCRIPT_RETURN ? script->startInstrNum : instrNum;
    return instrNum != SCRIPT_RETURN;
}

bool Scripts_InstructionSet(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrSet* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: SET with subtype %02d.\n", en->npcId, en->curScriptNum, in->subId);
    #endif

    switch (in->subId)
    {
        case SET_TARGET_LIMB:                       
        case SET_TARGET_DISTANCE:                  
        case SET_HEAD_LIMB:                         
        case SET_WAIST_LIMB:                                       
        case SET_LOOKAT_TYPE:                       
        case SET_HEAD_VERT_AXIS:                    
        case SET_HEAD_HORIZ_AXIS:                   
        case SET_WAIST_VERT_AXIS:                   
        case SET_WAIST_HORIZ_AXIS:                  
        case SET_CUTSCENE_SLOT:                     
        case SET_BLINK_SEGMENT:                     
        case SET_TALK_SEGMENT:   
        case SET_ANIM_INTERP_FRAMES:         
        case SET_ALPHA:                             Scripts_Set(en, playState, AADDR(en, basic_set_offsets[in->subId]), in, UINT8); break;

        case SET_MOVEMENT_DISTANCE:           
        case SET_MAXIMUM_ROAM:      
        case SET_MOVEMENT_LOOP_DELAY:               
        case SET_ATTACKED_SFX:                   
        case SET_LIGHT_RADIUS:
        case SET_NPC_ID:                            Scripts_Set(en, playState, AADDR(en, basic_set_offsets[in->subId]), in, UINT16); break;
        case SET_CUTSCENE_FRAME:                    
        {
            void Cutscene_Execute(PlayState* play, CutsceneContext* csCtx);
            
            extern void Cutscene_Execute(PlayState* play, CutsceneContext* csCtx);
                #if GAME_VERSION == 0
                    asm("Cutscene_Execute = 0x80068ECC");
                #elif GAME_VERSION == 1
                    asm("Cutscene_Execute = 0x80056A94");
                #endif          
            
            
            playState->csCtx.state = 0;
            Cutscene_SetSegment(playState, playState->csCtx.segment);
            Cutscene_Execute(playState, &playState->csCtx);
            Scripts_Set(en, playState, AADDR(playState, basic_set_offsets[in->subId]), in, UINT16); 
            Cutscene_Execute(playState, &playState->csCtx);

            break;
        }

        case SET_COLLISION_RADIUS:                 
        case SET_COLLISION_HEIGHT:                  
        case SET_MOVEMENT_LOOP_START:               
        case SET_MOVEMENT_LOOP_END:                 
        case SET_COLLISION_YOFFSET:                 
        case SET_TARGET_OFFSET_X:                   
        case SET_TARGET_OFFSET_Y:                   
        case SET_TARGET_OFFSET_Z:                  
        case SET_MODEL_OFFSET_X:                    
        case SET_MODEL_OFFSET_Y:                    
        case SET_MODEL_OFFSET_Z:                    
        case SET_CAMERA_ID:    
        case SET_RIDDEN_NPC:                            
        case SET_LOOKAT_OFFSET_X:                   
        case SET_LOOKAT_OFFSET_Y:                   
        case SET_LOOKAT_OFFSET_Z:                   
        case SET_CURRENT_PATH_NODE:                 
        case SET_CURRENT_ANIMATION_FRAME:           
        case SET_LIGHT_OFFSET_X:                    
        case SET_LIGHT_OFFSET_Y:                    
        case SET_LIGHT_OFFSET_Z:                    
        case SET_TIMED_PATH_START_TIME:             
        case SET_TIMED_PATH_END_TIME:               Scripts_Set(en, playState, AADDR(en, basic_set_offsets[in->subId]), in, INT16); break;

        case SET_MOVEMENT_SPEED:                    
        case SET_TALK_RADIUS:                       
        case SET_SMOOTHING_CONSTANT:                
        case SET_SHADOW_RADIUS:                
        case SET_UNCULL_FORWARD:         
        case SET_UNCULL_DOWN:         
        case SET_UNCULL_SCALE:                      Scripts_Set(en, playState, AADDR(en, basic_set_offsets[in->subId]), in, FLOAT); break;

        case SET_LOOP_MOVEMENT:                     
        case SET_HAS_COLLISION:                    
        case SET_DO_BLINKING_ANIMATIONS:            
        case SET_DO_TALKING_ANIMATIONS:             
        case SET_JUST_SCRIPT:                       
        case SET_OPEN_DOORS:                        
        case SET_MOVEMENT_IGNORE_Y:                 
        case SET_FADES_OUT:                                         
        case SET_LIGHT_GLOW:                        
        case SET_PAUSE_CUTSCENE:       
        case SET_INVISIBLE:                         
        case SET_TALK_PERSIST:                      
        case SET_IS_SPEAKING:                       Scripts_Set(en, playState, AADDR(en, basic_set_offsets[in->subId]), in, BOOL); break;

#if DEBUG_STRUCT == 1
        case SET_DEBUG_VAR:                         Scripts_Set(en, playState, &en->dbgVar, in, INT32); break;
        case SET_DEBUG_VARF:                        Scripts_Set(en, playState, &en->fDbgVar, in, FLOAT); break;
#else
        case SET_DEBUG_VAR:                         
        case SET_DEBUG_VARF:                        break;
#endif
        case SET_TIME_OF_DAY:                       
        {
            bool first_run = Scripts_SetupTemp(script, in);
        
            if (first_run)
            {
                u16 end_time = gSaveContext.dayTime;
                Scripts_MathOperation(&end_time, Scripts_GetVarval(en, playState, in->varType, in->value, true), in->operator, INT16);
                script->tempValues[0] = end_time;
            }

            int time_speed = gSaveContext.dayTime - en->lastDayTime;

            // If time difference is smaller than current time speed, set the time directly.
            if (ABS(en->lastDayTime - script->tempValues[0]) < time_speed)
            {
                gSaveContext.dayTime = script->tempValues[0];
                return Scripts_FreeAndContinue(script);
            }
            else
            {
                // If time difference is larger than current time speed, get the time difference and increase time by <= 0x500 per frame
                // to make the time smoothly advance to that time (can't just set this directly, because if the current time is higher than
                // the destination time, then setting it directly won't work)
                u16 timediff = MIN(ABS(gSaveContext.dayTime - script->tempValues[0]), 0x500);

                if (gSaveContext.dayTime + timediff == script->tempValues[0])
                {
                    gSaveContext.dayTime = script->tempValues[0];
                    return Scripts_FreeAndContinue(script);
                }
                else
                {
                    gSaveContext.dayTime += timediff;
                    return SCRIPT_STOP;
                }
            }

            break;
        }
        case SET_NO_AUTO_ANIM:                      en->autoAnims = !Scripts_GetBool(en, playState, in); break;

        case SET_PRESS_SWITCHES:                
        case SET_IS_TARGETTABLE:                
        case SET_VISIBLE_ONLY_UNDER_LENS:       
        case SET_IS_ALWAYS_ACTIVE:              
        case SET_IS_ALWAYS_DRAWN:                   Scripts_ToggleActorFlag(en, 
                                                                           AADDR(en, toggle_offsets[in->subId - SET_PRESS_SWITCHES][0]), 
                                                                           Scripts_GetBool(en, playState, in), 
                                                                           toggle_offsets[in->subId - SET_PRESS_SWITCHES][1]); break;

        case SET_REACTS_IF_ATTACKED:
        {
            bool val = Scripts_GetBool(en, playState, in);
            en->settings.reactsToAttacks = val;

            if (!en->settings.reactsToAttacks)
            {
                en->collider.base.acFlags &= ~AC_ON;
                en->collider.base.acFlags &= ~AC_HIT;
                en->wasHit = false;
                en->wasHitTimer = 0;
            }
            else
                en->collider.base.acFlags |= AC_ON;
            
            break;
        }
        case SET_EXISTS_IN_ALL_ROOMS:
        {
            bool val = Scripts_GetBool(en, playState, in);
            en->settings.existsInAllRooms = val;
            
            if (en->settings.existsInAllRooms)
                en->actor.room = -1;
            else
                en->actor.room = playState->roomCtx.curRoom.num;

            break;
        }
        case SET_CASTS_SHADOW:
        {
            bool val = Scripts_GetBool(en, playState, in);

            if (!val)
                en->actor.shape.shadowDraw = NULL;
            else
                ActorShape_Init(&en->actor.shape, 0.0f, ActorShadow_DrawCircle, en->settings.shadowRadius);

            en->actor.shape.shadowAlpha = SHADOW_ALPHA;
            en->settings.castsShadow = val;
            break;
        }
        case SET_GENERATES_LIGHT:  
        {
            bool value = Scripts_GetBool(en, playState, in);

            if (value)
            {
                if (en->lightNode == NULL)
                    en->lightNode = LightContext_InsertLight(playState, &playState->lightCtx, &en->light);
            }
            else
            {
                if (en->lightNode != NULL)
                    LightContext_RemoveLight(playState, &playState->lightCtx, en->lightNode);

                en->lightNode = NULL;
            }

            en->settings.generatesLight = value;
            break;
        }
        case SET_MOVEMENT_TYPE:     
        {
            en->actor.home.pos = en->actor.world.pos;
            Scripts_Set(en, playState, &en->settings.movementType, in, UINT8);
            Movement_StopMoving(en, playState, true);
            break;
        }       
        case SET_TALK_MODE:                         
        {
            bool value = Scripts_GetBool(en, playState, in);

            en->autoAnims = !value;
            en->canMove = !value;
            en->stopPlayer = value;

            if (value)
                GET_PLAYER(playState)->stateFlags1 |= PLAYER_STOPPED_MASK;
            else
                GET_PLAYER(playState)->stateFlags1 &= ~PLAYER_STOPPED_MASK;

            break;
        }
        case SET_PLAYER_BOMBS:                      
        case SET_PLAYER_BOMBCHUS:                   
        case SET_PLAYER_ARROWS:                     
        case SET_PLAYER_DEKUNUTS:                   
        case SET_PLAYER_DEKUSTICKS:                 
        case SET_PLAYER_BEANS:                      
        case SET_PLAYER_SEEDS:                      
        case SET_PLAYER_RUPEES:                      
        case SET_PLAYER_HEALTH:                     
        case SET_PLAYER_MAGIC:                      Scripts_SetInventory(en, playState, inventory_set_slots[in->subId - SET_PLAYER_BOMBS], in); break;  

        case SET_ENV_COLOR:                         Scripts_SetColor(en, playState, &en->settings.envColor, in); break;
        case SET_LIGHT_COLOR:                       Scripts_SetColor(en, playState, &en->settings.lightColor, in); break;

        case SET_RESPONSE_ACTIONS:                  script->responsesInstrNum = script->curInstrNum; break;

        case SET_ANIMATION_OBJECT: 
        case SET_ANIMATION_OFFSET:
        case SET_ANIMATION_SPEED:
        case SET_ANIMATION_STARTFRAME:
        case SET_ANIMATION_ENDFRAME:                Scripts_SetAnimation(en, playState, in); break;

        case SET_DLIST_OFFSET:
        case SET_DLIST_TRANS_X:
        case SET_DLIST_TRANS_Y:
        case SET_DLIST_TRANS_Z:
        case SET_DLIST_ROT_X:
        case SET_DLIST_ROT_Y:
        case SET_DLIST_ROT_Z:
        case SET_DLIST_SCALE:
        case SET_DLIST_OBJECT:
        case SET_DLIST_LIMB:                        Scripts_SetDList(en, playState, in); break;
        case SET_DLIST_COLOR:
        {
            ScrInstrDlistColorSet* instr = (ScrInstrDlistColorSet*)in;

            int dlistId = Scripts_GetVarval(en, playState, instr->varTypeDListID, instr->DListId, false);

            en->extraDLists[dlistId].envColor.r = Scripts_GetVarval(en, playState, instr->varTypeR, instr->R, false);
            en->extraDLists[dlistId].envColor.g = Scripts_GetVarval(en, playState, instr->varTypeG, instr->G, false);
            en->extraDLists[dlistId].envColor.b = Scripts_GetVarval(en, playState, instr->varTypeB, instr->B, false);
            break;
        }
        case SET_FLAG_INF:
        case SET_FLAG_EVENT:
        case SET_FLAG_SWITCH:
        case SET_FLAG_SCENE:
        case SET_FLAG_TREASURE:
        case SET_FLAG_ROOM_CLEAR:
        case SET_FLAG_SCENE_COLLECT:
        case SET_FLAG_TEMPORARY:                    
        case SET_FLAG_INTERNAL:                   Scripts_SetFlag(en, playState, in); break;
        case SET_MASS: 
        {
            Scripts_Set(en, playState, &en->settings.mass, in, UINT8);
            en->actor.colChkInfo.mass = en->settings.mass;
            break;
        }
        case SET_GRAVITY_FORCE: 
        {
            Scripts_Set(en, playState, &en->settings.gravity, in, FLOAT); 
            en->actor.gravity = en->settings.gravity;
            break;
        }
        case SET_MOVEMENT_PATH_ID:
        {
            Scripts_Set(en, playState, &en->settings.pathId, in, UINT8);
            Setup_Path(en, playState, en->settings.pathId);
            break;
        }       
        case SET_PLAYER_CAN_MOVE:
        {
            en->stopPlayer = !Scripts_GetBool(en, playState, in);

            if (en->stopPlayer)
                GET_PLAYER(playState)->stateFlags1 |= PLAYER_STOPPED_MASK;
            else
                GET_PLAYER(playState)->stateFlags1 &= ~PLAYER_STOPPED_MASK;

            break;
        }
        case SET_ACTOR_CAN_MOVE:
        {
            en->canMove = Scripts_GetBool(en, playState, in);
            Movement_SetNextDelay(en);
            break;
        }
        case SET_PLAYER_ANIMATE_MODE:
        {
            bool val = Scripts_GetBool(en, playState, in);

            if (val)
            {
                if ((void*)GET_PLAYER(playState)->actor.update != &Scripts_PlayerAnimateMode)
                {
                    script->PlayerUpdate = GET_PLAYER(playState)->actor.update;
                    GET_PLAYER(playState)->actor.update = (void*)&Scripts_PlayerAnimateMode;
                }
            }
            else
            {
                if ((void*)GET_PLAYER(playState)->actor.update == &Scripts_PlayerAnimateMode)
                {
                    GET_PLAYER(playState)->actor.update = script->PlayerUpdate;
                    script->PlayerUpdate = NULL;
                }
            }
            break;
        }
        case SET_ANIMATION:
        case SET_ANIMATION_INSTANTLY:
        {
            ScrInstrDoubleSet* instr = (ScrInstrDoubleSet*)in;

            s32 animId = Scripts_GetVarval(en, playState, instr->varType1, instr->value1, false);
            s32 once = Scripts_GetVarval(en, playState, instr->varType2, instr->value2, false);

            Setup_Animation(en, playState, animId, instr->subId == SET_ANIMATION, once, true, false, false);
            break;
        }        
        case SET_PLAYER_ANIMATION:
        {
            ScrInstrSetPlayerAnim* instr = (ScrInstrSetPlayerAnim*)in;

            u32 offset = Scripts_GetVarval(en, playState, instr->offsetType, instr->offset, false);
            u32 startFrame = Scripts_GetVarval(en, playState, instr->startFrameType, instr->startFrame, false);
            u32 endFrame = Scripts_GetVarval(en, playState, instr->endFrameType, instr->endFrame, false);
            float speed = Scripts_GetVarval(en, playState, instr->speedType, instr->speed, false);   

            Setup_AnimationImpl(&GET_PLAYER(playState)->actor, playState, &GET_PLAYER(playState)->skelAnime, offset, ANIMTYPE_LINK, -1, 0, -1, -1, 0, startFrame, endFrame, speed, -4, true, instr->once, true);
            break;       
        }
        case SET_SCRIPT_START:
        {
            ScrInstrStartSet* instr = (ScrInstrStartSet*)in;
            script->startInstrNum = instr->instrNum;
            break;
        }
        case SET_BLINK_PATTERN:
        {
            ScrInstrPatternSet* instr = (ScrInstrPatternSet*)in;
            bcopy(&instr->pattern, &en->settings.blinkPattern, 4);
            en->currentBlinkFrame = 0;
            en->blinkTimer = 0;
            break;
        }
        case SET_TALK_PATTERN:
        {
            ScrInstrPatternSet* instr = (ScrInstrPatternSet*)in;
            bcopy(&instr->pattern, &en->settings.talkPattern, 4);
            en->currentTalkFrame = 0;
            break;            
        }
        case SET_SEGMENT_ENTRY:
        {   
            ScrInstrDoubleSet* instr = (ScrInstrDoubleSet*)in;
            int segId = Scripts_GetVarval(en, playState, instr->varType1, instr->value1, false);
            int segEntryId = Scripts_GetVarval(en, playState, instr->varType2, instr->value2, false);
            en->segmentDataIds[segId] = segEntryId;
            break;
        }
        case SET_DLIST_VISIBILITY:
        {   
            ScrInstrDoubleSet* instr = (ScrInstrDoubleSet*)in;
            int showType = Scripts_GetVarval(en, playState, instr->varType1, instr->value1, false);
            int dListId = Scripts_GetVarval(en, playState, instr->varType2, instr->value2, false);
            en->extraDLists[dListId].showType = showType;
            break;
        }
        case SET_CAMERA_TRACKING_ON:
        {
            ScrInstrActorSet* instr = (ScrInstrActorSet*)in;
            void* actor = Scripts_GetActorByType(en, playState, instr->target, instr->actorNumType, instr->actorNum);

            if (actor != NULL)
                playState->cameraPtrs[playState->activeCamId]->player = (Player*)actor;

            break;
        }
        case SET_REF_ACTOR:
        {
            ScrInstrActorSet* instr = (ScrInstrActorSet*)in;
            en->refActor = Scripts_GetActorByType(en, playState, instr->target, instr->actorNumType, instr->actorNum);
            break;
        }
        case SET_EXT_VAR:
        {
            ScrInstrExtVarSet* instr = (ScrInstrExtVarSet*)in;
            u32 actorId = Scripts_GetVarval(en, playState, instr->actorNumVarType, instr->actorNum, false);
            NpcMaker* exActor = Scene_GetNpcMakerByID(en, playState, actorId);

            if (exActor != NULL)
            {
                Scripts_MathOperation(&exActor->scriptVars[instr->extVarNum - 1], 
                                    Scripts_GetVarval(en, playState, instr->varType, instr->value, true), 
                                    instr->operator, 
                                    INT32);
            }
            break;
        }
        case SET_EXT_VARF:
        {
            ScrInstrExtVarSet* instr = (ScrInstrExtVarSet*)in;
            u32 actorId = Scripts_GetVarval(en, playState, instr->actorNumVarType, instr->actorNum, false);
            NpcMaker* exActor = Scene_GetNpcMakerByID(en, playState, actorId);

            if (exActor != NULL)
            {
                Scripts_MathOperation(&exActor->scriptFVars[instr->extVarNum - 1], 
                                    Scripts_GetVarval(en, playState, instr->varType, instr->value, true), 
                                    instr->operator, 
                                    FLOAT);
            }
            break;
        }
        case SET_ATTACKED_EFFECT: 
        {
            Scripts_Set(en, playState, &en->settings.effectIfAttacked, in, UINT8);
            en->collider.base.colType = en->settings.effectIfAttacked;
            break;
        }

        case SET_RAM:
        {
            ScrInstrSetRAM* instr = (ScrInstrSetRAM*)in;
    
            switch (instr->lenght)
            {
                case 0: AVAL(instr->address, u8, 0) = instr->value; break;
                case 1: AVAL(instr->address, u16, 0) = instr->value; break;
                case 2: AVAL(instr->address, u32, 0) = instr->value; break;
            }
            
            break;   
        }
        case SET_LABELTOVAR:
        case SET_LABELTOVARF:
        {
            break;
        }
        case SUBT_GLOBAL8:
        case SUBT_GLOBAL16:
        case SUBT_GLOBAL32:
        case SUBT_GLOBALF:
        case SUBT_ACTOR8:
        case SUBT_ACTOR16:
        case SUBT_ACTOR32:
        case SUBT_ACTORF:
        case SUBT_SAVE8:
        case SUBT_SAVE16:
        case SUBT_SAVE32:
        case SUBT_SAVEF:
        case SUBT_VAR:
        case SUBT_VARF:
        {
            ScrInstrDoubleSet* instr = (ScrInstrDoubleSet*)in;

            u32 valt;
            void* addr = Scripts_RamSubIdSetup(en, playState, instr->value1.ui32, instr->subId, &valt);

            if (addr == NULL)
                break;

            Scripts_MathOperation(addr, 
                                  Scripts_GetVarval(en, playState, instr->varType2, instr->value2, true), 
                                  instr->operator, 
                                  valt);
                                      
            break;
        }
        default: break;
    }

    script->curInstrNum++;
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionEnableTalking(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrTextbox* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: ENABLE_TALKING\n", en->npcId, en->curScriptNum);
    #endif  

    if (en->wasHit)
    {
        script->curInstrNum++;
        return SCRIPT_CONTINUE;
    }

    //z_actor_poll_speak_cube
    en->canTalk = func_8002F2CC(&en->actor, playState, en->settings.talkRadius + en->collider.dim.radius);

    u32 id = Scripts_GetTextId(en, playState, in->skipChildMsgId, in->vartypeChild, in->childMsgId, in->varTypeAdult, in->adultMsgId);
    Scripts_SetMessage(en, playState, id, &en->actor.textId, false, true);

    script->curInstrNum++;
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionShowTextbox(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrTextbox* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: SHOW_TEXTBOX\n", en->npcId, en->curScriptNum);
    #endif  

    u32 id = Scripts_GetTextId(en, playState, in->skipChildMsgId, in->vartypeChild, in->childMsgId, in->varTypeAdult, in->adultMsgId);

    if (in->id == SHOW_TEXTBOX)
    {
        Scripts_SetMessage(en, playState, id, &en->actor.textId, true, true);

        // We need to set this so that await talking_end still works
        en->isTalking = true;
        en->textboxDisplayed = true;
        en->talkingFinished = false;
    }
    else
    {
        Scripts_SetMessage(en, playState, id, NULL, true, false);

        if (id > __INT16_MAX__)
            Message_Overwrite(en, playState, R_CUSTOM_MSG_ID(id));   
    }

    script->curInstrNum++;
    return SCRIPT_STOP;
}

bool Scripts_InstructionEnableTrade(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrTrade* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: ENABLE_TRADE\n", en->npcId, en->curScriptNum);
    #endif  

    if (en->wasHit)
    {
        script->curInstrNum++;
        return SCRIPT_CONTINUE;
    }

    // Find which item we're meant to check for, and which item Link is actually trading.
    en->tradeItem = Scripts_GetVarval(en, playState, in->correct.varTypeItem, in->correct.item, false);
    s8 curTradedItem = GET_PLAYER(playState)->exchangeItemId;

    if (!en->isTalking)
    {
        if (en->canTrade)
        {
            // If the item being traded is correct, set the correct text ID.
            if (en->tradeItem == curTradedItem)
            {
                int id = Scripts_GetTextId(en, playState, 0, in->correct.varTypeChild, in->correct.childMsgId, in->correct.varTypeAdult, in->correct.adultMsgId);
                Scripts_SetMessage(en, playState, id, &en->actor.textId, false, true);
            }
            // If the item traded is wrong, set the incorrect id.
            else
            {
                // If SOMETHING is being traded...
                if (curTradedItem != EXCH_ITEM_NONE)
                {
                    // Loop through the incorrect item list, trying to find the item being traded.
                    for (int i = 0; i < in->failureDefsNumber; i++)
                    {
                        int fail_item = Scripts_GetVarval(en, playState, in->failure[i].varTypeItem, in->failure[i].item, true);

                        // Set the message if the item matches, or item in the list is -1 (which is the default for all items not set specifically, and shuffled to always be at the very end).
                        if (fail_item == -1 || fail_item == curTradedItem)
                        {
                            int id = Scripts_GetTextId(en, playState, 0, in->failure[i].varTypeChild, in->failure[i].childMsgId, in->failure[i].varTypeAdult, in->failure[i].adultMsgId);
                            Scripts_SetMessage(en, playState, id, &en->actor.textId, false, true);
                            break;
                        }
                    }
                }
                // If nothing is being traded, set that text id (this is what will be displayed when the player speaks to the NPC)
                else
                {
                    int id = Scripts_GetTextId(en, playState, 0, in->varTypeTalkChild, in->childTalkMsgId, in->varTypeTalkAdult, in->adultTalkMsgId);
                    Scripts_SetMessage(en, playState, id, &en->actor.textId, false, true);         
                }
            }
        }

        //z_actor_poll_trade_cube
        en->canTrade = func_8002F298(&en->actor, playState, en->settings.talkRadius + en->collider.dim.radius, EXCH_ITEM_BLUE_FIRE);

    }

    script->curInstrNum++;
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionFace(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrFace* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: FACE\n", en->npcId, en->curScriptNum);
    #endif      

    if (en->pickedUpState != STATE_IDLE)
        return Scripts_FreeAndContinue(script); 

    bool firstRun = Scripts_SetupTemp(script, in);

    if (firstRun)
    {
        script->tempValues[2] = (s32)Scripts_GetActorByType(en, playState, in->subject, in->subjectActorNumType, in->subjectActorNum);
        script->tempValues[3] = (s32)Scripts_GetActorByType(en, playState, in->target, in->targetActorNumType, in->targetActorNum);

        Actor* subject = (Actor*)script->tempValues[2];
        Actor* target = (Actor*)script->tempValues[3];

        script->tempValues[5] = 40;

        switch (in->faceType)
        {
            case FACE_TOWARDS:
                script->tempValues[0] = Math_Vec3f_Yaw(&subject->world.pos, &target->world.pos);
            case FACE_AWAY_FROM:     
                script->tempValues[0] = target->world.rot.y;   
            case FACE_AND: 
            {
                script->tempValues[0] = Math_Vec3f_Yaw(&subject->world.pos, &target->world.pos);
                script->tempValues[1] = Math_Vec3f_Yaw(&target->world.pos, &subject->world.pos);
            }
        }
    }
    else
        script->tempValues[5]--;

    s16 target_rot_sub = script->tempValues[0];
    s16 target_rot_tar = script->tempValues[1];
    Actor* subject = (Actor*)script->tempValues[2];
    Actor* target = (Actor*)script->tempValues[3];

    if (subject == NULL || target == NULL)
        return Scripts_FreeAndContinue(script);   

    bool done = (script->tempValues[5] == 0);

    if (!done)
    {
        switch (in->faceType)
        {
            case FACE_TOWARDS:
            case FACE_AWAY_FROM:
            {
                if (!Movement_RotTowards(&subject->shape.rot.y, target_rot_sub, 0))
                    done = true;

                break;
            }
            case FACE_AND:
            {
                s16 diff_sub = Movement_RotTowards(&subject->shape.rot.y, target_rot_sub, 0);
                s16 diff_tar = Movement_RotTowards(&target->shape.rot.y, target_rot_tar, 0);

                if (diff_sub == 0 && diff_tar == 0)
                    done = true;

                break;
            }
        }
    }

    if (done)
        return Scripts_FreeAndContinue(script);  
    else
        return SCRIPT_STOP;
}

bool Scripts_InstructionRotation(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrRotation* in)
{
    #define ACTOR ((Actor*)script->tempValues[0])
    #define SPEED (script->fTempValues[4])
    #define ROT ((Vec3f*)&script->fTempValues[1])

    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: ROTATE\n", en->npcId, en->curScriptNum);
    #endif  

    bool firstRun = Scripts_SetupTemp(script, in);

    // On the first run, we gather the needed data from the instruction.
    // We can't do this every time the instruction is run, because randomness would mess it up.
    // Could just seed the random number, but this also ends up being more efficient...
    if (firstRun)
    {
        // Actor
        script->tempValues[0] = (s32)Scripts_GetActorByType(en, playState, in->target, in->actorNumType, in->actorNum);

        // Speed
        SPEED = Scripts_GetVarval(en, playState, in->speedType, in->speed, true);

        Vec3f rot = Scripts_GetVarvalVec3f(en, playState, (Vartype[]){in->xType, in->yType, in->zType}, (ScriptVarval[]){in->x, in->y, in->z}, 1);
        Math_Vec3f_Copy(ROT, &rot);
    }

    if (ACTOR == NULL)
        return Scripts_FreeAndContinue(script);

    s16 incomplete = 0;

    switch (in->subId)
    {
        // In this case, we just directly set the rotation to the one specified.
        case ROT_SET: ACTOR->shape.rot = (Vec3s){ROT->x, ROT->y, ROT->z}; break;
        // In this case, we smoothly change the rotation to the one specified.
        case ROT_ROTATE_TO:
        {
            incomplete = Movement_RotTowards(&ACTOR->shape.rot.x, ROT->x, SPEED) + 
                         Movement_RotTowards(&ACTOR->shape.rot.y, ROT->y, SPEED) + 
                         Movement_RotTowards(&ACTOR->shape.rot.z, ROT->z, SPEED);
            break;
        }
        // In this case, we change by the amount specified.
        case ROT_ROTATE_BY:
        {
            incomplete = Movement_StepToZero(&ROT->x, &ACTOR->shape.rot.x, SPEED) + 
                         Movement_StepToZero(&ROT->y, &ACTOR->shape.rot.y, SPEED) +
                         Movement_StepToZero(&ROT->z, &ACTOR->shape.rot.z, SPEED);
            break;
        }
    }

    // if we're not done yet, stop executing.
    if (incomplete)
        return SCRIPT_STOP;
    else
        return Scripts_FreeAndContinue(script);

    #undef ACTOR
    #undef ROT
    #undef SPEED
}

bool Scripts_InstructionPosition(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrPosition* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: POSITION\n", en->npcId, en->curScriptNum);
    #endif 

    #define ACTOR ((Actor*)script->tempValues[0])
    #define NPCACTOR ((NpcMaker*)script->tempValues[0])
    #define ENDPOS ((Vec3f*)&script->fTempValues[1])
    #define SPEED (script->fTempValues[4])
    #define LASTDIST (script->fTempValues[5])

    bool first_run = Scripts_SetupTemp(script, in);

    // On the first run, we gather the needed data from the instruction.
    // We can't do this every time the instruction is run, because randomness would mess it up.
    // Could just seed the random number, but this also ends up being more efficient...
    if (first_run)
    {
        // Actor
        script->tempValues[0] = (s32)Scripts_GetActorByType(en, playState, in->target, in->actorNumType, in->actorNum);

        if (ACTOR != NULL)
        {
            // Position
            Vec3f pos = Scripts_GetVarvalVec3f(en, playState, (Vartype[]){in->xType, in->yType, in->zType}, (ScriptVarval[]){in->x, in->y, in->z}, 1);

            if (in->subId > 1)
            {
                Actor* subject = ACTOR;

                if (in->subId >= 4)
                    subject = en->refActor;

                if (in->subId % 2)
                {
                    Math_AffectMatrixByRot(subject->shape.rot.y, &pos, subject);
                    Math_Vec3f_Sum(&pos, &subject->world.pos, &pos);        
                }
                else
                    Math_Vec3f_Sum(&pos, &subject->world.pos, &pos);
            }

            *ENDPOS = pos;
            SPEED = Scripts_GetVarval(en, playState, in->speedType, in->speed, true);
            LASTDIST = Movement_CalcDist(&ACTOR->world.pos, ENDPOS, in->ignoreY);
        }
    }

    if (ACTOR == NULL)
        return Scripts_FreeAndContinue(script);  

    // If the actor's ID is the same as the actor's executing the script, then conclude they're an NPC Maker NPC.
    bool isNpcMaker = (ACTOR->id == en->actor.id);

    // If actor is an NPC Maker actor, but can't move, we do nothing until it can (this is useful for stuff like getting hit)
    if (isNpcMaker && !NPCACTOR->canMove)
        return SCRIPT_STOP;

    // If type is set, just directly set the position.
    if (in->subId == POS_SET)
        ACTOR->world.pos = *ENDPOS;

    // Handle switching to walking animation, but only if this is an NPC Maker.
    if (isNpcMaker)
    {
        // Saving the current movement type so it can be restored later
        if (first_run)
            script->tempValues[1] = NPCACTOR->settings.movementType;

        NPCACTOR->isMoving = true;
        NPCACTOR->stopped = false;
        NPCACTOR->settings.movementType = MOVEMENT_MISC;

        Setup_Animation(NPCACTOR, playState, ANIM_WALK, true, false, false, !NPCACTOR->autoAnims, true);
    }

    // Caculate movement vector, add it to the position and rotate towards the goal.
    // In both cases, if the speed exceeds the movement distance, we automatically set the goal as the current position.
    if (!in->ignoreY)
    {
        Vec3f movVec = Movement_CalcVector(&ACTOR->world.pos, ENDPOS, SPEED);
        Math_Vec3f_Sum(&ACTOR->world.pos, &movVec, &ACTOR->world.pos);
    }
    else
    {
        ACTOR->world.rot.y = Math_Vec3f_Yaw(&ACTOR->world.pos, ENDPOS);

        if (LASTDIST < SPEED)
            ACTOR->world.pos = *ENDPOS;
        else
        {
            en->actor.speedXZ = SPEED;
            Movement_Apply(ACTOR, NULL);
        }
    }

    // Calculate if we're there yet.
    float distFromEnd = Movement_CalcDist(&ACTOR->world.pos, ENDPOS, in->ignoreY);
    float distFromEndXZ = in->ignoreY ? distFromEnd : Movement_CalcDist(&ACTOR->world.pos, ENDPOS, true);
    float distDiff = ABS(LASTDIST - distFromEnd);
    
    // If we aren't there yet, rotate towards the destination and stop executing script for this frame.
    // If too little progress was made, we got stuck somewhere and should stop moving.
    if (distFromEnd > MOVEMENT_DISTANCE_EQUAL_MARGIN && distDiff >= (SPEED / 10))
    {
        LASTDIST = distFromEnd;

        // Only rotate if there's actual XZ distance to go, though.
        if (distFromEndXZ != 0)
            Movement_RotTowards(&ACTOR->shape.rot.y, Math_Vec3f_Yaw(&ACTOR->world.pos, ENDPOS), 0);

        return SCRIPT_STOP;
    }
    else
    {
        en->actor.speedXZ = 0;
        
        // Handle switching the animation back to idle if this is the NPC Maker actor.
        if (isNpcMaker)
        {
            NPCACTOR->isMoving = false;
            NPCACTOR->stopped = true;
            NPCACTOR->settings.movementType = script->tempValues[1];

            Setup_Animation(NPCACTOR, playState, ANIM_IDLE, true, false, false, !NPCACTOR->autoAnims, true);
        }

        return Scripts_FreeAndContinue(script);
    }

    #undef ACTOR
    #undef NPCACTOR
    #undef ENDPOS
    #undef SPEED
    #undef LASTDIST
}

bool Scripts_InstructionScale(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrScale* in)
{
    #define ACTOR ((Actor*)script->tempValues[0])
    #define SPEED (script->fTempValues[0])
    #define SCALE (script->fTempValues[1])

    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: SCALE\n", en->npcId, en->curScriptNum);
    #endif  

    bool firstRun = Scripts_SetupTemp(script, in);

    // On the first run, we gather the needed data from the instruction.
    // We can't do this every time the instruction is run, because randomness would mess it up.
    // Could just seed the random number, but this also ends up being more efficient...
    if (firstRun)
    {
        // Actor
        script->tempValues[0] = (s32)Scripts_GetActorByType(en, playState, in->target, in->actorNumType, in->actorNum);

        SPEED = Scripts_GetVarval(en, playState, in->speed_type, in->speed, true);
        SCALE = Scripts_GetVarval(en, playState, in->scale_type, in->scale, true);
    }

    if (ACTOR == NULL)
        return Scripts_FreeAndContinue(script);

    float incomplete = 0;

    switch (in->subId)
    {
        // In this case, we just set the scale directly.
        case SCALE_SET: Actor_SetScale(ACTOR, SCALE); break;
        // In this case we smoothly scale up to the defined scale.
        case SCALE_SCALE_TO:
        {
            float new = ACTOR->scale.x;

            incomplete = Math_SmoothStepToF(&new, SCALE, SCALE_SMOOTH_SCALE, SPEED, SCALE_SMOOTH_MIN);
            Actor_SetScale(ACTOR, new);
            break;
        }
        // In this case, we smoothly scale up to defined scale + current scale
        case SCALE_SCALE_BY:
        {
            if (firstRun)
                script->fTempValues[2] = ACTOR->scale.x;

            float new = ACTOR->scale.x;
            incomplete = Math_SmoothStepToF(&new, SCALE + script->fTempValues[2], SCALE_SMOOTH_SCALE, SPEED, SCALE_SMOOTH_MIN);
            
            Actor_SetScale(ACTOR, new);
            break;
        }
    }

    // if we're not done yet, stop executing.
    if (incomplete)
        return SCRIPT_STOP;
    else
        return Scripts_FreeAndContinue(script);

    #undef ACTOR
    #undef SCALE
    #undef SPEED
}

bool Scripts_InstructionPlay(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrPlay* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: PLAY\n", en->npcId, en->curScriptNum);
    #endif     

    u32 value = 0; 
    
    if (in->subId < PLAY_SFX_PARAMS)
        value = Scripts_GetVarval(en, playState, in->varType, in->value, false);

    switch (in->subId)
    {
        case PLAY_BGM: Audio_QueueSeqCmd(value); break;
        case PLAY_CUTSCENE: 
        {
            Cutscene_SetSegment(playState, Scene_GetCurrentCutscenePtr(playState)); 
            
            if (playState->csCtx.segment != NULL)
                gSaveContext.cutsceneTrigger = 1;
    
            break;
        }
        case PLAY_CUTSCENE_ID: 
        {
            Cutscene_SetSegment(playState, Scene_GetCutscenePtr(playState, value)); 
            
            if (playState->csCtx.segment != NULL)
                gSaveContext.cutsceneTrigger = 1;
            
            break;
        }
        case PLAY_SFX:
        {
            Audio_PlayActorSfx2(&en->actor, value);
            break;
        }
        case PLAY_SFX_GLOBAL: 
        {
            en->scriptSfxTempPos.x = gSfxDefaultPos.x - 1;
            en->scriptSfxTempPos.y = gSfxDefaultPos.y;
            en->scriptSfxTempPos.z = gSfxDefaultPos.z;
            
            Audio_PlaySfxGeneral(value, &en->scriptSfxTempPos, 4, &gSfxDefaultFreqAndVolScale, &gSfxDefaultFreqAndVolScale, &gSfxDefaultReverb); 
            break;
        }
        case PLAY_SFX_PARAMS:
        case PLAY_SFX_GLOBAL_PARAMS:
        {
            ScrInstrPlayWithParams* inP = (ScrInstrPlayWithParams*)in;

            value = Scripts_GetVarval(en, playState, inP->idVarType, inP->value, false);
            en->scriptVolTemp = Scripts_GetVarval(en, playState, inP->volumeVarType, inP->volume, false); 
            en->scriptPitchTemp = Scripts_GetVarval(en, playState, inP->pitchVarType, inP->pitch, false); 
            en->scriptReverbTemp = Scripts_GetVarval(en, playState, inP->reverbVarType, inP->reverb, true); 

            if (in->subId == PLAY_SFX_PARAMS)
            {
                en->scriptSfxTempPos.x = en->actor.world.pos.x;
                en->scriptSfxTempPos.y = en->actor.world.pos.y;
                en->scriptSfxTempPos.x = en->actor.world.pos.z;
            }
            else
            {
                en->scriptSfxTempPos.x = gSfxDefaultPos.x - 1;
                en->scriptSfxTempPos.y = gSfxDefaultPos.y;
                en->scriptSfxTempPos.z = gSfxDefaultPos.z;      
            }

            Audio_PlaySfxGeneral(value, &en->scriptSfxTempPos, 4, &en->scriptPitchTemp, &en->scriptVolTemp, &en->scriptReverbTemp); 
            break; 
        }
    }

    script->curInstrNum++;
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionKill(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrKill* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: KILL\n", en->npcId, en->curScriptNum);
    #endif   

    Actor* actor = Scripts_GetActorByType(en, playState, in->subId, in->actorNumType, in->actorNum);

    if (actor != NULL)
        Actor_Kill(actor);

    if (actor == &en->actor)
        return SCRIPT_STOP;
    else
    {
       script->curInstrNum++;
       return SCRIPT_CONTINUE;  
    } 
}

bool Scripts_InstructionOcarina(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrOcarina* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: OCARINA\n", en->npcId, en->curScriptNum);
    #endif   

    u32 song = Scripts_GetVarval(en, playState, in->ocaSongType, in->ocaSong, false);

    if (!en->listeningToSong)
    {
        if ((en->settings.talkRadius + en->collider.dim.radius) >= en->actor.xzDistToPlayer || 
            playState->actorCtx.targetCtx.targetedActor == &en->actor)
        {
            GET_PLAYER(playState)->stateFlags2 |= 0x800000;
            en->actor.flags |= 0x02000000;

            // Check if player has entered the ocarina state.
            if (GET_PLAYER(playState)->stateFlags2 & 0x1000000)
            {
                func_8010BD88(playState, 0x22);
                GET_PLAYER(playState)->stateFlags2 |= 0x2000000;
                GET_PLAYER(playState)->unk_6A8 = &en->actor;

                #if LOGGING > 3
                    is64Printf("_%2d: Player whipped out an ocarina!\n", en->npcId);
                #endif   

                // Show prompt. For songs game officially recognizes as playable, use the built in method.
                // Otherwise, we're listening to song 0 (any song).
                // Set actor as listening to song.
                // z_ocarina_show_prompt
                func_8010BD58(playState, song > 5 ? song : 0);
                en->listeningToSong = true;
                Movement_StopMoving(en, playState, !en->autoAnims);
            }
        }

        script->curInstrNum = in->falseInstrNum;
    }
    else
    {
        u16* playedSong = &playState->msgCtx.lastPlayedSong;
        u16* songState = &playState->msgCtx.ocarinaMode;

        if (en->correctSongHeard)
        {
            // Dumb workaround to make SURE Saria's Song text doesn't appear.
            GET_PLAYER(playState)->stateFlags1 |= PLAYER_STOPPED_MASK;
            script->curInstrNum = in->trueInstrNum;
            en->correctSongHeard = false;
            en->listeningToSong = false;
            return SCRIPT_STOP;       
        }
        // If song is officially reconized as correct, or song played was the one specified
        // in the instruction, jump to the instruction block.
        if ((song > 5 && *songState == SONGSTATUS_CORRECT) || (song <= 5 && *playedSong == song))
        {
            #if LOGGING > 3
                is64Printf("_%2d: Correct song was heard.\n", en->npcId);
            #endif   
            
            en->correctSongHeard = true;
            *songState = SONGSTATUS_CANCELED;
            *playedSong = 0xFF;
            return SCRIPT_STOP;
        }
        // If wrong song was played, we cancel prompt.
        else if (*songState > SONGSTATUS_CANCELED)
        {
            *playedSong = 0xFF;
            *songState = SONGSTATUS_CANCELED;
            return SCRIPT_STOP;
        }
        // If song playing was canceled, we stop listening.
        else if (*songState == SONGSTATUS_CANCELED)
        {
            *playedSong = 0xFF;
            en->listeningToSong = false;
        }
        // Otherwise, jump past the instruction block.
        else 
            script->curInstrNum = in->falseInstrNum;
    }  

    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionSpawn(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrSpawn* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: SPAWN\n", en->npcId, en->curScriptNum);
    #endif
    
    bool setAsRef = in->posType >= 10;
    int posType = in->posType >= 10 ? in->posType - 10 : in->posType;


    Vec3s rotation = (Vec3s){
                                Scripts_GetVarval(en, playState, in->rotXType, in->rotX, true),
                                Scripts_GetVarval(en, playState, in->rotYType, in->rotY, true),
                                Scripts_GetVarval(en, playState, in->rotZType, in->rotZ, true),
                            };

    int actorNum = Scripts_GetVarval(en, playState, in->actorNumType, in->actorNum, false);
    int actorParam = Scripts_GetVarval(en, playState, in->actorParamType, in->actorParam, false);
    Vec3f position = Scripts_GetVarvalVec3f(en, playState, (Vartype[]){in->posXType, in->posYType, in->posZType}, (ScriptVarval[]){in->posX, in->posY, in->posZ}, 1);
    
    Actor* subject = &en->actor;

    if (posType >= 3)
        subject = en->refActor;

    if (posType)
    {
        if (posType % 2)
            Math_Vec3f_Sum(&position, &subject->world.pos, &position);
        else 
        {
            Math_AffectMatrixByRot(subject->shape.rot.y, &position, NULL);
            Math_Vec3f_Sum(&position, &subject->world.pos, &position);
        }
    }

    Actor* spawned = Actor_Spawn(&playState->actorCtx, playState, actorNum, position.x, position.y, position.z, rotation.x, rotation.y, rotation.z, actorParam);

    if (setAsRef)
        en->refActor = spawned;

    script->curInstrNum++;
    return SCRIPT_CONTINUE;  
}

bool Scripts_InstructionItem(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrItem* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: ITEM\n", en->npcId, en->curScriptNum);
    #endif   

    u32 item = Scripts_GetVarval(en, playState, in->itemVarType, in->item, true);

    // Pickup actor if giving no item, with 70% interact radius (so talking and picking up may happen at the same time)
    if (item == GI_NONE && in->subId == ITEM_GIVE)
        func_8002F434(&en->actor, playState, item, en->settings.talkRadius * 0.7f, en->settings.talkRadius * 0.7f);
    else
    {
        bool firstRun = Scripts_SetupTemp(script, in);

        switch (in->subId)
        {
            case ITEM_AWARD: 
            {
                // Because Link cannot receive things while in a cutscene, we save the current cutscene state
                // and turn cutscene off for Link, then wait a moment for this to register...
                if (firstRun)
                {
                    script->waitTimer = 2;

                    if (GET_PLAYER(playState)->csMode != 0)
                    {
                        script->tempValues[0] = GET_PLAYER(playState)->csMode;
                        //z_cutscene_link_action
                        func_8002DF54(playState, &en->actor, 0x7);
                    }
                    
                    // Save current state to restore later.
                    script->tempValues[1] = en->stopPlayer;

                    if (en->stopPlayer)
                    {
                        en->stopPlayer = false;
                        GET_PLAYER(playState)->stateFlags1 &= ~PLAYER_STOPPED_MASK;
                    }

                    return SCRIPT_STOP;
                }
                else
                {
                    // Once we have waited two frames, we give the actor an item, and wait a bit of time again (to not restore the cutscene state prematurely)...
                    if (script->tempValues[2] == -1)
                    {
                        //z_actor_give_item
                        func_8002F434(&en->actor, playState, item, __UINT32_MAX__, __UINT32_MAX__);
                        script->waitTimer = 2;
                        script->tempValues[2] = 1;
                        return SCRIPT_STOP;
                    }
                    else
                    {
                        // Wait for textbox end...
                        // Player talk state
                        if (Message_GetState(&playState->msgCtx) != TEXT_STATE_CLOSING && Message_GetState(&playState->msgCtx) != TEXT_STATE_NONE)
                            return SCRIPT_STOP;

                        //...after which, if Link WAS in a cutscene, we restore the cutscene state.
                        //z_cutscene_link_action
                        if (script->tempValues[0] != -1)
                            func_8002DF54(playState, &en->actor, script->tempValues[0]);     

                        if (script->tempValues[1] != 0)
                            en->stopPlayer = true;
                    }
                }
                
                break;
            }
            case ITEM_GIVE: 
            {
                if (item > GI_MAX)
                {
                    switch (item)
                    {
                        case UPGRADE_MAGIC:                 gSaveContext.isMagicAcquired = true; break;
                        case UPGRADE_DOUBLE_MAGIC:          gSaveContext.isDoubleMagicAcquired = true; gSaveContext.magicLevel = 0; break;
                        case UPGRADE_DOUBLE_DEFENCE:        gSaveContext.isDoubleDefenseAcquired = true; break;
                    }

                    break; 
                }
                else
                {
                    // Don't give the player anything if they don't have an empty bottle and the item is a bottled item.
                    if (IS_BOTTLE_ITEM(item))
                    {
                        if (!Inventory_HasEmptyBottle())
                            break;
                    }

                    Item_Give(playState, item);        
                    break; 
                }

                break;
            }
            case ITEM_TAKE: 
            {
                if (item > ITEM_NUT_UPGRADE_40)
                {
                    switch (item)
                    {
                        // Fall through on purpose - if magic is taken away, then double magic is taken away too.
                        case UPGRADE_MAGIC:                 gSaveContext.isMagicAcquired = false; 
                        case UPGRADE_DOUBLE_MAGIC:          gSaveContext.isDoubleMagicAcquired = false; gSaveContext.magicLevel = 0; break;
                        case UPGRADE_DOUBLE_DEFENCE:        gSaveContext.isDoubleDefenseAcquired = false; break;
                    }
                    break; 
                }
                else
                {
                    // Remove Link's mask if we're taking it away.
                    if (IS_MASK(item))
                        Player_UnsetMask(playState);
                    // Code for bottle items
                    else if (IS_BOTTLE_ITEM(item))
                    {
                        // If player is holding the item in question, we take that one specifically.
                        if ((GET_PLAYER(playState)->heldItemButton != 0) && 
                        (gSaveContext.inventory.items[gSaveContext.equips.cButtonSlots[GET_PLAYER(playState)->heldItemButton - 1]] == item))
                            Player_UpdateBottleHeld(playState, GET_PLAYER(playState), ITEM_BOTTLE, PLAYER_AP_BOTTLE);
                        else
                            Inventory_ReplaceItem(playState, item, ITEM_BOTTLE);
                    }
                    else
                        Inventory_DeleteItem(item, SLOT(item));

                    break;
                }

                break;
            }
        }
    }

    return Scripts_FreeAndContinue(script);
}

bool Scripts_InstructionWarp(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrWarp* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: WARP\n", en->npcId, en->curScriptNum);
    #endif  

    u32 warpId = Scripts_GetVarval(en, playState, in->warpIdvarType, in->warpId, false);
    u32 cutsceneId = Scripts_GetVarval(en, playState, in->cutsceneIdvarType, in->cutsceneId, false);
    u32 transType = Scripts_GetVarval(en, playState, in->transTypeType, in->transType, false);
    playState->nextEntranceIndex = warpId;
    playState->transitionTrigger = TRANS_TRIGGER_START;

    if (transType != 0xFF)
        playState->transitionType = transType;

    if (cutsceneId > 0)
        cutsceneId = 0xFFF0 + (cutsceneId - 4);

    gSaveContext.nextCutsceneIndex = cutsceneId;
    script->curInstrNum++;
    return SCRIPT_CONTINUE; 
}

bool Scripts_InstructionScript(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrScript* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: SCRIPT\n", en->npcId, en->curScriptNum);
    #endif  

    u32 scriptID = Scripts_GetVarval(en, playState, in->scriptIdVarType, in->scriptId, false);
    en->scriptInstances[scriptID].active = in->subID;
    script->curInstrNum++;
    return SCRIPT_CONTINUE; 
}

extern void Audio_StopBGMAndFanfares(u16 FadeoutDur);
    #if GAME_VERSION == 0
        asm("Audio_StopBGMAndFanfares = 0x800F6AB0");
    #elif GAME_VERSION == 1
        asm("Audio_StopBGMAndFanfares = 0x800C77D0");
    #endif  

bool Scripts_InstructionStop(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrStop* in)
{
    #if LOGGING > 3
        is64Printf("_[%2d, %1d]: STOP\n", en->npcId, en->curScriptNum);
    #endif  

    u32 Val = Scripts_GetVarval(en, playState, in->stopIdVarType, in->stopId, false);

    switch (in->subID)
    {
        case STOP_SFX:  Audio_StopSfxById(Val); break;
        case STOP_BGM:  Audio_StopBGMAndFanfares(Val); break;
        default: break;
    }

    script->curInstrNum++;
    return SCRIPT_CONTINUE; 
}
