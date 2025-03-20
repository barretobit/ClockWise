using AutoMapper;
using ClockWise.Api.DTOs;
using ClockWise.Api.Models;
using ClockWise.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClockWise.Api.Controllers
{
    [ApiController]
    [Route("api/registry")]
    public class RegistersController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegistersController> _logger;

        public RegistersController(ICompanyRepository companyRepository, IEmployeeRepository employeeRepository, IMapper mapper, ILogger<RegistersController> logger)
        {
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterDto registerDto)
        {
            try
            {
                var company = new Company
                {
                    Name = "Undefined",
                    IsEnabled = true
                };

                await _companyRepository.CreateCompanyAsync(company);

                var companyDto = _mapper.Map<CompanyDto>(company);

                var employee = new Employee
                {
                    CompanyId = companyDto.Id,
                    Name = companyDto.Name,
                    Email = registerDto.Email,
                    PasswordHash = registerDto.Password,
                    IsEnabled = true
                };

                await _employeeRepository.CreateEmployeeAsync(employee);

                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering.");
                return StatusCode(500, "Internal Server Error.");
            }
        }
    }
}
