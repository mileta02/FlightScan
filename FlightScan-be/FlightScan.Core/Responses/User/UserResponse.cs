using FlightScan.Core.Enums;

namespace FlightScan.Core.Responses.User
{
    public class UserResponse
    {
        public UserRole Role { get; set; }
        public string Username { get; set; } = null!;
    }
}
