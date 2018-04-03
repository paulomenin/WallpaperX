using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperX
{
    class PictureCache
    {
        string SourceName { get; set; }

        public PictureCache(string sourceName)
        {
            SourceName = sourceName;
        }

        public string GetCachePath()
        {
            string exeLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string cachePath = Path.Combine(exeLocation, "cache", SourceName);

            return cachePath;
        }

        public Uri DownloadImage(Uri url, string filename = "")
        {
            // Generate output filename
            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = Path.GetFileName(url.LocalPath);
            }

            string outputFilePath = Path.Combine(GetCachePath(), filename);
            string outputDir = Path.GetDirectoryName(outputFilePath);

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            if (!File.Exists(outputFilePath))
            {
                // Download
                using (var client = new WebClient())
                {
                    client.DownloadFile(url, outputFilePath);
                }
            }

            return new Uri(outputFilePath);
        }
    }
}
