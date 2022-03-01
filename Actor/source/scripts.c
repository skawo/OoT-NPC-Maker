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


inline ScrInstr* Scripts_GetInstrPtr(ScriptInstance* script, u32 instruction_num)
{
    u32 curOffs = AVAL(script->scriptPtr, u16, SCRIPT_INSTR_SIZE * instruction_num);
    return (ScrInstr*)AADDR(script->scriptPtr, curOffs);
}

// Used as the update function for the player whenever animate mode is set.
// The actor which turns on the animate mode MUST unset it in the same state as when the animate mode was set!
// (e.g if animate mode is set while textbox is on screen, it must be unset while textbox is on screen)
void Scripts_PlayerAnimateMode(Player* pl, GlobalContext* globalCtx)
{
    LinkAnimation_Update(globalCtx, &pl->skelAnime);
}

void Scripts_Main(NpcMaker* en, GlobalContext* globalCtx)
{
    for (int i = 0; i < en->scripts->numScripts; i++)
    {
        ScriptInstance* script = &en->scriptInstances[i];

        if (script->scriptPtr != NULL)
        {
            if (script->active)
            {
                // Player responding to a textbox may desync from the script (especially if the player mashes the button), 
                // so we need to check for this before executing anything.
                Scripts_ResponseInstruction(en, globalCtx, script);

                // Player getting spotted due to a search particle.
                if (script->spotted && script->jumpToWhenSpottedInstrNum >= 0)
                {
                    script->curInstrNum = script->jumpToWhenSpottedInstrNum;
                    script->spotted = 0;
                    script->jumpToWhenSpottedInstrNum = -1;
                }

                if (script->waitTimer != 0)
                    script->waitTimer--;
                else
                    while(Scripts_Execute(en, globalCtx, script));
            }
        }   
    }
}

