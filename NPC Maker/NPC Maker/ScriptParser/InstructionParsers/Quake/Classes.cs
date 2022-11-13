using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionQuake : Instruction
    {
        ScriptVarVal Speed { get; set; }
        ScriptVarVal Type { get; set; }

        public InstructionQuake(ScriptVarVal _Speed, ScriptVarVal _QuakeType) : base((int)Lists.Instructions.QUAKE)
        {
            Speed = _Speed;
            Type = _QuakeType;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(Speed.Vartype, Data);
            Helpers.AddObjectToByteList(Type.Vartype, Data);
            Helpers.Ensure4ByteAlign(Data);

            Helpers.AddObjectToByteList(Speed.Value, Data);
            Helpers.AddObjectToByteList(Type.Value, Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }
    }
}

