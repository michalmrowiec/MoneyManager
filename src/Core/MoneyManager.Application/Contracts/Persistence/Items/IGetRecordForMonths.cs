namespace MoneyManager.Application.Contracts.Persistence.Items
{
    public interface IGetRecordForMonths<T> where T : class
    {
        Task<IList<T>> GetRecordsForMonthAsync(int userId, int year, int month);
    }
}
