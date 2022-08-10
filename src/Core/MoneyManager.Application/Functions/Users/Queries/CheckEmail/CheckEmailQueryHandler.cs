using MediatR;
using MoneyManager.Application.Contracts.Persistence.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
