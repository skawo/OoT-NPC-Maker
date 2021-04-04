using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionWarp : Instruction
    {
        public UInt32 WarpID;
        public byte VarType;

        public InstructionWarp(UInt32 _WarpID, byte _VarType) : base((byte)Lists.Instructions.WARP)
        {
            WarpID = _WarpID;
            VarType = _VarType;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(VarType, Data);
            Helpers.AddObjectToByteList(WarpID, Data);
            Helpers.Ensure2ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 6);
            return Data.ToArray();
        }
    }
}

