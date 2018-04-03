using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperX.PictureSource
{
    class PictureSourceFactory
    {
        public static IPictureSource Get(string sourceName)
        {
            switch (sourceName)
            {
                case "bing":
                    return new BingSource();
                default:
                    throw new ArgumentException("Source Name not recognized.");
            }
        }
    }
}
