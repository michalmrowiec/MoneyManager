﻿using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Infractructure.Repositories.Items
{
    internal class PlannedBudgetsRepository : ItemRepositoryBase<PlannedBudget>, IPlannedBudgetRepository
    {
        public PlannedBudgetsRepository(MoneyManagerContext dbContext) : base(dbContext)
        { }

        public async Task<IList<PlannedBudget>> GetRecordsForMonthAsync(int userId, int year, int month)
        {
            return await _dbContext.PlannedBudgets
                .Where(x => x.UserId == userId && x.PlanForMonth.Year == year && x.PlanForMonth.Month == month).ToListAsync();
        }
    }
}
