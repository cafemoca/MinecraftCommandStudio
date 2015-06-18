using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Rendering;

namespace Cafemoca.CommandEditor.Renderings
{
    /// <summary>
    /// Source: http://stackoverflow.com/questions/5072761/avalonedit-highlight-current-line-even-when-not-focused
    /// </summary>
    public class HighlightCurrentLineBackgroundRenderer : IBackgroundRenderer
    {
        public SolidColorBrush BackgroundColorBrush { get; set; }

        private TextEditor _editor;

        public HighlightCurrentLineBackgroundRenderer(TextEditor editor, SolidColorBrush brush = null)
        {
            this._editor = editor;
            this.BackgroundColorBrush = new SolidColorBrush(
                brush == null ? Color.FromArgb(0x10, 0x80, 0x80, 0x80) : brush.Color);
        }

        public KnownLayer Layer
        {
            get { return KnownLayer.Background; }
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (this._editor.Document == null)
            {
                return;
            }

            textView.EnsureVisualLines();
            var currentLine = this._editor.Document.GetLineByOffset(this._editor.CaretOffset);

            foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, currentLine))
            {
                drawingContext.DrawRectangle(
                    new SolidColorBrush(this.BackgroundColorBrush.Color), null,
                    new Rect(rect.Location, new Size(textView.ActualWidth, rect.Height)));
            }
        }
    }
}
