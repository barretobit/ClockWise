﻿namespace ClockWise.Api.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PublicShortName { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}