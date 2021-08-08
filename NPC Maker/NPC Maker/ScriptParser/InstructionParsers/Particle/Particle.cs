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

                object PosX = (float)0;
                object PosY = (float)0;
                object PosZ = (float)0;
                byte PosXT = 0;
                byte PosYT = 0;
                byte PosZT = 0;

                object AccelX = (float)0;
                object AccelY = (float)0;
                object AccelZ = (float)0;
                byte AccelXT = 0;
                byte AccelYT = 0;
                byte AccelZT = 0;

                object VelX = (float)0;
                object VelY = (float)0;
                object VelZ = (float)0;
                byte VelXT = 0;
                byte VelYT = 0;
                byte VelZT = 0;

                object[] PrimRGBA = new object[] { (float)0, (float)0, (float)0, (float)0 };
                byte[] PrimRGBAVarT = new byte[] { 0, 0, 0, 0 };
                object[] SecRGBA = new object[] { (float)0, (float)0, (float)0, (float)0 };
                byte[] SecRGBAVarT = new byte[] { 0, 0, 0, 0 };

                object Scale = (float)0;
                byte ScaleT = 0;

                object ScaleUpdate = (float)0;
                byte ScaleUpdateT = 0;

                object Life = (float)0;
                byte LifeT = 0;

                object Var = (float)0;
                byte VarT = 0;

                object Yaw = (float)0;
                byte YawT = 0;

                object DListIndex = (float)-1;
                byte DListIndexT = 0;

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
                                ScriptHelpers.GetXYZPos(Split, 2, 3, 4, ref PosXT, ref PosYT, ref PosZT, ref PosX, ref PosY, ref PosZ);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.ACCELERATION:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 4);
                                ScriptHelpers.GetXYZPos(Split, 1, 2, 3, ref AccelXT, ref AccelYT, ref AccelZT, ref AccelX, ref AccelY, ref AccelZ);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.VELOCITY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 4);
                                ScriptHelpers.GetXYZPos(Split, 1, 2, 3, ref VelXT, ref VelYT, ref VelZT, ref VelX, ref VelY, ref VelZ);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.COLOR1:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 5);
                                ScriptHelpers.GetRGBA(Split, 1, ref PrimRGBA, ref PrimRGBAVarT);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.COLOR2:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 5);
                                ScriptHelpers.GetRGBA(Split, 1, ref SecRGBA, ref SecRGBAVarT);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.SCALE:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                ScaleT = ScriptHelpers.GetVarType(Split, 1);
                                Scale = ScriptHelpers.GetValueByType(Split, 1, ScaleT, Int16.MinValue, Int16.MaxValue);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.SCALE_UPDATE:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                ScaleUpdateT = ScriptHelpers.GetVarType(Split, 1);
                                ScaleUpdate = ScriptHelpers.GetValueByType(Split, 1, ScaleUpdateT, Int16.MinValue, Int16.MaxValue);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.SCALE_UPDATE_DOWN:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                VarT = ScriptHelpers.GetVarType(Split, 1);
                                Var = ScriptHelpers.GetValueByType(Split, 1, VarT, Int16.MinValue, Int16.MaxValue);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.OPACITY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                VarT = ScriptHelpers.GetVarType(Split, 1);
                                Var = ScriptHelpers.GetValueByType(Split, 1, VarT, 0, byte.MaxValue);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.RANDOMIZE_XZ:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                VarT = ScriptHelpers.GetVarType(Split, 1);
                                Var = ScriptHelpers.GetValueByType(Split, 1, VarT, 0, 1);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.SCORE_AMOUNT:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                VarT = ScriptHelpers.GetVarType(Split, 1);
                                Var = ScriptHelpers.GetValueByType(Split, 1, VarT, 0, 3);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.COUNT:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                VarT = ScriptHelpers.GetVarType(Split, 1);
                                Var = ScriptHelpers.GetValueByType(Split, 1, VarT, 0, UInt16.MaxValue);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.LIGHTPOINT_COLOR:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                VarT = ScriptHelpers.GetVarType(Split, 1);
                                Var = ScriptHelpers.Helper_GetEnumByNameOrVarType(Split, 1, VarT, typeof(Lists.LightPointColors), ParseException.UnrecognizedParticle(Split));
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.FADE_DELAY:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                PrimRGBAVarT[0] = ScriptHelpers.GetVarType(Split, 1);
                                PrimRGBA[0] = ScriptHelpers.GetValueByType(Split, 1, VarT, 0, 255);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.DURATION:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                LifeT = ScriptHelpers.GetVarType(Split, 1);
                                Life = ScriptHelpers.GetValueByType(Split, 1, LifeT, 0, Int16.MaxValue);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.YAW:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                YawT = ScriptHelpers.GetVarType(Split, 1);
                                Yaw = ScriptHelpers.GetValueByType(Split, 1, YawT, 0, UInt16.MaxValue);
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.DLIST:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                DListIndexT = ScriptHelpers.GetVarType(Split, 1);
                                DListIndex = ScriptHelpers.Helper_GetDListID(Split, 1, DListIndexT, Entry.ExtraDisplayLists);
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
                    PosXT = PosXT,
                    PosYT = PosYT,
                    PosZT = PosZT,
                    AccelX = AccelX,
                    AccelY = AccelY,
                    AccelZ = AccelZ,
                    AccelXT = AccelXT,
                    AccelYT = AccelYT,
                    AccelZT = AccelZT,
                    VelX = VelX,
                    VelY = VelY,
                    VelZ = VelZ,
                    VelXT = VelXT,
                    VelYT = VelYT,
                    VelZT = VelZT,
                    PrimRGBA = PrimRGBA,
                    PrimRGBAVarT = PrimRGBAVarT,
                    SecRGBA = SecRGBA,
                    SecRGBAVarT = SecRGBAVarT,
                    Scale = Scale,
                    ScaleT = ScaleT,
                    ScaleUpdate = ScaleUpdate,
                    ScaleUpdateT = ScaleUpdateT,
                    Life = Life,
                    LifeT = LifeT,
                    Var = Var,
                    VarT = VarT,
                    Yaw = Yaw,
                    YawT = YawT,
                    DListIndex = DListIndex,
                    DListIndexT = DListIndexT,
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
            for (int i = LineNo + 1; i < Lines.Count(); i++)
            {
                if (Lines[i].Split(' ')[0].ToUpper() == Lists.Instructions.PARTICLE.ToString())
                {
                    int j = i;

                    i = GetCorrespondingEndParticle(Lines, i);

                    if (i < 0)
                        throw ParseException.ParticleNotClosed(Lines[j]);

                    continue;
                }

                if (Lines[i].ToUpper().Trim() == Lists.Keyword_EndParticle)
                    return i;
            }

            return -1;
        }
    }
}
