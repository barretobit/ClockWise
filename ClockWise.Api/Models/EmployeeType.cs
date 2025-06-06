﻿namespace ClockWise.Api.Models
{
    public class EmployeeType
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string TypeName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}