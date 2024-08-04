using System;
using System.Collections.Generic;

namespace NPC_Maker.Scripts
{
    public class InstructionQuake : Instruction
    {
        ScriptVarVal Speed { get; set; }
        ScriptVarVal Type { get; set; }
        ScriptVarVal Duration { get; set; }

        ScriptVarVal x { get; set; }

        ScriptVarVal y { get; set; }

        ScriptVarVal zrot { get; set; }

        ScriptVarVal zoom { get; set; }

        public InstructionQuake(ScriptVarVal _Speed, ScriptVarVal _QuakeType, ScriptVarVal _Duration, ScriptVarVal _x, ScriptVarVal _y, ScriptVarVal _zrot, ScriptVarVal _zoom) : base((int)Lists.Instructions.QUAKE)
        {
            Speed = _Speed;
            Type = _QuakeType;
            Duration = _Duration;
            zoom = _zoom;
            x = _x;
            y = _y;
            zrot = _zrot;
        }

        public override byte[] ToBytes(List<InstructionLabel> Labels)
        {
            List<byte> Data = new List<byte>();

            Helpers.AddObjectToByteList(ID, Data);
            Helpers.AddObjectToByteList(Speed.Vartype, Data);
            Helpers.AddObjectToByteList(Type.Vartype, Data);
            Helpers.AddObjectToByteList(Duration.Vartype, Data);

            Helpers.AddObjectToByteList(x.Vartype, Data);
            Helpers.AddObjectToByteList(y.Vartype, Data);
            Helpers.AddObjectToByteList(zrot.Vartype, Data);
            Helpers.AddObjectToByteList(zoom.Vartype, Data);

            Helpers.AddObjectToByteList(Speed.Value, Data);
            Helpers.AddObjectToByteList(Type.Value, Data);
            Helpers.AddObjectToByteList(Duration.Value, Data);
            Helpers.AddObjectToByteList(x.Value, Data);
            Helpers.AddObjectToByteList(y.Value, Data);
            Helpers.AddObjectToByteList(zrot.Value, Data);
            Helpers.AddObjectToByteList(zoom.Value, Data);

            ScriptDataHelpers.ErrorIfExpectedLenWrong(Data, 36);

            return Data.ToArray();
        }
    }
}

