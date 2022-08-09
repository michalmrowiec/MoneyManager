using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Infractructure.Repositories.Items
{
    internal class CategoryRepository : ItemRepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(MoneyManagerContext dbContext) : base(dbContext)
        { }
    }
}
