using MoneyManager.Application.Responses;
using MoneyManager.Domain.Authentication;
using FluentValidation.Results;

namespace MoneyManager.Application.Functions.Users.Commands.ChangePasswordUser
{
    public class ChangePasswordUserCommandResposne : BaseResponse
    {
        public UserToken? UserToken { get; set; }

        public ChangePasswordUserCommandResposne()
        { }

        public ChangePasswordUserCommandResposne(string? message = null) : base(message)
        { }

        public ChangePasswordUserCommandResposne(ValidationResult validationResult) : base(validationResult)
        { }

        public ChangePasswordUserCommandResposne(string message, bool success) : base(message, success)
        { }

        public ChangePasswordUserCommandResposne(UserToken userToken)
        {
            UserToken = userToken;
        }
    }
}
