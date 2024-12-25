using MediatR;

namespace MoneyManager.Application.Functions.CryptoAssets.Queries.CryptocurrencySymbolsAndNames
{
    public record class CryptocurrencySymbolsAndNamesQuery() : IRequest<List<CryptoSymbolName>>;
}
