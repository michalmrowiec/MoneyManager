using MediatR;
using MoneyManager.Domain.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Users.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<UserToken>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
