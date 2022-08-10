using MoneyManager.Domain.Authentication;
using MoneyManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Contracts.Persistence.Users
{
    public interface IUserAsyncRepository
    {
        Task<UserToken> Register(RegisterUser registerUser);
        Task<UserToken> Login(LoginUser loginUser);
        Task<bool> CheckEmail(string email);
    }
}