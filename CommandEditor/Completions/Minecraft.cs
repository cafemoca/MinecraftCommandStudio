using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Cafemoca.CommandEditor.Completions
{
    internal static class Minecraft
    {
        private const string CompletionsDefinition = "Cafemoca.CommandEditor.Resources.Completions.xml";

        public static XDocument Document;

        public static void LoadMinecraftDefinition()
        {
            var asm = Assembly.GetExecutingAssembly();
            var setting = new XmlReaderSettings()
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
            };

            using (var stream = asm.GetManifestResourceStream(CompletionsDefinition) ?? Stream.Null)
            using (var reader = XmlReader.Create(stream, setting))
            {
                Document = reader != null ? XDocument.Load(reader) : new XDocument();
                Document.Descendants().Elements("Desc")
                    .ForEach(x => x.Value = x.Value.Replace("  ", "").Trim());
            }

            LoadCompletions();
        }

        public static IEnumerable<CompletionData> CommandCompletion { get; private set; }
        public static IEnumerable<CompletionData> ItemCompletion { get; private set; }
        public static IEnumerable<CompletionData> BlockCompletion { get; private set; }
        public static IEnumerable<CompletionData> EntityCompletion { get; private set; }
        public static IEnumerable<CompletionData> EffectCompletion { get; private set; }
        public static IEnumerable<CompletionData> EnchantCompletion { get; private set; }
        public static IEnumerable<CompletionData> PatternCompletion { get; private set; }
        public static IEnumerable<CompletionData> ColorCompletion { get; private set; }
        public static IEnumerable<CompletionData> SelectorCompletion { get; private set; }
        public static IEnumerable<CompletionData> BooleanCompletion { get; private set; }
        public static IEnumerable<CompletionData> SelectorArgCompletion { get; private set; }

        private static IEnumerable<CompletionData> LoadCompletion(string parent, string child)
        {
            return Document.Root
                .Element(parent)
                .Elements(child)
                .Select(x => new CompletionData(
                    x.Attribute("name").Value,
                    x.Attribute("value") != null
                        ? x.Attribute("value").Value
                        : x.Attribute("name").Value,
                    x.Attribute("desc") != null
                        ? x.Attribute("desc").Value
                        : x.Element("Desc") != null
                            ? x.Element("Desc").Value
                            : null));
        }

        public static void LoadCompletions()
        {
            try
            {
                CommandCompletion = LoadCompletion("Commands", "Command");
                ItemCompletion = LoadCompletion("Items", "Item");
                BlockCompletion = LoadCompletion("Blocks", "Block");
                EntityCompletion = LoadCompletion("Entities", "Entity");
                EffectCompletion = LoadCompletion("Effects", "Effect");
                EnchantCompletion = LoadCompletion("Enchants", "Enchant");
                PatternCompletion = LoadCompletion("Patterns", "Pattern");
                ColorCompletion = LoadCompletion("Colors", "Color");
                SelectorCompletion = LoadCompletion("Selectors", "Selector");
                BooleanCompletion = LoadCompletion("Booleans", "Boolean");
                SelectorArgCompletion = LoadCompletion("SelectorArgs", "Arg");
            }
            catch
            {
            }
        }

        public static IEnumerable<CompletionData> ToCompletionData(this IEnumerable<string> text)
        {
            if (text == null || text.IsEmpty())
            {
                return null;
            }
            return text.Select(x => new CompletionData(x, x));
        }
    }
}
