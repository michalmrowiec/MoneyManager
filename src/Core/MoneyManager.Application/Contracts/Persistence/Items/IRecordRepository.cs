using MoneyManager.Domain.Entities;

namespace MoneyManager.Application.Contracts.Persistence.Items
{
    public interface IRecordRepository : IItemAsyncRepositoryBase<Record>, IGetRecordForMonths<Record>
    {
        Task<IList<Record>> GetRecordsForCategory(int userId, int cateogryId);
        Task<IList<Record>> GetRecordsForMonth(int userId, int year, int month);
        Task<Record[]> AddRangeRecordAsync(Record[] records);
    }
}
