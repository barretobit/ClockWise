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
    public class EmployeesControllerTests
    {
        private readonly IMapper _mapper;

        public EmployeesControllerTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task GetAllEmployees_ReturnsOkResultWithEmployees()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Email = "john@example.com", IsEnabled = true },
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane@example.com", IsEnabled = true }
            };
            mockRepository.Setup(repo => repo.GetAllEnabledEmployeesAsync()).ReturnsAsync(employees);

            var controller = new EmployeesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.GetAllEmployees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEmployees = Assert.IsType<List<EmployeeDto>>(okResult.Value);
            Assert.Equal(2, returnedEmployees.Count);
        }

        [Fact]
        public async Task GetAllEmployees_ReturnsNotFound_WhenNoEmployeesExist()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(repo => repo.GetAllEnabledEmployeesAsync()).ReturnsAsync(new List<Employee>());

            var controller = new EmployeesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.GetAllEmployees();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No employees found", notFoundResult.Value);
        }

        [Fact]
        public async Task GetEmployee_ReturnsOkResultWithEmployee()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var employee = new Employee { Id = 1, Name = "John Doe", Email = "john@example.com", IsEnabled = true };
            mockRepository.Setup(repo => repo.GetEmployeeByIdAsync(1)).ReturnsAsync(employee);

            var controller = new EmployeesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.GetEmployee(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEmployee = Assert.IsType<EmployeeDto>(okResult.Value);
            Assert.Equal("John Doe", returnedEmployee.Name);
        }

        [Fact]
        public async Task GetEmployee_ReturnsNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(repo => repo.GetEmployeeByIdAsync(It.IsAny<int>())).ReturnsAsync((Employee)null);

            var controller = new EmployeesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.GetEmployee(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Employee not found", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateEmployee_ReturnsCreatedAtActionWithEmployeeDto()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var createEmployeeDto = new EmployeeDto
            {
                Name = "New Employee",
                Email = "new@example.com",
                CompanyId = 1,
                EmployeeTypeId = 2
            };

            var employee = new Employee
            {
                Id = 10,
                Name = "New Employee",
                Email = "new@example.com",
                CompanyId = 1,
                EmployeeTypeId = 2,
                IsEnabled = true
            };

            mockRepository.Setup(repo => repo.CreateEmployeeAsync(It.IsAny<Employee>()))
                          .Callback<Employee>(e => e.Id = 10)
                          .Returns(Task.CompletedTask);

            var controller = new EmployeesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.CreateEmployee(createEmployeeDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedEmployee = Assert.IsType<EmployeeDto>(createdResult.Value);
            Assert.Equal(10, returnedEmployee.Id);
        }

        [Fact]
        public async Task UpdateEmployee_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var existingEmployee = new Employee { Id = 1, Name = "John Doe", Email = "john@example.com", IsEnabled = true };
            var updateDto = new EmployeeDto { Id = 1, Name = "Updated Name", Email = "updated@example.com", IsEnabled = false };

            mockRepository.Setup(repo => repo.GetEmployeeByIdAsync(1)).ReturnsAsync(existingEmployee);
            mockRepository.Setup(repo => repo.UpdateEmployeeAsync(existingEmployee)).Returns(Task.CompletedTask);

            var controller = new EmployeesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.UpdateEmployee(1, updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateEmployee_ReturnsNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(repo => repo.GetEmployeeByIdAsync(It.IsAny<int>())).ReturnsAsync((Employee)null);

            var controller = new EmployeesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.UpdateEmployee(1, new EmployeeDto());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnsNoContent_WhenDeleteIsSuccessful()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            var existingEmployee = new Employee { Id = 1, Name = "John Doe" };

            mockRepository.Setup(repo => repo.GetEmployeeByIdAsync(1)).ReturnsAsync(existingEmployee);
            mockRepository.Setup(repo => repo.DeleteEmployeeAsync(existingEmployee)).Returns(Task.CompletedTask);

            var controller = new EmployeesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.DeleteEmployee(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnsNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(repo => repo.GetEmployeeByIdAsync(It.IsAny<int>())).ReturnsAsync((Employee)null);

            var controller = new EmployeesController(mockRepository.Object, _mapper);

            // Act
            var result = await controller.DeleteEmployee(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}