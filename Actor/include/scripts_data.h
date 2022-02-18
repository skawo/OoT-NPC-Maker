#ifndef SCRIPTS_DATA_H

#define SCRIPTS_DATA_H

#include "npc_maker_types.h"

u16 basic_set_offsets[] =
{
    OFFSETOF(NpcMaker, settings.targetLimb),            
    OFFSETOF(NpcMaker, settings.targetDistance),
    OFFSETOF(NpcMaker, settings.headLimb),
    OFFSETOF(NpcMaker, settings.waistLimb),
    OFFSETOF(NpcMaker, settings.lookAtType),
    OFFSETOF(NpcMaker, settings.headVertAxis),
    OFFSETOF(NpcMaker, settings.headHorAxis),
    OFFSETOF(NpcMaker, settings.waistVertAxis),
    OFFSETOF(NpcMaker, settings.waistHorAxis),
    OFFSETOF(NpcMaker, settings.cutsceneId),
    OFFSETOF(NpcMaker, settings.blinkTexSegment),
    OFFSETOF(NpcMaker, settings.talkTexSegment),
    OFFSETOF(NpcMaker, settings.alpha),

    OFFSETOF(NpcMaker, settings.movementDistance),
    OFFSETOF(NpcMaker, settings.maxRoamDist),
    OFFSETOF(NpcMaker, settings.movementDelay),
    OFFSETOF(NpcMaker, settings.sfxIfAttacked),
    OFFSETOF(NpcMaker, settings.lightRadius),
    OFFSETOF(GlobalContext, csCtx.frames),

    OFFSETOF(NpcMaker, collider.dim.radius),
    OFFSETOF(NpcMaker, collider.dim.height),
    OFFSETOF(NpcMaker, settings.pathLoopStartNode),
    OFFSETOF(NpcMaker, settings.pathLoopEndNode),
    OFFSETOF(NpcMaker, collider.dim.yShift),
    OFFSETOF(NpcMaker, settings.targetPosOffset.x),
    OFFSETOF(NpcMaker, settings.targetPosOffset.y),
    OFFSETOF(NpcMaker, settings.targetPosOffset.z),
    OFFSETOF(NpcMaker, settings.modelPosOffset.x),
    OFFSETOF(NpcMaker, settings.modelPosOffset.y),
    OFFSETOF(NpcMaker, settings.modelPosOffset.z),
    OFFSETOF(NpcMaker, cameraId),
    OFFSETOF(NpcMaker, settings.lookAtPosOffset.x),
    OFFSETOF(NpcMaker, settings.lookAtPosOffset.y),
    OFFSETOF(NpcMaker, settings.lookAtPosOffset.z),
    OFFSETOF(NpcMaker, curPathNode),
    OFFSETOF(NpcMaker, skin.skelAnime.curFrame),
    OFFSETOF(NpcMaker, settings.lightPosOffset.x),
    OFFSETOF(NpcMaker, settings.lightPosOffset.y),
    OFFSETOF(NpcMaker, settings.lightPosOffset.z),    
    OFFSETOF(NpcMaker, settings.timedPathStart),
    OFFSETOF(NpcMaker, settings.timedPathEnd),

    OFFSETOF(NpcMaker, settings.movementSpeed),
    OFFSETOF(NpcMaker, settings.talkRadius),
    OFFSETOF(NpcMaker, settings.smoothingConstant),
    OFFSETOF(NpcMaker, settings.shadowRadius),
    
    OFFSETOF(NpcMaker, settings.loopPath),
    OFFSETOF(NpcMaker, settings.hasCollision),
    OFFSETOF(NpcMaker, doBlinkingAnm),
    OFFSETOF(NpcMaker, doTalkingAnm),
    OFFSETOF(NpcMaker, settings.execJustScript),
    OFFSETOF(NpcMaker, settings.opensDoors),
    OFFSETOF(NpcMaker, settings.ignorePathYAxis),
    OFFSETOF(NpcMaker, settings.fadeOut),
    OFFSETOF(NpcMaker, settings.lightGlow),
    OFFSETOF(NpcMaker, pauseCutscene),
    OFFSETOF(NpcMaker, settings.invisible),
    OFFSETOF(NpcMaker, persistTalk),
};

u8 setAnimsOffsets[] = 
{
    OFFSETOF(NpcAnimationEntry, objectId),
    OFFSETOF(NpcAnimationEntry, offset),
    OFFSETOF(NpcAnimationEntry, startFrame),
    OFFSETOF(NpcAnimationEntry, endFrame),
    OFFSETOF(NpcAnimationEntry, speed),
};

u8 setDlistOffsets[] = 
{
    OFFSETOF(ExDListEntry, offset),
    OFFSETOF(ExDListEntry, translation.x),
    OFFSETOF(ExDListEntry, translation.y),
    OFFSETOF(ExDListEntry, translation.z),
    OFFSETOF(ExDListEntry, scale),
    OFFSETOF(ExDListEntry, rotation.z),
    OFFSETOF(ExDListEntry, rotation.z),
    OFFSETOF(ExDListEntry, rotation.z),
    OFFSETOF(ExDListEntry, limb),
    OFFSETOF(ExDListEntry, objectId),
};

u32 toggle_offsets[][2] =
{
    {OFFSETOF(NpcMaker, settings.pushesSwitches), PUSH_SWITCHES_MASK},
    {OFFSETOF(NpcMaker, settings.isTargettable), TARGETTABLE_MASK},
    {OFFSETOF(NpcMaker, settings.visibleWithLens), DRAWN_WITH_LENS_MASK},
    {OFFSETOF(NpcMaker, settings.alwaysActive), ALWAYS_ACTIVE_MASK},
    {OFFSETOF(NpcMaker, settings.alwaysDrawn), ALWAYS_DRAWN_MASK},
};

u8 inventory_set_slots[][2] =
{
    {ITEM_BOMB, SLOT_BOMB},
    {ITEM_BOMBCHU, SLOT_BOMBCHU},
    {ITEM_BOW, SLOT_BOW},
    {ITEM_NUT, SLOT_NUT},
    {ITEM_STICK, SLOT_STICK},
    {ITEM_BEAN, SLOT_BEAN},
    {ITEM_SEEDS, SLOT_SLINGSHOT},
    {ITEM_RUPEE_BLUE, 0},
    {ITEM_HEART, 0},
};

#endif