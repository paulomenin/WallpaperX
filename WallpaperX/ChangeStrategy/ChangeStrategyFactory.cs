using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperX.PictureSource;

namespace WallpaperX.ChangeStrategy
{
    class ChangeStrategyFactory
    {
        public static ChangeStrategy Get(string strategyType, IPictureSource pictureSource)
        {
            switch (strategyType)
            {
                case "OneTime":
                    return new OneTimeStrategy(pictureSource);
                case "EveryHour":
                    return new EveryHourStrategy(pictureSource);
                default:
                    return new OneTimeStrategy(pictureSource);
            }
        }
    }
}
