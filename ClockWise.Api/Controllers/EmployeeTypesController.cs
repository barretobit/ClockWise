using ClockWise.Api.DTOs;
using ClockWise.Api.Models;
using ClockWise.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClockWise.Api.Controllers
{
    [ApiController]
    [Route("api/employeeTypes")]
    public class EmployeeTypesController : ControllerBase
    {
        private readonly IEmployeeTypeRepository _employeeTypeRepository;

        public EmployeeTypesController(IEmployeeTypeRepository employeeTypeRepository)
        {
            _employeeTypeRepository = employeeTypeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeTypeDto>>> GetAllEmployeeTypes()
        {
            var employeeTypes = await _employeeTypeRepository.GetAllEnabledEmployeeTypesAsync();

            if (employeeTypes == null || employeeTypes.Count == 0)
            {
                return NotFound("No employee types found");
            }

            var employeeTypesDtos = employeeTypes.Select(e => new EmployeeTypeDto
            {
                Id = e.Id,
                TypeName = e.TypeName,
                CompanyId = e.CompanyId,
                IsEnabled = e.IsEnabled
            }).ToList();

            return Ok(employeeTypesDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeTypeDto>> GetEmployeeType(int id)
        {
            var employeeType = await _employeeTypeRepository.GetEmployeeTypeByIdAsync(id);

            if (employeeType == null)
            {
                return NotFound("Employee Type not found");
            }

            var employeeTypeDto = new EmployeeTypeDto
            {
                Id = employeeType.Id,
                TypeName = employeeType.TypeName,
                CompanyId = employeeType.CompanyId,
                IsEnabled = employeeType.IsEnabled
            };

            return Ok(employeeTypeDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeType(EmployeeTypeDto createEmployeeTypeDto)
        {
            var employeeType = new EmployeeType
            {
                TypeName = createEmployeeTypeDto.TypeName,
                CompanyId = createEmployeeTypeDto.CompanyId,
                IsEnabled = true,
            };

            await _employeeTypeRepository.CreateEmployeeTypeAsync(employeeType);

            var employeeTypeDto = new EmployeeTypeDto
            {
                Id = employeeType.Id,
                TypeName = employeeType.TypeName,
                CompanyId = employeeType.CompanyId,
                IsEnabled = employeeType.IsEnabled,
            };

            return CreatedAtAction(nameof(GetEmployeeType), new { id = employeeType.Id }, employeeTypeDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeType(int id, EmployeeTypeDto updateEmployeeTypeDto)
        {
            var employeeType = await _employeeTypeRepository.GetEmployeeTypeByIdAsync(id);

            if (employeeType == null)
            {
                return NotFound();
            }

            employeeType.TypeName = updateEmployeeTypeDto.TypeName;
            employeeType.IsEnabled = updateEmployeeTypeDto.IsEnabled;

            try
            {
                await _employeeTypeRepository.UpdateEmployeeTypeAsync(employeeType);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _employeeTypeRepository.EmployeeTypeExistsAsync(id))
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
        public async Task<IActionResult> DeleteEmployeeType(int id)
        {
            var employeeType = await _employeeTypeRepository.GetEmployeeTypeByIdAsync(id);

            if (employeeType == null)
            {
                return NotFound();
            }

            await _employeeTypeRepository.DeleteEmployeeTypeAsync(employeeType);

            return NoContent();
        }
    }
}