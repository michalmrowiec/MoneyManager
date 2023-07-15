namespace MoneyManager.Client.Models.ViewModels
{
    public class CreateCryptoAsset
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
