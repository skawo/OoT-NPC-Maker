#include "../include/h_scripts.h"
#include "../include/h_scene.h"
#include "../include/h_rom.h"
#include "../include/h_math.h"

void Scripts_ToggleActorFlag(NpcMaker* en, u8* field, bool setting, u32 mask)
{
    AVAL(field, u8, 0) = setting;

    if (setting)
        en->actor.flags |= mask;
    else
        en->actor.flags  &= ~(mask);
}

bool Scripts_GetBool(NpcMaker* en, PlayState* playState, void* instruction)
{
    ScrInstrSet* instr = (ScrInstrSet*)instruction;
    return Scripts_GetVarval(en, playState, instr->varType, instr->value, false) ? true : false;
}

float Scripts_GetVarval(NpcMaker* en, PlayState* playState, Vartype type, ScriptVarval value, bool signedVal)
{
    float out = 0;

    switch (type)
    {
        default:                out = value.flo; break;
        case RANDOM:            
        {
            s16* minMax = (s16*)&value;
            out = Math_RandGetBetween(minMax[1], minMax[0] + 1);
            break;
        }
        case GLOBAL8:           
        case GLOBAL16:          
        case GLOBAL32:          
        case GLOBALF:           
        case ACTOR8:           
        case ACTOR16:           
        case ACTOR32:           
        case ACTORF:            
        case SAVE8:            
        case SAVE16:            
        case SAVE32:            
        case SAVEF:             
        {
            u32* addrs[] = {
                                (void*)playState, 
                                (void*)en->refActor, 
                                (void*)&gSaveContext
                           };
                           
            u32* addr = addrs[(type - GLOBAL8) / 4];
            int varType = (signedVal * 4) + (type - GLOBAL8) % 4;

            switch (varType)
            {
                case 0:     out = AVAL(addr, u8, value.ui32); break;
                case 1:     out = AVAL(addr, u16, value.ui32); break;
                case 2:     out = AVAL(addr, u32, value.ui32); break;
                case 4:     out = AVAL(addr, s8, value.ui32); break;
                case 5:     out = AVAL(addr, s16, value.ui32); break;
                case 6:     out = AVAL(addr, s32, value.ui32); break;
                default:    out = AVAL(addr, float, value.ui32); break;
            }

            break;
        }
        case SCRIPT_VAR:        
        {
            if (en->scriptVars == NULL || en->settings.numVars < value.ui32)
            {
                #if LOGGING > 0
                    is64Printf("_%2d: Attempted read from script var out of range.\n");
                #endif       
            }
            else
                out = en->scriptVars[value.ui32 - 1]; 

            break;
        }
        case SCRIPT_VARF:       
        {
            if (en->scriptFVars == NULL || en->settings.numFVars < value.ui32)
            {
                out = 0;

                #if LOGGING > 0
                    is64Printf("_%2d: Attempted read from float script var out of range.\n");
                #endif       
            }
            else
                out = en->scriptFVars[value.ui32 - 1]; 

            break;
        } 
    }
    return out;
}

Vec3f Scripts_GetVarvalVec3f(NpcMaker* en, PlayState* playState, Vartype xyzType[], ScriptVarval values[], float divider)
{
    if (divider == 0)
        divider = 1;

    Vec3f out;

    for (int i = 0; i < 3; i++)
        AVAL(&out, float, i * sizeof(float)) = (Scripts_GetVarval(en, playState, xyzType[i], values[i], true) / divider);

    return out;
}

Color_RGBA8 Scripts_GetVarvalRGBA(NpcMaker* en, PlayState* playState, Vartype colorTypes[], ScriptVarval values[])
{
    Color_RGBA8 out;

    for (int i = 0; i < 4; i++)
        AVAL(&out, u8, i * sizeof(u8)) = Scripts_GetVarval(en, playState, colorTypes[i], values[i], false);

    return out;
}

