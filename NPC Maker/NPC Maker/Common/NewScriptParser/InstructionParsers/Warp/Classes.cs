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

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(VarType, Data);
            DataHelpers.AddObjectToByteList(WarpID, Data);
            DataHelpers.Ensure2ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 6);
            return Data.ToArray();
        }
    }
}

