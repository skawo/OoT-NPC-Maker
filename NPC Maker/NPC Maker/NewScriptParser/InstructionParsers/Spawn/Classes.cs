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

            ParserHelpers.AddObjectToByteList(ID, Data);
            ParserHelpers.AddObjectToByteList(PosRelativeness, Data);
            ParserHelpers.AddObjectToByteList(ActorID, Data);
            ParserHelpers.AddObjectToByteList(ActorVariable, Data);
            ParserHelpers.AddObjectToByteList(PosX, Data);
            ParserHelpers.AddObjectToByteList(PosY, Data);
            ParserHelpers.AddObjectToByteList(PosZ, Data);
            ParserHelpers.AddObjectToByteList(RotX, Data);
            ParserHelpers.AddObjectToByteList(RotY, Data);
            ParserHelpers.AddObjectToByteList(RotZ, Data);

            ParserHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }
}

