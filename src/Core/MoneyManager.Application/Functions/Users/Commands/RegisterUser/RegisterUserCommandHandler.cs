using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Users;
using MoneyManager.Domain.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserToken>
    {
        private readonly IUserAsyncRepository _userAsyncRepository;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(IUserAsyncRepository userAsyncRepository, IMapper mapper)
        {
            _userAsyncRepository = userAsyncRepository;
            _mapper = mapper;
        }

        public async Task<UserToken> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var registerUser = _mapper.Map<Domain.Authentication.RegisterUser>(request);
            return await _userAsyncRepository.Register(registerUser);
        }
    }
}
