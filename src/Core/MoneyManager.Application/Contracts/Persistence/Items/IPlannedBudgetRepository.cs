using MoneyManager.Domain.Entities;

namespace MoneyManager.Application.Contracts.Persistence.Items
{
    public interface IPlannedBudgetRepository : IItemAsyncRepositoryBase<PlannedBudget>, IGetRecordForMonths<PlannedBudget>
    {

    }
}
