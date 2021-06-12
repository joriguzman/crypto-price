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

        private readonly Dictionary<string, string> _binanceSymbols = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _bitfinexSymbols = new Dictionary<string, string>();

        public IPriceCallback Callback { get; set; }
        public bool IsSubscriptionStarted { get; private set; }

        public PriceDataProvider(IBinanceSocketClient binanceSocketClient, IBitfinexSocketClient bitfinexSocketClient)
        {
            _binanceSocketClient = binanceSocketClient;
            _bitfinexSocketClient = bitfinexSocketClient;

            SetSymbols();
        }

        private void SetSymbols()
        {
            _binanceSymbols["BTCUSD"] = "BTCUSDT";
            _binanceSymbols["ETHUSD"] = "ETHUSDT";
            _binanceSymbols["DOGEUSD"] = "DOGEUSDT";

            _bitfinexSymbols["BTCUSD"] = "tBTCUSD";
            _bitfinexSymbols["ETHUSD"] = "tETHUSD";
            _bitfinexSymbols["DOGEUSD"] = "tDOGE:USD";
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
            foreach (var pair in _bitfinexSymbols)
            {
                var symbol = pair.Key;
                var exchangeTicker = pair.Value;

                await _bitfinexSocketClient.SubscribeToTickerUpdatesAsync(exchangeTicker, data =>
                {
                    Price price = new Price()
                    {
                        Exchange = "Bitfinex",
                        Symbol = symbol,
                        LastPrice = data.LastPrice,
                        Volume = data.Volume,
                        PriceChangePercent = data.DailyChangePercentage
                    };

                    //Console.WriteLine($"Bitfinex: {price.Symbol}, {price.LastPrice}");
                    Callback?.OnPriceUpdate(price);
                });
            }
        }

        private async Task SubscribeToBinance()
        {
            await _binanceSocketClient.Spot.SubscribeToAllSymbolTickerUpdatesAsync(data =>
            {
                foreach (var tick in data.Where(t => _binanceSymbols.ContainsValue(t.Symbol)))
                {
                    var symbol = tick.Symbol;
                    Price price = new Price()
                    {
                        Exchange = "Binance",
                        Symbol = symbol,
                        LastPrice = tick.LastPrice,
                        Volume = tick.QuoteVolume,
                        PriceChangePercent = tick.PriceChangePercent / 100
                    };

                    //Console.WriteLine($"Binance: {price.Symbol}, {price.LastPrice}");
                    Callback?.OnPriceUpdate(price);
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