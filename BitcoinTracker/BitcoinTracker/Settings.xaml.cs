using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace BitcoinTracker
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : MetroWindow
    {
        List<string> APIs = new List<string> { "Blockchain", "Bitstamp", "GDax" };
        List<string> CurrencyList = new List<string> { "USD", "EUR" };

        public Settings()
        {
            InitializeComponent();
            foreach (string API in APIs) { comboAPI.Items.Add(API); }
            foreach (string Currency in CurrencyList){comboCurrency.Items.Add(Currency);}
            comboAPI.SelectedItem = Properties.Settings.Default.API;
            sliderInterval.Value = Properties.Settings.Default.updateInterval;
            comboCurrency.SelectedItem = Properties.Settings.Default.currencyTag;
            txtBitcoins.Text = Properties.Settings.Default.currentBitcoins.ToString();
            chckTopMost.IsChecked = Properties.Settings.Default.inForeground;
            chckTaskbar.IsChecked = Properties.Settings.Default.inTaskbar;
            chckPulseIcon.IsChecked = Properties.Settings.Default.showPulseIcon;
        }

        private void sliderInterval_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblInterval.Content = (int)sliderInterval.Value + " ms";
        }

        private void txtBitcoins_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            int count = 0;
            foreach (char c in txtBitcoins.Text)
                if (c == '.') count++;
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
            double d = 0;
            bool isDouble = Double.TryParse(txtBitcoins.Text, out d);
            if (isDouble)
            {
                Properties.Settings.Default.API = comboAPI.SelectedItem.ToString();
                Properties.Settings.Default.updateInterval = (int)sliderInterval.Value;
                Properties.Settings.Default.currencyTag = comboCurrency.SelectedItem.ToString();
                Properties.Settings.Default.currentBitcoins = d;
                Properties.Settings.Default.inForeground = chckTopMost.IsChecked.Value;
                Properties.Settings.Default.showPulseIcon = chckPulseIcon.IsChecked.Value;
                Properties.Settings.Default.inTaskbar = chckTaskbar.IsChecked.Value;
                Properties.Settings.Default.Save();
                MainWindow.Instance.ShowInTaskbar = chckTaskbar.IsChecked.Value;
                MainWindow.Instance.Topmost = chckTopMost.IsChecked.Value;
                this.Close();
            }
            else
            {
                MessageBox.Show("That's not a valid Bitcoin amount!");
            }
        }
    }
}
