using MoneyManager.Domain.Authentication;

namespace MoneyManager.Application.Contracts.Persistence.Users
{
    public interface IUserAsyncRepository
    {
        Task<UserToken> Register(RegisterUser registerUser);
        Task<UserToken> Login(LoginUser loginUser);
        Task<bool> CheckEmail(string email);
    }
}