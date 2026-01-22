using Hexa_CSTS.Data;
using Hexa_CSTS.Models;
using Microsoft.EntityFrameworkCore;



namespace Hexa_CSTS.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HexaDbContext _context;

        public UserRepository(HexaDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Users.FindAsync(id);
            if (existing == null) return;

            _context.Users.Remove(existing);
            await _context.SaveChangesAsync();
        }


        public async Task DeactivateAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
        {
            var query = _context.Users.AsQueryable().Where(u => u.Email == email);

            if (excludeUserId.HasValue)
            {
                query = query.Where(u => u.UserId != excludeUserId.Value);
            }

            return await query.AnyAsync();
        }


        public async Task<bool> ExistsAsync(int id) =>
            await _context.Users.AnyAsync(u => u.UserId == id);

        public async Task<List<User>> GetActiveUsers() =>
            await _context.Users
                .AsNoTracking()
                .Where(u => u.IsActive)
                .ToListAsync();

        public async Task<List<User>> GetAllUsers() =>
            await _context.Users.AsNoTracking().ToListAsync();

        public async Task<User> GetByEmailAsync(string email) =>
            await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User> GetByIdAsync(int id) =>
            await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);

        public async Task<List<User>> GetRole(UserRole role) =>
            await _context.Users
                .AsNoTracking()
                .Where(u => u.Role == role)
                .ToListAsync();

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}