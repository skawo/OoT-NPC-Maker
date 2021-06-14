using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.NewScriptParser
{
    public class ParseException : Exception
    {
        public string Line { get; set; }

        public ParseException(string _Message, string _Line) : base(_Message)
        {
            Line = _Line.Trim();
        }

        public override string ToString()
        {
            return Message + " " + Line;
        }

        public static ParseException GeneralError(string _Line)
        {
            return new ParseException("Error parsing line: ", _Line);
        }

        public static ParseException ProcedureError()
        {
            return new ParseException("Problem with procedures.", "");
        }

        public static ParseException ScriptTooBigError()
        {
            return new ParseException("Script has exceeded the maximum allowed amount of bytes.", "");
        }

        public static ParseException DefineError()
        {
            return new ParseException("Problem with defines.", "");
        }

        public static ParseException GeneralError(string[] _Line)
        {
            return GeneralError(String.Join(" ", _Line));
        }


        public static ParseException UnrecognizedFunctionSubtype(string[] _Line)
        {
            return new ParseException("Unrecognized function subtype: ", String.Join(" ", _Line));
        }

        public static ParseException LabelAlreadyExists(string _Line)
        {
            return new ParseException("Label is defined twice: ", _Line);
        }

        public static ParseException LabelAlreadyExists(string[] _Line)
        {
            return LabelAlreadyExists(String.Join(" ", _Line));
        }

        public static ParseException LabelNameCannotBe(string _Line)
        {
            return new ParseException("Label name cannot be using a keyword: ", _Line);
        }

        public static ParseException LabelNotFound(string _Line)
        {
            return new ParseException("Label not found: ", _Line);
        }

        public static ParseException LabelNotFound(string[] _Line)
        {
            return ParseException.LabelNotFound(String.Join(" ", _Line));
        }

        public static ParseException LabelNameCannotBe(string[] _Line)
        {
            return LabelNameCannotBe(String.Join(" ", _Line));
        }

        public static ParseException ParamCountWrong(string[] _Line)
        {
            return new ParseException("Wrong number of parameters: ", String.Join(" ", _Line));
        }

        public static ParseException ParamCountSmall(string[] _Line)
        {
            return new ParseException("Not enough parameters: ", String.Join(" ", _Line));
        }

        public static ParseException ParamCountBig(string[] _Line)
        {
            return new ParseException("Too many parameters: ", String.Join(" ", _Line));
        }

        public static ParseException ParamOutOfRange(string[] _Line)
        {
            return new ParseException("A parameter is out of range: ", String.Join(" ", _Line));
        }

        public static ParseException IfNotClosed(string _Line)
        {
            return new ParseException("This IF does not have a corresponding ENDIF: ", _Line);
        }

        public static ParseException SpawnNotClosed(string _Line)
        {
            return new ParseException("This SPAWN does not have a corresponding ENDSPAWN: ", _Line);
        }

        public static ParseException TalkNotClosed(string _Line)
        {
            return new ParseException("This TALK does not have a corresponding ENDTALK: ", _Line);
        }

        public static ParseException TradeNotClosed(string _Line)
        {
            return new ParseException("This TRADE does not have a corresponding ENDTRADE: ", _Line);
        }

        public static ParseException FaceCantBeSame(string _Line)
        {
            return new ParseException("Subject and target cannot be the same: ", _Line);
        }

        public static ParseException ParticleNotClosed(string _Line)
        {
            return new ParseException("This PARTICLE does not have a corresponding ENDPARTICLE: ", _Line);
        }

        public static ParseException FaceCantBeSame(string[] _Line)
        {
            return FaceCantBeSame(String.Join(" ", _Line));
        }

        public static ParseException TalkNotClosed(string[] _Line)
        {
            return TalkNotClosed(String.Join(" ", _Line));
        }

        public static ParseException ProcRecursion(string _Line)
        {
            return new ParseException("Procedure recursion is not allowed: ", _Line);
        }

        public static ParseException ProcRecursion(string[] _Line)
        {
            return ProcRecursion(String.Join(" ", _Line));
        }

        public static ParseException ProcedureNotClosed(string _Line)
        {
            return new ParseException("Procedure does not have a corresponding ENDPROC: ", _Line);
        }

        public static ParseException ProcedureNotClosed(string[] _Line)
        {
            return ProcedureNotClosed(String.Join(" ", _Line));
        }

        public static ParseException SpawnNotClosed(string[] _Line)
        {
            return SpawnNotClosed(String.Join(" ", _Line));
        }

        public static ParseException ParticleNotClosed(string[] _Line)
        {
            return ParticleNotClosed(String.Join(" ", _Line));
        }

        public static ParseException IfNotClosed(string[] _Line)
        {
            return IfNotClosed(String.Join(" ", _Line));
        }

        public static ParseException TradeNotClosed(string[] _Line)
        {
            return TradeNotClosed(String.Join(" ", _Line));
        }

        public static ParseException TradeMissingComponents(string _Line)
        {
            return new ParseException("This TRADE instruction lacks one of the necessary members (CORRECT, FAILURE, TALK): ", _Line);
        }

        public static ParseException TradeMissingComponents(string[] _Line)
        {
            return TradeMissingComponents(String.Join(" ", _Line));
        }


        public static ParseException WhileNotClosed(string _Line)
        {
            return new ParseException("This WHILE does not have a corresponding ENDWHILE: ", _Line);
        }

        public static ParseException WhileNotClosed(string[] _Line)
        {
            return WhileNotClosed(String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedCondition(string[] _Line)
        {
            return new ParseException("Not a valid condition: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedTradeItem(string[] _Line)
        {
            return new ParseException("Not a valid trade Item: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedTradeStatus(string[] _Line)
        {
            return new ParseException("Not a valid trade status: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedMask(string[] _Line)
        {
            return new ParseException("Not a valid mask: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedSegmentDataEntry(string[] _Line)
        {
            return new ParseException("Not a valid segment data entry, or segment data entry defined for the wrong segment: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedDList(string[] _Line)
        {
            return new ParseException("Not a valid display list: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedAnimation(string[] _Line)
        {
            return new ParseException("Not a valid animation: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedQuestItem(string[] _Line)
        {
            return new ParseException("Not a valid quest item: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedParticle(string[] _Line)
        {
            return new ParseException("Not a valid particle type: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedButton(string[] _Line)
        {
            return new ParseException("Not a valid button: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedDungeonItem(string[] _Line)
        {
            return new ParseException("Not a valid dungeon item: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedInventoryItem(string[] _Line)
        {
            return new ParseException("Not a valid inventory item: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedAwardItem(string[] _Line)
        {
            return new ParseException("Not a valid award item: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedMovementStyle(string[] _Line)
        {
            return new ParseException("Not a valid movement style: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedEffectIfAttacked(string[] _Line)
        {
            return new ParseException("Not a valid effect if attacked: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedLookAtStyle(string[] _Line)
        {
            return new ParseException("Not a valid look-at style: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedAxis(string[] _Line)
        {
            return new ParseException("Axis not recognized: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedSegment(string[] _Line)
        {
            return new ParseException("Not a valid segment: ", String.Join(" ", _Line));
        }

        public static ParseException UnregonizedDlistVisibility(string[] _Line)
        {
            return new ParseException("Unrecognized display list visibility option: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedParameter(string[] _Line)
        {
            return new ParseException("Not a valid parameter: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedOperator(string[] _Line)
        {
            return new ParseException("Not a valid operator: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedActor(string[] _Line)
        {
            return new ParseException("Not a valid actor: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedActorCategory(string[] _Line)
        {
            return new ParseException("Not a valid actor category: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedSFX(string[] _Line)
        {
            return new ParseException("Not a valid sound effect: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedBGM(string[] _Line)
        {
            return new ParseException("Not a valid BGM: ", String.Join(" ", _Line));
        }

        public static ParseException UnexpectedTradeInstruction(string _Line)
        {
            return new ParseException("Unexpected member in trade instruction: ", _Line);
        }

        public static ParseException UnexpectedTradeInstruction(string[] _Line)
        {
            return UnexpectedTradeInstruction(String.Join(" ", _Line));
        }

        public static ParseException DuplicateTradeInstruction(string _Line)
        {
            return new ParseException("Duplicate member in trade instruction: ", _Line);
        }

        public static ParseException DuplicateTradeInstruction(string[] _Line)
        {
            return DuplicateTradeInstruction(String.Join(" ", _Line));
        }


        public static ParseException UnrecognizedInstruction(string[] _Line)
        {
            return new ParseException("Unrecognized instruction: ", String.Join(" ", _Line));
        }

        public static ParseException BadTime(string[] _Line)
        {
            return new ParseException("Time is not well formed. Should be military time HH:mm: ", String.Join(" ", _Line));
        }

        public static ParseException ParamConversionError(string[] _Line)
        {
            return new ParseException("Could not convert one of the parameters: ", String.Join(" ", _Line));
        }
    }
}
