﻿using MediatR;
using MoneyManager.Application.Contracts.Services;
using MoneyManager.Application.Functions.Users.Queries.GetUserId;

namespace MoneyManager.Application.Functions.Users.Commands.SendForgotPasswordEmail
{
    internal class SendForgotPasswordEmailCommandHandler : IRequestHandler<SendForgotPasswordEmailCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IEmailService _emailSender;
        private readonly IGenerateResetPasswordJWT _generateResetPasswordJWT;
        public SendForgotPasswordEmailCommandHandler(IMediator mediator, IEmailService emailSender, IGenerateResetPasswordJWT generateResetPasswordJWT)
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

            await _emailSender.SendForgotPasswordEmailAsync(request.UserEmail, token);

            return true;
        }
    }
}