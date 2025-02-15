using ClockWise.Api.Models;

namespace ClockWise.Api.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetAllEnabledCompaniesAsync();

        Task<Company> GetCompanyByIdAsync(int id);

        Task CreateCompanyAsync(Company company);

        Task UpdateCompanyAsync(Company company);

        Task DeleteCompanyAsync(Company company);

        Task<bool> CompanyExistsAsync(int id);
    }
}