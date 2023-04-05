using MoneyManager.Domain.Entities;

namespace MoneyManager.Application.Contracts.Persistence
{
    public interface IApiKeyRepository
    {
        Task<ApiKey> GetApiKey(string apiKey);

    }
}
