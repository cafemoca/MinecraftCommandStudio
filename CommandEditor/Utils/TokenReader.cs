using System;
using System.Collections.Generic;

namespace Cafemoca.CommandEditor.Utils
{
    internal class TokenReader
    {
        private readonly List<Token> _tokens;
        private int _cursor;

        public bool IsRemainToken
        {
            get { return this._cursor < this._tokens.Count; }
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

        public Token LookAhead()
        {
            if (!this.IsRemainToken)
            {
                throw new Exception("IsRemainToken");
            }
            return _tokens[this._cursor];
        }

        public Token LookRelative(int relative)
        {
            relative--;
            if (this._cursor + relative > this._tokens.Count ||
                this._cursor + relative < 0)
            {
                throw new Exception("IndexOutOfRange?");
            }
            return this._tokens[this._cursor];
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
    }
}
