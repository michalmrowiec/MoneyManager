using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Contracts.Persistence
{
    public interface IAsyncRepositoryBase<T> where T : class
    {
        Task<T> GetByIdAsync(int userId, int itemId);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(int userId, T entity);
        Task DeleteAsync(int userId, T entity);
    }
}
