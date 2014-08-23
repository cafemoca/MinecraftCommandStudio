using Cafemoca.McCommandStudio.Services;
using Cafemoca.McCommandStudio.Settings;
using Cafemoca.McCommandStudio.ViewModels;
using Cafemoca.McCommandStudio.Views;
using Livet;
using MahApps.Metro;
using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Cafemoca.McCommandStudio
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        internal static MainWindowViewModel MainViewModel { get; private set; }
        internal static MainWindow MainView { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (e.Args.Contains("/updated"))
            {
                Task.Run(() => AutoUpdateService.PostUpdate());
            }

            DispatcherHelper.UIDispatcher = this.Dispatcher;
            Setting.Load();

            ThemeManager.AddAppTheme("McsDark", new Uri(mcsDarkTheme, UriKind.Relative));
            ThemeManager.AddAppTheme("McsLight", new Uri(mcsLightTheme, UriKind.Relative));
            var accent = ThemeManager.GetAccent("Yellow");
            var theme = ThemeManager.GetAppTheme("McsDark");
            ThemeManager.ChangeAppStyle(App.Current, accent, theme);

            MainViewModel = new MainWindowViewModel();
            this.MainWindow = new MainWindow() { DataContext = MainViewModel };

            MainView = this.MainWindow as MainWindow;
            MainViewModel.InitializeCommandBinding(MainView);

            Observable.FromEvent<MouseButtonEventHandler, MouseButtonEventArgs>(
                    h => (sender, args) => h(args),
                    h => this.MainWindow.MouseLeftButtonDown += h,
                    h => this.MainWindow.MouseLeftButtonDown -= h)
                .Where(m => m.ButtonState == MouseButtonState.Pressed)
                .Subscribe(_ => this.MainWindow.DragMove());

            this.MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            Setting.Save();
        }

        private const string mcsDarkTheme = "/McCommandStudio;component/Themes/Colors/McsDark.xaml";
        private const string mcsLightTheme = "/McCommandStudio;component/Themes/Colors/McsLight.xaml";

        private static readonly Version _version = Assembly.GetEntryAssembly().GetName().Version;
        internal static Version Version
        {
#if DEBUG
            get { return Version.Parse("0.0.0"); }
#else
            get { return _version; }
#endif
        }

        private static readonly string _binDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        internal static string BinDirectory
        {
            get { return _binDirectory; }
        }
    }
}
