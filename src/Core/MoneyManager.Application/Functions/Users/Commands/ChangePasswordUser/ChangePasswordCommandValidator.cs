using FluentValidation;

namespace MoneyManager.Application.Functions.Users.Commands.ChangePasswordUser
{
    internal class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordUserCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .MinimumLength(6)
                .WithMessage("{PropertyName} must be above 6 characters")
                .MaximumLength(50)
                .WithMessage("{PropertyName} must not exceed 35 characters");

            RuleFor(x => x.RepeatPassword)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .MinimumLength(6)
                .WithMessage("{PropertyName} must be above 6 characters")
                .MaximumLength(50)
                .WithMessage("{PropertyName} must not exceed 35 characters")
                .Equal(x => x.Password)
                .WithMessage("Passwords are not the same");
        }
    }
}
