#ifndef SCRIPTS_TYPES_H
#define SCRIPTS_TYPES_H

#include "npc_maker_types.h"
#include "scripts_defines.h"

typedef struct ScriptsHeader
{
    u32 numScripts;
    u32* scriptDataOffsets[];
} ScriptsHeader;

typedef struct ScriptInstance
{
    u32* scriptPtr;
    void* instrUsingTempValuesPtr;
    float fTempValues[NUM_SCRIPT_TEMP_VARS];
    s32 tempValues[NUM_SCRIPT_TEMP_VARS];
    void* PlayerUpdate;

    s16 responsesInstrNum;
    s16 jumpToWhenReponded;
    s16 jumpToWhenSpottedInstrNum;
    s16 spotted;

    u16 waitTimer;
    u16 curInstrNum;
    u16 startInstrNum;
    u16 padding;
    
} ScriptInstance;

typedef struct ScrInstr
{
    u8 id;
    u8 pad[3];
} ScrInstr;

typedef struct ScrInstrSub
{
    u8 id;
    u8 subId;
    u8 pad[2];
} ScrInstrSub;

typedef union ScriptVarval
{
    u32 ui32;
    s32 i32;
    float flo;    
} ScriptVarval;

typedef union CommonType
{
    u8 ui8;
    s8 i8;
    u16 ui16;
    s16 i16;
    u32 ui32;
    s32 i32;
    float flo;
} CommonType;

#pragma region set

typedef struct ScrInstrSet
{
    u8 id;
    u8 subId;
    u8 varType;
    u8 operator;
    ScriptVarval value;
} ScrInstrSet;

typedef struct ScrInstrDoubleSet
{
    u8 id;
    u8 subId;
    u8 operator;
    u8 varType1 : 4;
    u8 varType2 : 4;
    ScriptVarval value1;
    ScriptVarval value2;
} ScrInstrDoubleSet;

typedef struct ScrInstrSetPlayerAnim
{
    u8 id;
    u8 subId;
    u8 offsetType;
    u8 speedType;
    u8 startFrameType;
    u8 endFrameType;
    u8 once;
    u8 __pad__;

    ScriptVarval offset;
    ScriptVarval speed;
    ScriptVarval startFrame;
    ScriptVarval endFrame;
} ScrInstrSetPlayerAnim;

typedef struct ScrInstrColorSet
{
    u8 id;
    u8 subId;
    u8 varTypeR : 4;
    u8 varTypeG : 4;
    u8 varTypeB;
    ScriptVarval R;
    ScriptVarval G;
    ScriptVarval B;
} ScrInstrColorSet;

typedef struct ScrInstrResponsesSet
{
    u8 id;
    u8 subId;
    u16 resp1InstrNum;
    u16 resp2InstrNum;
    u16 resp3InstrNum;
} ScrInstrResponsesSet;

typedef struct ScrInstrStartSet
{
    u8 id;
    u8 subId;
    u16 instrNum;
} ScrInstrStartSet;

typedef struct ScrInstrPatternSet
{
    u8 id;
    u8 sub_id;
    u16 pad;
    u8 pattern[4];
} ScrInstrPatternSet;

typedef struct ScrInstrActorSet
{
    u8 id;
    u8 subId;
    u8 target;
    u8 actorNumType;
    ScriptVarval actorNum;
} ScrInstrActorSet;

typedef struct ScrInstrExtVarSet
{
    u8 id;
    u8 subId;
    u8 operator;
    u8 varType;
    ScriptVarval value;
    ScriptVarval actorNum;
    u8 actorNumVarType;
    u8 extVarNum;
} ScrInstrExtVarSet;

#pragma endregion

#pragma region goto

typedef struct ScrInstrGoto
{
    u8 id;
    u8 pad;
    u16 instrNum;
} ScrInstrGoto;


#pragma endregion

#pragma region if

typedef struct ScrInstrIf
{
    u8 id;
    u8 subId;
    u8 vartype;
    u8 condition;
    ScriptVarval value;
    u16 trueInstrNum;
    u16 falseInstrNum;

} ScrInstrIf;

