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
        async Task<List<User>> IUserRepository.GetUsersByCompanyIdAsync(int companyId)
        {
            return await _context.Users.Where(u => u.IsEnabled && u.CompanyId == companyId).ToListAsync();
        }

        Task IUserRepository.CreateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        Task IUserRepository.DeleteUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        Task<List<User>> IUserRepository.GetAllEnabledUsersAsync()
        {
            throw new NotImplementedException();
        }

        Task<User> IUserRepository.GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        Task<User> IUserRepository.GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<User> IUserRepository.GetUserByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }


        Task IUserRepository.UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserRepository.UserExistsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
