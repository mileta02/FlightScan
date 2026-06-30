using AutoMapper;
using FlightScan.Application.Cqrs.Commands.User;
using FlightScan.Application.Errors;
using FlightScan.Application.Handlers.User;
using FlightScan.Core.Enums;
using FlightScan.Core.Interfaces;
using NSubstitute;
using Shouldly;

namespace FlightScan.Tests.User;

public class CreateUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly CreateUserHandler _handler;
    private readonly CreateUserCommandValidator _validator;

    public CreateUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _userRepository.GetByUsernameAsync(Arg.Any<string>()).Returns((Core.Entities.User?)null);
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateUserHandler(_unitOfWork, _userRepository, _mapper);
        _validator = new CreateUserCommandValidator(_userRepository);
    }

    [Fact]
    public async Task WhenRoleIsAdministrator_ShouldFailValidationWithCorrectMessage()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Role = UserRole.Administrator,
            Username = "validusername",
            Password = "validpassword"
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors
            .Select(e => e.ErrorMessage)
            .ShouldContain(ValidationMessages.administratorRoleInvalid);
    }

    [Fact]
    public async Task WhenCommandIsValidWithAgent_ShouldCreateUserAndSaveChanges()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Role = UserRole.Agent,
            Username = "validagent",
            Password = "validagentpassword"
        };
        var mappedUser = new Core.Entities.User { Username = command.Username, Role = command.Role };
        _mapper.Map<Core.Entities.User>(command).Returns(mappedUser);
        _unitOfWork.SaveChangesAsync().Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _userRepository.Received(1).CreateAsync(
            Arg.Is<Core.Entities.User>(u =>
                u.Username == command.Username &&
                u.Role == command.Role));
        await _unitOfWork.Received(1).SaveChangesAsync();
        result.Id.ShouldBe(mappedUser.Id);
        BCrypt.Net.BCrypt.Verify(command.Password, mappedUser.Password).ShouldBeTrue();
    }

    [Fact]
    public async Task WhenCommandIsValidWithVisitor_ShouldCreateUserAndSaveChanges()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Role = UserRole.Visitor,
            Username = "validvisitor",
            Password = "validvisitorpassword"
        };
        var mappedUser = new Core.Entities.User { Username = command.Username, Role = command.Role };
        _mapper.Map<Core.Entities.User>(command).Returns(mappedUser);
        _unitOfWork.SaveChangesAsync().Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _userRepository.Received(1).CreateAsync(
            Arg.Is<Core.Entities.User>(u =>
                u.Username == command.Username &&
                u.Role == command.Role));
        await _unitOfWork.Received(1).SaveChangesAsync();
        result.Id.ShouldBe(mappedUser.Id);
        BCrypt.Net.BCrypt.Verify(command.Password, mappedUser.Password).ShouldBeTrue();
    }

}
