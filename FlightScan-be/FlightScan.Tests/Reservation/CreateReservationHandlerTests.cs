using FlightScan.Application.Cqrs.Commands.Reservation;
using FlightScan.Application.Handlers.Reservation;
using FlightScan.Core.Enums;
using FlightScan.Core.Exceptions;
using FlightScan.Core.Interfaces;
using FlightScan.Core.Responses.Reservations;
using NSubstitute;
using Shouldly;

namespace FlightScan.Tests.Reservation;

public class CreateReservationHandlerTests
{
    private readonly IFlightRepository _flightRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReservationNotificationService _notifications;
    private readonly CreateReservationHandler _handler;

    public CreateReservationHandlerTests()
    {
        _flightRepository = Substitute.For<IFlightRepository>();
        _reservationRepository = Substitute.For<IReservationRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _notifications = Substitute.For<IReservationNotificationService>();

        _handler = new CreateReservationHandler(
            _flightRepository,
            _reservationRepository,
            _userRepository,
            _unitOfWork,
            _notifications);
    }

    [Fact]
    public async Task WhenFlightIsCancelled_ShouldThrowBadRequestException()
    {
        // Arrange
        var flight = BuildFlight(isCancelled: true);
        _flightRepository.GetByIdAsync(flight.Id).Returns(flight);
        var command = new CreateReservationCommand { FlightId = flight.Id, SeatsCount = 2, UserId = 1 };

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.ShouldThrowAsync<BadRequestException>();
        ex.Message.ShouldBe("Cannot reserve a cancelled flight.");
    }

    [Fact]
    public async Task WhenFlightDepartsInLessThan3Days_ShouldThrowBadRequestException()
    {
        // Arrange
        var flight = BuildFlight(departureDate: DateTime.UtcNow.AddDays(2));
        _flightRepository.GetByIdAsync(flight.Id).Returns(flight);
        var command = new CreateReservationCommand { FlightId = flight.Id, SeatsCount = 2, UserId = 1 };

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.ShouldThrowAsync<BadRequestException>();
        ex.Message.ShouldBe("Cannot reserve a flight departing in less than 3 days.");
    }

    [Fact]
    public async Task WhenAvailableSeatsAreLessThanRequested_ShouldThrowBadRequestException()
    {
        // Arrange
        var flight = BuildFlight(availableSeats: 1, departureDate: DateTime.UtcNow.AddDays(10));
        _flightRepository.GetByIdAsync(flight.Id).Returns(flight);
        var command = new CreateReservationCommand { FlightId = flight.Id, SeatsCount = 3, UserId = 1 };

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.ShouldThrowAsync<BadRequestException>();
        ex.Message.ShouldBe("Not enough available seats.");
    }

    [Fact]
    public async Task WhenReservationIsSuccessful_ShouldDecreaseAvailableSeats()
    {
        // Arrange
        var flight = BuildFlight(availableSeats: 10, departureDate: DateTime.UtcNow.AddDays(10));
        var user = new Core.Entities.User { Id = 1, Username = "testvisitor", Role = UserRole.Visitor };
        _flightRepository.GetByIdAsync(flight.Id).Returns(flight);
        _userRepository.GetByIdAsync(user.Id).Returns(user);
        _unitOfWork.SaveChangesAsync().Returns(1);
        var command = new CreateReservationCommand { FlightId = flight.Id, SeatsCount = 3, UserId = user.Id };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        flight.AvailableSeats.ShouldBe(7);
    }

    [Fact]
    public async Task WhenReservationIsSuccessful_ShouldCreateReservationWithPendingStatus()
    {
        // Arrange
        var flight = BuildFlight(availableSeats: 10, departureDate: DateTime.UtcNow.AddDays(10));
        var user = new Core.Entities.User { Id = 1, Username = "testvisitor", Role = UserRole.Visitor };
        _flightRepository.GetByIdAsync(flight.Id).Returns(flight);
        _userRepository.GetByIdAsync(user.Id).Returns(user);
        _unitOfWork.SaveChangesAsync().Returns(1);
        var command = new CreateReservationCommand { FlightId = flight.Id, SeatsCount = 3, UserId = user.Id };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _reservationRepository.Received(1).CreateAsync(
            Arg.Is<Core.Entities.Reservation>(r =>
                r.Status == ReservationStatus.Pending &&
                r.ReservedSeats == 3 &&
                r.FlightId == flight.Id &&
                r.UserId == user.Id));
    }

    [Fact]
    public async Task WhenReservationIsSuccessful_ShouldSendNotificationWithCorrectData()
    {
        // Arrange
        var flight = BuildFlight(availableSeats: 10, departureDate: DateTime.UtcNow.AddDays(10));
        var user = new Core.Entities.User { Id = 1, Username = "testvisitor", Role = UserRole.Visitor };
        _flightRepository.GetByIdAsync(flight.Id).Returns(flight);
        _userRepository.GetByIdAsync(user.Id).Returns(user);
        _unitOfWork.SaveChangesAsync().Returns(1);
        var command = new CreateReservationCommand { FlightId = flight.Id, SeatsCount = 3, UserId = user.Id };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _notifications.Received(1).NotifyNewReservationAsync(
            Arg.Is<PendingReservationResponse>(n =>
                n.Username == user.Username &&
                n.FlightId == flight.Id &&
                n.ReservedSeats == 3 &&
                n.Status == ReservationStatus.Pending.ToString()));
    }

    #region HelperMethods
    private static Core.Entities.Flight BuildFlight(int availableSeats = 10, DateTime? departureDate = null, bool isCancelled = false)
    {
        return new Core.Entities.Flight
        {
            Id = 1,
            WhereFrom = City.Beograd,
            WhereTo = City.Nis,
            TotalSeats = 10,
            AvailableSeats = availableSeats,
            DepartureDate = departureDate ?? DateTime.UtcNow.AddDays(10),
            IsCancelled = isCancelled
        };
    }
    #endregion
}
