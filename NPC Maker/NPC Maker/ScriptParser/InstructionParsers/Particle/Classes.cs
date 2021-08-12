using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionParticle : Instruction
    {
        public byte Type { get; set; }
        public byte PosType { get; set; }

        public ScriptVarVal PosX { get; set; }
        public ScriptVarVal PosY { get; set; }
        public ScriptVarVal PosZ { get; set; }

        public ScriptVarVal AccelX { get; set; }
        public ScriptVarVal AccelY { get; set; }
        public ScriptVarVal AccelZ { get; set; }

        public ScriptVarVal VelX { get; set; }
        public ScriptVarVal VelY { get; set; }
        public ScriptVarVal VelZ { get; set; }

        public ScriptVarVal PrimR { get; set; }
        public ScriptVarVal PrimG { get; set; }
        public ScriptVarVal PrimB { get; set; }
        public ScriptVarVal PrimA { get; set; }

        public ScriptVarVal SecR { get; set; }
        public ScriptVarVal SecG { get; set; }
        public ScriptVarVal SecB { get; set; }
        public ScriptVarVal SecA { get; set; }

        public ScriptVarVal Scale { get; set; }
        public ScriptVarVal ScaleUpdate { get; set; }
        public ScriptVarVal Life { get; set; }
        public ScriptVarVal Var { get; set; }
        public ScriptVarVal Yaw { get; set; }
        public ScriptVarVal DListIndex { get; set; }

        public InstructionLabel LabelJumpIfFound { get; set; }


        public InstructionParticle() : base((byte)Lists.Instructions.PARTICLE)
        {
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(Type, Data);

            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, LabelJumpIfFound, ref Data);

            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Life.Vartype, Var.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Yaw.Vartype, DListIndex.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(PosX.Vartype, PosY.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(PosZ.Vartype, AccelX.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(AccelY.Vartype, AccelZ.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(VelX.Vartype, VelY.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(VelZ.Vartype, PosType, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Scale.Vartype, ScaleUpdate.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(PrimR.Vartype, PrimG.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(PrimB.Vartype, PrimA.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(SecR.Vartype, SecG.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(SecB.Vartype, SecA.Vartype, 4), Data);

            Helpers.Ensure4ByteAlign(Data);

            Helpers.AddObjectToByteList(PosX.Value, Data);
            Helpers.AddObjectToByteList(PosY.Value, Data);
            Helpers.AddObjectToByteList(PosZ.Value, Data);
            Helpers.AddObjectToByteList(AccelX.Value, Data);
            Helpers.AddObjectToByteList(AccelY.Value, Data);
            Helpers.AddObjectToByteList(AccelZ.Value, Data);
            Helpers.AddObjectToByteList(VelX.Value, Data);
            Helpers.AddObjectToByteList(VelY.Value, Data);
            Helpers.AddObjectToByteList(VelZ.Value, Data);

            Helpers.AddObjectToByteList(PrimR.Value, Data);
            Helpers.AddObjectToByteList(PrimG.Value, Data);
            Helpers.AddObjectToByteList(PrimB.Value, Data);
            Helpers.AddObjectToByteList(PrimA.Value, Data);
            Helpers.AddObjectToByteList(SecR.Value, Data);
            Helpers.AddObjectToByteList(SecG.Value, Data);
            Helpers.AddObjectToByteList(SecB.Value, Data);
            Helpers.AddObjectToByteList(SecA.Value, Data);

            Helpers.AddObjectToByteList(Scale.Value, Data);
            Helpers.AddObjectToByteList(ScaleUpdate.Value, Data);

            Helpers.AddObjectToByteList(Life.Value, Data);
            Helpers.AddObjectToByteList(Var.Value, Data);
            Helpers.AddObjectToByteList(Yaw.Value, Data);
            Helpers.AddObjectToByteList(DListIndex.Value, Data);

            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 108);

            return Data.ToArray();
        }
    }
}

