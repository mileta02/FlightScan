using FlightScan.Core.Entities;
using FlightScan.Core.Specifications;

namespace FlightScan.Core.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation?> GetByIdAsync(int id);
        Task<(List<Reservation> Items, int TotalCount)> GetByUserIdAsync(int userId, int pageIndex, int pageSize);
        public Task<(List<Reservation> Items, int TotalCount)> GetPendingAsync(ReservationSpecParams specParams);
        Task CreateAsync(Reservation reservation);
    }
}
