using MoneyManager.Application.Responses;
using MoneyManager.Domain.Authentication;

namespace MoneyManager.Application.Functions.Users.Commands.ChangeEmail
{
    public class ChangeEmailCommandResponse : BaseResponse
    {
        public UserToken? UserToken { get; set; }

        public ChangeEmailCommandResponse(string message, bool success) : base(message, success)
        { }

        public ChangeEmailCommandResponse(string message, bool success, UserToken userToken)
        {
            UserToken = userToken;
        }
    }
}
