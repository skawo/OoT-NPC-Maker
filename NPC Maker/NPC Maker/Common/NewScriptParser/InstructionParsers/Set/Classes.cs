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
            DataHelpers.AddObjectToByteList(ValueType, Data);
            DataHelpers.AddObjectToByteList(Value, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.SetSubTypes)SubID).ToString();
        }
    }

    public class InstructionSetWObject : InstructionSub
    {
        public object U16;
        public object Object;

        public InstructionSetWObject(byte _SubID, UInt16 _U16, object _Object) : base((int)Lists.Instructions.SET, _SubID)
        {
            U16 = _U16;
            Object = _Object;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();
            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(U16, Data);
            DataHelpers.AddObjectToByteList(Object, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }

    public class InstructionSetScriptVar : InstructionSub
    {
        public byte Operator;
        public float Value;
        public byte ValueType;

        public InstructionSetScriptVar(byte _SubID, byte _Operator, float _Value, byte _ValueType)
                                      : base((int)Lists.Instructions.SET, _SubID)
        {
            Operator = _Operator;
            Value = _Value;
            ValueType = _ValueType;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();
            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(Operator, Data);
            DataHelpers.AddObjectToByteList(Value, Data);
            DataHelpers.AddObjectToByteList(ValueType, Data);
            DataHelpers.Ensure4ByteAlign(Data);
            return Data.ToArray();
        }
    }

    public class InstructionSetEnvColor : InstructionSub
    {
        public byte R;
        public byte G;
        public byte B;
        public byte ValType1;
        public byte ValType2;
        public byte ValType3;

        public InstructionSetEnvColor(byte _SubID, byte _R, byte _G, byte _B,
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
            DataHelpers.AddObjectToByteList(R, Data);
            DataHelpers.AddObjectToByteList(G, Data);
            DataHelpers.AddObjectToByteList(B, Data);
            DataHelpers.AddObjectToByteList(ValType1, Data);
            DataHelpers.AddObjectToByteList(ValType2, Data);
            DataHelpers.AddObjectToByteList(ValType3, Data);
            DataHelpers.Ensure4ByteAlign(Data);

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
            DataHelpers.AddObjectToByteList(Resp1.InstructionNumber, Data);
            DataHelpers.AddObjectToByteList(Resp2.InstructionNumber, Data);
            DataHelpers.AddObjectToByteList(Resp3.InstructionNumber, Data);
            DataHelpers.Ensure4ByteAlign(Data);

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

    public class InstructionSetCameraTracking : InstructionSub
    {
        byte Target { get; set; }
        UInt16 Value { get; set; }
        byte Value2 { get; set; }
        byte ValueType { get; set; }
        byte ValueType2 { get; set; }

        public InstructionSetCameraTracking(byte _SubID, byte _Target, UInt16 _Value, byte _Value2, 
                                            byte _ValueType, byte _ValueType2) : base((int)Lists.Instructions.SET, _SubID)
        {
            Target = _Target;
            Value = _Value;
            Value2 = _Value2;
            ValueType = _ValueType;
            ValueType2 = _ValueType2;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();
            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(Target, Data);
            DataHelpers.AddObjectToByteList(Value2, Data);
            DataHelpers.AddObjectToByteList(Value, Data);
            DataHelpers.AddObjectToByteList(ValueType, Data);
            DataHelpers.AddObjectToByteList(ValueType2, Data);
            DataHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }

    public class InstructionSetKeyframes : InstructionSub
    {
        UInt16 AnimationID { get; set; }
        byte[] Pattern { get; set; }

        public InstructionSetKeyframes(byte _SubID, UInt16 _AnimationID, byte[] _Pattern) 
                                      : base((int)Lists.Instructions.SET, _SubID)
        {
            AnimationID = _AnimationID;
            Pattern = _Pattern;
        }

        public override byte[] ToBytes()
        {
            List<byte> Data = new List<byte>();
            DataHelpers.AddObjectToByteList(ID, Data);
            DataHelpers.AddObjectToByteList(SubID, Data);
            DataHelpers.AddObjectToByteList(AnimationID, Data);
            Data.AddRange(Pattern);
            DataHelpers.Ensure4ByteAlign(Data);

            return Data.ToArray();
        }
    }

}

