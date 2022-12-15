using FluentValidation.Results;
using MoneyManager.Application.Responses;
using MoneyManager.Domain.Authentication;

namespace MoneyManager.Application.Functions.Users.Commands.LoginUser
{
    public class LoginUserCommandResponse : BaseResponse
    {
        public UserToken? UserToken { get; set; }
        public LoginUserCommandResponse() : base()
        { }

        public LoginUserCommandResponse(ValidationResult validationResult) : base(validationResult)
        { }

        public LoginUserCommandResponse(string message) : base(message)
        { }

        public LoginUserCommandResponse(string message, bool succes) : base(message, succes)
        { }

        public LoginUserCommandResponse(UserToken userToken)
        {
            UserToken = userToken;
        }
    }
}