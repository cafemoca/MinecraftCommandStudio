using System.Windows.Input;

namespace Cafemoca.MinecraftCommandStudio.Internals.Utils.Commands
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
            _exit = new RoutedUICommand("", "Exit", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            _loadFile = new RoutedUICommand("", "LoadFile", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.N, ModifierKeys.Control, "Ctrl+N"));
            _new = new RoutedUICommand("", "NewFile", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.O, ModifierKeys.Control, "Ctrl+O"));
            _open = new RoutedUICommand("", "Open", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.S, ModifierKeys.Control, "Ctrl+S"));
            _save = new RoutedUICommand("", "Save", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Alt, "Ctrl+Alt+S"));
            _saveAs = new RoutedUICommand("", "SaveAs", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift, "Ctrl+Shift+S"));
            _saveAll = new RoutedUICommand("", "SaveAll", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            _pinUnpin = new RoutedUICommand("", "Pin", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.F4, ModifierKeys.Control, "Ctrl+F4"));
            inputs.Add(new KeyGesture(Key.W, ModifierKeys.Control, "Ctrl+W"));
            _closeFile = new RoutedUICommand("", "Close", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.G, ModifierKeys.Control, "Ctrl+G"));
            _gotoLine = new RoutedUICommand("", "GotoLine", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            _openSettings = new RoutedUICommand("", "OpenSettings", typeof(AppCommand), inputs);
        }

        private static RoutedUICommand _exit;
        public static RoutedUICommand Exit
        {
            get { return _exit; }
        }

        private static RoutedUICommand _loadFile;
        public static RoutedUICommand LoadFile
        {
            get { return _loadFile; }
        }

        private static RoutedUICommand _new;
        public static RoutedUICommand New
        {
            get { return _new; }
        }

        private static RoutedUICommand _open;
        public static RoutedUICommand Open
        {
            get { return _open; }
        }

        private static RoutedUICommand _save;
        public static RoutedUICommand Save
        {
            get { return _save; }
        }

        private static RoutedUICommand _saveAs;
        public static RoutedUICommand SaveAs
        {
            get { return _saveAs; }
        }

        private static RoutedUICommand _saveAll;
        public static RoutedUICommand SaveAll
        {
            get { return _saveAll; }
        }

        private static RoutedUICommand _pinUnpin;
        public static RoutedUICommand PinUnpin
        {
            get { return _pinUnpin; }
        }

        private static RoutedUICommand _closeFile;
        public static RoutedUICommand CloseFile
        {
            get { return _closeFile; }
        }

        private static RoutedUICommand _gotoLine;
        public static RoutedUICommand GotoLine
        {
            get { return _gotoLine; }
        }

        private static RoutedUICommand _openSettings;
        public static RoutedUICommand OpenSettings
        {
            get { return _openSettings; }
        }
    }
}
