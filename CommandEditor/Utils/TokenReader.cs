using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;

namespace Cafemoca.CommandEditor.Utils
{
    internal class TokenReader : IDisposable
    {
        private List<Token> _tokens;
        private int _cursor;
        private Token _current;

        public TokenReader(IEnumerable<Token> tokens, bool reverse = false)
        {
            this._tokens = new List<Token>(reverse ? tokens.Reverse() : tokens);
            this._cursor = 0;
        }

        public int Cursor
        {
            get { return this._cursor; }
        }

        public int Count
        {
            get { return this._tokens.Count; }
        }

        public int RemainCount
        {
            get { return this._tokens.Count - this._cursor; }
        }

        public bool IsRemainToken
        {
            get { return this._cursor < this._tokens.Count; }
        }

        public Token Get()
        {
            if (!this.IsRemainToken)
            {
                throw new ReadTokenException();
            }
            return this._current = this._tokens[this._cursor++];
        }

        public Token SkipGet(Func<Token, bool> predicate)
        {
            this.Skip(predicate);
            return this.IsRemainToken
                ? this._current = this._tokens[this._cursor++]
                : this._current = default(Token);
        }

        public bool CheckNext(Func<Token, bool> predicate)
        {
            return this.IsRemainToken
                ? predicate(this._tokens[this._cursor])
                : false;
        }

        public bool CheckCurrent(Func<Token, bool> predicate)
        {
            return (this._cursor > 0)
                ? predicate(this._tokens[this._cursor - 1])
                : false;
        }

        public bool CheckPrevious(Func<Token, bool> predicate)
        {
            return (this._cursor > 1)
                ? predicate(this._tokens[this._cursor - 2])
                : false;
        }

        public bool CheckAt(int index, Func<Token, bool> predicate)
        {
            return (index >= 0 || index < this._tokens.Count)
                ? predicate(this._tokens[index])
                : false;
        }
        
        public bool CheckAtRelative(int relative, Func<Token, bool> predicate)
        {
            relative--;
            return (this._cursor + relative >= 0 && this._cursor + relative < this._tokens.Count)
                ? predicate(this._tokens[this._cursor + relative])
                : false;
        }

        public bool MoveNext()
        {
            if (this.IsRemainToken)
            {
                this._current = this._tokens[this._cursor++];
                return true;
            }
            return false;
        }

        public bool MovePrev()
        {
            if (this._cursor > 0)
            {
                this._current = this._tokens[this._cursor--];
                return true;
            }
            return false;
        }

        public void MoveFirst()
        {
            this._current = this._tokens[this._cursor = 0];
        }

        public void MoveLast()
        {
            this._current = this._tokens[this._cursor = this._tokens.Count - 1];
        }

        public void Skip(Func<Token, bool> predicate)
        {
            while (this.IsRemainToken && !predicate(this._tokens[this._cursor]))
            {
                this.MoveNext();
            }
        }

        public bool Skip(int count)
        {
            if (this.RemainCount >= count)
            {
                this._cursor += count;
                this._current = this.IsRemainToken
                    ? this._tokens[this._cursor]
                    : default(Token);
                return true;
            }
            else
            {
                this._cursor += count;
                this._current = default(Token);
                return false;
            }
        }

        public void Reverse()
        {
            this._tokens.Reverse();

            var cursor = (this._cursor == 0)
                ? this._tokens.Count + 1
                : this._cursor;

            this._cursor = this._tokens.Count - cursor + 1;
        }

        public bool CheckSequence(params Func<Token, bool>[] predicate)
        {
            if (predicate == null)
            {
                return false;
            }
            foreach (var p in predicate)
            {
                var result = p(this.Get());
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckTargetSelector(bool inScoreboard = false)
        {
            if (inScoreboard &&
                this._current.IsMatchType(TokenType.Literal, TokenType.Asterisk))
            {
                return true;
            }
            if (this._current.IsMatchType(TokenType.TargetSelector))
            {
                if (!this.MoveNext())
                {
                    return true;
                }
                else if (this._current.IsMatchType(TokenType.ArrayBlock))
                {
                    var block = this._current.Value.Tokenize(TokenizeType.Block);
                    if (block.Any(x => x.IsMatchType(TokenType.ArrayBlock, TokenType.TagBlock)))
                    {
                        return false;
                    }
                    return true;
                }
                return true;
            }
            return false;
        }

        public bool CheckMinecraftIdName(bool stopAtColon = false)
        {
            if (this._current.IsMatchLiteral("minecraft"))
            {
                if (!this.MoveNext())
                {
                    return false;
                }
                else if (this._current.IsMatchType(TokenType.Colon))
                {
                    if (stopAtColon)
                    {
                        return true;
                    }
                    if (!this.MoveNext())
                    {
                        return false;
                    }
                    if (this._current.IsMatchType(TokenType.Literal))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Dispose()
        {
        }
    }

    public class ReadTokenException : Exception
    {

    }
}
