using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace McSlimUpdater
{
    public class Updater
    {
        public Version Version { get; private set; }
        public string XmlFileName { get; private set; }
        public string BasePath { get; private set; }
        public int ProcessId { get; private set; }

        public Updater(Version version, string xmlFileName, string basePath, int processId)
        {
            this.Version = version;
            this.XmlFileName = xmlFileName;
            this.BasePath = basePath;
            this.ProcessId = processId;
        }

        public async Task StartUpdate(CancellationToken cancel)
        {
            this.NotifyProgress("アップデートのバージョンファイルを読み込んでいます ...");
            var xml = File.ReadAllText(this.XmlFileName, Encoding.UTF8);
            var releases = Releases.Parse(xml);
            var patches = releases.GetPatches(this.Version);
            this.NotifyProgress(patches.Count() + " 個のアップデートが適用されます.");

            try
            {
                var process = Process.GetProcessById(this.ProcessId);

                this.NotifyProgress("メインプロセスが終了するまで待機します ...");

                process.WaitForExit(10000);
                if (!process.HasExited)
                {
                    this.NotifyProgress("メインプロセスを強制的に終了します.");
                    process.Kill();
                    this.NotifyProgress("メインプロセスが終了されました. 続行します.");
                }
            }
            catch
            {
            }

            foreach (var patch in patches)
            {
                var ver = patch.Version.ToString();
                if (patch.Version.Revision == 0)
                {
                    ver = patch.Version.ToString(3);
                }
                this.NotifyProgress("パッケージファイルの適用を開始します: v" + ver + " - " + patch.ReleaseDate.ToString("yyyy/MM/dd"));
                foreach (var action in patch.Actions)
                {
                    await action.DoWork(this);
                }
            }
        }

        public async Task<byte[]> DownloadBinaryAsync(string url)
        {
            var client = new HttpClient { Timeout = TimeSpan.FromSeconds(90) };
            var result = await client.GetByteArrayAsync(url);
            return result;
        }

        public void NotifyProgress(string text)
        {
            App.MainViewModel.AppendLog(text + Environment.NewLine);
        }
    }
}
