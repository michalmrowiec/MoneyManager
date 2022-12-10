using MoneyManager.Domain.Authentication;

namespace MoneyManager.Application.Contracts.Persistence
{
    public interface IGenerateResetPasswordJWT
    {
        UserToken GenerateToken(string userEmail, int userId, string userName);
    }
}
