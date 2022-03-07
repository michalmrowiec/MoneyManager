using FluentValidation.Results;
using MoneyManager.Application.Responses;
using MoneyManager.Domain.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Users.Commands.RegisterUser
{
    public class RegisterUserCommandResponse : BaseResponse
    {
        public UserToken? UserToken { get; set; }
        public RegisterUserCommandResponse() : base()
        { }

        public RegisterUserCommandResponse(ValidationResult validationResult) : base(validationResult)
        { }

        public RegisterUserCommandResponse(string message) : base(message)
        { }

        public RegisterUserCommandResponse(string message, bool succes) : base(message, succes)
        { }

        public RegisterUserCommandResponse(UserToken userToken)
        {
            UserToken = userToken;
        }
    }
}
