#ifndef SCRIPTS_DATA_H

#define SCRIPTS_DATA_H

#include "npc_maker_types.h"

u16 basic_set_offsets[] =
{
    offsetof(NpcMaker, settings.targetLimb),            
    offsetof(NpcMaker, settings.targetDistance),
    offsetof(NpcMaker, settings.headLimb),
    offsetof(NpcMaker, settings.waistLimb),
    offsetof(NpcMaker, settings.lookAtType),
    offsetof(NpcMaker, settings.headVertAxis),
    offsetof(NpcMaker, settings.headHorAxis),
    offsetof(NpcMaker, settings.waistVertAxis),
    offsetof(NpcMaker, settings.waistHorAxis),
    offsetof(NpcMaker, settings.cutsceneId),
    offsetof(NpcMaker, settings.blinkTexSegment),
    offsetof(NpcMaker, settings.talkTexSegment),
    offsetof(NpcMaker, settings.alpha),

    offsetof(NpcMaker, settings.movementDistance),
    offsetof(NpcMaker, settings.maxRoamDist),
    offsetof(NpcMaker, settings.movementDelay),
    offsetof(NpcMaker, settings.sfxIfAttacked),
    offsetof(NpcMaker, settings.lightRadius),
    offsetof(NpcMaker, npcId),
    offsetof(PlayState, csCtx.frames),

    offsetof(NpcMaker, collider.dim.radius),
    offsetof(NpcMaker, collider.dim.height),
    offsetof(NpcMaker, settings.pathLoopStartNode),
    offsetof(NpcMaker, settings.pathLoopEndNode),
    offsetof(NpcMaker, collider.dim.yShift),
    offsetof(NpcMaker, settings.targetPosOffset.x),
    offsetof(NpcMaker, settings.targetPosOffset.y),
    offsetof(NpcMaker, settings.targetPosOffset.z),
    offsetof(NpcMaker, settings.modelPosOffset.x),
    offsetof(NpcMaker, settings.modelPosOffset.y),
    offsetof(NpcMaker, settings.modelPosOffset.z),
    offsetof(NpcMaker, cameraId),
    offsetof(NpcMaker, settings.riddenNPCId),
    offsetof(NpcMaker, settings.lookAtPosOffset.x),
    offsetof(NpcMaker, settings.lookAtPosOffset.y),
    offsetof(NpcMaker, settings.lookAtPosOffset.z),
    offsetof(NpcMaker, curPathNode),
    offsetof(NpcMaker, skin.skelAnime.curFrame),
    offsetof(NpcMaker, settings.lightPosOffset.x),
    offsetof(NpcMaker, settings.lightPosOffset.y),
    offsetof(NpcMaker, settings.lightPosOffset.z),    
    offsetof(NpcMaker, settings.timedPathStart),
    offsetof(NpcMaker, settings.timedPathEnd),

    offsetof(NpcMaker, settings.movementSpeed),
    offsetof(NpcMaker, settings.talkRadius),
    offsetof(NpcMaker, settings.smoothingConstant),
    offsetof(NpcMaker, settings.shadowRadius),
    offsetof(NpcMaker, actor.uncullZoneForward),
    offsetof(NpcMaker, actor.uncullZoneDownward),
    offsetof(NpcMaker, actor.uncullZoneScale),
    
    offsetof(NpcMaker, settings.loopPath),
    offsetof(NpcMaker, settings.hasCollision),
    offsetof(NpcMaker, doBlinkingAnm),
    offsetof(NpcMaker, doTalkingAnm),
    offsetof(NpcMaker, settings.execJustScript),
    offsetof(NpcMaker, settings.opensDoors),
    offsetof(NpcMaker, settings.ignorePathYAxis),
    offsetof(NpcMaker, settings.fadeOut),
    offsetof(NpcMaker, settings.lightGlow),
    offsetof(NpcMaker, pauseCutscene),
    offsetof(NpcMaker, settings.invisible),
    offsetof(NpcMaker, persistTalk),
    offsetof(NpcMaker, isTalking),
    offsetof(NpcMaker, settings.animInterpFrames),
};

u8 setAnimsOffsets[] = 
{
    offsetof(NpcAnimationEntry, objectId),
    offsetof(NpcAnimationEntry, offset),
    offsetof(NpcAnimationEntry, startFrame),
    offsetof(NpcAnimationEntry, endFrame),
    offsetof(NpcAnimationEntry, speed),
};

u8 setDlistOffsets[] = 
{
    offsetof(ExDListEntry, offset),
    offsetof(ExDListEntry, translation.x),
    offsetof(ExDListEntry, translation.y),
    offsetof(ExDListEntry, translation.z),
    offsetof(ExDListEntry, scale),
    offsetof(ExDListEntry, rotation.x),
    offsetof(ExDListEntry, rotation.y),
    offsetof(ExDListEntry, rotation.z),
    offsetof(ExDListEntry, limb),
    offsetof(ExDListEntry, objectId),
};

u32 toggle_offsets[][2] =
{
    {offsetof(NpcMaker, settings.pushesSwitches), PUSH_SWITCHES_MASK},
    {offsetof(NpcMaker, settings.isTargettable), TARGETTABLE_MASK},
    {offsetof(NpcMaker, settings.visibleWithLens), DRAWN_WITH_LENS_MASK},
    {offsetof(NpcMaker, settings.alwaysActive), ALWAYS_ACTIVE_MASK},
    {offsetof(NpcMaker, settings.alwaysDrawn), ALWAYS_DRAWN_MASK},
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
    {ITEM_RECOVERY_HEART, 0},
    {ITEM_MAGIC_SMALL, 0},
};

#endif