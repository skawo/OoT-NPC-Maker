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

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);

            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(TypeT, RelativePos, 4), Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(PosXT, PosYT, 4), Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(PosZT, AccelXT, 4), Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(AccelYT, AccelZT, 4), Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(VelXT, VelYT, 4), Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(VelZT, ScaleT, 4), Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(ScaleUpdateT, RadiusUpdateDT, 4), Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(LifeT, NumBoltsT, 4), Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(YawT, DListIndexT, 4), Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(ColorTypeT, AlphaT, 4), Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(PrimRGBAVarT[0], PrimRGBAVarT[1], 4), Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(PrimRGBAVarT[2], PrimRGBAVarT[3], 4), Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(SecRGBAVarT[0], SecRGBAVarT[1], 4), Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(SecRGBAVarT[2], SecRGBAVarT[3], 4), Data);

            DataHelpers.Ensure2ByteAlign(Data);

            DataHelpers.AddObjectToByteList(Type, Data);
            DataHelpers.AddObjectToByteList(PosX, Data);
            DataHelpers.AddObjectToByteList(PosY, Data);
            DataHelpers.AddObjectToByteList(PosZ, Data);
            DataHelpers.AddObjectToByteList(AccelX, Data);
            DataHelpers.AddObjectToByteList(AccelY, Data);
            DataHelpers.AddObjectToByteList(AccelZ, Data);
            DataHelpers.AddObjectToByteList(VelX, Data);
            DataHelpers.AddObjectToByteList(VelY, Data);
            DataHelpers.AddObjectToByteList(VelZ, Data);

            DataHelpers.AddObjectToByteList(PrimRGBA[0], Data);
            DataHelpers.AddObjectToByteList(PrimRGBA[1], Data);
            DataHelpers.AddObjectToByteList(PrimRGBA[2], Data);
            DataHelpers.AddObjectToByteList(PrimRGBA[3], Data);
            DataHelpers.AddObjectToByteList(SecRGBA[0], Data);
            DataHelpers.AddObjectToByteList(SecRGBA[1], Data);
            DataHelpers.AddObjectToByteList(SecRGBA[2], Data);
            DataHelpers.AddObjectToByteList(SecRGBA[3], Data);

            DataHelpers.AddObjectToByteList(Scale, Data);
            DataHelpers.AddObjectToByteList(ScaleUpdate, Data);
            DataHelpers.AddObjectToByteList(RadiusUpdateD, Data);

            DataHelpers.AddObjectToByteList(Life, Data);
            DataHelpers.AddObjectToByteList(NumBolts, Data);
            DataHelpers.AddObjectToByteList(Yaw, Data);
            DataHelpers.AddObjectToByteList(DListIndex, Data);
            DataHelpers.AddObjectToByteList(ColorType, Data);
            DataHelpers.AddObjectToByteList(Alpha, Data);
            DataHelpers.AddObjectToByteList(LabelJumpIfFound.InstructionNumber, Data);

            DataHelpers.Ensure2ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 128);

            return Data.ToArray();
        }
    }
}

