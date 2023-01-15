using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Text;

namespace Twileloop.EPub.Utils
{
    internal static class Helpers
    {
        public static string CreateTempDirectory()
        {
            string tempFolderPath = Path.Combine(AppContext.BaseDirectory, Guid.NewGuid().ToString());
            if (Directory.Exists(tempFolderPath))
            {
                Directory.Delete(tempFolderPath, recursive: true);
            }
            Directory.CreateDirectory(tempFolderPath);
            return tempFolderPath;
        }

        public static void ExtractEpub(EPubContext context)
        {
            // Open the zip file using the ZipFile class from the DotNetZip library
            using (var zip = ZipFile.Read(context.EPubLocation))
            {
                // Iterate through the entries in the zip file
                foreach (var entry in zip)
                {
                    string fullPath = Path.Combine(context.ExtractedLocationRoot, entry.FileName);
                    if (entry.IsDirectory)
                    {
                        // If the entry is a directory, create it if it does not exist.
                        if (!Directory.Exists(fullPath))
                        {
                            Directory.CreateDirectory(fullPath);
                        }
                    }
                    else
                    {
                        // If the entry is a file, extract it.
                        // Create the directory if it does not exist.
                        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                        entry.Extract(context.ExtractedLocationRoot);
                    }
                }
            }
        }
    }
}
