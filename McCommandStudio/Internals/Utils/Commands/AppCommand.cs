using System.Windows.Input;

namespace Cafemoca.McCommandStudio.Internals.Utils.Commands
{
    /// <summary>
    /// Original: http://edi.codeplex.com/SourceControl/latest#Util/Command/AppCommand.cs
    /// </summary>
    public class AppCommand
    {
        static AppCommand()
        {
            InputGestureCollection inputs = null;

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.F4, ModifierKeys.Alt, "Alt+F4"));
            AppCommand._exit = new RoutedUICommand("", "Exit", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand._loadFile = new RoutedUICommand("", "LoadFile", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.N, ModifierKeys.Control, "Ctrl+N"));
            AppCommand._new = new RoutedUICommand("", "NewFile", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.O, ModifierKeys.Control, "Ctrl+O"));
            AppCommand._open = new RoutedUICommand("", "Open", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.S, ModifierKeys.Control, "Ctrl+S"));
            AppCommand._save = new RoutedUICommand("", "Save", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Alt, "Ctrl+Alt+S"));
            AppCommand._saveAs = new RoutedUICommand("", "SaveAs", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl+Shift+S"));
            AppCommand._saveAll = new RoutedUICommand("", "SaveAll", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand._pinUnpin = new RoutedUICommand("", "Pin", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.F4, ModifierKeys.Control, "Ctrl+F4"));
            inputs.Add(new KeyGesture(Key.W, ModifierKeys.Control, "Ctrl+W"));
            AppCommand._closeFile = new RoutedUICommand("", "Close", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.G, ModifierKeys.Control, "Ctrl+G"));
            AppCommand._gotoLine = new RoutedUICommand("", "GotoLine", typeof(AppCommand), inputs);
        }

        private static RoutedUICommand _exit;
        public static RoutedUICommand Exit
        {
            get { return AppCommand._exit; }
        }

        private static RoutedUICommand _loadFile;
        public static RoutedUICommand LoadFile
        {
            get { return AppCommand._loadFile; }
        }

        private static RoutedUICommand _new;
        public static RoutedUICommand New
        {
            get { return AppCommand._new; }
        }

        private static RoutedUICommand _open;
        public static RoutedUICommand Open
        {
            get { return AppCommand._open; }
        }

        private static RoutedUICommand _save;
        public static RoutedUICommand Save
        {
            get { return AppCommand._save; }
        }

        private static RoutedUICommand _saveAs;
        public static RoutedUICommand SaveAs
        {
            get { return AppCommand._saveAs; }
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

        private static RoutedUICommand _gotoLine;
        public static RoutedUICommand GotoLine
        {
            get { return AppCommand._gotoLine; }
        }
    }
}
