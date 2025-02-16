using ClockWise.Api.Data;
using ClockWise.Api.Models;
using ClockWise.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClockWise.Api.UnitTests.Repositories
{
    public class CompanyRepositoryTests
    {
        private readonly ClockWiseDbContext _dbContext;
        private readonly CompanyRepository _repository;

        public CompanyRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ClockWiseDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ClockWiseDbContext(options);
            _repository = new CompanyRepository(_dbContext);

            _dbContext.Companies.AddRange(new List<Company>
        {
            new Company { Id = 1, Name = "Company A", IsEnabled = true },
            new Company { Id = 2, Name = "Company B", IsEnabled = false },
            new Company { Id = 3, Name = "Company C", IsEnabled = true }
        });
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task GetAllEnabledCompaniesAsync_Returns_OnlyEnabledCompanies()
        {
            // Act
            var result = await _repository.GetAllEnabledCompaniesAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, c => Assert.True(c.IsEnabled));
        }
    }
}