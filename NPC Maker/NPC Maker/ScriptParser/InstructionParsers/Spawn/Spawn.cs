using System;
using System.Collections.Generic;
using System.Linq;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseSpawnInstruction(List<string> Lines, string[] SplitLine, ref int LineNo)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);

                int End = GetCorrespondingEndSpawn(Lines, LineNo);

                ScriptVarVal ActorVar = new ScriptVarVal();
                ScriptVarVal PosX = new ScriptVarVal();
                ScriptVarVal PosY = new ScriptVarVal();
                ScriptVarVal PosZ = new ScriptVarVal();

                ScriptVarVal RotX = new ScriptVarVal();
                ScriptVarVal RotY = new ScriptVarVal();
                ScriptVarVal RotZ = new ScriptVarVal();

                byte PosType = 0;

                ScriptVarVal ActorID = ScriptHelpers.Helper_GetActorId(SplitLine, 1);

                List<string> Params = Lines.Skip(LineNo + 1).Take(End - LineNo - 1).ToList();

                if (End == -1)
                    throw ParseException.SpawnNotClosed(SplitLine);

                LineNo = End;

                bool[] Used = new bool[4] { false, false, false, false };

                foreach (string Par in Params)
                {
                    string[] Split = Par.Split(' ');

                    if (Split.Count() == 0)
                        continue;

                    if (!Enum.IsDefined(typeof(Lists.SpawnParams), Split[0].ToUpper()))
                        throw ParseException.UnrecognizedFunctionSubtype(Split);

                    int SubID = (int)System.Enum.Parse(typeof(Lists.SpawnParams), Split[0].ToUpper());

                    if (Used[SubID])
                        throw ParseException.DuplicateSpawnInstruction(Split);

                    switch (SubID)
                    {
                        case (int)Lists.SpawnParams.VARIABLE:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                ScriptHelpers.GetScriptVarVal(Split, 1, 0, UInt16.MaxValue, ref ActorVar);

                                Used[(int)Lists.SpawnParams.VARIABLE] = true;

                                continue;
                            }
                        case (int)Lists.SpawnParams.POSITION:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 5);

                                PosType += Convert.ToByte(ScriptHelpers.Helper_GetEnumByName(Split, 1, typeof(Lists.SpawnPosParams), ParseException.UnrecognizedParameter(Split)));
                                ScriptHelpers.GetXYZPos(Split, 2, 3, 4, ref PosX, ref PosY, ref PosZ);

                                Used[(int)Lists.SpawnParams.POSITION] = true;

                                continue;
                            }
                        case (int)Lists.SpawnParams.ROTATION:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 4);
                                ScriptHelpers.GetXYZ(Split, 1, 2, 3, ref RotX, ref RotY, ref RotZ, Int16.MinValue, Int16.MaxValue);

                                Used[(int)Lists.SpawnParams.ROTATION] = true;

                                continue;
                            }
                        case (int)Lists.SpawnParams.SET_AS_REF:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 1);
                                PosType += 10;

                                Used[(int)Lists.SpawnParams.SET_AS_REF] = true;

                                continue;
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }
                }

                return new InstructionSpawn((byte)PosType, PosX, PosY, PosZ, RotX, RotY, RotZ, ActorID, ActorVar);
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionNop();
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }

        private int GetCorrespondingEndSpawn(List<string> Lines, int LineNo)
        {
            for (int i = LineNo + 1; i < Lines.Count(); i++)
            {
                if (Lines[i].Split(' ')[0].ToUpper() == Lists.Instructions.SPAWN.ToString())
                {
                    int j = i;

                    i = GetCorrespondingEndWhile(Lines, i);

                    if (i < 0)
                        throw ParseException.IfNotClosed(Lines[j]);

                    continue;
                }

                if (Lines[i].ToUpper().Trim() == Lists.Keyword_EndSpawn)
                    return i;
            }

            return -1;
        }
    }
}
