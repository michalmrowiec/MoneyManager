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
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserCommandResponse>
    {
        private readonly IUserAsyncRepository _userAsyncRepository;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(IUserAsyncRepository userAsyncRepository, IMapper mapper)
        {
            _userAsyncRepository = userAsyncRepository;
            _mapper = mapper;
        }

        public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new RegisterUserCommandValidator();
            var validatorResult = await validator.ValidateAsync(request);


            if (!validatorResult.IsValid)
                return new RegisterUserCommandResponse(validatorResult);

            var registerUser = _mapper.Map<Domain.Authentication.RegisterUser>(request);

            var userToken = await _userAsyncRepository.Register(registerUser);

            return new RegisterUserCommandResponse(userToken);
        }
    }
}
