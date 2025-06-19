using System;
using System.Collections.Generic;
using System.Linq;

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
                if (PeekOperator(expression, pos, Lists.Keyword_Or))
                {
                    if (!IsWholeWord(expression, pos, Lists.Keyword_Or.Length))
                    {
                        pos++;
                        continue;
                    }

                    pos += Lists.Keyword_Or.Length;
                    SkipWhitespace(expression, ref pos);
                    var right = ParseLogicalAnd(expression, ref pos);
                    var orNode = new LogicNode { Type = NodeType.OR };
                    orNode.Children.Add(left);
                    orNode.Children.Add(right);
                    left = orNode;
                    SkipWhitespace(expression, ref pos);
                }
                else
                {
                    break;
                }
            }

            return left;
        }

        private static LogicNode ParseLogicalAnd(string expression, ref int pos)
        {
            SkipWhitespace(expression, ref pos);
            var left = ParseCondition(expression, ref pos);
            SkipWhitespace(expression, ref pos);

            while (pos < expression.Length)
            {
                if (PeekOperator(expression, pos, Lists.Keyword_And))
                {
                    if (!IsWholeWord(expression, pos, Lists.Keyword_And.Length))
                    {
                        pos++;
                        continue;
                    }

                    pos += Lists.Keyword_And.Length;
                    SkipWhitespace(expression, ref pos);
                    var right = ParseCondition(expression, ref pos);
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

        private static LogicNode ParseCondition(string expression, ref int pos)
        {
            SkipWhitespace(expression, ref pos);

            if (pos >= expression.Length)
                throw new Scripts.ParseException("Unexpected end of expression:", expression);

            if (expression[pos] == '(')
            {
                pos++; // skip '('
                var node = ParseLogicalOr(expression, ref pos);
                SkipWhitespace(expression, ref pos);

                if (pos >= expression.Length || expression[pos] != ')')
                    throw new Scripts.ParseException("Missing closing parenthesis:", expression);

                pos++; // skip ')'
                return node;
            }

            string[] logicalOps = { Lists.Keyword_And, Lists.Keyword_Or };
            var start = pos;
            int nestedParens = 0;

            while (pos < expression.Length)
            {
                if (expression[pos] == '(')
                {
                    nestedParens++;
                    pos++;
                    continue;
                }
                else if (expression[pos] == ')')
                {
                    if (nestedParens > 0)
                    {
                        nestedParens--;
                        pos++;
                        continue;
                    }
                    break;
                }

                bool isOperator = false;
                foreach (var op in logicalOps)
                {
                    if (PeekOperator(expression, pos, op) && IsWholeWord(expression, pos, op.Length))
                    {
                        isOperator = true;
                        break;
                    }
                }

                if (isOperator && nestedParens == 0)
                    break;

                pos++;
            }

            if (pos <= start)
                throw new Scripts.ParseException("Expected condition:", expression);

            var condition = expression.Substring(start, pos - start).Trim();
            return new LogicNode { Type = NodeType.STATEMENT, Statement = condition };
        }

        private static bool PeekOperator(string expression, int pos, string op)
        {
            var remaining = expression.Length - pos;
            return remaining >= op.Length &&
                   expression.Substring(pos, op.Length).Equals(op, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsWholeWord(string expression, int pos, int length)
        {
            // Check if the operator is preceded by whitespace or start of string
            bool startOk = (pos == 0) || char.IsWhiteSpace(expression[pos - 1]);

            // Check if the operator is followed by whitespace or end of string
            bool endOk = (pos + length >= expression.Length) ||
                         char.IsWhiteSpace(expression[pos + length]);

            return startOk && endOk;
        }

        private static void SkipWhitespace(string expression, ref int pos)
        {
            while (pos < expression.Length && char.IsWhiteSpace(expression[pos]))
                pos++;
        }
    }
}
