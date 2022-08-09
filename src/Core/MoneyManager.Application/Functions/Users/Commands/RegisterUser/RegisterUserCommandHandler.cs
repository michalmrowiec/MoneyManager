using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Users;
using MoneyManager.Application.Functions.Users.Services;
using MoneyManager.Domain.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserCommandResponse>
    {
        private readonly IUserAsyncRepository _userAsyncRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public RegisterUserCommandHandler(IUserAsyncRepository userAsyncRepository, IMapper mapper, IMediator mediator)
        {
            _userAsyncRepository = userAsyncRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new RegisterUserCommandValidator();
            var validatorResult = await validator.ValidateAsync(request);


            if (!validatorResult.IsValid)
                return new RegisterUserCommandResponse(validatorResult);

            var registerUser = _mapper.Map<Domain.Authentication.RegisterUser>(request);

            var userToken = await _userAsyncRepository.Register(registerUser);

            JwtSecurityTokenHandler tokenHandler = new();
            CreateStartCategoryForNewUser createStartCategory = new(_mediator);

            var token = tokenHandler.ReadJwtToken(userToken.Token);
            await createStartCategory.CrateStartCategory(int.Parse(token.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value));

            return new RegisterUserCommandResponse(userToken);
        }
    }
}
