using System.Xml.Linq;

namespace Twileloop.EPub.Utils
{
    public class EPubContext
    {
        //Locations
        public string EPubLocation { get; set; }
        public string ExtractedLocationRoot { get; set; }
        public string ExtractedLocationOEBPS { get; set; }
        //XML files
        public XDocument NcxDocument { get; set; }
        public XDocument ContentDocument { get; set; }
        //Flags
        public OpenMode OpenMode { get; set; }
    }
}
