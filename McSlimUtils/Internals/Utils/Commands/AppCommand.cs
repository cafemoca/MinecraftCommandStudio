using System.Windows.Input;

namespace Cafemoca.McSlimUtils.Internals.Utils.Commands
{
    public class AppCommand
    {
        private static RoutedUICommand exit;
        private static RoutedUICommand about;
        private static RoutedUICommand programSettings;

        private static RoutedUICommand loadFile;
        private static RoutedUICommand saveAll;
        private static RoutedUICommand exportUMLToImage;
        private static RoutedUICommand exportTextToHTML;

        private static RoutedUICommand pinUnpin;
        private static RoutedUICommand addMruEntry;
        private static RoutedUICommand removeMruEntry;
        private static RoutedUICommand closeFile;
        private static RoutedUICommand viewTheme;

        private static RoutedUICommand browseURL;
        private static RoutedUICommand showStartPage;

        private static RoutedUICommand gotoLine;
        private static RoutedUICommand findText;
        private static RoutedUICommand findNextText;
        private static RoutedUICommand findPreviousText;
        private static RoutedUICommand replaceText;

        private static RoutedUICommand disableHighlighting;

        static AppCommand()
        {
            InputGestureCollection inputs = null;

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.F4, ModifierKeys.Alt, "Alt+F4"));
            AppCommand.exit = new RoutedUICommand("", "Exit", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand.about = new RoutedUICommand("", "About", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand.programSettings = new RoutedUICommand("Edit or Review your program settings", "ProgramSettings", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand.loadFile = new RoutedUICommand("", "LoadFile", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand.saveAll = new RoutedUICommand("", "SaveAll", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand.exportUMLToImage = new RoutedUICommand("", "ExportUMLToImage", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand.exportTextToHTML = new RoutedUICommand("", "ExportTextToHTML", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand.pinUnpin = new RoutedUICommand("", "Pin", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand.addMruEntry = new RoutedUICommand("", "AddEntry", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand.removeMruEntry = new RoutedUICommand("", "RemoveEntry", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.F4, ModifierKeys.Control, "Ctrl+F4"));
            inputs.Add(new KeyGesture(Key.W, ModifierKeys.Control, "Ctrl+W"));
            AppCommand.closeFile = new RoutedUICommand("", "Close", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand.viewTheme = new RoutedUICommand("", "ViewTheme", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand.browseURL = new RoutedUICommand("", "OpenURL", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand.showStartPage = new RoutedUICommand("", "StartPage", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            AppCommand.disableHighlighting = new RoutedUICommand("", "DisableHighlighting", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.G, ModifierKeys.Control, "Ctrl+G"));
            AppCommand.gotoLine = new RoutedUICommand("", "GotoLine", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.F, ModifierKeys.Control, "Ctrl+F"));
            AppCommand.findText = new RoutedUICommand("", "FindText", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.F3, ModifierKeys.None, "F3"));
            AppCommand.findNextText = new RoutedUICommand("", "FindNextText", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.F3, ModifierKeys.Shift, "Shift+F3"));
            AppCommand.findPreviousText = new RoutedUICommand("", "FindPreviousText", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.H, ModifierKeys.Control, "Ctrl+H"));
            AppCommand.replaceText = new RoutedUICommand("", "FindReplaceText", typeof(AppCommand), inputs);
        }

        public static RoutedUICommand Exit
        {
            get { return AppCommand.exit; }
        }

        public static RoutedUICommand About
        {
            get { return AppCommand.about; }
        }

        public static RoutedUICommand ProgramSettings
        {
            get { return AppCommand.programSettings; }
        }

        public static RoutedUICommand LoadFile
        {
            get { return AppCommand.loadFile; }
        }

        public static RoutedUICommand SaveAll
        {
            get { return AppCommand.saveAll; }
        }

        public static RoutedUICommand ExportUMLToImage
        {
            get { return AppCommand.exportUMLToImage; }
        }

        public static RoutedUICommand ExportTextToHTML
        {
            get { return AppCommand.exportTextToHTML; }
        }

        public static RoutedUICommand PinUnpin
        {
            get { return AppCommand.pinUnpin; }
        }

        public static RoutedUICommand AddMruEntry
        {
            get { return AppCommand.addMruEntry; }
        }

        public static RoutedUICommand RemoveMruEntry
        {
            get { return AppCommand.removeMruEntry; }
        }

        public static RoutedUICommand CloseFile
        {
            get { return AppCommand.closeFile; }
        }

        public static RoutedUICommand ViewTheme
        {
            get { return AppCommand.viewTheme; }
        }

        public static RoutedUICommand BrowseURL
        {
            get { return AppCommand.browseURL; }
        }

        public static RoutedUICommand ShowStartPage
        {
            get { return AppCommand.showStartPage; }
        }

        public static RoutedUICommand GotoLine
        {
            get { return AppCommand.gotoLine; }
        }

        public static RoutedUICommand FindText
        {
            get { return AppCommand.findText; }
        }

        public static RoutedUICommand FindNextText
        {
            get { return AppCommand.findNextText; }
        }

        public static RoutedUICommand FindPreviousText
        {
            get { return AppCommand.findPreviousText; }
        }

        public static RoutedUICommand ReplaceText
        {
            get { return AppCommand.replaceText; }
        }

        public static RoutedUICommand DisableHighlighting
        {
            get { return AppCommand.disableHighlighting; }
        }
    }
}
