using MoneyManager.Domain.Entities.CryptoAssets;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;

namespace MoneyManager.Infractructure.Services.CryptocurrencyServices
{
    internal class CoingeckoApiService : ICryptoApiProvider
    {
        private readonly HttpClient _httpClient = new();

        public async Task<(HttpStatusCode Status, List<CryptocurrencySimpleData> Value)> GetCryptocurrenciesDataFromApi(int top, string currency)
        {
            List<CryptocurrencySimpleData> cryptocurrencySimpleDatas = new();

            UriBuilder uriBuilder = new("https://api.coingecko.com/api/v3/coins/markets")
            {
                Query = $"vs_currency={currency}&order=market_cap_desc&per_page={top}&page=1&sparkline=false&price_change_percentage=24h%2C7d&locale=pl"
            };

            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MoneyManagerApp");

            var response = await _httpClient.GetAsync(uriBuilder.Uri);

            if (response.StatusCode != HttpStatusCode.OK)
                return (response.StatusCode, new());

            var resposeContent = await response.Content.ReadAsStringAsync();

            var cryptocurrencies = JsonConvert.DeserializeObject<List<dynamic>>(resposeContent);

            foreach (var c in cryptocurrencies)
            {
                decimal price, mc, pcp24h, pcp7d;

                string priceText = c?.current_price.ToString(CultureInfo.GetCultureInfo("en-US")).ToLower() ?? "0";
                if (priceText.Contains('e'))
                    _ = decimal.TryParse(priceText, NumberStyles.Float | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out price);
                else
                    _ = decimal.TryParse(c?.current_price.ToString() ?? 0M, out price);
                _ = decimal.TryParse(c?.market_cap.ToString() ?? 0M, out mc);
                _ = decimal.TryParse(c?.price_change_percentage_24h_in_currency.ToString() ?? 0M, out pcp24h);
                _ = decimal.TryParse(c?.price_change_percentage_7d_in_currency.ToString() ?? 0M, out pcp7d);

                DateTimeOffset updateDateOffset = DateTimeOffset.ParseExact(c?.last_updated.ToString("yyyy-MM-ddThh:mm:ss.fffZ"), "yyyy-MM-ddThh:mm:ss.fffZ", null);
                var updateDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(updateDateOffset, "Central European Standard Time").DateTime;

                cryptocurrencySimpleDatas.Add(new CryptocurrencySimpleData
                {
                    Name = c?.name.ToString(),
                    Symbol = c?.symbol.ToString(),
                    Price = price,
                    PricePercentChange24h = Math.Round(pcp24h, 2),
                    PricePercentChange7d = Math.Round(pcp7d, 2),
                    MarketCap = Math.Round(mc, 2),
                    UpdateDate = updateDate,
                });
            }

            return (response.StatusCode, cryptocurrencySimpleDatas);
        }
    }
}
