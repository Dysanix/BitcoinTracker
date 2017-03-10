using MahApps.Metro.Controls;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Timers;
using System.Windows;

namespace BitcoinTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        System.Timers.Timer tmrRefresh = new System.Timers.Timer();
        WebClient webClient = new WebClient();

        public MainWindow()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\BitcoinTracker.exe.config"))
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\BitcoinTracker.exe.config", Properties.Resources.configfile);
            }
            InitializeComponent();
            webClient.Proxy = null;
            webClient.Encoding = Encoding.UTF8;
            UpdateWallet();
            tmrRefresh.Interval = Properties.Settings.Default.updateInterval;
            tmrRefresh.Elapsed += TmrRefresh_Elapsed;
            tmrRefresh.Start();
        }

        private void TmrRefresh_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateWallet();
        }

        private void UpdateWallet()
        {
            string JSON = webClient.DownloadString("https://blockchain.info/nl/ticker");
            dynamic exchangeRatesComplete = JsonConvert.DeserializeObject(JSON);
            dynamic exchangeRatesCurrency = exchangeRatesComplete[Properties.Settings.Default.currencyTag];
            double currentWallet = exchangeRatesCurrency["last"] * Properties.Settings.Default.currentBitcoins;
            if ((currentWallet % 1) == 0)
            {
                lblWalletEUR.Invoke(new Action(() => lblWalletEUR.Content = exchangeRatesCurrency["symbol"] + currentWallet));
            }
            else
            {
                lblWalletEUR.Invoke(new Action(() => lblWalletEUR.Content = exchangeRatesCurrency["symbol"] + String.Format("{0:0.00}", currentWallet)));
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            new Settings().ShowDialog();
            tmrRefresh.Interval = Properties.Settings.Default.updateInterval;
            UpdateWallet();
        }
    }
}
