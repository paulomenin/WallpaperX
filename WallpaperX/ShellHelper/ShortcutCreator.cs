using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MS.WindowsAPICodePack.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperX.ShellHelper
{
    class ShortcutCreator
    {
        public static bool TryCreateMenuShortcut(string appId, string appName)
        {
            return TryCreateShortcut("\\Microsoft\\Windows\\Start Menu\\Programs\\", appId, appName);
        }

        public static bool TryCreateStartupShortcut(string appId, string appName)
        {
            return TryCreateShortcut("\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\\", appId, appName);
        }

        static bool TryCreateShortcut(string location, string appId, string appName)
        {
            String shortcutPath = Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData) +
                location + appName + ".lnk";
            if (!File.Exists(shortcutPath))
            {
                InstallShortcut(appId, shortcutPath);
                return true;
            }

            return false;
        }

        static void InstallShortcut(string appId, string shortcutPath)
        {
            // Find the path to the current executable
            String exePath = Process.GetCurrentProcess().MainModule.FileName;
            IShellLinkW newShortcut = (IShellLinkW)new CShellLink();

            // Create a shortcut to the exe
            VerifySucceeded(newShortcut.SetPath(exePath));
            VerifySucceeded(newShortcut.SetArguments(""));

            // Open the shortcut property store, set the AppUserModelId property
            IPropertyStore newShortcutProperties = (IPropertyStore)newShortcut;

            using (PropVariant applicationId = new PropVariant(appId))
            {
                VerifySucceeded(newShortcutProperties.SetValue(
                    SystemProperties.System.AppUserModel.ID, applicationId));
                VerifySucceeded(newShortcutProperties.Commit());
            }

            // Commit the shortcut to disk
            IPersistFile newShortcutSave = (IPersistFile)newShortcut;

            VerifySucceeded(newShortcutSave.Save(shortcutPath, true));
        }

        static void VerifySucceeded(UInt32 hresult)
        {
            if (hresult <= 1)
                return;

            throw new Exception("Failed with HRESULT: " + hresult.ToString("X"));
        }
    }
}
