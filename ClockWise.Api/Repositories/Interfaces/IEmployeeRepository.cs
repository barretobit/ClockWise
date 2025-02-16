using ClockWise.Api.Models;

namespace ClockWise.Api.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllEnabledEmployeesAsync();

        Task<Employee> GetEmployeeByIdAsync(int id);

        Task CreateEmployeeAsync(Employee employee);

        Task UpdateEmployeeAsync(Employee employee);

        Task DeleteEmployeeAsync(Employee employee);

        Task<bool> EmployeeExistsAsync(int id);
    }
}