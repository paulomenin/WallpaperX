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
        public static void ShowToast(string appId, string message, string imagePath = null)
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText01);

            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
            stringElements[0].AppendChild(toastXml.CreateTextNode(message));

            if (! string.IsNullOrWhiteSpace(imagePath))
            {
                imagePath = "file:///" + imagePath;
                XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
                imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;
            }

            ToastNotification toast = new ToastNotification(toastXml);

            ToastNotificationManager.CreateToastNotifier(appId).Show(toast);
        }
    }
}
