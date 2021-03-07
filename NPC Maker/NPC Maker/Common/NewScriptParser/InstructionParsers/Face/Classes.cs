using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionFace : Instruction
    {
        public byte Subject;
        public byte FaceType;
        public byte Target;

        public UInt32 SubjectActor;
        public Int32 SubjectActorCat;
        public byte SubjectActorT;
        public byte SubjectActorCatT;

        public UInt32 TargetActor;
        public Int32 TargetActorCat;
        public byte TargetActorT;
        public byte TargetActorCatT;

        public InstructionFace(byte _Subject, byte _FaceType, byte _Target, 
                               UInt32 _SubjectActor, Int32 _SubjectActorCat, byte _SubjectActorT, byte _SubjectActorCatT,
                               UInt32 _TargetActor, Int32 _TargetActorCat, byte _TargetActorT, byte _TargetActorCatT) : base((byte)Lists.Instructions.FACE)
        {
            Subject = _Subject;
            FaceType = _FaceType;
            Target = _Target;
            SubjectActor = _SubjectActor;
            SubjectActorCat = _SubjectActorCat;
            SubjectActorT = _SubjectActorT;
            SubjectActorCatT = _SubjectActorCatT;
            TargetActor = _TargetActor;
            TargetActorCat = _TargetActorCat;
            TargetActorCatT = _TargetActorCatT;
            TargetActorT = _TargetActorT;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(Subject, Target, 4), Data);
            DataHelpers.AddObjectToByteList(FaceType, Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(SubjectActorT, SubjectActorCatT, 4), Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(TargetActorT, TargetActorCatT, 4), Data);
            DataHelpers.Ensure2ByteAlign(Data);
            DataHelpers.AddObjectToByteList(SubjectActor, Data);
            DataHelpers.AddObjectToByteList(SubjectActorCat, Data);
            DataHelpers.AddObjectToByteList(TargetActor, Data);
            DataHelpers.AddObjectToByteList(TargetActorCat, Data);
            DataHelpers.Ensure2ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 22);

            return Data.ToArray();
        }
    }
}

