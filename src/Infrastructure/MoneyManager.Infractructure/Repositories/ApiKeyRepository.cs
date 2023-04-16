using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.Contracts.Persistence;
using MoneyManager.Domain.Entities;

namespace MoneyManager.Infractructure.Repositories
{
    public class ApiKeyRepository : IApiKeyRepository
    {
        protected readonly MoneyManagerContext _dbContext;

        public ApiKeyRepository(MoneyManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiKey> GetApiKey(string apiKey)
        {
            return await _dbContext.ApiKeys.FirstOrDefaultAsync(k => k.Key == apiKey);
        }
    }
}
