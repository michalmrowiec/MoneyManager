using MediatR;
using MoneyManager.Application.Contracts.Persistence;
using MoneyManager.Application.Functions.Users.Queries.CheckEmail;

namespace MoneyManager.Application.Functions.Users.Commands.SendForgotPasswordEmail
{
    internal class SendForgotPasswordEmailCommandHandler : IRequestHandler<SendForgotPasswordEmailCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IEmailSender _emailSender;
        public SendForgotPasswordEmailCommandHandler(IMediator mediator, IEmailSender emailSender)
        {
            _mediator = mediator;
            _emailSender = emailSender;
        }
        public async Task<bool> Handle(SendForgotPasswordEmailCommand request, CancellationToken cancellationToken)
        {
            var emailExist = await _mediator.Send(new CheckEmailQuery(request.UserEmail));

            if (emailExist)
            {
                await _emailSender.SendForgotPasswordEmailAsync("placeHolder", request.UserEmail);
                return true;
            }

            return false;
        }
    }
}