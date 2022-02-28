using Microsoft.EntityFrameworkCore;

namespace MoneyManager.Server.Entities
{
    public class TrackerDbContext : DbContext
    {
        public TrackerDbContext(DbContextOptions<TrackerDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users => Set<User>();
        public DbSet<RecordItem> RecordItems => Set<RecordItem>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecordItem>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(25);


            modelBuilder.Entity<RecordItem>()
                .Property(x => x.Amount)
                .IsRequired();
        }
    }
}
