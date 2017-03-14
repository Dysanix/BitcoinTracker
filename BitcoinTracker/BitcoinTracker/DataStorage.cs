using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinTracker
{
    class DataStorage
    {
        public static string BtVersion = "1.4.2";
        public static double LastWallet = 0;
        public static string LastJsonString = "";
        public static int JsonRepetitions = 0;
        public static string LastApi = "";
        public static int LastApiPulseRepetitions = 0;
        public static bool NotificationUpdateShown = false;
        public static SoundPlayer NotificationSound = new SoundPlayer(Properties.Resources.notification);
        public static UpdateManager UpdateManager = new UpdateManager();
    }
}
