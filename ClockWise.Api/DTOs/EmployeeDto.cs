namespace ClockWise.Api.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int CompanyId { get; set; }
        public int EmployeeTypeId { get; set; }
        public bool IsEnabled { get; set; }
    }
}