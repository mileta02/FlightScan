using FlightScan.Core.Entities;
using FlightScan.Core.Enums;
using FlightScan.Core.Specifications;

namespace FlightScan.Core.Interfaces
{
    public interface IFlightRepository
    {
        Task<(List<Flight> Items, int TotalCount)> GetAllAsync(FlightSpecParams specParams);
        Task<Flight?> GetByIdAsync(int id);
        Task CreateAsync(Flight flight);
        Task<bool> ExistsAsync(City WhereFrom, City WhereTo, DateTime DepartureDate);
    }
}
