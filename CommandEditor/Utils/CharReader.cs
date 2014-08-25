using System;
using System.Collections.Generic;

namespace Cafemoca.CommandEditor.Utils
{
    internal class CharReader
    {
        private readonly List<char> _chars;
        private int _cursor;

        public bool IsRemainChar
        {
            get { return this._cursor < this._chars.Count; }
        }

        public CharReader(IEnumerable<char> chars)
        {
            this._chars = new List<char>(chars);
        }

        public char Get()
        {
            if (!this.IsRemainChar)
            {
                return '\0';
            }
            return this._chars[this._cursor++];
        }

        public char Ahead
        {
            get { return this.IsRemainChar ? this._chars[this._cursor] : '\0'; }
        }

        public char Backward
        {
            get { return this._cursor > 1 ? this._chars[this._cursor - 2] : '\0'; }
        }

        public char LookAtRelative(int relative)
        {
            relative--;
            if (this._cursor + relative > this._chars.Count ||
                this._cursor + relative < 0)
            {
                return '\0';
            }
            return this._chars[this._cursor];
        }
    }
}
