using MediatR;

namespace MoneyManager.Application.Functions.Users.Commands.SendForgotPasswordEmail
{
    public record SendForgotPasswordEmailCommand(string UserEmail) : IRequest<bool>;
}
