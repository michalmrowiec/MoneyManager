using MoneyManager.Client.Models.ViewModels.Interfaces;

namespace MoneyManager.Application.Functions.CryptoAssets.Queries
{
    public class CryptoAssetVM : IId
    {
        public int Id { get; set; }
        public string CryptocurrencySymbol { get; set; }
        public string CryptocurrencyName { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal ActualPrice { get; set; }
        public decimal PricePercentChange24h { get; set; }
        public decimal MarketCap { get; set; }
        public DateTime DataForDateTime { get; set; }
    }
}
