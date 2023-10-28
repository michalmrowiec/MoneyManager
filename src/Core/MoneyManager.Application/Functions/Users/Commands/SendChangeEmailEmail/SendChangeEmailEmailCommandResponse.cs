using MoneyManager.Application.Responses;
using FluentValidation.Results;

namespace MoneyManager.Application.Functions.Users.Commands.SendChangeEmailEmail
{
    public class SendChangeEmailEmailCommandResponse : BaseResponse
    {
        public SendChangeEmailEmailCommandResponse(ValidationResult validationResult) : base(validationResult)
        { }
        public SendChangeEmailEmailCommandResponse(string message) : base(message)
        { }
    }
}
