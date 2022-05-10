#ifndef H_GRAPHICS_H
#define H_GRAPHICS_H

#include "npc_maker_types.h"


Gfx transparencyDList[] = {
     gsDPSetRenderMode(G_RM_FOG_SHADE_A, AA_EN | Z_CMP | Z_UPD | IM_RD | CLR_ON_CVG | CVG_DST_WRAP | ZMODE_XLU |
                                            FORCE_BL | GBL_c2(G_BL_CLR_IN, G_BL_A_IN, G_BL_CLR_MEM, G_BL_1MA)),
     gsDPSetAlphaCompare(G_AC_THRESHOLD),
     gsSPEndDisplayList() ,
};

Gfx endDList[] = {
    gsSPEndDisplayList(),
};

#if COLLISION_VIEWER == 1

	Gfx xluMaterial[] = {
		gsSPTexture(qu016(0.999985), qu016(0.999985), 0, G_TX_RENDERTILE, G_OFF),
		gsDPPipeSync(),
		gsDPSetRenderMode(AA_EN | Z_CMP | Z_UPD | IM_RD | FORCE_BL | CVG_DST_CLAMP | ZMODE_XLU | CVG_X_ALPHA | ALPHA_CVG_SEL | GBL_c1(G_BL_CLR_FOG, G_BL_A_SHADE, G_BL_CLR_IN, G_BL_1MA), G_RM_AA_ZB_TEX_EDGE2),
		gsDPSetCombineLERP(
			0, 0, 0, ENVIRONMENT
			, 0, 0, 0, ENVIRONMENT
			, 0, 0, 0, COMBINED
			, 0, 0, 0, COMBINED
		),
		gsSPEndDisplayList()
	};

	Vtx vCylinder[32] = {
		gdSPDefVtxN(0, 200, -100, 0, 0, 0, 190, 169, 255),
		gdSPDefVtxN(38, 0, -92, 0, 0, 71, 175, 86, 255),
		gdSPDefVtxN(0, 0, -100, 0, 0, 0, 190, 87, 255),
		gdSPDefVtxN(38, 200, -92, 0, 0, 71, 175, 170, 255),
		gdSPDefVtxN(71, 0, -71, 0, 0, 135, 135, 88, 255),
		gdSPDefVtxN(71, 200, -71, 0, 0, 135, 135, 168, 255),
		gdSPDefVtxN(92, 0, -38, 0, 0, 175, 71, 86, 255),
		gdSPDefVtxN(92, 200, -38, 0, 0, 175, 71, 170, 255),
		gdSPDefVtxN(100, 0, 0, 0, 0, 190, 0, 87, 255),
		gdSPDefVtxN(100, 200, 0, 0, 0, 190, 0, 169, 255),
		gdSPDefVtxN(92, 0, 38, 0, 0, 175, -71, 86, 255),
		gdSPDefVtxN(92, 200, 38, 0, 0, 175, -71, 170, 255),
		gdSPDefVtxN(71, 0, 71, 0, 0, 135, 121, 88, 255),
		gdSPDefVtxN(71, 200, 71, 0, 0, 135, 121, 168, 255),
		gdSPDefVtxN(38, 0, 92, 0, 0, 71, 81, 86, 255),
		gdSPDefVtxN(38, 200, 92, 0, 0, 71, 81, 170, 255),
		gdSPDefVtxN(0, 0, 100, 0, 0, 0, 66, 87, 255),
		gdSPDefVtxN(0, 200, 100, 0, 0, 0, 66, 169, 255),
		gdSPDefVtxN(-38, 0, 92, 0, 0, -71, 81, 86, 255),
		gdSPDefVtxN(-38, 200, 92, 0, 0, -71, 81, 170, 255),
		gdSPDefVtxN(-71, 0, 71, 0, 0, 121, 121, 88, 255),
		gdSPDefVtxN(-71, 200, 71, 0, 0, 121, 121, 168, 255),
		gdSPDefVtxN(-92, 0, 38, 0, 0, 81, -71, 86, 255),
		gdSPDefVtxN(-92, 200, 38, 0, 0, 81, -71, 170, 255),
		gdSPDefVtxN(-100, 0, 0, 0, 0, 66, 0, 87, 255),
		gdSPDefVtxN(-100, 200, 0, 0, 0, 66, 0, 169, 255),
		gdSPDefVtxN(-92, 0, -38, 0, 0, 81, 71, 86, 255),
		gdSPDefVtxN(-92, 200, -38, 0, 0, 81, 71, 170, 255),
		gdSPDefVtxN(-71, 0, -71, 0, 0, 121, 135, 88, 255),
		gdSPDefVtxN(-71, 200, -71, 0, 0, 121, 135, 168, 255),
		gdSPDefVtxN(-38, 0, -92, 0, 0, -71, 175, 86, 255),
		gdSPDefVtxN(-38, 200, -92, 0, 0, -71, 175, 170, 255)
	};

	Gfx dlCylinder[] = {
		gsDPPipeSync(),
		gsSPSetGeometryMode(G_CULL_BACK | G_SHADING_SMOOTH),
		gsSPVertex(&vCylinder, 32, 0),
		gsSP2Triangles(0, 1, 2, 0, 3, 4, 1, 0),
		gsSP2Triangles(5, 6, 4, 0, 7, 8, 6, 0),
		gsSP2Triangles(9, 10, 8, 0, 11, 12, 10, 0),
		gsSP2Triangles(13, 14, 12, 0, 15, 16, 14, 0),
		gsSP2Triangles(17, 18, 16, 0, 19, 20, 18, 0),
		gsSP2Triangles(21, 22, 20, 0, 23, 24, 22, 0),
		gsSP2Triangles(25, 26, 24, 0, 27, 28, 26, 0),
		gsSP2Triangles(5, 29, 21, 0, 29, 30, 28, 0),
		gsSP2Triangles(31, 2, 30, 0, 6, 14, 22, 0),
		gsSP2Triangles(0, 3, 1, 0, 3, 5, 4, 0),
		gsSP2Triangles(5, 7, 6, 0, 7, 9, 8, 0),
		gsSP2Triangles(9, 11, 10, 0, 11, 13, 12, 0),
		gsSP2Triangles(13, 15, 14, 0, 15, 17, 16, 0),
		gsSP2Triangles(17, 19, 18, 0, 19, 21, 20, 0),
		gsSP2Triangles(21, 23, 22, 0, 23, 25, 24, 0),
		gsSP2Triangles(25, 27, 26, 0, 27, 29, 28, 0),
		gsSP2Triangles(5, 3, 0, 0, 0, 31, 29, 0),
		gsSP2Triangles(29, 27, 25, 0, 25, 23, 21, 0),
		gsSP2Triangles(21, 19, 17, 0, 17, 15, 13, 0),
		gsSP2Triangles(13, 11, 9, 0, 9, 7, 13, 0),
		gsSP2Triangles(7, 5, 13, 0, 5, 0, 29, 0),
		gsSP2Triangles(29, 25, 21, 0, 21, 17, 5, 0),
		gsSP2Triangles(17, 13, 5, 0, 29, 31, 30, 0),
		gsSP2Triangles(31, 0, 2, 0, 30, 2, 1, 0),
		gsSP2Triangles(1, 4, 30, 0, 4, 6, 30, 0),
		gsSP2Triangles(6, 8, 10, 0, 10, 12, 6, 0),
		gsSP2Triangles(12, 14, 6, 0, 14, 16, 22, 0),
		gsSP2Triangles(16, 18, 22, 0, 18, 20, 22, 0),
		gsSP2Triangles(22, 24, 26, 0, 26, 28, 30, 0),
		gsSP2Triangles(22, 26, 30, 0, 30, 6, 22, 0),
		gsSPClearGeometryMode(G_CULL_BACK | G_SHADING_SMOOTH),
		gsSPEndDisplayList()
	};

#endif

#endif