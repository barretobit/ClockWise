using ClockWise.Api.Data;
using ClockWise.Api.DTOs;
using ClockWise.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClockWise.Api.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ClockWiseDbContext _context;

        public CompaniesController(ClockWiseDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _context.Companies.Where(c => c.IsEnabled == true).ToListAsync();

            if (companies == null || companies.Count == 0)
            {
                return NotFound("No Companies found");
            }

            var companyDtos = companies.Select(c => new CompanyDto
            {
                Id = c.Id,
                Name = c.Name,
                PublicShortName = c.PublicShortName,
                IsEnabled = c.IsEnabled
            }).ToList();

            return Ok(companyDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompanyById(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound("Company not found");
            }

            var companyDto = new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                PublicShortName = company.PublicShortName,
                IsEnabled = company.IsEnabled
            };

            return Ok(companyDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(CompanyDto createCompanyDto)
        {
            var company = new Company
            {
                Name = createCompanyDto.Name,
                PublicShortName = createCompanyDto.PublicShortName,
                IsEnabled = true
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            var companyDto = new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                PublicShortName = company.PublicShortName,
                IsEnabled = company.IsEnabled
            };

            return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, companyDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyDto updateCompanyDto)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            company.Name = updateCompanyDto.Name;
            company.PublicShortName = updateCompanyDto.PublicShortName;
            company.IsEnabled = updateCompanyDto.IsEnabled;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            company.IsEnabled = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}