// Returns address to write/read to/from
void* Scripts_RamSubIdSetup(NpcMaker* en, PlayState* playState, u32 value, u32 subId, u32* outValtype)
{
    if (subId == SUBT_VARF)
    {
        *outValtype = FLOAT; 

        if (en->scriptFVars == NULL || en->settings.numFVars < value)
        {
            #if LOGGING > 0
                is64Printf("_%2d: Attempted write to float script var out of range.\n");
            #endif   

            return NULL;
        }
        else
            return &en->scriptFVars[value - 1]; 
    }
    else if (subId == SUBT_VAR)
    {
        *outValtype = INT32; 

        if (en->scriptVars == NULL || en->settings.numVars < value)
        {
            #if LOGGING > 0
                is64Printf("_%2d: Attempted write to script var out of range.\n");
            #endif   

            return NULL;
        }
        else
            return &en->scriptVars[value - 1]; 
    }
    else
    {
        int id = subId - SUBT_GLOBAL8;
        *outValtype = 2 * (id % 4);

        switch (id / 4)
        {
            case 0:    return AADDR(playState, value); break;
            case 1:    return AADDR(en->refActor, value); break;
            case 2:    return AADDR(&gSaveContext, value); break;
            default:   return NULL; break;
        }
    }
}

void Scripts_MathOperation(void* dest, float value, Operator op, DataType dataType)
{
    CommonType var;
    float temp = 0;

    int size = dataType == FLOAT ? 4 : MAX(1, (dataType / 2) * 2);
    bcopy(dest, &var, size);

    switch (dataType)
    {
        case INT8:              temp = var.i8; break;
        case BOOL:
        case UINT8:             temp = var.ui8; break;
        case INT16:             temp = var.i16; break;
        case UINT16:            temp = var.ui16; break;
        case INT32:             temp = var.i32; break;
        case UINT32:            temp = var.ui32; break;
        case FLOAT:             temp = var.flo; break;
    }

    switch (op)
    {
        case O_SET:             temp = value; break;
        case O_ADD:             temp += value; break;
        case O_SUBTRACT:        temp -= value; break;
        case O_MULTIPLY:        temp *= value; break;
        case O_DIVIDE:          
        {
            if (value != 0)
                temp /= value;
            else
            {
                #if LOGGING > 0
                        is64Printf("_Script attempted divide by zero! Returning original value.\n");
                #endif 
            }

            break;
        } 
    }
    
    switch (dataType)
    {
        case INT8:              var.i8 = temp; break;
        case BOOL:
        case UINT8:             var.ui8 = temp; break;
        case INT16:             var.i16 = temp; break;
        case UINT16:            var.ui16 = temp; break;
        case INT32:             var.i32 = temp; break;
        case UINT32:            var.ui32 = temp; break;
        case FLOAT:             var.flo = temp; break;
    }

    bcopy(&var, dest, size);
}

void* Scripts_GetActorByType(NpcMaker* en, PlayState* playState, u32 targetActor, u8 actorNumType, 
                             ScriptVarval actorNumValue)
{
    int actorNum = Scripts_GetVarval(en, playState, actorNumType, actorNumValue, false);

    switch (targetActor)
    {
        case TARGET_SELF:              return en;
        case TARGET_PLAYER:            return GET_PLAYER(playState);
        case TARGET_NPCMAKER:          return Scene_GetNpcMakerByID(en, playState, actorNum);
        case TARGET_ACTOR_ID:          return Scene_GetActorByID(actorNum, playState, &en->actor, NULL);
        case TARGET_REF_ACTOR:         return en->refActor;
        default:                       return NULL;
    }
}

// Checks if the passed instruction is currently using temp values.
// If not, it resets them to default.
// Returns result of check.
bool Scripts_SetupTemp(ScriptInstance* script, void* instruction)
{
    if (script->instrUsingTempValuesPtr == (ScrInstr*)instruction)
        return false;
    else
    {
        Scripts_FreeTemp(script);
        script->instrUsingTempValuesPtr = instruction;
        return true;
    }
}

// Resets temp values to default.
void Scripts_FreeTemp(ScriptInstance* script)
{
    script->instrUsingTempValuesPtr = NULL;

    for (int i = 0; i < NUM_SCRIPT_TEMP_VARS; i++)
    {
        script->fTempValues[i] = -1;
        script->tempValues[i] = -1;
    }
}

