#include "../include/update.h"
#include "../include/init.h"
#include "../include/movement.h"
#include "../include/h_rom.h"
#include "../include/h_movement.h"
#include "../include/h_scene.h"

extern char DummyMessageData;

void Update_Misc(NpcMaker* en, GlobalContext* globalCtx)
{
    en->lastDayTime = gSaveContext.dayTime;

    if (en->stopPlayer)
        PLAYER->stateFlags1 |= PLAYER_STOPPED_MASK;
        
    if (en->cameraId - 1 > 0)
        Camera_ChangeDataIdx(&globalCtx->mainCamera, en->cameraId - 1);
}

void Update_TextureAnimations(NpcMaker *en, GlobalContext* global)
{
    if (en->exSegData == NULL)
        return;

    #pragma region Blinking

    // We check if anything is defined for the segment set as the one used for blinking, and if blinking is enabled.
    if (en->settings.blinkTexSegment >= 8 && en->exSegData[en->settings.blinkTexSegment - 8] != 0 && en->doBlinkingAnm)
    {
        // We wait until the timer between blinks is exhausted before starting to blink.
        if (en->blinkTimer)
            en->blinkTimer--;
        else
        {
            // User can set the interval between each texture, which is checked here...
            if (en->blinkingFramesBetween >= en->settings.blinkAnimSpeed)
            {
                //...and then reset. We get the texture set as current (which was iterated last frame to the next one in the cycle).
                en->blinkingFramesBetween = 0;
                s32 segTexture = en->settings.blinkPattern[en->currentBlinkFrame];

                // If the current texture is the last one, or undefined, then we set the current texture to 0.
                if (en->currentBlinkFrame == MAX_BLINK_FRAME || segTexture == BLINK_FRAME_BLANK)
                {
                    segTexture = 0;
                    en->currentBlinkFrame = 0;
                    en->blinkTimer = Rand_S16Offset(RANDOM_BLINK_MIN_FRAMES, RANDOM_BLINK_MAX_FRAMES);
                }
                
                // We iterate the current texture for next time.
                en->currentBlinkFrame++;
                // We set the current segment data for this segment to the selected number of the texture in pattern.
                if (en->settings.blinkPattern[segTexture] != BLINK_FRAME_BLANK)
                    en->segmentDataIds[en->settings.blinkTexSegment - 8] = en->settings.blinkPattern[segTexture];
            }
            else
                en->blinkingFramesBetween++;
        }
    }

    #pragma endregion

    #pragma region Talking

    // Pretty much the same as above, really, except we don't do this based on a timer,
    // but whenever the actor is talking.
    if (en->settings.talkTexSegment >= 8 && en->exSegData[en->settings.talkTexSegment - 8] != 0 && en->doTalkingAnm)
    {
        if (en->isTalking && func_8010BDBC(&global->msgCtx) == MSGSTATUS_DRAWING)
        {
            if (en->talkingFramesBetween >= en->settings.talkAnimSpeed)
            {
                en->talkingFramesBetween = 0;
                s32 segTexture = en->settings.talkPattern[en->currentTalkFrame];

                if (en->currentTalkFrame == MAX_TALK_FRAME || segTexture == TALK_FRAME_BLANK)
                {
                    segTexture = 0;
                    en->currentTalkFrame = 0;
                }
                    
                en->currentTalkFrame++;

                if (en->settings.talkPattern[segTexture] != TALK_FRAME_BLANK)
                    en->segmentDataIds[en->settings.talkTexSegment - 8] = en->settings.talkPattern[segTexture];
            }
            else
                en->talkingFramesBetween++;
        }
        else
            en->segmentDataIds[en->settings.talkTexSegment - 8] = 0;
    }

    #pragma endregion
}

void Update_Animations(NpcMaker* en, GlobalContext* globalCtx)
{
    if (en->animations == NULL || en->currentAnimId < 0)
    {
        en->animationFinished = true;
        return;
    }

    NpcAnimationEntry anim = en->animations[en->currentAnimId];
    s32 realObjId = R_OBJECT(en, anim.objectId);

    if (realObjId != en->settings.objectId)
    {
        if (!Rom_SetObjectToActor(&en->actor, globalCtx, realObjId))
        {
            #if LOGGING == 1
                osSyncPrintf("_%2d: Animation had object %04x set, but it wasn't loaded, so the animation will not play.", en->npcId, realObjId);
            #endif       

            en->animationFinished = true;       
            return;
        }
    }

    switch (en->settings.animationType)
    {
        case ANIMTYPE_LINK: en->animationFinished = LinkAnimation_Update(globalCtx, &en->skin.skelAnime); break;
        case ANIMTYPE_NORMAL: en->animationFinished = SkelAnime_Update(&en->skin.skelAnime); break;
        default: break;
    }

    // The way this is handled normally sucks, so I'm handling it separately.
    if (en->skin.skelAnime.curFrame >= anim.endFrame - anim.speed)
    {
        switch (en->skin.skelAnime.mode)
        {
            // For interpolation, we just reset the animation.
            case ANIMMODE_LOOP_PARTIAL_INTERP: Setup_Animation(en, globalCtx, en->currentAnimId, true, false, true, false); break;
            // If we don't need to interpolate, we set the current frame to the start frame.
            case ANIMMODE_LOOP_PARTIAL: en->skin.skelAnime.curFrame = anim.startFrame; break;
            default: break;
        }
    }

    
    if (realObjId != en->settings.objectId)
        Rom_SetObjectToActor(&en->actor, globalCtx, en->settings.objectId);
}

