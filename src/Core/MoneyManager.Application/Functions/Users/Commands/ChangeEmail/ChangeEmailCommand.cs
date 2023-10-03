using MediatR;

namespace MoneyManager.Application.Functions.Users.Commands.ChangeEmail
{
    public record ChangeEmailCommand(Guid KeyConfirmingEmailChange) : IRequest<ChangeEmailCommandResponse>;
}
