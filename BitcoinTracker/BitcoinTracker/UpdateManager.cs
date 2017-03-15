using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BitcoinTracker
{
    class UpdateManager
    {
        Timer _tmrRefresh = null;
        readonly WebClient _webClient = new WebClient();
        private static dynamic _updateObject;
        public event EventHandler NewUpdate;

        public UpdateManager()
        {
            if (_tmrRefresh == null)
            {
                _tmrRefresh = new Timer();
                _tmrRefresh.Interval = 3600000;
                _tmrRefresh.Elapsed += (sender, args) => { CheckForUpdates(); };
                _tmrRefresh.Start();
            }
            NewUpdate += (sender, args) => { };
            CheckForUpdates();
        }

        public void CheckForUpdates()
        {
            try
            {
                _webClient.Proxy = null;
                _updateObject =
                    JsonConvert.DeserializeObject(
                        _webClient.DownloadString("https://raw.githubusercontent.com/Dysanix/BitcoinTracker/master/BitcoinTracker/version.txt"));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            if (!GetLastestVersion().Equals(DataStorage.BtVersion))
            {
                NewUpdate(this, new EventArgs());
            }
        }

        public static string[] ToArray(JArray jarray)
        {
            return JsonConvert.DeserializeObject<string[]>(JsonConvert.SerializeObject(jarray));
        }

        public string GetLastestVersion()
        {
            return _updateObject["version"];
        }

        public string[] GetChangelog()
        {
            return ToArray(_updateObject["changelog"]);
        }

        public string[] GetDonations()
        {
            return ToArray(_updateObject["donations"]);
        }

        public string GetDownload()
        {
            return _updateObject["download"];
        }

        public string GetScan()
        {
            return _updateObject["scan"];
        }

        public string GetMessage()
        {
            return _updateObject["message"];
        }
    }
}
