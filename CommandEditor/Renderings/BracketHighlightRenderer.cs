using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Cafemoca.CommandEditor.Renderings
{
    public class BracketHighlightRenderer : IBackgroundRenderer
    {
        public static readonly Color DefaultBackground = Color.FromArgb(22, 0, 0, 255);
        public static readonly Color DefaultBorder = Color.FromArgb(52, 0, 0, 255);

        public KnownLayer Layer
        {
            get { return KnownLayer.Selection; }
        }

        private BracketSearchResult _result;
        private Pen _borderPen;
        private Brush _backgroundBrush;
        private TextView _textView;

        public void SetHighlight(BracketSearchResult result)
        {
            if (this._result != result)
            {
                this._result = result;
                this._textView.InvalidateLayer(this.Layer);
            }
        }

        public BracketHighlightRenderer(TextView textView)
        {
            if (textView == null)
            {
                throw new ArgumentNullException();
            }

            this._textView = textView;
            this._textView.BackgroundRenderers.Add(this);
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (this._result == null)
            {
                return;
            }

            var builder = new BackgroundGeometryBuilder();

            builder.CornerRadius = 1;
            builder.AlignToMiddleOfPixels = true;

            builder.AddSegment(textView, new TextSegment()
            {
                StartOffset = _result.OpenBracketOffset,
                Length = _result.OpenBracketLength
            });

            builder.CloseFigure();

            builder.AddSegment(textView, new TextSegment()
            {
                StartOffset = _result.CloseBracketOffset,
                Length = _result.CloseBracketLength
            });

            var geometry = builder.CreateGeometry();
            if (geometry != null)
            {
                drawingContext.DrawGeometry(_backgroundBrush, _borderPen, geometry);
            }
        }

        private void UpdateColors(Color background, Color foreground)
        {
            this._borderPen = new Pen(new SolidColorBrush(foreground), 1);
            this._borderPen.Freeze();

            this._backgroundBrush = new SolidColorBrush(background);
            this._backgroundBrush.Freeze();
        }

        public static void ApplyCustomizationsToRendering(BracketHighlightRenderer renderer, IEnumerable<Color> customizations)
        {
            renderer.UpdateColors(DefaultBackground, DefaultBorder);
            foreach (var color in customizations)
            {
                renderer.UpdateColors(color, color);
            }
        }
    }
}
