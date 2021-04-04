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

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(Subject, Target, 4), Data);
            Helpers.AddObjectToByteList(FaceType, Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(SubjectActorT, SubjectActorCatT, 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(TargetActorT, TargetActorCatT, 4), Data);
            Helpers.Ensure2ByteAlign(Data);
            Helpers.AddObjectToByteList(SubjectActor, Data);
            Helpers.AddObjectToByteList(SubjectActorCat, Data);
            Helpers.AddObjectToByteList(TargetActor, Data);
            Helpers.AddObjectToByteList(TargetActorCat, Data);
            Helpers.Ensure2ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 22);

            return Data.ToArray();
        }
    }
}

