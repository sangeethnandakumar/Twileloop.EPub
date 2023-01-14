namespace Twileloop.EPub.Entities
{
    public class EBookTOC
    {
        public string Title { get; set; }
        public Dictionary<string, string> MetaHead { get; set; }
        public List<EBookNavPoint> NavPoints { get; set; }
    }
}