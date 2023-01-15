using System.Xml.Linq;
using Twileloop.EPub.Abstractions;
using Twileloop.EPub.Entities;
using Twileloop.EPub.Utils;

namespace Twileloop.EPub.Implementations
{
    public class EPubInfo : IEPubInfo
    {
        public EPubContext Context { get; set; }
        public EPubInfo(EPubContext context)
        {
            Context = context;
        }

        public List<EBookManifestItem> Manifest => throw new NotImplementedException();

        public List<EBookSpineItem> Spine => throw new NotImplementedException();

        public List<EBookGuide> Guides => GetEBookGuides();

        public List<XElement> MetaTags => throw new NotImplementedException();

        public EBookMetaData MetaData => new EBookMetaData
        {
            DublinCoreMetadata = GetDCMetadata(),
            DublinCoreMetadataTerms = GetDCTermsMetadata(),
            MetaData = GetNonStandardMetaData(),
        };

        private List<MetaData> GetNonStandardMetaData()
        {
            XNamespace ns = Context.ContentDocument.Root.Name.Namespace;
            var metadata = Context.ContentDocument.Root.Element(ns + "metadata");
            var nonStandardMetaDataList = new List<MetaData>();

            if (metadata != null)
            {
                var metaElements = metadata.Elements(ns + "meta");
                foreach (var meta in metaElements)
                {
                    var nonStandardMetaData = new MetaData()
                    {
                        Name = meta.Attribute("name")?.Value,
                        Content = meta.Attribute("content")?.Value
                    };
                    nonStandardMetaDataList.Add(nonStandardMetaData);
                }
            }
            return nonStandardMetaDataList;
        }

        private DCMetadata GetDCMetadata()
        {
            XNamespace ns = Context.ContentDocument.Root.Name.Namespace;

            var metadata = Context.ContentDocument.Root.Element(ns + "metadata");
            XNamespace dc = "http://purl.org/dc/elements/1.1/";

            var dcMetadata = new DCMetadata();
            if (metadata != null)
            {
                var title = metadata.Element(dc + "title");
                if (title != null)
                    dcMetadata.Title = title.Value;

                var creator = metadata.Element(dc + "creator");
                if (creator != null)
                    dcMetadata.Creator = creator.Value;

                var subjects = metadata.Elements(dc + "subject");
                if (subjects != null)
                    dcMetadata.Subjects = subjects.Select(s => s.Value).ToList();

                var description = metadata.Element(dc + "description");
                if (description != null)
                    dcMetadata.Description = description.Value;

                var publisher = metadata.Element(dc + "publisher");
                if (publisher != null)
                    dcMetadata.Publisher = publisher.Value;

                var contributor = metadata.Element(dc + "contributor");
                if (contributor != null)
                    dcMetadata.Contributor = contributor.Value;

                var date = metadata.Element(dc + "date");
                if (date != null)
                    dcMetadata.Date = date.Value;

                var type = metadata.Element(dc + "type");
                if (type != null)
                    dcMetadata.Type = type.Value;

                var format = metadata.Element(dc + "format");
                if (format != null)
                    dcMetadata.Format = format.Value;

                var identifier = metadata.Element(dc + "identifier");
                if (identifier != null)
                    dcMetadata.Identifier = identifier.Value;

                var source = metadata.Element(dc + "source");
                if (source != null)
                    dcMetadata.Source = source.Value;

                var language = metadata.Element(dc + "language");
                if (language != null)
                    dcMetadata.Language = language.Value;

                var relation = metadata.Element(dc + "relation");
                if (relation != null)
                    dcMetadata.Relation = relation.Value;

                var coverage = metadata.Element(dc + "coverage");
                if (coverage != null)
                    dcMetadata.Coverage = coverage.Value;

                var rights = metadata.Element(dc + "rights");
                if (rights != null)
                    dcMetadata.Rights = rights.Value;
            }
            return dcMetadata;
        }

