using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Application.Contracts.Services;
using MoneyManager.Domain.Entities.CryptoAssets;
using System.Net;
using System.Xml.Linq;

namespace MoneyManager.Infractructure.Services.CryptocurrencyServices
{
    public class CryptoService : IAsyncCryptocurrencyService
    {
        private readonly ICryptoSimpleDatasRepository _datasRepository;

        public CryptoService(ICryptoSimpleDatasRepository cryptoSimpleDatasRepository)
        {
            _datasRepository = cryptoSimpleDatasRepository;
        }

        public async Task<(HttpStatusCode Status, List<CryptocurrencySimpleData> Value)> GetSimplePriceForCryptocurrencies(string[] cryptocurrencies, string currency)
        {
            List<CryptocurrencySimpleData> cryptocurrenciesWitchData = new();
            List<string> cryptocurrenciesNeedsData = new();
            List<string> cryptocurrenciesNeedsRefreshData = new();

            foreach (var c in cryptocurrencies)
            {
                var cData = await _datasRepository.GetBySymbolAsync(c);

                if (cData == null)
                {
                    cryptocurrenciesNeedsData.Add(c);
                    continue;
                }
                else if (cData.UpdateDate.AddHours(1) < DateTime.Now)
                {
                    cryptocurrenciesNeedsRefreshData.Add(c);
                }

                cryptocurrenciesWitchData.Add(new()
                {
                    Name = cData.Name,
                    Symbol = cData.Symbol,
                    Price = cData.Price,
                    PricePercentChange24h = Math.Round(cData.PricePercentChange24h, 2),
                    MarketCap = Math.Round(cData.MarketCap, 2),
                    UpdateDate = cData.UpdateDate,
                });
            }

            CoingeckoApiService cgs = new();

            cryptocurrenciesNeedsData.AddRange(cryptocurrenciesNeedsRefreshData);

            var cryptocurrenciesWitchDataNew = await cgs.GetSimplePriceForCryptocurrencies(cryptocurrenciesNeedsData.ToArray(), currency);

            if (cryptocurrenciesWitchDataNew.Status != HttpStatusCode.OK)
                return (HttpStatusCode.OK, cryptocurrenciesWitchData);

            //var a1 = cryptocurrenciesWitchDataNew.Value.Where(x => cryptocurrenciesNeedsData.All(y => y == x.Name)).ToArray();

            List<CryptocurrencySimpleData> a1 = new();
            List<CryptocurrencySimpleData> a2 = new();


            foreach ( var c in cryptocurrenciesWitchDataNew.Value)
            {
                if (cryptocurrenciesNeedsData.Contains(c.Name))
                    a1.Add(c);
                else
                    a2.Add(c);
            }

            var a = await _datasRepository.AddRangeAsync(a1.ToArray());

            //var a2 = cryptocurrenciesWitchDataNew.Value.Where(x => cryptocurrenciesNeedsRefreshData.All(y => y == x.Name)).ToList();

            //a2.ForEach(x => _datasRepository.UpdateAsync(x));

            foreach (var c in a1)
            {
                cryptocurrenciesWitchData.Add(new()
                {
                    Name = c.Name,
                    Symbol = c.Symbol,
                    Price = c.Price,
                    PricePercentChange24h = Math.Round(c.PricePercentChange24h, 2),
                    MarketCap = Math.Round(c.MarketCap, 2),
                    UpdateDate = c.UpdateDate,
                });
            }

            foreach (var c in a2)
            {
                await _datasRepository.UpdateAsync(c);

                cryptocurrenciesWitchDataNew.Value.First(x => x.Symbol == c.Symbol).Name = c.Name;
                cryptocurrenciesWitchDataNew.Value.First(x => x.Symbol == c.Symbol).Symbol = c.Symbol;
                cryptocurrenciesWitchDataNew.Value.First(x => x.Symbol == c.Symbol).Price = c.Price;
                cryptocurrenciesWitchDataNew.Value.First(x => x.Symbol == c.Symbol).PricePercentChange24h = Math.Round(c.PricePercentChange24h, 2);
                cryptocurrenciesWitchDataNew.Value.First(x => x.Symbol == c.Symbol).MarketCap = Math.Round(c.MarketCap, 2);
                cryptocurrenciesWitchDataNew.Value.First(x => x.Symbol == c.Symbol).UpdateDate = c.UpdateDate;
            }

            return (HttpStatusCode.OK, cryptocurrenciesWitchData);
        }
    }
}
