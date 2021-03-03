using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private Instruction ParseChangeScriptInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);
                int SubID = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.ScriptChangeSubtypes));

                byte VarType = ScriptHelpers.GetVarType(SplitLine, 2);
                UInt32 ActorID = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 2, VarType, 0, UInt16.MaxValue));

                byte IndexVarType = ScriptHelpers.GetVarType(SplitLine, 3);
                UInt32 ActorScriptIndexID = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, 3, VarType, 0, byte.MaxValue));

                switch (SubID)
                {
                    case (int)Lists.ScriptChangeSubtypes.OVERWRITE:
                        {
                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 5);
                            return new InstructionChangeScript((byte)SubID, ActorID, VarType, ActorScriptIndexID, IndexVarType, SplitLine[4]);
                        }
                    case (int)Lists.ScriptChangeSubtypes.RESTORE:
                        {
                            ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 4);
                            return new InstructionChangeScript((byte)SubID, ActorID, VarType, ActorScriptIndexID, IndexVarType, "__NONE__");
                        }
                    default: 
                        throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                }
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionChangeScript(0, 0, 0, 0, 0, "__NONE__");
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }
    }
}
