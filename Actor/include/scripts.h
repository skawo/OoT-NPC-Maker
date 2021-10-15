#ifndef SCRIPTS_H
#define SCRIPTS_H

#include "npc_maker_types.h"

void Scripts_PlayerAnimateMode(Player* pl, GlobalContext* globalCtx);
void Scripts_Main(NpcMaker* en, GlobalContext* global);
bool Scripts_Execute(NpcMaker* en, GlobalContext* global, ScriptInstance* script);
ScrInstr* Scripts_GetInstrPtr(ScriptInstance* script, u32 instruction_num);
bool Scripts_InstructionSet(NpcMaker* en, GlobalContext* global, ScriptInstance* script, ScrInstrSet* instruction);
bool Scripts_InstructionGoto(NpcMaker* en, GlobalContext* global, ScriptInstance* script, ScrInstrGoto* instruction);
bool Scripts_InstructionIf(NpcMaker* en, GlobalContext* global, ScriptInstance* script, ScrInstrIf* in);
bool Scripts_InstructionAwait(NpcMaker* en, GlobalContext* global, ScriptInstance* script, ScrInstrAwait* in);
bool Scripts_InstructionShowTextbox(NpcMaker* en, GlobalContext* global, ScriptInstance* script, ScrInstrTextbox* in);
bool Scripts_InstructionEnableTalking(NpcMaker* en, GlobalContext* global, ScriptInstance* script, ScrInstrTextbox* in);
void Scripts_ResponseInstruction(NpcMaker* en, GlobalContext* global, ScriptInstance* script);
bool Scripts_InstructionEnableTrade(NpcMaker* en, GlobalContext* global, ScriptInstance* script, ScrInstrTrade* in);
bool Scripts_InstructionFace(NpcMaker* en, GlobalContext* global, ScriptInstance* script, ScrInstrFace* in);
bool Scripts_InstructionScale(NpcMaker* en, GlobalContext* global, ScriptInstance* script, ScrInstrScale* in);
bool Scripts_InstructionPosition(NpcMaker* en, GlobalContext* global, ScriptInstance* script, ScrInstrPosition* in);
bool Scripts_InstructionRotation(NpcMaker* en, GlobalContext* global, ScriptInstance* script, ScrInstrRotation* in);
bool Scripts_InstructionPlay(NpcMaker* en, GlobalContext* global, ScriptInstance* script, ScrInstrPlay* in);
bool Scripts_InstructionKill(NpcMaker* en, GlobalContext* global, ScriptInstance* script, ScrInstrKill* in);
bool Scripts_InstructionOcarina(NpcMaker* en, GlobalContext* global, ScriptInstance* script, ScrInstrOcarina* in);
bool Scripts_InstructionSpawn(NpcMaker* en, GlobalContext* global, ScriptInstance* script, ScrInstrSpawn* in);
bool Scripts_InstructionItem(NpcMaker* en, GlobalContext* global, ScriptInstance* script, ScrInstrItem* in);
bool Scripts_InstructionWarp(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrWarp* in);
bool Scripts_InstructionParticle(NpcMaker* en, GlobalContext* globalCtx, ScriptInstance* script, ScrInstrParticle* in);

#endif