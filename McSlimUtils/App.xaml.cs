using Cafemoca.McSlimUtils.Services;
using Cafemoca.McSlimUtils.Settings;
using Cafemoca.McSlimUtils.ViewModels;
using Cafemoca.McSlimUtils.Views;
using Livet;
using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Cafemoca.McSlimUtils
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

            DispatcherHelper.UIDispatcher = this.Dispatcher;
            Setting.Initialize();

            var accent = ThemeManager.GetAccent("Yellow");
            var theme = ThemeManager.GetAppTheme("BaseDark");
            ThemeManager.ChangeAppStyle(App.Current, accent, theme);

            MainViewModel = new MainWindowViewModel();
            this.MainWindow = new MainWindow() { DataContext = MainViewModel };

            MainView = this.MainWindow as MainWindow;

            Observable.FromEvent<MouseButtonEventHandler, MouseButtonEventArgs>(
                    h => (sender, args) => h(args),
                    h => this.MainWindow.MouseLeftButtonDown += h,
                    h => this.MainWindow.MouseLeftButtonDown -= h)
                .Where(m => m.ButtonState == MouseButtonState.Pressed)
                .Subscribe(_ => this.MainWindow.DragMove());

            this.MainWindow.Show();
        }
    }
}
