using FlightScan.Core.Enums;

namespace FlightScan.Core.Entities
{
    public class Flight
    {
        public int Id { get; set; }
        public City WhereFrom { get; set; }
        public City WhereTo { get; set; }
        public int Stops { get; set; }
        public DateTime DepartureDate { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }  
        public bool IsCancelled { get; set; }
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
