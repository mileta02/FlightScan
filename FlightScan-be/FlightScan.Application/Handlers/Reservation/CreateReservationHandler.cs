using FlightScan.Application.Cqrs.Commands.Reservation;
using FlightScan.Core.Enums;
using FlightScan.Core.Exceptions;
using FlightScan.Core.Interfaces;
using FlightScan.Core.Responses.Common;
using FlightScan.Core.Responses.Reservations;
using MediatR;

namespace FlightScan.Application.Handlers.Reservation
{
    public class CreateReservationHandler : IRequestHandler<CreateReservationCommand, CreateResponse>
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReservationNotificationService _notifications;

        public CreateReservationHandler(IFlightRepository flightRepository, IReservationRepository reservationRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IReservationNotificationService notifications)
        {
            _flightRepository = flightRepository;
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _notifications = notifications;
        }

        public async Task<CreateResponse> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var flight = await _flightRepository.GetByIdAsync(request.FlightId);

            if (flight!.IsCancelled)
                throw new BadRequestException("Cannot reserve a cancelled flight.");

            if (flight.DepartureDate.Date < DateTime.UtcNow.Date.AddDays(3))
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

            var user = await _userRepository.GetByIdAsync(request.UserId);
            await _notifications.NotifyNewReservationAsync(new PendingReservationResponse
            {
                Id = reservation.Id,
                FlightId = flight.Id,
                Username = user!.Username,
                WhereFrom = flight.WhereFrom.ToString(),
                WhereTo = flight.WhereTo.ToString(),
                DepartureDate = flight.DepartureDate,
                ReservedSeats = request.SeatsCount,
                Status = ReservationStatus.Pending.ToString(),
                CreatedAt = reservation.CreatedAt
            });

            return new CreateResponse { Id = reservation.Id };
        }
    }
}
