using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Cafemoca.McSlimUpdater
{
    public class Releases
    {
        public IEnumerable<Release> Release { get; private set; }

        public static Releases Parse(string xml)
        {
            var doc = XDocument.Parse(xml);
            if (doc.Root == null || doc.Root.Name != "Releases")
            {
                throw new ArgumentException("Version.xml is not valid.");
            }
            return new Releases
            {
                Release = doc.Root.Elements("Release").Select(r => new Release(r)),
            };
        }

        public IEnumerable<Release> GetPatches(Version currentVersion)
        {
            var patches = this.Release
                .OrderBy(r => r.Version)
                .SkipWhile(r => r.Version <= currentVersion);
            if (patches.Any(r => !r.CanSkip))
            {
                return patches.Where(r => !r.CanSkip);
            }
            return new[] { patches.LastOrDefault() };
        }
    }

    public class Release
    {
        public Version Version { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public bool CanSkip { get; private set; }
        public IEnumerable<ActionBase> Actions { get; private set; }

        public Release(XElement element)
        {
            this.Version = Version.Parse(element.Attribute("version").Value);
            this.ReleaseDate = DateTime.Parse(element.Attribute("date").Value);

            var skip = element.Attribute("skippable");
            this.CanSkip = skip != null ? bool.Parse(skip.Value) : true;

            this.Actions = element.Elements().Select(a => a.Parse());
        }
    }
}
