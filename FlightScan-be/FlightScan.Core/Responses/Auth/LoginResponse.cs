using FlightScan.Core.Enums;

namespace FlightScan.Core.Responses.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public UserRole Role {  get; set; }

    }
}
