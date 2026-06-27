using FlightScan.Core.Entities;

namespace FlightScan.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAndPasswordAsync(string username, string password);
    }
}
