using FlightScan.Application.Cqrs.Commands.Flight;
using FlightScan.Application.Handlers.Flight;
using FlightScan.Core.Enums;
using FlightScan.Core.Exceptions;
using FlightScan.Core.Interfaces;
using NSubstitute;
using Shouldly;

namespace FlightScan.Tests.Flight;

public class CancelFlightHandlerTests
{
    private readonly IFlightRepository _flightRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CancelFlightHandler _handler;

    public CancelFlightHandlerTests()
    {
        _flightRepository = Substitute.For<IFlightRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CancelFlightHandler(_flightRepository, _unitOfWork);
    }

    [Fact]
    public async Task WhenFlightIsAlreadyCancelled_ShouldThrowBadRequestException()
    {
        // Arrange
        var flight = new Core.Entities.Flight
        {
            Id = 1,
            WhereFrom = City.Beograd,
            WhereTo = City.Nis,
            DepartureDate = DateTime.UtcNow.AddDays(10),
            IsCancelled = true
        };
        _flightRepository.GetByIdAsync(flight.Id).Returns(flight);
        var command = new CancelFlightCommand { Id = flight.Id };

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.ShouldThrowAsync<BadRequestException>();
        ex.Message.ShouldBe("Flight is already cancelled.");
    }

    [Fact]
    public async Task WhenFlightIsActive_ShouldSetIsCancelledToTrue()
    {
        // Arrange
        var flight = new Core.Entities.Flight
        {
            Id = 1,
            WhereFrom = City.Beograd,
            WhereTo = City.Nis,
            DepartureDate = DateTime.UtcNow.AddDays(10),
            IsCancelled = false
        };
        _flightRepository.GetByIdAsync(flight.Id).Returns(flight);
        _unitOfWork.SaveChangesAsync().Returns(1);
        var command = new CancelFlightCommand { Id = flight.Id };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        flight.IsCancelled.ShouldBeTrue();
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

}
