using MoneyManager.Domain.Entities.CryptoAssets;
using Newtonsoft.Json;
using System.Net;

namespace MoneyManager.Infractructure.Services.CryptocurrencyServices
{
    internal class CoingeckoApiService : ICryptoApiProvider
    {
        private readonly HttpClient _httpClient = new();

        public async Task<(HttpStatusCode Status, List<CryptocurrencySimpleData> Value)> GetBasicCryptocurrenciesInfo(string[] cryptocurrencies, string currency)
        {
            string baseUrl = "https://api.coingecko.com/api/v3/simple/price";

            var dcl = await GetAllCurrencies();

            if (dcl.Status != HttpStatusCode.OK)
                return (dcl.Status, new());

            var cryptocurrenciesList = dcl.Value;

            var cryptocurrenciesIds = cryptocurrenciesList
                .Where(x => x.Name == cryptocurrencies.FirstOrDefault(y => y == x.Name))
                .Select(x => x.Id)
                .ToArray();

            UriBuilder builder = new(baseUrl)
            {
                Query = $"ids={string.Join(",", cryptocurrenciesIds)}&vs_currencies={currency.ToLower()}&include_market_cap=true&include_24hr_change=true"
            };

            var httpResponse = await _httpClient.GetAsync(builder.Uri);
            if (httpResponse.StatusCode != HttpStatusCode.OK)
                return (httpResponse.StatusCode, new());

            var a = await httpResponse.Content.ReadAsStringAsync();

            var dl = JsonConvert.DeserializeObject<dynamic>(a);

            List<CryptocurrencySimpleData> cryptocurrenciesWitchData = new();

            foreach (var cr in cryptocurrenciesIds)
            {
                _ = decimal.TryParse(dl?[cr]?[currency]?.ToString(), out decimal cu);
                _ = decimal.TryParse(dl?[cr]?[currency + "_market_cap"]?.ToString(), out decimal mc);
                _ = decimal.TryParse(dl?[cr]?[currency + "_24h_change"]?.ToString(), out decimal ch);

                cryptocurrenciesWitchData.Add(new()
                {
                    Name = cryptocurrenciesList.FirstOrDefault(x => x.Id == cr)?.Name ?? "",
                    Symbol = cryptocurrenciesList.FirstOrDefault(x => x.Id == cr)?.Symbol ?? "",
                    Price = cu,
                    PricePercentChange24h = Math.Round(ch, 2),
                    MarketCap = Math.Round(mc, 2),
                    UpdateDate = DateTime.Now,
                });
            }

            return (httpResponse.StatusCode, cryptocurrenciesWitchData);
        }

        private async Task<(HttpStatusCode Status, List<CoinSimple> Value)> GetAllCurrencies()
        {
            string url = @"https://api.coingecko.com/api/v3/coins/list?include_platform=false";

            var httpResponse = await _httpClient.GetAsync(url);
            if (httpResponse.StatusCode != HttpStatusCode.OK)
                return (httpResponse.StatusCode, new());

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var coins = JsonConvert.DeserializeObject<List<CoinSimple>>(responseContent) ?? new();

            return (httpResponse.StatusCode, coins);
        }
    }
}
