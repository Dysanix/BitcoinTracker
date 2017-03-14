using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Notifications.Wpf;

namespace BitcoinTracker
{
    class Utils
    {
        public static void ShowNotification(string message, NotificationType type, TimeSpan timeSpan, Action onclick = null, string title = "BitcoinTracker")
        {
            var notificationManager = new NotificationManager();
            notificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = type
            }, onClick: onclick, expirationTime: timeSpan);
            if (!Properties.Settings.Default.muted)
            {
                DataStorage.NotificationSound.Play();
            }
        }

        public static DoubleAnimation PulseAnimation(double duration)
        {
            return new DoubleAnimation()
            {
                From = 0,
                To = 1,
                AutoReverse = true,
                Duration = new Duration(TimeSpan.FromSeconds(duration))
            };
        }

        public static BitmapImage LoadBitmapFromResource(string pathInApplication, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }

            if (pathInApplication[0] == '/')
            {
                pathInApplication = pathInApplication.Substring(1);
            }
            return new BitmapImage(new Uri(@"pack://application:,,,/" + assembly.GetName().Name + ";component/" + pathInApplication, UriKind.Absolute));
        }
    }
}
