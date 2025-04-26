using ClockWise.Api.Models;

namespace ClockWise.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllEnabledUsersAsync();

        Task<List<User>> GetUsersByCompanyIdAsync(int companyId);

        Task<User> GetUserByIdAsync(int id);

        Task<User> GetUserByUsernameAsync(string username);

        Task<User> GetUserByEmailAsync(string email);

        Task CreateUserAsync(User user);

        Task UpdateUserAsync(User user);

        Task DeleteUserAsync(User user);

        Task<bool> UserExistsAsync(int id);
    }
}