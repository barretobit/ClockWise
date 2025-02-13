using ClockWise.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ClockWise.Api.Data
{
    public class ClockWiseDbContext(DbContextOptions<ClockWiseDbContext> options) : DbContext(options)
    {
        public required DbSet<Company> Companies { get; set; }
        public required DbSet<Employee> Employees { get; set; }
        public required DbSet<EmployeeType> EmployeeTypes { get; set; }
        public required DbSet<WorkSession> WorkSessions { get; set; }
    }
}
