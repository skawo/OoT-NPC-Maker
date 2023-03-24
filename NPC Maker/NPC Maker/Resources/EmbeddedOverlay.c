#include <z64hdr/oot_mq_debug/z64hdr.h>
#include <npcmaker/npc_maker_types.h>

/*
To make NPC Maker able to use your function, it must have the following form:

1. Its name must have a NpcM_ prefix (eg. NpcM_Function),
2. Must have two parameters: NpcMaker* and PlayState*
3. If you plan to pass the result to scripts, the return type must be float
4. A unique name

Examples of valid function signatures are thus:

void NpcM_Function(NpcMaker* npc, PlayState* play)
float NpcM_Function2(NpcMaker* npc, PlayState* play)
float NpcM_DoAThing(NpcMaker* npc, PlayState* play)

Warning; When using these functions from scripts, make sure
the return type is valid.
*/