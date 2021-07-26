using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionFace : Instruction
    {
        public byte Subject;
        public byte FaceType;
        public byte Target;

        public UInt32 SubjectActor;
        public byte SubjectActorT;

        public UInt32 TargetActor;
        public byte TargetActorT;

        public InstructionFace(byte _Subject, byte _FaceType, byte _Target,
                               UInt32 _SubjectActor, byte _SubjectActorT,
                               UInt32 _TargetActor, byte _TargetActorT) : base((byte)Lists.Instructions.FACE)
        {
            Subject = _Subject;
            FaceType = _FaceType;
            Target = _Target;
            SubjectActor = _SubjectActor;
            SubjectActorT = _SubjectActorT;
            TargetActor = _TargetActor;
            TargetActorT = _TargetActorT;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Target, Subject, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(SubjectActorT, TargetActorT, 4), Data);
            Helpers.AddObjectToByteList(FaceType, Data);

            Helpers.AddObjectToByteList(SubjectActor, Data);
            Helpers.AddObjectToByteList(TargetActor, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }
    }
}