bool Scripts_FreeAndContinue(ScriptInstance* script)
{
    Scripts_FreeTemp(script);
    script->curInstrNum++;
    return SCRIPT_CONTINUE; 
}

#pragma region SET

void Scripts_Set(NpcMaker* en, PlayState* playState, void* dest, void* instruction, DataType dataType)
{
    ScrInstrSet* instr = (ScrInstrSet*)instruction;

    if (dataType == BOOL)
        AVAL(dest, u8, 0) = Scripts_GetBool(en, playState, instr);
    else 
        Scripts_MathOperation(dest, Scripts_GetVarval(en, playState, instr->varType, instr->value, !(dataType % 2) || dataType == FLOAT), instr->operator, dataType);
}

void Scripts_SetInventory(NpcMaker* en, PlayState* playState, u8 slotSettings[], void* instruction)
{
    ScrInstrSet* instr = (ScrInstrSet*)instruction;

    s32 curVal = 0;

    switch (slotSettings[0])
    {
        case ITEM_RUPEE_BLUE:               curVal = gSaveContext.rupees + gSaveContext.rupeeAccumulator; break;
        case ITEM_RECOVERY_HEART:           curVal = gSaveContext.health + gSaveContext.healthAccumulator; break;
        case ITEM_MAGIC_SMALL:              curVal = gSaveContext.magicTarget; break;
        default:                            curVal = gSaveContext.inventory.ammo[slotSettings[1]]; break;
    }
    
    s32 chgVal = curVal;
    float var = Scripts_GetVarval(en, playState, instr->varType, instr->value, true);

    // For ease of use (one heart = 0x10, and we're calculating that automatically here)
    if (slotSettings[0] == ITEM_RECOVERY_HEART)
        var *= ONE_HEART;

    Scripts_MathOperation(&chgVal, var, instr->operator, INT32);
    s32 difference = chgVal - curVal;

    switch (slotSettings[0])
    {
        case ITEM_RUPEE_BLUE:               Rupees_ChangeBy(difference); break;
        case ITEM_RECOVERY_HEART:           Health_ChangeBy(playState, difference); break;
        case ITEM_MAGIC_SMALL:              
        {
            if (gSaveContext.magicTarget + chgVal < 0)
                difference = -gSaveContext.magicTarget;
            
            Magic_RequestChange(playState, difference, MAGIC_ADD); break;
        }
        default:                            Inventory_ChangeAmmo(slotSettings[0], difference); break;
    }
}

extern u8 setDlistOffsets[];
extern u8 setAnimsOffsets[];

void Scripts_SetAnimation(NpcMaker* en, PlayState* playState, void* instruction)
{
    ScrInstrDoubleSet* instr = (ScrInstrDoubleSet*)instruction;

    int animId = Scripts_GetVarval(en, playState, instr->varType1, instr->value1, false);
    int destAddrsOffset = instr->subId - SET_ANIMATION_OBJECT;
    void* destAddr = AADDR(&en->animations[animId], setAnimsOffsets[destAddrsOffset]);

    switch (instr->subId)
    {
        case SET_ANIMATION_OBJECT:          
        case SET_ANIMATION_OFFSET:          
        case SET_ANIMATION_STARTFRAME:      
        case SET_ANIMATION_ENDFRAME:        
        {
            u32 property = Scripts_GetVarval(en, playState, instr->varType2, instr->value2, false); 
            Scripts_MathOperation(destAddr, property, instr->operator, UINT32); 
            break; 
        }
        case SET_ANIMATION_SPEED:           
        {
            float property = Scripts_GetVarval(en, playState, instr->varType2, instr->value2, true);
            Scripts_MathOperation(destAddr, property, instr->operator, FLOAT); 
            break; 
        }
    }
}

