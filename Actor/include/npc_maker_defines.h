#ifndef NPC_MAKER_DEFINES_H
#define NPC_MAKER_DEFINES_H

#define DUMMY_MSG_DATA 0x30313161
#define DUMMY_MESSAGE 0x011A
#define NO_CUSTOM_MESSAGE -1 

#define GlobalContext PlayState
#define PSkinAwb Skin

#define ROT16(R16A0) (182.044444 * (R16A0))
#define AVAL(base,type,offset)  (*(type*)((u8*)(base)+(offset)))
#define AADDR(a,o)  ((void*)((u8*)(a)+(o)))

#define IS_MASK(item) item >= ITEM_MASK_BUNNY && item <= ITEM_MASK_TRUTH
#define IS_BOTTLE_ITEM(item) item >= ITEM_POTION_RED && item <= ITEM_POE

#define NUM_USER_VARIABLES 10

#define PUSH_SWITCHES_MASK 0x04000000
#define TARGETTABLE_MASK 0x00000001
#define ALWAYS_ACTIVE_MASK 0x00000010
#define ALWAYS_DRAWN_MASK 0x00000020
#define DRAWN_WITH_LENS_MASK 0x00000080
#define PLAYER_STOPPED_MASK 0x20000000
#define NO_LIGHT_BIND (1 << 22)

#define NULL_ANIM_BLOCK_SIZE 0
#define NULL_EXDLIST_BLOCK_SIZE 0
#define NULL_SCRIPTS_BLOCK_SIZE 0
#define NULL_EX_COLORS_BLOCK_SIZE 0
#define NULL_SEG_BLOCK_SIZE 32

#define _ZQDL(ZQDL_A0, ZQDL_A1) ZQDL_A0->state.gfxCtx->ZQDL_A1
#define POLY_OPA _ZQDL(playState, polyOpa)
#define POLY_XLU _ZQDL(playState, polyXlu)
#define POLY_OVERLAY _ZQDL(playState, overlay)

#define DRAW_TYPE(h_type) ((h_type % 2 && h_type != SKIN) ? XLU : OPA)
#define DRAW_DEST(h_type) (DRAW_TYPE(h_type) == XLU ? &POLY_XLU : &POLY_OPA)

#define FADE_OUT_DISTANCE 500.0f
#define SHADOW_ALPHA 160	// must be divisible by SHADOW_ALPHA_UPDATE
#define SHADOW_ALPHA_UPDATE 5

#define FADE_OUT_FADE_IN_SPEED_MAX 10
#define FADE_OUT_FADE_IN_SPEED_MIN 1
#define FADE_OUT_FADE_IN_SCALE 2

#define OBJECT_CURRENT -1
#define OBJECT_RAM -2 
#define OBJECT_NONE -3
#define OBJECT_XLUDLIST -4
#define OBJECT_ENDDLIST -5

#define STATIC_EXDLIST_RELATIVE -1
#define STATIC_EXDLIST_ABSOLUTE -2 
#define STATIC_EXDLIST_AT_CAM -3 
#define STATIC_EXDLIST_AT_DISPLAY -4 
#define STATIC_EXDLIST_ORTHOGRAPHIC -5
#define STATIC_EXDLIST_ORTHOGRAPHIC_WIDE -6

#define SEG_OFFSET(seg) (0x01000000 * seg)
#define OFFSET_ADDRESS(segment, offset) offset >= SEG_OFFSET(segment) ? offset : offset + SEG_OFFSET(segment)

#define STOPPED_NODE -2
#define INVALID_NODE -1
#define INVALID_PATH 0
#define START_NODE(en) (en->settings.pathLoopStartNode >= 0 ? en->settings.pathLoopStartNode : 0)
#define END_NODE(en) (en->settings.pathLoopEndNode >= 0 ? en->settings.pathLoopEndNode : en->curPathNumNodes - 1)

#define PATH_ID(en) (en->settings.pathId - 1) 
#define CUTSCENE_ID(en) (en->settings.cutsceneId - 1)
#define R_OBJECT(en, obj) obj == OBJECT_CURRENT ? en->settings.objectId : obj
#define R_FILESTART(en, fS) fS == OBJECT_CURRENT ? en->settings.fileStart : fS
#define R_CUSTOM_MSG_ID(id) (id - 32768)

#define MAX_BLINK_FRAME 4
#define MAX_TALK_FRAME 4
#define RANDOM_BLINK_MIN_FRAMES 30
#define RANDOM_BLINK_MAX_FRAMES 80
#define BLINK_FRAME_BLANK 0xFF
#define TALK_FRAME_BLANK 0xFF

#define WAS_HIT_DELAY_BEFORE_RETURNING_TO_NORMAL 20

#define LOOKAT_AREA_HORIZ_MULTIPLIER 7.0f
#define LOOKAT_AREA_VERT_MULTIPLIER 8.0f
#define LOOKAT_HEAD_WAIST_MULTIPIER 0.5f
#define LOOKAT_WAIST_HORIZ_MULTIPIER 0.5f
#define LOOKAT_WAIST_VERT_MULTIPIER 0.3f

#define MOVEMENT_DISTANCE_EQUAL_MARGIN 5.0f
#define LINK_RUNAWAY_RADIUS_MULTIPLIER 3.0f

