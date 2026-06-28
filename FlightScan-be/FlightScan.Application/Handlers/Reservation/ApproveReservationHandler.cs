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

        public ApproveReservationHandler(IReservationRepository reservationRepository, IUnitOfWork unitOfWork)
        {
            _reservationRepository = reservationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ApproveReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _reservationRepository.GetByIdAsync(request.Id);

            if (reservation!.Status == ReservationStatus.Accepted)
                throw new BadRequestException("Reservations is already accepted.");

            reservation.Status = ReservationStatus.Accepted;
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