void Scripts_SetDList(NpcMaker* en, PlayState* playState, void* instruction)
{
    ScrInstrDoubleSet* instr = (ScrInstrDoubleSet*)instruction;

    int dlistId = Scripts_GetVarval(en, playState, instr->varType1, instr->value1, false);
    int destAddrsOffset = instr->subId - SET_DLIST_OFFSET;
    void* destAddr = AADDR(&en->extraDLists[dlistId], setDlistOffsets[destAddrsOffset]);

    switch (instr->subId)
    {
        case SET_DLIST_OFFSET:              
        {
            u32 property = Scripts_GetVarval(en, playState, instr->varType2, instr->value2, false);
            Scripts_MathOperation(destAddr, property, instr->operator, UINT32); 
            break; 
        }
        case SET_DLIST_TRANS_X:             
        case SET_DLIST_TRANS_Y:             
        case SET_DLIST_TRANS_Z:             
        case SET_DLIST_SCALE:              
        {
            float property = Scripts_GetVarval(en, playState, instr->varType2, instr->value2, false);
            Scripts_MathOperation(destAddr, property, instr->operator, FLOAT); 
            break; 
        }
        case SET_DLIST_ROT_X:               
        case SET_DLIST_ROT_Y:               
        case SET_DLIST_ROT_Z:    
        case SET_DLIST_LIMB:
        case SET_DLIST_OBJECT:
        {
            s16 property = Scripts_GetVarval(en, playState, instr->varType2, instr->value2, false);
            Scripts_MathOperation(destAddr, property, instr->operator, INT16); 

            if (instr->subId == SET_DLIST_OBJECT)
                Rom_LoadObjectIfUnloaded(playState, en->extraDLists[dlistId].objectId);

            break; 
        }
    }
}

void Scripts_SetColor(NpcMaker* en, PlayState* playState, Color_RGB8* dest, void* instruction)
{
    ScrInstrColorSet* instr = (ScrInstrColorSet*)instruction;
    dest->r = Scripts_GetVarval(en, playState, instr->varTypeR, instr->R, false);
    dest->g = Scripts_GetVarval(en, playState, instr->varTypeG, instr->G, false);
    dest->b = Scripts_GetVarval(en, playState, instr->varTypeB, instr->B, false);
}

void Scripts_SetFlag(NpcMaker* en, PlayState* playState, void* instruction)
{
    ScrInstrDoubleSet* instr = (ScrInstrDoubleSet*)instruction;
    s32 flag = Scripts_GetVarval(en, playState, instr->varType1, instr->value1, true);
    bool set = false;

    if (instr->varType2 == NORMAL)
        set = (bool)instr->value2.ui32;
    else
        set = (bool)Scripts_GetVarval(en, playState, instr->varType2, instr->value2, false);

    switch (instr->subId)
    {
        case SET_FLAG_SWITCH: set ?         Flags_SetSwitch(playState, flag) : Flags_UnsetSwitch(playState, flag); break;
        case SET_FLAG_SCENE: set ?          Flags_SetUnknown(playState, flag) : Flags_UnsetUnknown(playState, flag); break;
        case SET_FLAG_ROOM_CLEAR: set ?     Flags_SetClear(playState, flag) : Flags_UnsetClear(playState, flag); break;
        case SET_FLAG_TEMPORARY: set ?      Flags_SetTempClear(playState, flag) : Flags_UnsetTempClear(playState, flag); break;
        case SET_FLAG_INF: 
        {
            if (set)
                Flags_SetInfTable(flag);
            else
                gSaveContext.infTable[flag >> 4] &= ~(1 << (flag & 0xF));
            
            break;
        }
        case SET_FLAG_EVENT:
        {
            if (set)
                Flags_SetEventChkInf(flag);
            else
                gSaveContext.eventChkInf[flag >> 4] &= ~(1 << (flag & 0xF)); 
                    
            break;
        }
        case SET_FLAG_TREASURE:
        {
            if (set)
                 Flags_SetTreasure(playState, flag);
            else
                playState->actorCtx.flags.chest &= ~(1 << flag); 
                
            break;
        }
        case SET_FLAG_SCENE_COLLECT:
        {
            if (set)
                Flags_SetCollectible(playState, flag);
            else
            {
                if (flag) 
                {
                    if (flag < 0x20)
                        playState->actorCtx.flags.collect &= ~(1 << flag);
                    else 
                        playState->actorCtx.flags.tempCollect &= ~(1 << (flag - 0x20));
                }                
            }

            break;
        }
        case SET_FLAG_INTERNAL: 
        {
            if (set)
                en->flags_internal |= 1UL << flag;
            else
                en->flags_internal &= ~(1UL << flag); 
            
            break;
        }
    }
}

