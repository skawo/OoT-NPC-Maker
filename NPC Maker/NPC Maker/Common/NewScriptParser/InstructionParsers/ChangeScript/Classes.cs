using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionChangeScript : InstructionSub
    {
        public UInt16 NPCID;
        public InstructionLabel Start;
        public byte ScriptId;

        public InstructionChangeScript(Byte SubID, UInt16 _NPCID, byte _ScriptId, string Label) : base((byte)Lists.Instructions.CHANGE_SCRIPT, SubID)
        {
            NPCID = _NPCID;
            Start = new InstructionLabel(Label);
            ScriptId = _ScriptId;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(NPCID, Data);
            DataHelpers.AddObjectToByteList(ScriptId, Data);
            DataHelpers.AddObjectToByteList(Start.InstructionNumber, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }
}

