using FluentValidation;
using MediatR;
using MoneyManager.Application.Functions.Users.Queries.CheckEmail;

namespace MoneyManager.Application.Functions.Users.Commands.SendChangeEmailEmail
{
    internal class SendChangeEmailEmailCommandValidator : AbstractValidator<SendChangeEmailEmailCommand>
    {
        private readonly IMediator _mediator;
        public SendChangeEmailEmailCommandValidator(IMediator mediator)
        {
            _mediator = mediator;

            RuleFor(x => x.NewEmail)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .EmailAddress()
                .WithMessage("{PropertyName} must be email address")
                .Custom((value, context) =>
                {
                    //Validate email in use
                    if (_mediator.Send(new CheckEmailQuery(value)).Result) context.AddFailure("Email", "Email is taken");
                });
        }
    }
}
