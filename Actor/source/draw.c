#include "../include/draw.h"
#include "../include/h_rom.h"
#include "../include/h_data.h"
#include "../include/h_movement.h"
#include "../include/h_math.h"
#include "../include/h_graphics.h"

void Draw_Debug(NpcMaker* en, GlobalContext* globalCtx)
{
    #if COLLISION_VIEWER == 1

        if (en->settings.showColsDebugOn && en->settings.hasCollision)
        {
            Matrix_Push();

            //z_matrix_translate_3f_800D1694
            func_800D1694(en->actor.world.pos.x, en->actor.world.pos.y + en->collider.dim.yShift, en->actor.world.pos.z, &en->actor.shape.rot);
            Matrix_Scale(MAX(1, en->collider.dim.radius) / 128.0f, en->collider.dim.height / 204.0f, MAX(1, en->collider.dim.radius) / 128.0f, 1);

            gSPMatrix(POLY_XLU.p++, Matrix_NewMtx(globalCtx->state.gfxCtx, "", __LINE__), G_MTX_MODELVIEW | G_MTX_LOAD);
            gSPDisplayList(POLY_XLU.p++, xluMaterial);
            gDPSetEnvColor(POLY_XLU.p++, 0xFF, 0x00, 0x00, 0x7F);
            gSPDisplayList(POLY_XLU.p++, dlCylinder);

            Matrix_Pop();
        }

    #endif

    #if DEBUG_STRUCT == 1

        #if EXDLIST_EDITOR == 1

        if (en->dbgEnabledPosEditor && en->numExDLists != 0)
        {
            Gfx* gfx = Graph_GfxPlusOne(globalCtx->state.gfxCtx->polyOpa.p);
            gSPDisplayList(globalCtx->state.gfxCtx->overlay.p++, gfx);
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
                GfxPrint_Printf(&printer, "L %04d", dlist.limb);
            }
            else
                en->dbgPosEditorCursorPos = 0;

            GfxPrint_SetPos(&printer, 22, 10 + en->dbgPosEditorCursorPos);
            GfxPrint_Printf(&printer, ">");

            gfx = GfxPrint_Close(&printer);
            GfxPrint_Destroy(&printer);
            gSPEndDisplayList(gfx++);
            Graph_BranchDlist(globalCtx->state.gfxCtx->polyOpa.p, gfx);
            globalCtx->state.gfxCtx->polyOpa.p = gfx;
        }

        #endif

        #if LOG_VERSION == 1

        if (en->dgbDrawVersion)
        {
            Gfx* gfx = Graph_GfxPlusOne(globalCtx->state.gfxCtx->polyOpa.p);
            gSPDisplayList(globalCtx->state.gfxCtx->overlay.p++, gfx);
            GfxPrint printer;

            GfxPrint_Init(&printer);
            GfxPrint_Open(&printer, gfx);
            GfxPrint_SetColor(&printer, 255, 255, 255, 255);

            GfxPrint_SetPos(&printer, 3, 1);

            GfxPrint_Printf(&printer, "DEBUG v.%01d.%01d [%08x] [%08x]", MAJOR_VERSION, MINOR_VERSION, globalCtx->csCtx.frames, en->dbgVar2);

            gfx = GfxPrint_Close(&printer);
            GfxPrint_Destroy(&printer);
            gSPEndDisplayList(gfx++);
            Graph_BranchDlist(globalCtx->state.gfxCtx->polyOpa.p, gfx);
            globalCtx->state.gfxCtx->polyOpa.p = gfx;
        }

        #endif

    #endif    
}

inline u32 Draw_GetDrawDestType(NpcMaker* en, GlobalContext* globalCtx)
{
    return (en->curAlpha == 255 && globalCtx->actorCtx.unk_03 == 0) ? OPA : DRAW_TYPE(en->settings.drawType);
}

void Draw_Setup(NpcMaker* en, GlobalContext* globalCtx, int drawType)
{
    if (drawType == XLU)
        func_80093D84(globalCtx->state.gfxCtx);
    else
        func_80093D18(globalCtx->state.gfxCtx);
}

void Draw_Lights(NpcMaker* en, GlobalContext* globalCtx, Vec3f* translation)
{
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
}

