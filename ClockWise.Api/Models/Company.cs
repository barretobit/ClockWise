namespace ClockWise.Api.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PublicShortName { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }
}