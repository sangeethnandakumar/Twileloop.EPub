using System.Xml.Linq;
using Twileloop.EPub.Entities;

namespace Twileloop.EPub.Abstractions
{
    public interface IEPubInfo
    {
        //Info
        public EBookMetaData MetaData { get; }
        public List<XElement> MetaTags { get; }
        public List<EBookManifestItem> Manifest { get; }
        public List<EBookSpineItem> Spine { get; }
        public List<EBookGuide> Guides { get; }
        public bool IsResourceIsOfType(EBookResource resource, GuideTypes type);
        public void AddItemToManifest(string href, string id, string mediaType);
        public void RemoveGuide(EBookGuide guide);
        public void RemoveMetaTag(string name);
    }
}