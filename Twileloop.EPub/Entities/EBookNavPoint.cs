namespace Twileloop.EPub.Entities
{
    public class EBookNavPoint
    {
        public string Id { get; set; }
        public int PlayOrder { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Anchor { get; set; }
        public List<EBookNavPoint> Children { get; set; }
    }
}