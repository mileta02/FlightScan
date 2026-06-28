using AutoMapper;
using FlightScan.Application.Cqrs.Queries.Reservation;
using FlightScan.Core.Helpers;
using FlightScan.Core.Interfaces;
using FlightScan.Core.Responses.Reservations;
using MediatR;

namespace FlightScan.Application.Handlers.Reservation
{
    public class GetMyReservationsHandler : IRequestHandler<GetMyReservationsQuery, Pagination<ReservationResponse>>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;

        public GetMyReservationsHandler(IReservationRepository reservationRepository, IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
        }

        public async Task<Pagination<ReservationResponse>> Handle(GetMyReservationsQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _reservationRepository.GetByUserIdAsync(request.UserId, request.Params.PageIndex, request.Params.PageSize);
            var mapped = _mapper.Map<List<ReservationResponse>>(items);
            return new Pagination<ReservationResponse>(request.Params.PageIndex, request.Params.PageSize, totalCount, mapped);
        }
    }
}
