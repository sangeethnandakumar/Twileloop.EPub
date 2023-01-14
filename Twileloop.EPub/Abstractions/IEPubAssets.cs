using Twileloop.EPub.Entities;

namespace Twileloop.EPub.Abstractions
{
    public interface IEPubAssets
    {
        //Assets
        public List<EBookAsset> Result { get; }
        public void AddAsset(string localFile, EBookAsset asset);
        public void RemoveAsset(EBookAsset asset);
        public EBookAsset FindAssetById(string assetId);
        public void GetAssetsIn(string directory);
    }
}