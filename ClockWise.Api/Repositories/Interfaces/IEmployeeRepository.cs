using ClockWise.Api.Models;

namespace ClockWise.Api.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllEnabledEmployeesAsync();

        Task<List<Employee>> GetAllDisabledEmployeesAsync();

        Task<List<Employee>> SearchEmployeesByNameAsync(string name);

        Task<Employee> GetEmployeeByIdAsync(int id);

        Task<List<Employee>> GetEmployeesByCompanyIdAsync(int companyId);

        Task<Employee> GetEmployeeByEmailAsync(string email);

        Task CreateEmployeeAsync(Employee employee);

        Task UpdateEmployeeAsync(Employee employee);

        Task DeleteEmployeeAsync(Employee employee);

        Task<bool> EmployeeExistsAsync(int id);
    }
}