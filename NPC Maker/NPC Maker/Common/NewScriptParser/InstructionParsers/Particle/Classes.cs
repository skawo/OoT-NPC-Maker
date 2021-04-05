using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionParticle : Instruction
    {
        public Int32 Type { get; set; }
        public byte TypeT { get; set; }

        public byte RelativePos { get; set; }

        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public byte PosXT { get; set; }
        public byte PosYT { get; set; }
        public byte PosZT { get; set; }

        public float AccelX { get; set; }
        public float AccelY { get; set; }
        public float AccelZ { get; set; }
        public byte AccelXT { get; set; }
        public byte AccelYT { get; set; }
        public byte AccelZT { get; set; }

        public float VelX { get; set; }
        public float VelY { get; set; }
        public float VelZ { get; set; }
        public byte VelXT { get; set; }
        public byte VelYT { get; set; }
        public byte VelZT { get; set; }

        public UInt32[] PrimRGBA { get; set; }
        public byte[] PrimRGBAVarT { get; set; }
        public UInt32[] SecRGBA { get; set; }
        public byte[] SecRGBAVarT { get; set; }

        public float Scale { get; set; }
        public byte ScaleT { get; set; }

        public Int32 ScaleUpdate { get; set; }
        public byte ScaleUpdateT { get; set; }

        public Int32 RadiusUpdateD { get; set; }
        public byte RadiusUpdateDT { get; set; }

        public Int32 Life { get; set; }
        public byte LifeT { get; set; }

        public Int32 NumBolts { get; set; }
        public byte NumBoltsT { get; set; }

        public Int32 Yaw { get; set; }
        public byte YawT { get; set; }

        public Int32 DListIndex { get; set; }
        public byte DListIndexT { get; set; }

        public Int32 ColorType { get; set; }
        public byte ColorTypeT { get; set; }

        public InstructionLabel LabelJumpIfFound { get; set; }

        public Int32 Alpha { get; set; }
        public byte AlphaT { get; set; }

        public InstructionParticle() : base((byte)Lists.Instructions.PARTICLE)
        {
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);

            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(TypeT, RelativePos, 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(PosXT, PosYT, 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(PosZT, AccelXT, 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(AccelYT, AccelZT, 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(VelXT, VelYT, 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(VelZT, ScaleT, 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(ScaleUpdateT, RadiusUpdateDT, 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(LifeT, NumBoltsT, 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(YawT, DListIndexT, 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(ColorTypeT, AlphaT, 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(PrimRGBAVarT[0], PrimRGBAVarT[1], 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(PrimRGBAVarT[2], PrimRGBAVarT[3], 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(SecRGBAVarT[0], SecRGBAVarT[1], 4), Data);
            Helpers.AddObjectToByteList(Helpers.SmooshTwoValues(SecRGBAVarT[2], SecRGBAVarT[3], 4), Data);

            Helpers.Ensure4ByteAlign(Data);

            Helpers.AddObjectToByteList(Type, Data);
            Helpers.AddObjectToByteList(PosX, Data);
            Helpers.AddObjectToByteList(PosY, Data);
            Helpers.AddObjectToByteList(PosZ, Data);
            Helpers.AddObjectToByteList(AccelX, Data);
            Helpers.AddObjectToByteList(AccelY, Data);
            Helpers.AddObjectToByteList(AccelZ, Data);
            Helpers.AddObjectToByteList(VelX, Data);
            Helpers.AddObjectToByteList(VelY, Data);
            Helpers.AddObjectToByteList(VelZ, Data);

            Helpers.AddObjectToByteList(PrimRGBA[0], Data);
            Helpers.AddObjectToByteList(PrimRGBA[1], Data);
            Helpers.AddObjectToByteList(PrimRGBA[2], Data);
            Helpers.AddObjectToByteList(PrimRGBA[3], Data);
            Helpers.AddObjectToByteList(SecRGBA[0], Data);
            Helpers.AddObjectToByteList(SecRGBA[1], Data);
            Helpers.AddObjectToByteList(SecRGBA[2], Data);
            Helpers.AddObjectToByteList(SecRGBA[3], Data);

            Helpers.AddObjectToByteList(Scale, Data);
            Helpers.AddObjectToByteList(ScaleUpdate, Data);
            Helpers.AddObjectToByteList(RadiusUpdateD, Data);

            Helpers.AddObjectToByteList(Life, Data);
            Helpers.AddObjectToByteList(NumBolts, Data);
            Helpers.AddObjectToByteList(Yaw, Data);
            Helpers.AddObjectToByteList(DListIndex, Data);
            Helpers.AddObjectToByteList(ColorType, Data);
            Helpers.AddObjectToByteList(Alpha, Data);
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, LabelJumpIfFound, ref Data);

            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 128);

            return Data.ToArray();
        }
    }
}

