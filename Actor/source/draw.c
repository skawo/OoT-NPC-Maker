#include "../include/draw.h"
#include "../include/h_rom.h"
#include "../include/h_data.h"
#include "../include/h_movement.h"
#include "../include/h_math.h"
#include "../include/h_graphics.h"

void Draw_Debug(NpcMaker* en, PlayState* playState)
{
    #if LOGGING > 2
        is64Printf("_%2d: DEBUG DRAW\n", en->npcId);
    #endif   

    #if COLLISION_VIEWER == 1

        if (en->settings.showColsDebugOn && en->settings.hasCollision)
        {
            Matrix_Push();

            //z_matrix_translate_3f_800D1694
            Matrix_SetTranslateRotateYXZ(en->actor.world.pos.x, en->actor.world.pos.y + en->collider.dim.yShift, en->actor.world.pos.z, &en->actor.shape.rot);
            Matrix_Scale(MAX(1, en->collider.dim.radius) / 128.0f, en->collider.dim.height / 204.0f, MAX(1, en->collider.dim.radius) / 128.0f, 1);

            gSPMatrix(POLY_XLU.p++, Matrix_NewMtx(playState->state.gfxCtx, "", __LINE__), G_MTX_MODELVIEW | G_MTX_LOAD);
            gSPDisplayList(POLY_XLU.p++, xluMaterial);
            gDPSetEnvColor(POLY_XLU.p++, 0xFF, 0x00, 0x00, 0x7F);
            gSPDisplayList(POLY_XLU.p++, dlCylinder);

            Matrix_Pop();
        }

    #endif

    #if DEBUG_STRUCT == 1

        #if LOOKAT_EDITOR == 1

        if (en->settings.showLookAtEditorDebugOn)
        {
            #if LOGGING > 2
                is64Printf("_%2d: LOOKAT editor is enabled.\n", en->npcId);
            #endif  

            Gfx* gfx = Graph_GfxPlusOne(playState->state.gfxCtx->polyOpa.p);
            gSPDisplayList(playState->state.gfxCtx->overlay.p++, gfx);
            GfxPrint printer;

            GfxPrint_Init(&printer);
            GfxPrint_Open(&printer, gfx);
            GfxPrint_SetColor(&printer, 255, 255, 255, 255);


            char lookAts[5][15] = {"None", "Body", "Head", "Waist", "H & W"}; 
            char axises[6][3] = {"+X", "-X", "+Y", "-Y", "+Z", "-Z"}; 


            GfxPrint_SetPos(&printer, 24, 10);
            GfxPrint_Printf(&printer, "Type %s", lookAts[en->settings.lookAtType]);  
            GfxPrint_SetPos(&printer, 24, 11);
            GfxPrint_Printf(&printer, "Head limb %d", en->settings.headLimb); 
            GfxPrint_SetPos(&printer, 24, 12);
            GfxPrint_Printf(&printer, "Head V Axis %s", axises[en->settings.headVertAxis]); 
            GfxPrint_SetPos(&printer, 24, 13);
            GfxPrint_Printf(&printer, "Head H Axis %s", axises[en->settings.headHorAxis]); 
            GfxPrint_SetPos(&printer, 24, 14);
            GfxPrint_Printf(&printer, "Waist limb %d", en->settings.waistLimb); 
            GfxPrint_SetPos(&printer, 24, 15);
            GfxPrint_Printf(&printer, "Waist V Axis %s", axises[en->settings.waistVertAxis]); 
            GfxPrint_SetPos(&printer, 24, 16);
            GfxPrint_Printf(&printer, "Waist H Axis %s", axises[en->settings.waistHorAxis]); 
            GfxPrint_SetPos(&printer, 24, 17);
            GfxPrint_Printf(&printer, "Deg Ver %d", en->settings.lookAtDegreesVert);  
            GfxPrint_SetPos(&printer, 24, 18);
            GfxPrint_Printf(&printer, "Deg Hor %d", en->settings.lookAtDegreesHor);         
            GfxPrint_SetPos(&printer, 24, 19);
            GfxPrint_Printf(&printer, "Offset X %d", (int)en->settings.lookAtPosOffset.x); 
            GfxPrint_SetPos(&printer, 24, 20);
            GfxPrint_Printf(&printer, "Offset Y %d", (int)en->settings.lookAtPosOffset.y); 
            GfxPrint_SetPos(&printer, 24, 21);
            GfxPrint_Printf(&printer, "Offset Z %d", (int)en->settings.lookAtPosOffset.z);   

            GfxPrint_SetPos(&printer, 22, 10 + en->dbgPosEditorCursorPos);
            GfxPrint_Printf(&printer, ">");   

            gfx = GfxPrint_Close(&printer);
            GfxPrint_Destroy(&printer);
            gSPEndDisplayList(gfx++);
            Graph_BranchDlist(playState->state.gfxCtx->polyOpa.p, gfx);
            playState->state.gfxCtx->polyOpa.p = gfx;                     
        }

        #endif

        #if EXDLIST_EDITOR == 1

        if (en->settings.showDlistEditorDebugOn && en->numExDLists != 0)
        {
            Gfx* gfx = Graph_GfxPlusOne(playState->state.gfxCtx->polyOpa.p);
            gSPDisplayList(playState->state.gfxCtx->overlay.p++, gfx);
            GfxPrint printer;

            GfxPrint_Init(&printer);
            GfxPrint_Open(&printer, gfx);
            GfxPrint_SetColor(&printer, 255, 255, 255, 255);

            GfxPrint_SetPos(&printer, 24, 10);
            GfxPrint_Printf(&printer, "E %02d", en->dbgPosEditorCurEditing);

            if (en->dbgPosEditorCurEditing <= en->numExDLists)
            {
                ExDListEntry dlist = en->extraDLists[en->dbgPosEditorCurEditing];
                GfxPrint_SetPos(&printer, 24, 11);
                GfxPrint_Printf(&printer, "TX %04f", dlist.translation.x);
                GfxPrint_SetPos(&printer, 24, 12);
                GfxPrint_Printf(&printer, "TY %04f", dlist.translation.y);
                GfxPrint_SetPos(&printer, 24, 13);
                GfxPrint_Printf(&printer, "TZ %04f", dlist.translation.z);
                GfxPrint_SetPos(&printer, 24, 14);
                GfxPrint_Printf(&printer, "RX %04d", dlist.rotation.x);
                GfxPrint_SetPos(&printer, 24, 15);
                GfxPrint_Printf(&printer, "RY %04d", dlist.rotation.y);
                GfxPrint_SetPos(&printer, 24, 16);
                GfxPrint_Printf(&printer, "RZ %04d", dlist.rotation.z);
                GfxPrint_SetPos(&printer, 24, 17);
                GfxPrint_Printf(&printer, "Sc %04f", dlist.scale);
                GfxPrint_SetPos(&printer, 24, 18);
                GfxPrint_Printf(&printer, "St %01d", dlist.showType);                
                GfxPrint_SetPos(&printer, 24, 19);
                GfxPrint_Printf(&printer, "L %04d", dlist.limb);
            }
            else
                en->dbgPosEditorCursorPos = 0;

            GfxPrint_SetPos(&printer, 22, 10 + en->dbgPosEditorCursorPos);
            GfxPrint_Printf(&printer, ">");

            gfx = GfxPrint_Close(&printer);
            GfxPrint_Destroy(&printer);
            gSPEndDisplayList(gfx++);
            Graph_BranchDlist(playState->state.gfxCtx->polyOpa.p, gfx);
            playState->state.gfxCtx->polyOpa.p = gfx;
        }

        #endif

        #if LOG_VERSION == 1

        if (en->settings.printToScreenDebugOn)
        {
            Gfx* gfx = Graph_GfxPlusOne(playState->state.gfxCtx->polyOpa.p);
            gSPDisplayList(playState->state.gfxCtx->overlay.p++, gfx);
            GfxPrint printer;

            GfxPrint_Init(&printer);
            GfxPrint_Open(&printer, gfx);
            GfxPrint_SetColor(&printer, 255, 255, 255, 255);

            GfxPrint_SetPos(&printer, 3, 1);

            GfxPrint_Printf(&printer, "DEBUG v.%01d.%01d [%08x] [%08f]", MAJOR_VERSION, MINOR_VERSION, en->dbgVar, en->fDbgVar);

            gfx = GfxPrint_Close(&printer);
            GfxPrint_Destroy(&printer);
            gSPEndDisplayList(gfx++);
            Graph_BranchDlist(playState->state.gfxCtx->polyOpa.p, gfx);
            playState->state.gfxCtx->polyOpa.p = gfx;
        }

        #endif

    #endif    

    #if LOGGING > 2
        is64Printf("_%2d: DEBUG DRAW END\n", en->npcId);
    #endif 
}

