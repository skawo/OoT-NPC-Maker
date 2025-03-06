#include "../include/update.h"
#include "../include/init.h"
#include "../include/movement.h"
#include "../include/h_rom.h"
#include "../include/h_movement.h"
#include "../include/h_scene.h"

void Update_Misc(NpcMaker* en, PlayState* playState)
{
    en->lastDayTime = gSaveContext.dayTime;

    if (en->stopPlayer)
    {
        #if LOGGING > 2
            is64Printf("_%2d: Stopping player!\n", en->npcId);
        #endif           

        GET_PLAYER(playState)->stateFlags1 |= PLAYER_STOPPED_MASK;
    }
        
    if (en->cameraId - 1 > 0)
    {
        #if LOGGING > 2
            is64Printf("_%2d: Setting camera ID to %2d\n", en->npcId, en->cameraId - 1);
        #endif   

        Camera_ChangeBgCamIndex(&playState->mainCamera, en->cameraId - 1);
    }

    #define SET_FIELD(field, margin, marginmin, mul) if (field + 1 < margin) field += (1 * mul); else field = marginmin;
    #define SET_FIELD_MINUS(field, margin, marginmin, mul) if (field - 1 >= marginmin) field -= (1 * mul); else field = margin - 1;

    #if LOOKAT_EDITOR == 1

    if (en->settings.showLookAtEditorDebugOn)
    {
        #if LOGGING > 3
            is64Printf("_%2d: LOOKAT editor is enabled.\n", en->npcId);
        #endif  

        if (en->dbgPosEditorCooldown)
        {
            en->dbgPosEditorCooldown--;
            return;
        }

        if (CHECK_BTN_ALL(playState->state.input->press.button, BTN_DDOWN))
        {
            SET_FIELD(en->dbgPosEditorCursorPos, 12, 0, 1); 
        }
        else if (CHECK_BTN_ALL(playState->state.input->press.button, BTN_DUP))
        {
            SET_FIELD_MINUS(en->dbgPosEditorCursorPos, 12, 0, 1); 
        }
        else if (CHECK_BTN_ALL(playState->state.input->cur.button, BTN_DRIGHT))
        {
            en->dbgPosEditorCooldown = 2;

            float mul = 1;

            if (CHECK_BTN_ALL(playState->state.input->cur.button, BTN_B))
                mul = 100;

            switch (en->dbgPosEditorCursorPos)
            {
                case 0: SET_FIELD(en->settings.lookAtType, 5, 0, 1); break;
                case 1: en->settings.headLimb += 1; break;
                case 2: SET_FIELD(en->settings.headVertAxis, 6, 0, 1); break;
                case 3: SET_FIELD(en->settings.headHorAxis, 6, 0, 1); break;
                case 4: en->settings.waistLimb += 1; break;
                case 5: SET_FIELD(en->settings.waistVertAxis, 6, 0, 1); break;
                case 6: SET_FIELD(en->settings.waistHorAxis, 6, 0, 1); break;
                case 7: SET_FIELD(en->settings.lookAtDegreesVert, 360, 0, mul == 100 ? 10 : 1); break;
                case 8: SET_FIELD(en->settings.lookAtDegreesHor, 360, 0, mul == 100 ? 10 : 1); break;
                case 9: SET_FIELD(en->settings.lookAtPosOffset.x, 32767, -332767, mul); break;
                case 10: SET_FIELD(en->settings.lookAtPosOffset.y, 32767, -332767, mul); break;
                case 11: SET_FIELD(en->settings.lookAtPosOffset.z, 32767, -332767, mul); break;
            }
        }
        else if (CHECK_BTN_ALL(playState->state.input->cur.button, BTN_DLEFT))
        {
            en->dbgPosEditorCooldown = 2;

            float mul = 1;

            if (CHECK_BTN_ALL(playState->state.input->cur.button, BTN_B))
                mul = 100;


            switch (en->dbgPosEditorCursorPos)
            {
                case 0: SET_FIELD_MINUS(en->settings.lookAtType, 5, 0, 1); break;
                case 1: en->settings.headLimb -= 1; break;
                case 2: SET_FIELD_MINUS(en->settings.headVertAxis, 6, 0, 1); break;
                case 3: SET_FIELD_MINUS(en->settings.headHorAxis, 6, 0, 1); break;
                case 4: en->settings.waistLimb -= 1; break;
                case 5: SET_FIELD_MINUS(en->settings.waistVertAxis, 6, 0, 1); break;
                case 6: SET_FIELD_MINUS(en->settings.waistHorAxis, 6, 0, 1); break;
                case 7: SET_FIELD_MINUS(en->settings.lookAtDegreesVert, 360, 0, mul == 100 ? 10 : 1); break;
                case 8: SET_FIELD_MINUS(en->settings.lookAtDegreesHor, 360, 0, mul == 100 ? 10 : 1); break;                
                case 9: SET_FIELD_MINUS(en->settings.lookAtPosOffset.x, 32767, -332767, mul); break;
                case 10: SET_FIELD_MINUS(en->settings.lookAtPosOffset.y, 32767, -332767, mul); break;
                case 11: SET_FIELD_MINUS(en->settings.lookAtPosOffset.z, 32767, -332767, mul); break;
            }
        }
    }

    #endif

    #if EXDLIST_EDITOR == 1

    if (en->settings.showDlistEditorDebugOn && en->numExDLists != 0)
    {
        #if LOGGING > 3
            is64Printf("_%2d: EXDLIST editor is enabled.\n", en->npcId);
        #endif   

        if (en->dbgPosEditorCooldown)
        {
            en->dbgPosEditorCooldown--;
            return;
        }

        if (CHECK_BTN_ALL(playState->state.input->press.button, BTN_DDOWN))
        {
            en->dbgPosEditorCursorPos++;

            if (en->dbgPosEditorCursorPos > 9)
                en->dbgPosEditorCursorPos = 0;
        }

        if (CHECK_BTN_ALL(playState->state.input->press.button, BTN_DUP))
        {
            if (en->dbgPosEditorCursorPos == 0)
                en->dbgPosEditorCursorPos = 9;
            else
                en->dbgPosEditorCursorPos--;
        }

        if (CHECK_BTN_ALL(playState->state.input->cur.button, BTN_DRIGHT))
        {
            en->dbgPosEditorCooldown = 2;

            float mul = 1;

            if (CHECK_BTN_ALL(playState->state.input->cur.button, BTN_B))
                mul *= 100;

            if (CHECK_BTN_ALL(playState->state.input->cur.button, BTN_Z))
                mul *= 0.5;

            switch (en->dbgPosEditorCursorPos)
            {
                case 0: 
                {
                    if (en->dbgPosEditorCurEditing + 1 < en->numExDLists)
                        en->dbgPosEditorCurEditing++; 
                    else
                        en->dbgPosEditorCurEditing = 0;
                        
                    break;
                }
                case 1: en->extraDLists[en->dbgPosEditorCurEditing].translation.x += (1 * mul); break;
                case 2: en->extraDLists[en->dbgPosEditorCurEditing].translation.y += (1 * mul); break;
                case 3: en->extraDLists[en->dbgPosEditorCurEditing].translation.z += (1 * mul); break;
                case 4: en->extraDLists[en->dbgPosEditorCurEditing].rotation.x += (10 * mul); break;
                case 5: en->extraDLists[en->dbgPosEditorCurEditing].rotation.y += (10 * mul); break;
                case 6: en->extraDLists[en->dbgPosEditorCurEditing].rotation.z += (10 * mul); break;
                case 7: en->extraDLists[en->dbgPosEditorCurEditing].scale += (0.01f * mul); break;
                case 8: en->extraDLists[en->dbgPosEditorCurEditing].showType += 1; break;
                case 9: en->extraDLists[en->dbgPosEditorCurEditing].limb += 1; break;
            }
        }
        if (CHECK_BTN_ALL(playState->state.input->cur.button, BTN_DLEFT))
        {
            en->dbgPosEditorCooldown = 2;

            float mul = 1;

            if (CHECK_BTN_ALL(playState->state.input->cur.button, BTN_B))
                mul *= 100;

            if (CHECK_BTN_ALL(playState->state.input->cur.button, BTN_Z))
                mul *= 0.5;

            switch (en->dbgPosEditorCursorPos)
            {
                case 0: 
                {
                    if (en->dbgPosEditorCurEditing - 1 >= 0)
                        en->dbgPosEditorCurEditing--;
                    else
                        en->dbgPosEditorCurEditing = en->numExDLists - 1;
                        
                    break;
                }
                case 1: en->extraDLists[en->dbgPosEditorCurEditing].translation.x -= (1 * mul); break;
                case 2: en->extraDLists[en->dbgPosEditorCurEditing].translation.y -= (1 * mul); break;
                case 3: en->extraDLists[en->dbgPosEditorCurEditing].translation.z -= (1 * mul); break;
                case 4: en->extraDLists[en->dbgPosEditorCurEditing].rotation.x -= (10 * mul); break;
                case 5: en->extraDLists[en->dbgPosEditorCurEditing].rotation.y -= (10 * mul); break;
                case 6: en->extraDLists[en->dbgPosEditorCurEditing].rotation.z -= (10 * mul); break;
                case 7: en->extraDLists[en->dbgPosEditorCurEditing].scale -= (0.01f * mul); break;
                case 8: en->extraDLists[en->dbgPosEditorCurEditing].showType -= 1; break;
                case 9: en->extraDLists[en->dbgPosEditorCurEditing].limb -= 1; break;                
            }
        }
    }


    #endif
}

