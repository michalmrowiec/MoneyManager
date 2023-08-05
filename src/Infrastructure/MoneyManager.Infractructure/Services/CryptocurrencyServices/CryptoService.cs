using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Application.Contracts.Services;
using MoneyManager.Application.Functions.CryptoAssets.Queries;
using MoneyManager.Domain.Entities.CryptoAssets;

namespace MoneyManager.Infractructure.Services.CryptocurrencyServices
{
    public class CryptoService : IAsyncCryptocurrencyService
    {
        private readonly ICryptoSimpleDatasRepository _datasRepository;
        private readonly ICryptoApiProvider _cryptoApiProvider;

        public CryptoService(ICryptoSimpleDatasRepository cryptoSimpleDatasRepository, ICryptoApiProvider cryptoApiProvider)
        {
            _datasRepository = cryptoSimpleDatasRepository;
            _cryptoApiProvider = cryptoApiProvider;
        }

        public async Task<Dictionary<string, string>> GetCryptocurrencySymbolsAndNames()
        {
            return await _datasRepository.GetSymbolsAndNames();
        }

        public async Task<(ApiResponseStatus Status, List<CryptocurrencySimpleData> Value)> GetSimplePriceForCryptocurrencies(string[] cryptocurrencies, string currency)
        {
            List<CryptocurrencySimpleData> result = new();
            List<CryptocurrencySimpleData> toAdd = new();
            List<CryptocurrencySimpleData> toUpdate = new();

            var datas = await _datasRepository.GetByNamesAsync(cryptocurrencies);

            if (datas.Count == 0 || (datas.Any(c => c.UpdateDate.AddHours(1) < DateTime.Now)))
            {
                var (status, cryptos) = await _cryptoApiProvider.GetCryptocurrenciesDataFromApi(200, currency);

                if (cryptos.Count != 0)
                {
                    await _datasRepository.DeleteAllAsync();
                    await _datasRepository.AddRangeAsync(cryptos.ToArray());
                }
            }

            datas = await _datasRepository.GetByNamesAsync(cryptocurrencies);

            return (ApiResponseStatus.Ok, await _datasRepository.GetByNamesAsync(cryptocurrencies));
        }
    }
}
