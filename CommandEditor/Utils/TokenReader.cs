using System;
using System.Collections.Generic;

namespace Cafemoca.CommandEditor.Utils
{
    internal class TokenReader : IDisposable
    {
        private readonly List<Token> _tokens;

        private int _cursor = 0;
        public int Cursor
        {
            get { return this._cursor; }
        }

        public bool IsRemainToken
        {
            get { return this._cursor < this._tokens.Count; }
        }

        public Token Ahead
        {
            get
            {
                if (!this.IsRemainToken)
                {
                    throw new Exception("IsRemainToken");
                }
                return this._tokens[this._cursor];
            }
        }

        public Token Backward
        {
            get
            {
                if (this._cursor < 2)
                {
                    throw new IndexOutOfRangeException("Backward");
                }
                return this._tokens[this._cursor - 2];
            }
        }

        public TokenReader(IEnumerable<Token> tokens)
        {
            this._tokens = new List<Token>(tokens);
        }

        public Token Get()
        {
            if (!this.IsRemainToken)
            {
                throw new Exception("IsRemainToken");
            }
            return this._tokens[this._cursor++];
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

        public Token LookAtRelative(int relative)
        {
            relative--;
            if (this._cursor + relative > this._tokens.Count ||
                this._cursor + relative < 0)
            {
                throw new Exception("IndexOutOfRange?");
            }
            return this._tokens[this._cursor + relative];
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

        public void Dispose()
        {
        }
    }
}
