using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionSet : InstructionSub
    {
        ScriptVarVal Value { get; set; }
        public byte Operator;

        public InstructionSet(byte _SubID, ScriptVarVal _Value, byte _Operator)
                             : base((int)Lists.Instructions.SET, _SubID)
        {
            Value = _Value;
            Operator = _Operator;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Value.Vartype, Data);
            Helpers.AddObjectToByteList(Operator, Data);
            Helpers.AddObjectToByteList(Value.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 8);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.SetSubTypes)SubID).ToString();
        }
    }

    public class InstructionSetWTwoValues : InstructionSub
    {
        ScriptVarVal Value { get; set; }
        ScriptVarVal Value2 { get; set; }
        public byte Operator { get; set; }

        public InstructionSetWTwoValues(byte _SubID, ScriptVarVal _Value, ScriptVarVal _Value2, byte _Operator)
                                        : base((int)Lists.Instructions.SET, _SubID)
        {
            Value = _Value;
            Value2 = _Value2;
            Operator = _Operator;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();
            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Operator, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Value.Vartype, Value2.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Value.Value, Data);
            Helpers.AddObjectToByteList(Value2.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 12);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.SetSubTypes)SubID).ToString();
        }
    }

    public class InstructionSetExtVar : InstructionSub
    {
        ScriptVarVal Value { get; set; }
        ScriptVarVal Value2 { get; set; }
        public byte Operator;
        public byte ExtVarNum;

        public InstructionSetExtVar(byte _SubID, byte _ExtVarNum, ScriptVarVal _Value, ScriptVarVal _Value2, byte _Operator)
                                        : base((int)Lists.Instructions.SET, _SubID)
        {
            Value = _Value;
            Value2 = _Value2;
            Operator = _Operator;
            ExtVarNum = _ExtVarNum;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();
            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Operator, Data);
            Helpers.AddObjectToByteList(Value.Vartype, Data);
            Helpers.AddObjectToByteList(Value.Value, Data);
            Helpers.AddObjectToByteList(Value2.Value, Data);
            Helpers.AddObjectToByteList(Value2.Vartype, Data);
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
        ScriptVarVal Offs { get; set; }
        ScriptVarVal Speed { get; set; }
        ScriptVarVal StFrame { get; set; }
        ScriptVarVal EnFrame { get; set; }
        public byte Once;

        public InstructionSetPlayerAnim(byte _SubID, ScriptVarVal _Offs, ScriptVarVal _Speed, ScriptVarVal _StF, ScriptVarVal _EnF, byte _Once)
                                        : base((int)Lists.Instructions.SET, _SubID)
        {
            Offs = _Offs;
            Speed = _Speed;
            StFrame = _StF;
            EnFrame = _EnF;
            Once = _Once;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();
            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Offs.Vartype, Data);
            Helpers.AddObjectToByteList(Speed.Vartype, Data);
            Helpers.AddObjectToByteList(StFrame.Vartype, Data);
            Helpers.AddObjectToByteList(EnFrame.Vartype, Data);
            Helpers.AddObjectToByteList(Once, Data);

            Helpers.Ensure4ByteAlign(Data);

            Helpers.AddObjectToByteList(Offs.Value, Data);
            Helpers.AddObjectToByteList(Speed.Value, Data);
            Helpers.AddObjectToByteList(StFrame.Value, Data);
            Helpers.AddObjectToByteList(EnFrame.Value, Data);

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
        ScriptVarVal R { get; set; }
        ScriptVarVal G { get; set; }
        ScriptVarVal B { get; set; }


        public InstructionSetEnvColor(byte _SubID, ScriptVarVal _R, ScriptVarVal _G, ScriptVarVal _B) : base((int)Lists.Instructions.SET, _SubID)
        {
            R = _R;
            G = _G;
            B = _B;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(R.Vartype, G.Vartype, 4), Data);
            Helpers.AddObjectToByteList(B.Vartype, Data);

            Helpers.AddObjectToByteList(R.Value, Data);
            Helpers.AddObjectToByteList(G.Value, Data);
            Helpers.AddObjectToByteList(B.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 16);

            return Data.ToArray();
        }

        public override string ToString()
        {
            return ((Lists.Instructions)ID).ToString() + ", " + ((Lists.SetSubTypes)SubID).ToString();
        }
    }

    public class InstructionSetDlistColor : InstructionSub
    {
        ScriptVarVal R { get; set; }
        ScriptVarVal G { get; set; }
        ScriptVarVal B { get; set; }
        ScriptVarVal DlistID { get; set; }

        public InstructionSetDlistColor(byte _SubID, ScriptVarVal _R, ScriptVarVal _G, ScriptVarVal _B, ScriptVarVal _DlistId) : base((int)Lists.Instructions.SET, _SubID)
        {
            R = _R;
            G = _G;
            B = _B;
            DlistID = _DlistId;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(R.Vartype, G.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(B.Vartype, DlistID.Vartype, 4), Data);

            Helpers.AddObjectToByteList(R.Value, Data);
            Helpers.AddObjectToByteList(G.Value, Data);
            Helpers.AddObjectToByteList(B.Value, Data);
            Helpers.AddObjectToByteList(DlistID.Value, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 20);

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

    public class InstructionSetActor : InstructionSub
    {
        byte Target { get; set; }
        ScriptVarVal Value { get; set; }

        public InstructionSetActor(byte _SubID, byte _Target, ScriptVarVal _Value) : base((int)Lists.Instructions.SET, _SubID)
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
            Helpers.AddObjectToByteList(Value.Vartype, Data);
            Helpers.AddObjectToByteList(Value.Value, Data);
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

