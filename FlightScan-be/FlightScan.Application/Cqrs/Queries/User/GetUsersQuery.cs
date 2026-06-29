using FlightScan.Core.Helpers;
using FlightScan.Core.Responses.User;
using FlightScan.Core.Specifications;
using MediatR;

namespace FlightScan.Application.Cqrs.Queries.User
{
    public class GetUsersQuery : IRequest<Pagination<UserResponse>>
    {
        public UserSpecParams SpecParams {  get; set; }
    }
}
