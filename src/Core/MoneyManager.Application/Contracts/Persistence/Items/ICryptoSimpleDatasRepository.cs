using MoneyManager.Domain.Entities.CryptoAssets;

namespace MoneyManager.Application.Contracts.Persistence.Items
{
    public interface ICryptoSimpleDatasRepository
    {
        Task<CryptocurrencySimpleData?> GetByNameAsync(string name);
        Task<CryptocurrencySimpleData> AddAsync(CryptocurrencySimpleData entity);
        Task UpdateAsync(CryptocurrencySimpleData entity);
        Task DeleteAsync(CryptocurrencySimpleData entity);
        Task<CryptocurrencySimpleData[]> AddRangeAsync(CryptocurrencySimpleData[] entities);

    }
}