        private DCTermsMetadata GetDCTermsMetadata()
        {
            XNamespace ns = Context.ContentDocument.Root.Name.Namespace;

            var metadata = Context.ContentDocument.Root.Element(ns + "metadata");
            XNamespace dcterms = "http://purl.org/dc/terms/";
            var dctermsMetadata = new DCTermsMetadata();
            if (metadata != null)
            {
                var audience = metadata.Element(dcterms + "audience");
                if (audience != null)
                    dctermsMetadata.Audience = audience.Value;

                var provenance = metadata.Element(dcterms + "provenance");
                if (provenance != null)
                    dctermsMetadata.Provenance = provenance.Value;

                var abstracts = metadata.Element(dcterms + "abstract");
                if (abstracts != null)
                    dctermsMetadata.Abstract = abstracts.Value;

                var spatial = metadata.Element(dcterms + "spatial");
                if (spatial != null)
                    dctermsMetadata.Spatial = spatial.Value;

                var temporal = metadata.Element(dcterms + "temporal");
                if (temporal != null)
                    dctermsMetadata.Temporal = temporal.Value;
            }
            return dctermsMetadata;
        }


/// <summary>
/// Returns all available guides
/// </summary>
/// <returns></returns>
private List<EBookGuide> GetEBookGuides()
        {
            var ns = Context.ContentDocument.Root.GetDefaultNamespace();
            var guideElements = Context.ContentDocument.Root.Element(ns + "guide").Elements(ns + "reference");
            var guides = new List<EBookGuide>();

            foreach (var guideElement in guideElements)
            {
                var title = guideElement.Attribute("title").Value;
                var reference = guideElement.Attribute("href").Value;
                var guideType = (GuideTypes)Enum.Parse(typeof(GuideTypes), guideElement.Attribute("type").Value, true);

                var referenceValue = "";
                var referenceTail = "";
                if (reference.Contains("#"))
                {
                    referenceValue = reference.Split("#")[0];
                    referenceTail = reference.Split("#")[1];
                }
                else
                {
                    referenceValue = reference;
                }

                guides.Add(new EBookGuide()
                {
                    Title = title,
                    Reference = referenceValue,
                    ReferenceTail = referenceTail,
                    AbsoluteReference = reference,
                    GuideType = guideType
                });
            }

            return guides;
        }

        /// <summary>
        /// Checks if the specified resource is of a specific Guide Type
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsResourceIsOfType(EBookResource resource, GuideTypes type)
        {
            return Guides.Any(x => x.Reference == resource.Reference && x.GuideType == type);
        }

        /// <summary>
        /// Adds an item to the epub manifest
        /// </summary>
        /// <param name="href"></param>
        /// <param name="id"></param>
        /// <param name="mediaType"></param>
        public void AddItemToManifest(string href, string id, string mediaType)
        {
            var root = Context.ContentDocument.Root;
            var ns = root.Name.Namespace;
            var manifestNode = Context.ContentDocument.Descendants(ns + "manifest").FirstOrDefault();
            if(manifestNode is not null)
            {
                var item = new XElement(ns + "item",
                        new XAttribute("href", href),
                        new XAttribute("id", id),
                        new XAttribute("media-type", mediaType));
                manifestNode.Add(item);
            }            
        }

        public void RemoveGuide(EBookGuide guide)
        {
            XNamespace ns = Context.ContentDocument.Root.Name.Namespace;

            // Select the guide element
            var guideElement = Context.ContentDocument.Root.Element(ns + "guide");

            if (guide != null)
            {
                // Remove the guide element by href and title
                var toRemove = guideElement.Elements(ns + "reference")
                    .Where(x => (string)x.Attribute("href") == guide.AbsoluteReference && (string)x.Attribute("title") == guide.Title)
                    .ToList();
                toRemove.ForEach(x => x.Remove());
            }
        }

        public void RemoveMetaTag(string name)
        {
            XNamespace ns = Context.ContentDocument.Root.Name.Namespace;

            // Select the guide element
            var metaElement = Context.ContentDocument.Root.Element(ns + "metadata");

            if (metaElement != null)
            {
                // Remove the guide element by href and title
                var toRemove = metaElement.Elements(ns + "meta")
                .Where(x => (string)x.Attribute("name") == name).ToList();
                toRemove.ForEach(x => x.Remove());
            }
        }
    }
}
