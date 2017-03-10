using MahApps.Metro.Controls;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace BitcoinTracker
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : MetroWindow
    {
        public Settings()
        {
            InitializeComponent();
            comboCurrency.Items.Add("EUR");
            comboCurrency.Items.Add("USD");
            comboCurrency.Items.Add("ISK");
            comboCurrency.Items.Add("HKD");
            comboCurrency.Items.Add("TWD");
            comboCurrency.Items.Add("CHF");
            comboCurrency.Items.Add("DKK");
            comboCurrency.Items.Add("CLP");
            comboCurrency.Items.Add("CAD");
            comboCurrency.Items.Add("CNY");
            comboCurrency.Items.Add("THB");
            comboCurrency.Items.Add("AUD");
            comboCurrency.Items.Add("SGD");
            comboCurrency.Items.Add("KRW");
            comboCurrency.Items.Add("JPY");
            comboCurrency.Items.Add("PLN");
            comboCurrency.Items.Add("GBP");
            comboCurrency.Items.Add("SEK");
            comboCurrency.Items.Add("NZD");
            comboCurrency.Items.Add("BRL");
            comboCurrency.Items.Add("RUB");
            comboCurrency.SelectedIndex = 0;
            sliderInterval.Value = Properties.Settings.Default.updateInterval;
            comboCurrency.SelectedItem = Properties.Settings.Default.currencyTag;
            txtBitcoins.Text = Properties.Settings.Default.currentBitcoins.ToString();
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
                Properties.Settings.Default.updateInterval = (int)sliderInterval.Value;
                Properties.Settings.Default.currencyTag = comboCurrency.SelectedItem.ToString();
                Properties.Settings.Default.currentBitcoins = d;
                Properties.Settings.Default.Save();
                this.Close();
            }
            else
            {
                MessageBox.Show("That's not a valid Bitcoin amount!");
            }
        }
    }
}
