using MediatR;
using MoneyManager.Application.Contracts.Persistence.Users;

namespace MoneyManager.Application.Functions.Users.Commands.ChangePasswordUser
{
    internal class ChangePasswordUserCommandHandler : IRequestHandler<ChangePasswordUserCommand, ChangePasswordUserCommandResposne>
    {
        private readonly IUserAsyncRepository _userAsyncRepository;

        public ChangePasswordUserCommandHandler(IUserAsyncRepository userAsyncRepository)
        {
            _userAsyncRepository = userAsyncRepository;
        }

        public async Task<ChangePasswordUserCommandResposne> Handle(ChangePasswordUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new ChangePasswordCommandValidator();
            var validatorResult = await validator.ValidateAsync(request);

            if (!validatorResult.IsValid)
                return new ChangePasswordUserCommandResposne(validatorResult);

            var changePasswordResult = await _userAsyncRepository.ChangePassword(request.UserId, request.Password, request.RepeatPassword);

            if (changePasswordResult is false)
                return new ChangePasswordUserCommandResposne("Something went wrong. Contact support.", false);

            return new ChangePasswordUserCommandResposne();
        }
    }
}
