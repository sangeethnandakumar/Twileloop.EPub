namespace Twileloop.EPub.Entities
{
    public class DCMetadata
    {
        public string Title { get; set; }
        public string Creator { get; set; }
        public List<string> Subjects { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Contributor { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Format { get; set; }
        public string Identifier { get; set; }
        public string Source { get; set; }
        public string Language { get; set; }
        public string Relation { get; set; }
        public string Coverage { get; set; }
        public string Rights { get; set; }
    }

    public class DCTermsMetadata
    {
        public string Audience { get; set; }
        public string Provenance { get; set; }
        public string Abstract { get; set; }
        public string Spatial { get; set; }
        public string Temporal { get; set; }
    }

    public class MetaData
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }

    public class EBookMetaData
    {
        public DCMetadata DublinCoreMetadata { get; set; }
        public DCTermsMetadata DublinCoreMetadataTerms { get; set; }
        public List<MetaData> MetaData { get; set; }
    }
}