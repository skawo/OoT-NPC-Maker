#ifdef NPCM_Z64ROM
#include "../include/npc_maker_types.h"

NpcMakerVirtualTable* NpcM_Vtable = NULL;

void NpcMaker_LoadVtable()
{
    u32* word = (u32*)0x80700000;
    u32* end = (u32*)0x80800000;
    bool vtableFound = false;

    for (; word < end; word += (NPCM_VTABLE_ALIGNMENT / sizeof(u32)))
    {
        if (word[0] != NPCM_VTABLE_M0) continue;
        if (word[1] != NPCM_VTABLE_M1) continue;
        if (word[2] != NPCM_VTABLE_M2) continue;
        if (word[3] != NPCM_VTABLE_M3) continue;
        if (word[4] != NPCM_VTABLE_M4) continue;
        if (word[5] != NPCM_VTABLE_M5) continue;
        if (word[6] != NPCM_VTABLE_M6) continue;
        if (word[7] != NPCM_VTABLE_M7) continue;
        vtableFound = true;
        break;
    }

    assert(vtableFound == true);

    #if LOGGING > 0
        is64Printf("_vtable at %08X\n", word);
    #endif

    NpcM_Vtable = (NpcMakerVirtualTable*)(word);
}
#endif