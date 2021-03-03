using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class InstructionSet : InstructionSubWValueType
    {
        public object Value;

        public InstructionSet(byte _SubID, object _Value, byte _ValueType)
                             : base((int)Lists.Instructions.SET, _SubID, _ValueType)
        {
            Value = _Value;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.Ensure4ByteAlign(Data);
            DataHelpers.AddObjectToByteList(ValueType, Data);
            DataHelpers.AddObjectToByteList(Value, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 8);

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

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();
            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(Operator, Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(ValueType, ValueType2, 4), Data);
            DataHelpers.AddObjectToByteList(Value, Data);
            DataHelpers.AddObjectToByteList(Value2, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }
    }

    public class InstructionSetEnvColor : InstructionSub
    {
        public UInt32 R;
        public UInt32 G;
        public UInt32 B;
        public byte ValType1;
        public byte ValType2;
        public byte ValType3;

        public InstructionSetEnvColor(byte _SubID, UInt32 _R, UInt32 _G, UInt32 _B,
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

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();

            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(ValType1, ValType2, 4), Data);
            DataHelpers.AddObjectToByteList(ValType3, Data);

            DataHelpers.AddObjectToByteList(R, Data);
            DataHelpers.AddObjectToByteList(G, Data);
            DataHelpers.AddObjectToByteList(B, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 16);

            return Data.ToArray();
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

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();
            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.Ensure4ByteAlign(Data);
            DataHelpers.AddObjectToByteList(Resp1.InstructionNumber, Data);
            DataHelpers.AddObjectToByteList(Resp2.InstructionNumber, Data);
            DataHelpers.AddObjectToByteList(Resp3.InstructionNumber, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 16);

            return Data.ToArray();
        }
    }

    public class InstructionSetScriptStart : InstructionSub
    {
        public InstructionLabel Goto;

        public InstructionSetScriptStart(InstructionLabel _Goto) : base((int)Lists.Instructions.SET, (int)Lists.SetSubTypes.SCRIPT_START)
        {
            Goto = _Goto;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();
            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(Goto.InstructionNumber, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }

    public class InstructionSetPattern : InstructionSub
    {
        byte[] Pattern { get; set; }

        public InstructionSetPattern(byte _SubID, byte[] _Pattern) : base((int)Lists.Instructions.SET, _SubID)
        {
            Pattern = _Pattern;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();
            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            Data.AddRange(Pattern);
            DataHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }

    public class InstructionSetCameraTracking : InstructionSubWValueType
    {
        byte Target { get; set; }
        UInt32 Value { get; set; }
        UInt32 Value2 { get; set; }
        byte ValueType2 { get; set; }

        public InstructionSetCameraTracking(byte _SubID, byte _Target, UInt32 _Value, byte _ValueType, UInt32 _Value2, byte _ValueType2) : base((int)Lists.Instructions.SET, _SubID, _ValueType)
        {
            Target = _Target;
            Value = _Value;
            Value2 = _Value2;
            ValueType2 = _ValueType2;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();
            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(Target, Data);
            DataHelpers.AddObjectToByteList(DataHelpers.SmooshTwoValues(ValueType, ValueType2, 4), Data);
            DataHelpers.AddObjectToByteList(Value, Data);
            DataHelpers.AddObjectToByteList(Value2, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }
    }

    public class InstructionSetKeyframes : InstructionSubWValueType
    {
        UInt32 AnimationID { get; set; }
        byte[] Pattern { get; set; }

        public InstructionSetKeyframes(byte _SubID, UInt32 _AnimationID, byte _AnimVarType, byte[] _Pattern) 
                                      : base((int)Lists.Instructions.SET, _SubID, _AnimVarType)
        {
            AnimationID = _AnimationID;
            Pattern = _Pattern;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();
            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(ValueType, Data);
            DataHelpers.Ensure4ByteAlign(Data);
            DataHelpers.AddObjectToByteList(AnimationID, Data);
            Data.AddRange(Pattern);
            DataHelpers.Ensure4ByteAlign(Data);

            DataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }
    }

}

