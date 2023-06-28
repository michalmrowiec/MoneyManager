using MoneyManager.Domain.Entities.CryptoAssets;
using System.Net;

namespace MoneyManager.Infractructure.Services.CryptocurrencyServices
{
    public interface ICryptoApiProvider
    {
        /// <summary>
        /// This method retrieves basic cryptocurrency data from an external API.
        /// </summary>
        /// <param name="cryptocurrencies">Full official names of cryptocurrencies, e.g.: "Bitcoin", "Ethereum", "BNB", "Bitcoin Cash".</param>
        /// <param name="currency">The currency in which the given cryptocurrencies are to be valued, e.g.: "USD", "EUR".</param>
        /// <returns></returns>
        Task<(HttpStatusCode Status, List<CryptocurrencySimpleData> Value)> GetBasicCryptocurrenciesInfo(string[] cryptocurrencies, string currency);
    }
}