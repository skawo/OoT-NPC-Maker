#include "../include/npc_maker_types.h"
#include "../include/draw.h"
#include "../include/update.h"
#include "../include/init.h"
#include "../include/movement.h"
#include "../include/scripts.h"
#include "../include/h_scripts.h"
#include "../include/h_rom.h"

static void NpcMaker_Init(NpcMaker* en, PlayState* playState);
static void NpcMaker_PostInit(NpcMaker* en, PlayState* playState);
static void NpcMaker_Update(NpcMaker* en, PlayState* playState);
static void NpcMaker_Update(NpcMaker* en, PlayState* playState);
static void NpcMaker_Draw(NpcMaker* en, PlayState* playState);
static void NpcMaker_Destroy(NpcMaker* en, PlayState* playState);

float NpcMaker_RunCFunc(NpcMaker* en, PlayState* playState, u32 offset)
{
    if (offset == 0xFFFFFFFF)
        return 0;

    #if LOGGING == 1
        osSyncPrintf("_Running embedded function %8x", en->embeddedOverlay + offset);
    #endif

    typedef float EmbeddedFunction(NpcMaker* en, PlayState* playState);
    EmbeddedFunction* f = (EmbeddedFunction*)en->embeddedOverlay + offset;
    float out = f(en, playState);

    #if LOGGING == 1
        osSyncPrintf("_Embedded function finished.");
    #endif

    return out;
}

static void NpcMaker_Init(NpcMaker* en, PlayState* playState)
{
    #if LOGGING == 1
        osSyncPrintf("___NPC MAKER DEBUG___");
    #endif
}

// Setting up the object needs to happen in update for some unknown reason,
// because otherwise it fails if the object is already loaded in by the scene.
static void NpcMaker_PostInit(NpcMaker* en, PlayState* playState)
{
    Setup_Defaults(en, playState);
		
    if (!Setup_LoadSetup(en, playState))
    {
        Actor_Kill(&en->actor);
        return;
    }

    NpcMaker_RunCFunc(en, playState, en->CFuncs[0]);

    Setup_Objects(en, playState);
    Setup_Misc(en, playState);
    Setup_Model(en, playState);

    #if LOGGING == 1
        osSyncPrintf("_%2d: Initialization complete.", en->npcId);
    #endif    

    en->actor.update = (ActorFunc)&NpcMaker_Update;
	en->actor.draw = (ActorFunc)&NpcMaker_Draw;
	en->actor.destroy = (ActorFunc)&NpcMaker_Destroy;
}

static void NpcMaker_Update(NpcMaker* en, PlayState* playState)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: ======== Actor update, frame %2d ======== ", en->npcId, playState->gameplayFrames);
    #endif

    if (en->CFuncsWhen[1] == REPLACE_UPDATE)
    {
        NpcMaker_RunCFunc(en, playState, en->CFuncs[1]);
        return;
    }

    // Set the object location again to account for the fileStart
    if (en->settings.fileStart)
        Rom_SetObjectToActor(&en->actor, playState, en->settings.objectId, en->settings.fileStart);

    if (en->CFuncsWhen[1] == BEFORE_SCRIPTS)
        NpcMaker_RunCFunc(en, playState, en->CFuncs[1]);
    
    if (en->CFuncsWhen[1] == INSTEAD_OF_SCRIPTS)
        NpcMaker_RunCFunc(en, playState, en->CFuncs[1]);
    else
        Scripts_Main(en, playState);

    if (en->CFuncsWhen[1] == AFTER_SCRIPTS)
        NpcMaker_RunCFunc(en, playState, en->CFuncs[1]);

    // Update current conversation status and copy messages into message context if need be...
    Update_Conversation(en, playState);
	
    if (en->pauseCutscene)
    {
        playState->csCtx.frames--;
        playState->csCtx.unk_18 = 0xF000;
    }

    // Don't run some stuff if "just script" option is set.
    if (!en->settings.execJustScript)
    {
        // If we're in cutscene mode, we're always moving in the cutscene movement mode
        if (playState->csCtx.state && en->settings.cutsceneId)        
            Movement_Main(en, playState, MOVEMENT_CUTSCENE, false, false);
        else
        {
            Movement_Main(en, playState, en->settings.movementType, en->settings.ignorePathYAxis, true);

            if (en->settings.lookAtType == LOOK_BODY)
                en->actor.shape.rot.y = en->actor.yawTowardsPlayer;
            else if (en->settings.lookAtType > LOOK_BODY)
                Update_HeadWaistRot(en, playState);
        }

        // Animations, collision, etc. are updated AFTER movement, since movement sets up the next animation and this new movement position is what we want to check.
        if (en->currentAnimId >= 0)
            Update_Animations(en, playState);

        Update_TextureAnimations(en, playState);
        Update_Collision(en, playState);
        Update_ModelAlpha(en, playState);
    }

    Update_Misc(en, playState);

    #if LOGGING == 1
        osSyncPrintf("_%2d: Actor update complete.", en->npcId);
    #endif
}