void Draw_LightsRebind(NpcMaker* en, GlobalContext* globalCtx) 
{
    Vec3f bindPos = en->actor.world.pos;
    bindPos.y += ((en->actor.focus.pos.y - en->actor.world.pos.y) / 2);

    Lights* lights;
    lights = LightContext_NewLights(&globalCtx->lightCtx, globalCtx->state.gfxCtx);
    Lights_BindAll(lights, globalCtx->lightCtx.listHead, &bindPos);
    Lights_Draw(lights, globalCtx->state.gfxCtx);
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

// Matrix should be set before this is called.
void Draw_ExtDList(NpcMaker *en, GlobalContext* globalCtx, ExDListEntry* dList)
{
    s32 object = R_OBJECT(en, dList->objectId);
    u32 dListOffset = object == OBJECT_RAM ? dList->offset : OFFSET_ADDRESS(6, dList->offset);

    // Always drawing to the other buffer than the main model is.
    int dT = Draw_GetDrawDestType(en, globalCtx);
    TwoHeadGfxArena *dest = dT ? &POLY_OPA : &POLY_XLU;

    switch (object)
    {
        case OBJECT_NONE: return;
        case OBJECT_RAM: break;
        default:
        {
            // Setting segment 6 if object is different to the currently loaded object...
            if (object != en->settings.objectId)
            {
                void* pointer = Rom_GetObjectDataPtr(object, globalCtx);

                if (pointer == NULL)
                {
                    #if LOGGING == 1
                        osSyncPrintf("_Dlist wants object %04x set, but it wasn't loaded, so the dlist will not be drawn.", object);
                    #endif

                    return;
                }
                else  
                    gSPSegment(dest->p++, 6, pointer);
            }
                
            break;
        }
    }

    Draw_SetEnvColor(&dest->p, dList->envColor, en->curAlpha);
    gSPMatrix(dest->p++, Matrix_NewMtx(globalCtx->state.gfxCtx, "", __LINE__), G_MTX_MODELVIEW | G_MTX_LOAD);
    gSPDisplayList(dest->p++, dListOffset);
    

    // Resetting segment 6 if object that was used is different to what the npc is using.
    if (en->settings.objectId > 0 && object != en->settings.objectId)
        gSPSegment(dest->p++, 6, Rom_GetObjectDataPtr(en->settings.objectId, globalCtx));

    Draw_SetEnvColor(&dest->p, en->curColor, en->curAlpha);
}

void Draw_AffectMatrix(ExDListEntry dlist, Vec3f* translation, Vec3s* rotation)
{
    if (translation != NULL && rotation != NULL)
        Matrix_JointPosition(translation, rotation);

    Matrix_Translate(dlist.translation.x, dlist.translation.y, dlist.translation.z, 1);
    Matrix_RotateRPY(dlist.rotation.x, dlist.rotation.y, dlist.rotation.z, 1);
    Matrix_Scale(dlist.scale, dlist.scale, dlist.scale, 1);
}

void Draw_CalcFocusPos(GlobalContext* globalCtx, s32 limb, NpcMaker* en)
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

void Draw_PostLimbDraw(GlobalContext* globalCtx, s32 limb, Gfx** dListPtr, Vec3s* rot, void* instance, Gfx** gfxP)
{
    NpcMaker* en = (NpcMaker*)instance;
    Draw_CalcFocusPos(globalCtx, limb, en);
}

s32 Draw_PostLimbDrawSkin(Actor* instance, GlobalContext* globalCtx, s32 limb, PSkinAwb* skelanime)
{
    // Should be only doing this for limbs that have textures to change, but whatever.
    NpcMaker* en = (NpcMaker*)instance;
    Draw_SetupSegments(en, globalCtx);
    Draw_CalcFocusPos(globalCtx, limb, en);
    
    return 1;
}

s32 Draw_OverrideLimbDraw(GlobalContext* globalCtx, s32 limbNumber, Gfx** dListPtr, Vec3f* translation, Vec3s* rotation, void* instance, Gfx** gfxP)
{
    NpcMaker* en = (NpcMaker*)instance;
    int sLimbNumber = limbNumber - 1;

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
        
        Draw_Lights(en, globalCtx, &worldPos);
    }

#pragma endregion 

#pragma region External Dlists

    for (int i = 0; i < en->numExDLists; i++)
    {
        ExDListEntry dlist = en->extraDLists[i];
        
        if (sLimbNumber == dlist.limb)
        {
            s32 object = R_OBJECT(en, dlist.objectId);  

            if (dlist.showType >= INSTEAD_OF_LIMB)
                *dListPtr = 0;

            if (dlist.showType == CONTROL && object == en->settings.objectId)
            {
                Math_Vec3s_Sum(rotation, &dlist.rotation, rotation);
                Math_Vec3f_Sum(translation, &dlist.translation, translation);
                Matrix_Scale(dlist.scale, dlist.scale, dlist.scale, 1);

                u32 dListOffset = object == OBJECT_RAM ? dlist.offset : OFFSET_ADDRESS(6, dlist.offset);
                *dListPtr = (Gfx*)dListOffset;

            }
            else if (dlist.showType != NOT_VISIBLE)
            {
                Matrix_Push();
                Draw_AffectMatrix(dlist, translation, rotation);
                Draw_ExtDList(en, globalCtx, &dlist);
                Matrix_Pop();                
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

    return 0;
}

void Draw_SetupSegments(NpcMaker* en, GlobalContext* globalCtx)
{
    if (en->exSegData == NULL)
        return;

    for (int i = 0; i < 8; i++)
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
                case OBJECT_XLUDLIST:       pointer = 0; data->offset = (u32)&transparencyDList; break;
                case OBJECT_ENDDLIST:       pointer = 0; data->offset = (u32)&endDList; break;
                default:
                {
                    pointer = (u32)Rom_GetObjectDataPtr(r_obj, globalCtx);

                    if (pointer == 0)
                    {
                        #if LOGGING == 1
                            osSyncPrintf("_Segment data had object %04x set, but it wasn't loaded, so the segment will not be updated.", data->objectId);
                        #endif

                        continue;
                    }                       
                }
            }

            gSPSegment(POLY_XLU.p++, i + 8, pointer + data->offset);
            gSPSegment(POLY_OPA.p++, i + 8, pointer + data->offset);
        }
    }
}

