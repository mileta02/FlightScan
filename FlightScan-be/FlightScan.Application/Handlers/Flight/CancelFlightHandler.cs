using FlightScan.Application.Cqrs.Commands.Flight;
using FlightScan.Core.Exceptions;
using FlightScan.Core.Interfaces;
using MediatR;

namespace FlightScan.Application.Handlers.Flight
{
    public class CancelFlightHandler : IRequestHandler<CancelFlightCommand, Unit>
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelFlightHandler(IFlightRepository flightRepository, IUnitOfWork unitOfWork)
        {
            _flightRepository = flightRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CancelFlightCommand request, CancellationToken cancellationToken)
        {
            var flight = await _flightRepository.GetByIdAsync(request.Id);

            if (flight!.IsCancelled)
                throw new BadRequestException("Flight is already cancelled.");

            flight.IsCancelled = true;
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
