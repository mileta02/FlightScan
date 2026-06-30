using FlightScan.Application.Cqrs.Commands.Auth;
using FlightScan.Application.Handlers.Auth;
using FlightScan.Core.Enums;
using FlightScan.Core.Exceptions;
using FlightScan.Core.Interfaces;
using NSubstitute;
using Shouldly;

namespace FlightScan.Tests.Auth;

public class LoginHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly LoginHandler _handler;

    public LoginHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _jwtService = Substitute.For<IJwtService>();

        _handler = new LoginHandler(_userRepository, _jwtService);
    }

    [Fact]
    public async Task WhenUserDoesNotExist_ShouldThrowUnauthorizedException()
    {
        // Arrange
        _userRepository.GetByUsernameAsync("unknown").Returns((Core.Entities.User?)null);
        var command = new LoginCommand { Username = "unknownuser", Password = "unknownpassword" };

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.ShouldThrowAsync<UnauthorizedException>();
        ex.Message.ShouldBe("Invalid username or password.");
    }

    [Fact]
    public async Task WhenPasswordIsWrong_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var user = new Core.Entities.User
        {
            Id = 1,
            Username = "testagent",
            Role = UserRole.Agent,
            Password = BCrypt.Net.BCrypt.HashPassword("correctpassword")
        };
        _userRepository.GetByUsernameAsync(user.Username).Returns(user);
        var command = new LoginCommand { Username = user.Username, Password = "wrongpassword" };

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.ShouldThrowAsync<UnauthorizedException>();
        ex.Message.ShouldBe("Invalid username or password.");
    }

    [Fact]
    public async Task WhenCredentialsAreValid_ShouldReturnLoginResponseWithToken()
    {
        // Arrange
        const string password = "validpassword";
        const string expectedToken = "generated-jwt-token";
        var user = new Core.Entities.User
        {
            Id = 1,
            Username = "testagent",
            Role = UserRole.Agent,
            Password = BCrypt.Net.BCrypt.HashPassword(password)
        };
        _userRepository.GetByUsernameAsync(user.Username).Returns(user);
        _jwtService.GenerateToken(user).Returns(expectedToken);
        var command = new LoginCommand { Username = user.Username, Password = password };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Token.ShouldBe(expectedToken);
        result.Username.ShouldBe(user.Username);
        result.Role.ShouldBe(user.Role);
    }

}
