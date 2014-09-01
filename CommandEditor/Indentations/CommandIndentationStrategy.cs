using Cafemoca.CommandEditor.Renderings;
using Cafemoca.CommandEditor.Utils;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Indentation;
using ICSharpCode.AvalonEdit.Indentation.CSharp;
using System;
using System.Linq;

namespace Cafemoca.CommandEditor.Indentations
{
    public class CommandIndentationStrategy : DefaultIndentationStrategy
    {
        private BracketSearcher _bracketSearcher;

        public CommandIndentationStrategy() { }

        public CommandIndentationStrategy(TextEditorOptions options)
        {
            this.IndentationString = options.IndentationString;
        }

        private string _indentationString = "\t";
        public string IndentationString
        {
            get { return this._indentationString; }
            set { this._indentationString = value ?? "\t"; }
        }

        public void Indent(TextDocument document, bool keepEmptyLines)
        {
            this.Indent(new TextDocumentAccessor(document), keepEmptyLines);
        }

        public void Indent(IDocumentAccessor document, bool keepEmptyLines)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            var settings = new IndentationSettings(this.IndentationString, keepEmptyLines);
            var reformatter = new IndentationReformatter();
            reformatter.Reformat(document, settings);
        }

        public override void IndentLine(TextDocument document, DocumentLine line)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            var lineNumber = line.LineNumber;
            var accessor = new TextDocumentAccessor(document, lineNumber, lineNumber);

            this.Indent(accessor, false);

            var text = accessor.Text;
            if (text.Length == 0)
            {
                base.IndentLine(document, line);
            }
        }

        public override void IndentLines(TextDocument document, int beginLine, int endLine)
        {
            this.Indent(new TextDocumentAccessor(document, beginLine, endLine), true);
        }

        public void IndentBlock(TextDocument document, int index)
        {
            if (index < 1 ||
                index > document.TextLength)
            {
                return;
            }
            if (this._bracketSearcher == null)
            {
                this._bracketSearcher = new BracketSearcher();
            }
            var bracketSearchResult = this._bracketSearcher.SearchBrackets(document, index);
            if (bracketSearchResult == null)
            {
                return;
            }
            var a = document.GetLineByOffset(bracketSearchResult.OpenBracketOffset);
            var b = document.GetLineByOffset(bracketSearchResult.CloseBracketOffset);
            this.IndentLines(document, a.LineNumber, b.LineNumber);
        }
    }
}
