using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionScale : InstructionSub
    {
        ScriptVarVal ActorID { get; set; }
        ScriptVarVal Scale { get; set; }
        ScriptVarVal Speed { get; set; }
        public byte Target;

        public InstructionScale(byte _SubID, byte _Target, ScriptVarVal _ActorID, ScriptVarVal _Scale, ScriptVarVal _Speed)
                                : base((int)Lists.Instructions.SCALE, _SubID)
        {
            ActorID = _ActorID;
            Scale = _Scale;
            Target = _Target;
            Speed = _Speed;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(SubID, Target, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(ActorID.Vartype, Speed.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Scale.Vartype, 0, 4), Data);

            Helpers.AddObjectToByteList(ActorID.Value, Data);
            Helpers.AddObjectToByteList(Scale.Value, Data);
            Helpers.AddObjectToByteList(Speed.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 16);

            return Data.ToArray();
        }
    }
}