static void NpcMaker_Draw(NpcMaker* en, PlayState* playState)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: Drawing actor.", en->npcId);
    #endif

    if (en->CFuncsWhen[2] == REPLACE_DRAW)
        NpcMaker_RunCFunc(en, playState, en->CFuncs[2]);
    else
    {
        if (en->CFuncsWhen[2] == BEFORE_MODEL)
            NpcMaker_RunCFunc(en, playState, en->CFuncs[2]);

        Draw_Debug(en, playState);

        if (en->settings.execJustScript)
            return;
        
        Draw_LightsRebind(en, playState);
        Draw_SetGlobalEnvColor(en, playState);
        Draw_SetupSegments(en, playState);

        if (!en->settings.invisible && en->curAlpha != 0)
            Draw_Model(en, playState);
            
        if (en->settings.castsShadow && !en->settings.hasCollision)
        {
            Vec3f shadow;
            shadow.z = shadow.y = shadow.x = en->settings.shadowRadius / 90.0f;
            //z_actor_shadow_draw_vec3f
            func_80033C30(&en->actor.world.pos, &shadow, 127, playState);
        }

        if (en->CFuncsWhen[2] == AFTER_MODEL)
            NpcMaker_RunCFunc(en, playState, en->CFuncs[2]);
    }

    #if LOGGING == 1
        osSyncPrintf("_%2d: Drawing actor complete.", en->npcId);
    #endif
}

static void NpcMaker_Destroy(NpcMaker* en, PlayState* playState)
{
    #if LOGGING == 1
        osSyncPrintf("_%2d: Destroying actor.", en->npcId);
    #endif

    Collider_DestroyCylinder(playState, &en->collider);

    if (en->lightNode != NULL)
        LightContext_RemoveLight(playState, &playState->lightCtx, en->lightNode);

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

    NpcMaker_RunCFunc(en, playState, en->CFuncs[4]);
	
    if (en->embeddedOverlay != 0)
        ZeldaArena_Free(en->embeddedOverlay);
	
    SkelAnime_Free(&en->skin.skelAnime, playState);	

    #if LOGGING == 1
        osSyncPrintf("_%2d: Destroying actor complete.", en->npcId);
    #endif
}

static void NpcMaker_None(NpcMaker* en, PlayState* playState)
{
}

/* .data */

#ifdef NPCM_Z64ROM
ActorInit sNpcMakerInit = 	
{
    .id = 0x0003, // <-- Set this to whichever actor ID you're using.
    .category = ACTORCAT_NPC,
    .flags = 0x00000000,
    .objectId = 0x1,
    .instanceSize = sizeof(NpcMaker),
    .init = (ActorFunc)NpcMaker_Init,
    .destroy = (ActorFunc)NpcMaker_None,
    .update = (ActorFunc)NpcMaker_PostInit,
    .draw = (ActorFunc)NpcMaker_None
};
#else
ActorInitExplPad __attribute__((section(".data"))) sActorVars = 
{
    .id = 0xDEAD, .padding = 0xBEEF, // <-- magic values, do not change
    .category = ACTORCAT_NPC,
    .flags = 0x00000000,
    .objectId = 0x1,
    .instanceSize = sizeof(NpcMaker),
    .init = (ActorFunc)NpcMaker_Init,
    .destroy = (ActorFunc)NpcMaker_None,
    .update = (ActorFunc)NpcMaker_PostInit,
    .draw = (ActorFunc)NpcMaker_None
};
#endif



