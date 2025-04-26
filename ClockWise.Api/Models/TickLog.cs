namespace ClockWise.Api.Models
{
    public class TickLog
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Tick { get; set; }
        public bool IsApproved { get; set; } = true;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}