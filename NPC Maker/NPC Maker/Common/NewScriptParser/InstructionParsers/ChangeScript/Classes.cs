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

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(ScriptIdVT, Data);
            Helpers.AddObjectToByteList(NPCIdVT, Data);
            Helpers.AddObjectToByteList(ScriptId, Data);
            Helpers.AddObjectToByteList(NPCID, Data);
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, Start, ref Data);

            Helpers.Ensure2ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 14);

            return Data.ToArray();
        }
    }
}

