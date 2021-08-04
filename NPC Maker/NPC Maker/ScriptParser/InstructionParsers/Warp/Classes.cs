using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionWarp : Instruction
    {
        public object WarpID;
        public byte VarType;

        public InstructionWarp(object _WarpID, byte _VarType) : base((byte)Lists.Instructions.WARP)
        {
            WarpID = _WarpID;
            VarType = _VarType;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(VarType, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(WarpID, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 8);
            return Data.ToArray();
        }
    }
}

