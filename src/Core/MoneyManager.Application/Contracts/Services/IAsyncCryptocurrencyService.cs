using MoneyManager.Domain.Entities.CryptoAssets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Contracts.Services
{
    public interface IAsyncCryptocurrencyService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cryptocurrencies">Full official names of cryptocurrencies, e.g.: "Bitcoin", "Ethereum", "BNB".</param>
        /// <param name="currency">The currency in which the given cryptocurrencies are to be valued, e.g.: "USD", "EUR".</param>
        /// <returns></returns>
        Task<List<CryptocurrencySimpleData>> GetSimplePriceForCryptocurrencies(string[] cryptocurrencies, params string[] currency);
    }
}
