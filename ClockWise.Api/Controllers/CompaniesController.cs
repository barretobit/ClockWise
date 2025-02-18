using AutoMapper;
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
        private readonly IMapper _mapper;

        public CompaniesController(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _companyRepository.GetAllEnabledCompaniesAsync();

            if (companies == null || companies.Count == 0)
            {
                return NotFound("No Companies found");
            }

            var companyDtos = _mapper.Map<List<CompanyDto>>(companies);

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

            var companyDto = _mapper.Map<CompanyDto>(company);

            return Ok(companyDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(CompanyDto createCompanyDto)
        {
            var company = _mapper.Map<Company>(createCompanyDto);
            company.IsEnabled = true;

            await _companyRepository.CreateCompanyAsync(company);

            var companyDto = _mapper.Map<CompanyDto>(company);

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

            _mapper.Map(updateCompanyDto, company);

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