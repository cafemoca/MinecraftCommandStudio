using ICSharpCode.AvalonEdit;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Cafemoca.CommandEditor
{
    public partial class CommandEditor : TextEditor
    {
        #region ExtendedOptions 依存関係プロパティ

        public static readonly DependencyProperty ExtendedOptionsProperty =
            DependencyProperty.Register("ExtendedOptions", typeof(ExtendedOptions), typeof(CommandEditor));

        public ExtendedOptions ExtendedOptions
        {
            get { return (ExtendedOptions)GetValue(ExtendedOptionsProperty); }
            set { SetValue(ExtendedOptionsProperty, value); }
        }

        #endregion

        #region BindableText 依存関係プロパティ

        public static readonly DependencyProperty BindableTextProperty =
            DependencyProperty.Register("BindableText", typeof(string), typeof(CommandEditor),
            new PropertyMetadata(null, (obj, e) =>
            {
                var editor = obj as CommandEditor;
                if ((e == null) ||
                    (editor == null) ||
                    (editor == null) ||
                    (editor.Text == (string)e.NewValue))
                {
                    return;
                }
                editor.Text = (string)e.NewValue ?? string.Empty;
                
            }));

        public string BindableText
        {
            get { return (string)GetValue(BindableTextProperty); }
            set { SetValue(BindableTextProperty, value); }
        }

        #endregion

        #region Column 依存関係プロパティ

        private static readonly DependencyProperty ColumnProperty =
            DependencyProperty.Register("Column", typeof(int), typeof(CommandEditor),
            new UIPropertyMetadata(1));

        public int Column
        {
            get { return (int)this.GetValue(ColumnProperty); }
            set { this.SetValue(ColumnProperty, value); }
        }

        #endregion

        #region Line 依存関係プロパティ

        private static readonly DependencyProperty LineProperty =
            DependencyProperty.Register("Line", typeof(int), typeof(CommandEditor),
            new UIPropertyMetadata(1));

        public int Line
        {
            get { return (int)this.GetValue(LineProperty); }
            set { this.SetValue(LineProperty, value); }
        }

        #endregion

        #region CurrentLineBackground 依存関係プロパティ

        private static readonly DependencyProperty CurrentLineBackgroundProperty =
            DependencyProperty.Register("CurrentLineBackground", typeof(SolidColorBrush), typeof(CommandEditor),
            new UIPropertyMetadata(new SolidColorBrush(Color.FromArgb(33, 33, 33, 33)), (obj, e) =>
            {
                var editor = obj as CommandEditor;
                if (e == null || editor == null)
                {
                    return;
                }
                var brush = e.NewValue as SolidColorBrush;
                if (brush != null)
                {
                    editor.SetHighlightCurrentLineBackground(brush);
                }
            }));

        public SolidColorBrush CurrentLineBackground
        {
            get { return (SolidColorBrush)GetValue(CurrentLineBackgroundProperty); }
            set { SetValue(CurrentLineBackgroundProperty, value); }
        }

        #endregion

        #region BracketBackground 依存関係プロパティ

        private static readonly DependencyProperty BracketBackgroundProperty =
            DependencyProperty.Register("BracketBackground", typeof(SolidColorBrush), typeof(CommandEditor),
            new UIPropertyMetadata(new SolidColorBrush(Color.FromArgb(64, 128, 180, 255)), (obj, e) =>
            {
                var editor = obj as CommandEditor;
                if (e == null || editor == null)
                {
                    return;
                }
                var brush = e.NewValue as SolidColorBrush;
                if (brush != null)
                {
                    editor.SetHighlightBracketBackground(brush);
                }
            }));

        public SolidColorBrush BracketBackground
        {
            get { return (SolidColorBrush)GetValue(BracketBackgroundProperty); }
            set { SetValue(BracketBackgroundProperty, value); }
        }

        #endregion

        #region private

        #region PreviousChar

        private char PreviousChar
        {
            get
            {
                return (this.CaretOffset > 0)
                    ? this.Document.GetCharAt(this.CaretOffset - 1)
                    : '\0';
            }
        }

        #endregion

        #region NextChar

        private char NextChar
        {
            get
            {
                return (this.Document.TextLength > this.CaretOffset)
                    ? this.Document.GetCharAt(this.CaretOffset)
                    : '\0';
            }
        }

        #endregion

        #region IsSelected

        private bool IsSelected
        {
            get
            {
                return this.SelectionLength > 0;
            }
        }

        #endregion

        #region IsSelectedSingleLine

        private bool IsSelectedSingleLine
        {
            get
            {
                return this.IsSelected &&
                    !this.SelectedText.Any(x => "\r\n".Contains(x));
            }
        }

        #endregion

        #region IsSelectedMultiLine

        private bool IsSelectedMultiLine
        {
            get
            {
                return this.IsSelected &&
                    this.SelectedText.Any(x => "\r\n".Contains(x));
            }
        }

        #endregion

        #endregion
    }
}
