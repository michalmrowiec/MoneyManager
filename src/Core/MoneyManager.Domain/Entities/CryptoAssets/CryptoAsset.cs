using MoneyManager.Domain.Entities.Interfaces;

namespace MoneyManager.Domain.Entities.CryptoAssets
{
    public class CryptoAsset : IIdentifier
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
    }
}
