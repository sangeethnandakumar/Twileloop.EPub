using Twileloop.EPub.Abstractions;
using Twileloop.EPub.Utils;

namespace Twileloop.EPub.Implementations
{
    public class EPubUtils : IEPubUtils
    {
        public EPubContext Context { get; set; }
        public EPubUtils(EPubContext context)
        {
            Context = context;
        }
        public List<string> GetResourcesInChapterOrder()
        {
            throw new NotImplementedException();
        }

        public List<string> GetResourcesInSpineOrder()
        {
            throw new NotImplementedException();
        }

        public List<string> GetResourcesInTOCOrder()
        {
            throw new NotImplementedException();
        }
    }
}
