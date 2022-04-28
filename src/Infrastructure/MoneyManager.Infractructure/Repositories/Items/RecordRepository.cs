using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Infractructure.Repositories.Items
{
    internal class RecordRepository : ItemRepositoryBase<Record>, IRecordRepository
    {
        public RecordRepository(MoneyManagerContext dbContext) : base(dbContext)
        { }

        public async Task<IList<Record>> GetRecordsForCategory(int userId, int cateogryId)
        {
            var records = await _dbContext.RecordItems.Where(x => x.UserId == userId && x.CategoryId == cateogryId).ToListAsync();
            var category = _dbContext.Categories.Where(x => x.UserId == userId).First(x => x.Id == cateogryId);

            records.ForEach(x => { x.CategoryId = category.Id; x.Category = category; });

            return records;
        }

        public override async Task<IList<Record>> GetAllAsync(int userId)
        {
            var records = await _dbContext.RecordItems.Where(x => x.UserId == userId).ToListAsync();
            var categories = await _dbContext.Categories.Where(x => x.UserId == userId).ToListAsync();

            var recordsWithCategories =
            from record in records
            join category in categories on record.CategoryId equals category.Id into ps
            from supCategory in ps.DefaultIfEmpty()
            select new Record
            {
                Id = record.Id,
                Name = record.Name,
                Amount = record.Amount,
                TransactionDate = record.TransactionDate,
                CategoryId = supCategory?.Id,
                Category = supCategory
            };

            return recordsWithCategories.ToList();
        }

        public override async Task<Record> GetByIdAsync(int userId, int itemId)
        {
            return await _dbContext.RecordItems.FirstAsync(x => x.UserId == userId && x.Id == itemId);
        }

        //public override async Task UpdateAsync(Record entity)
        //{
        //    var record = await _dbContext.RecordItems.FindAsync(entity.Id);
        //    if(record == null)
        //        return;
        //    _dbContext.Entry(entity).CurrentValues.SetValues(record);
        //    await _dbContext.SaveChangesAsync();
        //}
    }
}
