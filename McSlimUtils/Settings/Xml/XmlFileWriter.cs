using System.IO;
using System.Xml.Serialization;

namespace Cafemoca.McSlimUtils.Settings.Xml
{
    /// <summary>
    /// Source: https://github.com/Grabacr07/KanColleViewer/blob/master/Grabacr07.KanColleViewer/Models/Data/Xml/XmlFileWriter.cs
    /// </summary>
    public static class XmlFileWriter
    {
        public static void WriteXml<T>(this T saveData, string savePath) where T : new()
        {
            var dir = Path.GetDirectoryName(Path.GetFullPath(savePath)) ?? "";
            Directory.CreateDirectory(dir);

            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                serializer.Serialize(stream, saveData);
            }
        }
    }
}
