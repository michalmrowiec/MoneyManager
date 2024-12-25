using MoneyManager.Domain.Entities.CryptoAssets;

namespace MoneyManager.Application.Contracts.Persistence.Items
{
    public interface ICryptoSimpleDatasRepository
    {
        Task<CryptocurrencySimpleData?> GetByNameAsync(string name);
        Task<CryptocurrencySimpleData> AddAsync(CryptocurrencySimpleData entity);
        Task UpdateAsync(CryptocurrencySimpleData entity);
        Task DeleteAsync(CryptocurrencySimpleData entity);
        Task DeleteAllAsync();
        Task<CryptocurrencySimpleData[]> AddRangeAsync(CryptocurrencySimpleData[] entities);
        Task<List<CryptoSymbolName>> GetSymbolsAndNames();
        Task<List<CryptocurrencySimpleData>> GetByNamesAsync(string[] names);
    }
}
