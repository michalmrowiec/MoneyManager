using MediatR;
using Microsoft.Extensions.Caching.Memory;
using MoneyManager.Application.Contracts.Persistence.Users;
using MoneyManager.Application.Contracts.Services;
using MoneyManager.Application.Functions.Users.Commands.SendChangeEmailEmail.Dto;

namespace MoneyManager.Application.Functions.Users.Commands.SendChangeEmailEmail
{
    internal class SendChangeEmailEmailCommandHandler : IRequestHandler<SendChangeEmailEmailCommand, SendChangeEmailEmailCommandResponse>
    {
        private readonly IUserAsyncRepository _userAsyncRepository;
        private readonly IEmailService _emailSender;
        private readonly IMemoryCache _memoryCache;
        private readonly IMediator _mediator;

        public SendChangeEmailEmailCommandHandler(IEmailService emailSender, IMemoryCache memoryCache, IUserAsyncRepository userAsyncRepository, IMediator mediator)
        {
            _userAsyncRepository = userAsyncRepository;
            _emailSender = emailSender;
            _memoryCache = memoryCache;
            _mediator = mediator;
        }
        public async Task<SendChangeEmailEmailCommandResponse> Handle(SendChangeEmailEmailCommand request, CancellationToken cancellationToken)
        {
            var validator = new SendChangeEmailEmailCommandValidator(_mediator);
            var validatorResult = await validator.ValidateAsync(request);

            if(!validatorResult.IsValid)
            {
                return new SendChangeEmailEmailCommandResponse(validatorResult);
            }

            var user = await _userAsyncRepository.GetUserByIdAsync(request.UserId);

            var keyConfirmingEmailChange = Guid.NewGuid();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(DateTimeOffset.Now.AddHours(1));

            _memoryCache.Set(
                keyConfirmingEmailChange,
                new ChangeEmailModel(request.UserId, NewEmail: request.NewEmail, OldEmail: user.Email),
                cacheEntryOptions);

            await _emailSender.SendChangeEmailEmailAsync(request.NewEmail, keyConfirmingEmailChange.ToString());

            return new SendChangeEmailEmailCommandResponse("Please check your email to confirm your email address change.");
        }
    }
}
