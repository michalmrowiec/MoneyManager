using FluentValidation.Results;
using MoneyManager.Application.Responses;

namespace MoneyManager.Application.Functions.CryptoAssets.Commands.CreateCryptoAsset
{
    public class CreateCryptoAssetCommandResponse : BaseResponse
    {
        public int? CryptoAssetId { get; set; }
        public CreateCryptoAssetCommandResponse() : base()
        { }

        public CreateCryptoAssetCommandResponse(ValidationResult validationResult) : base(validationResult)
        { }

        public CreateCryptoAssetCommandResponse(string message) : base(message)
        { }

        public CreateCryptoAssetCommandResponse(string message, bool succes) : base(message, succes)
        { }

        public CreateCryptoAssetCommandResponse(int cryptoAssetId)
        {
            CryptoAssetId = cryptoAssetId;
        }
    }
}
