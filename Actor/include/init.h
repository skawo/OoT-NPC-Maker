#ifndef INIT_H
#define INIT_H

#include "npc_maker_types.h"

void Setup_Defaults(NpcMaker* en, GlobalContext* global);
bool Setup_LoadSetup(NpcMaker* en, GlobalContext* global);
void Setup_Objects(NpcMaker* en, GlobalContext* global);
void Setup_Misc(NpcMaker* en, GlobalContext* global);
void Setup_Path(NpcMaker* en, GlobalContext* global, int path_id);
void Setup_Model(NpcMaker* en, GlobalContext* global);
void Setup_ScriptVars(NpcMaker* en, void** ptr, u32 count);
void Setup_Animation(NpcMaker* en, GlobalContext*global, int anim_id, bool interpolate, bool play_once, bool force_set, bool do_nothing);
bool Setup_AnimationImpl(Actor* actor, GlobalContext*global, SkelAnime* skelanime, int anim_addr, int anim_type, int object, int fileStart, int actor_object, int actor_object_filestart,
                          int anim_start, int anim_end, float speed, bool interpolate, bool play_once);
u32 Setup_LoadSection(NpcMaker* en, GlobalContext* globalCtx, u8* buffer, u32 offset, u32 entryAddress, u32* alloc_dest, u16* entries_number_out, u32 entry_size, 
                      u32 null_block_size, bool noCopy, s32 block_size);

#endif