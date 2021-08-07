using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker
{
    public static class Defaults
    {
        public static ScriptEntry DefaultDefines = new ScriptEntry()
        {
            Name = "Definitions",
            Text = 
$@"#define actor.posX actorf.0x24
#define actor.posY actorf.0x28 
#define actor.posZ actorf.0x2C 
#define actor.dirX actor16.0x30 
#define actor.dirY actor16.0x32 
#define actor.dirZ actor16.0x34 

#define actor.homePosX actorf.0x8 
#define actor.homePosY actorf.0xC 
#define actor.homePosZ actorf.0x10 
#define actor.homeDirX actor16.0x12 
#define actor.homeDirY actor16.0x14 
#define actor.homeDirZ actor16.0x16 
                
#define actor.scaleX actorf.0x50 
#define actor.scaleY actorf.0x54 
#define actor.scaleZ actorf.0x58 

#define actor.velocityX actorf.0x5C 
#define actor.velocityY actorf.0x60 
#define actor.velocityZ actorf.0x64 

#define actor.rotX actor16.0xB4 
#define actor.rotY actor16.0xB6 
#define actor.rotZ actor16.0xB8
"
        };

        public static ScriptEntry DefaultMacros = new ScriptEntry()
        {
            Name = "Macros",
            Text = 

$@"{Lists.Keyword_Procedure} basic_talk textbox
    {Lists.Instructions.TALK} textbox
        {Lists.Instructions.SET} {Lists.SetSubTypes.TALK_MODE} {Lists.Keyword_True}
        {Lists.Instructions.SET} {Lists.SetSubTypes.CURRENT_ANIMATION} 0
        {Lists.Instructions.FACE} {Lists.TargetActorSubtypes.SELF} {Lists.FaceSubtypes.TOWARDS} {Lists.TargetActorSubtypes.PLAYER}
        {Lists.Instructions.AWAIT} {Lists.AwaitSubTypes.TALKING_END}
        {Lists.Instructions.SET} {Lists.SetSubTypes.TALK_MODE} {Lists.Keyword_False}
    {Lists.Keyword_EndTalk}
{Lists.Keyword_EndProcedure}


{Lists.Keyword_Procedure} talk_with_anim textbox talk_animation
    {Lists.Instructions.TALK} textbox
        {Lists.Instructions.SET} {Lists.SetSubTypes.TALK_MODE} {Lists.Keyword_True}
        {Lists.Instructions.SET} {Lists.SetSubTypes.CURRENT_ANIMATION} talk_animation
        {Lists.Instructions.FACE} {Lists.TargetActorSubtypes.SELF} {Lists.FaceSubtypes.TOWARDS} {Lists.TargetActorSubtypes.PLAYER}
        {Lists.Instructions.AWAIT} {Lists.AwaitSubTypes.TALKING_END}
        {Lists.Instructions.SET} {Lists.SetSubTypes.CURRENT_ANIMATION} 0
        {Lists.Instructions.SET} {Lists.SetSubTypes.TALK_MODE} {Lists.Keyword_False}
    {Lists.Keyword_EndTalk}
{Lists.Keyword_EndProcedure}
".ToLower()


        };
    }
}
