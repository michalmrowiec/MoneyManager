using MoneyManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Contracts.Persistence.Items
{
    public interface IPlannedBudgetRepository : IItemAsyncRepositoryBase<PlannedBudget>
    {
        Task<IList<PlannedBudget>> GetPlannedBudgetsForMonth(int userId, int year, int month);
    }
}
