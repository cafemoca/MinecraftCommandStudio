using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Cafemoca.McSlimUpdater;

namespace Cafemoca.McSlimUpdater
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private static readonly string StudioFileName = "McCommandStudio.exe";
        private readonly CancellationTokenSource _cancelSource = new CancellationTokenSource();
        private string _basePath = string.Empty;

        internal static MainViewModel MainViewModel { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainViewModel = new MainViewModel();
            this.MainWindow = new MainWindow { DataContext = MainViewModel };
            this.MainWindow.Show();

            // McSlimUpdater.exe [version] [xml_file_name] [base_path] [proccess_id]
            var args = e.Args;
            if (args == null || args.Length < 4)
            {
                this.StartMain(false);
                App.Current.Shutdown();
                Environment.Exit(1);
            }

            this._basePath = args[2];
            var version = Version.Parse(args[0]);
            var process = int.Parse(args[3]);
            var updater = new Updater(version, args[1], this._basePath, process);

            Task.Run(async () =>
            {
                try
                {
                    await updater.StartUpdate(this._cancelSource.Token);

                    updater.NotifyProgress("Mc Command Studio を起動します.");

                    this.StartMain(true);
                    App.Current.Dispatcher.Invoke(() => App.Current.Shutdown());
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    updater.NotifyProgress("--- アップデート中にエラーが発生しました ---");
                    updater.NotifyProgress(ex.Message);
                    MessageBox.Show("アップデートに失敗しました。" + Environment.NewLine +
                                    "何度試行してもアップデートが失敗する場合、申し訳ございませんが公式サイトから最新版をダウンロードしてください。",
                                    "アップデート エラー",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    updater.NotifyProgress("Mc Command Studio を起動します.");

                    this.StartMain(false);
                    App.Current.Dispatcher.Invoke(() => App.Current.Shutdown());
                    Environment.Exit(1);
                }
            });
        }

        private void StartMain(bool updated)
        {
            Thread.Sleep(100);
            var info = new ProcessStartInfo(Path.Combine(this._basePath, StudioFileName))
            {
                Arguments = updated ? "/updated" : string.Empty,
                UseShellExecute = true,
                WorkingDirectory = this._basePath,
            };
            Process.Start(info);
        }
    }
}