void Scripts_ResponseInstruction(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script)
{
    if (script->responsesInstrNum < 0)
        return;

    ScrInstrResponsesSet* instruction = (ScrInstrResponsesSet*)Scripts_GetInstrPtr(script, script->responsesInstrNum);

    // Wait until the selection pops up and player has responded to it...

    // Player talk state, player responded to textbox
    if (func_8010BDBC(&globalCtx->msgCtx) == MSGSTATUS_SELECTING && func_80106BC8(globalCtx))
    {
        switch (globalCtx->msgCtx.choiceIndex)
        {
            case 0:     script->jumpToWhenReponded = instruction->resp1InstrNum; break;
            case 1:     script->jumpToWhenReponded = instruction->resp2InstrNum; break;
            case 2:     script->jumpToWhenReponded = instruction->resp3InstrNum; break;
            default:    break;
        }
    }

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

bool Scripts_Execute(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script)
{
    ScrInstr* instruction = Scripts_GetInstrPtr(script, script->curInstrNum);

    switch (instruction->id)
    {
        case IF:
        case WHILE:              return Scripts_InstructionIf(en, globalCtx, script, (ScrInstrIf*)instruction); break;
        case AWAIT:              return Scripts_InstructionAwait(en, globalCtx, script, (ScrInstrAwait*)instruction); break;
        case GOTO:               return Scripts_InstructionGoto(en, globalCtx, script, (ScrInstrGoto*)instruction); break;
        case SET:                return Scripts_InstructionSet(en, globalCtx, script, (ScrInstrSet*)instruction); break;
        case ENABLE_TALKING:     return Scripts_InstructionEnableTalking(en, globalCtx, script, (ScrInstrTextbox*)instruction); break;
        case SHOW_TEXTBOX:       return Scripts_InstructionShowTextbox(en, globalCtx, script, (ScrInstrTextbox*)instruction); break;
        case SHOW_TEXTBOX_SP:    return Scripts_InstructionShowTextbox(en, globalCtx, script, (ScrInstrTextbox*)instruction); break;
        case TRADE:              return Scripts_InstructionEnableTrade(en, globalCtx, script, (ScrInstrTrade*)instruction); break;
        case FACE:               return Scripts_InstructionFace(en, globalCtx, script, (ScrInstrFace*)instruction); break;
        case ROTATION:           return Scripts_InstructionRotation(en, globalCtx, script, (ScrInstrRotation*)instruction); break;
        case POSITION:           return Scripts_InstructionPosition(en, globalCtx, script, (ScrInstrPosition*)instruction); break;
        case SCALE:              return Scripts_InstructionScale(en, globalCtx, script, (ScrInstrScale*)instruction); break;
        case PLAY:               return Scripts_InstructionPlay(en, globalCtx, script, (ScrInstrPlay*)instruction); break;
        case KILL:               return Scripts_InstructionKill(en, globalCtx, script, (ScrInstrKill*)instruction); break;
        case OCARINA:            return Scripts_InstructionOcarina(en, globalCtx, script, (ScrInstrOcarina*)instruction); break;
        case SPAWN:              return Scripts_InstructionSpawn(en, globalCtx, script, (ScrInstrSpawn*)instruction); break;
        case ITEM:               return Scripts_InstructionItem(en, globalCtx, script, (ScrInstrItem*)instruction); break;
        case WARP:               return Scripts_InstructionWarp(en, globalCtx, script, (ScrInstrWarp*)instruction); break;
        case SCRIPT:             return Scripts_InstructionScript(en, globalCtx, script, (ScrInstrScript*)instruction); break;
        case PARTICLE:           return Scripts_InstructionParticle(en, globalCtx, script, (ScrInstrParticle*)instruction); break;
        case FORCE_TALK:         en->isTalking = true; globalCtx->talkWithPlayer(globalCtx, &en->actor); script->curInstrNum++; return SCRIPT_CONTINUE;
        case CLOSE_TEXTBOX:      globalCtx->msgCtx.msgMode = MSGMODE_CLOSING; script->curInstrNum++; return SCRIPT_CONTINUE;
        case NOP:                script->curInstrNum++; return SCRIPT_STOP;
        default:                                
        {
            #if LOGGING == 1
                osSyncPrintf("_%2d: Encountered invalid instruction %2d. Stopping script.", en->npcId, instruction->id);
            #endif     

            return SCRIPT_STOP;
        } 
    }
}

bool Scripts_InstructionParticle(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrParticle* in)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: PARTICLE", en->npcId);
    #endif

    Vec3f pos = Scripts_GetVarvalVec3f(en, globalCtx, (Vartype[]){in->posXType, in->posYType, in->posZType}, (ScriptVarval[]){in->posX, in->posY, in->posZ}, 1);

    if (in->posType == POSTYPE_DIRECTION)
        Math_AffectMatrixByRot(en->actor.shape.rot.y, &pos);

    if (in->posType && in->type != PARTICLE_FIRE_TAIL)
        Math_Vec3f_Sum(&pos, &en->actor.world.pos, &pos);

    Vec3f accel = Scripts_GetVarvalVec3f(en, globalCtx, (Vartype[]){in->accelXType, in->accelYType, in->accelZType}, (ScriptVarval[]){in->accelX, in->accelY, in->accelZ}, 100);
    Vec3f vel = Scripts_GetVarvalVec3f(en, globalCtx, (Vartype[]){in->velXType, in->velYType, in->velZType}, (ScriptVarval[]){in->velX, in->velY, in->velZ}, 100);
    Color_RGBA8 prim = Scripts_GetVarvalRGBA(en, globalCtx, (Vartype[]){in->primRType, in->primGType, in->primBType, in->primAType}, (ScriptVarval[]){in->primR, in->primG, in->primB, in->primA});
    Color_RGBA8 env = Scripts_GetVarvalRGBA(en, globalCtx, (Vartype[]){in->envRType, in->envGType, in->envBType, in->envAType}, (ScriptVarval[]){in->envR, in->envG, in->envB, in->envA});
    float scale = Scripts_GetVarval(en, globalCtx, in->scaleType, in->scale, true);
    float scaleUpd = Scripts_GetVarval(en, globalCtx, in->scaleUpdType, in->scaleUpdate, true);
    float life = Scripts_GetVarval(en, globalCtx, in->lifeType, in->life, true);
    float var = Scripts_GetVarval(en, globalCtx, in->varType, in->var, false);

    switch (in->type)
    {
        case PARTICLE_DUST:             EffectSsDust_Spawn(globalCtx, 0, &pos, &vel, &accel, &prim, &env, scale, scaleUpd, life, 0); break;
        case PARTICLE_EXPLOSION:        EffectSsBomb2_SpawnLayered(globalCtx, &pos, &vel, &accel, scale, scaleUpd); break;
        case PARTICLE_SPARK:            EffectSsGSpk_SpawnAccel(globalCtx, &en->actor, &pos, &vel, &accel, &prim, &env, scale, scaleUpd); break;
        case PARTICLE_BUBBLE:           EffectSsDtBubble_SpawnCustomColor(globalCtx, &pos, &vel, &accel, &prim, &env, scale, life, var); break;
        case PARTICLE_WATER_SPLASH:     EffectSsSibuki_SpawnBurst(globalCtx, &pos); break;
        case PARTICLE_SMOKE:            EffectSsSibuki2_Spawn(globalCtx, &pos, &vel, &accel, scale); break;
        case PARTICLE_ICE_CHUNK:        EffectSsEnIce_Spawn(globalCtx, &pos, scale, &vel, &accel, &prim, &env, life); break;
        case PARTICLE_ICE_BURST:        EffectSsIcePiece_SpawnBurst(globalCtx, &pos, scale); break;
        case PARTICLE_RED_FLAME:        EffectSsKFire_Spawn(globalCtx, &pos, &vel, &accel, scale, 100); break;
        case PARTICLE_BLUE_FLAME:       EffectSsKFire_Spawn(globalCtx, &pos, &vel, &accel, scale, 0); break;
        case PARTICLE_ELECTRICITY:      EffectSsFhgFlash_SpawnShock(globalCtx, &en->actor, &pos, scale, 0); break;
        case PARTICLE_FOCUSED_STAR:     EffectSsKiraKira_SpawnFocused(globalCtx, &pos, &vel, &accel, &prim, &env, scale, life); break;
        case PARTICLE_DISPERSED_STAR:   EffectSsKiraKira_SpawnDispersed(globalCtx, &pos, &vel, &accel, &prim, &env, scale, life); break;
        case PARTICLE_BURN_MARK:        EffectSsDeadDs_Spawn(globalCtx, &pos, &vel, &accel, scale, scaleUpd, var, life); break;
        case PARTICLE_RING:             EffectSsBlast_Spawn(globalCtx, &pos, &vel, &accel, &prim, &env, scale, scaleUpd, var, life); break;
        case PARTICLE_FLAME:            EffectSsDeadDb_Spawn(globalCtx, &pos, &vel, &accel, scale, scaleUpd, prim.r, prim.g, prim.b, prim.a, env.r, env.g, env.b, env.a, life, 0); break;
        case PARTICLE_FIRE_TAIL:        EffectSsFireTail_Spawn(globalCtx, in->posType ? &en->actor : NULL, &pos, scale, &vel, 0, &prim, &env, var, -1, life); break;
        case PARTICLE_HIT_MARK_FLASH:     
        case PARTICLE_HIT_MARK_DUST:  
        case PARTICLE_HIT_MARK_BURST:  
        case PARTICLE_HIT_MARK_SPARK:  
                                        EffectSsHitMark_Spawn(globalCtx, in->type - PARTICLE_HIT_MARK_FLASH, scale, &pos); break;
                                        
        // Requires OBJECT_FHG
        case PARTICLE_LIGHT_POINT:      EffectSsFhgFlash_SpawnLightBall(globalCtx, &pos, &vel, &accel, scale, var); break; 
        // Requires OBJECT_YABUSAME_POINT
        case PARTICLE_SCORE:            EffectSsExtra_Spawn(globalCtx, &pos, &vel, &accel, scale, var); break;
        // Requires OBJECT_DODONGO
        case PARTICLE_DODONGO_FIRE:     EffectSsDFire_Spawn(globalCtx, &pos, &vel, &accel, scale, scaleUpd, var, prim.r, life); break;
        // Requires OBJECT_FZ
        case PARTICLE_FREEZARD_SMOKE:   EffectSsIceSmoke_Spawn(globalCtx, &pos, &vel, &accel, scale); break;

        case PARTICLE_LIGHTNING:        
        {
            s16 yaw = Scripts_GetVarval(en, globalCtx, in->yawType, in->yaw, true);
            EffectSsLightning_Spawn(globalCtx, &pos, &prim, &env, scale, yaw, life, var);
            break;
        }
        case PARTICLE_DISPLAY_LIST: 
        {
            s16 exDlistIndex = Scripts_GetVarval(en, globalCtx, in->dListType, in->dList, true);
            ExDListEntry exDList;
            Gfx* offset = NULL;

            if (exDlistIndex < 0)
            {
                exDList.objectId = -1;
                exDList.scale = 35.0f;
            }
            else
            {
                exDList = en->extraDLists[(int)exDlistIndex];
                exDList.objectId = R_OBJECT(en, exDList.objectId);
                offset = (Gfx*)(OFFSET_ADDRESS(6, exDList.offset));
            }

            EffectSsHahen_SpawnBurst(globalCtx, &pos, scale, 0, exDList.scale, scaleUpd, var, exDList.objectId, life, offset);
            break;
        }
        case PARTICLE_SEARCH_EFFECT:
        {
            script->jumpToWhenSpottedInstrNum = in->foundInstrNum;  

            Math_AffectMatrixByRot(en->actor.shape.rot.y + en->limbRotA, &vel);         
            EffectSsSolderSrchBall_Spawn(globalCtx, &pos, &vel, &accel, 0, &script->spotted);
            break;
        }
    }
    script->curInstrNum++;
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionIf(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrIf* in)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: IF/WHILE with subtype %02d.", en->npcId, in->subId);
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
        case IF_FLAG_INTERNAL:                  branch = Scripts_IfFlag(en, globalCtx, in); break;
                                            
        case IF_LINK_IS_ADULT:                  branch = Scripts_IfBool(en, globalCtx, !globalCtx->linkAgeOnLoad, in); break;
        case IF_IS_DAY:                         branch = Scripts_IfBool(en, globalCtx, MORNING_TIME < gSaveContext.dayTime && NIGHT_TIME > gSaveContext.dayTime, in); break;
        case IF_IS_TALKING:                     branch = Scripts_IfBool(en, globalCtx, en->isTalking, in); break;
        case IF_PLAYER_HAS_EMPTY_BOTTLE:        branch = Scripts_IfBool(en, globalCtx, Inventory_HasEmptyBottle(), in); break;
        case IF_IN_CUTSCENE:                    branch = Scripts_IfBool(en, globalCtx, globalCtx->csCtx.segment != NULL, in); break;
        case IF_TEXTBOX_ON_SCREEN:              branch = Scripts_IfBool(en, globalCtx, func_8010BDBC(&globalCtx->msgCtx), in); break;    
        case IF_TEXTBOX_DRAWING:                branch = Scripts_IfBool(en, globalCtx, globalCtx->msgCtx.msgMode == MSGMODE_DRAWING, in); break; 
        case IF_PLAYER_HAS_MAGIC:               branch = Scripts_IfBool(en, globalCtx, gSaveContext.magicAcquired, in); break; 
        case IF_ATTACKED:                       branch = Scripts_IfBool(en, globalCtx, en->wasHitThisFrame, in); break; 
        case IF_REF_ACTOR_EXISTS:               branch = Scripts_IfBool(en, globalCtx, en->refActor != NULL, in); break; 
        case IF_PICKUP_IDLE:
        case IF_PICKUP_PICKED_UP:
        case IF_PICKUP_THROWN:
        case IF_PICKUP_LANDED:                  branch = Scripts_IfBool(en, globalCtx, en->pickedUpState == (in->subId - IF_PICKUP_IDLE), in); break; 
        case IF_LENS_OF_TRUTH_ON:               branch = Scripts_IfBool(en, globalCtx, globalCtx->actorCtx.unk_03 != 0, in); break; 

        case IF_PLAYER_RUPEES:                  branch = Scripts_IfValue(en, globalCtx, gSaveContext.rupees, in, INT16); break;
        case IF_SCENE_ID:                       branch = Scripts_IfValue(en, globalCtx, globalCtx->sceneNum, in, INT16); break;
        case IF_ROOM_ID:                        branch = Scripts_IfValue(en, globalCtx, globalCtx->roomCtx.curRoom.num, in, INT8); break;
        case IF_PLAYER_SKULLTULAS:              branch = Scripts_IfValue(en, globalCtx, gSaveContext.inventory.gsTokens, in, INT16); break;
        case IF_PATH_NODE:                      branch = Scripts_IfValue(en, globalCtx, en->curPathNode, in, INT16); break;
        case IF_ANIMATION_FRAME:                branch = Scripts_IfValue(en, globalCtx, (u16)en->skin.skelAnime.curFrame, in, INT16); break;
        case IF_CUTSCENE_FRAME:                 branch = Scripts_IfValue(en, globalCtx, globalCtx->csCtx.frames, in, INT16); break;
        case IF_PLAYER_HEALTH:                  branch = Scripts_IfValue(en, globalCtx, gSaveContext.health, in, INT16); break;       
        
        case IF_PLAYER_BOMBS:                   
        case IF_PLAYER_BOMBCHUS:                   
        case IF_PLAYER_ARROWS:                      
        case IF_PLAYER_DEKUNUTS:               
        case IF_PLAYER_DEKUSTICKS:                  
        case IF_PLAYER_BEANS:                    
        case IF_PLAYER_SEEDS:                   branch = Scripts_IfValue(en, globalCtx, gSaveContext.inventory.ammo[inventory_set_slots[in->subId - IF_PLAYER_BOMBS][1]], in, INT16); break;      

        case IF_STICK_X:                        branch = Scripts_IfValue(en, globalCtx, globalCtx->state.input->cur.stick_x, in, INT8); break;
        case IF_STICK_Y:                        branch = Scripts_IfValue(en, globalCtx, globalCtx->state.input->cur.stick_y, in, INT8); break;

        case IF_ITEM_BEING_TRADED:              
        {
            // If we're not in the trading radius, and not talking, then we're definitely not trading anything.
            if (!en->canTrade && !en->isTalking)
                branch = in->falseInstrNum;
            // Otherwise, check if the item being traded matches
            else
                branch = Scripts_IfValue(en, globalCtx, PLAYER->exchangeItemId, in, INT32); 

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
            u32 tradeStatusToCheck = Scripts_GetVarval(en, globalCtx, in->vartype, in->value, false);
            s8 curTraded = PLAYER->exchangeItemId;

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
        case IF_PLAYER_MASK: branch = Scripts_IfValue(en, globalCtx, PLAYER->currentMask, in, UINT8); break;
        case IF_TIME_OF_DAY: branch = Scripts_IfValue(en, globalCtx, gSaveContext.dayTime, in, UINT16); break;
        case IF_ANIMATION: branch = Scripts_IfValue(en, globalCtx, en->currentAnimId, in, UINT16); break;
        case IF_PLAYER_HAS_INVENTORY_ITEM: 
        {
            u32 item = (u32)Scripts_GetVarval(en, globalCtx, in->vartype, in->value, false);

            if (item == ITEM_BOTTLE)
                branch = Scripts_IfBool(en, globalCtx, Inventory_HasEmptyBottle(), in);
            else if (IS_BOTTLE_ITEM(item))
                branch = Scripts_IfBool(en, globalCtx, Inventory_HasSpecificBottle(item), in);
            else
                branch = Scripts_IfBool(en, globalCtx, INV_CONTENT(item) == item, in); 
                
            break; 
        }
        case IF_PLAYER_HAS_QUEST_ITEM: branch = Scripts_IfBool(en, globalCtx, CHECK_QUEST_ITEM((u32)Scripts_GetVarval(en, globalCtx, in->vartype, in->value, false)), in); break; 
        case IF_PLAYER_HAS_DUNGEON_ITEM:
        {
            ScrInstrDoubleIf* instr = (ScrInstrDoubleIf*)in;
            u32 dungeon = Scripts_GetVarval(en, globalCtx, instr->varType1, instr->value1, false);
            u32 item = Scripts_GetVarval(en, globalCtx, instr->varType2, instr->value2, false);

            branch = Scripts_IfBoolTwoValues(en, globalCtx, CHECK_DUNGEON_ITEM(item, dungeon), in); 
            break; 
        }
        case IF_BUTTON_PRESSED: 
        {
            bool pressed = CHECK_BTN_ALL(globalCtx->state.input->press.button, (u32)Scripts_GetVarval(en, globalCtx, in->vartype, in->value, false));
            branch = Scripts_IfBool(en, globalCtx, pressed, in); 
            break;
        }
        case IF_BUTTON_HELD: 
        {
            bool held = CHECK_BTN_ALL(globalCtx->state.input->cur.button, (u32)Scripts_GetVarval(en, globalCtx, in->vartype, in->value, false));
            branch = Scripts_IfBool(en, globalCtx, held, in); 
            break;
        }
        case IF_TARGETTED: branch = Scripts_IfBool(en, globalCtx, globalCtx->actorCtx.targetCtx.targetedActor == &en->actor, in); break;
        case IF_DISTANCE_FROM_PLAYER: 
        {
            branch = Scripts_IfValue(en, globalCtx, en->actor.xzDistToPlayer - PLAYER->cylinder.dim.radius - en->settings.collisionRadius, in, FLOAT); 
            break;
        }

        case IF_EXT_VAR:
        {
            ScrInstrExtVarIf* instr = (ScrInstrExtVarIf*)in;
            u32 actor_id = Scripts_GetVarval(en, globalCtx, instr->actorNumVarType, instr->actorNum, false);
            NpcMaker* ex_actor = Scene_GetNpcMakerByID(en, globalCtx, actor_id);

            if (ex_actor == NULL)
                branch = in->falseInstrNum;
            else
                branch = Scripts_IfExtVar(en, globalCtx, ex_actor->scriptVars[instr->extVarNum], in, INT32);
                
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

            branch = Scripts_IfValue(en, globalCtx, i, in, INT32); 
            break;
        }

        case SUBT_RANDOM:
        {
            ScrInstrDoubleIf* instr = (ScrInstrDoubleIf*)in;
            s16 value = Scripts_GetVarval(en, globalCtx, instr->varType1, instr->value1, true);
            branch = Scripts_IfTwoValues(en, globalCtx, value, in, INT16);
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
            void* addr = Scripts_RamSubIdSetup(en, globalCtx, instr->value1.ui32, instr->subId, &valt);
            
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

            branch = Scripts_IfTwoValues(en, globalCtx, value, in, valt);
            break;
        }
    }


    script->curInstrNum = branch;
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionAwait(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrAwait* in)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: AWAIT with subtype %02d.", en->npcId, in->subId);
    #endif
    
    bool firstRun = Scripts_SetupTemp(script, in);

    // On the first run, generate a new random number and store it.
    if (firstRun)
        script->tempValues[0] = Rand_Next();
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
            script->waitTimer = Scripts_GetVarval(en, globalCtx, in->varType, in->value, false); 
            script->curInstrNum++; 
            Rand_Seed(script->tempValues[1]);
            Rand_Next();
            return SCRIPT_STOP;
        }
        case AWAIT_RESPONSE:                
        {
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
 
            conditionMet = Scripts_IfFlag(en, globalCtx, &instr);
            break;
        }
        case AWAIT_FOREVER:                         conditionMet = false; break;
        case AWAIT_MOVEMENT_PATH_END:               conditionMet = Scripts_AwaitBool(en, globalCtx, (en->stopped && (en->curPathNode == en->curPathNumNodes)), C_TRUE); break;
        case AWAIT_TALKING_END:                     conditionMet = Scripts_AwaitBool(en, globalCtx, en->talkingFinished, C_TRUE); break;
        case AWAIT_TEXTBOX_ON_SCREEN:               conditionMet = Scripts_AwaitBool(en, globalCtx, func_8010BDBC(&globalCtx->msgCtx) != MSGSTATUS_NONE, in->condition); break;
        case AWAIT_TEXTBOX_DRAWING:                 conditionMet = Scripts_AwaitBool(en, globalCtx, func_8010BDBC(&globalCtx->msgCtx) != MSGSTATUS_DRAWING, in->condition); break;
        case AWAIT_TEXTBOX_DISMISSED:               conditionMet = Scripts_AwaitBool(en, globalCtx, globalCtx->msgCtx.msgMode == MSGMODE_CLOSING, C_TRUE); break;
        case AWAIT_PATH_NODE:                       conditionMet = Scripts_AwaitValue(en, globalCtx, en->curPathNode, INT16, in->condition, in->varType, in->value); break;
        case AWAIT_ANIMATION_FRAME:                 conditionMet = Scripts_AwaitValue(en, globalCtx, en->skin.skelAnime.curFrame, UINT32, in->condition, in->varType, in->value); break;
        case AWAIT_CUTSCENE_FRAME:                  conditionMet = Scripts_AwaitValue(en, globalCtx, globalCtx->csCtx.frames, UINT16, in->condition, in->varType, in->value); break;
        case AWAIT_TIME_OF_DAY:                     conditionMet = Scripts_AwaitValue(en, globalCtx, gSaveContext.dayTime, UINT16, in->condition, in->varType, in->value); break;
        case AWAIT_TEXTBOX_NUM:                     conditionMet = Scripts_AwaitValue(en, globalCtx, en->textboxNum + 1, INT8, C_MOREOREQ, in->varType, in->value); break;
        case AWAIT_STICK_X:                         conditionMet = Scripts_AwaitValue(en, globalCtx, globalCtx->state.input->cur.stick_x, INT8, in->condition, in->varType, in->value); break;
        case AWAIT_STICK_Y:                         conditionMet = Scripts_AwaitValue(en, globalCtx, globalCtx->state.input->cur.stick_y, INT8, in->condition, in->varType, in->value); break;
        case AWAIT_BUTTON_HELD:                     conditionMet = CHECK_BTN_ALL(globalCtx->state.input->cur.button, (u32)Scripts_GetVarval(en, globalCtx, in->varType, in->value, false)); break;
        case AWAIT_BUTTON_PRESSED:                  conditionMet = CHECK_BTN_ALL(globalCtx->state.input->press.button, (u32)Scripts_GetVarval(en, globalCtx, in->varType, in->value, false)); break;
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
                script->tempValues[2] = (s32)PLAYER->skelAnime.animation;

            conditionMet = (PLAYER->skelAnime.curFrame >= PLAYER->skelAnime.endFrame - 1 - PLAYER->skelAnime.playSpeed) || ((s32)PLAYER->skelAnime.animation != script->tempValues[2]);
            break;               
        }
        case AWAIT_EXT_VAR: 
        {
            ScrInstrExtVarAwait* instr = (ScrInstrExtVarAwait*)in;

            u32 actor_id = Scripts_GetVarval(en, globalCtx, instr->actorNumVarType, instr->actorNum, false);
            NpcMaker* exActor = Scene_GetNpcMakerByID(en, globalCtx, actor_id);

            conditionMet = Scripts_AwaitValue(en, globalCtx, exActor->scriptVars[instr->extVarNum], INT32, instr->condition, instr->varType, instr->value); break;
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
            void* addr = Scripts_RamSubIdSetup(en, globalCtx, instr->value.ui32, instr->subId, &valType);
            
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

            conditionMet = Scripts_AwaitValue(en, globalCtx, value, valType, instr->condition, instr->varType2, instr->value2);
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

bool Scripts_InstructionGoto(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrGoto* in)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: GOTO going to %04d.", en->npcId, in->instrNum);
    #endif

    script->curInstrNum = in->instrNum == SCRIPT_RETURN ? script->startInstrNum : in->instrNum;
    return in->instrNum != SCRIPT_RETURN;
}

