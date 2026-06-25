using MediatR;
using FlightScan.Application.Cqrs.Commands.Auth;
using FlightScan.Core.Responses.Auth;
using FlightScan.Core.Interfaces;

namespace FlightScan.Application.Handlers.Auth
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        public LoginHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user =  await _userRepository.GetByUsernameAsync(request.Username!);

            if (user == null)
                throw new UnauthorizedAccessException("TestError");

            return new LoginResponse { Role = user.Role, Username = user.Username, Token = "TestToken" };
        }
    }
}