typedef struct ScrInstrDoubleIf
{
    u8 id;
    u8 subId;
    u8 varType1 : 4;
    u8 varType2 : 4;
    u8 condition;
    ScriptVarval value1;
    ScriptVarval value2;
    u16 trueInstrNum;
    u16 falseInstrNum;

} ScrInstrDoubleIf;

typedef struct ScrInstrExtVarIf
{
    u8 id;
    u8 subId;
    u8 varType : 4;
    u8 actorNumVarType : 4;
    u8 condition : 4;
    u8 extVarNum : 4; 
    ScriptVarval value;
    ScriptVarval actorNum;
    u16 trueInstrNum;
    u16 falseInstrNum;

} ScrInstrExtVarIf;


#pragma endregion

#pragma region await

typedef struct ScrInstrAwait
{
    u8 id;
    u8 subId;
    u8 varType;
    u8 condition;
    ScriptVarval value;

} ScrInstrAwait;

typedef struct ScrInstrDoubleAwait
{
    u8 id;
    u8 subId;
    u8 varType : 4;
    u8 varType2 : 4;
    u8 condition;
    ScriptVarval value;
    ScriptVarval value2;

} ScrInstrDoubleAwait;

typedef struct ScrInstrExtVarAwait
{
    u8 id;
    u8 subId;
    u8 varType : 4;
    u8 actorNumVarType : 4;
    u8 condition : 4;
    u8 extVarNum : 4;
    ScriptVarval value;
    ScriptVarval actorNum;

} ScrInstrExtVarAwait;



#pragma endregion

#pragma region talk

typedef struct ScrInstrTextbox
{
    u8 id;
    u8 varTypeAdult;
    u8 vartypeChild;
    u8 pad;
    ScriptVarval adultMsgId;
    ScriptVarval childMsgId;

} ScrInstrTextbox;


#pragma endregion

#pragma region trade

typedef struct TradeSetting
{
    ScriptVarval item;
    ScriptVarval adultMsgId;
    ScriptVarval childMsgId;
    u8 varTypeItem;
    u8 varTypeAdult;
    u8 varTypeChild;
    u8 pad;

} TradeSetting;

typedef struct ScrInstrTrade
{
    u8 id;
    u8 varTypeTalkAdult : 4;
    u8 varTypeTalkChild : 4;
    u16 failureDefsNumber;
    ScriptVarval adultTalkMsgId;
    ScriptVarval childTalkMsgId;

    TradeSetting correct;
    TradeSetting failure[];

} ScrInstrTrade;


#pragma endregion

#pragma region face

typedef struct ScrInstrFace
{
    u8 id;
    u8 target : 4;
    u8 subject : 4;
    u8 subjectActorNumType : 4;
    u8 targetActorNumType : 4;   
    u8 faceType;
    ScriptVarval subjectActorNum;
    ScriptVarval targetActorNum;

} ScrInstrFace;


#pragma endregion

#pragma region rotation

typedef struct ScrInstrRotation
{
    u8 id;
    u8 subId;
    u8 target;
    u8 speedType;
    
    ScriptVarval x;
    ScriptVarval y;
    ScriptVarval z;
    ScriptVarval actorNum;
    ScriptVarval speed;

    u8 xType;
    u8 yType;
    u8 zType;
    u8 actorNumType;

} ScrInstrRotation;


#pragma endregion

#pragma region position

typedef struct ScrInstrPosition
{
    u8 id;
    u8 subId : 4;
    u8 speedType : 4;
    u8 ignoreY : 4;
    u8 target : 4;
    u8 actorNumType;
    
    ScriptVarval x;
    ScriptVarval y;
    ScriptVarval z;
    ScriptVarval actorNum;
    ScriptVarval speed;

    u8 xType : 4;
    u8 yType : 4;
    u8 zType : 4;
    u8 __pad__ : 4;

} ScrInstrPosition;


