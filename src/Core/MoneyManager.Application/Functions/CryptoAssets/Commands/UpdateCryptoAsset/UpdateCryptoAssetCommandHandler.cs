using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.Entities.CryptoAssets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.CryptoAssets.Commands.UpdateCryptoAsset
{
    public class UpdateCryptoAssetCommandHandler : IRequestHandler<UpdateCryptoAssetCommand>
    {
        private readonly ICryptoAssetsRepository _cryptoAssetsRepository;
        private readonly IMapper _mapper;

        public UpdateCryptoAssetCommandHandler(ICryptoAssetsRepository cryptoAssetsRepository, IMapper mapper)
        {
            _cryptoAssetsRepository= cryptoAssetsRepository;
            _mapper= mapper;
        }

        public async Task<Unit> Handle(UpdateCryptoAssetCommand request, CancellationToken cancellationToken)
        {
            var cryptoAsset = _mapper.Map<CryptoAsset>(request);

            await _cryptoAssetsRepository.UpdateAsync(cryptoAsset);

            return Unit.Value;
        }
    }
}
