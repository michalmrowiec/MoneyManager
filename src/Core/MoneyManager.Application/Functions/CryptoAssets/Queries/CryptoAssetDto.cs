namespace MoneyManager.Application.Functions.CryptoAssets.Queries
{
    public class CryptoAssetDto
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal ActualPrice { get; set; }
        public double PricePercentChange24h { get; set; }
        public int UserId { get; set; }
    }
}
