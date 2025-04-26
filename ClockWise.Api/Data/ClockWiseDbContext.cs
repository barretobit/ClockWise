using ClockWise.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClockWise.Api.Data
{
    public class ClockWiseDbContext(DbContextOptions<ClockWiseDbContext> options) : DbContext(options)
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<TickLog> TickLogs { get; set; }


        // New Architecture

        public DbSet<User> Users { get; set; }
    }
}