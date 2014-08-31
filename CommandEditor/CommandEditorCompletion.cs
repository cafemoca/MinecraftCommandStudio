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

            var bracketPair = new Dictionary<string, string>()
            {
                { "(", ")" },
                { "{", "}" },
                { "[", "]" },
            };

            switch (input)
            {
                case ")":
                case "}":
                case "]":
                    if (this.ExtendedOptions.AutoReformat)
                    {
                        this.BeginChange();
                        this.TextArea.IndentationStrategy.AsCommandIndentationStrategy().Indent(this.Document, true);
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
                            var command = reader.SkipGet(x => x.IsMatchType(TokenType.Command));
                            if (!command.IsEmpty())
                            {
                                reader.Reverse();
                                switch (command.Value)
                                {
                                    case "/scoreboard":
                                        var scoreboard = this.CompletionScoreboard(reader);
                                        if (scoreboard != null)
                                        {
                                            return scoreboard;
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

        private IEnumerable<CompletionData> GetCompletionOrEmpty(IEnumerable<string> completion, bool isRemainToken)
        {
            return !isRemainToken ? completion.ToCompletionData() : null;
        }

        private bool CheckPlayer(TokenReader reader)
        {
            if (reader.Now.IsMatchType(TokenType.Literal, TokenType.Asterisk))
            {
                return true;
            }
            if (reader.Now.IsMatchType(TokenType.TargetSelector))
            {
                if (!reader.IsRemainToken)
                {
                    return true;
                }
                else if (reader.Ahead.IsMatchType(TokenType.OpenSquareBracket))
                {
                    reader.MoveNext();
                    var close = reader.SkipGet(x => x.IsMatchType(TokenType.CloseSquareBracket));
                    if (!close.IsEmpty())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private IEnumerable<CompletionData> CompletionScoreboard(TokenReader reader)
        {
            try
            {
                // scoreboard (objectives|players|teams)
                if (!reader.IsRemainToken)
                {
                    return MinecraftCompletions.GetScoreboardCompletion();
                }
                // scoreboard ...
                else
                {
                    var now = reader.Get();

                    // scoreboard objectives ?
                    if (now.IsMatchLiteral("objectives"))
                    {
                        // scoreboard objectives (list|add|remove|setdisplay)
                        if (!reader.IsRemainToken)
                        {
                            return MinecraftCompletions.GetScoreboardObjectivesCompletion();
                        }
                        // scoreboard objectives ...
                        else
                        {
                            now = reader.Get();

                            // scoreboard objectives add ?
                            if (now.IsMatchLiteral("add"))
                            {
                                // scoreboard objectives add <objective>
                                if (!reader.IsRemainToken)
                                {
                                    return this.ExtendedOptions.ScoreNames.ToCompletionData();
                                }
                                // scoreboard objectives add ...
                                else
                                {
                                    now = reader.Get();

                                    // scoreboard objectives add <objective> ?
                                    if (now.IsMatchType(TokenType.Literal, TokenType.String))
                                    {
                                        // scoreboard objectives add <objective> <criteria>
                                        if (!reader.IsRemainToken)
                                        {
                                            return MinecraftCompletions.GetScoreboardCriteriaCompletion();
                                        }
                                        // scoreboard objectives add <objective> ...
                                        else
                                        {
                                            now = reader.Get();

                                            // scoreboard objectives add <objective> trigger <trigger>
                                            if (now.IsMatchLiteral("trigger"))
                                            {
                                                return this.GetCompletionOrEmpty(this.ExtendedOptions.ScoreNames, reader.IsRemainToken);
                                            }
                                        }
                                    }
                                }
                            }
                            // scoreboard objectives remove <objective>
                            else if (now.IsMatchLiteral("remove"))
                            {
                                return GetCompletionOrEmpty(this.ExtendedOptions.ScoreNames, reader.IsRemainToken);
                            }
                            // scoreboard objectives setdisplay ?
                            else if (now.IsMatchLiteral("setdisplay"))
                            {
                                // scoreboard objectives setdisplay (list|sidebar|belowName)
                                if (!reader.IsRemainToken)
                                {
                                    return MinecraftCompletions.GetScoreboardSlotsCompletion();
                                }
                                // scoreboard objectives setdisplay ...
                                else
                                {
                                    now = reader.Get();

                                    // scoreboard objectives setdisplay (list|sidebar|belowName) <objectives>
                                    if (now.IsMatchLiteral("list", "sidebar", "belowName") ||
                                        now.ContainsLiteral("sidebar.team."))
                                    {
                                        return GetCompletionOrEmpty(this.ExtendedOptions.ScoreNames, reader.IsRemainToken);
                                    }
                                }
                            }
                        }
                    }
                    // scoreboard players ?
                    else if (now.IsMatchLiteral("players"))
                    {
                        // scoreboard players (list|set|add|remove|reset|enable|test)
                        if (!reader.IsRemainToken)
                        {
                            return MinecraftCompletions.GetScoreboardPlayersCompletion();
                        }
                        // scoreboard players ...
                        else
                        {
                            now = reader.Get();

                            // scoreboard players list <playername>
                            if (now.IsMatchLiteral("list"))
                            {
                                return GetCompletionOrEmpty(this.ExtendedOptions.PlayerNames, reader.IsRemainToken);
                            }
                            // scoreboard players (set|add|remove|reset|) ?
                            else if (now.IsMatchLiteral("set", "add", "remove", "reset"))
                            {
                                // scoreboard players (set|add|remove|reset) <playername>
                                if (!reader.IsRemainToken)
                                {
                                    return this.ExtendedOptions.PlayerNames.ToCompletionData();
                                }
                                // scoreboard players (set|add|remove|reset) ...
                                else
                                {
                                    now = reader.Get();

                                    // scoreboard players (set|add|remove|reset) <playername> <objective>
                                    if (this.CheckPlayer(reader))
                                    {
                                        return GetCompletionOrEmpty(this.ExtendedOptions.ScoreNames, reader.IsRemainToken);
                                    }
                                }
                            }
                            // scoreboard players enable ?
                            else if (now.IsMatchLiteral("enable"))
                            {
                                // scoreboard players enable <playername>
                                if (!reader.IsRemainToken)
                                {
                                    return this.ExtendedOptions.PlayerNames.ToCompletionData();
                                }
                                // scoreboard players enable ...
                                else
                                {
                                    now = reader.Get();

                                    // scoreboard players enable <playername> <trigger>
                                    if (this.CheckPlayer(reader))
                                    {
                                        return GetCompletionOrEmpty(this.ExtendedOptions.ScoreNames, reader.IsRemainToken);
                                    }
                                }
                            }
                            // scoreboard players test ?
                            else if (now.IsMatchLiteral("test"))
                            {
                                // scoreboard players test <playername>
                                if (!reader.IsRemainToken)
                                {
                                    return this.ExtendedOptions.PlayerNames.ToCompletionData();
                                }
                                // scoreboard players test ...
                                else
                                {
                                    now = reader.Get();

                                    // scoreboard players enable <playername> <objective>
                                    if (this.CheckPlayer(reader))
                                    {
                                        return GetCompletionOrEmpty(this.ExtendedOptions.ScoreNames, reader.IsRemainToken);
                                    }
                                }
                            }
                            // scoreboard players operation ?
                            else if (now.IsMatchLiteral("operation"))
                            {
                                // scoreboard players operation <target>
                                if (!reader.IsRemainToken)
                                {
                                    return this.ExtendedOptions.PlayerNames.ToCompletionData();
                                }
                                // scoreboard players operation ...
                                else
                                {
                                    now = reader.Get();

                                    // scoreboard players operation <target> ?
                                    if (this.CheckPlayer(reader))
                                    {
                                        // scoreboard players operation <target> <targetObjective>
                                        if (!reader.IsRemainToken)
                                        {
                                            return this.ExtendedOptions.ScoreNames.ToCompletionData();
                                        }
                                        // scoreboard players operation <target> ...
                                        else
                                        {
                                            now = reader.Get();

                                            // scoreboard players operation <target> <targetObjective> ?
                                            if (now.IsMatchType(TokenType.Literal, TokenType.String))
                                            {
                                                // scoreboard players operation <target> <targetObjective> <operation>
                                                if (!reader.IsRemainToken)
                                                {
                                                    return MinecraftCompletions.GetScoreboardOperationCompletion();
                                                }
                                                // scoreboard players operation <target> <targetObjective> ...
                                                else
                                                {
                                                    now = reader.Get();

                                                    // scoreboard players operation <target> <targetObjective> <operation> ?
                                                    if (now.IsMatchType(TokenType.ScoreAdd,      // +=
                                                                        TokenType.ScoreSubtract, // -=
                                                                        TokenType.ScoreMultiple, // *=
                                                                        TokenType.ScoreDivide,   // /=
                                                                        TokenType.ScoreModulo,   // %=
                                                                        TokenType.Equal,         // =
                                                                        TokenType.ScoreMin,      // <
                                                                        TokenType.ScoreMax,      // >
                                                                        TokenType.ScoreSwaps))   // ><
                                                    {
                                                        // scoreboard players operation <target> <targetObjective> <operation> <selector>
                                                        if (!reader.IsRemainToken)
                                                        {
                                                            return this.ExtendedOptions.PlayerNames.ToCompletionData();
                                                        }
                                                        // scoreboard players operation <target> <targetObjective> <operation> ...
                                                        else
                                                        {
                                                            now = reader.Get();

                                                            // scoreboard players operation <target> <targetObjective> <operation> <selector> ?
                                                            if (this.CheckPlayer(reader))
                                                            {
                                                                return this.GetCompletionOrEmpty(this.ExtendedOptions.ScoreNames, reader.IsRemainToken);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (now.IsMatchLiteral("teams"))
                    {
                        // scoreboard teams (list|add|remove|empty|join|leave|option)
                        if (!reader.IsRemainToken)
                        {
                            return MinecraftCompletions.GetScoreboardTeamsCompletion();
                        }
                        // scoreboard teams ...
                        else
                        {
                            now = reader.Get();

                            // scoreboard teams list <playername>
                            if (now.IsMatchLiteral("list"))
                            {
                                return GetCompletionOrEmpty(this.ExtendedOptions.TeamNames, reader.IsRemainToken);
                            }
                            // scoreboard teams (add|remove|empty) <teamname>
                            else if (now.IsMatchLiteral("add", "remove", "empty"))
                            {
                                return GetCompletionOrEmpty(this.ExtendedOptions.TeamNames, reader.IsRemainToken);
                            }
                            // scoreboard teams (join|leave) ?
                            else if (now.IsMatchLiteral("join", "leave"))
                            {
                                // scoreboard teams (join|leave) <teamname>
                                if (!reader.IsRemainToken)
                                {
                                    return this.ExtendedOptions.TeamNames.ToCompletionData();
                                }
                                // scoreboard teams (join|leave) ...
                                else
                                {
                                    now = reader.Get();

                                    // scoreboard temas (join|leave) <teamname> <playername>
                                    if (now.IsMatchType(TokenType.Literal, TokenType.String))
                                    {
                                        return this.GetCompletionOrEmpty(this.ExtendedOptions.PlayerNames, reader.IsRemainToken);
                                    }
                                }
                            }
                            // scoreboard teams (option) ?
                            else if (now.IsMatchLiteral("option"))
                            {
                                // scoreboard teams option <teamname>
                                if (!reader.IsRemainToken)
                                {
                                    return this.ExtendedOptions.TeamNames.ToCompletionData();
                                }
                                // scoreboard teams option ...
                                else
                                {
                                    now = reader.Get();

                                    // scoreboard temas option <teamname> ?
                                    if (now.IsMatchType(TokenType.Literal, TokenType.String))
                                    {
                                        // scoreboard teams option <teamname> <option>
                                        if (!reader.IsRemainToken)
                                        {
                                            return MinecraftCompletions.GetScoreboardTeamOptionCompletion();
                                        }
                                        // scoreboard teams option <teamname> ...
                                        else
                                        {
                                            now = reader.Get();

                                            // scoreboard teams option <teamname> color ?
                                            if (now.IsMatchLiteral("color"))
                                            {
                                                return !reader.IsRemainToken
                                                    ? MinecraftCompletions.GetColorCompletion()
                                                    : null;
                                            }
                                            else if (now.IsMatchLiteral("friendlyfire", "seeFriendlyInvisibles"))
                                            {
                                                return !reader.IsRemainToken
                                                    ? MinecraftCompletions.GetBooleanCompletion()
                                                    : null;
                                            }
                                            else if (now.IsMatchLiteral("nametagVisibility", "deathMessageVisibility"))
                                            {
                                                return !reader.IsRemainToken
                                                    ? MinecraftCompletions.GetScoreboardTeamOptionArgsCompletion()
                                                    : null;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
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
