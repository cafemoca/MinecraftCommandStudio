using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Cafemoca.McSlimUpdater
{
    public sealed class PackageAction : ActionBase
    {
        public string Url { get; set; }

        public PackageAction(string url)
        {
            this.Url = url;
        }

        public async override Task DoWork(Updater updater)
        {
            updater.NotifyProgress("アップデートパッケージファイルをダウンロードしています ...");
            var file = null as byte[];
            for (var i = 0; i < 3; i++)
            {
                file = await updater.DownloadBinaryAsync(this.Url);
                if (file != null)
                {
                    break;
                }
                updater.NotifyProgress("--- パッケージファイルのダウンロードに失敗しました. ---");
                updater.NotifyProgress("接続を待機しています ...");
                await Task.Run(() => Thread.Sleep(10000));
                updater.NotifyProgress("再試行します.");
            }
            if (file == null)
            {
                updater.NotifyProgress("--- パッケージファイルのダウンロードに失敗しました. ---");
                throw new UpdateException("パッケージファイルが正常にダウンロードできませんでした.");
            }
            using (var ms = new MemoryStream(file))
            {
                updater.NotifyProgress("アップデートを適用します.");
                var archive = new ZipArchive(ms, ZipArchiveMode.Read);
                foreach (var entry in archive.Entries)
                {
                    updater.NotifyProgress(entry.Name + " を展開しています ...");
                    await this.Extract(entry, updater.BasePath);
                }
                updater.NotifyProgress("完了しました.");
            }
        }

        private async Task Extract(ZipArchiveEntry entry, string basePath)
        {
            var fn = Path.Combine(basePath, entry.FullName);
            var dir = Path.GetDirectoryName(fn);
            if (dir == null)
            {
                throw new UpdateException("適用対象のファイルパスが正常ではありません.");
            }
            try
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                if (string.IsNullOrEmpty(Path.GetFileName(fn)))
                {
                    return;
                }
                if (File.Exists(fn))
                {
                    File.Delete(fn);
                }
                using (var fstream = File.Create(fn))
                {
                    await entry.Open().CopyToAsync(fstream);
                }
                return;
            }
            catch (Exception ex)
            {
                var res = MessageBox.Show(
                    "ファイルの展開に失敗しました: " + entry.FullName + Environment.NewLine +
                    ex.Message + Environment.NewLine +
                    "再試行しますか？",
                    "アップデート エラー",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Error);
                if (res == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            await this.Extract(entry, basePath);
        }
    }

    public abstract class ActionBase
    {
        public abstract Task DoWork(Updater updater);

        protected void AssertPath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("パスが指定されていません.");
            }
            if (path.Contains("%") ||
                path.Contains("..") ||
                Path.GetInvalidPathChars().Any(path.Contains))
            {
                throw new ArgumentException("無効なパスが指定されました.");
            }
        }
    }

    public static class ActionExtensions
    {
        public static ActionBase Parse(this XElement element)
        {
            switch (element.Name.LocalName)
            {
                case "Package":
                    return new PackageAction(element.Attribute("url").Value);
                default:
                    throw new UpdateException("認識できないパッケージングアクションの定義です: " + element.Name);
            }
        }
    }
}
