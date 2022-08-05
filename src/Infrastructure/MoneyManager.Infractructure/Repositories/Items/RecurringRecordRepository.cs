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
    internal class RecurringRecordRepository : ItemRepositoryBase<RecurringRecord>, IRecurringRecordRepository
    {
        public RecurringRecordRepository(MoneyManagerContext moneyManagerContext) : base(moneyManagerContext)
        {
        }

        public override async Task<RecurringRecord> GetByIdAsync(int userId, int itemId)
        {
            return await _dbContext.RecurringRecords.FirstAsync(x => x.UserId == userId && x.Id == itemId);
        }
        public override async Task<IList<RecurringRecord>> GetAllAsync(int userId)
        {
            return await _dbContext.RecurringRecords.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
