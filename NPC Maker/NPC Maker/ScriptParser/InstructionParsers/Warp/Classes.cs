using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionWarp : Instruction
    {
        ScriptVarVal WarpID { get; set; }
        ScriptVarVal CutsceneIndex { get; set; }
        ScriptVarVal SceneLoadFlag { get; set; }
        public InstructionWarp(ScriptVarVal _WarpID, ScriptVarVal _CutsceneIndex, ScriptVarVal _SceneLoadFlag) : base((byte)Lists.Instructions.WARP)
        {
            WarpID = _WarpID;
            CutsceneIndex = _CutsceneIndex;
            SceneLoadFlag = _SceneLoadFlag;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(WarpID.Vartype, Data);
            Helpers.AddObjectToByteList(CutsceneIndex.Vartype, Data);
            Helpers.AddObjectToByteList(SceneLoadFlag.Vartype, Data);
            Helpers.AddObjectToByteList(WarpID.Value, Data);
            Helpers.AddObjectToByteList(CutsceneIndex.Value, Data);
            Helpers.AddObjectToByteList(SceneLoadFlag.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 16);
            return Data.ToArray();
        }
    }
}

