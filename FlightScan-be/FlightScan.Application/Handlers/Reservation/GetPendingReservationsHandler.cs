using AutoMapper;
using FlightScan.Application.Cqrs.Queries.Reservation;
using FlightScan.Core.Helpers;
using FlightScan.Core.Interfaces;
using FlightScan.Core.Responses.Reservations;
using MediatR;

namespace FlightScan.Application.Handlers.Reservation
{
    public class GetPendingReservationsHandler : IRequestHandler<GetPendingReservationsQuery, Pagination<PendingReservationResponse>>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;

        public GetPendingReservationsHandler(IReservationRepository reservationRepository, IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
        }

        public async Task<Pagination<PendingReservationResponse>> Handle(GetPendingReservationsQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _reservationRepository.GetPendingAsync(request.Params);
            var mapped = _mapper.Map<List<PendingReservationResponse>>(items);
            return new Pagination<PendingReservationResponse>(request.Params.PageIndex, request.Params.PageSize, totalCount, mapped);
        }
    }
}
