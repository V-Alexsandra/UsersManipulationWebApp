using Microsoft.EntityFrameworkCore;
using UsersManipulation.Data.Entity;

namespace UsersManipulation.Data.Contexts.Implementation
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public async Task<int> SaveChangesAsync() => await base.SaveChangesAsync();
    }
}