#pragma endregion

#pragma region scale

typedef struct ScrInstrScale
{
    u8 id;
    u8 subId : 4;
    u8 target : 4;
    u8 actorNumType : 4;
    u8 speed_type : 4;
    u8 scale_type : 4;
    u8 __pad__ : 4;

    ScriptVarval actorNum;
    ScriptVarval scale;
    ScriptVarval speed;

} ScrInstrScale;


#pragma endregion

#pragma region play

typedef struct ScrInstrPlay
{
    u8 id;
    u8 subId;
    u8 varType;
    u8 pad;
    ScriptVarval value;
} ScrInstrPlay;


#pragma endregion

#pragma region kill

typedef struct ScrInstrKill
{
    u8 id;
    u8 subId;
    u8 actorNumType;
    u8 __pad__;
    ScriptVarval actorNum;
} ScrInstrKill;


#pragma endregion

#pragma region ocarina

typedef struct ScrInstrOcarina
{
    u8 id;
    u8 ocaSongType;
    u16 pad;
    ScriptVarval ocaSong;
    u16 trueInstrNum;
    u16 falseInstrNum;
} ScrInstrOcarina;


#pragma endregion

#pragma region spawn

typedef struct ScrInstrSpawn
{
    u8 id;
    u8 posType;

    u8 posXType : 4;
    u8 posYType : 4;
    u8 posZType : 4;
    u8 rotXType : 4;
    u8 actorNumType : 4;
    u8 actorParamType : 4;
    u8 rotYType : 4;
    u8 rotZType : 4;

    u16 pad;

    ScriptVarval actorNum;
    ScriptVarval actorParam;

    ScriptVarval posX;
    ScriptVarval posY;
    ScriptVarval posZ;
    ScriptVarval rotX;
    ScriptVarval rotY;
    ScriptVarval rotZ;
} ScrInstrSpawn;


#pragma endregion

#pragma region item

typedef struct ScrInstrItem
{
    u8 id;
    u8 subId;
    u8 itemVarType;
    u8 pad;
    ScriptVarval item;
} ScrInstrItem;


#pragma endregion

#pragma region warp

typedef struct ScrInstrWarp
{
    u8 id;
    u8 varType;
    u8 pad[2];
    ScriptVarval warpId;
} ScrInstrWarp;


#pragma endregion

#pragma region particle

typedef struct ScrInstrParticle
{
    u8 id;
    u8 type;

    u16 foundInstrNum;

    u8 lifeType : 4;
    u8 varType : 4;
    u8 yawType : 4;
    u8 dListType : 4;
    u8 posXType : 4;
    u8 posYType : 4;
    u8 posZType : 4;
    u8 accelXType : 4;
    u8 accelYType : 4;
    u8 accelZType : 4;
    u8 velXType : 4;
    u8 velYType : 4;
    u8 velZType : 4;
    u8 posType : 4;
    u8 scaleType : 4;
    u8 scaleUpdType : 4;
    u8 primRType : 4;
    u8 primGType : 4;
    u8 primBType : 4;
    u8 primAType : 4;
    u8 envRType : 4;
    u8 envGType : 4;
    u8 envBType : 4;
    u8 envAType : 4;

    ScriptVarval posX;
    ScriptVarval posY;
    ScriptVarval posZ;
    ScriptVarval accelX;
    ScriptVarval accelY;
    ScriptVarval accelZ;
    ScriptVarval velX;
    ScriptVarval velY;
    ScriptVarval velZ;
    
    ScriptVarval primR;
    ScriptVarval primG;
    ScriptVarval primB;
    ScriptVarval primA;

    ScriptVarval envR;
    ScriptVarval envG;
    ScriptVarval envB;
    ScriptVarval envA;

    ScriptVarval scale;
    ScriptVarval scaleUpdate;

    ScriptVarval life;
    ScriptVarval var;
    ScriptVarval yaw;
    ScriptVarval dList;

} ScrInstrParticle;


#pragma endregion


#endif