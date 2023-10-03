using MoneyManager.Application.Responses;

namespace MoneyManager.Application.Functions.Users.Commands.ChangeEmail
{
    public class ChangeEmailCommandResponse : BaseResponse
    {
        public ChangeEmailCommandResponse(string message, bool success) : base(message, success)
        { }
    }
}
