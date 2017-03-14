using MahApps.Metro.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Notifications.Wpf;
using Application = System.Windows.Application;
using Color = System.Drawing.Color;
using Timer = System.Timers.Timer;

namespace BitcoinTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WalletWidget : MetroWindow
    {
        private readonly Timer _tmrUpdateWallet = new Timer();
        public static WalletWidget Instance;

        private readonly BitmapImage _imgBlueDot = Utils.LoadBitmapFromResource("/img/bluedot.png");
        private readonly BitmapImage _imgGreenDot = Utils.LoadBitmapFromResource("/img/greendot.png");
        private readonly BitmapImage _imgRedDot = Utils.LoadBitmapFromResource("/img/reddot.png");

        private readonly UpdateManager _updateManager = DataStorage.UpdateManager;

        public WalletWidget()
        {
            Instance = this;
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + System.AppDomain.CurrentDomain.FriendlyName + ".config"))
            {
                File.WriteAllText(
                    AppDomain.CurrentDomain.BaseDirectory + "\\" + System.AppDomain.CurrentDomain.FriendlyName +
                    ".config", Properties.Resources.configfile);
                System.Windows.Forms.MessageBox.Show(
                    "Configuration file has been created! BitcoinTracker has to restart now!", "BitcoinTracker",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Application.Current.Shutdown();
            }
            else
            {
                InitializeComponent();
                Initialization();
            }
        }

        private void Initialization()
        {
            UpdateWallet();
            ShowInTaskbar = Properties.Settings.Default.inTaskbar;
            Topmost = Properties.Settings.Default.inForeground;
            _tmrUpdateWallet.Interval = Properties.Settings.Default.updateInterval;
            _tmrUpdateWallet.Elapsed += (sender, args) => UpdateWallet();
            _tmrUpdateWallet.Start();
            _updateManager.NewUpdate += (sender, args) => CheckForUpdates();
            CheckForUpdates();
        }

        private void CheckForUpdates()
        {
            if (Properties.Settings.Default.checkForUpdates)
            {
                if (!DataStorage.NotificationUpdateShown)
                {
                    if (!DataStorage.BtVersion.Equals(_updateManager.GetLastestVersion()))
                    {
                        DataStorage.NotificationUpdateShown = true;
                        var message = "A new update is available! Click this notification for more information.";
                        Utils.ShowNotification(message, NotificationType.Information, TimeSpan.FromSeconds(30), () => new Settings(new List<double> { Left, Top, Width, Height }, 2).ShowDialog());
                    }
                }
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
            base.OnClosing(e);
        }

        /*private void ShowWidget()
        {
            WalletWidget.Instance.Show();
            WalletWidget.Instance.WindowState = WindowState.Normal;
            WalletWidget.Instance.Focus();
        }*/

        private void UpdateWallet()
        {
            try
            {
                double value = 0;
                double currentWallet;

                var currency = 0;
                var symbol = "$";

                var api = Properties.Settings.Default.API;
                var currencytag = Properties.Settings.Default.currencyTag;

                var alertHigh = Properties.Settings.Default.alertHigh;
                var alertLow = Properties.Settings.Default.alertLow;

                if (currencytag.Equals("EUR")) { currency = 1; symbol = "€"; }

                switch (api)
                {
                    case "GDax":
                        value = new TickerValues().GetGDaxValue(currency);
                        break;
                    case "BTCe":
                        value = new TickerValues().GetBtceValue(currency);
                        break;
                    case "Blockchain":
                        value = new TickerValues().GetBlockchainValue(currency);
                        break;
                    case "Bitstamp":
                        value = new TickerValues().GetBitstampValue(currency);
                        break;
                }

                if (!DataStorage.LastApi.Equals(api))
                {
                    DataStorage.LastApi = api;
                    DataStorage.LastApiPulseRepetitions = 0;
                }

                currentWallet = value * Properties.Settings.Default.currentBitcoins;

                if (currentWallet > alertHigh && alertHigh != 0)
                {
                    var message = "Your wallet value has reached the high value of " + symbol + alertHigh + " on " + DateTime.Now + "! Click here to re-configure your alerts.";
                    Utils.ShowNotification(message,NotificationType.Success,TimeSpan.FromDays(1), () =>
                    {
                        if (Settings.Instance == null)
                        {
                            new Settings(new List<double> { Left, Top, Width, Height }, 1).ShowDialog();
                        }
                    });
                    Properties.Settings.Default.alertHigh = 0;
                    Properties.Settings.Default.Save();
                }
                if (currentWallet < alertLow && alertLow != 0)
                {
                    var message = "Your wallet value has reached the low value of " + symbol + alertLow + " on " + DateTime.Now + "! Click here to re-configure your alerts.";
                    Utils.ShowNotification(message, NotificationType.Error, TimeSpan.FromDays(1), () =>
                    {
                        if (Settings.Instance == null)
                        {
                            new Settings(new List<double> { Left, Top, Width, Height }, 1).ShowDialog();
                        }
                    });
                    Properties.Settings.Default.alertLow = 0;
                    Properties.Settings.Default.Save();
                }


                if (Math.Abs(currentWallet % 1) < 0)
                {
                    lblWalletEUR.Invoke(new Action(() => lblWalletEUR.Content = symbol + currentWallet));
                }
                else
                {
                    lblWalletEUR.Invoke(currency == 0
                        ? new Action(() => lblWalletEUR.Content = symbol + $"{currentWallet:0.00}")
                        : new Action(() => lblWalletEUR.Content = symbol + $"{currentWallet:0.00}".Replace(".", ",")));
                }
                if (DataStorage.JsonRepetitions > 20)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        txtInfo.Foreground = (new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0)));
                        txtInfo.Text = "This API seems stuck";
                        imgPulse.Source = _imgRedDot;
                        imgPulse.Visibility = Visibility.Visible;
                        imgPulse.BeginAnimation(OpacityProperty, Utils.PulseAnimation(1));
                        txtInfo.BeginAnimation(OpacityProperty, Utils.PulseAnimation(1));
                    });
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        txtInfo.Text = api;
                        imgPulse.Source = _imgBlueDot;
                        txtInfo.Foreground = (new SolidColorBrush(System.Windows.Media.Color.FromRgb(29, 79, 127)));
                        if (DataStorage.LastApiPulseRepetitions != 0)
                        {
                            if (DataStorage.LastWallet > currentWallet)
                            {
                                imgPulse.Source = _imgRedDot;
                                txtInfo.Foreground =
                                    (new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0)));
                            }
                            else if (DataStorage.LastWallet < currentWallet)
                            {
                                imgPulse.Source = _imgGreenDot;
                                txtInfo.Foreground =
                                    (new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0)));
                            }
                        }
                        imgPulse.Visibility = Visibility.Visible;
                        imgPulse.BeginAnimation(OpacityProperty, Utils.PulseAnimation(0.3));
                        if (DataStorage.LastApiPulseRepetitions < 3)
                        {
                            txtInfo.BeginAnimation(OpacityProperty, Utils.PulseAnimation(0.3));
                            DataStorage.LastApiPulseRepetitions += 1;
                        }
                        DataStorage.LastWallet = double.Parse($"{currentWallet:0.00}");
                    });
                }
            }
            catch
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    txtInfo.Foreground = (new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0)));
                    txtInfo.Text = "This API seems down";
                    lblWalletEUR.Content = null;
                    imgPulse.Source = _imgRedDot;
                    imgPulse.Visibility = Visibility.Visible;
                    imgPulse.BeginAnimation(OpacityProperty, Utils.PulseAnimation(1));
                    txtInfo.BeginAnimation(OpacityProperty, Utils.PulseAnimation(1));
                });
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            new Settings(new List<double> { Left, Top, Width, Height }).ShowDialog();
            _tmrUpdateWallet.Interval = Properties.Settings.Default.updateInterval;
            UpdateWallet();
        }
    }
}
