using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;

namespace MoneyManager.Application.Functions.CryptoAssets.Commands.DeleteCryptoAsset
{
    public class DeleteCryptoAssetCommandHandler : IRequestHandler<DeleteCryptoAssetCommand>
    {
        private readonly ICryptoAssetsRepository _cryptoAssetsRepository;

        public DeleteCryptoAssetCommandHandler(ICryptoAssetsRepository cryptoAssetsRepository)
        {
            _cryptoAssetsRepository = cryptoAssetsRepository;
        }

        public async Task<Unit> Handle(DeleteCryptoAssetCommand request, CancellationToken cancellationToken)
        {
            var cryptoAssetDelete = await _cryptoAssetsRepository.GetByIdAsync(request.UserId, request.Id);
            await _cryptoAssetsRepository.DeleteAsync(request.UserId, cryptoAssetDelete);
            return Unit.Value;
        }
    }
}
