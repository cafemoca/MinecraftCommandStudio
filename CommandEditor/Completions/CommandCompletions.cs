using Cafemoca.CommandEditor.Utils;
using System;
using System.Collections.Generic;

namespace Cafemoca.CommandEditor.Completions
{
    internal class CommandCompletions : IDisposable
    {
        public ExtendedOptions ExtendedOptions { get; private set; }

        public CommandCompletions(ExtendedOptions options)
        {
            this.ExtendedOptions = options;
        }

        public IEnumerable<CompletionData> AchievementCompletion(TokenReader reader)
        {
            try
            {
                // achievement give
                if (!reader.MoveNext())
                {
                    return new CompletionData("give") as IEnumerable<CompletionData>;
                }
                // achievement ...
                else
                {
                    // achievement give ?
                    if (reader.CheckCurrent(x => x.IsMatchLiteral("give")))
                    {
                        // achievement give (achievement|stat)
                        if (!reader.MoveNext())
                        {
                            return null;
                        }
                        // achievement give <stat_name> ...
                        else
                        {

                        }
                    }
                }

            }
            catch
            {
            }
            return null;
        }

        public IEnumerable<CompletionData> ClearCompletion(TokenReader reader)
        {
            try
            {
                // clear <player>
                if (!reader.MoveNext())
                {
                    return ExtendedOptions.PlayerNames.ToCompletionData();
                }
                // clear ...
                else
                {
                    // clear <player> ?
                    if (reader.CheckTargetSelector())
                    {
                        // clear <player> <item>
                        if (!reader.MoveNext())
                        {
                            return MinecraftCompletions.GetItemCompletion();
                        }
                        // clear <player> ...
                        else
                        {
                            // clear <player> <item> ?
                            if (reader.CheckMinecraftIdName(true))
                            {
                                // clear <player> <item>
                                if (!reader.MoveNext())
                                {
                                    return MinecraftCompletions.GetItemCompletion();
                                }
                                // clear <player> <item> ...
                                else
                                {
                                    reader.MoveNext();
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

        public IEnumerable<CompletionData> CloneCompletion(TokenReader reader)
        {
            try
            {
                // /clone ... ?
                if (reader.Skip(9))
                {
                    // clone ... (filtered|masked|replace)
                    if (!reader.MoveNext())
                    {
                        return new[]
                        {
                            new CompletionData("filtered"),
                            new CompletionData("masked"),
                            new CompletionData("replace"),
                        };
                    }
                    // clone ...
                    else
                    {
                        // clone ...
                        if (reader.CheckCurrent(x => x.IsMatchType(TokenType.Literal)))
                        {
                            // clone ... (filtered|masked|replace) (force|move|normal)
                            if (!reader.MoveNext())
                            {
                                return new[]
                                {
                                    new CompletionData("force"),
                                    new CompletionData("move"),
                                    new CompletionData("normal"),
                                };
                            }
                            // clone ... (filtered|masked|replace) ...
                            else
                            {
                                if (reader.CheckMinecraftIdName(true))
                                {
                                    if (!reader.MoveNext())
                                    {
                                        return MinecraftCompletions.GetItemCompletion();
                                    }
                                    else
                                    {
                                        reader.MoveNext();
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

        public IEnumerable<CompletionData> GiveCompletion(TokenReader reader)
        {
            try
            {
                // give <player>
                if (!reader.MoveNext())
                {
                    return this.ExtendedOptions.PlayerNames.ToCompletionData();
                }
                // give ...
                else
                {
                    // give <player> ?
                    if (this.CheckPlayer(reader))
                    {
                        // give <player> <item>
                        if (!reader.MoveNext())
                        {
                            return MinecraftCompletions.GetItemCompletion();
                        }
                        // give <player> ...
                        else
                        {
                            // give <player> minecraft: ?
                            if (reader.CheckCurrent(x => x.IsMatchLiteral("minecraft")) &&
                                reader.CheckNext(x => x.IsMatchType(TokenType.Colon)))
                            {
                                reader.MoveNext();
                                return !reader.IsRemainToken
                                    ? MinecraftCompletions.GetItemCompletion()
                                    : null;
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

        public IEnumerable<CompletionData> ScoreboardCompletion(TokenReader reader)
        {
            try
            {
                // scoreboard (objectives|players|teams)
                if (!reader.MoveNext())
                {
                    return MinecraftCompletions.GetScoreboardCompletion();
                }
                // scoreboard ...
                else
                {
                    // scoreboard objectives ?
                    if (reader.CheckCurrent(x => x.IsMatchLiteral("objectives")))
                    {
                        // scoreboard objectives (list|add|remove|setdisplay)
                        if (!reader.MoveNext())
                        {
                            return MinecraftCompletions.GetScoreboardObjectivesCompletion();
                        }
                        // scoreboard objectives ...
                        else
                        {
                            // scoreboard objectives add ?
                            if (reader.CheckCurrent(x => x.IsMatchLiteral("add")))
                            {
                                // scoreboard objectives add <objective>
                                if (!reader.MoveNext())
                                {
                                    return this.ExtendedOptions.ScoreNames.ToCompletionData();
                                }
                                // scoreboard objectives add ...
                                else
                                {
                                    // scoreboard objectives add <objective> ?
                                    if (reader.CheckCurrent(x => x.IsMatchType(TokenType.Literal, TokenType.String)))
                                    {
                                        // scoreboard objectives add <objective> <criteria>
                                        if (!reader.MoveNext())
                                        {
                                            return MinecraftCompletions.GetScoreboardCriteriaCompletion();
                                        }
                                        // scoreboard objectives add <objective> ...
                                        else
                                        {
                                            // scoreboard objectives add <objective> trigger <trigger>
                                            if (reader.CheckCurrent(x => x.IsMatchLiteral("trigger")))
                                            {
                                                return this.GetCompletionOrEmpty(this.ExtendedOptions.ScoreNames, reader.IsRemainToken);
                                            }
                                        }
                                    }
                                }
                            }
                            // scoreboard objectives remove <objective>
                            else if (reader.CheckCurrent(x => x.IsMatchLiteral("remove")))
                            {
                                return GetCompletionOrEmpty(this.ExtendedOptions.ScoreNames, reader.IsRemainToken);
                            }
                            // scoreboard objectives setdisplay ?
                            else if (reader.CheckCurrent(x => x.IsMatchLiteral("setdisplay")))
                            {
                                // scoreboard objectives setdisplay (list|sidebar|belowName)
                                if (!reader.MoveNext())
                                {
                                    return MinecraftCompletions.GetScoreboardSlotsCompletion();
                                }
                                // scoreboard objectives setdisplay ...
                                else
                                {
                                    // scoreboard objectives setdisplay (list|sidebar|belowName) <objectives>
                                    if (reader.CheckCurrent(x => x.IsMatchLiteral("list", "belowName")) ||
                                        reader.CheckCurrent(x => x.ContainsLiteral("sidebar")))
                                    {
                                        return this.GetCompletionOrEmpty(this.ExtendedOptions.ScoreNames, reader.IsRemainToken);
                                    }
                                }
                            }
                        }
                    }
                    // scoreboard players ?
                    else if (reader.CheckCurrent(x => x.IsMatchLiteral("players")))
                    {
                        // scoreboard players (list|set|add|remove|reset|enable|test)
                        if (!reader.MoveNext())
                        {
                            return MinecraftCompletions.GetScoreboardPlayersCompletion();
                        }
                        // scoreboard players ...
                        else
                        {
                            // scoreboard players list <playername>
                            if (reader.CheckCurrent(x => x.IsMatchLiteral("list")))
                            {
                                return this.GetCompletionOrEmpty(this.ExtendedOptions.PlayerNames, reader.IsRemainToken);
                            }
                            // scoreboard players (set|add|remove|reset|) ?
                            else if (reader.CheckCurrent(x => x.IsMatchLiteral("set", "add", "remove", "reset")))
                            {
                                // scoreboard players (set|add|remove|reset) <playername>
                                if (!reader.MoveNext())
                                {
                                    return this.ExtendedOptions.PlayerNames.ToCompletionData();
                                }
                                // scoreboard players (set|add|remove|reset) ...
                                else
                                {
                                    // scoreboard players (set|add|remove|reset) <playername> <objective>
                                    if (this.CheckPlayer(reader))
                                    {
                                        return this.GetCompletionOrEmpty(this.ExtendedOptions.ScoreNames, reader.IsRemainToken);
                                    }
                                }
                            }
                            // scoreboard players enable ?
                            else if (reader.CheckCurrent(x => x.IsMatchLiteral("enable")))
                            {
                                // scoreboard players enable <playername>
                                if (!reader.MoveNext())
                                {
                                    return this.ExtendedOptions.PlayerNames.ToCompletionData();
                                }
                                // scoreboard players enable ...
                                else
                                {
                                    // scoreboard players enable <playername> <trigger>
                                    if (this.CheckPlayer(reader))
                                    {
                                        return this.GetCompletionOrEmpty(this.ExtendedOptions.ScoreNames, reader.IsRemainToken);
                                    }
                                }
                            }
                            // scoreboard players test ?
                            else if (reader.CheckCurrent(x => x.IsMatchLiteral("test")))
                            {
                                // scoreboard players test <playername>
                                if (!reader.MoveNext())
                                {
                                    return this.ExtendedOptions.PlayerNames.ToCompletionData();
                                }
                                // scoreboard players test ...
                                else
                                {
                                    // scoreboard players enable <playername> <objective>
                                    if (this.CheckPlayer(reader))
                                    {
                                        return this.GetCompletionOrEmpty(this.ExtendedOptions.ScoreNames, reader.IsRemainToken);
                                    }
                                }
                            }
                            // scoreboard players operation ?
                            else if (reader.CheckCurrent(x => x.IsMatchLiteral("operation")))
                            {
                                // scoreboard players operation <target>
                                if (!reader.MoveNext())
                                {
                                    return this.ExtendedOptions.PlayerNames.ToCompletionData();
                                }
                                // scoreboard players operation ...
                                else
                                {
                                    // scoreboard players operation <target> ?
                                    if (this.CheckPlayer(reader))
                                    {
                                        // scoreboard players operation <target> <targetObjective>
                                        if (!reader.MoveNext())
                                        {
                                            return this.ExtendedOptions.ScoreNames.ToCompletionData();
                                        }
                                        // scoreboard players operation <target> ...
                                        else
                                        {
                                            // scoreboard players operation <target> <targetObjective> ?
                                            if (reader.CheckCurrent(x => x.IsMatchType(TokenType.Literal, TokenType.String)))
                                            {
                                                // scoreboard players operation <target> <targetObjective> <operation>
                                                if (!reader.MoveNext())
                                                {
                                                    return MinecraftCompletions.GetScoreboardOperationCompletion();
                                                }
                                                // scoreboard players operation <target> <targetObjective> ...
                                                else
                                                {
                                                    // scoreboard players operation <target> <targetObjective> <operation> ?
                                                    if (reader.CheckCurrent(x => x.IsMatchType(TokenType.ScoreAdd,      // +=
                                                                                               TokenType.ScoreSubtract, // -=
                                                                                               TokenType.ScoreMultiple, // *=
                                                                                               TokenType.ScoreDivide,   // /=
                                                                                               TokenType.ScoreModulo,   // %=
                                                                                               TokenType.Equal,         // =
                                                                                               TokenType.ScoreMin,      // <
                                                                                               TokenType.ScoreMax,      // >
                                                                                               TokenType.ScoreSwaps)))   // ><
                                                    {
                                                        // scoreboard players operation <target> <targetObjective> <operation> <selector>
                                                        if (!reader.MoveNext())
                                                        {
                                                            return this.ExtendedOptions.PlayerNames.ToCompletionData();
                                                        }
                                                        // scoreboard players operation <target> <targetObjective> <operation> ...
                                                        else
                                                        {
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
                    else if (reader.CheckCurrent(x => x.IsMatchLiteral("teams")))
                    {
                        // scoreboard teams (list|add|remove|empty|join|leave|option)
                        if (!reader.MoveNext())
                        {
                            return MinecraftCompletions.GetScoreboardTeamsCompletion();
                        }
                        // scoreboard teams ...
                        else
                        {
                            // scoreboard teams list <playername>
                            if (reader.CheckCurrent(x => x.IsMatchLiteral("list")))
                            {
                                return this.GetCompletionOrEmpty(this.ExtendedOptions.TeamNames, reader.IsRemainToken);
                            }
                            // scoreboard teams (add|remove|empty) <teamname>
                            else if (reader.CheckCurrent(x => x.IsMatchLiteral("add", "remove", "empty")))
                            {
                                return this.GetCompletionOrEmpty(this.ExtendedOptions.TeamNames, reader.IsRemainToken);
                            }
                            // scoreboard teams (join|leave) ?
                            else if (reader.CheckCurrent(x => x.IsMatchLiteral("join", "leave")))
                            {
                                // scoreboard teams (join|leave) <teamname>
                                if (!reader.MoveNext())
                                {
                                    return this.ExtendedOptions.TeamNames.ToCompletionData();
                                }
                                // scoreboard teams (join|leave) ...
                                else
                                {
                                    // scoreboard temas (join|leave) <teamname> <playername>
                                    if (reader.CheckCurrent(x => x.IsMatchType(TokenType.Literal, TokenType.String)))
                                    {
                                        return this.GetCompletionOrEmpty(this.ExtendedOptions.PlayerNames, reader.IsRemainToken);
                                    }
                                }
                            }
                            // scoreboard teams (option) ?
                            else if (reader.CheckCurrent(x => x.IsMatchLiteral("option")))
                            {
                                // scoreboard teams option <teamname>
                                if (!reader.MoveNext())
                                {
                                    return this.ExtendedOptions.TeamNames.ToCompletionData();
                                }
                                // scoreboard teams option ...
                                else
                                {
                                    // scoreboard temas option <teamname> ?
                                    if (reader.CheckCurrent(x => x.IsMatchType(TokenType.Literal, TokenType.String)))
                                    {
                                        // scoreboard teams option <teamname> <option>
                                        if (!reader.MoveNext())
                                        {
                                            return MinecraftCompletions.GetScoreboardTeamOptionCompletion();
                                        }
                                        // scoreboard teams option <teamname> ...
                                        else
                                        {
                                            // scoreboard teams option <teamname> color ?
                                            if (reader.CheckCurrent(x => x.IsMatchLiteral("color")))
                                            {
                                                return !reader.IsRemainToken
                                                    ? MinecraftCompletions.GetColorCompletion()
                                                    : null;
                                            }
                                            else if (reader.CheckCurrent(x => x.IsMatchLiteral("friendlyfire", "seeFriendlyInvisibles")))
                                            {
                                                return !reader.IsRemainToken
                                                    ? MinecraftCompletions.GetBooleanCompletion()
                                                    : null;
                                            }
                                            else if (reader.CheckCurrent(x => x.IsMatchLiteral("nametagVisibility", "deathMessageVisibility")))
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

        public IEnumerable<CompletionData> SetblockCompletion(TokenReader reader)
        {
            try
            {
                // setblock ... ?
                if (reader.Skip(3))
                {
                    // setblock ... <block>
                    if (!reader.MoveNext())
                    {
                        return MinecraftCompletions.GetBlockCompletion();
                    }
                    // setblock ... <block> ...
                    else
                    {
                        // setblock ... minecraft:<block> ?
                        if (reader.CheckMinecraftIdName(true))
                        {
                            // setblock ... minecraft:<block>
                            if (!reader.MoveNext())
                            {
                                return MinecraftCompletions.GetBlockCompletion();
                            }
                            // setblock ... <block> ...
                            else
                            {
                                if (!reader.MoveNext())
                                {
                                    return new[] { new CompletionData("aa"), };
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

        private IEnumerable<CompletionData> GetCompletionOrEmpty(IEnumerable<string> completion, bool isRemainToken)
        {
            return !isRemainToken ? completion.ToCompletionData() : null;
        }

        private bool CheckPlayer(TokenReader reader)
        {
            if (reader.CheckCurrent(x => x.IsMatchType(TokenType.Literal, TokenType.Asterisk)))
            {
                return true;
            }
            if (reader.CheckCurrent(x => x.IsMatchType(TokenType.TargetSelector)))
            {
                if (!reader.IsRemainToken)
                {
                    return true;
                }
                else if (reader.CheckNext(x => x.IsMatchType(TokenType.OpenSquareBracket)))
                {
                    reader.MoveNext();
                    var close = reader.SkipGet(x => x.IsMatchType(TokenType.CloseSquareBracket));
                    if (!close.IsEmpty())
                    {
                        return true;
                    }
                }
                return true;
            }
            return false;
        }

        public void Dispose()
        {
        }
    }
}
