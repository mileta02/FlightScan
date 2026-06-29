using FlightScan.Api.Hubs;
using FlightScan.Core.Interfaces;
using FlightScan.Core.Responses.Reservations;
using Microsoft.AspNetCore.SignalR;

namespace FlightScan.Api.Services
{
    public class ReservationNotificationService : IReservationNotificationService
    {
        private readonly IHubContext<FlightHub> _hub;

        public ReservationNotificationService(IHubContext<FlightHub> hub)
        {
            _hub = hub;
        }

        public async Task NotifyNewReservationAsync(PendingReservationResponse reservation)
        {
            await _hub.Clients.Group("Agents").SendAsync("NewReservation", reservation);
        }

        public async Task NotifyReservationApprovedAsync(int reservationId, int visitorUserId)
        {
            await _hub.Clients.User(visitorUserId.ToString()).SendAsync("ReservationApproved", new { id = reservationId });
        }
    }
}
