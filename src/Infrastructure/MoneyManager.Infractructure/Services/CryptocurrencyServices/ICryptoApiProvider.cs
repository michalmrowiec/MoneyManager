using MoneyManager.Application.Functions.CryptoAssets.Queries;
using MoneyManager.Domain.Entities.CryptoAssets;
using System.Net;

namespace MoneyManager.Infractructure.Services.CryptocurrencyServices
{
    public interface ICryptoApiProvider
    {
        Task<(HttpStatusCode Status, List<CryptocurrencySimpleData> Value)> GetCryptocurrenciesDataFromApi(int top, string currency);
    }
}