using AutoMapper;
using FlightScan.Application.Cqrs.Commands.User;
using FlightScan.Core.Entities;
using FlightScan.Core.Responses.Flights;

namespace FlightScan.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<CreateUserCommand, User>();

            // Flight
            CreateMap<Flight, FlightResponse>()
                .ForMember(dest => dest.WhereFrom, opt => opt.MapFrom(src => src.WhereFrom.ToString()))
                .ForMember(dest => dest.WhereTo, opt => opt.MapFrom(src => src.WhereTo.ToString()))
                .ForMember(dest => dest.IsLowAvailability, opt => opt.MapFrom(src => src.AvailableSeats < 5));
        }
    }
}
