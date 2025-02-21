namespace ClockWise.Api.DTOs
{
    public class TickLogDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Tick { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
    }
}