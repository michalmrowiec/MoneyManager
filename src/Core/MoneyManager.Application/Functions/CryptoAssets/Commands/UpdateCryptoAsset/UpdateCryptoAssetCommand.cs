using MediatR;

namespace MoneyManager.Application.Functions.CryptoAssets.Commands.UpdateCryptoAsset
{
    public record UpdateCryptoAssetCommand : IRequest
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
    }
}
