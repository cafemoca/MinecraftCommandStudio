using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Cafemoca.MinecraftCommandStudio.Settings.Xml
{
    /// <summary>
    /// Source: https://github.com/Grabacr07/KanColleViewer/blob/master/Grabacr07.KanColleViewer/Models/Data/Xml/XmlSerializableDictionary.cs
    /// </summary>
    public class XmlSerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        public XmlSerializableDictionary()
            : base()
        {
        }

        public XmlSerializableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            : base(dictionary.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
        {
        }

        public XmlSerializableDictionary(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
        }

        public void ReadXml(XmlReader reader)
        {
            var serializer = new XmlSerializer(typeof(LocalKeyValuePair));
            reader.Read();
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                var lkvp = serializer.Deserialize(reader) as LocalKeyValuePair;
                if (lkvp != null)
                {
                    this.Add(lkvp.Key, lkvp.Value);
                }
            }
            reader.Read();
        }

        public void WriteXml(XmlWriter writer)
        {
            var serializer = new XmlSerializer(typeof(LocalKeyValuePair));
            foreach (var key in this.Keys)
            {
                serializer.Serialize(writer, new LocalKeyValuePair(key, this[key]));
            }
        }

        public class LocalKeyValuePair
        {
            public TKey Key { get; set; }

            public TValue Value { get; set; }

            public LocalKeyValuePair()
            {
            }

            public LocalKeyValuePair(TKey key, TValue value)
            {
                this.Key = key;
                this.Value = value;
            }
        }

        public object Clone()
        {
            return this.CloneNative();
        }

        public XmlSerializableDictionary<TKey, TValue> CloneNative()
        {
            var result = new XmlSerializableDictionary<TKey, TValue>();
            foreach (var key in this.Keys)
            {
                result.Add(key, this[key]);
            }
            return result;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }
    }
}
