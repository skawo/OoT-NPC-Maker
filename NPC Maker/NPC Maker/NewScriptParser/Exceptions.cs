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
            Line = _Line;
        }

        public override string ToString()
        {
            return Message + " " + Line;
        }

        public static ParseException GeneralError(string _Line)
        {
            return new ParseException("Error parsing line: ", _Line);
        }

        public static ParseException GeneralError(string[] _Line)
        {
            return GeneralError(String.Join(" ", _Line));
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

        public static ParseException LabelNotFound(string[] _Line)
        {
            return new ParseException("Label not found: ", String.Join(" ", _Line));
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

        public static ParseException IfNotClosed(string[] _Line)
        {
            return new ParseException("This IF does not have a corresponding ENDIF: ", String.Join(" ", _Line));
        }

        public static ParseException WhileNotClosed(string[] _Line)
        {
            return new ParseException("This WHILE does not have a corresponding ENDWHILE: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedCondition(string[] _Line)
        {
            return new ParseException("Condition not recognized: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedTradeItem(string[] _Line)
        {
            return new ParseException("Trade Item not recognized: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedTradeStatus(string[] _Line)
        {
            return new ParseException("Trade status not recognized: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedMask(string[] _Line)
        {
            return new ParseException("Mask not recognized: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedAnimation(string[] _Line)
        {
            return new ParseException("Animation not recognized: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedBombBag(string[] _Line)
        {
            return new ParseException("Bomb bag not recognized: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedWallet(string[] _Line)
        {
            return new ParseException("Wallet not recognized: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedQuiver(string[] _Line)
        {
            return new ParseException("Quiver not recognized: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedStickCap(string[] _Line)
        {
            return new ParseException("Stick cap not recognized: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedDekuNutCap(string[] _Line)
        {
            return new ParseException("Deku Nut cap not recognized: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedScale(string[] _Line)
        {
            return new ParseException("Scale not recognized: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedGauntlets(string[] _Line)
        {
            return new ParseException("Gauntlets not recognized: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedMovementStyle(string[] _Line)
        {
            return new ParseException("Movement style not recognized: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedLookAtStyle(string[] _Line)
        {
            return new ParseException("Look-at style not recognized: ", String.Join(" ", _Line));
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
            return new ParseException("Unrecognized Display List visibility option: ", String.Join(" ", _Line));
        }

        public static ParseException UnrecognizedOperator(string[] _Line)
        {
            return new ParseException("Unrecognized operator: ", String.Join(" ", _Line));
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
