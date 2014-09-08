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
        private const string SyntaxDefinition = "Cafemoca.CommandEditor.Resources.MinecraftSyntax.xshd";

        private void LoadSyntaxHighlight()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream(SyntaxDefinition) ?? Stream.Null)
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
        private HighlightCurrentLineBackgroundRenderer _currentLineRenderer = null;

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

        private void SetHighlightCurrentLineBackground(SolidColorBrush brush)
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

                if (this._currentLineRenderer != null)
                {
                    if (this.TextArea.TextView.BackgroundRenderers.Contains(this._currentLineRenderer))
                    {
                        this.TextArea.TextView.BackgroundRenderers.Remove(this._currentLineRenderer);
                    }
                    this._currentLineRenderer = null;
                }

                this._currentLineRenderer = new HighlightCurrentLineBackgroundRenderer(this, brush.Clone());
                this.TextArea.TextView.BackgroundRenderers.Add(this._currentLineRenderer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void SetHighlightBracketBackground(SolidColorBrush brush)
        {
            try
            {
                var oldRenderer = null as BracketHighlightRenderer;

                foreach (var item in this.TextArea.TextView.BackgroundRenderers)
                {
                    if (item != null && item is BracketHighlightRenderer)
                    {
                        oldRenderer = item as BracketHighlightRenderer;
                    }
                }

                this.TextArea.TextView.BackgroundRenderers.Remove(oldRenderer);

                if (this._bracketRenderer != null)
                {
                    if (this.TextArea.TextView.BackgroundRenderers.Contains(this._bracketRenderer))
                    {
                        this.TextArea.TextView.BackgroundRenderers.Remove(this._bracketRenderer);
                    }
                    this._bracketRenderer = null;
                }

                this._bracketRenderer = new BracketHighlightRenderer(this.TextArea.TextView, brush);
                this.TextArea.TextView.BackgroundRenderers.Add(this._bracketRenderer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
