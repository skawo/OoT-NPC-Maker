using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionKill : Instruction
    {
        public UInt16 ActorID;
        public UInt16 ActorSub;

        public InstructionKill(byte _SubID, UInt16 _ActorID, UInt16 _ActorSub) : base((byte)Lists.Instructions.KILL)
        {
            ActorID = _ActorID;
            ActorSub = _ActorSub;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            ParserHelpers.AddObjectToByteList(ID, Data);
            ParserHelpers.AddObjectToByteList(ActorID, Data);
            ParserHelpers.AddObjectToByteList(ActorSub, Data);
            ParserHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }
}

