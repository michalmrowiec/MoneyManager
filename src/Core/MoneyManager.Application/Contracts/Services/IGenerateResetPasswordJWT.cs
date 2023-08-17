using MoneyManager.Domain.Authentication;

namespace MoneyManager.Application.Contracts.Services
{
    public interface IGenerateResetPasswordJWT
    {
        UserToken GenerateToken(string userEmail, int userId, string userName);
    }
}
