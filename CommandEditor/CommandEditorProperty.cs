using ICSharpCode.AvalonEdit;
using System.Windows;
using System.Windows.Media;

namespace Cafemoca.CommandEditor
{
    public partial class CommandEditor : TextEditor
    {
        #region BindableText 依存関係プロパティ

        public static readonly DependencyProperty BindableTextProperty =
            DependencyProperty.Register("BindableText", typeof(string), typeof(CommandEditor),
            new PropertyMetadata("", (obj, e) =>
            {
                var editor = obj as CommandEditor;
                if ((e == null) ||
                    (editor == null) ||
                    (editor.Document == null) ||
                    (editor.Document.Text == (string)e.NewValue))
                {
                    return;
                }
                editor.Document.Text = (string)e.NewValue ?? "";
                
            }));

        public string BindableText
        {
            get { return (string)GetValue(BindableTextProperty); }
            set { SetValue(BindableTextProperty, value); }
        }

        #endregion

        #region CompiledText 依存関係プロパティ

        public static readonly DependencyProperty CompiledTextProperty =
            DependencyProperty.Register("CompiledText", typeof(string), typeof(CommandEditor));

        public string CompiledText
        {
            get { return (string)GetValue(CompiledTextProperty); }
            set { SetValue(CompiledTextProperty, value); }
        }

        #endregion

        #region Column 依存関係プロパティ

        private static readonly DependencyProperty ColumnProperty =
            DependencyProperty.Register("Column", typeof(int), typeof(CommandEditor), new UIPropertyMetadata(1));

        public int Column
        {
            get { return (int)this.GetValue(ColumnProperty); }
            set { this.SetValue(ColumnProperty, value); }
        }

        #endregion

        #region Line 依存関係プロパティ

        private static readonly DependencyProperty LineProperty =
            DependencyProperty.Register("Line", typeof(int), typeof(CommandEditor), new UIPropertyMetadata(1));

        public int Line
        {
            get { return (int)this.GetValue(LineProperty); }
            set { this.SetValue(LineProperty, value); }
        }

        #endregion

        #region EscapeMode 依存関係プロパティ


        private static readonly DependencyProperty EscapeModeProperty =
            DependencyProperty.Register("EscapeMode", typeof(EscapeModeValue), typeof(CommandEditor), new PropertyMetadata(EscapeModeValue.New));

        public EscapeModeValue EscapeMode
        {
            get { return (EscapeModeValue)this.GetValue(EscapeModeProperty); }
            set { this.SetValue(EscapeModeProperty, value); }
        }

        #endregion

        #region QuoteMode 依存関係プロパティ


        private static readonly DependencyProperty QuoteModeProperty =
            DependencyProperty.Register("QuoteMode", typeof(QuoteModeValue), typeof(CommandEditor), new PropertyMetadata(QuoteModeValue.DoubleQuoteOnly));

        public QuoteModeValue QuoteMode
        {
            get { return (QuoteModeValue)this.GetValue(QuoteModeProperty); }
            set { this.SetValue(QuoteModeProperty, value); }
        }

        #endregion

        #region CurrentLineBackgroundProperty 依存関係プロパティ

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
                    editor.HighlightCurrentLine(brush);
                }
            }));

        public SolidColorBrush CurrentLineBackground
        {
            get { return (SolidColorBrush)GetValue(CurrentLineBackgroundProperty); }
            set { SetValue(CurrentLineBackgroundProperty, value); }
        }

        #endregion
    }

    public enum EscapeModeValue
    {
        New,
        Old,
    }

    public enum QuoteModeValue
    {
        DoubleQuoteOnly,
        UseSingleQuote,
    }
}
