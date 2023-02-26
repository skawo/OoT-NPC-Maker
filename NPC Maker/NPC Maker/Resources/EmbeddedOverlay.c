#include <z64hdr/oot_mq_debug/z64hdr.h>

/*
To make NPC Maker able to use your function, it must have the following form:

1. Its name must have a NpcM_type_ prefix (eg. NpcM_s32_Function),
2. Prefix type should match return type,
3. Return type should be one of the following: u8, s8, u16, s16, u32, s32, float
4. Must have two parameters: NpcMaker* and PlayState*
5. A unique name

Examples of valid function signatures are thus:

s32 NpcM_s32_Function(NpcMaker* npc, PlayState* play)
float NpcM_float_Function2(NpcMaker* npc, PlayState* play)
u8 NpcM_u8_DoAThing(NpcMaker* npc, PlayState* play)
*/