inline u32 Draw_GetDrawDestType(NpcMaker* en, PlayState* playState)
{
    return (en->curAlpha == 255 && !playState->actorCtx.lensActive) ? OPA : DRAW_TYPE(en->settings.drawType);
}

void Draw_Setup(NpcMaker* en, PlayState* playState, int drawType)
{
    if (drawType == XLU)
        Gfx_SetupDL_25Xlu(playState->state.gfxCtx);
    else
        Gfx_SetupDL_25Opa(playState->state.gfxCtx);
}

void Draw_Lights(NpcMaker* en, PlayState* playState, Vec3f* translation)
{
    #if LOGGING > 2
        is64Printf("_%2d: Drawing light\n", en->npcId);
    #endif 

    Vec3f transl_in_dir;
    Math_Vec3s_ToVec3f(&transl_in_dir, &en->settings.lightPosOffset);
    Math_AffectMatrixByRot(en->actor.shape.rot.y, &transl_in_dir, NULL);

    if (en->settings.generatesLight)
    {
        // Setting the light point.
        // If glow is enabled, the radius is offset slightly to create a shimmer effect.
        Lights_PointSetInfo(&en->light,
                            translation->x + transl_in_dir.x,
                            translation->y + transl_in_dir.y,
                            translation->z + transl_in_dir.z,
                            en->settings.lightColor.r,
                            en->settings.lightColor.g,
                            en->settings.lightColor.b,
                            en->settings.lightGlow ? Rand_S16Offset(en->settings.lightRadius, 15) : en->settings.lightRadius,
                            en->settings.lightGlow ? LIGHT_POINT_GLOW : LIGHT_POINT_NOGLOW);

    }  

    #if LOGGING > 2
        is64Printf("_%2d: Drawing light complete\n", en->npcId);
    #endif   
}

