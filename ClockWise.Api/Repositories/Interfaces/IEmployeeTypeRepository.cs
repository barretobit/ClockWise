using ClockWise.Api.Models;

namespace ClockWise.Api.Repositories.Interfaces
{
    public interface IEmployeeTypeRepository
    {
        Task<List<EmployeeType>> GetAllEnabledEmployeeTypesAsync();

        Task<EmployeeType> GetEmployeeTypeByIdAsync(int id);

        Task CreateEmployeeTypeAsync(EmployeeType employeeType);

        Task UpdateEmployeeTypeAsync(EmployeeType employeeType);

        Task DeleteEmployeeTypeAsync(EmployeeType employeeType);

        Task<bool> EmployeeTypeExistsAsync(int id);
    }
}