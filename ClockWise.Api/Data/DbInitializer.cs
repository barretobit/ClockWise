using ClockWise.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ClockWise.Api.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ClockWiseDbContext context)
        {
            context.Database.EnsureCreated();

            context.Companies.ExecuteDelete();
            context.EmployeeTypes.ExecuteDelete();
            context.Employees.ExecuteDelete();

            var company = new Company
            {
                Name = "CrediResolve",
                PublicShortName = "crvl",
                IsEnabled = true
            };

            context.Companies.Add(company);
            context.SaveChanges();

            var firstCompany = context.Companies.First();

            var employeeType1 = new EmployeeType
            {
                CompanyId = firstCompany.Id,
                TypeName = "Manager",
                IsEnabled = true
            };
            var employeeType2 = new EmployeeType
            {
                CompanyId = firstCompany.Id,
                TypeName = "Developer",
                IsEnabled = true
            };

            context.EmployeeTypes.Add(employeeType1);
            context.EmployeeTypes.Add(employeeType2);
            context.SaveChanges();

            var firstEmployeeType = context.EmployeeTypes.First();
            var secondEmployeeType = context.EmployeeTypes.Where(x => x.TypeName.Contains("Developer")).First();

            var employee1 = new Employee
            {
                Name = "Mónica Silva",
                Email = "monica.silva@crediresolve.pt",
                PasswordHash = "1234",
                CompanyId = firstCompany.Id,
                EmployeeTypeId = firstEmployeeType.Id,
                IsEnabled = true
            };

            var employee2 = new Employee
            {
                Name = "João Barreto",
                Email = "joao.barreto@crediresolve.pt",
                PasswordHash = "4321",
                CompanyId = firstCompany.Id,
                EmployeeTypeId = secondEmployeeType.Id,
                IsEnabled = true
            };

            context.Employees.Add(employee1);
            context.Employees.Add(employee2);
            context.SaveChanges();
        }
    }
}