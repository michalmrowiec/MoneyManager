using MediatR;

namespace MoneyManager.Application.Functions.CryptoAssets.Queries.GetAllCryptoAssets
{
    public record GetAllCryptoAssetsQuery(int UserId) : IRequest<List<CryptoAssetDto>>;
}
