using System.IO;
using System.Xml.Serialization;

namespace Cafemoca.McSlimUtils.Settings.Xml
{
    /// <summary>
    /// Source: https://github.com/Grabacr07/KanColleViewer/blob/master/Grabacr07.KanColleViewer/Models/Data/Xml/XmlFileReader.cs
    /// </summary>
    public static class XmlFileReader
    {
        public static T ReadXml<T>(this string filePath) where T : new()
        {
            if (filePath == null || !File.Exists(filePath))
            {
                throw new FileNotFoundException("XML ファイルが見つかりません", filePath);
            }
            var result = new T();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                result = ReadData<T>(stream);
            }
            return result;
        }

        public static T ReadData<T>(byte[] data) where T : new()
        {
            var result = new T();
            using (var stream = new MemoryStream(data))
            {
                result = ReadData<T>(stream);
            }
            return result;
        }

        private static T ReadData<T>(Stream stream) where T : new()
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }
    }
}
