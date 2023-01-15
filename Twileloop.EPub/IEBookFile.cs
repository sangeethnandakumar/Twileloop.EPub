using Twileloop.EPub.Abstractions;
using Twileloop.EPub.Utils;

namespace Twileloop.EPub
{

    public interface IEBookFile : IDisposable
    {
        void Load(string epubLocation, OpenMode openMode, EPubConfigOption configOption = null);
        public IEPubInfo Information { get; }
        public IEPubToc TableOfContents { get; }
        public IEPubResources Resources { get; }
        public IEPubAssets Assets { get; }
        public IEPubUtils Utilities { get; }
        void Save();
        void SaveAs(string newLocation);
    }
}