using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionWarp : Instruction
    {
        public UInt16 WarpID;

        public InstructionWarp(UInt16 _WarpID) : base((byte)Lists.Instructions.WARP)
        {
            WarpID = _WarpID;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(WarpID, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }
}

