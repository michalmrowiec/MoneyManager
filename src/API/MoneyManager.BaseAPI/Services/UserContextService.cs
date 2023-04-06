using System.Security.Claims;

namespace MoneyManager.API.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public int GetUserId => User is null ? 0 : int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
