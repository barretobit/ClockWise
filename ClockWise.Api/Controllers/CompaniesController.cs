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
        private readonly ILogger<CompaniesController> _logger;

        public CompaniesController(ICompanyRepository companyRepository, IMapper mapper, ILogger<CompaniesController> logger)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            try
            {
                var companies = await _companyRepository.GetAllEnabledCompaniesAsync();

                if (companies == null || companies.Count == 0)
                {
                    _logger.LogWarning("No enabled companies found in the database.");
                    return NotFound("No Companies found");
                }

                var companyDtos = _mapper.Map<List<CompanyDto>>(companies);

                return Ok(companyDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving companies.");
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompanyById(int id)
        {
            try
            {
                var company = await _companyRepository.GetCompanyByIdAsync(id);

                if (company == null)
                {
                    return NotFound("Company not found");
                }

                var companyDto = _mapper.Map<CompanyDto>(company);

                return Ok(companyDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving company -> {id}");
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(CompanyDto createCompanyDto)
        {
            try
            {
                var company = _mapper.Map<Company>(createCompanyDto);
                company.IsEnabled = true;

                await _companyRepository.CreateCompanyAsync(company);

                var companyDto = _mapper.Map<CompanyDto>(company);

                return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, companyDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating company.");
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyDto updateCompanyDto)
        {
            try
            {
                var company = await _companyRepository.GetCompanyByIdAsync(id);

                if (company == null)
                {
                    return NotFound();
                }

                _mapper.Map(updateCompanyDto, company);

                await _companyRepository.UpdateCompanyAsync(company);
            }
            catch (DbUpdateConcurrencyException dbException)
            {
                if (!await _companyRepository.CompanyExistsAsync(id))
                {
                    _logger.LogError(dbException, $"An error occurred while updating company -> {updateCompanyDto.Name}");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating company -> {updateCompanyDto.Name}");
                return StatusCode(500, "Internal Server Error.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            try
            {
                var company = await _companyRepository.GetCompanyByIdAsync(id);

                if (company == null)
                {
                    return NotFound();
                }

                await _companyRepository.DeleteCompanyAsync(company);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting company -> {id}");
                return StatusCode(500, "Internal Server Error.");
            }
        }
    }
}