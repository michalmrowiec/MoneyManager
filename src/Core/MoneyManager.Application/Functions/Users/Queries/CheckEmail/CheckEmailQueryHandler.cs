using MediatR;
using MoneyManager.Application.Contracts.Persistence.Users;

namespace MoneyManager.Application.Functions.Users.Queries.CheckEmail
{
    public class CheckEmailQueryHandler : IRequestHandler<CheckEmailQuery, bool>
    {
        private readonly IUserAsyncRepository _userAsyncRepository;

        public CheckEmailQueryHandler(IUserAsyncRepository userAsyncRepository)
        {
            _userAsyncRepository = userAsyncRepository;
        }
        public async Task<bool> Handle(CheckEmailQuery request, CancellationToken cancellationToken)
        {
            return await _userAsyncRepository.CheckEmail(request.Email);
        }
    }
}
