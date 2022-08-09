using System.Security.Claims;

namespace MoneyManager.API.Services
{
    public interface IUserContextService
    {

        int GetUserId { get; }
        ClaimsPrincipal? User { get; }
    }
}
