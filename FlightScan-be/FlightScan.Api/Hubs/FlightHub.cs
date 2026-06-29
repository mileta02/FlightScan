using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace FlightScan.Api.Hubs
{
    [Authorize]
    public class FlightHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var role = Context.User?.FindFirst(ClaimTypes.Role)?.Value;
            if (role == "Agent")
                await Groups.AddToGroupAsync(Context.ConnectionId, "Agents");

            await base.OnConnectedAsync();
        }
    }
}