void Update_TextureAnimations(NpcMaker *en, PlayState* playState)
{
    #if LOGGING > 2
        is64Printf("_%2d: Updating texture animations.\n", en->npcId);
    #endif    

    if (en->exSegData == NULL)
        return;

    #pragma region Blinking

    // We check if anything is defined for the segment set as the one used for blinking, and if blinking is enabled.
    if (en->settings.blinkTexSegment >= 8 && en->exSegData[en->settings.blinkTexSegment - 8] != 0 && en->doBlinkingAnm && en->settings.blinkPattern[0] != 0xFF)
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
                    en->currentBlinkFrame = 0;
                    segTexture = en->settings.blinkPattern[en->currentBlinkFrame];
                    en->blinkTimer = Rand_S16Offset(RANDOM_BLINK_MIN_FRAMES, RANDOM_BLINK_MAX_FRAMES);
                    en->blinkTimer *= (float)((float)3 / (float)R_UPDATE_RATE);
                }

                // We iterate the current texture for next time.
                en->currentBlinkFrame++;

                // We set the current segment data for this segment to the selected number of the texture in pattern.
                if (segTexture != 0xFF)
                    en->segmentDataIds[en->settings.blinkTexSegment - 8] = segTexture;
                else
                    en->segmentDataIds[en->settings.blinkTexSegment - 8] = 0;
            }
            else
                en->blinkingFramesBetween++;
        }
    }

    #pragma endregion

    #pragma region Talking

    // Pretty much the same as above, really, except we don't do this based on a timer,
    // but whenever the actor is talking.
    if (en->settings.talkTexSegment >= 8 && en->exSegData[en->settings.talkTexSegment - 8] != 0 && en->doTalkingAnm && en->settings.talkPattern[0] != 0xFF)
    {
        if (en->isTalking && playState->msgCtx.msgMode == MSGMODE_TEXT_DISPLAYING)
        {
            float rate = (float)((float)3 / (float)R_UPDATE_RATE);
            
            if (en->talkingFramesBetween >= (en->settings.talkAnimSpeed * rate))
            {
                en->talkingFramesBetween = 0;
                s32 segTexture = en->settings.talkPattern[en->currentTalkFrame];

                if (en->currentTalkFrame == MAX_TALK_FRAME || segTexture == TALK_FRAME_BLANK)
                {
                    segTexture = 0;
                    en->currentTalkFrame = 0;
                    segTexture = en->settings.talkPattern[en->currentTalkFrame];
                }

                en->currentTalkFrame++;
                en->segmentDataIds[en->settings.talkTexSegment - 8] = segTexture;
            }
            else
                en->talkingFramesBetween++;
        }
        else
        {
            if (en->settings.talkPattern[0] != 0xFF)
                en->segmentDataIds[en->settings.talkTexSegment - 8] = en->settings.talkPattern[0];
            else
                en->segmentDataIds[en->settings.talkTexSegment - 8] = 0;
        }
    }

    #pragma endregion

    #if LOGGING > 2
        is64Printf("_%2d: Updating texture animations complete.\n", en->npcId);
    #endif       
}

