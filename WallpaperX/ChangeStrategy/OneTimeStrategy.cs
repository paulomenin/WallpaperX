using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperX.PictureSource;

namespace WallpaperX.ChangeStrategy
{
    class OneTimeStrategy : ChangeStrategy
    {
        public OneTimeStrategy(IPictureSource pictureSource) : base(pictureSource)
        {
        }

        public override void RunLogic()
        {
            PictureInfo picture = PictureSource.GetPicture();

            Wallpaper.ChangeWallpaper(picture);
        }
    }
}
