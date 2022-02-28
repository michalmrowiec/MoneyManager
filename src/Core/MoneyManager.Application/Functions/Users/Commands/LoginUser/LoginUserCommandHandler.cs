using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Users;
using MoneyManager.Domain.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Users.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserToken>
    {
        private readonly IUserAsyncRepository _userAsyncRepository;
        private readonly IMapper _mapper;

        public LoginUserCommandHandler(IUserAsyncRepository userAsyncRepository, IMapper mapper)
        {
            _userAsyncRepository = userAsyncRepository;
            _mapper = mapper;
        }

        public async Task<UserToken> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var loginUser = _mapper.Map<Domain.Authentication.LoginUser>(request);
            return await _userAsyncRepository.Login(loginUser);
        }
    }
}
