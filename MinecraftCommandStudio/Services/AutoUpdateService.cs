using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Cafemoca.MinecraftCommandStudio.Internals.Extensions;
using TaskDialogInterop;

namespace Cafemoca.MinecraftCommandStudio.Services
{
    public static class AutoUpdateService
    {
        private const string RemoteVersionXml = "https://raw.githubusercontent.com/cafemoca/MinecraftCommandStudio/update/version.xml";

        private static readonly string xmlPath = Path.Combine(App.BinDirectory, "version.xml");
        private static readonly string updaterPath = Path.Combine(App.BinDirectory, "McSlimUpdater.exe");

        private static string updaterUri;
        private static byte[] xmldata;
        private static XDocument doc;

        internal static async Task<bool> CheckUpdateAsync(Version version)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //xmldata = await client.GetByteArrayAsync("https://dl.dropboxusercontent.com/s/73qsyynu680f3tt/version.xml?dl=1");
                    xmldata = await client.GetByteArrayAsync(RemoteVersionXml);
                    var d = new StringReader(Encoding.ASCII.GetString(xmldata));
                    doc = XDocument.Load(d);
                }
                if (doc.Root == null)
                {
                    throw new Exception("");
                }

                updaterUri = doc.Root.Attribute("updater").Value;

                var releases = doc.Root.Descendants("Release");
                var latest = releases
                    .Select(r => Version.Parse(r.Attribute("version").Value))
                    .OrderBy(v => v)
                    .LastOrDefault();
                if (version != null && latest > version)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return false;
        }

        internal static async Task StartUpdateAsync(Version version)
        {
            if (updaterUri.IsEmpty())
            {
                return;
            }
            try
            {
                if (!await ReadyUpdateAsync())
                {
                    return;
                }

                var ver = "0.0.0";
                if (version != null)
                {
                    ver = version.ToString(3);
                }

                var dir = App.BinDirectory;
                var pid = Process.GetCurrentProcess().Id;
                var args = new[]
                {
                    ver,
                    xmlPath,
                    dir,
                    pid.ToString(CultureInfo.InvariantCulture)
                }.Select(s => '"' + s + '"').JoinString(" ");

                var startInfo = new ProcessStartInfo(updaterPath)
                {
                    Arguments = args,
                    UseShellExecute = true,
                    WorkingDirectory = App.BinDirectory,
                };

                Process.Start(startInfo);
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        internal static void DeleteUpdater()
        {
            try
            {
                var retry = 0;
                while (File.Exists(xmlPath) ||
                       File.Exists(updaterPath))
                {
                    try
                    {
                        if (File.Exists(xmlPath))
                        {
                            File.Delete(xmlPath);
                        }
                        if (File.Exists(updaterPath))
                        {
                            File.Delete(updaterPath);
                        }
                    }
                    catch
                    {
                        if (retry > 10)
                        {
                            break;
                        }
                        retry++;
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private static async Task<bool> ReadyUpdateAsync()
        {
            App.MainViewModel.SaveAll();
            if (App.MainViewModel.Files.Where(x => x.IsModified.Value).Any())
            {
                var dialog = new TaskDialogOptions();
                dialog.Owner = App.MainView;
                dialog.Title = "続行しますか？";
                dialog.MainIcon = VistaTaskDialogIcon.Warning;
                dialog.MainInstruction = "保存していないファイルがあります！";
                dialog.Content = "保存しない場合、現在の変更は失われます。";
                dialog.CustomButtons = new[] { "変更を破棄して続行 (&D)", "キャンセル (&C)" };
                    
                var result = TaskDialog.Show(dialog);
                switch (result.CustomButtonResult)
                {
                    case 0:
                        return true;
                    case 1:
                        return false;
                }
            }
            try
            {
                using (var client = new HttpClient())
                {
                    var updater = await client.GetByteArrayAsync(updaterUri);
                    File.WriteAllBytes(updaterPath, updater);
                }
                File.WriteAllBytes(xmlPath, xmldata);
            }
            catch
            {
                if (File.Exists(updaterPath)) File.Delete(updaterPath);
                if (File.Exists(xmlPath)) File.Delete(xmlPath);
                return false;
            }
            return true;
        }
    }
}
