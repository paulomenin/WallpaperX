using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WallpaperX.ChangeStrategy;
using WallpaperX.PictureSource;

namespace WallpaperX
{
    class Program
    {
        static int Main(string[] args)
        {
            IPictureSource pictureSource = null;
            try
            {
                pictureSource = PictureSourceFactory.Get(ConfigurationManager.AppSettings["PictureSource"]);
            }
            catch
            {
                Console.WriteLine("PictureSource not configured properly.");
                return 1;
            }

            ChangeStrategy.ChangeStrategy changeStrategy = null;
            try
            {
                changeStrategy = ChangeStrategyFactory.Get(ConfigurationManager.AppSettings["ChangeStrategy"], pictureSource);
            }
            catch
            {
                Console.WriteLine("ChangeStrategy not configured properly.");
                return 1;
            }
            
            Thread strategyThread = new Thread(changeStrategy.RunLogic);
            strategyThread.Start();
            strategyThread.Join();

            return 0;
        }
    }
}
