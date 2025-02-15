using ClockWise.Api.Data;
using ClockWise.Api.Models;
using ClockWise.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClockWise.Api.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ClockWiseDbContext _context;

        public CompanyRepository(ClockWiseDbContext context)
        {
            _context = context;
        }

        public async Task<List<Company>> GetAllEnabledCompaniesAsync()
        {
            return await _context.Companies.Where(c => c.IsEnabled == true).ToListAsync();
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return await _context.Companies.FindAsync(id);
        }

        public async Task CreateCompanyAsync(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCompanyAsync(Company company)
        {
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCompanyAsync(Company company)
        {
            company.IsEnabled = false;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CompanyExistsAsync(int id)
        {
            return await _context.Companies.AnyAsync(e => e.Id == id);
        }
    }
}