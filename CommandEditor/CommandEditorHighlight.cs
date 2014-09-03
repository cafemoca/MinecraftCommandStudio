using Cafemoca.CommandEditor.Renderings;
using Cafemoca.CommandEditor.Utils;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Xml;

namespace Cafemoca.CommandEditor
{
    public partial class CommandEditor : TextEditor
    {
        private const string ResourceFile = "Cafemoca.CommandEditor.Resources.MinecraftSyntax.xshd";

        private void LoadSyntaxHighlight()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream(ResourceFile) ?? Stream.Null)
            using (var reader = XmlReader.Create(stream))
            {
                this.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }
        }

        private void BracketHighlightInitialize()
        {
            if (this._bracketRenderer != null)
            {
                this.TextArea.TextView.BackgroundRenderers.Remove(this._bracketRenderer);
                this._bracketRenderer = null;
            }
            this._bracketRenderer = new BracketHighlightRenderer(this.TextArea.TextView);
            this.TextArea.TextView.BackgroundRenderers.Add(this._bracketRenderer);
        }

        private BracketSearcher _bracketSearcher = null;
        private BracketHighlightRenderer _bracketRenderer = null;

        private void HighlightBrackets()
        {
            try
            {
                if (this._bracketSearcher == null)
                {
                    this._bracketSearcher = new BracketSearcher();
                }
                var bracketSearchResult = this._bracketSearcher.SearchBrackets(this.Document, this.TextArea.Caret.Offset);
                this._bracketRenderer.SetHighlight(bracketSearchResult);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void HighlightCurrentLine(SolidColorBrush brush)
        {
            try
            {
                var oldRenderer = null as HighlightCurrentLineBackgroundRenderer;

                foreach (var item in this.TextArea.TextView.BackgroundRenderers)
                {
                    if (item != null && item is HighlightCurrentLineBackgroundRenderer)
                    {
                        oldRenderer = item as HighlightCurrentLineBackgroundRenderer;
                    }
                }

                this.TextArea.TextView.BackgroundRenderers.Remove(oldRenderer);
                this.TextArea.TextView.BackgroundRenderers.Add(new HighlightCurrentLineBackgroundRenderer(this, brush.Clone()));

                if (this._bracketRenderer != null)
                {
                    this.TextArea.TextView.BackgroundRenderers.Remove(this._bracketRenderer);
                    this._bracketRenderer = null;
                }

                this._bracketRenderer = new BracketHighlightRenderer(this.TextArea.TextView);
                this.TextArea.TextView.BackgroundRenderers.Add(new BracketHighlightRenderer(this.TextArea.TextView));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
