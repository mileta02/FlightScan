using AutoMapper;
using FlightScan.Application.Cqrs.Commands.Flight;
using FlightScan.Core.Interfaces;
using FlightScan.Core.Responses.Common;
using MediatR;

namespace FlightScan.Application.Handlers.Flight
{
    public class CreateFlightHandler : IRequestHandler<CreateFlightCommand, CreateResponse>
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateFlightHandler(IFlightRepository flightRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _flightRepository = flightRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateResponse> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
        {
            var flight = _mapper.Map<Core.Entities.Flight>(request);

            await _flightRepository.CreateAsync(flight);
            await _unitOfWork.SaveChangesAsync();

            return new CreateResponse { Id = flight.Id };
        }
    }
}
