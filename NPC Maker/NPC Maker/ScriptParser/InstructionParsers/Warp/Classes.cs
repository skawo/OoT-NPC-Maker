using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionWarp : Instruction
    {
        ScriptVarVal WarpID { get; set; }
        ScriptVarVal CutsceneIndex { get; set; }

        public InstructionWarp(ScriptVarVal _WarpID, ScriptVarVal _CutsceneIndex) : base((byte)Lists.Instructions.WARP)
        {
            WarpID = _WarpID;
            CutsceneIndex = _CutsceneIndex;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(WarpID.Vartype, Data);
            Helpers.AddObjectToByteList(CutsceneIndex.Vartype, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(WarpID.Value, Data);
            Helpers.AddObjectToByteList(CutsceneIndex.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);
            return Data.ToArray();
        }
    }
}

