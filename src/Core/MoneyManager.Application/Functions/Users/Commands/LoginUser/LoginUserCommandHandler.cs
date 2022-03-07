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
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserCommandResponse>
    {
        private readonly IUserAsyncRepository _userAsyncRepository;
        private readonly IMapper _mapper;

        public LoginUserCommandHandler(IUserAsyncRepository userAsyncRepository, IMapper mapper)
        {
            _userAsyncRepository = userAsyncRepository;
            _mapper = mapper;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var loginUser = _mapper.Map<Domain.Authentication.LoginUser>(request);
            var userToken = await _userAsyncRepository.Login(loginUser);

            if (userToken.Email is null)
                return new LoginUserCommandResponse("", false);

            return new LoginUserCommandResponse(userToken);

        }
    }
}
