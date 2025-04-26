using AutoMapper;
using ClockWise.Api.DTOs;
using ClockWise.Api.Models;
using ClockWise.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClockWise.Api.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AuthController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IEmployeeRepository employeeRepository, IMapper mapper, ILogger<AuthController> logger)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Auth(AuthenticationDto authenticationDto)
        {
            try
            {
                Employee employee = await _employeeRepository.GetEmployeeByEmailAsync(authenticationDto.Email);

                if (employee == null)
                {
                    return NotFound("Employee not found.");
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering.");
                return StatusCode(500, "Internal Server Error.");
            }
        }
    }
}
