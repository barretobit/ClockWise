namespace ClockWise.Api.Models
{
    public class EmployeeType
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string TypeName { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}