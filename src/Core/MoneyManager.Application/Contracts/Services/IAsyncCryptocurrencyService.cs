using MoneyManager.Application.Functions.CryptoAssets.Queries;
using MoneyManager.Domain.Entities.CryptoAssets;

namespace MoneyManager.Application.Contracts.Services
{
    public interface IAsyncCryptocurrencyService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cryptocurrencies">Full official names of cryptocurrencies, e.g.: "Bitcoin", "Ethereum", "BNB", "Bitcoin Cash".</param>
        /// <param name="currency">The currency in which the given cryptocurrencies are to be valued, e.g.: "USD", "EUR".</param>
        /// <returns></returns>
        Task<(ApiResponseStatus Status, List<CryptocurrencySimpleData> Value)> GetSimplePriceForCryptocurrencies(string[] cryptocurrencies, string currency);
        Task<List<CryptoSymbolName>> GetCryptocurrencySymbolsAndNames();
    }
}
