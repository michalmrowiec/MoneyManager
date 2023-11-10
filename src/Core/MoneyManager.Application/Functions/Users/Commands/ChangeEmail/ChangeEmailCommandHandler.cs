using MediatR;
using Microsoft.Extensions.Caching.Memory;
using MoneyManager.Application.Contracts.Persistence.Users;
using MoneyManager.Application.Functions.Users.Commands.LoginUser;
using MoneyManager.Application.Functions.Users.Commands.SendChangeEmailEmail.Dto;

namespace MoneyManager.Application.Functions.Users.Commands.ChangeEmail
{
    internal class ChangeEmailCommandHandler : IRequestHandler<ChangeEmailCommand, ChangeEmailCommandResponse>
    {
        private readonly IUserAsyncRepository _userAsyncRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IMediator _mediator;

        public ChangeEmailCommandHandler(IUserAsyncRepository userAsyncRepository, IMemoryCache memoryCache, IMediator mediator)
        {
            _userAsyncRepository = userAsyncRepository;
            _memoryCache = memoryCache;
            _mediator = mediator;
        }
        public async Task<ChangeEmailCommandResponse> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
        {
            bool success = _memoryCache.TryGetValue(request.KeyConfirmingEmailChange, out ChangeEmailModel changeEmail);

            if (!success || request.LoginWithNewEmail.NewEmail != changeEmail.NewEmail)
            {
                return new ChangeEmailCommandResponse("Something went wrong. Contact support.", false);
            }

            var loginResult = await _mediator.Send(
                new LoginUserCommand { Email = changeEmail.OldEmail, Password = request.LoginWithNewEmail.Password });

            if (!loginResult.Success)
            {
                return new ChangeEmailCommandResponse("The user does not exist or the password is incorrect.", false);
            }

            var changeEmailResult = await _userAsyncRepository.ChangeEmail(changeEmail.UserId, changeEmail.NewEmail);

            if (!changeEmailResult || loginResult.UserToken == null)
            {
                return new ChangeEmailCommandResponse("Something went wrong. Contact support.", false);
            }

            _memoryCache.Remove(request.KeyConfirmingEmailChange);

            return new ChangeEmailCommandResponse("Email has been changed.", true, loginResult.UserToken);
        }
    }
}
