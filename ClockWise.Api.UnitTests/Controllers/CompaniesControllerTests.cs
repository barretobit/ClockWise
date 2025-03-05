using AutoMapper;
using ClockWise.Api.Controllers;
using ClockWise.Api.DTOs;
using ClockWise.Api.Models;
using ClockWise.Api.Models.Mappings;
using ClockWise.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ClockWise.Api.UnitTests.Controllers
{
    public class CompaniesControllerTests
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CompaniesController> _logger;

        public CompaniesControllerTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _logger = new Mock<ILogger<CompaniesController>>().Object;
        }

        [Fact]
        public async Task GetAllCompanies_ReturnsOkResultWithCompanies()
        {
            // Arrange
            var mockRepository = new Mock<ICompanyRepository>();
            var companies = new List<Company>
            {
                new Company { Id = 1, Name = "Company A", IsEnabled = true },
                new Company { Id = 3, Name = "Company C", IsEnabled = true }
            };

            mockRepository.Setup(repo => repo.GetAllEnabledCompaniesAsync()).ReturnsAsync(companies);

            var controller = new CompaniesController(mockRepository.Object, _mapper, _logger);

            // Act
            var result = await controller.GetAllCompanies();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCompanies = Assert.IsType<List<CompanyDto>>(okResult.Value);
            Assert.Equal(2, returnedCompanies.Count);
            Assert.Equal("Company A", returnedCompanies.First().Name);
            Assert.Equal("Company C", returnedCompanies.Last().Name);
        }

        [Fact]
        public async Task GetAllCompanies_ReturnsNotFound_WhenNoCompaniesExist()
        {
            // Arrange
            var mockRepository = new Mock<ICompanyRepository>();
            mockRepository.Setup(repo => repo.GetAllEnabledCompaniesAsync()).ReturnsAsync(new List<Company>());

            var controller = new CompaniesController(mockRepository.Object, _mapper, _logger);

            // Act
            var result = await controller.GetAllCompanies();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No Companies found", notFoundResult.Value);
        }

        [Fact]
        public async Task GetCompanyById_ReturnsOkResultWithCompany()
        {
            // Arrange
            var mockRepository = new Mock<ICompanyRepository>();
            var company = new Company { Id = 1, Name = "Company A", PublicShortName = "compA", IsEnabled = true };
            mockRepository.Setup(repo => repo.GetCompanyByIdAsync(1)).ReturnsAsync(company);

            var controller = new CompaniesController(mockRepository.Object, _mapper, _logger);

            // Act
            var result = await controller.GetCompanyById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCompany = Assert.IsType<CompanyDto>(okResult.Value);
            Assert.Equal(1, returnedCompany.Id);
            Assert.Equal("Company A", returnedCompany.Name);
            Assert.Equal("compA", returnedCompany.PublicShortName);
            Assert.True(returnedCompany.IsEnabled);
        }

        [Fact]
        public async Task GetCompanyById_ReturnsNotFound_WhenCompanyDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<ICompanyRepository>();
            mockRepository.Setup(repo => repo.GetCompanyByIdAsync(It.IsAny<int>())).ReturnsAsync((Company)null);

            var controller = new CompaniesController(mockRepository.Object, _mapper, _logger);

            // Act
            var result = await controller.GetCompanyById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Company not found", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateCompany_ReturnsCreatedAtActionWithCompanyDto()
        {
            // Arrange
            var mockRepository = new Mock<ICompanyRepository>();
            var createCompanyDto = new CompanyDto
            {
                Name = "New Company",
                PublicShortName = "newcompany"
            };

            var company = new Company
            {
                Id = 10,
                Name = "New Company",
                PublicShortName = "newcompany",
                IsEnabled = true
            };

            mockRepository.Setup(repo => repo.CreateCompanyAsync(It.IsAny<Company>())).Callback<Company>(c => c.Id = 10).Returns(Task.CompletedTask);

            var controller = new CompaniesController(mockRepository.Object, _mapper, _logger);

            // Act
            var result = await controller.CreateCompany(createCompanyDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedCompany = Assert.IsType<CompanyDto>(createdResult.Value);
            Assert.Equal(10, returnedCompany.Id);
            Assert.Equal("New Company", returnedCompany.Name);
            Assert.Equal("newcompany", returnedCompany.PublicShortName);
            Assert.True(returnedCompany.IsEnabled);
        }

        [Fact]
        public async Task UpdateCompany_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var mockRepository = new Mock<ICompanyRepository>();
            var existingCompany = new Company { Id = 1, Name = "Company A", PublicShortName = "compA", IsEnabled = true };
            var updateDto = new CompanyDto { Id = 1, Name = "Updated Company", PublicShortName = "updatedcompA", IsEnabled = false };

            mockRepository.Setup(repo => repo.GetCompanyByIdAsync(1)).ReturnsAsync(existingCompany);
            mockRepository.Setup(repo => repo.UpdateCompanyAsync(existingCompany)).Returns(Task.CompletedTask);
            mockRepository.Setup(repo => repo.CompanyExistsAsync(1)).ReturnsAsync(true);

            var controller = new CompaniesController(mockRepository.Object, _mapper, _logger);

            // Act
            var result = await controller.UpdateCompany(1, updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateCompany_ReturnsNotFound_WhenCompanyDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<ICompanyRepository>();
            mockRepository.Setup(repo => repo.GetCompanyByIdAsync(It.IsAny<int>())).ReturnsAsync((Company)null);

            var controller = new CompaniesController(mockRepository.Object, _mapper, _logger);

            // Act
            var result = await controller.UpdateCompany(1, new CompanyDto());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCompany_ReturnsNoContent_WhenDeleteIsSuccessful()
        {
            // Arrange
            var mockRepository = new Mock<ICompanyRepository>();
            var existingCompany = new Company { Id = 1, Name = "Company A" };

            mockRepository.Setup(repo => repo.GetCompanyByIdAsync(1)).ReturnsAsync(existingCompany);
            mockRepository.Setup(repo => repo.DeleteCompanyAsync(existingCompany)).Returns(Task.CompletedTask);

            var controller = new CompaniesController(mockRepository.Object, _mapper, _logger);

            // Act
            var result = await controller.DeleteCompany(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCompany_ReturnsNotFound_WhenCompanyDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<ICompanyRepository>();
            mockRepository.Setup(repo => repo.GetCompanyByIdAsync(It.IsAny<int>())).ReturnsAsync((Company)null);

            var controller = new CompaniesController(mockRepository.Object, _mapper, _logger);

            // Act
            var result = await controller.DeleteCompany(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}