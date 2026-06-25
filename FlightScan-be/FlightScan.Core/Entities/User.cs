using FlightScan.Core.Enums;

namespace FlightScan.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public UserRole Role { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
