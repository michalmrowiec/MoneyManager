using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Domain.Entities;

namespace MoneyManager.Infractructure.Repositories.Items
{
    internal class CategoryRepository : ItemRepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(MoneyManagerContext dbContext) : base(dbContext)
        { }
    }
}
