#ifndef NPC_MAKER_TYPES_H
#define NPC_MAKER_TYPES_H

#include "npc_maker.h"
#include "scripts_types.h"
#include "npc_maker_defines.h"
#include "scripts_defines.h"


typedef struct NpcMaker NpcMaker;
typedef struct InternalMsgEntry InternalMsgEntry;
 
 #ifndef GetNPCMakerFunc
	typedef NpcMaker* GetNPCMakerFunc(NpcMaker* en, PlayState*playState, u16 ID);
#endif

#ifndef GetPathPtrFunc
	typedef Path* GetPathPtrFunc(PlayState* playState, s16 path_id);
#endif

#ifndef GetActorFunc
	typedef Actor* GetActorFunc(int ID, PlayState* playState, Actor* closestTo, Actor* skip);
#endif

#ifndef GetCutscenePtrFunc
	typedef u32* GetCutscenePtrFunc(PlayState* playState, int setup_index);
#endif

#ifndef GetSceneHeaderFunc
	typedef u32* GetSceneHeaderFunc(PlayState* playState, int setup_index);
#endif

#ifndef GetInternalMessageFunc
	typedef void GetInternalMessageFunc(NpcMaker* en, PlayState* playState, s16 msgId, void* buffer);
#endif

#ifndef GetInternalMessagePtrFunc
	typedef void* GetInternalMessagePtrFunc(NpcMaker* en, PlayState* playState, s16 msgId);
#endif

#ifndef GetInternalMessageDataFunc
	typedef InternalMsgEntry GetInternalMessageDataFunc(NpcMaker* en, PlayState* playState, int ID);
#endif
 
typedef struct NpcSettings
{
    u8 cutsceneId;
    u8 headLimb;
    u8 waistLimb;
    u8 targetLimb;
    u8 pathId;
    u8 blinkAnimSpeed;
    u8 talkAnimSpeed;
    u8 drawType;
    u8 talkTexSegment;
    u8 blinkTexSegment;
    u8 animationType;
    u8 movementType;
    u8 waistHorAxis;
    u8 waistVertAxis;
    u8 headHorAxis;
    u8 headVertAxis;
    u8 lookAtType;
    u8 targetDistance;
    u8 effectIfAttacked;
    u8 mass;
    u8 alpha;
    s8 lightLimb;
    Color_RGB8 envColor;
    Color_RGB8 lightColor;
    u8 animInterpFrames;
    u8 pad[3];
    
    u8 hasCollision;
    u8 pushesSwitches;
    u8 ignorePathYAxis;
    u8 alwaysActive;
    u8 alwaysDrawn;
    u8 execJustScript;
    u8 reactsToAttacks;
    u8 opensDoors;

    u8 castsShadow;
    u8 isTargettable;
    u8 loopPath;
    u8 usesEnvColor;
    u8 fadeOut;
    u8 generatesLight;
    u8 lightGlow;
    u8 showColsDebugOn;

    u8 visibleWithLens;
    u8 invisible;
    u8 existsInAllRooms;
    u8 numVars;

    u8 numFVars;
    u8 showDlistEditorDebugOn;
    u8 showLookAtEditorDebugOn;
    u8 printToScreenDebugOn;

    u16 objectId;
    u16 lookAtDegreesVert;
    u16 lookAtDegreesHor;
    u16 collisionRadius;
    u16 collisionHeight;
    s16 collisionyShift;
    u16 shadowRadius;
    u16 movementDistance;
    u16 maxRoamDist;
    s16 pathLoopStartNode;
    s16 pathLoopEndNode;
    u16 movementDelay;
    u16 timedPathStart;
    u16 timedPathEnd;
    s16 sfxIfAttacked;
    s16 riddenNPCId;
    u16 lightRadius;
    Vec3s targetPosOffset;
    Vec3s modelPosOffset;
    Vec3s lightPosOffset;

    float modelScale;
    float talkRadius;
    float movementSpeed;
    float gravity;
    float smoothingConstant;
    u32 skeleton;
    s32 fileStart;
    float UncullFwd;
    float UncullDwn;
    float UncullScale;
    Vec3f lookAtPosOffset;

    u8 blinkPattern[4];
    u8 talkPattern[4];

} NpcSettings;    

typedef struct RomSection
{
    u32 Start;
    u32 End;
} RomSection;

typedef struct MessageEntry
{
    s16 msg_id;
    u8 settings;
    u8 pad;
    u8 segment;
    u32 offset : 24;
} MessageEntry;

typedef struct InternalMsgEntry
{
    u32 offset;
    u8 posType;
    u8 pad;
    u16 msgLen;

} InternalMsgEntry;

