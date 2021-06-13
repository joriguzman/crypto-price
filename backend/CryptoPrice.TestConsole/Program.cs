using Binance.Net;
using Binance.Net.Objects.Spot;
using Bitfinex.Net;
using Bitfinex.Net.Objects;
using CryptoExchange.Net.Logging;
using CryptoPrice.Api;
using System;
using System.Collections.Generic;
using System.IO;

namespace CryptoPrice.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var binanceClient = new BinanceSocketClient(new BinanceSocketClientOptions()
            {
                LogVerbosity = LogVerbosity.Info,
                LogWriters = new List<TextWriter> { Console.Out }
            });

            var bitfinexClient = new BitfinexSocketClient(new BitfinexSocketClientOptions()
            {
                LogVerbosity = LogVerbosity.Info,
                LogWriters = new List<TextWriter> { Console.Out }
            });

            PriceDataProvider dataProvider = new PriceDataProvider(binanceClient, bitfinexClient);
            dataProvider.OnPriceUpdate = price =>
            {
                Console.WriteLine($"Key {price.Key}, last price {price.LastPrice}");
            };

            dataProvider.StartSubscriptions().Wait();

            Console.WriteLine("Streaming prices. Press Enter to quit");
            Console.ReadLine();

            dataProvider.StopSubscriptions().Wait();
        }
    }
}
