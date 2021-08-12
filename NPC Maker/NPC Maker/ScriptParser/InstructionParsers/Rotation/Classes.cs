using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionRotation : InstructionSub
    {
        ScriptVarVal X { get; set; }
        ScriptVarVal Y { get; set; }
        ScriptVarVal Z { get; set; }
        ScriptVarVal ActorID { get; set; }
        ScriptVarVal Speed { get; set; }

        public byte Target;

        public InstructionRotation(byte _SubID, byte _Target, ScriptVarVal _ActorID, ScriptVarVal _XRot, ScriptVarVal _YRot, ScriptVarVal _ZRot, ScriptVarVal _Speed)
                                : base((int)Lists.Instructions.ROTATION, _SubID)
        {
            ActorID = _ActorID;
            X = _XRot;
            Y = _YRot;
            Z = _ZRot;
            Target = _Target;
            Speed = _Speed;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(SubID, Data);
            Helpers.AddObjectToByteList(Target, Data);
            Helpers.AddObjectToByteList(Speed.Vartype, Data);

            Helpers.AddObjectToByteList(X.Value, Data);
            Helpers.AddObjectToByteList(Y.Value, Data);
            Helpers.AddObjectToByteList(Z.Value, Data);
            Helpers.AddObjectToByteList(ActorID.Value, Data);
            Helpers.AddObjectToByteList(Speed.Value, Data);

            Helpers.AddObjectToByteList(X.Vartype, Data);
            Helpers.AddObjectToByteList(Y.Vartype, Data);
            Helpers.AddObjectToByteList(Z.Vartype, Data);
            Helpers.AddObjectToByteList(ActorID.Vartype, Data);
            Helpers.Ensure4ByteAlign(Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 28);

            return Data.ToArray();
        }
    }
}