void Draw_LightsRebind(NpcMaker* en, PlayState* playState) 
{
    #if LOGGING > 2
        is64Printf("_%2d: Rebinding lights\n", en->npcId);
    #endif 

    Vec3f bindPos = en->actor.world.pos;
    bindPos.y += ((en->actor.focus.pos.y - en->actor.world.pos.y) / 2);

    Lights* lights;
    lights = LightContext_NewLights(&playState->lightCtx, playState->state.gfxCtx);
    Lights_BindAll(lights, playState->lightCtx.listHead, &bindPos);
    Lights_Draw(lights, playState->state.gfxCtx);

    #if LOGGING > 2
        is64Printf("_%2d: Rebinding lights done\n", en->npcId);
    #endif     
}

inline void Draw_SetAxis(u8 axis, s16 value, Vec3s* rotation)
{
    /*
    switch (axis)
    {
        case PLUS_X:    rotation->x += value; break;
        case MINUX_X:   rotation->x -= value; break;
        case PLUS_Y:    rotation->y += value; break;
        case MINUS_Y:   rotation->y -= value; break;
        case PLUS_Z:    rotation->z += value; break;
        case MINUS_Z:   rotation->z += value; break;
        default:        break;
    }
    */

   *((u16*)rotation + axis / 2) += (axis % 2 ? -value : value);
}

void Draw_ExtDList(NpcMaker *en, PlayState* playState, ExDListEntry* dList, bool SwapDest)
{
    int dT = Draw_GetDrawDestType(en, playState);

    TwoHeadGfxArena* dest;

	if (SwapDest)
		dest = dT ? &POLY_OPA : &POLY_XLU;
	else
		dest = dT ? &POLY_XLU : &POLY_OPA;

    Draw_ExtDListInt(en, playState, dList, &dest->p);
}

