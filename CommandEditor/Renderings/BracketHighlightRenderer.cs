// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Windows.Media;
using Cafemoca.CommandEditor.Utils;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace Cafemoca.CommandEditor.Renderings
{
    /// <summary>
    /// Source: https://github.com/icsharpcode/SharpDevelop/blob/master/src/AddIns/DisplayBindings/AvalonEdit.AddIn/Src/BracketHighlightRenderer.cs
    /// </summary>
    public class BracketHighlightRenderer : IBackgroundRenderer
    {
        public static readonly Color DefaultBackground = Color.FromArgb(64, 128, 180, 255);
        public static readonly Color DefaultBorder = Color.FromArgb(128, 128, 180, 255);

        public KnownLayer Layer
        {
            get { return KnownLayer.Selection; }
        }

        private BracketSearchResult _result;
        private Pen _borderPen;
        private Brush _backgroundBrush;
        private TextView _textView;
        private Color _background;
        private Color _foreground;

        public void SetHighlight(BracketSearchResult result)
        {
            if (this._result != result)
            {
                this._result = result;
                this._textView.InvalidateLayer(this.Layer);
            }
        }

        public BracketHighlightRenderer(TextView textView)
            : this (textView, DefaultBackground, DefaultBorder)
        {
        }

        public BracketHighlightRenderer(TextView textView, SolidColorBrush brush)
            : this(textView, brush.Color, brush.Color)
        {
        }

        public BracketHighlightRenderer(TextView textView, SolidColorBrush background, SolidColorBrush foreground)
            : this(textView, background.Color, foreground.Color)
        {
        }

        public BracketHighlightRenderer(TextView textView, Color background, Color foreground)
        {
            if (textView == null)
            {
                throw new ArgumentNullException();
            }

            this._background = background;
            this._foreground = foreground;
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

            if (this._borderPen == null)
            {
                this.UpdateColors(this._background, this._foreground);
            }
            if (geometry != null)
            {
                drawingContext.DrawGeometry(this._backgroundBrush, this._borderPen, geometry);
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
                break;
            }
        }
    }
}
