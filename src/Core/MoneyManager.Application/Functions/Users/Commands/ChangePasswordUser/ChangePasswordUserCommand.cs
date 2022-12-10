using MediatR;

namespace MoneyManager.Application.Functions.Users.Commands.ChangePasswordUser
{
    public record ChangePasswordUserCommand(int UserId, string Password, string RepeatPassword) : IRequest<bool>;
}
