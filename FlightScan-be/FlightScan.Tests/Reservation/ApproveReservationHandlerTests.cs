using FlightScan.Application.Cqrs.Commands.Reservation;
using FlightScan.Application.Handlers.Reservation;
using FlightScan.Core.Enums;
using FlightScan.Core.Exceptions;
using FlightScan.Core.Interfaces;
using NSubstitute;
using Shouldly;

namespace FlightScan.Tests.Reservation;

public class ApproveReservationHandlerTests
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReservationNotificationService _notifications;
    private readonly ApproveReservationHandler _handler;

    public ApproveReservationHandlerTests()
    {
        _reservationRepository = Substitute.For<IReservationRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _notifications = Substitute.For<IReservationNotificationService>();
        _handler = new ApproveReservationHandler(_reservationRepository, _unitOfWork, _notifications);
    }

    [Fact]
    public async Task WhenReservationIsAlreadyApproved_ShouldThrowBadRequestException()
    {
        // Arrange
        var reservation = new Core.Entities.Reservation
        {
            Id = 1,
            UserId = 1,
            Status = ReservationStatus.Accepted
        };
        _reservationRepository.GetByIdAsync(reservation.Id).Returns(reservation);
        var command = new ApproveReservationCommand { Id = reservation.Id };

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.ShouldThrowAsync<BadRequestException>();
        ex.Message.ShouldBe("Reservations is already accepted.");
    }

    [Fact]
    public async Task WhenReservationIsPending_ShouldSetStatusToAccepted()
    {
        // Arrange
        var reservation = new Core.Entities.Reservation
        {
            Id = 1,
            UserId = 1,
            Status = ReservationStatus.Pending
        };
        _reservationRepository.GetByIdAsync(reservation.Id).Returns(reservation);
        _unitOfWork.SaveChangesAsync().Returns(1);
        var command = new ApproveReservationCommand { Id = reservation.Id };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        reservation.Status.ShouldBe(ReservationStatus.Accepted);
        await _unitOfWork.Received(1).SaveChangesAsync();
        await _notifications.Received(1).NotifyReservationApprovedAsync(reservation.Id, reservation.UserId);
    }

}
