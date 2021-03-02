using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private Instruction ParseSpawnInstruction(List<string> Lines, string[] SplitLine, ref int LineNo)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 1);

                int End = GetCorrespondingEndSpawn(Lines, LineNo);

                UInt16 ActorID = 1;
                UInt16 ActorVar = 0;
                decimal PosX = 0;
                decimal PosY = 0;
                decimal PosZ = 0;
                Int16 RotX = 0;
                Int16 RotY = 0;
                Int16 RotZ = 0;
                bool RelativePos = false;

                List<string> Params = Lines.Skip(LineNo + 1).Take(End - LineNo - 1).ToList();

                if (End == -1)
                    throw ParseException.SpawnNotClosed(SplitLine);

                LineNo = End;

                foreach (string Par in Params)
                {
                    string[] Split = Par.Split(' ');

                    if (Split.Count() == 0)
                        continue;

                    int SubID = (int)System.Enum.Parse(typeof(Lists.SpawnParams), Split[0].ToUpper());

                    switch (SubID)
                    {
                        case (int)Lists.SpawnParams.ACTOR_ID:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);

                                UInt32? Actor = ScriptHelpers.Helper_GetActorId(Split[1]);

                                if (Actor == null)
                                    Actor = Convert.ToUInt32(ScriptHelpers.GetValueAndCheckRange(Split, 1, 0, Dicts.Actors.Max(x => x).Value));

                                ActorID = (UInt16)Actor;

                                continue;
                            }
                        case (int)Lists.SpawnParams.VARIABLE:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                ActorVar = Convert.ToUInt16(ScriptHelpers.GetValueAndCheckRange(Split, 1, 0, UInt16.MaxValue));

                                continue;
                            }
                        case (int)Lists.SpawnParams.POSITION:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 5);

                                RelativePos = (ScriptHelpers.Helper_GetEnumByName(Split, 1, typeof(Lists.SpawnPosParams), ParseException.UnrecognizedParameter(Split)) == 0);

                                PosX = Convert.ToDecimal(Split[2]);
                                PosY = Convert.ToDecimal(Split[3]);
                                PosZ = Convert.ToDecimal(Split[4]);
                                continue;
                            }
                        case (int)Lists.SpawnParams.ROTATION:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 4);

                                RotX = Convert.ToInt16(ScriptHelpers.GetValueAndCheckRange(Split, 1, Int16.MinValue, Int16.MaxValue));
                                RotY = Convert.ToInt16(ScriptHelpers.GetValueAndCheckRange(Split, 2, Int16.MinValue, Int16.MaxValue));
                                RotZ = Convert.ToInt16(ScriptHelpers.GetValueAndCheckRange(Split, 3, Int16.MinValue, Int16.MaxValue));
                                continue;
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }
                }

                return new InstructionSpawn(RelativePos, PosX, PosY, PosZ, RotX, RotY, RotZ, ActorID, ActorVar);
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionSpawn(false, 0, 0, 0, 0, 0, 0, 1, 0);
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
