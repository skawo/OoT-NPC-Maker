using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionChangeScript : InstructionSub
    {
        public InstructionLabel Start;
        public UInt32 NPCID;
        public UInt32 ScriptId;
        public byte ScriptIdVT;
        public byte NPCIdVT;

        public InstructionChangeScript(Byte SubID, UInt32 _NPCID, byte _NPCIdVT, UInt32 _ScriptId, byte _ScriptIdVT, string Label) : base((byte)Lists.Instructions.CHANGE_SCRIPT, SubID)
        {
            Start = new InstructionLabel(Label);
            NPCID = _NPCID;
            NPCIdVT = _NPCIdVT;
            ScriptId = _ScriptId;
            ScriptIdVT = _ScriptIdVT;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(ScriptIdVT, Data);
            DataHelpers.AddObjectToByteList(NPCIdVT, Data);
            DataHelpers.AddObjectToByteList(ScriptId, Data);
            DataHelpers.AddObjectToByteList(NPCID, Data);
            DataHelpers.AddObjectToByteList(Start.InstructionNumber, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 16);

            return Data.ToArray();
        }
    }
}

