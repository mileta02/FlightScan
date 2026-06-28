using FlightScan.Core.Entities;

namespace FlightScan.Core.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation?> GetByIdAsync(int id);
        Task<(List<Reservation> Items, int TotalCount)> GetByUserIdAsync(int userId, int pageIndex, int pageSize);
        Task CreateAsync(Reservation reservation);
    }
}
