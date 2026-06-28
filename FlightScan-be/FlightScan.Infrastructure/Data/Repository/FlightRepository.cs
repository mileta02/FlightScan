using FlightScan.Core.Entities;
using FlightScan.Core.Interfaces;
using FlightScan.Core.Specifications;
using FlightScan.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FlightScan.Infrastructure.Data.Repository
{
    public class FlightRepository : IFlightRepository
    {
        private readonly AppDbContext _dbContext;

        public FlightRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(List<Flight> Items, int TotalCount)> GetAllAsync(FlightSpecParams specParams)
        {
            var query = _dbContext.Flights.AsQueryable();

            if (specParams.IsCancelled.HasValue)
                query = query.Where(f => f.IsCancelled == specParams.IsCancelled.Value);

            if (specParams.WhereFrom.HasValue)
                query = query.Where(f => f.WhereFrom == specParams.WhereFrom.Value);

            if (specParams.WhereTo.HasValue)
                query = query.Where(f => f.WhereTo == specParams.WhereTo.Value);

            if (specParams.HasAvailableSeats == true)
                query = query.Where(f => f.AvailableSeats > 0);

            if (specParams.NonstopOnly == true)
                query = query.Where(f => f.Stops == 0);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(f => f.DepartureDate)
                .Skip((specParams.PageIndex - 1) * specParams.PageSize)
                .Take(specParams.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Flight?> GetByIdAsync(int id)
        {
            return await _dbContext.Flights.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task CreateAsync(Flight flight)
        {
            await _dbContext.Flights.AddAsync(flight);
        }
    }
}