bool Scripts_InstructionSet(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrSet* in)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: SET with subtype %02d.", en->npcId, in->subId);
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
        case SET_ALPHA:                             Scripts_Set(en, globalCtx, AADDR(en, basic_set_offsets[in->subId]), in, UINT8); break;

        case SET_MOVEMENT_DISTANCE:           
        case SET_MAXIMUM_ROAM:      
        case SET_MOVEMENT_LOOP_DELAY:               
        case SET_ATTACKED_SFX:                   
        case SET_LIGHT_RADIUS:                      Scripts_Set(en, globalCtx, AADDR(en, basic_set_offsets[in->subId]), in, UINT16); break;
        case SET_CUTSCENE_FRAME:                    Scripts_Set(en, globalCtx, AADDR(globalCtx, basic_set_offsets[in->subId]), in, UINT16); break;

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
        case SET_LOOKAT_OFFSET_X:                   
        case SET_LOOKAT_OFFSET_Y:                   
        case SET_LOOKAT_OFFSET_Z:                   
        case SET_CURRENT_PATH_NODE:                 
        case SET_CURRENT_ANIMATION_FRAME:           
        case SET_LIGHT_OFFSET_X:                    
        case SET_LIGHT_OFFSET_Y:                    
        case SET_LIGHT_OFFSET_Z:                    
        case SET_TIMED_PATH_START_TIME:             
        case SET_TIMED_PATH_END_TIME:               Scripts_Set(en, globalCtx, AADDR(en, basic_set_offsets[in->subId]), in, INT16); break;

        case SET_MOVEMENT_SPEED:                    
        case SET_TALK_RADIUS:                       
        case SET_SMOOTHING_CONSTANT:                
        case SET_SHADOW_RADIUS:                     Scripts_Set(en, globalCtx, AADDR(en, basic_set_offsets[in->subId]), in, FLOAT); break;

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
        case SET_TALK_PERSIST:                      Scripts_Set(en, globalCtx, AADDR(en, basic_set_offsets[in->subId]), in, BOOL); break;
        case SET_TIME_OF_DAY:                       
        {
            bool first_run = Scripts_SetupTemp(script, in);
        
            if (first_run)
            {
                u16 end_time = gSaveContext.dayTime;
                Scripts_MathOperation(&end_time, Scripts_GetVarval(en, globalCtx, in->varType, in->value, true), in->operator, INT16);
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
        case SET_NO_AUTO_ANIM:                      en->autoAnims = !Scripts_GetBool(en, globalCtx, in); break;

        case SET_PRESS_SWITCHES:                
        case SET_IS_TARGETTABLE:                
        case SET_VISIBLE_ONLY_UNDER_LENS:       
        case SET_IS_ALWAYS_ACTIVE:              
        case SET_IS_ALWAYS_DRAWN:                   Scripts_ToggleActorFlag(en, 
                                                                           AADDR(en, toggle_offsets[in->subId - SET_PRESS_SWITCHES][0]), 
                                                                           Scripts_GetBool(en, globalCtx, in), 
                                                                           toggle_offsets[in->subId - SET_PRESS_SWITCHES][1]); break;

        case SET_REACTS_IF_ATTACKED:
        {
            bool val = Scripts_GetBool(en, globalCtx, in);
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
        case SET_CASTS_SHADOW:
        {
            bool val = Scripts_GetBool(en, globalCtx, in);

            if (!val)
                en->actor.shape.shadowDraw = NULL;
            else
                ActorShape_Init(&en->actor.shape, 0.0f, ActorShadow_DrawCircle, en->settings.shadowRadius);

            en->actor.shape.shadowAlpha = SHADOW_ALPHA;
            en->settings.castsShadow = val;
        }
        case SET_GENERATES_LIGHT:  
        {
            bool value = Scripts_GetBool(en, globalCtx, in);

            if (value)
            {
                if (en->lightNode == NULL)
                    en->lightNode = LightContext_InsertLight(globalCtx, &globalCtx->lightCtx, &en->light);
            }
            else
            {
                if (en->lightNode != NULL)
                    LightContext_RemoveLight(globalCtx, &globalCtx->lightCtx, en->lightNode);

                en->lightNode = NULL;
            }

            en->settings.generatesLight = value;
            break;
        }
        case SET_MOVEMENT_TYPE:     
        {
            en->actor.home.pos = en->actor.world.pos;
            Scripts_Set(en, globalCtx, &en->settings.movementType, in, UINT8);
            Movement_StopMoving(en, globalCtx, false);
            break;
        }       
        case SET_TALK_MODE:                         
        {
            bool value = Scripts_GetBool(en, globalCtx, in);

            en->autoAnims = !value;
            en->canMove = !value;
            en->stopPlayer = value;

            if (value)
                PLAYER->stateFlags1 |= PLAYER_STOPPED_MASK;
            else
                PLAYER->stateFlags1 &= ~PLAYER_STOPPED_MASK;

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
        case SET_PLAYER_HEALTH:                     Scripts_SetInventory(en, globalCtx, inventory_set_slots[in->subId - SET_PLAYER_BOMBS], in); break;  

        case SET_ENV_COLOR:                         Scripts_SetColor(en, globalCtx, &en->settings.envColor, in); break;
        case SET_LIGHT_COLOR:                       Scripts_SetColor(en, globalCtx, &en->settings.lightColor, in); break;

        case SET_RESPONSE_ACTIONS:                  script->responsesInstrNum = script->curInstrNum; break;

        case SET_ANIMATION_OBJECT: 
        case SET_ANIMATION_OFFSET:
        case SET_ANIMATION_SPEED:
        case SET_ANIMATION_STARTFRAME:
        case SET_ANIMATION_ENDFRAME:                Scripts_SetAnimation(en, globalCtx, in); break;

        case SET_DLIST_OFFSET:
        case SET_DLIST_TRANS_X:
        case SET_DLIST_TRANS_Y:
        case SET_DLIST_TRANS_Z:
        case SET_DLIST_ROT_X:
        case SET_DLIST_ROT_Y:
        case SET_DLIST_ROT_Z:
        case SET_DLIST_SCALE:
        case SET_DLIST_OBJECT:
        case SET_DLIST_LIMB:                        Scripts_SetDList(en, globalCtx, in); break;
        case SET_DLIST_COLOR:
        {
            ScrInstrDlistColorSet* instr = (ScrInstrDlistColorSet*)in;

            int dlistId = Scripts_GetVarval(en, globalCtx, instr->varTypeDListID, instr->DListId, false);

            en->extraDLists[dlistId].envColor.r = Scripts_GetVarval(en, globalCtx, instr->varTypeR, instr->R, false);
            en->extraDLists[dlistId].envColor.g = Scripts_GetVarval(en, globalCtx, instr->varTypeG, instr->G, false);
            en->extraDLists[dlistId].envColor.b = Scripts_GetVarval(en, globalCtx, instr->varTypeB, instr->B, false);
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
        case SET_FLAG_INTERNAL:                   Scripts_SetFlag(en, globalCtx, in); break;
        case SET_MASS: 
        {
            Scripts_Set(en, globalCtx, &en->settings.mass, in, UINT8);
            en->actor.colChkInfo.mass = en->settings.mass;
            break;
        }
        case SET_GRAVITY_FORCE: 
        {
            Scripts_Set(en, globalCtx, &en->settings.gravity, in, FLOAT); 
            en->actor.gravity = en->settings.gravity;
            break;
        }
        case SET_MOVEMENT_PATH_ID:
        {
            Scripts_Set(en, globalCtx, &en->settings.pathId, in, UINT8);
            Setup_Path(en, globalCtx, en->settings.pathId);
            break;
        }       
        case SET_PLAYER_CAN_MOVE:
        {
            en->stopPlayer = !Scripts_GetBool(en, globalCtx, in);

            if (en->stopPlayer)
                PLAYER->stateFlags1 |= PLAYER_STOPPED_MASK;
            else
                PLAYER->stateFlags1 &= ~PLAYER_STOPPED_MASK;

            break;
        }
        case SET_ACTOR_CAN_MOVE:
        {
            en->canMove = Scripts_GetBool(en, globalCtx, in);
            Movement_SetNextDelay(en);
            break;
        }
        case SET_PLAYER_ANIMATE_MODE:
        {
            bool val = Scripts_GetBool(en, globalCtx, in);

            if (val)
            {
                if ((void*)PLAYER->actor.update != &Scripts_PlayerAnimateMode)
                {
                    script->PlayerUpdate = PLAYER->actor.update;
                    PLAYER->actor.update = (void*)&Scripts_PlayerAnimateMode;
                }
            }
            else
            {
                if ((void*)PLAYER->actor.update == &Scripts_PlayerAnimateMode)
                {
                    PLAYER->actor.update = script->PlayerUpdate;
                    script->PlayerUpdate = NULL;
                }
            }
            break;
        }
        case SET_ANIMATION:
        case SET_ANIMATION_INSTANTLY:
        {
            ScrInstrDoubleSet* instr = (ScrInstrDoubleSet*)in;

            s32 animId = Scripts_GetVarval(en, globalCtx, instr->varType1, instr->value1, false);
            s32 once = Scripts_GetVarval(en, globalCtx, instr->varType2, instr->value2, false);

            Setup_Animation(en, globalCtx, animId, instr->subId == SET_ANIMATION, once, true, false);
            break;
        }        
        case SET_PLAYER_ANIMATION:
        {
            ScrInstrSetPlayerAnim* instr = (ScrInstrSetPlayerAnim*)in;

            u32 offset = Scripts_GetVarval(en, globalCtx, instr->offsetType, instr->offset, false);
            u32 startFrame = Scripts_GetVarval(en, globalCtx, instr->startFrameType, instr->startFrame, false);
            u32 endFrame = Scripts_GetVarval(en, globalCtx, instr->endFrameType, instr->endFrame, false);
            float speed = Scripts_GetVarval(en, globalCtx, instr->speedType, instr->speed, false);   

            Setup_AnimationImpl(&PLAYER->actor, globalCtx, &PLAYER->skelAnime, offset, ANIMTYPE_LINK, -1, -1, startFrame, endFrame, speed, true, instr->once);
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
            int segId = Scripts_GetVarval(en, globalCtx, instr->varType1, instr->value1, false);
            int segEntryId = Scripts_GetVarval(en, globalCtx, instr->varType2, instr->value2, false);
            en->segmentDataIds[segId] = segEntryId;
            break;
        }
        case SET_DLIST_VISIBILITY:
        {   
            ScrInstrDoubleSet* instr = (ScrInstrDoubleSet*)in;
            int showType = Scripts_GetVarval(en, globalCtx, instr->varType1, instr->value1, false);
            int dListId = Scripts_GetVarval(en, globalCtx, instr->varType2, instr->value2, false);
            en->extraDLists[dListId].showType = showType;
            break;
        }
        case SET_CAMERA_TRACKING_ON:
        {
            ScrInstrActorSet* instr = (ScrInstrActorSet*)in;
            void* actor = Scripts_GetActorByType(en, globalCtx, instr->target, instr->actorNumType, instr->actorNum);

            if (actor != NULL)
                globalCtx->cameraPtrs[globalCtx->activeCamera]->player = (Player*)actor;

            break;
        }
        case SET_REF_ACTOR:
        {
            ScrInstrActorSet* instr = (ScrInstrActorSet*)in;
            en->refActor = Scripts_GetActorByType(en, globalCtx, instr->target, instr->actorNumType, instr->actorNum);
            break;
        }
        case SET_EXT_VAR:
        {
            ScrInstrExtVarSet* instr = (ScrInstrExtVarSet*)in;
            u32 actorId = Scripts_GetVarval(en, globalCtx, instr->actorNumVarType, instr->actorNum, false);
            NpcMaker* exActor = Scene_GetNpcMakerByID(en, globalCtx, actorId);

            if (exActor != NULL)
            {
                Scripts_MathOperation(&exActor->scriptVars[instr->extVarNum - 1], 
                                    Scripts_GetVarval(en, globalCtx, instr->varType, instr->value, true), 
                                    instr->operator, 
                                    INT32);
            }
            
            break;
        }
        case SET_ATTACKED_EFFECT: 
        {
            Scripts_Set(en, globalCtx, &en->settings.effectIfAttacked, in, UINT8);
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
            void* addr = Scripts_RamSubIdSetup(en, globalCtx, instr->value1.ui32, instr->subId, &valt);

            if (addr == NULL)
                break;

            Scripts_MathOperation(addr, 
                                  Scripts_GetVarval(en, globalCtx, instr->varType2, instr->value2, true), 
                                  instr->operator, 
                                  valt);
								 	  
            break;
        }
        default: break;
    }

    script->curInstrNum++;
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionEnableTalking(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrTextbox* in)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: ENABLE_TALKING", en->npcId);
    #endif  

    if (en->wasHit)
    {
        script->curInstrNum++;
        return SCRIPT_CONTINUE;
    }

    //z_actor_poll_speak_cube
    en->canTalk = func_8002F2CC(&en->actor, globalCtx, en->settings.talkRadius + en->collider.dim.radius);

    u32 id = Scripts_GetTextId(en, globalCtx, in->vartypeChild, in->childMsgId, in->varTypeAdult, in->adultMsgId);
    Scripts_SetMessage(en, globalCtx, id, &en->actor.textId, false, true);

    script->curInstrNum++;
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionShowTextbox(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrTextbox* in)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: SHOW_TEXTBOX", en->npcId);
    #endif  

    u32 id = Scripts_GetTextId(en, globalCtx, in->vartypeChild, in->childMsgId, in->varTypeAdult, in->adultMsgId);

    if (in->id == SHOW_TEXTBOX)
    {
        Scripts_SetMessage(en, globalCtx, id, &en->actor.textId, true, true);

        // We need to set this so that await talking_end still works
        en->isTalking = true;
        en->textboxDisplayed = true;
        en->talkingFinished = false;
    }
    else
    {
        Scripts_SetMessage(en, globalCtx, id, NULL, true, false);

        if (id > __INT16_MAX__)
            Message_Overwrite(en, globalCtx, R_CUSTOM_MSG_ID(id));   
    }

    script->curInstrNum++;
    return SCRIPT_STOP;
}

bool Scripts_InstructionEnableTrade(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrTrade* in)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: ENABLE_TRADE", en->npcId);
    #endif  

    if (en->wasHit)
    {
        script->curInstrNum++;
        return SCRIPT_CONTINUE;
    }

    // Find which item we're meant to check for, and which item Link is actually trading.
    en->tradeItem = Scripts_GetVarval(en, globalCtx, in->correct.varTypeItem, in->correct.item, false);
    s8 curTradedItem = PLAYER->exchangeItemId;

    if (!en->isTalking)
    {
        if (en->canTrade)
        {
            // If the item being traded is correct, set the correct text ID.
            if (en->tradeItem == curTradedItem)
            {
                int id = Scripts_GetTextId(en, globalCtx, in->correct.varTypeChild, in->correct.childMsgId, in->correct.varTypeAdult, in->correct.adultMsgId);
                Scripts_SetMessage(en, globalCtx, id, &en->actor.textId, false, true);
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
                        int fail_item = Scripts_GetVarval(en, globalCtx, in->failure[i].varTypeItem, in->failure[i].item, true);

                        // Set the message if the item matches, or item in the list is -1 (which is the default for all items not set specifically, and shuffled to always be at the very end).
                        if (fail_item == -1 || fail_item == curTradedItem)
                        {
                            int id = Scripts_GetTextId(en, globalCtx, in->failure[i].varTypeChild, in->failure[i].childMsgId, in->failure[i].varTypeAdult, in->failure[i].adultMsgId);
                            Scripts_SetMessage(en, globalCtx, id, &en->actor.textId, false, true);
                            break;
                        }
                    }
                }
                // If nothing is being traded, set that text id (this is what will be displayed when the player speaks to the NPC)
                else
                {
                    int id = Scripts_GetTextId(en, globalCtx, in->varTypeTalkChild, in->childTalkMsgId, in->varTypeTalkAdult, in->adultTalkMsgId);
                    Scripts_SetMessage(en, globalCtx, id, &en->actor.textId, false, true);         
                }
            }
        }

        //z_actor_poll_trade_cube
        en->canTrade = func_8002F298(&en->actor, globalCtx, en->settings.talkRadius + en->collider.dim.radius, EXCH_ITEM_BLUE_FIRE);

    }

    script->curInstrNum++;
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionFace(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrFace* in)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: FACE", en->npcId);
    #endif      

    if (en->pickedUpState != STATE_IDLE)
        return Scripts_FreeAndContinue(script); 

    bool firstRun = Scripts_SetupTemp(script, in);

    if (firstRun)
    {
        script->tempValues[2] = (s32)Scripts_GetActorByType(en, globalCtx, in->subject, in->subjectActorNumType, in->subjectActorNum);
        script->tempValues[3] = (s32)Scripts_GetActorByType(en, globalCtx, in->target, in->targetActorNumType, in->targetActorNum);

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

bool Scripts_InstructionRotation(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrRotation* in)
{
    #define ACTOR ((Actor*)script->tempValues[0])
    #define SPEED (script->fTempValues[4])
    #define ROT ((Vec3f*)&script->fTempValues[1])

    #if LOGGING == 1
        osSyncPrintf("_%2d: ROTATE", en->npcId);
    #endif  

    bool firstRun = Scripts_SetupTemp(script, in);

    // On the first run, we gather the needed data from the instruction.
    // We can't do this every time the instruction is run, because randomness would mess it up.
    // Could just seed the random number, but this also ends up being more efficient...
    if (firstRun)
    {
        // Actor
        script->tempValues[0] = (s32)Scripts_GetActorByType(en, globalCtx, in->target, in->actorNumType, in->actorNum);

        // Speed
        SPEED = Scripts_GetVarval(en, globalCtx, in->speedType, in->speed, true);

        Vec3f rot = Scripts_GetVarvalVec3f(en, globalCtx, (Vartype[]){in->xType, in->yType, in->zType}, (ScriptVarval[]){in->x, in->y, in->z}, 1);
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

bool Scripts_InstructionPosition(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrPosition* in)
{
    #define ACTOR ((Actor*)script->tempValues[0])
    #define NPCACTOR ((NpcMaker*)script->tempValues[0])
    #define ENDPOS ((Vec3f*)&script->fTempValues[1])
    #define SPEED (script->fTempValues[4])

    #if LOGGING == 1
        osSyncPrintf("_%2d: POSITION", en->npcId);
    #endif  

    bool first_run = Scripts_SetupTemp(script, in);

    // On the first run, we gather the needed data from the instruction.
    // We can't do this every time the instruction is run, because randomness would mess it up.
    // Could just seed the random number, but this also ends up being more efficient...
    if (first_run)
    {
        // Actor
        script->tempValues[0] = (s32)Scripts_GetActorByType(en, globalCtx, in->target, in->actorNumType, in->actorNum);

        if (ACTOR != NULL)
        {
            // Position
            Vec3f pos = Scripts_GetVarvalVec3f(en, globalCtx, (Vartype[]){in->xType, in->yType, in->zType}, (ScriptVarval[]){in->x, in->y, in->z}, 1);

            if (in->subId == POS_MOVE_BY_DIRECTION)
                Math_AffectMatrixByRot(en->actor.shape.rot.y, &pos);

            if (in->subId >= POS_MOVE_BY)
                Math_Vec3f_Sum(&pos, &ACTOR->world.pos, &pos);

            Math_Vec3f_Copy(ENDPOS, &pos);

            SPEED = Scripts_GetVarval(en, globalCtx, in->speedType, in->speed, true);
        }
    }

    if (ACTOR == NULL)
        return Scripts_FreeAndContinue(script);  

    bool isNpcMaker = (ACTOR->id == en->actor.id);

    // If actor is an NPC Maker actor, but can't move, we do nothing until he can (this is useful for stuff like getting hit)
    if (isNpcMaker && !NPCACTOR->canMove)
        return SCRIPT_STOP;

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

        Setup_Animation(en, globalCtx, ANIM_WALK, true, false, false, !NPCACTOR->autoAnims);
    }

    // Caculate movement vector, add it to the position and rotate towards the goal.
    Vec3f movVec = Movement_CalcVector(&ACTOR->world.pos, ENDPOS, SPEED);
    Math_Vec3f_Sum(&ACTOR->world.pos, &movVec, &ACTOR->world.pos);

    // Calculate if we're there yet.
    float distFromEnd = Movement_CalcDist(&ACTOR->world.pos, ENDPOS, in->ignoreY);
    float distFromEndXZ = in->ignoreY ? distFromEnd : Movement_CalcDist(&ACTOR->world.pos, ENDPOS, true);

    // If we aren't there yet, rotate towards the destination and stop executing script for this frame.
    if (distFromEnd > MOVEMENT_DISTANCE_EQUAL_MARGIN)
    {
        // Only rotate if there's actual XZ distance to go, though.
        if (distFromEndXZ != 0)
            Movement_RotTowards(&ACTOR->shape.rot.y, Math_Vec3f_Yaw(&ACTOR->world.pos, ENDPOS), 0);

        return SCRIPT_STOP;
    }
    else
    {
        // Handle switching the animation back to idle if this is the NPC Maker actor.
        if (isNpcMaker)
        {
            NPCACTOR->isMoving = false;
            NPCACTOR->stopped = true;
            NPCACTOR->settings.movementType = script->tempValues[1];

            Setup_Animation(en, globalCtx, ANIM_IDLE, true, false, false, !NPCACTOR->autoAnims);
        }

        return Scripts_FreeAndContinue(script);
    }

    #undef ACTOR
    #undef NPCACTOR
    #undef ENDPOS
    #undef SPEED
}

bool Scripts_InstructionScale(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrScale* in)
{
    #define ACTOR ((Actor*)script->tempValues[0])
    #define SPEED (script->fTempValues[0])
    #define SCALE (script->fTempValues[1])

    #if LOGGING == 1
        osSyncPrintf("_%2d: SCALE", en->npcId);
    #endif  

    bool firstRun = Scripts_SetupTemp(script, in);

    // On the first run, we gather the needed data from the instruction.
    // We can't do this every time the instruction is run, because randomness would mess it up.
    // Could just seed the random number, but this also ends up being more efficient...
    if (firstRun)
    {
        // Actor
        script->tempValues[0] = (s32)Scripts_GetActorByType(en, globalCtx, in->target, in->actorNumType, in->actorNum);

        SPEED = Scripts_GetVarval(en, globalCtx, in->speed_type, in->speed, true);
        SCALE = Scripts_GetVarval(en, globalCtx, in->scale_type, in->scale, true);
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

bool Scripts_InstructionPlay(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrPlay* in)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: PLAY", en->npcId);
    #endif     

    u32 value = Scripts_GetVarval(en, globalCtx, in->varType, in->value, false);

    switch (in->subId)
    {
        case PLAY_SFX: Audio_PlayActorSound2(&en->actor,value); break;
        case PLAY_BGM: Audio_SetBGM(value); break;
        case PLAY_CUTSCENE: 
		{
			Cutscene_SetSegment(globalCtx, (u32)Scene_GetCurrentCutscenePtr(globalCtx)); 
			
			if (globalCtx->csCtx.segment != NULL)
				gSaveContext.cutsceneTrigger = 1;
	
			break;
		}
        case PLAY_CUTSCENE_ID: 
		{
			Cutscene_SetSegment(globalCtx, (u32)Scene_GetCutscenePtr(globalCtx, value)); 
			
			if (globalCtx->csCtx.segment != NULL)
				gSaveContext.cutsceneTrigger = 1;
			
			break;
		}
        case PLAY_SFX_GLOBAL: Audio_PlaySoundGeneral(value, VEC_ZERO, 4, FLOAT_ONE, FLOAT_ONE, FLOAT_ZERO); break;
    }

    script->curInstrNum++;
    return SCRIPT_CONTINUE;
}

bool Scripts_InstructionKill(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrKill* in)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: KILL", en->npcId);
    #endif   

    Actor* actor = Scripts_GetActorByType(en, globalCtx, in->subId, in->actorNumType, in->actorNum);

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

bool Scripts_InstructionOcarina(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrOcarina* in)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: OCARINA", en->npcId);
    #endif   

    u32 song = Scripts_GetVarval(en, globalCtx, in->ocaSongType, in->ocaSong, false);

    if (!en->listeningToSong)
    {
        if ((en->settings.talkRadius + en->collider.dim.radius) >= en->actor.xzDistToPlayer || 
            globalCtx->actorCtx.targetCtx.targetedActor == &en->actor)
        {
            PLAYER->stateFlags2 |= 0x800000;
            en->actor.flags |= 0x02000000;

            // Check if player has entered the ocarina state.
            if (PLAYER->stateFlags2 & 0x1000000)
            {
                func_8010BD88(globalCtx, 0x22);
                PLAYER->stateFlags2 |= 0x2000000;
                PLAYER->unk_6A8 = &en->actor;

                #if LOGGING == 1
                    osSyncPrintf("_%2d: Player whipped out an ocarina!", en->npcId);
                #endif   

                // Show prompt. For songs game officially recognizes as playable, use the built in method.
                // Otherwise, we're listening to song 0 (any song).
                // Set actor as listening to song.
                // z_ocarina_show_prompt
                func_8010BD58(globalCtx, song > 5 ? song : 0);
                en->listeningToSong = true;
                Movement_StopMoving(en, globalCtx, !en->autoAnims);
            }
        }

        script->curInstrNum = in->falseInstrNum;
    }
    else
    {
        u16* playedSong = &globalCtx->msgCtx.unk_E3EC;
        u16* songState = &globalCtx->msgCtx.unk_E3EE;

        if (en->correctSongHeard)
        {
            // Dumb workaround to make SURE Saria's Song text doesn't appear.
            PLAYER->stateFlags1 |= PLAYER_STOPPED_MASK;
            script->curInstrNum = in->trueInstrNum;
            en->correctSongHeard = false;
            en->listeningToSong = false;
            return SCRIPT_STOP;       
        }
        // If song is officially reconized as correct, or song played was the one specified
        // in the instruction, jump to the instruction block.
        if ((song > 5 && *songState == SONGSTATUS_CORRECT) || (song <= 5 && *playedSong == song))
        {
            #if LOGGING == 1
                osSyncPrintf("_%2d: Correct song was heard.", en->npcId);
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

bool Scripts_InstructionSpawn(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrSpawn* in)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: SPAWN", en->npcId);
    #endif
    
    Vec3f position = Scripts_GetVarvalVec3f(en, globalCtx, (Vartype[]){in->posXType, in->posYType, in->posZType}, (ScriptVarval[]){in->posX, in->posY, in->posZ}, 1);
    bool setAsRef = in->posType >= 10;
    int posType = in->posType >= 10 ? in->posType - 10 : in->posType;

    switch (posType)
    {
        case POSTYPE_DIRECTION:
            Math_AffectMatrixByRot(en->actor.shape.rot.y, &position);
        case POSTYPE_RELATIVE:
            Math_Vec3f_Sum(&position, &en->actor.world.pos, &position); break;
        default: break;
    }

    Vec3s rotation = (Vec3s){
                                Scripts_GetVarval(en, globalCtx, in->rotXType, in->rotX, true),
                                Scripts_GetVarval(en, globalCtx, in->rotYType, in->rotY, true),
                                Scripts_GetVarval(en, globalCtx, in->rotZType, in->rotZ, true),
                            };

    int actorNum = Scripts_GetVarval(en, globalCtx, in->actorNumType, in->actorNum, false);
    int actorParam = Scripts_GetVarval(en, globalCtx, in->actorParamType, in->actorParam, false);

    Actor* spawned = Actor_Spawn(&globalCtx->actorCtx, globalCtx, actorNum, position.x, position.y, position.z, rotation.x, rotation.y, rotation.z, actorParam);

    if (setAsRef)
        en->refActor = spawned;

    script->curInstrNum++;
    return SCRIPT_CONTINUE;  
}

bool Scripts_InstructionItem(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrItem* in)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: ITEM", en->npcId);
    #endif   

    u32 item = Scripts_GetVarval(en, globalCtx, in->itemVarType, in->item, true);

    // Pickup actor if giving no item, with 70% interact radius (so talking and picking up may happen at the same time)
    if (item == GI_NONE && in->subId == ITEM_GIVE)
        func_8002F434(&en->actor, globalCtx, item, en->settings.talkRadius * 0.7f, en->settings.talkRadius * 0.7f);
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

                    if (PLAYER->csMode != 0)
                    {
                        script->tempValues[0] = PLAYER->csMode;
                        //z_cutscene_link_action
                        func_8002DF54(globalCtx, &en->actor, 0x7);
                    }

                    if (en->stopPlayer)
                    {
                        script->tempValues[1] = en->stopPlayer;
                        en->stopPlayer = false;
                        PLAYER->stateFlags1 &= ~PLAYER_STOPPED_MASK;
                    }

                    return SCRIPT_STOP;
                }
                else
                {
                    // Once we have waited five frames, we give the actor an item, and wait a bit of time again (to not restore the cutscene state prematurely)...
                    if (script->tempValues[2] == -1)
                    {
                        //z_actor_give_item
                        func_8002F434(&en->actor, globalCtx, item, __UINT32_MAX__, __UINT32_MAX__);
                        script->waitTimer = 2;
                        script->tempValues[2] = 1;
                        return SCRIPT_STOP;
                    }
                    else
                    {
                        // Wait for textbox end...
                        // Player talk state
                        if (func_8010BDBC(&globalCtx->msgCtx) != MSGSTATUS_END)
                            return SCRIPT_STOP;

                        //...after which, if Link WAS in a cutscene, we restore the cutscene state.
                        //z_cutscene_link_action
                        if (script->tempValues[0] != -1)
                            func_8002DF54(globalCtx, &en->actor, script->tempValues[0]);     

                        if (script->tempValues[1] != 0)
                            en->stopPlayer = true;
                    }
                }
                
                break;
            }
            case ITEM_GIVE: 
            {
                // Don't give the player anything if they don't have an empty bottle and the item is a bottled item.
                if (IS_BOTTLE_ITEM(item))
                {
                    if (!Inventory_HasEmptyBottle())
                        break;
                }

                Item_Give(globalCtx, item);        
                break; 
            }
            case ITEM_TAKE: 
            {
                // Remove Link's mask if we're taking it away.
                if (IS_MASK(item))
                    Player_UnsetMask(globalCtx);
                // Code for bottle items
                else if (IS_BOTTLE_ITEM(item))
                {
                    // If item is holding the item in question, we take that one specifically.
                    if ((PLAYER->heldItemButton != 0) && 
                    (gSaveContext.inventory.items[gSaveContext.equips.cButtonSlots[PLAYER->heldItemButton - 1]] == item))
                        Player_UpdateBottleHeld(globalCtx, PLAYER, ITEM_BOTTLE, PLAYER_AP_BOTTLE);
                    else
                        Inventory_ReplaceItem(globalCtx, item, ITEM_BOTTLE);
                }
                else
                    Inventory_DeleteItem(item, SLOT(item));

                break;
            }
        }
    }

    return Scripts_FreeAndContinue(script);
}

bool Scripts_InstructionWarp(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrWarp* in)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: WARP", en->npcId);
    #endif  

    u32 warpId = Scripts_GetVarval(en, globalCtx, in->warpIdvarType, in->warpId, false);
    u32 cutsceneId = Scripts_GetVarval(en, globalCtx, in->cutsceneIdvarType, in->cutsceneId, false);
    u32 sceneLoadFlag = Scripts_GetVarval(en, globalCtx, in->sceneLoadFlagType, in->sceneLoadFlag, false);
    globalCtx->nextEntranceIndex = warpId;
    globalCtx->sceneLoadFlag = sceneLoadFlag;

    if (cutsceneId > 0)
        cutsceneId = 0xFFF0 + (cutsceneId - 4);

    gSaveContext.nextCutsceneIndex = cutsceneId;
    script->curInstrNum++;
    return SCRIPT_CONTINUE; 
}

bool Scripts_InstructionScript(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrScript* in)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: SCRIPT", en->npcId);
    #endif  

    u32 scriptID = Scripts_GetVarval(en, globalCtx, in->scriptIdVarType, in->scriptId, false);
    en->scriptInstances[scriptID].active = in->subID;
    script->curInstrNum++;
    return SCRIPT_CONTINUE; 
}