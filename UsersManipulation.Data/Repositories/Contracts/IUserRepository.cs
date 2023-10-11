using UsersManipulation.Data.Entity;

namespace UsersManipulation.Data.Repositories.Contracts
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        Task<UserEntity> GetUserByNameAsync(string username);
        Task<UserEntity> GetUserByEmailAsync(string email);
        Task<UserEntity> FindByEmailAsync(string email);
    }
}
