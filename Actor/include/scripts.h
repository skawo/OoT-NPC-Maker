#ifndef SCRIPTS_H
#define SCRIPTS_H

#include "npc_maker_types.h"

void Scripts_PlayerAnimateMode(Player* pl, PlayState* playState);
void Scripts_Main(NpcMaker* en, PlayState* playState);
bool Scripts_Execute(NpcMaker* en, PlayState* playState, ScriptInstance* script);
ScrInstr* Scripts_GetInstrPtr(ScriptInstance* script, u32 instruction_num);
bool Scripts_InstructionSet(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrSet* instruction);
bool Scripts_InstructionGoto(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrGoto* instruction);
bool Scripts_InstructionIf(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrIf* in);
bool Scripts_InstructionAwait(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrAwait* in);
bool Scripts_InstructionShowTextbox(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrTextbox* in);
bool Scripts_InstructionEnableTalking(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrTextbox* in);
void Scripts_ResponseInstruction(NpcMaker* en, PlayState* playState, ScriptInstance* script);
bool Scripts_InstructionEnableTrade(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrTrade* in);
bool Scripts_InstructionFace(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrFace* in);
bool Scripts_InstructionScale(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrScale* in);
bool Scripts_InstructionPosition(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrPosition* in);
bool Scripts_InstructionRotation(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrRotation* in);
bool Scripts_InstructionPlay(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrPlay* in);
bool Scripts_InstructionKill(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrKill* in);
bool Scripts_InstructionOcarina(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrOcarina* in);
bool Scripts_InstructionSpawn(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrSpawn* in);
bool Scripts_InstructionItem(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrItem* in);
bool Scripts_InstructionWarp(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrWarp* in);
bool Scripts_InstructionParticle(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrParticle* in);
bool Scripts_InstructionScript(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrScript* in);
bool Scripts_InstructionCCall(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrCCall* in);
bool Scripts_InstructionGet(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrGetExtVar* in);
bool Scripts_InstructionGotoVar(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrGotoVar* instruction);
bool Scripts_InstructionStop(NpcMaker* en, PlayState* playState, ScriptInstance* script, ScrInstrStop* in);
float NpcMaker_RunCFunc(NpcMaker* en, PlayState* playState, u32 offset, float* Args);

#endif