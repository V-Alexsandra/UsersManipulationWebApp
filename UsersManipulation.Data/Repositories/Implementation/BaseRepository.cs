using Microsoft.EntityFrameworkCore;
using UsersManipulation.Data.Contexts.Implementation;
using UsersManipulation.Data.Entity;
using UsersManipulation.Data.Repositories.Contracts;

namespace UsersManipulation.Data.Implementation
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected ApplicationDbContext appContext;
        protected DbSet<TEntity> DbSet;

        public BaseRepository(ApplicationDbContext appContext)
        {
            this.appContext = appContext;
            DbSet = appContext.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() =>
           await DbSet
           .AsNoTracking()
           .ToListAsync();

        public virtual async Task<TEntity?> GetByIdAsync(int id) =>
            await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            var created = await DbSet.AddAsync(entity);
            await appContext.SaveChangesAsync();

            return created.Entity;
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await appContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            await appContext.SaveChangesAsync();
        }
    }
}