typedef struct NpcAnimationEntry
{
    u32 offset;
    s32 fileStart;
    float speed;
    s16 objectId;
    u8 startFrame;
    u8 endFrame;

} NpcAnimationEntry;

typedef struct ExDListEntry
{
    u32 offset;
    s32 fileStart;
    Vec3f translation;
    float scale;
    s16 objectId;
    Vec3s rotation;
    s16 limb;
    u8 showType;
    Color_RGB8 envColor;

} ExDListEntry;

typedef struct ColorEntry
{
    u8 limb;
    Color_RGB8 color;

} ColorEntry;

typedef struct ExSegDataEntry
{
    u32 offset;
    s32 fileStart;
    s16 objectId;
    s16 __pad__;
} ExSegDataEntry;

typedef struct ReadableTime
{
    u8 hour;
    u8 minutes;
} ReadableTime;

typedef struct SectionLoad
{
    u32* allocDest;
    u16* entriesNumberOut;
    u32 entrySize;
    u32 nullBlockSize;
    u8 noCopy;
} SectionLoad;

typedef struct NpcMaker
{
    Actor actor;
    NpcSettings settings;

    u16 npcId;
    s16 currentAnimId;
    s16 cameraId;

    u16 numAnims;
    u16 numExDLists;
    u16 numExColors; 
    u16 exSegDataBlSize;

    u16 lastDayTime;
    u16 curAlpha;
    s16 curPathNode;
    s16 limbRotA;
    s16 limbRotB;

    u8 segmentDataIds[0x7];
    u16 blinkTimer;
    
    u8 currentBlinkFrame;
    u8 currentTalkFrame;
    u8 blinkingFramesBetween;
    u8 talkingFramesBetween;

    u8 movementDelayCounter;
    u8 roamMovementDelay;
    u8 curPathNumNodes;
    u8 wasHitTimer;
    u8 tradeItem;
    u8 canTrade;
    u8 canTalk;
    u8 stopPlayer;
    
    u8 isTalking;
    u8 persistTalk;
    u8 textboxDisplayed;
    u8 talkingFinished;
    u8 doBlinkingAnm;
    u8 doTalkingAnm;
    u8 autoAnims;
    u8 isWaitingForResponse;
    s8 textboxNum;
    u8 canMove;
    u8 isMoving;
    u8 stopped;
    u8 pauseCutscene;
    u8 animationFinished;
    u8 wasHit;
    u8 wasHitThisFrame;
    u8 listeningToSong;
    u8 correctSongHeard;
    u8 pickedUpState;
    u8 hadCollision;
    u8 hasStaticExDlists;
    u8 getSettingsFromRAMObject;

    Color_RGB8 curColor;

    Vec3f movementStartPos;
    Vec3f movementNextPos;
    float currentDistToNextPos;
    float distanceTotal;
    float traversedDistance;
    float lastTraversedDistance;
    float cutsceneMovementSpeed;

    s32* scriptVars;
    float* scriptFVars;

    Skin skin;
    ColliderCylinder collider;
    LightInfo light;
    LightNode* lightNode;

    NpcAnimationEntry* animations;
    ExDListEntry* extraDLists;
    ColorEntry* dListColors;
    u32* exSegData;
    ScriptsHeader* scripts;
    ScriptInstance* scriptInstances;
    u32 messagesDataOffset;
    s32 customMsgId;
    u32 flags_internal;

    NpcMaker* riddenNpc;
    Actor* refActor;
    MessageEntry* dummyMesEntry;
    u16 curTextBuffPos;

    u32 CFuncs[6];
    u8 CFuncsWhen[8];    
    u8* embeddedOverlay;
    u8 spawnTimer;

    float scriptVolTemp;
    float scriptPitchTemp;
    s8 scriptReverbTemp;
    Vec3f scriptSfxTempPos;

    GetNPCMakerFunc* FindNPCMakerFunction;
    GetActorFunc* FindActorFunction;
    GetCutscenePtrFunc* FindCutscenePtrFunction;
    GetSceneHeaderFunc* FindSceneHeaderFunction;
	GetPathPtrFunc* FindPathPtrFunction;
	GetInternalMessageFunc* GetInternalMsgFunc;
	GetInternalMessagePtrFunc* GetInternalMsgPtrFunc;
    GetInternalMessageDataFunc* GetInternalMsgDataPtrFunc;
    
    u32 numLanguages;
    u32 numMessages;    
    
    #if DEBUG_STRUCT == 1
        s32 dbgVar;
        s32 dbgVar2;
        float fDbgVar;
        float fDbgVar2;
        u8 dgbDrawVersion;
        u8 dbgUnused;
        u8 dbgPosEditorCooldown;
        u8 dbgPosEditorCursorPos;
        u8 dbgPosEditorCurEditing;
        u8 curScriptNum;
    #endif

} NpcMaker;

#endif