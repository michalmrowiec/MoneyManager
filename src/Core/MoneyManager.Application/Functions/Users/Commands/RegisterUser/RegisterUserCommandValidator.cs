using FluentValidation;
using MediatR;
using MoneyManager.Application.Functions.Users.Queries.CheckEmail;

namespace MoneyManager.Application.Functions.Users.Commands.RegisterUser
{
    internal class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        private readonly IMediator _mediator;
        internal RegisterUserCommandValidator(IMediator mediator)
        {
            _mediator = mediator;

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .MaximumLength(25)
                .WithMessage("{PropertyName} must not exceed 25 characters");

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .EmailAddress()
                .WithMessage("{PropertyName} must be email address")
                .Custom((value, context) =>
                {
                    //Validate email in use
                    if(_mediator.Send(new CheckEmailQuery(value)).Result) context.AddFailure("Email", "Email is taken");
                });

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .MinimumLength(6)
                .WithMessage("{PropertyName} must be above 6 characters")
                .MaximumLength(35)
                .WithMessage("{PropertyName} must not exceed 35 characters");

            RuleFor(x => x.RepeatPassword)
                .Equal(x => x.Password)
                .WithMessage("Passwords are not the same");
        }
    }
}