// Matrix should be set before this is called.
void Draw_ExtDListInt(NpcMaker *en, PlayState* playState, ExDListEntry* dList, Gfx** gfxP)
{
    #if LOGGING > 2
        is64Printf("_%2d: Drawing extra display list at limb %2d\n", en->npcId, dList->limb);
    #endif 

    s32 object = R_OBJECT(en, dList->objectId);
    u32 dListOffset = object == OBJECT_RAM ? dList->offset : OFFSET_ADDRESS(6, dList->offset);
	
    switch (object)
    {
        case OBJECT_NONE: return;
        case OBJECT_RAM: break;
        default:
        {
            // Setting segment 6 if object is different to the currently loaded object...
            if (object != en->settings.objectId || dList->fileStart != OBJECT_CURRENT)
            {
                void* pointer = Rom_GetObjectDataPtr(object, playState);

                if (pointer == NULL)
                {
                    #if LOGGING > 0
                        is64Printf("_Dlist wants object %04x set, but it wasn't loaded, so the dlist will not be drawn.\n", object);
                    #endif

                    return;
                }
                else  
                    gSPSegment((*gfxP)++, 6, AADDR(pointer, (R_FILESTART(en, dList->fileStart))));
            }
                
            break;
        }
    }

    Draw_SetEnvColor(gfxP, dList->envColor, en->curAlpha);
    gDPPipeSync((*gfxP)++);    
    gSPMatrix((*gfxP)++, Matrix_NewMtx(playState->state.gfxCtx, "", __LINE__), G_MTX_MODELVIEW | G_MTX_LOAD);
    gSPDisplayList((*gfxP)++, dListOffset);

    // Resetting segment 6 if object that was used is different to what the npc is using.
    if ((en->settings.objectId > 0 && object != en->settings.objectId) || dList->fileStart != OBJECT_CURRENT)
        gSPSegment((*gfxP)++, 6, AADDR(Rom_GetObjectDataPtr(en->settings.objectId, playState), en->settings.fileStart));

    Draw_SetEnvColor(gfxP, en->curColor, en->curAlpha);

    #if LOGGING > 2
        is64Printf("_%2d: Drawing extra display list complete\n", en->npcId);
    #endif 
}

void Draw_AffectMatrix(ExDListEntry dlist, Vec3f* translation, Vec3s* rotation)
{
    if (translation != NULL && rotation != NULL)
        Matrix_TranslateRotateZYX(translation, rotation);

    Matrix_Translate(dlist.translation.x, dlist.translation.y, dlist.translation.z, 1);
    Matrix_RotateZYX(dlist.rotation.x, dlist.rotation.y, dlist.rotation.z, 1);
    Matrix_Scale(dlist.scale, dlist.scale, dlist.scale, 1);
}

void Draw_CalcFocusPos(PlayState* playState, s32 limb, NpcMaker* en)
{
    if (limb == en->settings.targetLimb + 1)
    {
        Vec3f in = { en->settings.targetPosOffset.x, en->settings.targetPosOffset.y, en->settings.targetPosOffset.z };
        Matrix_MultVec3f(&in, &en->actor.focus.pos);
    }
}

void Draw_SetEnvColor(Gfx** destP, Color_RGB8 color, u8 alpha)
{
    gDPPipeSync((*destP)++);
    gDPSetEnvColor((*destP)++, color.r, color.g, color.b, alpha);
}

void Draw_PostLimbDraw(PlayState* playState, s32 limb, Gfx** dListPtr, Vec3s* rot, void* instance, Gfx** gfxP)
{
    NpcMaker* en = (NpcMaker*)instance;
    Draw_CalcFocusPos(playState, limb, en);
    
    u32 cFuncOffs = en->CFuncs[5];
	
    if (cFuncOffs != 0xFFFFFFFF)
    {
        #if LOGGING > 2
            is64Printf("_Running embedded post-limb function %8x\n", en->embeddedOverlay + cFuncOffs);
        #endif
        
        typedef float EmbeddedFunction(NpcMaker* en, PlayState* playState, s32 limbNumber, Gfx** dListPtr, Vec3f* translation, Vec3s* rotation, void* instance, Gfx** gfxP);
        EmbeddedFunction* f = (EmbeddedFunction*)en->embeddedOverlay + cFuncOffs;
        f(en, playState, limb, dListPtr, NULL, rot, instance, gfxP);

        #if LOGGING > 2
            is64Printf("_Embedded function finished.\n");
        #endif
    }
}

