using Microsoft.EntityFrameworkCore;
using Dentlike.Domain.Entities;

namespace Dentlike.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        // 通过 DI 传入
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // entity sets
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 自动扫描当前程序集中的 IEntityTypeConfiguration<T>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
