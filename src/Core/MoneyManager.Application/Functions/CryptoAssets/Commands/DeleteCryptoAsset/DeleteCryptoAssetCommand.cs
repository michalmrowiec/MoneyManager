using MediatR;

namespace MoneyManager.Application.Functions.CryptoAssets.Commands.DeleteCryptoAsset
{
    public record DeleteCryptoAssetCommand(int UserId, int Id) : IRequest;
}
