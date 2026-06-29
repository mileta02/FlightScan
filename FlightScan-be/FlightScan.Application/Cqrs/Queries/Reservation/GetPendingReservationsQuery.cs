using FlightScan.Core.Helpers;
using FlightScan.Core.Responses.Reservations;
using FlightScan.Core.Specifications;
using MediatR;

namespace FlightScan.Application.Cqrs.Queries.Reservation
{
    public class GetPendingReservationsQuery : IRequest<Pagination<PendingReservationResponse>>
    {
        public ReservationSpecParams Params { get; set; } = new();
    }
}