#pragma endregion

#pragma region AWAIT

inline bool Scripts_AwaitBool(NpcMaker* en, PlayState* playState, bool checked, u32 condition)
{
    return Scripts_IfCommon(en, playState, checked, condition, 1, 0);
}

inline bool Scripts_AwaitValue(NpcMaker* en, PlayState* playState, float value, DataType dataType, u32 condition, u8 valType, ScriptVarval comparedValue)
{
    return Scripts_IfValueCommon(en, playState, value, dataType, condition, valType, comparedValue, 1, 0);
}

#pragma endregion

#pragma region IF

bool Scripts_Compare(float value1, float value2, u32 condition, u32 compareType)
{
    if (compareType == BOOL_COMPARE)
        return (value1 ? true : false) == (condition ? true : false);
    else
    {
        switch (condition)
        {
            case C_EQUALTO:                   return value1 == value2; break;
            case C_LESSTHAN:                  return value1 < value2; break;
            case C_MORETHAN:                  return value1 > value2; break;
            case C_LESSOREQ:                  return value1 <= value2; break;
            case C_MOREOREQ:                  return value1 >= value2; break;
            case C_NOTEQUAL:                  return value1 != value2; break;
            default:                          return false; break;
        }
    }
}

inline u16 Scripts_GetBranch(bool result, u16 trueInstruction, u16 falseInstruction)
{
    return result ? trueInstruction : falseInstruction;
}

u16 Scripts_IfFlag(NpcMaker* en, PlayState* playState, void* instruction)
{
    ScrInstrIf* instr = (ScrInstrIf*)instruction;
    bool ret;

    u32 flag = Scripts_GetVarval(en, playState, instr->vartype, instr->value, false);
    switch (instr->subId)
    {
        case IF_FLAG_SWITCH:               ret = Flags_GetSwitch(playState, flag); break;
        case IF_FLAG_SCENE:                ret = Flags_GetUnknown(playState, flag); break;
        case IF_FLAG_ROOM_CLEAR:           ret = Flags_GetClear(playState, flag); break;
        case IF_FLAG_TEMPORARY:            ret = Flags_GetTempClear(playState, flag); break;
        case IF_FLAG_INF:                  ret = Flags_GetInfTable(flag); break;
        case IF_FLAG_EVENT:                ret = Flags_GetEventChkInf(flag); break;
        case IF_FLAG_TREASURE:             ret = Flags_GetTreasure(playState, flag); break;
        case IF_FLAG_SCENE_COLLECT:        ret = Flags_GetCollectible(playState, flag); break;
        case IF_FLAG_INTERNAL:             ret = en->flags_internal & (1 << flag); break;
        default:                           ret = false; break;
    }

    return Scripts_GetBranch(Scripts_Compare(ret, 0, instr->condition, BOOL_COMPARE), instr->trueInstrNum, instr->falseInstrNum);
}

inline u16 Scripts_IfCommon(NpcMaker* en, PlayState* playState, bool checked, u32 condition, u16 gotoTrue, u16 gotoFalse)
{
    return Scripts_GetBranch(Scripts_Compare(checked, 0, condition, BOOL_COMPARE), gotoTrue, gotoFalse);
}

u16 Scripts_IfBool(NpcMaker* en, PlayState* playState, bool checked, void* instruction)
{
    ScrInstrIf* instr = (ScrInstrIf*)instruction;
    return Scripts_IfCommon(en, playState, checked, instr->condition, instr->trueInstrNum, instr->falseInstrNum);
}

u16 Scripts_IfBoolTwoValues(NpcMaker* en, PlayState* playState, bool checked, void* instruction)
{
    ScrInstrDoubleIf* instr = (ScrInstrDoubleIf*)instruction;
    return Scripts_IfCommon(en, playState, checked, instr->condition, instr->trueInstrNum, instr->falseInstrNum);
}