void Update_Animations(NpcMaker* en, PlayState* playState)
{
    #if LOGGING > 2
        is64Printf("_%2d: Updating animation.\n", en->npcId);
    #endif    

    if (en->animations == NULL || en->currentAnimId < 0)
    {
        en->animationFinished = true;
        return;
    }

    NpcAnimationEntry anim = en->animations[en->currentAnimId];
    s32 realObjId = R_OBJECT(en, anim.objectId);

    if (realObjId != en->settings.objectId || anim.fileStart != OBJECT_CURRENT)
    {
        if (!Rom_SetObjectToActor(&en->actor, playState, realObjId, (R_FILESTART(en, anim.fileStart))))
        {
            #if LOGGING > 0
                is64Printf("_%2d: Animation had object %04x set, but it wasn't loaded, so the animation will not play.\n", en->npcId, realObjId);
            #endif       

            en->animationFinished = true;       
            return;
        }
    }

    switch (en->settings.animationType)
    {
        case ANIMTYPE_LINK: en->animationFinished = LinkAnimation_Update(playState, &en->skin.skelAnime); break;
        case ANIMTYPE_NORMAL: en->animationFinished = SkelAnime_Update(&en->skin.skelAnime); break;
        default: break;
    }

    // The way this is handled normally sucks, so I'm handling it separately.
    if (en->skin.skelAnime.curFrame >= anim.endFrame - anim.speed)
    {
        switch (en->skin.skelAnime.mode)
        {
            // For interpolation, we just reset the animation.
            case ANIMMODE_LOOP_PARTIAL_INTERP: Setup_Animation(en, playState, en->currentAnimId, true, false, true, false, false); break;
            // If we don't need to interpolate, we set the current frame to the start frame.
            case ANIMMODE_LOOP_PARTIAL: en->skin.skelAnime.curFrame = anim.startFrame; break;
            default: break;
        }
    }

    
    if (realObjId != en->settings.objectId || anim.fileStart != OBJECT_CURRENT)
        Rom_SetObjectToActor(&en->actor, playState, en->settings.objectId, en->settings.fileStart);

    #if LOGGING > 2
        is64Printf("_%2d: Updating animation complete.\n", en->npcId);
    #endif            
}