s32 Draw_PostLimbDrawSkin(Actor* instance, PlayState* playState, s32 limb, Skin* skelanime)
{
    // Should be only doing this for limbs that have textures to change, but whatever.
    NpcMaker* en = (NpcMaker*)instance;
    Draw_SetupSegments(en, playState);
    Draw_CalcFocusPos(playState, limb, en);
    
    u32 cFuncOffs = en->CFuncs[5];
	
    if (cFuncOffs != 0xFFFFFFFF)
    {
        #if LOGGING > 2
            is64Printf("_Running embedded post-limb function %8x\n", en->embeddedOverlay + cFuncOffs);
        #endif
        
        typedef float EmbeddedFunction(NpcMaker* en, PlayState* playState, s32 limbNumber, Gfx** dListPtr, Vec3f* translation, Vec3s* rotation, void* instance, Gfx** gfxP);
        EmbeddedFunction* f = (EmbeddedFunction*)en->embeddedOverlay + cFuncOffs;
        f(en, playState, limb, NULL, NULL, NULL, instance, NULL);

        #if LOGGING > 2
            is64Printf("_Embedded function finished.\n");
        #endif
    }
    
    return 1;
}

s32 Draw_OverrideLimbDraw(PlayState* playState, s32 limbNumber, Gfx** dListPtr, Vec3f* translation, Vec3s* rotation, void* instance, Gfx** gfxP)
{
    NpcMaker* en = (NpcMaker*)instance;
    int sLimbNumber = limbNumber - 1;
    s32 out = 0;

    #if LOGGING > 2
        is64Printf("_%2d: Drawing limb %2d\n", en->npcId, sLimbNumber);
    #endif

#pragma region LimbRotations

    if (sLimbNumber == en->settings.headLimb)
    {
        if (en->settings.lookAtType == LOOK_HEAD || en->settings.lookAtType == LOOK_BOTH)
        {
            Draw_SetAxis(en->settings.headHorAxis, en->limbRotA * (en->settings.lookAtType == LOOK_BOTH ? LOOKAT_HEAD_WAIST_MULTIPIER : 1), rotation);
            Draw_SetAxis(en->settings.headVertAxis, en->limbRotB, rotation);
        }
    }

    if (sLimbNumber == en->settings.waistLimb)
    {
        if (en->settings.lookAtType == LOOK_WAIST || en->settings.lookAtType == LOOK_BOTH)
        {
            Draw_SetAxis(en->settings.waistHorAxis, en->limbRotA * (en->settings.lookAtType == LOOK_BOTH ? LOOKAT_WAIST_HORIZ_MULTIPIER : 1), rotation);
            Draw_SetAxis(en->settings.waistVertAxis, en->limbRotB * (en->settings.lookAtType == LOOK_BOTH ? LOOKAT_WAIST_VERT_MULTIPIER : 1), rotation);
        }
    }

#pragma endregion

#pragma region OriginTranslation

    if (!sLimbNumber)
    {
        translation->x += en->settings.modelPosOffset.x;
        translation->y += en->settings.modelPosOffset.y;
        translation->z += en->settings.modelPosOffset.z;
    }

#pragma endregion

#pragma region Lights

    if (sLimbNumber == en->settings.lightLimb)
    {
        Vec3f worldPos;
        Vec3f in = { translation->x, translation->y, translation->z};
		Matrix_MultVec3f(&in, &worldPos);
        
        Draw_Lights(en, playState, &worldPos);
    }

#pragma endregion 

#pragma region External Dlists

    for (int i = 0; i < en->numExDLists; i++)
    {
        ExDListEntry dlist = en->extraDLists[i];
        
        if (sLimbNumber == dlist.limb)
        {
            s32 object = R_OBJECT(en, dlist.objectId);  

            if (dlist.showType == INSTEAD_OF_LIMB || dlist.showType == IN_SKELETON)
                *dListPtr = 0;

            if (dlist.showType >= IN_SKELETON && object == en->settings.objectId)
            {
                Math_Vec3s_Sum(rotation, &dlist.rotation, rotation);
                Math_Vec3f_Sum(translation, &dlist.translation, translation);
                Matrix_Scale(dlist.scale, dlist.scale, dlist.scale, 1);
				
				if (dlist.showType != CONTROL_EXISTING)
				{
					u32 dListOffset = object == OBJECT_RAM ? dlist.offset : OFFSET_ADDRESS(6, dlist.offset);
					*dListPtr = (Gfx*)dListOffset;
				}

            }
            else if (dlist.showType != NOT_VISIBLE)
            {
				if (dlist.objectId == OBJECT_ENDDLIST)
					*dListPtr = (Gfx*)&endDList;
				else if (dlist.objectId == OBJECT_XLUDLIST)
					*dListPtr = (Gfx*)&transparencyDList;
                else if (dlist.objectId != OBJECT_NONE)
                {
                    Matrix_Push();
                    Draw_AffectMatrix(dlist, translation, rotation);
                    Draw_ExtDList(en, playState, &dlist, true);
                    Matrix_Pop();               
                }
                else
                    out = 1;
            }
        }
    }

#pragma endregion

#pragma region Colors

    for (int i = 0; i < en->numExColors; i++)
    {
        ColorEntry col = en->dListColors[i];

        if (sLimbNumber == col.limb)
            en->curColor = col.color;
    }

    Draw_SetEnvColor(gfxP, en->curColor, en->curAlpha);

#pragma endregion

    u32 cFuncOffs = en->CFuncs[3];
	
    if (cFuncOffs == 0xFFFFFFFF)
        return out;
    else
    {

        #if LOGGING > 2
            is64Printf("_Running embedded limb function %8x\n", en->embeddedOverlay + cFuncOffs);
        #endif

        typedef float EmbeddedFunction(NpcMaker* en, PlayState* playState, s32 limbNumber, Gfx** dListPtr, Vec3f* translation, Vec3s* rotation, void* instance, Gfx** gfxP);
        EmbeddedFunction* f = (EmbeddedFunction*)en->embeddedOverlay + cFuncOffs;
        out = f(en, playState, limbNumber, dListPtr, translation, rotation, instance, gfxP);

        #if LOGGING > 2
            is64Printf("_Embedded function finished.\n");
        #endif

        return out;
    }
}

