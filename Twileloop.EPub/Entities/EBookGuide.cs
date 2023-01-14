namespace Twileloop.EPub.Entities
{
    public class EBookGuide
    {
        public string Title { get; set; }
        public string Reference { get; set; }
        public string ReferenceTail { get; set; }
        public string AbsoluteReference { get; set; }
        public GuideTypes GuideType { get; set; }
    }
}