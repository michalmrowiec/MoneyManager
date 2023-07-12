using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Application.Contracts.Services;

namespace MoneyManager.Application.Functions.CryptoAssets.Queries.CryptocurrencySymbolsAndNames
{
    public class CryptocurrencySymbolsAndNamesQueryHandler : IRequestHandler<CryptocurrencySymbolsAndNamesQuery, Dictionary<string, string>>
    {
        private readonly IAsyncCryptocurrencyService _cryptocurrencyService;

        public CryptocurrencySymbolsAndNamesQueryHandler(IAsyncCryptocurrencyService cryptocurrencyService)
        {
            _cryptocurrencyService = cryptocurrencyService;
        }

        public async Task<Dictionary<string, string>> Handle(CryptocurrencySymbolsAndNamesQuery request, CancellationToken cancellationToken)
        {
            var res = await _cryptocurrencyService.GetCryptocurrencySymbolsAndNames();
            return res.CryptocurrencySymbolsAndNames;
        }
    }
}
