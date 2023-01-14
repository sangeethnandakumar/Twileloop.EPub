namespace Twileloop.EPub.Entities
{
    public class EBookChapter
    {
        public string Chapter { get; set; }
        public string Href { get; set; }
        public List<EBookChapter> Children { get; set; }
    }
}