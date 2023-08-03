using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Domain.Entities.CryptoAssets;

namespace MoneyManager.Infractructure.Repositories.Items
{
    internal class CryptoSimpleDatasRepository : ICryptoSimpleDatasRepository
    {
        protected readonly MoneyManagerContext _dbContext;
        public CryptoSimpleDatasRepository(MoneyManagerContext moneyManagerContext)
        {
            _dbContext = moneyManagerContext;
        }

        public async Task<CryptocurrencySimpleData> AddAsync(CryptocurrencySimpleData entity)
        {
            await _dbContext.Set<CryptocurrencySimpleData>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
        public async Task<CryptocurrencySimpleData[]> AddRangeAsync(CryptocurrencySimpleData[] entities)
        {
            await _dbContext.CryptoSimpleDatas.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();

            return entities;
        }

        public async Task DeleteAllAsync()
        {
            _dbContext.CryptoSimpleDatas.RemoveRange(_dbContext.CryptoSimpleDatas.ToArray());
        }

        public async Task DeleteAsync(CryptocurrencySimpleData entity)
        {
            _dbContext.Set<CryptocurrencySimpleData>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CryptocurrencySimpleData?> GetByNameAsync(string name)
        {
            return await _dbContext.CryptoSimpleDatas.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<CryptocurrencySimpleData>> GetByNamesAsync(string[] names)
        {
            return await _dbContext.CryptoSimpleDatas.Where(c => names.Contains(c.Name)).ToListAsync();
        }

        public async Task<Dictionary<string, string>> GetSymbolsAndNames()
        {
            return await _dbContext.CryptoSimpleDatas.AsNoTracking().ToDictionaryAsync(c => c.Symbol, c => c.Name);
        }

        public async Task UpdateAsync(CryptocurrencySimpleData entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
