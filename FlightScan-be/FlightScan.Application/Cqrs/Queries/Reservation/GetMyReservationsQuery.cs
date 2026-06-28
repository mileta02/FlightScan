using FlightScan.Core.Helpers;
using FlightScan.Core.Responses.Reservations;
using FlightScan.Core.Specifications;
using MediatR;

namespace FlightScan.Application.Cqrs.Queries.Reservation
{
    public class GetMyReservationsQuery : IRequest<Pagination<ReservationResponse>>
    {
        public int UserId { get; set; }
        public ReservationSpecParams Params { get; set; } = new();
    }
}
