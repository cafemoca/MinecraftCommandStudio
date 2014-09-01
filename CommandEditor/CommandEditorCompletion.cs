using Cafemoca.CommandEditor.Completions;
using Cafemoca.CommandEditor.Extensions;
using Cafemoca.CommandEditor.Utils;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Indentation.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Cafemoca.CommandEditor
{
    public partial class CommandEditor : TextEditor
    {
        private CompletionWindow _completionWindow;

        private void ShowCompletionWindow(IEnumerable<CompletionData> completions)
        {
            if (this._completionWindow != null)
            {
                return;
            }
            var window = new CompletionWindow(this.TextArea);
            window.Closed += (o, e) =>
            {
                if (this._completionWindow.Equals(window))
                {
                    this._completionWindow = null;
                }
            };
            completions.ForEach(window.CompletionList.CompletionData.Add);
            this._completionWindow = window;
            window.Show();
        }

        private void CloseCompletionWindow()
        {
            if (this._completionWindow != null)
            {
                this._completionWindow.Close();
            }
        }

        private void TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if ((e.Text.Length > 0) &&
                (this._completionWindow != null) &&
                !char.IsLetterOrDigit(e.Text[0]))
            {
                this._completionWindow.CompletionList.RequestInsertion(e);
            }
        }

        private void TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (this._completionWindow != null)
            {
                return;
            }

            var index = this.CaretOffset;
            var input = e.Text;

            this.FixOnTextEntered(index, input);

            if (this.ExtendedOptions.EnableCompletion)
            {
                var completionData = this.GetCompletionData(index, input);
                if (completionData != null)
                {
                    this.ShowCompletionWindow(completionData);
                }
            }
        }

        private void FixOnPreviewKeyDown(KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
            {
                return;
            }

            var index = this.CaretOffset;
            var key = e.Key;

            var prev = this.PreviousChar.ToString();
            var next = this.NextChar.ToString();

            var line = this.Document.GetLineByOffset(index);
            
            switch (key)
            {
                case Key.Back:
                    this.DeleteBothBracket(e);
                    if (this.TextArea.Caret.Column == 1 &&
                        line.LineNumber > 1)
                    {
                        var eol = this.Text
                            .Skip(line.PreviousLine.EndOffset)
                            .TakeWhile(x => "\r\n".Contains(x))
                            .Count();
                        var indent = this.Text
                            .Skip(line.PreviousLine.EndOffset + eol)
                            .TakeWhile(x => "\t ".Contains(x))
                            .Count();
                        if (indent > 0)
                        {
                            e.Handled = true;
                            this.Document.Remove(line.PreviousLine.EndOffset, eol + indent);
                        }
                    }
                    break;
                case Key.Delete:
                    //this.DeleteBothBracket(e);
                    if (index == line.EndOffset &&
                        line.LineNumber < LineCount)
                    {
                        var eol = this.Text
                            .Skip(index)
                            .TakeWhile(x => "\r\n".Contains(x))
                            .Count();
                        var indent = this.Text
                            .Skip(index)
                            .SkipWhile(x => "\r\n".Contains(x))
                            .TakeWhile(x => "\t ".Contains(x))
                            .Count();
                        if (indent > 0)
                        {
                            e.Handled = true;
                            this.Document.Remove(index, indent + eol);
                        }
                    }
                    break;
                case Key.Enter:
                    if (!this.IsSelected &&
                        "({[\t ,".Contains(prev) &&
                        "]})".Contains(next))
                    {
                        e.Handled = true;
                        this.BeginChange();
                        this.Document.Insert(index,
                            Environment.NewLine +
                            this.TextArea.IndentationStrategy.AsCommandIndentationStrategy().IndentationString +
                            Environment.NewLine);
                        this.TextArea.IndentationStrategy.IndentLines(this.Document, line.LineNumber, line.NextLine.NextLine.LineNumber);
                        this.CaretOffset--;
                        this.EndChange();
                    }
                    break;
                case Key.Home:
                case Key.End:
                    this.CloseCompletionWindow();
                    break;
                case Key.F2:
                    if (this.Text.IsEmpty())
                    {
                        return;
                    }
                    if (this.ExtendedOptions.EnableCompletion)
                    {
                        var completionData = this.GetCompletionData(index, " ");
                        if (completionData != null)
                        {
                            this.ShowCompletionWindow(completionData);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void TrimStartLine(TextDocumentAccessor accessor)
        {
            accessor.Text = accessor.Text.TrimStart();
        }

        private void DeleteBothBracket(KeyEventArgs e)
        {
            if (this.CheckBothSide("(", ")") ||
                this.CheckBothSide("{", "}") ||
                this.CheckBothSide("[", "]") ||
                this.CheckBothSide("'", "'") ||
                this.CheckBothSide("\"", "\""))
            {
                e.Handled = true;
                this.Document.Remove(this.CaretOffset - 1, 2);
            }
        }

        private void FixOnPreviewTextInput(TextCompositionEventArgs e)
        {
            var index = this.CaretOffset;
            var input = e.Text;

            var prev = this.PreviousChar.ToString();
            var next = this.NextChar.ToString();

            var bracketPair = new Dictionary<string, string>()
            {
                { "(", ")" },
                { "{", "}" },
                { "[", "]" },
            };

            e.Handled = false;
            
            switch (input)
            {
                case "'":
                case "\"":
                    if (this.IsSelectedSingleLine ||
                       (this.IsSelectedMultiLine && this.ExtendedOptions.EncloseMultiLine))
                    {
                        if (this.ExtendedOptions.EncloseSelection)
                        {
                            e.Handled = true;
                            this.BeginChange();
                            this.Document.Insert(this.SelectionStart, input);
                            this.Document.Insert(this.SelectionStart + this.SelectionLength, input);
                            this.EndChange();
                        }
                        break;
                    }
                    if (this.IsSelectedMultiLine && !this.ExtendedOptions.EncloseMultiLine)
                    {
                        break;
                    }
                    if (next == input &&
                        prev == input)
                    {
                        e.Handled = true;
                        this.CaretOffset++;
                        break;
                    }
                    if (this.ExtendedOptions.BracketCompletion &&
                        ("\t\r\n \0".Contains(next) ||
                        ("({[".Contains(prev) && "]})".Contains(next))))
                    {
                        e.Handled = true;
                        this.Document.Insert(index, input + input);
                        if (this.IsSelectedMultiLine)
                        {
                            this.CaretOffset++;
                        }
                        else
                        {
                            this.CaretOffset--;
                        }
                        break;
                    }
                    break;
                case "(":
                case "{":
                case "[":
                    if (this.IsSelectedSingleLine ||
                       (this.IsSelectedMultiLine && this.ExtendedOptions.EncloseMultiLine))
                    {
                        if (this.ExtendedOptions.EncloseSelection)
                        {
                            e.Handled = true;
                            this.BeginChange();
                            this.Document.Insert(this.SelectionStart, input);
                            this.Document.Insert(this.SelectionStart + this.SelectionLength, bracketPair[input]);
                            this.EndChange();
                        }
                        break;
                    }
                    if (this.IsSelectedMultiLine && !this.ExtendedOptions.EncloseMultiLine)
                    {
                        break;
                    }
                    if (this.ExtendedOptions.BracketCompletion &&
                        ("\t\r\n \0".Contains(next) ||
                        ("({[".Contains(prev) && "]})".Contains(next))))
                    {
                        e.Handled = true;
                        this.Document.Insert(index, input + bracketPair[input]);
                        if (this.IsSelectedMultiLine)
                        {
                            this.CaretOffset++;
                        }
                        else
                        {
                            this.CaretOffset--;
                        }
                        break;
                    }
                    break;
                case ")":
                case "}":
                case "]":
                    if (next == input &&
                        prev == bracketPair.First(x => x.Value == input).Key)
                    {
                        e.Handled = true;
                        this.CaretOffset++;
                        break;
                    }
                    break;
                default:
                    break;
            }
        }

        private void FixOnTextEntered(int index, string input)
        {
            var prev = this.PreviousChar.ToString();
            var next = this.NextChar.ToString();

            switch (input)
            {
                case ")":
                case "}":
                case "]":
                    if (this.ExtendedOptions.AutoReformat)
                    {
                        this.BeginChange();
                        this.TextArea.IndentationStrategy
                            .AsCommandIndentationStrategy()
                            .IndentBlock(this.Document, index);
                        this.EndChange();
                    }
                    break;
                case "\r":
                case "\n":
                    var indent = this.Text
                        .Skip(this.CaretOffset)
                        .TakeWhile(x => "\t ".Contains(x))
                        .Count();
                    this.CaretOffset += indent;
                    break;
                default:
                    break;
            }
        }
        
        private IEnumerable<CompletionData> GetCompletionData(int index, string input)
        {
            var tokens = this.Text
                .Tokenize(TokenizeType.All);

            var next = this.NextChar;
            var prev = (this.CaretOffset > 1)
                ? this.Document.GetCharAt(this.CaretOffset - 2)
                : '\0';

            var beforeTokens = tokens
                .TakeWhile(x => x.Index < index)
                .Where(x => !x.IsMatchType(TokenType.Blank, TokenType.Comment));
            var afterTokens = tokens
                .SkipWhile(x => x.Index < index)
                .Where(x => !x.IsMatchType(TokenType.Blank, TokenType.Comment));
            var prevToken = beforeTokens.LastOrDefault();

            if (beforeTokens.FirstOrDefault().IsEmpty() ||
                prevToken.IsMatchType(TokenType.Comment))
            {
                return null;
            }

            using (var reader = new TokenReader(beforeTokens, true))
            {
                try
                {
                    switch (input.ToLower())
                    {
                        case "/":
                            if ("/*".Contains(prev))
                            {
                                return null;
                            }
                            return MinecraftCompletions.GetCommandCompletion();
                        case "*":
                            if (prev == '/')
                            {
                                return null;
                            }
                            break;
                        case "@":
                            return MinecraftCompletions.GetTargetCompletion();
                        case ")":
                        case "}":
                        case "]":
                            break;
                        case "{":
                            break;
                        case "_":
                            if (prevToken.IsMatchLiteral("score_"))
                            {
                                return this.ExtendedOptions.ScoreNames.ToCompletionData();
                            }
                            break;
                        case "!":
                            if (reader.IsRemainToken &&
                                reader.Ahead.IsMatchType(TokenType.Equal))
                            {
                                prevToken = reader.Get();
                                goto case "=";
                            }
                            break;
                        case "=":
                            if (reader.IsRemainToken)
                            {
                                reader.MoveNext();
                                if (reader.Ahead.IsMatchLiteral("team"))
                                {
                                    return this.ExtendedOptions.TeamNames.ToCompletionData();
                                }
                                if (reader.Ahead.IsMatchLiteral("name"))
                                {
                                    return this.ExtendedOptions.PlayerNames.ToCompletionData();
                                }
                            }
                            break;
                        case ":":
                            break;
                        case "\r\n":
                        case "\r":
                        case "\n":
                        case "\t":
                        case " ":
                            if (!reader.IsRemainToken)
                            {
                                return null;
                            }
                            if (input == "\r" || input == "\n")
                            {
                                if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
                                {
                                    return null;
                                }
                            }
                            var command = reader.SkipGet(x => x.IsMatchType(TokenType.Command));
                            if (!command.IsEmpty())
                            {
                                var comp = new CommandCompletions(this.ExtendedOptions);
                                reader.Reverse();
                                switch (command.Value)
                                {
                                    case "/scoreboard":
                                        var scoreboard = comp.ScoreboardCompletion(reader);
                                        if (scoreboard != null)
                                        {
                                            return scoreboard;
                                        }
                                        break;
                                    case "/give":
                                        var give = comp.GiveCompletion(reader);
                                        if (give != null)
                                        {
                                            return give;
                                        }
                                        break;
                                }
                            }
                            break;
                        default:
                            if (prevToken.IsMatchType(TokenType.TargetSelector) &&
                                prevToken.Value.Length >= 1)
                            {
                                return null;
                            }
                            break;
                    }
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        private bool CheckBothSide(char prev, char next)
        {
            return this.CheckBothSide(prev, next);
        }

        private bool CheckBothSide(string prev, string next)
        {
            return this.PreviousChar.ToString() == prev &&
                   this.NextChar.ToString() == next;
        }
    }
}