void Draw_SetGlobalEnvColor(NpcMaker* en, GlobalContext* globalCtx)
{
    if (en->settings.usesEnvColor)
    {
        Draw_SetEnvColor(&POLY_XLU.p, en->settings.envColor, en->curAlpha);
        Draw_SetEnvColor(&POLY_OPA.p, en->settings.envColor, en->curAlpha);
        en->curColor = en->settings.envColor;
    }
    // If global color is not set, then we set env color to white.
    else
    {
        Color_RGB8 defEnvColor = (Color_RGB8){255, 255, 255};
        Draw_SetEnvColor(&POLY_XLU.p, defEnvColor, en->curAlpha);
        Draw_SetEnvColor(&POLY_OPA.p, defEnvColor, en->curAlpha);
        en->curColor = defEnvColor;
    }
}

void Draw_StaticExtDLists(NpcMaker* en, GlobalContext* globalCtx)
{
    for (int i = 0; i < en->numExDLists; i++)
    {
        ExDListEntry dlist = en->extraDLists[i];

        if (dlist.limb < 0)
        {
            if (dlist.showType != NOT_VISIBLE)
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
                        Camera* cam = globalCtx->cameraPtrs[globalCtx->activeCamera];

                        if (globalCtx->csCtx.state != 0)
                            cam = Gameplay_GetCamera(globalCtx, globalCtx->csCtx.unk_14);

                        OLib_Vec3fDistNormalize(&translation, &cam->eye, &cam->at);
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
                func_800D1694(translation.x, translation.y, translation.z, &rotation);

                float scale = dlist.scale *= en->actor.scale.x;

                Matrix_Scale(scale, scale, scale, 1);
                Draw_ExtDList(en, globalCtx, &dlist);
                Matrix_Pop();                
            }
        }
    }   
}

void Draw_Model(NpcMaker* en, GlobalContext* globalCtx)
{
    int dT = Draw_GetDrawDestType(en, globalCtx);

    TwoHeadGfxArena* dest = (dT ? &POLY_XLU : &POLY_OPA);
    Draw_Setup(en, globalCtx, dT);

    // Draw static exdlists (ones not attached to a limb)
    if (en->hasStaticExDlists)
        Draw_StaticExtDLists(en, globalCtx);

    // Draw skeleton
    if (en->settings.objectId > 0)
    {
        switch (en->settings.drawType)
        {
            case OPA_MATRIX:
            case XLU_MATRIX:
            {
                dest->p = (Gfx*) SkelAnime_DrawFlex(
                                                    globalCtx,
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
                                                globalCtx,
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
                                globalCtx,
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
}