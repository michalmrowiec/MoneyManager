using MediatR;

namespace MoneyManager.Application.Functions.Users.Commands.ChangePasswordUser
{
    public record ChangePasswordUserCommand : IRequest<ChangePasswordUserCommandResposne>
    {
        public int UserId { get; set; }
        public string Password { get; init; }
        public string RepeatPassword { get; init; }
    }
}
