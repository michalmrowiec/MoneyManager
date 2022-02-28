using FluentValidation;

namespace MoneyManager.Application.Functions.Records
{
    public class CreateRecordCommandValidator : AbstractValidator<CreateRecordCommand>
    {
        public CreateRecordCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(25)
                .WithMessage("{PropertyName} must not exceed 25 characters");

            RuleFor(x => x.Amount)
                .NotEmpty()
                .WithMessage("{PropertyName} is required");
        }
    }
}