using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Domain.Entities;

namespace MoneyManager.Infractructure.Repositories.Items
{
    internal class RecordRepository : ItemRepositoryBase<Record>, IRecordRepository
    {
        public RecordRepository(MoneyManagerContext dbContext) : base(dbContext)
        { }

        public async Task<Record[]> AddRangeRecordAsync(Record[] records)
        {
            await _dbContext.Records.AddRangeAsync(records);
            await _dbContext.SaveChangesAsync();

            return records;
        }

        public override async Task<IList<Record>> GetAllRecordsAsync(int userId)
        {
            var records = await _dbContext.Records
                .Include(r => r.Category)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return records;
        }

        public async Task<IList<Record>> GetRecordsForCategoryAsync(int userId, int cateogryId)
        {
            var records = await _dbContext.Records
                .Include(r => r.Category)
                .Where(r => r.UserId == userId && r.CategoryId == cateogryId)
                .ToListAsync();

            return records;
        }

        public async Task<IList<Record>> GetRecordsForMonthAsync(int userId, int year, int month)
        {
            var records = await _dbContext.Records
                .Where(r => r.UserId == userId && r.TransactionDate.Year == year && r.TransactionDate.Month == month)
                .ToListAsync();

            return records;
        }
    }
}
