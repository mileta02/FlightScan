using AutoMapper;
using FlightScan.Application.Cqrs.Commands.Flight;
using FlightScan.Application.Cqrs.Commands.User;
using FlightScan.Core.Entities;
using FlightScan.Core.Responses.Flights;

namespace FlightScan.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User
            CreateMap<CreateUserCommand, User>();
            #endregion

            #region Flight
            CreateMap<CreateFlightCommand, Flight>()
                .ForMember(dest => dest.AvailableSeats, opt => opt.MapFrom(src => src.TotalSeats))
                .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(_ => false));

            CreateMap<Flight, FlightResponse>()
                .ForMember(dest => dest.WhereFrom, opt => opt.MapFrom(src => src.WhereFrom.ToString()))
                .ForMember(dest => dest.WhereTo, opt => opt.MapFrom(src => src.WhereTo.ToString()))
                .ForMember(dest => dest.IsLowAvailability, opt => opt.MapFrom(src => src.AvailableSeats < 5));
            #endregion
        }
    }
}
