using MoneyManager.Domain.Entities.Interfaces;

namespace MoneyManager.Domain.Entities.CryptoAssets
{
    public class CryptocurrencySimpleData : IIdentifier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal PricePercentChange24h { get; set; }
        public decimal MarketCap { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
