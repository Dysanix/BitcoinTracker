using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinTracker
{
    internal class TickerValues
    {
        private readonly WebClient _webClient = new WebClient();
        public TickerValues()
        {
            _webClient.Proxy = null;
            _webClient.Encoding = Encoding.UTF8;
        }

        public void SetUserAgent()
        {
            _webClient.Headers.Add("Accept-Language", " en-US");
            _webClient.Headers.Add("Accept", " text/html, application/xhtml+xml, */*");
            _webClient.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");
        }
        
        public void CheckRepetition(string json)
        {
            if (json.Equals(DataStorage.LastJsonString))
            {
                DataStorage.JsonRepetitions += 1;
            }
            else
            {
                DataStorage.JsonRepetitions = 0;
            }
            DataStorage.LastJsonString = json;
        }

        public double GetBlockchainValue(int currency)
        {
            SetUserAgent();
            var json = _webClient.DownloadString("https://blockchain.info/nl/ticker");
            CheckRepetition(json);
            dynamic exchangeRatesComplete = JsonConvert.DeserializeObject(json);
            return currency == 0 ? exchangeRatesComplete["USD"]["last"] : exchangeRatesComplete["EUR"]["last"];
        }

        public double GetBitstampValue(int currency)
        {
            SetUserAgent();
            if (currency == 0)
            {
                var jsonusd = _webClient.DownloadString("https://www.bitstamp.net/api/v2/ticker/btcusd/");
                CheckRepetition(jsonusd);
                dynamic exchangeRatesCompleteUsd = JsonConvert.DeserializeObject(jsonusd);
                return exchangeRatesCompleteUsd["last"];
            }
            var jsoneur = _webClient.DownloadString("https://www.bitstamp.net/api/v2/ticker/btceur/");
            CheckRepetition(jsoneur);
            dynamic exchangeRatesCompleteEur = JsonConvert.DeserializeObject(jsoneur);
            return exchangeRatesCompleteEur["last"];
        }

        public double GetGDaxValue(int currency)
        {
            SetUserAgent();
            if (currency == 0)
            {
                var jsonusd = _webClient.DownloadString("https://api.gdax.com/products/BTC-USD/ticker");
                CheckRepetition(jsonusd);
                dynamic exchangeRatesCompleteUsd = JsonConvert.DeserializeObject(jsonusd);
                return exchangeRatesCompleteUsd["price"];
            }
            var jsoneur = _webClient.DownloadString("https://api.gdax.com/products/BTC-EUR/ticker");
            CheckRepetition(jsoneur);
            dynamic exchangeRatesCompleteEur = JsonConvert.DeserializeObject(jsoneur);
            return exchangeRatesCompleteEur["price"];
        }

        public double GetBtceValue(int currency)
        {
            SetUserAgent();
            if (currency == 0)
            {
                var jsonusd = _webClient.DownloadString("https://btc-e.com/api/3/ticker/btc_usd");
                CheckRepetition(jsonusd);
                dynamic exchangeRatesCompleteUsd = JsonConvert.DeserializeObject(jsonusd);
                return exchangeRatesCompleteUsd["btc_usd"]["last"];
            }
            var jsoneur = _webClient.DownloadString("https://btc-e.com/api/3/ticker/btc_eur");
            CheckRepetition(jsoneur);
            dynamic exchangeRatesCompleteEur = JsonConvert.DeserializeObject(jsoneur);
            return exchangeRatesCompleteEur["btc_eur"]["last"];
        }
    }
}
