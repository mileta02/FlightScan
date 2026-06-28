using MediatR;
using FlightScan.Application.Cqrs.Commands.Auth;
using FlightScan.Core.Responses.Auth;
using FlightScan.Core.Interfaces;
using FlightScan.Core.Exceptions;

namespace FlightScan.Application.Handlers.Auth
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public LoginHandler(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username!);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password!, user.Password))
                throw new UnauthorizedException("Invalid username or password.");

            var token = _jwtService.GenerateToken(user);

            return new LoginResponse { Username = user.Username, Role = user.Role, Token = token };
        }
    }
}