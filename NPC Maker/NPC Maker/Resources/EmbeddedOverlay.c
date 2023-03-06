#include <z64hdr/oot_mq_debug/z64hdr.h>

/*
To make NPC Maker able to use your function, it must have the following form:

1. Its name must have a NpcM_ prefix (eg. NpcM_Function),
2. Must have two parameters: NpcMaker* and PlayState*
3. A unique name

Examples of valid function signatures are thus:

s32 NpcM_Function(NpcMaker* npc, PlayState* play)
float NpcM_Function2(NpcMaker* npc, PlayState* play)
u8 NpcM_DoAThing(NpcMaker* npc, PlayState* play)

Warning; When using these functions from scripts, make sure
the return type is valid.
*/