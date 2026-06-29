using AutoMapper;
using FlightScan.Application.Cqrs.Queries.User;
using FlightScan.Core.Helpers;
using FlightScan.Core.Interfaces;
using FlightScan.Core.Responses.User;
using MediatR;

namespace FlightScan.Application.Handlers.User
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, Pagination<UserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetUsersHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Pagination<UserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _userRepository.GetAllAsync(request.SpecParams);
            var mapped = _mapper.Map<List<UserResponse>>(items);

            return new Pagination<UserResponse>(request.SpecParams.PageIndex, request.SpecParams.PageSize, totalCount, mapped);
        }
    }
}
