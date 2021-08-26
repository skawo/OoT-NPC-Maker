using System;

namespace NPC_Maker.Scripts
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

                var ActorNum1 = new ScriptVarVal();
                var ActorNum2 = new ScriptVarVal();

                SubjectType = (byte)(GetActor(SplitLine, 1, ref ActorNum1));

                switch (SubjectType)
                {
                    case (byte)Lists.TargetActorSubtypes.NPCMAKER:
                        {
                            ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 4);

                            FaceType = GetFaceType(SplitLine, 3);
                            TargetType = (byte)GetActor(SplitLine, 4, ref ActorNum2);

                            if (TargetType == SubjectType && ActorNum1.Value == ActorNum2.Value && ActorNum1.Vartype == ActorNum2.Vartype)
                                throw ParseException.FaceCantBeSame(SplitLine);

                            break;
                        }
                    case (byte)Lists.TargetActorSubtypes.ACTOR_ID:
                        {
                            ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 5);

                            FaceType = GetFaceType(SplitLine, 4);
                            TargetType = (byte)GetActor(SplitLine, 5, ref ActorNum2);

                            if (TargetType == SubjectType && ActorNum1.Value == ActorNum2.Value && ActorNum1.Vartype == ActorNum2.Vartype)
                                throw ParseException.FaceCantBeSame(SplitLine);

                            break;
                        }
                    case (byte)Lists.TargetActorSubtypes.PLAYER:
                    case (byte)Lists.TargetActorSubtypes.SELF:
                    case (int)Lists.TargetActorSubtypes.REF_ACTOR:
                        {
                            ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, 3);

                            FaceType = GetFaceType(SplitLine, 2);
                            TargetType = (byte)GetActor(SplitLine, 3, ref ActorNum2);

                            if (TargetType == SubjectType)
                                throw ParseException.FaceCantBeSame(SplitLine);

                            break;
                        }
                    default:
                        throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
                }

                return new InstructionFace(SubjectType, FaceType, TargetType, ActorNum1, ActorNum2);
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

        private byte GetFaceType(string[] SplitLine, int Index)
        { 
            int ft = ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.FaceSubtypes), Index);

            if (ft < 0)
                throw ParseException.UnrecognizedInstruction(SplitLine);

            return (byte)ft;
        }

        private int GetActor(string[] SplitLine, int Index, ref ScriptVarVal NumActor)
        {
            int Type = Convert.ToInt32(ScriptHelpers.GetSubIDValue(SplitLine, typeof(Lists.TargetActorSubtypes), Index));

            switch (Type)
            {
                case (int)Lists.TargetActorSubtypes.NPCMAKER:
                    {
                        ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, Index + 2);
                        ScriptHelpers.GetScriptVarVal(SplitLine, Index + 1, 0, UInt16.MaxValue, ref NumActor);

                        break;
                    }
                case (int)Lists.TargetActorSubtypes.ACTOR_ID:
                    {
                        ScriptHelpers.ErrorIfNumParamsSmaller(SplitLine, Index + 2);
                        NumActor = ScriptHelpers.Helper_GetActorId(SplitLine, Index + 1);
                        break;
                    }
                case (int)Lists.TargetActorSubtypes.PLAYER: break;
                case (int)Lists.TargetActorSubtypes.SELF: break;
                case (int)Lists.TargetActorSubtypes.REF_ACTOR:
                default:
                    throw ParseException.UnrecognizedFunctionSubtype(SplitLine);
            }

            return Type;
        }


    }
}
