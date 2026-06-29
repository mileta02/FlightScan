using FlightScan.Core.Entities;
using FlightScan.Core.Specifications;

namespace FlightScan.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<(List<User> Items, int TotalCount)> GetAllAsync(UserSpecParams specParams);
        Task CreateAsync(User user);
    }
}
