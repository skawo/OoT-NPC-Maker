#ifndef H_SCRIPTS_H
#define H_SCRIPTS_H

#include "npc_maker_types.h"

void Scripts_ToggleActorFlag(NpcMaker* en, u8* field, bool setting, u32 mask);
float Scripts_GetVarval(NpcMaker* en, GlobalContext* global, Vartype type, ScriptVarval value, bool signedVal);
Vec3f Scripts_GetVarvalVec3f(NpcMaker* en, GlobalContext* globalCtx, Vartype xyzType[], ScriptVarval values[], float divider);
Vec3s Scripts_GetVarvalVec3s(NpcMaker* en, GlobalContext* globalCtx, Vartype xyzType[], ScriptVarval values[]);
Color_RGBA8 Scripts_GetVarvalRGBA(NpcMaker* en, GlobalContext* globalCtx, Vartype colorTypes[], ScriptVarval values[]);
void Scripts_Set(NpcMaker* en, GlobalContext* global, void* dest, void* instruction, DataType type);
void Scripts_MathOperation(void* dest, float value, Operator op, DataType type);
s16 Rand_GetBetween(s16 min, s16 max);
bool Scripts_GetBool(NpcMaker* en, GlobalContext* global, void* instruction);
void Scripts_SetInventory(NpcMaker* en, GlobalContext* global, u8 slot_settings[], void* instruction);
void Scripts_SetFlag(NpcMaker* en, GlobalContext* global, void* instruction);
void Scripts_SetColor(NpcMaker* en, GlobalContext* global, Color_RGB8* dest, void* instruction);
void Scripts_SetAnimation(NpcMaker* en, GlobalContext* global, void* instruction);
u16 Scripts_IfFlag(NpcMaker* en, GlobalContext* global, void* instruction);
u16 Scripts_GetBranch(bool result, u16 true_instruction, u16 false_instruction);
bool Scripts_Compare(float value1, float value2, u32 condition, u32 compare_type);
u16 Scripts_IfBool(NpcMaker* en, GlobalContext* global, bool checked, void* instruction);
u16 Scripts_IfBoolTwoValues(NpcMaker* en, GlobalContext* global, bool checked, void* instruction);
u16 Scripts_IfValue(NpcMaker* en, GlobalContext* global, float value, void* instruction, DataType type);
u16 Scripts_IfTwoValues(NpcMaker* en, GlobalContext* global, float value, void* instruction, DataType type);
u16 Scripts_IfExtVar(NpcMaker* en, GlobalContext* global, float value, void* instruction, DataType type);
void* Scripts_RamSubIdSetup(NpcMaker* en, GlobalContext* global, u32 value, u32 sub_id, u32* out_valtype);
u16 Scripts_IfCommon(NpcMaker* en, GlobalContext* global, bool checked, u32 condition, u16 goto_true, u16 goto_false);
bool Scripts_AwaitBool(NpcMaker* en, GlobalContext* global, bool checked, u32 condition);
bool Scripts_AwaitValue(NpcMaker* en, GlobalContext* global, float value, DataType read_type, u32 condition, u8 val_type, ScriptVarval compared_value);
u32 Scripts_GetTextId(NpcMaker* en, GlobalContext* global, u8 vartype_child, ScriptVarval child, u8 vartype_adult, ScriptVarval adult);
bool Scripts_SetupTemp(ScriptInstance* script, void* instruction);
void Scripts_FreeTemp(ScriptInstance* script);
bool Scripts_FreeAndContinue(ScriptInstance* script);
void Scripts_SetMessage(NpcMaker* en, GlobalContext* global, int msg_id, u16* non_custom_field, bool show_set, bool setActor);
bool Scripts_InstructionNop(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstr* in);
bool Scripts_InstructionForceTalk(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstr* in);
bool Scripts_InstructionCloseTextbox(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstr* in);
bool Scripts_InstructionSave(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstr* in);
bool Scripts_InstructionFadeIn(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrFade* in);
bool Scripts_InstructionFadeOut(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrFade* in);

u16 Scripts_IfValueCommon(NpcMaker* en, GlobalContext* global, float value, DataType read_type, u32 condition, u8 val_type, 
                         ScriptVarval compared_value, u16 goto_true, u16 goto_false);

void* Scripts_GetActorByType(NpcMaker* en, GlobalContext* globalCtx, u32 targetActor, u8 actorNumType, ScriptVarval actorNumValue);
void Scripts_SetDList(NpcMaker* en, GlobalContext* globalCtx, void* instruction);


#endif