using FlightScan.Core.Entities;
using FlightScan.Core.Interfaces;
using FlightScan.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FlightScan.Infrastructure.Data.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _dbContext;

        public ReservationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Reservation?> GetByIdAsync(int id) =>
            await _dbContext.Reservations
                .Include(r => r.Flight)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
    }
}
