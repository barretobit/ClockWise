using ClockWise.Api.Data;
using ClockWise.Api.Models;
using ClockWise.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClockWise.Api.Repositories
{
    public class EmployeeTypeRepository : IEmployeeTypeRepository
    {
        private readonly ClockWiseDbContext _context;

        public EmployeeTypeRepository(ClockWiseDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeType>> GetAllEnabledEmployeeTypesAsync()
        {
            return await _context.EmployeeTypes.Where(e => e.IsEnabled == true).ToListAsync();
        }

        public async Task<EmployeeType> GetEmployeeTypeByIdAsync(int id)
        {
            return await _context.EmployeeTypes.FindAsync(id);
        }

        public async Task CreateEmployeeTypeAsync(EmployeeType employeeType)
        {
            _context.EmployeeTypes.Add(employeeType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeTypeAsync(EmployeeType employeeType)
        {
            _context.EmployeeTypes.Update(employeeType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeTypeAsync(EmployeeType employeeType)
        {
            employeeType.IsEnabled = false;
            _context.EmployeeTypes.Update(employeeType);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmployeeTypeExistsAsync(int id)
        {
            return await _context.EmployeeTypes.AnyAsync(e => e.Id == id);
        }
    }
}