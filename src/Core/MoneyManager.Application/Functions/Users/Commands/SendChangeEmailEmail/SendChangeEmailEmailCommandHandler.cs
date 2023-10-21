using MediatR;
using Microsoft.Extensions.Caching.Memory;
using MoneyManager.Application.Contracts.Services;

namespace MoneyManager.Application.Functions.Users.Commands.SendChangeEmailEmail
{
    internal class SendChangeEmailEmailCommandHandler : IRequestHandler<SendChangeEmailEmailCommand, bool>
    {
        private readonly IEmailService _emailSender;
        private readonly IMemoryCache _memoryCache;

        public SendChangeEmailEmailCommandHandler(IEmailService emailSender, IMemoryCache memoryCache)
        {
            _emailSender = emailSender;
            _memoryCache = memoryCache;
        }
        public async Task<bool> Handle(SendChangeEmailEmailCommand request, CancellationToken cancellationToken)
        {
            var keyConfirmingEmailChange = Guid.NewGuid();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(DateTimeOffset.Now.AddHours(1));

            _memoryCache.Set(keyConfirmingEmailChange, new ChangeEmailModel(request.UserId, request.NewEmail), cacheEntryOptions);

            await _emailSender.SendChangeEmailEmailAsync(request.NewEmail, keyConfirmingEmailChange.ToString());

            return true;
        }
    }
}
