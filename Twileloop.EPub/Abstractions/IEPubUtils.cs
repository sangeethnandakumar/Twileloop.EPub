namespace Twileloop.EPub.Abstractions
{
    public interface IEPubUtils
    {
        //Utils
        List<string> GetResourcesInSpineOrder();
        List<string> GetResourcesInTOCOrder();
        List<string> GetResourcesInChapterOrder();
    }
}