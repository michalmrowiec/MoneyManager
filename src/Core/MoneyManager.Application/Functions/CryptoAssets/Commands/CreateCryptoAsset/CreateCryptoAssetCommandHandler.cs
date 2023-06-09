using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Domain.Entities.CryptoAssets;

namespace MoneyManager.Application.Functions.CryptoAssets.Commands.CreateCryptoAsset
{
    public class CreateCryptoAssetCommandHandler : IRequestHandler<CreateCryptoAssetCommand, CreateCryptoAssetCommandResponse>
    {
        private readonly ICryptoAssetsRepository _cryptoAssetsRepository;
        private readonly IMapper _mapper;

        public CreateCryptoAssetCommandHandler(ICryptoAssetsRepository cryptoAssetsRepository, IMapper mapper)
        {
            _cryptoAssetsRepository = cryptoAssetsRepository;
            _mapper = mapper;
        }
        public async Task<CreateCryptoAssetCommandResponse> Handle(CreateCryptoAssetCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateCrytoAssetCommandValidator();
            var validatorResult = await validator.ValidateAsync(request);

            if (!validatorResult.IsValid)
                return new CreateCryptoAssetCommandResponse(validatorResult);

            var cryptoAsset = _mapper.Map<CryptoAsset>(request);

            cryptoAsset = await _cryptoAssetsRepository.AddAsync(cryptoAsset);

            return new CreateCryptoAssetCommandResponse(cryptoAsset.Id);
        }
    }
}
