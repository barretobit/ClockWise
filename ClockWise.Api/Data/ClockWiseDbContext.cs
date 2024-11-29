using ClockWise.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ClockWise.Api.Data
{
    public class ClockWiseDbContext : DbContext
    {
        public ClockWiseDbContext(DbContextOptions<ClockWiseDbContext> options) : base(options) { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<WorkSession> WorkSessions { get; set; }
    }
}
