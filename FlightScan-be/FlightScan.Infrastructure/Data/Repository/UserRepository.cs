using FlightScan.Core.Entities;
using FlightScan.Core.Interfaces;
using FlightScan.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FlightScan.Infrastructure.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByUsernameAndPasswordAsync(string username, string password)
        { 
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        } 
    }
}