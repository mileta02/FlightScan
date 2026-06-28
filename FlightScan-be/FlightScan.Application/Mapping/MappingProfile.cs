using AutoMapper;
using FlightScan.Application.Cqrs.Commands.User;
using FlightScan.Core.Entities;

namespace FlightScan.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<CreateUserCommand, User>();
        }
    }
}
