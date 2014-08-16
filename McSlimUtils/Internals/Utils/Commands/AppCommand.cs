using System.Windows.Input;

namespace Cafemoca.McSlimUtils.Internals.Utils.Commands
{
    public class AppCommand
    {
        static AppCommand()
        {
            InputGestureCollection inputs = null;

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.F4, ModifierKeys.Alt, "Alt+F4"));
            AppCommand._exit = new RoutedUICommand("", "Exit", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand._about = new RoutedUICommand("", "About", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand._programSettings = new RoutedUICommand("Edit or Review your program settings", "ProgramSettings", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand._loadFile = new RoutedUICommand("", "LoadFile", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand._saveAll = new RoutedUICommand("", "SaveAll", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand._pinUnpin = new RoutedUICommand("", "Pin", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.F4, ModifierKeys.Control, "Ctrl+F4"));
            inputs.Add(new KeyGesture(Key.W, ModifierKeys.Control, "Ctrl+W"));
            AppCommand._closeFile = new RoutedUICommand("", "Close", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand._browseURL = new RoutedUICommand("", "OpenURL", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand._showStartPage = new RoutedUICommand("", "StartPage", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand._disableHighlighting = new RoutedUICommand("", "DisableHighlighting", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.G, ModifierKeys.Control, "Ctrl+G"));
            AppCommand._gotoLine = new RoutedUICommand("", "GotoLine", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.F, ModifierKeys.Control, "Ctrl+F"));
            AppCommand._findText = new RoutedUICommand("", "FindText", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.F3, ModifierKeys.None, "F3"));
            AppCommand._findNextText = new RoutedUICommand("", "FindNextText", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.F3, ModifierKeys.Shift, "Shift+F3"));
            AppCommand._findPreviousText = new RoutedUICommand("", "FindPreviousText", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.H, ModifierKeys.Control, "Ctrl+H"));
            AppCommand._replaceText = new RoutedUICommand("", "FindReplaceText", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.R, ModifierKeys.Alt, "Alt + R"));
            AppCommand._replaceNextText = new RoutedUICommand("", "ReplaceNextText", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.A, ModifierKeys.Alt, "Alt + A"));
            AppCommand._replaceAllText = new RoutedUICommand("", "ReplaceAllText", typeof(AppCommand), inputs);
        }

        private static RoutedUICommand _exit;
        public static RoutedUICommand Exit
        {
            get { return AppCommand._exit; }
        }

        private static RoutedUICommand _about;
        public static RoutedUICommand About
        {
            get { return AppCommand._about; }
        }

        private static RoutedUICommand _programSettings;
        public static RoutedUICommand ProgramSettings
        {
            get { return AppCommand._programSettings; }
        }

        private static RoutedUICommand _loadFile;
        public static RoutedUICommand LoadFile
        {
            get { return AppCommand._loadFile; }
        }

        private static RoutedUICommand _saveAll;
        public static RoutedUICommand SaveAll
        {
            get { return AppCommand._saveAll; }
        }

        private static RoutedUICommand _pinUnpin;
        public static RoutedUICommand PinUnpin
        {
            get { return AppCommand._pinUnpin; }
        }

        private static RoutedUICommand _closeFile;
        public static RoutedUICommand CloseFile
        {
            get { return AppCommand._closeFile; }
        }

        private static RoutedUICommand _browseURL;
        public static RoutedUICommand BrowseURL
        {
            get { return AppCommand._browseURL; }
        }

        private static RoutedUICommand _showStartPage;
        public static RoutedUICommand ShowStartPage
        {
            get { return AppCommand._showStartPage; }
        }

        private static RoutedUICommand _gotoLine;
        public static RoutedUICommand GotoLine
        {
            get { return AppCommand._gotoLine; }
        }

        private static RoutedUICommand _replaceAllText;
        public static RoutedUICommand FindText
        {
            get { return AppCommand._findText; }
        }

        private static RoutedUICommand _findNextText;
        public static RoutedUICommand FindNextText
        {
            get { return AppCommand._findNextText; }
        }

        private static RoutedUICommand _findPreviousText;
        public static RoutedUICommand FindPreviousText
        {
            get { return AppCommand._findPreviousText; }
        }

        private static RoutedUICommand _replaceText;
        public static RoutedUICommand ReplaceText
        {
            get { return AppCommand._replaceText; }
        }

        private static RoutedUICommand _replaceNextText;
        public static RoutedUICommand ReplaceNextText
        {
            get { return AppCommand._replaceNextText; }
        }

        private static RoutedUICommand _findText;
        public static RoutedUICommand ReplaceAllText
        {
            get { return AppCommand._replaceAllText; }
        }

        private static RoutedUICommand _disableHighlighting;
        public static RoutedUICommand DisableHighlighting
        {
            get { return AppCommand._disableHighlighting; }
        }
    }
}
