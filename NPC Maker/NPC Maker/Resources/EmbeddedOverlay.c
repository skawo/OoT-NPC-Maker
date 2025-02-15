#include "npc_maker_header.h"
#include <npcmaker/npc_maker_types.h>

/*
To make NPC Maker able to use your function, it must have the following form:

1. Its name must have a NpcM_ prefix (eg. NpcM_Function),
2. Must have at least two parameters: NpcMaker* and PlayState*. Up to 8 additional float arguments can be passed on from scripts.
3. If you plan to pass the result to scripts, the return type must be float
5. A unique name

Examples of valid function signatures are thus, for example:

void NpcM_Function(NpcMaker* npc, PlayState* play, float argument, float argument1)
float NpcM_Function2(NpcMaker* npc, PlayState* play)
float NpcM_DoAThing(NpcMaker* npc, PlayState* play)

The exception for this is functions which should run "On limb", their signature is as follows:

float NpcM_Function(NpcMaker* npc, PlayState* playState, s32 limbNumber, Gfx** dListPtr, Vec3f* translation, Vec3s* rotation, void* instance, Gfx** gfxP)

This function should return 0 if the limb is meant to be drawn, and 1 if it is not.

Warning; When using these functions from scripts, make sure
the return type is valid.
*/