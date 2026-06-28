using FlightScan.Application.Cqrs.Commands.Reservation;
using FlightScan.Core.Enums;
using FlightScan.Core.Exceptions;
using FlightScan.Core.Interfaces;
using FlightScan.Core.Responses.Common;
using MediatR;

namespace FlightScan.Application.Handlers.Reservation
{
    public class CreateReservationHandler : IRequestHandler<CreateReservationCommand, CreateResponse>
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateReservationHandler(IFlightRepository flightRepository, IReservationRepository reservationRepository, IUnitOfWork unitOfWork)
        {
            _flightRepository = flightRepository;
            _reservationRepository = reservationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateResponse> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var flight = await _flightRepository.GetByIdAsync(request.FlightId);

            if (flight!.IsCancelled)
                throw new BadRequestException("Cannot reserve a cancelled flight.");

            if (flight.DepartureDate <= DateTime.UtcNow.AddDays(3))
                throw new BadRequestException("Cannot reserve a flight departing in less than 3 days.");

            if (flight.AvailableSeats < request.SeatsCount)
                throw new BadRequestException("Not enough available seats.");

            flight.AvailableSeats -= request.SeatsCount;

            var reservation = new Core.Entities.Reservation
            {
                FlightId = request.FlightId,
                UserId = request.UserId,
                ReservedSeats = request.SeatsCount,
                Status = ReservationStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _reservationRepository.CreateAsync(reservation);
            await _unitOfWork.SaveChangesAsync();

            return new CreateResponse { Id = reservation.Id };
        }
    }
}
