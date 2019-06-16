using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace WallpaperX
{
    class Toast
    {
        public static void ShowToast(string appId, string message, string imagePath = null, string attribution = null)
        {
            string toastXmlString =
                $@"<toast>
                     <visual>
                       <binding template='ToastGeneric'>
                         <image placement='hero' src='{imagePath}'/>
                         <image placement='appLogoOverride' hint-crop='circle' src='{imagePath}'/>
                         <text>{message}</text>
                         <text>{attribution}</text>
                       </binding>
                     </visual>
                   </toast>";

            XmlDocument toastXml = new XmlDocument();
            toastXml.LoadXml(toastXmlString);    

            ToastNotification toast = new ToastNotification(toastXml);

            ToastNotificationManager.CreateToastNotifier(appId).Show(toast);
        }
    }
}
