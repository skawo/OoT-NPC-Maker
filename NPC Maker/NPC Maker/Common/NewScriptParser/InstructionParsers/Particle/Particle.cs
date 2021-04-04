using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private Instruction ParseParticleInstruction(List<string> Lines, string[] SplitLine, ref int LineNo)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsNotEq(SplitLine, 1);

                int End = GetCorrespondingEndParticle(Lines, LineNo);

                Int32 Type = 0;
                byte TypeT = 0;

                byte RelativePos = 0;

                float PosX = 0;
                float PosY = 0;
                float PosZ = 0;
                byte PosXT = 0;
                byte PosYT = 0;
                byte PosZT = 0;

                float AccelX = 0;
                float AccelY = 0;
                float AccelZ = 0;
                byte AccelXT = 0;
                byte AccelYT = 0;
                byte AccelZT = 0;

                float VelX = 0;
                float VelY = 0;
                float VelZ = 0;
                byte VelXT = 0;
                byte VelYT = 0;
                byte VelZT = 0;

                UInt32[] PrimRGBA = new UInt32[] { 0, 0, 0, 0 };
                byte[] PrimRGBAVarT = new byte[] { 0, 0, 0, 0 };
                UInt32[] SecRGBA = new UInt32[] { 0, 0, 0, 0 };
                byte[] SecRGBAVarT = new byte[] { 0, 0, 0, 0 };

                Int32 Scale = 0;
                byte ScaleT = 0;

                Int32 ScaleUpdate = 0;
                byte ScaleUpdateT = 0;

                Int32 RadiusUpdateD = 0;
                byte RadiusUpdateDT = 0;

                Int32 Life = 0;
                byte LifeT = 0;

                Int32 NumBolts = 0;
                byte NumBoltsT = 0;

                Int32 Yaw = 0;
                byte YawT = 0;

                Int32 DListIndex = 0;
                byte DListIndexT = 0;

                Int32 ColorType = 0;
                byte ColorTypeT = 0;

                string LabelJumpIfFound = "__RETURN__";

                Int32 Alpha = 0;
                byte AlphaT = 0;



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

                    switch (SubID)
                    {
                        case (int)Lists.ParticleSubOptions.TYPE:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);

                                TypeT = ScriptHelpers.GetVarType(Split, 1);
                                Type = Convert.ToInt32(ScriptHelpers.Helper_GetEnumByNameOrVarType(Split, 1, TypeT, typeof(Lists.ParticleTypes), ParseException.UnrecognizedParticle(Split)));

                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.POSITION:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 5);

                                RelativePos = (byte)(ScriptHelpers.Helper_GetEnumByName(Split, 1, typeof(Lists.SpawnPosParams), ParseException.UnrecognizedParameter(Split)));
                                ScriptHelpers.GetXYZPos(Split, 2, 3, 4, ref PosXT, ref PosYT, ref PosZT, ref PosX, ref PosY, ref PosZ);

                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.ACCEL:
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
                        case (int)Lists.ParticleSubOptions.RADIUS:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);

                                ScaleT = ScriptHelpers.GetVarType(Split, 1);
                                Scale = Convert.ToInt32(ScriptHelpers.GetValueByType(Split, 1, ScaleT, Int16.MinValue, Int16.MaxValue));

                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.SCALE_UPDATE:
                        case (int)Lists.ParticleSubOptions.RADIUS_UPDATE:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);

                                ScaleUpdateT = ScriptHelpers.GetVarType(Split, 1);
                                ScaleUpdate = Convert.ToInt32(ScriptHelpers.GetValueByType(Split, 1, ScaleUpdateT, Int16.MinValue, Int16.MaxValue));

                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.RADIUS_UPDATE_DOWN:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);

                                RadiusUpdateDT = ScriptHelpers.GetVarType(Split, 1);
                                RadiusUpdateD = Convert.ToInt32(ScriptHelpers.GetValueByType(Split, 1, RadiusUpdateDT, Int16.MinValue, Int16.MaxValue));

                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.NUM_BOLTS:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);

                                NumBoltsT = ScriptHelpers.GetVarType(Split, 1);
                                NumBolts = Convert.ToInt32(ScriptHelpers.GetValueByType(Split, 1, NumBoltsT, 0, UInt16.MaxValue));

                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.YAW:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);

                                YawT = ScriptHelpers.GetVarType(Split, 1);
                                Yaw = Convert.ToInt32(ScriptHelpers.GetValueByType(Split, 1, YawT, 0, UInt16.MaxValue));

                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.DLIST:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);

                                DListIndexT = ScriptHelpers.GetVarType(Split, 1);
                                DListIndex = Convert.ToInt32(ScriptHelpers.Helper_GetDListID(Split, 1, DListIndexT, Entry.ExtraDisplayLists));

                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.LIGHTPOINT_COLOR:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);

                                ColorTypeT = ScriptHelpers.GetVarType(Split, 1);
                                ColorType = Convert.ToInt32(ScriptHelpers.Helper_GetEnumByNameOrVarType(Split, 1, ColorTypeT, typeof(Lists.LightPointColors), ParseException.UnrecognizedParticle(Split)));

                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.DETECTED_LABEL:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);
                                LabelJumpIfFound = Split[1];
                                continue;
                            }
                        case (int)Lists.ParticleSubOptions.ALPHA:
                            {
                                ScriptHelpers.ErrorIfNumParamsNotEq(Split, 2);

                                AlphaT = ScriptHelpers.GetVarType(Split, 1);
                                Alpha = Convert.ToInt32(ScriptHelpers.GetValueByType(Split, 1, AlphaT, 0, byte.MaxValue));

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
                    TypeT = TypeT,
                    RelativePos = RelativePos,
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
                    RadiusUpdateD = RadiusUpdateD,
                    RadiusUpdateDT = RadiusUpdateDT,
                    Alpha = Alpha,
                    AlphaT = AlphaT,
                    Life = Life,
                    LifeT = LifeT,
                    NumBolts = NumBolts,
                    NumBoltsT = NumBoltsT,
                    Yaw = Yaw,
                    YawT = YawT,
                    DListIndex = DListIndex,
                    DListIndexT = DListIndexT,
                    ColorType = ColorType,
                    ColorTypeT = ColorTypeT,
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

        private int GetCorrespondingEndParticle(List<string> Lines, int LineNo)
        {
            for (int i = LineNo + 1; i < Lines.Count(); i++)
            {
                if (Lines[i].Split(' ')[0].ToUpper() == Lists.Instructions.PARTICLE.ToString())
                {
                    int j = i;

                    i = GetCorrespondingEndWhile(Lines, i);

                    if (i < 0)
                        throw ParseException.IfNotClosed(Lines[j]);

                    continue;
                }

                if (Lines[i].ToUpper().Trim() == Lists.Keyword_EndParticle)
                    return i;
            }

            return -1;
        }
    }
}
