using ClockWise.Api.Models;

namespace ClockWise.Api.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ClockWiseDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Companies.Any()) return;

            var company = new Company 
            { 
                Name = "CrediResolve",
                PublicShortName = "crvl"            
            };

            context.Companies.Add(company);

            var employee = new Employee
            {
                Name = "Mónica Silva",
                Email = "monica.silva@crediresolve.pt",
                PasswordHash = "1234",
                Company = company
            };
            context.Employees.Add(employee);

            context.SaveChanges();
        }
    }
}