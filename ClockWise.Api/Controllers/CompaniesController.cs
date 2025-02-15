using ClockWise.Api.DTOs;
using ClockWise.Api.Models;
using ClockWise.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClockWise.Api.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;

        public CompaniesController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _companyRepository.GetAllEnabledCompaniesAsync();

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
            var company = await _companyRepository.GetCompanyByIdAsync(id);

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

            await _companyRepository.CreateCompanyAsync(company);

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
            var company = await _companyRepository.GetCompanyByIdAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            company.Name = updateCompanyDto.Name;
            company.PublicShortName = updateCompanyDto.PublicShortName;
            company.IsEnabled = updateCompanyDto.IsEnabled;

            try
            {
                await _companyRepository.UpdateCompanyAsync(company);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _companyRepository.CompanyExistsAsync(id))
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
            var company = await _companyRepository.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            await _companyRepository.DeleteCompanyAsync(company);

            return NoContent();
        }
    }
}