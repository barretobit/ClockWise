using AutoMapper;
using ClockWise.Api.DTOs;

namespace ClockWise.Api.Models.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CompanyDto, Company>().ReverseMap();
            CreateMap<EmployeeDto, Employee>().ReverseMap();
            CreateMap<EmployeeTypeDto, EmployeeType>().ReverseMap();
            CreateMap<TickLogDto, TickLog>().ReverseMap();
        }
    }
}