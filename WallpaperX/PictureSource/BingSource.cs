using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WallpaperX.PictureSource
{
    class ImageResult
    {
        public string Url { get; set; }
        public string UrlBase { get; set; }
        public string Copyright { get; set; }
    }

    class BingSource : IPictureSource
    {
        private string BingBaseUrl { get; set; }
        private string BingWallpaperDataUrl { get; set; }
        private string PictureSize { get; set; }
        private string Market { get; set; }

        private const string ALL_MARKETS = "all";
        private const string DEFAULT_MARKET = "en-ww";
        private List<string> BingMarkets = new List<string> {
            "en-ww",
            "de-de",
            "en-au",
            "en-ca",
            "en-gb",
            "en-in",
            "en-us",
            "fr-ca",
            "fr-fr",
            "ja-jp",
            "pt-br",
            "zh-cn",
        };
        private string currentMarket { get; set; }
        private int currentMarketIdx = 0;

        public BingSource()
        {
            var bingConfig = ConfigurationManager.GetSection("bingSettings") as NameValueCollection;
            BingBaseUrl = bingConfig["BaseUrl"];
            PictureSize = bingConfig["PictureSize"];
            Market = bingConfig["Market"];
        }

        private void SetMarket()
        {
            if (BingMarkets.Contains(Market))
            {
                currentMarket = Market;
            }
            else
            {
                if (string.Equals(currentMarket, ALL_MARKETS))
                {
                    currentMarket = BingMarkets[currentMarketIdx];
                    currentMarketIdx = (currentMarketIdx + 1) % BingMarkets.Count;
                }
                else
                {
                    currentMarket = DEFAULT_MARKET;
                }

            }

            BingWallpaperDataUrl = $"/HPImageArchive.aspx?format=js&idx=0&n=1&mkt={currentMarket}";
        }

        public PictureInfo GetPicture()
        {
            SetMarket();

            string pictureUrl = string.Empty;
            string metadataJson = String.Empty;

            // Download metadata
            using (var client = new WebClient())
            {
                metadataJson = client.DownloadString(BingBaseUrl + BingWallpaperDataUrl);
                byte[] bytes = Encoding.Default.GetBytes(metadataJson);
                metadataJson = Encoding.UTF8.GetString(bytes);
            }

            JObject metadataObject = JObject.Parse(metadataJson);
            List<JToken> imagesMetadata = metadataObject["images"].Children().ToList();

            if (imagesMetadata.Count > 0)
            {
                pictureUrl = string.Format("{0}{1}_{2}.jpg",
                    BingBaseUrl,
                    imagesMetadata[0].ToObject<ImageResult>().UrlBase,
                    PictureSize);
            }

            // Download Image
            PictureCache pictureCache = new PictureCache("bing");

            Uri pictureUri = null;
            try
            {
                pictureUri = pictureCache.DownloadImage(new Uri(pictureUrl));
            }
            catch
            {
                pictureUrl = string.Format("{0}{1}",
                    BingBaseUrl,
                    imagesMetadata[0].ToObject<ImageResult>().Url);

                pictureUri = pictureCache.DownloadImage(new Uri(pictureUrl));
            }

            PictureInfo info = new PictureInfo();
            info.PictureUrl = pictureUri;
            info.Description = imagesMetadata[0].ToObject<ImageResult>().Copyright;

            return info;
        }
    }
}
