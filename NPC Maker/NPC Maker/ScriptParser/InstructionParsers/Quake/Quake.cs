using System;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseQuakeInstruction(string[] SplitLine)
        {
            try
            {
                try
                {
                    ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                    var Type = ScriptHelpers.GetScriptVarVal(SplitLine, 1, typeof(Lists.QuakeTypes), ParseException.UnrecognizedQuake(SplitLine));
                    var Speed = ScriptHelpers.GetScriptVarVal(SplitLine, 2, 0, Int16.MaxValue);
                    var Duration = ScriptHelpers.GetScriptVarVal(SplitLine, 3, 0, Int16.MaxValue);

                    var x = new ScriptVarVal(0, 0);
                    var y = new ScriptVarVal(1, 0);
                    var zoom = new ScriptVarVal(0xFA, 0);
                    var zrot = new ScriptVarVal(1, 0);

                    if (SplitLine.Length > 4)
                        x = ScriptHelpers.GetScriptVarVal(SplitLine, 4, 0, Int16.MaxValue);

                    if (SplitLine.Length > 5)
                        y = ScriptHelpers.GetScriptVarVal(SplitLine, 5, 0, Int16.MaxValue);

                    if (SplitLine.Length > 6)
                        zoom = ScriptHelpers.GetScriptVarVal(SplitLine, 6, 0, Int16.MaxValue);

                    if (SplitLine.Length > 7)
                        zrot = ScriptHelpers.GetScriptVarVal(SplitLine, 7, 0, Int16.MaxValue);


                    return new InstructionQuake(Speed, Type, Duration, x, y, zrot, zoom);

                }
                catch (ParseException pEx)
                {
                    outScript.ParseErrors.Add(pEx);
                    return new InstructionNop();
                }
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }
    }
}
