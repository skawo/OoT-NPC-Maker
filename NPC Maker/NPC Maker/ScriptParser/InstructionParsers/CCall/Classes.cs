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
            var data = new List<byte>();

            // Header
            Helpers.AddObjectToByteList(ID, data);
            Helpers.AddObjectToByteList(NumArgs, data);
            Helpers.AddObjectToByteList(Destination.Vartype, data);
            Helpers.Ensure4ByteAlign(data);

            Helpers.AddObjectToByteList(FuncAddr, data);
            Helpers.AddObjectToByteList(Destination.Value, data);

            if (NumArgs > 0)
            {
                for (int i = 0; i < 8; i += 2)
                {
                    byte first = (i < NumArgs) ? Params[i].Vartype : (byte)0;
                    byte second = (i + 1 < NumArgs) ? Params[i + 1].Vartype : (byte)0;

                    Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(first, second, 4), data);
                }

                Helpers.Ensure4ByteAlign(data);
            }

            for (int i = 0; i < NumArgs; i++)
                Helpers.AddObjectToByteList(Params[i].Value, data);

            Helpers.Ensure4ByteAlign(data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(data, 12 + (NumArgs != 0 ? 4 : 0) + (NumArgs * 4));
            return data.ToArray();
        }

    }
}

