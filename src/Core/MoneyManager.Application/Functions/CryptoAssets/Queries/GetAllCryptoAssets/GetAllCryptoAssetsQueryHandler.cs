using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Application.Functions.Records;

namespace MoneyManager.Application.Functions.CryptoAssets.Queries.GetAllCryptoAssets
{
    public class GetAllCryptoAssetsQueryHandler : IRequestHandler<GetAllCryptoAssetsQuery, List<CryptoAssetDto>>
    {
        private readonly ICryptoAssetsRepository _cryptoAssetsRepository;
        private readonly IMapper _mapper;

        public GetAllCryptoAssetsQueryHandler(ICryptoAssetsRepository cryptoAssetsRepository, IMapper mapper)
        {
            _cryptoAssetsRepository = cryptoAssetsRepository;
            _mapper = mapper;
        }

        public async Task<List<CryptoAssetDto>> Handle(GetAllCryptoAssetsQuery request, CancellationToken cancellationToken)
        {
            //return _mapper.Map<List<CryptoAssetDto>>(await _cryptoAssetsRepository.GetAllRecordsAsync(request.UserId));
            var listOfCryptoAssets = _mapper.Map<List<CryptoAssetDto>>(await _cryptoAssetsRepository.GetAllRecordsAsync(request.UserId));

        }
    }
}
