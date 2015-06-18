using System;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Indentation;
using ICSharpCode.AvalonEdit.Indentation.CSharp;

namespace Cafemoca.CommandEditor.Indentations
{
    public class CommandIndentationStrategy : DefaultIndentationStrategy
    {
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
    }
}
