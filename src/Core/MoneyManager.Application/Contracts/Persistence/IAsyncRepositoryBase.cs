namespace MoneyManager.Application.Contracts.Persistence
{
    public interface IAsyncRepositoryBase<T> where T : class
    {
        Task<T?> GetByIdAsync(int userId, int itemId);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int userId, T entity);
    }
}
