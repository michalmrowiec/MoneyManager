using Microsoft.EntityFrameworkCore;
using MoneyManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Infractructure
{
    public class MoneyManagerContext : DbContext
    {
        public MoneyManagerContext(DbContextOptions<MoneyManagerContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Record> RecordItems => Set<Record>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<RecurringRecord> RecurringRecords => Set<RecurringRecord>();
    }
}
