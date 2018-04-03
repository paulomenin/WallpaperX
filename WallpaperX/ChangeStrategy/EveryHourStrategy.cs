using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperX.PictureSource;

namespace WallpaperX.ChangeStrategy
{
    class EveryHourStrategy : ChangeStrategy
    {
        public EveryHourStrategy(IPictureSource pictureSource) : base(pictureSource)
        {
        }

        public override void RunLogic()
        {
            while (true)
            {
                Uri imagePath = PictureSource.GetPicture();

                Wallpaper.Set(imagePath);

                System.Threading.Thread.Sleep(1000 * 60 * 60); // Sleep for 1 hour
            }
        }
    }
}
