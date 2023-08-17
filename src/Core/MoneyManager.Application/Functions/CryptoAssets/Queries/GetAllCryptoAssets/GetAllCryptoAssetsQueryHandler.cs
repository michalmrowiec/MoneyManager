using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Application.Contracts.Services;

namespace MoneyManager.Application.Functions.CryptoAssets.Queries.GetAllCryptoAssets
{
    public class GetAllCryptoAssetsQueryHandler : IRequestHandler<GetAllCryptoAssetsQuery, List<CryptoAssetDto>>
    {
        private readonly ICryptoAssetsRepository _cryptoAssetsRepository;
        private readonly IAsyncCryptocurrencyService _cryptocurrencyService;
        private readonly IMapper _mapper;

        public GetAllCryptoAssetsQueryHandler(ICryptoAssetsRepository cryptoAssetsRepository, IMapper mapper, IAsyncCryptocurrencyService cryptocurrencyService)
        {
            _cryptoAssetsRepository = cryptoAssetsRepository;
            _mapper = mapper;
            _cryptocurrencyService = cryptocurrencyService;
        }

        public async Task<List<CryptoAssetDto>> Handle(GetAllCryptoAssetsQuery request, CancellationToken cancellationToken)
        {
            var listOfCryptoAssets = await _cryptoAssetsRepository.GetAllRecordsAsync(request.UserId);

            var cryptocurrencySimpleDatas = await _cryptocurrencyService.GetSimplePriceForCryptocurrencies(
                listOfCryptoAssets.Select(x => x.Name).ToArray(), "usd");

            List<CryptoAssetDto> result = new();

            if(cryptocurrencySimpleDatas.Status != ApiResponseStatus.Ok)
                return result;

            foreach (var cryptoAsset in listOfCryptoAssets)
            {
                var cryptocurrencySimpleData = cryptocurrencySimpleDatas.Value.FirstOrDefault(x => x.Name == cryptoAsset.Name) ?? new();

                result.Add(new CryptoAssetDto()
                {
                    Id = cryptoAsset.Id,
                    CryptocurrencySymbol = cryptoAsset.Symbol,
                    CryptocurrencyName = cryptoAsset.Name,
                    Description = cryptoAsset.Description,
                    Amount = cryptoAsset.Amount,
                    ActualPrice = cryptocurrencySimpleData.Price,
                    PricePercentChange24h = cryptocurrencySimpleData.PricePercentChange24h,
                    MarketCap = cryptocurrencySimpleData.MarketCap,
                    DataForDateTime= cryptocurrencySimpleData.UpdateDate,
                    UserId = cryptoAsset.UserId
                });
            }

            return result;
        }
    }
}
