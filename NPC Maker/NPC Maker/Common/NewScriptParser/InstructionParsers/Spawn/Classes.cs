using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionSpawn : Instruction
    {
        bool PosRelativeness { get; set; }
        float PosX { get; set; }
        float PosY { get; set; }
        float PosZ { get; set; }
        Int32 RotX { get; set; }
        Int32 RotY { get; set; }
        Int32 RotZ { get; set; }
        UInt32 ActorID { get; set; }
        UInt32 ActorVariable { get; set; }

        byte ActorIDVarT { get; set; }
        byte ActorVarT { get; set; }
        byte PosXT { get; set; }
        byte PosYT { get; set; }
        byte PosZT { get; set; }
        byte RotXT { get; set; }
        byte RotYT { get; set; }
        byte RotZT { get; set; }

        public InstructionSpawn(bool _PosRelativeness, float _PosX, byte _PosXT, float _PosY, byte _PosYT, float _PosZ, byte _PosZT,
                                Int32 _RotX, byte _RotXT, Int32 _RotY, byte _RotYT, Int32 _RotZ, byte _RotZT, UInt32 _ActorID, byte _ActorIDVarT, UInt32 _ActorVariable, byte _ActorVarT) : base((byte)Lists.Instructions.SPAWN)
        {
            PosRelativeness = _PosRelativeness;
            PosX = _PosX;
            PosY = _PosY;
            PosZ = _PosZ;
            RotX = _RotX;
            RotY = _RotY;
            RotZ = _RotZ;
            PosXT = _PosXT;
            PosYT = _PosYT;
            PosZT = _PosZT;
            RotXT = _RotXT;
            RotYT = _RotYT;
            RotZT = _RotZT;
            ActorID = _ActorID;
            ActorIDVarT = _ActorIDVarT;
            ActorVariable = _ActorVariable;
            ActorVarT = _ActorVarT;

        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);

            DataHelpers.AddObjectToByteList(PosRelativeness, Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(PosXT, PosYT, 4), Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(PosZT, RotXT, 4), Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(RotYT, RotZT, 4), Data);

            DataHelpers.AddObjectToByteList(ActorID, Data);
            DataHelpers.AddObjectToByteList(ActorVariable, Data);

            DataHelpers.AddObjectToByteList(PosX, Data);
            DataHelpers.AddObjectToByteList(PosY, Data);
            DataHelpers.AddObjectToByteList(PosZ, Data);
            DataHelpers.AddObjectToByteList(RotX, Data);
            DataHelpers.AddObjectToByteList(RotY, Data);
            DataHelpers.AddObjectToByteList(RotZ, Data);

            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(ActorIDVarT, ActorVarT, 4), Data);
            DataHelpers.Ensure4ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 40);

            return Data.ToArray();
        }
    }
}

