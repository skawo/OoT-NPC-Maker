using System;
using System.Collections.Generic;
using System.Linq;

namespace NPC_Maker.Scripts
{
    public partial class ScriptParser
    {
        private Instruction ParseParticleInstruction(List<string> Lines, string[] SplitLine, ref int LineNo)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 2);

                int End = GetCorrespondingEndParticle(Lines, LineNo);

                byte Type = 0;

                byte PosType = 0;

                var PosX = new ScriptVarVal();
                var PosY = new ScriptVarVal();
                var PosZ = new ScriptVarVal();

                var AccelX = new ScriptVarVal();
                var AccelY = new ScriptVarVal();
                var AccelZ = new ScriptVarVal();

                var VelX = new ScriptVarVal();
                var VelY = new ScriptVarVal();
                var VelZ = new ScriptVarVal();

                var PrimR = new ScriptVarVal();
                var PrimG = new ScriptVarVal();
                var PrimB = new ScriptVarVal();
                var PrimA = new ScriptVarVal();

                var SecR = new ScriptVarVal();
                var SecG = new ScriptVarVal();
                var SecB = new ScriptVarVal();
                var SecA = new ScriptVarVal();

                var Scale = new ScriptVarVal();
                var ScaleUpdate = new ScriptVarVal();
                var Life = new ScriptVarVal();
                var Var = new ScriptVarVal();
                var Yaw = new ScriptVarVal();
                var DListIndex = new ScriptVarVal(-1);

                string LabelJumpIfFound = "__RETURN__";

                Type = Convert.ToByte(ScriptHelpers.Helper_GetEnumByName(SplitLine, 1, typeof(Lists.ParticleTypes), ParseException.UnrecognizedParticle(SplitLine)));

                List<string> Params = Lines.Skip(LineNo + 1).Take(End - LineNo - 1).ToList();

                if (End == -1)
                    throw ParseException.ParticleNotClosed(SplitLine);

                LineNo = End;

                foreach (string Par in Params)
                {
                    string[] Split = Par.Split(' ');

                    if (Split.Count() == 0)
                        continue;

                    if (!Enum.IsDefined(typeof(Lists.ParticleSubOptions), Split[0].ToUpper()))
                        throw ParseException.UnrecognizedFunctionSubtype(Split);

                    int SubID = (int)System.Enum.Parse(typeof(Lists.ParticleSubOptions), Split[0].ToUpper());
                    CheckParticleSubValid(SplitLine, (Lists.ParticleTypes)Type, (Lists.ParticleSubOptions)SubID);

                    switch (SubID)
                    {
                        case (int)Lists.ParticleSubOptions.POSITION:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 5);
                                PosType = Convert.ToByte(ScriptHelpers.Helper_GetEnumByName(Split, 1, typeof(Lists.SpawnPosParams), ParseException.UnrecognizedParameter(Split)));
                                ScriptHelpers.GetXYZPos(Split, 2, 3, 4, ref PosX, ref PosY, ref PosZ);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.ACCELERATION:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 4);
                                ScriptHelpers.GetXYZPos(Split, 1, 2, 3, ref AccelX, ref AccelY, ref AccelZ);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.VELOCITY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 4);
                                ScriptHelpers.GetXYZPos(Split, 1, 2, 3, ref VelX, ref VelY, ref VelZ);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.COLOR1:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 5);
                                ScriptHelpers.GetRGBorRGBA(Split, 1, ref PrimR, ref PrimG, ref PrimB, ref PrimA);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.COLOR2:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 5);
                                ScriptHelpers.GetRGBorRGBA(Split, 1, ref SecR, ref SecG, ref SecB, ref SecA);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.SCALE:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                ScriptHelpers.GetScriptVarVal(Split, 1, Int16.MinValue, Int16.MaxValue, ref Scale);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.SCALE_UPDATE:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                ScriptHelpers.GetScriptVarVal(Split, 1, Int16.MinValue, Int16.MaxValue, ref ScaleUpdate);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.SCALE_UPDATE_DOWN:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                ScriptHelpers.GetScriptVarVal(Split, 1, Int16.MinValue, Int16.MaxValue, ref Var);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.OPACITY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                ScriptHelpers.GetScriptVarVal(Split, 1, 0, byte.MaxValue, ref Var);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.RANDOMIZE_XZ:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                ScriptHelpers.GetScriptVarVal(Split, 1, 0, 1, ref Var);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.SCORE_AMOUNT:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                ScriptHelpers.GetScriptVarVal(Split, 1, 0, 3, ref Var);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.COUNT:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                ScriptHelpers.GetScriptVarVal(Split, 1, 0, UInt16.MaxValue, ref Var);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.LIGHTPOINT_COLOR:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                ScriptHelpers.GetScriptVarVal(Split, 1, typeof(Lists.LightPointColors), ParseException.UnrecognizedParameter(Split), ref Var);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.FADE_DELAY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                ScriptHelpers.GetScriptVarVal(Split, 1, 0, byte.MaxValue, ref PrimR);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.DURATION:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                ScriptHelpers.GetScriptVarVal(Split, 1, 0, Int16.MaxValue, ref Life);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.YAW:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                ScriptHelpers.GetScriptVarVal(Split, 1, 0, Int16.MaxValue, ref Yaw);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.DLIST:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                DListIndex = ScriptHelpers.Helper_GetDListID(Split, 1, Entry.ExtraDisplayLists);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.SPOTTED:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                LabelJumpIfFound = Split[1];
                                continue;
                            }
                        default:
                            throw ParseException.UnrecognizedFunctionSubtype(Split);
                    }
                }

                return new InstructionParticle()
                {
                    ID = (byte)Lists.Instructions.PARTICLE,
                    Type = Type,
                    PosType = PosType,
                    PosX = PosX,
                    PosY = PosY,
                    PosZ = PosZ,
                    AccelX = AccelX,
                    AccelY = AccelY,
                    AccelZ = AccelZ,
                    VelX = VelX,
                    VelY = VelY,
                    VelZ = VelZ,
                    PrimR = PrimR,
                    PrimG = PrimG,
                    PrimB = PrimB,
                    PrimA = PrimA,
                    SecR = SecR,
                    SecG = SecG,
                    SecB = SecB,
                    SecA = SecA,
                    Scale = Scale,
                    ScaleUpdate = ScaleUpdate,
                    Life = Life,
                    Var = Var,
                    Yaw = Yaw,
                    DListIndex = DListIndex,
                    LabelJumpIfFound = new InstructionLabel(LabelJumpIfFound)
                };
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

        private void CheckParticleSubValid(string[] SplitLine, Lists.ParticleTypes Type, Lists.ParticleSubOptions ParticleSub)
        {
            if (!Dicts.UsableParticleSubOptions[Type].Contains(ParticleSub))
                throw ParseException.InvalidParticleSub(SplitLine, Type.ToString(), GetValidParticleSubs(Type));
        }

        private string GetValidParticleSubs(Lists.ParticleTypes Type)
        {
            return String.Join(",", Dicts.UsableParticleSubOptions[Type].ToArray());
        }

        private int GetCorrespondingEndParticle(List<string> Lines, int LineNo)
        {
            return ScriptHelpers.GetCorresponding(Lines, LineNo, Lists.Instructions.PARTICLE.ToString(), Lists.Keyword_EndParticle);
        }
    }
}
