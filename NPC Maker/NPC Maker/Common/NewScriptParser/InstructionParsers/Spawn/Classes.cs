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
        Int16 RotX { get; set; }
        Int16 RotY { get; set; }
        Int16 RotZ { get; set; }
        UInt16 ActorID { get; set; }
        UInt16 ActorVariable { get; set; }

        public InstructionSpawn(bool _PosRelativeness, decimal _PosX, decimal _PosY, decimal _PosZ, 
                                Int16 _RotX, Int16 _RotY, Int16 _RotZ, UInt16 _ActorID, UInt16 _ActorVariable  ) : base((byte)Lists.Instructions.SPAWN)
        {
            PosRelativeness = _PosRelativeness;
            PosX = (float)_PosX;
            PosY = (float)_PosY;
            PosZ = (float)_PosZ;
            RotX = _RotX;
            RotY = _RotY;
            RotZ = _RotZ;
            ActorID = _ActorID;
            ActorVariable = _ActorVariable;

        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(PosRelativeness, Data);
            DataHelpers.AddObjectToByteList(ActorID, Data);
            DataHelpers.AddObjectToByteList(ActorVariable, Data);
            DataHelpers.AddObjectToByteList(PosX, Data);
            DataHelpers.AddObjectToByteList(PosY, Data);
            DataHelpers.AddObjectToByteList(PosZ, Data);
            DataHelpers.AddObjectToByteList(RotX, Data);
            DataHelpers.AddObjectToByteList(RotY, Data);
            DataHelpers.AddObjectToByteList(RotZ, Data);

            DataHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }
}

