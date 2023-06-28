using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Application.Contracts.Services;
using MoneyManager.Application.Functions.CryptoAssets.Queries;
using MoneyManager.Domain.Entities.CryptoAssets;
using System.Net;

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

        public async Task<(ApiResponseStatus Status, List<CryptocurrencySimpleData> Value)> GetSimplePriceForCryptocurrencies(string[] cryptocurrencies, string currency)
        {
            List<CryptocurrencySimpleData> cryptocurrenciesWithData = new();
            List<string> cryptocurrenciesNeedsData = new();
            List<string> cryptocurrenciesNeedsRefreshData = new();

            foreach (var c in cryptocurrencies)
            {
                //why get single record and not get all needed???
                var cData = await _datasRepository.GetByNameAsync(c);

                if (cData == null)
                {
                    cryptocurrenciesNeedsData.Add(c);
                    continue;
                }
                else if (cData.UpdateDate.AddHours(1) < DateTime.Now)
                {
                    cryptocurrenciesNeedsRefreshData.Add(c);
                }

                cryptocurrenciesWithData.Add(new()
                {
                    Id = cData.Id,
                    Name = cData.Name,
                    Symbol = cData.Symbol,
                    Price = cData.Price,
                    PricePercentChange24h = Math.Round(cData.PricePercentChange24h, 2),
                    MarketCap = Math.Round(cData.MarketCap, 2),
                    UpdateDate = cData.UpdateDate,
                });
            }

            List<string> tmp = new();
            tmp.AddRange(cryptocurrenciesNeedsData);
            tmp.AddRange(cryptocurrenciesNeedsRefreshData);

            var cryptocurrenciesWitchDataNew = await _cryptoApiProvider.GetBasicCryptocurrenciesInfo(tmp.ToArray(), currency);

            if (cryptocurrenciesWitchDataNew.Status == HttpStatusCode.TooManyRequests)
                return (ApiResponseStatus.ApiOverloaded, cryptocurrenciesWithData);

            if (cryptocurrenciesWitchDataNew.Status != HttpStatusCode.OK)
                return (ApiResponseStatus.UnableConnectToApi, cryptocurrenciesWithData);

            List<CryptocurrencySimpleData> toAdd = new();
            List<CryptocurrencySimpleData> toUpdate = new();


            foreach (var c in cryptocurrenciesWitchDataNew.Value)
            {
                if (cryptocurrenciesNeedsData.Contains(c.Name))
                    toAdd.Add(c);
                else
                {
                    c.Id = cryptocurrenciesWithData.First(x => x.Symbol == c.Symbol).Id;
                    toUpdate.Add(c);
                }
            }

            await _datasRepository.AddRangeAsync(toAdd.ToArray());

            foreach (var c in toAdd)
            {
                cryptocurrenciesWithData.Add(new()
                {
                    Name = c.Name,
                    Symbol = c.Symbol,
                    Price = c.Price,
                    PricePercentChange24h = Math.Round(c.PricePercentChange24h, 2),
                    MarketCap = Math.Round(c.MarketCap, 2),
                    UpdateDate = c.UpdateDate,
                });
            }

            foreach (var c in toUpdate)
            {
                await _datasRepository.UpdateAsync(c);

                var existingCrypto = cryptocurrenciesWithData.First(x => x.Symbol == c.Symbol);

                existingCrypto.Name = c.Name;
                existingCrypto.Symbol = c.Symbol;
                existingCrypto.Price = c.Price;
                existingCrypto.PricePercentChange24h = Math.Round(c.PricePercentChange24h, 2);
                existingCrypto.MarketCap = Math.Round(c.MarketCap, 2);
                existingCrypto.UpdateDate = c.UpdateDate;
            }

            return (ApiResponseStatus.Ok, cryptocurrenciesWithData);
        }
    }
}
