#include "../include/npc_maker_types.h"
#include "../include/draw.h"
#include "../include/update.h"
#include "../include/init.h"
#include "../include/movement.h"
#include "../include/scripts.h"
#include "../include/h_scripts.h"
#include "../include/h_rom.h"

static void NpcMaker_Init(NpcMaker* en, GlobalContext* globalCtx);
static void NpcMaker_PostInit(NpcMaker* en, GlobalContext* globalCtx);
static void NpcMaker_Update(NpcMaker* en, GlobalContext* globalCtx);
static void NpcMaker_Update(NpcMaker* en, GlobalContext* globalCtx);
static void NpcMaker_Draw(NpcMaker* en, GlobalContext* globalCtx);


static void NpcMaker_Init(NpcMaker* en, GlobalContext* globalCtx)
{
    #if LOGGING == 1
        osSyncPrintf("___NPC MAKER DEBUG___");
    #endif

    Setup_Defaults(en, globalCtx);

    if (!Setup_LoadSetup(en, globalCtx))
    {
        Actor_Kill(&en->actor);
        return;
    }
}

// Setting up the object needs to happen in update for some unknown reason,
// because otherwise it fails if the object is already loaded in by the scene.
static void NpcMaker_PostInit(NpcMaker* en, GlobalContext* globalCtx)
{
    Setup_Objects(en, globalCtx);
    Setup_Misc(en, globalCtx);
    Setup_Model(en, globalCtx);

    en->actor.update = (ActorFunc)&NpcMaker_Update;
    NpcMaker_Update(en, globalCtx);
}

static void NpcMaker_Update(NpcMaker* en, GlobalContext* globalCtx)
{
    // Executing scripts...
    Scripts_Main(en, globalCtx);

    // Update current conversation status and copy messages into message context if need be...
    Update_Conversation(en, globalCtx);
	
    if (en->pauseCutscene)
    {
        globalCtx->csCtx.frames--;
        globalCtx->csCtx.unk_18 = 0xF000;
    }

    // Don't run some stuff if "just script" option is set.
    if (!en->settings.execJustScript)
    {
        // If we're in cutscene mode, we're always moving in the cutscene movement mode
        if (globalCtx->csCtx.state && en->settings.cutsceneId)
        {                
            Movement_Main(en, globalCtx, MOVEMENT_CUTSCENE, false, false);
        }
        else
        {
            Movement_Main(en, globalCtx, en->settings.movementType, en->settings.ignorePathYAxis, true);

            if (en->settings.lookAtType == LOOK_BODY)
                en->actor.shape.rot.y = en->actor.yawTowardsPlayer;
            else if (en->settings.lookAtType > LOOK_BODY)
                Update_HeadWaistRot(en, globalCtx);
        }

        // Animations, collision, etc. is set AFTER movement, since movement sets up the next animation.
        if (en->currentAnimId >= 0)
            Update_Animations(en, globalCtx);

        Update_TextureAnimations(en, globalCtx);
        Update_Collision(en, globalCtx);
        Update_ModelAlpha(en, globalCtx);
    }

    Update_Misc(en, globalCtx);
}

static void NpcMaker_Draw(NpcMaker* en, GlobalContext* globalCtx)
{
    Draw_Debug(en, globalCtx);

    if (en->settings.execJustScript)
        return;
    
    Draw_LightsRebind(en, globalCtx);
    Draw_SetGlobalEnvColor(en, globalCtx);
    Draw_SetupSegments(en, globalCtx);

    if (!en->settings.invisible && en->curAlpha != 0)
        Draw_Model(en, globalCtx);
        
    if (en->settings.castsShadow && !en->settings.hasCollision)
    {
        Vec3f shadow;
        shadow.z = shadow.y = shadow.x = en->settings.shadowRadius / 90.0f;
        //z_actor_shadow_draw_vec3f
        func_80033C30(&en->actor.world.pos, &shadow, 127, globalCtx);
    }
}

static void NpcMaker_Destroy(NpcMaker* en, GlobalContext* globalCtx)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: Destroying actor.", en->npcId);
    #endif

    Collider_DestroyCylinder(globalCtx, &en->collider);

    if (en->lightNode != NULL)
        LightContext_RemoveLight(globalCtx, &globalCtx->lightCtx, en->lightNode);

    void* frees[] = {
                        en->scripts,
                        en->scriptInstances,
                        en->animations,
                        en->extraDLists,
                        en->dListColors,
                        en->exSegData,
                        en->scriptFVars,
                        en->scriptVars
                    };

    for (int i = 0; i < ARRAY_COUNT(frees); i++)
    {
        if (frees[i] != NULL)
            ZeldaArena_Free(frees[i]);
    }
}

/* .data */
const ActorInitExplPad init_vars = 
{
    .id = 0xDEAD, .padding = 0xBEEF, // <-- magic values, do not change
    .category = ACTORCAT_NPC,
    .flags = 0x00000000,
    .objectId = 0x1,
    .instanceSize = sizeof(NpcMaker),
    .init = (ActorFunc)NpcMaker_Init,
    .destroy = (ActorFunc)NpcMaker_Destroy,
    .update = (ActorFunc)NpcMaker_PostInit,
    .draw = (ActorFunc)NpcMaker_Draw
};