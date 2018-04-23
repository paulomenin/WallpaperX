using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WallpaperX.ChangeStrategy;
using WallpaperX.PictureSource;
using WallpaperX.ShellHelper;

namespace WallpaperX
{
    class Program
    {
        public static string APP_ID = "WallpaperX.App";

        static int Main(string[] args)
        {
            try
            {
                if (string.Equals("True", ConfigurationManager.AppSettings["ShowToast"]))
                {
                    ShortcutCreator.TryCreateMenuShortcut(APP_ID, "WallpaperX");
                }

                if (string.Equals("True", ConfigurationManager.AppSettings["StartWithWindows"]))
                {
                    ShortcutCreator.TryCreateStartupShortcut(APP_ID, "WallpaperX");
                }
            }
            catch
            {
                Console.WriteLine("Cannot install Start Menu shortcut.");
                return 1;
            }

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
