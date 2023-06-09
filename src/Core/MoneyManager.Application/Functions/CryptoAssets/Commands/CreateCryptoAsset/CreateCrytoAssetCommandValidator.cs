using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace MoneyManager.Application.Functions.CryptoAssets.Commands.CreateCryptoAsset
{
    internal class CreateCrytoAssetCommandValidator : AbstractValidator<CreateCryptoAssetCommand>
    {
        public CreateCrytoAssetCommandValidator()
        {
            RuleFor(x => x.Symbol)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .NotNull();

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .NotNull();
        }
    }
}
