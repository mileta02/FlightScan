using FlightScan.Application.Errors;
using FlightScan.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace FlightScan.Application.Cqrs.Commands.Reservation
{
    public class ApproveReservationCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class ApproveReservationCommandValidator : AbstractValidator<ApproveReservationCommand>
    {
        private readonly IReservationRepository _reservationRepository;

        public ApproveReservationCommandValidator(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;

            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage(ValidationMessages.reservationIdInvalid)
                .MustAsync(async (id, ct) => await _reservationRepository.GetByIdAsync(id) != null)
                .WithMessage(ValidationMessages.reservationNotFound);
        }
    }
}
