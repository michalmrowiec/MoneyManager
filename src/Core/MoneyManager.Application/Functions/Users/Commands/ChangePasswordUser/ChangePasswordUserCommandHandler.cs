using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Users;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Users.Commands.ChangePasswordUser
{
    internal class ChangePasswordUserCommandHandler : IRequestHandler<ChangePasswordUserCommand, bool>
    {
        private readonly IUserAsyncRepository _userAsyncRepository;

        public ChangePasswordUserCommandHandler(IUserAsyncRepository userAsyncRepository)
        {
            _userAsyncRepository = userAsyncRepository;
        }

        public async Task<bool> Handle(ChangePasswordUserCommand request, CancellationToken cancellationToken)
        {
            return await _userAsyncRepository.ChangePassword(request.UserId, request.Password, request.RepeatPassword);
        }
    }
}
