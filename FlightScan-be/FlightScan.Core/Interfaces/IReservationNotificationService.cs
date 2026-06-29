using FlightScan.Core.Responses.Reservations;

namespace FlightScan.Core.Interfaces
{
    public interface IReservationNotificationService
    {
        Task NotifyNewReservationAsync(PendingReservationResponse reservation);
        Task NotifyReservationApprovedAsync(int reservationId, int visitorUserId);
    }
}
