using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Notifications.Wpf;

namespace BitcoinTracker
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : MetroWindow
    {
        private readonly List<string> _apIs = new List<string> { "BTCe", "GDax", "Bitstamp", "Blockchain" };
        private readonly List<string> _currencyList = new List<string> { "USD", "EUR" };

        private readonly BitmapImage _imgMute = Utils.LoadBitmapFromResource("/img/mute.png");
        private readonly BitmapImage _imgUnmute = Utils.LoadBitmapFromResource("/img/unmute.png");

        private readonly UpdateManager _updateManager = DataStorage.UpdateManager;

        public static Settings Instance = null;

        public Settings(List<double> point,int tabIndex = 0)
        {
            Instance = this;
            InitializeComponent();

            Left = point[0] - point[2];
            Top = point[1] + point[3];

            foreach (var api in _apIs) { comboAPI.Items.Add(api); }
            foreach (var currency in _currencyList){comboCurrency.Items.Add(currency);}
            comboAPI.SelectedItem = Properties.Settings.Default.API;
            sliderInterval.Value = Properties.Settings.Default.updateInterval;
            comboCurrency.SelectedItem = Properties.Settings.Default.currencyTag;
            txtBitcoins.Text = Properties.Settings.Default.currentBitcoins.ToString();
            chckTopMost.IsChecked = Properties.Settings.Default.inForeground;
            chckTaskbar.IsChecked = Properties.Settings.Default.inTaskbar;

            doubleHigh.Text = Properties.Settings.Default.alertHigh.ToString();
            doubleLow.Text = Properties.Settings.Default.alertLow.ToString();

            if (Properties.Settings.Default.muted) { btnMuteImage.Source=_imgUnmute; }

            SettingsTabControl.SelectedIndex = tabIndex;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Instance = null;
            base.OnClosing(e);
        }

        public void SetCreditText()
        {
            lblCredits.Inlines.Clear();
            lblCredits.FontSize = 14;
            lblCredits.Inlines.Add(new Bold(new Run("BitcoinTracker " + DataStorage.BtVersion + "\n")));
            lblCredits.Inlines.Add("Created by Dysanix\n\n");
            lblCredits.Inlines.Add(new Bold(new Run("♡ Donations ♡\n")));
            foreach (string donator in _updateManager.GetDonations())
            {
                lblCredits.Inlines.Add(new Italic(new Run(donator + "\n")));
            }
            lblCredits.Inlines.Add(new Bold(new Run("\nDonate BTC\n")));
            lblCredits.Inlines.Add(" 13NJUqZyb5rDPCj5zJnGhCFxQGy3JhdwNe\n\n");
        }

        private void sliderInterval_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblInterval.Content = (double)sliderInterval.Value/1000 + " sec";
        }

        private void txtBitcoins_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9.]+");
            var count = txtBitcoins.Text.Count(c => c == '.');
            if (count <= 1 && regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            double d;
            var isDouble = double.TryParse(txtBitcoins.Text, out d);
            if (isDouble)
            {
                Properties.Settings.Default.API = comboAPI.SelectedItem.ToString();
                Properties.Settings.Default.updateInterval = (int)sliderInterval.Value;
                Properties.Settings.Default.currencyTag = comboCurrency.SelectedItem.ToString();
                Properties.Settings.Default.currentBitcoins = d;
                Properties.Settings.Default.inForeground = chckTopMost.IsChecked.Value;
                Properties.Settings.Default.inTaskbar = chckTaskbar.IsChecked.Value;
                Properties.Settings.Default.Save();
                WalletWidget.Instance.ShowInTaskbar = chckTaskbar.IsChecked.Value;
                WalletWidget.Instance.Topmost = chckTopMost.IsChecked.Value;
                Close();
            }
            else
            {
                MessageBox.Show("That's not a valid Bitcoin amount!","BitcoinTracker",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            }
        }

        private void SettingsTabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            chckCheckForUpdates.IsChecked = Properties.Settings.Default.checkForUpdates;
            if (SettingsTabControl.SelectedIndex == 2)
            {
                if (!_updateManager.GetLastestVersion().Equals(DataStorage.BtVersion))
                {
                    lblUpdateStatus.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    lblUpdateStatus.FontSize = 16;
                    lblUpdateStatus.Content = "New update available! (" + _updateManager.GetLastestVersion() + ")";
                    stckUpdateLog.Children.Clear();
                    foreach (var change in _updateManager.GetChangelog())
                    {
                        stckUpdateLog.Children.Add(new CheckBox()
                        {
                            Content = change,
                            IsChecked = true,
                            IsEnabled = false
                        });
                        stckUpdateLog.Children.Add(new Label() { Height = 2 });
                    }
                    btnUpdateDownload.Click += (o, args) => { Process.Start(_updateManager.GetDownload()); };
                    btnUpdateVirusScan.Click += (o, args) => { Process.Start(_updateManager.GetScan()); };
                    btnUpdateDownload.IsEnabled = true;
                    btnUpdateVirusScan.IsEnabled = true;
                    if (!_updateManager.GetMessage().Equals(""))
                    {
                        MessageBox.Show(_updateManager.GetMessage(), "BitcoinTracker", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                }
                else
                {
                    stckUpdateLog.Children.Clear();
                    foreach (var change in _updateManager.GetChangelog())
                    {
                        stckUpdateLog.Children.Add(new CheckBox()
                        {
                            Content = change,
                            IsChecked = true,
                            IsEnabled = false
                        });
                        stckUpdateLog.Children.Add(new Label() { Height = 2 });
                        btnUpdateVirusScan.Click += (o, args) => { Process.Start(_updateManager.GetScan()); };
                        btnUpdateVirusScan.IsEnabled = true;
                    }
                }
            }
            else if (SettingsTabControl.SelectedIndex == 3)
            {
                SetCreditText();
            }
            else if (SettingsTabControl.SelectedIndex == 1)
            {
                var symbol = "$";
                var currencytag = Properties.Settings.Default.currencyTag;
                if (currencytag.Equals("EUR")) { symbol = "€"; }
                lblHigherThan.Content = "Higher than (" + symbol + "):";
                lblLowerThan.Content = "Lower than (" + symbol + "):";
            }
        }

        private void ChckCheckForUpdates_OnClick(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.checkForUpdates = chckCheckForUpdates.IsChecked.Value;
            Properties.Settings.Default.Save();
        }
        
        private void BtnMute_OnClick(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.muted)
            {
                Properties.Settings.Default.muted = false;
                Properties.Settings.Default.Save();
                btnMuteImage.Source = _imgMute;
            }
            else
            {
                Properties.Settings.Default.muted = true;
                Properties.Settings.Default.Save();
                btnMuteImage.Source = _imgUnmute;
            }
        }

        private int SaveAlerts()
        {

            double alertHigh;
            double alertLow;
            if (double.TryParse(doubleHigh.Text, out alertHigh) && double.TryParse(doubleLow.Text, out alertLow))
            {
                if (alertHigh != 0 && alertHigh < DataStorage.LastWallet)
                {
                    MessageBox.Show("High value has to be higher than your current wallet value!", "BitcoinTracker", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return 1;
                }
                if (alertLow != 0 && alertLow > DataStorage.LastWallet)
                {
                    MessageBox.Show("Low value has to be lower than your current wallet value!", "BitcoinTracker", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return 1;
                }
                if (alertHigh < 0 || alertLow < 0)
                {
                    MessageBox.Show("Value cannot be negative!", "BitcoinTracker", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return 1;
                }
                Properties.Settings.Default.alertHigh = double.Parse(doubleHigh.Text);
                Properties.Settings.Default.alertLow = double.Parse(doubleLow.Text);
                Properties.Settings.Default.Save();
                Utils.ShowNotification("Alert has been set!",NotificationType.Information,TimeSpan.FromSeconds(5));
                return 0;
            }
            else
            {
                MessageBox.Show("That's not a valid value!", "BitcoinTracker", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return 1;
            }
        }

        private string _tempPrevious = "";

        private void DoubleHighLow_OnLostFocus(object sender, RoutedEventArgs e)
        {
            DoubleTextBox doubleTb = (DoubleTextBox) sender;
            if (!doubleTb.Text.Equals(_tempPrevious))
            {
                int i = SaveAlerts();
                if (i == 1)
                    doubleTb.Text = _tempPrevious;
            }
        }

        private void DoubleHighLow_OnGotFocus(object sender, RoutedEventArgs e)
        {
            DoubleTextBox doubleTb = (DoubleTextBox)sender;
            _tempPrevious = doubleTb.Text;
        }
    }
}
