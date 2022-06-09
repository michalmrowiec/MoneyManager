using MoneyManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Contracts.Persistence.Items
{
    public interface IRecordRepository : IItemAsyncRepositoryBase<Record>
    {
        Task<IList<Record>> GetRecordsForCategory(int userId, int cateogryId);
    }
}
