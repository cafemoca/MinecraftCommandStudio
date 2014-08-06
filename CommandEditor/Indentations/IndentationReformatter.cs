using Cafemoca.CommandEditor.Extensions;
using ICSharpCode.AvalonEdit.Indentation.CSharp;
using System;
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
        private bool _inChar;
        private bool _verbatim;
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
            this._inChar = false;
            this._verbatim = false;
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
                if (this._blockComment || (this._inString && this._verbatim))
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

            if (TrimEnd(document))
            {
                line = document.Text.TrimStart();
            }

            var oldBlock = this._block;
            var startInComment = this._blockComment;
            var startInString = (this._inString && this._verbatim);

            this._lineComment = false;
            this._inChar = false;
            this._escape = false;
            if (!this._verbatim)
            {
                this._inString = false;
            }

            this._lastNonCommentChar = '\n';

            var cha = ' ';
            var lastChar = ' ';
            var nextChar = line[0];

            for (int i = 0; i < line.Length; i++)
            {
                if (this._lineComment)
                {
                    break;
                }

                lastChar = cha;
                cha = nextChar;
                if (i + 1 < line.Length)
                {
                    nextChar = line[i + 1];
                }
                else
                {
                    nextChar = '\n';
                }

                if (this._escape)
                {
                    this._escape = false;
                    continue;
                }

                switch (cha)
                {
                    case '/':
                        if (this._blockComment && lastChar == '*')
                        {
                            this._blockComment = false;
                        }
                        if (!this._inString && !this._inChar)
                        {
                            if (!this._blockComment && nextChar == '/')
                            {
                                this._lineComment = true;
                            }
                            if (!this._lineComment && nextChar == '*')
                            {
                                this._blockComment = true;
                            }
                        }
                        break;
                    case '"':
                        if (!(this._inChar || this._lineComment || this._blockComment))
                        {
                            this._inString = !this._inString;
                            if (!this._inString && this._verbatim)
                            {
                                if (nextChar == '"')
                                {
                                    this._escape = true;
                                    this._inString = true;
                                }
                                else
                                {
                                    this._verbatim = false;
                                }
                            }
                            else if (this._inString && lastChar == '@')
                            {
                                this._verbatim = true;
                            }
                        }
                        break;
                    case '\'':
                        if (!(this._inString || this._lineComment || this._blockComment))
                        {
                            this._inChar = !this._inChar;
                        }
                        break;
                    case '\\':
                        if ((this._inString && !this._verbatim) || this._inChar)
                        {
                            this._escape = true;
                        }
                        break;
                }

                if (this._lineComment || this._blockComment || this._inString || this._inChar)
                {
                    if (this._wordBuilder.Length > 0)
                    {
                        this._block.LastLiteral = this._wordBuilder.ToString();
                    }
                    this._wordBuilder.Length = 0;
                    continue;
                }

                if (!char.IsWhiteSpace(cha) && cha != '[' && cha != '/' && !Regex.IsMatch(cha.ToString(), "[A-Za-z0-9]"))
                {
                    if (this._block.Bracket == '{')
                    {
                        this._block.Continuation = true;
                    }
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
                    case '{':
                        this._block.ResetOneLineBlock();
                        this._blocks.Push(this._block);
                        this._block.StartLine = document.LineNumber;
                        this._block.Indent(settings);
                        this._block.Bracket = '{';
                        break;
                    case '}':
                        while (this._block.Bracket != '{')
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
                    case '(':
                    case '[':
                        this._blocks.Push(this._block);
                        if (this._block.StartLine == document.LineNumber)
                        {
                            this._block.InnerIndent = this._block.OuterIndent;
                        }
                        else
                        {
                            this._block.StartLine = document.LineNumber;
                        }
                        this._block.Indent(
                            settings.IndentString.Repeat(oldBlock.OneLineBlock) +
                            (oldBlock.Continuation ? settings.IndentString : "") +
                            (i == line.Length - 1 ? settings.IndentString : new String(' ', i + 1)));
                        this._block.Bracket = cha;
                        break;
                    case ')':
                        if (this._blocks.Count == 0) break;
                        if (this._block.Bracket == '(')
                        {
                            this._block = this._blocks.Pop();
                        }
                        break;
                    case ']':
                        if (this._blocks.Count == 0)
                        {
                            break;
                        }
                        if (this._block.Bracket == '[')
                        {
                            this._block = this._blocks.Pop();
                        }
                        break;
                    case ';':
                    case ',':
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

            if (startInString ||
                (startInComment && line[0] != '*') ||
                document.Text.StartsWith("//\t", StringComparison.Ordinal) ||
                (document.Text == "//"))
            {
                return;
            }

            if (line[0] == '}')
            {
                indent.Append(oldBlock.OuterIndent);
                oldBlock.ResetOneLineBlock();
                oldBlock.Continuation = false;
            }
            else
            {
                indent.Append(oldBlock.InnerIndent);
            }

            if (indent.Length > 0 && oldBlock.Bracket == '(' && line[0] == ')')
            {
                indent.Remove(indent.Length - 1, 1);
            }
            else if (indent.Length > 0 && oldBlock.Bracket == '[' && line[0] == ']')
            {
                indent.Remove(indent.Length - 1, 1);
            }

            if (line[0] == ':')
            {
                oldBlock.Continuation = true;
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

            if (line[0] != '{')
            {
                if (line[0] != ')' && oldBlock.Continuation && oldBlock.Bracket == '{')
                {
                    indent.Append(settings.IndentString);
                }
                indent.Append(settings.IndentString.Repeat(oldBlock.OneLineBlock));
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

        private static bool TrimEnd(IDocumentAccessor document)
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
