using FlightScan.Core.Entities;

namespace FlightScan.Core.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
