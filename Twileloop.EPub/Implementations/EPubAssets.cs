using Twileloop.EPub.Abstractions;
using Twileloop.EPub.Entities;
using Twileloop.EPub.Utils;

namespace Twileloop.EPub.Implementations
{
    public class EPubAssets : IEPubAssets
    {
        private readonly IEPubInfo pubInfo;
        public EPubContext Context { get; set; }
        public EPubAssets(EPubContext context, IEPubInfo pubInfo)
        {
            Context = context;
            this.pubInfo = pubInfo;
        }
        public List<EBookAsset> Result => ExtractAllAssets();

        /// <summary>
        /// Extract all assets from Manifest
        /// </summary>
        /// <returns></returns>
        private List<EBookAsset> ExtractAllAssets()
        {
            var assets = new List<EBookAsset>();
            var ns = Context.ContentDocument.Root.Name.Namespace;
            var manifest = Context.ContentDocument.Root.Element(ns + "manifest");

            foreach (var item in manifest.Elements(ns + "item"))
            {
                var asset = new EBookAsset
                {
                    Id = item.Attribute("id").Value,
                    FileName = item.Attribute("href").Value,
                    MediaType = item.Attribute("media-type").Value,
                    Directory = Path.GetDirectoryName(item.Attribute("href").Value).Replace("/", Path.DirectorySeparatorChar.ToString())
                };
                assets.Add(asset);
            }
            return assets.Where(x => x.MediaType != AssetMediaType.XHTML).ToList();
        }

        /// <summary>
        /// Function to add an asset to the specified folder location in epub
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="asset"></param>
        public void AddAsset(string sourceFile, EBookAsset asset)
        {
            //Step 1: Deflate directory structure
            var fullDirectory = (Context.ExtractedLocationOEBPS + Path.DirectorySeparatorChar + asset.Directory)
               .Replace("/", Path.DirectorySeparatorChar.ToString())
               .Replace("\\", Path.DirectorySeparatorChar.ToString());

            if (!Directory.Exists(fullDirectory))
            {
                Directory.CreateDirectory(fullDirectory);
            }

            //Step 2: Copy Image
            try
            {
                File.Copy(sourceFile, fullDirectory + Path.DirectorySeparatorChar + asset.FileName, true);
            }
            catch (IOException e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }

            //Step 3: Register In Manifest
            var relativePath = fullDirectory.Replace(Context.ExtractedLocationOEBPS + Path.DirectorySeparatorChar, string.Empty) + Path.DirectorySeparatorChar + asset.FileName;
            var htmlPath = relativePath.Replace(Path.DirectorySeparatorChar.ToString(), "/");
            pubInfo.AddItemToManifest(htmlPath, asset.Id, asset.MediaType);
        }

        public void GetAssetsIn(string directory)
        {
            throw new NotImplementedException();
        }

        public void RemoveAsset(EBookAsset asset)
        {
            var assetLocation = Path.Combine(Context.ExtractedLocationOEBPS, asset.Directory, asset.FileName);
            File.Delete(assetLocation);
        }

        public EBookAsset FindAssetById(string assetId)
        {
            return Result.FirstOrDefault(x=>x.Id== assetId);
        }
    }
}
