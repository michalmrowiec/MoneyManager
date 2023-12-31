using MediatR;
using MoneyManager.Application.Functions.Users.Commands.SendChangeEmailEmail.Dto;

namespace MoneyManager.Application.Functions.Users.Commands.ChangeEmail
{
    public record ChangeEmailCommand(Guid KeyConfirmingEmailChange, LoginWithNewEmailModel LoginWithNewEmail) : IRequest<ChangeEmailCommandResponse>;
}
