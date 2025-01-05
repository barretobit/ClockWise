using ClockWise.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClockWise.Api.Controllers
{
    [ApiController]
    [Route("{publicShortName}")]
    public class CompaniesController : ControllerBase
    {
        private readonly ClockWiseDbContext _context;

        public CompaniesController(ClockWiseDbContext context)
        {
            _context = context;
        }

        [HttpGet("index")]
        public async Task<IActionResult> GetLandingPage(string publicShortName)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.PublicShortName == publicShortName);

            if (company == null) return NotFound("Company not found");

            return Ok(new
            {
                company.Name,
                LandingPage = $"{Request.Scheme}://{Request.Host}/{publicShortName}/login"
            });
        }

        [HttpGet("login")]
        public IActionResult GetLoginPage(string publicShortName)
        {
            // Logic to serve the login page for the given company
            return Ok($"Login page for {publicShortName}");
        }
    }
}