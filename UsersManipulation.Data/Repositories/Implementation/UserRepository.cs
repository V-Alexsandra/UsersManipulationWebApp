using Microsoft.EntityFrameworkCore;
using UsersManipulation.Data.Contexts.Implementation;
using UsersManipulation.Data.Entity;
using UsersManipulation.Data.Repositories.Contracts;

namespace UsersManipulation.Data.Implementation
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        protected ApplicationDbContext appContext;

        public UserRepository(ApplicationDbContext appContext) : base(appContext)
        {
        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            return await DbSet
                   .AsNoTracking()
                   .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<UserEntity> GetUserByNameAsync(string username)
        {
            return await DbSet
                   .AsNoTracking()
                   .FirstOrDefaultAsync(e => e.Name == username);
        }

        public async Task<UserEntity> FindByEmailAsync(string email)
        {
            return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
