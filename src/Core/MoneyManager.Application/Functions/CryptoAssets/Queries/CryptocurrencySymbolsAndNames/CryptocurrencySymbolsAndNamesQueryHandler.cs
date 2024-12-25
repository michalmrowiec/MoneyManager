using MediatR;
using MoneyManager.Application.Contracts.Services;

namespace MoneyManager.Application.Functions.CryptoAssets.Queries.CryptocurrencySymbolsAndNames
{
    public class CryptocurrencySymbolsAndNamesQueryHandler : IRequestHandler<CryptocurrencySymbolsAndNamesQuery, List<CryptoSymbolName>>
    {
        private readonly IAsyncCryptocurrencyService _cryptocurrencyService;

        public CryptocurrencySymbolsAndNamesQueryHandler(IAsyncCryptocurrencyService cryptocurrencyService)
        {
            _cryptocurrencyService = cryptocurrencyService;
        }

        public async Task<List<CryptoSymbolName>> Handle(CryptocurrencySymbolsAndNamesQuery request, CancellationToken cancellationToken)
        {
            return await _cryptocurrencyService.GetCryptocurrencySymbolsAndNames();
        }
    }
}
