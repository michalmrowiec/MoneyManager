﻿namespace MoneyManager.Application.Functions.CryptoAssets.Queries
{
    public class CryptoAssetDto
    {
        public int Id { get; set; }
        public string CryptocurrencySymbol { get; set; }
        public string CryptocurrencyName { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal ActualPrice { get; set; }
        public decimal PricePercentChange24h { get; set; }
        public string SymbolOfSettlementCurrency { get; set; }
        public decimal MarketCap { get; set; }
        public DateTime DataForDateTime { get; set; }

        public int UserId { get; set; }
    }
}
