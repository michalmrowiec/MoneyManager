namespace MoneyManager.Application.Contracts.Persistence.Items
{
    public interface IItemAsyncRepositoryBase<T> : IAsyncRepositoryBase<T> where T : class
    {
        Task<IList<T>> GetAllRecordsAsync(int userId);
    }
}
