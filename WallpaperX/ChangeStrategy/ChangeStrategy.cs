using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperX.PictureSource;

namespace WallpaperX.ChangeStrategy
{
    internal abstract class ChangeStrategy
    {
        protected IPictureSource PictureSource {  get;  set; }

        public ChangeStrategy(IPictureSource pictureSource)
        {
            PictureSource = pictureSource;
        }

        public abstract void RunLogic();
    }
}
