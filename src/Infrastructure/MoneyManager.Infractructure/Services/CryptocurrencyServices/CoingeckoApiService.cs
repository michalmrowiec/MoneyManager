using MoneyManager.Application.Contracts.Services;
using MoneyManager.Domain.Entities.CryptoAssets;

namespace MoneyManager.Infractructure.Services.CryptocurrencyServices
{
    internal class CoingeckoApiService : IAsyncCryptocurrencyService
    {
        public Task<List<CryptocurrencySimpleData>> GetSimplePriceForCryptocurrencies(string[] cryptocurrencies, params string[] currency)
        {
            throw new NotImplementedException();
        }
    }
}
