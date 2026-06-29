using Microsoft.AspNetCore.SignalR;

namespace FlightScan.Api.Hubs
{
    public class UserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirst("UserId")?.Value;
        }
    }
}
