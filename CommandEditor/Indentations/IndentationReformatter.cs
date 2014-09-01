using Cafemoca.CommandEditor.Extensions;
using Cafemoca.CommandEditor.Utils;
using ICSharpCode.AvalonEdit.Indentation.CSharp;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Cafemoca.CommandEditor.Indentations
{
    public class IndentationReformatter
    {
        private struct Block
        {
            public string OuterIndent;
            public string InnerIndent;
            public string LastLiteral;
            public char Bracket;
            public bool Continuation;
            public int OneLineBlock;
            public int PreviousOneLineBlock;
            public int StartLine;

            public void ResetOneLineBlock()
            {
                PreviousOneLineBlock = OneLineBlock;
                OneLineBlock = 0;
            }

            public void Indent(IndentationSettings settings)
            {
                this.Indent(settings.IndentString);
            }

            public void Indent(string indentationString)
            {
                this.OuterIndent = InnerIndent;
                this.InnerIndent += indentationString;
                this.Continuation = false;
                this.ResetOneLineBlock();
                this.LastLiteral = "";
            }
        }

        private StringBuilder _wordBuilder;
        private Stack<Block> _blocks;
        private Block _block;

        private bool _inString;
        private bool _escape;

        private bool _lineComment;
        private bool _blockComment;

        private char _lastNonCommentChar;

        public void Reformat(IDocumentAccessor document, IndentationSettings settings)
        {
            this.Initialize();

            while (document.MoveNext())
            {
                this.Step(document, settings);
            }
        }

        public void Initialize()
        {
            this._wordBuilder = new StringBuilder();
            this._blocks = new Stack<Block>();
            this._block = new Block();
            this._block.InnerIndent = "";
            this._block.OuterIndent = "";
            this._block.Bracket = '{';
            this._block.Continuation = false;
            this._block.LastLiteral = "";
            this._block.OneLineBlock = 0;
            this._block.PreviousOneLineBlock = 0;
            this._block.StartLine = 0;

            this._inString = false;
            this._escape = false;

            this._lineComment = false;
            this._blockComment = false;

            this._lastNonCommentChar = ' ';
        }

        public void Step(IDocumentAccessor document, IndentationSettings settings)
        {
            var line = document.Text;
            if (settings.LeaveEmptyLines && line.Length == 0)
            {
                return;
            }
            line = line.TrimStart();

            var indent = new StringBuilder();
            if (line.Length == 0)
            {
                if (this._blockComment)
                {
                    return;
                }
                indent.Append(this._block.InnerIndent);
                indent.Append(settings.IndentString.Repeat(this._block.OneLineBlock));
                if (this._block.Continuation)
                {
                    indent.Append(settings.IndentString);
                }
                if (document.Text != indent.ToString())
                {
                    document.Text = indent.ToString();
                }
                return;
            }

            if (document.TrimEnd())
            {
                line = document.Text.TrimStart();
            }

            var oldBlock = this._block;
            var startInComment = this._blockComment;

            this._lineComment = false;
            this._escape = false;

            this._lastNonCommentChar = '\n';

            var reader = new CharReader(line);

            var cha = ' ';
            var prev = '\0';
            var next = '\n';

            var indented = false;

            while (reader.IsRemainChar)
            {
                cha = reader.Get();
                prev = reader.Backward;
                next = reader.Ahead;

                if (this._lineComment)
                {
                    break;
                }
                if (this._escape)
                {
                    this._escape = false;
                    continue;
                }
                
                switch (cha)
                {
                    case '/':
                        if (this._blockComment && prev == '*')
                        {
                            this._blockComment = false;
                        }
                        if (!this._inString)
                        {
                            if (!this._blockComment && next == '/')
                            {
                                this._lineComment = true;
                            }
                            if (!this._lineComment && next == '*')
                            {
                                this._blockComment = true;
                            }
                        }
                        break;
                    case '"':
                        if (!(this._lineComment || this._blockComment))
                        {
                            if (this._inString)
                            {
                                this._inString = !this._escape;
                            }
                        }
                        break;
                    case '\\':
                        if (this._inString)
                        {
                            this._escape = true;
                        }
                        break;
                    default:
                        break;
                }

                if (this._lineComment || this._blockComment || this._inString)
                {
                    if (this._wordBuilder.Length > 0)
                    {
                        this._block.LastLiteral = this._wordBuilder.ToString();
                    }
                    this._wordBuilder.Length = 0;
                    continue;
                }

                if (char.IsLetterOrDigit(cha))
                {
                    this._wordBuilder.Append(cha);
                }
                else
                {
                    if (this._wordBuilder.Length > 0)
                    {
                        this._block.LastLiteral = this._wordBuilder.ToString();
                    }
                    this._wordBuilder.Length = 0;
                }

                switch (cha)
                {
                    case '(':
                    case '{':
                    case '[':
                        this._block.ResetOneLineBlock();
                        this._blocks.Push(this._block);
                        this._block.StartLine = document.LineNumber;
                        if (!indented)
                        {
                            this._block.Indent(settings);
                            indented = true;
                        }
                        this._block.Bracket = cha;
                        break;
                    case ')':
                    case '}':
                    case ']':
                        var openBracket = StringChecker.GetOpenBracket(cha);
                        while (this._block.Bracket != openBracket)
                        {
                            if (this._blocks.Count == 0)
                            {
                                break;
                            }
                            this._block = this._blocks.Pop();
                        }
                        if (this._blocks.Count == 0)
                        {
                            break;
                        }
                        this._block = this._blocks.Pop();
                        this._block.Continuation = false;
                        this._block.ResetOneLineBlock();
                        break;
                }

                if (!char.IsWhiteSpace(cha))
                {
                    this._lastNonCommentChar = cha;
                }
            }

            if (this._wordBuilder.Length > 0)
            {
                this._block.LastLiteral = this._wordBuilder.ToString();
            }
            this._wordBuilder.Length = 0;

            if ((startInComment && line[0] != '*') ||
                document.Text.StartsWith("//\t", StringComparison.Ordinal) ||
                (document.Text == "//"))
            {
                return;
            }

            if ("]})".Contains(line[0]))
            {
                indent.Append(oldBlock.OuterIndent);
                oldBlock.ResetOneLineBlock();
                oldBlock.Continuation = false;
            }
            else
            {
                indent.Append(oldBlock.InnerIndent);
            }

            if (document.IsReadOnly)
            {
                if (!oldBlock.Continuation && oldBlock.OneLineBlock == 0 &&
                    oldBlock.StartLine == this._block.StartLine &&
                    this._block.StartLine < document.LineNumber && this._lastNonCommentChar != ':')
                {
                    indent.Length = 0;
                    line = document.Text;
                    for (int i = 0; i < line.Length; ++i)
                    {
                        if (!char.IsWhiteSpace(line[i]))
                        {
                            break;
                        }
                        indent.Append(line[i]);
                    }

                    if (startInComment && indent.Length > 0 && indent[indent.Length - 1] == ' ')
                    {
                        indent.Length -= 1;
                    }
                    this._block.InnerIndent = indent.ToString();
                }
                return;
            }

            if (startInComment)
            {
                indent.Append(' ');
            }

            if (indent.Length != (document.Text.Length - line.Length) ||
                !document.Text.StartsWith(indent.ToString(), StringComparison.Ordinal) ||
                char.IsWhiteSpace(document.Text[indent.Length]))
            {
                document.Text = indent.ToString() + line;
            }
        }
    }

    internal static class ReformatterExtensions
    {
        internal static bool TrimEnd(this IDocumentAccessor document)
        {
            var line = document.Text;
            if (!char.IsWhiteSpace(line[line.Length - 1]))
            {
                return false;
            }
            if (line.EndsWith("// ", StringComparison.Ordinal) ||
                line.EndsWith("* ", StringComparison.Ordinal))
            {
                return false;
            }

            document.Text = line.TrimEnd();
            return true;
        }
    }
}