void Draw_SetupSegments(NpcMaker* en, PlayState* playState)
{
    #if LOGGING > 1
        is64Printf("_%2d: Setting up segment data.\n", en->npcId);
    #endif 

    if (en->exSegData == NULL)
        return;

    for (int i = 0; i < 6; i++)
    {
        u32* texaddress = (u32*)en->exSegData[i];

        if (texaddress != NULL)
        {
            // We get the currently set entry for this segment.
            ExSegDataEntry* data = Data_GetExtraSegmentData(en, i, en->segmentDataIds[i]);

            u32 pointer = 0;
            s32 r_obj = R_OBJECT(en, data->objectId);

            switch (r_obj)
            {
                case OBJECT_NONE:           continue;
                case OBJECT_RAM:            pointer = 0; break;
                case OBJECT_XLUDLIST:       pointer = 0; data->fileStart = 0; data->offset = (u32)&transparencyDList; break;
                case OBJECT_ENDDLIST:       pointer = 0; data->fileStart = 0; data->offset = (u32)&endDList; break;
                default:
                {
                    pointer = (u32)Rom_GetObjectDataPtr(r_obj, playState);

                    if (pointer == 0)
                    {
                        #if LOGGING > 0
                            is64Printf("_Segment data had object %04x set, but it wasn't loaded, so the segment will not be updated.\n", data->objectId);
                        #endif

                        continue;
                    }                       
                }
            }

            gSPSegment(POLY_XLU.p++, i + 8, pointer + data->offset + (R_FILESTART(en, data->fileStart)));
            gSPSegment(POLY_OPA.p++, i + 8, pointer + data->offset + (R_FILESTART(en, data->fileStart)));
        }
    }

    #if LOGGING > 1
        is64Printf("_%2d: Setting up segment data done.\n", en->npcId);
    #endif     
}

void Draw_SetGlobalEnvColor(NpcMaker* en, PlayState* playState)
{
    if (en->settings.usesEnvColor)
    {
        Draw_SetEnvColor(&POLY_XLU.p, en->settings.envColor, en->curAlpha);
        Draw_SetEnvColor(&POLY_OPA.p, en->settings.envColor, en->curAlpha);
        en->curColor = en->settings.envColor;
    }
    // If playState color is not set, then we set env color to white.
    else
    {
        Color_RGB8 defEnvColor = (Color_RGB8){255, 255, 255};
        Draw_SetEnvColor(&POLY_XLU.p, defEnvColor, en->curAlpha);
        Draw_SetEnvColor(&POLY_OPA.p, defEnvColor, en->curAlpha);
        en->curColor = defEnvColor;
    }
}

extern void __View_ApplyTo(View* view, s32 mask, Gfx** gfxP);
#if GAME_VERSION == 0
    asm("__View_ApplyTo = 0x800AB9EC");
#elif GAME_VERSION == 1
    asm("__View_ApplyTo = 0x80092A88");
#endif	

