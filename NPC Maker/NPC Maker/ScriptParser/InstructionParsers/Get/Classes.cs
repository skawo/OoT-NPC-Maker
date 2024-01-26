using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionGetExtVar : InstructionSub
    {
        ScriptVarVal ActorID { get; set; }
        ScriptVarVal Destination { get; set; }
        public byte Operator;
        public byte ExtVarNum;

        public InstructionGetExtVar(byte _SubID, byte _ExtVarNum, ScriptVarVal _ActorID, ScriptVarVal _Destination)
                                        : base((int)Lists.Instructions.GET, _SubID)
        {
            ActorID = _ActorID;
            Destination = _Destination;
            ExtVarNum = _ExtVarNum;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();
            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(ExtVarNum, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(ActorID.Vartype, Destination.Vartype, 4), Data);
            Helpers.AddObjectToByteList(ActorID.Value, Data);
            Helpers.AddObjectToByteList(Destination.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.SetSubTypes)SubID).ToString();
        }
    }

}

