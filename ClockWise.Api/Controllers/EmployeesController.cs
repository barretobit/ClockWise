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