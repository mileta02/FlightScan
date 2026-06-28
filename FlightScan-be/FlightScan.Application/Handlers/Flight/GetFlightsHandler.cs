using AutoMapper;
using FlightScan.Application.Cqrs.Queries.Flight;
using FlightScan.Core.Helpers;
using FlightScan.Core.Interfaces;
using FlightScan.Core.Responses.Flights;
using MediatR;

namespace FlightScan.Application.Handlers.Flight
{
    public class GetFlightsHandler : IRequestHandler<GetFlightsQuery, Pagination<FlightResponse>>
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;

        public GetFlightsHandler(IFlightRepository flightRepository, IMapper mapper)
        {
            _flightRepository = flightRepository;
            _mapper = mapper;
        }

        public async Task<Pagination<FlightResponse>> Handle(GetFlightsQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _flightRepository.GetAllAsync(request.Params);
            var mapped = _mapper.Map<List<FlightResponse>>(items);

            return new Pagination<FlightResponse>(request.Params.PageIndex, request.Params.PageSize, totalCount, mapped);
        }
    }
}