void Update_HeadWaistRot(NpcMaker *en, PlayState* playState)
{
    s16 targetHor = -(en->actor.yawTowardsPlayer - en->actor.shape.rot.y);
    s16 targetVert = Math_Vec3f_Pitch(&en->settings.lookAtPosOffset, &GET_PLAYER(playState)->actor.focus.pos);

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

void Update_Conversation(NpcMaker* en, PlayState* playState)
{
    #if LOGGING > 2
        is64Printf("_%2d: Updating conversation status.\n", en->npcId);
    #endif    

    int talkState = Message_GetState(&playState->msgCtx);

    // Checking if the player has talked to the NPC.
    if (Actor_ProcessTalkRequest(&en->actor, playState))
    {
        #if LOGGING > 0
            is64Printf("_%2d: Started talking!\n", en->npcId);
        #endif  

        en->talkingFinished = false;
        en->isTalking = true;
        en->textboxNum = -1;
    }

    // The hackiest workaround to make talk mode persist even if message is closed.
    if (en->persistTalk)
        playState->msgCtx.ocarinaMode = SONGSTATUS_CORRECT;

    // Trading bug workaround.
    if (en->isTalking)
        GET_PLAYER(playState)->actor.textId = en->actor.textId;

    // If message status is "new message", increase textbox count. Also,
    // save current textbox buffer position.
    if (en->isTalking && playState->msgCtx.msgMode == MSGMODE_TEXT_NEXT_MSG)
    {
        en->curTextBuffPos = playState->msgCtx.msgBufPos;
        en->textboxNum++;
    }

    // Overwrite message if custom ID is set.
    if (en->isTalking && en->customMsgId >= 0)
    {
        // See if the game has copied the dummy message to RAM.
        // "011a" is the first 4 bytes of contents of message 0x011a
        if (DUMMY_MSG_DATA == *(u32*)playState->msgCtx.font.msgBuf)
        {
            #if LOGGING > 1
                is64Printf("_%2d: Setting a custom message.\n", en->npcId);
            #endif  

            Message_Overwrite(en, playState, en->customMsgId);
        }
    }

    // If we talked to the NPC, and msgstatus is 3, then textbox is visible.
    if (en->isTalking && playState->msgCtx.msgMode == MSGMODE_TEXT_STARTING)
    {
        #if LOGGING > 0
            is64Printf("_%2d: Textbox shown!\n", en->npcId);
        #endif  

        en->textboxDisplayed = true;
    }

    // If textbox was displayed, and now the message status is blank, then talking has finished.
    if (en->textboxDisplayed && (playState->msgCtx.msgMode == MSGMODE_NONE || talkState == MSGMODE_TEXT_CONTINUING))
    {
        #if LOGGING > 0
            is64Printf("_%2d: _Talking has finished!\n", en->npcId);
        #endif  

        en->talkingFinished = true;
        en->isTalking = false;
        en->textboxDisplayed = false;

        // Setting textbox number to maximum, so that any AWAIT TEXTBOX_NUM commands pass.
        en->textboxNum = __INT8_MAX__;
    }

    #if LOGGING > 2
        is64Printf("_%2d: Conversation status updated.\n", en->npcId);
    #endif       
}

void Update_HitsReaction(NpcMaker* en, PlayState* playState)
{
    #if LOGGING > 2
        is64Printf("_%2d: Checking for hits.\n", en->npcId);
    #endif    

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
            Movement_StopMoving(en, playState, true);

            // If sfx is set, play it.
            // z_actor_play_sfx
            if (en->settings.sfxIfAttacked >= 0)
                func_8002F7DC(&en->actor, en->settings.sfxIfAttacked);

            // Play the attacked animation and setup info that we've been hit.
            Setup_Animation(en, playState, ANIM_ATTACKED, true, true, false, !en->autoAnims, false);
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
            Setup_Animation(en, playState, ANIM_IDLE, true, false, false, false, false);
        }
    }

    #if LOGGING > 2
        is64Printf("_%2d: Checking for hits complete.\n", en->npcId);
    #endif       
}

