using AutoMapper;
using ClockWise.Api.Controllers;
using ClockWise.Api.DTOs;
using ClockWise.Api.Models;
using ClockWise.Api.Models.Mappings;
using ClockWise.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ClockWise.Api.UnitTests.Controllers
{
    public class EmployeeTypesControllerTests
    {
        private readonly IMapper _mapper;

        public EmployeeTypesControllerTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetAllEmployeeTypes_ReturnsOkResultWithEmployeeTypes()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeTypeRepository>();
            var employeeTypes = new List<EmployeeType>
            {
                new EmployeeType { Id = 1, TypeName = "Full-Time", IsEnabled = true },
                new EmployeeType { Id = 2, TypeName = "Part-Time", IsEnabled = true }
            };
            mockRepository.Setup(repo => repo.GetAllEnabledEmployeeTypesAsync()).ReturnsAsync(employeeTypes);

            var controller = new EmployeeTypesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.GetAllEmployeeTypes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEmployeeTypes = Assert.IsType<List<EmployeeTypeDto>>(okResult.Value);
            Assert.Equal(2, returnedEmployeeTypes.Count);
        }

        [Fact]
        public async Task GetAllEmployeeTypes_ReturnsNotFound_WhenNoEmployeeTypesExist()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeTypeRepository>();
            mockRepository.Setup(repo => repo.GetAllEnabledEmployeeTypesAsync()).ReturnsAsync(new List<EmployeeType>());

            var controller = new EmployeeTypesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.GetAllEmployeeTypes();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No employee types found", notFoundResult.Value);
        }

        [Fact]
        public async Task GetEmployeeType_ReturnsOkResultWithEmployeeType()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeTypeRepository>();
            var employeeType = new EmployeeType { Id = 1, TypeName = "Full-Time", IsEnabled = true };
            mockRepository.Setup(repo => repo.GetEmployeeTypeByIdAsync(1)).ReturnsAsync(employeeType);

            var controller = new EmployeeTypesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.GetEmployeeType(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEmployeeType = Assert.IsType<EmployeeTypeDto>(okResult.Value);
            Assert.Equal("Full-Time", returnedEmployeeType.TypeName);
        }

        [Fact]
        public async Task GetEmployeeType_ReturnsNotFound_WhenEmployeeTypeDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeTypeRepository>();
            mockRepository.Setup(repo => repo.GetEmployeeTypeByIdAsync(It.IsAny<int>())).ReturnsAsync((EmployeeType)null);

            var controller = new EmployeeTypesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.GetEmployeeType(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Employee Type not found", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateEmployeeType_ReturnsCreatedAtActionWithEmployeeTypeDto()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeTypeRepository>();
            var createEmployeeTypeDto = new EmployeeTypeDto
            {
                TypeName = "Contract",
                CompanyId = 1
            };

            var employeeType = new EmployeeType
            {
                Id = 10,
                TypeName = "Contract",
                CompanyId = 1,
                IsEnabled = true
            };

            mockRepository.Setup(repo => repo.CreateEmployeeTypeAsync(It.IsAny<EmployeeType>()))
                          .Callback<EmployeeType>(e => e.Id = 10)
                          .Returns(Task.CompletedTask);

            var controller = new EmployeeTypesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.CreateEmployeeType(createEmployeeTypeDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedEmployeeType = Assert.IsType<EmployeeTypeDto>(createdResult.Value);
            Assert.Equal(10, returnedEmployeeType.Id);
        }

        [Fact]
        public async Task UpdateEmployeeType_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeTypeRepository>();
            var existingEmployeeType = new EmployeeType { Id = 1, TypeName = "Full-Time", IsEnabled = true };
            var updateDto = new EmployeeTypeDto { Id = 1, TypeName = "Updated Type", IsEnabled = false };

            mockRepository.Setup(repo => repo.GetEmployeeTypeByIdAsync(1)).ReturnsAsync(existingEmployeeType);
            mockRepository.Setup(repo => repo.UpdateEmployeeTypeAsync(existingEmployeeType)).Returns(Task.CompletedTask);

            var controller = new EmployeeTypesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.UpdateEmployeeType(1, updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateEmployeeType_ReturnsNotFound_WhenEmployeeTypeDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeTypeRepository>();
            mockRepository.Setup(repo => repo.GetEmployeeTypeByIdAsync(It.IsAny<int>())).ReturnsAsync((EmployeeType)null);

            var controller = new EmployeeTypesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.UpdateEmployeeType(1, new EmployeeTypeDto());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteEmployeeType_ReturnsNoContent_WhenDeleteIsSuccessful()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeTypeRepository>();
            var existingEmployeeType = new EmployeeType { Id = 1, TypeName = "Admin" };

            mockRepository.Setup(repo => repo.GetEmployeeTypeByIdAsync(1)).ReturnsAsync(existingEmployeeType);
            mockRepository.Setup(repo => repo.DeleteEmployeeTypeAsync(existingEmployeeType)).Returns(Task.CompletedTask);

            var controller = new EmployeeTypesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.DeleteEmployeeType(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteEmployeeType_ReturnsNotFound_WhenEmployeeTypeDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeTypeRepository>();
            mockRepository.Setup(repo => repo.GetEmployeeTypeByIdAsync(It.IsAny<int>())).ReturnsAsync((EmployeeType)null);

            var controller = new EmployeeTypesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.DeleteEmployeeType(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}