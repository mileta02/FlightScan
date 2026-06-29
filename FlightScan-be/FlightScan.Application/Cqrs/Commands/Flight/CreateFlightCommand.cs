using FlightScan.Application.Errors;
using FlightScan.Core.Enums;
using FlightScan.Core.Interfaces;
using FlightScan.Core.Responses.Common;
using FluentValidation;
using MediatR;

namespace FlightScan.Application.Cqrs.Commands.Flight
{
    public class CreateFlightCommand : IRequest<CreateResponse>
    {
        public City WhereFrom { get; set; }
        public City WhereTo { get; set; }
        public int Stops { get; set; }
        public DateTime DepartureDate { get; set; }
        public int TotalSeats { get; set; }
    }

    public class CreateFlightCommandValidator : AbstractValidator<CreateFlightCommand>
    {
        private readonly IFlightRepository _flightRepository;

        public CreateFlightCommandValidator(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;

            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.WhereFrom)
                .IsInEnum().WithMessage(ValidationMessages.cityInvalid);

            RuleFor(x => x.WhereTo)
                .IsInEnum().WithMessage(ValidationMessages.cityInvalid)
                .NotEqual(x => x.WhereFrom).WithMessage(ValidationMessages.sameCityInvalid);

            RuleFor(x => x.Stops)
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.stopsInvalid);

            RuleFor(x => x.DepartureDate)
                .GreaterThan(DateTime.UtcNow).WithMessage(ValidationMessages.departureDateInvalid);

            RuleFor(x => x.TotalSeats)
                .GreaterThan(0).WithMessage(ValidationMessages.totalSeatsInvalid);

            RuleFor(x => x)
                .MustAsync(async (cmd, ct) =>
                {
                    var exists = await _flightRepository.ExistsAsync(cmd.WhereFrom, cmd.WhereTo, cmd.DepartureDate);
                    return !exists;
                })
                .WithMessage(ValidationMessages.sameFlightExists);
        }
    }
}
