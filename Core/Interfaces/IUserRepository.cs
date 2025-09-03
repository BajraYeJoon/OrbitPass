using OrbitPass.Core.Entities;

namespace OrbitPass.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<User>> GetAllAsync();
    }
}