using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionFace : Instruction
    {
        public byte Subject;
        public byte FaceType;
        public byte Target;

        public ScriptVarVal SubjectActor;
        public ScriptVarVal TargetActor;

        public InstructionFace(byte _Subject, byte _FaceType, byte _Target, ScriptVarVal _SubjectActor, ScriptVarVal _TargetActor) : base((byte)Lists.Instructions.FACE)
        {
            Subject = _Subject;
            FaceType = _FaceType;
            Target = _Target;
            SubjectActor = _SubjectActor;
            TargetActor = _TargetActor;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Target, Subject, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(SubjectActor.Vartype, TargetActor.Vartype, 4), Data);
            Helpers.AddObjectToByteList(FaceType, Data);

            Helpers.AddObjectToByteList(SubjectActor.Value, Data);
            Helpers.AddObjectToByteList(TargetActor.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }
    }
}

