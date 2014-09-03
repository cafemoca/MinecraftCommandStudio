using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cafemoca.CommandEditor.Utils
{
    public static class Compiler
    {
        public static string Compile(this string text)
        {
            return text.Compile(TokenizeType.Command, 0, EscapeModeValue.New, ParentType.Default);
        }

        public static string Compile(this string text, EscapeModeValue escapeMode)
        {
            return text.Compile(TokenizeType.Command, 0, escapeMode, ParentType.Default);
        }

        public static string Compile(this string text, TokenizeType tokenizeType,
            int escapeLevel, EscapeModeValue escapeMode, ParentType parentType)
        {
            if (text.IsEmpty())
            {
                return string.Empty;
            }

            var tokens = text.Tokenize(tokenizeType)
                .Where(t => !t.IsMatchType(TokenType.Blank, TokenType.Comment));

            if (tokens.IsEmpty())
            {
                return string.Empty;
            }
            
            var builder = new StringBuilder(tokens.Count());

            try
            {
                using (var reader = new TokenReader(tokens))
                {
                    while (reader.IsRemainToken)
                    {
                        var token = reader.Get();
                        var value = token.Value;

                        var start = false;
                        var end = false;
                        var escape = GetRepeatedEscape(escapeLevel, escapeMode) + "\"";

                        switch (token.Type)
                        {
                            case TokenType.String:
                                if (escapeMode == EscapeModeValue.New)
                                {
                                    for (int i = 0; i < escapeLevel; i++)
                                    {
                                        value = value.Escape('"');
                                    }
                                }
                                else if (escapeMode == EscapeModeValue.Old)
                                {
                                    value = value
                                        .Replace("\\", GetRepeatedEscape(escapeLevel, escapeMode))
                                        .Replace("\"", GetRepeatedEscape(escapeLevel, escapeMode) + "\"");
                                }
                                break;
                            case TokenType.StringBlock:
                                value = value
                                    .Unenclose('(', ')')
                                    .Compile(TokenizeType.Command, escapeLevel + 1, escapeMode, ParentType.Default)
                                    .Quote(escape);
                                break;
                            case TokenType.TagBlock:
                                start = value.First() == '{';
                                end = value.Last() == '}' ;
                                if (parentType == ParentType.RawJson ||
                                    (reader.Cursor > 1 &&
                                     reader.CheckPrevious(x => x.IsMatchType(TokenType.Colon)) &&
                                     reader.CheckAtRelative(-2, x => x.IsMatchLiteral(EscapeKey, false))))
                                {
                                    value = value
                                        .Unenclose('{', '}')
                                        .Compile(TokenizeType.Block, escapeLevel + 1, escapeMode, ParentType.TagBlock);
                                    value = value.Enclose(
                                        start ? escape + "{" : "",
                                        end ? "}" + escape : "");
                                }
                                else
                                {
                                    value = value
                                        .Unenclose('{', '}')
                                        .Compile(TokenizeType.Block, escapeLevel, escapeMode, ParentType.TagBlock);
                                    value = value.Enclose(
                                        start ? "{" : "",
                                        end ? "}" : "");
                                }
                                break;
                            case TokenType.ArrayBlock:
                                start = value.First() == '[';
                                end = value.Last() == ']';
                                if (parentType == ParentType.RawJson)
                                {
                                    value = value
                                        .Unenclose('[', ']')
                                        .Compile(TokenizeType.Block, escapeLevel + 1, escapeMode, ParentType.ArrayBlock);
                                    value = value.Enclose(
                                        start ? escape + "[" : "",
                                        end ? "]" + escape : "");
                                    break;
                                }
                                else if (reader.Cursor > 1 &&
                                         reader.CheckPrevious(x => x.IsMatchType(TokenType.Colon)) &&
                                         reader.CheckAtRelative(-2, x => x.IsMatchLiteral("pages")))
                                {
                                    value = value
                                        .Unenclose('[', ']')
                                        .Compile(TokenizeType.Block, escapeLevel, escapeMode, ParentType.RawJson);
                                }
                                else
                                {
                                    value = value
                                        .Unenclose('[', ']')
                                        .Compile(TokenizeType.Block, escapeLevel, escapeMode, ParentType.ArrayBlock);
                                }
                                value = value.Enclose(
                                    start ? "[" : "",
                                    end ? "]" : "");
                                break;
                        }

                        if (parentType == ParentType.Default && reader.Cursor > 1)
                        {
                            if (!(token.IsMatchType(TokenType.ArrayBlock) &&
                                  reader.CheckPrevious(x => x.IsMatchType(TokenType.TargetSelector)) &&
                                  !value.CheckNext(0, '{', '[')) &&
                                !(token.IsMatchType(TokenType.Colon) &&
                                  reader.CheckPrevious(x => x.IsMatchLiteral("minecraft"))) &&
                                !(token.IsMatchType(TokenType.Literal) &&
                                  reader.CheckPrevious(x => x.IsMatchType(TokenType.Colon))))
                            {
                                builder.Append(" ");
                            }
                        }
                        if (parentType != ParentType.Default && reader.Cursor > 1)
                        {
                            if (token.IsMatchType(TokenType.Literal) &&
                                reader.CheckPrevious(x => x.IsMatchType(TokenType.Literal)))
                            {
                                builder.Append(" ");
                            }
                            if (token.IsMatchType(TokenType.String) &&
                                reader.CheckPrevious(x => x.IsMatchType(TokenType.String)))
                            {
                                builder.Remove(builder.Length - 1, 1);
                                value.TrimStart('"');
                            }
                        }

                        builder.Append(value);
                    }
                }
            }
            catch
            {
            }

            return builder.ToString();
        }

        private static readonly string[] EscapeKey =
        {
            "Text1",
            "Text2",
            "Text3",
            "Text4",
            "Command",
        };

        private static string GetRepeatedEscape(int escapeLevel, EscapeModeValue escapeMode)
        {
            if (escapeLevel <= 0)
            {
                return string.Empty;
            }
            if (escapeLevel == 1)
            {
                return "\\";
            }
            var count = escapeMode == EscapeModeValue.New
                ? (int) Math.Pow(2, escapeLevel) - 1
                : (int) Math.Pow(2, escapeLevel - 1);

            var b = new StringBuilder(count);
            Enumerable.Range(1, count).ForEach(_ => b.Append("\\"));
            return b.ToString();
        }
    }

    public enum ParentType
    {
        Default,
        TagBlock,
        ArrayBlock,
        RawJson,
    }

    public enum EscapeModeValue
    {
        New,
        Old,
    }
}
