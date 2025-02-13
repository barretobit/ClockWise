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
                Id = 1,
                Name = "CrediResolve",
                PublicShortName = "crvl",
                IsEnabled = true
            };

            context.Companies.Add(company);

            var employeeType = new EmployeeType
            {
                Id = 1,
                CompanyId = company.Id,
                TypeName = "Manager"
            };
            context.EmployeeTypes.Add(employeeType);

            var employee = new Employee
            {
                Name = "Mónica Silva",
                Email = "monica.silva@crediresolve.pt",
                PasswordHash = "1234",
                CompanyId = company.Id,
                EmployeeTypeId = employeeType.Id,
                IsEnabled = true
            };
            context.Employees.Add(employee);

            context.SaveChanges();
        }
    }
}