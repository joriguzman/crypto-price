namespace CryptoPrice.Api
{
    public class Price
    {
        public string Exchange { get; set; }
        public string Symbol { get; set; }
        public decimal LastPrice { get; set; }
        public decimal PriceChangePercent { get; set; }
        public string Key => $"{Exchange}:{Symbol}";
    }
}
