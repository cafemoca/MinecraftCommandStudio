using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;

namespace Cafemoca.CommandEditor.Utils
{
    internal class TokenReader : IDisposable
    {
        private List<Token> _tokens;

        public int Cursor { get; set; }

        public int Count
        {
            get { return this._tokens.Count; }
        }

        public bool IsRemainToken
        {
            get { return this.Cursor < this._tokens.Count; }
        }

        public TokenReader(IEnumerable<Token> tokens, bool reverse = false)
        {
            this._tokens = new List<Token>(reverse ? tokens.Reverse() : tokens);
            this.Cursor = 0;
        }

        public Token Now
        {
            get
            {
                if (this.Cursor < 1)
                {
                    throw new IndexOutOfRangeException("Now");
                }
                return this._tokens[this.Cursor - 1];
            }
        }

        public Token Ahead
        {
            get
            {
                if (!this.IsRemainToken)
                {
                    throw new Exception("IsRemainToken");
                }
                return this._tokens[this.Cursor];
            }
        }

        public Token Backward
        {
            get
            {
                if (this.Cursor < 2)
                {
                    throw new IndexOutOfRangeException("Backward");
                }
                return this._tokens[this.Cursor - 2];
            }
        }

        public Token Get()
        {
            if (!this.IsRemainToken)
            {
                throw new Exception("IsRemainToken");
            }
            return this._tokens[this.Cursor++];
        }

        public Token LookAt(int index)
        {
            if (index + 1 > this._tokens.Count ||
                index < 0)
            {
                throw new IndexOutOfRangeException("LookAt");
            }
            return this._tokens[index];
        }

        public Token LookAtRelative(int relative, bool safe = false)
        {
            relative--;
            if (this.Cursor + relative > this._tokens.Count ||
                this.Cursor + relative < 0)
            {
                if (safe)
                {
                    return default(Token);
                }
                else
                {
                    throw new Exception("IndexOutOfRange?");
                }
            }
            return this._tokens[this.Cursor + relative];
        }

        public Token AssertGet(TokenType type)
        {
            if (!this.IsRemainToken)
            {
                throw new Exception("IsRemainToken");
            }
            var token = this.Get();
            if (token.Type != type)
            {
                throw new Exception("UnexpectedToken");
            }
            return token;
        }

        public void CursorToLast()
        {
            this.Cursor = this._tokens.Count - 1;
        }

        public void MoveNext()
        {
            this.Cursor++;
        }

        public void Skip(int count)
        {
            this.Cursor += count;
        }

        public Token SkipGet(Func<Token, bool> predicate)
        {
            while (this.IsRemainToken && !predicate(this._tokens[this.Cursor]))
            {
                this.Get();
            }
            return this._tokens[this.Cursor++];
        }

        public void Reverse()
        {
            this._tokens.Reverse();

            var cursor = this.Cursor;
            this.Cursor = this.Count - cursor + 1;
        }

        public void Dispose()
        {
        }
    }
}
