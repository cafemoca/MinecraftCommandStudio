namespace Cafemoca.CommandEditor.Renderings
{
    public class BracketSearchResult
    {
        public int OpenBracketOffset { get; private set; }

        public int OpenBracketLength { get; private set; }

        public int CloseBracketOffset { get; private set; }

        public int CloseBracketLength { get; private set; }

        public BracketSearchResult(int openBracketOffset, int openBracketLength, int closeBracketOffset, int closeBracketLength)
        {
            this.OpenBracketOffset = openBracketOffset;
            this.OpenBracketLength = openBracketLength;
            this.CloseBracketOffset = closeBracketOffset;
            this.CloseBracketLength = closeBracketLength;
        }
    }
}
