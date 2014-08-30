using ICSharpCode.AvalonEdit;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Cafemoca.CommandEditor
{
    public partial class CommandEditor : TextEditor
    {
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

        #region PlayerNames 依存関係プロパティ

        private static readonly DependencyProperty PlayerNamesProperty =
            DependencyProperty.Register("PlayerNames", typeof(IEnumerable<string>), typeof(CommandEditor));

        public IEnumerable<string> PlayerNames
        {
            get { return (IEnumerable<string>)this.GetValue(PlayerNamesProperty); }
            set { this.SetValue(PlayerNamesProperty, value); }
        }

        #endregion

        #region ScoreNames 依存関係プロパティ

        private static readonly DependencyProperty ScoreNamesProperty =
            DependencyProperty.Register("ScoreNames", typeof(IEnumerable<string>), typeof(CommandEditor));

        public IEnumerable<string> ScoreNames
        {
            get { return (IEnumerable<string>)this.GetValue(ScoreNamesProperty); }
            set { this.SetValue(ScoreNamesProperty, value); }
        }

        #endregion

        #region TeamNames 依存関係プロパティ

        private static readonly DependencyProperty TeamNamesProperty =
            DependencyProperty.Register("TeamNames", typeof(IEnumerable<string>), typeof(CommandEditor));

        public IEnumerable<string> TeamNames
        {
            get { return (IEnumerable<string>)this.GetValue(TeamNamesProperty); }
            set { this.SetValue(TeamNamesProperty, value); }
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

        #region EncloseSelection 依存関係プロパティ

        private static readonly DependencyProperty EncloseSelectionProperty =
            DependencyProperty.Register("EncloseSelection", typeof(bool), typeof(CommandEditor),
            new UIPropertyMetadata(true));

        public bool EncloseSelection
        {
            get { return (bool)this.GetValue(EncloseSelectionProperty); }
            set { this.SetValue(EncloseSelectionProperty, value); }
        }

        #endregion

        #region EncloseMultiLine 依存関係プロパティ

        private static readonly DependencyProperty EncloseMultiLineProperty =
            DependencyProperty.Register("EncloseMultiLine", typeof(bool), typeof(CommandEditor),
            new UIPropertyMetadata(false));

        public bool EncloseMultiLine
        {
            get { return (bool)this.GetValue(EncloseMultiLineProperty); }
            set { this.SetValue(EncloseMultiLineProperty, value); }
        }

        #endregion

        #region AutoReformat 依存関係プロパティ

        private static readonly DependencyProperty AutoReformatProperty =
            DependencyProperty.Register("AutoReformat", typeof(bool), typeof(CommandEditor),
            new UIPropertyMetadata(true));

        public bool AutoReformat
        {
            get { return (bool)this.GetValue(AutoReformatProperty); }
            set { this.SetValue(AutoReformatProperty, value); }
        }

        #endregion

        #region BracketCompletion 依存関係プロパティ

        private static readonly DependencyProperty BracketCompletionProperty =
            DependencyProperty.Register("BracketCompletion", typeof(bool), typeof(CommandEditor),
            new UIPropertyMetadata(true));

        public bool BracketCompletion
        {
            get { return (bool)this.GetValue(BracketCompletionProperty); }
            set { this.SetValue(BracketCompletionProperty, value); }
        }

        #endregion

        #region EnableCompletion 依存関係プロパティ

        private static readonly DependencyProperty EnableCompletionProperty =
            DependencyProperty.Register("EnableCompletion", typeof(bool), typeof(CommandEditor),
            new UIPropertyMetadata(true));

        public bool EnableCompletion
        {
            get { return (bool)this.GetValue(EnableCompletionProperty); }
            set { this.SetValue(EnableCompletionProperty, value); }
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

        #region IsSelection

        private bool IsSelection
        {
            get { return this.SelectionLength > 0; }
        }

        #endregion

        #region IsSingleLineSelection

        private bool IsSingleLineSelection
        {
            get { return this.IsSelection && !this.SelectedText.Any(x => "\r\n".Contains(x)); }
        }

        #endregion

        #region IsMultiLineSelection

        private bool IsMultiLineSelection
        {
            get { return this.IsSelection && this.SelectedText.Any(x => "\r\n".Contains(x)); }
        }

        #endregion

        #endregion
    }
}
