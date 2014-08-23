using Hnx8.ReadJEnc;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Cafemoca.McCommandStudio.Models
{
    public static class FileManager
    {
        public static TextFile LoadTextFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new TextFile();
            }
            try
            {
                var info = new FileInfo(filePath);
                using (var reader = new FileReader(info))
                {
                    //reader.ReadJEnc = Setting.Current.DefaultEncoding;
                    var code = reader.Read(info);
                    var enc = code.GetEncoding();
                    return new TextFile(filePath, reader.Text, enc);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new TextFile();
            }
        }

        public static void SaveTextFile(string path, string text, Encoding enc)
        {
            try
            {
                if (enc == null)
                {
                    enc = Encoding.UTF8;
                }
                File.WriteAllText(path, text, enc);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static void SaveTextFile(TextFile file)
        {
            SaveTextFile(file.FilePath, file.Text, file.Encoding);
        }
    }

    public class TextFile
    {
        public string FilePath { get; set; }
        public string Text { get; set; }
        public Encoding Encoding { get; set; }

        public TextFile()
            : this(null, null, null)
        {
        }

        public TextFile(string path, string text, Encoding enc)
        {
            this.FilePath = path;
            this.Text = text;
            this.Encoding = enc;
        }
    }
}
