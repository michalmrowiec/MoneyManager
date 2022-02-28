using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Contracts.Persistence.Items
{
    public interface IItemAsyncRepositoryBase<T> : IAsyncRepositoryBase<T> where T : class
    {
        Task<IList<T>> GetAllAsync(int userId);
    }
}
