using FlightScan.Core.Interfaces;
using FlightScan.Infrastructure.Data.Context;

namespace FlightScan.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public UnitOfWork(AppDbContext dbContext) {
            _dbContext = dbContext;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
