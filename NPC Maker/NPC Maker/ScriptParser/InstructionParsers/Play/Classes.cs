using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionPlay : InstructionSub
    {
        ScriptVarVal Value { get; set; }

        public InstructionPlay(byte _SubID, ScriptVarVal _Value) : base((int)Lists.Instructions.PLAY, _SubID)
        {
            Value = _Value;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Value.Vartype, Data);
            Helpers.Ensure4ByteAlign(Data);
            Helpers.AddObjectToByteList(Value.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 8);

            return Data.ToArray();
        }
    }

    public class InstructionPlayWithParams : InstructionSub
    {
        ScriptVarVal Value { get; set; }
        ScriptVarVal Volume { get; set; }
        ScriptVarVal Pitch { get; set; }
        ScriptVarVal Reverb { get; set; }

        public InstructionPlayWithParams(byte _SubID, ScriptVarVal _Value, ScriptVarVal _Volume, ScriptVarVal _Pitch, ScriptVarVal _Reverb) : base((int)Lists.Instructions.PLAY, _SubID)
        {
            Value = _Value;
            Volume = _Volume;
            Pitch = _Pitch;
            Reverb = _Reverb;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Value.Vartype, Volume.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Pitch.Vartype, Reverb.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Value.Value, Data);
            Helpers.AddObjectToByteList(Volume.Value, Data);
            Helpers.AddObjectToByteList(Pitch.Value, Data);
            Helpers.AddObjectToByteList(Reverb.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 20);

            return Data.ToArray();
        }
    }
}

