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

                UInt32 ActorID = 1;
                UInt32 ActorVar = 0;
                float PosX = 0;
                float PosY = 0;
                float PosZ = 0;
                Int32 RotX = 0;
                Int32 RotY = 0;
                Int32 RotZ = 0;
                byte RelativePos = 0;

                byte ActorIDVarT = 0;
                byte ActorVarT = 0;
                byte PosXT = 0;
                byte PosYT = 0;
                byte PosZT = 0;
                byte RotXT = 0;
                byte RotYT = 0;
                byte RotZT = 0;


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

                                ActorIDVarT = ScriptHelpers.GetVarType(Split, 1);
                                ActorID = ScriptHelpers.Helper_GetActorId(Split, 1, ActorIDVarT);

                                continue;
                            }
                        case (int)Lists.SpawnParams.VARIABLE:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);

                                ActorVarT = ScriptHelpers.GetVarType(Split, 1);
                                ActorVar = Convert.ToUInt32(ScriptHelpers.GetValueByType(Split, 1, ActorVarT, 0, UInt16.MaxValue));

                                continue;
                            }
                        case (int)Lists.SpawnParams.POSITION:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 5);

                                RelativePos = (byte)(ScriptHelpers.Helper_GetEnumByName(Split, 1, typeof(Lists.SpawnPosParams), ParseException.UnrecognizedParameter(Split)));
                                ScriptHelpers.GetXYZPos(Split, 2, 3, 4, ref PosXT, ref PosYT, ref PosZT, ref PosX, ref PosY, ref PosZ);

                                continue;
                            }
                        case (int)Lists.SpawnParams.ROTATION:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 4);
                                ScriptHelpers.GetXYZRot(Split, 1, 2, 3, ref RotXT, ref RotYT, ref RotZT, ref RotX, ref RotY, ref RotZ);

                                continue;
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                    }
                }

                return new InstructionSpawn((byte)RelativePos, PosX, PosXT, PosY, PosYT, PosZ, PosZT, RotX, RotXT, RotY, RotYT, RotZ, RotZT, ActorID, ActorIDVarT, ActorVar, ActorVarT);
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionSpawn(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
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
