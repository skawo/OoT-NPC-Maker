using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionCCall : Instruction
    {
        UInt32 FuncAddr { get; set; }

        ScriptVarVal Destination { get; set; }

        byte NumArgs { get; set; }

        List<ScriptVarVal> Params { get; set; }

        public InstructionCCall(UInt32 _FuncAddr, ScriptVarVal _OutValDestination, List<ScriptVarVal> _Params) : base((byte)Lists.Instructions.CCALL)
        {
            FuncAddr = _FuncAddr;
            Destination = _OutValDestination;
            Params = _Params;
            NumArgs = (byte)Params.Count;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(NumArgs, Data);
            Helpers.AddObjectToByteList(Destination.Vartype, Data);
            Helpers.Ensure4ByteAlign(Data);

            Helpers.AddObjectToByteList(FuncAddr, Data);
            Helpers.AddObjectToByteList(Destination.Value, Data);

            if (NumArgs != 0)
            {
                for (int i = 0; i < 8; i += 2)
                {
                    if (NumArgs > i + 1)
                        Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Params[i].Vartype, Params[i + 1].Vartype, 4), Data);
                    else if (NumArgs > i)
                        Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Params[i].Vartype, (byte)0, 4), Data);
                    else
                        Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether((byte)0, (byte)0, 4), Data);
                }

                Helpers.Ensure4ByteAlign(Data);
            }

            for (int i = 0; i < NumArgs; i++)
                Helpers.AddObjectToByteList(Params[i].Value, Data);

            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12 + (NumArgs != 0 ? 4 : 0) + (NumArgs * 4));
            return Data.ToArray();
        }
    }
}

