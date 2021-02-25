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

            ParserHelpers.AddObjectToByteList(ID, Data);
            ParserHelpers.AddObjectToByteList(WarpID, Data);
            ParserHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }
}

