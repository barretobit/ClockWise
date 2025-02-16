using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClockWise.Api.Controllers;
using ClockWise.Api.DTOs;
using ClockWise.Api.Models;
using ClockWise.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ClockWise.Api.UnitTests.Controllers
{
    public class CompaniesControllerTests
    {
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

            var controller = new CompaniesController(mockRepository.Object);

            // Act
            var result = await controller.GetAllCompanies();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCompanies = Assert.IsType<List<CompanyDto>>(okResult.Value);
            Assert.Equal(2, returnedCompanies.Count);
            Assert.Equal("Company A", returnedCompanies.First().Name);
            Assert.Equal("Company C", returnedCompanies.Last().Name);
        }
    }
}