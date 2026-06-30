using FlightScan.Core.Entities;
using FlightScan.Core.Enums;
using FlightScan.Core.Interfaces;
using FlightScan.Core.Specifications;
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

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _dbContext.Reservations
                .Include(r => r.Flight)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<(List<Reservation> Items, int TotalCount)> GetByUserIdAsync(int userId, int pageIndex, int pageSize)
        {
            var query = _dbContext.Reservations
                .Include(r => r.Flight)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(List<Reservation> Items, int TotalCount)> GetPendingAsync(ReservationSpecParams specParams)
        {
            var query = _dbContext.Reservations
                .Include(r => r.Flight)
                .Include(r => r.User)
                .Where(r => r.Status == ReservationStatus.Pending && !r.Flight.IsCancelled)
                .OrderByDescending(r => r.CreatedAt);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((specParams.PageIndex - 1) * specParams.PageSize)
                .Take(specParams.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task CreateAsync(Reservation reservation)
        {
            await _dbContext.Reservations.AddAsync(reservation);
        }
    }
}
