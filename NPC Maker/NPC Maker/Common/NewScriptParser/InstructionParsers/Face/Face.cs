using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public partial class ScriptParser
    {
        private Instruction ParseFaceInstruction(string[] SplitLine)
        {
            try
            {
                ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 2);

                byte SubjectType = 0;
                byte FaceType = 0;
                byte TargetType = 0;

                UInt32 ActorNum1 = 0;
                Int32 ActorCat1 = 0;
                byte ANumVarT1 = 0;
                byte ACatVarT1 = 0;
                UInt32 ActorNum2 = 0;
                Int32 ActorCat2 = 0;
                byte ANumVarT2 = 0;
                byte ACatVarT2 = 0;

                SubjectType = (byte)GetActor(SplitLine, 1, ref ActorNum1, ref ActorCat1, ref ANumVarT1, ref ANumVarT2);

                switch (SubjectType)
                {
                    case (byte)Lists.TargetActorSubtypes.CONFIG_ID:
                        {
                            ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                            FaceType = GetFaceType(SplitLine, 3);
                            TargetType = (byte)GetActor(SplitLine, 4, ref ActorNum2, ref ActorCat2, ref ANumVarT1, ref ACatVarT2);

                            if (TargetType == SubjectType && ActorNum1 == ActorNum2)
                                throw ParseException.FaceCantBeSame(SplitLine);

                            break;
                        }
                    case (byte)Lists.TargetActorSubtypes.ACTOR_ID:
                        {
                            ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 5);

                            FaceType = GetFaceType(SplitLine, 4);
                            TargetType = (byte)GetActor(SplitLine, 5, ref ActorNum2, ref ActorCat2, ref ANumVarT1, ref ACatVarT2);

                            if (TargetType == SubjectType && ActorNum1 == ActorNum2)
                                throw ParseException.FaceCantBeSame(SplitLine);

                            break;
                        }
                    case (byte)Lists.TargetActorSubtypes.PLAYER:
                    case (byte)Lists.TargetActorSubtypes.SELF:
                        {
                            ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

                            FaceType = GetFaceType(SplitLine, 2);
                            TargetType = (byte)GetActor(SplitLine, 3, ref ActorNum2, ref ActorCat2, ref ANumVarT1, ref ACatVarT2);

                            if (TargetType == SubjectType)
                                throw ParseException.FaceCantBeSame(SplitLine);

                            break;
                        }
                    default:
                        throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                }

                return new InstructionFace(SubjectType, FaceType, TargetType, ActorNum1, ActorCat1, ANumVarT1, ACatVarT1, ActorNum2, ActorCat2, ANumVarT2, ACatVarT2);
            }
            catch (ParseException pEx)
            {
                outScript.ParseErrors.Add(pEx);
                return new InstructionFace(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            }
            catch (Exception)
            {
                outScript.ParseErrors.Add(ParseException.GeneralError(SplitLine));
                return new InstructionNop();
            }
        }

        private byte GetFaceType(string[] SplitLine, int Index)
        {
            return (byte)ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.FaceSubtypes), Index);
        }

        private int GetActor(string[] SplitLine, int Index, ref UInt32 NumActor, ref Int32 NumCat, ref byte NumActorT, ref byte NumCatT)
        {
            int Type = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.TargetActorSubtypes), Index);

            switch (Type)
            {
                case (int)Lists.TargetActorSubtypes.CONFIG_ID:
                    {
                        ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, Index + 2);

                        NumActorT = ScriptHelpers.GetVarType(SplitLine, Index + 1);
                        NumActor = Convert.ToUInt32(ScriptHelpers.GetValueByType(SplitLine, Index + 1, NumActorT, 0, UInt16.MaxValue));
                        break;
                    }
                case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                    {
                        ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, Index + 3);

                        NumActorT = ScriptHelpers.GetVarType(SplitLine, Index + 1);
                        NumActor = (UInt32)ScriptHelpers.Helper_GetActorId(SplitLine, Index + 1, NumActorT);

                        NumCatT = ScriptHelpers.GetVarType(SplitLine, Index + 2);
                        NumCat = (Int32)ScriptHelpers.Helper_GetActorCategory(SplitLine, Index + 2, NumCatT);

                        break;
                    }
                case (int)Lists.TargetActorSubtypes.PLAYER: break;
                case (int)Lists.TargetActorSubtypes.SELF: break;
                default:
                    throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
            }

            return Type;
        }


    }
}
