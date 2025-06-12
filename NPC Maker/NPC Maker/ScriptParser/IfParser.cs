using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPC_Maker.ScriptParser
{
    public enum NodeType
    {
        STATEMENT,
        OR,
        AND
    }

    public class LogicNode
    {
        public NodeType Type { get; set; }
        public string Statement { get; set; }
        public List<LogicNode> Children { get; set; } = new List<LogicNode>();
    }

    public static class IfParser
    {
        public static LogicNode ParseIf(string expression)
        {
            expression = expression.Trim();

            if (expression.StartsWith($"{Lists.Instructions.IF} ", StringComparison.OrdinalIgnoreCase))
                expression = expression.Substring(3).Trim();
            else
                throw new Scripts.ParseException("Not an IF statement:", expression);

            int pos = 0;
            return ParseLogicalOr(expression, ref pos);
        }

        private static LogicNode ParseLogicalOr(string expression, ref int pos)
        {
            SkipWhitespace(expression, ref pos);
            var left = ParseLogicalAnd(expression, ref pos);
            SkipWhitespace(expression, ref pos);

            while (pos < expression.Length)
            {
                if (MatchOperator(expression, ref pos, Lists.Keyword_Or))
                {
                    SkipWhitespace(expression, ref pos);
                    var right = ParseLogicalAnd(expression, ref pos);
                    var orNode = new LogicNode { Type = NodeType.OR };
                    orNode.Children.Add(left);
                    orNode.Children.Add(right);
                    left = orNode;
                    SkipWhitespace(expression, ref pos);
                }
                else
                    break;
            }

            return left;
        }

        private static LogicNode ParseLogicalAnd(string expression, ref int pos)
        {
            SkipWhitespace(expression, ref pos);
            var left = ParsePrimary(expression, ref pos);
            SkipWhitespace(expression, ref pos);

            while (pos < expression.Length)
            {
                if (MatchOperator(expression, ref pos, Lists.Keyword_And))
                {
                    SkipWhitespace(expression, ref pos);
                    var right = ParsePrimary(expression, ref pos);
                    var andNode = new LogicNode { Type = NodeType.AND };
                    andNode.Children.Add(left);
                    andNode.Children.Add(right);
                    left = andNode;
                    SkipWhitespace(expression, ref pos);
                }
                else
                {
                    break;
                }
            }

            return left;
        }

        private static LogicNode ParsePrimary(string expression, ref int pos)
        {
            SkipWhitespace(expression, ref pos);

            if (pos >= expression.Length)
                throw new Scripts.ParseException("Unexpected end of expression:", expression);

            if (expression[pos] == '(') // skip '('
            {
                pos++; // skip '('
                var node = ParseLogicalOr(expression, ref pos);
                SkipWhitespace(expression, ref pos);

                if (pos >= expression.Length || expression[pos] != ')')
                    throw new Scripts.ParseException("Missing closing parenthesis:", expression);

                pos++; // skip ')'
                return node;
            }
            else
            {
                // Parse a statement
                var start = pos;

                while (pos < expression.Length && !IsLogicalOperator(expression, pos) && expression[pos] != ')')
                {
                    pos++;
                }

                if (pos <= start)
                    throw new Scripts.ParseException("Expected statement:", expression);

                var statement = expression.Substring(start, pos - start).Trim();
                return new LogicNode { Type = NodeType.STATEMENT, Statement = statement };
            }
        }

        private static bool MatchOperator(string expression, ref int pos, string op)
        {
            var remaining = expression.Length - pos;

            if (remaining >= op.Length && expression.Substring(pos, op.Length).Equals(op, StringComparison.OrdinalIgnoreCase))
            {
                pos += op.Length;
                return true;
            }

            return false;
        }

        private static bool IsLogicalOperator(string expression, int pos)
        {
            if (pos >= expression.Length) return false;
            return MatchOperator(expression, ref pos, Lists.Keyword_And) || MatchOperator(expression, ref pos, Lists.Keyword_Or);
        }

        private static void SkipWhitespace(string expression, ref int pos)
        {
            while (pos < expression.Length && char.IsWhiteSpace(expression[pos]))
                pos++;
        }
    }

}
