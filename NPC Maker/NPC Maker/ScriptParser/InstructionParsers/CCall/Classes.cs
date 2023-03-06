using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionCCall : Instruction
    {
        UInt32 FuncAddr { get; set; }

        ScriptVarVal Destination { get; set; }

        public InstructionCCall(UInt32 _FuncAddr, ScriptVarVal _OutValDestination) : base((byte)Lists.Instructions.CCALL)
        {
            FuncAddr = _FuncAddr;
            Destination = _OutValDestination;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(Destination.Vartype, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(FuncAddr, Data);
            Helpers.AddObjectToByteList(Destination.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);
            return Data.ToArray();
        }
    }
}

