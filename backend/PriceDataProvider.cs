using Binance.Net.Interfaces;
using Bitfinex.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoPrice.Api
{
    public class PriceDataProvider
    {
        private readonly IBinanceSocketClient _binanceSocketClient;
        private readonly IBitfinexSocketClient _bitfinexSocketClient;

        private readonly Dictionary<string, string> _binanceTickers = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _bitfinexTickers = new Dictionary<string, string>();

        public Action<Price> OnPriceUpdate { get; set; }
        public bool IsSubscriptionStarted { get; private set; }

        public PriceDataProvider(IBinanceSocketClient binanceSocketClient, IBitfinexSocketClient bitfinexSocketClient)
        {
            _binanceSocketClient = binanceSocketClient;
            _bitfinexSocketClient = bitfinexSocketClient;

            SetTickers();
        }

        private void SetTickers()
        {
            // Key: Exchange Ticker, Value: Symbol
            _binanceTickers["BTCUSDT"] = "BTCUSD";
            _binanceTickers["ETHUSDT"] = "ETHUSD";
            _binanceTickers["DOGEUSDT"] = "DOGEUSD";
            _binanceTickers["LTCUSDT"] = "LTCUSD";
            _binanceTickers["ADAUSDT"] = "ADAUSD";
            _binanceTickers["XRPUSDT"] = "XRPUSD";

            _bitfinexTickers["tBTCUSD"] = "BTCUSD";
            _bitfinexTickers["tETHUSD"] = "ETHUSD";
            _bitfinexTickers["tDOGE:USD"] = "DOGEUSD";
            _bitfinexTickers["tLTCUSD"] = "LTCUSD";
            _bitfinexTickers["tADAUSD"] = "ADAUSD";
            _bitfinexTickers["tDOTUST"] = "DOTUSD";
        }

        public async Task StartSubscriptions()
        {
            if (!IsSubscriptionStarted)
            {
                await SubscribeToBinance();
                await SubscribeToBitfinex();
                IsSubscriptionStarted = true;
            }
        }

        private async Task SubscribeToBitfinex()
        {
            foreach (var pair in _bitfinexTickers)
            {
                var exchangeTicker = pair.Key;
                var symbol = pair.Value;

                await _bitfinexSocketClient.SubscribeToTickerUpdatesAsync(exchangeTicker, data =>
                {
                    Price price = new Price()
                    {
                        Exchange = "Bitfinex",
                        Symbol = symbol,
                        LastPrice = data.LastPrice,
                        Volume = data.Volume * data.LastPrice, // Volume is base volume, change to alternate volume
                        PriceChangePercent = data.DailyChangePercentage
                    };

                    //Console.WriteLine($"Bitfinex: {price.Symbol}, {price.LastPrice}");
                    OnPriceUpdate?.Invoke(price);
                });
            }
        }

        private async Task SubscribeToBinance()
        {
            await _binanceSocketClient.Spot.SubscribeToAllSymbolTickerUpdatesAsync(data =>
            {
                foreach (var tick in data.Where(t => _binanceTickers.ContainsKey(t.Symbol)))
                {
                    var symbol = tick.Symbol;
                    Price price = new Price()
                    {
                        Exchange = "Binance",
                        Symbol = symbol,
                        LastPrice = tick.LastPrice,
                        Volume = tick.QuoteVolume, // USD volume
                        PriceChangePercent = tick.PriceChangePercent / 100
                    };

                    //Console.WriteLine($"Binance: {price.Symbol}, {price.LastPrice}");
                    OnPriceUpdate?.Invoke(price);
                }
            });
        }

        public async Task StopSubscriptions()
        {
            await _binanceSocketClient.UnsubscribeAll();
            await _bitfinexSocketClient.UnsubscribeAll();
            IsSubscriptionStarted = false;
        }
    }
}