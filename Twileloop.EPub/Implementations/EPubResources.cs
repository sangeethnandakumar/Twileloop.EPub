using System.Xml.Linq;
using Twileloop.EPub.Abstractions;
using Twileloop.EPub.Entities;
using Twileloop.EPub.Utils;

namespace Twileloop.EPub.Implementations
{
    public class EPubResources : IEPubResources
    {
        public EPubContext Context { get; set; }
        public EPubResources(EPubContext context)
        {
            Context = context;
        }

        public List<EBookResource> HTMLResources => ExtractAllResources("application/xhtml+xml");

        public string ReadResourceAsText(EBookResource resource)
        {
            var href = resource.Reference.Replace("/", Path.DirectorySeparatorChar.ToString());
            var fileLoc = Path.Combine(Context.ExtractedLocationOEBPS, href);
            return File.ReadAllText(fileLoc);
        }

        /// <summary>
        /// Function extract specific resources by looking at Manifest
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        private List<EBookResource> ExtractAllResources(string resourceType)
        {
            var resources = new List<EBookResource>();
            var ns = Context.ContentDocument.Root.Name.Namespace;
            var manifest = Context.ContentDocument.Root.Element(ns + "manifest");

            foreach (var item in manifest.Elements(ns + "item"))
            {
                var resource = new EBookResource
                {
                    Id = item.Attribute("id").Value,
                    Reference = item.Attribute("href").Value,
                    MediaType = item.Attribute("media-type").Value
                };
                resources.Add(resource);
            }
            if(!string.IsNullOrEmpty(resourceType))
            {
                return resources.Where(x => x.MediaType == resourceType).ToList();
            }
            return resources;
        }

        public void WriteResourceAsText(EBookResource resource, string content)
        {
            var href = resource.Reference.Replace("/", Path.DirectorySeparatorChar.ToString());
            var fileLoc = Path.Combine(Context.ExtractedLocationOEBPS, href);
            File.WriteAllText(fileLoc, content);
        }
    }
}