u16 Scripts_IfValueCommon(NpcMaker* en, PlayState* playState, float value, DataType dataType, u32 condition, 
                           u8 valType, ScriptVarval compared_value, u16 gotoTrue, u16 gotoFalse)
{
    bool ret = false;

    if (dataType == BOOL)
        ret = Scripts_Compare(value, Scripts_GetVarval(en, playState, valType, compared_value, !(dataType % 2) || dataType == FLOAT), condition, BOOL_COMPARE);
    else
        ret = Scripts_Compare(value, Scripts_GetVarval(en, playState, valType, compared_value, !(dataType % 2) || dataType == FLOAT), condition, VALUE_COMPARE);
    
    return Scripts_GetBranch(ret, gotoTrue, gotoFalse);
}

u16 Scripts_IfExtVar(NpcMaker* en, PlayState* playState, float value, void* instruction, DataType dataType)
{
    ScrInstrExtVarIf* instr = (ScrInstrExtVarIf*)instruction;
    return Scripts_IfValueCommon(en, playState, value, dataType, instr->condition, instr->varType, instr->value, instr->trueInstrNum, instr->falseInstrNum);
}

u16 Scripts_IfTwoValues(NpcMaker* en, PlayState* playState, float value, void* instruction, DataType dataType)
{
    ScrInstrDoubleIf* instr = (ScrInstrDoubleIf*)instruction;
    return Scripts_IfValueCommon(en, playState, value, dataType, instr->condition, instr->varType2, instr->value2, instr->trueInstrNum, instr->falseInstrNum);
}

u16 Scripts_IfValue(NpcMaker* en, PlayState* playState, float value, void* instruction, DataType dataType)
{
    ScrInstrIf* instr = (ScrInstrIf*)instruction;
    return Scripts_IfValueCommon(en, playState, value, dataType, instr->condition, instr->vartype, instr->value, instr->trueInstrNum, instr->falseInstrNum);
}

#pragma endregion

#pragma region TEXT

u32 Scripts_GetTextId(NpcMaker* en, PlayState* playState, u8 skipChildMsgId, u8 varTypeChild, ScriptVarval child, u8 varTypeAdult, ScriptVarval adult)
{
    if (playState->linkAgeOnLoad && !skipChildMsgId)
        return Scripts_GetVarval(en, playState, varTypeChild, child, true);
    else
        return Scripts_GetVarval(en, playState, varTypeAdult, adult, true);
}

void Scripts_ShowMessage(NpcMaker* en, PlayState* playState, u16 msgId, bool setActor)
{
    int talkState = Message_GetState(&playState->msgCtx);

    // If we've talked to the NPC, and the message status isn't blank, then we continue the message with this new one.
    // Textbox number is set to 0, because the first message isn't counted as new message?
    if (en->isTalking && talkState != TEXT_STATE_NONE)
    {
        Message_ContinueTextbox(playState, msgId);
        en->textboxNum = 0;
        en->persistTalk = false;
    }
    // Else, we show a new textbox.
    else
    {
        Message_StartTextbox(playState, msgId, setActor ? &en->actor : NULL);
        en->textboxNum = -1;
        en->persistTalk = false;
    }
}

void Scripts_SetMessage(NpcMaker* en, PlayState* playState, int msgId, u16* field, bool showSet, bool setActor)
{
    if (msgId > __INT16_MAX__)
    {
        if (en->customMsgId != R_CUSTOM_MSG_ID(msgId))
        {
            en->customMsgId = R_CUSTOM_MSG_ID(msgId);

            if (en->dummyMesEntry != NULL)
            {
                InternalMsgEntry data = Data_GetCustomMessage(en, playState, en->customMsgId);
                en->dummyMesEntry->settings = data.posType;
            }
        }

        msgId = DUMMY_MESSAGE;
    }
    else
        en->customMsgId = NO_CUSTOM_MESSAGE;      

    if (field != NULL)
        *field = msgId;

    if (showSet)
        Scripts_ShowMessage(en, playState, msgId, setActor);
}

#pragma endregion