void Draw_StaticExtDLists(NpcMaker* en, PlayState* playState)
{
    #if LOGGING > 1
        is64Printf("_%2d: Drawing static ExDLists.\n", en->npcId);
    #endif  

    for (int i = 0; i < en->numExDLists; i++)
    {
        ExDListEntry dlist = en->extraDLists[i];

        if (dlist.limb < 0)
        {
            if (dlist.showType != NOT_VISIBLE && dlist.limb > STATIC_EXDLIST_AT_DISPLAY)
            {
                Vec3f translation = (Vec3f){0,0,0};
                Vec3s rotation = (Vec3s){0,0,0};

                switch (dlist.limb)
                {
                    case STATIC_EXDLIST_RELATIVE:
                    {
                        translation = en->actor.world.pos;
                        rotation = en->actor.world.rot;         
                        break;               
                    }
                    case STATIC_EXDLIST_AT_CAM:
                    {
                        Camera* cam = playState->cameraPtrs[playState->activeCamId];

                        if (playState->csCtx.state != 0)
                            cam = Play_GetCamera(playState, playState->csCtx.subCamId);

                        // Get vector from cam
                        OLib_Vec3fDistNormalize(&translation, &cam->eye, &cam->at);
                        // Translation.z -> used as distance from the camera
                        Math_Vec3f_Scale(&translation, dlist.translation.z);
                        Math_Vec3f_Sum(&translation, &cam->eye, &translation);

                        rotation = cam->camDir;
                        rotation.y += 0x8000;  
                        break;               
                    }
                    default: break;
                }

                Math_Vec3f_Sum(&translation, &dlist.translation, &translation);
                Math_Vec3s_Sum(&rotation, &dlist.rotation, &rotation);

                if (dlist.limb == STATIC_EXDLIST_AT_CAM)
                    translation.z -= dlist.translation.z;

                Matrix_Push();
                Matrix_SetTranslateRotateYXZ(translation.x, translation.y, translation.z, &rotation);

                float scale = dlist.scale *= en->actor.scale.x;

                Matrix_Scale(scale, scale, scale, 1);
                Draw_ExtDList(en, playState, &dlist, false);
                Matrix_Pop();                
            }
            else if (dlist.showType != NOT_VISIBLE && dlist.limb == STATIC_EXDLIST_AT_DISPLAY)
            {
                Matrix_Push();

                Matrix_Translate(playState->view.eye.x, playState->view.eye.y, playState->view.eye.z, MTXMODE_NEW);
         
                Matrix_Scale(0.01f, 0.01f, 0.01f, MTXMODE_APPLY);
                Matrix_ReplaceRotation(&playState->billboardMtxF);
                Matrix_Translate(dlist.translation.x, dlist.translation.y, (1 / dlist.scale) * dlist.translation.z * (1000 / playState->view.fovy) - playState->view.fovy, MTXMODE_APPLY); 
                Matrix_RotateZYX(dlist.rotation.x, dlist.rotation.y, dlist.rotation.z, MTXMODE_APPLY);               

                Draw_ExtDList(en, playState, &dlist, false);
                
                Matrix_Pop();
            }
            else if (dlist.showType != NOT_VISIBLE && dlist.limb == STATIC_EXDLIST_ORTHOGRAPHIC)
            {
                #define __gfxCtx playState->state.gfxCtx

                View view;
                View_Init(&view, playState->state.gfxCtx);
                view.flags = VIEW_VIEWPORT | VIEW_PROJECTION_ORTHO;

                Gfx* gfxRef = POLY_OPA_DISP;
                Gfx* gfx = Graph_GfxPlusOne(gfxRef);
                gSPDisplayList(OVERLAY_DISP++, gfx);                

                SET_FULLSCREEN_VIEWPORT(&view);

                __View_ApplyTo(&view, VIEW_ALL, &gfx);

                gDPPipeSync(gfx++);
                gSPTexture(gfx++, 0xFFFF, 0xFFFF, 0, G_TX_RENDERTILE, G_ON);
                gDPSetCombineMode(gfx++, G_CC_MODULATEIDECALA, G_CC_MODULATEIA_PRIM2);
                gDPSetOtherMode(gfx++, G_AD_NOTPATTERN | G_CD_MAGICSQ | G_CK_NONE | G_TC_FILT | G_TF_BILERP | G_TT_NONE | G_TL_TILE |
                                    G_TD_CLAMP | G_TP_PERSP | G_CYC_2CYCLE | G_PM_NPRIMITIVE,
                                G_AC_NONE | G_ZS_PIXEL | G_RM_FOG_SHADE_A | G_RM_AA_ZB_OPA_SURF2);
                gSPSetGeometryMode(gfx++, G_ZBUFFER | G_SHADE | G_CULL_BACK | G_FOG | G_LIGHTING | G_SHADING_SMOOTH);

                Matrix_Push();

                int z = dlist.translation.z - 200;
                Matrix_Translate(dlist.translation.x, dlist.translation.y, z, MTXMODE_NEW);
                
                // The emulator used for Zelda: OoT on Wii VC has the support for orthographic projection broken; all faces get drawn in the wrong order.
                // This can be used to fix that; by setting the dlist's draw scale to a negative, the dlist gets drawn properly.
                // (Note: the emulator used in the GCN Zelda Collector's Edition has this even more broken, and orthographic projection cannot be used there at all!)
                if (dlist.scale < 0)
                    Matrix_Scale(-dlist.scale, -dlist.scale, dlist.scale, MTXMODE_APPLY);
                else
                    Matrix_Scale(dlist.scale , dlist.scale, dlist.scale, MTXMODE_APPLY);
                       
                Matrix_RotateZYX(dlist.rotation.x, dlist.rotation.y, dlist.rotation.z, MTXMODE_APPLY);   

                Draw_ExtDListInt(en, playState, &dlist, &gfx);

                gSPEndDisplayList(gfx++);
                Graph_BranchDlist(gfxRef, gfx);
                POLY_OPA_DISP = gfx;
                
                Matrix_Pop();
            }
        }
    }   

    #if LOGGING > 1
        is64Printf("_%2d: Drawing static ExDLists done.\n", en->npcId);
    #endif 
}

