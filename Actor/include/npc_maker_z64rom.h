#ifndef NPC_MAKER_VTABLE_H
#define NPC_MAKER_VTABLE_H

struct NpcMaker;

#ifdef NPCM_Z64ROM
    // included internally by npcmaker
    #include <zocarina.h>
#else
    // compiled by z64rom
    #include <uLib.h>
    #define ObjectEntry ObjectStatus
#endif

#define NPCM_VTABLE_ALIGNMENT 256
#define NPCM_VTABLE_WORD(a, b, c, d) ((a << 24) | (b << 16) | (c << 8) | (d))

#define NPCM_VTABLE_M0 NPCM_VTABLE_WORD('N', 'P', 'C', ' ')
#define NPCM_VTABLE_M1 NPCM_VTABLE_WORD('M', 'a', 'k', 'e')
#define NPCM_VTABLE_M2 NPCM_VTABLE_WORD('r', ' ', 'V', 'i')
#define NPCM_VTABLE_M3 NPCM_VTABLE_WORD('r', 't', 'u', 'a')
#define NPCM_VTABLE_M4 NPCM_VTABLE_WORD('l', ' ', 'T', 'a')
#define NPCM_VTABLE_M5 NPCM_VTABLE_WORD('b', 'l', 'e', ' ')
#define NPCM_VTABLE_M6 NPCM_VTABLE_WORD('z', '6', '4', 'r')
#define NPCM_VTABLE_M7 NPCM_VTABLE_WORD('o', 'm', 0xC0, 0xFE)

typedef struct NpcMakerVirtualTable
{
    u32 magic[8];
    DmaEntry* dmaTable;
    RomFile* objectTable;

    int (*getLanguage)(void);
    void* (*loadAnimation)(struct NpcMaker*, int animId, int objectId);
    int (*getAnimationSize)(struct NpcMaker*, int animId, int objectId);
    ObjectEntry* (*objectSlot)(PlayState* playState, int objectId);

    u32 reserved[32];
} NpcMakerVirtualTable;

#ifdef NPCM_Z64ROM
    void NpcMaker_LoadVtable(void);
    extern NpcMakerVirtualTable* NpcM_Vtable;
#else
    __attribute__((weak)) int NpcM_GetLanguage() {return 0;}
    __attribute__((weak)) void* NpcM_LoadAnimation(struct NpcMaker* en, int animId, int objectId) {return NULL;}
    __attribute__((weak)) int NpcM_GetAnimationSize(struct NpcMaker* en, int animId, int objectId) {return 0;}

    static ObjectEntry* NpcM_GetObjectSlot(PlayState* play, int objectId) {
        return &play->objectCtx.status[objectId];
    }

    __attribute__((aligned(NPCM_VTABLE_ALIGNMENT)))
    NpcMakerVirtualTable NPC_Maker_Virtual_Table_z64rom =
    {
        { NPCM_VTABLE_M0, NPCM_VTABLE_M1, NPCM_VTABLE_M2, NPCM_VTABLE_M3, NPCM_VTABLE_M4, NPCM_VTABLE_M5, NPCM_VTABLE_M6, NPCM_VTABLE_M7 },
        .dmaTable = gDmaDataTable,
        .objectTable = gObjectTable,

        .getLanguage = NpcM_GetLanguage,
        .loadAnimation = NpcM_LoadAnimation,
        .getAnimationSize = NpcM_GetAnimationSize,
        .objectSlot = NpcM_GetObjectSlot,
    };
#endif

#endif // NPC_MAKER_VTABLE_H
