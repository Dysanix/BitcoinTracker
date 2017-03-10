using MahApps.Metro.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace BitcoinTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        System.Timers.Timer tmrRefresh = new System.Timers.Timer();
        WebClient webClient = new WebClient();
        public static MainWindow Instance;
        double lastWallet = 0;

        BitmapImage imgBluedot = LoadBitmapFromResource("/img/bluedot.png");
        BitmapImage imgGreendot = LoadBitmapFromResource("/img/greendot.png");
        BitmapImage imgReddot = LoadBitmapFromResource("/img/reddot.png");

        public MainWindow()
        {
            Instance = this;
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\BitcoinTracker.exe.config"))
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\BitcoinTracker.exe.config", Properties.Resources.configfile);
            }
            InitializeComponent();
            webClient.Proxy = null;
            webClient.Encoding = Encoding.UTF8;
            UpdateWallet();
            ShowInTaskbar = Properties.Settings.Default.inTaskbar;
            Topmost = Properties.Settings.Default.inForeground;
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
            try
            {
                webClient.Headers.Add("Accept-Language", " en-US");
                webClient.Headers.Add("Accept", " text/html, application/xhtml+xml, */*");
                webClient.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");
                double value = 0;
                int currency = 0;
                string symbol = "$";
                if (Properties.Settings.Default.currencyTag.Equals("EUR"))
                {
                    currency = 1;
                    symbol = "€";
                }
                switch (Properties.Settings.Default.API)
                {
                    case "Blockchain":
                        value = getBlockchainValue(currency);
                        break;
                    case "Bitstamp":
                        value = getBitstampValue(currency);
                        break;
                    case "GDax":
                        value = getGDaxValue(currency);
                        break;

                }
                double currentWallet = value * Properties.Settings.Default.currentBitcoins;
                if ((currentWallet % 1) == 0)
                {
                    lblWalletEUR.Invoke(new Action(() => lblWalletEUR.Content = symbol + currentWallet));
                }
                else
                {
                    lblWalletEUR.Invoke(new Action(() => lblWalletEUR.Content = symbol + String.Format("{0:0.00}", currentWallet)));
                }
                if (Properties.Settings.Default.showPulseIcon)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        imgPulse.Source = imgBluedot;
                        if (lastWallet > currentWallet)
                        {
                            imgPulse.Source = imgReddot;
                        }
                        else if (lastWallet < currentWallet)
                        {
                            imgPulse.Source = imgGreendot;
                        }
                        imgPulse.Visibility = Visibility.Visible;
                        DoubleAnimation dAnimation = new DoubleAnimation() { From = 0, To = 1, AutoReverse = true, Duration = new Duration(TimeSpan.FromSeconds(0.3)) };
                        imgPulse.BeginAnimation(OpacityProperty, dAnimation);
                    });
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        imgPulse.Visibility = Visibility.Hidden;
                    });
                }
                lastWallet = currentWallet;
            }
            catch
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    imgPulse.Source = imgReddot;
                    imgPulse.Visibility = Visibility.Visible;
                    DoubleAnimation dAnimation = new DoubleAnimation() { From = 0, To = 1, AutoReverse = true, Duration = new Duration(TimeSpan.FromSeconds(0.7)) };
                    imgPulse.BeginAnimation(OpacityProperty, dAnimation);
                });
            }
        }

        public double getBlockchainValue(int currency)
        {
            string JSON = webClient.DownloadString("https://blockchain.info/nl/ticker");
            dynamic exchangeRatesComplete = JsonConvert.DeserializeObject(JSON);
            if (currency == 0)
            {
                return exchangeRatesComplete["USD"]["last"];
            }
            return exchangeRatesComplete["EUR"]["last"];
        }

        public double getBitstampValue(int currency)
        {
            if (currency == 0)
            {
                string JSONUSD = webClient.DownloadString("https://www.bitstamp.net/api/v2/ticker/btcusd/");
                dynamic exchangeRatesCompleteUSD = JsonConvert.DeserializeObject(JSONUSD);
                return exchangeRatesCompleteUSD["last"];
            }
            string JSONEUR = webClient.DownloadString("https://www.bitstamp.net/api/v2/ticker/btceur/");
            dynamic exchangeRatesCompleteEUR = JsonConvert.DeserializeObject(JSONEUR);
            return exchangeRatesCompleteEUR["last"];
        }

        public double getGDaxValue(int currency)
        {
            if (currency == 0)
            {
                string JSONUSD = webClient.DownloadString("https://api.gdax.com/products/BTC-USD/ticker");
                dynamic exchangeRatesCompleteUSD = JsonConvert.DeserializeObject(JSONUSD);
                return exchangeRatesCompleteUSD["price"];
            }
            string JSONEUR = webClient.DownloadString("https://api.gdax.com/products/BTC-EUR/ticker");
            dynamic exchangeRatesCompleteEUR = JsonConvert.DeserializeObject(JSONEUR);
            return exchangeRatesCompleteEUR["price"];
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

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            new Settings().ShowDialog();
            tmrRefresh.Interval = Properties.Settings.Default.updateInterval;
            UpdateWallet();
        }
    }
}
