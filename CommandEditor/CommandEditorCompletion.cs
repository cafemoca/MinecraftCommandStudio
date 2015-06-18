using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using Cafemoca.CommandEditor.Completions;
using Cafemoca.CommandEditor.Extensions;
using Cafemoca.CommandEditor.Utils;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Indentation.CSharp;

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
                    var closeBracket = StringChecker.GetCloseBracket(input).ToString();
                    if (this.IsSelectedSingleLine ||
                       (this.IsSelectedMultiLine && this.ExtendedOptions.EncloseMultiLine))
                    {
                        if (this.ExtendedOptions.EncloseSelection)
                        {
                            e.Handled = true;
                            this.BeginChange();
                            this.Document.Insert(this.SelectionStart, input);
                            this.Document.Insert(this.SelectionStart + this.SelectionLength, closeBracket);
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
                        this.Document.Insert(index, input + closeBracket);
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
                        prev == StringChecker.GetOpenBracket(input).ToString())
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
                        if (index < 1 ||
                            index > this.Document.TextLength)
                        {
                            return;
                        }
                        if (this._bracketSearcher == null)
                        {
                            this._bracketSearcher = new BracketSearcher();
                        }
                        var bracketSearchResult = this._bracketSearcher.SearchBrackets(this.Document, index);
                        if (bracketSearchResult == null)
                        {
                            return;
                        }
                        var a = this.Document.GetLineByOffset(bracketSearchResult.OpenBracketOffset);
                        var b = this.Document.GetLineByOffset(bracketSearchResult.CloseBracketOffset);
                        this.TextArea.IndentationStrategy
                            .AsCommandIndentationStrategy()
                            .IndentLines(this.Document, a.LineNumber, b.LineNumber);
                        this.EndChange();
                    }
                    break;
                case "\r\n":
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

        private const string HandleInputStrings = "!=(){}[],:/@_ \r\n";
        
        private IEnumerable<CompletionData> GetCompletionData(int index, string input)
        {
            if (!HandleInputStrings.Contains(input))
            {
                return null;
            }

            var block = this._bracketSearcher.SearchCurrentBlock(this.Document, index - 1);

            var bracket = string.Empty;
            var inBlock = TokenType.StringBlock;

            if (block != null)
            {
                bracket = this.Document.GetCharAt(block.OpenBracketOffset).ToString();

                if (bracket == "{")
                {
                    inBlock = TokenType.TagBlock;
                }
                else if (bracket == "[")
                {
                    inBlock = TokenType.ArrayBlock;
                }
            }

            var tokens = this.Text.Tokenize(TokenizeType.All);

            var next = this.NextChar;
            var prev = (this.CaretOffset > 1)
                ? this.Document.GetCharAt(this.CaretOffset - 2)
                : '\0';

            var before = tokens
                .TakeWhile(x => x.Index < index)
                .Where(x => !x.IsMatchType(TokenType.Blank));

            var current = before.LastOrDefault();
            var prevToken = before.SkipLast(1).LastOrDefault();

            if (before.IsEmpty() ||
                current.IsMatchType(TokenType.String, TokenType.Comment))
            {
                return null;
            }
            if ("\r\n".Contains(input) &&
                (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
            {
                return null;
            }

            before = before.Where(x => !x.IsMatchType(TokenType.Comment));

            using (var reader = new TokenReader(before, true))
            {
                try
                {
                    #region コマンド
                    if (inBlock == TokenType.StringBlock)
                    {
                        switch (input.ToLower())
                        {
                            case "/":
                                return Minecraft.CommandCompletion;
                            case "@":
                                return Minecraft.SelectorCompletion;
                            case "_":
                                return (inBlock == TokenType.ArrayBlock && current.IsMatchType(TokenType.Literal))
                                    ? (current.Value.IndexOf("score_") == 0)
                                        ? (current.Value == "score_")
                                            ? this.ExtendedOptions.ScoreNames.ToCompletionData()
                                            : new[] { new CompletionData("min") }
                                        : null
                                    : null;
                        }

                        /*
                        if (!before.Any(x => x.IsMatchType(TokenType.Command)))
                        {
                            return null;
                        }
                        */

                        var command = reader.SkipGet(x => x.IsMatchType(TokenType.Command) || x.IsMatchLiteral("detect"));
                        if (command.IsEmpty())
                        {
                            return null;
                        }

                        switch (input.ToLower())
                        {
                            case ":":
                                if (!command.IsEmpty())
                                {
                                    if (reader.CheckAt(1, x => x.IsMatchLiteral("minecraft")))
                                    {
                                        goto case " ";
                                    }
                                }
                                break;
                            case ",":
                                break;
                            case "\r\n":
                            case "\r":
                            case "\n":
                            case " ":
                                if (!command.IsEmpty() && inBlock == TokenType.StringBlock)
                                {
                                    reader.Reverse();
                                    var args = this.RemainTokensToArgs(reader);
                                    var def = command.IsMatchLiteral("detect")
                                        ? Minecraft.Document.Root.Element("ExCommands").Elements("Command")
                                            .Where(x => x.Attribute("name").Value == command.Value).SingleOrDefault()
                                        : Minecraft.Document.Root.Element("Commands").Elements("Command")
                                            .Where(x => "/" + x.Attribute("name").Value == command.Value).SingleOrDefault();

                                    if (def == null)
                                    {
                                        return null;
                                    }

                                    var elements = def.Elements("Arg");
                                    var element = null as XElement;

                                    foreach (var arg in args)
                                    {
                                        if (elements.IsEmpty())
                                        {
                                            return null;
                                        }
                                        if (elements.Count() > 1)
                                        {
                                            Console.WriteLine(arg.Value);
                                            if (elements.Any(x => x.Attribute("type").Value == "key" && arg.IsMatchLiteral(x.Attribute("key").Value)))
                                            {
                                                elements = elements.Where(x => arg.IsMatchLiteral(x.Attribute("key").Value)).Elements("Arg");
                                            }
                                            else if (elements.Any(x => x.Attributes().Any(a => a.Name == "default" && a.Value == "true")))
                                            {
                                                elements = elements.Where(x => x.Attributes().Any(a => a.Name == "default" && a.Value == "true")).Elements("Arg");
                                            }
                                            else
                                            {
                                                return null;
                                            }
                                        }
                                        else
                                        {
                                            elements = elements.Elements("Arg");
                                        }
                                    }

                                    if (elements.Count() > 1)
                                    {
                                        if (elements.Any(x => x.Attributes().Any(a => a.Name == "default" && a.Value == "true")))
                                        {
                                            elements = elements.Where(x => x.Attributes().Any(a => a.Name == "default" && a.Value == "true"));
                                        }
                                        else
                                        {
                                            return elements.Attributes()
                                                .Where(x => x.Name == "key")
                                                .Select(x => new CompletionData(x.Value));
                                        }
                                    }

                                    element = elements.SingleOrDefault();
                                    if (element == null)
                                    {
                                        return null;
                                    }

                                    var type = element.Attribute("type").Value;

                                    switch (type)
                                    {
                                        case "key":
                                            return new[] { new CompletionData(element.Attribute("key").Value) };
                                        case "keywords":
                                            return element.Elements("Word")
                                                .Select(x =>
                                                new
                                                {
                                                    Name = x.Value.Trim(),
                                                    Tip = x.Attribute("desc") != null
                                                        ? x.Attribute("desc").Value
                                                        : x.Element("Desc") != null
                                                            ? x.Element("Desc").Value
                                                            : null
                                                })
                                                .Select(x => new CompletionData(x.Name, x.Tip));
                                        case "target":
                                            return this.ExtendedOptions.PlayerNames.ToCompletionData();
                                        case "target_entity":
                                            return new[] { new CompletionData("@e") };
                                        case "score":
                                            return this.ExtendedOptions.ScoreNames.ToCompletionData();
                                        case "team":
                                            return this.ExtendedOptions.TeamNames.ToCompletionData();
                                        case "item":
                                            return Minecraft.ItemCompletion;
                                        case "block":
                                            return Minecraft.BlockCompletion;
                                        case "entity":
                                            return Minecraft.EntityCompletion;
                                        case "effect":
                                            return prevToken.IsMatchLiteral("minecraft")
                                                ? Minecraft.EffectCompletion
                                                : Minecraft.EffectCompletion
                                                    .Concat(new[] { new CompletionData("clear", "すべてのエフェクトを消去します。") });
                                        case "enchant":
                                            return Minecraft.EnchantCompletion;
                                        case "boolean":
                                            return Minecraft.BooleanCompletion;
                                        case "color":
                                            return Minecraft.ColorCompletion;
                                        case "command":
                                            if (command.Value == "/execute" || command.Value == "detect")
                                            {
                                                return new[] { new CompletionData("detect") }
                                                    .Concat(Minecraft.CommandCompletion);
                                            }
                                            return Minecraft.CommandCompletion;
                                        default:
                                            return null;
                                    }
                                }
                                break;
                        }
                    }
                    #endregion
                    #region [ ... ]
                    if (inBlock == TokenType.ArrayBlock)
                    {
                        switch (input)
                        {
                            case "!":
                                if (reader.CheckNext(x => x.IsMatchType(TokenType.Equal)))
                                {
                                    reader.MoveNext();
                                    goto case "=";
                                }
                                return null;
                            case "=":
                                if (reader.IsRemainToken)
                                {
                                    var selector = before.TakeWhile(x => x.Index < block.OpenBracketOffset).LastOrDefault();
                                    if (!selector.IsEmpty() && selector.IsMatchType(TokenType.TargetSelector))
                                    {
                                        reader.MoveNext();
                                        if (reader.CheckNext(x => x.IsMatchLiteral("team")))
                                        {
                                            return this.ExtendedOptions.TeamNames.ToCompletionData();
                                        }
                                        if (reader.CheckNext(x => x.IsMatchLiteral("name")))
                                        {
                                            return this.ExtendedOptions.PlayerNames.ToCompletionData();
                                        }
                                    }
                                }
                                return null;
                        }
                    }
                    #endregion
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        private List<Token> RemainTokensToArgs(TokenReader reader)
        {
            var result = new List<Token>();

            while (reader.IsRemainToken)
            {
                var current = reader.Get();

                if (current.IsMatchType(TokenType.TargetSelector))
                {
                    if (reader.CheckNext(x => x.IsMatchType(TokenType.OpenSquareBracket)))
                    {
                        reader.Skip(x => x.IsMatchType(TokenType.CloseSquareBracket));
                        reader.MoveNext();
                    }
                }
                else if (current.IsMatchLiteral("minecraft"))
                {
                    if (reader.CheckNext(x => x.IsMatchType(TokenType.Colon)))
                    {
                        reader.MoveNext();
                        if (reader.CheckNext(x => x.IsMatchType(TokenType.Literal)))
                        {
                            current = reader.Get();
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                result.Add(current);
            }

            return result;
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
