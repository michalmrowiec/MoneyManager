using MediatR;

namespace MoneyManager.Application.Functions.Users.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<LoginUserCommandResponse>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