#define MINIMUM_RANDOM_DELAY 30

#define LOOKAT_ROTATION_MAX 8000
#define LOOKAT_ROTATION_MIN 1000
#define LOOKAT_ROTATION_SCALE 5

#define MOVEMENT_ROTATION_MAX 16000
#define MOVEMENT_ROTATION_MIN 400
#define MOVEMENT_ROTATION_SCALE 3

#define MOVEMENT_SMOOTHEN_ROTATION_MAX 16000
#define MOVEMENT_SMOOTHEN_ROTATION_MIN 400
#define MOVEMENT_SMOOTHEN_ROTATION_SCALE 3

#define SCALE_SMOOTH_MAX 0.001f
#define SCALE_SMOOTH_MIN 0.0005f
#define SCALE_SMOOTH_SCALE 2.0f

#define DOOR_OPEN_DIST_MARGIN 80.0f
#define DOOR_OPEN_SHUT_SPEED 3000
#define DOOR_OPEN_DEST_ROT 0x3FFF
#define DOOR_FACING_ROT_OFFSET 0x3FFF
#define SLIDE_DOOR_OPEN_SHUT_SPEED 15.0f
#define SLIDE_DOOR_OPEN_DIST 200.0f

#define MOVEMENT_SPEED_DEFAULT 1.0f

#define MORNING_TIME 0x4555
#define NIGHT_TIME 0xC001

#define ONE_HEART 0x10

#define MIN_THROW_VELOCITY 3
#define MAX_THROW_VELOCITY 12

#ifndef MAX
    #define MAX(a, b)               ((a) > (b) ? (a) : (b))
#endif

#ifndef MIN
    #define MIN(a, b)               ((a) < (b) ? (a) : (b))
#endif

extern void is64Printf(const char* fmt, ...);
    #if GAME_VERSION == 0
        asm("is64Printf = osSyncPrintf");
    #else
        //is64Printf does not really work in 1.0. Substitute your own!          
    #endif	

#ifndef CIRCLE_SHADOW
    #if GAME_VERSION == 0
        #define CIRCLE_SHADOW 0x04049210
    #else
        #define CIRCLE_SHADOW 0x0404E740
    #endif
#endif
   

typedef enum item_award_upgrades
{
	UPGRADE_MAGIC = 0xF0,
	UPGRADE_DOUBLE_MAGIC = 0xF1,
	UPGRADE_DOUBLE_DEFENCE = 0xF2,
} item_award_upgrades;

typedef enum cfunc_update_when
{
	BEFORE_SCRIPTS = 0,
	INSTEAD_OF_SCRIPTS = 1,
	AFTER_SCRIPTS = 2,
	REPLACE_UPDATE = 3,
} cfunc_update_when;

typedef enum cfunc_draw_when
{
	BEFORE_MODEL = 0,
	AFTER_MODEL = 1,
	REPLACE_DRAW = 2,
} cfunc_draw_when;

typedef enum dlist_visibility
{
	NOT_VISIBLE = 0,
	WITH_LIMB = 1,
	INSTEAD_OF_LIMB = 2,
    IN_SKELETON = 3,
	CONTROL_EXISTING = 4,
} dlist_visibility;

typedef enum axes
{
	PLUS_X = 0,
	MINUS_X = 1,
	PLUS_Y = 2,
	MINUS_Y = 3,
	PLUS_Z = 4,
	MINUS_Z = 5,
} axes;

typedef enum look_types
{
	LOOK_NONE = 0,
	LOOK_BODY = 1,
	LOOK_HEAD = 2,
	LOOK_WAIST = 3,
	LOOK_BOTH = 4,
} look_type;

typedef enum picked_up_state
{
	STATE_IDLE = 0,
    STATE_PICKED_UP = 1,
    STATE_THROWN = 2,
    STATE_LANDED = 3,
} picked_up_state;

// There are more, but their exact purpose is not known.
typedef enum song_status
{
	SONGSTATUS_NONE = 0, 
	SONGSTATUS_PLAYING = 1,
    SONGSTATUS_WARP = 2,
    SONGSTATUS_CORRECT = 3,
	SONGSTATUS_CANCELED = 4,
} song_status;

typedef enum anim_ids
{
	ANIM_IDLE = 0,
	ANIM_WALK = 1,
	ANIM_ATTACKED = 2,
} anim_ids;

typedef enum hierarchy_types
{
	OPA_MATRIX = 0,
	XLU_MATRIX = 1,
	OPA_NONMATRIX = 2,
	XLU_NONMATRIX = 3,
	SKIN = 4,
} hierarchy_type;

typedef enum draw_type
{
	OPA = 0,
	XLU = 1,
} draw_type;

typedef enum animation_type
{
	ANIMTYPE_NORMAL = 0,
	ANIMTYPE_LINK = 1,
} animation_type;

typedef enum movement_type
{
	MOVEMENT_NONE = 0,
	MOVEMENT_ROAM = 1,
	MOVEMENT_FOLLOW = 2,
	MOVEMENT_RUN_AWAY = 3,
	MOVEMENT_PATH = 4,
	MOVEMENT_TIMED_PATH = 5,
	MOVEMENT_CUTSCENE = 6,
    MOVEMENT_MISC = 7,
} movement_type;

#endif 