void Update_HeadWaistRot(NpcMaker *en, GlobalContext* globalCtx)
{
    s16 targetHor = -(en->actor.yawTowardsPlayer - en->actor.shape.rot.y);
    s16 targetVert = Math_Vec3f_Pitch(&en->settings.lookAtPosOffset, &PLAYER->actor.focus.pos);

    // Checking if player is in range.
    if (ABS(targetHor) > ROT16(en->settings.lookAtDegreesHor) ||
        ABS(targetVert) > ROT16(en->settings.lookAtDegreesVert) ||
        en->actor.xzDistToPlayer > en->settings.collisionRadius * LOOKAT_AREA_HORIZ_MULTIPLIER ||
        en->actor.yDistToPlayer > en->settings.collisionRadius * LOOKAT_AREA_VERT_MULTIPLIER)
    {
        targetHor = 0;
        targetVert = 0;
    }

    Math_SmoothStepToS(&en->limbRotA, targetHor, LOOKAT_ROTATION_SCALE, LOOKAT_ROTATION_MAX, LOOKAT_ROTATION_MIN);
    Math_SmoothStepToS(&en->limbRotB, targetVert, LOOKAT_ROTATION_SCALE, LOOKAT_ROTATION_MAX, LOOKAT_ROTATION_MIN);
}

void Update_Conversation(NpcMaker* en, GlobalContext* globalCtx)
{
    int talkState = func_8010BDBC(&globalCtx->msgCtx);

    // Checking if the player has talked to the NPC.
    if (func_8002F194(&en->actor, globalCtx))
    {
        #if LOGGING == 1
            osSyncPrintf("_%2d: Started talking!", en->npcId);
        #endif  

        en->talkingFinished = false;
        en->isTalking = true;
        en->textboxNum = -1;
    }

    // The hackiest workaround to make talk mode persist even if message is closed.
    if (en->persistTalk)
        globalCtx->msgCtx.unk_E3EE = SONGSTATUS_CORRECT;

    // Trading bug workaround.
    if (en->isTalking)
        PLAYER->actor.textId = en->actor.textId;

    // If message status is "new message", increase textbox count.
    if (en->isTalking && globalCtx->msgCtx.msgMode == MSGMODE_NEWMSG)
        en->textboxNum++;

    // Get dummy message data.
    if (AVAL(&globalCtx->msgCtx, u16, 0xE2F8) == DUMMY_MESSAGE && en->dummyMsgStart == 0xFFFFFFFF)
        en->dummyMsgStart = *(u32*)globalCtx->msgCtx.font.msgBuf;

    // Overwrite message if custom ID is set.
    if (en->isTalking && en->customMsgId >= 0)
    {
        // See if the game has copied the dummy message to RAM.
        // "011a" is the first 4 bytes of contents of message 0x011a
        if (en->dummyMsgStart == *(u32*)globalCtx->msgCtx.font.msgBuf)
        {
            #if LOGGING == 1
                osSyncPrintf("_%2d: Setting a custom message.", en->npcId);
            #endif  

            Message_Overwrite(en, globalCtx, en->customMsgId);
        }
    }

    // If we talked to the NPC, and msgstatus is 3, then textbox is visible.
    if (en->isTalking && globalCtx->msgCtx.msgMode == MSGMODE_UNK_03)
    {
        #if LOGGING == 1
            osSyncPrintf("_%2d: Textbox shown!", en->npcId);
        #endif  

        en->textboxDisplayed = true;
    }

    // If textbox was displayed, and now the message status is blank, then talking has finished.
    if (en->textboxDisplayed && (globalCtx->msgCtx.msgMode == MSGMODE_NONE || talkState == MSGSTATUS_EVENT))
    {
        #if LOGGING == 1
            osSyncPrintf("_%2d: _Talking has finished!", en->npcId);
        #endif  

        en->talkingFinished = true;
        en->isTalking = false;
        en->textboxDisplayed = false;

        // Setting textbox number to maximum, so that any AWAIT TEXTBOX_NUM commands pass.
        en->textboxNum = __INT8_MAX__;
    }
}

