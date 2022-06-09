﻿using Microsoft.EntityFrameworkCore;
using MoneyManager.Application.Contracts.Persistence.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Infractructure.Repositories.Items
{
    internal class ItemRepositoryBase<T> : IItemAsyncRepositoryBase<T> where T : class
    {
        protected readonly MoneyManagerContext _dbContext;
        public ItemRepositoryBase(MoneyManagerContext moneyManagerContext)
        {
            _dbContext = moneyManagerContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(int userId, T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<IList<T>> GetAllAsync(int userId)
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int userId, int itemId)
        {
            return await _dbContext.Set<T>().FindAsync(itemId);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
