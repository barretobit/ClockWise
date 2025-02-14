using ClockWise.Api.Data;
using ClockWise.Api.DTOs;
using ClockWise.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClockWise.Api.Controllers
{
    [ApiController]
    [Route("api/employeeTypes")]
    public class EmployeeTypesController : ControllerBase
    {
        private readonly ClockWiseDbContext _context;

        public EmployeeTypesController(ClockWiseDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeTypeDto>>> GetAllEmployeeTypes()
        {
            var employeeTypes = await _context.EmployeeTypes.Where(e => e.IsEnabled == true).ToListAsync();

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
            var employeeType = await _context.EmployeeTypes.FindAsync(id);

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

            _context.EmployeeTypes.Add(employeeType);
            await _context.SaveChangesAsync();

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
            var employeeType = await _context.EmployeeTypes.FindAsync(id);

            if (employeeType == null)
            {
                return NotFound();
            }

            employeeType.TypeName = updateEmployeeTypeDto.TypeName;
            employeeType.IsEnabled = updateEmployeeTypeDto.IsEnabled;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeTypeExists(id))
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
            var employeeType = await _context.EmployeeTypes.FindAsync(id);

            if (employeeType == null)
            {
                return NotFound();
            }

            employeeType.IsEnabled = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeTypeExists(id))
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

        private bool EmployeeTypeExists(int id)
        {
            return _context.EmployeeTypes.Any(e => e.Id == id);
        }
    }
}