void Update_HitsReaction(NpcMaker* en, GlobalContext* globalCtx)
{
    en->wasHitThisFrame = en->collider.base.acFlags & AC_HIT;

    // If we're riding something, that thing sets up our hit reacting, so we don't do anything here.
    // If auto anims are off, then we can't do anything, anyway.
    if (en->riddenNpc != NULL || !en->autoAnims)
        return;

    // If hit collider is on...
    if (en->collider.base.acFlags & AC_ON)
    {   
        // ...and we have been hit...
        if (en->collider.base.acFlags & AC_HIT)
        {
            // Turn off the hit collider.
            en->collider.base.acFlags &= ~AC_ON;
            en->collider.base.acFlags &= ~AC_HIT;

            // Stop moving.
            Movement_StopMoving(en, globalCtx, true);

            // If sfx is set, play it.
            // z_actor_play_sfx
            if (en->settings.sfxIfAttacked >= 0)
                func_8002F7DC(&en->actor, en->settings.sfxIfAttacked);

            // Play the attacked animation and setup info that we've been hit.
            Setup_Animation(en, globalCtx, ANIM_ATTACKED, true, true, false, !en->autoAnims);
            en->wasHitTimer = en->skin.skelAnime.endFrame + WAS_HIT_DELAY_BEFORE_RETURNING_TO_NORMAL;
            en->wasHit = true;
        }
    }


    // If we've been hit...
    if (en->wasHit)
    {   
        // If the timer is set, we decrease it.
        if (en->wasHitTimer)
            en->wasHitTimer--;

        // ...and if the hit timer has passed, we return to normal...
        if (!en->wasHitTimer)
        {
            // Unset hit info...
            en->wasHitTimer = 0;
            en->wasHit = false;

            // Turn hits back on.
            if (en->settings.reactsToAttacks)
                en->collider.base.acFlags |= AC_ON;

            // We can go back to moving.
            Setup_Animation(en, globalCtx, ANIM_IDLE, true, false, false, false);
        }
    }
}

void Update_Collision(NpcMaker* en, GlobalContext* globalCtx)
{
    // Update the collider
    Collider_UpdateCylinder(&en->actor, &en->collider);

    // We need to react to hits before we update the bumpbox and hurtbox (as that resets indication there's been any collisions).
    Update_HitsReaction(en, globalCtx);

    // We update the bump collision (i.e against platforms and such)
    if (en->settings.hasCollision)
    {
        Actor_UpdateBgCheckInfo(globalCtx, &en->actor, 20.0f, en->collider.dim.radius, en->collider.dim.height, 5);
        CollisionCheck_SetOC(globalCtx, &globalCtx->colChkCtx, &en->collider.base);
    }

    if (en->collider.base.acFlags & AC_ON)
        CollisionCheck_SetAC(globalCtx, &globalCtx->colChkCtx, &en->collider.base);
}

void Update_ModelAlpha(NpcMaker* en, GlobalContext* globalCtx)
{
    if (en->settings.fadeOut)
    {
        if (en->actor.xzDistToPlayer > FADE_OUT_DISTANCE && en->curAlpha)
        {
            Math_SmoothStepToS((s16*)&en->curAlpha, 0, FADE_OUT_FADE_IN_SCALE, FADE_OUT_FADE_IN_SPEED_MAX, FADE_OUT_FADE_IN_SPEED_MIN);
            
            if (en->actor.shape.shadowAlpha)
                en->actor.shape.shadowAlpha -= SHADOW_ALPHA_UPDATE;
        }
        else if (en->actor.xzDistToPlayer <= FADE_OUT_DISTANCE && en->curAlpha < en->settings.alpha)
        {
            Math_SmoothStepToS((s16*)&en->curAlpha, en->settings.alpha, FADE_OUT_FADE_IN_SCALE, FADE_OUT_FADE_IN_SPEED_MAX, FADE_OUT_FADE_IN_SPEED_MIN);

            if (en->actor.shape.shadowAlpha != SHADOW_ALPHA)
                en->actor.shape.shadowAlpha += SHADOW_ALPHA_UPDATE;
        }
    }
    else
    {
        // In case this is turned off by scripts
        en->curAlpha = en->settings.alpha;
        en->actor.shape.shadowAlpha = SHADOW_ALPHA;
    }    
}