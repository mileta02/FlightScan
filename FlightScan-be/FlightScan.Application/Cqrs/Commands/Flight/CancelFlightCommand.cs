using FlightScan.Application.Errors;
using FlightScan.Core.Interfaces;
using FluentValidation;
using MediatR;

namespace FlightScan.Application.Cqrs.Commands.Flight
{
    public class CancelFlightCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class CancelFlightCommandValidator : AbstractValidator<CancelFlightCommand>
    {
        private readonly IFlightRepository _flightRepository;

        public CancelFlightCommandValidator(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;

            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage(ValidationMessages.flightIdInvalid)
                .MustAsync(async (id, ct) => await _flightRepository.GetByIdAsync(id) != null)
                .WithMessage(ValidationMessages.flightNotFound);
        }
    }
}
