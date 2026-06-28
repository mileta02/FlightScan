using FlightScan.Core.Helpers;
using FlightScan.Core.Responses.Flights;
using FlightScan.Core.Specifications;
using MediatR;

namespace FlightScan.Application.Cqrs.Queries.Flight
{
    public class GetFlightsQuery : IRequest<Pagination<FlightResponse>>
    {
        public FlightSpecParams Params { get; set; } = new();
    }
}
