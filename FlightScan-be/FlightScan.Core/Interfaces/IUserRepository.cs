using FlightScan.Core.Entities;

namespace FlightScan.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}
