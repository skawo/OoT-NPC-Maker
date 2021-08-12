using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionSpawn : Instruction
    {
        byte PosType { get; set; }

        ScriptVarVal PosX { get; set; }
        ScriptVarVal PosY { get; set; }
        ScriptVarVal PosZ { get; set; }

        ScriptVarVal RotX { get; set; }
        ScriptVarVal RotY { get; set; }
        ScriptVarVal RotZ { get; set; }

        ScriptVarVal ActorID { get; set; }
        ScriptVarVal ActorVariable { get; set; }

        public InstructionSpawn(byte _PosType, 
                                ScriptVarVal _PosX, ScriptVarVal _PosY, ScriptVarVal _PosZ,
                                ScriptVarVal _RotX, ScriptVarVal _RotY, ScriptVarVal _RotZ, 
                                ScriptVarVal _ActorID, ScriptVarVal _ActorVariable) : base((byte)Lists.Instructions.SPAWN)
        {
            PosType = _PosType;
            PosX = _PosX;
            PosY = _PosY;
            PosZ = _PosZ;
            RotX = _RotX;
            RotY = _RotY;
            RotZ = _RotZ;
            ActorID = _ActorID;
            ActorVariable = _ActorVariable;

        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(PosType, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(PosX.Vartype, PosY.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(PosZ.Vartype, RotX.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(ActorID.Vartype, ActorVariable.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(RotY.Vartype, RotZ.Vartype, 4), Data);

            Helpers.Ensure4ByteAlign(Data);

            Helpers.AddObjectToByteList(ActorID.Value, Data);
            Helpers.AddObjectToByteList(ActorVariable.Value, Data);

            Helpers.AddObjectToByteList(PosX.Value, Data);
            Helpers.AddObjectToByteList(PosY.Value, Data);
            Helpers.AddObjectToByteList(PosZ.Value, Data);
            Helpers.AddObjectToByteList(RotX.Value, Data);
            Helpers.AddObjectToByteList(RotY.Value, Data);
            Helpers.AddObjectToByteList(RotZ.Value, Data);

            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 40);

            return Data.ToArray();
        }
    }
}

