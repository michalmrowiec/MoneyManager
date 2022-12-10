using MediatR;
using MoneyManager.Application.Contracts.Persistence;
using MoneyManager.Application.Functions.Users.Queries.GetUserId;
using static System.Net.WebRequestMethods;

namespace MoneyManager.Application.Functions.Users.Commands.SendForgotPasswordEmail
{
    internal class SendForgotPasswordEmailCommandHandler : IRequestHandler<SendForgotPasswordEmailCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IEmailSender _emailSender;
        private readonly IGenerateResetPasswordJWT _generateResetPasswordJWT;
        public SendForgotPasswordEmailCommandHandler(IMediator mediator, IEmailSender emailSender, IGenerateResetPasswordJWT generateResetPasswordJWT)
        {
            _mediator = mediator;
            _emailSender = emailSender;
            _generateResetPasswordJWT = generateResetPasswordJWT;
        }
        public async Task<bool> Handle(SendForgotPasswordEmailCommand request, CancellationToken cancellationToken)
        {
            var userId = await _mediator.Send(new GetUserIdQuery(request.UserEmail));

            if (userId is null)
                return false;

            var token = _generateResetPasswordJWT.GenerateToken(request.UserEmail, (int)userId, "").Token;

            //TODO: uri as href in html syntax
            string url = @"https://www.moneymanager.hostingasp.pl/forgotpassword?user_id=" + userId + "&access_token=" + token;

            await _emailSender.SendForgotPasswordEmailAsync(url, request.UserEmail);

            return true;
        }
    }
}