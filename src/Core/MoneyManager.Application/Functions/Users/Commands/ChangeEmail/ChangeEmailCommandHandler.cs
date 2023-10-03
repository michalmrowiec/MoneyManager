using MediatR;
using Microsoft.Extensions.Caching.Memory;
using MoneyManager.Application.Contracts.Persistence.Users;

namespace MoneyManager.Application.Functions.Users.Commands.ChangeEmail
{
    internal class ChangeEmailCommandHandler : IRequestHandler<ChangeEmailCommand, ChangeEmailCommandResponse>
    {
        private readonly IUserAsyncRepository _userAsyncRepository;
        private readonly IMemoryCache _memoryCache;

        public ChangeEmailCommandHandler(IUserAsyncRepository userAsyncRepository, IMemoryCache memoryCache)
        {
            _userAsyncRepository = userAsyncRepository;
            _memoryCache = memoryCache;
        }
        public async Task<ChangeEmailCommandResponse> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
        {
            bool success = _memoryCache.TryGetValue(request.KeyConfirmingEmailChange, out ChangeEmailModel changeEmail);

            if (!success)
            {
                return new ChangeEmailCommandResponse("Something went wrong. Contact support.", false);
            }

            var result = await _userAsyncRepository.ChangeEmail(changeEmail.UserId, changeEmail.NewEmail);

            if (!result)
            {
                return new ChangeEmailCommandResponse("Something went wrong. Contact support.", false);
            }

            _memoryCache.Remove(request.KeyConfirmingEmailChange);

            return new ChangeEmailCommandResponse("Email has been changed.", true);
        }
    }
}
