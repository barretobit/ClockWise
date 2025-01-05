﻿namespace ClockWise.Api.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PublicShortName { get; set; } = string.Empty; 
        public List<Employee> Employees { get; set; } = new();
    }
}