﻿using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
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
            Helpers.AddObjectToByteList(Target, Data);
            Helpers.AddObjectToByteList(FaceType, Data);
            Helpers.AddObjectToByteList(SubjectActorT, Data);
            Helpers.AddObjectToByteList(SubjectActorCatT, Data);
            Helpers.AddObjectToByteList(TargetActorT, Data);
            Helpers.AddObjectToByteList(TargetActorCatT, Data);
            Helpers.AddObjectToByteList(Subject, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(SubjectActor, Data);
            Helpers.AddObjectToByteList(SubjectActorCat, Data);
            Helpers.AddObjectToByteList(TargetActor, Data);
            Helpers.AddObjectToByteList(TargetActorCat, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 24);

            return Data.ToArray();
        }
    }
}
