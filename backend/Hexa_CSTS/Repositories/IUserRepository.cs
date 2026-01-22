using Hexa_CSTS.Models;


namespace Hexa_CSTS.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<List<User>> GetActiveUsers();
        Task<List<User>> GetAllUsers();
        Task<List<User>> GetRole(UserRole role);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task DeactivateAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email, int? excludeUserId = null);
    }
}