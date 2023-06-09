using MediatR;
using MoneyManager.Domain.Entities;

namespace MoneyManager.Application.Functions.CryptoAssets.Commands.CreateCryptoAsset
{
    public record CreateCryptoAssetCommand : IRequest<CreateCryptoAssetCommandResponse>
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
    }
}
