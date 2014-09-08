using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Cafemoca.CommandEditor.Utils
{
    public class Commands
    {
        public IEnumerable<Command> Command { get; private set; }

        public static Commands Parse(string xml)
        {
            var doc = XDocument.Parse(xml);
            if (doc.Root == null || doc.Root.Name != "CommandDefinition")
            {
                throw new ArgumentException("Version.xml is not valid.");
            }
            return new Commands
            {
                Command = doc.Root.Elements("Command").Select(r => new Command(r)),
            };
        }
    }

    public class Command
    {
        public string Name { get; private set; }
        public IEnumerable<XElement> Args { get; private set; }

        public Command(XElement element)
        {
            this.Name = element.Attribute("name").Value;
            this.Args = element.Elements();
        }
    }
}
