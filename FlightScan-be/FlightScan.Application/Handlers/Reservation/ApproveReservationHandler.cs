using FlightScan.Application.Cqrs.Commands.Reservation;
using FlightScan.Core.Enums;
using FlightScan.Core.Exceptions;
using FlightScan.Core.Interfaces;
using MediatR;

namespace FlightScan.Application.Handlers.Reservation
{
    public class ApproveReservationHandler : IRequestHandler<ApproveReservationCommand, Unit>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReservationNotificationService _notifications;

        public ApproveReservationHandler(IReservationRepository reservationRepository, IUnitOfWork unitOfWork, IReservationNotificationService notifications)
        {
            _reservationRepository = reservationRepository;
            _unitOfWork = unitOfWork;
            _notifications = notifications;
        }

        public async Task<Unit> Handle(ApproveReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _reservationRepository.GetByIdAsync(request.Id);

            if (reservation!.Status == ReservationStatus.Accepted)
                throw new BadRequestException("Reservations is already accepted.");

            reservation.Status = ReservationStatus.Accepted;
            await _unitOfWork.SaveChangesAsync();

            await _notifications.NotifyReservationApprovedAsync(reservation.Id, reservation.UserId);

            return Unit.Value;
        }
    }
}
