using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionSet : InstructionSubWValueType
    {
        public object Value;
        public byte Operator;

        public InstructionSet(byte _SubID, object _Value, byte _ValueType, byte _Operator)
                             : base((int)Lists.Instructions.SET, _SubID, _ValueType)
        {
            Value = _Value;
            Operator = _Operator;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(ValueType, Data);
            Helpers.AddObjectToByteList(Operator, Data);
            Helpers.AddObjectToByteList(Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 8);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.SetSubTypes)SubID).ToString();
        }
    }

    public class InstructionSetWTwoValues : InstructionSubWValueType
    {
        public object Value;
        public object Value2;
        public byte ValueType2;
        public byte Operator;

        public InstructionSetWTwoValues(byte _SubID, object _Value, byte _ValueType, object _Value2, byte _ValueType2, byte _Operator)
                                        : base((int)Lists.Instructions.SET, _SubID, _ValueType)
        {
            Value = _Value;
            Value2 = _Value2;
            ValueType2 = _ValueType2;
            Operator = _Operator;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();
            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Operator, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(ValueType, ValueType2, 4), Data);
            Helpers.AddObjectToByteList(Value, Data);
            Helpers.AddObjectToByteList(Value2, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.SetSubTypes)SubID).ToString();
        }
    }

    public class InstructionSetExtVar : InstructionSubWValueType
    {
        public object Value;
        public object Value2;
        public byte ValueType2;
        public byte Operator;
        public byte ExtVarNum;

        public InstructionSetExtVar(byte _SubID, byte _ExtVarNum, object _Value, byte _ValueType, object _Value2, byte _ValueType2, byte _Operator)
                                        : base((int)Lists.Instructions.SET, _SubID, _ValueType)
        {
            Value = _Value;
            Value2 = _Value2;
            ValueType2 = _ValueType2;
            Operator = _Operator;
            ExtVarNum = _ExtVarNum;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();
            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Operator, Data);
            Helpers.AddObjectToByteList(ValueType, Data);
            Helpers.AddObjectToByteList(Value, Data);
            Helpers.AddObjectToByteList(Value2, Data);
            Helpers.AddObjectToByteList(ValueType2, Data);
            Helpers.AddObjectToByteList(ExtVarNum, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 16);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.SetSubTypes)SubID).ToString();
        }
    }

    public class InstructionSetPlayerAnim : InstructionSub
    {
        public object Offs;
        public object Speed;
        public object StFrame;
        public object EnFrame;
        public byte OffsT;
        public byte SpeedT;
        public byte StFrameT;
        public byte EnFrameT;
        public byte Once;

        public InstructionSetPlayerAnim(byte _SubID, byte _OffsT, object _Offs, byte _SpeedT, object _Speed, byte _StFT, object _StF, byte _EnFT, object _EnF, byte _Once)
                                        : base((int)Lists.Instructions.SET, _SubID)
        {
            Offs = _Offs;
            Speed = _Speed;
            StFrame = _StF;
            EnFrame = _EnF;
            OffsT = _OffsT;
            SpeedT = _SpeedT;
            StFrameT = _StFT;
            EnFrameT = _EnFT;
            Once = _Once;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();
            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(OffsT, Data);
            Helpers.AddObjectToByteList(SpeedT, Data);
            Helpers.AddObjectToByteList(StFrameT, Data);
            Helpers.AddObjectToByteList(EnFrameT, Data);
            Helpers.AddObjectToByteList(Once, Data);

            Helpers.Ensure4ByteAlign(Data);

            Helpers.AddObjectToByteList(Offs, Data);
            Helpers.AddObjectToByteList(Speed, Data);
            Helpers.AddObjectToByteList(StFrame, Data);
            Helpers.AddObjectToByteList(EnFrame, Data);

            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 24);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.SetSubTypes)SubID).ToString();
        }
    }

    public class InstructionSetEnvColor : InstructionSub
    {
        public object R;
        public object G;
        public object B;
        public byte ValType1;
        public byte ValType2;
        public byte ValType3;

        public InstructionSetEnvColor(byte _SubID, object _R, object _G, object _B,
                                      byte _ValType1, byte _ValType2, byte _ValType3)
                                     : base((int)Lists.Instructions.SET, _SubID)
        {
            R = _R;
            G = _G;
            B = _B;
            ValType1 = _ValType1;
            ValType2 = _ValType2;
            ValType3 = _ValType3;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(ValType1, ValType2, 4), Data);
            Helpers.AddObjectToByteList(ValType3, Data);

            Helpers.AddObjectToByteList(R, Data);
            Helpers.AddObjectToByteList(G, Data);
            Helpers.AddObjectToByteList(B, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 16);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.SetSubTypes)SubID).ToString();
        }
    }

    public class InstructionSetResponses : InstructionSub
    {
        InstructionLabel Resp1 { get; set; }
        InstructionLabel Resp2 { get; set; }
        InstructionLabel Resp3 { get; set; }

        public InstructionSetResponses(byte _SubID, InstructionLabel _Resp1, InstructionLabel _Resp2, InstructionLabel _Resp3) : base((int)Lists.Instructions.SET, _SubID)
        {
            Resp1 = _Resp1;
            Resp2 = _Resp2;
            Resp3 = _Resp3;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();
            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);

            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, Resp1, ref Data);
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, Resp2, ref Data);
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, Resp3, ref Data);

            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 8);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.SetSubTypes)SubID).ToString();
        }
    }

    public class InstructionSetScriptStart : InstructionSub
    {
        public InstructionLabel Goto;

        public InstructionSetScriptStart(InstructionLabel _Goto) : base((int)Lists.Instructions.SET, (int)Lists.SetSubTypes.SCRIPT_START)
        {
            Goto = _Goto;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();
            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            ScriptDataHelpers.FindLabelAndAddToByteList(Labels, Goto, ref Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 4);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.SetSubTypes)SubID).ToString();
        }
    }

    public class InstructionSetPattern : InstructionSub
    {
        byte[] Pattern { get; set; }

        public InstructionSetPattern(byte _SubID, byte[] _Pattern) : base((int)Lists.Instructions.SET, _SubID)
        {
            Pattern = _Pattern;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();
            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.Ensure4ByteAlign(Data);
            Data.AddRange(Pattern);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 8);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.SetSubTypes)SubID).ToString();
        }
    }

    public class InstructionSetActor : InstructionSubWValueType
    {
        byte Target { get; set; }
        object Value { get; set; }


        public InstructionSetActor(byte _SubID, byte _Target, object _Value, byte _ValueType) : base((int)Lists.Instructions.SET, _SubID, _ValueType)
        {
            Target = _Target;
            Value = _Value;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();
            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Target, Data);
            Helpers.AddObjectToByteList(ValueType, Data);
            Helpers.AddObjectToByteList(Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 8);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.SetSubTypes)SubID).ToString();
        }
    }

}

