using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
