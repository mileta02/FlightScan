using AutoMapper;
using FlightScan.Application.Cqrs.Commands.User;
using FlightScan.Core.Interfaces;
using FlightScan.Core.Responses.Common;
using MediatR;

namespace FlightScan.Application.Handlers.User
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateUserHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<CreateResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<Core.Entities.User>(request);
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            await _userRepository.CreateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new CreateResponse { Id = user.Id };
        }
    }
}
