﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionChangeScript : InstructionSub
    {
        public UInt16 NPCID;
        public InstructionLabel Start;

        public InstructionChangeScript(Byte SubID, UInt16 _NPCID, string Label) : base((byte)Lists.Instructions.CHANGE_SCRIPT, SubID)
        {
            NPCID = _NPCID;
            Start = new InstructionLabel(Label);
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(NPCID, Data);
            DataHelpers.AddObjectToByteList(Start.InstructionNumber, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }
}
