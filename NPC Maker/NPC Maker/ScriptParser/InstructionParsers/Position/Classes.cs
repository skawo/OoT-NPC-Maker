using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionPosition : InstructionSub
    {
        ScriptVarVal X { get; set; }
        ScriptVarVal Y { get; set; }
        ScriptVarVal Z { get; set; }
        ScriptVarVal ActorID { get; set; }
        ScriptVarVal Speed { get; set; }

        public byte Target;
        public byte IgnoreY;

        public InstructionPosition(byte _SubID, byte _Target, byte _IgnoreY, ScriptVarVal _ActorID, ScriptVarVal _XPos, ScriptVarVal _YPos, ScriptVarVal _ZPos, ScriptVarVal _Speed)
                                : base((int)Lists.Instructions.POSITION, _SubID)
        {
            ActorID = _ActorID;

            X = _XPos;
            Y = _YPos;
            Z = _ZPos;
            Target = _Target;

            Speed = _Speed;
            IgnoreY = _IgnoreY;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(SubID, Speed.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(IgnoreY, Target, 4), Data);
            Helpers.AddObjectToByteList(ActorID.Vartype, Data);

            Helpers.AddObjectToByteList(X.Value, Data);
            Helpers.AddObjectToByteList(Y.Value, Data);
            Helpers.AddObjectToByteList(Z.Value, Data);
            Helpers.AddObjectToByteList(ActorID.Value, Data);
            Helpers.AddObjectToByteList(Speed.Value, Data);

            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(X.Vartype, Y.Vartype, 4), Data);
            Helpers.AddObjectToByteList(Helpers.PutTwoValuesTogether(Z.Vartype, 0, 4), Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 28);

            return Data.ToArray();
        }
    }
}

