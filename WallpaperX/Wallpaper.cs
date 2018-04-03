using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace WallpaperX
{
    class Wallpaper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);

        private const UInt32 SPI_SETDESKWALLPAPER = 20;
        private const UInt32 SPIF_UPDATEINIFILE = 0x01;
        private const UInt32 SPIF_SENDWININICHANGE = 0x02;

        public enum Style
        {
            Fill,
            Fit,
            Span,
            Stretch,
            Tile,
            Center
        }

        public static void Set(Uri filename, Style? style = null)
        {
            if (! style.HasValue)
            {
                try
                {
                    string wallpaperStyle = ConfigurationManager.AppSettings["WallpaperStyle"];
                    style = (Style)Enum.Parse(typeof(Style), wallpaperStyle);
                }
                catch (ArgumentException)
                {
                    style = Style.Fill;
                }
            }

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

            switch (style.Value)
            {
                case Style.Fill:
                    key.SetValue(@"WallpaperStyle", "10");
                    key.SetValue(@"TileWallpaper", "0");
                    break;
                case Style.Fit:
                    key.SetValue(@"WallpaperStyle", "6");
                    key.SetValue(@"TileWallpaper", "0");
                    break;
                case Style.Span:
                    key.SetValue(@"WallpaperStyle", "22");
                    key.SetValue(@"TileWallpaper", "0");
                    break;
                case Style.Stretch:
                    key.SetValue(@"WallpaperStyle", "2");
                    key.SetValue(@"TileWallpaper", "0");
                    break;
                case Style.Tile:
                    key.SetValue(@"WallpaperStyle", "0");
                    key.SetValue(@"TileWallpaper", "1");
                    break;
                case Style.Center:
                    key.SetValue(@"WallpaperStyle", "0");
                    key.SetValue(@"TileWallpaper", "0");
                    break;
                default:
                    break;
            }

            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0,
                filename.AbsolutePath,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}
