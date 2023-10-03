using MediatR;
using Microsoft.Extensions.Caching.Memory;
using MoneyManager.Application.Contracts.Services;

namespace MoneyManager.Application.Functions.Users.Commands.SendChangeEmailEmail
{
    internal class SendChangeEmailEmailCommandHandler : IRequestHandler<SendChangeEmailEmailCommand, bool>
    {
        private readonly IEmailSender _emailSender;
        private readonly IMemoryCache _memoryCache;

        public SendChangeEmailEmailCommandHandler(IEmailSender emailSender, IMemoryCache memoryCache)
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

            string rer = @"https://www.moneymanager.hostingasp.pl/confirm-change-email?&keyConfirmingEmailChange=" + keyConfirmingEmailChange;
            string url = "<a href=" + rer + ">Confirm change emial</a>";

            await _emailSender.SendChangeEmialEmailAsync(url, request.NewEmail);

            return true;
        }
    }
}
