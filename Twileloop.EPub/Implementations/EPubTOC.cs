using Twileloop.EPub.Abstractions;
using Twileloop.EPub.Entities;
using Twileloop.EPub.Utils;

namespace Twileloop.EPub.Implementations
{
    public class EPubTOC : IEPubTOC
    {
        public EPubContext Context { get; set; }
        public EPubTOC(EPubContext context)
        {
            Context = context;
        }


        public EBookTOC Result => throw new NotImplementedException();

        public string AddSection(string title, string resource)
        {
            throw new NotImplementedException();
        }

        public string AddSubSection(string sectionId, string title, string resource)
        {
            throw new NotImplementedException();
        }

        public void RemoveSection(string sectionId)
        {
            throw new NotImplementedException();
        }
    }
}
