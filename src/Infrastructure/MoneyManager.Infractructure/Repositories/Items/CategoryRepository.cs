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

        public override async Task<IList<Category>> GetAllAsync(int userId)
        {
            return await _dbContext.Categories.Where(x => x.UserId == userId).ToListAsync(); ;
        }

        public override async Task<Category> GetByIdAsync(int userId, int itemId)
        {
            return await _dbContext.Categories.FirstAsync(x => x.UserId == userId && x.Id == itemId);
        }
    }
}
