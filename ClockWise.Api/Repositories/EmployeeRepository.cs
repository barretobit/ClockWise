using ClockWise.Api.Data;
using ClockWise.Api.Models;
using ClockWise.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClockWise.Api.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ClockWiseDbContext _context;

        public EmployeeRepository(ClockWiseDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllEnabledEmployeesAsync()
        {
            return await _context.Employees.Where(e => e.IsEnabled == true).ToListAsync();
        }

        public async Task<List<Employee>> GetAllDisabledEmployeesAsync()
        {
            return await _context.Employees
                                 .Where(e => !e.IsEnabled)
                                 .ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<List<Employee>> SearchEmployeesByNameAsync(string name)
        {
            return await _context.Employees
                                .Where(e => e.IsEnabled && 
                                            e.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase))
                                .ToListAsync();
        }

        public async Task<List<Employee>> GetEmployeesByCompanyIdAsync(int companyId)
        {
            return await _context.Employees.Where(e => e.IsEnabled == true && e.CompanyId == companyId).ToListAsync();
        }

        public async Task<Employee> GetEmployeeByEmailAsync(string email)
        {
            return await _context.Employees.Where(e => e.IsEnabled == true && e.Email.Contains(email)).FirstOrDefaultAsync();
        }

        public async Task CreateEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(Employee employee)
        {
            employee.IsEnabled = false;
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmployeeExistsAsync(int id)
        {
            return await _context.Employees.AnyAsync(e => e.Id == id);
        }
    }
}