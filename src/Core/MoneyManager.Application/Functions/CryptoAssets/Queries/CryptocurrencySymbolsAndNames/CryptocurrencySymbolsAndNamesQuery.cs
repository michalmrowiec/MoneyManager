using MediatR;

namespace MoneyManager.Application.Functions.CryptoAssets.Queries.CryptocurrencySymbolsAndNames
{
    public record class CryptocurrencySymbolsAndNamesQuery() : IRequest<Dictionary<string, string>>;
}
