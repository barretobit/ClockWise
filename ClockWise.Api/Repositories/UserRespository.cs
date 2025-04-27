using ClockWise.Api.Data;
using ClockWise.Api.Models;
using ClockWise.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClockWise.Api.Repositories
{
    public class UserRespository : IUserRepository
    {
        private readonly ClockWiseDbContext _context;

        public UserRespository(ClockWiseDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllEnabledUsersAsync()
        {
            return await _context.Users.Where(u => u.IsEnabled == true).ToListAsync();
        }

        public async Task<List<User>> GetUsersByCompanyIdAsync(int companyId)
        {
            return await _context.Users.Where(u => u.IsEnabled && u.CompanyId == companyId).ToListAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Where(u => u.IsEnabled || u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.Where(u => u.IsEnabled || u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.Where(u => u.IsEnabled || u.UserName == username).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            user.IsEnabled = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }
    }
}