void Draw_Model(NpcMaker* en, PlayState* playState)
{
    int dT = Draw_GetDrawDestType(en, playState);

    TwoHeadGfxArena* dest = (dT ? &POLY_XLU : &POLY_OPA);
	
    Draw_Setup(en, playState, dT);

    // Reset the file location to account for the file start offset.
    if (1)
    {
        Rom_SetObjectToActor(&en->actor, playState, en->settings.objectId, en->settings.fileStart);
        gSPSegment(POLY_XLU.p++, 0x06, playState->objectCtx.status[en->actor.objBankIndex].segment + en->settings.fileStart);
        gSPSegment(POLY_OPA.p++, 0x06, playState->objectCtx.status[en->actor.objBankIndex].segment + en->settings.fileStart);
    }    

    // Draw static exdlists (ones not attached to a limb)
    if (en->hasStaticExDlists)
        Draw_StaticExtDLists(en, playState);
	

    #if LOGGING > 1
        is64Printf("_%2d: Drawing the skeleton.\n", en->npcId);
    #endif  

    // Draw skeleton
    if (en->settings.objectId > 0)
    {
        switch (en->settings.drawType)
        {
            case OPA_MATRIX:
            case XLU_MATRIX:
            {
                dest->p = (Gfx*) SkelAnime_DrawFlex(
                                                    playState,
                                                    (void*)en->skin.skelAnime.skeleton,
                                                    en->skin.skelAnime.jointTable,
                                                    en->skin.skelAnime.limbCount,
                                                    &Draw_OverrideLimbDraw,
                                                    &Draw_PostLimbDraw,
                                                    &en->actor,
                                                    dest->p
                                                   ); 
                break;
            }
            case OPA_NONMATRIX:
            case XLU_NONMATRIX:
            {
                dest->p = (Gfx*) SkelAnime_Draw( 
                                                playState,
                                                (void*)en->skin.skelAnime.skeleton,
                                                en->skin.skelAnime.jointTable,
                                                &Draw_OverrideLimbDraw,
                                                &Draw_PostLimbDraw,
                                                &en->actor,
                                                dest->p
                                               ); 
                break;
            }
            case SKIN:
            {
                //z_skelanime_draw_weighted_unk
                func_800A6360(
                                &en->actor,
                                playState,
                                &en->skin,
                                NULL,
                                &Draw_PostLimbDrawSkin,
                                1
                             ); 
                break;
            }
            default: break;
        }
    }

    #if LOGGING > 1
        is64Printf("_%2d: Drawing the skeleton complete.\n", en->npcId);
    #endif  
}