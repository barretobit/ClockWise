using AutoMapper;
using ClockWise.Api.DTOs;
using ClockWise.Api.Models;
using ClockWise.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClockWise.Api.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllEnabledEmployeesAsync();

            if (employees == null || employees.Count == 0)
            {
                return NotFound("No employees found");
            }

            var employeeDtos = _mapper.Map<List<EmployeeDto>>(employees);
            return Ok(employeeDtos);
        }

        /// <summary>
        /// Retrieves all disabled employees.
        /// </summary>
        /// <returns>A list of disabled employee DTOs.</returns>
        [HttpGet("disabled")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllDisabledEmployees()
        {
            var employees = await _employeeRepository.GetAllDisabledEmployeesAsync();

            if (employees == null || !employees.Any())
            {
                return NotFound("No disabled employees found");
            }

            var employeeDtos = _mapper.Map<List<EmployeeDto>>(employees);
            return Ok(employeeDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return Ok(employeeDto);
        }

        /// <summary>
        /// Searches for employees by a given name (first name, last name, or full name).
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>A list of employee DTOs matching the search criteria.</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> SearchEmployeesByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Search name cannot be empty.");
            }

            var employees = await _employeeRepository.SearchEmployeesByNameAsync(name);

            if (employees == null || !employees.Any())
            {
                return NotFound($"No employees found matching '{name}'");
            }

            var employeeDtos = _mapper.Map<List<EmployeeDto>>(employees);
            return Ok(employeeDtos);
        }

        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeesByCompanyId(int companyId)
        {
            var employees = await _employeeRepository.GetEmployeesByCompanyIdAsync(companyId);

            if (employees == null || employees.Count == 0)
            {
                return NotFound("No employees found");
            }

            var employeeDtos = _mapper.Map<List<EmployeeDto>>(employees);
            return Ok(employeeDtos);
        }

        [HttpGet("/byemail/{email}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeByEmail(string email)
        {
            Employee employee = await _employeeRepository.GetEmployeeByEmailAsync(email);

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            var employeeDto = _mapper.Map<List<EmployeeDto>>(employee);
            return Ok(employeeDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeDto createEmployeeDto)
        {
            var employee = _mapper.Map<Employee>(createEmployeeDto);
            employee.IsEnabled = true;

            await _employeeRepository.CreateEmployeeAsync(employee);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employeeDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeDto updateEmployeeDto)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _mapper.Map(updateEmployeeDto, employee);

            try
            {
                await _employeeRepository.UpdateEmployeeAsync(employee);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _employeeRepository.EmployeeExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            await _employeeRepository.DeleteEmployeeAsync(employee);
            return NoContent();
        }
    }
}