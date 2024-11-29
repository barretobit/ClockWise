namespace ClockWise.Api.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
