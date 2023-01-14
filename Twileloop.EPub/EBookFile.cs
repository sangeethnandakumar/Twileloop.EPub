using Ionic.Zip;
using System.Xml.Linq;
using Twileloop.EPub.Abstractions;
using Twileloop.EPub.Processor;
using Twileloop.EPub.Utils;

namespace Twileloop.EPub.Implementations
{
    public class EBookFile : IEBookFile
    {
        internal EPubContext Context { get; set; } = new EPubContext();

        public IEPubInfo Information { get; set; }
        public IEPubTOC TableOfContents { get; set; }
        public IEPubResources Resources { get; set; }
        public IEPubAssets Assets { get; set; }
        public IEPubUtils Utilities { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Function to load an EPub file into memory
        /// </summary>
        /// <param name="epubLocation"></param>
        /// <param name="openMode"></param>
        /// <param name="configOption"></param>
        public void Load(string epubLocation, OpenMode openMode, EPubConfigOption configOption = null)
        {
            if(configOption is null)
            {
                configOption= new EPubConfigOption();
            }
            //Get EPUB
            Context.EPubLocation = epubLocation;
            //Extracted location
            Context.ExtractedLocationRoot = Helpers.CreateTempDirectory();
            //Extract file to temp folder
            Helpers.ExtractEpub(Context);
            //Load locations
            Context.ExtractedLocationOEBPS = Path.Combine(Context.ExtractedLocationRoot, "OEBPS");
            //Load NCX
            Context.NcxDocument = XDocument.Load(Path.Combine(Context.ExtractedLocationOEBPS, configOption.NCXRelativeFilePath));
            //Load OPF
            Context.ContentDocument = XDocument.Load(Path.Combine(Context.ExtractedLocationOEBPS, configOption.OPFRelativeFilePath));

            Information = new EPubInfo(Context);
            TableOfContents = new EPubTOC(Context);
            Resources = new EPubResources(Context);
            Assets = new EPubAssets(Context, Information);
            Utilities = new EPubUtils(Context);
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void SaveAs(string newLocation)
        {
            CompressDirectory(Path.Combine(Context.ExtractedLocationRoot), newLocation);
        }

        private static void CompressDirectory(string inputDirectory, string outputFile)
        {
            if (!Directory.Exists(inputDirectory))
            {
                throw new DirectoryNotFoundException($"The directory '{inputDirectory}' does not exist.");
            }

            // Ensure that the output file has the ".epub" extension
            if (!outputFile.EndsWith(".epub", StringComparison.OrdinalIgnoreCase))
            {
                outputFile = outputFile + ".epub";
            }

            // Get the full path of the input directory
            var inputPath = Path.GetFullPath(inputDirectory);

            // Create a ZipFile for the output file
            using (var zip = new ZipFile())
            {
                // Set the compression level to Fastest and the compression method to Deflate
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                zip.CompressionMethod = CompressionMethod.Deflate;

                // Add the files in the input directory to the ZipFile
                AddFilesToZip(zip, inputPath, inputPath);

                // Save the ZipFile
                zip.Save(outputFile);
            }
        }

        private static void AddFilesToZip(ZipFile zip, string root, string path)
        {
            // Add the files in the current directory to the ZipFile
            foreach (var file in Directory.GetFiles(path))
            {
                // Calculate the relative path of the file
                var relativePath = file.Substring(root.Length + 1);
                // Add the file to the ZipFile
                zip.AddFile(file, Path.GetDirectoryName(relativePath));
            }

            // Recursively add the files in the subdirectories to the ZipFile
            foreach (var directory in Directory.GetDirectories(path))
            {
                AddFilesToZip(zip, root, directory);
            }
        }
    }
}
