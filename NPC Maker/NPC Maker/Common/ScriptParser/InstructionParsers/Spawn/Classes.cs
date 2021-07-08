using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.Scripts
{
    public class InstructionSpawn : Instruction
    {
        byte PosRelativeness { get; set; }
        object PosX { get; set; }
        object PosY { get; set; }
        object PosZ { get; set; }
        object RotX { get; set; }
        object RotY { get; set; }
        object RotZ { get; set; }
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

        public InstructionSpawn(byte _PosRelativeness, object _PosX, byte _PosXT, object _PosY, byte _PosYT, object _PosZ, byte _PosZT,
                                object _RotX, byte _RotXT, object _RotY, byte _RotYT, object _RotZ, byte _RotZT, UInt32 _ActorID, byte _ActorIDVarT, 
                                UInt32 _ActorVariable, byte _ActorVarT) : base((byte)Lists.Instructions.SPAWN)
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

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(PosRelativeness, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(PosXT, PosYT, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(PosZT, RotXT, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(ActorIDVarT, ActorVarT, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(RotYT, RotZT, 4), Data);

            Helpers.Ensure4ByteAlign(Data);

            Helpers.AddObjectToByteList(ActorID, Data);
            Helpers.AddObjectToByteList(ActorVariable, Data);

            Helpers.AddObjectToByteList(PosX, Data);
            Helpers.AddObjectToByteList(PosY, Data);
            Helpers.AddObjectToByteList(PosZ, Data);
            Helpers.AddObjectToByteList(RotX, Data);
            Helpers.AddObjectToByteList(RotY, Data);
            Helpers.AddObjectToByteList(RotZ, Data);

            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 40);

            return Data.ToArray();
        }
    }
}

