using Twileloop.EPub.Entities;

namespace Twileloop.EPub.Abstractions
{
    public interface IEPubResources
    {
        //Resources
        public List<EBookResource> HTMLResources { get; }
        public string ReadResourceAsText(EBookResource resource);
        public void WriteResourceAsText(EBookResource resource, string content);
    }
}