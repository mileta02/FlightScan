using FlightScan.Core.Entities;

namespace FlightScan.Core.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation?> GetByIdAsync(int id);
    }
}
