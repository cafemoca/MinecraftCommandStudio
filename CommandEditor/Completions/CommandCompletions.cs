using Cafemoca.CommandEditor.Utils;
using System.Collections.Generic;

namespace Cafemoca.CommandEditor.Completions
{
    internal class CommandCompletions
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
                if (!reader.IsRemainToken)
                {
                    return new CompletionData("give") as IEnumerable<CompletionData>;
                }
                // achievement ...
                else
                {
                    var now = reader.Get();

                    // achievement give ?
                    if (now.IsMatchLiteral("give"))
                    {
                        //
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
                if (!reader.IsRemainToken)
                {
                    return this.ExtendedOptions.PlayerNames.ToCompletionData();
                }
                // give ...
                else
                {
                    var now = reader.Get();

                    // give <player> ?
                    if (this.CheckPlayer(reader))
                    {
                        // give <player> <item>
                        if (this.CheckEndOfTokens(reader, ref now))
                        {
                            return MinecraftCompletions.GetItemCompletion();
                        }
                        // give <player> ...
                        else
                        {
                            // give <player> minecraft: ?
                            if (now.IsMatchLiteral("minecraft") &&
                                reader.Ahead.IsMatchType(TokenType.Colon))
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
                        if (this.CheckEndOfTokens(reader, ref now))
                        {
                            return MinecraftCompletions.GetScoreboardObjectivesCompletion();
                        }
                        // scoreboard objectives ...
                        else
                        {
                            // scoreboard objectives add ?
                            if (now.IsMatchLiteral("add"))
                            {
                                // scoreboard objectives add <objective>
                                if (this.CheckEndOfTokens(reader, ref now))
                                {
                                    return this.ExtendedOptions.ScoreNames.ToCompletionData();
                                }
                                // scoreboard objectives add ...
                                else
                                {
                                    // scoreboard objectives add <objective> ?
                                    if (now.IsMatchType(TokenType.Literal, TokenType.String))
                                    {
                                        // scoreboard objectives add <objective> <criteria>
                                        if (this.CheckEndOfTokens(reader, ref now))
                                        {
                                            return MinecraftCompletions.GetScoreboardCriteriaCompletion();
                                        }
                                        // scoreboard objectives add <objective> ...
                                        else
                                        {
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
                                if (this.CheckEndOfTokens(reader, ref now))
                                {
                                    return MinecraftCompletions.GetScoreboardSlotsCompletion();
                                }
                                // scoreboard objectives setdisplay ...
                                else
                                {
                                    // scoreboard objectives setdisplay (list|sidebar|belowName) <objectives>
                                    if (now.IsMatchLiteral("list", "sidebar", "belowName") ||
                                        now.ContainsLiteral("sidebar.team."))
                                    {
                                        return this.GetCompletionOrEmpty(this.ExtendedOptions.ScoreNames, reader.IsRemainToken);
                                    }
                                }
                            }
                        }
                    }
                    // scoreboard players ?
                    else if (now.IsMatchLiteral("players"))
                    {
                        // scoreboard players (list|set|add|remove|reset|enable|test)
                        if (this.CheckEndOfTokens(reader, ref now))
                        {
                            return MinecraftCompletions.GetScoreboardPlayersCompletion();
                        }
                        // scoreboard players ...
                        else
                        {
                            // scoreboard players list <playername>
                            if (now.IsMatchLiteral("list"))
                            {
                                return this.GetCompletionOrEmpty(this.ExtendedOptions.PlayerNames, reader.IsRemainToken);
                            }
                            // scoreboard players (set|add|remove|reset|) ?
                            else if (now.IsMatchLiteral("set", "add", "remove", "reset"))
                            {
                                // scoreboard players (set|add|remove|reset) <playername>
                                if (this.CheckEndOfTokens(reader, ref now))
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
                            else if (now.IsMatchLiteral("enable"))
                            {
                                // scoreboard players enable <playername>
                                if (this.CheckEndOfTokens(reader, ref now))
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
                            else if (now.IsMatchLiteral("test"))
                            {
                                // scoreboard players test <playername>
                                if (this.CheckEndOfTokens(reader, ref now))
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
                            else if (now.IsMatchLiteral("operation"))
                            {
                                // scoreboard players operation <target>
                                if (this.CheckEndOfTokens(reader, ref now))
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
                                        if (this.CheckEndOfTokens(reader, ref now))
                                        {
                                            return this.ExtendedOptions.ScoreNames.ToCompletionData();
                                        }
                                        // scoreboard players operation <target> ...
                                        else
                                        {
                                            // scoreboard players operation <target> <targetObjective> ?
                                            if (now.IsMatchType(TokenType.Literal, TokenType.String))
                                            {
                                                // scoreboard players operation <target> <targetObjective> <operation>
                                                if (this.CheckEndOfTokens(reader, ref now))
                                                {
                                                    return MinecraftCompletions.GetScoreboardOperationCompletion();
                                                }
                                                // scoreboard players operation <target> <targetObjective> ...
                                                else
                                                {
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
                                                        if (this.CheckEndOfTokens(reader, ref now))
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
                    else if (now.IsMatchLiteral("teams"))
                    {
                        // scoreboard teams (list|add|remove|empty|join|leave|option)
                        if (this.CheckEndOfTokens(reader, ref now))
                        {
                            return MinecraftCompletions.GetScoreboardTeamsCompletion();
                        }
                        // scoreboard teams ...
                        else
                        {
                            // scoreboard teams list <playername>
                            if (now.IsMatchLiteral("list"))
                            {
                                return this.GetCompletionOrEmpty(this.ExtendedOptions.TeamNames, reader.IsRemainToken);
                            }
                            // scoreboard teams (add|remove|empty) <teamname>
                            else if (now.IsMatchLiteral("add", "remove", "empty"))
                            {
                                return this.GetCompletionOrEmpty(this.ExtendedOptions.TeamNames, reader.IsRemainToken);
                            }
                            // scoreboard teams (join|leave) ?
                            else if (now.IsMatchLiteral("join", "leave"))
                            {
                                // scoreboard teams (join|leave) <teamname>
                                if (this.CheckEndOfTokens(reader, ref now))
                                {
                                    return this.ExtendedOptions.TeamNames.ToCompletionData();
                                }
                                // scoreboard teams (join|leave) ...
                                else
                                {
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
                                if (this.CheckEndOfTokens(reader, ref now))
                                {
                                    return this.ExtendedOptions.TeamNames.ToCompletionData();
                                }
                                // scoreboard teams option ...
                                else
                                {
                                    // scoreboard temas option <teamname> ?
                                    if (now.IsMatchType(TokenType.Literal, TokenType.String))
                                    {
                                        // scoreboard teams option <teamname> <option>
                                        if (this.CheckEndOfTokens(reader, ref now))
                                        {
                                            return MinecraftCompletions.GetScoreboardTeamOptionCompletion();
                                        }
                                        // scoreboard teams option <teamname> ...
                                        else
                                        {
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
                return true;
            }
            return false;
        }

        private bool CheckMinecraftIdName(TokenReader reader)
        {
            if (reader.Now.IsMatchLiteral("minecraft"))
            {
                if (!reader.IsRemainToken)
                {
                    return false;
                }
                else if (reader.Ahead.IsMatchType(TokenType.Colon))
                {
                    reader.MoveNext();
                    if (reader.Ahead.IsMatchType(TokenType.Literal))
                    {
                        reader.MoveNext();
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckEndOfTokens(TokenReader reader, ref Token now)
        {
            if (!reader.IsRemainToken)
            {
                return true;
            }
            else
            {
                now = reader.Get();

                return false;
            }
        }
    }
}
