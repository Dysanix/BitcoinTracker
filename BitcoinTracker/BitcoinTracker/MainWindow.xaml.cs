using MahApps.Metro.Controls;
using Newtonsoft.Json;
using System;
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
            catch { }
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