void Update_Collision(NpcMaker* en, PlayState* playState)
{
    #if LOGGING > 1
        is64Printf("_%2d: Updating collision.\n", en->npcId);
    #endif    

    // Update the collider
    Collider_UpdateCylinder(&en->actor, &en->collider);

    // We need to react to hits before we update the bumpbox and hurtbox (as that resets indication there's been any collisions).
    Update_HitsReaction(en, playState);

    // We update the bump collision (i.e against platforms and such)
    if (en->settings.hasCollision)
    {
        Actor_UpdateBgCheckInfo(playState, &en->actor, 20.0f, en->collider.dim.radius, en->collider.dim.height, 5);
        CollisionCheck_SetOC(playState, &playState->colChkCtx, &en->collider.base);
    }

    if (en->collider.base.acFlags & AC_ON)
        CollisionCheck_SetAC(playState, &playState->colChkCtx, &en->collider.base);

    #if LOGGING > 1
        is64Printf("_%2d: Updating collision complete.\n", en->npcId);
    #endif 
}

void Update_ModelAlpha(NpcMaker* en, PlayState* playState)
{
    #if LOGGING > 2
        is64Printf("_%2d: Updating model transparency.\n", en->npcId);
    #endif   

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

    #if LOGGING > 2
        is64Printf("_%2d: Updating model transparency complete.\n", en->npcId);
    #endif   

}