using FlightScan.Application.Errors;
using FlightScan.Core.Interfaces;
using FlightScan.Core.Responses.Common;
using FluentValidation;
using MediatR;

namespace FlightScan.Application.Cqrs.Commands.Reservation
{
    public class CreateReservationCommand : IRequest<CreateResponse>
    {
        public int FlightId { get; set; }
        public int SeatsCount { get; set; }
        public int UserId { get; set; }
    }

    public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
    {
        private readonly IFlightRepository _flightRepository;

        public CreateReservationCommandValidator(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;

            RuleFor(x => x.FlightId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage(ValidationMessages.flightIdInvalid)
                .MustAsync(async (id, ct) => await _flightRepository.GetByIdAsync(id) != null)
                .WithMessage(ValidationMessages.flightNotFound);

            RuleFor(x => x.SeatsCount)
                .GreaterThan(0).WithMessage(ValidationMessages.seatsCountInvalid);
        }
    }
}
