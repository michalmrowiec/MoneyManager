using MediatR;
using MoneyManager.Application.Contracts.Persistence.Users;

namespace MoneyManager.Application.Functions.Users.Queries.GetUserId
{
    internal class GetUserIdQueryHandler : IRequestHandler<GetUserIdQuery, int?>
    {
        private readonly IUserAsyncRepository _userAsyncRepository;

        public GetUserIdQueryHandler(IUserAsyncRepository userAsyncRepository)
        {
            _userAsyncRepository = userAsyncRepository;
        }

        public async Task<int?> Handle(GetUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _userAsyncRepository.GetUserId(request.UserEmail);
        }
    }
}
